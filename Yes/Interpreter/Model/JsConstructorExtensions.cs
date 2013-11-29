using System.Collections.Generic;
using System.Linq;

namespace Yes.Interpreter.Model
{
    public static class JsConstructorExtensions
    {
        public static IJsValue Construct(this IJsConstructor constructor, IEnumerable<IJsValue> arguments)
        {
            return constructor.Construct(arguments.ToList());
        }
    }
}