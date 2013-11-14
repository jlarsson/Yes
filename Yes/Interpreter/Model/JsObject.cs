using System;
using System.Collections.Generic;
using Yes.Runtime;
using Yes.Runtime.Environment;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsObject: IJsObject{
        private Dictionary<string, IPropertyDescriptor> _properties;
        public IEnvironment Environment { get; private set; }
        public IJsObject Prototype { get; protected set; }
        public JsObject(IEnvironment environment, IJsObject prototype)
        {
            Environment = environment;
            Prototype = prototype;
            Extensible = true;
            DefineOwnProperty(new AccessorPropertyDescriptor("prototype", () => Prototype,
                                                             v => Prototype = Conversion.Cast<IJsObject>(v, true)));
        }

        public virtual JsTypeCode TypeCode
        {
            get { return JsTypeCode.Object; }
        }

        public virtual IReference GetReference(IJsValue name)
        {
            return GetReference(name.ToString());
        }

        public virtual IReference GetReference(string name)
        {
            // Find property definition in inheritance chain
            // Equivalent of CanPut
            IJsObject proto = this;
            IPropertyDescriptor pd = null;
            while (proto != null)
            {
                pd = proto.GetOwnProperty(name);
                if (pd != null)
                {
                    if (!pd.Writable)
                    {
                        return new ReadonlyReference();
                    }
                    break;
                }
                proto = proto.GetPrototype();
            }
            if (pd == null)
            {
                if (!Extensible)
                {
                    return new ReadonlyReference();
                }
                // Property not found, but we are extensible to create a new data property
                return _properties[name] = new DataPropertyDescriptor(name);
            }

            // If the property is not own, we make a copy that is own
            // This is equivalent of calling DefineOwnProperty
            if (!_properties.ContainsKey(name))
            {
                return new LambdaReference(name, LazyGetOwnValue, LazySetOwnValue);
            }
            return pd;
        }

        private IJsValue LazyGetOwnValue(string name)
        {
            var pd = GetOwnProperty(name);
            return pd == null ? JsUndefined.Instance : pd.GetValue();
        }

        private IJsValue LazySetOwnValue(string name, IJsValue value)
        {
            var pd = GetOwnProperty(name);
            if (pd != null)
            {
                return pd.SetValue(value);
            }

            if (!Extensible)
            {
                throw new ApplicationException();
            }

            var proto = GetPrototype();
            while (proto != null)
            {
                pd = GetOwnProperty(name);
                if (pd != null)
                {
                    DefineOwnProperty(pd.MakeOwnCopy(Environment, value));
                    return value;
                }
                proto = proto.GetPrototype();
            }
            throw new ApplicationException();
        }

        public class DataPropertyDescriptor : IPropertyDescriptor
        {
            public DataPropertyDescriptor(string name): this(name, JsUndefined.Instance, PropertyDescriptorFlags.Enumerable|PropertyDescriptorFlags.Writable|PropertyDescriptorFlags.Configurable)
            {
            }

            public DataPropertyDescriptor(string name, IJsValue value, PropertyDescriptorFlags flags)
            {
                Name = name;
                Value = value;
                Flags = flags;
            }

            public IJsValue Value { get; protected set; }

            public IJsValue GetValue()
            {
                return Value;
            }

            public IJsValue SetValue(IJsValue value)
            {
                return Value = value;
            }

            public string Name { get; private set; }

            public PropertyDescriptorFlags Flags { get; private set; }
            public bool Writable { get { return (Flags & PropertyDescriptorFlags.Writable) != 0; } }
            public bool Enumerable { get { return (Flags & PropertyDescriptorFlags.Enumerable) != 0; } }
            public bool Configurable { get { return (Flags & PropertyDescriptorFlags.Configurable) != 0; } }

            public IPropertyDescriptor MakeOwnCopy(IEnvironment environment, IJsValue value)
            {
                return new DataPropertyDescriptor(Name, value, Flags);
            }
        }

        public class ReadonlyReference : IReference
        {
            public IJsValue GetValue()
            {
                return JsUndefined.Instance;
            }

            public IJsValue SetValue(IJsValue value)
            {
                throw new System.NotImplementedException();
            }
        }

        public int? ToArrayIndex()
        {
            return null;
        }

        public bool ToBoolean()
        {
            return true;
        }

        public double ToNumber()
        {
            // TODO: Call ToNumber of primitive value
            return double.NaN;
        }

        public bool Extensible { get; set; }

        public IJsObject GetPrototype()
        {
            return Prototype;
        }

        public IPropertyDescriptor GetOwnProperty(string name)
        {
            IPropertyDescriptor pd;
            return ((_properties != null) && _properties.TryGetValue(name, out pd)) ? pd : null;
        }

        public IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<string, IPropertyDescriptor>();
            }
            return _properties[descriptor.Name] = descriptor;
        }
    }
}