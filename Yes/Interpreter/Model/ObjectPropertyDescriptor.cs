using Yes.Runtime.Error;

namespace Yes.Interpreter.Model
{
    public class ObjectPropertyDescriptor: IPropertyDescriptor
    {
        public ObjectPropertyDescriptor(IJsObject owner, string name)
            : this(
                owner, name, JsUndefined.Value,
                PropertyDescriptorFlags.Enumerable | PropertyDescriptorFlags.Writable |
                PropertyDescriptorFlags.Configurable)
        {
        }

        public ObjectPropertyDescriptor(IJsObject owner, string name, IJsValue value, PropertyDescriptorFlags flags)
        {
            Owner = owner;
            Name = name;
            Value = value;
            Flags = flags;
        }

        public IJsObject Owner { get; private set; }
        public string Name { get; private set; }
        public PropertyDescriptorFlags Flags { get; private set; }

        public IJsValue Value { get; protected set; }

        public IJsValue GetValue(IJsValue self)
        {
            return Value;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            if (!Writable)
            {
                throw new JsReferenceError();
            }
            if (!ReferenceEquals(self, Owner))
            {
                // We are setting an inherite dproperty in a subclassed instance
                var @this = (self as IJsObject);

                // Is the property present in subclassed instance?
                var pd = @this.GetOwnProperty(Name);
                if (pd != null)
                {
                    return pd.SetValue(self, value);
                }

                // If not, create it and return
                @this.DefineOwnProperty(new ObjectPropertyDescriptor(@this, Name, value, Flags));
                return value;
            }

            return Value = value;
        }


        public bool Writable
        {
            get { return (Flags & PropertyDescriptorFlags.Writable) != 0; }
        }

        public bool Enumerable
        {
            get { return (Flags & PropertyDescriptorFlags.Enumerable) != 0; }
        }

        public bool Configurable
        {
            get { return (Flags & PropertyDescriptorFlags.Configurable) != 0; }
        }
    }
}