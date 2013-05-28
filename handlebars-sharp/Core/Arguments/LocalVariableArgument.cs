namespace Handlebars.Core.Arguments
{
    using System;

    public class LocalVariableArgument : VariableArgument
    {
        public string Key { get; protected set; }

        public LocalVariableArgument (string key)
        {
            Key = key;
        }

        public override Object Evaluate (Context context)
        {
            var results = context.EvaluateLocal (Key);

            if (!results.Found)
            {
                // TODO: Handle variable resolution.
            }

            return results.Value;
        }
    }
}