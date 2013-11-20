using System.Collections.Generic;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public abstract class JsConstructorFunction<T> : JsObject, IJsConstructor, IJsFunction where T : IJsObject
    {
        protected JsConstructorFunction(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class)
        {
            ConstructedClass = constructedClass;
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