namespace Yes.Interpreter.Model
{
    public interface IJsObjectMembers{
        IJsValue GetMember(IJsValue name);
        IJsValue SetMember(string name, IJsValue value);
        IJsValue SetMember(IJsValue name, IJsValue value);
    }
}