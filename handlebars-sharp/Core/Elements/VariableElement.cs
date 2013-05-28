namespace Handlebars.Core.Elements
{
    using Handlebars.Core.Interfaces;

    public class VariableElement : InlineElement
    {
        public bool Secure { get; protected set; }

        public VariableElement (ITag tag, bool secure) : base (tag)
        {
            Secure = secure;
        }

        public override void RenderToContext (Context context)
        {
            if (context != null)
            {
                var data = InternalTag.GetData (context, Arguments);
                if (data != null)
                {
                    if (Secure)
                    {
                        context.Writer.Write (System.Net.WebUtility.HtmlEncode (data));
                    }
                    else
                    {
                        context.Writer.Write (data);
                    }
                }
            }
        }
    }
}