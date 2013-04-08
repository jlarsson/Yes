namespace Yes.Interpreter.Ast
{
    public class Neq : AbstractBinaryOperation
    {
        public Neq(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Neq; }
        }
    }
}