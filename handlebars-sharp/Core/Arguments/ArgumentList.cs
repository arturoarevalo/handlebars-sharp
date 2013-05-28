namespace Handlebars.Core.Arguments
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Handlebars.Collections.Generic;
    using Handlebars.Core.Interfaces;

    public class ArgumentList : IEnumerable<KeyValuePair <string, IArgument>>
    {
        public static IArgument EmptyArgument = new NullArgument ();
        public static ArgumentList EmptyArgumentList = new ArgumentList ();

        protected OrderedDictionary <string, IArgument> Arguments { get; set; }

        public ArgumentList ()
        {
            Arguments = new OrderedDictionary<string, IArgument> ();
        }

        public ArgumentList (IEnumerable <string> items)
        {
            Arguments = new OrderedDictionary<string, IArgument> ();
            
            foreach (var item in items)
            {
                int integerValue;
                string argumentName;
                string argumentValue;

                if (item.StartsWith (@"""") || Int32.TryParse (item, out integerValue))
                {
                    argumentName = GetNextArgumentName ();
                    argumentValue = item;
                }
                else
                {
                    var parts = item.Split ('=');
                    if (parts.Length > 1)
                    {
                        argumentName = parts [0];
                        argumentValue = String.Join ("=", parts.Skip (1));
                    }
                    else
                    {
                        argumentName = GetNextArgumentName ();
                        argumentValue = item;
                    }
                }

                Arguments.Add (argumentName, ArgumentFactory.CreateArgument (argumentValue, true));
            }
        }

        public int Count
        {
            get { return Arguments.Count; }
        }

        public IArgument this [int index]
        {
            get
            {
                if (index < 0 || index >= Arguments.Count)
                {
                    return EmptyArgument;
                }
                else
                {
                    return Arguments [index].Value;
                }
            }
        }

        public IArgument this[string index]
        {
            get
            {
                IArgument value;
                if (Arguments.TryGetValue (index, out value))
                {
                    return value;
                }
                else
                {
                    return EmptyArgument;
                }
            }
        }

        public ArgumentList Add (string name, IArgument argument)
        {
            Arguments.Add (name, argument);
            return this;
        }

        public ArgumentList Prepend (string name, IArgument argument)
        {
            Arguments.Insert (0, new KeyValuePair <string, IArgument> (name, argument));
            return this;
        }

        protected string GetNextArgumentName ()
        {
            return String.Format (@"__argument{0}", Arguments.Count);
        }

        public bool IsNamed (int index)
        {
            return !Arguments [index].Key.StartsWith (@"__argument");
        }

        public string GetName (int index)
        {
            return Arguments [index].Key;
        }

        public IEnumerator <KeyValuePair <string, IArgument>> GetEnumerator ()
        {
            return Arguments.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }
    }
}