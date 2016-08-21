using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Common.Extensions
{
    public static class TplExtensions
    {
        public static List<TDest> ParallelTransformEach<TDest, TSource>(this IEnumerable<TSource> values, Func<TSource, TDest> function, int maxDegreeOfParallelism = 4)
        {
            var rv = new List<TDest>();
            Parallel.ForEach(values, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, t =>
            {
                rv.Add(function(t));
            });
            return rv;
        }

        //public static void SampleTaskQueue(IEnumerable<string> values, int queueSize = 4)
        //{
        //    var tasks = new Queue<Task>();
        //    foreach (var value in values)
        //    {
        //        var t = Task.Run(() =>
        //        {
        //            // do work
        //        });
        //        tasks.Enqueue(t);

        //        if (tasks.Count != queueSize) continue;
        //        for (var i = 0; i < queueSize; i++)
        //        {
        //            Task.WaitAll(tasks.ToArray());
        //            tasks.Clear();
        //        }
        //    }
        //    if (tasks.Count > 0) Task.WaitAll(tasks.ToArray());
        //}
    }
}