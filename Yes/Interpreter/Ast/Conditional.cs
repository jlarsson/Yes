using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Conditional : IAst
    {
        public IAst Test { get; set; }
        public IAst TrueValue { get; set; }
        public IAst FalseValue { get; set; }

        public Conditional(IAst test, IAst trueValue, IAst falseValue)
        {
            Test = test;
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            return Test.Evaluate(environment).ToBoolean()
                       ? TrueValue.Evaluate(environment)
                       : FalseValue.Evaluate(environment);
        }
    }
}