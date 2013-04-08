namespace Yes.Interpreter.Ast
{
    public class Lte : AbstractBinaryOperation
    {
        public Lte(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Lte; }
        }
    }
}