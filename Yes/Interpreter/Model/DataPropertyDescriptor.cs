namespace Yes.Interpreter.Model
{
    public class DataPropertyDescriptor : IPropertyDescriptor
    {
        public DataPropertyDescriptor(string name)
            : this(
                name, JsUndefined.Instance,
                PropertyDescriptorFlags.Enumerable | PropertyDescriptorFlags.Writable |
                PropertyDescriptorFlags.Configurable)
        {
        }

        public DataPropertyDescriptor(string name, IJsValue value, PropertyDescriptorFlags flags)
        {
            Name = name;
            Value = value;
            Flags = flags;
        }

        public IJsValue Value { get; protected set; }

        public IJsValue GetValue(IJsValue self)
        {
            return Value;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            return Value = value;
        }

        public string Name { get; private set; }

        public PropertyDescriptorFlags Flags { get; private set; }

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

        public IPropertyDescriptor MakeOwnCopy(IJsValue value)
        {
            return new DataPropertyDescriptor(Name, value, Flags);
        }
    }
}