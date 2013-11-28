using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Error
{
    public class JsReferenceException : JsException
    {
        public JsReferenceException()
        {
        }

        public JsReferenceException(string message): base(message)
        {
        }

        public JsReferenceException(string format, object arg0)
            : base(format, arg0)
        {
        }
        public JsReferenceException(string format, object arg0, object arg1)
            : base(format, arg0, arg1)
        {
        }

        public override IJsValue ToJsValue(IEnvironment environment)
        {
            return environment.CreateReferenceError(Message);
        }
    }
}