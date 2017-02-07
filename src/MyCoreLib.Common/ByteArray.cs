
namespace MyCoreLib.Common
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Provides some helper methods for an array of bytes.
    /// </summary>
    public static partial class ByteArray
    {
        /// <summary>
        /// Generate a url safe base string.
        /// </summary>
        public static string ToSafeBase64String(this byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (buffer.Length == 0)
                return string.Empty;

            int totalLength = buffer.Length;
            int numGroups = buffer.Length / 3;
            int lastGroup = totalLength - numGroups * 3;

            StringBuilder sb = new StringBuilder(4 * (totalLength + 2) / 3);

            unsafe
            {
                fixed (char* ptrBase64Chars = s_customBase64Chars)
                {
                    int pos = 0;
                    for (int i = 0; i < numGroups; i++)
                    {
                        int b0 = buffer[pos++];
                        int b1 = buffer[pos++];
                        int b2 = buffer[pos++];
                        sb.Append(ptrBase64Chars[b0 >> 2]);
                        sb.Append(ptrBase64Chars[/* the last 2 bits of b0 */((b0 & 3) << 4) | /* the first 4 bits of b1 */(b1 >> 4)]);
                        sb.Append(ptrBase64Chars[/* the last 4 bits of b1 */((b1 & 0x0f) << 2) | /* the first 2 bits of b2 */(b2 >> 6)]);
                        sb.Append(ptrBase64Chars[/* the last 6 bits of b2 */b2 & 0x3f]);
                    }

                    if (lastGroup > 0)
                    {
                        int b0 = buffer[pos++];
                        sb.Append(ptrBase64Chars[b0 >> 2]);

                        if (lastGroup == 1)
                        {
                            sb.Append(ptrBase64Chars[/* only keep the first 2 bits*/(b0 & 3) << 4]);
                        }
                        else
                        {

                            int b1 = buffer[pos++];
                            sb.Append(ptrBase64Chars[/* the last 2 bits of b0 */((b0 & 3) << 4) | /* the first 4 bits of b1 */(b1 >> 4)]);
                            sb.Append(ptrBase64Chars[/* the last 4 bits of b1 */((b1 & 0x0f) << 2)]);
                        }
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Try to parse the specified base-64 string.
        /// </summary>
        public static bool TryFromSafeBase64String(this string s, out byte[] buffer)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            if (s.Length == 0)
            {
                buffer = new byte[0];
                return true;
            }

            buffer = null;

            // Determines the final length of decoded buffer.
            int length = s.Length;
            int numGroups = length >> 2;

            // int numPadding = (((length + 3) >> 2) << 2) - length;
            int missingBytesInLastGroup;
            switch (length & 3)
            {
                case 3: missingBytesInLastGroup = 2; break;
                case 2: missingBytesInLastGroup = 1; break;
                case 0: missingBytesInLastGroup = 0; break;
                default: return false; // This is impossible.
            }

            int missingCharsInLastGroup = (((length + 3) >> 2) << 2) - length;
            byte[] result = new byte[3 * numGroups + missingBytesInLastGroup];

            unsafe
            {
                fixed (byte* fixedByteMapPtr = CustomBase64Map)
                {
                    fixed (char* fixedStringPtr = s)
                    {
                        char* stringPtr = fixedStringPtr;

                        int bytePos = 0;
                        for (int i = 0; i < numGroups; i++)
                        {
                            int ch0 = fixedByteMapPtr[*stringPtr++];
                            int ch1 = fixedByteMapPtr[*stringPtr++];
                            int ch2 = fixedByteMapPtr[*stringPtr++];
                            int ch3 = fixedByteMapPtr[*stringPtr++];

                            // Not allow invalid characters
                            if (ch0 == 0xFF || ch1 == 0xFF || ch2 == 0xFF || ch3 == 0xFF)
                                return false;

                            unchecked
                            {
                                result[bytePos++] = (byte)((ch0 << 2) | (ch1 >> 4));
                                result[bytePos++] = (byte)((ch1 << 4) | (ch2 >> 2));
                                result[bytePos++] = (byte)((ch2 << 6) | ch3);
                            }
                        }

                        if (missingCharsInLastGroup != 0)
                        {
                            // There will be at least 2 chars remaining because 1 byte could product 2 chars in the base64 string.
                            int ch0 = fixedByteMapPtr[*stringPtr++];
                            int ch1 = fixedByteMapPtr[*stringPtr++];

                            if (ch0 == 0xFF || ch1 == 0xFF)
                                return false;

                            unchecked
                            {
                                result[bytePos++] = (byte)((ch0 << 2) | (ch1 >> 4));

                                // If only 1 char is missing, that is, 3 chars are in the group, then we need to decode the second byte.
                                // Because 2 bytes could produce 3 chars as following.
                                //
                                // Base64: |-----c1----|-----c2----|---c3---
                                // --------|---------------|---------------|
                                // Bit:    |0 1 2 3 4 5 6 7|0 1 2 3 4 5 6 7|
                                //
                                if (missingCharsInLastGroup == 1)
                                {
                                    int ch2 = fixedByteMapPtr[*stringPtr++];
                                    if (ch2 == 0xFF)
                                        return false;

                                    result[bytePos++] = (byte)((ch1 << 4) | (ch2 >> 2));
                                }
                            }
                        }
                    }
                }
            }

            buffer = result;
            return true;
        }

        /// <summary>
        /// Returns a byte array from a string representing a hexidecimal number.
        /// </summary>
        /// <param name="hexString">
        /// The string containing a valid hexidecimal number.
        /// </param>
        /// <param name="throwOnError">
        /// A value indicating whether throws an exception if an invalid hexidecimal string.
        /// </param>
        /// <returns>
        /// The byte array representing the hexidecimal.
        /// Or null, if <paramref name="hexString"/> is invalid.
        /// </returns>
        public static byte[] FromHexString(this string hexString, bool throwOnError)
        {
            if (hexString == null || hexString.Length == 0)
            {
                return new byte[0];
            }

            if ((hexString.Length & 1) == 1)
            {
                // Invalid hex string
                //
                if (throwOnError)
                {
                    throw new ArgumentException("Invalid hex string.", "hexString");
                }
                return null;
            }
            if (hexString[0] == '0' &&
                (hexString[1] == 'x' || hexString[1] == 'X'))
            {
                // Removes the "0x" prefix
                //
                hexString = hexString.Substring(2);
            }

            byte[] hexValuesBuffer = HexCharToIntegerMapping;
            byte[] byteArray;
            int byteArrayLength = hexString.Length / 2;
            int hexStringCharPos = 0;
            int byteArrayPos = 0;

            byteArray = new byte[hexString.Length / 2];

            while (byteArrayPos < byteArrayLength)
            {
                int higherFourBits;
                int lowerFourBits;

                // Tries to the higher four bits in the byte.
                //
                try
                {
                    higherFourBits = hexValuesBuffer[hexString[hexStringCharPos++]];
                }
                catch (ArgumentNullException e)
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException("Invalid hex string.", "hexString", e);
                    }
                    return null;
                }
                catch (ArgumentException e)
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException("Invalid hex string.", "hexString", e);
                    }
                    return null;
                }
                catch (IndexOutOfRangeException e)
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException("Invalid hex string.", "hexString", e);
                    }
                    return null;
                }

                // Tries to the lower four bits in the byte.
                //
                try
                {
                    lowerFourBits = hexValuesBuffer[hexString[hexStringCharPos++]];
                }
                catch (ArgumentNullException e)
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException("Invalid hex string.", "hexString", e);
                    }
                    return null;
                }
                catch (ArgumentException e)
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException("Invalid hex string.", "hexString", e);
                    }
                    return null;
                }
                catch (IndexOutOfRangeException e)
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException("Invalid hex string.", "hexString", e);
                    }
                    return null;
                }

                byteArray[byteArrayPos++] = (byte)((higherFourBits << 4) | lowerFourBits);
            }

            return byteArray;
        }

        /// <summary>
        /// Converts a byte array to a binary string.
        /// </summary>
        public static string ToHexString(this byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            return ToHexString(buffer, buffer.Length);
        }

        /// <summary>
        /// Converts a byte array to a binary string.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is greater than the length of <paramref name="buffer"/>.
        /// </exception>
        public static string ToHexString(this byte[] buffer, int length)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (buffer.Length == 0 || length == 0)
            {
                return string.Empty;
            }
            if (length > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            char[] hexChars = new char[length * 2];
            int hexCharsPos = 0;

            unsafe
            {
                fixed (char* ptrCharMapping = s_integerToHexCharMapping)
                {
                    // Converts the byte array to a string
                    //
                    for (int i = 0; i < length; i++)
                    {
                        byte b = buffer[i];

                        hexChars[hexCharsPos++] = ptrCharMapping[(b & 0xF0) >> 4];
                        hexChars[hexCharsPos++] = ptrCharMapping[b & 0x0F];
                    }
                }
            }

            return new string(hexChars);
        }

        /// <summary>
        /// Determine if two byte arrays are equal.
        /// </summary>
        /// <param name="byteArray1">
        /// The first byte array to compare.
        /// </param>
        /// <param name="byteArray2">
        /// The byte array to compare to the first.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the two byte arrays are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool CompareBytes(this byte[] byteArray1, byte[] byteArray2)
        {
            return CompareBytes(byteArray1, 0, byteArray2, 0, byteArray1.Length);
        }

        public static bool CompareBytes(this byte[] byteArray1, int offset1, byte[] byteArray2, int offset2, int count)
        {
            if (offset1 < 0) throw new ArgumentOutOfRangeException("offset1");
            if (offset2 < 0) throw new ArgumentOutOfRangeException("offset2");
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            if (byteArray1 == null || byteArray2 == null)
            {
                return false;
            }
            if (offset1 + count > byteArray1.Length || offset2 + count > byteArray2.Length)
                return false;

            bool result = true;
            unsafe
            {
                fixed (byte* fixedPtr1 = byteArray1)
                {
                    fixed (byte* fixedPtr2 = byteArray2)
                    {
                        byte* ptr1 = fixedPtr1 + offset1;
                        byte* ptr2 = fixedPtr2 + offset2;

                        for (int i = 0; i < count; i++)
                        {
                            if ((*ptr1++) != (*ptr2++))
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Combines two byte arrays into one.
        /// </summary>
        /// <param name="buffer1">The prefixed bytes.</param>
        /// <param name="buffer2">The suffixed bytes.</param>
        /// <returns>The combined byte arrays.</returns>
        public static byte[] CombineBytes(this byte[] buffer1, byte[] buffer2)
        {
            byte[] combinedBytes = new byte[buffer1.Length + buffer2.Length];
            Buffer.BlockCopy(buffer1, 0, combinedBytes, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, combinedBytes, buffer1.Length, buffer2.Length);

            return combinedBytes;
        }

        /// <summary>
        /// Fills <paramref name="bytes"/> zeros.
        /// </summary>
        /// <param name="bytes">
        /// The byte array to fill.
        /// </param>
        public static void ZeroOutBytes(byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                for (int index = 0; index < bytes.Length; index++)
                {
                    bytes[index] = 0;
                }
            }
        }

        /// <summary>
        /// Creates a random set of bytes.
        /// </summary>
        public static byte[] GetRandomBytes(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            if (size == 0)
            {
                return new byte[0];
            }
            else
            {
                byte[] randomBytes = new byte[size];
                GetRandomBytes(randomBytes);

                return randomBytes;
            }
        }

        /// <summary>
        /// Fills a byte array with a cryptographically strong random set of bytes.
        /// </summary>
        /// <param name="bytes">The byte array to fill.</param>
        public static void GetRandomBytes(byte[] bytes)
        {
            if (!s_rngIntialized)
            {
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                if (null != Interlocked.CompareExchange(ref s_randomNumberGenerator, null, rng))
                {
                    // The instance has already been initialized.
                    // We will then need to dispose the new one.
                    rng.Dispose();
                }

                s_rngIntialized = true;
            }

            if (s_randomNumberGenerator != null)
            {
                lock (s_randomNumberGenerator)
                {
                    s_randomNumberGenerator.GetBytes(bytes);
                }
            }
            else
            {
                Random rnd = new Random(RandomSeed);
                rnd.NextBytes(bytes);
            }
        }

        #region Hash methods: MD5, SHA1 & SHA256
        public static byte[] MD5(this byte[] bytes)
        {
            return MD5(bytes, 0, bytes.Length);
        }

        public static byte[] MD5(this byte[] bytes, int offset, int count)
        {
            
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return md5.ComputeHash(bytes, offset, count);
            }
        }

        public static byte[] SHA1(this byte[] bytes)
        {
            return SHA1(bytes, 0, bytes.Length);
        }

        public static byte[] SHA1(this byte[] bytes, int offset, int count)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                return sha1.ComputeHash(bytes, offset, count);
            }
        }

        public static byte[] SHA2(this byte[] bytes)
        {
            return SHA2(bytes, 0, bytes.Length);
        }

        public static byte[] SHA2(this byte[] bytes, int offset, int count)
        {
            
            using (var sha2 = System.Security.Cryptography.SHA256.Create())
            {
                return sha2.ComputeHash(bytes, offset, count);
            }
        }
        #endregion
    }
}
