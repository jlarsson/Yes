using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public interface IEnvironment
    {
        IContext Context { get; }
        IEnvironment Parent { get; }
        IControlFlow ControlFlow { get; }
        IReference CreateReference(string name, IJsValue value);
        IReference GetReference(string name);
        IReference GetOwnReference(string name);
    }

    public static class EnvironmentExtensions
    {
        public static IJsArray CreateArray(this IEnvironment environment, IEnumerable<IJsValue> arguments)
        {
            return environment.Context.CreateArray(arguments);
        }
        public static IJsBool CreateBool(this IEnvironment environment, bool value)
        {
            return environment.Context.CreateBool(value);
        }
        public static IJsFunction CreateFunction(this IEnvironment environment, string name, string[] argumentNames, IAst body)
        {
            return environment.Context.CreateFunction(name, argumentNames, body);
        }
        public static IJsNumber CreateNumber(this IEnvironment environment, double value)
        {
            return environment.Context.CreateNumber(value);
        }
        public static IJsObject CreateObject(this IEnvironment environment)
        {
            return environment.Context.CreateObject();
        }
        public static IJsString CreateString(this IEnvironment environment, string value)
        {
            return environment.Context.CreateString(value);
        }
        public static IJsValue CreateError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.ErrorConstructor.Construct(message, fileName, lineNumber);
        }
        public static IJsValue CreateArgumentError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.ErrorConstructor.Construct(message, fileName, lineNumber);
        }
        public static IJsValue CreateEvalError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.EvalErrorConstructor.Construct(message, fileName, lineNumber);
        }
        public static IJsValue CreateRangeError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.RangeErrorConstructor.Construct(message, fileName, lineNumber);
        }
        public static IJsValue CreateReferenceError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.ReferenceErrorConstructor.Construct(message, fileName, lineNumber);
        }
        public static IJsValue CreateSyntaxError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.SyntaxErrorConstructor.Construct(message, fileName, lineNumber);
        }
        public static IJsValue CreateTypeError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.TypeErrorConstructor.Construct(message, fileName, lineNumber);
        }
        public static IJsValue CreateUriError(this IEnvironment environment, string message, string fileName = "", int lineNumber = 0)
        {
            return environment.Context.UriErrorConstructor.Construct(message, fileName, lineNumber);
        }
    }
}