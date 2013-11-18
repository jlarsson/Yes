using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Yes.Parsing.Tdop;
using Yes.Runtime.Error;

namespace Yes.Parsing
{
    public class JavascriptParser
    {
        public IEnumerable<Lexeme> Tokenize(string source)
        {
            var types = new Dictionary<JavascriptLexer.LexemeType, string>()
                                                                       {
                                                                           {JavascriptLexer.LexemeType.Number, "(number)"},
                                                                           {JavascriptLexer.LexemeType.String, "(string)"},
                                                                           {JavascriptLexer.LexemeType.Name, "(name)"},
                                                                       };
            return from lexeme in new JavascriptLexer().Lex(source)
                   where lexeme.Type != JavascriptLexer.LexemeType.Comment
                   where lexeme.Type != JavascriptLexer.LexemeType.Error
                   select new Lexeme()
                              {
                                  Value = lexeme.Value,
                                  Id = types.ContainsKey(lexeme.Type) ? types[lexeme.Type] : lexeme.Value,
                                  Source = source,
                                  SourcePosition = lexeme.Position
                              };
        }

        public TAst Parse<TAst>(IAstFactory<TAst> factory, string source) where TAst : class
        {
            var grammar = JavascriptGrammar<Lexeme, TAst>.Default;
            var state = new JavascriptParserState();
            var p = new Tdop<Lexeme, TAst, IAstFactory<TAst>, IJavascriptParserState>(grammar, factory, Tokenize(source));
            p.Advance();
            return grammar.Statements(state, p);
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
                throw new JsSyntaxError();
                //throw new ParsingException(message, Source, SourcePosition);
            }

            #endregion
        }

        #endregion
    }
}