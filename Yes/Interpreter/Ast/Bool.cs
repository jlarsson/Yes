using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Bool : IAst
    {
        public Bool(bool value)
        {
            Value = value;
        }

        public bool Value { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.CreateBool(Value);
        }

        #endregion
    }
}