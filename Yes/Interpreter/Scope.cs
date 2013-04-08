using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;

namespace Yes.Interpreter
{
    //public interface IPrototypes
    //{
    //    IJsValue FunctionPrototype { get; }
    //    IJsValue ObjectPrototype { get; }
    //}

    //public class Prototypes: IPrototypes
    //{
    //    public IJsValue FunctionPrototype { get; set; }
    //    public IJsValue ObjectPrototype { get; set; }
    //}

    public class Scope : IScope
    {
        private readonly Dictionary<string, IJsValue> _variables = new Dictionary<string, IJsValue>();

        public Scope()
        {
            ProtoTypes = new ProtoTypes()
                             {
                                 Bool = JsBool.CreatePrototype(this),
                                 Number = JsNumber.CreatePrototype(this),
                                 Object = JsCommonObject.CreatePrototype(this),
                                 Function = JsFunction.CreatePrototype(this)
                             };
        }

        protected Scope(Scope scope): this()
        {
            Parent = scope;
            ProtoTypes = Parent.ProtoTypes;
        }

        protected Scope Parent { get; set; }

        #region IScope Members

        public IScope CreateChildScope()
        {
            return new Scope(this);
        }

        public IScope GetVariableScope(string name)
        {
            var scope = this;
            while (scope != null)
            {
                if (scope._variables.ContainsKey(name))
                {
                    return scope;
                }
                scope = scope.Parent;
            }
            return null;
        }

        public IProtoTypes ProtoTypes { get; protected set; }

        public bool Break { get; set; }

        public bool Return
        {
            get { return ReturnValue != null; }
        }

        public IJsValue ReturnValue { get; set; }

        public IJsValue TryGetVariable(string name)
        {
            var scope = this;
            while (scope != null)
            {
                IJsValue value;
                if (scope._variables.TryGetValue(name, out value))
                {
                    return value;
                }
                scope = scope.Parent;
            }
            return null;
        }

        public void SetVariable(string name, IJsValue value)
        {
            _variables[name] = value;
        }

        public IJsUndefined CreateUndefined()
        {
            return new JsUndefined(this);
        }

        public IJsNull CreateNull()
        {
            return JsNull.Value;
        }

        public IJsBool CreateBool(bool value)
        {
            return new JsBool(this, value);
        }

        public IJsNumber CreateNumber(double value)
        {
            return new JsNumber(this, value);
        }

        public IJsFunction CreateFunction(string name, string[] arguments, IAst statements)
        {
            return new JsFunction(this, name, arguments, statements);
        }

        public IJsValue CreateHostFunction(Func<IScope, IJsValue[], IJsValue> function)
        {
            return new JsHostFunction(this, function);
        }

        public IJsValue CreateObject(IEnumerable<Tuple<string, IJsValue>> members)
        {
            return new JsCommonObject(this, members);
        }

        #endregion
    }
}