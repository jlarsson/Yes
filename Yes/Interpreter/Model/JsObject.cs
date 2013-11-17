using System.Collections.Generic;
using System.Linq;
using Yes.Runtime;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsObject : IJsObject
    {
        private Dictionary<string, IPropertyDescriptor> _properties;

        public JsObject(IEnvironment environment, IJsObject prototype)
        {
            Environment = environment;
            Prototype = prototype;
        }

        public IEnvironment Environment { get; private set; }
        public IJsObject Prototype { get; protected set; }

        //[JsInstanceProperty("constructor", Configurable = false, Enumerable = false)]
        //public IJsValue JsConstructor
        //{
        //    get { return null;}
        //    set {  }
        //}

        [JsInstanceProperty("prototype", Configurable = false, Enumerable = false)]
        public IJsValue JsPrototype
        {
            get { return Prototype; }
            set { Prototype = Conversion.Cast<IJsObject>(value, true); }
        }

        [JsInstanceMethod("hasOwnProperty", Configurable = false, Enumerable = false)]
        public IJsValue JsHasOwnProperty(IJsValue[] args)
        {
            return Environment.CreateBool(GetOwnProperty(args.Select(a => a.ToString()).FirstOrDefault()) != null);
        }
        [JsInstanceMethod("isPrototypeOf", Configurable = false, Enumerable = false)]
        public IJsValue JsIsPrototypeOf(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("propertyIsEnumerable", Configurable = false, Enumerable = false)]
        public IJsValue JsPropertyIsEnumerable(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("toLocaleString", Configurable = false, Enumerable = false)]
        public IJsValue JsToLocaleString(IJsValue[] args)
        {
            return JsToString(args);
        }
        [JsInstanceMethod("toString", Configurable = false, Enumerable = false)]
        public IJsValue JsToString(IJsValue[] args)
        {
            return Environment.CreateString(ToString());
        }

        [JsInstanceMethod("valueOf", Configurable = false, Enumerable = false)]
        public IJsValue JsValueOf(IJsValue[] args)
        {
            return this;
        }

        #region IJsObject Members

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
            return new ObjectPropertyDescriptor(null /* no owner, yet... */, name, JsUndefined.Value,
                                                PropertyDescriptorFlags.Configurable |
                                                PropertyDescriptorFlags.Enumerable | PropertyDescriptorFlags.Writable);
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

        public virtual bool Extensible { get { return true; } }

        public virtual IJsObject GetPrototype()
        {
            return Prototype as IJsObject;
        }

        public virtual IPropertyDescriptor GetOwnProperty(string name)
        {
            IPropertyDescriptor pd;
            return ((_properties != null) && _properties.TryGetValue(name, out pd)) ? pd : null;
        }

        public IEnumerable<IPropertyDescriptor> GetOwnProperties()
        {
            return _properties == null ? Enumerable.Empty<IPropertyDescriptor>() : _properties.Values;
        }

        public virtual IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<string, IPropertyDescriptor>();
            }
            return _properties[descriptor.Name] = descriptor;
        }

        public virtual IJsValue CloneTo(IEnvironment environment)
        {
            return new JsObject(Environment,Prototype);
        }

        #endregion

        public IPropertyDescriptor CreateDataProperty(string name, IJsValue value, PropertyDescriptorFlags flags)
        {
            return new ObjectPropertyDescriptor(this, name, value, flags);
        }
    }
}