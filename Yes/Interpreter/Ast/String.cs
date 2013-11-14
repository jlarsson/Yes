using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class String: IAst
    {
        public string Value { get; protected set; }

        public String(string value)
        {
            Value = value;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.CreateString(Value);
        }
    }
}