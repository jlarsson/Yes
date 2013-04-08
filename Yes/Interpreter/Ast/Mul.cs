namespace Yes.Interpreter.Ast
{
    public class Mul : AbstractBinaryOperation
    {
        public Mul(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Mul; }
        }
    }
}