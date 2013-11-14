using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes
{
    public interface IContext
    {
        IEnvironment Environment { get; }
    }

    public static class ContextExtensions
    {
        public static IJsValue CreateArray(this IContext context, IEnumerable<IJsValue> arguments)
        {
            return context.Environment.CreateArray(arguments);
        }

        public static IJsValue CreateBool(this IContext context, bool value)
        {
            return context.Environment.CreateBool(value);
        }

        public static IJsValue CreateFunction(this IContext context, string name, string[] argumentNames, IAst body)
        {
            return context.Environment.CreateFunction(name, argumentNames, body);
        }

        public static IJsValue CreateNumber(this IContext context, double value)
        {
            return context.Environment.CreateNumber(value);
        }

        public static IJsValue CreateObject(this IContext context)
        {
            return context.Environment.CreateObject();
        }

        public static IJsValue CreateString(this IContext context, string value)
        {
            return context.Environment.CreateString(value);
        }

        public static IJsFunction CreateHostFunction(this IContext context,
                                                     Func<IEnvironment, IJsValue, IJsValue[], IJsValue> function)
        {
            return new JsHostFunction(context.Environment, function);
        }
    }
}