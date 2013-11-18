using System;

namespace Yes.Parsing
{
    [Flags]
    public enum LexicalFeature
    {
        None = 0x00,
        Break = 0x01,
        Continue = 0x02,
    }
}