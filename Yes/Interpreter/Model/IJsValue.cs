using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public interface IJsValue
    {
        JsTypeCode TypeCode { get; }
        IReference GetReference(IJsValue name);
        IReference GetReference(string name);

        int? ToArrayIndex();
        bool ToBoolean();
        double ToNumber();
        string ToString();
    }
}