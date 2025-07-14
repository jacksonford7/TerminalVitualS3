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


namespace CSLSite
{
    public class asignacionDae
    {
        private static SqlConnection ConexionportalMaster()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalMaster"].ConnectionString);
        }
        private static SqlConnection ConexionEcuapass()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"].ConnectionString);
        }
        private static SqlConnection ConexionN5()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString);
        }
        //nuevo validacionde la placa
        public static bool IsTruckTag(string placa)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [N5].[dbo].[fx_is_tag_truck](@placa)";
                comando.Parameters.AddWithValue("@placa", placa);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return bool.Parse(sale.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "IsTruckTag", placa, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static DataTable GetRucXBkg(string Bkg)
        {
            var d = new DataTable();
            using (var c = ConexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "sp_populate_booking_exportador";
                coman.Parameters.AddWithValue("@booking", Bkg);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "asignacionDae", "GetRucXBkg", "sp_populate_booking_exportador", DateTime.Now.ToShortDateString());
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

        public static DataTable GetCntrXBkg(string Bkg)
        {
            var d = new DataTable();
            using (var c = ConexionEcuapass())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_ASIGNACION_DAE_CNTR_X_BKG";
                coman.Parameters.AddWithValue("@BOOKING", Bkg);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "asignacionDae", "GetCntrXBkg", "SP_ASIGNACION_DAE_CNTR_X_BKG", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValDAE(string dae, string cntr, string bkg, string ruc, string usuario)
        {
            var d = new DataTable();
            using (var c = ConexionEcuapass())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_VALIDA_DAE_IIE";
                coman.Parameters.AddWithValue("@DAE", dae);
                coman.Parameters.AddWithValue("@CONTENEDOR", cntr);
                coman.Parameters.AddWithValue("@BKG", bkg);
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@USUARIO_S3", usuario);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "asignacionDae", "getValDAE", "SP_VALIDA_DAE_IIE", DateTime.Now.ToShortDateString());
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

        public static DataTable getEmail(string ruc, string usuario)
        {
            var d = new DataTable();
            using (var c = ConexionportalMaster())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_EMAIL_MONITOR_IIE";
                coman.Parameters.AddWithValue("@RUC", ruc);
                coman.Parameters.AddWithValue("@USUARIO", usuario);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "asignacionDae", "getEmail", "SP_GET_EMAIL_MONITOR_IIE", DateTime.Now.ToShortDateString());
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
            return d;
        }

        public static bool RegistraAsignacionDAE(string xmldae, string ruc, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionEcuapass())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_INSERT_ECU_DAE_ASIGNACION";
                            comando.Parameters.AddWithValue("@XMLDAE", xmldae);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
                            comando.Parameters.AddWithValue("@RUC", ruc);
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
                            var t = log_csl.save_log<Exception>(ex, "asignacionDae", "RegistraAsignacionDAE", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "asignacionDae", "RegistraAsignacionDAE", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }


        public static bool RegistraAsignacionDAE_New(string xmldae, string ruc, string usuario, string email, out string mensaje)
        {
            try
            {
                using (var conn = ConexionEcuapass())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_INSERT_ECU_DAE_ASIGNACION";
                            comando.Parameters.AddWithValue("@XMLDAE", xmldae);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
                            comando.Parameters.AddWithValue("@RUC", ruc);
                            comando.Parameters.AddWithValue("@EMAIL", email);
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
                            var t = log_csl.save_log<Exception>(ex, "asignacionDae", "RegistraAsignacionDAE", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "asignacionDae", "RegistraAsignacionDAE", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

    }


}