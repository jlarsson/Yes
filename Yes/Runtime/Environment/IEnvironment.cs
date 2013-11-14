using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public interface IEnvironment
    {
        IEnvironment Parent { get; }
        IControlFlow ControlFlow { get; }
        IReference CreateReference(string name, IJsValue value);
        IReference GetReference(string name);
        IReference GetOwnReference(string name);
    }

    public static class EnvironmentExtensions
    {
        public static IJsValue CreateArray(this IEnvironment environment, IEnumerable<IJsValue> arguments)
        {
            return ArrayConstructor.Construct(environment, arguments);
        }
        public static IJsValue CreateBool(this IEnvironment environment, bool value)
        {
            return BooleanConstructor.Construct(value);
        }
        public static IJsValue CreateFunction(this IEnvironment environment, string name, string[] argumentNames, IAst body)
        {
            return FunctionConstructor.Construct(environment, name, argumentNames, body);
        }
        public static IJsValue CreateNumber(this IEnvironment environment, double value)
        {
            return NumberConstructor.Construct(value);
        }
        public static IJsValue CreateObject(this IEnvironment environment)
        {
            return ObjectConstructor.Construct(environment);
        }
        public static IJsValue CreateString(this IEnvironment environment, string value)
        {
            return StringConstructor.Construct(value);
        }
    }
}