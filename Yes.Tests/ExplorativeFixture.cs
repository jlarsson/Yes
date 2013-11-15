using System;
using NUnit.Framework;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

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

            var console = context.CreateObject();
            console.GetReference("log").SetValue(console, context.CreateHostFunction((scope, self, args) => null));
            context.Environment.CreateReference("console", console);

            context.Execute("var x = 1; console.log(x,2,3);");
        }
        [Test]
        public void Array()
        {
            var context = new Context();
            context.AddPrintFunction();
            context.Execute("var x = [1,2,3,4,5,6,7,8]; print(x);");
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
            context.Execute("var x = {a:1,b:2,c: function () {return this.a+this.b;}}; print(x.c());");
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

            var v = (context.Environment.GetReference("fac").GetValue() as IJsFunction).Apply(null, context.Environment.CreateNumber(6));

            Console.Out.WriteLine(v);
        }

        [Test]
        public void T2()
        {
            var context = new Context();

            context.AddPrintFunction();

            context.Execute("function f(a,b) {return a+b}; print(f(1,2));");


            var v = (context.Environment.GetReference("f").GetValue() as IJsFunction).Apply(null, context.Environment.CreateNumber(11),
                context.CreateNumber(22)
                );
        }

        [Test]
        public void F()
        {
            var context = new Context();

            context.AddPrintFunction();

            context.Execute("var y = 'apa'; var x = [1,2,3]; x[10-9] = 35;x['0'] = 123; x[3] = 'hej'; x.push(18); print(x);");

            //context.Execute("var x = new (function (l){this.p = function (){print(l);}})(1234); x.p();");
        }
    }
}