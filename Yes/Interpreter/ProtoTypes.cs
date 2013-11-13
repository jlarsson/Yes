using Yes.Interpreter.Model;

namespace Yes.Interpreter
{
    public class ProtoTypes : IProtoTypes
    {
        public IJsValue Undefined { get; set; }
        public IJsValue Array { get; set; }
        public IJsValue Bool { get; set; }
        public IJsValue Number { get; set; }
        public IJsValue String { get; set; }
        public IJsValue Object { get; set; }
        public IJsValue Function { get; set; }
    }
}