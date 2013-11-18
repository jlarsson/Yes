using System;

namespace Yes.Parsing.Tdop
{
    public interface IRule<TLexeme, TAst, TAstFactory, TState>
    {
        string Id { get; }
        int Lbp { get; }
        Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TAst, TAst> Led { get; }
        Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TLexeme, TAst> Nud { get; }
        Func<TState, ITdop<TLexeme, TAst, TAstFactory, TState>, TAst> Std { get; }
    }
}