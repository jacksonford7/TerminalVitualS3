using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace BillionAutenticacion
{
    public class Cls_QuerySegura
    {
        //encrypta cualquier string, las llaves en webconfig
        public static string EncryptQueryString(string inputText, string key = null, string salt = null)
        {
            try
            {
                if (string.IsNullOrEmpty(inputText))
                {
                    return null;
                }
                if (string.IsNullOrEmpty(key))
                {
                    key = System.Configuration.ConfigurationManager.AppSettings["ikey"];
                }
                if (string.IsNullOrEmpty(salt))
                {
                    salt = System.Configuration.ConfigurationManager.AppSettings["isalt"];
                }
                byte[] plainText = Encoding.UTF8.GetBytes(inputText);
                using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                {
                    PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));
                    using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainText, 0, plainText.Length);
                                cryptoStream.FlushFinalBlock();
                                string base64 = Convert.ToBase64String(memoryStream.ToArray());
                                // Generate a string that won't get screwed up when passed as a query string.
                                string urlEncoded = HttpUtility.UrlEncode(base64);
                                return urlEncoded;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        //desencrypta cualquier string, las llaves en webconfig
        public static string DecryptQueryString(string inputText, string key = null, string salt = null)
        {
            var originals = inputText;
            try
            {
                if (string.IsNullOrEmpty(inputText))
                {
                    return null;
                }

                if (string.IsNullOrEmpty(key))
                {
                    key = System.Configuration.ConfigurationManager.AppSettings["ikey"];
                }

                if (string.IsNullOrEmpty(salt))
                {
                    salt = System.Configuration.ConfigurationManager.AppSettings["isalt"];
                }

                inputText = HttpUtility.UrlDecode(inputText);
                byte[] encryptedData;
                try
                {
                    encryptedData = Convert.FromBase64String(inputText.Replace(" ", "+"));
                }
                catch
                {
                    encryptedData = Convert.FromBase64String(originals.Replace(" ", "+"));

                }
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));
                using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                {
                    using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainText = new byte[encryptedData.Length];
                                cryptoStream.Read(plainText, 0, plainText.Length);
                                string utf8 = Encoding.UTF8.GetString(plainText);
                                return utf8;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
