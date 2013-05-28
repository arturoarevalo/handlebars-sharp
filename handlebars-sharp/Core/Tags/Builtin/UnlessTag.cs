namespace Handlebars.Core.Tags.Builtin
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class UnlessTag : IBlockTag
    {
        public string Name { get { return @"unless"; } }

        public IEnumerable<Context> GetContexts (Context context, ArgumentList arguments)
        {
            if (IfTag.EvaluateCondition (context, arguments))
            {
                yield return null;
            }
            else
            {
                yield return context;
            }
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