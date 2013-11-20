using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

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

        public IJsValue Evaluate(IEnvironment environment)
        {
            var obj = Instance.Evaluate(environment);
            return obj.GetReference(MemberName.Evaluate(environment)).GetValue(obj);
        }

        #endregion

        public IJsValue SetValue(IEnvironment environment, IJsValue value)
        {
            var obj = Instance.Evaluate(environment);
            return obj.GetReference(MemberName.Evaluate(environment)).SetValue(obj, value);
        }

        public IJsValue Delete(IEnvironment environment)
        {
            var obj = Instance.Evaluate(environment);
            return environment.CreateBool(obj
                                              .Cast<IJsObject>("Cannot convert {0} to object", obj)
                                              .DeleteProperty(MemberName.Evaluate(environment).ToString()));
        }

        public IJsValue Evaluate(IEnvironment environment, out IJsValue @this)
        {
            var obj = Instance.Evaluate(environment);
            @this = obj;
            return obj.GetReference(MemberName.Evaluate(environment)).GetValue(obj);
        }
    }
}