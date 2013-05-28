namespace Handlebars.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Handlebars.Collections.Specialized;
    using Handlebars.Core.Interfaces;

    public class Context
    {
        public TextWriter Writer { get; set; }
        public Object Source { get; protected set; }
        public Context Parent { get; protected set; }

        public Func<string, IRenderable> PartialResolver { get; set; }
        public Func<string, Object> VariableResolver { get; set; }

        protected Dictionary <string, Object> LocalVariables;

        public Context (object source, TextWriter writer)
        {
            Source = source;
            Writer = writer;
            Parent = null;
        }

        protected Context ()
        {}

        public Context Root
        {
            get
            {
                Context context = this;
                while (context.Parent != null)
                {
                    context = context.Parent;
                }
                return context;
            }
        }

        public Context CreateChild (object source)
        {
            return new Context
                   {
                       Source = source,
                       Writer = Writer,
                       Parent = this,
                       VariableResolver = VariableResolver,
                       PartialResolver = PartialResolver
                   };
        }

        public Context Clone ()
        {
            // TODO: 
            return new Context
                   {
                       Source = Source,
                       Writer = Writer,
                       Parent = Parent,
                       VariableResolver = VariableResolver,
                       PartialResolver = PartialResolver,
                       LocalVariables = LocalVariables
                   };
        }

        protected PropertyDictionary sourcePropertyDictionary;

        public IRenderable ResolvePartial (string name)
        {
            if (PartialResolver != null)
            {
                return PartialResolver (name);
            }

            return null;
        }


        protected PropertyDictionary PropertyDictionary
        {
            get
            {
                if (sourcePropertyDictionary == null)
                {
                    sourcePropertyDictionary = new PropertyDictionary (Source);
                }

                return sourcePropertyDictionary;
            }
        }

        public void SetLocalVariable (string key, Object value)
        {
            // Lazy initialization.
            if (LocalVariables == null)
            {
                LocalVariables = new Dictionary <string, object> ();
            }

            LocalVariables [key] = value;
        }

        public EvaluationResult EvaluateLocal (string key)
        {
            EvaluationResult result = new EvaluationResult ();

            Context context = this;
            do
            {
                result.Found = context.LocalVariables != null && context.LocalVariables.TryGetValue (key, out result.Value);
                context = context.Parent;
            } while (result.Found == false && context != null);

            return result;
        }

        public EvaluationResult Evaluate (string key)
        {
            EvaluationResult result;
            result.Found = PropertyDictionary.TryGetValue (key, out result.Value);
            return result;
        }

        public EvaluationResult EvaluateRecursive (string key)
        {
            EvaluationResult result;
            Context context = this;
            do
            {
                result.Found = context.PropertyDictionary.TryGetValue (key, out result.Value);
                context = context.Parent;
            } while (result.Found == false && context != null);

            if (!result.Found)
            {
                return EvaluateLocal (key);
            }
            else
            {
                return result;
            }
        }
    }
}