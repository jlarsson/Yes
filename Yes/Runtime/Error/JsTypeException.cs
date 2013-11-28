using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Error
{
    public class JsTypeException: JsException
    {
        public JsTypeException()
        {
        }

        public JsTypeException(string format, object arg0): base(format,arg0)
        {
        }

        public override IJsValue ToJsValue(IEnvironment environment)
        {
            return environment.CreateTypeError(Message);
        }
    }
}