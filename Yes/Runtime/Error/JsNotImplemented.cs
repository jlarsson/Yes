using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Error
{
    class JsNotImplemented: JsException
    {
        public override IJsValue ToJsValue(IEnvironment environment)
        {
            return environment.CreateError(Message);
        }
    }
}