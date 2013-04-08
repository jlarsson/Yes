using Yes.Interpreter;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Parsing;

namespace Yes
{
    public class Context : IContext
    {
        public Context()
        {
            Scope = new Scope();
        }

        #region IContext Members

        public IScope Scope { get; private set; }

        #endregion

        public IJsValue Execute(string source)
        {
            var ast = new JavascriptParser().Parse(new AstFactory(), source);
            return ast.Evaluate(Scope);
        }
    }
}