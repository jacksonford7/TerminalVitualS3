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
        private static SqlConnection ConexionN5()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString);
        }

        private static SqlConnection Conexionvalidar()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["validar"].ConnectionString);
        }

        private static SqlConnection conexionmidle()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }

        private static SqlConnection conexionecuap()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"].ConnectionString);
        }


        public static bool ActualizaLiquidacionPasePuertaZAL(String wxml, out string error)
        {
            string mensaje = "";
            error = mensaje;

            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.VBS_P_ACT_PASE_PUERTA_ZAL";
                            comando.Parameters.AddWithValue("@xmlDatos", wxml);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            error = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        {
                            mensaje = ex.Message.ToString();
                            error = mensaje;
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (SqlError e in ex.Errors)
                            {
                                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                            }
                            var t = log_csl.save_log<Exception>(ex, "proforma_zal", "ActualizaLiquidacionPasePuertaZAL", DateTime.Now.ToShortDateString(), "sistema");
                            mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                            error = mensaje;
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
                var t = log_csl.save_log<Exception>(ex, "proforma_zal", "ActualizaLiquidacionPasePuertaZAL", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                error = mensaje;
                return false;
            }

        }


        public static bool SaveCancelarPasePuertaZAL(String wxml, out string error)
        {
            string mensaje = "";
            error = mensaje;

            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.VBS_P_CAN_PASE_PUERTA_ZAL";
                            comando.Parameters.AddWithValue("@xmlDatos", wxml);                  
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            error = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        {
                            mensaje = ex.Message.ToString();
                            error = mensaje;  
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (SqlError e in ex.Errors)
                            {
                                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                            }
                            var t = log_csl.save_log<Exception>(ex, "proforma_zal", "SaveCancelarPasePuertaZAL", DateTime.Now.ToShortDateString(), "sistema");
                            mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                            error = mensaje;
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
                var t = log_csl.save_log<Exception>(ex, "proforma_zal", "SaveCancelarPasePuertaZAL", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                error = mensaje;
                return false;
            }

        }

        public static bool AddPaseZAL_Transferencia(string xml, string email, out int idpase, out string Mensaje, long deposito)
        {
            string cError = "";
          
            idpase = 0;
            Mensaje = string.Empty;
            bool Grabo = false;
            try
            {

                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.VBS_P_PASE_PUERTA_ZAL_TRANSFERENCIA";
                            comando.Parameters.AddWithValue("@xmlDatos", xml);
                            comando.Parameters.AddWithValue("@eMail", email);
                            var pase = comando.Parameters.Add("@pase", SqlDbType.Int);
                            var msg = comando.Parameters.Add("@mensaje", SqlDbType.VarChar,500);
                            comando.Parameters.AddWithValue("@deposito", deposito);
                            pase.Direction = ParameterDirection.Output;
                            msg.Direction = ParameterDirection.Output;

                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            idpase = int.Parse(comando.Parameters[2].Value.ToString());
                            Mensaje = comando.Parameters[3].Value.ToString();
                            cError = result;
                            Grabo = true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        {
                            cError = ex.Message.ToString();
                            Grabo = false;
                            Mensaje = cError;
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (SqlError e in ex.Errors)
                            {
                                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                            }
                            var t = log_csl.save_log<Exception>(ex, "transferencia_zal", "AddPaseZAL_Transferencia", DateTime.Now.ToShortDateString(), "sistema");
                            cError = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                            Grabo = false;
                            Mensaje = cError;
                        }   
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }

                    return Grabo;

                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "transferencia_zal", "AddPaseZAL_Transferencia", DateTime.Now.ToShortDateString(), "sistema");
                cError = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                Grabo = false;
                Mensaje = cError;
            }

            return Grabo;

        }

        public static bool SaveCancelarPasePuertaZAL_Transferencia(String wxml, out string error)
        {
            string mensaje = "";
            error = mensaje;

            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.VBS_P_CAN_PASE_PUERTA_ZAL_TRANSFERENCIA";
                            comando.Parameters.AddWithValue("@xmlDatos", wxml);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            error = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
                        {
                            mensaje = ex.Message.ToString();
                            error = mensaje;
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (SqlError e in ex.Errors)
                            {
                                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                            }
                            var t = log_csl.save_log<Exception>(ex, "transferencia_zal", "SaveCancelarPasePuertaZAL_Transferencia", DateTime.Now.ToShortDateString(), "sistema");
                            mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                            error = mensaje;
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
                var t = log_csl.save_log<Exception>(ex, "transferencia_zal", "SaveCancelarPasePuertaZAL_Transferencia", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                error = mensaje;
                return false;
            }

        }


        public static bool AddPaseZAL(string xml,string email, long deposito,out int idpase)
        {
            string mensaje = "";
            int pas = 0;
            idpase = pas;

            try
            {

                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.VBS_P_PASE_PUERTA_ZAL";
                            comando.Parameters.AddWithValue("@xmlDatos", xml);
                            comando.Parameters.AddWithValue("@eMail", email);
                            var p = comando.Parameters.Add("@pase", SqlDbType.Int);
                            comando.Parameters.AddWithValue("@deposito", deposito);
                            p.Direction = ParameterDirection.Output;
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            pas = int.Parse(comando.Parameters[2].Value.ToString());
                            mensaje = result;

                            if (mensaje.Contains("ya se encuentra autorizado"))
                                return false;
                            else
                            {
                                idpase = pas;
                                return true;
                            }
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
                            var t = log_csl.save_log<Exception>(ex, "proforma_zal", "AddPaseZAL", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "proforma_zal", "AddPaseZAL", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                idpase = pas;
                return false;
            }
        }

        public static DataTable GetDetallePaseZAL(string bkg, string refe, string fecsal, string linea, long deposito)
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_ID_PASE_ZAL";
                coman.Parameters.AddWithValue("@BOOKING", bkg);
                coman.Parameters.AddWithValue("@REFERENCIA", refe);
                coman.Parameters.AddWithValue("@FECHASAL", fecsal);
                coman.Parameters.AddWithValue("@LINEA", linea);
                coman.Parameters.AddWithValue("@deposito", deposito);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "service", "SP_GET_ID_PASE_ZAL", DateTime.Now.ToString(), "GetIdPasZAL");
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

        public static DataTable GetDetallePaseZAL_Ruc(string bkg, string refe, string fecsal, string linea, long deposito, string ruc)
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_ID_PASE_ZAL_RUC";
                coman.Parameters.AddWithValue("@BOOKING", bkg);
                coman.Parameters.AddWithValue("@REFERENCIA", refe);
                coman.Parameters.AddWithValue("@FECHASAL", fecsal);
                coman.Parameters.AddWithValue("@LINEA", linea);
                coman.Parameters.AddWithValue("@deposito", deposito);
                coman.Parameters.AddWithValue("@RUC", ruc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "service", "SP_GET_ID_PASE_ZAL", DateTime.Now.ToString(), "GetIdPasZAL");
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

        public static DataTable GetDetallePaseZALContado(string bkg, string refe, string fecsal, string linea, long deposito)
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_ID_PASE_ZAL_NUEVO";
                coman.Parameters.AddWithValue("@BOOKING", bkg);
                coman.Parameters.AddWithValue("@REFERENCIA", refe);
                coman.Parameters.AddWithValue("@FECHASAL", fecsal);
                coman.Parameters.AddWithValue("@LINEA", linea);
                coman.Parameters.AddWithValue("@deposito", deposito);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "service", "SP_GET_ID_PASE_ZAL_NUEVO", DateTime.Now.ToString(), "GetIdPasZAL");
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

        public static DataTable GetDetallePaseZALContado_Ruc(string bkg, string refe, string fecsal, string linea, long deposito, string ruc)
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_GET_ID_PASE_ZAL_NUEVO_RUC";
                coman.Parameters.AddWithValue("@BOOKING", bkg);
                coman.Parameters.AddWithValue("@REFERENCIA", refe);
                coman.Parameters.AddWithValue("@FECHASAL", fecsal);
                coman.Parameters.AddWithValue("@LINEA", linea);
                coman.Parameters.AddWithValue("@deposito", deposito);
                coman.Parameters.AddWithValue("@RUC", ruc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "service", "SP_GET_ID_PASE_ZAL_NUEVO", DateTime.Now.ToString(), "GetIdPasZAL");
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

        public static bool AddCancelPaseZAL(
        string pase,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.VBS_P_CANCEL_PASE_PUERTA_ZAL";
                            comando.Parameters.AddWithValue("@PASE", pase);
                            comando.Parameters.AddWithValue("@USER", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            /*
                            if (mensaje.Contains("ya se encuentra autorizado"))
                                return false;
                            else
                                return true;*/

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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddCancelPaseZAL", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddCancelPaseZAL", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool AddUpdatePaseZAL(
        string pase,
        string usuario,
        string chofer,
        string placa,
        out string mensaje)
        {
            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.VBS_P_UPDATE_PASE_PUERTA_ZAL";
                            comando.Parameters.AddWithValue("@PASE", pase);
                            comando.Parameters.AddWithValue("@USER", usuario);
                            comando.Parameters.AddWithValue("@CHOFER", chofer);
                            comando.Parameters.AddWithValue("@PLACA", placa);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            /*
                            if (mensaje.Contains("ya se encuentra autorizado"))
                                return false;
                            else
                                return true;*/

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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddUpdatePaseZAL", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddUpdatePaseZAL", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static DataTable GetCatalagoLineaAsumeHorasReefer()
        {
            var d = new DataTable();
            using (var c = Conexionvalidar())
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
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "GetCatalagoLineaAsumeHorasReefer", DateTime.Now.ToShortDateString(), "sistema");
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
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "getPoblarCredenciales", DateTime.Now.ToShortDateString(), "sistema");
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
                    csl_log.log_csl.save_log<SqlException>(ex, "man_pro_expo", "GetCatalagoReferenciasPagoTerceros", "SP_PROCESO_MANTENIMIENTOS_HORAS_REEFER", "@TIPO=0");
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

        public static DataTable GetCatalagoClientesPagoTerceros(string ruc)
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_PROCESO_MANTENIMIENTO_PAGO_TERCEROS";
                coman.Parameters.AddWithValue("@TIPO", 1);
                coman.Parameters.AddWithValue("@RUC", ruc);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "GetCatalagoClientesPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
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

        public static DataTable GetAsumePagoTerceros(string ruc_cliente, string referencia)
        {
            var d = new DataTable();
            using (var c = Conexionvalidar())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_CONSULTA_PAGO_TERCEROS";
                if (!string.IsNullOrEmpty(ruc_cliente))
                {
                    coman.Parameters.AddWithValue("@SEPT_RUC_CLIENTE", ruc_cliente);
                }
                if (!string.IsNullOrEmpty(referencia))
                {
                    coman.Parameters.AddWithValue("@SAPT_REFERENCIA", referencia);
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "GetAsumePagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
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

        public static DataTable GetValidaPagoTerceros(string ruc_cliente, string ruc_asume)
        {
            var d = new DataTable();
            using (var c = Conexionvalidar())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "SP_VALIDA_PAGO_TERCEROS";
                coman.Parameters.AddWithValue("@SEPT_RUC_CLIENTE", ruc_cliente);
                coman.Parameters.AddWithValue("@SEPT_RUC_ASUME", ruc_asume);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "GetValidaPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
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

        public static DataTable GetCntrXBook(string ruc_cliente, string referencia)
        {
            var d = new DataTable();
            using (var c = ConexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "FNA_CONTENEDORES_X_BOOKING";
                coman.Parameters.AddWithValue("@RUC", ruc_cliente);
                coman.Parameters.AddWithValue("@REFERENCIA", referencia);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "GetCntrXBook", DateTime.Now.ToShortDateString(), "sistema");
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
        int asumetodo,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = Conexionvalidar())
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
                            comando.Parameters.AddWithValue("@ASUME_TODO", asumetodo);
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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddManHorasReefer", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddManHorasReefer", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool AddPagoTerceros(
        string ruccliente,
        string xmlpagoterceros,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = Conexionvalidar())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_INSERT_PAGO_TERCEROS";
                            comando.Parameters.AddWithValue("@SEPT_RUC_CLIENTE", ruccliente);
                            comando.Parameters.AddWithValue("@XMLPAGOTERCEROS", xmlpagoterceros);
                            comando.Parameters.AddWithValue("@SEPT_USUARIOINGRESO", usuario);
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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool AddMantPagoTerceros(
        string xmlpagoterceros,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = Conexionvalidar())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.SP_INSERT_MANT_PAGO_TERCEROS";
                            comando.Parameters.AddWithValue("@XMLPAGOTERCEROS", xmlpagoterceros);
                            comando.Parameters.AddWithValue("@SEPT_USUARIOINGRESO", usuario);
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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool AddBookAutoriza(
        string booking,
        string referencia,
        string linea,
        string usuario,
        out string mensaje)
        {
            try
            {
                using (var conn = Conexionvalidar())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.SP_INSERT_BOOK_AUTORIZA";
                            comando.Parameters.AddWithValue("@BOOKING", booking);
                            comando.Parameters.AddWithValue("@REFERENCIA", referencia);
                            comando.Parameters.AddWithValue("@LINEA", linea);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;

                            if (mensaje.Contains("ya se encuentra autorizado"))
                                return false;
                            else
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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddBookAutoriza", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddBookAutoriza", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool AddDae(
        string dae,
        string ruc,
        out string mensaje)
        {
            try
            {
                using (var conn = Conexionvalidar())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 3000;
                            comando.CommandText = "dbo.SP_CONSULTA_DAE";
                            comando.Parameters.AddWithValue("@DAE", dae);
                            comando.Parameters.AddWithValue("@EXP", ruc);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;

                            if (mensaje.Contains("ya se encuentra autorizado"))
                                return false;
                            else
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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "ConsultaDae", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "ConsultaDae", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static DataView consultaDepositos()
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "ZAl_C_Depot";
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

        public static DataView consultaDepositosFiltrado(string opcion )
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "ZAl_C_Depot_Filtro";
                coman.Parameters.AddWithValue("@opcion", opcion);

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

        public static DataView consultaLineas()
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "ZAl_C_LINEAS";

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

        public static DataView consultaLineas_Sav(Int64 ID_DEPOT)
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "ZAl_C_LINEAS_SAV";
                coman.Parameters.AddWithValue("@ID_DEPOT", ID_DEPOT);
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

        public static DataTable GetDae(string dae, string ruc)
        {
            var d = new DataTable();
            using (var c = conexionecuap())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "ECU_SP_GET_DAE";
                coman.Parameters.AddWithValue("@EXP", ruc);
                coman.Parameters.AddWithValue("@DAE", dae);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "GetDae", DateTime.Now.ToShortDateString(), "sistema");
                    return d;
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

       public static bool UpdateTurnoPorLinea(long idPase, long idPaseLinea ,out string mensaje)
        {
            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.VBS_P_UPDATE_TURNO_ZAL_DEPOT";
                            comando.Parameters.AddWithValue("@I_ID_PPZAL", idPase);
                            comando.Parameters.AddWithValue("@I_ID_PASE_LINEA", idPaseLinea);
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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "UpdateTurnoPorLinea", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool CancelarTurnoPorLinea(long idPase, string usuario, out string mensaje)
        {
            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.VBS_P_CANCELAR_TURNO_ZAL_DEPOT";
                            comando.Parameters.AddWithValue("@I_ID_PPZAL", idPase);
                            comando.Parameters.AddWithValue("@I_USUARIO", usuario);
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
                            var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "CancelarTurnoPorLinea", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "man_pro_expo", "AddPagoTerceros", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
    }
}