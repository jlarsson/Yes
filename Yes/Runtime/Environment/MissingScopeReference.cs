using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Runtime.Environment
{
    public class MissingScopeReference : IReference
    {
        public string Name { get; protected set; }

        public MissingScopeReference(string name)
        {
            Name = name;
        }

        public IJsValue GetValue(IJsValue self)
        {
            return JsUndefined.Value;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            throw new JsReferenceException("{0} is not defined", Name);
        }
    }
}