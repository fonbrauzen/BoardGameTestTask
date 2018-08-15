using System;
using System.Collections.Generic;

namespace BoardGameTestTaskApp.Utilities
{
    public static class LINQExstensions
    {
        public static int MaxIndex<T>(this IEnumerable<T> sequence, Func<T, int> selector)
        {
            int maxIndex = -1;
            int maxValue = 0; // Immediately overwritten anyway

            int index = 0;
            foreach (T value in sequence)
            {
                if (selector(value).CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = selector(value);
                }
                index++;
            }
            return maxIndex;
        }
    }
}
