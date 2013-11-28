using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;
using Yes.Runtime.Prototypes;

namespace Yes.Interpreter.Model
{
    public class JsArray : JsArrayProtype
    {
        private readonly List<IJsValue> _array;

        public JsArray(IEnvironment environment, IJsClass @class, int length)
            : this(environment, @class, Enumerable.Range(0, length).Select(_ => JsUndefined.Value))
        {
        }

        public JsArray(IEnvironment environment, IJsClass @class, IEnumerable<IJsValue> members)
            : this(environment, @class, members.ToList())
        {
        }
        public JsArray(IEnvironment environment, IJsClass @class, List<IJsValue> members)
            : base(environment, @class)
        {
            _array = members;
        }


        [JsMember("length", Enumerable = true) /* Republish to make property enumerable in instance instead of just prototype*/]
        public override IJsValue JsLength { get { return base.JsLength; } set { base.JsLength = value; } }

        public override string ToString()
        {
            return string.Join(",", _array);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsArray(environment, Class, _array ?? new List<IJsValue>());
        }

        public override int Length
        {
            get { return _array.Count; }
            set
            {
                if (value < _array.Count)
                {
                    _array.RemoveRange(value, _array.Count - value);
                }
                else if (value > _array.Count)
                {
                    _array.AddRange(Enumerable.Range(0, value - _array.Count).Select(i => JsUndefined.Value));
                }
            }
        }

        public override IJsValue this[int index]
        {
            get
            {
                if ((index < 0) || (index >= _array.Count))
                {
                    throw new JsRangeException();
                }
                return _array[index];
            }
            set
            {
                // Expand array if nescessary
                if (index >= _array.Count)
                {
                    Length = index + 1;
                }
                _array[index] = value;
            }
        }

        public override void Push(IJsValue value)
        {
            _array.Add(value);
        }
    }
}