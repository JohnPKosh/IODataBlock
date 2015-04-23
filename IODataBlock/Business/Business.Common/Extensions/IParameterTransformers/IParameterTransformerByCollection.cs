using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExBaseStringUtil
{
    public interface IParameterTransformerByCollection
    {

        String TransformTarget { get; set; }

        String NamedArg { get; set; }

        IEnumerable<Object> Values { get; set; }

        Func<IEnumerable<Object>, IEnumerable<String>> ValueFormatter { get; set; }

        String ValueSeperator { get; set; }

        Func<String, String> ReplacementFormatter { get; set; }

        String StartTag { get; set; }

        String EndTag { get; set; }

        String Result { get; }

        IEnumerable<String> GetFormattedValues();
    }
}
