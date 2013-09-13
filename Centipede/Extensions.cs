using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using CentipedeInterfaces;
using ResharperAnnotations;


[assembly: CLSCompliant(true)]

namespace Centipede
{
    public static class Extensions
    {
        public static TList BinaryFind<TList>(this TList[] list, Func<TList, int> comparer)
        {
            if (list.Length < 1)
                return default(TList);

            int pivot = list.Length / 2;
            TList pivotVal = list[pivot];
            TList[] arr = new TList[pivot];
            switch (comparer(pivotVal).Sign())
            {
            case Centipede.Sign.Negative:
                Array.ConstrainedCopy(list, 0, arr, 0, pivot);
                return BinaryFind(arr, comparer);
            case Centipede.Sign.Positive:
                Array.ConstrainedCopy(list, pivot, arr, 0, list.Length - pivot);
                return BinaryFind(arr, comparer);
            default:
                return pivotVal;
            }
        }

        public static string Pluralize(this int n, [NotNull]string word)
        {
            return word.Pluralize(n);
        }

        /// <summary>
        /// returns "correct" pluralization of noun <paramref name="word"/>, given <paramref name="n"/> items
        /// </summary>
        /// <param name="word">Noun to pluralize</param>
        /// <param name="n">Number of items</param>
        /// <returns><paramref name="word"/>, "correctly" pluralized</returns>
        /// <remarks>This is far more complex </remarks>
        public static string Pluralize([NotNull] this string word, int n)
        {
            if (n == 1)
                return word;

            Func<string,string> addS = w => w + "s";

            Func<string, Func<string, string>, Tuple<String, Func<string, string>>> t = Tuple.Create;
            var endings = new List<Tuple<string, Func<string, string>>>
                          {
                              t("ay", addS),
                              t("ey", addS),
                              t("iy", addS),
                              t("oy", addS),
                              t("uy", addS),
                              t("y", w => w.Substring(0, word.Length - 1) + "ies"),
                              t("us", w => w.Substring(0, word.Length - 2) + "i"),
                              t("s", w => w + "es"),
                              t("sheep", w => w)
                          };


            return endings.Where(tup => word.EndsWith(tup.Item1))
                          .Select(tup => tup.Item2(word))       
                          .FirstOrDefault() ?? addS(word);

/*
            if (word.EndsWith("y", StringComparison.InvariantCultureIgnoreCase))
            {
                return word.Substring(0, word.Length - 1) + "ies";
            }
            if (word.EndsWith("s"))
            {
                return word + "es";
            }
            return word + "s";
*/
        }

        public static XmlElement CreateChildElement(this XmlNode node, string name)
        {
            var doc = node as XmlDocument ?? node.OwnerDocument;
            
            XmlElement element = doc.CreateElement(name);

            node.AppendChild(element);

            return element;

        }


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
                list.Add(item);
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

        public static T Peek<T>(this IEnumerable<T> list)
        {
            return list.Last();
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

        public static int Clamp(this int i, int min, int max)
        {
            return Math.Min(Math.Max(i, min), max);
        }

        public static Sign Sign<T>(this IComparable<T> i)
        {
            return (Sign)(i.CompareTo(default(T))).Clamp(-1, 1);
        }
    }

    public class MathStack<T>
    {
        private readonly Stack<T> _stack;

        public MathStack(Stack<T> stack)
        {
            _stack = stack;
        }

        public void ApplyOperator(Func<T, T> @operator)
        {
            T a = Pop();
            Push(@operator(a));
        }

        public void ApplyOperator(Func<T, T, T> @operator)
        {
            T a = Pop();
            T b = Pop();
            Push(@operator(a, b));
        }

        public void ApplyOperator(Func<T, T, T, T> @operator)
        {
            T a = Pop();
            T b = Pop();
            T c = Pop();
            Push(@operator(a, b, c));
        }

        public void ApplyOperator(Func<T[], T> @operator, int argumentCount)
        {
            T[] args = new T[argumentCount];
            for (int i = 0; i < argumentCount; i++)
            {
                args[i] = Pop();
            }
            Push(@operator(args));
        }

        public void Push(T value)
        {
            _stack.Push(value);
        }

        public T Pop()
        {
            return _stack.Pop();
        }

        public T Peek()
        {
            return _stack.Peek();
        }
    }

    public static class MathematicalOperator
    {
        public static Func<dynamic, dynamic> Sqrt = a => (int)Math.Sqrt(a);
        public static Func<dynamic, dynamic> Negate = a => -a;
        public static Func<dynamic, dynamic> Sign = a => Math.Sign(a);

        public static Func<dynamic, dynamic, dynamic> Plus = (a, b) => a + b;
        public static Func<dynamic, dynamic, dynamic> Subtract = (a, b) => a - b;
        public static Func<dynamic, dynamic, dynamic> Multiply = (a, b) => a * b;
        public static Func<dynamic, dynamic, dynamic> DivideBy = (a, b) => a / b;
        public static Func<dynamic, dynamic, dynamic> DivideInto = (a, b) => b / a;
        public static Func<dynamic, dynamic, dynamic> Pow = (a, b) => (int)Math.Pow(a, b);

        public static Func<dynamic[], dynamic> Sum = arr => arr.Aggregate(0, (current, i) => current + i);
    }

    public enum Sign
    {
        Negative = -1,
        Zero = 0,
        Positive = 1
    }

}