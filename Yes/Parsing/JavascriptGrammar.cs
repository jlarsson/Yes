using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Parsing.Tdop;
using Yes.Runtime.Error;
using Yes.Utility;

namespace Yes.Parsing
{
    public class JavascriptGrammar<TLexeme, TAst> : Grammar<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState>
        where TLexeme : ILexeme
                                                                                              where TAst : class
    {
        public static readonly JavascriptGrammar<TLexeme, TAst> Default = new JavascriptGrammar<TLexeme, TAst>();

        protected JavascriptGrammar()
        {
            Literal("(number)", (state, f, l) => f.Number(Conversion.ParseDouble(l.Text)));
            Literal("(string)", (state, f, l) => f.String(l.Text));
            Literal("(name)", (state, f, l) => f.Name(l.Text));

            // For operator precedence: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Operator_Precedence?redirectlocale=en-US&redirectslug=JavaScript%2FReference%2FOperators%2FOperator_Precedence

            Assignment(10, "=", (state, f, lhs, rhs) => f.Assign(lhs, rhs));
            Assignment(10, "-=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("-", lhs, rhs)));
            Assignment(10, "+=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("+", lhs, rhs)));
            Assignment(10, "*=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("*", lhs, rhs)));
            Assignment(10, "/=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("/", lhs, rhs)));
            Assignment(10, "%=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("%", lhs, rhs)));
            Assignment(10, "<<=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("<<", lhs, rhs)));
            Assignment(10, ">>=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation(">>", lhs, rhs)));
            Assignment(10, ">>>=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation(">>>", lhs, rhs)));
            Assignment(10, "&=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("&", lhs, rhs)));
            Assignment(10, "^=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("^", lhs, rhs)));
            Assignment(10, "!=", (state, f, lhs, rhs) => f.Assign(lhs, f.BinaryOperation("!", lhs, rhs)));

            Led("?", 15, (state, p, l) =>
                             {
                                 var t = p.Expression(state, 0);
                                 p.Advance(":");
                                 var f = p.Expression(state, 0);

                                 return p.Factory.Conditional(l, t, f);
                             });
            Infix(20, "||", (state, f, lhs, rhs) => f.BinaryOperation("||", lhs, rhs));
            Infix(21, "&&", (state, f, lhs, rhs) => f.BinaryOperation("&&", lhs, rhs));
            Infix(22, "|", (state, f, lhs, rhs) => f.BinaryOperation("|", lhs, rhs));
            Infix(23, "^", (state, f, lhs, rhs) => f.BinaryOperation("^", lhs, rhs));
            Infix(24, "&", (state, f, lhs, rhs) => f.BinaryOperation("&", lhs, rhs));

            Prefix(40, "instanceof", (state, f, v) => f.UnaryOperation("instanceof", v));
            Infix(40, "in", (state, f, lhs, rhs) => f.BinaryOperation("in", lhs, rhs));

            Infix(40, "===", (state, f, lhs, rhs) => f.BinaryOperation("===", lhs, rhs));
            Infix(40, "!==", (state, f, lhs, rhs) => f.BinaryOperation("!==", lhs, rhs));
            Infix(40, "==", (state, f, lhs, rhs) => f.BinaryOperation("==", lhs, rhs));
            Infix(40, "!=", (state, f, lhs, rhs) => f.BinaryOperation("!=", lhs, rhs));
            Infix(40, "<", (state, f, lhs, rhs) => f.BinaryOperation("<", lhs, rhs));
            Infix(40, "<=", (state, f, lhs, rhs) => f.BinaryOperation("<=", lhs, rhs));
            Infix(40, ">", (state, f, lhs, rhs) => f.BinaryOperation(">", lhs, rhs));
            Infix(40, ">=", (state, f, lhs, rhs) => f.BinaryOperation(">=", lhs, rhs));

            Infix(50, "+", (state, f, lhs, rhs) => f.BinaryOperation("+", lhs, rhs));
            Infix(50, "-", (state, f, lhs, rhs) => f.BinaryOperation("-", lhs, rhs));
            Infix(60, "*", (state, f, lhs, rhs) => f.BinaryOperation("*", lhs, rhs));
            Infix(60, "/", (state, f, lhs, rhs) => f.BinaryOperation("/", lhs, rhs));
            Infix(60, "%", (state, f, lhs, rhs) => f.BinaryOperation("%", lhs, rhs));

            Prefix(70, "-", (state, f, v) => f.UnaryOperation("-", v));
            Prefix(70, "+", (state, f, v) => f.UnaryOperation("-", v));
            Prefix(70, "!", (state, f, v) => f.UnaryOperation("!", v));
            Prefix(70, "~", (state, f, v) => f.UnaryOperation("!", v));
            Prefix(70, "typeof", (state, f, v) => f.UnaryOperation("typeof", v));
            Prefix(70, "delete", (state, f, v) => f.Delete(v));
            Prefix(70, "void", (state, f, v) => f.UnaryOperation("void", v));

            Prefix(75, "++", (state, f, v) => f.PreAssign(v, f.BinaryOperation("+", v, f.Number(1))));
            Prefix(75, "--", (state, f, v) => f.PreAssign(v, f.BinaryOperation("-", v, f.Number(1))));
            Postfix(75, "++", (state, f, v) => f.PostAssign(v, f.BinaryOperation("+", v, f.Number(1))));
            Postfix(75, "--", (state, f, v) => f.PostAssign(v, f.BinaryOperation("-", v, f.Number(1))));


            // Infix (".",80) - member access
            Led(".", 80, (state, p, left) =>
                             {
                                 var name = ParseName(p);
                                 return p.Factory.Member(left, name);
                             });

            // Infix("(",80) - function application
            Led("(", 80, (state, p, left) =>
                             {
                                 var arguments = new List<TAst>();
                                 while (!p.CanAdvance(")"))
                                 {
                                     arguments.Add(p.Expression(state, 0));
                                     if (!p.TryAdvance(","))
                                     {
                                         break;
                                     }
                                 }
                                 p.Advance(")");
                                 return p.Factory.Apply(left, arguments);
                             });

            Led("[", 80, (state, p, left) =>
                             {
                                 var argument = p.Expression(state, 0);
                                 p.Advance("]");
                                 return p.Factory.IndexedMember(left, argument);
                             });

            // Handle E => (E)
            Nud("(", (state, p, l) =>
                         {
                             var e = p.Expression(state, 0);
                             p.Advance(")");
                             return e;
                         });

            Prefix("new", (state, p, f) =>
                              {
                                  var constructor = p.Expression(state, 100);
                                  var arguments = new List<TAst>();
                                  p.Advance("(");
                                  while (!p.CanAdvance(")"))
                                  {
                                      arguments.Add(p.Expression(state, 0));
                                      if (!p.TryAdvance(","))
                                      {
                                          break;
                                      }
                                  }
                                  p.Advance(")");

                                  return p.Factory.Construct(constructor,arguments);
                              });
            Prefix("function", (state, p, f) =>
                                   {
                                       using (state.NewScope())
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

                                           var statements = Statements(state, p);
                                           var result = p.Factory.Function(name, arguments, statements);
                                           p.Advance("}");
                                           return result;
                                       }
                                   });
            Std("{", (state, p, f) =>
                         {
                             var s = Statements(state, p);
                             p.Advance("}");
                             return s;
                         });

            Std("(comment)", (state, p, f) => null);
            Std("(error)", (state, p, f) => { throw new JsSyntaxException(); });

            Std("var", (state, p, f) =>
                           {
                               var declarations = new List<TAst>();
                               while (true)
                               {
                                   var name = ParseName(p);
                                   var value = default(TAst);
                                   if (p.TryAdvance("="))
                                   {
                                       value = p.Expression(state, 0);
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

            Std("return", (state, p, f) =>
                              {
                                  //if (!Scope.IsAllowed(Feature.Return))
                                  if (p.TryAdvance(";"))
                                  {
                                      return f.Return(null);
                                  }
                                  var v = p.Expression(state, 0);
                                  AdvanceOptionalSemiColon(p);
                                  return f.Return(v);
                              });

            Std("if", (state, p, f) =>
                          {
                              p.Advance("(");
                              var @if = p.Expression(state, 0);
                              p.Advance(")");
                              var @then = Block(state, p);
                              TAst @else = null;
                              if (p.TryAdvance("else"))
                              {
                                  @else = p.CanAdvance("if") ? Statement(state, p) : Block(state, p);
                              }
                              return f.IfThenElse(@if, @then, @else);
                          });
            Std("for", (state, p, f) =>
                           {
                               using (state.NewScope(LexicalFeature.Break | LexicalFeature.Continue))
                               {
                                   p.Advance("(");

                                   if (p.CanAdvance("var","(name)","in"))
                                   {
                                       p.Advance("var");
                                       var binding = ParseName(p);
                                       p.Advance("in");
                                       var inspected = p.Expression(state, 0);
                                       p.Advance(")");
                                       var b = Block(state, p);

                                       return f.ForIn(binding, inspected, b, true);
                                   }
                                   if (p.CanAdvance("(name)","in"))
                                   {
                                       var binding = ParseName(p);
                                       p.Advance("in");
                                       var inspected = p.Expression(state, 0);
                                       p.Advance(")");
                                       var b = Block(state, p);

                                       return f.ForIn(binding, inspected, b, false);

                                   }

                                   var initial = default(TAst);
                                   if (p.CanAdvance("var"))
                                   {
                                       initial = Statement(state, p);
                                   }
                                   else
                                   {
                                       if (!p.CanAdvance(";"))
                                       {
                                           initial = p.Expression(state, 0);
                                       }
                                       p.Advance(";");
                                   }


                                   var condition = default(TAst);
                                   if (!p.CanAdvance(";"))
                                   {
                                       condition = p.Expression(state, 0);
                                   }
                                   p.Advance(";");

                                   var loop = default(TAst);
                                   if (!p.CanAdvance(")"))
                                   {
                                       loop = p.Expression(state, 0);
                                   }
                                   p.Advance(")");

                                   var block = Block(state, p);

                                   return f.For(initial, condition, loop, block);
                               }
                           });
            Std("while", (state, p, f) =>
                             {
                                 using (state.NewScope(LexicalFeature.Break|LexicalFeature.Continue))
                                 {
                                     p.Advance("(");
                                     var cond = p.Expression(state, 0);
                                     p.Advance(")");
                                     var block = Block(state, p);
                                     return f.While(cond, block);
                                 }
                             });
            Std("break", (state, p, f) =>
                             {
                                 if (!state.Scope.IsAllowed(LexicalFeature.Break))
                                 {
                                     throw new JsSyntaxException();
                                 }
                                 AdvanceOptionalSemiColon(p);
                                 return f.Break();
                             });
            Std("continue", (state, p, f) =>
                                {
                                    if (!state.Scope.IsAllowed(LexicalFeature.Continue))
                                    {
                                        throw new JsSyntaxException();
                                    }
                                    AdvanceOptionalSemiColon(p);
                                    return f.Continue();
                                });

            Std("throw", (state, p, f) =>
            {
                var expression = p.Expression(state, 0);
                AdvanceOptionalSemiColon(p);

                return f.Throw(expression);

            });

            Std("try", (state, p, f) =>
                           {
                               var tcf = new TryCatchFinallyParameters<TAst>();
                               tcf.TryStatement = Block(state, p);

                               if (p.TryAdvance("catch"))
                               {
                                   var c = new CatchParameters<TAst>();
                                   if (p.TryAdvance("("))
                                   {
                                       if (p.CanAdvance("(name)"))
                                       {
                                           c.BindingName = ParseName(p);
                                       }
                                       p.Advance(")");
                                   }
                                   c.CatchStatement = Block(state, p);
                                   tcf.CatchParameters = c;
                               }
                               if (p.TryAdvance("finally"))
                               {
                                   tcf.FinallyStatement = Block(state, p);
                               }
                               return f.TryCatchFinally(tcf);
                           });

            Prefix("{", (state, p, l) =>
                            {
                                var members = new List<Tuple<TAst, TAst>>();
                                while (!p.CanAdvance("}"))
                                {
                                    var name = TryParseStringLiteral(p) ?? ParseName(p);
                                    p.Advance(":");
                                    var value = p.Expression(state, 0);

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

            Prefix("[", (state, p, l) =>
                            {
                                var members = new List<TAst>();
                                while(!p.CanAdvance("]"))
                                {
                                    var member = p.Expression(state, 0);
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
            Nud(".", (state, p, l) => { throw new JsSyntaxException(); });
        }

        private Rule Assignment(int bp, string id, Func<IJavascriptParserState, IAstFactory<TAst>, TAst, TAst, TAst> reduce)
        {
            // TODO: Ensure left is -lvalue
            return Led(id, bp, (state, p, left) => reduce(state, p.Factory, left, p.Expression(state, bp - 1)));
        }

        private bool AdvanceOptionalSemiColon(ITdop<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState> p)
        {
            if (p.CanAdvance(";"))
            {
                p.Advance(";");
                return true;
            }
            return false;
        }

        private TAst ParseName(ITdop<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState> p)
        {
            var t = p.Token;
            p.Advance("(name)");
            return p.Factory.LiteralName(t.Lexeme.Text);
        }

        private TAst TryParseName(ITdop<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState> p)
        {
            if (p.CanAdvance("(name)"))
            {
                var name = p.Factory.Name(p.Token.Lexeme.Text);
                p.Advance();
                return name;
            }
            return null;
        }

        private TAst TryParseStringLiteral(ITdop<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState> p)
        {
            if (p.CanAdvance("(string)"))
            {
                var s = p.Factory.String(p.Token.Lexeme.Text);
                p.Advance();
                return s;
            }
            return null;
        }

        protected TAst Block(IJavascriptParserState state, ITdop<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState> p)
        {
            if (p.CanAdvance("{"))
            {
                p.Advance("{");
                var block = Statements(state, p);
                p.Advance("}");
                return block;
            }
            return Statement(state, p);
        }

        protected TAst Statement(IJavascriptParserState state, ITdop<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState> p)
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
                return t.Rule.Std(state, p);
            }
            var e = p.Expression(state, 0);
            if ((e is IAstDirective) && (e as IAstDirective).IsUseStrict)
            {
                state.Scope.UseStrict = true;
            }

            AdvanceOptionalSemiColon(p);
            return e;
        }

        public TAst Statements(IJavascriptParserState state, ITdop<TLexeme, TAst, IAstFactory<TAst>, IJavascriptParserState> p)
        {
            var statements = new List<TAst>();
            while ((p.Token != null) && !p.CanAdvance("}"))
            {
                var s = Statement(state, p);
                if (s != null)
                {
                    statements.Add(s);
                }
            }
            return p.Factory.Block(statements);
        }
    }
}