using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public interface IControlFlow{
        bool Break { get; set; }
        bool Continue { get; set; }
        bool Return { get; set; }
        IJsValue ReturnValue { get; set; }
    }
}