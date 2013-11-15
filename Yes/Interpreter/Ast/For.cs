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
            var needsExplicitLoopEnvironment = (Initial is IAstModifiesEnvironment) &&
                                               ((IAstModifiesEnvironment) Initial).ModifiesEnvironment;
            var needsExplicitBlockEnvironment = (Block is IAstModifiesEnvironment) &&
                                               ((IAstModifiesEnvironment)Block).ModifiesEnvironment;

            var loopEnvironment = needsExplicitLoopEnvironment ? new Environment(environment) : environment;

            if (Initial != null)
            {
                Initial.Evaluate(loopEnvironment);
            }

            while ((Condition == null) || Condition.Evaluate(loopEnvironment).ToBoolean())
            {
                var blockEnvironment = needsExplicitBlockEnvironment ? new Environment(loopEnvironment) : loopEnvironment;
                Block.Evaluate(blockEnvironment);

                if (blockEnvironment.ControlFlow.Break)
                {
                    break;
                }
                if (blockEnvironment.ControlFlow.Return)
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