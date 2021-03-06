using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Parsing;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Operators;
using Environment = Yes.Runtime.Environment.Environment;

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

        public IAst Function(IAst name, IList<IAst> arguments, IAst statements)
        {
            return new Function(name, arguments, statements);
        }

        public IAst Object(IList<Tuple<IAst, IAst>> members)
        {
            return new ObjectLiteral(members);
        }

        public IAst Array(IList<IAst> members)
        {
            return new Array(members);
        }

        public IAst Apply(IAst function, IList<IAst> arguments)
        {
            return new Apply(function, arguments);
        }

        public IAst Member(IAst instance, IAst name)
        {
            return new Member(instance, name);
        }

        public IAst IndexedMember(IAst instance, IAst name)
        {
            return new IndexedMember(instance, name);
        }

        public IAst Assign(IAst lhs, IAst rhs)
        {
            return new Assign(lhs, rhs);
        }

        public IAst PreAssign(IAst lhs, IAst rhs)
        {
            return new PreAssign(lhs, rhs);
        }

        public IAst PostAssign(IAst lhs, IAst rhs)
        {
            return new PostAssign(lhs, rhs);
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

        public IAst Block(IList<IAst> statements)
        {
            if (statements.Count == 1)
            {
                return statements[0];
            }
            return new Block(statements);
        }

        public IAst Seq(IList<IAst> statements)
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

        public IAst Construct(IAst constructor, IList<IAst> arguments)
        {
            if (constructor is IAstWithName)
            {
                return new NamedConstruct((constructor as IAstWithName).Name, arguments);
            }
            return new Construct(constructor, arguments);
        }

        public IAst Throw(IAst expression)
        {
            return new Throw(expression);
        }

        public IAst TryCatchFinally(TryCatchFinallyParameters<IAst> p)
        {
            return new TryCatchFinally(p);
        }

        #endregion
    }

    public class TryCatchFinally : IAst
    {
        private readonly TryCatchFinallyParameters<IAst> _tcf;

        public TryCatchFinally(TryCatchFinallyParameters<IAst> tcf)
        {
            _tcf = tcf;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            try
            {
                _tcf.TryStatement.Evaluate(environment);
            }
            catch (JsException e)
            {
                if (_tcf.CatchParameters == null)
                {
                    throw;
                }
                return EvaluateCatch(environment, _tcf.CatchParameters.BindingName, _tcf.CatchParameters.CatchStatement, e.ToJsValue(environment));
            }
            finally
            {
                if (_tcf.FinallyStatement != null)
                {
                    _tcf.FinallyStatement.Evaluate(environment);
                }
            }
            return JsUndefined.Value;
        }

        private IJsValue EvaluateCatch(IEnvironment environment, IAst bindingName, IAst catchStatement, IJsValue value)
        {
            if (bindingName is IAstWithName)
            {
                environment = new Environment(environment);
                environment.CreateReference((bindingName as IAstWithName).Name, value);
            }
            catchStatement.Evaluate(environment);
            return environment.ControlFlow.ReturnValue ?? JsUndefined.Value;
        }
    }
}