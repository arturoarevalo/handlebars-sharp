namespace Handlebars.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Handlebars.Core.Arguments;
    using Handlebars.Core.Elements;
    using Handlebars.Core.Interfaces;
    using Handlebars.Core.Parser;
    using Handlebars.Core.Tags.Builtin;
    using Handlebars.Core.Tags.Internal;

    public class Compiler
    {
        protected IDictionary<string, ITag> InternalTags { get; set; }
        protected IDictionary<string, ITag> RegisteredTags { get; set; }

        public Compiler ()
        {
            InternalTags = new Dictionary<string, ITag> ();
            RegisteredTags = new Dictionary<string, ITag> ();

            // Register internal tags.
            RegisterInternalTag (new StaticTag ());
            RegisterInternalTag (new VariableTag ());
            RegisterInternalTag (new TemplateTag ());
            RegisterInternalTag (new RenderPartialTag ());

            // Register built-in tags.
            RegisterTag (new WithTag ());
            RegisterTag (new EachTag ());
            RegisterTag (new IfTag ());
            RegisterTag (new UnlessTag ());
        }


        public void RegisterInternalTag (ITag tag)
        {
            InternalTags.Add (tag.Name, tag);
        }

        public void RegisterTag (ITag tag)
        {
            RegisteredTags.Add (tag.Name, tag);
        }

        private class CompilerContext
        {
            public IRenderable CurrentElement { get; set; }
            public bool InConditionalPath { get; set; }

            public ITag Tag
            {
                get { return CurrentElement.Tag; }
            }

            public void AddStatement (IRenderable statement)
            {
                if (CurrentElement is BlockElement)
                {
                    (CurrentElement as BlockElement).Statements.Add (statement);
                }
                else if (CurrentElement is ConditionalElement)
                {
                    if (InConditionalPath)
                    {
                        (CurrentElement as ConditionalElement).ConditionalStatements.Add (statement);
                    }
                    else
                    {
                        (CurrentElement as ConditionalElement).Statements.Add (statement);
                    }
                }
                else
                {
                    throw new Exception (@"Current element is not block nor conditional.");
                }
            }
        }


        public Template Compile (string source)
        {
            Stack <CompilerContext> stack = new Stack <CompilerContext> ();
            Tokenizer tokenizer = new Tokenizer (source);
            Template template = new Template (InternalTags [@"template"]);

            CompilerContext context = new CompilerContext
                                      {
                                          CurrentElement = template,
                                          InConditionalPath = false
                                      };

            bool inPartial = false;
            string currentPartial = String.Empty;

            foreach (var token in tokenizer.Tokens.Where (token => !token.IsEmpty))
            {
                if (token.Type == TokenTypes.StaticText)
                {
                    // Static content.
                    context.AddStatement (new StaticElement (token.Data, InternalTags [@"static"]));
                } else {
                    // A moustache ...
                    if (token.Subtype == TokenSubtypes.BeginTag)
                    {
                        // A begin tag can be a conditional separator (#if -> #else).
                        bool isConditionalPath = false;
                        ITag tag = GetBestTag (token.TagName, out isConditionalPath);

                        if (isConditionalPath)
                        {
                            if (context.Tag == tag)
                            {
                                // We're switching to the conditional brach.
                                context.InConditionalPath = true;
                            }
                            else
                            {
                                throw new Exception (@"Syntax error. Unexpected conditional branch.");
                            }
                        }
                        else
                        {
                            // We're creating a new element.
                            IRenderable statement = InstantiateTag (tag, token.Arguments);

                            if (tag is IInlineTag)
                            {
                                context.AddStatement (statement);
                            }
                            else 
                            {
                                stack.Push (context);
                                context = new CompilerContext
                                          {
                                              CurrentElement = statement,
                                              InConditionalPath = false
                                          };
                            }
                        }

                    } else if (token.Subtype == TokenSubtypes.EndTag)
                    {
                        if (inPartial && token.TagName == currentPartial)
                        {
                            var element = context.CurrentElement as BlockElement;
                            context = stack.Pop ();

                            template.Partials.Add (currentPartial, element);

                            inPartial = false;
                            currentPartial = String.Empty;
                        }
                        else
                        {
                            // We're closing a tag.
                            bool isConditionalPath = false;
                            ITag tag = GetBestTag (token.TagName, out isConditionalPath);

                            // It can't be a conditional branch.
                            if (isConditionalPath)
                            {
                                throw new Exception (@"Syntax error. Unexpected conditional branch.");
                            }

                            if (context.Tag == tag)
                            {
                                var element = context.CurrentElement;
                                context = stack.Pop ();

                                context.AddStatement (element);
                            }
                            else
                            {
                                throw new Exception (@"Syntax error. Unexpected ending tag.");
                            }
                        }
                    }
                    else if (token.Subtype == TokenSubtypes.DeclarePartialTag)
                    {
                        if (inPartial)
                        {
                            throw new Exception (@"Syntax error. Nested partials are not allowed.");
                        }

                        stack.Push (context);
                        context = new CompilerContext
                                  {
                                      CurrentElement = new Template (InternalTags [@"template"]),
                                      InConditionalPath = false
                                  };

                        currentPartial = token.TagName;
                        inPartial = true;
                    }
                    else if (token.Subtype == TokenSubtypes.RenderPartialTag)
                    {
                        context.AddStatement (InstantiateTag (InternalTags [@"renderpartial"], token.Arguments.Prepend ("partial", new StringArgument (token.TagName))));
                    }
                    else if (token.Subtype == TokenSubtypes.Variable)
                    {
                        context.AddStatement (new VariableElement (InternalTags [@"variable"], token.Type == TokenTypes.DoubleMustache) { Arguments = token.Arguments });
                    }
                }
            }

            // Validation.
            if (stack.Count > 0 || context.Tag != InternalTags [@"template"])
            {
                throw new Exception(@"Syntax error. Unexpected end of template.");
            }

            return template;
        }

        protected IRenderable InstantiateTag (ITag tag, ArgumentList arguments)
        {
            IRenderable statement;

            if (tag is IInlineTag)
            {
                statement = new InlineElement (tag)
                            {
                                Arguments = arguments
                            };
            } 
            else if (tag is IConditionalTag)
            {
                statement = new ConditionalElement (tag)
                            {
                                Arguments = arguments
                            };
            }
            else if (tag is IBlockTag)
            {
                statement = new BlockElement (tag)
                            {
                                Arguments = arguments
                            };
            }
            else
            {
                throw new Exception (String.Format (@"Internal compiler error. Unknown tag type {0}", tag));
            }

            // TODO: Validation of parameters.

            return statement;
        }


        protected ITag GetBestTag (string tagName, out bool isConditionalPath)
        {
            isConditionalPath = false;

            foreach (var tag in RegisteredTags.Values)
            {
                if (tagName == tag.Name)
                {
                    return tag;
                }

                if ((tag is IConditionalTag) && (tagName == (tag as IConditionalTag).ConditionalName))
                {
                    isConditionalPath = true;
                    return tag;
                }
            }

            return null;
        }

    }
}