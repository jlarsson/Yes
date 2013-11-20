using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class FunctionConstructor : JsConstructorFunction, IFunctionConstructor
    {
        public FunctionConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override string ToString()
        {
            return "[Function: Function]";
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
            return new FunctionConstructor(environment, Class, ConstructedClass);
        }
    }
}