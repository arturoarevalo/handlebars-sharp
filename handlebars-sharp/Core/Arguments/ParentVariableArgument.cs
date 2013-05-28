namespace Handlebars.Core.Arguments
{
    using System;
    using Handlebars.Core.Interfaces;

    public class ParentVariableArgument : VariableArgument
    {
        public IArgument Argument { get; protected set; }

        public ParentVariableArgument (string name)
        {
            Argument = ArgumentFactory.CreateArgument (name, false);
        }

        public override Object Evaluate (Context context)
        {
            if (context.Parent != null)
            {
                return Argument.Evaluate (context.Parent);
            }
            else
            {
                return null;
            }
        }
    }
}