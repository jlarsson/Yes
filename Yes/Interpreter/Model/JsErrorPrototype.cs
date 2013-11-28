using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public abstract class JsErrorPrototype: JsObjectPrototype{
        protected JsErrorPrototype(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }

        public string Message { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }

        [JsMember("message", Enumerable = true)]
        public IJsValue JsMessage { get { return Environment.CreateString(Message); } set { Message = value.ToString(); } }

        [JsMember("name", Enumerable = true)]
        public IJsValue JsName { get { return Environment.CreateString(GetErrorName()); } }

        protected abstract string GetErrorName();
    }
}