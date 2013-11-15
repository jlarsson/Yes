using System;

namespace Yes.Runtime.Prototypes
{
    public class JsInstancePropertyAttribute: Attribute
    {
        public JsInstancePropertyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public bool Enumerable { get; set; }
        public bool Configurable { get; set; }
    }
}