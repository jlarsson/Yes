using System.Collections.Generic;
using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Array: IAst {
        public IEnumerable<IAst> Members { get; set; }

        public Array(IEnumerable<IAst> members)
        {
            Members = members;
        }

        public IJsValue Evaluate(IScope scope)
        {
            return scope.CreateArray(Members);
        }
    }
}