using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Parsing;
using Yes.Runtime.Environment;

namespace Yes
{
    public class Context : IContext
    {
        public Context()
        {
            Environment = new Environment(null);
            Environment.CreateReference("Array", new ArrayConstructor(Environment));
            Environment.CreateReference("Boolean", new BooleanConstructor(Environment));
            Environment.CreateReference("Function", new FunctionConstructor(Environment));
            Environment.CreateReference("Number", new NumberConstructor(Environment));
            Environment.CreateReference("Object", new ObjectConstructor(Environment));
            Environment.CreateReference("String", new StringConstructor(Environment));
        }

        public IJsValue Execute(string source)
        {
            var ast = new JavascriptParser().Parse(new AstFactory(), source);
            return ast.Evaluate(Environment);
        }

        public IEnvironment Environment { get; protected set; }
    }
}