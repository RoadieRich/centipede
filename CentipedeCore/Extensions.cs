using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;


namespace CentipedeInterfaces.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Add multiple keys to a dictionary, all with the same value
        /// </summary>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <typeparam name="TValue">Type of the values</typeparam>
        /// <param name="dictionary">Dictionary to add items to</param>
        /// <param name="keys">keys to set the value for</param>
        /// <param name="value">value to set</param>
        public static void AddKeysWithValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
                                                          IEnumerable<TKey> keys,
                                                          TValue value)
        {
            foreach (var key in keys)
            {
                dictionary.Add(key, value);
            }
        }

        public static XmlElement GetFirstElementByName(this XmlElement e, string name)
        {
            XmlNodeList elementsByTagName = e.GetElementsByTagName(name);
            return elementsByTagName[0] as XmlElement;
        }

        public static XmlElement GetFirstElementByName(this XmlDocument e, string name)
        {
            XmlNodeList elementsByTagName = e.GetElementsByTagName(name);
            return elementsByTagName[0] as XmlElement;
        }

        public static String AsText(this MessageLevel e)
        {
            return DisplayTextAttribute.ToDisplayString(e);
        }

        /// <summary>
        /// Returns indexes of items matching predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<int> IndexesWhere<T>(this IEnumerable<T> @this, Func<T, Boolean> predicate)
        {
            return from element in @this.Enumerate()
                   where predicate(element.Value)
                   select element.Key;
        }

        /// <summary>
        /// returns IEnumerable of <see cref="KeyValuePair{int,T}" /> mapping the indices onto the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, T>> Enumerate<T>(this IEnumerable<T> @this)
        {
            return @this.Select((item, index) => new KeyValuePair<int, T>(index, item));
        }

        /// <summary>
        /// Calls Decorate
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TDecorator"></typeparam>
        /// <param name="this"></param>
        /// <param name="decorator"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<TDecorator, TItem>> Decorate<TItem, TDecorator>(
            this IEnumerable<TItem> @this, Func<TItem, TDecorator> decorator)
        {
            return @this.Select(item => Tuple.Create(decorator(item), item));
        }

        public static IEnumerable<Tuple<TDecorator, TItem>> Decorate<TItem, TDecorator>(
            this IEnumerable<TItem> @this, 
            Func<TItem, int, TDecorator> decorator)
        {
            return @this.Select((item, index) => Tuple.Create(decorator(item, index), item));
        }

        public static IEnumerable<TItem> Undecorate<TItem, TDecorated>(this IEnumerable<KeyValuePair<TDecorated, TItem>> @this)
        {
            return @this.Select(pair => pair.Value);
        }
        public static IEnumerable<TItem> Undecorate<TItem, TDecorated>(this IEnumerable<Tuple<TDecorated, TItem>> @this)
        {
            return @this.Select(pair => pair.Item2);
        }

        public static Boolean MoveNext(this IEnumerator @this, int count)
        {
            return Enumerable.Range(0, count).Select(_ => @this.MoveNext()).All(b => b);
        }

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> @this, int start = 0, int stop = -1, int step = 1)
        {
            IEnumerable<T> enumerable = @this;
            if (stop != -1)
            {
                enumerable = enumerable.Take(stop);
            }
            
            IEnumerator<T> it = enumerable.Skip(start).GetEnumerator();

            it.MoveNext(start);
            while (it.MoveNext(step))
            {
                yield return it.Current;
            }
        }

        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return @this.Where(predicate.Not());
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
