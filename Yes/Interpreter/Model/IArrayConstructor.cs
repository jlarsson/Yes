using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IArrayConstructor: IJsConstructor
    {
        IJsArray Construct(IEnvironment environment, IEnumerable<IJsValue> arguments);
    }
}