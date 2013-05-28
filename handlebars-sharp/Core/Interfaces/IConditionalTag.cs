namespace Handlebars.Core.Interfaces
{
    public interface IConditionalTag : IBlockTag
    {
        string ConditionalName { get; }
    }
}