using System;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Error
{
    public abstract class JsException: ApplicationException
    {
        protected JsException()
        {
        }

        protected JsException(string message): base(message)
        {
        }

        protected JsException(string format, object arg0): base(string.Format(format, arg0))
        {
        }

        protected JsException(string format, object arg0, object arg1): base(string.Format(format, arg0, arg1))
        {
        }

        public abstract IJsValue ToJsValue(IEnvironment environment);
    }
}