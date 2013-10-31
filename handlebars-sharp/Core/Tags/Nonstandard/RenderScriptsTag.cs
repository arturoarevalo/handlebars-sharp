namespace Handlebars.Core.Tags.Nonstandard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Optimization;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    class RenderScriptsTag : IInlineTag
    {
        public string Name
        {
            get { return "renderscripts"; }
        }

        public IEnumerable <Context> GetContexts (Context context, ArgumentList arguments)
        {
            yield return context; 
        }

        public string GetData (Context context, ArgumentList arguments)
        {
            return Scripts.Render (arguments [0].ToString ()).ToHtmlString ();
        }
    }
}
