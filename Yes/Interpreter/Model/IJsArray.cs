namespace Yes.Interpreter.Model
{
    public interface IJsArray: IJsValue
    {
        int Length { get; set; }
        IJsValue this[int index] { get; set; }
        void Push(IJsValue value);
    }
}