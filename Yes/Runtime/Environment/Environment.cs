using System;
using System.Collections.Generic;
using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Runtime.Environment
{
    public class Environment : IEnvironment, IControlFlow
    {
        private Dictionary<string, IReference> _references;
        public IContext Context { get; protected set; }
        public IEnvironment Parent { get; protected set; }

        public IControlFlow ControlFlow
        {
            get { return this; }
        }

        public Environment(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            Context = context;
        }

        public Environment(IEnvironment parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            Parent = parent;
            Context = Parent.Context;
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
                //throw new JsReferenceError(); // TODO: throw if strict only

                // NOTE: node.js allows redeclaration
                reference.SetValue(value);
                return reference;
            }
            return _references[name] = new ValueReference(value);
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
            throw new JsReferenceException("{0} is not defined", name);
            //return new MissingScopeReference(name);
        }

        public IReference GetOwnReference(string name)
        {
            IReference reference;
            return ((_references != null) && _references.TryGetValue(name, out reference)) ? reference : null;
        }

        public bool Break { get; set; }

        public bool Continue { get; set; }

        public bool Return { get; set; }

        public IJsValue ReturnValue { get; set; }
    }
}