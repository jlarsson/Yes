using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;

namespace Yes
{
    public class JsClass : IJsClass
    {
        public static readonly IJsClass Default = new JsClass();

        public JsClass()
        {
            InstanceProperties = new Dictionary<string, IPropertyDescriptor>();
        }

        public static IJsClass Create(IEnvironment environment, Type type, IJsConstructor constructor)
        {
            var @class = new JsClass
                             {
                                 InstanceProperties = new Dictionary<string, IPropertyDescriptor>(),
                                 Prototype = new JsObject(environment, new JsClass())
                             };

            if ((constructor != null))
            {
                @class.Prototype.DefineOwnProperty(new ObjectPropertyDescriptor(
                                                       @class.Prototype, "constructor", constructor,
                                                       PropertyDescriptorFlags.Enumerable));
            }


            var properties =
                from prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                let propertyAttributes = prop.GetCustomAttributes(typeof (AbstractJsPropertyAttribute), false)
                where propertyAttributes != null
                let getMethod = prop.GetGetMethod()
                let setMethod = prop.GetSetMethod()
                from a in propertyAttributes.OfType<AbstractJsPropertyAttribute>()
                select new
                           {
                               a.IsPrototypeMember,
                               PropertyDescriptor =
                    new InstancePropertyDescriptor(a.Name, getMethod, setMethod)
                        {Enumerable = a.Enumerable, Configurable = a.Configurable}
                           };
            foreach (var property in properties)
            {
                if (property.IsPrototypeMember)
                {
                    @class.Prototype.DefineOwnProperty(property.PropertyDescriptor);
                }
                else
                {
                    @class.InstanceProperties[property.PropertyDescriptor.Name] = property.PropertyDescriptor;
                }
            }


            var methods =
                from method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                let methodAttributes = method.GetCustomAttributes(typeof (AbstractJsPropertyAttribute), false)
                where methodAttributes != null
                let hostFunction = CreateHostFunction(environment, method)
                from a in methodAttributes.OfType<AbstractJsPropertyAttribute>()
                select new
                           {
                               a.IsPrototypeMember,
                               a.Name,
                               Flags = a.GetFlags(),
                               Function = hostFunction
                           };

            foreach (var method in methods)
            {
                if (method.IsPrototypeMember)
                {
                    @class.Prototype.DefineOwnProperty(new ObjectPropertyDescriptor(@class.Prototype, method.Name,
                                                                                    method.Function,
                                                                                    method.Flags));
                }
                else
                {
                    // TODO: Change implementation
                    var pd = new ObjectPropertyDescriptor(null, method.Name, method.Function,
                                                          method.Flags);
                    @class.InstanceProperties[pd.Name] = pd;
                }
            }
            return @class;
        }

        private static IJsFunction CreateHostFunction(IEnvironment environment, MethodInfo method)
        {
            Func<IEnvironment, IJsValue, IJsValue[], IJsValue> func =
                (env, @this, args) => (method.Invoke(@this, new object[] {args}) as IJsValue) ?? JsUndefined.Value;
            return new JsHostFunction(environment, func);
        }

        public Dictionary<string, IPropertyDescriptor> InstanceProperties { get; protected set; }
        public IJsObject Prototype { get; set; }

        public IPropertyDescriptor GetInstanceProperty(string name)
        {
            IPropertyDescriptor pd;
            return InstanceProperties.TryGetValue(name, out pd) ? pd : null;
        }

        public IEnumerable<IPropertyDescriptor> GetInstanceProperties()
        {
            return InstanceProperties.Values;
        }
    }
}