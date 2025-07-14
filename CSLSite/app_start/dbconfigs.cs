using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using csl_log;
using System.Text;

namespace CSLSite
{
    public class dbconfig
    {
        public int id { get; set; }
        public string app_class { get; set; }
        public string config_name { get; set; }
        public string config_value { get; set; }

        public static List<dbconfig> GetActiveConfig(string db = null, string app = null, string cname = null)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[dbo].[pc_get_configs]";

                if (!string.IsNullOrEmpty(db))
                {
                    comando.Parameters.AddWithValue("@database_name", db.Trim());
                }

                if (!string.IsNullOrEmpty(app))
                {
                    comando.Parameters.AddWithValue("@app_name", app.Trim());
                }

                if (!string.IsNullOrEmpty(cname))
                {
                    comando.Parameters.AddWithValue("@config_name", cname.Trim());
                }

                try
                {
                    con.Open();
                    var oreader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                    dbconfig cf = null;
                    List<dbconfig> lsal = new List<dbconfig>();
                    while (oreader.Read())
                    {
                        cf = new dbconfig();
                        cf.id = oreader.GetInt32(0);
                        cf.app_class = oreader[1] as string;
                        cf.config_name = oreader[2] as string;
                        cf.config_value = oreader[3] as string;
                        lsal.Add(cf);
                    }
                    return lsal;
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "dbconfig", "GetActiveConfig", cname, "configuraciones");
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
        public static string GrupoUnidad(Int64 key, string estados)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[dbo].[pc_cntr_grupo_key]";

                if (!string.IsNullOrEmpty(estados))
                {
                    comando.Parameters.AddWithValue("@estado", estados.Trim().ToUpper());
                }
                comando.Parameters.AddWithValue("@key", key);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return sale.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "dbconfig", "GrupoUnidad", key.ToString(), "nuevo");
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

        public static bool UpdateAISV(string aisv, string usuario, string error)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[dbo].[pc_update_replicar]";

                if (!string.IsNullOrEmpty(error))
                {
                    comando.Parameters.AddWithValue("@error", error);
                }
                comando.Parameters.AddWithValue("@aisv", aisv);
                comando.Parameters.AddWithValue("@usuario", usuario);
                try
                {
                    con.Open();
                    int sale = comando.ExecuteNonQuery();
                    con.Close();
                    if (sale > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "dbconfig", "UpdateAISV", aisv, "UPDATE-REPLICAR");
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

        public static bool addMail(string mailpara, string asunto, string htmlmensaje, string copiaspara, string usuario)
        {
                using (var xcon = new SqlConnection())
                {
                    try
                    {
                        xcon.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                        var comando = xcon.CreateCommand();
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.CommandText = "dbo.sp_insert_mail_log";
                        comando.Parameters.AddWithValue("@asunto", asunto);
                        comando.Parameters.AddWithValue("@htmlmsg", htmlmensaje);
                        comando.Parameters.AddWithValue("@mailpara", mailpara);
                        comando.Parameters.AddWithValue("@copiaspara", copiaspara);
                        comando.Parameters.AddWithValue("@usuario", usuario);
                        try
                        {
                            xcon.Open();
                            comando.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (SqlError e in ex.Errors)
                            {
                                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                            }
                            var t = log_csl.save_log<SqlException>(ex, "turno", "add-Trx", "", "");
                            return false;
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        var t = log_csl.save_log<Exception>(ex, "turno", "add-gral", "A", "A");
                        return false;
                    }
                }
            }
        }
    
        
//----------------->
}