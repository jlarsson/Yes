namespace Yes.Interpreter.Ast
{
    public interface IAstModifiesEnvironment
    {
        bool ModifiesEnvironment { get; }
    }
}