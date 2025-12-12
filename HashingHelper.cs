using System;
using System.Security.Cryptography;
using System.Text;

namespace Hemmuppgiftcrud
{
    public static class HashingHelper
    {

        public static string GenerateSalt(int size = 16)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashWithSalt(string value, string base64Salt, int iterations = 100_000, int hashLength = 32)
        {
            var saltBytes = Convert.FromBase64String(base64Salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password: value,
                salt: saltBytes,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256);

            var hash = pbkdf2.GetBytes(hashLength);
            return Convert.ToBase64String(hash);
        }

        public static bool Verify(string value, string base64Salt, string expectedBase64Hash)
        {
            var computedHash = HashWithSalt(value, base64Salt);

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(expectedBase64Hash),
                Convert.FromBase64String(computedHash));
        }
    }
}
