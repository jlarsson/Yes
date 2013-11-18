using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class String: IAst, IAstDirective
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

        public bool IsUseStrict
        {
            get { return string.Equals("use strict", Value); }
        }
    }

    public interface IAstDirective
    {
        bool IsUseStrict { get; }
    }
}