using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

namespace Nysa.Collections
{

    public static class Heaps
    {
        /// <summary>
        /// A function that will swap a given heap parent value for
        /// whatever child value is greater, for some definition
        /// of greater. If no swap takes place, the function returns
        /// the parent index, otherwise, the index that was the
        /// greater child is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="parent"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public delegate Int32 SiftOnceFunction<T>(IList<T> list, Int32 parent, Int32 length);

        public static Int32 LeftChild(this Int32 @this) => (@this * 2) + 1;
        public static Int32 RightChild(this Int32 @this) => (@this * 2) + 2;
        public static Int32 Parent(this Int32 @this) => (@this - 1) / 2;

        public static SiftOnceFunction<T> SiftOnce<T>(Func<T, T, Int32> compare)
            => (list, parent, length) =>
            {
                var left = parent.LeftChild();

                if (left < length)
                {
                    var right = parent.RightChild();
                    var child = (right < length)
                                ? compare(list[left], list[right]) < 0 ? left : right
                                : left;

                    if (compare(list[parent], list[child]) > 0)
                    {
                        list.Swap(parent, child);
                        return child;
                    }
                }

                return parent;
            };

        public static void Heapify<T>(this IList<T> @this, Int32 length, SiftOnceFunction<T> sift)
        {
            for (Int32 i = (length - 1); i >= 0; i--)
            {
                var current = i;
                var next    = sift(@this, current, length);

                while (current != next)
                {
                    current = next;
                    next    = sift(@this, current, length);
                }
            }
        }

        public static void Heapify<T>(this IList<T> @this, SiftOnceFunction<T> sift)
            => @this.Heapify(@this.Count, sift);

    }

}
