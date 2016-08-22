using Business.Common.System.Args;
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExBaseArguments
{
    public class ArgKeyCaseSensitiveComparer : EqualityComparer<Arg>
    {
        public override bool Equals(Arg arg1, Arg arg2)
        {
            return arg1.K.Equals(arg2.K, StringComparison.InvariantCulture);
        }

        public override int GetHashCode(Arg value)
        {
            return value.K.GetHashCode();
        }
    }
}