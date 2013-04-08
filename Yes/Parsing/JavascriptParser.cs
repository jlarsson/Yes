using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Yes.Parsing.Tdop;

namespace Yes.Parsing
{
    public class JavascriptParser
    {
        private static readonly Regex Matcher = new Regex(
            @"\s*(===|==|!=|<<=|>>=|\+\+|--|(\d+)|(_?[a-z]+[_a-z0-9]*)|.)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public IEnumerable<Lexeme> Tokenize(string source)
        {
            return from match in Matcher.Matches(source).OfType<Match>()
                   select CreateLexeme(match, source);
        }

        public TAst Parse<TAst>(IAstFactory<TAst> factory, string source) where TAst : class
        {
            var grammar = JavascriptGrammar<Lexeme, TAst>.Default;
            var p = new Tdop<Lexeme, TAst, IAstFactory<TAst>>(grammar, factory, Tokenize(source));
            p.Advance();
            return grammar.Statements(p);
        }

        protected Lexeme CreateLexeme(Match match, string source)
        {
            var value = match.Groups[1].Value;
            double dummy;
            if (double.TryParse(value, out dummy))
            {
                return new Lexeme
                           {
                               SourcePosition = match.Index,
                               Source = source,
                               Id = "(number)",
                               Value = dummy
                           };
            }
            if (char.IsLetter(value[0]) || (value[0] == '_'))
            {
                return new Lexeme
                           {
                               SourcePosition = match.Index,
                               Source = source,
                               Id = "(name)",
                               Value = value
                           };
            }
            return new Lexeme
                       {
                           SourcePosition = match.Index,
                           Source = source,
                           Id = value,
                           Value = value
                       };
        }

        #region Nested type: Lexeme

        public class Lexeme : ILexeme
        {
            public string Source;
            public int SourcePosition;

            #region ILexeme Members

            public string Id { get; set; }
            public object Value { get; set; }

            public void Error(string message)
            {
                throw new ParsingException(message, Source, SourcePosition);
            }

            #endregion
        }

        #endregion
    }
}