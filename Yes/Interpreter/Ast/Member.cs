using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Member : IAst, ILValue, IEvaluateThisAndMember
    {
        public Member(IAst instance, IAst name)
        {
            Instance = instance;
            MemberName = name;
        }

        public IAst Instance { get; protected set; }
        public IAst MemberName { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IScope scope)
        {
            var obj = Instance.Evaluate(scope);
            return obj.Members.GetMember(MemberName.Evaluate(scope));
        }

        #endregion

        public IJsValue SetValue(IScope scope, IJsValue value)
        {
            return Instance.Evaluate(scope).Members.SetMember(MemberName.Evaluate(scope), value);
        }

        public IJsValue Evaluate(IScope scope, out IJsValue @this)
        {
            var obj = Instance.Evaluate(scope);
            @this = obj;
            return obj.Members.GetMember(MemberName.Evaluate(scope));
        }
    }
}