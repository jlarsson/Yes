using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Apply : IAst
    {
        public Apply(IAst function, IList<IAst> arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public IAst Function { get; set; }
        public IList<IAst> Arguments { get; set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            IJsValue self = null;

            var hasThis = Function as IEvaluateThisAndMember;
            var function = hasThis != null 
                ? hasThis.Evaluate(environment, out self)
                : Function.Evaluate(environment);

            return function
                .Cast<IJsFunction>("{0} is not a function", function)
                .Apply(self, Arguments.Select(a => a.Evaluate(environment)));
        }

        #endregion
    }
}