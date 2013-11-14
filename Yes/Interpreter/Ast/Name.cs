using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Name : IAst, IAstWithName, IPropertyName, ILValue
    {
        public Name(string name)
        {
            Value = name;
        }

        public string Value { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            return environment.GetReference(Value).GetValue();
        }

        #endregion

        #region ILValue Members

        public IJsValue SetValue(IEnvironment environment, IJsValue value)
        {
            return environment.GetReference(Value).SetValue(value);
        }

        #endregion

        #region INameAst Members

        string IAstWithName.Name
        {
            get { return Value; }
        }

        #endregion

        #region IPropertyName Members

        public string PropertyName
        {
            get { return Value; }
        }

        #endregion
    }
}