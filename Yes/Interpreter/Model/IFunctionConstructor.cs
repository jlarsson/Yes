using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IFunctionConstructor: IJsConstructor
    {
        IJsFunction Construct(IEnvironment environment, string name, IList<string> argumentNames, IAst body);
    }
}