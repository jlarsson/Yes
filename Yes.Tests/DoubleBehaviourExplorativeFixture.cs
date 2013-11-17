using System;
using NUnit.Framework;

namespace Yes.Tests
{
    [TestFixture]
    public class DoubleBehaviourExplorativeFixture
    {
        void Test(double a, double b, string @operator, Func<double,double,double> f)
        {
            Console.Out.WriteLine("{0}{1}{2}={3}",a,@operator,b,f(a,b));
        }
        [Test]
        public void Test()
        {
            Test(double.NegativeInfinity,double.NegativeInfinity,"*", (a,b) => a*b);
            Test(double.PositiveInfinity, double.NegativeInfinity, "*", (a, b) => a * b);
            Test(double.PositiveInfinity, double.PositiveInfinity, "*", (a, b) => a * b);
            Test(double.NegativeInfinity, double.PositiveInfinity, "*", (a, b) => a * b);

            Test(double.NegativeInfinity, 1d, "*", (a, b) => a * b);
            Test(double.PositiveInfinity, 1d, "*", (a, b) => a * b);
            Test(1d, double.NegativeInfinity, "*", (a, b) => a * b);
            Test(1d, double.PositiveInfinity, "*", (a, b) => a * b);

            Test(double.NaN, double.NaN, "*", (a, b) => a * b);
            Test(double.NegativeInfinity, double.NaN, "*", (a, b) => a * b);
            Test(double.PositiveInfinity, double.NaN, "*", (a, b) => a * b);
            Test(double.NaN, double.NegativeInfinity, "*", (a, b) => a * b);
            Test(double.NaN, double.PositiveInfinity, "*", (a, b) => a * b);

        }
    }
}