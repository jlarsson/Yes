using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsBoolean : JsBooleanPrototype
    {
        private readonly bool _value;

        public JsBoolean(IEnvironment environment, IJsClass @class, bool value)
            : base(environment, @class)
        {
            _value = value;
        }

        public override bool Value { get { return _value; } }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsBoolean(environment, Class, Value);
        }
    }
}