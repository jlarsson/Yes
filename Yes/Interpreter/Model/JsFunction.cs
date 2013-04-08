using System.Linq;
using Yes.Interpreter.Ast;

namespace Yes.Interpreter.Model
{
    public class JsFunction : JsCommonObject, IJsFunction
    {
        public JsFunction(IScope scope, string name, string[] arguments, IAst statements) : base(scope)
        {
            Name = name;
            Arguments = arguments;
            Statements = statements;
        }

        public string Name { get; protected set; }
        public string[] Arguments { get; protected set; }
        public IAst Statements { get; protected set; }

        #region IJsFunction Members

        public IJsValue Apply(IJsValue @this, params IJsValue[] arguments)
        {
            var argumentsScope = Scope.CreateChildScope();

            foreach (var a in arguments.Zip(Arguments, (v, n) => new
                                                                     {
                                                                         Name = n,
                                                                         Value = v
                                                                     }))
            {
                argumentsScope.SetVariable(a.Name, a.Value);
            }
            argumentsScope.SetVariable("this", @this ?? this);

            var applyScope = argumentsScope.CreateChildScope();

            return Statements.Evaluate(applyScope);
        }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Function; }
        }

        public override bool IsTruthy()
        {
            return true;
        }

        public override bool IsFalsy()
        {
            return false;
        }

        #endregion

        public new static IJsValue CreatePrototype(Scope scope)
        {
            var prototype = new JsPrototype(scope);
            return prototype;
        }
    }
}