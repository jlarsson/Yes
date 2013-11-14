using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Null : IAst
    {
        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            return JsNull.Value;
        }

        #endregion
    }
}