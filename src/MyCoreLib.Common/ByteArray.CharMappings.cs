
namespace MyCoreLib.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Threading;


    /// <summary>
    /// Provides some helper methods for an array of bytes.
    /// </summary>
    static partial class ByteArray
    {

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static extern void MemoryBarrier();

        private static int RandomSeed
        {
            get
            {
                unchecked
                {
                    // The statement is designed to prevent a seed
                    // from being repeatedly used.
                    //
                    return Environment.TickCount + (s_identityValue++);
                }
            }
        }

        private static int s_identityValue;
        private static RandomNumberGenerator s_randomNumberGenerator;
        private static bool s_rngIntialized;

        private static byte[] s_hexCharToIntegerMapping;
        private static char[] s_integerToHexCharMapping = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'
        };

        private static byte[] s_customBase64Map;
        private static char[] s_customBase64Chars = new char[]
        {
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            '0','1','2','3','4','5','6','7','8','9', '-', '_'
        };

        internal static byte[] HexCharToIntegerMapping
        {
            get
            {
                if (s_hexCharToIntegerMapping == null)
                {
                    // Creates a mapping from an ASCII character to an integer.
                    //
                    byte[] hexValuesBuffer = new byte['g'];
                    int pos = hexValuesBuffer.Length;

                    while ((--pos) >= 0)
                    {
                        // The decimal values of the ASCII characters.
                        //
                        // [Char] [decimal] [hexadecimal]
                        //  0      48        0x30
                        //  1      49        0x31
                        //  9      57        0x39
                        // ...
                        //  A      65        0x41
                        //  F      70        0x46
                        // ...
                        //  a      97        0x61
                        //  f      102       0x66
                        //
                        if ('0' <= pos && pos <= '9')
                        {
                            hexValuesBuffer[pos] = (byte)(pos - '0');
                            continue;
                        }

                        if ('a' <= pos && pos <= 'f')
                        {
                            hexValuesBuffer[pos] = (byte)(pos - 'a' + 10);
                            continue;
                        }

                        if ('A' <= pos && pos <= 'F')
                        {
                            hexValuesBuffer[pos] = (byte)(pos - 'A' + 10);
                        }
                    }

                    MemoryBarrier();
                    s_hexCharToIntegerMapping = hexValuesBuffer;
                }

                return s_hexCharToIntegerMapping;
            }
        }

        private static byte[] CustomBase64Map
        {
            get
            {
                if (s_customBase64Map == null)
                {
                    byte[] buffer = new byte[0xFF + 1];

                    // Fill the memory with 0xFF which stands for invalid chars
                    for (int i = 0; i < buffer.Length; i++)
                        buffer[i] = 0xFF;

                    for (int i = 0; i < s_customBase64Chars.Length; i++)
                    {
                        buffer[s_customBase64Chars[i]] = (byte)i;
                    }

                    MemoryBarrier();
                    s_customBase64Map = buffer;
                }

                return s_customBase64Map;
            }
        }
    }
}
