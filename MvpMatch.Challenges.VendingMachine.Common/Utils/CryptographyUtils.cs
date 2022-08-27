using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace MvpMatch.Challenges.VendingMachine.Common.Utils
{
    public static class CryptographyUtils
    {
        public static string HashWithSalt(string plainText, byte[] saltBytes = null)
        {
            if(saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];
                
                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            Array.Copy(plainTextBytes, plainTextWithSaltBytes, plainTextBytes.Length);

            // Append salt bytes to the resulting array.
            Array.Copy(saltBytes, 0, plainTextWithSaltBytes, plainTextBytes.Length, saltBytes.Length);

            // Compute hash value of our plain text with appended salt.
            HashAlgorithm hash = new SHA1Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            // Copy hash bytes into resulting array.
            Array.Copy(hashBytes, hashWithSaltBytes, hashBytes.Length);

            // Append salt bytes to the result.
            Array.Copy(saltBytes, 0, hashWithSaltBytes, hashBytes.Length, saltBytes.Length);

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        public static bool VerifyHashWithSalt(string plainText, string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // Size of hash (without salt) for Sha1.
            var hashSizeInBits = 160;
            var hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
            {
                return false;
            }

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            Array.Copy(hashWithSaltBytes, hashSizeInBytes, saltBytes, 0, saltBytes.Length);

            // Compute a new hash string.
            string expectedHashString = HashWithSalt(plainText, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

        public static string Encrypt(string text)
        {
            var textBytes = Encoding.Unicode.GetBytes(text);
            var encryptedBytes = ProtectedData.Protect(textBytes, null, DataProtectionScope.LocalMachine);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            var textBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.LocalMachine);

            return Encoding.Unicode.GetString(textBytes);
        }
    }
}
