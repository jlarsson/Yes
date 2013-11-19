using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class ObjectConstructor : JsConstructor<JsObject>, IObjectConstructor
    {
        public ObjectConstructor(IEnvironment environment)
            : base(environment, environment.Context.GetClass<ObjectConstructor>())
        {
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

        [JsInstanceMember("create", Configurable = false)]
        public IJsValue JsCreate(IJsValue[] args)
        {
            var proto = BindParameters.OfTypeOrNull<IJsObject>(args, 0);
            // TODO: Handle properties, https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/Object/create?redirect=no
            return new JsObject(Environment, new JsClass(){Prototype = proto});
        }

        [JsInstanceMember("defineProperty", Configurable = false)]
        public IJsValue JsDefineProperty(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("defineProperties", Configurable = false)]
        public IJsValue JsDefineProperties(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("getOwnPropertyDescriptor", Configurable = false)]
        public IJsValue JsGetOwnPropertyDescriptor(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("keys", Configurable = false)]
        public IJsValue Keys(IJsValue[] args)
        {
            return GetOwnPropertyNames(args);
        }

        [JsInstanceMember("getOwnPropertyNames", Configurable = false)]
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

        [JsInstanceMember("getPrototypeOf", Configurable = false)]
        public IJsValue JsGetPrototypeOf(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("preventExtensions", Configurable = false)]
        public IJsValue JsPreventExtensions(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("isExtensible", Configurable = false)]
        public IJsValue JsIsExtensible(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("seal", Configurable = false)]
        public IJsValue JsSeal(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("is", Configurable = false)]
        public IJsValue JsIs(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("isSealed", Configurable = false)]
        public IJsValue JsIsSealed(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("freeze", Configurable = false)]
        public IJsValue JsFreeze(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMember("isFrozen", Configurable = false)]
        public IJsValue JsIsFrozen(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new ObjectConstructor(environment);
        }
    }
}