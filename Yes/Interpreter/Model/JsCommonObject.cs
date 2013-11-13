using System.Collections.Generic;

namespace Yes.Interpreter.Model
{
    public interface IPropertyValueHolder
    {
        IJsValue GetValue();
        IJsValue SetValue(IJsValue value);
    }

    public class PropertyValueHolder : IPropertyValueHolder
    {
        public IJsValue Value { get; private set; }
        public IJsValue GetValue()
        {
            return Value;
        }

        public IJsValue SetValue(IJsValue value)
        {
            return Value = value;
        }
    }

    public class JsCommonObject : AbstractJsValue, IJsObject, IJsObjectMembers
    {
        private Dictionary<string, IPropertyValueHolder> _members;

        public JsCommonObject(IScope scope) : base(scope)
        {
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

        public override IJsValue GetMember(IJsValue name)
        {
            var memberName = name.ToString();
            if (_members != null)
            {

                IPropertyValueHolder holder;
                if (_members.TryGetValue(memberName, out holder))
                {
                    return holder.GetValue();
                }

                // Was the protype asked for but not found?
                var memberIsPrototype = "prototype".Equals(memberName);
                if (memberIsPrototype)
                {
                    return Prototype;
                }

                // Do we have a custom prototype
                IPropertyValueHolder customPrototypeHolder;
                if (_members.TryGetValue("prototype", out customPrototypeHolder))
                {
                    return customPrototypeHolder.GetValue().Members.GetMember(name);
                }
            }

            // Look in the default prototype
            var prototype = Prototype;
            return prototype == null ? Scope.CreateUndefined() : prototype.Members.GetMember(name);
        }

        public IJsValue SetMember(string name, IJsValue value)
        {
            if (_members == null)
            {
                _members = new Dictionary<string, IPropertyValueHolder>();
            }
            IPropertyValueHolder holder;
            if (!_members.TryGetValue(name, out holder))
            {
                _members[name] = holder = new PropertyValueHolder();
            }
            return holder.SetValue(value);
        }

        public override IJsValue SetMember(IJsValue name, IJsValue value)
        {
            return SetMember(name.ToString(), value);
        }

        #endregion

        public static IJsValue CreatePrototype(Scope scope)
        {
            var prototype = new JsPrototype(scope);
            return prototype;
        }
    }
}