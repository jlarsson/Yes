namespace Yes.Parsing.Tdop
{
    public interface ILexeme
    {
        string Id { get; }
        string Text { get; }
        object Value { get; }

        string Source { get; }
        int Position { get; }
        int Length { get; }
        int Line { get; }
        int Column { get; }
    }
}