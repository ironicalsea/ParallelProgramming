using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParallelProgramming
{
    public class MapReducePattern
    {
        public IEnumerable<WordStatistics> GetWordStatistics(string dir, string filter)
        {
            return
                MapReduce(
                    Directory.EnumerateFiles(dir, filter, SearchOption.AllDirectories).AsParallel(),
                    path => File
                            .ReadLines(path)
                            .SelectMany(line => line.Split(_delimiters))
                            .Where(word => !string.IsNullOrWhiteSpace(word)),
                    word => word.ToLower(),
                    group => new[]
                        {
                            new WordStatistics
                            {
                                Word = group.Key,
                                Count = group.Count()
                            }
                        })
                    .ToList();
        }

        private static ParallelQuery<TResult> MapReduce<TSource, TMapped, TKey, TResult>(
             ParallelQuery<TSource> source,
             Func<TSource, IEnumerable<TMapped>> map,
             Func<TMapped, TKey> keySelector,
             Func<IGrouping<TKey, TMapped>, IEnumerable<TResult>> reduce)
        {
            return source
                    .SelectMany(map)
                    .GroupBy(keySelector)
                    .SelectMany(reduce);
        }

        private static readonly char[] _delimiters =
            Enumerable
            .Range(0, 256)
            .Select(i => (char)i).Where(c => Char.IsWhiteSpace(c) || Char.IsPunctuation(c)).ToArray();

        public class WordStatistics
        {
            public string Word { get; set; }
            public int Count { get; set; }
        }
    }
}