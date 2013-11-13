using System.Collections.Generic;
using System.Linq;
using Yes.Interpreter.Ast;

namespace Yes.Interpreter.Model
{
    public class JsArrayProtoype: JsCommonObject
    {
        public JsArrayProtoype(IScope scope) : base(scope)
        {
        }
    }

    public class JsArray: JsCommonObject, IJsArray
    {
        private readonly List<IJsValue> _array;

        public JsArray(IScope scope, IEnumerable<IAst> members) : base(scope)
        {
            _array = (from m in members select m.Evaluate(scope)).ToList();
        }

        public override string ToString()
        {
            return string.Join(",", _array);
        }

        public override IJsValue GetMember(IJsValue name)
        {
            var index = name.TryEvaluateToIndex();
            if (index != null)
            {
                var i = index.Value;
                if ((i < 0) || (i >= _array.Count))
                {
                    return Scope.Throw("Array index is out of bounds");
                }
                return _array[i];
            }
            return base.GetMember(name);
        }

        public override IJsValue SetMember(IJsValue name, IJsValue value)
        {
            var index = name.TryEvaluateToIndex();
            if (index != null)
            {
                return SetArrayMember(index.Value, value);
            }
            return base.SetMember(name, value);
        }

        private IJsValue SetArrayMember(int index, IJsValue value)
        {
            if (index < 0)
            {
                return Scope.Throw("Array index is out of bounds");
            }
            if (index < _array.Count)
            {
                _array[index] = value;
                return value;
            }
            if (index == _array.Count)
            {
                _array.Add(value);
                return value;
            }
            return Scope.Throw("Array index is out of bounds");
        }
    }
}