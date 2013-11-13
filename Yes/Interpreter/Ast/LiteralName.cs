using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class LiteralName : IAst, INameAst, IPropertyName
    {
        public string Name { get; set; }

        public LiteralName(string name)
        {
            Name = name;
        }

        public IJsValue Evaluate(IScope scope)
        {
            return scope.CreateString(Name);
        }

        public string PropertyName
        {
            get { return Name; }
        }
    }
}