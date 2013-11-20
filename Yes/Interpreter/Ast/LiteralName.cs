using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class LiteralName : IAstWithName, IPropertyName
    {
        public string Name { get; set; }

        public LiteralName(string name)
        {
            Name = name;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.CreateString(Name);
        }

        public string PropertyName
        {
            get { return Name; }
        }
    }
}