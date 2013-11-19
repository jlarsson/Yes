using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public abstract class JsConstructor<T> : JsObject, IJsConstructor, IJsFunction where T : JsObject
    {
        protected JsConstructor(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
            ConstructedClass = environment.Context.GetClass<T>(this);
        }

        protected IJsClass ConstructedClass { get; set; }

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