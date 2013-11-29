using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Function : IAst
    {
        public Function(IAst name, IList<IAst> arguments, IAst statements)
        {
            Statements = statements;
            Name = name == null ? null : name.ReferenceCast<IAstWithName>().Name;
            Arguments = arguments;
        }

        public IAst Statements { get; protected set; }

        public IList<IAst> Arguments { get; protected set; }

        public string Name { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            var f = environment.CreateFunction(
                Name, 
                Arguments.Select(a => a.ReferenceCast<IAstWithName>().Name).ToList(), 
                Statements);
            if (!string.IsNullOrEmpty(Name))
            {
                environment.CreateReference(Name,f);
            }
            return f;
        }

        #endregion
    }
}