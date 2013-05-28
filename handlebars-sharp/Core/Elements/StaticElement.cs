namespace Handlebars.Core.Elements
{
    using Handlebars.Core.Interfaces;

    public class StaticElement : InlineElement
    {
        public string Data { get; protected set; }

        public StaticElement (string data, ITag tag) : base (tag)
        {
            Data = data;
        }

        public override void RenderToContext (Context context)
        {
            context.Writer.Write (Data);	
        }
    }
}