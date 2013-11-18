using System.Linq;
using NUnit.Framework;
using Yes.Parsing;

namespace Yes.Tests.Lexing
{
    [TestFixture]
    public class StringLexingFixture
    {
        [Test]
        public void Test()
        {
            const string source = @"
'a'
'a \r\n'
";
            Assert.That(new JavascriptLexer().Lex(source).All(l => l.Id == "(string)"));
            
        }
    }
}