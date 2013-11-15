using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Parsing;
using Yes.Runtime;
using Yes.Runtime.Environment;

namespace Yes
{
    public class Context : IContext
    {
        public Context()
        {
            Operators = new Operators();
            Environment = new Environment(this);
            ArrayConstructor = new ArrayConstructor(Environment);
            BooleanConstructor = new BooleanConstructor(Environment);
            FunctionConstructor = new FunctionConstructor(Environment);
            NumberConstructor = new NumberConstructor(Environment);
            ObjectConstructor = new ObjectConstructor(Environment);
            StringConstructor = new StringConstructor(Environment);

            Environment.CreateReference("Array", ArrayConstructor);
            Environment.CreateReference("Boolean", BooleanConstructor);
            Environment.CreateReference("Function", FunctionConstructor);
            Environment.CreateReference("Number", NumberConstructor);
            Environment.CreateReference("Object", ObjectConstructor);
            Environment.CreateReference("String", StringConstructor);
        }

        public IJsValue Execute(string source)
        {
            var ast = new JavascriptParser().Parse(new AstFactory(Operators), source);
            return ast.Evaluate(Environment);
        }

        public IOperators Operators { get; set; }

        public IEnvironment Environment { get; protected set; }

        public IArrayConstructor ArrayConstructor { get; private set; }

        public IBooleanConstructor BooleanConstructor { get; private set; }
        public IFunctionConstructor FunctionConstructor { get; private set; }
        public INumberConstructor NumberConstructor { get; private set; }
        public IObjectConstructor ObjectConstructor { get; private set; }
        public IStringConstructor StringConstructor { get; private set; }
    }
}