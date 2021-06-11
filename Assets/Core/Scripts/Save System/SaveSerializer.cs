using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Core.SaveSystem
{
    public class SaveSerializer
    {
        public static byte[] Serialize(object file)
        {
            return(ObjectToByteArray(file));
        }

        public static object Deserialize(byte[] file)
        {   
            return(ByteArrayToObject(file));
        }

        [System.ObsoleteAttribute("TODO: Remove Binary Formatter")]
        static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        [System.ObsoleteAttribute("TODO: Remove Binary Formatter")]
        static object ByteArrayToObject(byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                return formatter.Deserialize(stream);
            }
        }

    }
}