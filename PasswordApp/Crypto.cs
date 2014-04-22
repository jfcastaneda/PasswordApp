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

/*
 * PasswordApp: This program will allow the user to save
 * their passwords on their phone.
 * 
 * Crypto.cs: This file holds the Crypto class.
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 4/14/2014
 * Added all the crypto methods. 
 */

namespace PasswordApp
{
    public static class Crypto
    {
        /*
         * This method handles using the password to encrypt the provided data.
         * Will return the encrypted data
         */
        public static string Encrypt(string data, string password)
        {
            var algorithm = GetAlgorithm(password);
            // Set up streams
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write)) 
            {
                // Convert the original data to bytes then write them to the CryptoStream
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();

                // Convert the encrypted bytes back into a string
               return Convert.ToBase64String(memoryStream.ToArray());
            }

        }

        /*
         * This method handles using the password to decrypt the provided data.
         * Will return the decrypted data
         */
        public static string Decrypt(string data, string password)
        {
            //to avoid errors
            if (data == null) return null;
            var algorithm = GetAlgorithm(password);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Write)) 
            {
                // Convert the encrypted string to bytes then write them to the CryptoStream
                byte[] buffer = Convert.FromBase64String(data);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();

                // Convert the original data back to a string
                buffer = memoryStream.ToArray();
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }

        /*
         * This method handles using the hasging the password
         * Will return the hashed password
         */
        public static string Hash(string password)
        {
            //load array of bytes
            byte[] saltBytes = Settings.Salt;

            // fill array with strong sequence of bytes 
            byte[] dataBytes = Encoding.UTF8.GetBytes(password); 
            byte[] allBytes = new byte[saltBytes.Length + dataBytes.Length]; 
            saltBytes.CopyTo(allBytes, 0); 
            dataBytes.CopyTo(allBytes, saltBytes.Length); 
            byte[] hash = new SHA256Managed().ComputeHash(allBytes); 
            string hashedPassword = Convert.ToBase64String(hash);
            return hashedPassword;
        }


        /*
         * This method handles using generating new salt bytes
         * Will return the salt bytes
         */
        public static byte[] GenerateNewSalt(int length)
        {
            byte[] random = new Byte[length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
           
            //getnonzerobytes does not exist on this windows phone verion
            //getbytes is also cryptographically strong and I don't see the advantage of one over the other
            rng.GetBytes(random);
            return random;
        }

        /*
         * This method handles setting up the algorithm for encoding
         * Will return the algorythim value
         */
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
