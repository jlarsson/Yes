namespace Yes.Parsing.Tdop
{
    public interface ITdop<TToken, TAst, TAstFactory>
    {
        TAstFactory Factory { get; }
        ISymbol<TToken, TAst, TAstFactory> Token { get; }

        TAst Expression(int bp);
        bool CanAdvance(string id);
        void Advance();
        void Advance(string id);
        bool TryAdvance(string id);
        TAst Parse();
    }
}