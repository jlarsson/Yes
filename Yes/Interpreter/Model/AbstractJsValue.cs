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
        public virtual int? TryEvaluateToIndex()
        {
            return null;
        }

        #endregion

        public virtual IJsValue GetMember(IJsValue name)
        {
            if ("prototype".Equals(name))
            {
                return Prototype;
            }
            return Prototype.Members.GetMember(name);
        }

        public virtual IJsValue SetMember(string name, IJsValue value)
        {
            return Scope.Throw("Can't assign to member {0} of {1}", name, this);
        }

        public virtual IJsValue SetMember(IJsValue name, IJsValue value)
        {
            return Scope.Throw("Can't assign to member {0} of {1}", name, this);
        }
    }
}