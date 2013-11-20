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
            var ctor = Constructor.Evaluate(environment);
            return ctor
                .Cast<IJsConstructor>("{0} is not a function", ctor)
                .Construct(Arguments.Select(a => a.Evaluate(environment)));
        }
    }
}