using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class ObjectLiteral: IAst{
        public IEnumerable<Tuple<IAst, IAst>> Members { get; set; }

        public ObjectLiteral(IEnumerable<Tuple<IAst, IAst>> members)
        {
            Members = members;
        }

        public IJsValue Evaluate(IScope scope)
        {
            return scope.CreateObject();
        }
    }
}