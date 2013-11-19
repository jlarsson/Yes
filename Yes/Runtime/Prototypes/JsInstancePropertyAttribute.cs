using System;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Prototypes
{
    public abstract class AbstractJsPropertyAttribute : Attribute
    {
        protected AbstractJsPropertyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public bool Enumerable { get; set; }
        public bool Configurable { get; set; }
        public bool Writable { get; set; }
        public abstract bool IsPrototypeMember { get; }
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