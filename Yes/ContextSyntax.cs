using System;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

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
                                                   return JsUndefined.Instance;
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Action<IJsValue[]> action)
        {
            SetHostFunction(context, name, (scope, self, args) =>
                                               {
                                                   action(args);
                                                   return JsUndefined.Instance;
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Action<IEnvironment, IJsValue[]> action)
        {
            SetHostFunction(context, name, (env, self, args) =>
                                               {
                                                   action(env, args);
                                                   return JsUndefined.Instance;
                                               });
        }

        public static void SetHostFunction(this IContext context, string name, Func<IEnvironment, IJsValue> function)
        {
            SetHostFunction(context, name, (env, args) => function(env));
        }

        public static void SetHostFunction(this IContext context, string name,
                                           Func<IEnvironment, IJsValue, IJsValue[], IJsValue> function)
        {
            context.Environment.CreateReference(name, context.CreateHostFunction(function));
        }
    
        public static void SetHostFunction(this IContext context, string name,
                                           Action<IEnvironment, IJsValue, IJsValue[]> action)
        {
            context.Environment.CreateReference(name, context.CreateHostFunction((scope, self, args) =>
                                                                                 {
                                                                                     action(scope, self, args);
                                                                                     return JsUndefined.Instance;
                                                                                 }));
        }
    }
}