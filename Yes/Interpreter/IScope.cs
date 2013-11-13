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
        IJsValue SetVariable(string name, IJsValue value);
        IJsValue CreateUndefined();
        IJsNull CreateNull();
        IJsBool CreateBool(bool value);
        IJsNumber CreateNumber(double value);
        IJsValue CreateString(string value);
        IJsFunction CreateFunction(string name, string[] arguments, IAst statements);
        IJsValue CreateHostFunction(Func<IScope, IJsValue, IJsValue[], IJsValue> function);
        IJsValue CreateObject();
        IJsValue CreateArray(IEnumerable<IAst> members);

        IJsValue Throw(string format, params object[] args);
    }
}