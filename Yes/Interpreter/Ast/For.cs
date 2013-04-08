using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class For : IAst
    {
        public For(IAst initial, IAst condition, IAst loop, IAst block)
        {
            Initial = initial;
            Condition = condition;
            Loop = loop;
            Block = block;
        }

        public IAst Initial { get; protected set; }
        public IAst Condition { get; protected set; }
        public IAst Loop { get; protected set; }
        public IAst Block { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            if (Initial != null)
            {
                Initial.Evaluate(scope);
            }

            scope.Break = false;
            while ((Condition == null) || Condition.Evaluate(scope).IsTruthy())
            {
                Block.Evaluate(scope);

                if (scope.Return || scope.Break)
                {
                    scope.Break = false;
                    break;
                }
                if (Loop != null)
                {
                    Loop.Evaluate(scope);
                }
            }
            return scope.CreateUndefined();
        }

        #endregion
    }
}