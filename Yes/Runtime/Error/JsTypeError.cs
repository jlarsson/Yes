namespace Yes.Runtime.Error
{
    public class JsTypeError: JsError
    {
        public JsTypeError()
        {
        }

        public JsTypeError(string format, object arg0): base(format,arg0)
        {
        }
    }
}