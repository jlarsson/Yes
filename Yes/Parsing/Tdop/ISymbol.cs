namespace Yes.Parsing.Tdop
{
    public interface ISymbol<TLexeme, TAst, TAstFactory, TState>
    {
        TLexeme Lexeme { get; }
        IRule<TLexeme, TAst, TAstFactory, TState> Rule { get; }
    }
}