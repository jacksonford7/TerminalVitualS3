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
    public enum tComando { Texto, Procedure }
    public class CLSData
    {
        //metodo que devuelve la conexión nada mas
        private static SqlConnection getConex()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString);
        }
        //ejecuta un sp, query, y RETORNA 1 SOLO VALOR ESCALAR!!!
        public static string ValorEscalar(string comandtext, Dictionary<string, string> parametros, tComando tipo)
        {
            string salida = null;
            try
            {
                using (var con = getConex())
                {
                    var com = con.CreateCommand();
                    com.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    com.CommandTimeout = 60;
                    com.CommandText = comandtext.Trim().ToUpper();
                    foreach (var item in parametros)
                    {
                        com.Parameters.AddWithValue("@" + item.Key, item.Value);
                    }
                    try
                    {
                        con.Open();
                        var objeto = com.ExecuteScalar();
                        con.Close();
                        if (objeto != null && objeto.GetType() != typeof(DBNull))
                        {
                            salida = objeto.ToString();
                        }
                        return salida;
                    }
                    catch (SqlException ex)
                    {
                        var errlis = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            errlis.AppendFormat("Linea:{0},Mensaje:{1},Numero:{2},Servidor:{3}", e.LineNumber, e.Message, e.Number, e.Server);
                        }
                        log_csl.save_log<ApplicationException>(new ApplicationException(errlis.ToString()), "CLSData", "ValorEscalar", comandtext, "N4");
                        return salida;
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "CLSData", "ValorEscalar", comandtext, "N4");
                return salida;
            }
        }
        //ejecuta un reader y retorna el yield
        public static IEnumerable<IDataRecord> ValorLecturas(string SelectCommandText, tComando tipo, Dictionary<string, string> parametros = null)
        {
            using (var conn = getConex())
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = SelectCommandText;
                    cmd.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandTimeout = 60;
                    cmd.CommandText = SelectCommandText.Trim().ToUpper();
                    if (parametros != null)
                    {
                        foreach (var item in parametros)
                        {
                            cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                        }
                    }
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            yield return reader;
                        }
                    }
                }
            }
        }
        //ejecuta un reader y retorna el yield
        public static IEnumerable<IDataRecord> ValorLecturas2(string SelectCommandText, tComando tipo, int idservicio, int idusuario, int? idfiltro)
        {
            using (var conn = getConex())
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = SelectCommandText;
                    cmd.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandTimeout = 60;
                    cmd.Parameters.AddWithValue("@idservicio", idservicio);
                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
                    cmd.Parameters.AddWithValue("@idfiltro", idfiltro);
                    cmd.CommandText = SelectCommandText.Trim().ToUpper();

                    //if (parametros != null)
                    //{
                    //    foreach (var item in parametros)
                    //    {
                    //        cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                    //    }
                    //}
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            yield return reader;
                        }
                    }
                }
            }
        }

        public static  List<DataRow> ValorLecturasOffLine(string SelectCommandText, tComando tipo, Dictionary<string, string> parametros = null)
        {
            var hsk = new List<DataRow>();
            using (var conn = getConex())
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = SelectCommandText;
                    cmd.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandTimeout = 60;
                    cmd.CommandText = SelectCommandText.Trim().ToUpper();
                    if (parametros != null)
                    {
                        foreach (var item in parametros)
                        {
                            cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                        }
                    }
                    var t = new DataTable();
                    try
                    {
                        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            t.Load(reader);
                        }
                    }
                    catch (TimeoutException ex)
                    {
                        log_csl.save_log<TimeoutException>(ex, "CLSData", "ValorLecturasOffLine", SelectCommandText, "N4");
                        return null;
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        log_csl.save_log<Exception>(ex, "CLSData", "ValorLecturasOffLine",SelectCommandText, "N4");
                        return null;
                    }
                    catch (Exception ex)
                    {
                        log_csl.save_log<Exception>(ex, "CLSData", "ValorLecturasOffLine", SelectCommandText, "N4");
                        return null;
                    }
                    hsk = t.AsEnumerable().ToList();
                }
            }
            return hsk;
        }

    }
    public class zona
    {
        public int? idcorp { get; set; }
        public int? idempre { get; set; }
        public int? idzona { get; set; }
        public int? idservicio { get; set; }
        public string titulo { get; set; }
        public string icono { get; set; }
    }
    public class permisos
    {
        public int? idzona { get; set; }
        public int? idservicio { get; set; }
        public char[] opciones { get; set; }
    }
    public class opcion
    {
      
        public int? idservicio { get; set; }
        public int? idopcion { get; set; }
        public string descripcion { get; set; }
        public string icono { get; set; }
        public string textointro { get; set; }
        public string url { get; set; }
        public bool activa { get; set; }
    }

}