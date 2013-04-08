using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Function : IAst
    {
        public Function(IAst name, IEnumerable<IAst> arguments, IAst statements)
        {
            Statements = statements;
            Name = name == null ? null : ((INameAst) name).Name;
            Arguments = arguments.Select(a => (INameAst) a).Select(n => n.Name).ToArray();
        }

        public IAst Statements { get; protected set; }

        public string[] Arguments { get; protected set; }

        public string Name { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            var f = scope.CreateFunction(Name, Arguments, Statements);
            if (!string.IsNullOrEmpty(Name))
            {
                scope.SetVariable(Name, f);
            }
            return f;
        }

        #endregion
    }
}