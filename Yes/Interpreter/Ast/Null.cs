using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Null : IAst
    {
        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            return scope.CreateNull();
        }

        #endregion
    }
}