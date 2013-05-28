namespace Handlebars.Core.Tags.Builtin
{
    using System.Collections;
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class EachTag : IBlockTag
    {
        public string Name { get { return @"each"; } }

        public IEnumerable<Context> GetContexts (Context context, ArgumentList arguments)
        {
            var enumeration = arguments [0].Evaluate (context);

            if (enumeration != null && !(enumeration is string))
            {
                if (enumeration is IDictionary)
                {
                    int index = 0;
                    foreach (DictionaryEntry item in (enumeration as IDictionary))
                    {
                        var nestedContext = context.CreateChild (item.Value);

                        nestedContext.SetLocalVariable ("key", item.Key);
                        nestedContext.SetLocalVariable ("index", index);
                        index++;

                        yield return nestedContext;
                    }
                }
                else if (enumeration is IEnumerable)
                {
                    int index = 0;
                    foreach (var item in (enumeration as IEnumerable))
                    {
                        var nestedContext = context.CreateChild (item);

                        nestedContext.SetLocalVariable ("index", index);
                        index++;

                        yield return nestedContext;
                    }
                }
            }
        }

        public void BeginRender (Context context)
        {
            // Nothing to do here.
        }

        public void EndRender (Context context)
        {
            // Nothing to do here.
        }
    }
}