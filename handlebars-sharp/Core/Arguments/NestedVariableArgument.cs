namespace Handlebars.Core.Arguments
{
    using System;
    using System.Collections;
    using System.Linq;
    using Handlebars.Core.Interfaces;

    public class IndexedVariableArgument : VariableArgument
    {
        public IArgument MainArgument { get; protected set; }
        public int Index { get; protected set; }

        public IndexedVariableArgument (IArgument mainArgument, int index)
        {
            MainArgument = mainArgument;
            Index = index;
        }

        public override Object Evaluate (Context context)
        {
            var mainObject = MainArgument.Evaluate (context);

            if (mainObject != null && mainObject is IEnumerable)
            {
                try
                {
                    return (mainObject as IEnumerable).Cast <Object> ().ElementAt (Index);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }

    public class NestedVariableArgument : VariableArgument
    {
        public IArgument MainArgument { get; protected set; }
        public IArgument NestedArgument { get; protected set; }

        public NestedVariableArgument (string name, bool allowContextRecursion)
        {
            string [] parts = name.Split (new [] { '.' }, 2);

            /*
            if (allowContextRecursion)
            {
                MainArgument = new RecursiveVariableArgument (parts [0]);
            } else 
            {
                MainArgument = new StrictVariableArgument (parts[0]);
            }
            */
            MainArgument = ArgumentFactory.CreateArgument (parts [0], allowContextRecursion);
            NestedArgument = ArgumentFactory.CreateArgument (parts [1], false);

            if (NestedArgument != null)
            {
                // Optimize indexed access.
                if (NestedArgument is IntegerArgument)
                {
                    MainArgument = new IndexedVariableArgument (MainArgument, (NestedArgument as IntegerArgument).Data);
                    NestedArgument = null;
                }
                else if (NestedArgument is NestedVariableArgument && (NestedArgument as NestedVariableArgument).MainArgument is IntegerArgument)
                {
                    MainArgument = new IndexedVariableArgument (MainArgument, ((NestedArgument as NestedVariableArgument).MainArgument as IntegerArgument).Data);
                    NestedArgument = (NestedArgument as NestedVariableArgument).NestedArgument;
                }
            }
        }

        public override Object Evaluate (Context context)
        {
            Context nested = context.CreateChild (MainArgument.Evaluate (context));
            return NestedArgument.Evaluate (nested);
        }
    }
}