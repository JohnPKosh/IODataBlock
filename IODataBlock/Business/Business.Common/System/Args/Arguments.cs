using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Business.Common.IO.Serialization;
using ExBaseArguments;

namespace Business.Common.System.Args
{
    /// <summary>
    /// Arguments class for storing keyed args from standard console args.
    /// </summary>
    [DataContract(Namespace = @"http://www.broadvox.com/Args/", Name = "AV")]
    [KnownType(typeof(Arg))]
    public class Arguments
    {
        #region Class Initialization

        public Arguments(string id = null, IEqualityComparer<Arg> comparer = null)
        {
            Id = id;
            _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            if (comparer != null && comparer.GetType() == typeof(ArgKeyCaseSensitiveComparer))
            {
                Items = new HashSet<Arg>(comparer);
                _valueComparer = new ArgValueCaseSensitiveComparer();
                _stringComparisonType = StringComparison.InvariantCulture;
            }
            else
            {
                Items = new HashSet<Arg>(new ArgKeyIgnoreCaseComparer());
                _valueComparer = new ArgValueIgnoreCaseComparer();
                _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            }
        }

        public Arguments(IEnumerable<string> items, string id = null, IEqualityComparer<Arg> comparer = null)
        {
            Id = id;
            _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            if (comparer != null && comparer.GetType() == typeof(ArgKeyCaseSensitiveComparer))
            {
                Items = new HashSet<Arg>(comparer);
                _valueComparer = new ArgValueCaseSensitiveComparer();
                _stringComparisonType = StringComparison.InvariantCulture;
            }
            else
            {
                Items = new HashSet<Arg>(new ArgKeyIgnoreCaseComparer());
                _valueComparer = new ArgValueIgnoreCaseComparer();
                _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            }
            BuildArguments(items);
        }

        public Arguments(IEnumerable<Arg> items, string id = null, IEqualityComparer<Arg> comparer = null)
        {
            Id = id;
            _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            if (comparer != null && comparer.GetType() == typeof(ArgKeyCaseSensitiveComparer))
            {
                Items = new HashSet<Arg>(comparer);
                _valueComparer = new ArgValueCaseSensitiveComparer();
                _stringComparisonType = StringComparison.InvariantCulture;
            }
            else
            {
                Items = new HashSet<Arg>(new ArgKeyIgnoreCaseComparer());
                _valueComparer = new ArgValueIgnoreCaseComparer();
                _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            }
            Items.UnionWith(items);
        }

        public Arguments(IDictionary<string, string> items, string id = null, IEqualityComparer<Arg> comparer = null)
        {
            Id = id;
            _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            if (comparer != null && comparer.GetType() == typeof(ArgKeyCaseSensitiveComparer))
            {
                Items = new HashSet<Arg>(comparer);
                _valueComparer = new ArgValueCaseSensitiveComparer();
                _stringComparisonType = StringComparison.InvariantCulture;
            }
            else
            {
                Items = new HashSet<Arg>(new ArgKeyIgnoreCaseComparer());
                _valueComparer = new ArgValueIgnoreCaseComparer();
                _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            }
            UnionWith(items);
        }

        #endregion Class Initialization

        #region Fields / Properties

        #region Public Fields / Properties

        /* Start DataMembers */

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the HashSet items public property.
        /// </summary>
        /// <value>The items HashSet.</value>
        [DataMember(Name = "I", Order = 2)]
        public HashSet<Arg> Items { get; set; }

        /* End DataMembers */

        #region Ignored Members

        /// <summary>
        /// Gets the keys as IEnumerable.
        /// </summary>
        /// <value>The keys.</value>
        [IgnoreDataMember]
        public IEnumerable<string> Keys
        {
            get
            {
                return Items.Select(x => x.K);
            }
        }

        /// <summary>
        /// Gets the values as IEnumerable.
        /// </summary>
        /// <value>The values.</value>
        [IgnoreDataMember]
        public IEnumerable<string> Values
        {
            get
            {
                return Items.Select(x => x.K);
            }
        }

        /// <summary>
        /// Gets the count of Items.
        /// </summary>
        /// <value>The count.</value>
        [IgnoreDataMember]
        public int Count => Items.Count;

        #endregion Ignored Members

        #endregion Public Fields / Properties

        #region Private Fields  / Properties

        /// <summary>
        /// Private IEqualityComparer for the HashSet Items property.
        /// </summary>
        private readonly IEqualityComparer<Arg> _valueComparer;

        /// <summary>
        /// Private StringComparison enum field for the class.
        /// </summary>
        private readonly StringComparison _stringComparisonType;

        /// <summary>
        /// Private Regex for locating argument Keys in the console's args String[].
        /// </summary>
        private readonly Regex _r = new Regex(@"(?:\s|^)+(-x:<group>|[-/][a-z_?]{1}[0-9a-z_]*|[-/][a-z_]{1}[0-9a-z_]*[:=]{1})", RegexOptions.IgnoreCase);

        #endregion Private Fields  / Properties

        #endregion Fields / Properties

        #region Factory Methods

        /// <summary>
        /// Factory method to create an instance of the Arguments class.
        /// </summary>
        /// <param name="args">IEnumerable String arguments to convert.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns an instance of the Arguments class.</returns>
        public static Arguments CreateArguments(string args, string id = null)
        {
            var argarr = args.Split(" ".ToArray(), StringSplitOptions.None);
            return new Arguments(argarr, id);
        }

        /// <summary>
        /// Factory method to create an instance of the Arguments class.
        /// </summary>
        /// <param name="args">IEnumerable String arguments to convert.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns an instance of the Arguments class.</returns>
        public static Arguments CreateArguments(IEnumerable<string> args, string id = null)
        {
            return new Arguments(args, id);
        }

        /// <summary>
        /// Reads the string arguments from a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns an instance of the Arguments class.</returns>
        public static Arguments ReadStringArgumentsFromFile(string filePath, string id = null)
        {
            return ReadStringArgumentsFromFile(new FileInfo(Path.GetFullPath(filePath)), id);
        }

        /// <summary>
        /// Reads the string arguments from a file.
        /// </summary>
        /// <param name="fi">The FileInfo object.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns an instance of the Arguments class.</returns>
        public static Arguments ReadStringArgumentsFromFile(FileInfo fi, string id = null)
        {
            if (!fi.Exists)
                throw new FileNotFoundException(
                    "Arguments.LoadStringArgumentsFromFile: FileNotFoundException - The file was not found!");
            FileStream fs = null;
            try
            {
                fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (var sr = new StreamReader(fs))
                {
                    var readLine = sr.ReadLine();
                    if (readLine != null) return CreateArguments(readLine.Trim(), id);
                    else return null;
                }
            }
            finally
            {
                fs?.Dispose();
            }
        }

        /// <summary>
        /// Reads the arguments from file.
        /// </summary>
        /// <param name="argumentsFilePath">The arguments file path.</param>
        /// <returns></returns>
        public static Arguments ReadArgumentsFromFile(string argumentsFilePath)
        {
            return ReadArgumentsFromFile(new FileInfo(argumentsFilePath));
        }

        /// <summary>
        /// Reads the arguments from file.
        /// </summary>
        /// <param name="argumentsFileInfo">The arguments file info.</param>
        /// <returns></returns>
        public static Arguments ReadArgumentsFromFile(FileInfo argumentsFileInfo)
        {
            return new Arguments().Deserialize(argumentsFileInfo);
        }

        /// <summary>
        /// Expando to arguments.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="formatString">The format string.</param>
        /// <returns></returns>
        public static Arguments ExpandoToArguments(dynamic node, string formatString = null)
        {
            var rv = new Arguments();
            foreach (var property in node)
            {
                var t = property.Value.GetType();
                if (t.IsGenericType && !t.UnderlyingSystemType.IsValueType) continue;
                var value = property.Value.ToString();
                rv.Add(property.Key, string.IsNullOrWhiteSpace(formatString) ? value : string.Format(formatString, value));
            }
            return rv;
        }

        /// <summary>
        /// Froms the name value collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static Arguments FromNameValueCollection(NameValueCollection collection)
        {
            var rv = new Arguments();
            foreach (var c in collection.AllKeys)
            {
                rv.Add(c, collection[c]);
            }
            return rv;
        }

        #endregion Factory Methods

        #region Indexers

        public string this[string key]
        {
            get { return Items.FirstOrDefault(x => x.K.Equals(key, _stringComparisonType)).V; }
            set { Upsert(key, value); }
        }

        public string this[IEnumerable<string> i]
        {
            get
            {
                foreach (var str in i)
                {
                    if (ContainsKey(str)) return this[str];
                }
                throw new KeyNotFoundException();
            }
        }

        #endregion Indexers

        #region Methods

        #region Helper Methods

        /// <summary>
        /// Private helper method to build the Arguments class Keys and Values from the IEnumerable args supplied in the class constructor.
        /// </summary>
        /// <param name="args">IEnumerable String arguments to convert.</param>
        /// <param name="defaultsPrefix">Specifies prefix for defaults. Default value is default_</param>
        private void BuildArguments(IEnumerable<string> args, string defaultsPrefix = "default_")
        {
            var inputStringArgs = args.ToList();
            var str = string.Join(" ", inputStringArgs) + " ";
            str = str.EscapeArgs();
            var matches = _r.Matches(str);
            if (matches.Count == 0)
            {
                for (var i = 0; i < inputStringArgs.Count; i++)
                {
                    Add(i.ToString(CultureInfo.InvariantCulture), inputStringArgs[i].UnEscapeArgs());
                }
            }
            else if (matches.Count > 0 && matches[0].Index > 1)
            {
                var defaultvar = str.Substring(0, matches[0].Index).Trim();
                if (!string.IsNullOrEmpty(defaultvar))
                {
                    for (var i = 0; i < inputStringArgs.Count; i++)
                    {
                        if (inputStringArgs[i].Contains(matches[0].Value.Trim())) break;
                        Add(defaultsPrefix + i, inputStringArgs[i].UnEscapeArgs());
                    }
                }
            }
            for (var i = 0; i < matches.Count; i++)
            {
                var start = matches[i].Index;
                var keylen = matches[i].Length;
                var keyend = matches[i].Index + matches[i].Length + 1;

                if (i < matches.Count - 1)
                {
                    var end = matches[i + 1].Index;
                    var key = str.Substring(start, keylen).Trim();
                    key = Regex.Replace(key, @"[-/]?", string.Empty);
                    key = Regex.Replace(key, @"[:=]?\s{0,1}", string.Empty);
                    var val = end > keyend ? str.Substring(keyend, end - keyend).Trim() : "True";
                    if (!ContainsKey(key)) Add(key, val.UnEscapeArgs());
                }
                else
                {
                    var key = str.Substring(start, keylen).Trim();
                    key = Regex.Replace(key, @"[-/]?", string.Empty);
                    key = Regex.Replace(key, @"[:=]?", string.Empty);
                    var val = str.Substring(keyend).Trim();
                    if (string.IsNullOrEmpty(val)) val = "True";
                    if (!ContainsKey(key)) Add(key, val.UnEscapeArgs());
                }
            }
        }

        /// <summary>
        /// Gets the arguments as command line string.
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <param name="keySuffix">The key suffix.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public string GetArgumentsAsCommandLineString(string keyPrefix = "-", string keySuffix = ":", string seperator = @" ")
        {
            var strlist = Items.Select(a => $@"{keyPrefix}{a.K}{keySuffix}{a.V}").ToList();
            return string.Join(seperator, strlist);
        }

        #region HashSet Helper Methods

        /// <summary>
        /// Upserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns true if new record inserted, false if upserted.</returns>
        public bool Upsert(string key, string value)
        {
            key = key.Trim();
            if (Add(key, value)) return true;
            Items.Remove(new Arg { K = key });
            Add(key, value);
            return false;
        }

        /// <summary>
        /// Upserts the specified arg.
        /// </summary>
        /// <param name="arg">The arg.</param>
        /// <returns>Returns true if new record inserted, false if upserted.</returns>
        public bool Upsert(Arg arg)
        {
            if (Items.Add(arg)) return true;
            Items.Remove(arg);
            Items.Add(arg);
            return false;
        }

        public bool ContainsOneOfTheseKeys(IEnumerable<string> values)
        {
            return values.Any(ContainsKey);
        }

        #endregion HashSet Helper Methods

        #region Dictionary Helper Methods

        /// <summary>
        /// Read Only Dictionary property for Boolean KeyValuePairs in Arguments Dictionary.
        /// </summary>
        public Dictionary<string, bool> BooleanValues
        {
            get
            {
                var tempout = false;
                return Items.Where(a => bool.TryParse(a.V, out tempout)).ToDictionary(a => a.K, a => tempout);
            }
        }

        /// <summary>
        /// Read Only Dictionary property for DateTime KeyValuePairs in Arguments Dictionary.
        /// </summary>
        public Dictionary<string, DateTime> DateTimeValues
        {
            get
            {
                var tempout = new DateTime();
                return Items.Where(a => DateTime.TryParse(a.V, out tempout)).ToDictionary(a => a.K, a => tempout);
            }
        }

        /// <summary>
        /// Public Read Only Dictionary property for non-Boolean KeyValuePairs in Arguments Dictionary.
        /// </summary>
        public Dictionary<string, string> NonBooleanValues
        {
            get
            {
                bool tempout;
                return Items.Where(a => !bool.TryParse(a.V, out tempout)).ToDictionary(a => a.K, a => a.V);
            }
        }

        /// <summary>
        /// Public Helper Method to find and split KeyValuePair Dictionary items that contain a character seperated list of values.
        /// </summary>
        /// <param name="seperator">Seperator character used to split Value string on.</param>
        /// <returns>Returns a Dictionary of KeyValuePairs that contain character seperated values.</returns>
        public Dictionary<string, string[]> GetStringArrayValues(string seperator)
        {
            return GetStringArrayValues(seperator, StringSplitOptions.None);
        }

        /// <summary>
        /// Public Helper Method to find and split KeyValuePair Dictionary items that contain a character seperated list of values.
        /// </summary>
        /// <param name="seperator">Seperator String used to split Value string on.</param>
        /// <param name="options">StringSplitOptions enumeration for handling empty values.</param>
        /// <returns>Returns a Dictionary of KeyValuePairs that contain character seperated values.</returns>
        public Dictionary<string, string[]> GetStringArrayValues(string seperator, StringSplitOptions options)
        {
            var rv = new Dictionary<string, string[]>();
            foreach (var a in Items)
            {
                var alist = a.V.Split(seperator.ToArray(), options);
                if (alist.Length > 1)
                {
                    rv.Add(a.K, alist.Select(x => x.Trim()).ToArray());
                }
            }
            return rv;
        }

        /// <summary>
        /// Public Helper Method to find only DefaultArgsN (unnamed args from commandline) in Arguments.
        /// </summary>
        /// <returns>Returns a Dictionary of default arguments.</returns>
        public Dictionary<string, string> GetDefaultArgs(string defaultsPrefix = "default_")
        {
            return Where(x => x.K.StartsWith(defaultsPrefix)).ToDictionary(di => di.K, di => di.V);
        }

        /// <summary>
        /// Public Helper Method to Concatenate the DefaultArgsN to a String with a specified seperator.
        /// </summary>
        /// <param name="seperator">Seperator String used to concatenate the Value strings on.</param>
        /// <param name="defaultsPrefix"></param>
        /// <returns>String</returns>
        public string ConcatenateDefaultArgs(string seperator, string defaultsPrefix = "default_")
        {
            var s = new StringBuilder();
            foreach (var d in GetDefaultArgs(defaultsPrefix))
            {
                s.Append(d.Value + seperator);
            }
            return s.ToString();
        }

        #endregion Dictionary Helper Methods

        #endregion Helper Methods

        #region Inherited HashSet Helper Methods

        public bool Add(string key, string value)
        {
            return Items.Add(new Arg { K = key.Trim(), V = value });
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(Arg arg)
        {
            return Items.Contains(arg, Items.Comparer);
        }

        public bool Contains(Arg arg, IEqualityComparer<Arg> comparer)
        {
            return Items.Contains(arg, comparer);
        }

        public bool ContainsKey(string key)
        {
            return Items.Contains(new Arg { K = key }, Items.Comparer);
        }

        public bool ContainsValue(string value)
        {
            return Items.Contains(new Arg { V = value }, _valueComparer);
        }

        public bool ContainsKey(string key, IEqualityComparer<Arg> comparer)
        {
            return Items.Contains(new Arg { K = key }, comparer);
        }

        public bool ContainsValue(string value, IEqualityComparer<Arg> comparer)
        {
            return Items.Contains(new Arg { V = value }, comparer);
        }

        public void CopyTo(Arg[] array)
        {
            Items.CopyTo(array);
        }

        public void CopyTo(Arg[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Arg[] array, int arrayIndex, int count)
        {
            Items.CopyTo(array, arrayIndex, count);
        }

        public void ExceptWith(Arguments other)
        {
            Items.ExceptWith(other.Items);
        }

        public void ExceptWith(IEnumerable<Arg> other)
        {
            Items.ExceptWith(other);
        }

        public void ExceptWith(IEnumerable<string> other)
        {
            Items.ExceptWith(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public HashSet<Arg>.Enumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public void IntersectWith(Arguments other)
        {
            Items.IntersectWith(other.Items);
        }

        public void IntersectWith(IEnumerable<Arg> other)
        {
            Items.IntersectWith(other);
        }

        public void IntersectWith(IEnumerable<string> other)
        {
            Items.IntersectWith(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public bool IsProperSubsetOf(Arguments other)
        {
            return Items.IsProperSubsetOf(other.Items);
        }

        public bool IsProperSubsetOf(IEnumerable<Arg> other)
        {
            return Items.IsProperSubsetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<string> other)
        {
            return Items.IsProperSubsetOf(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public bool IsProperSupersetOf(Arguments other)
        {
            return Items.IsProperSupersetOf(other.Items);
        }

        public bool IsProperSupersetOf(IEnumerable<Arg> other)
        {
            return Items.IsProperSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<string> other)
        {
            return Items.IsProperSupersetOf(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public bool IsSubsetOf(Arguments other)
        {
            return Items.IsSubsetOf(other.Items);
        }

        public bool IsSubsetOf(IEnumerable<Arg> other)
        {
            return Items.IsSubsetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<string> other)
        {
            return Items.IsSubsetOf(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public bool IsSupersetOf(Arguments other)
        {
            return Items.IsSupersetOf(other.Items);
        }

        public bool IsSupersetOf(IEnumerable<Arg> other)
        {
            return Items.IsSupersetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<string> other)
        {
            return Items.IsSupersetOf(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public bool Overlaps(Arguments other)
        {
            return Items.Overlaps(other.Items);
        }

        public bool Overlaps(IEnumerable<Arg> other)
        {
            return Items.Overlaps(other);
        }

        public bool Overlaps(IEnumerable<string> other)
        {
            return Items.Overlaps(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public bool Remove(Arg item)
        {
            return Items.Remove(item);
        }

        public bool Remove(string item)
        {
            return Items.Remove(new Arg { K = item });
        }

        public int RemoveWhere(Predicate<Arg> match)
        {
            return Items.RemoveWhere(match);
        }

        public bool SetEquals(Arguments other)
        {
            return Items.SetEquals(other.Items);
        }

        public bool SetEquals(IEnumerable<Arg> other)
        {
            return Items.SetEquals(other);
        }

        public bool SetEquals(IEnumerable<string> other)
        {
            return Items.SetEquals(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public void SymmetricExceptWith(Arguments other)
        {
            Items.SymmetricExceptWith(other.Items);
        }

        public void SymmetricExceptWith(IEnumerable<Arg> other)
        {
            Items.SymmetricExceptWith(other);
        }

        public void SymmetricExceptWith(IEnumerable<string> other)
        {
            Items.SymmetricExceptWith(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public void TrimExcess()
        {
            Items.TrimExcess();
        }

        public void UnionWith(Arguments other)
        {
            Items.UnionWith(other.Items);
        }

        public void UnionWith(IEnumerable<Arg> other)
        {
            Items.UnionWith(other);
        }

        public void UnionWith(IDictionary<string, string> other)
        {
            foreach (var kp in other)
            {
                Add(kp.Key, kp.Value);
            }
        }

        public void UnionWith(Hashtable other, Func<object, string> keyConverter = null, Func<object, string> valueConverter = null)
        {
            if (keyConverter == null) keyConverter = x => x.ToString();
            if (valueConverter == null) valueConverter = x => x.ToString();
            foreach (DictionaryEntry de in other)
            {
                Add(keyConverter(de.Key), valueConverter(de.Value));
            }
        }

        #endregion Inherited HashSet Helper Methods

        #region Inherited System.Collections Helper Methods

        public bool All(Func<Arg, bool> predicate)
        {
            return Items.All(predicate);
        }

        public bool Any()
        {
            return Items.Any();
        }

        public bool Any(Func<Arg, bool> predicate)
        {
            return Items.Any(predicate);
        }

        public IEnumerable<Arg> AsEnumerable()
        {
            return Items.AsEnumerable();
        }

        public ParallelQuery AsParallel()
        {
            return Items.AsParallel();
        }

        public IQueryable AsQueryable()
        {
            return Items.AsQueryable();
        }

        public IEnumerable<Arg> Concat(IEnumerable<Arg> other)
        {
            return Items.Concat(other);
        }

        public IEnumerable<Arg> Concat(Arguments other)
        {
            return Items.Concat(other.Items);
        }

        public int CountArgs(Func<Arg, bool> predicate)
        {
            return Items.Count(predicate);
        }

        public IEnumerable<Arg> Except(Arguments other)
        {
            return Items.Except(other.Items);
        }

        public IEnumerable<Arg> Except(IEnumerable<Arg> other)
        {
            return Items.Except(other);
        }

        public IEnumerable<Arg> Except(IEnumerable<string> other)
        {
            return Items.Except(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public Arg First(Func<Arg, bool> predicate)
        {
            return Items.First(predicate);
        }

        public Arg FirstOrDefault(Func<Arg, bool> predicate)
        {
            return Items.FirstOrDefault(predicate);
        }

        public IEnumerable<Arg> Intersect(Arguments other)
        {
            return Items.Intersect(other.Items);
        }

        public IEnumerable<Arg> Intersect(IEnumerable<Arg> other)
        {
            return Items.Intersect(other);
        }

        public IEnumerable<Arg> Intersect(IEnumerable<string> other)
        {
            return Items.Intersect(other.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Arg { K = x }));
        }

        public Int64 LongCount()
        {
            return Items.LongCount();
        }

        public Int64 LongCount(Func<Arg, bool> predicate)
        {
            return Items.LongCount(predicate);
        }

        public IEnumerable<TResult> Select<TResult>(Func<Arg, TResult> selector)
        {
            return Items.Select(selector);
        }

        public Dictionary<string, string> ToDictionary()
        {
            return Items.ToDictionary(x => x.K, y => y.V);
        }

        public List<Arg> ToList()
        {
            return Items.ToList();
        }

        public IEnumerable<Arg> Union(Arguments other)
        {
            return Items.Union(other.Items);
        }

        public IEnumerable<Arg> Union(IEnumerable<Arg> other)
        {
            return Items.Union(other);
        }

        public IEnumerable<Arg> Where(Func<Arg, bool> predicate)
        {
            return Items.Where(predicate);
        }

        #endregion Inherited System.Collections Helper Methods

        #endregion Methods
    }
}