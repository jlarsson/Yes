namespace Yes.Interpreter.Ast
{
    public class Eq : AbstractBinaryOperation
    {
        public Eq(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Eq; }
        }
    }
}