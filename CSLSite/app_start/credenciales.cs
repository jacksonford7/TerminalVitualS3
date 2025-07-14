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
using BillionEntidades;
using SqlConexion;
using Configuraciones;
using N4Ws.Entidad;
using System.Reflection;
using Newtonsoft.Json;

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
            string descripcionempresa = string.Empty;
            error = string.Empty;
            try
            {
                String error_consulta = string.Empty;
                error = string.Empty;
                descripcionempresa = onlyControl.AC_C_EMPRESA(rucempresa, 0, ref error_consulta).DefaultViewManager.DataSet.Tables[0].Rows[0]["EMPE_NOM"].ToString();
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    error = error_consulta;
                }
            }
            catch(Exception ex)
            {
                error = "NO SE HA PODIDO OBTENER LA DESCRIPCION DE LA EMPRESA DESDE OC, " + ex.Message;
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
        public static DataTable GetSolicitudes(bool todos, string ruccipas, string numsolicitud, string tiposolicitud, string tipoestado, string fecdesde, string fechasta, string asotrans)
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
                    if (!string.IsNullOrEmpty(asotrans))
                    {
                        coman.Parameters.AddWithValue("@ASOTRANS", asotrans);
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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

        public static DataTable GetDocumentosVehiculoXNumSolicitud_New(string numsolicitud, string idsolveh)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "TRANSP_SCA_DOCUMENTOS_SOLICITUD_VEICULO";
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolveh);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosVehiculoXNumSolicitud_New", DateTime.Now.ToShortDateString(), "gs");
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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

        public static DataTable GetDocumentosVehiculoXNumSolicitudCliente_New(string numsolicitud, string idsolveh)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "TRANSP_SCA_DOCUMENTOS_SOLICITUD_VEHICULO_CLIENTE";
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolveh);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosVehiculoXNumSolicitudCliente_New", DateTime.Now.ToShortDateString(), "gs");
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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

        public static DataTable GetDocumentosColaboradorXNumSolicitud_New(string numsolicitud, string idsolcol)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "TRANSP_SCA_DOCUMENTOS_SOLICITUD_COLABORADOR";
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolcol);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaboradorXNumSolicitud_New", DateTime.Now.ToShortDateString(), "gs");
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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
                coman.CommandText = "SCA_PROCESO_CREDENCIALES_TV";
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

        public static DataTable GetDocumentosColaboradorXNumSolicitudCliente_New(string numsolicitud, string idsolcol)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "TRANSP_SCA_DOCUMENTOS_SOLICITUD_COLABORADOR_CLIENTE";
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@IDSOLVEH", idsolcol);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetDocumentosColaboradorXNumSolicitudCliente_New", DateTime.Now.ToShortDateString(), "gs");
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

        public static DataTable GetSolicitudVehiculo_New(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "TRANSP_SCA_CONSULTA_SOLCITUD_VEHICULO";
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudVehiculo_New", DateTime.Now.ToShortDateString(), "gs");
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

        public static DataTable GetSolicitudColaborador_New(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "TRANSP_SCA_CONSULTA_SOLICITUD_COLABORADOR";
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudColaborador_New", DateTime.Now.ToShortDateString(), "gs");
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
        public static string GetTipoSolicitudXNumSolicitudAsoTrans(string numsolicitud)
        {
            string valor = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_PROCESO_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 71);
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
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_DET_TV";
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
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_PASE_PROVISIONAL_TV";//SCA_ADD_SOLICITUD_COLABORADOR_PASE_PROVISIONAL
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
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_VEHICULO_DET_TV";
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
                            mensaje = "OK";// result;
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

        public static bool ApruebaSolicitudVehiculo_New(
        string numsolicitud,
        string rucempresa,
        string NumeroFactura,
        decimal MontoFactura,
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
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_VEHICULO_NEW";
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@NUMERO_FACTURA", NumeroFactura);
                            comando.Parameters.AddWithValue("@MONTO_FACTURA", MontoFactura);
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

        public static bool ApruebaSolicitudVehiculo_Transportista(
       string numsolicitud,
       string rucempresa,
       string NumeroFactura,
       decimal MontoFactura,
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
                            comando.CommandText = "dbo.TRANSP_SCA_APROBACION_SOLICITUD_VEHICULO";
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@NUMERO_FACTURA", NumeroFactura);
                            comando.Parameters.AddWithValue("@MONTO_FACTURA", MontoFactura);
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


        public static bool ActualizarSolicitudVehiculo(
        string numsolicitud,
        string IDSOLVEH,
        string IDSOLDOC,
        DateTime? FECHA_DOCUMENTO, 
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
                            comando.CommandText = "dbo.TRANSP_SCA_ACTUALIZAR_SOLICITUD_VEHICULO";
                            comando.Parameters.AddWithValue("@IDSOLVEH", IDSOLVEH);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@IDSOLDOC", IDSOLDOC);
                            comando.Parameters.AddWithValue("@FECHA_DOCUMENTO", FECHA_DOCUMENTO);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
                        
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizarSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
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


        public static bool ActualizarSolicitudColaborador(
         string numsolicitud,
         string IDSOLVEH,
         string IDSOLDOC,
         DateTime? FECHA_DOCUMENTO,
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
                            comando.CommandText = "dbo.TRANSP_SCA_ACTUALIZAR_SOLICITUD_COLABORADOR";
                            comando.Parameters.AddWithValue("@IDSOLVEH", IDSOLVEH);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@IDSOLDOC", IDSOLDOC);
                            comando.Parameters.AddWithValue("@FECHA_DOCUMENTO", FECHA_DOCUMENTO);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);

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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizarSolicitudVehiculo", DateTime.Now.ToShortDateString(), "sistema");
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


        public static bool ActualizarSolicitudVehiculo_Fechas(
        string numsolicitud,
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
                            comando.CommandText = "dbo.TRANSP_SCA_ACTUALIZAR_SOLICITUD_VEHICULO_FECHAS";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);

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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizarSolicitudVehiculo_Fechas", DateTime.Now.ToShortDateString(), "sistema");
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

        public static bool ActualizarSolicitudColaborador_Fechas(
        string numsolicitud,
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
                            comando.CommandText = "dbo.TRANSP_SCA_ACTUALIZAR_SOLICITUD_COLABORADOR_FECHAS";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);

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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizarSolicitudVehiculo_Fechas", DateTime.Now.ToShortDateString(), "sistema");
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

        public static bool RechazaSolicitudVehiculo_Transportista(
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
                            comando.CommandText = "dbo.TRANSP_SCA_RECHAZA_SOLICITUD_VEHICULO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLVEHICULOS", xmlvehiculos);
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

        public static bool ApruebaSolicitudColaborador_New(
        string numsolicitud,
        string rucempresa,
        string NumeroFactura,
        decimal MontoFactura,
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
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_COLABORADOR_NEW";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOSREC", xmldocrechazados);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            comando.Parameters.AddWithValue("@NUMERO_FACTURA", NumeroFactura);
                            comando.Parameters.AddWithValue("@MONTO_FACTURA", MontoFactura);
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


        public static bool ApruebaSolicitudColaborador_Transportista(
     string numsolicitud,
     string rucempresa,
     string NumeroFactura,
     decimal MontoFactura,
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
                            comando.CommandText = "dbo.TRANSP_SCA_APROBACION_SOLICITUD_COLABORADOR";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOSREC", xmldocrechazados);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            comando.Parameters.AddWithValue("@NUMERO_FACTURA", NumeroFactura);
                            comando.Parameters.AddWithValue("@MONTO_FACTURA", MontoFactura);
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

       public static bool ApruebaSolicitudColaboradorPaseProvisional_New(string numsolicitud,string rucempresa,string xmlcolaboradores,string xmldocumentos,string xmlacceso,string usuario,bool banderafac, string fechainicio, string fechacaducidad, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_APROBACION_SOLICITUD_COLABORADOR_PASE_PROVISIONAL_NEW";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmldocumentos);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlcolaboradores);
                            comando.Parameters.AddWithValue("@XMLACCESO", xmlacceso);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            comando.Parameters.AddWithValue("@BANDERAFAC", banderafac);
                            comando.Parameters.AddWithValue("@FECHA_INGRESO", fechainicio);
                            comando.Parameters.AddWithValue("@FECHA_CADUCA", fechacaducidad);
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

        public static bool RechazaSolicitudColaborador_New(
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
                            comando.CommandText = "dbo.TRANSP_SCA_RECHAZA_SOLICITUD_COLABORADOR";
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

        public static bool RechazaSolicitudColaboradorOPC(
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
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_COLABORADOR_OPC";
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

        public static bool GetSolicitudEstadoAnulada(string numsolicitud)
        {
            bool valor = false;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_ValidarStatusSolicitudAnulada";
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

        public static bool AnularSolicitudEmpresa(string encryptnumsolicitud, string numsolicitud, string motivorechazo, string rucempresa, string nombreempresa, string useremail, string xmlempresa, string usuario, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_ANULAR_SOLICITUD_EMPRESA";
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AnularSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AnularSolicitudEmpresa", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static DataTable GetListadoFotosCredencialSubir(long idSolicitud, long idSolCol, string identificacion)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_ListaFotosReconocimientoFacialCredencial";
                coman.Parameters.AddWithValue("@i_idSolicitud", idSolicitud);
                coman.Parameters.AddWithValue("@i_id_solcol", idSolCol);
                coman.Parameters.AddWithValue("@i_identificacion", identificacion);
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

        public static bool AddFotosRegistroFacial(string xmlDocumentos, string usuario, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_ADD_RegistroFacial";
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

        public static DataTable GetRegistroFacialXNumSolicitudCliente(string numsolicitud, string idsolcol)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_ConsultarRegistroFacial";
                coman.Parameters.AddWithValue("@i_idSolicitud", numsolicitud);
                coman.Parameters.AddWithValue("@i_idSolcol", idsolcol);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetRegistroFacialXNumSolicitudCliente", DateTime.Now.ToShortDateString(), "gs");
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

        public static DataTable GetSolicitudColaboradorCliente_New(string numsolicitud, string ruccipas)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "TRANSP_SCA_ConsultaSolicitudColaborador";
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "credenciales", "GetSolicitudColaboradorCliente_New", DateTime.Now.ToShortDateString(), "gs");
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

        public static bool ActualizarEstadoFotosRegistroFacial(long idSolicitud, long idSolCol, int secuencia, string estado, string motivo, string usuario, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_ActualizarEstadoFoto";
                            comando.Parameters.AddWithValue("@i_idSolicitud", idSolicitud);
                            comando.Parameters.AddWithValue("@i_idSolcol", idSolCol);
                            comando.Parameters.AddWithValue("@i_secuencia", secuencia);
                            comando.Parameters.AddWithValue("@i_estado", estado);
                            comando.Parameters.AddWithValue("@i_usuario", usuario);
                            comando.Parameters.AddWithValue("@i_motivo", motivo);
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizarEstadoFotosRegistroFacial", DateTime.Now.ToShortDateString(), "sistema");
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

        public static bool ActualizarEstadoColaborador(long idSolicitud, long idSolCol, string estado, string motivo, string usuario, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_RechazarColaborador";
                            comando.Parameters.AddWithValue("@i_idSolicitud", idSolicitud);
                            comando.Parameters.AddWithValue("@i_idSolcol", idSolCol);
                            comando.Parameters.AddWithValue("@i_estado", estado);
                            comando.Parameters.AddWithValue("@i_usuario", usuario);
                            comando.Parameters.AddWithValue("@i_motivo", motivo);
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizarEstadoFotosRegistroFacial", DateTime.Now.ToShortDateString(), "sistema");
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

        public static bool AddTraceRegistroFacial(long idSolicitud, long idSolCol, int secuencia, string step, string request, string response, bool estado, string usuario, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_ADD_RegistroFacialTrace";
                            comando.Parameters.AddWithValue("@i_IDSOLICITUD", idSolicitud);
                            comando.Parameters.AddWithValue("@i_IDSOLCOL", idSolCol);
                            comando.Parameters.AddWithValue("@i_SECUENCIA", secuencia);
                            comando.Parameters.AddWithValue("@i_STEP", step);
                            comando.Parameters.AddWithValue("@i_REQUEST", request);
                            comando.Parameters.AddWithValue("@i_RESPONSE", response);
                            comando.Parameters.AddWithValue("@i_ESTADO", estado);
                            comando.Parameters.AddWithValue("@i_usuarioCrea", usuario);

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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "AddTraceRegistroFacial", DateTime.Now.ToShortDateString(), "sistema");
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

        public static DataTable GetFotosSolicitudColaboradorOnlyControl(string numsolicitud, string identificacion)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_ConsultaFotosXSolicitudRF";
                coman.Parameters.AddWithValue("@i_idSolicitud", numsolicitud);
                coman.Parameters.AddWithValue("@i_identificacion", identificacion);
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

        public static bool AddPagoConfirmado_New( string numsolicitud, string razsocial, string ruccipas, string emailuser, string usuario, string numdoc, string fecing, string fecsal, string area, string departamento, string turno, string permiso, int idPermiso, out string mensaje)
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
                            comando.Parameters.AddWithValue("@PERMISO", permiso);
                            comando.Parameters.AddWithValue("@ID_PERMISO", idPermiso);
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

        public static bool AddSolicitudColaboradorPaseProvisional_New(string xmlImagenes ,string nombreempresa, string cedulanombrereplegal, string nombrereplegal, string fechainicio, string fechacaducidad, string useremail, string tiposolicitud, string rucempresa, string area, string actividad, string xmlColaboradores, string xmlDocumentos, string usuario, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_PASE_PROVISIONAL_TV_NEW";//SCA_ADD_SOLICITUD_COLABORADOR_PASE_PROVISIONAL
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@CEDULAREPLEGAL", cedulanombrereplegal);
                            comando.Parameters.AddWithValue("@NOMBRESREPLEGAL", nombrereplegal);
                            comando.Parameters.AddWithValue("@FECHAINICIO", fechainicio);
                            comando.Parameters.AddWithValue("@FECHACADUCIDAD", fechacaducidad);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@AREA", area);
                            comando.Parameters.AddWithValue("@ACTIVIDAD", actividad);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlColaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@XMLIMAGENES", xmlImagenes);
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

        public static DataTable GetSolicitudColaboradorPermisoProvisional_New(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_GET_SOLICITUDCOLABORADOR_PROVICIONAL";
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

        public static DataTable GetSolicitudColaboradores(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_GET_SOLICITUDCOLABORADORES";
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


        public static bool ActualizarCodigoNominaRegistroFacial(long idSolicitud, long idSolCol, string nomina_id, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_ActualizarCodigoNominaOC";
                            comando.Parameters.AddWithValue("@i_idSolicitud", idSolicitud);
                            comando.Parameters.AddWithValue("@i_idSolcol", idSolCol);
                            comando.Parameters.AddWithValue("@i_nomina_id", nomina_id);
                            
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
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ActualizarCodigoNominaRegistroFacial", DateTime.Now.ToShortDateString(), "sistema");
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

        public static bool ValidaFacturacion(string numsolicitud)
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
                            comando.CommandText = "dbo.SCA_Validar_factura";
                            comando.Parameters.AddWithValue("@IDSOLICITUD", numsolicitud);
                            conn.Open();
                            var result = comando.ExecuteScalar().ToString();
                            conn.Close();
                            return bool.Parse(result);
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "credenciales", "ValidaFacturacion", DateTime.Now.ToShortDateString(), "sistema");
                        return true;
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
                var t = log_csl.save_log<Exception>(ex, "credenciales", "ValidaFacturacion", DateTime.Now.ToShortDateString(), "sistema");
                return true;
            }
        }

        public static bool AddSolicitudOPC(string nombreempresa, string cedulanombrereplegal, string nombrereplegal, string fechainicio, string fechacaducidad, string useremail, string tiposolicitud, string rucempresa, string area, string actividad, string xmlColaboradores,  string usuario, out string mensaje)
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
                            comando.CommandText = "SCA_ADD_SOLICITUD_COLABORADOR_OPC";
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@CEDULAREPLEGAL", cedulanombrereplegal);
                            comando.Parameters.AddWithValue("@NOMBRESREPLEGAL", nombrereplegal);
                            comando.Parameters.AddWithValue("@FECHAINICIO", fechainicio);
                            comando.Parameters.AddWithValue("@FECHACADUCIDAD", fechacaducidad);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@AREA", area);
                            comando.Parameters.AddWithValue("@ACTIVIDAD", actividad);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlColaboradores);
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

        public static bool AddActivaciondePermisosOPC(string numsolicitud, string razsocial, string ruccipas, string emailuser, string usuario, string numdoc, string fecing, string fecsal, string area, string departamento, string turno, string permiso, int idPermiso, out string mensaje)
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
                            comando.CommandText = "dbo.SCA_ACTIVA_PERMISOS_OPC";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", razsocial);
                            comando.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                            comando.Parameters.AddWithValue("@EMAILUSER", emailuser);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            comando.Parameters.AddWithValue("@NUMERODOCUMENTO", numdoc);
                            comando.Parameters.AddWithValue("@AREA", area);
                            comando.Parameters.AddWithValue("@DPTO", departamento);
                            comando.Parameters.AddWithValue("@TURNO", turno);
                            comando.Parameters.AddWithValue("@PERMISO", permiso);
                            comando.Parameters.AddWithValue("@ID_PERMISO", idPermiso);
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

        public static bool RechazaSolicitudOPC(string numsolicitud,string rucempresa,string nombreempresa,string useremail,string comentario,string usuario,out string mensaje)
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
                            comando.CommandText = "dbo.SCA_RECHAZA_SOLICITUD_OPC";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@COMENTARIO", comentario);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
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

        public static bool RechazarColaboradorEspecifico(
        string numsolicitud,
        string idSolCol,
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
                            comando.CommandText = "dbo.SCA_RECHAZA_COLABORADOR_ESPECIFICO";
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@IDSOLCOL", idSolCol);
                            comando.Parameters.AddWithValue("@COMENTARIO", comentario);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
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
                        //mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        mensaje = ex.Message;
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
    }
    public class ttipocli
    {
        public string id { get; set; }
    }
    public class serviciosCredenciales : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public string nombre { get; set; }
        public string codigoN4 { get; set; }
        public string notasN4 { get; set; }
        public bool? estado { get; set; }
        public string RUCCIPAS { get; set; }
        #endregion

        public serviciosCredenciales()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static serviciosCredenciales GetServicio(long _id)
        {
            OnInit("Portal_Sca");
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectOnly<serviciosCredenciales>(nueva_conexion, 4000, "SCA_ConsultarServicios", parametros);
        }

        public static DataView consultaServicios()
        {
            OnInit("Portal_Sca");
            SqlConnection cn = new SqlConnection(nueva_conexion);

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_ConsultarServicios";
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch
                {
                    throw;
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
            return d.DefaultView;
        }

        public static string GetCodigoNomina(string identificacion)
        {
            string id = string.Empty;
            OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
            id = oc.GetIdAc_NominaPeaton(identificacion);
            return id;
        }

        public static DataSet CrearPermiso(DataSet DsEntrada, out string error, out int registros_actualizados_correcto, out int registros_actualizados_incorrecto)
        {
            DataSet dsSalida;
            string detalle = string.Empty;
            registros_actualizados_correcto = 0;
            registros_actualizados_incorrecto = 0;
            try
            {
                OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
              
                dsSalida = new DataSet();
                dsSalida = oc.AC_R_PERMISO_NEW(DsEntrada, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref detalle);
            }
            catch (Exception ex)
            {
                dsSalida = null;
                error = ex.Message;
            }
            error = detalle;
            return dsSalida;
        }

        public static RespuestaSwNeuro CrearPermiso(string _nomina_id, int _horario, DateTime _f_ingreso, DateTime _f_salida, int _cod_permiso, string _id_permiso, string _areas)
        {
            RespuestaSwNeuro resultado = null;
            try
            {
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add(new DataColumn("ID", typeof(String)));
                dt.Columns.Add(new DataColumn("F_HORARIO", typeof(Int32)));
                dt.Columns.Add(new DataColumn("F_INGRESO", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("F_SALIDA", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("TIPO_CONTROL", typeof(Int32))); //1 LIBRE (NO CADUCA O NO VALIDA FECHA) - 2 CONTROLADO
                dt.Columns.Add(new DataColumn("COD_PERMISO", typeof(Int32))); // NEW - EJ: 65
                dt.Columns.Add(new DataColumn("ID_PERMISO", typeof(String))); // NEW - EJ: FA
                dt.Columns.Add(new DataColumn("AREAS", typeof(String))); // NEW

                dr = dt.NewRow();
                dr["ID"] = _nomina_id;//"999999";
                dr["F_HORARIO"] = _horario;// 7
                dr["F_INGRESO"] = _f_ingreso;// Now.Date
                dr["F_SALIDA"] = _f_salida;// DateAdd("d", 1, Now.Date)
                dr["TIPO_CONTROL"] = 2;
                dr["COD_PERMISO"] = _cod_permiso;// 64
                dr["ID_PERMISO"] = _id_permiso;// "H"
                dr["AREAS"] = _areas;// "916" '"873, 874"
                dt.Rows.Add(dr);

                DataSet DS = new DataSet();
                DS.Tables.Add(dt);

                OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
                int reg1 = 0;
                int reg2 = 0;
                string detalle = string.Empty;
                DataSet dtsalida = new DataSet();
                dtsalida = oc.AC_R_PERMISO_NEW(DS, ref reg1, ref reg2, ref detalle);

                resultado = new RespuestaSwNeuro();
                resultado.codigo = "0";
                if (dtsalida != null)
                {
                    if (dtsalida.Tables.Count > 0)
                    {
                        resultado.codigo = dtsalida.Tables[0].Rows.Count.ToString();
                    }
                }
                resultado.dato = detalle;
                resultado.mensaje = detalle;
            }
            catch (Exception ex)
            {
                resultado = new RespuestaSwNeuro();
                resultado.codigo = "3";
                resultado.dato = "Error al CrearPermiso en OC";
                resultado.mensaje = ex.Message;
            }
            return resultado;
        }

        public static DataSet ConsultaPermiso()
        {
            DataSet dtsalida = new DataSet();
            try
            {
                OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
                //int reg1 = 0;
                //int reg2  = 0;
                string detalle = string.Empty;

                dtsalida = oc.AC_TIPO_PERMISO(ref detalle);
                string filtro = Cls_Conexion.Nueva_Conexion("PERMISOS_OC");
                DataTable dtPermiso = dtsalida.Tables[0].Select(filtro).CopyToDataTable();

                dtsalida.Tables[0].Clear();
                dtsalida.Tables[0].Merge(dtPermiso);
            }
            catch
            {
                dtsalida = null;
            }

            return dtsalida;
        }

        public static RespuestaSwNeuro ActualizaFace(string nomina_id, string img1, string img2, string img3, string imgt1, string imgt2, string imgt3)
        {
            RespuestaSwNeuro _exit = null;
            try
            {
                OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
                OnlyControl.DataTemplates clsinterfaz = new OnlyControl.DataTemplates();

                //byte[] bData;
                //BinaryReader br = new BinaryReader(System.IO.File.OpenRead("D:\BIOMETRIC\ARCHIVO\1.jpg"));
                //bData = br.ReadBytes(br.BaseStream.Length)

                clsinterfaz.Image1 = img1;
                clsinterfaz.Image2 = img2;
                clsinterfaz.Image3 = img3;
                clsinterfaz.Template1 =imgt1;
                clsinterfaz.Template2 =imgt2;
                clsinterfaz.Template3 =imgt3;

                //#########################################################
                //SE ENVIA EL ID DE OC PARA IDENTIFICAR EL REGISTRO
                //#########################################################
                string result = oc.ActualizaFace(nomina_id, clsinterfaz);
                _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(result);

            }
            catch (Exception ex)
            {
                _exit = new RespuestaSwNeuro();
                _exit.codigo = "3";
                _exit.dato = "Error al ActualizaFace en OC";
                _exit.mensaje = ex.Message;
            }
            return _exit;
        }

        public static RespuestaSwNeuro Elimina_Face(string nomina_id)
        {
            RespuestaSwNeuro _exit = null;
            try
            {
                OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
                //#########################################################
                //SE ENVIA EL ID(6 DIGITOS) DE OC PARA IDENTIFICAR EL REGISTRO
                //#########################################################
                string result = oc.EliminaFace(nomina_id);
                _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(result);
            }
            catch (Exception ex)
            {
                _exit = new RespuestaSwNeuro();
                _exit.codigo = "3";
                _exit.dato = "Error al Elimina_Face en OC";
                _exit.mensaje = ex.Message;
            }
            return _exit;
        }

        public static RespuestaSwNeuro ConsultaFace(string nomina_id)
        {
            RespuestaSwNeuro _exit = null;
            try
            {
                OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
                OnlyControl.DataTemplates clsinterfaz = new OnlyControl.DataTemplates();
                string result = oc.ConsultaFace(nomina_id, ref clsinterfaz);
            }
            catch (Exception ex)
            {
                _exit = new RespuestaSwNeuro();
                _exit.codigo = "3";
                _exit.dato = "Error al ConsultaFace en OC";
                _exit.mensaje = ex.Message;
            }
            return _exit;

        }
    }
    public class ServicioSCA : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4_SERVICE";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //inicializa de la instancia de servicio
        private static Servicios InicializaServicio(out string erno)
        {
            var p = new Servicios();
            if (!p.Accesorio.Inicializar(out erno))
            {
                return null;
            }
            return p;
        }

        public static Respuesta.ResultadoOperacion<bool> CargarServicioCredencial(string servicio, string rucEmpresa, string tarifa, string cantidad, string usuario, out string outRequest, out string outResponse)
        {
            
            if (tarifa =="PVE")
            {
                outRequest = "";
                outResponse = "";
                return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "REGISTRO VEHICULAR PARA INGRESO TEMPORAL");
                
            }
            string pv;
            outRequest = "";
            outResponse = "";
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            N4WS_CRE.Entidad.GroovyCodeExtension code = new N4WS_CRE.Entidad.GroovyCodeExtension();
            code.name = "CGSACreateInvoice";
            code.location = "code-extension";
            code.parameters.Add("INVOICETYPE", servicio);
            code.parameters.Add("PAYEE", rucEmpresa);
            code.parameters.Add("TARIFF", tarifa);//"EM.CR.US.EX.BA.RE");
            code.parameters.Add("QTY", cantidad);

            outRequest = code.ToString();

            //Poner el evento
            var n4r = N4WS_CRE.Entidad.Servicios.EjecutarCODEExtensionGenericoN4BillingCredenciales(code, usuario);
            outResponse = n4r.response.ToString();

            if (n4r.response.ToString().Contains("<result>ERROR"))
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "CargarServicioCredencial", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("Error al cargar evento");
            }

            var xds = System.Xml.Linq.XDocument.Parse(n4r.response.ToString());
            var resultado = xds.Root.Descendants("result").FirstOrDefault().Value.ToString();
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, resultado);
        }

        public static Respuesta.ResultadoOperacion<bool> CargarServicioMultiDespacho(string INVOICETYPE, string rucEmpresa, string CODIGO_TARIJA_N4, string cantidad, string usuario, out string outRequest, out string outResponse)
        {

            
            string pv;
            outRequest = "";
            outResponse = "";
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            N4WS_CRE.Entidad.GroovyCodeExtension code = new N4WS_CRE.Entidad.GroovyCodeExtension();
            code.name = "CGSACreateInvoice";
            code.location = "code-extension";
            code.parameters.Add("INVOICETYPE", INVOICETYPE);
            code.parameters.Add("PAYEE", rucEmpresa);
            code.parameters.Add("TARIFF", CODIGO_TARIJA_N4);//"EM.CR.US.EX.BA.RE");
            code.parameters.Add("QTY", cantidad);

            outRequest = code.ToString();

            //Poner el evento
            var n4r = N4WS_CRE.Entidad.Servicios.EjecutarCODEExtensionGenericoN4BillingCredenciales(code, usuario);
            outResponse = n4r.response.ToString();

            if (n4r.response.ToString().Contains("<result>ERROR"))
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "CargarServicioMultiDespacho", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("Error al cargar evento");
            }

            var xds = System.Xml.Linq.XDocument.Parse(n4r.response.ToString());
            var resultado = xds.Root.Descendants("result").FirstOrDefault().Value.ToString();
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, resultado);
        }

        public static Respuesta.ResultadoOperacion<bool> CargarServicioMultiDespachoTransporte(string INVOICETYPE, string rucEmpresa, string CODIGO_TARIJA_1, string CODIGO_TARIJA_2, string cargas, string usuario, out string outRequest, out string outResponse)
        {


            string pv;
            outRequest = "";
            outResponse = "";
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            N4WS_CRE.Entidad.GroovyCodeExtension code = new N4WS_CRE.Entidad.GroovyCodeExtension();
            code.name = "CGSACreateInvoiceMultiple";
            code.location = "code-extension";
            code.parameters.Add("INVOICETYPE", INVOICETYPE);
            code.parameters.Add("PAYEE", rucEmpresa);
            code.parameters.Add("NOTES", cargas);
            code.parameters.Add("T.TARIFA1", CODIGO_TARIJA_1);
            code.parameters.Add("T.TARIFA2", CODIGO_TARIJA_2);

            outRequest = code.ToString();

            //Poner el evento
            var n4r = N4WS_CRE.Entidad.Servicios.EjecutarCODEExtensionGenericoN4BillingCredenciales(code, usuario);
            outResponse = (n4r.response == null ? "" :  n4r.response.ToString());

            if (n4r.response.ToString().Contains("<result>ERROR"))
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "CargarServicioMultiDespachoTransporte", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("Error al cargar evento");
            }

            var xds = System.Xml.Linq.XDocument.Parse(n4r.response.ToString());
            var resultado = xds.Root.Descendants("result").FirstOrDefault().Value.ToString();
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, resultado);
        }

        public static Respuesta.ResultadoOperacion<bool> CargarServicioBTS(string INVOICETYPE, string rucEmpresa, string NOTAS, List<Tuple<string, string>> LISTA, string usuario, out string outRequest, out string outResponse)
        {


            string pv;
            outRequest = "";
            outResponse = "";
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            //rucEmpresa = "0991370226001";

            N4WS_CRE.Entidad.GroovyCodeExtension code = new N4WS_CRE.Entidad.GroovyCodeExtension();
            code.name = "CGSACreateInvoiceMultipleItem";
            code.location = "code-extension";
            code.parameters.Add("INVOICETYPE", INVOICETYPE);
            code.parameters.Add("PAYEE", rucEmpresa);
            code.parameters.Add("FINALIZE", "0");
            code.parameters.Add("NOTES", NOTAS);

            int fila = 1;
            foreach (var item in LISTA)
            {
                code.parameters.Add(string.Format("{0}_{1}",item.Item1,fila), item.Item2);
                fila++;
            }


            outRequest = code.ToString();

            //Poner el evento
            var n4r = N4WS_CRE.Entidad.Servicios.EjecutarCODEExtensionGenericoN4BillingCredenciales(code, usuario);
            outResponse = (n4r.response == null ? "" : n4r.response.ToString());

            if (n4r.response.ToString().Contains("<result>ERROR"))
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "CGSACreateInvoiceMultipleItem", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("Error al cargar evento");
            }

            var xds = System.Xml.Linq.XDocument.Parse(n4r.response.ToString());
            var resultado = xds.Root.Descendants("result").FirstOrDefault().Value.ToString();
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, resultado);
        }

        public static Respuesta.ResultadoOperacion<bool> CrearBL_BTS(string NBR, string LINE, string VISIT, string CATEGORY, 
            string SHIPPER, string CONSIGNEE, string CARGOFIRST, string CARGOSECOND, string OPERATION, 
            string WEIGHT, string VOLUME, string CANWEIGHT, string POL, string NOTES, string ITEMNBR,
            string COMMODITY, string PRODUCT, string PACKAGING, string TOTALWEIGHT, string QTY,
            string MARKS, string POSITION,
            string usuario, out string outRequest, out string outResponse)
        {


            string pv;
            outRequest = "";
            outResponse = "";
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

   
            N4WS_CRE.Entidad.GroovyCodeExtension code = new N4WS_CRE.Entidad.GroovyCodeExtension();
            code.name = "CGSABillOfLading";
            code.location = "code-extension";
            code.parameters.Add("NBR", NBR);
            code.parameters.Add("LINE", LINE);
            code.parameters.Add("VISIT", VISIT);
            code.parameters.Add("SHIPPER", SHIPPER);
            code.parameters.Add("CONSIGNEE", CONSIGNEE);
            code.parameters.Add("CARGOFIRST", CARGOFIRST);
            code.parameters.Add("CARGOSECOND", CARGOSECOND);
            code.parameters.Add("OPERATION", OPERATION);
            code.parameters.Add("WEIGHT", WEIGHT);
            code.parameters.Add("VOLUME", VOLUME);
            code.parameters.Add("CANWEIGHT", CANWEIGHT);
            code.parameters.Add("POL", POL);
            code.parameters.Add("NOTES", NOTES);
            code.parameters.Add("ITEMNBR", ITEMNBR);
            code.parameters.Add("COMMODITY", COMMODITY);
            code.parameters.Add("PRODUCT", PRODUCT);
            code.parameters.Add("PACKAGING", PACKAGING);
            code.parameters.Add("TOTALWEIGHT", TOTALWEIGHT);
            code.parameters.Add("QTY", QTY);
            code.parameters.Add("MARKS", MARKS);
            code.parameters.Add("POSITION", POSITION);
            code.parameters.Add("USER", CATEGORY);

            outRequest = code.ToString();

            //Poner el evento
            var n4r = N4WS_CRE.Entidad.Servicios.EjecutarCODEExtensionGenericoN4BillingCredenciales(code, usuario);
            outResponse = (n4r.response == null ? "" : n4r.response.ToString());

            if (n4r.response.ToString().Contains("<result>ERROR"))
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "CGSABillOfLading", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("Error al cargar evento");
            }

            var xds = System.Xml.Linq.XDocument.Parse(n4r.response.ToString());
            var resultado = xds.Root.Descendants("result").FirstOrDefault().Value.ToString();
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, resultado);
        }



    }



    public class TarifarioN4 : Cls_Bil_Base
    {
        #region "Propiedades"
        public int? ID { get; set; }
        public int IDTIPSOL { get; set; }
        public string CODIGO { get; set; }
        public string TIPO { get; set; }
        public int TIPOEMPRESA { get; set; }
        public string TIPOEMPRESADESC { get; set; }
        public string TARIFA_N4 { get; set; }
        public bool? ESTADO { get; set; }
        #endregion

        public TarifarioN4()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static TarifarioN4 GetTarifa(int _tipoSolicitud, int _tipoEmpresa, long _numSolicitud, string _norma, string _subtipo)
        {
            OnInit("Portal_Sca");
            parametros.Clear();
            parametros.Add("i_tipoSolicitud", _tipoSolicitud);
            parametros.Add("i_tipoEmpresa", _tipoEmpresa);
            parametros.Add("i_numSolicitud", _numSolicitud);
            parametros.Add("i_norma", _norma);
            parametros.Add("i_subtipo", _subtipo);
            return sql_puntero.ExecuteSelectOnly<TarifarioN4>(nueva_conexion, 4000, "SCA_ConsultarTarifario", parametros);
        }

        public static DataView consultaTarifarioTipo(string v_tipo)
        {
            OnInit("Portal_Sca");
            SqlConnection cn = new SqlConnection(nueva_conexion);

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.Parameters.AddWithValue("@i_tipo", v_tipo);
                coman.CommandText = "SCA_CONSULTAR_TARIFARIO_TIPO";
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    throw;
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
            return d.DefaultView;
        }
    }

    public class TraceSet : Cls_Bil_Base
    {
        #region "Propiedades"
        public long ID { get; set; }
        public long IDSOLICITUD { get; set; }
        public string RUCEMPRESA { get; set; }
        public string NORMA { get; set; }
        public string SUBTIPO { get; set; }
        public int IDTARIFA { get; set; }
        public string TARIFAN4 { get; set; }
        public string REQUESTXML { get; set; }
        public string RESPONSEXML { get; set; }
        public bool? ESTADO { get; set; }
        public string USUARIO { get; set; }
        public DateTime FECHACREACION { get; set; }
        #endregion

        public TraceSet()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static Int64? SaveTrace(long _numSolicitud, string _rucEmpresa, string _norma, string _subtipo, int _idtarifa, string _tarifaN4, string _xmlRequest, string _xmlResponse, bool _estado, string _user, out string OnError)
        {
            OnInit("Portal_Sca");
            parametros.Clear();
            parametros.Add("i_IDSOLICITUD", _numSolicitud);
            parametros.Add("i_RUCEMPRESA", _rucEmpresa);
            parametros.Add("i_NORMA", _norma);
            parametros.Add("i_SUBTIPO", _subtipo);
            parametros.Add("i_IDTARIFA", _idtarifa);
            parametros.Add("i_TARIFAN4", _tarifaN4);
            parametros.Add("i_REQUESTXML", _xmlRequest);
            parametros.Add("i_RESPONSEXML", _xmlResponse);
            parametros.Add("i_ESTADO", _estado);
            parametros.Add("i_USUARIO", _user);
            // return sql_puntero.ExecuteInsertUpdateDelete<TraceSet>(nueva_conexion, 4000, "SCA_ADD_TRACE", parametros);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "SCA_ADD_TRACE", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }
    public class RespuestaSwNeuro
    {
        public string mensaje { get; set; }
        public string codigo { get; set; }
        public string dato { get; set; }
    }
}