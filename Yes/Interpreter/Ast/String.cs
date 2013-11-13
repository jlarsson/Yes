using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class String: IAst
    {
        public string Value { get; protected set; }

        public String(string value)
        {
            Value = value;
        }

        public IJsValue Evaluate(IScope scope)
        {
            return scope.CreateString(Value);
        }
    }
}