using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using csl_log;
using System.Data.SqlClient;
using System.Data;


namespace ProcesaUnits
{
    public class DataHelper
    {
        public static bool UnitIsCreated(string cntr)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [n5].[dbo].[fx_cntr_exists](@cntr)";
                comando.Parameters.AddWithValue("@cntr", cntr);
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
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "UnitIsCreated", cntr, "N4");
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

        public DataTable GetTablaNaves(string fecha)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                using (SqlCommand comando = con.CreateCommand())
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandText = "[mb_get_VESSEL_BOOKING_info]";
                    comando.Parameters.AddWithValue("@FECHA", fecha);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            tabla.Load(reader);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        log_csl.save_log<Exception>(ex, "jAisvContainer", "GetTabla", fecha, "N4");
                    }
                }
            }

            return tabla;
        }


        public static void InsertTurnos(int idTurno,
            string usuarioModificacion, DateTime fechaModificacion, string aisv, string contenedor, string chofer, string placa)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VBS"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[VBS_UPDATE_TURNOS_DISPONIBLE]";
                comando.Parameters.AddWithValue("ID_TURNO", idTurno);
                comando.Parameters.AddWithValue("USUARIO_MODIFICACION", usuarioModificacion);
                comando.Parameters.AddWithValue("FECHA_MODIFICACION", fechaModificacion);
                comando.Parameters.AddWithValue("AISV", aisv);
                comando.Parameters.AddWithValue("CONTENEDOR", contenedor);
                comando.Parameters.AddWithValue("CHOFER", chofer);
                comando.Parameters.AddWithValue("PLACA", placa);
  
                try
                {
                    con.Open();
                    int sale = comando.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar los turnos", ex);
                   // log_csl.save_log<Exception>(ex, "DataHelper", "InsertTurnos", V, "N4");
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



        public static void InsertUnits(string xmsl)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["validar"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[CSL].[CSL_P_Mant_cslContenedorVacio]";
                comando.Parameters.AddWithValue("@xmlDatos", xmsl);
                try
                {
                    con.Open();
                    int sale = comando.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "DataHelper", "InsertUnits", @xmsl, "N4");
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

        public static Tuple<string,string,string,string,string> GetAISVData (string cntr,string booking)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "sp_get_aisv_data";
                comando.Parameters.AddWithValue("@contenedor", cntr);
                comando.Parameters.AddWithValue("@booking", booking);
                try
                {
                    con.Open();
                    var sale = comando.ExecuteReader();
                    if (sale.HasRows)
                    {
                        sale.Read();
                        string mailes = sale[1] as string;
                        if (sale[2] != DBNull.Value)
                        {
                            mailes = string.Concat(mailes, ";", sale.GetString(2));
                        }
                        if (sale[3] != DBNull.Value)
                        {
                            mailes = string.Concat(mailes, ";", sale.GetString(3));
                        }
                        if (sale[4] != DBNull.Value)
                        {
                            mailes = string.Concat(mailes, ";", sale.GetString(4));
                        }
                        if (sale[5] != DBNull.Value)
                        {
                            mailes = string.Concat(mailes, ";", sale.GetString(5));
                        }
                        return Tuple.Create(sale[0] as string, mailes, sale[6] as string, sale.GetDateTime(7).ToString("dd/MM/yyyy HH:mm"), sale[8] as string);
                    }
                    con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "DataHelper", "UnitIsCreatedService", cntr, "N4");
                    return null;
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

        public static bool getOk(string server, string user, string pass, string catalogo, ref string sqlmessage)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                SqlConnectionStringBuilder sqq = new SqlConnectionStringBuilder();
                sqq.DataSource = server;
                sqq.UserID = user;
                sqq.Password = pass;
                sqq.InitialCatalog = catalogo;
                sqq.ConnectTimeout = 20;
                con.ConnectionString = sqq.ConnectionString;
                try
                {
                    con.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2}",e.LineNumber,e.Server,e.Message));
                    }
                    sqlmessage = sb.ToString();
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
        }

    
    }
}