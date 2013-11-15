using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Break : IAst, IAstModifiesEnvironment
    {
        public IJsValue Evaluate(IEnvironment environment)
        {
            environment.ControlFlow.Break = true;
            return JsUndefined.Value;
        }

        public bool ModifiesEnvironment
        {
            get { return true; }
        }
    }
}