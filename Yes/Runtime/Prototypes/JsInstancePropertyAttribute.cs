using System;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Prototypes
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Property)]
    public class JsMemberAttribute : Attribute
    {
        public JsMemberAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public bool Enumerable { get; set; }
        public bool Configurable { get; set; }
        public bool Writable { get; set; }
        public PropertyDescriptorFlags GetFlags()
        {
            var f = PropertyDescriptorFlags.None;
            if (Writable)
            {
                f |= PropertyDescriptorFlags.Writable;
            }
            if (Enumerable)
            {
                f |= PropertyDescriptorFlags.Enumerable;
            }
            if (Configurable)
            {
                f |= PropertyDescriptorFlags.Configurable;
            }
            return f;
        }
    }
}