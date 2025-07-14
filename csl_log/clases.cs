using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace csl_log
{
    public class log_csl
    {
        private static string cadena = string.Empty;
        private static string filerute = string.Empty;
        private static string track = string.Empty;
        //recibe un error de cualquier tipo y lo siembra en la base.
        public static Int64 save_log<T>(T error, string clase, string metodo, string input, string usuario = null) where T : System.Exception
        {
            try
            {
                initdataconf();
                Int64 result = 0;
                //coexion
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_insert_log";
                            comando.Parameters.AddWithValue("@clase", clase);
                            comando.Parameters.AddWithValue("@metodo", metodo);
                            comando.Parameters.AddWithValue("@usuario", usuario!=null?usuario:"NoUserReport");
                            comando.Parameters.AddWithValue("@inputdata", input);
                            comando.Parameters.AddWithValue("@mensaje00",string.Format("Target:{0},Message:{1}",error.TargetSite, error.Message));
                            comando.Parameters.AddWithValue("@mensaje01", string.Format("Pila:{0},Message:{1}", error.StackTrace, error.InnerException != null ? error.InnerException.Message : DBNull.Value.ToString()));
                            conexion.Open();
                            result = Int64.Parse(comando.ExecuteScalar().ToString());
                            conexion.Close();
                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log<Exception>(new Exception(sb.ToString()), "log_csl", "save_log", input, usuario);
                        return 0;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "log_csl", "save_log", input, usuario, true);
                return 0;
            }
        }
        //recibe un error de cualquier tipo y lo siembra en un archivo txt
        public static void save_log<T>(T error, string clase, string metodo, string input, string usuario = null, bool file = false) where T : System.Exception
        {
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;
            parOpts.TaskScheduler = TaskScheduler.Current;
            TaskCreationOptions tco = new TaskCreationOptions();
            tco = TaskCreationOptions.PreferFairness;
            Task task = null;
            task = Task.Factory.StartNew(() =>
            {
                try
                {
                    initfileconf();
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(filerute)))
                    {

                        filerute = string.Format("csl_{0}.txt",DateTime.Now.ToString("ddMMyyyymmss"));
                    }

                    using (var w = System.IO.File.AppendText(filerute))
                    {
                        string message00 = error.Message;
                        string message01 = error.InnerException != null ? error.InnerException.Message : string.Empty;
                        w.WriteLine(string.Format("{0}->{1},{2},{3},{4},{5},{6}", DateTime.Now, clase, metodo, input, usuario, message00, message01));
                    }
                    
                }
                finally
                {
                    //adorno decorativo
                }
            }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);
            task.ContinueWith((a) => { a.Dispose(); });
        }
        //iniciar la cadena de configuración 
        private static void initdataconf()
        {
            if (cadena.Trim().Length <= 0)
            {
                if (System.Configuration.ConfigurationManager.ConnectionStrings["service"] != null)
                {
                    cadena = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                    return;
                }
                throw new InvalidOperationException("Error al inicializar conexión [D01]");
            }
        }
        //iniciar la ruta del archivo
        private static void initfileconf()
        {
            if (filerute.Trim().Length <= 0 && System.Configuration.ConfigurationManager.AppSettings["service"] != null)
            {
                filerute = string.Format("{0}\\csl_{1}.txt", System.Configuration.ConfigurationManager.AppSettings["service"],DateTime.Now.ToString("ddMMMMyyyyHHmmss"));
                return;
            }
        }
        //iniciar la cadena de configuración 
        private static void inittrackerconf()
        {
            if (track.Trim().Length <= 0)
            {
                if (System.Configuration.ConfigurationManager.ConnectionStrings["loger"] != null)
                {
                    track = System.Configuration.ConfigurationManager.ConnectionStrings["loger"].ConnectionString;
                    return;
                }
                throw new InvalidOperationException("Error al inicializar conexión [D02]");
            }
        }
        //Este método comprueba que este token sea válido.
        public static bool validateToken(string token)
        {
            if (token == null)
            {
                return false;
            }
            try
            {
                initdataconf();
                Int64 result = 0;
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_validate_token";
                            comando.Parameters.AddWithValue("@token", token.Trim());
                            conexion.Open();
                            result = int.Parse(comando.ExecuteScalar().ToString());
                            conexion.Close();
                            return result > 0;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log<Exception>(new Exception(sb.ToString()), "log_csl", "save_log", "Data", token);
                        return false;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "log_csl", "save_log", "Data", token, true);
                return false;
            }
        }
        //este metodo crea el token los inserta en la base
        public static bool createToken(string user, string objeto, out string token)
        {
            token = null;
            try
            {
                initdataconf();
                Int64 result = 0;
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            var xtoken = unikeToken();
                            xtoken = string.IsNullOrEmpty(xtoken) ? string.Format("{0}{1}XNULL", DateTime.Now.Year, DateTime.Now.Millisecond) : xtoken;
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_insert_token";
                            comando.Parameters.AddWithValue("@objeto", objeto);
                            comando.Parameters.AddWithValue("@usuario", user);
                            comando.Parameters.AddWithValue("@token", xtoken);
                            token = xtoken;
                            conexion.Open();
                            var x = comando.ExecuteScalar();
                            if (x != null)
                            {
                                result = int.Parse(x.ToString());
                            }
                            conexion.Close();
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log<Exception>(new Exception(sb.ToString()), "log_csl", "save_token", objeto, token);
                        return false;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "log_csl", "save_token", objeto, user, true);
                return false;
            }
        }
        //Genera un token unico
        private static string unikeToken()
        {
            try
            {
                int maxSize = 8;
                char[] chars = new char[62];
                string a;
                a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                chars = a.ToCharArray();
                int size = maxSize;
                byte[] data = new byte[1];
                using (var crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetNonZeroBytes(data);
                    size = maxSize;
                    data = new byte[size];
                    crypto.GetNonZeroBytes(data);
                }
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                { result.Append(chars[b % (chars.Length - 1)]); }
                return result.ToString();
            }
            catch
            {
                return null;
            }
        }
        //inserta cabeceras del tracking
        public static void saveTracking_cab(string sesion, int idcorporacion, int idusuario, string ip, string browser, string token)
        {
            try
            {
                inittrackerconf();
                using (var conexion = new SqlConnection(track))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_add_track_cb";
                            comando.Parameters.AddWithValue("@Sesion", string.IsNullOrEmpty( sesion)?DateTime.Now.ToString("ddmmyyyyhhmmss"):sesion);
                            comando.Parameters.AddWithValue("@IdCorporacion", idcorporacion);
                            comando.Parameters.AddWithValue("@IdUsuario", idusuario);
                            comando.Parameters.AddWithValue("@Ip", ip);
                            comando.Parameters.AddWithValue("@Browser", browser);
                            comando.Parameters.AddWithValue("@token", token);
                            conexion.Open();
                            comando.ExecuteNonQuery();
                            conexion.Close();
                        }
                    }

                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log<Exception>(new Exception(sb.ToString()), "log_csl", "save_log", idusuario.ToString() ,token );
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "log_csl", "save_log", idusuario.ToString(), token, true);
            }
        }
        //inserta detalles del tracking
        public static void saveTracking_det(string sesion,  string transaccion, string token)
        {
            try
            {
                inittrackerconf();
                using (var conexion = new SqlConnection(track))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_add_track_dt";
                            comando.Parameters.AddWithValue("@Sesion", sesion);
                            comando.Parameters.AddWithValue("@Transaccion", transaccion);
                            comando.Parameters.AddWithValue("@token", token);
                            conexion.Open();
                            comando.ExecuteNonQuery();
                            conexion.Close();
                        }
                    }

                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log<Exception>(new Exception(sb.ToString()), "log_csl", "save_log", sesion, token);
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "log_csl", "save_log", sesion, token, true);
            }
        }
        //ciera el tracking de la cabecera
        public static void closeTracking_cab(string token,int idcorporacion, int idusuario)
        {
            try
            {
                inittrackerconf();
                using (var conexion = new SqlConnection(track))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_up_track_cb";
                            comando.Parameters.AddWithValue("@IdCorporacion", idcorporacion);
                            comando.Parameters.AddWithValue("@IdUsuario", idusuario);
                            var sesion = System.Text.RegularExpressions.Regex.Replace(token, @"\r\n?|\n|\0", string.Empty);
                            comando.Parameters.AddWithValue("@token", sesion);
                            conexion.Open();
                            comando.ExecuteNonQuery();
                            conexion.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log<Exception>(new Exception(sb.ToString()), "log_csl", "save_log", idusuario.ToString(), token);
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "log_csl", "save_log", idusuario.ToString(), token, true);
            }
        }
        //mensajero asyncrono solo lo inserta en la base y esta los envía (TRIGGER)
        public static void mailSenderDB(string htmlmsg, int moduloID ,string mailpara, string usuario = null) 
        {
            try
            {
                initdataconf();
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_insert_mail_log";
                            comando.Parameters.AddWithValue("@htmlmsg", htmlmsg );
                            comando.Parameters.AddWithValue("@mailpara", mailpara);
                            comando.Parameters.AddWithValue("@usuario", usuario != null ? usuario : "NoUserReport");
                            comando.Parameters.AddWithValue("@moduloID", moduloID);
                            conexion.Open();
                            comando.ExecuteNonQuery();
                            conexion.Close();
                            return;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                          save_log<Exception>(new Exception(sb.ToString()), "log_csl", "mailSenderDB", htmlmsg, usuario);
                        return;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "log_csl", "mailSenderDB", "mailerror", usuario, true);
                return;
            }
        }
    }
}
