using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Centipede
{

    public interface ICentipedeSerializable
    {
        void Serialize(Stream serializationStream, object obj);
        object Deserialize(Stream serializationStream);
    }
    public interface ICentipedeSerializable<T> : ICentipedeSerializable
    {
        void Serialize(Stream serializationStream, T obj);
        new T Deserialize(Stream serializationStream);
    }

    
}
