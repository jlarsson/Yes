namespace Yes.Interpreter.Ast
{
    public class IndexedMember : MemberBase
    {
        public IndexedMember(IAst instance, IAst member)
            : base(instance, member, (env, obj) => obj.GetReference(member.Evaluate(env)))
        {
        }
    }
}