using System;
using System.Collections.Generic;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public class BoundArgumentsEnvironment : IEnvironment
    {
        private IReference[] _references;

        public BoundArgumentsEnvironment(IEnvironment parent, IList<string> declaredArgumentNames, IList<IJsValue> actualArgumentValues)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            Parent = parent;
            DeclaredNames = declaredArgumentNames;
            ActualValues = actualArgumentValues;
        }

        public IList<IJsValue> ActualValues { get; protected set; }
        public IList<string> DeclaredNames { get; protected set; }

        public IContext Context
        {
            get { return Parent.Context; }
        }

        public IEnvironment Parent { get; private set; }

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
            var index = DeclaredNames.IndexOf(name);
            if (index < 0)
            {
                return null;
            }
            if (_references == null)
            {
                _references = new IReference[DeclaredNames.Count];
            }
            return _references[index] ??
                   (_references[index] =
                    new ValueReference((index < ActualValues.Count ? ActualValues[index] : null) ?? JsUndefined.Value));
        }
    }
}