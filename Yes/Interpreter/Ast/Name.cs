using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Name : IAst, INameAst, IPropertyName, ILValue
    {
        public Name(string name)
        {
            Value = name;
        }

        public string Value { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            return scope.TryGetVariable(Value) ?? scope.CreateUndefined();
        }

        #endregion

        #region ILValue Members

        public IJsValue SetValue(IScope scope, IJsValue value)
        {
            var variableScope = scope.GetVariableScope(Value) ?? scope;
            variableScope.SetVariable(Value, value);
            return value;
        }

        #endregion

        #region INameAst Members

        string INameAst.Name
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