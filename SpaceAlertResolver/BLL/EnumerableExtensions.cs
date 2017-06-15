using System;
using System.Collections.Generic;

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
    }
}
