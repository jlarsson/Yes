using System;
using System.Collections.Generic;

namespace Yes.Parsing
{
    public interface IAstFactory<TAst>
    {
        TAst Name(string value);
        TAst LiteralName(string name);
        TAst Null();
        TAst Bool(bool value);
        TAst Number(double value);
        TAst String(string value);
        TAst Function(TAst name, IList<TAst> arguments, TAst statements);
        TAst Object(IList<Tuple<TAst, TAst>> members);
        TAst Array(IList<TAst> members);
        TAst Apply(TAst function, IList<TAst> arguments);
        TAst Member(TAst instance, TAst name);
        TAst IndexedMember(TAst instance, TAst name);
        TAst Assign(TAst lhs, TAst rhs);
        TAst PreAssign(TAst lhs, TAst rhs);
        TAst PostAssign(TAst lhs, TAst rhs);
        TAst Delete(TAst ast);
        TAst UnaryOperation(string @operator, TAst value); 
        TAst BinaryOperation(string @operator, TAst lhs, TAst rhs);
        TAst Conditional(TAst test, TAst trueValue, TAst falseValue);
        TAst Block(IList<TAst> statements);
        TAst Seq(IList<TAst> statements);
        TAst Var(TAst name, TAst value);
        TAst Return(TAst value);
        TAst Break();
        TAst Continue(); 
        TAst IfThenElse(TAst @if, TAst then, TAst @else);
        TAst While(TAst condition, TAst statements);
        TAst For(TAst initial, TAst condition, TAst loop, TAst block);
        TAst ForIn(TAst binding, TAst inspected, TAst block, bool declareBinding);
        TAst Construct(TAst constructor, IList<TAst> arguments);
        TAst Throw(TAst expression);
        TAst TryCatchFinally(TryCatchFinallyParameters<TAst> p);
    }
}