using Yes.Interpreter.Model;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;

namespace Yes.Interpreter.Ast
{
    public class Throw : IAst
    {
        public IAst Expression { get; protected set; }

        public Throw(IAst expression)
        {
            Expression = expression;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            throw new JsWrapperException(Expression.Evaluate(environment));
        }
    }
}