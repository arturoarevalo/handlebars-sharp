namespace Handlebars.Core.Interfaces
{
    using Handlebars.Core.Arguments;

    public interface IInlineTag : ITag
    {
        string GetData (Context context, ArgumentList arguments);
    }
}