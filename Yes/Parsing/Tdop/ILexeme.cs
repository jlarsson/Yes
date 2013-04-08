namespace Yes.Parsing.Tdop
{
    public interface ILexeme
    {
        string Id { get; }
        object Value { get; }
        void Error(string message);
    }
}