using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;

namespace Yes.Interpreter
{
    public class Scope : IScope
    {
        private Dictionary<string, IJsValue> _variables;

        protected Dictionary<string, IJsValue> Variables
        {
            get { return _variables ?? (_variables = new Dictionary<string, IJsValue>()); }
        }

        public Scope()
        {
            ProtoTypes = new ProtoTypes()
                             {
                                 Array = new JsArrayProtoype(this),
                                 Bool = JsBool.CreatePrototype(this),
                                 Number = JsNumber.CreatePrototype(this),
                                 String = JsString.CreatePrototype(this),
                                 Object = JsCommonObject.CreatePrototype(this),
                                 Function = JsFunction.CreatePrototype(this),
                                 Undefined = JsUndefined.CreatePrototype(this)
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
                if (scope.Variables.ContainsKey(name))
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
                if (scope.Variables.TryGetValue(name, out value))
                {
                    return value;
                }
                scope = scope.Parent;
            }
            return null;
        }

        public IJsValue SetVariable(string name, IJsValue value)
        {
            return Variables[name] = value;
        }

        public IJsValue CreateUndefined()
        {
            return TryGetVariable("undefined") ?? SetVariable("undefined", new JsUndefined(this));
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

        public IJsValue CreateString(string value)
        {
            return new JsString(this, value);
        }

        public IJsFunction CreateFunction(string name, string[] arguments, IAst statements)
        {
            return new JsFunction(this, name, arguments, statements);
        }

        public IJsValue CreateHostFunction(Func<IScope, IJsValue, IJsValue[], IJsValue> function)
        {
            return new JsHostFunction(this, function);
        }

        public IJsValue CreateObject()
        {
            return new JsCommonObject(this);
        }

        public IJsValue CreateArray(IEnumerable<IAst> members)
        {
            return new JsArray(this, members);
        }

        public IJsValue Throw(string format, params object[] args)
        {
            throw new ApplicationException(string.Format(format,args));
        }

        #endregion
    }
}