using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Array: IAst {
        public IEnumerable<IAst> Members { get; set; }

        public Array(IEnumerable<IAst> members)
        {
            Members = members;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.CreateArray(Members.Select(m => m.Evaluate(environment)));
        }
    }
}