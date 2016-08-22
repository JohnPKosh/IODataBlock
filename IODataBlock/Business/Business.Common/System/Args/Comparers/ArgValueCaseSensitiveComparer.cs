using Business.Common.System.Args;
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExBaseArguments
{
    public class ArgValueCaseSensitiveComparer : EqualityComparer<Arg>
    {
        public override bool Equals(Arg arg1, Arg arg2)
        {
            return arg1.V.Equals(arg2.V, StringComparison.InvariantCulture);
        }

        public override int GetHashCode(Arg value)
        {
            return value.V.GetHashCode();
        }
    }
}