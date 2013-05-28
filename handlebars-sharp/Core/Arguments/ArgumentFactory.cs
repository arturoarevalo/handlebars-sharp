namespace Handlebars.Core.Arguments
{
    using System;
    using Handlebars.Core.Interfaces;

    public static class ArgumentFactory
    {
        public static IArgument CreateArgument (string name, bool allowContextRecursion)
        {
            int integerValue;

            if (name.StartsWith (@""""))
            {
                return new StringArgument (name.Substring (1, name.Length - 2));
            }
            else if (Int32.TryParse (name, out integerValue))
            {
                return new IntegerArgument (integerValue);
            }
            else if (name.StartsWith (@"../"))
            {
                return new ParentVariableArgument (name.Substring (3));
            }
            else if (name.Contains (@"."))
            {
                var nested = new NestedVariableArgument (name, allowContextRecursion);
                if (nested.NestedArgument == null)
                {
                    return nested.MainArgument;
                }
                else
                {
                    return nested;
                }
            }
            else if (String.IsNullOrEmpty (name) || name == @"this")
            {
                return new ThisArgument ();
            }
            else
            {
                if (allowContextRecursion)
                {
                    if (name.StartsWith (@"@"))
                    {
                        return new LocalVariableArgument (name.Substring (1));
                    }
                    else
                    {
                        return new RecursiveVariableArgument (name);
                    }
                }
                else
                {
                    return new StrictVariableArgument (name);
                }
            }
        }
    }
}