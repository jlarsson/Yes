using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Block : IAst
    {
        public Block(IAst[] statements)
        {
            Statements = statements;
        }

        public IAst[] Statements { get; set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            var flow = environment.ControlFlow;
            foreach (var statement in Statements)
            {
                if (flow.Return || flow.Break)
                {
                    break;
                }
                statement.Evaluate(environment);
            }
            return flow.ReturnValue ?? JsUndefined.Instance;
        }

        #endregion
    }
}