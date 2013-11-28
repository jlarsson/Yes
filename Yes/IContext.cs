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

        IArrayConstructor ArrayConstructor { get; }
        IBooleanConstructor BooleanConstructor { get; }
        IFunctionConstructor FunctionConstructor { get; }
        INumberConstructor NumberConstructor { get; }
        IObjectConstructor ObjectConstructor { get; }
        IStringConstructor StringConstructor { get; }
        IErrorConstructor ErrorConstructor { get; }
        IErrorConstructor EvalErrorConstructor { get; }
        IErrorConstructor RangeErrorConstructor { get; }
        IErrorConstructor ReferenceErrorConstructor { get; }
        IErrorConstructor SyntaxErrorConstructor { get; }
        IErrorConstructor TypeErrorConstructor { get; }
        IErrorConstructor UriErrorConstructor { get; }
        IAst ParseScript(string source);
    }

    public static class ContextExtensions
    {
        public static IJsArray CreateArray(this IContext context, IEnumerable<IJsValue> arguments)
        {
            return context.ArrayConstructor.Construct(arguments) as IJsArray;
        }

        public static IJsBool CreateBool(this IContext context, bool value)
        {
            return context.BooleanConstructor.Construct(value);
        }

        public static IJsFunction CreateFunction(this IContext context, string name, string[] argumentNames, IAst body)
        {
            return context.FunctionConstructor.Construct(context.Environment, name, argumentNames, body);
        }

        public static IJsNumber CreateNumber(this IContext context, double value)
        {
            return context.NumberConstructor.Construct(value);
        }

        public static IJsObject CreateObject(this IContext context)
        {
            return context.ObjectConstructor.Construct(context.Environment);
        }

        public static IJsString CreateString(this IContext context, string value)
        {
            return context.StringConstructor.Construct(value);
        }

        public static IJsFunction CreateHostFunction(this IContext context,
                                                     Func<IEnvironment, IJsValue, IJsValue[], IJsValue> function)
        {
            return new JsHostFunction(context.Environment, function);
        }
    }
}