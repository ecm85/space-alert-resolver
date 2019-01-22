using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T2> SelectWithIndex<T1, T2>(this IEnumerable<T1> input, Func<T1, int, T2> selector)
        {
            var index = 0;
            foreach (var value in input)
            {
                yield return selector(value, index);
                index++;
            }
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> input, Action<T, int> action)
        {
            var index = 0;
            foreach (var value in input)
            {
                action(value, index);
                index++;
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            return source.ShuffleIterator(random);
        }

        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random random)
        {
            var buffer = source.ToList();
            while(buffer.Any())
            {
                var nextIndex = random.Next(0, buffer.Count - 1);
                yield return buffer[nextIndex];
                buffer.RemoveAt(nextIndex);
            }
        }
    }
}
