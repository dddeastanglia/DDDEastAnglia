using System;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public static class ListExtensions
    {
        /// <summary>
        /// Randomly shuffles the provided list.
        /// All possible orders are equally likely.
        /// Based on the Fisher-Yates shuffle. http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        /// </summary>
        public static void RandomShuffle<T>(this IList<T> data)
        {
            Random randomGen = new Random();
            for (int i = data.Count - 1; i >= 1; i--)
            {
                // Randomly select items to move to the end of the list.
                // After each iteration, the item just moved to the end
                // will no longer be in the set of items that are
                // available to swap.
                int swapIndex = randomGen.Next(0, i + 1);

                T swap = data[i];
                data[i] = data[swapIndex];
                data[swapIndex] = swap;
            }
        }
    }
}