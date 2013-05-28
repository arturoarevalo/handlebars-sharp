namespace Handlebars.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Handlebars.Core.Elements;
    using Handlebars.Core.Interfaces;

    public class Template : BlockElement
    {
        public Dictionary <string, BlockElement> Partials { get; protected set; }

        internal Template (ITag tag)
            : base (tag)
        {
            // Templates can only be created from the compiler.
            Partials = new Dictionary <string, BlockElement> ();
        }

        public string Render (Object source, Func <string, IRenderable> resolver = null)
        {
            var context = new Context (source, new StringWriter ());

            context.PartialResolver = resolver;

            RenderToContext (context);

            return context.Writer.ToString ();
        }

        public override void RenderToContext (Context context)
        {
            // Clone context ...
            var previousPartialResolver = context.PartialResolver;

            try
            {
                context.PartialResolver = name =>
                                          {
                                              BlockElement partial;
                                              if (Partials.TryGetValue (name, out partial))
                                              {
                                                  return partial;
                                              }

                                              if (previousPartialResolver != null)
                                              {
                                                  return previousPartialResolver (name);
                                              }

                                              return null;
                                          };

                base.RenderToContext (context);
            }
            finally
            {
                // Restore partial resolver.
                context.PartialResolver = previousPartialResolver;
            }
        }
    }
}