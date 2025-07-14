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
using System.Configuration;


namespace CSLSite
{
    public class pasePuerta
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
        private static SqlConnection ConexionN4Middleware()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }
        public static DataTable GetInfoPPWebBRBK(string factura, string carga)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_BBK";
                coman.Parameters.AddWithValue("@TIPO", 6);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                coman.Parameters.AddWithValue("@CARGA", carga);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pase_web", "GetInfoPPWebCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetTurnosInfoReeferContenedor(int ID_PPWEB, string XML_TURNOS, string FECHA_ACTUAL_SALIDA)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_TURNO_CONTENDOR_RF";
                coman.Parameters.AddWithValue("@ID_PPWEB", ID_PPWEB);
                coman.Parameters.AddWithValue("@XML_TURNOS", XML_TURNOS);
                var hrf = Convert.ToDateTime(FECHA_ACTUAL_SALIDA).ToString("yyyy-MM-dd HH:mm");
                coman.Parameters.AddWithValue("@FECHA_ACTUAL_SALIDA", hrf);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetTurnosInfoReefer", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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
        /*
        public Boolean CONSULTA_ESTADO_PASE_SIN_TURNO(String MRN, String MSN, String HSN)
        {
            SqlTransaction sqlTransaction = new SqlTransaction();
            try
            {
                Boolean Estado = false;
                SqlDataReader dr = new SqlDataReader();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.CFS_PROCESO_DESPACHO_CARGA";

                cmd.Parameters.Add("@TIPO", SqlDbType.Char).Value = "10";
                cmd.Parameters.Add("@MRN", SqlDbType.VarChar).Value = MRN;
                cmd.Parameters.Add("@MSN", SqlDbType.VarChar).Value = MSN;
                cmd.Parameters.Add("@HSN", SqlDbType.VarChar).Value = HSN;

                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();

                if (!dr.HasRows)
                {
                    Estado = false;
                }
                else
                {
                    while (dr.Read())
                    {
                        Estado = dr.GetBoolean(0);
                    }
                }
                conn.Close();

                return Estado;
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw;
            }
        }
        */

        public static DataTable GetInfoPasePuertaBRBK(string mrn, string msn, string hsn, string factura)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_BBK";
                coman.Parameters.AddWithValue("@TIPO", 2);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoPasePuertaCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoPasePuertaBRBKSinRIDT(string mrn, string msn, string hsn)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_BBK";
                coman.Parameters.AddWithValue("@TIPO", 15);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoPasePuertaCFSSinRIDT", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValClienteBBK(string factura)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_BBK";
                coman.Parameters.AddWithValue("@TIPO", 16);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValClienteCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static bool InsertaReservaDeTurno(String xml, String xml2, string fecha, Int64 pase, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.BBK_PROCESO_DESPACHO_CARGA";
                            comando.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "4";
                            comando.Parameters.Add("@XML_PP", SqlDbType.Xml).Value = xml;
                            comando.Parameters.Add("@XML_SI", SqlDbType.Xml).Value = xml2;
                            //comando.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha;
                            comando.Parameters.Add("@USUARIOING", SqlDbType.VarChar).Value = usuario;
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        /*
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        */
                        var t = log_csl.save_log<Exception>(ex, "pasePuerta", "InsertaReservaDeTurno", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "pasePuerta", "ActualizaDatosPaseWeb", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool InsertaReservaDeTurnoActualizacion(String xml, String xml2, string fecha, Int64 pase, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.BBK_PROCESO_DESPACHO_CARGA";
                            comando.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "2";
                            comando.Parameters.Add("@XML_PP", SqlDbType.Xml).Value = xml;
                            comando.Parameters.Add("@XML_SI", SqlDbType.Xml).Value = xml2;
                            //comando.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha;
                            comando.Parameters.Add("@USUARIOING", SqlDbType.VarChar).Value = usuario;
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        /*
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        */
                        var t = log_csl.save_log<Exception>(ex, "pasePuerta", "InsertaReservaDeTurnoActualizacion", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "pasePuerta", "ActualizaDatosPaseWeb", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static DataTable GetValFechaDomingosCFS(string fecha_s)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 5);
                coman.Parameters.AddWithValue("@FECHA_S", fecha_s);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValFechaDomingosCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValFechaSalidaBRBK(string fechasalida, string factura, string mrn, string msn, string hsn)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_BBK";
                coman.Parameters.AddWithValue("@TIPO", 8);
                coman.Parameters.AddWithValue("@FEC_SAL_INPUT", Convert.ToDateTime(fechasalida).ToString("yyyy-MM-dd"));
                coman.Parameters.AddWithValue("@FACTURADO", factura);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pase_web", "GetValFechaSalida", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoFacturasClientePPWebBRBKXCarga(string Ruc, string mrn, string msn, string hsn)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_BBK";
                coman.Parameters.AddWithValue("@TIPO", 4);
                coman.Parameters.AddWithValue("@FACTURADO", Ruc.Trim());
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pase_web", "GetInfoFacturasClientePPWebCFSXCarga", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetBodegaCargaBRBK(string consecutivo)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "BBK_CONSULTA_BODEGA_X_CONSECUTIVO";
                coman.Parameters.AddWithValue("@CONSECUTIVO", consecutivo);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetBodegaCargaBRBK", "BBK_CONSULTA_BODEGA_X_CONSECUTIVO", DateTime.Now.ToShortDateString());
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

        public static DataTable GetTurnosDisponiblesBRBK(string fecha, string bodega)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "BBK_CONSULTA_DISPONIBILIDAD_TURNOS";
                coman.Parameters.AddWithValue("@FECHA", fecha);
                coman.Parameters.AddWithValue("@BODEGA", bodega);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetTurnosDisponiblesBRBK", "BBK_CONSULTA_DISPONIBILIDAD_TURNOS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetVehiculoSubSec(string subitem)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 17);
                coman.Parameters.AddWithValue("@CODIGO_TARJA_SUBITEM", subitem);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetVehiculoSubSec", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValClienteCFS(string factura)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 16);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValClienteCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoPasePuertaCFSSinRIDT(string mrn, string msn, string hsn)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 15);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoPasePuertaCFSSinRIDT", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoCerrojoCFS(string mrn, string msn, string hsn)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "CFS_CONSULTA_CERROJO";
                coman.Parameters.AddWithValue("@Pv_mrn", mrn);
                coman.Parameters.AddWithValue("@Pv_msn", msn);
                coman.Parameters.AddWithValue("@Pv_hsn", hsn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoCerrojo", "CFS_CONSULTA_CERROJO", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValFechaSalida(string fechasalida, string factura, string mrn, string msn, string hsn)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 8);
                coman.Parameters.AddWithValue("@FEC_SAL_INPUT", Convert.ToDateTime(fechasalida).ToString("yyyy-MM-dd"));
                coman.Parameters.AddWithValue("@FACTURADO", factura);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValFechaSalida", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValFechaSalidaCntr(string fechasalida, string factura, string mrn, string msn, string hsn, string contenedor)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 11);
                coman.Parameters.AddWithValue("@FEC_SAL_INPUT", Convert.ToDateTime(fechasalida).ToString("yyyy-MM-dd"));
                coman.Parameters.AddWithValue("@FACTURADO", factura);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@CONTENEDOR", contenedor);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValFechaSalidaCntr", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValFechaSalidaCFS(string fechasalida, string factura, string mrn, string msn, string hsn, string contenedor)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 14);
                coman.Parameters.AddWithValue("@FEC_SAL_INPUT", Convert.ToDateTime(fechasalida).ToString("yyyy-MM-dd"));
                coman.Parameters.AddWithValue("@FACTURADO", factura);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@CONTENEDOR", contenedor);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValFechaSalidaCntr", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoPPWebCFS(string factura, string carga)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 6);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                coman.Parameters.AddWithValue("@CARGA", carga);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pase_web", "GetInfoPPWebCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        /*22-11-2019*/
        public static DataTable GetInfoPasePuertaCFS_2019(string mrn, string msn, string hsn, string factura, string agente)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 999);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                coman.Parameters.AddWithValue("@AGENTE", agente);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoPasePuertaCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoPasePuertaCFS(string mrn, string msn, string hsn, string factura)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 2);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoPasePuertaCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoPasePuertaCFSS3(string mrn, string msn, string hsn, string factura)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 22);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoPasePuertaCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoPasePuertaCFSGPase(string mrn, string msn, string hsn, string factura)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 22);
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoPasePuertaCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static Boolean CONSULTA_ESTADO_PASE_SIN_TURNO(String MRN, String MSN, String HSN)
        {
            
            //SqlConnection conn;
            //SqlDataReader dr;
            //SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                Boolean Estado = false;

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.CFS_PROCESO_DESPACHO_CARGA";

                cmd.Parameters.Add("@TIPO", SqlDbType.Char).Value = "10";
                cmd.Parameters.Add("@MRN", SqlDbType.VarChar).Value = MRN;
                cmd.Parameters.Add("@MSN", SqlDbType.VarChar).Value = MSN;
                cmd.Parameters.Add("@HSN", SqlDbType.VarChar).Value = HSN;

                SqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();

                if (!dr.HasRows)
                {
                    Estado = false;
                }
                else
                {
                    while (dr.Read())
                    {
                        Estado = dr.GetBoolean(0);
                    }
                }
                conn.Close();

                return Estado;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        
        public static DataView CONSULTA_HORARIOS_DISPONIBLES(string fecha, string xml)
        {
            SqlConnection conn;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
            conn.Open();
            DataTable dt = new DataTable();
            ds = new DataSet();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.CFS_CONSULTA_HORARIOS_DISPONIBLES";
            cmd.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha;
            //cmd.Parameters.Add("@BODEGA", SqlDbType.NVarChar).Value = bodega;
            cmd.Parameters.Add("@LISTA_SUBITEMS", SqlDbType.Xml).Value = xml;
            dr = cmd.ExecuteReader();
            /*if (!dr.HasRows)
            {
            }
            else
            {*/
            dr.Close();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            /*}*/
            cmd.Parameters.Clear();
            conn.Close();
            conn.Dispose();
            return ds.Tables[0].DefaultView;
        }

        public static void REGISTRA_HORARIOS_PASEPUERTA(String xml, String xml2, string fecha, Int64 pase, string usuario)
        {
            string mensaje = "";
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.CFS_PROCESO_DESPACHO_CARGA";

                            comando.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "4";
                            comando.Parameters.Add("@XML_PP", SqlDbType.Xml).Value = xml;
                            comando.Parameters.Add("@XML_SI", SqlDbType.Xml).Value = xml2;
                            comando.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha;
                            //cmd.Parameters.Add("@PASE", SqlDbType.BigInt).Value = pase;
                            comando.Parameters.Add("@USUARIOING", SqlDbType.VarChar).Value = usuario;
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        //if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        //{
                        //    mensaje = ex.Message.ToString();
                        //}
                        //else
                        //{
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                        mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                        //}
                        throw;
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
                var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                throw;
            }
        }


        public static bool AcualizaPaseWebCFS(string xmlppweb, string xmlpp, string xmlppwebdet, string factura, string agente, string facturado, string role, string importador, string nave, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_ACTUALIZA_PASE_WEB_CFS";
                            comando.Parameters.AddWithValue("@XML_PPWEB", xmlppweb);
                            comando.Parameters.AddWithValue("@XML_PP", xmlpp);
                            comando.Parameters.AddWithValue("@XML_DET", xmlppwebdet);
                            comando.Parameters.AddWithValue("@FACTURA", factura);
                            String value = agente;
                            Char delimiter = '-';
                            List<string> substringagente = value.Split(delimiter).ToList();
                            comando.Parameters.AddWithValue("@AGENTE", substringagente[0].Trim());
                            String value_ = importador;
                            Char delimiter_ = '-';
                            List<string> substringimportador = value_.Split(delimiter_).ToList();
                            comando.Parameters.AddWithValue("@IMPORTADOR", substringimportador[0].Trim());
                            comando.Parameters.AddWithValue("@NAVE", nave);
                            comando.Parameters.AddWithValue("@FACTURADO", facturado);
                            comando.Parameters.AddWithValue("@ROLE", "");
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
                        //if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        //{
                        //    mensaje = ex.Message.ToString();
                        //}
                        //else
                        //{
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                        mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                        //}
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
                var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                return false;
            }
        }

        public static bool AcualizaPaseWebCFSCA(string xmlppweb, string xmlpp, string xmlppwebdet, string factura, string agente, string facturado, string role, string importador, string nave, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_ACTUALIZA_PASE_WEB_CFS_CA";
                            comando.Parameters.AddWithValue("@XML_PPWEB", xmlppweb);
                            comando.Parameters.AddWithValue("@XML_PP", xmlpp);
                            comando.Parameters.AddWithValue("@XML_DET", xmlppwebdet);
                            comando.Parameters.AddWithValue("@FACTURA", factura);
                            String value = agente;
                            Char delimiter = '-';
                            List<string> substringagente = value.Split(delimiter).ToList();
                            comando.Parameters.AddWithValue("@AGENTE", substringagente[0].Trim());
                            String value_ = importador;
                            Char delimiter_ = '-';
                            List<string> substringimportador = value_.Split(delimiter_).ToList();
                            comando.Parameters.AddWithValue("@IMPORTADOR", substringimportador[0].Trim());
                            comando.Parameters.AddWithValue("@NAVE", nave);
                            comando.Parameters.AddWithValue("@FACTURADO", facturado);
                            comando.Parameters.AddWithValue("@ROLE", role);
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
                        //if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        //{
                        //    mensaje = ex.Message.ToString();
                        //}
                        //else
                        //{
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                        mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                        //}
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
                var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                return false;
            }
        }

        public static bool AcualizaCambiosPaseWebCFS(string xmlppweb, string xmlpp, string xmlppwebdet, string factura, string agente, string facturado, string role, string importador, string nave, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_CAMBIOS_PASE_WEB_CFS";
                            comando.Parameters.AddWithValue("@XML_PPWEB", xmlppweb);
                            comando.Parameters.AddWithValue("@XML_PP", xmlpp);
                            comando.Parameters.AddWithValue("@XML_DET", xmlppwebdet);
                            comando.Parameters.AddWithValue("@FACTURA", factura);
                            String value = agente;
                            Char delimiter = '-';
                            List<string> substringagente = value.Split(delimiter).ToList();
                            comando.Parameters.AddWithValue("@AGENTE", substringagente[0].Trim());
                            String value_ = importador;
                            Char delimiter_ = '-';
                            List<string> substringimportador = value_.Split(delimiter_).ToList();
                            comando.Parameters.AddWithValue("@IMPORTADOR", substringimportador[0].Trim());
                            comando.Parameters.AddWithValue("@NAVE", nave);
                            comando.Parameters.AddWithValue("@FACTURADO", facturado);
                            comando.Parameters.AddWithValue("@ROLE", role);
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
                        //if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        //{
                        //    mensaje = ex.Message.ToString();
                        //}
                        //else
                        //{
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaCambiosPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                        mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                        //}
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
                var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaCambiosPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                return false;
            }
        }

        public static DataView CONSULTA_HORARIOS_DISPONIBLES_FULL(string fecha, string xml)
        {
            SqlConnection conn;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
            conn.Open();
            DataTable dt = new DataTable();
            ds = new DataSet();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.CFS_CONSULTA_HORARIOS_DISPONIBLES_FULL";
            cmd.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha;
            //cmd.Parameters.Add("@BODEGA", SqlDbType.NVarChar).Value = bodega;
            cmd.Parameters.Add("@LISTA_SUBITEMS", SqlDbType.Xml).Value = xml;
            dr = cmd.ExecuteReader();
            /*if (!dr.HasRows)
            {
            }
            else
            {*/
            dr.Close();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            /*}*/
            cmd.Parameters.Clear();
            conn.Close();
            conn.Dispose();
            return ds.Tables[0].DefaultView;
        }

        public static DataTable GetInfoFacturasClientePPWebCFSXCarga(string Ruc, string mrn, string msn, string hsn, string agente)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 7);
                coman.Parameters.AddWithValue("@FACTURADO", Ruc.Trim());
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@AGENTE", agente);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pase_web", "GetInfoFacturasClientePPWebCFSXCarga", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoFacturasClientePPWebCFSXCargaSubSec(string Ruc, string mrn, string msn, string hsn, string agente)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 10);
                coman.Parameters.AddWithValue("@FACTURADO", Ruc.Trim());
                coman.Parameters.AddWithValue("@MRN", mrn);
                coman.Parameters.AddWithValue("@MSN", msn);
                coman.Parameters.AddWithValue("@HSN", hsn);
                coman.Parameters.AddWithValue("@AGENTE", agente);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pase_web", "GetInfoFacturasClientePPWebCFSXCarga", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoEmails(string agente, string ciatrans, string importador)
        {
            var d = new DataTable();
            using (var c = ConexionportalMaster())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_INFO_EMAILS_PPWEB";
                coman.Parameters.AddWithValue("@AGENTE", agente);
                coman.Parameters.AddWithValue("@CIATRANS", ciatrans);
                coman.Parameters.AddWithValue("@IMPORTADOR", importador);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoEmails", "SP_GET_INFO_EMAILS_PPWEB", DateTime.Now.ToShortDateString());
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

        public static DataTable GetInfoCerrojo(string documento)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 8);
                coman.Parameters.AddWithValue("@DOCUMENTO", documento);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoCerrojo", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static DataTable GetTurnosInfoReefer(string xml_turnos, string horas_rf)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 5);
                coman.Parameters.AddWithValue("@XML_TURNOS", xml_turnos);
                var hrf = Convert.ToDateTime(horas_rf).ToString("yyyy-MM-dd HH:mm");
                coman.Parameters.AddWithValue("@HORAS_RF", hrf);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetTurnosInfoReefer", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static DataTable GetEmpresainfo()
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 11);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetEmpresainfo", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static DataTable GetEmpresainfoFilter(string filtro)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 14);
                coman.Parameters.AddWithValue("@FILTRO", filtro);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetEmpresainfo", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static DataTable GetChoferinfo(string empresa)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_CHOFER_INFO_S3";
                coman.Parameters.AddWithValue("@EMPRESA", empresa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetChoferinfo", "SP_GET_CHOFER_INFO_S3", DateTime.Now.ToShortDateString());
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

        public static DataTable GetChoferinfozal(string empresa)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_CHOFER_INFO_ZAL";
                coman.Parameters.AddWithValue("@EMPRESA", empresa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetChoferinfo", "SP_GET_CHOFER_INFO_S3", DateTime.Now.ToShortDateString());
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

        public static DataTable GetPlacainfo(string empresa)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_PLACA_INFO_S3";
                coman.Parameters.AddWithValue("@EMPRESA", empresa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetPlacainfo", "SP_GET_PLACA_INFO_S3", DateTime.Now.ToShortDateString());
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

        public static DataTable GetPlacainfozal(string empresa)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_PLACA_INFO_ZAL";
                coman.Parameters.AddWithValue("@EMPRESA", empresa);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetPlacainfo", "SP_GET_PLACA_INFO_S3", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValCliente(string factura)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 4);
                coman.Parameters.AddWithValue("@FACTURA", factura);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValTurno", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static DataTable GetDatosGeneraPaseWeb(string facturado, string mrn, string msn, string hsn, string cntr/*, string fechasalida*/)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 78);
                coman.Parameters.AddWithValue("@FACTURADO", facturado);
                if (!string.IsNullOrEmpty(mrn))
                {
                    coman.Parameters.AddWithValue("@MRN", mrn);
                }
                if (!string.IsNullOrEmpty(msn))
                {
                    coman.Parameters.AddWithValue("@MSN", msn);
                }
                if (!string.IsNullOrEmpty(hsn))
                {
                    coman.Parameters.AddWithValue("@HSN", hsn);
                }
                if (!string.IsNullOrEmpty(cntr))
                {
                    coman.Parameters.AddWithValue("@CONTENEDOR", cntr);
                }
                /*
                if (!string.IsNullOrEmpty(fechasalida))
                {                   
                    coman.Parameters.AddWithValue("@FECHA_SALIDA", fechasalida);
                }
                */
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetDatosPasWeb", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static DataTable CONSULTA_HORARIOS_DISPONIBLES_(string fecha, string xml)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "CFS_CONSULTA_HORARIOS_DISPONIBLES";
                coman.Parameters.AddWithValue("@FECHA", fecha);
                coman.Parameters.AddWithValue("@LISTA_SUBITEMS", xml);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "CONSULTA_HORARIOS_DISPONIBLES_", "CFS_CONSULTA_HORARIOS_DISPONIBLES", DateTime.Now.ToShortDateString());
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

        public static DataTable GetDatosActualizaCancelaPaseWeb(string facturado, string mrn, string msn, string hsn, string cntr, string fechasalida)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 79);
                coman.Parameters.AddWithValue("@FACTURADO", facturado);
                if (!string.IsNullOrEmpty(mrn))
                {
                    coman.Parameters.AddWithValue("@MRN", mrn);
                }
                if (!string.IsNullOrEmpty(msn))
                {
                    coman.Parameters.AddWithValue("@MSN", msn);
                }
                if (!string.IsNullOrEmpty(hsn))
                {
                    coman.Parameters.AddWithValue("@HSN", hsn);
                }
                if (!string.IsNullOrEmpty(cntr))
                {
                    coman.Parameters.AddWithValue("@CONTENEDOR", cntr);
                }
                if (!string.IsNullOrEmpty(fechasalida))
                {
                    coman.Parameters.AddWithValue("@FECHA_SALIDA", fechasalida);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetDatosPasWeb", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static DataTable GetDatosActualizaCancelaPaseWebCFS(string facturado, string mrn, string msn, string hsn, string cntr, string fechasalida)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 9);
                coman.Parameters.AddWithValue("@FACTURADO", facturado);
                if (!string.IsNullOrEmpty(mrn))
                {
                    coman.Parameters.AddWithValue("@MRN", mrn);
                }
                if (!string.IsNullOrEmpty(msn))
                {
                    coman.Parameters.AddWithValue("@MSN", msn);
                }
                if (!string.IsNullOrEmpty(hsn))
                {
                    coman.Parameters.AddWithValue("@HSN", hsn);
                }
                if (!string.IsNullOrEmpty(cntr))
                {
                    coman.Parameters.AddWithValue("@CONTENEDOR", cntr);
                }
                if (!string.IsNullOrEmpty(fechasalida))
                {
                    coman.Parameters.AddWithValue("@FECHA_SALIDA", fechasalida);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetDatosActualizaCancelaPaseWebCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetDatosActualizaCancelaPaseWebCFSS3_2019(string facturado, string mrn, string msn, string hsn, string cntr, string fechasalida)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 888);
                coman.Parameters.AddWithValue("@FACTURADO", facturado);
                coman.Parameters.AddWithValue("@AGENTE", facturado);
                if (!string.IsNullOrEmpty(mrn))
                {
                    coman.Parameters.AddWithValue("@MRN", mrn);
                }
                if (!string.IsNullOrEmpty(msn))
                {
                    coman.Parameters.AddWithValue("@MSN", msn);
                }
                if (!string.IsNullOrEmpty(hsn))
                {
                    coman.Parameters.AddWithValue("@HSN", hsn);
                }
                if (!string.IsNullOrEmpty(cntr))
                {
                    coman.Parameters.AddWithValue("@CONTENEDOR", cntr);
                }
                if (!string.IsNullOrEmpty(fechasalida))
                {
                    coman.Parameters.AddWithValue("@FECHA_SALIDA", fechasalida);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetDatosActualizaCancelaPaseWebCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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


        public static DataTable GetDatosActualizaCancelaPaseWebCFSS3(string facturado, string mrn, string msn, string hsn, string cntr, string fechasalida)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_CFS";
                coman.Parameters.AddWithValue("@TIPO", 99);
                coman.Parameters.AddWithValue("@FACTURADO", facturado);
                if (!string.IsNullOrEmpty(mrn))
                {
                    coman.Parameters.AddWithValue("@MRN", mrn);
                }
                if (!string.IsNullOrEmpty(msn))
                {
                    coman.Parameters.AddWithValue("@MSN", msn);
                }
                if (!string.IsNullOrEmpty(hsn))
                {
                    coman.Parameters.AddWithValue("@HSN", hsn);
                }
                if (!string.IsNullOrEmpty(cntr))
                {
                    coman.Parameters.AddWithValue("@CONTENEDOR", cntr);
                }
                if (!string.IsNullOrEmpty(fechasalida))
                {
                    coman.Parameters.AddWithValue("@FECHA_SALIDA", fechasalida);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetDatosActualizaCancelaPaseWebCFS", "SP_PROCESO_PASE_WEB_CFS", DateTime.Now.ToShortDateString());
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

        public static DataTable GetDatosActualizaCancelaPaseWebBRBKS3(string facturado, string mrn, string msn, string hsn, string cntr, string fechasalida)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB_BBK";
                coman.Parameters.AddWithValue("@TIPO", 99);
                coman.Parameters.AddWithValue("@FACTURADO", facturado);
                if (!string.IsNullOrEmpty(mrn))
                {
                    coman.Parameters.AddWithValue("@MRN", mrn);
                }
                if (!string.IsNullOrEmpty(msn))
                {
                    coman.Parameters.AddWithValue("@MSN", msn);
                }
                if (!string.IsNullOrEmpty(hsn))
                {
                    coman.Parameters.AddWithValue("@HSN", hsn);
                }
                if (!string.IsNullOrEmpty(cntr))
                {
                    coman.Parameters.AddWithValue("@CONTENEDOR", cntr);
                }
                if (!string.IsNullOrEmpty(fechasalida))
                {
                    coman.Parameters.AddWithValue("@FECHA_SALIDA", fechasalida);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetDatosActualizaCancelaPaseWebBRBKS3", "SP_PROCESO_PASE_WEB_BBK", DateTime.Now.ToShortDateString());
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

        public static DataTable GetValTurno(string fecha_s, string turno)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_PASE_WEB";
                coman.Parameters.AddWithValue("@TIPO", 3);
                coman.Parameters.AddWithValue("@FECHA_S", fecha_s);
                coman.Parameters.AddWithValue("@TURNO", turno);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetValTurno", "SP_PROCESO_PASE_WEB", DateTime.Now.ToShortDateString());
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

        public static bool ActualizaDatosPaseWeb(string xmlppweb, string xmlsn, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_ACTUALIZA_PASE_WEB";
                            comando.Parameters.AddWithValue("@XML_PPWEB", xmlppweb);
                            comando.Parameters.AddWithValue("@XML_SN", xmlsn);
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
                        /*
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        */
                        var t = log_csl.save_log<Exception>(ex, "pasePuerta", "ActualizaDatosPaseWeb", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "pasePuerta", "ActualizaDatosPaseWeb", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool ActualizaOCancelaPasePuertaWeb(string xmlppweb, string xmlsn, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_CANCELA_ACTUALIZA_PASE_DE_PUERTA_WEB";
                            comando.Parameters.AddWithValue("@XML_PPWEB", xmlppweb);
                            comando.Parameters.AddWithValue("@XML_SN", xmlsn);
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
                        /*
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        */
                        var t = log_csl.save_log<Exception>(ex, "pasePuerta", "ActualizaDatosPaseWeb", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "pasePuerta", "ActualizaDatosPaseWeb", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static void CANCELA_HORARIOS_PASEPUERTA(String xml, string usuario)
        {
            string mensaje = "";
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.CFS_PROCESO_DESPACHO_CARGA";
                            comando.Parameters.AddWithValue("@TIPO", 6);
                            comando.Parameters.AddWithValue("@XML_CPP", xml);
                            comando.Parameters.AddWithValue("@USUARIOMOD", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            //return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        /*
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        */
                        var t = log_csl.save_log<Exception>(ex, "pasePuerta", "CANCELA_HORARIOS_PASEPUERTA", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);

                        //return false;
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
                var t = log_csl.save_log<Exception>(ex, "pasePuerta", "CANCELA_HORARIOS_PASEPUERTA", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                //return false;
            }
        }

        public static bool CancelaPasePuertaWebCFS(string xmlppweb, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_CANCELA_PASE_DE_PUERTA_WEB_CFS";
                            comando.Parameters.AddWithValue("@XML_PPWEB", xmlppweb);
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
                        /*
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        */
                        var t = log_csl.save_log<Exception>(ex, "pase_web", "SP_CANCELA_PASE_DE_PUERTA_WEB", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "pase_web", "SP_CANCELA_PASE_DE_PUERTA_WEB", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool AcualizaPaseWebBRBK(string xmlppweb, string xmlpp, string xmlppwebdet, string factura, string agente, string facturado, string role, string importador, string nave, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = ConexionN4Middleware())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_ACTUALIZA_PASE_WEB_BBK";
                            comando.Parameters.AddWithValue("@XML_PPWEB", xmlppweb);
                            comando.Parameters.AddWithValue("@XML_PP", xmlpp);
                            comando.Parameters.AddWithValue("@XML_DET", xmlppwebdet);
                            comando.Parameters.AddWithValue("@FACTURA", factura);
                            String value = agente;
                            Char delimiter = '-';
                            List<string> substringagente = value.Split(delimiter).ToList();
                            comando.Parameters.AddWithValue("@AGENTE", substringagente[0].Trim());
                            String value_ = importador;
                            Char delimiter_ = '-';
                            List<string> substringimportador = value_.Split(delimiter_).ToList();
                            comando.Parameters.AddWithValue("@IMPORTADOR", substringimportador[0].Trim());
                            comando.Parameters.AddWithValue("@NAVE", nave);
                            comando.Parameters.AddWithValue("@FACTURADO", facturado);
                            comando.Parameters.AddWithValue("@ROLE", role);
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
                        //if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        //{
                        //    mensaje = ex.Message.ToString();
                        //}
                        //else
                        //{
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebBRBK", DateTime.Now.ToShortDateString(), "facturacion");
                        mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                        //}
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
                var t = log_csl.save_log<Exception>(ex, "pase_web", "AcualizaPaseWebCFS", DateTime.Now.ToShortDateString(), "facturacion");
                mensaje = string.Format("Error al generar Pase Web, código de error {0}", t);
                return false;
            }
        }
    }
}
