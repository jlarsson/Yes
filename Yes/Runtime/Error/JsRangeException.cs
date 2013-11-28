using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Error
{
    public class JsRangeException : JsException
    {
        public override IJsValue ToJsValue(IEnvironment environment)
        {
            return environment.CreateRangeError(Message);
        }
    }
}