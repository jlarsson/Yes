namespace Yes.Interpreter.Ast
{
    public class Gte : AbstractBinaryOperation
    {
        public Gte(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Gte; }
        }
    }
}