using System.Collections.Generic;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public class Environment : IEnvironment, IControlFlow
    {
        private Dictionary<string, IReference> _references;
        public IEnvironment Parent { get; protected set; }

        public IControlFlow ControlFlow
        {
            get { return this; }
        }

        public Environment(IEnvironment parent)
        {
            Parent = parent;
        }

        public IReference CreateReference(string name, IJsValue value)
        {
            if (_references == null)
            {
                _references = new Dictionary<string, IReference>();
            }
            IReference reference;
            if (_references.TryGetValue(name, out reference))
            {
                throw new JsException(); // TODO: throw if strict only
            }
            return _references[name] = reference = new ValueReference(value);
        }

        public IReference GetReference(string name)
        {
            IEnvironment env = this;
            while (env != null)
            {
                var reference = env.GetOwnReference(name);
                if (reference != null)
                {
                    return reference;
                }
                env = env.Parent;
            }
            return new MissingScopeReference();
        }

        public IReference GetOwnReference(string name)
        {
            IReference reference;
            return ((_references != null) && _references.TryGetValue(name, out reference)) ? reference : null;
        }

        public bool Break { get; set; }

        public bool Return { get; set; }

        public IJsValue ReturnValue { get; set; }
    }
}