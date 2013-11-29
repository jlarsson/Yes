using System;
using System.Collections.Generic;
using System.Linq;
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

        public override IJsValue Construct(IList<IJsValue> arguments)
        {
            if (arguments.Count == 0)
            {
                return Construct(Environment, "", new string[0], null);
            }

            var script = arguments[arguments.Count - 1].ToString();
            var names = arguments.Take(arguments.Count - 1).Select(a => a.ToString()).ToArray();
            var body = Environment.Context.ParseScript(script);
            return Construct(Environment, "", names, body);
        }

        public IJsFunction Construct(IEnvironment environment, string name, IList<string> argumentNames, IAst body)
        {
            return new JsFunction(environment, null, name, argumentNames, body);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new FunctionConstructor(environment, Class, ConstructedClass);
        }
    }
}