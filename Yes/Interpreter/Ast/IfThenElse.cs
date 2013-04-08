using Yes.Interpreter.Model;

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

        public IJsValue Evaluate(IScope scope)
        {
            if (If.Evaluate(scope).IsTruthy())
            {
                return Then.Evaluate(scope);
            }
            return Else == null ? scope.CreateUndefined() : Else.Evaluate(scope);
        }

        #endregion
    }
}