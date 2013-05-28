namespace Handlebars.Core.Arguments
{
    using System;

    public class RecursiveVariableArgument : StrictVariableArgument
    {
        public RecursiveVariableArgument (string key) : base (key)
        {
        }

        public override Object Evaluate (Context context)
        {
            var results = context.EvaluateRecursive (Key);

            if (!results.Found)
            {
                // TODO: Handle variable resolution.
            }

            return results.Value;
        }
    }
}