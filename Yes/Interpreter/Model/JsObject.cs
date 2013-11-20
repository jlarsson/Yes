using System.Collections.Generic;
using System.Linq;
using Yes.Runtime;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public abstract class JsObjectPrototype : IJsObject
    {
        private Dictionary<string, IPropertyDescriptor> _properties;

        protected JsObjectPrototype(IEnvironment environment, IJsClass @class)
        {
            Environment = environment;
            Class = @class ?? JsClass.Default;
            Prototype = Class.Prototype;
        }

        protected IJsClass Class { get; set; }

        public IEnvironment Environment { get; private set; }
        public IJsObject Prototype { get; protected set; }

        [JsMember("hasOwnProperty", Configurable = false, Enumerable = true)]
        public IJsValue JsHasOwnProperty(IJsValue[] args)
        {
            return Environment.CreateBool(GetOwnProperty(args.Select(a => a.ToString()).FirstOrDefault()) != null);
        }

        [JsMember("isPrototypeOf", Configurable = false, Enumerable = true)]
        public IJsValue JsIsPrototypeOf(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("propertyIsEnumerable", Configurable = false, Enumerable = true)]
        public IJsValue JsPropertyIsEnumerable(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("toLocaleString", Configurable = false, Enumerable = true)]
        public IJsValue JsToLocaleString(IJsValue[] args)
        {
            return JsToString(args);
        }

        [JsMember("toString", Configurable = false, Enumerable = true)]
        public IJsValue JsToString(IJsValue[] args)
        {
            return Environment.CreateString(ToString());
        }

        [JsMember("valueOf", Configurable = false, Enumerable = true)]
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
            var pd = GetProperty(name);
            if (pd != null)
            {
                // Prperty found, return its reference
                return pd;
            }
            return Extensible
                       ? (IReference)new ObjectPropertyDescriptor(
                           null /* no owner, yet... */,
                           name,
                           JsUndefined.Value,
                           PropertyDescriptorFlags.Configurable | PropertyDescriptorFlags.Enumerable | PropertyDescriptorFlags.Writable)
                       : new ReadonlyReference();
        }

        public virtual int? ToArrayIndex()
        {
            return null;
        }

        public JsVariant ToVariant()
        {
            return new JsVariant(this);
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

        public virtual bool Extensible
        {
            get { return true; }
        }

        public virtual IJsObject GetPrototype()
        {
            return Prototype;
        }

        public virtual IPropertyDescriptor GetOwnProperty(string name)
        {
            IPropertyDescriptor pd;
            return ((_properties != null) && _properties.TryGetValue(name, out pd)) ? pd : Class.GetInstanceProperty(name);
        }

        public IPropertyDescriptor GetProperty(string name)
        {
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
            return null;
        }

        public IEnumerable<IPropertyDescriptor> GetProperties()
        {
            IJsObject proto = this;
            while (proto != null)
            {
                foreach (var pd in proto.GetOwnProperties())
                {
                    yield return pd;
                }
                proto = proto.GetPrototype();
            }
        }

        public bool HasProperty(string name)
        {
            return GetProperty(name) != null;
        }

        public bool DeleteProperty(string name)
        {
            var pd = GetOwnProperty(name);
            if (pd == null)
            {
                return true;
            }
            if (pd.Configurable)
            {
                _properties.Remove(name);
                return true;
            }
            throw new JsTypeError();
        }

        public IEnumerable<IPropertyDescriptor> GetOwnProperties()
        {
            return (_properties == null ? Enumerable.Empty<IPropertyDescriptor>() : _properties.Values).
                Concat(Class.GetInstanceProperties());
        }

        public virtual IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor)
        {
            if (!Extensible)
            {
                throw new JsTypeError();
            }
            var existing = GetOwnProperty(descriptor.Name);
            if ((existing != null) && !existing.Configurable)
            {
                throw new JsTypeError();
            }
            if (_properties == null)
            {
                _properties = new Dictionary<string, IPropertyDescriptor>();
            }

            return _properties[descriptor.Name] = descriptor;
        }

        public virtual IJsValue CloneTo(IEnvironment environment)
        {
            return new JsObject(Environment, Class);
        }

        #endregion
    }

    public class JsObject: JsObjectPrototype
    {
        public JsObject(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }
    }
}