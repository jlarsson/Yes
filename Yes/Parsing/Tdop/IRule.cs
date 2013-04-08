using System;

namespace Yes.Parsing.Tdop
{
    public interface IRule<TLexeme, TAst, TAstFactory>
    {
        string Id { get; }
        int Lbp { get; }
        Func<ITdop<TLexeme, TAst, TAstFactory>, TAst, TAst> Led { get; }
        Func<ITdop<TLexeme, TAst, TAstFactory>, TLexeme, TAst> Nud { get; }
        Func<ITdop<TLexeme, TAst, TAstFactory>, TAst> Std { get; }
    }
}