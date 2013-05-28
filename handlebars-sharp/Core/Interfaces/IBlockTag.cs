namespace Handlebars.Core.Interfaces
{
    public interface IBlockTag : ITag
    {
        void BeginRender (Context context);
        void EndRender (Context context);
    }
}