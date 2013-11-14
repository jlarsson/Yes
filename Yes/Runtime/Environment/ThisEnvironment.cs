using System;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public class ThisEnvironment : IEnvironment
    {
        private IReference _reference;

        public ThisEnvironment(IEnvironment parent, IJsValue @this)
        {
            Parent = parent;
            This = @this;
        }

        public IJsValue This { get; protected set; }
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
                return _reference ??
                       (_reference =
                        new LambdaReference("this", _ => This, delegate { throw new ApplicationException(); }));
            }
            return null;
        }
    }
}