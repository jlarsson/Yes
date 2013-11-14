namespace Yes.Interpreter.Ast
{
    public interface IAstWithName: IAst
    {
        string Name { get; }
    }
}