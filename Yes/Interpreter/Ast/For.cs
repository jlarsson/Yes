using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

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

        public IJsValue Evaluate(IEnvironment environment)
        {
            if (Initial != null)
            {
                Initial.Evaluate(environment);
            }

            var flow = environment.ControlFlow;
            flow.Break = false;
            while ((Condition == null) || Condition.Evaluate(environment).ToBoolean())
            {
                Block.Evaluate(environment);

                if (flow.Return || flow.Break)
                {
                    flow.Break = false;
                    break;
                }
                if (Loop != null)
                {
                    Loop.Evaluate(environment);
                }
            }
            return JsUndefined.Instance;
        }

        #endregion
    }
}