using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class ArrayConstructor: JsConstructorFunction, IArrayConstructor
    {
        public ArrayConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override string ToString()
        {
            return "[Function: Array]";
        }

        public override IJsValue Construct(IList<IJsValue> arguments)
        {
            if (arguments.Count == 1)
            {
                var length = arguments[0].ToArrayIndex();
                if (length.HasValue && (length >= 0))
                {
                    return new JsArray(Environment,ConstructedClass,length.Value);
                }
            }
            return new JsArray(Environment, ConstructedClass, arguments);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new ArrayConstructor(environment, Class, ConstructedClass);
        }
    }
}