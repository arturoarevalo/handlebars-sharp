namespace Handlebars.Core.Arguments
{
    using System;

    public class ThisArgument : VariableArgument
    {
        public override Object Evaluate (Context context)
        {
            return context.Source;
        }
    }
}