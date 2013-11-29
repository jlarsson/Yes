using System.Collections.Generic;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public abstract class JsFunctionPrototype: JsObjectPrototype, IJsFunction{
        protected JsFunctionPrototype(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }

        public abstract IJsValue Apply(IJsValue @this, IList<IJsValue> arguments);
    }
}