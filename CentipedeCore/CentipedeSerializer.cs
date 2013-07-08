using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using CentipedeInterfaces;
using ResharperAnnotations;

namespace Centipede
{
    public static class CentipedeSerializer
    {
        public static object Deserialize(Stream serializationStream)
        {
            if (serializationStream == null)
            {
                throw new ArgumentNullException("serializationStream");
            }

            var typeCode = ReadTypeCode(serializationStream);
            return _Typereaders[typeCode](serializationStream);

        }

        private static readonly Dictionary<TypeCode, Func<Stream, dynamic>> _Typereaders;
        private static readonly Dictionary<TypeCode, Action<Stream, dynamic>> _Typewriters;
        private static List<Type> _registeredTypes;

        static CentipedeSerializer()
        {
            _Typereaders = new Dictionary<TypeCode, Func<Stream, dynamic>>
                           {
                               { TypeCode.Boolean, stream => BitConverter.ToBoolean(stream.ReadBytes(1).ToArray(), 0) },
                               { TypeCode.Char, stream => BitConverter.ToChar(stream.ReadBytes(2).ToArray(), 0) },
                               { TypeCode.SByte, stream => (SByte)stream.ReadByte() },
                               { TypeCode.Byte, stream => (byte)stream.ReadByte() },
                               { TypeCode.Int16, stream => BitConverter.ToInt16(stream.ReadBytes(2).ToArray(), 0) },
                               { TypeCode.UInt16, stream => BitConverter.ToUInt16(stream.ReadBytes(2).ToArray(), 0) },
                               { TypeCode.Int32, stream => ReadInt32(stream) },
                               { TypeCode.UInt32, stream => BitConverter.ToUInt32(stream.ReadBytes(4).ToArray(), 0) },
                               { TypeCode.Int64, stream => BitConverter.ToInt64(stream.ReadBytes(8).ToArray(), 0) },
                               { TypeCode.UInt64, stream => BitConverter.ToUInt64(stream.ReadBytes(8).ToArray(), 0) },
                               { TypeCode.Single, stream => BitConverter.ToSingle(stream.ReadBytes(4).ToArray(), 0) },
                               { TypeCode.Double, stream => BitConverter.ToDouble(stream.ReadBytes(8).ToArray(), 0) },
                               { TypeCode.Decimal, ReadDecimal },
                               { TypeCode.DateTime, ReadDateTime },
                               { TypeCode.String, ReadString }
                           };

            _Typewriters = new Dictionary<TypeCode, Action<Stream, dynamic>>
                           {
                               { TypeCode.Decimal,  (stream, o) => stream.WriteBytes((Byte[])Decimal.GetBits(o)) },
                               { TypeCode.Int32,    (stream, o) => WriteInt32(stream, o) },
                               { TypeCode.DateTime, (stream, o) => WriteDateTime(stream, o) },
                               { TypeCode.String,   (stream, o) => WriteString(stream, o) },
                           };

            _Typewriters.AddKeysWithValue(new[]
                                          {
                                              TypeCode.Boolean,
                                              TypeCode.Char,
                                              TypeCode.SByte,
                                              TypeCode.Byte,
                                              TypeCode.Int16,
                                              TypeCode.UInt16,
                                              TypeCode.UInt32,
                                              TypeCode.Int64,
                                              TypeCode.UInt64,
                                              TypeCode.Single,
                                              TypeCode.Double
                                          },
                                          (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o)));
            
            RegisterSerializableType(typeof(ICollection));
            RegisterSerializableType(typeof(IDictionary));
            RegisterSerializableType(typeof(BigInteger));

        }

        private static void WriteDateTime(Stream stream, DateTime o)
        {
            stream.WriteBytes(BitConverter.GetBytes(((DateTime)o).Ticks));
        }

        private static void WriteString(Stream stream, [NotNull] String o)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            byte[] bytes = Encoding.Unicode.GetBytes(o);
            stream.WriteBytes(BitConverter.GetBytes(bytes.Length));
            stream.WriteBytes(bytes);
        }

        private static dynamic ReadDateTime(Stream serializationStream)
        {
            return new DateTime(BitConverter.ToInt64(serializationStream.ReadBytes(8).ToArray(), 0));
        }

        private static dynamic ReadDecimal(Stream stream)
        {
            int[] bits = new[]
                         {
                             ReadInt32(stream), ReadInt32(stream), ReadInt32(stream), ReadInt32(stream)
                         };
            return new Decimal(bits);
        }

        private static String ReadString(Stream serializationStream)
        {
            return Encoding.Unicode.GetString(serializationStream.ReadBytes(ReadInt32(serializationStream)).ToArray());
        }

        private static void WriteSequence<T>(Stream stream, ICollection<T> list)
        {
            var valType = GetTypeCode(list.First());
            var valWriter = _Typewriters[valType];

            WriteLength(stream, list.Count);
            WriteTypeCode(stream, valType);
            
            foreach (T item in list)
            {
                valWriter(stream, item);
            }
        }

        public static void RegisterSerializableType(Type type)
        {
            var serializerMethod = type.GetMethod("Serializer", BindingFlags.Static);
            Action<Stream, dynamic> serializer =
                (stream, o) =>
                serializerMethod.Invoke(null, new object[] { stream, o });

            var deserializerMethod = type.GetMethod("Deserializer", BindingFlags.Static);
            Func<Stream, ICentipedeDataType> deserializer =
                stream =>
                (ICentipedeDataType)
                deserializerMethod.Invoke(null, new object[] { stream });

            RegisterSerializableType(type, null, deserializer);
        }

        public static void RegisterSerializableType(Type type,
                                                    Action<Stream, ICentipedeDataType> serializer,
                                                    Func<Stream, ICentipedeDataType> deserializer)
        {

            Debug.Assert(type.IsSubclassOf(typeof(ICentipedeDataType)));
            int index = ((IList)_registeredTypes).Add(type);

            TypeCode typeCode = (TypeCode)(-1 - index);

            _Typewriters.Add(typeCode, (Action<Stream, dynamic>)serializer);
            _Typereaders.Add(typeCode, deserializer);
        }

        private static void WriteInt32(Stream stream, Int32 i)
        {
            stream.WriteBytes(BitConverter.GetBytes(i));
        }


        private static int ReadInt32(Stream stream)
        {
            return BitConverter.ToInt32(stream.ReadBytes(4).ToArray(), 0);
        }

        private static List<dynamic> ReadSequence(Stream serializationStream)
        {
            TypeCode itemType = ReadTypeCode(serializationStream);
            int length = ReadLength(serializationStream);
            return Enumerable.Range(0, length).Select(i => _Typereaders[itemType](serializationStream)).ToList();
        }

        private static Dictionary<dynamic, dynamic> ReadDictionary(Stream serializationStream)
        {
            Dictionary<dynamic, dynamic> dictionary = new Dictionary<dynamic, dynamic>();
            int length = ReadLength(serializationStream);
            TypeCode keytype = ReadTypeCode(serializationStream);
            TypeCode valType = ReadTypeCode(serializationStream);

            var keyReader = _Typereaders[keytype];
            var valReader = _Typereaders[valType];

            for (int i = 0; i < length; i++)
            {
                dynamic key = keyReader(serializationStream);
                dynamic val = valReader(serializationStream);
                dictionary.Add(key, val);
            }

            return dictionary;
        }

        private static void WriteDictionary<TKey, TVal>(Stream stream, IDictionary<TKey, TVal> dictionary)
        {
            WriteLength(stream, dictionary.Count);
            TypeCode keyType = GetTypeCode(dictionary.Keys.First());
            TypeCode valType = GetTypeCode(dictionary.Values.First());

            var valWriter = _Typewriters[valType];
            var keyWriter = _Typewriters[keyType];

            foreach (KeyValuePair<TKey, TVal> keyValuePair in dictionary)
            {
                keyWriter(stream, keyValuePair.Key);
                valWriter(stream, keyValuePair.Value);
            }
        }

        private static int ReadLength(Stream serializationStream)
        {
            return BitConverter.ToInt32(serializationStream.ReadBytes(4).ToArray(), 0);
        }

        private static void WriteLength(Stream serializationStream, int l)
        {
            serializationStream.WriteBytes(BitConverter.GetBytes(l));
        }

        private static TypeCode ReadTypeCode(Stream serializationStream)
        {
            return (TypeCode)BitConverter.ToInt32(serializationStream.ReadBytes(4).ToArray(), 0);
        }

        private static void WriteTypeCode(Stream stream, TypeCode typeCode)
        {
            stream.WriteBytes(BitConverter.GetBytes((int)typeCode));
        }
        public static void Serialize(Stream serializationStream, object graph)
        {
            var typeCode = GetTypeCode(graph);
            serializationStream.WriteBytes(BitConverter.GetBytes((int)typeCode));
            _Typewriters[typeCode](serializationStream, graph);
            
            serializationStream.Flush();
        }

        private static TypeCode GetTypeCode(object o)
        {
            try
            {
                int index = _registeredTypes.OfType<Type>().Reverse().Enumerate().First(t => t.Value.IsInstanceOfType(o)).Key;
                return (TypeCode)(-1 - index);
            }
            catch (InvalidOperationException)
            {
                return Type.GetTypeCode(o.GetType());
            }
            
        }
    }

    internal static class SerializerStreamExtensions
    {
        public static void WriteBytes(this Stream s, IEnumerable<byte> bytes)
        {
            foreach (var b in bytes)
            {
                s.WriteByte(b);
            }
        }

        public static IEnumerable<byte> ReadBytes(this Stream s, int n)
        {
            return Enumerable.Range(0, n).Select(_ => (Byte)s.ReadByte());
        }
    }
}