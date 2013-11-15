using Yes.Interpreter.Model;

namespace Yes.Runtime
{
    public interface IReference
    {
        IJsValue GetValue(IJsValue self = null);
        IJsValue SetValue(IJsValue self, IJsValue value);
    }

    public static class ReferenceExtensions
    {
        public static IJsValue SetValue(this IReference reference, IJsValue value)
        {
            return reference.SetValue(null, value);
        }
    }
}