namespace Handlebars.Core.Tags.Builtin
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Interfaces;

    public class IfTag : IConditionalTag
    {
        public string Name { get { return @"if"; } }
        public string ConditionalName { get { return @"else"; } }

        public static bool EvaluateCondition (Context context, ArgumentList arguments)
        {
            Object value = arguments[0].Evaluate (context);

            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                return (bool) value;
            }

            if (value is IEnumerable)
            {
                return (value as IEnumerable).Cast <Object> ().Any ();
            }

            var str = value.ToString ();
            return !(str.Equals (@"false", StringComparison.InvariantCultureIgnoreCase) || str.Equals ("0", StringComparison.InvariantCultureIgnoreCase) || String.IsNullOrEmpty (str));
        }

        public IEnumerable<Context> GetContexts (Context context, ArgumentList arguments)
        {
            if (EvaluateCondition (context, arguments))
            {
                yield return context;
                yield return null;
            }
            else
            {
                yield return null;
                yield return context;
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