namespace Handlebars.Core.Arguments
{
    using System;
    using Handlebars.Core.Interfaces;

    public abstract class VariableArgument : IArgument
    {
        public abstract Object Evaluate (Context context);

        public string ToString (Context context)
        {
            Object data = Evaluate (context);

            if (data != null)
            {
                return data.ToString ();
            }

            return @"NULL";
        }
    }
}