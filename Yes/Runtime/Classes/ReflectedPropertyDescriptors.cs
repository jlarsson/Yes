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
                properties = new List<IPropertyDescriptor>();
                properties.AddRange(
                    from prop in
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    let propertyAttributes = prop.GetCustomAttributes(typeof (JsMemberAttribute), false)
                    where propertyAttributes != null
                    let getMethod = prop.GetGetMethod()
                    let setMethod = prop.GetSetMethod()
                    from a in propertyAttributes.OfType<JsMemberAttribute>()
                    select
                        new InstancePropertyDescriptor(a.Name, getMethod, setMethod)
                            {Enumerable = a.Enumerable, Configurable = a.Configurable}
                    );

                properties.AddRange(
                    from method in
                        type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    let methodAttributes = method.GetCustomAttributes(typeof (JsMemberAttribute), false)
                    where methodAttributes != null
                    let hostFunction = CreateHostFunction(method)
                    from a in methodAttributes.OfType<JsMemberAttribute>()
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

        private IJsFunction CreateHostFunction(MethodInfo method)
        {
            Func<IEnvironment, IJsValue, IJsValue[], IJsValue> func =
                (env, @this, args) =>
                    {
                        try
                        {
                            return (method.Invoke(@this, new object[] {args}) as IJsValue) ?? JsUndefined.Value;
                        }
                        catch (TargetInvocationException e)
                        {
                            throw e.InnerException;
                        }
                    };
            return new JsHostFunction(Environment, func);
        }
    }
}