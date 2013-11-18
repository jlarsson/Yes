namespace Yes.Parsing.Tdop
{
    public interface ITdop<TToken, TAst, TAstFactory, TState>
    {
        TAstFactory Factory { get; }
        ISymbol<TToken, TAst, TAstFactory, TState> Token { get; }

        bool CanAdvance(string id);
        bool CanAdvance(params string[] ids);
        void Advance();
        void Advance(string id);
        bool TryAdvance(string id);
        TAst Expression(TState state, int bp);
        TAst Parse(TState state);
    }
}