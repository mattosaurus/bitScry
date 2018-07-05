using bitScry.Function.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace bitScry.Function.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            T[] elements = source.ToArray();

            for (int i = elements.Length - 1; i >= 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        public static IEnumerable<T> GetRandomElement<T>(this IEnumerable<T> source, int length = 1)
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                T[] elements = source.ToArray();
                List<T> result = new List<T>();

                for (int i = 0; i < length; i++)
                {
                    int randomNumber = Common.GetRandomInteger(0, elements.Count());
                    result.Add(elements[randomNumber]);
                }

                return result;
            }
        }
    }
}
