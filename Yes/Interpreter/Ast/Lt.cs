namespace Yes.Interpreter.Ast
{
    public class Lt : AbstractBinaryOperation
    {
        public Lt(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Lt; }
        }
    }
}