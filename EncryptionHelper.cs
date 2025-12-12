using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    // This will be a demo class which shows symmetrical encrypting
    // cleartext --> Encrypting -> "Lagra" -> read -> De-encrypt - Clear text
    public class EncryptionHelper
    {

        // En v
        // 1 = 1, 2, 4, 8, 16, 32, 64...
        // E D C B A 
        // 0x42 is a hexadecimal value (base 16). it represents 66 bytes but in decimal (base10)
        // the value is made up
        private const byte Key = 0x42; // 66 bytes

        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Convert the text to bytes
            // Why? the text is Unicode (char/strings)
            // XOR so that you can distort our string and then we'll need to convert it to a byte array

            var bytes = System.Text.Encoding.UTF8.GetBytes(text);

            /* 2.
             * A logical operation
             * 0 ^ 0 = 0
             * 1 ^ 0 = 1
             * 0 ^ 1 = 1
             * 1 ^ 1 = 0
             * different bits gives 1, same bits gives 0
             * 
             * Why just XOR?
             *  - Simple to implement and understand
             *  - Symmetrical: (A ^ B) ^ B = A
             *  - bytes[i] is a byte (0-255)
             *  - Key is also a byte (0-255)
             *  - bytes[i] ^ Key gives a int-result, so vi cast it back to byte
             */
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ Key); // XOR operation
            }

            // To be able to save the result as text. We code bytes to Base64
            // Why Base64?
            // after vi have XOR:ed can bytes include unavailable characters for text/JSON
            // Easier to store in the filex JSon, Databases etc.
            return Convert.ToBase64String(bytes);
        }

        public static string Decrypt(string krypteradText)
        {
            // 1
            //return krypteradText;
            // 2
            // Recreate Base64 string to bytes again
            // XOR back with same Key
            // here we make use of the XOR property
            // Originaltext ^ Key = Encrypted
            // krypteradText ^ Key = Original Text / Decrypted
            // That's why the code looks the same
            var bytes = Convert.FromBase64String(krypteradText);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ Key); // XOR operation
            }

            // 3 Convert it back from byte to clear text with UTF8
            return System.Text.Encoding.UTF8.GetString(bytes);

        }

        public class Hashinghelper
        {
            public static string Generatesalt(int size = 16)
            {
                var saltBytes = RandomNumberGenerator.GetBytes(size);
                return Convert.ToBase64String(saltBytes);
            }

            public static string HasWithSalt(string value, string base64Salt, int interations = 100_000, int hasLength = 32)
            {
                var saltBytes = Convert.FromBase64String(base64Salt);
                using var pbkdf2 = new Rfc2898DeriveBytes(
                    password: value,
                    salt: saltBytes,
                    iterations: interations,
                    hashAlgorithm: HashAlgorithmName.SHA256);

                var hash = pbkdf2.GetBytes(hasLength);
                return Convert.ToBase64String(hash);
            }

            public static bool verify(string value, string base64Salt, string expectedBase64Hash)
            {
                var computedHash = HasWithSalt(value, base64Salt);
                return CryptographicOperations.FixedTimeEquals(
                    Convert.FromBase64String(expectedBase64Hash),
                    Convert.FromBase64String(computedHash));
            }




        }
    }
}
