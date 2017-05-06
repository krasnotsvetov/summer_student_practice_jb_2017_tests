using PEExtractor.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Common.Extension
{
    public static class BinaryReaderExtension
    {

        /// <summary>
        /// Read POD structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ReadStruct<T>(this BinaryReader reader) where T : struct
        {
            var handle = GCHandle.Alloc(reader.ReadBytes(Marshal.SizeOf<T>()), GCHandleType.Pinned);
            T rv = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            handle.Free();
            return rv;
        }


        /// <summary>
        /// Check signature. If it not equal, the Signature exception will thrown
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="signature"></param>
        public static void CheckSignature(this BinaryReader reader, string signature)
        {
            for (int i = 0; i < signature.Length; i++)
            {
                if (reader.ReadChar() != signature[i])
                {
                    throw new SignatureException($"Signature is incorrect, expected : {signature}");
                }
            }
        }

        /// <summary>
        /// Change stream position
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="position"></param>
        public static void SetStreamPosition(this BinaryReader reader, long position)
        {
            reader.BaseStream.Position = position;
        }

     
    }
}
    