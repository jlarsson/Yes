using System;
using System.Collections.Generic;

namespace Yes.Parsing.Tdop
{
    public class Tdop<TLexeme, TAst, TAstFactory> : ITdop<TLexeme, TAst, TAstFactory> where TLexeme : ILexeme
    {
        public Tdop(IGrammar<TLexeme, TAst, TAstFactory> grammar, TAstFactory factory, IEnumerable<TLexeme> lexemes)
        {
            Grammar = grammar;
            Factory = factory;
            Lexemes = lexemes.GetEnumerator();
        }

        public IEnumerator<TLexeme> Lexemes { get; protected set; }
        public IGrammar<TLexeme, TAst, TAstFactory> Grammar { get; protected set; }

        #region ITdop<TLexeme,TAst,TAstFactory> Members

        public ISymbol<TLexeme, TAst, TAstFactory> Token { get; protected set; }

        public TAstFactory Factory { get; protected set; }

        public TAst Expression(int rbp)
        {
            var t = Token;

            Advance();

            //if (t.Rule.Std != null)
            //{
            //    return t.Rule.Std(this);
            //}

            var left = Nud(t);

            //while (rbp < Token.Rule.Lbp)
            //while ((Token != null) && (rbp < Token.Rule.Lbp))
            while ((Token != null) && (Token.Rule != null) && (rbp < Token.Rule.Lbp))
            {
                t = Token;
                Advance();
                left = Led(t, left);
            }
            return left;
        }

        public bool CanAdvance(string id)
        {
            return (Token != null) && ((Token.Lexeme.Id == id) || (Token.Lexeme.Value.ToString() == id));
        }

        public void Advance()
        {
            if (Lexemes == null)
            {
                return;
            }
            if (!Lexemes.MoveNext())
            {
                Token = null;
                Lexemes = null;
                return;
            }
            var lexeme = Lexemes.Current;
            Token = new Symbol
                        {
                            Lexeme = lexeme,
                            Rule = Grammar.GetRule(lexeme)
                        };
        }

        public void Advance(string id)
        {
            if (Token == null)
            {
                throw new ApplicationException("Expected " + id + " at end of input");
            }
            if (Token.Lexeme.Id != id)
            {
                Token.Lexeme.Error("Expected " + id);
                throw new ApplicationException("Expected " + id);
            }
            Advance();
        }

        public bool TryAdvance(string id)
        {
            if (Token == null)
            {
                return false;
            }
            if (Token.Lexeme.Id != id)
            {
                return false;
            }
            Advance();
            return true;
        }

        public TAst Parse()
        {
            Advance();
            return Expression(0);
        }

        #endregion

        private TAst Led(ISymbol<TLexeme, TAst, TAstFactory> symbol, TAst left)
        {
            return symbol.Rule.Led(this, left);
        }

        private TAst Nud(ISymbol<TLexeme, TAst, TAstFactory> symbol)
        {
            return symbol.Rule.Nud(this, symbol.Lexeme);
        }

        #region Nested type: Symbol

        public class Symbol : ISymbol<TLexeme, TAst, TAstFactory>
        {
            #region ISymbol<TLexeme,TAst,TAstFactory> Members

            public TLexeme Lexeme { get; set; }
            public IRule<TLexeme, TAst, TAstFactory> Rule { get; set; }

            #endregion
        }

        #endregion
    }
}