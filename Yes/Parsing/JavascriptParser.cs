using System.Collections.Generic;
using System.Linq;
using Yes.Parsing.Tdop;

namespace Yes.Parsing
{
    public class JavascriptParser
    {
        public IEnumerable<ILexeme> Tokenize(string source)
        {
            return new JavascriptLexer().Lex(source, new JavascriptLexemeMapper());
        }

        public TAst Parse<TAst>(IAstFactory<TAst> factory, string source) where TAst : class
        {
            var grammar = JavascriptGrammar<ILexeme, TAst>.Default;
            var state = new JavascriptParserState();
            var p = new Tdop<ILexeme, TAst, IAstFactory<TAst>, IJavascriptParserState>(grammar, factory, Tokenize(source).ToList());
            return grammar.Statements(state, p);
        }
    }
}