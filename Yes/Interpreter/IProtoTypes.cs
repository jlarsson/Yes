using Yes.Interpreter.Model;

namespace Yes.Interpreter
{
    public interface IProtoTypes{
        IJsValue Undefined { get; }
        IJsValue Bool { get; }
        IJsValue Number { get; }
        IJsValue Object { get; }
        IJsValue Function { get; }
    }
}