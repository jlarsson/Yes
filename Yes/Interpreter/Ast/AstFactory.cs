using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Parsing;
using Yes.Runtime;
using Yes.Runtime.Operators;
using Yes.Utility;

namespace Yes.Interpreter.Ast
{
    public class AstFactory : IAstFactory<IAst>
    {
        public IOperators Operators { get; set; }

        public AstFactory(IOperators operators)
        {
            Operators = operators;
        }

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

        public IAst LiteralName(string name)
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
            return new LiteralName(name);
        }

        public IAst Null()
        {
            return new Null();
        }

        public IAst Bool(bool value)
        {
            return new Bool(value);
        }

        public IAst Number(double value)
        {
            return new Number(value);
        }

        public IAst String(string value)
        {
            return new String(value);
        }

        public IAst Function(IAst name, IEnumerable<IAst> arguments, IAst statements)
        {
            return new Function(name, arguments.ToArray(), statements);
        }

        public IAst Object(IEnumerable<Tuple<IAst, IAst>> members)
        {
            return new ObjectLiteral(members.Select(t => Tuple.Create((IAstWithName)t.Item1,t.Item2)));
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

        public IAst Delete(IAst ast)
        {
            return new Delete(ast);
        }

        public IAst UnaryOperation(string @operator, IAst value)
        {
            return new UnaryOperation(Operators.GetUnaryOperator(@operator), value);
        }

        public IAst BinaryOperation(string @operator, IAst lhs, IAst rhs)
        {
            return new BinaryOperation(Operators.GetBinaryOperator(@operator), lhs, rhs);
        }

        public IAst Conditional(IAst test, IAst trueValue, IAst falseValue)
        {
            return new Conditional(test, trueValue, falseValue);
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

        public IAst Continue()
        {
            return new Continue();
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

        public IAst ForIn(IAst binding, IAst inspected, IAst block, bool declareBinding)
        {
            return new ForIn(binding, inspected, block, declareBinding);
        }

        public IAst Construct(IAst constructor, IEnumerable<IAst> arguments)
        {
            if (constructor is IAstWithName)
            {
                return new NamedConstruct((constructor as IAstWithName).Name, arguments.ToArray());
            }
            return new Construct(constructor, arguments.ToArray());
        }

        #endregion
    }
}