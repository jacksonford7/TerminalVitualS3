using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Net;

namespace CSLSite.app_start
{
    public class CredencialesHelper
    {
        public static bool UploadFile(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // .NET 4.5

                var ws_file = new GetFile.Service();
                
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"),Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}",dateServer,filename);
                return true;
            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException!=null?ex.InnerException.Message:ex.Message;
                return false;
            }

        }

        public static bool UploadFile_NotaCredito(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_nc"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;  


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }


        public static bool UploadFile_Zal(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_zal"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;
            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        public static bool UploadFile_Stc(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_stc"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;
            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        public static bool UploadFile_Aisv(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_aisv"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        public static bool UploadFile_AppCgsa(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_appcgsa"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        public static bool UploadFile_Dae(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_dae"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        public static bool UploadFile_Inspeccion(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_inspeccion"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }


        public static bool UploadFile_CarbonoNeutro(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_carbononeutro"].ToString() + dateServer;

                string CARPETA = System.DateTime.Now.Year.ToString().Trim();
                string SUBCARPETA = System.DateTime.Now.Month.ToString().Trim();
                string DIR_CARPETA = string.Format("{0}{1}", ConfigurationManager.AppSettings["rutaserver_carbononeutro"].ToString(), CARPETA);
                string DIR_SUBCARPETA = string.Format("{0}{1}\\{2}", ConfigurationManager.AppSettings["rutaserver_carbononeutro"].ToString(), CARPETA, SUBCARPETA);

                if (!(Directory.Exists(DIR_CARPETA)))
                {
                    Directory.CreateDirectory(DIR_CARPETA);
                }
                if (!(Directory.Exists(DIR_SUBCARPETA)))
                {
                    Directory.CreateDirectory(DIR_SUBCARPETA);
                }

                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        public static bool UploadFileRF(string fullPath, byte[] ImagenBase64, out string fileResume)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // .NET 4.5

                var ws_file = new GetFile.Service();
                byte[] inputStream = ImagenBase64;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;
            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }
        }

        public static bool UploadFile_Transportistas(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        public static bool UploadFile_CertificadoSellos(string fullPath, Stream file, out string fileResume)
        {

            try
            {

                var ws_file = new GetFile.Service();
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver_certificadosellos"].ToString() + dateServer;

                string CARPETA = System.DateTime.Now.Year.ToString().Trim();
                string SUBCARPETA = System.DateTime.Now.Month.ToString().Trim();
                string DIR_CARPETA = string.Format("{0}{1}", ConfigurationManager.AppSettings["rutaserver_certificadosellos"].ToString(), CARPETA);
                string DIR_SUBCARPETA = string.Format("{0}{1}\\{2}", ConfigurationManager.AppSettings["rutaserver_certificadosellos"].ToString(), CARPETA, SUBCARPETA);

                if (!(Directory.Exists(DIR_CARPETA)))
                {
                    Directory.CreateDirectory(DIR_CARPETA);
                }
                if (!(Directory.Exists(DIR_SUBCARPETA)))
                {
                    Directory.CreateDirectory(DIR_SUBCARPETA);
                }

                var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", dateServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return false;
            }

        }

        //public static string Descargar_NotaCredito(out string fileResume)
        //{
        //    string valor = "";
        //    try
        //    {

        //        var ws_file = new GetFile.Service();
        //       // var br = new BinaryReader(file);
        //      //  byte[] inputStream = br.ReadBytes((Int32)file.Length);
        //        String dateServer = credenciales.GetDateServer();
        //        String rutaServer = ConfigurationManager.AppSettings["rutaserver_nc"].ToString() + dateServer;

        //        var file = ws_file.DownloadFile(rutaServer, "01.msg");
        //        valor = "ddd";
        //        //var filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));
        //        //var s = ws_file.UploadFile(inputStream, rutaServer, filename);
        //        //if (string.IsNullOrEmpty(file))
        //        //{
        //        //    fileResume = "El webservice de subir archivo no esta disponible";
        //        //    return false;
        //        //}
        //        //s = s.ToLower();
        //        //if (!s.Contains("successfully"))
        //        //{
        //        //    fileResume = s;
        //        //    return false;
        //        //}
        //        //fileResume = string.Format("{0}{1}", dateServer, filename);



        //    }
        //    catch (Exception ex)
        //    {
        //        fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        return string.Empty;
        //    }

        //}
    }
}