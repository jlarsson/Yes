using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public interface IJsValue
    {
        JsTypeCode TypeCode { get; }
        IReference GetReference(IJsValue name);
        IReference GetReference(string name);

        int? ToArrayIndex();
        object ToPrimitive();
        bool ToBoolean();
        double ToNumber();
        int ToInteger();
        string ToString();
    }
}