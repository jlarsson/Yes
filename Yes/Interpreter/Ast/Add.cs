namespace Yes.Interpreter.Ast
{
    public class Add : AbstractBinaryOperation
    {
        public Add(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Add; }
        }
    }
}