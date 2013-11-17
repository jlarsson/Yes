using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public abstract class JsConstructor<T> : JsObject, IJsConstructor, IJsFunction where T : JsObject
    {
        protected JsConstructor(IEnvironment environment, IJsObject prototype)
            : base(environment, prototype)
        {
            ClassPrototype = environment.Context.GetPrototype<T>(this);
        }

        public IJsObject ClassPrototype { get; protected set; }

        #region IJsConstructor Members

        public abstract IJsValue Construct(IEnumerable<IJsValue> arguments);

        #endregion

        #region IJsFunction Members

        public IJsValue Apply(IJsValue @this, params IJsValue[] arguments)
        {
            return Construct(arguments);
        }

        #endregion
    }
}