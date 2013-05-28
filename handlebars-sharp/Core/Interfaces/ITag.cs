namespace Handlebars.Core.Interfaces
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;

    public interface ITag
    {
        string Name { get; }

        IEnumerable<Context> GetContexts (Context context, ArgumentList arguments);
    }
}