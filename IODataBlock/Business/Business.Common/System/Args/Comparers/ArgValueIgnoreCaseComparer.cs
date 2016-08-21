using Business.Common.System.Args;
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExBaseArguments
{
    public class ArgValueIgnoreCaseComparer : EqualityComparer<Arg>
    {
        public override bool Equals(Arg arg1, Arg arg2)
        {
            return arg1.V.Equals(arg2.V, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode(Arg value)
        {
            return value.V.ToLowerInvariant().GetHashCode();
        }
    }
}