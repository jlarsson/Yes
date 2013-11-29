using System.Collections.Generic;
using System.Linq;

namespace Yes.Interpreter.Model
{
    public static class JsFunctionExtensions
    {
        public static IJsValue Apply(this IJsFunction function, IJsValue @this, params IJsValue[] arguments)
        {
            return function.Apply(@this, (IList<IJsValue>)arguments);
        }
        public static IJsValue Apply(this IJsFunction function, IJsValue @this, IEnumerable<IJsValue> arguments)
        {
            return function.Apply(@this, arguments.ToList());
        }
    }
}