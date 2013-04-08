using System.Linq;
using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Apply : IAst
    {
        public Apply(IAst function, IAst[] arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public IAst Function { get; set; }
        public IAst[] Arguments { get; set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            IJsValue self = null;
            IJsFunction function;

            var hasThis = Function as IEvaluateThisAndMember;
            if (hasThis != null)
            {
                function = hasThis.Evaluate(scope, out self) as IJsFunction;
            }
            else
            {
                function = Function.Evaluate(scope) as IJsFunction;
            }

            if (function == null)
            {
                Error.Throw("{0} is not a function", function);
            }
            return function.Apply(self, Arguments.Select(a => a.Evaluate(scope)).ToArray());
        }

        #endregion
    }
}