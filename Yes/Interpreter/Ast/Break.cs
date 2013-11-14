using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Break: IAst{
        public IJsValue Evaluate(IEnvironment environment)
        {
            environment.ControlFlow.Break = true;
            return JsUndefined.Instance;
        }
    }
}