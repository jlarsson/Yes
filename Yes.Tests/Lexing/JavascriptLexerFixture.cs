using System;
using NUnit.Framework;
using Yes.Parsing;

namespace Yes.Tests.Lexing
{
    [TestFixture]
    public class JavascriptLexerFixture
    {
        [Test]
        public void Test()
        {
            var source = @"
_ // hello /*
.1 ++=!=< << (0.2){} $a _2f /* x";

            foreach (var l in new JavascriptLexer().Lex(source))
            {
                Console.Out.WriteLine("{0}: {1}", l.Id, l.Value);
            }
            
        }
    }
}
