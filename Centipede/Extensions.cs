using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CentipedeInterfaces;


[assembly: CLSCompliant(true)]

namespace Centipede
{
    [ResharperAnnotations.UsedImplicitly(ResharperAnnotations.ImplicitUseTargetFlags.WithMembers)]
    public static class Extensions
    {
        public static void RemoveRange(this IList list, IEnumerable items)
        {
            foreach (var item in items)
            {
                list.Remove(item);
            }
        }

        public static bool RemoveRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            return items.All(list.Remove);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        public static void AddRange(this IList list, IEnumerable items)
        {
            foreach (var item in items)
            {
                list.Remove(item);
            }
        }

        public static bool Pop<T>(this IList<T> list, T value)
        {
            return list.Contains(value) && list.Remove(value);
        }

        public static T Pop<T>(this IList<T> list)
        {
            T value = list.Last();
            list.RemoveAt(list.Count - 1);
            return value;
        }

        public static void Push<T>(this IList<T> list, T value)
        {
            list.Add(value);
        }

        public static T Peek<T>(this IList<T> list)
        {
            return list.Last();
        }

        private class MyStruct<T> : IComparable<MyStruct<T>>, IEquatable<MyStruct<T>>
                where T : IComparable, IEquatable<T>
        {
            private readonly T _item;

            public T Item
            {
                get
                {
                    return _item;
                }
            }

            public int Index
            {
                get;
                set;
            }

            public MyStruct(T item)
            {
                _item = item;
            }


            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <returns>
            /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public int CompareTo(MyStruct<T> other)
            {
                return ((IComparable<T>)Item).CompareTo(other.Item);
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(MyStruct<T> other)
            {
                return Item.CompareTo(other.Item) == 0;
            }
        }

        public static MessageLevel AddFlag(this MessageLevel e, MessageLevel flag)
        {
            return e | flag;
        }

        public static MessageLevel RemoveFlag(this MessageLevel e, MessageLevel flag)
        {
            return e & ~flag;
        }

        public static MessageLevel ToggleFlag(this MessageLevel e, MessageLevel flag)
        {
            return e ^ flag;
        }

        public static MessageLevel SetFlag(this MessageLevel e, MessageLevel flag, bool value)
        {
            return value ? e.AddFlag(flag) : e.RemoveFlag(flag);
        }

        public static void MoveUp(this DataRow row)
        {
            row.Move(1);
        }

        public static void MoveDown(this DataRow row)
        {
            row.Move(-1);
        }

        public static void Move(this DataRow row, int delta)
        {
            DataTable table = row.Table;

            int oldIndex = table.Rows.IndexOf(row);
            if (oldIndex <= 0)
            {
                return;
            }
            table.Rows.Remove(row);
            int newIndex = oldIndex + delta;
            table.Rows.InsertAt(row, newIndex.Clamp(0, table.Rows.Count));

        }

        private static int Clamp(this int i, int min, int max)
        {
            return Math.Min(Math.Max(i, min), max);
        }
    }

    /// <summary>
    /// Double Ended Queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Deque<T> : List<T>
    {
        public void PushFront(T item)
        {
            Insert(0, item);
        }

        public void PushBack(T item)
        {
            Add(item);
        }

        public T PeekFront()
        {
            return this.First();
        }

        public T PeekBack()
        {
            return this.Last();
        }

        public T PopFront()
        {
            T value = this.First();
            RemoveAt(0);
            return value;
        }

        public T PopBack()
        {
            T value = this.Last();
            RemoveAt(Count - 1);
            return value;
        }
    }
}