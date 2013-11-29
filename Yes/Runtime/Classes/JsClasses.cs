using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Classes
{
    public class JsClasses: IJsClasses
    {
        public IEnvironment Environment { get; set; }
        public IReflectedPropertyDescriptors PropertyDescriptors { get; set; }

        readonly Dictionary<Type, IJsClass> _classesByType = new Dictionary<Type, IJsClass>();
        private readonly Dictionary<Type, IJsObject> _instancesByType = new Dictionary<Type, IJsObject>();

        public JsClasses(IEnvironment environment, IReflectedPropertyDescriptors propertyDescriptors)
        {
            Environment = environment;
            PropertyDescriptors = propertyDescriptors;
        }

        public IJsClass GetClass<T>()
        {
            return GetClass(typeof (T));
        }

        public IJsClass GetClass(Type type)
        {
            if (type == null)
            {
                return null;
            }
            if (!(typeof(IJsObject).IsAssignableFrom(type)))
            {
                return null;
            }
            IJsClass @class;
            if (!_classesByType.TryGetValue(type, out @class))
            {
                var prototypeClass = GetClass(type.BaseType);
                var prototype = prototypeClass == null ? null : GetInstance(type.BaseType, prototypeClass);// new JsObject(Environment, prototypeClass);

                var propertyDescriptors = PropertyDescriptors.GetInstanceDescriptors(type).ToDictionary(kv => kv.Key,
                                                                                                        kv => kv.Value);
                if (prototype != null)
                {
                    propertyDescriptors["prototype"] = new OwnedValuePropertyDescriptor(null, "prototype",prototype,PropertyDescriptorFlags.Writable);
                }
                propertyDescriptors["__impl__"] = new AccessorPropertyDescriptor("__impl__",
                                                                                 self => Environment.CreateString(type.Name),
                                                                                 null);

                @class = new JsClass(propertyDescriptors, prototype);
                _classesByType.Add(type,@class);
            }
            return @class;
        }

        private IJsObject GetInstance(Type type, IJsClass @class)
        {
            IJsObject instance;
            if (!_instancesByType.TryGetValue(type, out instance))
            {
                instance = new JsObject(Environment, @class);
                _instancesByType.Add(type, instance);
            }
            return instance;
        }
    }
}