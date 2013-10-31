namespace Handlebars.Core.Arguments
{
    using System;
    using System.Globalization;
    using Handlebars.Core.Interfaces;

    public class IntegerArgument : IArgument
    {
        public int Data { get; protected set; }

        public IntegerArgument (int data)
        {
            Data = data;
        }

        public Object Evaluate (Context context)
        {
            return Data;
        }

        public string ToString (Context context)
        {
            return Data.ToString (CultureInfo.InvariantCulture);
        }

        public override string ToString ()
        {
            return Data.ToString (CultureInfo.InvariantCulture);
        }
    }
}