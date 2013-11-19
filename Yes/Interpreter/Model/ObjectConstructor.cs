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
            : base(environment, environment.Context.GetPrototype<ObjectConstructor>())
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
            return new JsObject(environment, ClassPrototype);
        }

        #endregion

        [JsInstanceMethod("create", Configurable = false)]
        public IJsValue JsCreate(IJsValue[] args)
        {
            var proto = BindParameters.OfTypeOrNull<IJsObject>(args, 0);
            // TODO: Handle properties, https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/Object/create?redirect=no
            return new JsObject(Environment, proto);
        }

        [JsInstanceMethod("defineProperty", Configurable = false)]
        public IJsValue JsDefineProperty(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("defineProperties", Configurable = false)]
        public IJsValue JsDefineProperties(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("getOwnPropertyDescriptor", Configurable = false)]
        public IJsValue JsGetOwnPropertyDescriptor(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("keys", Configurable = false)]
        public IJsValue Keys(IJsValue[] args)
        {
            var obj = args.Select(a => a as IJsObject).FirstOrDefault();

            return Environment.CreateArray(
                from pd in (obj == null ? Enumerable.Empty<IPropertyDescriptor>() : obj.GetOwnProperties())
                where pd.Enumerable
                select Environment.CreateString(pd.Name));
        }

        [JsInstanceMethod("getOwnPropertyNames", Configurable = false)]
        public IJsValue GetOwnPropertyNames(IJsValue[] args)
        {
            var obj = args.Select(a => a as IJsObject).FirstOrDefault();

            return Environment.CreateArray(
                from pd in (obj == null ? Enumerable.Empty<IPropertyDescriptor>() : obj.GetOwnProperties())
                select Environment.CreateString(pd.Name));
        }

        [JsInstanceMethod("getPrototypeOf", Configurable = false)]
        public IJsValue JsGetPrototypeOf(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("preventExtensions", Configurable = false)]
        public IJsValue JsPreventExtensions(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("isExtensible", Configurable = false)]
        public IJsValue JsIsExtensible(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("seal", Configurable = false)]
        public IJsValue JsSeal(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("is", Configurable = false)]
        public IJsValue JsIs(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("isSealed", Configurable = false)]
        public IJsValue JsIsSealed(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("freeze", Configurable = false)]
        public IJsValue JsFreeze(IJsValue[] args)
        {
            throw new JsNotImplemented();
        }

        [JsInstanceMethod("isFrozen", Configurable = false)]
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