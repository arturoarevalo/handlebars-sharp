namespace Handlebars.Core.Parser
{
    using System;
    using System.Linq;
    using Handlebars.Core.Arguments;

    public class Token
    {
        public TokenTypes Type { get; set; }
        public string Data { get; set; }

        public bool IsEmpty
        {
            get { return String.IsNullOrEmpty (Data); }
        }


        public TokenSubtypes Subtype
        {
            get
            {
                if (Data.StartsWith (@"#"))
                {
                    return TokenSubtypes.BeginTag;
                }

                if (Data.StartsWith (@"/"))
                {
                    return TokenSubtypes.EndTag;
                }
			
                if (Data.StartsWith (@">")) {
                    return TokenSubtypes.RenderPartialTag;
                }

                if (Data.StartsWith (@"<"))
                {
                    return TokenSubtypes.DeclarePartialTag;
                }

                return TokenSubtypes.Variable;
            }
        }

        public ArgumentList Arguments
        {
            get
            {
                if (Type == TokenTypes.StaticText)
                {
                    return ArgumentList.EmptyArgumentList;
                }

                var items = Data.Split (' ');

                if (Subtype == TokenSubtypes.Variable)
                {
                    return new ArgumentList (items);
                }
                else
                {
                    return new ArgumentList(items.Skip (1));
                }
            }
        }

        public string TagName
        {
            get
            {
                if (Subtype != TokenSubtypes.Variable)
                {
                    // Remove tag prefix (#, >, <) from the tag name.
                    return Data.Split (' ') [0].Substring (1);
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public override string ToString ()
        {
            return String.Format (@"Token Type:{0} Data:[{1}]", Type, Data);
        }
    }
}