using System;
using System.Collections.Generic;
using Yes.Runtime.Error;

namespace Yes.Parsing.Tdop
{
    public class Tdop<TLexeme, TAst, TAstFactory, TState> : ITdop<TLexeme, TAst, TAstFactory, TState> where TLexeme : ILexeme
    {
        public Tdop(IGrammar<TLexeme, TAst, TAstFactory, TState> grammar, TAstFactory factory, IEnumerable<TLexeme> lexemes)
        {
            Grammar = grammar;
            Factory = factory;
            Lexemes = lexemes.GetEnumerator();
        }

        public IEnumerator<TLexeme> Lexemes { get; protected set; }
        public IGrammar<TLexeme, TAst, TAstFactory, TState> Grammar { get; protected set; }

        #region ITdop<TLexeme,TAst,TAstFactory> Members

        public ISymbol<TLexeme, TAst, TAstFactory, TState> Token { get; protected set; }

        public TAstFactory Factory { get; protected set; }

        public bool CanAdvance(string id)
        {
            return (Token != null) && ((Token.Lexeme.Id == id) || (Token.Lexeme.Text == id));
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
                throw new JsSyntaxError();
            }
            Advance();
        }

        public bool TryAdvance(string id)
        {
            if (Token == null)
            {
                return false;
            }
            var canAdvance = Token.Lexeme.Id == id;
            canAdvance |= Token.Lexeme.Text == id;
            if (canAdvance)
            {
                Advance();
                return true;
            }
            return false;
        }

        public TAst Expression(TState state, int rbp)
        {
            var t = Token;

            Advance();

            var left = Nud(t, state);

            while ((Token != null) && (Token.Rule != null) && (rbp < Token.Rule.Lbp))
            {
                t = Token;
                Advance();
                left = Led(t, left, state);
            }
            return left;
        }

        public TAst Parse(TState state)
        {
            Advance();
            return Expression(state, 0);
        }

        #endregion

        private TAst Led(ISymbol<TLexeme, TAst, TAstFactory, TState> symbol, TAst left, TState state)
        {
            return symbol.Rule.Led(state, this, left);
        }

        private TAst Nud(ISymbol<TLexeme, TAst, TAstFactory, TState> symbol, TState state)
        {
            return symbol.Rule.Nud(state, this, symbol.Lexeme);
        }

        #region Nested type: Symbol

        public class Symbol : ISymbol<TLexeme, TAst, TAstFactory, TState>
        {
            #region ISymbol<TLexeme,TAst,TAstFactory> Members

            public TLexeme Lexeme { get; set; }
            public IRule<TLexeme, TAst, TAstFactory, TState> Rule { get; set; }

            #endregion
        }

        #endregion
    }
}