using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class ObjectConstructor : JsConstructorFunction, IObjectConstructor
    {
        public ObjectConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override string ToString()
        {
            return "[Function: Object]";
        }

        #region IObjectConstructor Members

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            var value = arguments.FirstOrDefault();
            if (value is IJsObject)
            {
                return (value as IJsObject).CloneTo(Environment);
            }
            return Construct(Environment);
        }

        public IJsObject Construct(IEnvironment environment)
        {
            return new JsObject(environment, ConstructedClass);
        }

        #endregion

        [JsMember("create", Configurable = false)]
        public IJsValue JsCreate(IJsValue[] args)
        {
            var proto = BindParameters.OfTypeOrNull<IJsObject>(args, 0);
            // TODO: Handle properties, https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/Object/create?redirect=no
            return new JsObject(Environment, new JsClass(new Dictionary<string, IPropertyDescriptor>(), proto));
        }

        [JsMember("defineProperty", Configurable = false, Enumerable = true)]
        public IJsValue JsDefineProperty(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("defineProperties", Configurable = false, Enumerable = true)]
        public IJsValue JsDefineProperties(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("getOwnPropertyDescriptor", Configurable = false, Enumerable = true)]
        public IJsValue JsGetOwnPropertyDescriptor(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("keys", Configurable = false, Enumerable = true)]
        public IJsValue Keys(IJsValue[] args)
        {
            return GetOwnPropertyNames(args);
        }

        [JsMember("getOwnPropertyNames", Configurable = false, Enumerable = true)]
        public IJsValue GetOwnPropertyNames(IJsValue[] args)
        {
            var obj = args.Select(a => a as IJsObject).FirstOrDefault();

            return Environment.CreateArray(
                (obj == null ? Enumerable.Empty<IPropertyDescriptor>() : obj.GetOwnProperties())
                .Where(pd => pd.Enumerable)
                .Select(pd => pd.Name)
                //.Distinct()
                .Select(n => Environment.CreateString(n)));
        }

        [JsMember("getPrototypeOf", Configurable = false, Enumerable = true)]
        public IJsValue JsGetPrototypeOf(IJsValue[] args)
        {
            var obj = args.FirstOrDefault();
            if (obj == null)
            {
                throw new JsReferenceException("Cannot convert parameter to object");
            }
            return obj
                .Cast<IJsObject>("Cannot convert {0} to object", obj)
                .GetPrototype();
        }

        [JsMember("preventExtensions", Configurable = false, Enumerable = true)]
        public IJsValue JsPreventExtensions(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("isExtensible", Configurable = false, Enumerable = true)]
        public IJsValue JsIsExtensible(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("seal", Configurable = false, Enumerable = true)]
        public IJsValue JsSeal(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("is", Configurable = false, Enumerable = true)]
        public IJsValue JsIs(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("isSealed", Configurable = false, Enumerable = true)]
        public IJsValue JsIsSealed(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("freeze", Configurable = false, Enumerable = true)]
        public IJsValue JsFreeze(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsMember("isFrozen", Configurable = false, Enumerable = true)]
        public IJsValue JsIsFrozen(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new ObjectConstructor(environment, Class, ConstructedClass);
        }
    }
}