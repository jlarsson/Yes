using System;
using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public interface IDefineBinaryOperatorFunctions
    {
        void Define(Action<JsTypeCode, JsTypeCode, BinaryOperation, Func<IScope, IJsValue, IJsValue, IJsValue>> define);
    }
}