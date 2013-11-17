using Yes.Runtime;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IJsValue
    {
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