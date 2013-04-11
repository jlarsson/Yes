using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Yes.Interpreter;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Parsing;

namespace Yes.Tests
{
    [TestFixture]
    public class ExplorativeFixture
    {
        [Test]
        public void CustomInstance()
        {
            var context = new Context();
            context.AddPrintFunction();

            var console = context.Scope.CreateObject(
                new Dictionary<string,IJsValue>()
                    {
                        {"log", context.Scope.CreateHostFunction(
                            (scope,args) =>
                                {
                                    Console.Out.WriteLine(string.Join<IJsValue>("", args));
                                    return scope.CreateUndefined();
                                }
                            )
                            }
                    }
                    .Select(kv => Tuple.Create(kv.Key,kv.Value))
                );
            context.Scope.SetVariable("console", console);


            context.Execute("var x = 1; console.log(x,2,3);");

        }
        [Test]
        public void Array()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("var x = [1,2,3,4,5,6,7,8];");
        }

        [Test]
        public void This()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("var x = {a:123,f: function (){print(this.a);}};x.f();");
        }

        [Test]
        public void For()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("for (var i = 0; i < 10; i = i+1){print(i); }");
        }

        [Test]
        public void Assignment2()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("var p = print; var x ={}; p(x.a);x.a = 2; p(x.a);");
        }

        [Test]
        public void Assignment()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("var x =1; print(x);x = 2; print(x);");
        }

        [Test]
        public void ObjectLiteral()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("var x = {a:1,b:2,c: function () {return this.a;}}; print(x.c());");
        }

        [Test]
        public void Var()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("var x = 123; print(x);");
        }

        [Test]
        public void Fib()
        {
            var context = new Context();

            context.AddPrintFunction();

            context.Execute("function fac(n) { if(n < 2) { return n; } else { { return n*fac(n-1); } }}");

            var v = (context.Scope.TryGetVariable("fac") as IJsFunction).Apply(null, context.Scope.CreateNumber(6));

            Console.Out.WriteLine(v);
        }

        [Test]
        public void T2()
        {
            var context = new Context();

            context.AddPrintFunction();

            context.Execute("function f(a,b) {return a+b}; print(f(1,2));");


            var v = (context.Scope.TryGetVariable("f") as IJsFunction).Apply(null, context.Scope.CreateNumber(11),
                context.Scope.CreateNumber(22)
                );
        }

        [Test]
        public void Test()
        {
            //var x = new JavascriptParser().Parse(new AstFactory(), "1*2+3*4");
            var x = new JavascriptParser().Parse(new AstFactory(), "function f(a,b) {return a+b}; print(f(1,2));");
            var scope = new Scope();
            scope.SetVariable("print", new JsHostFunction(scope, (s, args) =>
                                                                     {
                                                                         Console.Out.WriteLine(string.Join("",
                                                                                                           args.Select
                                                                                                               (a =>
                                                                                                                a.
                                                                                                                    ToString
                                                                                                                    ())));
                                                                         return s.CreateUndefined();
                                                                     }));

            x.Evaluate(scope);
        }
    }
}