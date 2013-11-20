using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Delete : IAst
    {
        public IAst Member { get; set; }

        public Delete(IAst member)
        {
            Member = member;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var lvalue = Member.Cast<ILValue>();
            return lvalue.Delete(environment);
        }
    }
}