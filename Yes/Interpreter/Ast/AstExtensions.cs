using Yes.Runtime.Error;

namespace Yes.Interpreter.Ast
{
    public static class AstExtensions
    {
        public static T ReferenceCast<T>(this IAst ast) where T : class
        {
            var t = ast as T;
            if (t == null)
            {
                throw new JsReferenceError();
            }
            return t;
        }

        public static T ReferenceCast<T>(this IAst ast, string message) where T : class
        {
            var t = ast as T;
            if (t == null)
            {
                throw new JsReferenceError(message);
            }
            return t;
        }
    }
}