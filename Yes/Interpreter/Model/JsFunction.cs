using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Ast;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsFunction : JsFunctionPrototype, IJsConstructor
    {
        public JsFunction(IEnvironment environment, IJsClass @class, string name, string[] arguments, IAst body)
            : base(environment, @class)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public string Name { get; protected set; }
        public string[] Arguments { get; protected set; }
        public IAst Body { get; protected set; }

        #region IJsFunction Members

        public override IJsValue Apply(IJsValue @this, params IJsValue[] arguments)
        {
            if (Body == null)
            {
                return JsUndefined.Value;
            }
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

            return Body.Evaluate(applyEnvironment);
        }

        #endregion

        public virtual IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            var self = Environment.CreateObject();
            Apply(self, arguments.ToArray());
            return self;
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsFunction(environment, Class, Name, Arguments, Body);
        }
    }
}