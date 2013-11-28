using System;
using System.Collections.Generic;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;

namespace Yes.Runtime.Operators
{
    public class Operators : IOperators
    {
        private static readonly Dictionary<string, IUnaryOperator> UnaryOperators =
            new Dictionary<string, IUnaryOperator>();

        private static readonly Dictionary<string, IBinaryOperator> BinaryOperators =
            new Dictionary<string, IBinaryOperator>();

        static Operators()
        {
            Unary("-", Neg);
            Unary("void",(environment,value) => JsUndefined.Value);

            Binary("+", Add);
            Binary("-", Arith((l, r) => l - r));
            Binary("*", Arith((l, r) => l*r));
            Binary("/", Arith((l, r) => Math.Abs(r) < double.Epsilon ? double.NaN : l/r));
            Binary("%", Arith((l, r) => Math.Abs(r) < double.Epsilon ? double.NaN : l - r*Math.Floor(l/r)));
            Binary("<", Relational((l, r) => l < r, (l, r) => StringComparer.Ordinal.Compare(l, r) < 0));
            Binary("<=", Relational((l, r) => l <= r, (l, r) => StringComparer.Ordinal.Compare(l, r) <= 0));
            Binary(">", Relational((l, r) => l > r, (l, r) => StringComparer.Ordinal.Compare(l, r) > 0));
            Binary(">=", Relational((l, r) => l >= r, (l, r) => StringComparer.Ordinal.Compare(l, r) >= 0));

            Binary("||", (e, a, b) => a.ToBoolean() ? a : b);
            Binary("&&", (e, a, b) => !a.ToBoolean() ? a : b);

            Binary("in", (environment, l,r) => environment.CreateBool((r is IJsObject) && ((r as IJsObject).HasProperty(l.ToString()))));
        }

        #region IOperators Members

        public IUnaryOperator GetUnaryOperator(string symbol)
        {
            IUnaryOperator @operator;
            return UnaryOperators.TryGetValue(symbol, out @operator)
                       ? @operator
                       : new UnaryOperator(symbol, delegate { throw new JsSyntaxException(); });
        }

        public IBinaryOperator GetBinaryOperator(string symbol)
        {
            IBinaryOperator @operator;
            return BinaryOperators.TryGetValue(symbol, out @operator)
                       ? @operator
                       : new BinaryOperator(symbol, delegate { throw new JsSyntaxException(); });
        }

        #endregion

        private static void Unary(string symbol, Func<IEnvironment, IJsValue, IJsValue> eval)
        {
            UnaryOperators[symbol] = new UnaryOperator(symbol, eval);
        }

        private static void Binary(string symbol, Func<IEnvironment, IJsValue, IJsValue, IJsValue> eval)
        {
            BinaryOperators[symbol] = new BinaryOperator(symbol, eval);
        }

        public static Func<IEnvironment, IJsValue, IJsValue, IJsValue> Arith(Func<double, double, double> mathOperator)
        {
            return (environment, lhs, rhs) => ArithEval(environment, lhs, rhs, mathOperator);
        }

        public static IJsValue ArithEval(IEnvironment environment, IJsValue lhs, IJsValue rhs,
                                         Func<double, double, double> mathOperator)
        {
/*
            var l = lhs.ToNumber();
            if (double.IsNaN(l) || double.IsInfinity(l))
            {
                return environment.CreateNumber(double.NaN);
            }
            var r = rhs.ToNumber();
            if (double.IsNaN(r) || double.IsInfinity(r))
            {
                return environment.CreateNumber(double.NaN);
            }
            return environment.CreateNumber(mathOperator(l, r));
 */
            var v = mathOperator(lhs.ToNumber(), rhs.ToNumber());
            return environment.CreateNumber(double.IsInfinity(v) ? double.NaN : v);
        }

        public static Func<IEnvironment, IJsValue, IJsValue, IJsValue> Relational(
            Func<double, double, bool> relationalOperator, Func<string, string, bool> stringRelationalOperator)
        {
            return
                (environment, lhs, rhs) =>
                RelationalEval(environment, lhs, rhs, relationalOperator, stringRelationalOperator);
        }

        public static IJsValue RelationalEval(IEnvironment environment, IJsValue lhs, IJsValue rhs,
                                              Func<double, double, bool> relationalOperator,
                                              Func<string, string, bool> stringRelationalOperator)
        {
            if ((lhs.ToPrimitive() is string) || (rhs.ToPrimitive() is string))
            {
                return environment.CreateBool(stringRelationalOperator(lhs.ToString(), rhs.ToString()));
            }
            var ln = lhs.ToNumber();
            if (double.IsNaN(ln))
            {
                return JsUndefined.Value;
            }
            var rn = rhs.ToNumber();
            if (double.IsNaN(rn))
            {
                return JsUndefined.Value;
            }
            return environment.CreateBool(relationalOperator(ln, rn));
        }

        public static IJsValue Add(IEnvironment environment, IJsValue lhs, IJsValue rhs)
        {
            if ((lhs.ToPrimitive() is string) || (rhs.ToPrimitive() is string))
            {
                return environment.CreateString(lhs.ToString() + rhs.ToString());
            }
            return ArithEval(environment, lhs, rhs, (l, r) => l + r);
        }

        public static IJsValue Neg(IEnvironment environment, IJsValue value)
        {
            return environment.CreateNumber(-value.ToNumber());
        }

        #region Nested type: BinaryOperator

        public class BinaryOperator : IBinaryOperator
        {
            public BinaryOperator(string symbol, Func<IEnvironment, IJsValue, IJsValue, IJsValue> eval)
            {
                Symbol = symbol;
                Eval = eval;
            }

            public string Symbol { get; private set; }
            public Func<IEnvironment, IJsValue, IJsValue, IJsValue> Eval { get; private set; }

            #region IBinaryOperator Members

            public IJsValue Evaluate(IEnvironment environment, IJsValue lhs, IJsValue rhs)
            {
                return Eval(environment, lhs, rhs);
            }

            #endregion
        }

        #endregion

        #region Nested type: UnaryOperator

        public class UnaryOperator : IUnaryOperator
        {
            public UnaryOperator(string symbol, Func<IEnvironment, IJsValue, IJsValue> eval)
            {
                Symbol = symbol;
                Eval = eval;
            }

            public string Symbol { get; protected set; }
            public Func<IEnvironment, IJsValue, IJsValue> Eval { get; protected set; }

            #region IUnaryOperator Members

            public IJsValue Evaluate(IEnvironment environment, IJsValue value)
            {
                return Eval(environment, value);
            }

            #endregion
        }

        #endregion
    }
}