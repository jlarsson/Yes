using System;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public interface IDefineBinaryOperatorFunctions
    {
        void Define(Action<JsTypeCode, JsTypeCode, BinaryOperation, Func<IEnvironment, IJsValue, IJsValue, IJsValue>> define);
    }
}