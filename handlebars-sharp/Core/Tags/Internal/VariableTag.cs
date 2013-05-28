namespace Handlebars.Core.Tags.Internal
{
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class VariableTag : IInlineTag
    {
        public string Name
        {
            get
            {
                return @"variable";
            }
        }

        public IEnumerable <Context> GetContexts (Context context, ArgumentList arguments)
        {
            yield break;
        }

        public string GetData (Context context, ArgumentList arguments)
        {
            return arguments [0].ToString (context);
        }
    }
}