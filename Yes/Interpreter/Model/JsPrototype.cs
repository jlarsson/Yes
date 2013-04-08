using System;
using System.Collections.Generic;

namespace Yes.Interpreter.Model
{
    public class JsPrototype : JsCommonObject
    {
        public JsPrototype(IScope scope) : base(scope)
        {
        }

        public JsPrototype(IScope scope, IEnumerable<Tuple<string, IJsValue>> members) : base(scope, members)
        {
        }

        public override IJsValue Prototype
        {
            get { return Scope.CreateNull(); }
        }
    }
}