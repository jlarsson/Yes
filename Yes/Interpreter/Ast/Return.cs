using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Return : IAst
    {
        public Return(IAst value)
        {
            Value = value;
        }

        public IAst Value { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.ControlFlow.ReturnValue = (Value == null ? JsUndefined.Value : Value.Evaluate(environment));
        }

        #endregion
    }
}