using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Function : IAst
    {
        public Function(IAst name, IEnumerable<IAst> arguments, IAst statements)
        {
            Statements = statements;
            Name = name == null ? null : ((IAstWithName) name).Name;
            Arguments = arguments.Select(a => (IAstWithName) a).Select(n => n.Name).ToArray();
        }

        public IAst Statements { get; protected set; }

        public string[] Arguments { get; protected set; }

        public string Name { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            var f = environment.CreateFunction(Name, Arguments, Statements);
            if (!string.IsNullOrEmpty(Name))
            {
                environment.CreateReference(Name,f);
            }
            return f;
        }

        #endregion
    }
}