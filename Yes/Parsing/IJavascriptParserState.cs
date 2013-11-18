namespace Yes.Parsing
{
    public interface IJavascriptParserState
    {
        ILexicalScope Scope { get; }
        ILexicalScope NewScope(LexicalFeature allowedFeatures = LexicalFeature.None);
    }
}