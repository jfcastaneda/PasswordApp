using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace PasswordApp
{
    public static class Crypto
    {
        public static string Encrypt(string data, string password)
        {
            var algorithm = GetAlgorithm(password);
            // Set up streams
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(
              memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
            {
                // Convert the original data to bytes then write them to the CryptoStream
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                // Convert the encrypted bytes back into a string
               return Convert.ToBase64String(memoryStream.ToArray());
            }

        }
        public static string Decrypt(string data, string password)
        {
            var algorithm = GetAlgorithm(password);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(
              memoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
            {
                // Convert the encrypted string to bytes then write them
                // to the CryptoStream
                byte[] buffer = Convert.FromBase64String(data);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();

                // Convert the original data back to a string
                buffer = memoryStream.ToArray();
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }
        public static string Hash(string password)
        {
            // create PRNG 
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider(); 
            // create array of bytes 
            byte[] saltBytes = new byte[16]; 
            // fill array with strong sequence of bytes 
            csp.GetBytes(saltBytes); 
            byte[] dataBytes = Encoding.UTF8.GetBytes(password); 
            byte[] allBytes = new byte[saltBytes.Length + dataBytes.Length]; 
            saltBytes.CopyTo(allBytes, 0); 
            dataBytes.CopyTo(allBytes, saltBytes.Length); 
            byte[] hash = new SHA256Managed().ComputeHash(allBytes); 
            string hashedPassword = Convert.ToBase64String(hash);
            return hashedPassword;
        }
        public static byte[] GenerateNewSalt(int length)
        {

            byte[] random = new Byte[length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            //getnonzerobytes does not exist on this windows phone verion
            //getbytes is also cryptographically strong and I don't see the advantage of one over the other
            rng.GetBytes(random);
            return random;
        }
        static SymmetricAlgorithm GetAlgorithm(string password)
        {
            SymmetricAlgorithm sa = new AesManaged();
            Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, Settings.Salt);
            sa.Key = bytes.GetBytes(sa.KeySize / 8);
            sa.IV = bytes.GetBytes(sa.BlockSize / 8);
            return sa;
        }
    }
}
