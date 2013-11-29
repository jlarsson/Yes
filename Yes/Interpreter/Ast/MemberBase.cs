using System;
using Yes.Interpreter.Model;
using Yes.Runtime;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public abstract class MemberBase: IAst, ILValue, IEvaluateThisAndMember
    {
        private readonly Func<IEnvironment, IJsValue, IReference> _getMemberReference;

        protected MemberBase(IAst instance, IAst member, Func<IEnvironment, IJsValue, IReference> getMemberReference)
        {
            Instance = instance;
            Member = member;
            _getMemberReference = getMemberReference;
        }

        public IAst Instance { get; protected set; }
        public IAst Member { get; protected set; }

        #region IAst Members

        public IJsValue Evaluate(IEnvironment environment)
        {
            var obj = Instance.Evaluate(environment);
            return _getMemberReference(environment,obj).GetValue(obj);
        }

        #endregion

        public IJsValue SetValue(IEnvironment environment, IJsValue value)
        {
            var obj = Instance.Evaluate(environment);
            return _getMemberReference(environment, obj).SetValue(obj, value);
        }

        public IJsValue Delete(IEnvironment environment)
        {
            var obj = Instance.Evaluate(environment);
            return environment.CreateBool(obj
                                              .Cast<IJsObject>("Cannot convert {0} to object", obj)
                                              .DeleteProperty(Member.Evaluate(environment).ToString()));
        }

        public IJsValue Evaluate(IEnvironment environment, out IJsValue @this)
        {
            var obj = Instance.Evaluate(environment);
            @this = obj;
            return _getMemberReference(environment, obj).GetValue(obj);
        }
    }
}