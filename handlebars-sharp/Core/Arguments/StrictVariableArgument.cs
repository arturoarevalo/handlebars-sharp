namespace Handlebars.Core.Arguments
{
    using System;

    public class StrictVariableArgument : VariableArgument
    {
        public string Key { get; protected set; }

        public StrictVariableArgument (string key)
        {
            Key = key;
        }

        public override Object Evaluate (Context context)
        {
            var results = context.Evaluate (Key);

            if (!results.Found)
            {
                // TODO: Handle variable resolution.
            }

            return results.Value;
        }

        public override string ToString ()
        {
            return Key;
        }
    }
}