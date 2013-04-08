namespace Yes.Interpreter.Model
{
    public interface IJsObjectMembers{
        IJsValue GetMember(string memberName);
        IJsValue SetMember(string name, IJsValue value);
    }
}