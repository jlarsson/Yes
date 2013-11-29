using System.Collections.Generic;
using Yes.Runtime;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public abstract class JsArgumentsPrototype : JsObjectPrototype, IJsArguments
    {
        public IEnvironment CallEnvironment { get; set; }
        public IList<IJsValue> Arguments { get; set; }

        protected JsArgumentsPrototype(IEnvironment environment, IEnvironment callEnvironment, IJsClass @class,
                                       IList<IJsValue> arguments)
            : base(environment, @class)
        {
            CallEnvironment = callEnvironment;
            Arguments = arguments;
        }

        [JsMember("length", Enumerable = true)
            /* Republish to make property enumerable in instance instead of just prototype*/]
        public virtual IJsValue JsLength
        {
            get { return Environment.CreateNumber(Arguments.Count); }
        }

        [JsMember("callee", Enumerable = true)]
        public IJsValue JsCallee
        {
            get
            {
                throw new JsTypeException();
/*
                // NOTE: callee is prohibited in strict mode
                //if (strict) throw Environment.CreateTypeError("");
                var env = CallEnvironment;
                while (env != null)
                {
                    if (env is IFunctionEnvironment)
                    {
                        return (env as IFunctionEnvironment).Function;
                    }
                    env = env.Parent;
                }
                return JsNull.Value;
 */
            }
        }

        public override IReference GetReference(IJsValue name)
        {
            var index = name.ToArrayIndex();
            if (index.HasValue && (index >= 0))
            {
                return GetElementReference(index.Value);
            }
            return base.GetReference(name.ToString());
        }

        protected virtual IReference GetElementReference(int index)
        {
            return new ReadonlyValueReference(index < Arguments.Count ? Arguments[index] : JsUndefined.Value);
        }
    }
}