using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Return : IAst
    {
        public Return(IAst value)
        {
            Value = value;
        }

        public IAst Value { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            return scope.ReturnValue = Value == null ? scope.CreateUndefined() : Value.Evaluate(scope);
        }

        #endregion
    }
}