using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace CentipedeInterfaces
{
    public static class Extensions
    {
        public static String AsText(this MessageLevel e)
        {
            return DisplayTextAttribute.ToDisplayString(e);
        }

        public static IEnumerable<int> IndexesWhere<T>(this IEnumerable<T> @this, Func<T, Boolean> predicate)
        {
            return from element in @this.Enumerate()
                   where predicate(element.Value)
                   select element.Key;
        }

        public static IEnumerable<KeyValuePair<int, T>> Enumerate<T>(this IEnumerable<T> @this)
        {
            return @this.Select((o, i) => new KeyValuePair<int, T>(i, o));
        }

        public static Boolean MoveNext(this IEnumerator @this, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!@this.MoveNext())
                    return false;
            }
            return true;
        }

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> @this, int start = 0, int stop = -1, int step = 1)
        {
            stop = stop != -1 ? stop : @this.Count();
            var it = @this.Take(stop).Skip(start).GetEnumerator();

            it.MoveNext(start);
            while (it.MoveNext(step))
            {
                yield return it.Current;
            }
        }

        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return @this.Where(i => !predicate(i));
        }

        public static Func<bool> Not(this Func<bool> function)
        {
            return () => !function();
        }

        public static Func<T1, bool> Not<T1>(this Func<T1, bool> function)
        {
            return arg => !function(arg);
        }

        public static Func<T1, T2, bool> Not<T1, T2>(this Func<T1, T2, bool> function)
        {
            return (a, b) => !function(a, b);
        }

        public static Func<T1, T2, T3, bool> Not<T1, T2, T3>(this Func<T1, T2, T3, bool> function)
        {
            return (a, b, c) => !function(a, b, c);
        }

        public static Func<T1, TOut> Chain<T1, T2, TOut>(this Func<T1, T2> funcA, Func<T2, TOut> funcB)
        {
            return arg => funcB(funcA(arg));
        }

    }

    public static  class Operator
    {
        public static Func<bool> Not(Func<bool> function)
        {
            return () => !function();
        }

        public static Func<T1, bool> Not<T1>(Func<T1, bool> function)
        {
            return arg => !function(arg);
        }

        public static Func<T1, T2, bool> Not<T1, T2>(Func<T1, T2, bool> function)
        {
            return (a, b) => !function(a, b);
        }

        public static Func<T1, T2, T3, bool> Not<T1, T2, T3>(Func<T1, T2, T3, bool> function)
        {
            return (a, b, c) => !function(a, b, c);
        }
    }

}
