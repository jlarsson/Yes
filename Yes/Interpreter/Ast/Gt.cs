namespace Yes.Interpreter.Ast
{
    public class Gt : AbstractBinaryOperation
    {
        public Gt(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Gt; }
        }
    }
}