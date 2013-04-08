using Yes.Interpreter.Model;

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

        public IJsValue Evaluate(IScope scope)
        {
            return scope.CreateBool(Value);
        }

        #endregion
    }
}