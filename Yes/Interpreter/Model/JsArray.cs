using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Runtime;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public class JsArray: JsObject, IJsArray
    {
        private readonly List<IJsValue> _array;

        public JsArray(IEnvironment environment, IJsObject prototype, IEnumerable<IJsValue> members) : base(environment, prototype)
        {
            DefineOwnProperty(new AccessorPropertyDescriptor("length",
                                                             JsGetLength,
                                                             JsSetLength));

            _array = members.ToList();
        }

        protected IJsValue JsGetLength()
        {
            return Environment.CreateNumber(_array.Count);
        }

        protected IJsValue JsSetLength(IJsValue length)
        {
            var index = length.ToArrayIndex();
            if (!index.HasValue)
            {
                throw new JsTypeError();
            }
            var l = index.Value;
            if (l < 0)
            {
                throw new JsArgumentError();
            }
            if (l < _array.Count)
            {
                _array.RemoveRange(l, _array.Count-l);
            }
            else if (l > _array.Count)
            {
                _array.AddRange(Enumerable.Range(0, l - _array.Count).Select(i => JsUndefined.Instance));
            }
            return length;
        }
        protected IJsValue JsGetElement(int index)
        {
            if ((index < 0) || (index >= _array.Count))
            {
                throw new JsRangeError();
            }
            return _array[index];
        }
        protected IJsValue JsSetElement(int index, IJsValue value)
        {
            if (index < 0)
            {
                throw new JsRangeError();
            }
            if (index < _array.Count)
            {
                return _array[index] = value;
            }
            if (index == _array.Count)
            {
                _array.Add(value);
                return value;
            }
            throw new JsRangeError();
        }


        public override IReference GetReference(IJsValue name)
        {
            var index = name.ToArrayIndex();
            if (index.HasValue)
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

        public override string ToString()
        {
            return string.Join(",", _array);
        }

        protected IReference GetElementReference(int value)
        {
            return new ArrayElementReference(value,
                JsGetElement,
                JsSetElement
                );
        }


        [JsInstanceMethod("push")]
        public IJsValue JsPush(IJsValue argument)
        {
            _array.Add(argument);
            return this;
        }

        public class ArrayElementReference : IReference
        {
            private readonly int _index;
            private readonly Func<int, IJsValue> _getter;
            private readonly Func<int, IJsValue, IJsValue> _setter;

            public ArrayElementReference(int index, Func<int,IJsValue> getter, Func<int,IJsValue,IJsValue> setter)
            {
                _index = index;
                _getter = getter;
                _setter = setter;
            }

            public IJsValue GetValue()
            {
                return _getter(_index);
            }

            public IJsValue SetValue(IJsValue value)
            {
                return _setter(_index,value);
            }
        }
    }
}