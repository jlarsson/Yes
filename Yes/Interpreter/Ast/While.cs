using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

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

        public IJsValue Evaluate(IEnvironment environment)
        {
            var flow = environment.ControlFlow;
            flow.Break = false;
            while (Condition.Evaluate(environment).ToBoolean())
            {
                Statements.Evaluate(environment);

                if (flow.Return || flow.Break)
                {
                    flow.Break = false;
                    break;
                }
            }
            return JsUndefined.Instance;
        }

        #endregion
    }
}