using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class NamedConstruct : IAst
    {
        public string Constructor { get; set; }
        public IList<IAst> Arguments { get; set; }

        public NamedConstruct(string constructor, IList<IAst> arguments)
        {
            Constructor = constructor;
            Arguments = arguments;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var ctor = environment.GetReference(Constructor).GetValue();
            return ctor
                .Cast<IJsConstructor>("{0} is not a function", Constructor)
                .Construct(Arguments.Select(a => a.Evaluate(environment)));
        }
    }
}