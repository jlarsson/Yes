using System.Linq;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class ForIn : IAst
    {
        public IAst Binding { get; set; }
        public IAst Inspected { get; set; }
        public IAst Block { get; set; }
        public bool DeclareBinding { get; set; }

        public ForIn(IAst binding, IAst inspected, IAst block, bool declareBinding)
        {
            Binding = binding;
            Inspected = inspected;
            Block = block;
            DeclareBinding = declareBinding;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var inspected = Inspected.Evaluate(environment) as IJsObject;

            var propertyNames = inspected.GetOwnProperties().Select(pd => pd.Name).ToList();


            var bindingName = ((IAstWithName) Binding).Name;

            var bindingEnvironment = environment;
            if (DeclareBinding)
            {
                bindingEnvironment = new Environment(environment);
                bindingEnvironment.CreateReference(bindingName, JsUndefined.Value);
            }

            foreach (var propertyName in propertyNames)
            {
                bindingEnvironment.GetReference(bindingName).SetValue(null, environment.CreateString(propertyName));

                Block.Evaluate(bindingEnvironment);

                if (bindingEnvironment.ControlFlow.Break)
                {
                    break;
                }
                if (bindingEnvironment.ControlFlow.Return)
                {
                    break;
                }
            }
            return JsUndefined.Value;
        }
    }
}