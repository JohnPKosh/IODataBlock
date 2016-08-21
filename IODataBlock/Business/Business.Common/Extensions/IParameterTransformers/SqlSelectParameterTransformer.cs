using Business.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace ExBaseStringUtil
{
    public class SqlSelectParameterTransformer : IParameterTransformerByCollection
    {
        public SqlSelectParameterTransformer()
        {
            ValueFormatter = x => x.Select(i => $@"{"["}{i.ToString().Trim()}{"]"}").ToList();
            ValueSeperator = ",\r\n";
            ReplacementFormatter = null;
            StartTag = @"$(";
            EndTag = @")";
        }

        public SqlSelectParameterTransformer(string transformTarget, string namedArg, IEnumerable<object> values)
        {
            TransformTarget = transformTarget;
            NamedArg = namedArg;
            Values = values;

            ValueFormatter = x => x.Select(i => $@"{"["}{i.ToString().Trim()}{"]"}").ToList();
            ValueSeperator = ",\r\n";
            ReplacementFormatter = null;
            StartTag = @"$(";
            EndTag = @")";
        }

        #region Interface Implementation

        public string TransformTarget { get; set; }

        public string NamedArg { get; set; }

        public IEnumerable<object> Values { get; set; }

        public Func<IEnumerable<object>, IEnumerable<string>> ValueFormatter { get; set; }

        public string ValueSeperator { get; set; }

        public Func<string, string> ReplacementFormatter { get; set; }

        public string StartTag { get; set; }

        public string EndTag { get; set; }

        public string Result => TransformTarget.ReplaceNamedParameterByIEnumerableObjects(NamedArg, Values, ValueFormatter, ValueSeperator, ReplacementFormatter, StartTag, EndTag);

        public IEnumerable<string> GetFormattedValues()
        {
            return ValueFormatter?.Invoke(Values);
        }

        #endregion Interface Implementation
    }
}