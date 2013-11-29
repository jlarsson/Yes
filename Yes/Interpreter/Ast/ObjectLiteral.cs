using System;
using System.Collections.Generic;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class ObjectLiteral: IAst{
        public IList<Tuple<IAst, IAst>> Members { get; set; }

        public ObjectLiteral(IList<Tuple<IAst, IAst>> members)
        {
            Members = members;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var obj = environment.CreateObject();
            foreach (var member in Members)
            {
                obj.GetReference(
                    member.Item1.ReferenceCast<IAstWithName>().Name).SetValue(obj, member.Item2.Evaluate(environment));
            }
            return obj;
        }
    }
}