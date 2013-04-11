using System;
using System.Collections.Generic;

namespace Yes.Parsing
{
    public interface IAstFactory<TAst>
    {
        TAst Name(string value);
        TAst Null();
        TAst Bool(bool value);
        TAst Number(object value);
        TAst String(object value);
        TAst Function(TAst name, IEnumerable<TAst> arguments, TAst statements);
        TAst Object(IEnumerable<Tuple<TAst, TAst>> members);
        TAst Array(IEnumerable<TAst> members);
        TAst Apply(TAst function, IEnumerable<TAst> arguments);
        TAst Member(TAst instance, TAst name);
        TAst Assign(TAst lhs, TAst rhs);
        TAst DecAssign(TAst lhs, TAst rhs);
        TAst IncAssign(TAst lhs, TAst rhs);
        TAst Neg(TAst v);
        TAst Not(TAst v);
        TAst TypeOf(TAst v);
        TAst Eeq(TAst lhs, TAst rhs);
        TAst Eneq(TAst lhs, TAst rhs);
        TAst Eq(TAst lhs, TAst rhs);
        TAst Neq(TAst lhs, TAst rhs);
        TAst Gt(TAst lhs, TAst rhs);
        TAst Gte(TAst lhs, TAst rhs);
        TAst Lt(TAst lhs, TAst rhs);
        TAst Lte(TAst lhs, TAst rhs);
        TAst Add(TAst lhs, TAst rhs);
        TAst Sub(TAst lhs, TAst rhs);
        TAst Mul(TAst lhs, TAst rhs);
        TAst Div(TAst lhs, TAst rhs);
        TAst Block(IEnumerable<TAst> statements);
        TAst Seq(IEnumerable<TAst> statements);
        TAst Var(TAst name, TAst value);
        TAst Return(TAst value);
        TAst Break(); 
        TAst IfThenElse(TAst @if, TAst then, TAst @else);
        TAst While(TAst condition, TAst statements);
        TAst For(TAst initial, TAst condition, TAst loop, TAst block);
    }
}