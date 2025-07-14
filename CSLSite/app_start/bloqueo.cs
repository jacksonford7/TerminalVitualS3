using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CSLSite.app_start
{
    public class bloqueo
    {
        private static SqlConnection ConexionN4Middle()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }

        public DataTable GetLockClientes(String cliente)
        {
            DataTable wvresult = new DataTable();

           
                using (var c = ConexionN4Middle())
                {
                    var coman = c.CreateCommand();
                    coman.CommandType = CommandType.StoredProcedure;
                    coman.CommandText = "FNA_P_LOCK_CLIENTS_GET";
                    coman.Parameters.AddWithValue("@CLNT_CUSTOMER", cliente);
                    try
                    {
                        c.Open();
                        wvresult.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                    }
                    catch (SqlException ex)
                    {
                        csl_log.log_csl.save_log<SqlException>(ex, "Bloqueo", "GetLockClientes", "FNA_P_LOCK_CLIENTS_GET", DateTime.Now.ToShortDateString());
                    }
                    finally
                    {
                        if (c.State == ConnectionState.Open)
                        {
                            c.Close();
                        }
                        c.Dispose();
                    }
                }
                


            return wvresult;
        }


        public DataTable RLibLockCliente(DateTime FechaIni, DateTime FechaFin, string Estado, string Cliente)
        {
            DataTable wvresult = new DataTable();


            using (var c = ConexionN4Middle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "FNA_PRO_LOCK_CLIENTS";
                coman.Parameters.AddWithValue("@FECHA_INI", FechaIni);
                coman.Parameters.AddWithValue("@FECHA_FIN", FechaFin);
                coman.Parameters.AddWithValue("@DT_STATUS", Estado);
                coman.Parameters.AddWithValue("@CUSTOMER", Cliente==""? null: Cliente);
                try
                {
                    c.Open();
                    wvresult.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "ReportBloqueo", "RLibLockCliente", "FNA_PRO_LIBERATION_CLIENTS", DateTime.Now.ToShortDateString());
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }



            return wvresult;
        }
    }
}