using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Xml.Linq;

namespace Business.Utilities.Extensions
{
    public static class IEnumerableExtensionBase
    {
        public static void IEnumerableCallAction<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var o in values)
            {
                action(o);
            }
        }

        public static void IEnumerableCallActions<T>(this IEnumerable<T> values, params Action<T>[] actions)
        {
            foreach (var t in values)
            {
                foreach (var t1 in actions)
                {
                    t1(t);
                }
            }
        }

        public static IEnumerable<TDest> IEnumerableTransformEach<TDest, TSource>(this IEnumerable<TSource> values, Func<TSource, TDest> function)
        {
            return values.Select(function);
        }

        public delegate TOut Action<in TIn, out TOut>(TIn element);

        public static IEnumerable<TOut> Transform<TIn, TOut>(IEnumerable<TIn> list, Action<TIn, TOut> method)
        {
            return list.Select(entry => method(entry));
        }

        public static ObservableCollection<T> IEnumerableToObservableCollection<T>(this IEnumerable<T> values)
        {
            return new ObservableCollection<T>(values);
        }

        public static IEnumerable<TSource> IEnumerableTop<TSource, TKey>(this IEnumerable<TSource> source, Int32 count, Func<TSource, TKey> orderBy)
        {
            return source.OrderBy(orderBy).Take(count).AsEnumerable();
        }

        public static XElement IEnumerableToXElement<TSource>(this IEnumerable<TSource> list
            , String filePath
            , String itemName = @"Arg"
            , String valueName = "V"
            ) where TSource : IConvertible
        {
            var x = new XElement(itemName);
            foreach (var e in list)
            {
                x.Add(new XElement(valueName, e));
            }
            return x;
        }

        public static IEnumerable<TSource> IEnumerableFromXElement<TSource>(
            XElement xml
            , String valueName = "V"
            ) where TSource : IConvertible
        {
            return xml.Elements(valueName).Select(el => (TSource)Convert.ChangeType(el.Value, typeof(TSource)));
        }

        public static void IEnumerableToXElementToFile<TSource>(this IEnumerable<TSource> list
            , String filePath
            , String itemName = @"Arg"
            , String valueName = "V"
            ) where TSource : IConvertible
        {
            var x = new XElement(itemName);
            foreach (var e in list)
            {
                x.Add(new XElement(valueName, e));
            }
            x.Save(filePath);
        }

        public static IEnumerable<TSource> IEnumerableFromXElementFromFile<TSource>(
            String filePath
            , String valueName = "V"
            ) where TSource : IConvertible
        {
            var x = XElement.Load(filePath);
            return x.Elements(valueName).Select(el => (TSource)Convert.ChangeType(el.Value, typeof(TSource)));
        }

        public static bool In<T>(this T source, params T[] list)
        {
            return list.ToList().Contains(source);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static bool NotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static bool NotAny<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }

        public static bool IsNullOrNotAny<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source == null || !source.Any(predicate);
        }

        public static bool NotNullAndAny<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source != null && source.Any(predicate);
        }

        public static IEnumerable<T> SkipTake<T>(this IEnumerable<T> source, Int32 skip, Int32 take)
        {
            return source.Skip(skip).Take(take);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, Int32 index, Int32 size)
        {
            return source.Skip(index * size).Take(size);
        }

        public static IEnumerable<T> PageWhere<T>(this IEnumerable<T> source, Int32 index, Int32 size, Func<T, bool> predicate)
        {
            return source.Where(predicate).Skip(index * size).Take(size);
        }

        public static IEnumerable<T> PageWhere<T>(this IEnumerable<T> source, Int32 index, Int32 size, string predicate, params object[] values)
        {
            return source.AsQueryable().Where(predicate, values).Skip(index * size).Take(size);
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            yield return value;
            foreach (var element in source)
            {
                yield return element;
            }
        }

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            foreach (var element in source)
            {
                yield return element;
            }
            yield return value;
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> value)
        {
            foreach (var element in value)
            {
                yield return element;
            }
            foreach (var element in source)
            {
                yield return element;
            }
        }

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> value)
        {
            foreach (var element in source)
            {
                yield return element;
            }
            foreach (var element in value)
            {
                yield return element;
            }
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<IEnumerable<TSource>> source, IEnumerable<TSource> value)
        {
            foreach (var element in value)
            {
                yield return element;
            }
            foreach (var child in source.SelectMany(element => element))
            {
                yield return child;
            }
        }

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<IEnumerable<TSource>> source, IEnumerable<TSource> value)
        {
            foreach (var child in source.SelectMany(element => element))
            {
                yield return child;
            }
            foreach (var element in value)
            {
                yield return element;
            }
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<IEnumerable<TSource>> source, IEnumerable<IEnumerable<TSource>> value)
        {
            foreach (var child in value.SelectMany(element => element))
            {
                yield return child;
            }
            foreach (var child in source.SelectMany(element => element))
            {
                yield return child;
            }
        }

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<IEnumerable<TSource>> source, IEnumerable<IEnumerable<TSource>> value)
        {
            foreach (var element in source)
            {
                foreach (var child in element)
                {
                    yield return child;
                }
            }
            foreach (var element in value)
            {
                foreach (var child in element)
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Pages<T>(this IEnumerable<T> source, Int32 size)
        {
            var cnt = 0;
            while (true)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var page = source.SkipTake(cnt++ * size, size);
                // ReSharper disable once PossibleMultipleEnumeration
                if (page.IsNullOrEmpty()) break;
                // ReSharper disable once PossibleMultipleEnumeration
                yield return page;
            }
        }

        public static IEnumerable<IEnumerable<T>> PagesWhere<T>(this IEnumerable<T> source, Int32 size, Func<T, bool> predicate)
        {
            var cnt = 0;
            while (true)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var page = source.Where(predicate).SkipTake(cnt++ * size, size);
                // ReSharper disable once PossibleMultipleEnumeration
                if (page.IsNullOrEmpty()) break;
                // ReSharper disable once PossibleMultipleEnumeration
                yield return page;
            }
        }

        public static IEnumerable<IEnumerable<T>> PagesWhere<T>(this IEnumerable<T> source, Int32 size, string predicate, params object[] values)
        {
            var cnt = 0;
            while (true)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var page = source.AsQueryable().Where(predicate, values).SkipTake(cnt++ * size, size);
                // ReSharper disable once PossibleMultipleEnumeration
                if (page.IsNullOrEmpty()) break;
                // ReSharper disable once PossibleMultipleEnumeration
                yield return page;
            }
        }

        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> values)
        {
            if (!values.Any())
                return values;
            if (!values.First().Any())
                return Transpose(values.Skip(1));

            var x = values.First().First();
            var xs = values.First().Skip(1);
            var xss = values.Skip(1);
            return
             new[] {new[] {x}
           .Concat(xss.Select(ht => ht.First()))}
               .Concat(new[] { xs }
               .Concat(xss.Select(ht => ht.Skip(1)))
               .Transpose());
        }

        /*
        //Input: transpose [[1,2,3],[4,5,6],[7,8,9]]
//Output: [[1,4,7],[2,5,8],[3,6,9]]
var result = new[] {new[] {1, 2, 3}, new[] {4, 5, 6}, new[] {7, 8, 9}}.Transpose();
        */

        public static Dictionary<TKey1, Dictionary<TKey2, TValue>> Pivot<TSource, TKey1, TKey2, TValue>(
            this IEnumerable<TSource> source, Func<TSource, TKey1> key1Selector
            , Func<TSource, TKey2> key2Selector
            , Func<IEnumerable<TSource>, TValue> aggregate
            )
        {
            return source.GroupBy(key1Selector).Select(
            x => new
            {
                X = x.Key,
                Y = x.GroupBy(key2Selector).Select(
                z => new
                {
                    Z = z.Key,
                    V = aggregate(z)
                }
                ).ToDictionary(e => e.Z, o => o.V)
            }
            ).ToDictionary(e => e.X, o => o.Y);
        }

        /*
         class Program {
    internal class Employee {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Function { get; set; }
        public decimal Salary { get; set; }
    }

    static void Main(string[] args) {
        var l = new List<Employee>() {
            new Employee() { Name = "Fons", Department = "R&D", Function = "Trainer", Salary = 2000 },
            new Employee() { Name = "Jim", Department = "R&D", Function = "Trainer", Salary = 3000 },
            new Employee() { Name = "Ellen", Department = "Dev", Function = "Developer", Salary = 4000 },
            new Employee() { Name = "Mike", Department = "Dev", Function = "Consultant", Salary = 5000 },
            new Employee() { Name = "Jack", Department = "R&D", Function = "Developer", Salary = 6000 },
            new Employee() { Name = "Demy", Department = "Dev", Function = "Consultant", Salary = 2000 }};

        var result1 = l.Pivot(emp => emp.Department, emp2 => emp2.Function, lst => lst.Sum(emp => emp.Salary));

        foreach (var row in result1) {
            Console.WriteLine(row.Key);
            foreach (var column in row.Value) {
                Console.WriteLine("  " + column.Key + "\t" + column.Value);
            }
        }

*/

        public static IEnumerable<TSource> SelectRecursive<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> recursiveSelector
            )
        {
            var stack = new Stack<IEnumerator<TSource>>();
            stack.Push(source.GetEnumerator());
            try
            {
                while (stack.Count > 0)
                {
                    if (stack.Peek().MoveNext())
                    {
                        var current = stack.Peek().Current;
                        yield return current;
                        stack.Push(recursiveSelector(current).GetEnumerator());
                    }
                    else
                    {
                        stack.Pop().Dispose();
                    }
                }
            }
            finally
            {
                while (stack.Count > 0)
                {
                    stack.Pop().Dispose();
                }
            }
        }

        #region to xml string or xelements

        //public static XElement ToXElement<T>(this T input)
        //{
        //    return XElement.Parse(input.ToXmlString());
        //}

        //public static IEnumerable<XElement> ToXElements<T>(this IEnumerable<T> input)
        //{
        //    foreach (var item in input)
        //        yield return input.ToXElement();
        //}

        //public static IEnumerable<string> ToXmlString<T>(this IEnumerable<T> input)
        //{
        //    foreach (var item in input)
        //        yield return item.ToXmlString();
        //}

        //public static string ToXmlString<T>(this T input)
        //{
        //    using (var writer = new StringWriter())
        //    {
        //        input.ToXml(writer);
        //        return writer.ToString();
        //    }
        //}

        //public static void ToXml<T>(this T objectToSerialize, Stream stream)
        //{
        //    new XmlSerializer(typeof(T)).Serialize(stream, objectToSerialize);
        //}

        //public static void ToXml<T>(this T objectToSerialize, StringWriter writer)
        //{
        //    new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize);
        //}

        #endregion to xml string or xelements

        public static IDictionary<TKey, IEnumerable<T>> PartitionToDictionary<TKey, T>(this IEnumerable<T> source, IEnumerable<TKey> partitionKeys, Func<TKey, IEnumerable<T>, IEnumerable<T>> partitioner)
        {
            return partitionKeys.ToDictionary(k => k, k => partitioner(k, source));
        }
    }
}