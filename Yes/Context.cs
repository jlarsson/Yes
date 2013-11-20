using System;
using Yes.Interpreter.Ast;
using Yes.Interpreter.Model;
using Yes.Parsing;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Operators;
using Environment = Yes.Runtime.Environment.Environment;

namespace Yes
{
    public class Context : IContext
    {
        public Context()
        {
            Operators = new Operators();
            Environment = new Environment(this);
            // NOTE: The order of initialization is important since prototypes (with constructor property) is implicitly created

            Classes = new JsClasses(Environment, new ReflectedPropertyDescriptors(Environment));
/*
            ObjectConstructor = CreateContructor<ObjectConstructor, JsObject>((e, c, cc) => new ObjectConstructor(e, c, cc));
            ArrayConstructor = CreateContructor<ArrayConstructor, JsArray>((e, c, cc) => new ArrayConstructor(e, c, cc));
            BooleanConstructor = CreateContructor<BooleanConstructor, JsBool>((e, c, cc) => new BooleanConstructor(e, c, cc));
            FunctionConstructor = CreateContructor<FunctionConstructor, JsFunction>((e, c, cc) => new FunctionConstructor(e, c, cc));
            NumberConstructor = CreateContructor<NumberConstructor, JsNumber>((e, c, cc) => new NumberConstructor(e, c, cc));
            StringConstructor = CreateContructor<StringConstructor, JsString>((e, c, cc) => new StringConstructor(e, c, cc));
*/
            Environment.CreateReference("Array", ArrayConstructor = CreateContructor<ArrayConstructor, JsArray>((e, c, cc) => new ArrayConstructor(e, c, cc)));
            Environment.CreateReference("Boolean", BooleanConstructor = CreateContructor<BooleanConstructor, JsBoolean>((e, c, cc) => new BooleanConstructor(e, c, cc)));
            Environment.CreateReference("Function", FunctionConstructor = CreateContructor<FunctionConstructor, JsFunction>((e, c, cc) => new FunctionConstructor(e, c, cc)));
            Environment.CreateReference("Number", NumberConstructor = CreateContructor<NumberConstructor, JsNumber>((e, c, cc) => new NumberConstructor(e, c, cc)));
            Environment.CreateReference("Object", ObjectConstructor = CreateContructor<ObjectConstructor, JsObject>((e, c, cc) => new ObjectConstructor(e, c, cc)));
            Environment.CreateReference("String", StringConstructor = CreateContructor<StringConstructor, JsString>((e, c, cc) => new StringConstructor(e, c, cc)));
        }

        public IJsClasses Classes { get; set; }
        public IOperators Operators { get; set; }

        TConstructor CreateContructor<TConstructor, TConstructed>(Func<IEnvironment, IJsClass, IJsClass, TConstructor> factory) where TConstructor: JsConstructorFunction where TConstructed : IJsObject
        {
            var constructorClass = Classes.GetClass<TConstructor>();
            var constructedClass = Classes.GetClass<TConstructed>();
            var constructor = factory(Environment, constructorClass, constructedClass);

            var constructedPrototype = constructedClass.Prototype;
            if (constructedPrototype != null)
            {
                if (constructedPrototype.GetOwnProperty("constructor") == null)
                {
                    constructedPrototype.DefineOwnProperty(
                        new ObjectPropertyDescriptor(
                            constructedPrototype,
                            "constructor", 
                            constructor,
                            PropertyDescriptorFlags.Enumerable));
                }
            }

            return constructor;
        }

        #region IContext Members

        public IEnvironment Environment { get; protected set; }

        public IArrayConstructor ArrayConstructor { get; private set; }

        public IBooleanConstructor BooleanConstructor { get; private set; }
        public IFunctionConstructor FunctionConstructor { get; private set; }
        public INumberConstructor NumberConstructor { get; private set; }
        public IObjectConstructor ObjectConstructor { get; private set; }
        public IStringConstructor StringConstructor { get; private set; }

        #endregion

        public IJsValue Execute(string source)
        {
            var ast = new JavascriptParser().Parse(new AstFactory(Operators), source);
            return (ast != null) ? ast.Evaluate(Environment) : JsUndefined.Value;
        }
    }
}