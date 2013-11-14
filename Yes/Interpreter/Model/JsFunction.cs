using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Ast;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsFunction : JsObject, IJsFunction, IJsConstructor
    {
        public JsFunction(IEnvironment environment, IJsObject prototype, string name, string[] arguments, IAst statements) : base(environment, prototype)
        {
            Name = name;
            Arguments = arguments;
            Statements = statements;
        }

        public string Name { get; protected set; }
        public string[] Arguments { get; protected set; }
        public IAst Statements { get; protected set; }

        #region IJsFunction Members

        public virtual IJsValue Apply(IJsValue @this, params IJsValue[] arguments)
        {
            var applyEnvironment =
                new Environment(
                    new ThisEnvironment(
                        new BoundArgumentsEnvironment(
                            Environment,
                            Arguments,
                            arguments
                            ),
                        @this
                        ));

            return Statements.Evaluate(applyEnvironment);
        }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Function; }
        }

        #endregion

        public IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            var self = Environment.CreateObject();
            Apply(self, arguments.ToArray());
            return self;
        }
    }
}