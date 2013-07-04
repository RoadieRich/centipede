using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Centipede
{
    public class CentipedeSerializer : Formatter
    {
        public override object Deserialize(Stream serializationStream)
        {
            if (serializationStream == null)
            {
                throw new ArgumentNullException("serializationStream");
            }

            return _typereaders[ReadTypeCode(serializationStream)](serializationStream);
            
        }

        private static Dictionary<TypeCode, Func<Stream, object>> _typereaders;

        static CentipedeSerializer()
        {
            _typereaders = new Dictionary<TypeCode, Func<Stream, object>>()
                               {
                                   {
                                       TypeCode.Boolean, stream => ReadBool(stream)
                                   },
                                   {
                                       TypeCode.Char, stream => ReadChar(stream)
                                   },
                                   {
                                       TypeCode.SByte, stream => ReadSByte(stream)
                                   },
                                   {
                                       TypeCode.Byte, stream => ReadByte(stream)
                                   },
                                   {
                                       TypeCode.Int16, stream => ReadInt16(stream)
                                   },
                                   {
                                       TypeCode.UInt16, stream => ReadUInt16(stream)
                                   },
                                   {
                                       TypeCode.Int32, stream => ReadInt32(stream)
                                   },
                                   {
                                       TypeCode.UInt32, stream => ReadUInt32(stream)
                                   },
                                   {
                                       TypeCode.Int64, stream => ReadInt64(stream)
                                   },
                                   {
                                       TypeCode.UInt64, stream => ReadUInt64(stream)
                                   },
                                   {
                                       TypeCode.Single, stream => ReadSingle(stream)
                                   },
                                   {
                                       TypeCode.Double, stream => ReadDouble(stream)
                                   },
                                   {
                                       TypeCode.Decimal, stream => ReadDecimal(stream)
                                   },
                                   {
                                       TypeCode.DateTime, serializationStream =>
                                       new DateTime(ReadInt64(serializationStream))
                                   },
                                   {
                                       TypeCode.String, ReadString
                                   },
                                   {
                                       (TypeCode)int.MaxValue, ReadDictionary
                                   },
                                   {
                                       (TypeCode)int.MaxValue - 1, ReadSequence
                                   }
                               };
        }


        private static List<dynamic> ReadSequence(Stream serializationStream)
        {
            TypeCode itemType = ReadTypeCode(serializationStream);
            int length = ReadInt32(serializationStream);
            return Enumerable.Range(0, length).Select(i => _typereaders[itemType](serializationStream)).ToList();
        }

        private static Dictionary<dynamic, dynamic> ReadDictionary(Stream serializationStream)
        {
            Dictionary<dynamic, dynamic> dictionary = new Dictionary<dynamic, dynamic>();
            int length = ReadInt32(serializationStream);
            TypeCode keytype = ReadTypeCode(serializationStream);
            TypeCode valType = ReadTypeCode(serializationStream);

            var keyReader = _typereaders[keytype];
            var valReader = _typereaders[valType];

            for (int i = 0; i < length; i++ )
            {
                dynamic key = keyReader(serializationStream);
                dynamic val = valReader(serializationStream);
                dictionary.Add(key, val);
            }
            
            return dictionary;
        }

        private static TypeCode ReadTypeCode(Stream serializationStream)
        {
            return (TypeCode)ReadInt32(serializationStream);
        }

        private static String ReadString(Stream serializationStream)
        {
            int length = BitConverter.ToInt32(serializationStream.ReadBytes(4), 0);
            return Encoding.Unicode.GetString(serializationStream.ReadBytes(length));
        }

        private static Decimal ReadDecimal(Stream serializationStream)
        {
            int[] bits = new[]
                         {
                             ReadInt32(serializationStream),
                             ReadInt32(serializationStream),
                             ReadInt32(serializationStream),
                             ReadInt32(serializationStream)
                         };
            return new Decimal(bits);
        }

        private static double ReadDouble(Stream serializationStream)
        {
            return BitConverter.ToDouble(serializationStream.ReadBytes(8), 0);
        }

        private static float ReadSingle(Stream serializationStream)
        {
            return BitConverter.ToSingle(serializationStream.ReadBytes(4), 0);
        }

        private static ulong ReadUInt64(Stream serializationStream)
        {
            return BitConverter.ToUInt64(serializationStream.ReadBytes(8), 0);
        }

        private static long ReadInt64(Stream serializationStream)
        {
            return BitConverter.ToInt64(serializationStream.ReadBytes(8), 0);
        }

        private static uint ReadUInt32(Stream serializationStream)
        {
            return BitConverter.ToUInt32(serializationStream.ReadBytes(4), 0);
        }

        private static int ReadInt32(Stream serializationStream)
        {
            return BitConverter.ToInt32(serializationStream.ReadBytes(4), 0);
        }

        private static ushort ReadUInt16(Stream serializationStream)
        {
            return BitConverter.ToUInt16(serializationStream.ReadBytes(2), 0);
        }

        private static short ReadInt16(Stream serializationStream)
        {
            return BitConverter.ToInt16(serializationStream.ReadBytes(2), 0);
        }

        private static byte ReadByte(Stream serializationStream)
        {
            return (byte)serializationStream.ReadByte();
        }

        private static sbyte ReadSByte(Stream serializationStream)
        {
            return (SByte)serializationStream.ReadByte();
        }

        private static char ReadChar(Stream serializationStream)
        {
            return BitConverter.ToChar(serializationStream.ReadBytes(2), 0);
        }

        private static bool ReadBool(Stream serializationStream)
        {
            return BitConverter.ToBoolean(serializationStream.ReadBytes(1), 0);
        }

        public override void Serialize(Stream serializationStream, object graph)
        {
            var typeCode = Type.GetTypeCode(graph.GetType());
            serializationStream.WriteBytes(BitConverter.GetBytes((int)typeCode));

            switch (typeCode)
            {
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.Boolean:
                    serializationStream.WriteBytes(BitConverter.GetBytes((Boolean)graph));
                    return;
                case TypeCode.Char:
                    serializationStream.WriteBytes(BitConverter.GetBytes((Char)graph));
                    return;
                case TypeCode.SByte:
                    serializationStream.WriteBytes(BitConverter.GetBytes((SByte)graph));
                    return;
                case TypeCode.Byte:
                    serializationStream.WriteBytes(BitConverter.GetBytes((Byte)graph));
                    return;
                case TypeCode.Int16:
                    serializationStream.WriteBytes(BitConverter.GetBytes((Int16)graph));
                    return;
                case TypeCode.UInt16:
                    serializationStream.WriteBytes(BitConverter.GetBytes((UInt16)graph));
                    return;
                case TypeCode.Int32:
                    serializationStream.WriteBytes(BitConverter.GetBytes((Int32)graph));
                    return;
                case TypeCode.UInt32:
                    serializationStream.WriteBytes(BitConverter.GetBytes((UInt32)graph));
                    return;
                case TypeCode.Int64:
                    serializationStream.WriteBytes(BitConverter.GetBytes((Int64)graph));
                    return;
                case TypeCode.UInt64:
                    serializationStream.WriteBytes(BitConverter.GetBytes((UInt64)graph));
                    return;
                case TypeCode.Single:
                    serializationStream.WriteBytes(BitConverter.GetBytes((Single)graph));
                    return;
                case TypeCode.Double:
                    serializationStream.WriteBytes(BitConverter.GetBytes((double)graph));
                    return;
                case TypeCode.Decimal:
                    var parts = Decimal.GetBits((decimal)graph);
                    foreach (int i in parts)
                    {
                        serializationStream.WriteBytes(BitConverter.GetBytes(i));
                    }

                    return;
                case TypeCode.DateTime:
                    DateTime dt = (DateTime)graph;
                    serializationStream.WriteBytes(BitConverter.GetBytes(dt.Ticks));
                    return;
                case TypeCode.String:
                    byte[] bytes = Encoding.Unicode.GetBytes((string)graph);
                    serializationStream.WriteBytes(BitConverter.GetBytes(bytes.Length));
                    serializationStream.WriteBytes(bytes);
                    return;
            }

            IEnumerable enumerable = graph as IEnumerable;
            if (enumerable != null)
            {
                IDictionary<dynamic, dynamic> dictionary = enumerable as IDictionary<dynamic, dynamic>;
                if (dictionary != null)
                {
                    Type keyType = dictionary.Keys.First().GetType();
                    Type valType = dictionary.Values.First().GetType();

                    serializationStream.WriteBytes(BitConverter.GetBytes(int.MaxValue));
                    
                }
                else
                {

                    foreach (object item in enumerable)
                    {
                        this.Serialize(serializationStream, item);
                    }
                }
            }
        }

        public override ISurrogateSelector SurrogateSelector { get; set; }
        public override SerializationBinder Binder { get; set; }
        public override StreamingContext Context { get; set; }

        protected override void WriteArray(object obj, string name, Type memberType)
        {
            throw new NotImplementedException();
        }

        protected override void WriteBoolean(bool val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteByte(byte val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteChar(char val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteDateTime(DateTime val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteDecimal(decimal val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteDouble(double val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteInt16(short val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteInt32(int val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteInt64(long val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteObjectRef(object obj, string name, Type memberType)
        {
            throw new NotImplementedException();
        }

        protected override void WriteSByte(sbyte val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteSingle(float val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteTimeSpan(TimeSpan val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteUInt16(ushort val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteUInt32(uint val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteUInt64(ulong val, string name)
        {
            throw new NotImplementedException();
        }

        protected override void WriteValueType(object obj, string name, Type memberType)
        {
            throw new NotImplementedException();
        }
    }
    internal static class SerializerExtension
    {
        public static byte[] GetBytes(this Int64 i)
        {
            return new[] { (byte)(i / 0xffffff), (byte)(i / 0xffff), (byte)(i / 0xff), (byte)(i & 0xff) };
        }

        public static void WriteBytes(this Stream s, IEnumerable<byte> bytes)
        {
            foreach (var b in bytes)
            {
                s.WriteByte(b);
            }

        }
        private static IEnumerable<byte> ReadBytesHelper(Stream s, int n)
        {
            List<byte> bytes = new List<byte>(n);
            for (int i = 0; i < n; i++)
            {
                yield return (byte)s.ReadByte();
            }

        }
        public static byte[] ReadBytes(this Stream s, int n)
        {
            return ReadBytesHelper(s, n).ToArray();
        }

    }
}