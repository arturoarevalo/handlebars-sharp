namespace Handlebars.Core.Tags.Builtin
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class WithTag : IBlockTag
    {
        public string Name { get { return @"with"; } }

        public IEnumerable<Context> GetContexts (Context context, ArgumentList arguments)
        {
            var nestedObject = arguments [0].Evaluate (context);

            if (nestedObject != null)
            {
                yield return context.CreateChild (nestedObject);
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