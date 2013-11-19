namespace Yes.Runtime.Prototypes
{
    public class JsInstanceMember: AbstractJsPropertyAttribute
    {
        public JsInstanceMember(string name): base(name)
        {
        }

        public override bool IsPrototypeMember
        {
            get { return false; }
        }
    }
}