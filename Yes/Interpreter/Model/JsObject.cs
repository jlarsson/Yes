using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsGlobalObject: JsObject
    {
        public JsGlobalObject(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
            var context = environment.Context;
            Put("Array", context.ArrayConstructor);
            Put("Boolean", context.BooleanConstructor);
            Put("Function", context.FunctionConstructor);
            Put("Number", context.NumberConstructor);
            Put("Object", context.ObjectConstructor);
            Put("String", context.StringConstructor);

            Put("Error", context.ErrorConstructor);
            Put("EvalError", context.EvalErrorConstructor);
            Put("RangeError", context.RangeErrorConstructor);
            Put("ReferenceError", context.ReferenceErrorConstructor);
            Put("SyntaxError", context.SyntaxErrorConstructor);
            Put("TypeError", context.TypeErrorConstructor);
            Put("URIError", context.UriErrorConstructor);

            Put("Global", this);
        }

        private void Put(string name, IJsValue value)
        {
            GetReference(name).SetValue(this, value);
        }
    }

    public class JsObject: JsObjectPrototype
    {
        public JsObject(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }
    }
}