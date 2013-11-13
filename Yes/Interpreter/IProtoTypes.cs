using Yes.Interpreter.Model;

namespace Yes.Interpreter
{
    public interface IProtoTypes{
        IJsValue Undefined { get; }
        IJsValue Array { get; }
        IJsValue Bool { get; }
        IJsValue Number { get; }
        IJsValue String { get; }
        IJsValue Object { get; }
        IJsValue Function { get; }
        
    }
}