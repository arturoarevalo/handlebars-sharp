namespace Handlebars.Core.Tags.Internal
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class RenderPartialTag : IInlineTag
    {
        public string Name
        {
            get
            {
                return @"renderpartial";
            }
        }

        public IEnumerable<Context> GetContexts (Context context, ArgumentList arguments)
        {
            int count = arguments.Count;

            if (count == 1)
            {
                yield return context;
            }
            else
            {
                if (count == 2 && !arguments.IsNamed (1))
                {
                    // Called with a new context.
                    yield return context.CreateChild (arguments [1].Evaluate (context));
                }
                else
                {
                    // Called with 2 or more named parameters.
                    var nestedContext = context.Clone ();

                    for (int i = 1; i < count; i++)
                    {
                        var name = arguments.GetName (i);
                        nestedContext.SetLocalVariable (name, arguments [i].Evaluate (context));
                    }

                    yield return nestedContext;
                }
            }
        }

        public string GetData (Context context, ArgumentList arguments)
        {
            var renderable = context.ResolvePartial (arguments [0].Evaluate (context).ToString ());

            if (renderable != null)
            {
                renderable.RenderToContext (context);
            }

            return null;
        }
    }
}