using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public abstract class JsBooleanPrototype: JsObjectPrototype, IJsBool{
        protected JsBooleanPrototype(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }

        public abstract bool Value { get; }

        public override int? ToArrayIndex()
        {
            return Value ? 1 : 0;
        }

        public override object ToPrimitive()
        {
            return Value;
        }

        public override bool ToBoolean()
        {
            return Value;
        }

        public override double ToNumber()
        {
            return Value ? 1d : 0d;
        }

        public override int ToInteger()
        {
            return Value ? 1 : 0;
        }

        public override string ToString()
        {
            return Value ? "true" : "false";
        }

    }
}