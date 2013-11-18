namespace Yes.Parsing
{
    public interface ILexemeMapper
    {
        string CommentId(string text);
        string OperatorId(string text);
        string ErrorId(string text);
        string NumberId(string text);
        string NameId(string text);
        string StringId(string text);
    }
}