using System;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime;
using Yes.Runtime.Environment;

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

        public IJsValue Evaluate(IEnvironment environment)
        {
            IJsValue self = null;
            IJsFunction function;

            var hasThis = Function as IEvaluateThisAndMember;
            if (hasThis != null)
            {
                function = hasThis.Evaluate(environment, out self) as IJsFunction;
            }
            else
            {
                function = Function.Evaluate(environment) as IJsFunction;
            }

            if (function == null)
            {
                throw new JsTypeError();
            }
            return function.Apply(self, Arguments.Select(a => a.Evaluate(environment)).ToArray());
        }

        #endregion
    }
}