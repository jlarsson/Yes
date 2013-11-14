using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class FunctionConstructor: JsConstructor{
        public FunctionConstructor(IEnvironment environment) : base(environment)
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            throw new System.NotImplementedException();
        }

        public static IJsValue Construct(IEnvironment environment, string name, string[] argumentNames, IAst body)
        {
            return new JsFunction(environment, null, name, argumentNames, body);
        }
    }
}