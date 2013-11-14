using System;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class ComparisonOperatorFunctions : IDefineBinaryOperatorFunctions
    {
        #region IDefineBinaryOperatorFunctions Members

        public void Define(Action<JsTypeCode, JsTypeCode, BinaryOperation, Func<IEnvironment, IJsValue, IJsValue, IJsValue>> define)
        {
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Lt,
                   (s, l, r) => s.CreateBool(((IJsNumber) l).Value < ((IJsNumber) r).Value));
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Lte,
                   (s, l, r) => s.CreateBool(((IJsNumber) l).Value <= ((IJsNumber) r).Value));
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Gt,
                   (s, l, r) => s.CreateBool(((IJsNumber) l).Value > ((IJsNumber) r).Value));
            define(JsTypeCode.Number, JsTypeCode.Number, BinaryOperation.Gte,
                   (s, l, r) => s.CreateBool(((IJsNumber) l).Value >= ((IJsNumber) r).Value));
        }

        #endregion
    }
}