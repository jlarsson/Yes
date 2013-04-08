namespace Yes.Interpreter.Ast
{
    public class Eneq : AbstractBinaryOperation
    {
        public Eneq(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Eneq; }
        }
    }
}