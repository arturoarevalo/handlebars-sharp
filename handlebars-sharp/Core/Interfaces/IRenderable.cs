namespace Handlebars.Core.Interfaces
{
    public interface IRenderable
    {
        ITag Tag { get; }

        void RenderToContext (Context context);
    }
}