namespace Handlebars.Core.Tags.Nonstandard
{
    using System.Collections.Generic;
    using System.Web.Optimization;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    class RenderStylesTag : IInlineTag
    {
        public string Name
        {
            get { return "renderstyles"; }
        }

        public IEnumerable<Context> GetContexts (Context context, ArgumentList arguments)
        {
            yield return context;
        }

        public string GetData (Context context, ArgumentList arguments)
        {
            return Styles.Render (arguments[0].ToString ()).ToHtmlString ();
        }
    }
}