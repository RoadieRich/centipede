using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

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
                               {
                                   TypeCode.Boolean, stream => BitConverter.ToBoolean(stream.ReadBytes(1).ToArray(), 0)
                               },
                               {
                                   TypeCode.Char, stream => BitConverter.ToChar(stream.ReadBytes(2).ToArray(), 0)
                               },
                               {
                                   TypeCode.SByte, stream => (SByte)stream.ReadByte()
                               },
                               {
                                   TypeCode.Byte, stream => (byte)stream.ReadByte()
                               },
                               {
                                   TypeCode.Int16, stream => BitConverter.ToInt16(stream.ReadBytes(2).ToArray(), 0)
                               },
                               {
                                   TypeCode.UInt16, stream => BitConverter.ToUInt16(stream.ReadBytes(2).ToArray(), 0)
                               },
                               {
                                   TypeCode.Int32, stream => ReadInt32(stream)
                               },
                               {
                                   TypeCode.UInt32, stream => BitConverter.ToUInt32(stream.ReadBytes(4).ToArray(), 0)
                               },
                               {
                                   TypeCode.Int64, stream => BitConverter.ToInt64(stream.ReadBytes(8).ToArray(), 0)
                               },
                               {
                                   TypeCode.UInt64, stream => BitConverter.ToUInt64(stream.ReadBytes(8).ToArray(), 0)
                               },
                               {
                                   TypeCode.Single, stream => BitConverter.ToSingle(stream.ReadBytes(4).ToArray(), 0)
                               },
                               {
                                   TypeCode.Double, stream => BitConverter.ToDouble(stream.ReadBytes(8).ToArray(), 0)
                               },
                               {
                                   TypeCode.Decimal, stream =>
                                                         {
                                                             int[] bits = new[]
                                                                          {
                                                                              ReadInt32(stream),
                                                                              ReadInt32(stream),
                                                                              ReadInt32(stream),
                                                                              ReadInt32(stream)
                                                                          };
                                                             return new Decimal(bits);
                                                         }
                               },
                               {
                                   TypeCode.DateTime, serializationStream =>
                                                      new DateTime(BitConverter.ToInt64(
                                                          serializationStream.ReadBytes(8).ToArray(), 0))
                               },
                               {
                                   TypeCode.String,
                                   serializationStream =>
                                   Encoding.Unicode.GetString(
                                       serializationStream.ReadBytes(ReadInt32(serializationStream)).ToArray())
                               },
                               {
                                   (TypeCode)(-1), ReadDictionary
                               },
                               {
                                   (TypeCode)(-2), ReadSequence
                               },
                               {
                                   (TypeCode)(-3), s => new BigInteger(s.ReadBytes(ReadLength(s)).ToArray())
                               }
                           };

            _Typewriters = new Dictionary<TypeCode, Action<Stream, dynamic>>
                           {

                               {
                                   TypeCode.Boolean, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Char, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.SByte, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Byte, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Int16, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.UInt16, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Int32, (stream, o) => WriteInt32(stream, o)
                               },
                               {
                                   TypeCode.UInt32, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Int64, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.UInt64, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Single, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Double, (stream, o) => stream.WriteBytes((Byte[])BitConverter.GetBytes(o))
                               },
                               {
                                   TypeCode.Decimal, (stream, o) => stream.WriteBytes((Byte[])Decimal.GetBits(o))
                               },
                               {
                                   TypeCode.DateTime,
                                   (stream, o) => stream.WriteBytes(BitConverter.GetBytes(((DateTime)o).Ticks))
                               },
                               {
                                   TypeCode.String, (stream, o) =>
                                                        {
                                                            byte[] bytes = Encoding.Unicode.GetBytes(o);
                                                            stream.WriteBytes(BitConverter.GetBytes(bytes.Length));
                                                            stream.WriteBytes(bytes);
                                                        }
                               },
                               {
                                   (TypeCode)(-1), (stream, o) => WriteDictionary(stream, o)
                               },
                               {
                                   (TypeCode)(-2), (stream, o) => WriteSequence(stream, o)
                               },
                               { (TypeCode)(-3), (stream, o) =>
                                                     {
                                                         var bytes = ((BigInteger)o).ToByteArray();
                                                         
                                                         WriteLength(stream, bytes.Length);
                                                         stream.WriteBytes(bytes);
                                                     }}

                           };
            _registeredTypes = new List<Type>
                              {
                                  typeof(IDictionary),
                                  typeof(ICollection),
                                  typeof(BigInteger)
                              };
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

        public static void RegisterSerializableType(Type type,
                                                       Action<Stream, dynamic> serializer,
                                                       Func<Stream, dynamic> deserializer)
        {
            int index = ((IList)_registeredTypes).Add(type);

            TypeCode typeCode = (TypeCode)(-1 - index);
            
            _Typewriters.Add(typeCode, serializer);
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
            var index = _registeredTypes.FindIndex(t => t.IsInstanceOfType(o));
            return index != -1 ? (TypeCode)(-1 - index) : Type.GetTypeCode(o.GetType());
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