namespace Yes.Interpreter.Model
{
    public interface IJsObject: IJsValue
    {
        bool Extensible { get; }
        IJsObject GetPrototype();
        IPropertyDescriptor GetOwnProperty(string name);
        IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor);
    }
}