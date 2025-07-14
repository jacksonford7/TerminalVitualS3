using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CSLSite.app_start
{
    public class oFile
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string extension { get; set; }
        public string rutafisica { get; set; }
        public string rutavirtual { get; set; }
        public string creador { get; set; }
        public string token { get; set; }
        public bool activo { get; set; }
        DateTime? fechacreado { get; set; }
        DateTime? fecharecicle { get; set; }
        public bool recicle { get; set; }

        public bool guardar()
        {

            if (string.IsNullOrEmpty(this.nombre))
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Nombre es una cadena vacía"), 
                "oFile", "guardar", null, "Portal");
                return false;
            }

            if (string.IsNullOrEmpty(this.rutafisica))
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Rutafisica es una cadena vacía"),
                "oFile", "guardar", null, "Portal");
                return false;
            }
            if (string.IsNullOrEmpty(this.rutavirtual))
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("RutaVirtual es una cadena vacía"),
                "oFile", "guardar", null, "Portal");
                return false;
            }
            if (string.IsNullOrEmpty(this.creador))
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Creador es una cadena vacía"),
                "oFile", "guardar", null, "Portal");
                return false;
            }
            var cadena = "Data Source=cgwdb01;Initial Catalog=Portal_Sca;Persist Security Info=True;User ID=aisv;Password=Ictsi2012!AS;Connection Timeout=120";
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"] != null) ?
                    System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"].ConnectionString : cadena;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[Portal_Sca].[dbo].[pc_insertar_archivo]";
                comando.Parameters.AddWithValue("@nombre", nombre.Trim().ToLower());
                comando.Parameters.AddWithValue("@fisica", this.rutafisica.Trim().ToLower());
                comando.Parameters.AddWithValue("@virtual", this.rutavirtual.Trim().ToLower());
                comando.Parameters.AddWithValue("@creador", this.creador.Trim().ToLower());
                comando.Parameters.AddWithValue("@token", this.token.Trim().ToLower());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale == null || sale.GetType() == typeof(DBNull))
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Devolución fue nula"),"oFile", "guardar", null, "Portal");
                        return false;
                    }
                    else
                    {
                        int select = 0;
                        if (!int.TryParse(sale.ToString(), out select))
                        {
                            csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Conversión de numero no fue posible"),"oFile", "guardar", null, "Portal");
                            return false;
                        }
                        this.id = select;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "oFile", "guardar", null, "Portal");
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
        public static oFile getFile(int id, string token)
        {
            var file = new oFile();
            var cadena = "Data Source=cgwdb01;Initial Catalog=Portal_Sca;Persist Security Info=True;User ID=aisv;Password=Ictsi2012!AS;Connection Timeout=120";
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"] != null) ?
                    System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"].ConnectionString : cadena;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[Portal_Sca].[dbo].[pc_obtener_archivo]";
                comando.Parameters.AddWithValue("@id", id);
                comando.Parameters.AddWithValue("@token", token);
                try
                {
                    con.Open();
                    var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        file.id = reader.GetInt32(0);
                        file.nombre = reader.GetString(1);
                        file.extension = reader.GetString(2);
                        file.rutafisica = reader.GetString(3);
                        file.rutavirtual = reader.GetString(4);
                        file.creador = reader.GetString(5);
                        file.token = reader.GetString(6);
                        file.activo = reader.GetBoolean(7);
                        file.fechacreado = reader.GetDateTime(8);
                        file.fecharecicle = reader.GetDateTime(9) ;
                        file.recicle = reader.GetBoolean(10);
                    }
                    return file;
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "oFile", "getFile", null, "Portal");
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

        public static DataTable DinamycReport(string conID, string commandText, bool isProcedure, Dictionary<string, object> parameters)
        {

            DataTable oData = new DataTable();
            if (string.IsNullOrEmpty(conID))
            {
                oData.Columns.Add("ERROR", typeof(string));
                var oRow = oData.NewRow();
                oRow["ERROR"] = string.Format("Problema no específico TIPO: {0}", "1C");
                oData.Rows.Add(oRow);
                return oData;
            }
            if (string.IsNullOrEmpty(commandText))
            {
                oData.Columns.Add("ERROR", typeof(string));
                var oRow = oData.NewRow();
                oRow["ERROR"] = string.Format("Problema no específico TIPO: {0}", "2P");
                return oData;
            }
            if (parameters == null)
            {
                oData.Columns.Add("ERROR", typeof(string));
                var oRow = oData.NewRow();
                oRow["ERROR"] = string.Format("Problema no específico TIPO: {0}", "3N");
                oData.Rows.Add(oRow);
                return oData;
            }
            var cadena = System.Configuration.ConfigurationManager.ConnectionStrings[conID];
            if (cadena == null)
            {
                oData.Columns.Add("ERROR", typeof(string));
                var oRow = oData.NewRow();
                oRow["ERROR"] = string.Format("Problema no específico TIPO: {0}", "2CE");
                oData.Rows.Add(oRow);
                return oData;
            }

            using (var conn = new SqlConnection(cadena.ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText.Trim().ToUpper();
                    cmd.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandTimeout = 180;
                    if (parameters.Count > 0)
                    {
                        foreach (var p in parameters)
                        {
                            cmd.Parameters.AddWithValue(string.Format("@{0}", p.Key.Trim().ToUpper()), p.Value);
                        }
                    }
                    //controlar error
                    try
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (reader.HasRows)
                        {
                            oData.Load(reader);
                        }
                        else
                        {
                            oData.Columns.Add("ERROR", typeof(string));
                            var oRow = oData.NewRow();
                            oRow["ERROR"] = "La consulta no generó registros que presentar.";
                            oData.Rows.Add(oRow);
                        }
                        return oData;
                    }
                    catch (SqlException ex)
                    {
                        var sb = new StringBuilder();
                        foreach (SqlError eq in ex.Errors)
                        {
                            sb.AppendFormat("Message:{0}, Procedimiento:{1} , Linea:{2}", eq.Message, eq.Procedure, eq.LineNumber);
                        }
                       var code=  csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "oFile", "DinamycReport", conID, commandText);
                        oData.Columns.Add("ERROR", typeof(string));
                        var oRow = oData.NewRow();
                        oRow["ERROR"] = string.Format("Problema no específico #: {0}", code);
                        oData.Rows.Add(oRow);
                        return oData;
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        cmd.Dispose();
                    }

                }

            }
        }


    }

}