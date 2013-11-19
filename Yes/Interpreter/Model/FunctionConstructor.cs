using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class FunctionConstructor : JsConstructor<JsFunction>, IFunctionConstructor
    {
        public FunctionConstructor(IEnvironment environment)
            : base(environment, environment.Context.GetClass<FunctionConstructor>())
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            throw new NotImplementedException();
        }

        public IJsFunction Construct(IEnvironment environment, string name, string[] argumentNames, IAst body)
        {
            return new JsFunction(environment, null, name, argumentNames, body);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new FunctionConstructor(environment);
        }
    }
}