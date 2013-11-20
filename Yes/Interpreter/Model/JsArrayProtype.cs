using System;
using System.Linq;
using Yes.Runtime;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public abstract class JsArrayProtype : JsObjectPrototype, IJsArray
    {
        protected JsArrayProtype(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }

        public override IReference GetReference(IJsValue name)
        {
            var index = name.ToArrayIndex();
            if (index.HasValue && (index > 0))
            {
                return GetElementReference(index.Value);
            }
            return base.GetReference(name.ToString());
        }

        public override IReference GetReference(string name)
        {
            int index;
            if (int.TryParse(name, out index))
            {
                return GetElementReference(index);
            }
            return base.GetReference(name);
        }

        [JsMember("length", Enumerable = true)]
        public virtual IJsValue JsLength
        {
            get { return Environment.CreateNumber(Length); }
            set
            {
                var index = value.ToArrayIndex();
                if (!index.HasValue)
                {
                    throw new JsTypeError();
                }
                var length = index.Value;
                if (length < 0)
                {
                    throw new JsArgumentError();
                }
                Length = length;
            }
        }

        [JsMember("push", Enumerable = true)]
        public IJsValue JsPush(IJsValue[] argument)
        {
            Push(argument.FirstOrDefault() ?? JsUndefined.Value);
            return this;
        }

        public abstract int Length { get; set; }
        public abstract IJsValue this[int index] { get; set; }
        public abstract void Push(IJsValue value);

        protected IReference GetElementReference(int value)
        {
            return new ArrayElementReference(value, i => this[i], (i, v) => this[i] = v);
        }

        public class ArrayElementReference : IReference
        {
            private readonly int _index;
            private readonly Func<int, IJsValue> _getter;
            private readonly Func<int, IJsValue, IJsValue> _setter;

            public ArrayElementReference(int index, Func<int, IJsValue> getter, Func<int, IJsValue, IJsValue> setter)
            {
                _index = index;
                _getter = getter;
                _setter = setter;
            }

            public IJsValue GetValue(IJsValue self)
            {
                return _getter(_index);
            }

            public IJsValue SetValue(IJsValue self, IJsValue value)
            {
                return _setter(_index, value);
            }
        }
    }
}