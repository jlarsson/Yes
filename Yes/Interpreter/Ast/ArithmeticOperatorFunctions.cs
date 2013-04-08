using System;
using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class ArithmeticOperatorFunctions : IDefineBinaryOperatorFunctions
    {
        #region IDefineBinaryOperatorFunctions Members

        public void Define(
            Action<JsTypeCode, JsTypeCode, BinaryOperation, Func<IScope, IJsValue, IJsValue, IJsValue>> define)
        {
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Add,
                   (s, l, r) => s.CreateNumber(((IJsNumber) l).Value + ((IJsNumber) r).Value));
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Sub,
                   (s, l, r) => s.CreateNumber(((IJsNumber) l).Value - ((IJsNumber) r).Value));
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Mul,
                   (s, l, r) => s.CreateNumber(((IJsNumber) l).Value*((IJsNumber) r).Value));
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Div,
                   (s, l, r) => s.CreateNumber(((IJsNumber) l).Value/((IJsNumber) r).Value));
        }

        #endregion
    }
}