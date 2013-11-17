using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;

namespace Yes.Runtime.Operators
{
    public class Operators : IOperators
    {
        private static readonly Dictionary<string, IBinaryOperator> BinaryOperators = new[]
                                                                                          {
                                                                                              new BinaryOperator("+",
                                                                                                                 Add),
                                                                                              new BinaryOperator("-",
                                                                                                                 Arith(
                                                                                                                     (l,
                                                                                                                      r)
                                                                                                                     =>
                                                                                                                     l -
                                                                                                                     r))
                                                                                              ,
                                                                                              new BinaryOperator("*",
                                                                                                                 Arith(
                                                                                                                     (l,
                                                                                                                      r)
                                                                                                                     =>
                                                                                                                     l*r))
                                                                                              ,
                                                                                              new BinaryOperator("/",
                                                                                                                 Arith(
                                                                                                                     (l,
                                                                                                                      r)
                                                                                                                     =>
                                                                                                                     Math
                                                                                                                         .
                                                                                                                         Abs
                                                                                                                         (r) <
                                                                                                                     double
                                                                                                                         .
                                                                                                                         Epsilon
                                                                                                                         ? double
                                                                                                                               .
                                                                                                                               NaN
                                                                                                                         : l/
                                                                                                                           r))
                                                                                              ,
                                                                                              new BinaryOperator("%",
                                                                                                                 Arith(
                                                                                                                     (l,
                                                                                                                      r)
                                                                                                                     =>
                                                                                                                     Math
                                                                                                                         .
                                                                                                                         Abs
                                                                                                                         (r) <
                                                                                                                     double
                                                                                                                         .
                                                                                                                         Epsilon
                                                                                                                         ? double
                                                                                                                               .
                                                                                                                               NaN
                                                                                                                         : Math
                                                                                                                               .
                                                                                                                               IEEERemainder
                                                                                                                               (l,
                                                                                                                                r)))
                                                                                              ,
                                                                                              new BinaryOperator("<",
                                                                                                                 Relational
                                                                                                                     ((l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      l <
                                                                                                                      r,
                                                                                                                      (l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      StringComparer
                                                                                                                          .
                                                                                                                          Ordinal
                                                                                                                          .
                                                                                                                          Compare
                                                                                                                          (l,
                                                                                                                           r) <
                                                                                                                      0))
                                                                                              ,
                                                                                              new BinaryOperator("<=",
                                                                                                                 Relational
                                                                                                                     ((l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      l <=
                                                                                                                      r,
                                                                                                                      (l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      StringComparer
                                                                                                                          .
                                                                                                                          Ordinal
                                                                                                                          .
                                                                                                                          Compare
                                                                                                                          (l,
                                                                                                                           r) <=
                                                                                                                      0))
                                                                                              ,
                                                                                              new BinaryOperator(">",
                                                                                                                 Relational
                                                                                                                     ((l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      l >
                                                                                                                      r,
                                                                                                                      (l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      StringComparer
                                                                                                                          .
                                                                                                                          Ordinal
                                                                                                                          .
                                                                                                                          Compare
                                                                                                                          (l,
                                                                                                                           r) >
                                                                                                                      0))
                                                                                              ,
                                                                                              new BinaryOperator(">=",
                                                                                                                 Relational
                                                                                                                     ((l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      l >=
                                                                                                                      r,
                                                                                                                      (l,
                                                                                                                       r)
                                                                                                                      =>
                                                                                                                      StringComparer
                                                                                                                          .
                                                                                                                          Ordinal
                                                                                                                          .
                                                                                                                          Compare
                                                                                                                          (l,
                                                                                                                           r) >=
                                                                                                                      0))
                                                                                              ,
                                                                                          }
            .ToDictionary(o => o.Symbol, o => o as IBinaryOperator);

        private static readonly Dictionary<string, IUnaryOperator> UnaryOperators = new[]
                                                                                        {
                                                                                            new UnaryOperator("-", Neg)
                                                                                        }
            .ToDictionary(o => o.Symbol, o => o as IUnaryOperator);

        #region IOperators Members

        public IUnaryOperator GetUnaryOperator(string symbol)
        {
            IUnaryOperator @operator;
            return UnaryOperators.TryGetValue(symbol, out @operator)
                       ? @operator
                       : new UnaryOperator(symbol, delegate { throw new JsSyntaxError(); });
        }

        public IBinaryOperator GetBinaryOperator(string symbol)
        {
            IBinaryOperator @operator;
            return BinaryOperators.TryGetValue(symbol, out @operator)
                       ? @operator
                       : new BinaryOperator(symbol, delegate { throw new JsSyntaxError(); });
        }

        #endregion

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
            public IJsValue Evaluate(IEnvironment environment, IJsValue value)
            {
                return Eval(environment, value);
            }
        }

        #endregion
    }
}