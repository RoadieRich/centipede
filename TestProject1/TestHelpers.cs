using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TestProject1
{
    internal static class TestHelpers
    {
        public enum CharSet
        {
            All,
            Letters,
            Uppercase,
            Lowercase,
            Numbers
        }
        public static String RandomString(int length, int minLength=-1, CharSet charSet = CharSet.All, List<char> chars = null)
        {
            StringBuilder sb = new StringBuilder(length);

            if (chars != null)
            {
                switch (charSet)
                {
                case CharSet.All:
                    chars = Alphanumerics;
                    break;
                case CharSet.Letters:
                    chars = Alpha;
                    break;
                case CharSet.Uppercase:
                    chars = Uppercase;
                    break;
                case CharSet.Lowercase:
                    chars = Lowercase;
                    break;
                case CharSet.Numbers:
                    chars = Numbers;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("charSet");
                }
            }
            int thisLength = minLength < 0 ? Rand.Next(minLength, length) : length;

            for (int i = 0; i < thisLength; i++)
            {
                sb.Append(Rand.Choice(chars));
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
            List<Char> initialChars = new List<Char>(Alpha) { '_' };
            return String.Format("{0}{1}", Rand.Choice(initialChars), RandomString(len - 1));
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

        public static String RandomSentence(int wordCount, int maxWordLength=10, int minWordLength=1)
        {
            List<String> words = new List<string>(wordCount);
            words.Add(string.Format("{0}{1}", Rand.Choice(Uppercase), RandomString(maxWordLength-1, minWordLength, CharSet.Lowercase)));
            for (int i = 0; i < wordCount; i++)
            {
                words.Add(RandomString(maxWordLength, minWordLength, CharSet.Lowercase));
            }
            return string.Format("{0}.", String.Join(" ", words));
        }

    }

    internal static class ExtensionMethods
    {
        /// <summary>
        /// Return a random element from list 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rand">An instance of <seealso cref="T:System.Random"/></param>
        /// <list type=""></list>
        /// <param name="values"></param>
        /// <returns>A random element of <paramref name="values"/></returns>
        /// <remarks>It's random, so potential unorderedness of <paramref name="values"/> is irrelevant.</remarks>
        public static T Choice<T>(this Random rand, IEnumerable<T> values)
        {
            IList<T> list = values as IList<T> ?? values.ToList();
            int len = list.Count();
            int index = rand.Next(len);
            return list[index];
        }

    }
}
