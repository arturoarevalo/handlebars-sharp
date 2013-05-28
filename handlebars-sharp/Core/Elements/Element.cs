namespace Handlebars.Core.Elements
{
    using Handlebars.Core.Interfaces;

    public abstract class Element <T> : IRenderable where T : ITag
    {
        public ITag Tag { get { return InternalTag; } }
        public T InternalTag { get; protected set; }

        public Element (ITag tag)
        {
            InternalTag = (T) tag;
        }

        public abstract void RenderToContext (Context context);
    }
}