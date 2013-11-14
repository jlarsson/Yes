using System;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public abstract class AbstractBinaryOperation : IAst
    {
        private static readonly int MaxTypeCode =
            Enum.GetValues(typeof (JsTypeCode)).OfType<JsTypeCode>().Select(v => (int) v).Max() + 1;

        private static readonly int MaxOperation =
            Enum.GetValues(typeof (BinaryOperation)).OfType<BinaryOperation>().Select(v => (int) v).Max() + 1;

        private static readonly Func<IEnvironment, IJsValue, IJsValue, IJsValue>[] _lookup;

        static AbstractBinaryOperation()
        {
            _lookup = new Func<IEnvironment, IJsValue, IJsValue, IJsValue>[
                MaxTypeCode*MaxTypeCode*MaxOperation];

            var operators = new IDefineBinaryOperatorFunctions[]
                                {new ComparisonOperatorFunctions(), new ArithmeticOperatorFunctions()};

            foreach (var o in operators)
            {
                o.Define((lhs, rhs, op, f) => _lookup[GetLookupIndex(lhs, rhs, op)] = f);
            }
        }


        protected AbstractBinaryOperation(IAst lhs, IAst rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public abstract BinaryOperation Operation { get; }
        public IAst Lhs { get; protected set; }
        public IAst Rhs { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            var l = Lhs.Evaluate(environment);
            var r = Rhs.Evaluate(environment);

            var f = _lookup[GetLookupIndex(l.TypeCode, r.TypeCode, Operation)];

            return f(environment, l, r);
        }

        #endregion

        private static int GetLookupIndex(JsTypeCode lhs, JsTypeCode rhs, BinaryOperation op)
        {
            return (MaxTypeCode)*(MaxTypeCode*(int) op + (int) lhs) + ((int) rhs);
        }
    }
}