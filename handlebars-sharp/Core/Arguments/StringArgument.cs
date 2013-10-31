namespace Handlebars.Core.Arguments
{
    using System;
    using System.Globalization;
    using Handlebars.Core.Interfaces;

    public class StringArgument : IArgument
    {
        public string Data { get; protected set; }

        public StringArgument (string data)
        {
            Data = data;
        }

        public Object Evaluate (Context context)
        {
            return Data;
        }

        public string ToString (Context context)
        {
            return Data;
        }

        public override string ToString ()
        {
            return Data;
        }
    }
}