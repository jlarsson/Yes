using Yes.Runtime.Error;

namespace Yes.Interpreter.Ast
{
    public static class AstExtensions
    {
        public static T Cast<T>(this IAst ast) where T : class
        {
            var t = ast as T;
            if (t == null)
            {
                throw new JsReferenceError();
            }
            return t;
        }
    }
}