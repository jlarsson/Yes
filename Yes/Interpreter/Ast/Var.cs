using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Var : IAst, IAstModifiesEnvironment
    {
        public Var(IAst name, IAst value)
        {
            Value = value;
            Name = ((IAstWithName) name).Name;
        }

        public IAst Value { get; protected set; }

        public string Name { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.CreateReference(Name, Value == null ? JsUndefined.Value : Value.Evaluate(environment)).GetValue();
        }

        #endregion

        public bool ModifiesEnvironment
        {
            get { return true; }
        }
    }
}