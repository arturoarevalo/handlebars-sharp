namespace Handlebars.Core.Interfaces
{
    using System;

    public interface IArgument
    {
        Object Evaluate (Context context);
        string ToString (Context context);
    }
}