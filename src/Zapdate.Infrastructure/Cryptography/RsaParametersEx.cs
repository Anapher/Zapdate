using System;
using System.IO;
using System.Security.Cryptography;

namespace Zapdate.Infrastructure.Cryptography
{
    [Serializable]
    public class RSAParametersEx
    {
        public byte[]? D;
        public byte[]? DP;
        public byte[]? DQ;
        public byte[] Exponent;
        public byte[]? InverseQ;
        public byte[] Modulus;
        public byte[]? P;
        public byte[]? Q;

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private RSAParametersEx()
        {
        }
#pragma warning restore CS8618

        public RSAParametersEx(byte[] modulus, byte[] exponent)
        {
            Modulus = modulus;
            Exponent = exponent;
        }

        public RSAParametersEx(RSAParameters rsaParameters)
        {
            Exponent = rsaParameters.Exponent;
            Modulus = rsaParameters.Modulus;
            P = rsaParameters.P;
            Q = rsaParameters.Q;
            DP = rsaParameters.DP;
            DQ = rsaParameters.DQ;
            InverseQ = rsaParameters.InverseQ;
            D = rsaParameters.D;
        }

        public RSAParameters ToRSAParameters()
        {
            return new RSAParameters
            {
                Exponent = Exponent,
                Modulus = Modulus,
                P = P,
                Q = Q,
                DP = DP,
                DQ = DQ,
                InverseQ = InverseQ,
                D = D
            };
        }

        public RSAParametersEx ToPublicKey()
        {
            return new RSAParametersEx { Modulus = Modulus, Exponent = Exponent };
        }

        public static implicit operator RSAParametersEx(RSAParameters rsaParameters)
        {
            return new RSAParametersEx(rsaParameters);
        }

        public static implicit operator RSAParameters(RSAParametersEx rsaParameters)
        {
            return rsaParameters.ToRSAParameters();
        }

        public override string ToString()
        {
            //https://stackoverflow.com/questions/23734792/c-sharp-export-private-public-rsa-key-from-rsacryptoserviceprovider-to-pem-strin
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE

                if (D == null || P == null || Q == null || DP == null || DQ == null || InverseQ == null)
                    throw new InvalidOperationException("The key does not contain all parameters.");

                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, Modulus);
                    EncodeIntegerBigEndian(innerWriter, Exponent);
                    EncodeIntegerBigEndian(innerWriter, D);
                    EncodeIntegerBigEndian(innerWriter, P);
                    EncodeIntegerBigEndian(innerWriter, Q);
                    EncodeIntegerBigEndian(innerWriter, DP);
                    EncodeIntegerBigEndian(innerWriter, DQ);
                    EncodeIntegerBigEndian(innerWriter, InverseQ);
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                return Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }

        public static RSAParametersEx? Parse(string value)
        {
            return Parse(Convert.FromBase64String(value));
        }

        public static RSAParametersEx? Parse(byte[] value)
        {
            //https://stackoverflow.com/questions/1162504/decrypting-with-private-key-from-pem-file-in-c-sharp-with-net-crypto-library/1162519#1162519
            using (var memoryStream = new MemoryStream(value))
            using (var reader = new BinaryReader(memoryStream))
            {
                var result = new RSAParametersEx();

                var twoBytes = reader.ReadUInt16();
                if (twoBytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    reader.ReadByte();    //advance 1 byte
                else if (twoBytes == 0x8230)
                    reader.ReadInt16(); //advance 2 bytes
                else
                    throw new ArgumentException();

                twoBytes = reader.ReadUInt16();
                if (twoBytes != 0x0102) //version number
                    return null;

                if (reader.ReadByte() != 0x00)
                    return null;

                var size = GetIntegerSize(reader);
                result.Modulus = reader.ReadBytes(size);

                size = GetIntegerSize(reader);
                result.Exponent = reader.ReadBytes(size);

                size = GetIntegerSize(reader);
                result.D = reader.ReadBytes(size);

                size = GetIntegerSize(reader);
                result.P = reader.ReadBytes(size);

                size = GetIntegerSize(reader);
                result.Q = reader.ReadBytes(size);

                size = GetIntegerSize(reader);
                result.DP = reader.ReadBytes(size);

                size = GetIntegerSize(reader);
                result.DQ = reader.ReadBytes(size);

                size = GetIntegerSize(reader);
                result.InverseQ = reader.ReadBytes(size);

                return result;
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            //https://stackoverflow.com/questions/1162504/decrypting-with-private-key-from-pem-file-in-c-sharp-with-net-crypto-library/1162519#1162519
            byte bt;
            int count;
            bt = binr.ReadByte();
            if (bt != 0x02)     //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
            if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {   //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);       //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        //https://stackoverflow.com/questions/23734792/c-sharp-export-private-public-rsa-key-from-rsacryptoserviceprovider-to-pem-strin
        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        //https://stackoverflow.com/questions/23734792/c-sharp-export-private-public-rsa-key-from-rsacryptoserviceprovider-to-pem-strin
        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }
    }
}