using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TestProject1
{
    internal static class TestHelpers
    {
        public static String RandomString(int length, bool alphanumeric = true)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < (length - 1); i++)
            {
                sb.Append(Rand.Choice(Alphanumerics));
            }
            return sb.ToString();
        }

        private static readonly List<Char> Lowercase = new List<Char>("abcdefghijklmnopqrstuvwxyz".ToCharArray());
        private static readonly List<Char> Uppercase = new List<Char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        private static readonly List<Char> Numbers = new List<Char>("1234567890".ToCharArray());

        private static readonly List<Char> Alpha = new List<Char>(Lowercase).Concat(Uppercase).ToList();
        private static readonly List<Char> Alphanumerics = new List<char>(Alpha).Concat(Numbers).ToList();

        private static readonly Random Rand = new Random();

        public static string RandomName(int length = 10)
        {
            int len = Rand.Next(length);
            List<Char> initialChars = new List<Char>(Alpha);
            initialChars.Add('_');
            return String.Format("{0}{1}", Rand.Choice(initialChars), RandomString(length - 1));
        }

        public static int RandomInt
        {
            get
            {
                return Rand.Next();
            }
        }

        public static Double RandomFloat
        {
            get
            {
                return Rand.NextDouble();
            }
        }

        public static int IntBetween(int a, int b)
        {
            return Rand.Next(a, b + 1);
        }

    }

    internal static class ExtensionMethods
    {
        public static T Choice<T>(this Random rand, IList<T> list)
        {
            int len = list.Count;
            int index = rand.Next(len);
            return list[index];
        }

    }
}
