using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class NamedConstruct : IAst
    {
        public string Constructor { get; set; }
        public IAst[] Arguments { get; set; }

        public NamedConstruct(string constructor, IAst[] arguments)
        {
            Constructor = constructor;
            Arguments = arguments;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var ctor = environment.GetReference(Constructor).GetValue() as IJsConstructor;
            return ctor.Construct(Arguments.Select(a => a.Evaluate(environment)).ToArray());
        }
    }
}