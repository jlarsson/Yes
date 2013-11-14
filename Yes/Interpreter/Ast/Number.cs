using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

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

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.CreateNumber(Value);
        }

        #endregion
    }
}