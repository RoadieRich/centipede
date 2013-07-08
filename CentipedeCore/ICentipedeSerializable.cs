using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ResharperAnnotations;

namespace Centipede
{

    public interface ICentipedeDataType
    {
        [PublicAPI]
        void Serialize(Stream serializationStream, object obj);

        [PublicAPI]
        object Deserialize(Stream serializationStream);
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ICentipedeDataType<T> : ICentipedeDataType
    {
        void Serialize(Stream serializationStream, T obj);
        new T Deserialize(Stream serializationStream);
    }
}
