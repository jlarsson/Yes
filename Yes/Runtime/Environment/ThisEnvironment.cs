using System;
using System.Collections.Generic;
using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Runtime.Environment
{
    public class FunctionEnvironment : IEnvironment, IFunctionEnvironment
    {
        private IReference _thisReference;
        private IReference _argumentsReference;

        public FunctionEnvironment(IEnvironment parent, IJsFunction function, IJsValue @this, IList<IJsValue> arguments)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            Parent = parent;
            Function = function;
            This = @this;
            Arguments = arguments;
        }

        public IJsValue This { get; protected set; }
        public IList<IJsValue> Arguments { get; set; }

        public IContext Context
        {
            get { return Parent.Context; }
        }

        public IEnvironment Parent { get; protected set; }

        public IControlFlow ControlFlow
        {
            get { return Parent.ControlFlow; }
        }

        public IReference CreateReference(string name, IJsValue value)
        {
            return Parent.CreateReference(name, value);
        }

        public IReference GetReference(string name)
        {
            return GetOwnReference(name) ?? Parent.GetReference(name);
        }

        public IReference GetOwnReference(string name)
        {
            if (string.Equals("this", name))
            {
                return _thisReference ??
                       (_thisReference =
                        new LambdaReference("this", _ => This, delegate { throw new JsReferenceException(); }));
            }
            if (string.Equals("arguments", name))
            {
                return _argumentsReference ?? (_argumentsReference = new ValueReference(CreateArgumentsObject()));
            }
            return null;
        }

        private IJsValue CreateArgumentsObject()
        {
            return Context.ArgumentsConstructor.Construct(Parent, Arguments);
        }

        public IJsFunction Function { get; protected set; }
    }
}