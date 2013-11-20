namespace Yes.Runtime.Error
{
    public class JsReferenceError : JsError
    {
        public JsReferenceError()
        {
        }

        public JsReferenceError(string message): base(message)
        {
        }

        public JsReferenceError(string format, object arg0)
            : base(format, arg0)
        {
        }
        public JsReferenceError(string format, object arg0, object arg1)
            : base(format, arg0, arg1)
        {
        }
    }
}