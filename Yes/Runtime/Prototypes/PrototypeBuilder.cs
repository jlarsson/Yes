using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Prototypes
{
    public class PrototypeBuilder
    {
        public IEnumerable<IPropertyDescriptor> CreatePropertyDescriptorsForType<T>(IEnvironment environment, IJsObject prototype) where T : IJsObject
        {
            return 
                CreateMethodPropertyDescriptorsForType(environment, typeof(T), prototype)
                .Union(CreateAccessorPropertyDescriptorsForType(environment, typeof(T)));
        }

        private IEnumerable<IPropertyDescriptor> CreateAccessorPropertyDescriptorsForType(IEnvironment environment, Type type)
        {
            return
                from prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                let propertyAttributes = prop.GetCustomAttributes(typeof (JsInstancePropertyAttribute), false)
                where propertyAttributes != null
                let getMethod = prop.GetGetMethod()
                let setMethod = prop.GetSetMethod()

                from a in propertyAttributes.OfType<JsInstancePropertyAttribute>()
                select new InstancePropertyDescriptor(a.Name, getMethod, setMethod){Enumerable = a.Enumerable, Configurable = a.Configurable};
        }

        private IEnumerable<IPropertyDescriptor> CreateMethodPropertyDescriptorsForType(IEnvironment environment, Type type, IJsObject prototype)
        {
            return
                from method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                let methodAttributes = method.GetCustomAttributes(typeof (JsInstanceMethodAttribute), false)
                where methodAttributes != null

                let hostFunction = CreateHostFunction(environment, method)

                from a in methodAttributes.OfType<JsInstanceMethodAttribute>()
                select new ObjectPropertyDescriptor(prototype,a.Name, hostFunction, a.GetFlags());
        }

        private IJsFunction CreateHostFunction(IEnvironment environment, MethodInfo method)
        {
            Func<IEnvironment, IJsValue, IJsValue[], IJsValue> func = (env, @this, args) => (method.Invoke(@this, args) as IJsValue) ?? JsUndefined.Instance;
            return new JsHostFunction(environment,func);
        }
    }
}
