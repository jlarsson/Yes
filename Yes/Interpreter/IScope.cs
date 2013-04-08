using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;

namespace Yes.Interpreter
{
    public interface IScope
    {
        IProtoTypes ProtoTypes { get; }
        bool Break { get; set; }
        bool Return { get; }
        IJsValue ReturnValue { get; set; }
        IScope CreateChildScope();
        IScope GetVariableScope(string name); 
        IJsValue TryGetVariable(string name);
        void SetVariable(string name, IJsValue value);
        IJsUndefined CreateUndefined();
        IJsNull CreateNull();
        IJsBool CreateBool(bool value);
        IJsNumber CreateNumber(double value);
        IJsFunction CreateFunction(string name, string[] arguments, IAst statements);
        IJsValue CreateHostFunction(Func<IScope, IJsValue[], IJsValue> function);
        IJsValue CreateObject(IEnumerable<Tuple<string, IJsValue>> members);
    }
}