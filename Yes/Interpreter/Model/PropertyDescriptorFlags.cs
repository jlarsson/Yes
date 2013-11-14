using System;

namespace Yes.Interpreter.Model
{
    [Flags]
    public enum PropertyDescriptorFlags : byte { Writable = 0x01, Enumerable = 0x02, Configurable = 0x04 }
}