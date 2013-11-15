using System.Collections.Generic;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public abstract class JsConstructor : JsObject, IJsConstructor
    {
        private IJsObject _prototype;

        protected JsConstructor(IEnvironment environment) : base(environment, null)
        {
        }

        public abstract IJsValue Construct(IEnumerable<IJsValue> arguments);

        protected IJsObject ClassPrototype
        {
            get { return _prototype ?? (_prototype = CreatePrototype()); }
        }

        protected virtual IJsObject CreatePrototype()
        {
            return new JsObject(Environment, null);
        }

        protected JsObject CreateProtypeForImplementation<T>(IJsObject basePrototype) where T: IJsObject
        {
            var proto = new JsObject(Environment, basePrototype);
            proto.DefineOwnProperty(proto.CreateDataProperty("constructor", this, PropertyDescriptorFlags.Enumerable));

            foreach (var pd in new PrototypeBuilder().CreatePropertyDescriptorsForType<T>(Environment, proto))
            {
                proto.DefineOwnProperty(pd);
            }
            return proto;
        }
    }
}