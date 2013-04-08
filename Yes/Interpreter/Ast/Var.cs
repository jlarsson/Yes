using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Var : IAst
    {
        public Var(IAst name, IAst value)
        {
            Value = value;
            Name = ((INameAst) name).Name;
        }

        public IAst Value { get; protected set; }

        public string Name { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            scope.SetVariable(Name, Value == null ? scope.CreateUndefined() : Value.Evaluate(scope));
            return scope.CreateUndefined();
        }

        #endregion
    }
}