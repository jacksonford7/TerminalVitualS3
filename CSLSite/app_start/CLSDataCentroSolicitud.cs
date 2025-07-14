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
    //public enum tComando { Texto, Procedure }
    public enum tConexion { Master, Service, N4Catalog, Sca, Ecuapass }
    public class CLSDataCentroSolicitud
    {

        //metodo que devuelve la conexión nada mas
        private static SqlConnection getCon(tConexion conexion)
        {
            SqlConnection conex = new SqlConnection();
            switch(conexion){
                case tConexion.Master:
                    conex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalMaster"].ConnectionString);
                    break;
                case tConexion.N4Catalog:
                    conex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
                    break;
                case tConexion.Service:
                    conex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString);
                    break;
                case tConexion.Sca:
                    conex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"].ConnectionString);
                    break;
                case tConexion.Ecuapass:
                    conex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"].ConnectionString);
                    break;
            }
            return conex;
        }

        private static SqlConnection getConex()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString);                   
        }

        //metodo que devuelve la conexión nada mas
        private static SqlConnection getConexMaster()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalMaster"].ConnectionString);
        }

        //metodo que devuelve la conexión nada mas
        private static SqlConnection getConexN4()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
        }

        //ejecuta un sp, query, y RETORNA 1 SOLO VALOR ESCALAR!!!
        public static string ValorEscalar(string comandtext, Dictionary<string, string> parametros, tComando tipo, tConexion conexion)
        {
            string salida = null;
            try
            {
                using (var con = getCon(conexion))
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

        public static IEnumerable<IDataRecord> ValorLecturaCerrojoElectronico(string SelectCommandText, tComando tipo, tConexion conexion, DataTable dtDatos, string inbound)        
        {

            using (var conn = getCon(conexion))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = SelectCommandText;
                    cmd.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandTimeout = 60;
                    cmd.CommandText = SelectCommandText.Trim().ToUpper();
                    if (dtDatos != null)
                    {
                        cmd.Parameters.AddWithValue("@tablaContenedores", dtDatos);
                        cmd.Parameters.AddWithValue("@transitStateInboundC", inbound);
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
                        log_csl.save_log<Exception>(ex, "CLSData", "ValorLecturasOffLine", SelectCommandText, "N4");
                        return null;
                    }
                    catch (Exception ex)
                    {
                        log_csl.save_log<Exception>(ex, "CLSData", "ValorLecturasOffLine", SelectCommandText, "N4");
                        return null;
                    }
                    finally
                    {
                        t.Dispose();
                    }
                    hsk = t.AsEnumerable().ToList();
                }
            }
            return hsk;
        }

        //ejecuta un reader y retorna el yield
        public static IEnumerable<IDataRecord> ValorLectura(string SelectCommandText, tComando tipo, Dictionary<string, string> parametros = null)
        {
            using (var conn = getConexN4())
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

        public static bool addMail(out string number, string mailpara, string asunto, string htmlmensaje, string copiaspara, string usuario, string idlinea, string linea)
        {
            HashSet<SqlCommand> lista_c = new HashSet<SqlCommand>();
            try
            {
                number = string.Empty;

                using (var xcon = getConex())
                {
                    try
                    {
                        var comando = xcon.CreateCommand();
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.CommandText = "dbo.sp_insert_mail_log";
                        comando.Parameters.AddWithValue("@asunto", asunto);
                        comando.Parameters.AddWithValue("@htmlmsg", htmlmensaje);
                        comando.Parameters.AddWithValue("@mailpara", mailpara);
                        comando.Parameters.AddWithValue("@copiaspara", copiaspara);
                        comando.Parameters.AddWithValue("@usuario", usuario);
                        lista_c.Add(comando);
                        if (xcon.State != ConnectionState.Open)
                        {
                            xcon.Open();
                        }
                        using (var tx = xcon.BeginTransaction())
                        {
                            try
                            {
                                foreach (var c in lista_c)
                                {
                                    c.Transaction = tx;
                                    c.ExecuteNonQuery();
                                }
                                tx.Commit();
                            }
                            catch (SqlException ex)
                            {
                                tx.Rollback();
                                StringBuilder sb = new StringBuilder();
                                foreach (SqlError e in ex.Errors)
                                {
                                    sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                                }
                                var t = log_csl.save_log<SqlException>(ex, "CLSDataCentroSolicitud", "addMail", idlinea, linea);
                                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                                return false;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<SqlException>(ex, "CLSDataCentroSolicitud", "add", idlinea, linea);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;

                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "turnoConsolidacion", "add-gral", idlinea, linea);
                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                return false;
            }

        }


        //ejecuta un reader y retorna el yield, es necesario enviar que cadena de conexión se va a usar.
        public static IEnumerable<IDataRecord> ValorLecturaConexion(string SelectCommandText, tComando tipo, tConexion conexion, Dictionary<string, string> parametros = null)
        {
            using (var conn = getCon(conexion))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = SelectCommandText;
                    cmd.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandTimeout = 120;
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


        public static bool excuteOnqueryInOut(
            string comandtext, 
            Dictionary<string, string> inparametros,
            ref List<Tuple<string,string,int>> outparametros,
            tComando tipo, tConexion conexion)
        {
           
            try
            {
                using (var con = getCon(conexion))
                {
                    var com = con.CreateCommand();
                    com.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    com.CommandTimeout = 60;
                    com.CommandText = comandtext.Trim().ToUpper();
                    foreach (var item in inparametros)
                    {
                        com.Parameters.AddWithValue("@" + item.Key, item.Value).Direction = ParameterDirection.Input;
                    }
                    foreach (var item in outparametros)
                    {
                        //com.Parameters.AddWithValue("@" + item.Key, item.Value).Direction = ParameterDirection.Output;
                        SqlParameter par = new SqlParameter();
                        par.Direction = ParameterDirection.Output;
                        par.ParameterName = string.Format("@{0}",item.Item1);
                        par.Size = item.Item3;
                       

                    }
                    try
                    {
                        con.Open();
                        var objeto = com.ExecuteNonQuery();
                        con.Close();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        var errlis = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            errlis.AppendFormat("Linea:{0},Mensaje:{1},Numero:{2},Servidor:{3}", e.LineNumber, e.Message, e.Number, e.Server);
                        }
                        log_csl.save_log<ApplicationException>(new ApplicationException(errlis.ToString()), "CLSData", "excuteOnqueryInOut", comandtext, "Terminal Virtual");
                        return false;
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
                log_csl.save_log<Exception>(ex, "CLSData", "excuteOnqueryInOut", comandtext, "Terminal Virtual");
                return false;
            }
        }


        private SqlDbType getSQLtipo(System.Type tipo)
        {
            SqlParameter p1;
            System.ComponentModel.TypeConverter tc;
            p1 = new SqlParameter();
            tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType);

            if (tc.CanConvertFrom(tipo))
            {
                p1.DbType =(DbType) tc.ConvertFrom(tipo.Name);
            }
            else
            {
                try
                {
                    p1.DbType = (DbType) tc.ConvertFrom(tipo.Name);
                }
                catch 
                {
               
                }
   
            
            }
            return p1.SqlDbType;
          
        }

        
        public static bool crearIIE(Int64 ikey, string ibooking,string iuser, string idae, out string transaccionm)
        {

            if (ikey <= 0 || string.IsNullOrEmpty(ibooking) || string.IsNullOrEmpty(iuser) || string.IsNullOrEmpty(idae))
            {

                transaccionm = "No se puede iniciar la transacción no se encontró codigo de carga o booking,dae";
                return false;
            }

            try
            {
                using (var con = getCon(tConexion.Ecuapass))
                {
                    var com = con.CreateCommand();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 60;
                    com.CommandText = "pc_insert_iie_portal";

                    com.Parameters.AddWithValue("@ikey", ikey);
                    com.Parameters.AddWithValue("@ibooking", ibooking);
                    com.Parameters.AddWithValue("@iuser", iuser);
                    com.Parameters.AddWithValue("@idae", idae);
                    com.Parameters.AddWithValue("@isdebug", false);

                    SqlParameter transaccion = new SqlParameter();
                    transaccion.Direction = ParameterDirection.Output;
                    transaccion.ParameterName = "@transaccion";
                    transaccion.Size = 500;
                    transaccion.SqlDbType = SqlDbType.VarChar;
                    com.Parameters.Add(transaccion);

                    SqlParameter documento = new SqlParameter();
                    documento.Direction = ParameterDirection.Output;
                    documento.ParameterName = "@documento";
                    documento.Size = 50;
                    documento.SqlDbType = SqlDbType.VarChar;
                    com.Parameters.Add(documento);

                    SqlParameter estado = new SqlParameter();
                    estado.Direction = ParameterDirection.Output;
                    estado.ParameterName = "@estado";
                    estado.SqlDbType = SqlDbType.Bit;
                    com.Parameters.Add(estado);


                  
         
                    try
                    {
                        con.Open();
                        var objeto = com.ExecuteNonQuery();
                        con.Close();
                        if (estado.Value != null)
                        {
                            transaccionm = transaccion.Value as string;
                            return Boolean.Parse(estado.Value.ToString());
                        }
                        transaccionm = "Fue imposible realizar la transacción error de comunicación";
                        return false;
                    }
                    catch (SqlException ex)
                    {
                        var errlis = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            errlis.AppendFormat("Linea:{0},Mensaje:{1},Numero:{2},Servidor:{3}", e.LineNumber, e.Message, e.Number, e.Server);
                        }
                        transaccionm = "Hubo un problema de comunicación favor intente mas tarde";
                        log_csl.save_log<ApplicationException>(new ApplicationException(errlis.ToString()), "CLSData", "crearIIE", ikey.ToString(), "Terminal Virtual");
                        return false;
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                           
                        }
                        com.Dispose();
                        con.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "CLSData", "crearIIE", ikey.ToString(), "Terminal Virtual");
                transaccionm = "Hubo un problema de comunicación favor intente mas tarde";
                return false;
            }
        }

        
    }
}