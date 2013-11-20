using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsNumber : JsNumberPrototype
    {
        private readonly double _value;
        public JsNumber(IEnvironment environment, IJsClass @class, double value)
            : base(environment, @class)
        {
            _value = value;
        }

        public override double Value
        {
            get { return _value; }
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsNumber(environment,Class,Value);
        }
    }
}