using System;
using System.Linq;
using Yes.Interpreter;
using Yes.Interpreter.Model;

namespace Yes
{
    public static class ContextSyntax
    {
        public static void AddPrintFunction(this IContext context)
        {
            context.SetHostFunction("print",
                                    (scope, self, args) => Console.Out.WriteLine(string.Join<IJsValue>("", args)));
        }

        public static void SetHostFunction(this IContext context, string name, Action function)
        {
            SetHostFunction(context, name, (scope, self, args) =>
                                               {
                                                   function();
                                                   return scope.CreateUndefined();
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Action<IJsValue[]> action)
        {
            SetHostFunction(context, name, (scope, self, args) =>
                                               {
                                                   action(args);
                                                   return scope.CreateUndefined();
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Action<IScope, IJsValue[]> action)
        {
            SetHostFunction(context, name, (scope, self, args) =>
                                               {
                                                   action(scope, args);
                                                   return scope.CreateUndefined();
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Func<IScope, IJsValue> function)
        {
            SetHostFunction(context, name, (scope, args) => function(scope));
        }

        public static void SetHostFunction(this IContext context, string name,
                                           Func<IScope, IJsValue, IJsValue[], IJsValue> function)
        {
            context.Scope.SetVariable(name, context.Scope.CreateHostFunction(function));
        }
    
        public static void SetHostFunction(this IContext context, string name,
                                           Action<IScope, IJsValue, IJsValue[]> action)
        {
            context.Scope.SetVariable(name, context.Scope.CreateHostFunction((scope,self, args) =>
                                                                                 {
                                                                                     action(scope, self, args);
                                                                                     return scope.CreateUndefined();
                                                                                 }));
        }
    }
}