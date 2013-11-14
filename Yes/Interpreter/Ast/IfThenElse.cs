using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class IfThenElse : IAst
    {
        public IfThenElse(IAst @if, IAst @then, IAst @else)
        {
            If = @if;
            Then = then;
            Else = @else;
        }

        public IAst If { get; protected set; }
        public IAst Then { get; protected set; }
        public IAst Else { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            if (If.Evaluate(environment).ToBoolean())
            {
                return Then.Evaluate(environment);
            }
            return Else == null ? JsUndefined.Instance : Else.Evaluate(environment);
        }

        #endregion
    }
}