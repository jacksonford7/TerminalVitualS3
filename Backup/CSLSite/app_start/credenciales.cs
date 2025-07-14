using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSLSite.CatalogosTableAdapters;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using csl_log;
using System.IO;
using System.Collections;
using ConectorN4;

namespace CSLSite
{
    public class credenciales
    {
        private SqlConnection conn;
        private SqlTransaction sqlTransaction;
        private SqlDataReader dr;
        private SqlCommand cmd;
        private static OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
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
        public static String SaveLog(string error, string clase, string metodo, string input, string usuario = null)
        {
            try
            {
                initdataconf();
                String result = string.Empty;
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
                            comando.Parameters.AddWithValue("@usuario", usuario != null ? usuario : "NoUserReport");
                            comando.Parameters.AddWithValue("@inputdata", input);
                            comando.Parameters.AddWithValue("@mensaje00", string.Format("Target:{0},Message:{1}", error, error));
                            comando.Parameters.AddWithValue("@mensaje01", string.Format("Pila:{0},Message:{1}", error, error != null ? error : DBNull.Value.ToString()));
                            conexion.Open();
                            comando.ExecuteScalar().ToString();
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
                        return sb.ToString();
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
                //save_log<Exception>(ex, "log_csl", "save_log", input, usuario, true);
                return ex.Message.ToString();
            }
        }
        public static string GetDescripcionEmpresaOnlyControl(string rucempresa, out string error)
        {
            String error_consulta = string.Empty;
            error = string.Empty;
            var descripcionempresa = onlyControl.AC_C_EMPRESA(rucempresa, 0, ref error_consulta).DefaultViewManager.DataSet.Tables[0].Rows[0]["EMPE_NOM"].ToString();
            if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
            }
            return descripcionempresa;
        }
        public static DataTable GetNominaOnlyControl(string rucempresa, out string error)
        {
            String error_consulta = string.Empty;
            error = error_consulta;
            DataTable dtNominaOnyControl = new DataTable();
            var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
                return dtNominaOnyControl;
            }
            var dtPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
                return dtNominaOnyControl;
            }
            dtNominaOnyControl = dtPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0];
            return dtNominaOnyControl;
        }
        public static DataTable GetAreaOnlyControl(string rucempresa, out string error)
        {
            String error_consulta = string.Empty;
            error = error_consulta;
            DataTable dtAreaOnlyControl = new DataTable();
            var AC_C_AREA = onlyControl.AC_C_AREA("%", 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
                return dtAreaOnlyControl;
            }
            dtAreaOnlyControl = AC_C_AREA.DefaultViewManager.DataSet.Tables[0];
            return dtAreaOnlyControl;
        }
        public static DataTable GetDptoOnlyControl(string rucempresa, out string error)
        {
            String error_consulta = string.Empty;
            error = error_consulta;
            DataTable dtDptoOnlyControl = new DataTable();
            var AC_C_DEPTO = onlyControl.AC_C_DEPTO("", 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
                return dtDptoOnlyControl;
            }
            dtDptoOnlyControl = AC_C_DEPTO.DefaultViewManager.DataSet.Tables[0];
            return dtDptoOnlyControl;
        }
        public static DataTable GetCargoOnlyControl(string rucempresa, out string error)
        {
            String error_consulta = string.Empty;
            error = error_consulta;
            DataTable dtCargoOnlyControl = new DataTable();
            var AC_C_CARGO = onlyControl.AC_C_CARGO("", 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
                return dtCargoOnlyControl;
            }
            dtCargoOnlyControl = AC_C_CARGO.DefaultViewManager.DataSet.Tables[0];
            return dtCargoOnlyControl;
        }
        public static DataTable GetActividadOnlyControl(string rucempresa, out string error)
        {
            String error_consulta = string.Empty;
            error = error_consulta;
            DataTable dtActividadOnlyControl = new DataTable();
            var AC_C_ACTIVIDAD = onlyControl.AC_C_ACTIVIDAD("%", ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
                return dtActividadOnlyControl;
            }
            dtActividadOnlyControl = AC_C_ACTIVIDAD.DefaultViewManager.DataSet.Tables[0];
            return dtActividadOnlyControl;
        }
        public static string GetRegistraCargoOnlyControl(string cargo, out string error)
        {
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            error = error_consulta;
            DataTable dtCargoOnlyControl = new DataTable();
            dtCargoOnlyControl.Columns.Add("ID");
            dtCargoOnlyControl.Columns.Add("NOMBRE");
            dtCargoOnlyControl.Rows.Add("", cargo);
            DataSet dsCargoOnlyControl = new DataSet();
            DataSet dsErrorAC_R_CARGO = new DataSet();
            dsCargoOnlyControl.Tables.Add(dtCargoOnlyControl);
            var AC_C_AREA = onlyControl.AC_R_CARGO(dsCargoOnlyControl, 1, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta);
            if (registros_actualizados_incorrecto > 0)
            {
                if (string.IsNullOrEmpty(error_consulta))
                {
                    var dserror = dsErrorAC_R_CARGO.DefaultViewManager.DataSet.Tables;
                    var dterror = dserror[0];
                    var error2 = string.Empty;
                    for (int i2 = 0; i2 < dterror.Rows.Count; i2++)
                    {
                        error2 = error2 + " \\n *" +
                               "Error: " + dterror.Rows[i2]["ERROR"].ToString() + " \\n ";
                    }
                    error = error2;
                }
            }
            else if (!string.IsNullOrEmpty(error_consulta))
            {
                error = error_consulta;
            }
            return error;
        }
        public static string GetPassAlfaNumeric()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var results = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return results;
        }
        private static string cadena = string.Empty;
        public static string registraEmpresaN4(ObjectSesion sesObj, string xml)
        {
            string me = string.Empty;
            string errorValidacion = string.Empty;
            n4WebService n4s = new n4WebService();
            var i = n4s.InvokeN4Service(sesObj, xml, ref errorValidacion, (new Guid()).ToString());
            return errorValidacion;
        }
        /// <summary>
        /// getBinaryFile：Return array of byte which you specified。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] ReadBinaryFile(string path, string fileName, out FileStream fileStream_)
        {
            fileStream_ = null;
            if (File.Exists(path + fileName))
            {
                try
                {
                    ///Open and read a file。
                    FileStream fileStream = File.OpenRead(path + fileName);
                    fileStream_ = fileStream;
                    return ConvertStreamToByteBuffer(fileStream_, path + fileName);
                }
                catch (Exception)
                {
                    return new byte[0];
                }
            }
            else
            {
                return new byte[0];
            }
        }
        /// <summary>
        /// ConvertStreamToByteBuffer：Convert Stream To ByteBuffer。
        /// </summary>
        /// <param name="theStream"></param>
        /// <returns></returns>
        public static byte[] ConvertStreamToByteBuffer(System.IO.Stream theStream, string src)
        {
          MemoryStream tempStream = new MemoryStream();
            //int b1;
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(src);
            using (tempStream)
            {
                using (iTextSharp.text.pdf.PdfStamper stamper = new iTextSharp.text.pdf.PdfStamper(reader, tempStream))
                {
                    Dictionary<String, String> info = reader.Info;
                    info["Title"] = "";
                    info["Subject"] = "";
                    info["Keywords"] = "";
                    info["Creator"] = "";
                    info["Author"] = "";
                    stamper.MoreInfo = info;
                }
            }
            //while ((b1 = theStream.ReadByte()) != -1)
            //{
            //    tempStream.WriteByte(((byte)b1));
            //}
            reader.Close();
            return tempStream.ToArray();
        }
        private static SqlConnection conexionN5()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString);
        }
        private static SqlConnection conexionPortalServicio()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["catalogo"].ConnectionString);
        }
        private static SqlConnection conexion()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sca"].ConnectionString);
        }
        private static SqlConnection conexionN4()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
        }
        private static SqlConnection serviceCsl()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString);
        }
        private static SqlConnection conexionOnlyAccess()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["onlyaccess"].ConnectionString);
        }
        private static SqlConnection conexionMDA()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["mda"].ConnectionString);
        }
        public static HashSet<Tuple<string, string>> getTipoSolicitudes()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoSolicitudes())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("%", "* Elija el tipo de solicitud. *"));
            return xlista;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoSolicitudes()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", "TS");
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoSolicitudes", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static HashSet<Tuple<string, string>> getTipoUsuarios()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoUsuarios())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("%", "* Elija el tipo de usuario. *"));
            return xlista;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoUsuarios()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 1);
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoUsuarios", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static HashSet<Tuple<string, string>> getTipoClientEmpresa()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoClientEmpresa())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            //xlista.Add(Tuple.Create("0", "* Elija el tipo de usuario. *"));
            return xlista;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoClientEmpresa()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 1);
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoClientEmpresa", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static HashSet<Tuple<string, string>> getCategoriaVehiculo(int tiposolicitud)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoSolicitud(tiposolicitud))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Elija el tipo de categoria. *"));
            return xlista;
        }
        public static HashSet<Tuple<string, string>> getTipoSolicitud(int tiposolicitud)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoSolicitud(tiposolicitud))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Elija el tipo de credencial. *"));
            return xlista;
        }
        public static HashSet<Tuple<string, string>> getTipoLicencia(string codigo)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoLicencia(codigo))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Elija el tipo de licencia. *"));
            return xlista;
        }
        public static HashSet<Tuple<string, string>> getTipoSolicitudVeh(int tiposolicitud)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoSolicitud(tiposolicitud))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Elija el tipo de registro. *"));
            return xlista;
        }
        public static HashSet<Tuple<string, string>> getTipoEmpresa(string rucempresa)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoEmpresa(rucempresa))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Elija el tipo de empresa. *"));
            return xlista;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoEmpresa(string rucempresa)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 43);
                coman.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoSolicitud", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoSolicitud(int tiposolicitud)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", tiposolicitud);
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoSolicitud", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoLicencia(string codigo)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 67);
                coman.Parameters.AddWithValue("@CODTIPLIC", codigo);
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoLicencia", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static HashSet<Tuple<string, string>> getTipoEstados()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoEstados())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("%", "* Elija el estado. *"));
            return xlista;
        }
        public static HashSet<Tuple<string, string>> getTipoEstadosList()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            xlista.Add(Tuple.Create("%", "TODOS"));
            foreach (var i in ReturnTipoEstados())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            return xlista;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoEstados()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", "ES");
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoEstados", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static HashSet<Tuple<string, string>> getTipoEventosColaborador(string tipo)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoEventosColaborador(tipo))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Elija el permiso. *"));
            return xlista;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoEventosColaborador(string tipo)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", tipo);
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoEventosColaborador", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static HashSet<Tuple<string, string>> getTipoEstadosLanchas()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoEstadosLanchas())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("%", "* Elija el estado. *"));
            return xlista;
        }
        public static IEnumerable<Tuple<string, string>> ReturnTipoEstadosLanchas()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = conexion();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var coman = con.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandTimeout = 120;
                coman.CommandText = "MDL_PROCESO_LANCHAS";
                coman.Parameters.AddWithValue("@TIPO", 2);
                try
                {
                    con.Open();
                    using (var reader = coman.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    coman.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "credenciales", "ReturnTipoEstados", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        public static DataTable GetValidaUsuarioYClave(string usuario, string clave, string idusuario)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 31);
                coman.Parameters.AddWithValue("@USUARIO", usuario);
                coman.Parameters.AddWithValue("@CLAVE", clave);
                coman.Parameters.AddWithValue("@IDUSUARIO", idusuario);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetValidaUsuarioYClave", "usuario:" + usuario + "||" + "clave:" + clave, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudFacturas(string ruccipas, string numsolicitud, string fechaini, string fechafin)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 17);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!string.IsNullOrEmpty(numsolicitud))
                {
                    coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                }
                if (!string.IsNullOrEmpty(fechaini) && !string.IsNullOrEmpty(fechafin))
                {
                    coman.Parameters.AddWithValue("@FECDESDE", fechaini);
                    coman.Parameters.AddWithValue("@FECHASTA", fechafin);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudFacturas", ruccipas, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDatosEmpresa(string numsolicitud, string estado)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 54);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@ESTADO", estado);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetOtrosDocumentos", "33", "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetOtrosDocumentos()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 33);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetOtrosDocumentos", "33", "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetTipoDeDocumentos(string ruccipas, string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 18);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!string.IsNullOrEmpty(numsolicitud))
                {
                    coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudFacturas", ruccipas, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetTipoDeDocumentosColaborador(string ruccipas, string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 18);
                //coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!string.IsNullOrEmpty(numsolicitud))
                {
                    coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudFacturas", ruccipas, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetTipoDeDocumentosVehiculo(string ruccipas, string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 49);
                //coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!string.IsNullOrEmpty(numsolicitud))
                {
                    coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudFacturas", ruccipas, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudesVehiculo(string placa)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_SOLICITUDES_VEHICULO";
                coman.Parameters.AddWithValue("@PLACA", placa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesVehiculo", placa, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetEmpresaVehiculo(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 51);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetEmpresaVehiculo", numsolicitud, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetEmpresaColaborador(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 52);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetEmpresaColaborador", numsolicitud, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudesColaborador(string numsolicitud, string cedula, string rucempresa, string fecdesde, string fechasta, bool todos)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_SOLICITUDES_COLABORADOR";
                if (!todos)
                {
                    if (!string.IsNullOrEmpty(numsolicitud))
                    {
                        coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                    }
                    if (!string.IsNullOrEmpty(rucempresa))
                    {
                        coman.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                    }
                    coman.Parameters.AddWithValue("@CEDULA", cedula);
                    if (!string.IsNullOrEmpty(fecdesde))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechaini;
                        if (!DateTime.TryParseExact(fecdesde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECDESDE", dfechaini.ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(fechasta))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechafin;
                        if (!DateTime.TryParseExact(fechasta, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECHASTA", dfechafin.ToString("yyyy-MM-dd"));
                    }
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesColaborador", cedula, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDatosColaborador(string rucempresa, string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_DATOS_COLABORADOR_AC";
                coman.Parameters.AddWithValue("@RUC", rucempresa);
                if (!string.IsNullOrEmpty(cedula))
                {
                    coman.Parameters.AddWithValue("@CEDULA", cedula);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDatosColaborador", cedula, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDatosVehiculo(string rucempresa, string placa)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_DATOS_VEHICULO_AC";
                coman.Parameters.AddWithValue("@RUC", rucempresa);
                if (!string.IsNullOrEmpty(placa))
                {
                    coman.Parameters.AddWithValue("@PLACA", placa);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDatosVehiculo", placa, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetPermisoAccesoColaborador(string cedula, string rucempresa)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_PERMISO_ACCESO";
                coman.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                coman.Parameters.AddWithValue("@CEDULA", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesColaborador", cedula, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetPermisoColaborador(string numsolicitud, string cedula, string rucempresa, string fecdesde, string fechasta, bool todos)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_PERMISO_COLABORADOR";
                if (!todos)
                {
                    if (!string.IsNullOrEmpty(numsolicitud))
                    {
                        coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                    }
                    if (!string.IsNullOrEmpty(rucempresa))
                    {
                        coman.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                    }
                    coman.Parameters.AddWithValue("@CEDULA", cedula);
                    if (!string.IsNullOrEmpty(fecdesde))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechaini;
                        if (!DateTime.TryParseExact(fecdesde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECDESDE", dfechaini.ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(fechasta))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechafin;
                        if (!DateTime.TryParseExact(fechasta, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECHASTA", dfechafin.ToString("yyyy-MM-dd"));
                    }
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesColaborador", cedula, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudAccesoLanchas(bool todos, string numsolicitud, string ruccipas, string tipoestado, string fecdesde, string fechasta)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "MDL_CONSULTA_SOLICITUD_ACCESO_LANCHAS";
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!todos)
                {
                    if (!string.IsNullOrEmpty(numsolicitud))
                    {
                        coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                    }
                    if (!string.IsNullOrEmpty(tipoestado))
                    {
                        coman.Parameters.AddWithValue("@IDESTADO", tipoestado);
                    }
                    if (!string.IsNullOrEmpty(fecdesde))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechaini;
                        if (!DateTime.TryParseExact(fecdesde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECDESDE", dfechaini.ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(fechasta))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechafin;
                        if (!DateTime.TryParseExact(fechasta, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECHASTA", dfechafin.ToString("yyyy-MM-dd"));
                    }
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesCliente", numsolicitud, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudLanchas(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "MDL_PROCESO_LANCHAS";
                coman.Parameters.AddWithValue("@TIPO", 3);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudes(bool todos, string ruccipas, string numsolicitud, string tiposolicitud, string tipoestado, string fecdesde, string fechasta)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_SOLICITUDES";
                if (!todos)
                {
                    if (!string.IsNullOrEmpty(ruccipas))
                    {
                        coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                    }
                    if (!string.IsNullOrEmpty(numsolicitud))
                    {
                        coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                    }
                    if (!string.IsNullOrEmpty(tiposolicitud))
                    {
                        coman.Parameters.AddWithValue("@IDTIPSOL", tiposolicitud);
                    }
                    if (!string.IsNullOrEmpty(tipoestado))
                    {
                        coman.Parameters.AddWithValue("@IDESTADO", tipoestado);
                    }
                    if (!string.IsNullOrEmpty(fecdesde))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechaini;
                        if (!DateTime.TryParseExact(fecdesde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECDESDE", dfechaini.ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(fechasta))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechafin;
                        if (!DateTime.TryParseExact(fechasta, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECHASTA", dfechafin.ToString("yyyy-MM-dd"));
                    }
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudes", numsolicitud + "|" + tiposolicitud + "|" + fecdesde + "|" + fechasta, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudesEmpresa(bool todos, string ruccipas, string numsolicitud, string tipoestado, string fecdesde, string fechasta)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_SOLICITUDES_EMPRESA";
                if (!todos)
                {
                    if (!string.IsNullOrEmpty(ruccipas))
                    {
                        coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                    }
                    if (!string.IsNullOrEmpty(numsolicitud))
                    {
                        coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                    }
                    if (!string.IsNullOrEmpty(tipoestado))
                    {
                        coman.Parameters.AddWithValue("@IDESTADO", tipoestado);
                    }
                    if (!string.IsNullOrEmpty(fecdesde))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechaini;
                        if (!DateTime.TryParseExact(fecdesde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECDESDE", dfechaini.ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(fechasta))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechafin;
                        if (!DateTime.TryParseExact(fechasta, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECHASTA", dfechafin.ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesEmpresa", numsolicitud + "|" + fecdesde + "|" + fechasta, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudesCliente(bool todos, string numsolicitud, string ruccipas, string tipoestado, string fecdesde, string fechasta)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_ESTADO_SOLICITUD";
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!todos)
                {
                    if (!string.IsNullOrEmpty(numsolicitud))
                    {
                        coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                    }
                    if (!string.IsNullOrEmpty(tipoestado))
                    {
                        coman.Parameters.AddWithValue("@IDESTADO", tipoestado);
                    }
                    if (!string.IsNullOrEmpty(fecdesde))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechaini;
                        if (!DateTime.TryParseExact(fecdesde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECDESDE", dfechaini.ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(fechasta))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dfechafin;
                        if (!DateTime.TryParseExact(fechasta, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                        {
                        }
                        coman.Parameters.AddWithValue("@FECHASTA", dfechafin.ToString("yyyy-MM-dd"));
                    }
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesCliente", numsolicitud, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentacionTesoreria(string ruccipas, string numsolicitud, bool todos)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 30);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!todos)
                {
                    coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudesCliente", numsolicitud, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDatosExportacion()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 5);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDatosExportacion", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosEmpresa(string xml)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 0);
                coman.Parameters.AddWithValue("@XMLTIPEMP", xml);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosEmpresa", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosOtros(string tipodoc)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 34);
                coman.Parameters.AddWithValue("@TIPODOC", tipodoc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosOtros", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosColaborador(/*string xml, */string ruccipas, string tipoempresa)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 7);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                coman.Parameters.AddWithValue("@IDTIPSOL", tipoempresa);
                //coman.Parameters.AddWithValue("@XMLTIPEMP", xml);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaborador", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosColaboradorPaseProvisional(string ruccipas, string tipo)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 45);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                coman.Parameters.AddWithValue("@TIPODOC", tipo);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaboradorPaseProvisional", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosVehiculo(/*string xml, */string ruccipas, string tipoempresa, string categoria)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 8);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                coman.Parameters.AddWithValue("@IDTIPSOL", tipoempresa);
                coman.Parameters.AddWithValue("@CATEGORIA", categoria);
                //coman.Parameters.AddWithValue("@XMLTIPEMP", xml);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDescripcionTipoSolicitudXNumSolicitud(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 24);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDescripcionTipoSolicitudXNumSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosEmpresaXNumSolicitud(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 5);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosEmpresaXNumSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosEmpresaXNumSolicitudCliente(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 29);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosEmpresaXNumSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosVehiculoXNumSolicitud(string numsolicitud, string idsolveh)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 10);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolveh);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosVehiculoXNumSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosVehiculoXNumSolicitudCliente(string numsolicitud, string idsolveh)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 28);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolveh);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosVehiculoXNumSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosColaboradorXNumSolicitud(string numsolicitud, string idsolcol)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 14);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolcol);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaboradorXNumSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosColaboradorXNumSolicitudPaseProvisional(string numsolicitud, string idsolcol)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 60);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLCOL", idsolcol);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaboradorXNumSolicitudPaseProvisional", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosColaboradorXNumSolicitudCliente(string numsolicitud, string idsolcol)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 26);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolcol);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaboradorXNumSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudEmpresa(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 6);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudEmpresa", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudVehiculo(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 9);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudVehiculoCliente(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 27);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudVehiculoOnlyControl(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 21);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetEstadoEmpresa(string ruc)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 53);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetEstadoEmpresa", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudVehiculoDetalle(string numsolicitud, string placa)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 16);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@PLACA", placa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudColaborador(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 13);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudColaboradorPermisoProvisional(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 46);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudPermisosDeAcceso(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 35);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudPermisosDeAcceso", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudPermisosDeAccesoVehiculo(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 59);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudPermisosDeAccesoVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudColaboradorCliente(string numsolicitud, string ruccipas)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 25);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetSolicitudColaboradorOnlyControl(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 20);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetColaboradores(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 32);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetColaboradoresSolicitudPermisosDeAcceso(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 37);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaDatosRepresentanteLegal(string ruc)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 55);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaDatosRepresentanteLegal", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaDatosColaboradorSCA(string ruc, string cedula)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 56);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruc);
                coman.Parameters.AddWithValue("@CIPAS", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaDatosRepresentanteLegal", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaColaboradorXCedula(string cedula)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 57);
                coman.Parameters.AddWithValue("@CIPAS", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaDatosRepresentanteLegal", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetActividadPermitida()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 62);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetActividadPermitida", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetActividadPermitidaPermisoDeAcceso()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 63);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudFacturas", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GeDatosEmail(string numsolicitud, string xmlcol, string usuario, string xmldoc)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 65);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@USUARIO_ING", usuario);
                coman.Parameters.AddWithValue("@XMLVEHICULOS", xmlcol);
                coman.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldoc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GeDatosEmail", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static int GetTiempoCaducidadCredencialPermanente()
        {
            int valor = 0;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 40);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@TIEMPOCADUCIDAD";
                psql.SqlDbType = SqlDbType.BigInt;
                psql.Size = 1;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = Convert.ToInt32(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static string GetConsultaEmpresa(string ruccipas)
        {
            string valempresa = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 15);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@VALIDAEMPRESA";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 3;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valempresa = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetValSeccion", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valempresa;
        }
        public static string GetTipoCredencial(string numsolicitud)
        {
            string valor = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 38);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@TIPOCREDENCIAL";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 3;
                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetTipoCredencial", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static string GetInfoTipoCredencial()
        {
            string valor = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", "CTC");
                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@INFOTIPCRE";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 50000;
                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static string GetTipoSolicitudXNumSolicitud(string numsolicitud)
        {
            string valor = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 19);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@CODIGO";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 1;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static string GetValSeccion(string xml)
        {
            string valseccion = null;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 1);
                coman.Parameters.AddWithValue("@XMLTIPEMP", xml);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@VALTIPEMP";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 3;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valseccion = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetValSeccion", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valseccion;
        }
        public static string GetDateServer()
        {
            string valor = null;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 2);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@GETDATESERVER";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 8;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static string GetTiempoPermisoPermanente()
        {
            string valor = null;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", "CPT");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@PERMISOPERMANENTE";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 8;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static string GetConsultaScriptNomina()
        {
            string script = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 41);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SCRIPT_NOMINA";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 900000;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    script = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaScriptNomina", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return script;
        }
        public static DataTable GetConsultaNomina(string ruc, string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 1);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULA", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNomina", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaNominaPeaton(string ruc, string cedula, string nombres, string apellidos)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 10);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                coman.Parameters.AddWithValue("@APELLIDOSCON", apellidos);
                coman.Parameters.AddWithValue("@NOMBRESCON", nombres);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNominaPeaton", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaVehiculo(string ruc, string placa)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 6);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@PLACAV", placa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNomina", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaVehiculosXEmpresaMda(string ruc, string placa)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 7);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@PLACAV", placa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNomina", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaConductoresXEmpresaMda(string ruc, string cedula, string nombres, string apellidos)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 9);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                coman.Parameters.AddWithValue("@APELLIDOSCON", apellidos);
                coman.Parameters.AddWithValue("@NOMBRESCON", nombres);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaConductoresXEmpresaMda", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaVehiculosXEmpresaLivianosMda(string ruc)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 8);
                coman.Parameters.AddWithValue("@RUC", ruc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNomina", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaVehiculosXEmpresaSca(string ruc, string placa)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 58);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruc);
                coman.Parameters.AddWithValue("@PLACA", placa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNomina", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaNominaPermiso(string ruc, string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 4);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULA", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNomina", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetDocumentosColaboradorLanchas()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "MDL_PROCESO_LANCHAS";
                coman.Parameters.AddWithValue("@TIPO", 1);
                //coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                //coman.Parameters.AddWithValue("@IDTIPSOL", tipoempresa);
                //coman.Parameters.AddWithValue("@XMLTIPEMP", xml);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaborador", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaDiasSolicitud()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 68);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaDiasSolicitud", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static string GetConsultaScriptNom_Puerta()
        {
            string script = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 42);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SCRIPT_NOM_PUERTA";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 900000;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    script = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaScriptNom_Puerta", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return script;
        }
        public static DataTable GetConsultaNom_Puerta(string id)
        {
            var d = new DataTable();
            using (var c = conexionOnlyAccess())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 2);
                coman.Parameters.AddWithValue("@NOM_ID", id);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaNom_Puerta", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static string GetConsultaScriptTurno()
        {
            string script = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 44);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SCRIPT_TBLTURNO";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 900000;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    script = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaScriptTurno", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return script;
        }
        public static DataTable GetConsultaTurno()
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 3);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaTurno", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static DataTable GetConsultaArea()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 64);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaArea", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static string GetConsultaScriptAc_Nomina()
        {
            string script = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 48);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SCRIPT_AC_NOMINA";
                psql.SqlDbType = SqlDbType.VarChar;
                psql.Size = 900000;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    script = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaScriptDetallePP", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return script;
        }
        public static DataTable GetConsultaAc_Nomina(string script, string idnom)
        {
            var d = new DataTable();
            using (var c = conexionOnlyAccess())
            {
                var coman = c.CreateCommand();
                coman.CommandText = script + "'" + idnom + "'";
                coman.CommandType = CommandType.Text;
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetConsultaDetallePP", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        public static bool GetSolicitudEstado(string numsolicitud)
        {
            bool valor = false;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 11);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SOLICITUDESTADO";
                psql.SqlDbType = SqlDbType.Bit;
                psql.Size = 1;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = Convert.ToBoolean(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static bool GetTipoSolicitud(string numsolicitud)
        {
            bool valor = false;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 12);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SOLICITUDESTADO";
                psql.SqlDbType = SqlDbType.Bit;
                psql.Size = 1;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = Convert.ToBoolean(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static bool GetCTipoCredencial(string numsolicitud)
        {
            bool valor = false;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 50);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SOLICITUDESTADO";
                psql.SqlDbType = SqlDbType.Bit;
                psql.Size = 1;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = Convert.ToBoolean(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static bool GetTipoSolicitudXRUC(string ruc, string tipoempresa)
        {
            bool valor = false;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 22);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruc);
                coman.Parameters.AddWithValue("@IDTIPSOL", tipoempresa);
                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SOLICITUDESTADO";
                psql.SqlDbType = SqlDbType.Bit;
                psql.Size = 1;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = Convert.ToBoolean(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static bool GetTipoSolicitudXRUCOPC(string ruc, string tipoempresa)
        {
            bool valor = false;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 61);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruc);
                coman.Parameters.AddWithValue("@IDTIPSOL", tipoempresa);
                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SOLICITUDESTADO";
                psql.SqlDbType = SqlDbType.Bit;
                psql.Size = 1;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    valor = Convert.ToBoolean(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return valor;
        }
        public static bool AddSolicitudEmpresa(
        string useremail,
        string xmltiposolicitud,
        string txtrazonsocial,
        string txtruccipas,
        string txtactividadcomercial,
        string txtdireccion,
        string txttelofi,
        string txtcontacto,
        string txttelcelcon,
        string emailsec2,
        string txtcertificaciones,
        string turl,
        string txtafigremios,
        string txtrefcom,
        string txtreplegal,
        string txttelreplegal,
        string txtdirdomreplegal,
        string txtci,
        string emailsec3,
        string tmailebilling,
            //string xmlDatosExpo,
            //string xmlLogisticaExpo,
            //string txtgps,
        string xmlDocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_EMPRESA";
                            //comando.Parameters.AddWithValue("@XMLDATOSEXPO", xmlDatosExpo);
                            //comando.Parameters.AddWithValue("@XMLLOGISTICAEXPO", xmlLogisticaExpo);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@XMLTIPOSOLICITUD", xmltiposolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", txtrazonsocial);
                            comando.Parameters.AddWithValue("@RUCCIPAS", txtruccipas);
                            comando.Parameters.AddWithValue("@ACTIVIDADCOM", txtactividadcomercial);
                            comando.Parameters.AddWithValue("@DIRECCIONOFI", txtdireccion);
                            comando.Parameters.AddWithValue("@TELFOFI", txttelofi);
                            comando.Parameters.AddWithValue("@CONTACTO", txtcontacto);
                            comando.Parameters.AddWithValue("@TELFCON", txttelcelcon);
                            comando.Parameters.AddWithValue("@EMAILCLI", emailsec2);
                            comando.Parameters.AddWithValue("@CERTIFICACIONES", txtcertificaciones);
                            comando.Parameters.AddWithValue("@PAGINAWEB", turl);
                            comando.Parameters.AddWithValue("@AFIGREMIOS", txtafigremios);
                            comando.Parameters.AddWithValue("@REFERENCIACOM", txtrefcom);
                            comando.Parameters.AddWithValue("@NOMREPLEGAL", txtreplegal);
                            comando.Parameters.AddWithValue("@TELFREPLEGAL", txttelreplegal);
                            comando.Parameters.AddWithValue("@DIRDOMREPLEGAL", txtdirdomreplegal);
                            comando.Parameters.AddWithValue("@CIREPLEGAL", txtci);
                            //comando.Parameters.AddWithValue("@GPSCUSTOPROD", txtgps);
                            comando.Parameters.AddWithValue("@EMAILREPLEGAL", emailsec3);
                            comando.Parameters.AddWithValue("@EMAILEBILLING", tmailebilling);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);

                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudColaboradorCab(
        string tiposolicitud,
        string usuario,
        out string mensaje,
        out Int32 identity)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_CAB";
                            comando.Parameters.AddWithValue("@TIPOSOLICITUD", tiposolicitud);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            comando.Parameters.Add("@NEWIDSOLICITUD", SqlDbType.BigInt).Direction = ParameterDirection.Output; 
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            identity = Convert.ToInt32(comando.Parameters["@NEWIDSOLICITUD"].Value);
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaboradorCab", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        identity = 0;
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaboradorCab", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                identity = 0;
                return false;
            }
        }
        public static bool AddSolicitudColaboradorDet(
        string nombreempresa,
        Int32 numsolicitud,
        string useremail,
        string tiposolicitud,
        string rucempresa,
        string xmlColaboradores,
        string xmlDocumentos,
        string usuario,
        bool   enviaemail,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_DET";
                            comando.Parameters.AddWithValue("@NEWIDSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@TIPOSOLICITUD", tiposolicitud);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlColaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            comando.Parameters.AddWithValue("@ENVIAEMAIL", enviaemail);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaboradorDet", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaboradorDet", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudColaboradorPaseProvisional(
        string nombreempresa,
        string cedulanombrereplegal,
        string nombrereplegal,
        string fechainicio,
        string fechacaducidad,
        string useremail,
        string tiposolicitud,
        string rucempresa,
        string area,
        string actividad,
        string xmlColaboradores,
        string xmlDocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_PASE_PROVISIONAL";
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@CEDULAREPLEGAL", cedulanombrereplegal);
                            comando.Parameters.AddWithValue("@NOMBRESREPLEGAL", nombrereplegal);
                            comando.Parameters.AddWithValue("@FECHAINICIO", fechainicio);
                            comando.Parameters.AddWithValue("@FECHACADUCIDAD", fechacaducidad);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            //comando.Parameters.AddWithValue("@TIPOSOLICITUD", tiposolicitud);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@AREA", area);
                            comando.Parameters.AddWithValue("@ACTIVIDAD", actividad);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlColaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaboradorPaseProvisional", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaboradorPaseProvisional", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudVehiculoCab(
        string tiposolicitud,
        string usuario,
        out string mensaje,
        out Int32 identity)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_VEHICULO_CAB";
                            comando.Parameters.AddWithValue("@TIPOSOLICITUD", tiposolicitud);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            comando.Parameters.Add("@NEWIDSOLICITUD", SqlDbType.BigInt).Direction = ParameterDirection.Output; 
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            identity = Convert.ToInt32(comando.Parameters["@NEWIDSOLICITUD"].Value);
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudVehiculoCab", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        identity = 0;
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudVehiculoCab", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                identity = 0;
                return false;
            }
        }
        public static bool AddSolicitudVehiculoDet(
        string nombreempresa,
        Int32 numsolicitud,
        string useremail,
        string tiposolicitud,
        string tipoempresa,
        string rucempresa,
        string xmlVehiculos,
        string xmlDocumentos,
        string usuario,
        bool enviaemail,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_VEHICULO_DET";
                            comando.Parameters.AddWithValue("@NEWIDSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@TIPOSOLICITUD", tiposolicitud);
                            comando.Parameters.AddWithValue("@TIPOEMPRESA", tipoempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@XMLVEHICULOS", xmlVehiculos);
                            comando.Parameters.AddWithValue("@ENVIAEMAIL", enviaemail);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ApruebaSolicitudVehiculo(
        string numsolicitud,
        string rucempresa,
            //string nombreempresa,
            //string useremail,
        string usuario,
        string xmlvehiculos,
        string xmldocumentos,
        string xmldocumentosR,
        bool banderafac,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_VEHICULO";
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            //comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            //comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOSR", xmldocumentosR);
                            comando.Parameters.AddWithValue("@XMLVEHICULOS", xmlvehiculos);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool RechazaSolicitudVehiculo(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlvehiculos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_VEHICULO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLVEHICULOS", xmlvehiculos);
                            //comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool FinalizaSolicitudEmpresa(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string usuario,
        string ip,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            var password = GetPassAlfaNumeric();
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_EMPRESA";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@PASSWORD", "01"/*password*/);
                            usuario usero = new usuario();
                            usero.loginname = rucempresa;
                            comando.Parameters.AddWithValue("@PASSWORDENCRYPT", "01"/*usero.autenticate(password)*//*QuerySegura.EncryptQueryString(password.ToString())*/);
                            comando.Parameters.AddWithValue("@IPCREACION", ip);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "FinalizaSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "FinalizaSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ActualizaSolicitudEmpresa(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string usuario,
        string ip,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            var password = GetPassAlfaNumeric();
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_APROBACION_ACTUALIZA_SOLICITUD_EMPRESA";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@PASSWORD", "01"/*password*/);
                            usuario usero = new usuario();
                            usero.loginname = rucempresa;
                            comando.Parameters.AddWithValue("@PASSWORDENCRYPT", "01"/*usero.autenticate(password)*//*QuerySegura.EncryptQueryString(password.ToString())*/);
                            comando.Parameters.AddWithValue("@IPCREACION", ip);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "FinalizaSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "FinalizaSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool RechazaSolicitudEmpresa(
        string encryptnumsolicitud,
            //string clave,
        string numsolicitud,
        string motivorechazo,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlempresa,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            var password = GetPassAlfaNumeric();
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_EMPRESA";
                            comando.Parameters.AddWithValue("@ENCRYPTNUMSOLICITUD", encryptnumsolicitud);
                            comando.Parameters.AddWithValue("@MOTIVORECHAZO", motivorechazo);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@XMLEMPRESA", xmlempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@PASSWORD", password.ToString());
                            comando.Parameters.AddWithValue("@PASSWORDENCRYPT", QuerySegura.EncryptQueryString(password.ToString()));
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ApruebaSolicitudColaborador(
        string numsolicitud,
        string rucempresa,
            //string nombreempresa,
            //string useremail,
        string xmlcolaboradores,
        string xmldocumentos,
        string xmldocrechazados,
        string usuario,
        bool banderafac,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_COLABORADOR";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOSREC", xmldocrechazados);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ApruebaSolicitudColaboradorPaseProvisional(
        string numsolicitud,
        string rucempresa,
            //string nombreempresa,
            //string useremail,
        string xmlcolaboradores,
        string xmldocumentos,
        string xmlacceso,
        string usuario,
        bool banderafac,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_COLABORADOR_PASE_PROVISIONAL";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@XMLACCESO", xmlacceso);
                            // comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            //comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaboradorPaseProvisional", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool RechazaSolicitudColaborador(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlcolaboradores,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_COLABORADOR";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool RechazaSolicitudColaboradorPermisoDeAcceso(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlcolaboradores,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_PERMISO_DE_ACCESO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool RechazaSolicitudColaboradorPermisoDeAccesoVehiculo(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlcolaboradores,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_PERMISO_DE_ACCESO_VEHICULO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudColaboradorPermisoDeAccesoVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudColaboradorPermisoDeAccesoVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool RechazaSolicitudColaboradorPaseProvisional(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlcolaboradores,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_COLABORADOR_PASE_PROVISIONAL";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool RechazaSolicitudConfirmacionDePago(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string comentario,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_RECHAZA_PAGO_CONFIRMADO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@COMENTARIO", comentario);
                            //comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
                            //comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            //comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudConfirmacionDePago", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "RechazaSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddConfirmacionDePago(
        string numsolicitud,
        string razsocial,
        string ruccipas,
        string emailuser,
        string xmldocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ENVIA_COMPROBANTE_DE_PAGO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", razsocial);
                            comando.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                            comando.Parameters.AddWithValue("@EMAILUSER", emailuser);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddPagoConfirmado(
        string numsolicitud,
        string razsocial,
        string ruccipas,
        string emailuser,
        string usuario,
        string numdoc,
        string fecing,
        string fecsal,
        string area,
        string departamento,
        string turno,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_PAGO_CONFIRMADO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", razsocial);
                            comando.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                            comando.Parameters.AddWithValue("@EMAILUSER", emailuser);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            comando.Parameters.AddWithValue("@NUMERODOCUMENTO", numdoc);
                            comando.Parameters.AddWithValue("@AREA", area);
                            comando.Parameters.AddWithValue("@DPTO", departamento);
                            comando.Parameters.AddWithValue("@TURNO", turno);
                            if (!string.IsNullOrEmpty(fecing))
                            {
                                CultureInfo enUS = new CultureInfo("en-US");
                                DateTime dfechaini;
                                if (!DateTime.TryParseExact(fecing, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                                {
                                }
                                comando.Parameters.AddWithValue("@FECHAINGRESO", dfechaini.ToString("yyyy-MM-dd"));
                            }
                            if (!string.IsNullOrEmpty(fecsal))
                            {
                                CultureInfo enUS = new CultureInfo("en-US");
                                DateTime dfechafin;
                                if (!DateTime.TryParseExact(fecsal, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                                {
                                }
                                comando.Parameters.AddWithValue("@FECHASALIDA", dfechafin.ToString("yyyy-MM-dd"));
                            }
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ReenviaSolicitudEmpresa(
        string idusuario,
        string numsolicitud,
        string txtrazonsocial,
        string txtruccipas,
        string txtactividadcomercial,
        string txtdireccion,
        string txttelofi,
        string txtcontacto,
        string txttelcelcon,
        string emailsec2,
        string txtcertificaciones,
        string turl,
        string txtafigremios,
        string txtrefcom,
        string txtreplegal,
        string txttelreplegal,
        string txtdirdomreplegal,
        string txtci,
        string emailsec3,
        string tmailebilling,
            //string xmlDatosExpo,
            //string xmlLogisticaExpo,
            //string txtgps,
        string xmlDocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_REENVIA_SOLICITUD_EMPRESA";
                            //comando.Parameters.AddWithValue("@XMLDATOSEXPO", xmlDatosExpo);
                            //comando.Parameters.AddWithValue("@XMLLOGISTICAEXPO", xmlLogisticaExpo);
                            comando.Parameters.AddWithValue("@IDUSUARIO", idusuario);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            //comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", txtrazonsocial);
                            comando.Parameters.AddWithValue("@RUCCIPAS", txtruccipas);
                            comando.Parameters.AddWithValue("@ACTIVIDADCOM", txtactividadcomercial);
                            comando.Parameters.AddWithValue("@DIRECCIONOFI", txtdireccion);
                            comando.Parameters.AddWithValue("@TELFOFI", txttelofi);
                            comando.Parameters.AddWithValue("@CONTACTO", txtcontacto);
                            comando.Parameters.AddWithValue("@TELFCON", txttelcelcon);
                            comando.Parameters.AddWithValue("@EMAILCLI", emailsec2);
                            comando.Parameters.AddWithValue("@CERTIFICACIONES", txtcertificaciones);
                            comando.Parameters.AddWithValue("@PAGINAWEB", turl);
                            comando.Parameters.AddWithValue("@AFIGREMIOS", txtafigremios);
                            comando.Parameters.AddWithValue("@REFERENCIACOM", txtrefcom);
                            comando.Parameters.AddWithValue("@NOMREPLEGAL", txtreplegal);
                            comando.Parameters.AddWithValue("@TELFREPLEGAL", txttelreplegal);
                            comando.Parameters.AddWithValue("@DIRDOMREPLEGAL", txtdirdomreplegal);
                            comando.Parameters.AddWithValue("@CIREPLEGAL", txtci);
                            //comando.Parameters.AddWithValue("@GPSCUSTOPROD", txtgps);
                            comando.Parameters.AddWithValue("@EMAILREPLEGAL", emailsec3);
                            comando.Parameters.AddWithValue("@EMAILEBILLING", tmailebilling);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "UpdateSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "UpdateSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool UpdateSolicitudEmpresa(
        string numsolicitud,
        string txtrazonsocial,
        string txtruccipas,
        string txtactividadcomercial,
        string txtdireccion,
        string txttelofi,
        string txtcontacto,
        string txttelcelcon,
        string emailsec2,
        string txtcertificaciones,
        string turl,
        string txtafigremios,
        string txtrefcom,
        string txtreplegal,
        string txttelreplegal,
        string txtdirdomreplegal,
        string txtci,
        string emailsec3,
        string tmailebilling,
            //string xmlDatosExpo,
            //string xmlLogisticaExpo,
            //string txtgps,
        string xmlDocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ACTUALIZA_SOLICITUD_EMPRESA";
                            //comando.Parameters.AddWithValue("@XMLDATOSEXPO", xmlDatosExpo);
                            //comando.Parameters.AddWithValue("@XMLLOGISTICAEXPO", xmlLogisticaExpo);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            //comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", txtrazonsocial);
                            comando.Parameters.AddWithValue("@RUCCIPAS", txtruccipas);
                            comando.Parameters.AddWithValue("@ACTIVIDADCOM", txtactividadcomercial);
                            comando.Parameters.AddWithValue("@DIRECCIONOFI", txtdireccion);
                            comando.Parameters.AddWithValue("@TELFOFI", txttelofi);
                            comando.Parameters.AddWithValue("@CONTACTO", txtcontacto);
                            comando.Parameters.AddWithValue("@TELFCON", txttelcelcon);
                            comando.Parameters.AddWithValue("@EMAILCLI", emailsec2);
                            comando.Parameters.AddWithValue("@CERTIFICACIONES", txtcertificaciones);
                            comando.Parameters.AddWithValue("@PAGINAWEB", turl);
                            comando.Parameters.AddWithValue("@AFIGREMIOS", txtafigremios);
                            comando.Parameters.AddWithValue("@REFERENCIACOM", txtrefcom);
                            comando.Parameters.AddWithValue("@NOMREPLEGAL", txtreplegal);
                            comando.Parameters.AddWithValue("@TELFREPLEGAL", txttelreplegal);
                            comando.Parameters.AddWithValue("@DIRDOMREPLEGAL", txtdirdomreplegal);
                            comando.Parameters.AddWithValue("@CIREPLEGAL", txtci);
                            //comando.Parameters.AddWithValue("@GPSCUSTOPROD", txtgps);
                            comando.Parameters.AddWithValue("@EMAILREPLEGAL", emailsec3);
                            comando.Parameters.AddWithValue("@EMAILEBILLING", tmailebilling);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "UpdateSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "UpdateSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudPermisosDeAcceso(
        string nombreempresa,
        string useremail,
        string rucempresa,
        string xmlPermisosDeAcceso,
        string xmlDocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_PERMISO_DE_ACCESO";
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@XMLPERMISOSDEACCESO", xmlPermisosDeAcceso);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudPermisosDeAccesoVehiculo(
        string nombreempresa,
        string useremail,
        string rucempresa,
        string xmlPermisosDeAcceso,
        string xmlDocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_PERMISO_DE_ACCESO_VEHICULO";
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@XMLPERMISOSDEACCESO", xmlPermisosDeAcceso);
                            //comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudPermisosDeAccesoVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudPermisosDeAccesoVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ApruebaSolicitudPermisoDeAcceso(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlcolaboradores,
        string xmldocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_PERMISOS_DE_ACCESO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            //comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            //comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            //comando.Parameters.AddWithValue("@BANDERAFAC", true);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ApruebaSolicitudPermisoDeAccesoVehiculo(
        string numsolicitud,
        string rucempresa,
        string nombreempresa,
        string useremail,
        string xmlcolaboradores,
        string xmldocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_PERMISOS_DE_ACCESO_VEHICULO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLVEHICULOS", xmlcolaboradores);
                            //comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            //comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            //comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            //comando.Parameters.AddWithValue("@BANDERAFAC", true);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudAccesoLanchas(
        string nombreempresa,
        string useremail,
        string rucempresa,
        string xmlColaboradores,
        string xmlDocumentos,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.MDL_ADD_SOLICITUD_LANCHA";
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlColaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddModeloVehiculo(
        string marca,
        string modelo,
        out string mensaje)
        {
            try
            {
                using (var conn = conexionMDA())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_CREDENCIALES";
                            comando.Parameters.AddWithValue("@TIPO", 11);
                            comando.Parameters.AddWithValue("@VMO_MARCA", marca);
                            comando.Parameters.AddWithValue("@VMO_MODELO", modelo);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddModeloVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool UpdateCargoConductor(
        string cedula,
        out string mensaje)
        {
            try
            {
                using (var conn = conexionMDA())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_CREDENCIALES";
                            comando.Parameters.AddWithValue("@TIPO", 13);
                            comando.Parameters.AddWithValue("@CEDULA", cedula);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "UpdateCargoConductor", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool UpdateUCREVehiculo(
        string placa,
        string marca,
        string modelo,
        out string mensaje)
        {
            try
            {
                using (var conn = conexionMDA())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SCA_CREDENCIALES";
                            comando.Parameters.AddWithValue("@TIPO", 12);
                            comando.Parameters.AddWithValue("@PLACAV", placa);
                            comando.Parameters.AddWithValue("@VMO_MARCA", marca);
                            comando.Parameters.AddWithValue("@VMO_MODELO", modelo);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "UpdateUCREVehiculo", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool ActualizaRazonSocialEmpresa(
        string numsolicitud,
        string razonsocial,
        out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            var password = GetPassAlfaNumeric();
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "SCA_PROCESO_CREDENCIALES";
                            comando.Parameters.AddWithValue("@TIPO", 69);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", razonsocial);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizaRazonSocialEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizaRazonSocialEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
    }
    public class ttipocli
    {
        public string id { get; set; }
    }
}