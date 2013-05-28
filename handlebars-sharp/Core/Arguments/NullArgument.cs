namespace Handlebars.Core.Arguments
{
    using System;
    using Handlebars.Core.Interfaces;

    public class NullArgument : IArgument
    {
        public Object Evaluate (Context context)
        {
            return null;
        }

        public string ToString (Context context)
        {
            return @"NULL";
        }
    }
}