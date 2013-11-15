using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Continue : IAst, IAstModifiesEnvironment
    {
        public IJsValue Evaluate(IEnvironment environment)
        {
            environment.ControlFlow.Continue = true;
            return JsUndefined.Value;
        }

        public bool ModifiesEnvironment
        {
            get { return true; }
        }
    }
}