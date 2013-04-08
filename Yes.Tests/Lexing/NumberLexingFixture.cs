using System.Linq;
using NUnit.Framework;
using Yes.Parsing;

namespace Yes.Tests.Lexing
{
    [TestFixture]
    public class NumberLexingFixture
    {
        [Test]
        public void AllNumbers()
        {
            const string source = @"
                1
                .1 
                1.2
                1.23e45
                1.23e-45
                .123e45
                .123e-45
                ";
            Assert.That(new JavascriptLexer().Lex(source).All(l => l.Type == JavascriptLexer.LexemeType.Number));
        }

        [Test]
        public void StringValues()
        {
            const string source = @"
                1
                .1 
                1.2
                1.23e45
                1.23e-45
                .123e45
                .123e-45
                ";
            Assert.That(
                from l in new JavascriptLexer().Lex(source)
                select l.Value,
                Is.EquivalentTo(new string[] {"1", ".1", "1.2", "1.23e45", "1.23e-45", ".123e45", ".123e-45"}));
        }

    }
}