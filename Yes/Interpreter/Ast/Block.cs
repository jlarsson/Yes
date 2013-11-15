using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Block : IAst, IAstModifiesEnvironment
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
                if (flow.Return || flow.Break || flow.Continue)
                {
                    break;
                }
                statement.Evaluate(environment);
            }
            return flow.ReturnValue ?? JsUndefined.Value;
        }

        #endregion

        public bool ModifiesEnvironment
        {
            get { return Statements.OfType<IAstModifiesEnvironment>().Any(m => m.ModifiesEnvironment); }
        }
    }
}