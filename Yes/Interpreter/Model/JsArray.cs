using System.Collections.Generic;
using Yes.Interpreter.Ast;

namespace Yes.Interpreter.Model
{
    public class JsArray: JsCommonObject, IJsArray
    {
        public JsArray(IScope scope, IEnumerable<IAst> members) : base(scope)
        {
        }
    }
}