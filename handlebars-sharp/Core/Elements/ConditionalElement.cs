namespace Handlebars.Core.Elements
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class ConditionalElement : Element <IConditionalTag>
    {
        public List <IRenderable> Statements { get; protected set; }
        public List <IRenderable> ConditionalStatements { get; protected set; }
        public ArgumentList Arguments { get; set; }

        public ConditionalElement (ITag tag) : base (tag)
        {
            Statements = new List<IRenderable> ();
            ConditionalStatements = new List<IRenderable> ();
            Arguments = ArgumentList.EmptyArgumentList;
        }

        public override void RenderToContext (Context context)
        {
            InternalTag.BeginRender (context);

            int index = 0;
            foreach (var innerContext in Tag.GetContexts (context, Arguments))
            {
                if (index == 0 && innerContext != null)
                {
                    foreach (var statement in Statements)
                    {
                        statement.RenderToContext (innerContext);
                    }
                }
                else if (index == 1 && innerContext != null)
                {
                    foreach (var statement in ConditionalStatements)
                    {
                        statement.RenderToContext (innerContext);
                    }
                }

                index++;
            }

            InternalTag.EndRender (context);
        }
    }
}