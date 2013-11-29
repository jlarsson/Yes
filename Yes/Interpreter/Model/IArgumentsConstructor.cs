using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IArgumentsConstructor: IJsConstructor
    {
        IJsArguments Construct(IEnvironment callEnvironment, IList<IJsValue> arguments);
    }
}