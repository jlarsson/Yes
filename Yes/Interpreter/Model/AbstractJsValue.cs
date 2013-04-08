namespace Yes.Interpreter.Model
{
    public abstract class AbstractJsValue : IJsValue, IJsObjectMembers
    {
        protected AbstractJsValue(IScope scope)
        {
            Scope = scope;
        }

        public IScope Scope { get; protected set; }

        #region IJsValue Members

        public abstract IJsValue Prototype { get; }
        public virtual IJsObjectMembers Members { get { return this; } }
        public abstract JsTypeCode TypeCode { get; }
        public abstract bool IsTruthy();
        public abstract bool IsFalsy();

        #endregion

        public virtual IJsValue GetMember(string memberName)
        {
            if ("prototype".Equals(memberName))
            {
                return Prototype;
            }
            return Prototype.Members.GetMember(memberName);
        }

        public virtual IJsValue SetMember(string name, IJsValue value)
        {
            return Scope.CreateUndefined();
        }
    }
}