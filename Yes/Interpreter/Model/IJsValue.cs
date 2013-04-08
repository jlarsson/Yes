namespace Yes.Interpreter.Model
{
    public interface IJsValue
    {
        IJsObjectMembers Members { get; }
        JsTypeCode TypeCode { get; }
        bool IsTruthy();
        bool IsFalsy();
    }
}