namespace Yes.Interpreter.Ast
{
    public class Sub : AbstractBinaryOperation
    {
        public Sub(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Sub; }
        }
    }
}