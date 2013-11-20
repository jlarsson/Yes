using System.Collections.Generic;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Classes
{
    public class JsClass : IJsClass
    {
        public static readonly IJsClass Default = new JsClass(new Dictionary<string, IPropertyDescriptor>(), null);
        private readonly Dictionary<string, IPropertyDescriptor> _instanceProperties;

        public JsClass(Dictionary<string,IPropertyDescriptor> instanceProperties, IJsObject prototype)
        {
            _instanceProperties = instanceProperties;
            Prototype = prototype;
        }

        public IJsObject Prototype { get; set; }

        public IPropertyDescriptor GetInstanceProperty(string name)
        {
            IPropertyDescriptor pd;
            return _instanceProperties.TryGetValue(name, out pd) ? pd : null;
        }

        public IEnumerable<IPropertyDescriptor> GetInstanceProperties()
        {
            return _instanceProperties.Values;
        }
    }
}