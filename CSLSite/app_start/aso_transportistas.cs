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
    public class aso_transportistas
    {
        private SqlConnection conn;
        private SqlTransaction sqlTransaction;
        private SqlDataReader dr;
        private SqlCommand cmd;
        private static OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private static SqlConnection conexionN4Middleware()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
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
        public static DataTable GetSolicitudes(bool todos, string numsolicitud, string ruccipas, string tipoestado, string fecdesde, string fechasta)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_ESTADO_SOLICITUD_ASO_TRANS";
                //coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                if (!string.IsNullOrEmpty(ruccipas))
                {
                    coman.Parameters.AddWithValue("@RUCCIPAS", ruccipas);
                }
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
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportistas", "GetSolicitudes", numsolicitud, "SCA_CONSULTA_ESTADO_SOLICITUD_ASO_TRANS");
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
        public static DataTable GetCiaTransAso(string ruc_razsocial)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CONSULTA_CIA_TRANS_ASO";
                coman.Parameters.AddWithValue("@RUC_RAZSOCIAL", ruc_razsocial);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportistas", "GetCiaTransAso", DateTime.Now.ToString(), "GetCiaTransAso");
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
        public static DataTable GetValidaHuella(string ruc, string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 15);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "asociacion_transporte", "GetValidaHuella", DateTime.Now.ToShortDateString(), "asociacion_transporte");
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
        public static DataTable GetValidaFoto(string ruc, string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 14);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "asociacion_transporte", "GetValidaFoto", DateTime.Now.ToShortDateString(), "asociacion_transporte");
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
        public static DataTable GetValidaCodigoUsuario(string ruc, string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 16);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetValidaCodigoUsuario", DateTime.Now.ToShortDateString(), "asociacion_transporte");
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
        public static DataTable GetConsultaTurno()
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 17);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetConsultaTurno", DateTime.Now.ToShortDateString(), "gs");
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
                coman.Parameters.AddWithValue("@TIPO", 70);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetConsultaArea", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetDiasPermisoAccesoTemporal()
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                coman.Parameters.AddWithValue("@TIPO", 0);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetDiasPermisoAccesoTemporal", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetCaducaDiasPermisoAccesoTemporal(string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 18);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetCaducaDiasPermisoAccesoTemporal", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetValEstadoSolicitud(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                coman.Parameters.AddWithValue("@TIPO", 1);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetValEstadoSolicitud", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetBannedChofer(string chofer)
        {
            var d = new DataTable();
            using (var c = conexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "FNA_FUN_CHOFER_BANNED";
                coman.Parameters.AddWithValue("@g_chofer", chofer);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetValEstadoSolicitud", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetAsoTrans(string numsolicitud)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                coman.Parameters.AddWithValue("@TIPO", 4);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetAsoTrans", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetValHuellaFoto(string numsolicitud, string cedula)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                coman.Parameters.AddWithValue("@TIPO", 6);
                coman.Parameters.AddWithValue("@CEDULA", cedula);
                coman.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetValHuellaFoto", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetValConductorXEmpresa(string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 21);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetValConductorXEmpresa", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetEmpresaNomAC(string ruc)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 19);
                coman.Parameters.AddWithValue("@RUC", ruc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetEmpresaNomAC", DateTime.Now.ToShortDateString(), "gs");
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
        public static DataTable GetFechasPermisoDeAcceso(string ruc, string cedula)
        {
            var d = new DataTable();
            using (var c = conexionMDA())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SCA_CREDENCIALES";
                coman.Parameters.AddWithValue("@TIPO", 20);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@CEDULACON", cedula);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetFechasPermisoDeAcceso", DateTime.Now.ToShortDateString(), "gs");
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
        public static bool AddEmailBloqueaPermiso(
       string cedula,
       string numsolicitud,
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
                            comando.CommandText = "dbo.SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                            comando.Parameters.AddWithValue("@TIPO", 8);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@CEDULA", cedula);
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
                        var t = log_csl.save_log<Exception>(ex, "aso_transportistas", "AddSolicitudColaboradorFotoHuella", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddEmailBloqueaPermiso", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudColaboradorFotoHuella(
        string cedula,
        string numsolicitud,
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
                            comando.CommandText = "dbo.SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                            comando.Parameters.AddWithValue("@TIPO", 5);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@CEDULA", cedula);
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
                        var t = log_csl.save_log<Exception>(ex, "aso_transportistas", "AddSolicitudColaboradorFotoHuella", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddSolicitudColaboradorFotoHuella", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);                
                return false;
            }
        }
        public static bool AddBloqueaSolicitudColaboradorPermiso(
        string cedula,
        string numsolicitud,
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
                            comando.CommandText = "dbo.SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                            comando.Parameters.AddWithValue("@TIPO", 7);
                            comando.Parameters.AddWithValue("@NUMSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@CEDULA", cedula);
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
                        var t = log_csl.save_log<Exception>(ex, "aso_transportistas", "AddSolicitudColaboradorFotoHuella", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddSolicitudColaboradorFotoHuella", DateTime.Now.ToShortDateString(), "sistema");
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
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_CAB_ASO_TRANS";
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
                        var t = log_csl.save_log<Exception>(ex, "aso_transportistas", "AddSolicitudColaboradorCab", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddSolicitudColaboradorCab", DateTime.Now.ToShortDateString(), "sistema");
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
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_DET_ASO_TRANS";
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
                        var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddSolicitudColaboradorDet", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddSolicitudColaboradorDet", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool AddSolicitudColaboradorDetTAG(
       string nombreempresa,
       Int32 numsolicitud,
       string useremail,
       string tiposolicitud,
       string rucempresa,
       string xmlColaboradores,
       string xmlDocumentos,
       string usuario,
       bool enviaemail,
       string tag,
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
                            comando.CommandText = "dbo.SCA_ADD_SOLICITUD_COLABORADOR_DET_ASO_TRANS_TAG";
                            comando.Parameters.AddWithValue("@NEWIDSOLICITUD", numsolicitud);
                            comando.Parameters.AddWithValue("@RAZONSOCIAL", nombreempresa);
                            comando.Parameters.AddWithValue("@EMAILUSER", useremail);
                            comando.Parameters.AddWithValue("@TIPOSOLICITUD", tiposolicitud);
                            comando.Parameters.AddWithValue("@RUCCIPAS", rucempresa);
                            comando.Parameters.AddWithValue("@XMLCOLABORADORES", xmlColaboradores);
                            comando.Parameters.AddWithValue("@XMLDOCUMENTOS", xmlDocumentos);
                            comando.Parameters.AddWithValue("@USUARIOING", usuario);
                            comando.Parameters.AddWithValue("@ENVIAEMAIL", enviaemail);
                            comando.Parameters.AddWithValue("@TAG", tag);
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
                        var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddSolicitudColaboradorDet", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "aso_transportista", "AddSolicitudColaboradorDet", DateTime.Now.ToShortDateString(), "sistema");
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
                            comando.CommandText = "dbo.SCA_PAGO_CONFIRMADO_ASO_TRANS";
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
                        var t = log_csl.save_log<Exception>(ex, "aso_transportista", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "aso_transportista", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static HashSet<Tuple<string, string>> getTipoEmpresa(string rucempresa)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoEmpresa(rucempresa))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            //xlista.Add(Tuple.Create("0", "* Elija el tipo de empresa. *"));
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
                coman.CommandText = "SCA_CONSULTA_TIPO_EMPRESA_CIA_TRANS";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "aso_transportista", "ReturnTipoSolicitud", "null", "N4");
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
        public static HashSet<Tuple<string, string>> getTipoSolicitud(int tiposolicitud)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in ReturnTipoSolicitud(tiposolicitud))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            //xlista.Add(Tuple.Create("0", "* Elija el tipo de credencial. *"));
            return xlista;
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
                coman.CommandText = "SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "aso_transportista", "ReturnTipoSolicitud", "null", "N4");
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
        public static string GetInfoTipoCredencial()
        {
            string valor = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTOS_ASO_TRANS";
                coman.Parameters.AddWithValue("@TIPO", 3);
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
                    csl_log.log_csl.save_log<SqlException>(ex, "aso_transportista", "GetDateServer", DateTime.Now.ToShortDateString(), "gs");
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
    }
}