using System;
using System.Collections.Generic;
using NUnit.Framework;
using Yes.Interpreter.Model;

namespace Yes.Tests.Array
{
    public class TestContext: Context
    {
        public List<string> Output { get; private set; }

        protected TestContext()
        {
        }

        public static TestContext Create()
        {
            var context = new TestContext(){Output = new List<string>()};
            context.SetHostFunction("print", (scope, self, args) =>
            {
                string s = string.Join<IJsValue>("", args);
                Console.Out.WriteLine(s);
                context.Output.Add(s);
            });
            return context;
        }
    }
    public class ToStringFixture
    {
        [Test]
        public void Test()
        {
            var context = TestContext.Create();
            context.Execute("var a = [1,2,3]; print(a);");
            CollectionAssert.AreEquivalent(new[]{"1,2,3"},context.Output);
        }
    }
}
