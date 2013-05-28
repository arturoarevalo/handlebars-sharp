namespace Handlebars.Core.Elements
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class BlockElement : Element <IBlockTag>
    {
        public List <IRenderable> Statements { get; protected set; }
        public ArgumentList Arguments { get; set; }

        public BlockElement (ITag tag) : base (tag)
        {
            Statements = new List <IRenderable> ();
            Arguments = ArgumentList.EmptyArgumentList;
        }

        public override void RenderToContext (Context context)
        {
            InternalTag.BeginRender (context);

            foreach (var innerContext in Tag.GetContexts (context, Arguments)) {
                if (innerContext != null)
                {
                    foreach (var statement in Statements)
                    {
                        statement.RenderToContext (innerContext);
                    }
                }
            }

            InternalTag.EndRender (context);
        }
    }
}