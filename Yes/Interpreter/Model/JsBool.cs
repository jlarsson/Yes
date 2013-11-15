using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsBool : JsObject, IJsBool
    {
        public JsBool(IEnvironment environment, IJsObject prototype, bool value)
            : base(environment, prototype)
        {
            Value = value;
        }

        public bool Value { get; protected set; }


        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Bool; }
        }

        public override int? ToArrayIndex()
        {
            return Value ? 1 : 0;
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