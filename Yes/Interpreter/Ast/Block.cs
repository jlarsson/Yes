using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Block : IAst
    {
        public Block(IAst[] statements)
        {
            Statements = statements;
        }

        public IAst[] Statements { get; set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            foreach (var statement in Statements)
            {
                if (scope.Return || scope.Break)
                {
                    break;
                }
                statement.Evaluate(scope);
            }
            return scope.ReturnValue ?? scope.CreateUndefined();
        }

        #endregion
    }
}