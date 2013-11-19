namespace Yes.Runtime.Prototypes
{
    public class JsPrototypeMember: AbstractJsPropertyAttribute
    {
        public JsPrototypeMember(string name) : base(name)
        {
        }
        public override bool IsPrototypeMember
        {
            get { return true; }
        }
    }
}