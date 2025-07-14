using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Services.Protocols;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using N4;

namespace ConectorBilling
{
    /// <summary>
    /// Esta clase representa la abstracción del uso del servicio básico de N4, facilita el acceso.
    /// </summary>
    public class wsN4Billing : ArgobasicService
    {
        private string _b64Credentials;
        string _username = string.Empty;
        string _password = string.Empty;
        const string _scope = "ICT/ECU/GYE/CGSA";
        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            System.Net.HttpWebRequest httpRequest = (System.Net.HttpWebRequest)base.GetWebRequest(uri);
            httpRequest.Headers.Add("Authorization", "Basic " + _b64Credentials);
            return (System.Net.WebRequest)httpRequest;
        }
        /// <summary>
        /// Método contructor, úselo si desea que se apliquen las credenciales del archivo de configuración.
        /// </summary>
        public wsN4Billing()
        {
            _username = (System.Configuration.ConfigurationManager.AppSettings["userIdBilling"] != null) ? System.Configuration.ConfigurationManager.AppSettings["userIdBilling"] : "admin";
            _password = (System.Configuration.ConfigurationManager.AppSettings["passwBilling"] != null) ? System.Configuration.ConfigurationManager.AppSettings["passwBilling"] : "adminCGSA$";
            byte[] bCredentials = Encoding.ASCII.GetBytes(this._username + ":" + this._password);
            _b64Credentials = Convert.ToBase64String(bCredentials);
        }
        /// <summary>
        /// Método contructor, úselo si desea usar credenciales propias.
        /// </summary>
        /// <param name="inUsername">Usuario de N4</param>
        /// <param name="inPassword">Contraseña</param>
        public wsN4Billing(string inUsername, string inPassword)
        {
            byte[] bCredentials = Encoding.ASCII.GetBytes(inUsername + ":" + inPassword);
            _b64Credentials = Convert.ToBase64String(bCredentials);
        }
        /// <summary>
        /// Este método invoca al servicio y retorna la respuesta de manera simplificada.
        /// </summary>
        /// <param name="coord">Coordenadas de N4</param>
        /// <param name="xmlDoc">Parametros en formato de plantilla N4</param>
        /// <param name="message">por referencia, aquí se muestra el mensaje de error explicito</param>
        /// <returns>0,1,2,3 -> N4. -1 Error </returns>
        public int InvokeN4Service(ObjectSesion2 user, string xmlDoc, ref string message, string secuencia, 
            ref string estado_transaccion, ref string numero_transaccion, ref string XML_Error )
        {
            estado_transaccion = "ERROR";

            try
            {
                basicInvoke bp = new basicInvoke();
                bp.scopeCoordinateIds = _scope;
                XDocument xds = XDocument.Parse(xmlDoc);
                string Errores = string.Empty;
                XElement puntero;
                XDocument xdocarga = null;
                string Action = string.Empty;
                string Type = string.Empty;
                message = string.Empty;
                numero_transaccion = string.Empty;
                XML_Error = string.Empty;

                bp.xmlDoc = xmlDoc;
                int tim = 0;
                if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["timeout"], out tim))
                {
                    this.Timeout = (tim > 0) ? tim * 60000 : -1;
                }
                else
                {
                    this.Timeout = -1;
                }
                var resp = this.basicInvoke(bp).basicInvokeResponse1;
                if (resp != null)
                {
                    Action = xds.Root.Descendants("credit").Attributes("action").FirstOrDefault().Value;

                    if (Action != null)
                    {
                        if (Action == "CREATE")
                        {
                            Type = xds.Root.Descendants("credit").Attributes("create-type").FirstOrDefault().Value; 
                        }
                        
                    }
                    xds = XDocument.Parse(resp);
                    XML_Error = resp;

                    int severidad = (xds.Root.Attributes("status").FirstOrDefault() != null) ? int.Parse(xds.Root.Attributes("status").FirstOrDefault().Value) : -1;
                    if (severidad >= 0)
                    {
                        StringBuilder msgCad = new StringBuilder();
                        var msf = xds.Root.Descendants("messages").Descendants("message");
                        var estado = xds.Root.Descendants("credit-response").FirstOrDefault().FirstAttribute.Value;

                        /*en base al estado se procesara la transaccion*/
                        if (estado != null)
                        {
                            estado_transaccion = estado.ToString().ToUpper();
                        }
                        else { estado_transaccion = "ERROR"; }


                        StringBuilder keys = new StringBuilder();

                        int c = 0;
                        if (msf != null)
                        {
                            int Ac = 0;
                            foreach (var m in msf)
                            {
                                msgCad.Append(string.Format("{0}, {1}\n\r",
                                    c++,
                                    (m.Attributes("message-text").FirstOrDefault() != null) ? m.Attributes("message-text").FirstOrDefault().Value : string.Empty
                                    ));

                                var mid = m.Attributes("message-id").FirstOrDefault();
                                if (mid != null && mid.Value.Trim().Length > 4)
                                {
                                    keys.AppendFormat("{0},", mid.Value);
                                }
                                /*Agrega detalle de errores a mostrar */
                                if (estado_transaccion == "ERROR")
                                {
                                    var nodo = m;
                                    xdocarga = XDocument.Parse(nodo.ToString());
                                    puntero = xdocarga.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("message")).FirstOrDefault();
                                    Errores = Errores + string.Format("{0} \n\r", puntero?.Value);

                                }
                                else
                                {
                                    /*Quiere decir que todo esta OK y se saca el numero de draft o nota de credito*/
                                    if (Ac == 0) {

                                        string valor = string.Empty;
                                        var nodo = m;
                                        if (nodo != null)
                                        {
                                            xdocarga = XDocument.Parse(nodo.ToString());
                                            puntero = xdocarga.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("message")).FirstOrDefault();
                                            valor = puntero?.Value;

                                            char[] delimiterChars = { ':', ',' };
                                            string[] cArreglo = valor.Split(delimiterChars);

                                            if (Action == "CREATE" && Type == "FULL")
                                            {
                                                numero_transaccion = cArreglo[1];
                                            }
                                            if (Action == "CREATE" && Type == "PARTIAL")
                                            {
                                                numero_transaccion = cArreglo[1];
                                            }
                                            if (Action == "FINALIZE" && Type == string.Empty)
                                            {
                                                numero_transaccion = cArreglo[2];
                                            }

                                        }

                                        Ac++;
                                    }
                                                                                 
                                }
                            }
                            var gt = keys.ToString().Split(',');
                            if (severidad > 0 && gt.Length > 0)
                            {
                                var n = gt.Where(a => !string.IsNullOrEmpty(a) && !string.IsNullOrWhiteSpace(a)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(n))
                                {
                                        message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código A01-{0}", AsyncLogger<ApplicationException>(user, xmlDoc, resp, new ApplicationException(string.Format("{0}, secuencia[{1}]", gt[1], secuencia)), -1, secuencia));
                                        message = string.Format("{0}, {1}\n\r",message , Errores);
                                }
                                else
                                {
                                    message = "Se produjo un error desconocido para la aplicación por favor comuniquese con CGSA...No se genero la nota de  ";
                                }
                            }
                            AsyncLogger<Exception>(user, xmlDoc, resp, null, severidad, keys.ToString());

                            if (severidad == 0 && message == string.Empty)
                            {
                                message = resp;
                                
                            }

                        }
                        else
                        {
                            message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código A01-{0}", AsyncLogger<ApplicationException>(user, xmlDoc, resp, new ApplicationException(string.Format("No se encontró elemento mensaje, secuencia[{0}]", secuencia)), -1, secuencia));
                        }


                        return severidad;
                    }
                    else
                    {
                        message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código A01-{0}", AsyncLogger<ApplicationException>(user, xmlDoc, resp, new ApplicationException(string.Format("No se encontró elemento status, secuencia[{0}]", secuencia)), -1, secuencia));
                        return -1;
                    }
                }
                else
                {
                    message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código A01-{0}", AsyncLogger<ApplicationException>(user, xmlDoc, resp, new ApplicationException(string.Format("N4 responde con texto vacío, secuencia[{0}]", secuencia)), -1, secuencia));
                    return -1;
                }
            }
            catch (System.Xml.XmlException e)
            {
                message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código X00-{0}", AsyncLogger<System.Xml.XmlException>(user, xmlDoc, "<XmlException>No se conectó con n4, xml sin formato</XmlException>", e));
                return -1;
            }
            catch (SoapException e)
            {
                message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código S00-{0}", AsyncLogger<SoapException>(user, xmlDoc, "<soapException>Se intentó conectar pero N4 no estaba disponible</soapException>", e));
                return -1;
            }
            catch (TimeoutException e)
            {
                message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código T01-{0}", AsyncLogger<TimeoutException>(user, xmlDoc, "<TimeoutException>Se intentó conectar pero N4 tardó demasiado</TimeoutException>", e));
                return -1;
            }
            catch (WebException e)
            {
                message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código W00-{0}", AsyncLogger<WebException>(user, xmlDoc, "<WebException>Se intentó conectar pero N4 no estaba disponible</WebException>", e));
                return -1;
            }
            catch (Exception e)
            {
                message = string.Format("Se produjo un problema durante la transacción, por favor repórtelo con este código E00-{0}", AsyncLogger<Exception>(user, xmlDoc, "<Exception>El componente controló un error general no clasificado</Exception>", e));
                return -1;
            }
        }


 
        private static Int64 AsyncLogger<T>(ObjectSesion2 sesion, string xmlin, string xmlout, T exx = null, int status = -1, string msgkey = null) where T : Exception
        {
            try
            {
                initdataconf();
                Int64 result = 0;
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_insertN4_log";
                            comando.Parameters.AddWithValue("@clase", sesion.clase);
                            comando.Parameters.AddWithValue("@metodo", sesion.metodo);
                            comando.Parameters.AddWithValue("@usuario", sesion.usuario);
                            comando.Parameters.AddWithValue("@token", sesion.token);
                            comando.Parameters.AddWithValue("@transaccion", sesion.transaccion);
                            comando.Parameters.AddWithValue("@status", status);
                            comando.Parameters.AddWithValue("@msgdetail", exx != null ? exx.Message : "Solo control");
                            comando.Parameters.AddWithValue("@xmlPortal", xmlin);
                            comando.Parameters.AddWithValue("@xmlN4", xmlout);
                            if (!string.IsNullOrEmpty(msgkey))
                            {
                                comando.Parameters.AddWithValue("@msgkey", msgkey);
                            }
                            conexion.Open();
                            var d = comando.ExecuteScalar();
                            if (d != null && d != DBNull.Value)
                            {
                                result = Int64.Parse(d.ToString());
                            }
                            else
                            {
                                result = 0;
                            }
                            conexion.Close();
                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log<Exception>(new Exception(sb.ToString()), "n4WebService", "AsyncLogger", sesion.transaccion, sesion.usuario);
                        return -1;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                save_log<Exception>(ex, "n4WebService", "AsyncLogger", sesion.transaccion, sesion.usuario);
                return 0;
            }
        }
        private static string cadena = string.Empty;
        private static string filerute = string.Empty;
        private static string track = string.Empty;
        public static void save_log<T>(T error, string clase, string metodo, string input, string usuario) where T : System.Exception
        {
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;
            parOpts.TaskScheduler = TaskScheduler.Current;
            TaskCreationOptions tco = new TaskCreationOptions();
            tco = TaskCreationOptions.PreferFairness;
            Task task = null;
            task = Task.Factory.StartNew(() =>
            {
                try
                {
                    initfileconf();
                    using (var w = System.IO.File.AppendText(filerute))
                    {
                        string message00 = error.Message;
                        string message01 = error.InnerException != null ? error.InnerException.Message : string.Empty;
                        w.WriteLine(string.Format("{0}->{1},{2},{3},{4},{5},{6}", DateTime.Now, clase, metodo, input, usuario, message00, message01));
                    }
                }
                finally
                {
                    //adorno decorativo
                }
            }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);
            task.ContinueWith((a) => { a.Dispose(); });
        }
        //iniciar la cadena de configuración 
        private static void initdataconf()
        {
            if (cadena.Trim().Length <= 0)
            {
                if (System.Configuration.ConfigurationManager.ConnectionStrings["service"] != null)
                {
                    cadena = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                    return;
                }
                throw new InvalidOperationException("Error al inicializar conexión [D01]");
            }
        }
        //iniciar la ruta del archivo
        private static void initfileconf()
        {
            if (filerute.Trim().Length <= 0 && System.Configuration.ConfigurationManager.AppSettings["service"] != null)
            {
                filerute = string.Format("{0}\\csl_WS.txt", System.Configuration.ConfigurationManager.AppSettings["service"]);
                return;
            }

            filerute = string.Format("{0}_csl_WS.txt", DateTime.Now.ToString("ddMMyyyymmss"));
        }
        //metodo que traduce el mensaje de N4 a pantalla
        public static bool GetCSLMessage(string msgkey, int estatus, out string mensaje)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["service"] != null) ? System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString : cadena;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [csl_services].[dbo].[fx_get_mensaje](@msgkey,@estatus)";
                comando.Parameters.AddWithValue("@msgkey", msgkey.Trim().ToUpper().Replace(",", string.Empty));
                comando.Parameters.AddWithValue("@estatus", estatus);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        mensaje = sale.ToString();
                        return true;
                    }
                    else
                    {
                        mensaje = msgkey;
                        return false;
                    }
                }
                catch
                {
                    mensaje = string.Format("No se pudo crear la nota de credito razón: [{0}]", msgkey);
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
        public static bool InsertAtMail(string htmlmsg, string para, string usuario, int modulo)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                string cadena = @"Data Source=cgwdb01;Initial Catalog=csl_services;uid=aisv;pwd=Ictsi2012!AS;Connection Timeout=30";
                con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["service"] != null) ? System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString : cadena;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[dbo].[sp_insert_mail_log]";
                comando.Parameters.AddWithValue("@htmlmsg", htmlmsg.Trim());
                comando.Parameters.AddWithValue("@mailpara", para.Trim());
                comando.Parameters.AddWithValue("@copiaspara", "");
                comando.Parameters.AddWithValue("@usuario", usuario);
                comando.Parameters.AddWithValue("@moduloID", 4);
                try
                {
                    con.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
                catch
                {
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
        public static Int64 save_log_WS<T>(T error, string clase, string metodo, string input, string usuario = null) where T : System.Exception
        {
            try
            {
                initdataconf();
                Int64 result = 0;
                //coexion
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_insert_log";
                            comando.Parameters.AddWithValue("@clase", clase);
                            comando.Parameters.AddWithValue("@metodo", metodo);
                            comando.Parameters.AddWithValue("@usuario", usuario != null ? usuario : "NoUserReport");
                            comando.Parameters.AddWithValue("@inputdata", input);
                            comando.Parameters.AddWithValue("@mensaje00", string.Format("Target:{0},Message:{1}", error.TargetSite, error.Message));
                            comando.Parameters.AddWithValue("@mensaje01", string.Format("Pila:{0},Message:{1}", error.StackTrace, error.InnerException != null ? error.InnerException.Message : DBNull.Value.ToString()));
                            conexion.Open();
                            result = Int64.Parse(comando.ExecuteScalar().ToString());
                            conexion.Close();
                            return result;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        save_log_WS_F<Exception>(new Exception(sb.ToString()), "log_csl_ws", "save_log", input, usuario);
                        return 0;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                save_log_WS_F<Exception>(ex, "log_csl_WS", "save_log", input, usuario, true);
                return 0;
            }
        }
        public static void save_log_WS_F<T>(T error, string clase, string metodo, string input, string usuario = null, bool file = false) where T : System.Exception
        {
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;
            parOpts.TaskScheduler = TaskScheduler.Current;
            TaskCreationOptions tco = new TaskCreationOptions();
            tco = TaskCreationOptions.PreferFairness;
            Task task = null;
            task = Task.Factory.StartNew(() =>
            {
                try
                {
                    initfileconf();
                    using (var w = System.IO.File.AppendText(filerute))
                    {
                        string message00 = error.Message;
                        string message01 = error.InnerException != null ? error.InnerException.Message : string.Empty;
                        w.WriteLine(string.Format("{0}->{1},{2},{3},{4},{5},{6}", DateTime.Now, clase, metodo, input, usuario, message00, message01));
                    }
                }
                finally
                {
                    //adorno decorativo
                }
            }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);
            task.ContinueWith((a) => { a.Dispose(); });
        }
    }
    public class ObjectSesion2
    {
        public string usuario { get; set; }
        public string token { get; set; }
        public string transaccion { get; set; }
        public string metodo { get; set; }
        public string clase { get; set; }
    }



}
