using System;

namespace Yes.Runtime.Error
{
    public class JsError: ApplicationException
    {
        public JsError()
        {
        }

        protected JsError(string message): base(message)
        {
        }

        protected JsError(string format, object arg0): base(string.Format(format, arg0))
        {
        }

        protected JsError(string format, object arg0, object arg1): base(string.Format(format, arg0, arg1))
        {
        }
    }
}