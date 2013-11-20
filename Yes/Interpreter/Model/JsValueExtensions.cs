using Yes.Runtime.Error;

namespace Yes.Interpreter.Model
{
    public static class JsValueExtensions
    {
        public static T Cast<T>(this IJsValue value) where T : class
        {
            var t = value as T;
            if (t == null)
            {
                throw new JsTypeError();
            }
            return t;
        }

        public static T Cast<T>(this IJsValue value, string format, object arg0) where T : class
        {
            var t = value as T;
            if (t == null)
            {
                throw new JsTypeError(format, arg0);
            }
            return t;
        }
    }
}