using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using csl_log;
using System.Data.SqlClient;
using System.IO;


namespace CSLSite.app_start
{
    public class ServicioEikon
    {
        IWebProxy proxy = null;
        public string Usuario { get; set; }
 
        private static SqlConnection conexionmidle()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }

        public bool Graba_Log(string xml, string opcion, string tipo, string usuario)
        {
            string mensaje = "";

            try
            {
                using (var conn = conexionmidle())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandTimeout = 8000;
                            comando.CommandText = "SAV_P_LOG_SW_CAS";
                            comando.Parameters.AddWithValue("@i_opcion", "EIKON_" + opcion);
                            comando.Parameters.AddWithValue("@i_tipo", tipo);
                            comando.Parameters.AddWithValue("@i_trace", xml);
                            comando.Parameters.AddWithValue("@i_usuario", usuario);
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
                            var t = log_csl.save_log<Exception>(ex, "ServicioEikon", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "ServicioEikon", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);

                return false;
            }
        }

        public ServicioEikon()
        {
            try
            {
                var user = new usuario();
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "Servicio Web eikon", "Tracker", "No disponible", "No disponible");
                    HttpContext.Current.Response.Redirect("../login.aspx", true);
                }
                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                Usuario = user.loginname;
            }
            catch
            {
                Usuario = string.Empty;
            }

        }

        private HttpWebRequest CrearPeticionJSON(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ProtocolVersion = HttpVersion.Version11;
            webRequest.ContentType = "application/json";
            webRequest.Accept = "text/xml";
            webRequest.Method = WebRequestMethods.Http.Post;
            //comentar
#if !DEBUG
            proxy = null;
#else
            /*proxy = new WebProxy("192.168.0.80:8080", false);
            proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");*/
            proxy = null;
#endif

            //****
            webRequest.Proxy = proxy;
            webRequest.Timeout = 6000000;

            return webRequest;
        }

        private static void InsertarSobreEnPeticionJSON(string jsonSerialObject, HttpWebRequest webRequest)
        {
            var postString = jsonSerialObject;


            var postData = Encoding.UTF8.GetBytes(postString);
            webRequest.ContentLength = postData.Length;
            using (Stream stream = webRequest.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
                stream.Close();
            }
        }

        public string HttpGetByWebRequ(string uri, string token)
        {

#if !DEBUG
            proxy = null;
           /*  proxy = new WebProxy("192.168.0.80:8080", false);
            proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");*/
#else
          /*  proxy = new WebProxy("192.168.0.80:8080", false);
            proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");*/
#endif

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Accept = "application/json; charset=utf-8";
            request.Proxy = proxy;
            request.Headers.Add("Authorization", "Bearer " + token);
            //request.Headers["Authorization"] = token;//"Basic " + authInfo;

            var response = (HttpWebResponse)request.GetResponse();

            string strResponse = "";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                strResponse = sr.ReadToEnd();

            }

            return strResponse;
        }

        public RespuestaCastEikon Peticion(string opcion, string container, string token, string bodega)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

            RespuestaCastEikon _exit = null;
            string jsonObject = string.Empty;
            
            var cfgs = dbconfig.GetActiveConfig(null, null, null);

            if (opcion == "TOKEN")
            {
                HttpWebRequest webRequest = null;
                var referencia = cfgs.Where(a => a.config_name.Contains("EikonPostLoginSAV")).FirstOrDefault();

                var v_user = cfgs.Where(a => a.config_name.Contains("EikonUser")).FirstOrDefault();
                var v_pass = cfgs.Where(a => a.config_name.Contains("EikonPass")).FirstOrDefault();

                if (bodega.Equals("SAV-DER")) 
                {
                     v_user = cfgs.Where(a => a.config_name.Contains("EikonUser")).FirstOrDefault();
                     v_pass = cfgs.Where(a => a.config_name.Contains("EikonPass")).FirstOrDefault();
                }

                if (bodega.Equals("ZAL/REPCONTVER"))
                {
                    v_user = cfgs.Where(a => a.config_name.Contains("EikonUser_Repcont")).FirstOrDefault();
                    v_pass = cfgs.Where(a => a.config_name.Contains("EikonPass_Repcont")).FirstOrDefault();
                }

                //v_user = cfgs.Where(a => a.config_name.Contains("EikonUser")).FirstOrDefault();
                //v_pass = cfgs.Where(a => a.config_name.Contains("EikonPass")).FirstOrDefault();

                webRequest = CrearPeticionJSON(referencia.config_value);

                DatosCredencialesEikon F = new DatosCredencialesEikon();

                F.email = v_user.config_value.ToString();
                F.password = v_pass.config_value.ToString();

                jsonObject = JsonConvert.SerializeObject(F);

                //GRABA LOG DE LLAMADO A SW (jsonObject)
                Graba_Log(jsonObject, opcion, "SOLICITUD", Usuario);

                InsertarSobreEnPeticionJSON(jsonObject, webRequest);
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                if (!asyncResult.AsyncWaitHandle.WaitOne())
                {
                    if (webRequest != null)
                    {
                        webRequest.Abort();
                        throw new TimeoutException("Se excedió el tiempo de espera permitido para esta petición");
                    }
                }

                StringBuilder peticionResult = new StringBuilder();
                try
                {
                    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                    {
                        if (webResponse != null)
                        {
                            StreamReader rd = new StreamReader(webResponse.GetResponseStream());
                            peticionResult.Append(rd.ReadToEnd());
                            rd.Close();
                            if (peticionResult.Length > 0)
                            {

                                ResultadoTokenEikon Token = JsonConvert.DeserializeObject<ResultadoTokenEikon>(peticionResult.ToString());

                                string v_result = peticionResult.ToString();

                                _exit = new RespuestaCastEikon { isCorrect = true, strResultado = v_result, strToken = Token };

                                //GRABA LOG DE RESPUESTA 
                                Graba_Log(_exit.strResultado, opcion, "RESPUESTA", Usuario);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _exit = new RespuestaCastEikon();
                    _exit.isCorrect = false;
                    _exit.strResultado = string.Format("Opcion : {0} - Error no controlado por sw ServicioEikon: {1} ", opcion, ex.Message);

                    var number = log_csl.save_log<Exception>(ex, "ServicioEikon", "Peticion() - Opcion=TOKEN", DateTime.Now.ToShortDateString(), Usuario);
                }
            }

            if (opcion == "VALIDA_CAST")
            {
                try
                {
                    var referencia = cfgs.Where(a => a.config_name.Contains("EikonGetCasDataSAV")).FirstOrDefault();
                    var jsonResponse = HttpGetByWebRequ(referencia.config_value + "" + container, token);

                    //GRABA LOG DE LLAMADO A SW (jsonObject)
                    Graba_Log(string.Format("Container={0} - Url={1} - Token={1} ", container, referencia.config_value + "" + container, token), opcion, "SOLICITUD", Usuario);

                    _exit = new RespuestaCastEikon();
                    var data = (JContainer)JsonConvert.DeserializeObject(jsonResponse);



                    if (data.HasValues)
                    {
                        JObject jObject = JObject.Parse(jsonResponse);
                        _exit = JsonConvert.DeserializeObject<RespuestaCastEikon>(jObject["data"].ToString());
                        _exit.strResultado = jsonResponse;
                    }
                    else
                    {
                        _exit.isCorrect = false;
                        _exit.strResultado = "Ocurrio un error en la llamada! :: " + data["Message"];
                    }

                    //GRABA LOG DE RESPUESTA A SW (_exit.strResultado)
                    Graba_Log(_exit.strResultado, opcion, "RESPUESTA", Usuario);
                }
                catch(Exception ex)
                {
                    _exit = new RespuestaCastEikon();
                    _exit.isCorrect = false;
                    _exit.strResultado = string.Format("Opcion : {0} - Error no controlado por sw EIKON: {1} el contenedor no existe..", opcion, ex.Message);
                    var number = log_csl.save_log<Exception>(ex, "ServicioEikon", "Peticion() - Opcion=VALIDA_CAST", DateTime.Now.ToShortDateString(), Usuario);
                }
            }

            return _exit;
        }

    }

    public class RespuestaCastEikon
    {
        public bool isCorrect { get; set; }
        public string strResultado { get; set; }
        public ResultadoTokenEikon strToken { get; set; }
        public int ecasnumero { get; set; }
        public string contenedor { get; set; }
        public DateTime ecasfechamaxsalida { get; set; }
        public DateTime ecasfechamaxdevolucion { get; set; }

        public string numeromanifiesto { get; set; }
        public string numerobl { get; set; }
        public string tipocontenedor { get; set; }
        public string ecasalmacentemporal { get; set; }
        public string ecaswarehousedevolucion { get; set; }
        public string ecascliente { get; set; }
        public string ecasclientename { get; set; }

        public RespuestaCastEikon()
        {
            this.isCorrect = true;
            this.strResultado = string.Empty;
            this.strToken = new ResultadoTokenEikon();
            this.ecasnumero = 0;
            this.contenedor = string.Empty;
            this.ecasfechamaxsalida = new DateTime(1900, 1, 1);
            this.ecasfechamaxdevolucion = new DateTime(1900, 1, 1);
            this.numeromanifiesto = string.Empty;
            this.numerobl = string.Empty;
            this.tipocontenedor = string.Empty;
            this.ecasalmacentemporal = string.Empty;
            this.ecaswarehousedevolucion = string.Empty;
            this.ecascliente = string.Empty;
            this.ecasclientename = string.Empty;
        }
    }

    public class DatosCredencialesEikon
    {
        public string email { get; set; }
        public string password { get; set; }
        
    }

    public class ResultadoTokenEikon
    {
        public string tokenType { get; set; }
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public string refreshToken { get; set; }
    }


    public class SolicitudEikon<T>
    {
        public SolicitudEikonD<T> COP;
    }

    public class SolicitudEikonD<T>
    {
        public T F;
    }

}