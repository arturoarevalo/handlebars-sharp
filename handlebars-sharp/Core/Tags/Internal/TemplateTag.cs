namespace Handlebars.Core.Tags.Internal
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class TemplateTag : IBlockTag
    {
        public string Name { get { return @"template"; } }

        public IEnumerable <Context> GetContexts (Context context, ArgumentList arguments)
        {
            yield return context;
        }

        public void BeginRender (Context context)
        {
            // Nothing to do here.
        }

        public void EndRender (Context context)
        {
            // Nothing to do here.
        }
    }
}