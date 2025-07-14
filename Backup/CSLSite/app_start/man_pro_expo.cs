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
    public class man_pro_expo
    {
        private static SqlConnection conexionvalidar()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["validar"].ConnectionString);
        }

        private static SqlConnection conexionmidle()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }

        public static DataTable GetCatalagoLineaAsumeHorasReefer()
        {
            var d = new DataTable();
            using (var c = conexionvalidar())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTOS_HORAS_REEFER";
                coman.Parameters.AddWithValue("@TIPO", 0);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "man_pro_expo", "GetCatalagoLineaAsumeHorasReefer", "SP_PROCESO_MANTENIMIENTOS_HORAS_REEFER", "@TIPO=0");
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

        public static DataTable getPoblarCredenciales(out string mensaje)
        {
            DataTable d = new DataTable();
            mensaje = string.Empty;
            using (var c = conexionmidle())
            {
                var t = c.CreateCommand();
                t.CommandType = CommandType.StoredProcedure;
                t.CommandText = "SP_PROCESO_PAGO_EN_LINEA";
                t.CommandType = System.Data.CommandType.StoredProcedure;
                t.Parameters.AddWithValue("@TIPO", 8);
                try
                {
                    c.Open();
                    d.Load(t.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    var error = log_csl.save_log<Exception>(ex, "Ws_Sap_EstadoDeCuenta.app_start", "PoblarCredenciales", DateTime.Now.ToShortDateString(), "Ws_Sap_EstadoDeCuenta");
                    mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", error);
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

        public static DataTable GetCatalagoReferenciasPagoTerceros()
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTO_PAGO_TERCEROS";
                coman.Parameters.AddWithValue("@TIPO", 0);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "man_pro_expo", "GetCatalagoLineaAsumeHorasReefer", "SP_PROCESO_MANTENIMIENTOS_HORAS_REEFER", "@TIPO=0");
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

        public static DataTable GetCatalagoClientesPagoTerceros()
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTO_PAGO_TERCEROS";
                coman.Parameters.AddWithValue("@TIPO", 1);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "man_pro_expo", "GetCatalagoLineaAsumeHorasReefer", "SP_PROCESO_MANTENIMIENTOS_HORAS_REEFER", "@TIPO=0");
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

        public static bool AddManHorasReefer(
        string linea,
        string horas,
        string fecha_vigencia,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexionvalidar())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_PROCESO_MANTENIMIENTOS_HORAS_REEFER";
                            comando.Parameters.AddWithValue("@TIPO", 1);
                            comando.Parameters.AddWithValue("@LINEA", linea);
                            comando.Parameters.AddWithValue("@HORAS", horas);
                            comando.Parameters.AddWithValue("@FECHA_VIGENCIA", fecha_vigencia);
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
                        if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        {
                            mensaje = ex.Message.ToString();
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (SqlError e in ex.Errors)
                            {
                                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                            }
                            var t = log_csl.save_log<Exception>(ex, "credenciales", "AddManHorasReefer", DateTime.Now.ToShortDateString(), "sistema");
                            mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        }
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
                var t = log_csl.save_log<Exception>(ex, "credenciales", "AddManHorasReefer", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
    }
}