using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class While : IAst
    {
        public While(IAst condition, IAst statements)
        {
            Condition = condition;
            Statements = statements;
        }

        public IAst Condition { get; protected set; }
        public IAst Statements { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            scope.Break = false;
            while (Condition.Evaluate(scope).IsTruthy())
            {
                Condition.Evaluate(scope);

                if (scope.Return || scope.Break)
                {
                    scope.Break = false;
                    break;
                }
            }
            return scope.CreateUndefined();
        }

        #endregion
    }
}