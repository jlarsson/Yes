namespace Yes.Interpreter.Model
{
    public class JsPrototype : JsCommonObject
    {
        public JsPrototype(IScope scope): base(scope)
        {
        }

        public override IJsValue Prototype
        {
            //get { return Scope.CreateNull(); }
            get { return null; }
        }
    }
}