using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using csl_log;

namespace CSLSite
{
    public class atraqueHelper
    {
        public static List<item> get_lines(string key)
        {
            //gkey,disponible,boking,iso,descripcion,referencia
        

            var xsalida = new List<item>();
            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
            if (con == null)
            {
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "[n5].[dbo].[pc_getlinesByService]";
                com.Parameters.AddWithValue("@service", key);
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(new item(reader[0] as string, reader[1] as string));
                        }
                    }
                    con.Close();
                    com.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "atraqueHelper", "GetLines", key.ToString(), "N4");
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return xsalida;
        }
        public static List<item> get_services(string key=null)
        {
            //gkey,disponible,boking,iso,descripcion,referencia
            var xsalida = new List<item>();
            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
            if (con == null)
            {
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "[n5].[dbo].[pc_getServices]";
                if (!string.IsNullOrEmpty(key))
                {
                    com.Parameters.AddWithValue("@service", key);
                }
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(new item(reader[0] as string, reader[1] as string));
                        }
                    }
                    con.Close();
                    com.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "atraqueHelper", "GetLines", key.ToString(), "N4");
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
        public static bool insertarAviso(string para, string copia, string asunto, string mensajeHTML, string usuario, int modulo, int evento, string sistema)
        {
            using (var con = new SqlConnection())
            {
                try
                {
                    var conx = System.Configuration.ConfigurationManager.ConnectionStrings["service"];
                    if (conx == null)
                    {
                        return false;
                    }
                    con.ConnectionString = conx.ConnectionString;
                    var mcom = con.CreateCommand();
                    mcom.CommandType = CommandType.StoredProcedure;
                    mcom.CommandText = "sp_insert_mail_log_pan";
                    mcom.Parameters.AddWithValue("@htmlmsg", mensajeHTML);
                    mcom.Parameters.AddWithValue("@mailpara", para);
                    mcom.Parameters.AddWithValue("@copiaspara", copia);
                    mcom.Parameters.AddWithValue("@usuario", usuario);
                    mcom.Parameters.AddWithValue("@asunto", asunto);
                    mcom.Parameters.AddWithValue("@moduloID", modulo);
                    mcom.Parameters.AddWithValue("@codigo_evento", evento);
                    mcom.Parameters.AddWithValue("@sistema", sistema);
                    con.Open();
                    mcom.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch
                {

                    return false;
                }
            }
        }
    }

    public class item
    {
        public string codigo { get; set; }
        public string valor { get; set; }
        public item(string codigo, string valor) { this.codigo = codigo.Trim(); this.valor = valor.Trim(); }
    }
}