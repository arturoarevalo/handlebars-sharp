namespace Handlebars.Core.Interfaces
{
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Elements;

    public class InlineElement : Element <IInlineTag>
    {
        public ArgumentList Arguments { get; set; }

        public InlineElement (ITag tag) : base (tag)
        {
            Arguments = ArgumentList.EmptyArgumentList;
        }

        public override void RenderToContext (Context context)
        {
            foreach (var innerContext in Tag.GetContexts (context, Arguments))
            {
                var data = InternalTag.GetData (innerContext, Arguments);
                if (data != null)
                {
                    context.Writer.Write (data);
                }
            }
        }
    }
}