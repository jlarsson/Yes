using System;
using System.Collections.Generic;
using Yes.Runtime.Error;

namespace Yes.Parsing.Tdop
{
    public class Tdop<TLexeme, TAst, TAstFactory, TState> : ITdop<TLexeme, TAst, TAstFactory, TState> where TLexeme : ILexeme
    {
        public Tdop(IGrammar<TLexeme, TAst, TAstFactory, TState> grammar, TAstFactory factory, IList<TLexeme> lexemes)
        {
            Grammar = grammar;
            Factory = factory;
            Lexemes = lexemes;
        }

        public int LexemeIndex { get; protected set; }
        public IList<TLexeme> Lexemes { get; protected set; }
        public TLexeme Lexeme { get { return LexemeIndex < Lexemes.Count ? Lexemes[LexemeIndex] : default(TLexeme); } }
        public IGrammar<TLexeme, TAst, TAstFactory, TState> Grammar { get; protected set; }

        #region ITdop<TLexeme,TAst,TAstFactory> Members

        private ISymbol<TLexeme, TAst, TAstFactory, TState> _token;
        public ISymbol<TLexeme, TAst, TAstFactory, TState> Token { get
        {
            if (_token == null)
            {
                if (LexemeIndex < Lexemes.Count)
                {
                    var lexeme = Lexemes[LexemeIndex];
                    _token = new Symbol
                    {
                        Lexeme = lexeme,
                        Rule = Grammar.GetRule(lexeme)
                    };
                }
            }
            return _token;
        } }

        public TAstFactory Factory { get; protected set; }

        public bool CanAdvance(string id)
        {
            return (Token != null) && ((Token.Lexeme.Id == id) || (Token.Lexeme.Text == id));
        }

        public bool CanAdvance(params string[] ids)
        {
            for(var i = 0; i < ids.Length; ++i)
            {
                var li = LexemeIndex + i;
                if (li >= Lexemes.Count)
                {
                    return false;
                }
                var l = Lexemes[li];
                if (!((l.Id == ids[i]) || (l.Text == ids[i])))
                {
                    return false;
                }
            }
            return true;
        }

        public void Advance()
        {
            _token = null;
            ++LexemeIndex;
/*
            if (Lexemes == null)
            {
                return;
            }
            if (LexemeIndex >= Lexemes.Count)
            {
                Token = null;
                Lexemes = null;
                return;
            }
            var lexeme = Lexeme;
            Token = new Symbol
                        {
                            Lexeme = lexeme,
                            Rule = Grammar.GetRule(lexeme)
                        };
            ++LexemeIndex;
 */ 
        }

        public void Advance(string id)
        {
            if (Token == null)
            {
                throw new ApplicationException("Expected " + id + " at end of input");
            }
            if (Token.Lexeme.Id != id)
            {
                throw new JsSyntaxException();
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