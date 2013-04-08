using System;
using Yes.Interpreter;
using Yes.Interpreter.Model;

namespace Yes
{
    public static class ContextSyntax
    {
        public static void AddPrintFunction(this IContext context)
        {
            context.SetHostFunction("print", args => Console.Out.WriteLine(string.Join<IJsValue>("", args)));
        }

        public static void SetHostFunction(this IContext context, string name, Action function)
        {
            SetHostFunction(context, name, (scope, args) =>
                                               {
                                                   function();
                                                   return scope.CreateUndefined();
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Action<IJsValue[]> function)
        {
            SetHostFunction(context, name, (scope, args) =>
                                               {
                                                   function(args);
                                                   return scope.CreateUndefined();
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Action<IScope, IJsValue[]> function)
        {
            SetHostFunction(context, name, (scope, args) =>
                                               {
                                                   function(scope, args);
                                                   return scope.CreateUndefined();
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Func<IScope, IJsValue> function)
        {
            SetHostFunction(context, name, (scope, args) => function(scope));
        }

        public static void SetHostFunction(this IContext context, string name,
                                           Func<IScope, IJsValue[], IJsValue> function)
        {
            context.Scope.SetVariable(name, context.Scope.CreateHostFunction(function));
        }
    }
}