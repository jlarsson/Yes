namespace Yes.Parsing.Tdop
{
    public interface IGrammar<TLexeme, TAst, TAstFactory, TState>
    {
        IRule<TLexeme, TAst, TAstFactory, TState> GetRule(TLexeme lexeme);
    }
}