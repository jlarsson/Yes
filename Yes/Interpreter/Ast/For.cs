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
            var loopEnvironment = new Environment(environment);

            if (Initial != null)
            {
                Initial.Evaluate(loopEnvironment);
            }

            while ((Condition == null) || Condition.Evaluate(loopEnvironment).ToBoolean())
            {
                // TODO: If block has variable declarations or control flow (break/continue/return), we need to setup an enviroment in each loop
                var blockEnvironment = new Environment(loopEnvironment);
                Block.Evaluate(blockEnvironment);

                if (blockEnvironment.ControlFlow.Break)
                {
                    break;
                }
                if (blockEnvironment.Return)
                {
                    break;
                }

                if (Loop != null)
                {
                    Loop.Evaluate(loopEnvironment);
                }
            }
            return JsUndefined.Value;
        }

        #endregion
    }
}