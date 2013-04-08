namespace Yes.Interpreter.Ast
{
    public class Eeq : AbstractBinaryOperation
    {
        public Eeq(IAst lhs, IAst rhs) : base(lhs, rhs)
        {
        }

        public override BinaryOperation Operation
        {
            get { return BinaryOperation.Eeq; }
        }
    }
}