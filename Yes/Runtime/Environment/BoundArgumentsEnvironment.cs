using System;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public class BoundArgumentsEnvironment : IEnvironment
    {
        private IReference[] _references;

        public BoundArgumentsEnvironment(IEnvironment parent, string[] names, IJsValue[] values)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            Parent = parent;
            Names = names;
            Values = values;
        }

        public IContext Context
        {
            get { return Parent.Context; }
        }

        public IEnvironment Parent { get; private set; }

        public IControlFlow ControlFlow
        {
            get { return Parent.ControlFlow; }
        }

        public string[] Names { get; set; }
        public IJsValue[] Values { get; set; }

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
            var index = Array.IndexOf(Names, name);
            if (index < 0)
            {
                return null;
            }
            if (_references == null)
            {
                _references = new IReference[Names.Length];
            }
            return _references[index] ??
                   (_references[index] =
                    new ValueReference((index < Values.Length ? Values[index] : null) ?? JsUndefined.Value));
        }
    }
}