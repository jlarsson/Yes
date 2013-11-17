namespace Yes.Runtime.Operators
{
    public interface IOperators
    {
        IUnaryOperator GetUnaryOperator(string @operator);
        IBinaryOperator GetBinaryOperator(string symbol);
        
    }
}