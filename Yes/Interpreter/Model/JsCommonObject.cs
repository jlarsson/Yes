using System;
using System.Collections.Generic;

namespace Yes.Interpreter.Model
{
    public class JsCommonObject : AbstractJsValue, IJsObject, IJsObjectMembers
    {
        private Dictionary<string, IJsValue> _members;

        public JsCommonObject(IScope scope) : base(scope)
        {
        }

        public JsCommonObject(IScope scope, IEnumerable<Tuple<string, IJsValue>> members) : this(scope)
        {
            EnsureMembers();
            foreach (var member in members)
            {
                _members[member.Item1] = member.Item2;
            }
        }

        public override IJsValue Prototype
        {
            get { return Scope.ProtoTypes.Object; }
        }

        #region IJsObject Members

        public override IJsObjectMembers Members
        {
            get { return this; }
        }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Object; }
        }

        public override bool IsTruthy()
        {
            return true;
        }

        public override bool IsFalsy()
        {
            return false;
        }

        #endregion

        #region IJsObjectMembers Members

        public override IJsValue GetMember(string memberName)
        {
            if (_members != null)
            {
                IJsValue value;
                if (_members.TryGetValue(memberName, out value))
                {
                    return value;
                }
            }
            if ("prototype".Equals(memberName))
            {
                return Prototype;
            }
            var prototype = GetEffectiveProtoType();
            return (prototype is IJsNull) ? Scope.CreateUndefined() : prototype.Members.GetMember(memberName);
        }

        public override IJsValue SetMember(string name, IJsValue value)
        {
            EnsureMembers();
            _members[name] = value;
            return value;
        }

        #endregion

        private IJsValue GetEffectiveProtoType()
        {
            IJsValue prototype;
            if ((_members != null) && _members.TryGetValue("prototype", out prototype))
            {
                return prototype;
            }

            return Prototype ?? Scope.CreateNull();
        }

        private void EnsureMembers()
        {
            if (_members == null)
            {
                _members = new Dictionary<string, IJsValue>
                               {
                                   {"prototype", Prototype}
                               };
            }
        }

        public static IJsValue CreatePrototype(Scope scope)
        {
            var prototype = new JsPrototype(scope);
            return prototype;
        }
    }
}