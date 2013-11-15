using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime
{
    public interface IOperators
    {
        IBinaryOperator GetBinaryOperator(string symbol);
    }

    public class Operators : IOperators
    {
        public class BinaryOperator: IBinaryOperator
        {
            public string Operator { get; private set; }
            public Func<IEnvironment, IJsValue, IJsValue, IJsValue> Eval { get; private set; }

            public BinaryOperator(string @operator,Func<IEnvironment, IJsValue, IJsValue, IJsValue> eval)
            {
                Operator = @operator;
                Eval = eval;
            }

            public IJsValue Evaluate(IEnvironment environment, IJsValue lhs, IJsValue rhs)
            {
                return Eval(environment, lhs, rhs);
            }
        }

        private static readonly Dictionary<string, IBinaryOperator> BinaryOperators = new[]
                                                                           {
                                                                               new BinaryOperator("+", Add),
                                                                               new BinaryOperator("-", Arith((l,r) => l - r)),
                                                                               new BinaryOperator("*", Arith((l,r) => l * r)),
                                                                               new BinaryOperator("/", Arith((l, r) => Math.Abs(r) < double.Epsilon ? double.NaN : l / r)),
                                                                               new BinaryOperator("%", Arith((l, r) => Math.Abs(r) < double.Epsilon ? double.NaN : Math.IEEERemainder(l,r))),

                                                                               new BinaryOperator("<",Relational((l,r) => l < r, (l,r) => StringComparer.Ordinal.Compare(l,r) < 0)),
                                                                               new BinaryOperator("<=",Relational((l,r) => l <= r, (l,r) => StringComparer.Ordinal.Compare(l,r) <= 0)),
                                                                               new BinaryOperator(">",Relational((l,r) => l > r, (l,r) => StringComparer.Ordinal.Compare(l,r) > 0)),
                                                                               new BinaryOperator(">=",Relational((l,r) => l >= r, (l,r) => StringComparer.Ordinal.Compare(l,r) >= 0)),
                                                                           }
            .ToDictionary(o => o.Operator, o => o as IBinaryOperator);

        public IBinaryOperator GetBinaryOperator(string symbol)
        {
            IBinaryOperator @operator;
            return BinaryOperators.TryGetValue(symbol, out @operator)
                       ? @operator
                       : new BinaryOperator(symbol, delegate { throw new JsSyntaxError(); });
        }

        public static Func<IEnvironment, IJsValue, IJsValue, IJsValue> Arith(Func<double,double,double> mathOperator)
        {
            return (environment, lhs, rhs) => ArithEval(environment, lhs, rhs, mathOperator);
        }

        public static IJsValue ArithEval(IEnvironment environment, IJsValue lhs, IJsValue rhs, Func<double,double,double> mathOperator)
        {
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

        }

        public static Func<IEnvironment, IJsValue, IJsValue, IJsValue> Relational(Func<double,double,bool> relationalOperator, Func<string,string,bool> stringRelationalOperator)
        {
            return
                (environment, lhs, rhs) =>
                RelationalEval(environment, lhs, rhs, relationalOperator, stringRelationalOperator);
        }

        public static IJsValue RelationalEval(IEnvironment environment, IJsValue lhs, IJsValue rhs, Func<double,double,bool> relationalOperator, Func<string,string,bool> stringRelationalOperator)
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
            return ArithEval(environment,lhs,rhs,(l,r) => l+r);
        }
    }
}