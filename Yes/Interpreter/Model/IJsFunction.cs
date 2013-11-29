using System.Collections.Generic;

namespace Yes.Interpreter.Model
{
    public interface IJsFunction : IJsValue
    {
        IJsValue Apply(IJsValue @this, IList<IJsValue> arguments);
    }
}