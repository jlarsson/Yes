using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Number : IAst
    {
        public Number(double value)
        {
            Value = value;
        }

        public double Value { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            return scope.CreateNumber(Value);
        }

        #endregion
    }
}