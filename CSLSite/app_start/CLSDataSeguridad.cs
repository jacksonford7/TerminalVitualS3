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
    
    public class CLSDataSeguridad
    {
        /// <summary>
        /// Método que devuelve la conexión según la base a la cual quiera conectarse.
        /// </summary>
        /// <param name="conexion">Especifica la conexión la que quiere ser recuperada</param>
        /// <returns> La conexión</returns>
        private static SqlConnection getConex(tConexion conexion)
        {
            if (conexion == tConexion.Master)
            {
                return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalMaster"].ConnectionString);
            }
            else if (conexion == tConexion.Service)
            {
                return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString);
            }
            else if (conexion == tConexion.Sca)
            {
                return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"].ConnectionString);
            }
            else
            {
                return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
            }
        }

       /// <summary>
       /// Método que ejecutar un sp, el cual retorna un único valor 
       /// </summary>
       /// <param name="conexion">La base donde se quiere conectar</param>
       /// <param name="comandtext">Nombre del sp</param>
       /// <param name="parametros">Listado de parametros que debe recibir el sp</param>
       /// <param name="tipo">Tipo de sentencia que se ejecutará</param>
       /// <returns>Valor único que retorna el sp</returns>
        public static string ValorEscalar(tConexion conexion, string comandtext, Dictionary<string, string> parametros, tComando tipo)
        {
            string salida = null;
            try
            {
                using (var con = getConex(conexion))
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
                        log_csl.save_log<ApplicationException>(new ApplicationException(errlis.ToString()), "CLSDataSeguridad", "ValorEscalar", comandtext, "Portal_Master");
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
                log_csl.save_log<Exception>(ex, "CLSDataSeguridad", "ValorEscalar", comandtext, "Portal_Master");
                return salida;
            }
        }


        /// <summary>
        /// Método que ejecuta un sp que recibira en uno de sus parametros un conjunto de datos(data table), el cual retorna un único valor
        /// </summary>
        /// <param name="conexion">La base donde se quiere conectar</param>
        /// <param name="comandtext">Nombre del sp</param>
        /// <param name="parametros">Listado de parametros que debe recibir el sp</param>
        /// <param name="nombreParametroDatos">Nombre del parametro que recibirá el conjunto de datos</param>
        /// <param name="dtDatos">Conjunto de datos a enviar</param>
        /// <param name="tipo">Tipo de sentencia que se ejecutará</param>
        /// <returns>Valor único que retorna el sp</returns>
        public static string ValorEscalarConjunto(tConexion conexion, string comandtext, Dictionary<string, string> parametros, string nombreParametroDatos ,DataTable dtDatos,  tComando tipo)
        {
            string salida = null;
            try
            {
                using (var con = getConex(conexion))
                {
                    var com = con.CreateCommand();
                    com.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                    com.CommandTimeout = 60;
                    com.CommandText = comandtext.Trim().ToUpper();
                    foreach (var item in parametros)
                    {
                        com.Parameters.AddWithValue("@" + item.Key, item.Value);
                    }

                    if (dtDatos != null)
                    {
                        com.Parameters.AddWithValue("@" + nombreParametroDatos, dtDatos);
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
                        log_csl.save_log<ApplicationException>(new ApplicationException(errlis.ToString()), "CLSDataSeguridad", "ValorEscalar", comandtext, "Portal_Master");
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
                log_csl.save_log<Exception>(ex, "CLSDataSeguridad", "ValorEscalar", comandtext, "Portal_Master");
                return salida;
            }
        }

        /// <summary>
        /// Método que ejecutará un sp que retornará un conjunto de datos 
        /// </summary>
        /// <param name="conexion">La base donde se quiere conectar</param>
        /// <param name="SelectCommandText">Nombre del sp</param>
        /// <param name="tipo">Tipo de sentencia que se ejecutará</param>
        /// <param name="parametros">Listado de parametros que debe recibir el sp</param>
        /// <returns>Conjunto de datos </returns>
        public static IEnumerable<IDataRecord> ValorLecturas(tConexion conexion, string SelectCommandText, tComando tipo, Dictionary<string, string> parametros = null)
        {
            using (var conn = getConex(conexion))
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

        public static DataView LlenaCombos(tConexion conexion, string SelectCommandText, tComando tipo, Dictionary<string, string> parametros = null)
        {
            var conn = getConex(conexion);

            SqlConnection cn = new SqlConnection(conn.ConnectionString.ToString());

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = tipo == tComando.Procedure ? CommandType.StoredProcedure : CommandType.Text;
                coman.CommandText = SelectCommandText;
                coman.CommandTimeout = 0;
                coman.CommandText = SelectCommandText.Trim().ToUpper();
                if (parametros != null)
                {
                    foreach (var item in parametros)
                    {
                        coman.Parameters.AddWithValue("@" + item.Key, item.Value);
                    }
                }
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch
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

        /// <summary>
        /// Método genérico que retorna los catálogos según el nombre del catálogo enviado como parametros
        /// </summary>
        /// <param name="conexion">La base donde se quiere conectar</param>
        /// <param name="nombreCatalogo">Nombre del catálogo del cual quiere recuperar el detalle</param>
        /// <returns>Detalle del catálogo</returns>
        public static IEnumerable<Tuple<string, string>> returnDetalleCatalogosSeguridad(tConexion conexion, string nombreCatalogo)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "CS_ConsultaDetalleCat_tmp";
                com.Parameters.AddWithValue("@nombreCatalogo", nombreCatalogo);
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnEmpresas", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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


        /// <summary>
        /// Método genérico que retorna los catálogos según el nombre del catálogo enviado como parametros
        /// </summary>
        /// <param name="conexion">La base donde se quiere conectar</param>
        /// <param name="nombreCatalogo">Nombre del catálogo del cual quiere recuperar el detalle</param>
        /// <returns>Detalle del catálogo</returns>
        public static IEnumerable<Tuple<string, string>> returnDetalleCatalogosSeguridadApp(tConexion conexion, string nombreCatalogo)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "CS_ConsultaDetalleCat_tmp_app";
                com.Parameters.AddWithValue("@nombreCatalogo", nombreCatalogo);
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnEmpresas", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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

        /// <summary>
        /// Método que realiza el guardado de un registro en el repositorio de envío de mails en la base de datos
        /// </summary>
        /// <param name="number"></param>
        /// <param name="mailpara">Correo de destino</param>
        /// <param name="asunto">Asunto del mail</param>
        /// <param name="htmlmensaje">Cuerpo del mail</param>
        /// <param name="copiaspara">Copias de destino del correo</param>
        /// <param name="usuario">Usuario que envia el correo</param>
        /// <param name="idlinea">Id de la linea</param>
        /// <param name="linea">Nombre de la linea</param>
        /// <returns></returns>
        public static bool addMail(out string number, string mailpara, string asunto, string htmlmensaje, string copiaspara, string usuario, string idlinea, string linea)
        {
            HashSet<SqlCommand> lista_c = new HashSet<SqlCommand>();
            try
            {
                number = string.Empty;
                using (var xcon = getConex(tConexion.Service))
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
                                var t = log_csl.save_log<SqlException>(ex, "credenciales", "add-Trx", idlinea, linea);
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
                        var t = log_csl.save_log<SqlException>(ex, "credenciales", "add", idlinea, linea);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "credenciales", "add-gral", idlinea, linea);
                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                return false;
            }

        }


        /// <summary>
        /// Método que retorna el catálogo completo de paises
        /// </summary>
        /// <param name="conexion">La base a la que se quiere conectar</param>
        /// <returns>Catálogo de países</returns>
        public static IEnumerable<Tuple<string, string>> returnPaises(tConexion conexion)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_pais";
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnEmpresas", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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


        /// <summary>
        /// Método que retorna el catálogo de provincias.
        /// </summary>
        /// <param name="conexion">La base a la que se quiere conectar</param>
        /// <returns>Catálogo de las provincias</returns>
        public static IEnumerable<Tuple<string, string>> returnProvincias(tConexion conexion)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_provincias";
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnEmpresas", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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


        /// <summary>
        /// Método que retorna las ciudades según la provincia enviada como parámetro
        /// </summary>
        /// <param name="conexion">La base a la que se quiere conectar</param>
        /// <param name="idProvincia">Id de la provincia de la cual quiere recuperar sus ciudades</param>
        /// <returns>Catálogo de las ciudades</returns>
        public static IEnumerable<Tuple<string, string>> returnCiudades(tConexion conexion, int idProvincia)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_cantones";
                com.Parameters.AddWithValue("@provincia", idProvincia.ToString());
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnEmpresas", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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


        public static IEnumerable<Tuple<int, string>> returnTipoUsuarios(tConexion conexion)
        {
            var xsalida = new HashSet<Tuple<int, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create(0, "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "MA_TipoUsuarios";
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetInt32(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "returnTipoUsuarios", "null", "N4");
                    xsalida.Add(Tuple.Create(0, "Catalogo incompleto [set]"));
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

        public static IEnumerable<Tuple<int, string>> returnFiltros(tConexion conexion, int iduser)
        {
            var xsalida = new HashSet<Tuple<int, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create(0, "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "[TV].[sp_user_filtro_tmp]";
                com.Parameters.AddWithValue("@idusuario", iduser.ToString());
                
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetInt32(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "returnFiltros", "null", "N4");
                    xsalida.Add(Tuple.Create(0, "Catalogo incompleto [set]"));
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


        public static IEnumerable<Tuple<int, string>> returnFiltrosApp(tConexion conexion, int iduser)
        {
            var xsalida = new HashSet<Tuple<int, string>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create(0, "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "[TV].[sp_user_filtro_tmp_app]";
                com.Parameters.AddWithValue("@idusuario", iduser.ToString());

                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetInt32(0), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "returnFiltros", "null", "N4");
                    xsalida.Add(Tuple.Create(0, "Catalogo incompleto [set]"));
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

        public static IEnumerable<Tuple<int, int,string, int>> returnOpcionesDisppoibles(tConexion conexion, int iduser, int idFiltro)
        {
            var xsalida = new HashSet<Tuple<int, int, string, int>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create(0,0, "Catalogo vacío [get]",0));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "[TV].[sp_user_options_tmp]";
                com.Parameters.AddWithValue("@idusuario", iduser.ToString());
                com.Parameters.AddWithValue("@idfiltro", idFiltro.ToString());
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(6)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "returnFiltros", "null", "N4");
                    xsalida.Add(Tuple.Create(0,0, "Catalogo incompleto [set]",0));
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

        public static IEnumerable<Tuple<int, int, string, int>> returnOpcionesAsignadas(tConexion conexion, int iduser)
        {
            var xsalida = new HashSet<Tuple<int, int, string, int>>();
            var con = getConex(conexion);
            if (con == null)
            {
                xsalida.Add(Tuple.Create(0, 0, "Catalogo vacío [get]", 0));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "[TV].[sp_user_options_tv_access]";//"[TV].[sp_user_opciones_tmp]";
                com.Parameters.AddWithValue("@idusuario", iduser.ToString());

                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(6)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "returnFiltros", "null", "N4");
                    xsalida.Add(Tuple.Create(0, 0, "Catalogo incompleto [set]", 0));
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
    }


}