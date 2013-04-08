namespace Yes.Parsing.Tdop
{
    public interface IGrammar<TLexeme, TAst, TAstFactory>
    {
        IRule<TLexeme, TAst, TAstFactory> GetRule(TLexeme lexeme);
    }
}