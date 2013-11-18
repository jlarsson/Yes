using System.Collections.Generic;

namespace Yes.Parsing
{
    public class JavascriptParserState : IJavascriptParserState
    {
        readonly Stack<ILexicalScope> _scopes = new Stack<ILexicalScope>();

        public JavascriptParserState()
        {
            NewScope();
        }

        public ILexicalScope Scope
        {
            get { return _scopes.Peek(); }
        }

        public ILexicalScope NewScope(LexicalFeature allowedFeatures = LexicalFeature.None)
        {
            var scope = new LexicalScope(this, allowedFeatures);
            _scopes.Push(scope);
            return scope;
        }

        protected void PopScope()
        {
            _scopes.Pop();
        }

        public class LexicalScope : ILexicalScope
        {
            public JavascriptParserState State { get; set; }
            public LexicalFeature AllowedFeatures { get; set; }

            public LexicalScope(JavascriptParserState state, LexicalFeature allowedFeatures)
            {
                State = state;
                AllowedFeatures = allowedFeatures;
            }

            public void Dispose()
            {
                State.PopScope();
            }

            public bool IsAllowed(LexicalFeature f)
            {
                return (AllowedFeatures & f) == f;
            }

            public bool UseStrict { get; set; }
        }
    }
}