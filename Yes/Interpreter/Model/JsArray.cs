using System;
using System.Collections.Generic;
using System.Linq;
using Yes.Runtime;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public class JsArray : JsObject, IJsArray
    {
        private readonly List<IJsValue> _array;

        public JsArray(IEnvironment environment, IJsObject prototype, int length)
            : this(environment, prototype, Enumerable.Range(0,length).Select(_ => JsUndefined.Value))
        {
        }

        public JsArray(IEnvironment environment, IJsObject prototype, IEnumerable<IJsValue> members)
            : this(environment, prototype, members.ToList())
        {
        }
        public JsArray(IEnvironment environment, IJsObject prototype, List<IJsValue> members)
            : base(environment, prototype)
        {
            _array = members;
        }

        private void SetLength(int length)
        {
            if (length < _array.Count)
            {
                _array.RemoveRange(length, _array.Count - length);
            }
            else if (length > _array.Count)
            {
                _array.AddRange(Enumerable.Range(0, length - _array.Count).Select(i => JsUndefined.Value));
            }

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
            // Expand array if nescessary
            if (index >= _array.Count)
            {
                SetLength(index + 1);

            }
            return _array[index] = value;
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

        public override string ToString()
        {
            return string.Join(",", _array);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsArray(environment, Prototype, _array ?? new List<IJsValue>());
        }

        protected IReference GetElementReference(int value)
        {
            return new ArrayElementReference(value,
                                             JsGetElement,
                                             JsSetElement
                );
        }


        [JsInstanceProperty("length")]
        public IJsValue JsLength
        {
            get
            {
                return Environment.CreateNumber(_array.Count);
            }
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
                SetLength(length);
            }
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