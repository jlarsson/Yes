using System;
using System.Collections.Generic;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Parsing;
using Yes.Runtime;
using Yes.Runtime.Environment;
using Yes.Runtime.Operators;
using Yes.Runtime.Prototypes;
using Environment = Yes.Runtime.Environment.Environment;

namespace Yes
{
    public class Context : IContext
    {
        private readonly Dictionary<Tuple<Type,IJsConstructor>, IJsObject> _type2protype = new Dictionary<Tuple<Type, IJsConstructor>, IJsObject>();

        public Context()
        {
            Operators = new Operators();
            Environment = new Environment(this);
            ArrayConstructor = new ArrayConstructor(Environment);
            BooleanConstructor = new BooleanConstructor(Environment);
            FunctionConstructor = new FunctionConstructor(Environment);
            NumberConstructor = new NumberConstructor(Environment);
            ObjectConstructor = new ObjectConstructor(Environment);
            StringConstructor = new StringConstructor(Environment);

            Environment.CreateReference("Array", ArrayConstructor);
            Environment.CreateReference("Boolean", BooleanConstructor);
            Environment.CreateReference("Function", FunctionConstructor);
            Environment.CreateReference("Number", NumberConstructor);
            Environment.CreateReference("Object", ObjectConstructor);
            Environment.CreateReference("String", StringConstructor);
        }

        public IOperators Operators { get; set; }

        #region IContext Members

        public IEnvironment Environment { get; protected set; }

        public IArrayConstructor ArrayConstructor { get; private set; }

        public IBooleanConstructor BooleanConstructor { get; private set; }
        public IFunctionConstructor FunctionConstructor { get; private set; }
        public INumberConstructor NumberConstructor { get; private set; }
        public IObjectConstructor ObjectConstructor { get; private set; }
        public IStringConstructor StringConstructor { get; private set; }


        public IJsObject GetPrototype<T>(IJsConstructor constructor = null) where T : IJsObject
        {
            return GetPrototype(typeof (T), constructor);
        }

        #endregion

        public IJsValue Execute(string source)
        {
            var ast = new JavascriptParser().Parse(new AstFactory(Operators), source);
            return (ast != null) ? ast.Evaluate(Environment) : JsUndefined.Value;
        }

        public IJsObject GetPrototype(Type type, IJsConstructor constructor = null)
        {
            if (!typeof (IJsObject).IsAssignableFrom(type))
            {
                return null;
            }
            IJsObject prototype;
            var key = Tuple.Create(type, constructor);
            if (!_type2protype.TryGetValue(key, out prototype))
            {
                prototype = new JsObject(Environment, GetPrototype(type.BaseType));
                foreach (var pd in new PrototypeBuilder().CreatePropertyDescriptorsForType(type, Environment, prototype))
                {
                    prototype.DefineOwnProperty(pd);
                }
                if ((constructor != null) && (null == prototype.GetOwnProperty("constructor")))
                {
                    prototype.DefineOwnProperty(new ObjectPropertyDescriptor(prototype, "constructor", constructor,
                                                                             PropertyDescriptorFlags.None));
                }

                _type2protype.Add(key, prototype);
            }
            return prototype;
        }
    }
}