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

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            var l = arguments.Select(a => a.ToString()).ToList();
            
            if (l.Count == 0)
            {
                return Construct(Environment, "", new string[0], null);
            }

            var script = l[l.Count - 1];
            var names = l.Take(l.Count - 1).ToArray();
            var body = Environment.Context.ParseScript(script);
            return Construct(Environment, "", names, body);
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