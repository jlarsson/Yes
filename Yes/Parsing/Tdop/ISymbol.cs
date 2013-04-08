namespace Yes.Parsing.Tdop
{
    public interface ISymbol<TLexeme, TAst, TAstFactory>
    {
        TLexeme Lexeme { get; }
        IRule<TLexeme, TAst, TAstFactory> Rule { get; }
    }
}