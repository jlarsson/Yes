using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;

namespace Yes.Runtime.Classes
{
    public class ReflectedPropertyDescriptors : IReflectedPropertyDescriptors
    {
        public interface IMethodHelper
        {
            Func<IJsValue, IJsValue> CreateGetter(IEnvironment environment, MethodInfo method);
            Func<IJsValue, IJsValue, IJsValue> CreateSetter(IEnvironment environment, MethodInfo method);
            Func<IJsValue, IList<IJsValue>, IJsValue> CreateFunction(IEnvironment environment, MethodInfo method);
        }
        public class MethodHelper<T>: IMethodHelper where T : class
        {
            public Func<IJsValue,IJsValue> CreateGetter(IEnvironment environment, MethodInfo method)
            {
                var methodDelegate = (Func<T, IJsValue>)Delegate.CreateDelegate(typeof(Func<T, IJsValue>), method);
                return @this => methodDelegate(@this.Cast<T>()) ?? JsUndefined.Value;
            }
            public Func<IJsValue, IJsValue, IJsValue> CreateSetter(IEnvironment environment, MethodInfo method)
            {
                var methodDelegate = (Action<T, IJsValue>)Delegate.CreateDelegate(typeof(Action<T, IJsValue>), method);
                return (@this,value) =>
                           {
                               methodDelegate(@this.Cast<T>(),value);
                               return JsUndefined.Value;
                           };
            }

            public Func<IJsValue, IList<IJsValue>, IJsValue> CreateFunction(IEnvironment environment, MethodInfo method)
            {
                var methodDelegate = (Func<T, IList<IJsValue>, IJsValue>)Delegate.CreateDelegate(typeof(Func<T, IList<IJsValue>, IJsValue>), method);
                return (@this, args) => methodDelegate(@this.Cast<T>(), args) ?? JsUndefined.Value;
            }
        }



        public IEnvironment Environment { get; set; }
        readonly Dictionary<Type,List<IPropertyDescriptor>> _propertiesByType = new Dictionary<Type,List<IPropertyDescriptor>>();
        readonly Dictionary<Type,Dictionary<string, IPropertyDescriptor>> _instanceDescriptorsByType = new Dictionary<Type, Dictionary<string, IPropertyDescriptor>>();

        public class Property
        {
            public bool IsPrototypeMember { get; set; }

            public IPropertyDescriptor PropertyDescriptor { get; set; }
        }

        public ReflectedPropertyDescriptors(IEnvironment environment)
        {
            Environment = environment;
        }

        IEnumerable<IPropertyDescriptor> GetPropertiesByType(Type type)
        {
            List<IPropertyDescriptor> properties;
            if (!_propertiesByType.TryGetValue(type, out properties))
            {
                var methodHelper = typeof (MethodHelper<>).MakeGenericType(type)
                                                 .GetConstructor(Type.EmptyTypes)
                                                 .Invoke(new object[0]) as IMethodHelper;

                properties = new List<IPropertyDescriptor>();
                properties.AddRange(
                    from prop in
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    let propertyAttributes = prop.GetCustomAttributes(typeof (JsMemberAttribute), false)
                    where propertyAttributes != null
                    let getMethod = prop.GetGetMethod()
                    let setMethod = prop.GetSetMethod()
                    from a in propertyAttributes.OfType<JsMemberAttribute>()

                    let getter = getMethod == null ? null : methodHelper.CreateGetter(Environment, getMethod)
                    let setter = setMethod == null ? null : methodHelper.CreateSetter(Environment, setMethod)

                    select new AccessorPropertyDescriptor(a.Name,getter,setter)
                            {Enumerable = a.Enumerable, Configurable = a.Configurable}
                    );

                properties.AddRange(
                    from method in
                        type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    let methodAttributes = method.GetCustomAttributes(typeof (JsMemberAttribute), false)
                    where methodAttributes != null
                    from a in methodAttributes.OfType<JsMemberAttribute>()
                    let function = methodHelper.CreateFunction(Environment, method)
                    let hostFunction = new JsHostFunction(Environment,(env,@this,args) => function(@this,args))
                    select new ObjectPropertyDescriptor(null, a.Name,
                                                        hostFunction,
                                                        a.GetFlags())
                    );
                _propertiesByType.Add(type, properties);
            }
            return properties;
        }

        public Dictionary<string, IPropertyDescriptor> GetInstanceDescriptors(Type type)
        {
            Dictionary<string, IPropertyDescriptor> descriptors;
            if (!_instanceDescriptorsByType.TryGetValue(type, out descriptors))
            {
                descriptors = new Dictionary<string, IPropertyDescriptor>();
                foreach (var pd in GetPropertiesByType(type))
                {
                    descriptors[pd.Name] = pd;
                }
                _instanceDescriptorsByType.Add(type, descriptors);
            }
            return descriptors;
        }
    }
}