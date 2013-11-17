using System;
using System.Collections.Generic;
using Yes.Parsing.Tdop;
using Yes.Runtime;
using Yes.Runtime.Error;

namespace Yes.Parsing
{
    public interface IStaticScope
    {
    }

    public class JavascriptGrammar<TLexeme, TAst> : Grammar<TLexeme, TAst, IAstFactory<TAst>> where TLexeme : ILexeme
                                                                                              where TAst : class
    {
        public static readonly JavascriptGrammar<TLexeme, TAst> Default = new JavascriptGrammar<TLexeme, TAst>();

        private readonly HashSet<string> _keywords = new HashSet<string>();

        protected Stack<StaticScope> Scopes = new Stack<StaticScope>();

        protected JavascriptGrammar()
        {
            NewScope();
            Keywords("return", "var", "if", "else", "for", "while", "break", "continue", "function", "new", "in");

            Literal("(number)", (f, l) => f.Number(l.Value));
            Literal("(string)", (f, l) => f.String(l.Value));
            Literal("(name)", (f, l) => f.Name(l.Value.ToString()));

            Infix("===", 40, (f, lhs, rhs) => f.BinaryOperation("===", lhs, rhs));
            Infix("!==", 40, (f, lhs, rhs) => f.BinaryOperation("!==", lhs, rhs));
            Infix("==", 40, (f, lhs, rhs) => f.BinaryOperation("==", lhs, rhs));
            Infix("!=", 40, (f, lhs, rhs) => f.BinaryOperation("!=", lhs, rhs));
            Infix("<", 40, (f, lhs, rhs) => f.BinaryOperation("<", lhs, rhs));
            Infix("<=", 40, (f, lhs, rhs) => f.BinaryOperation("<=", lhs, rhs));
            Infix(">", 40, (f, lhs, rhs) => f.BinaryOperation(">", lhs, rhs));
            Infix(">=", 40, (f, lhs, rhs) => f.BinaryOperation(">=", lhs, rhs));

            Infix("+", 50, (f, lhs, rhs) => f.BinaryOperation("+", lhs, rhs));
            Infix("-", 50, (f, lhs, rhs) => f.BinaryOperation("-", lhs, rhs));
            Infix("*", 60, (f, lhs, rhs) => f.BinaryOperation("*", lhs, rhs));
            Infix("/", 60, (f, lhs, rhs) => f.BinaryOperation("/", lhs, rhs));

            Prefix("-", 70, (f, v) => f.UnaryOperation("-",v));
            Prefix("!", 70, (f, v) => f.Not(v));
            Prefix("typeof", 70, (f, v) => f.TypeOf(v));

            // TODO: Check prcedence of in operator
            Infix("in", 80, (f, lhs, rhs) => f.BinaryOperation("in", lhs, rhs));



            Assignment("=", (f, lhs, rhs) => f.Assign(lhs, rhs));
            Assignment("-=", (f, lhs, rhs) => f.DecAssign(lhs, rhs));
            Assignment("+=", (f, lhs, rhs) => f.IncAssign(lhs, rhs));

            // Infix (".",80) - member access
            Led(".", 80, (p, left) =>
                             {
                                 var name = ParseName(p);
                                 return p.Factory.Member(left, name);
                             });

            // Infix("(",80) - function application
            Led("(", 80, (p, left) =>
                             {
                                 var arguments = new List<TAst>();
                                 while (!p.CanAdvance(")"))
                                 {
                                     arguments.Add(p.Expression(0));
                                     if (!p.TryAdvance(","))
                                     {
                                         break;
                                     }
                                 }
                                 p.Advance(")");
                                 return p.Factory.Apply(left, arguments);
                             });

            Led("[", 80, (p, left) =>
                             {
                                 var argument = p.Expression(0);
                                 p.Advance("]");
                                 return p.Factory.Member(left, argument);
                             });

            // Handle E => (E)
            Nud("(", (p, l) =>
                         {
                             var e = p.Expression(0);
                             p.Advance(")");
                             return e;
                         });

            Prefix("new", (p, f) =>
                              {
                                  var constructor = p.Expression(100);
                                  var arguments = new List<TAst>();
                                  p.Advance("(");
                                  while (!p.CanAdvance(")"))
                                  {
                                      arguments.Add(p.Expression(0));
                                      if (!p.TryAdvance(","))
                                      {
                                          break;
                                      }
                                  }
                                  p.Advance(")");

                                  return p.Factory.Construct(constructor,arguments);
                              });
            Prefix("function", (p, f) =>
                                   {
                                       using (NewScope())
                                       {
                                           var name = TryParseName(p);
                                           var arguments = new List<TAst>();

                                           p.Advance("(");
                                           while (!p.CanAdvance(")"))
                                           {
                                               var argument = TryParseName(p);
                                               if (argument == null)
                                               {
                                                   break;
                                               }
                                               arguments.Add(argument);
                                               if (!p.TryAdvance(","))
                                               {
                                                   break;
                                               }
                                           }
                                           p.Advance(")");
                                           p.Advance("{");

                                           var statements = Statements(p);
                                           var result = p.Factory.Function(name, arguments, statements);
                                           p.Advance("}");
                                           return result;
                                       }
                                   });
            Std("{", (p, f) =>
                         {
                             var s = Statements(p);
                             p.Advance("}");
                             return s;
                         });

            Std("var", (p, f) =>
                           {
                               var declarations = new List<TAst>();
                               while (true)
                               {
                                   var name = ParseName(p);
                                   var value = default(TAst);
                                   if (p.TryAdvance("="))
                                   {
                                       value = p.Expression(0);
                                   }
                                   declarations.Add(f.Var(name, value));
                                   if (!p.TryAdvance(","))
                                   {
                                       break;
                                   }
                               }
                               AdvanceOptionalSemiColon(p);
                               return f.Seq(declarations);
                           });

            Std("return", (p, f) =>
                              {
                                  //if (!Scope.IsAllowed(Feature.Return))
                                  if (p.TryAdvance(";"))
                                  {
                                      return f.Return(null);
                                  }
                                  var v = p.Expression(0);
                                  AdvanceOptionalSemiColon(p);
                                  return f.Return(v);
                              });

            Std("if", (p, f) =>
                          {
                              p.Advance("(");
                              var @if = p.Expression(0);
                              p.Advance(")");
                              var @then = Block(p);
                              TAst @else = null;
                              if (p.TryAdvance("else"))
                              {
                                  @else = p.CanAdvance("if") ? Statement(p) : Block(p);
                              }
                              return f.IfThenElse(@if, @then, @else);
                          });
            Std("for", (p, f) =>
                           {
                               using (NewScope(Feature.Break|Feature.Continue))
                               {
                                   p.Advance("(");

                                   // Todo: handle 'for var m in x'

                                   var initial = default(TAst);
                                   if (p.CanAdvance("var"))
                                   {
                                       initial = Statement(p);
                                   }
                                   else
                                   {
                                       if (!p.CanAdvance(";"))
                                       {
                                           initial = p.Expression(0);
                                       }
                                       p.Advance(";");
                                   }


                                   var condition = default(TAst);
                                   if (!p.CanAdvance(";"))
                                   {
                                       condition = p.Expression(0);
                                   }
                                   p.Advance(";");

                                   var loop = default(TAst);
                                   if (!p.CanAdvance(")"))
                                   {
                                       loop = p.Expression(0);
                                   }
                                   p.Advance(")");

                                   var block = Block(p);

                                   return f.For(initial, condition, loop, block);
                               }
                           });
            Std("while", (p, f) =>
                             {
                                 using (NewScope(Feature.Break|Feature.Continue))
                                 {
                                     p.Advance("(");
                                     var cond = p.Expression(0);
                                     p.Advance(")");
                                     var block = Block(p);
                                     return f.While(cond, block);
                                 }
                             });
            Std("break", (p, f) =>
                             {
                                 if (!Scope.IsAllowed(Feature.Break))
                                 {
                                     p.Token.Lexeme.Error("Keyword break is not valid here.");
                                 }
                                 AdvanceOptionalSemiColon(p);
                                 return f.Break();
                             });
            Std("continue", (p, f) =>
                                {
                                    if (!Scope.IsAllowed(Feature.Continue))
                                    {
                                        p.Token.Lexeme.Error("Keyword continue is not valid here.");
                                    }
                                    AdvanceOptionalSemiColon(p);
                                    return f.Continue();
                                });
            

            Prefix("{", (p, l) =>
                            {
                                var members = new List<Tuple<TAst, TAst>>();
                                while (!p.CanAdvance("}"))
                                {
                                    var name = TryParseStringLiteral(p) ?? ParseName(p);
                                    p.Advance(":");
                                    var value = p.Expression(0);

                                    members.Add(Tuple.Create(name, value));
                                    if (!p.CanAdvance(","))
                                    {
                                        break;
                                    }
                                    p.Advance();
                                }
                                p.Advance("}");

                                return p.Factory.Object(members);
                            });

            Prefix("[", (p, l) =>
                            {
                                var members = new List<TAst>();
                                while(!p.CanAdvance("]"))
                                {
                                    var member = p.Expression(0);
                                    members.Add(member);

                                    if (!p.CanAdvance(","))
                                    {
                                        break;
                                    }
                                    p.Advance();
                                }
                                p.Advance("]");
                                return p.Factory.Array(members);
                            });


            // Error production
            // We end up here with scrips such as '{}.x;', since parser takes a block but user typed an object literal
            Nud(".", (p, l) => { throw new JsSyntaxError(); });
        }

        protected StaticScope Scope
        {
            get { return Scopes.Peek(); }
        }

        protected StaticScope NewScope()
        {
            return NewScope(Feature.None);
        }

        protected StaticScope NewScope(Feature allowedFeatures)
        {
            var scope = new StaticScope(this, allowedFeatures);
            Scopes.Push(scope);
            return scope;
        }

        private void PopScope()
        {
            Scopes.Pop();
        }


        private Rule Assignment(string id, Func<IAstFactory<TAst>, TAst, TAst, TAst> reduce)
        {
            var bp = 10;
            // TODO: Ensure left is -lvalue
            return Led(id, bp, (p, left) => reduce(p.Factory, left, p.Expression(bp - 1)));
        }

        protected void Keywords(params string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                _keywords.Add(keyword);
            }
        }

        protected override string GetRuleId(TLexeme lexeme)
        {
            var id = lexeme.Id;
            if ("(name)".Equals(id))
            {
                var kwd = lexeme.Value.ToString();
                if (_keywords.Contains(kwd))
                {
                    return kwd;
                }
            }
            return id;
        }

        private bool AdvanceOptionalSemiColon(ITdop<TLexeme, TAst, IAstFactory<TAst>> p)
        {
            if (p.CanAdvance(";"))
            {
                p.Advance(";");
                return true;
            }
            return false;
        }

        private TAst ParseName(ITdop<TLexeme, TAst, IAstFactory<TAst>> p)
        {
            var t = p.Token;
            p.Advance("(name)");
            return p.Factory.LiteralName(t.Lexeme.Value.ToString());
        }

        private TAst TryParseName(ITdop<TLexeme, TAst, IAstFactory<TAst>> p)
        {
            if (p.CanAdvance("(name)"))
            {
                var name = p.Factory.Name(p.Token.Lexeme.Value.ToString());
                p.Advance();
                return name;
            }
            return null;
        }

        private TAst TryParseStringLiteral(ITdop<TLexeme, TAst, IAstFactory<TAst>> p)
        {
            if (p.CanAdvance("(string)"))
            {
                var s = p.Factory.String(p.Token.Lexeme.Value.ToString());
                p.Advance();
                return s;
            }
            return null;
        }

        protected TAst Block(ITdop<TLexeme, TAst, IAstFactory<TAst>> p)
        {
            if (p.CanAdvance("{"))
            {
                p.Advance("{");
                var block = Statements(p);
                p.Advance("}");
                return block;
            }
            return Statement(p);
        }

        protected TAst Statement(ITdop<TLexeme, TAst, IAstFactory<TAst>> p)
        {
            while (AdvanceOptionalSemiColon(p))
            {
            }

            var t = p.Token;
            if (t == null)
            {
                return null;
            }

            if ((t.Rule != null) && (t.Rule.Std != null))
            {
                p.Advance();
                return t.Rule.Std(p);
            }
            var e = p.Expression(0);

            AdvanceOptionalSemiColon(p);
            return e;
        }

        public TAst Statements(ITdop<TLexeme, TAst, IAstFactory<TAst>> p)
        {
            var statements = new List<TAst>();
            while ((p.Token != null) && !p.CanAdvance("}"))
            {
                var s = Statement(p);
                if (s != null)
                {
                    statements.Add(s);
                }
            }
            return p.Factory.Block(statements);
        }

        #region Nested type: Feature

        [Flags]
        protected enum Feature
        {
            None = 0x00,
            Break = 0x01,
            Continue = 0x02,
        }

        #endregion

        #region Nested type: StaticScope

        protected class StaticScope : IStaticScope, IDisposable
        {
            public StaticScope(JavascriptGrammar<TLexeme, TAst> grammar, Feature allowedFeatures)
            {
                Grammar = grammar;
                AllowedFeatures = allowedFeatures;
            }

            public JavascriptGrammar<TLexeme, TAst> Grammar { get; protected set; }
            public Feature AllowedFeatures { get; protected set; }

            #region IDisposable Members

            public void Dispose()
            {
                Grammar.PopScope();
            }

            #endregion

            public bool IsAllowed(Feature feature)
            {
                return (AllowedFeatures & feature) == feature;
            }
        }

        #endregion
    }
}