using System;

namespace Yes.Parsing
{
    public interface ILexicalScope: IDisposable
    {
        bool IsAllowed(LexicalFeature f);
        bool UseStrict { get; set; }
    }
}