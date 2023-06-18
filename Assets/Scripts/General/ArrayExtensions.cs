using System;
using System.Linq;

namespace General
{
    public static class ArrayExtensions
    {
        private static Random rnd = new Random();
        public static T[] Shuffle<T>(this T[] array)
        {
            T[] shuffledArray = (T[])array.Clone();

            int n = shuffledArray.Length;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                T temp = shuffledArray[n];
                shuffledArray[n] = shuffledArray[k];
                shuffledArray[k] = temp;
            }

            return shuffledArray;
        }
    }
}