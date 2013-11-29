using System.Collections.Generic;

namespace Yes.Interpreter.Model
{
    public interface IJsConstructor: IJsValue
    {
        IJsValue Construct(IList<IJsValue> arguments);
    }
}