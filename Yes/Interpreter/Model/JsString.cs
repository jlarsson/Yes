using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public class JsString : JsStringPrototype
    {
        private readonly string _value;

        public JsString(IEnvironment environment, IJsClass @class, string value)
            : base(environment, @class)
        {
            _value = value;
        }

        public override string Value { get { return _value; } }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsString(environment,Class,Value);
        }

        [JsMember("length", Enumerable = false, Configurable = false)]
        public IJsValue JsLength
        {
            get { return Environment.CreateNumber((Value ?? "").Length); }
        }
    }
}