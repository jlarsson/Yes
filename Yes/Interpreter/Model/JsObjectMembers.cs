using System.Collections.Generic;

namespace Yes.Interpreter.Model
{
    public class JsObjectMembers : IJsObjectMembers
    {
        private Dictionary<string, IJsValue> _members;

        public JsObjectMembers(IScope scope)
        {
            Scope = scope;
        }

        public IScope Scope { get; protected set; }

        #region IJsObjectMembers Members

        public IJsValue GetMember(string memberName)
        {
            if (_members != null)
            {
                IJsValue value;
                if (_members.TryGetValue(memberName, out value))
                {
                    return value;
                }
            }
            return Scope.CreateUndefined();
        }

        public IJsValue SetMember(string name, IJsValue value)
        {
            if (_members == null)
            {
                _members = new Dictionary<string, IJsValue>();
            }
            _members[name] = value;
            return value;
        }

        #endregion
    }
}