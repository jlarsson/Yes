using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Parsing;

namespace Yes.Interpreter.Ast
{
    public class AstFactory : IAstFactory<IAst>
    {
        #region IAstFactory<IAst> Members

        public IAst Name(string name)
        {
            if ("null".Equals(name))
            {
                return Null();
            }
            if ("true".Equals(name))
            {
                return Bool(true);
            }
            if ("false".Equals(name))
            {
                return Bool(false);
            }
            return new Name(name);
        }

        public IAst Null()
        {
            return new Null();
        }

        public IAst Bool(bool value)
        {
            return new Bool(value);
        }

        public IAst Number(object value)
        {
            return new Number((double) value);
        }

        public IAst String(object value)
        {
            throw new NotImplementedException();
        }

        public IAst Function(IAst name, IEnumerable<IAst> arguments, IAst statements)
        {
            return new Function(name, arguments.ToArray(), statements);
        }

        public IAst Object(IEnumerable<Tuple<IAst, IAst>> members)
        {
            return new ObjectLiteral(members);
        }

        public IAst Array(IEnumerable<IAst> members)
        {
            return new Array(members);
        }

        public IAst Apply(IAst function, IEnumerable<IAst> arguments)
        {
            return new Apply(function, arguments.ToArray());
        }

        public IAst Member(IAst instance, IAst name)
        {
            return new Member(instance, name);
        }

        public IAst Assign(IAst lhs, IAst rhs)
        {
            return new Assign(lhs, rhs);
        }

        public IAst DecAssign(IAst lhs, IAst rhs)
        {
            throw new NotImplementedException();
        }

        public IAst IncAssign(IAst lhs, IAst rhs)
        {
            throw new NotImplementedException();
        }

        public IAst Neg(IAst v)
        {
            throw new NotImplementedException();
        }

        public IAst Not(IAst v)
        {
            throw new NotImplementedException();
        }

        public IAst TypeOf(IAst v)
        {
            throw new NotImplementedException();
        }

        public IAst Eeq(IAst lhs, IAst rhs)
        {
            return new Eeq(lhs, rhs);
        }

        public IAst Eneq(IAst lhs, IAst rhs)
        {
            return new Eneq(lhs, rhs);
        }

        public IAst Eq(IAst lhs, IAst rhs)
        {
            return new Eq(lhs, rhs);
        }

        public IAst Neq(IAst lhs, IAst rhs)
        {
            return new Neq(lhs, rhs);
        }

        public IAst Gt(IAst lhs, IAst rhs)
        {
            return new Gt(lhs, rhs);
        }

        public IAst Gte(IAst lhs, IAst rhs)
        {
            return new Gte(lhs, rhs);
        }

        public IAst Lt(IAst lhs, IAst rhs)
        {
            return new Lt(lhs, rhs);
        }

        public IAst Lte(IAst lhs, IAst rhs)
        {
            return new Lte(lhs, rhs);
        }

        public IAst Add(IAst lhs, IAst rhs)
        {
            return new Add(lhs, rhs);
        }

        public IAst Sub(IAst lhs, IAst rhs)
        {
            return new Sub(lhs, rhs);
        }

        public IAst Mul(IAst lhs, IAst rhs)
        {
            return new Mul(lhs, rhs);
        }

        public IAst Div(IAst lhs, IAst rhs)
        {
            throw new NotImplementedException();
        }

        public IAst Block(IEnumerable<IAst> statements)
        {
            var l = statements.ToArray();
            if (l.Length == 1)
            {
                return l[0];
            }
            return new Block(l);
        }

        public IAst Seq(IEnumerable<IAst> statements)
        {
            return Block(statements);
        }

        public IAst Var(IAst name, IAst value)
        {
            return new Var(name, value);
        }

        public IAst Return(IAst value)
        {
            return new Return(value);
        }

        public IAst Break()
        {
            return new Break();
        }

        public IAst IfThenElse(IAst @if, IAst then, IAst @else)
        {
            return new IfThenElse(@if, @then, @else);
        }

        public IAst While(IAst condition, IAst statements)
        {
            return new While(condition, statements);
        }

        public IAst For(IAst initial, IAst condition, IAst loop, IAst block)
        {
            return new For(initial, condition, loop, block);
        }

        #endregion
    }
}