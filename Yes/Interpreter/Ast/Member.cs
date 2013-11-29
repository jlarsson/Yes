using System;
using Yes.Interpreter.Model;
using Yes.Runtime;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class Member : MemberBase
    {
        public Member(IAst instance, IAst member): base(instance, member, CreateGetMemberReference(member))
        {
        }

        private static Func<IEnvironment, IJsValue, IReference> CreateGetMemberReference(IAst name)
        {
            if (name is IAstWithName)
            {
                var literalName = (name as IAstWithName).Name;
                return (env, obj) => obj.GetReference(literalName);
            }
            return (env, obj) => obj.GetReference(name.Evaluate(env));
        }
    }
}