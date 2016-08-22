using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExBaseStringUtil
{
    public interface IParameterTransformerByCollection
    {
        string TransformTarget { get; set; }

        string NamedArg { get; set; }

        IEnumerable<object> Values { get; set; }

        Func<IEnumerable<object>, IEnumerable<string>> ValueFormatter { get; set; }

        string ValueSeperator { get; set; }

        Func<string, string> ReplacementFormatter { get; set; }

        string StartTag { get; set; }

        string EndTag { get; set; }

        string Result { get; }

        IEnumerable<string> GetFormattedValues();
    }
}