using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eikon.eFirma;
using Eikon.eFirma.Core;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using csl_log;

namespace CSLSite.app_start
{
    public class EcuapassCryptoSign
    {
        //NUEVO 2019--> PARA CONSULTAR DIRECTO LA DAE
        private static RijndaelManaged managed;
        string clv_pass;
        private static string path_final;

        public EcuapassCryptoSign( string path )
        {
            path_final = path;
            managed =  Generate_Passw();
            clv_pass = RSAEncrypt_Pad(managed.Key, false);
        }

        private static  RijndaelManaged Generate_Passw()
        {
            RijndaelManaged managed = new RijndaelManaged
            {
                //KeySize = 0x80,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.ECB,
                KeySize = 128,
                BlockSize = 128
            };
            managed.GenerateKey();
            return managed;
        }

         static public RijndaelManaged Generate_Passw_New()
        {
            RijndaelManaged managed = new RijndaelManaged
            {
                //KeySize = 0x80,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.ECB,
                KeySize = 128,
                BlockSize = 128
            };
            managed.GenerateKey();
            return managed;
        }

        static public string RSAEncrypt_Pad_New(byte[] DataToEncrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //r = new Random(Environment.TickCount);

                Certificate certificate = new Certificate();
                //certificate.LoadCertificate(Directory.GetCurrentDirectory() + @"\publicKey\cert_aduana.cer");
                certificate.LoadCertificate(path_final);

                var a = Convert.ToBase64String(DataToEncrypt);
                byte[] bite = Encoding.UTF8.GetBytes(a);
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg = (RSACryptoServiceProvider)certificate.CertObject.PublicKey.Key;
                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSAalg.Encrypt(bite, DoOAEPPadding);

                return Convert.ToBase64String(encryptedData);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static public string AES_Encrypt_New(string input, RijndaelManaged aes)
        {
            string encrypted = "";

            try
            {
                ICryptoTransform DESEncrypter = aes.CreateEncryptor();
                byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(input);
                encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return encrypted;
            }
            catch (Exception)
            { return ""; }
        }


        private static String EncryptStringToBytes_Aes(string plainText, byte[] Key)
        {

            try
            {
                byte[] encrypted;
                byte[] IV;
                using (Aes aesAlg = Aes.Create())
                {

                    aesAlg.Mode = CipherMode.ECB;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Key = Key;
                    aesAlg.GenerateIV();
                    IV = aesAlg.IV;
                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                var combinedIvCt = new byte[IV.Length + encrypted.Length];
                Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
                Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);
                String claveEn = Convert.ToBase64String(combinedIvCt);//.getBytes("UTF-8"))); 
                return claveEn;
            }
            catch (Exception e)
            {
                log_csl.save_log<Exception>(e, "EcuapassCryptoSign", "EncryptStringToBytes_Aes", plainText, "GenerarClave");
                return null;
            }


        }
        private static  byte[] RSAEncrypt_Pub(byte[] DataToEncrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                Certificate certificate = new Certificate();
                certificate.LoadCertificate(path_final);
                byte[] Key = certificate.CertObject.GetPublicKey();
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo = RSAalg.ExportParameters(false);
                RSAKeyInfo.Modulus = Key;
                RSAalg.ImportParameters(RSAKeyInfo); 
                encryptedData = RSAalg.Encrypt(DataToEncrypt, DoOAEPPadding);
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                log_csl.save_log<CryptographicException>(e, "EcuapassCryptoSign", "RSAEncrypt_Pub", path_final, "GenerarClave");
                return null;
            }

        }
        private static  string RSAEncrypt_Pad(byte[] DataToEncrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                Certificate certificate = new Certificate();
                certificate.LoadCertificate(path_final);
                var a = Convert.ToBase64String(DataToEncrypt);
                byte[] bite = Encoding.UTF8.GetBytes(a);
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg = (RSACryptoServiceProvider)certificate.CertObject.PublicKey.Key;  
                encryptedData = RSAalg.Encrypt(bite, DoOAEPPadding);
                return Convert.ToBase64String(encryptedData);
            }
            catch (CryptographicException e)
            {
                log_csl.save_log<CryptographicException>(e, "EcuapassCryptoSign", "RSAEncrypt_Pad", path_final, "GenerarClave");
                return null;
            }
        }
        private static String EncryptStringToBytes_Aes(string plainText, ICryptoTransform Key)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, Key, CryptoStreamMode.Write);
                stream2.Write(encoding.GetBytes(plainText), 0, plainText.Length);
                stream2.FlushFinalBlock();
                plainText = Convert.ToBase64String(stream.ToArray());
                stream.Close();
                stream2.Close();
                return plainText;
            }
            catch (Exception e)
            {
                log_csl.save_log<Exception>(e, "EcuapassCryptoSign", "EncryptStringToBytes_Aes", plainText, "GenerarClave");
                return null;
            }
        }
        private static  string AES_Encrypt(string input, RijndaelManaged aes)
        {
            string encrypted = string.Empty;
            try
            {
                ICryptoTransform DESEncrypter = aes.CreateEncryptor();
                byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(input);
                encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return encrypted;
            }
            catch (Exception e)
            {
                log_csl.save_log<Exception>(e, "EcuapassCryptoSign", "AES_Encrypt", input, "GenerarClave");
                return string.Empty;
            }
        }
        public string CadenaEncript(string cadena)
        {
           return AES_Encrypt(cadena, managed);
          // return null;
        }

        public string clave { get { return clv_pass; } }

    }
}