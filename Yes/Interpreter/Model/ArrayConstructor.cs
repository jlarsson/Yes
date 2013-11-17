using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class ArrayConstructor: JsConstructor<JsArray>, IArrayConstructor
    {
        public ArrayConstructor(IEnvironment environment)
            : base(environment, environment.Context.GetPrototype<ArrayConstructor>())
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            var a = arguments.ToList();
            if (a.Count == 1)
            {
                var length = a[0].ToArrayIndex();
                if (length.HasValue && (length >= 0))
                {
                    return new JsArray(Environment,ClassPrototype,length.Value);
                }
            }
            return new JsArray(Environment, ClassPrototype, a);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new ArrayConstructor(environment);
        }
    }
}