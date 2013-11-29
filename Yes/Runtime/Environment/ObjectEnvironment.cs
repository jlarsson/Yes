using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public class ObjectEnvironment: IEnvironment
    {
        public ObjectEnvironment(IEnvironment parent)
        {
            Parent = parent;
        }

        public IContext Context
        {
            get { return Parent.Context; }
        }

        public IJsObject Object { get; set; }

        public IEnvironment Parent { get; protected set; }

        public IControlFlow ControlFlow
        {
            get { return Parent.ControlFlow; }
        }

        public IReference CreateReference(string name, IJsValue value)
        {
            var reference = Object.GetReference(name);
            reference.SetValue(value);
            return reference;
        }

        public IReference GetReference(string name)
        {
            return Object.GetReference(name) ?? Parent.GetReference(name);
        }

        public IReference GetOwnReference(string name)
        {
            return Object.GetReference(name);
        }
    }
}