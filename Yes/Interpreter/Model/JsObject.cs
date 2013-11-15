using System.Collections.Generic;
using Yes.Runtime;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsObject : IJsObject
    {
        private Dictionary<string, IPropertyDescriptor> _properties;
        public IEnvironment Environment { get; private set; }
        public IJsObject Prototype { get; protected set; }

        public JsObject(IEnvironment environment, IJsObject prototype)
        {
            Environment = environment;
            Prototype = prototype;
            Extensible = true;
        }


        public IPropertyDescriptor CreateDataProperty(string name, IJsValue value, PropertyDescriptorFlags flags)
        {
            return new ObjectPropertyDescriptor(this, name, value, flags);
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
            // TODO: By specifying wether reference is readonly or not, we can optimze
            // especially when property is inherited

            // Find property definition in inheritance chain
            // Equivalent of CanPut
            IJsObject proto = this;
            IPropertyDescriptor pd = null;
            while (proto != null)
            {
                pd = proto.GetOwnProperty(name);
                if (pd != null)
                {
                    return pd;
                }
                proto = proto.GetPrototype();
            }
            if (!Extensible)
            {
                return new ReadonlyReference();
            }
            // Property not found, but we are extensible to create a new data property
            return new ObjectPropertyDescriptor(null/* no owner, yet... */, name, JsUndefined.Value, PropertyDescriptorFlags.Configurable | PropertyDescriptorFlags.Enumerable | PropertyDescriptorFlags.Writable);
        }

        public virtual int? ToArrayIndex()
        {
            return null;
        }

        public virtual object ToPrimitive()
        {
            return this;
        }

        public virtual bool ToBoolean()
        {
            return true;
        }

        public virtual double ToNumber()
        {
            // TODO: Call ToNumber of primitive value
            return double.NaN;
        }

        public virtual int ToInteger()
        {
            return 0;
        }

        public override string ToString()
        {
            return "[object Object]";
        }

        public bool Extensible { get; set; }

        public virtual IJsObject GetPrototype()
        {
            return Prototype;
        }

        public virtual IPropertyDescriptor GetOwnProperty(string name)
        {
            IPropertyDescriptor pd;
            return ((_properties != null) && _properties.TryGetValue(name, out pd)) ? pd : null;
        }

        public virtual IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<string, IPropertyDescriptor>();
            }
            return _properties[descriptor.Name] = descriptor;
        }

        [JsInstanceProperty("prototype",Configurable = false, Enumerable = false)]
        public IJsValue JsPrototype
        {
            get { return Prototype; }
            set { Prototype = Conversion.Cast<IJsObject>(value, true); }
        }
    }
}