using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public interface IJsValue
    {
        IReference GetReference(IJsValue name);
        IReference GetReference(string name);

        int? ToArrayIndex();
        JsVariant ToVariant();
        object ToPrimitive();
        bool ToBoolean();
        double ToNumber();
        int ToInteger();
        string ToString();
    }
}