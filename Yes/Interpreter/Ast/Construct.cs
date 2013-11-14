using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Construct : IAst
    {
        public IAst Constructor { get; set; }
        public IAst[] Arguments { get; set; }

        public Construct(IAst constructor, IAst[] arguments)
        {
            Constructor = constructor;
            Arguments = arguments;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var ctor = Constructor.Evaluate(environment) as IJsConstructor;
            return ctor.Construct(Arguments.Select(a => a.Evaluate(environment)));
        }
    }
}