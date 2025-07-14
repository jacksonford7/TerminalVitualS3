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
    public class ServicioMarGlobal
    {
        IWebProxy proxy = null;
        public string Usuario { get; set; }
 /*       
        public RespuestaCast CallAPI(string v_contenedor)
        {
            var cfgs = dbconfig.GetActiveConfig(null, null, null);
            RespuestaCast oReturn = new RespuestaCast();
            string strKeyApi = GetAPISecurityToken();

            if (strKeyApi == string.Empty)
            {
                return new RespuestaCast { isCorrect = false, strResultado = "Token - No Autorizado" };
            }

            var v_ref1 = cfgs.Where(a => a.config_name.Contains("BaseAddressSAV")).FirstOrDefault();
            var v_url1 = v_ref1.config_value;

            var v_ref = cfgs.Where(a => a.config_name.Contains("GeteCasDataSAV")).FirstOrDefault();
            var v_url = v_ref.config_value;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(v_url1);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strKeyApi);

                var response =  client.GetAsync(v_url + "?Container=" + v_contenedor);

                //GRABA LOG DE LLAMADO A SW (jsonObject)
                Graba_Log(string.Format("Contenedor={0} Url={1} Token={2}",v_contenedor,v_url, strKeyApi), "GeteCasDataSAV", "SOLICITUD", Usuario);

                if (response != null)
                {
                    var jsonResponse = response.Result.Content.ReadAsStringAsync().Result;

                    //GRABA LOG DE RESPUESTA A SW (jsonResponse)
                    Graba_Log(jsonResponse, "GeteCasDataSAV", "RESPUESTA", Usuario);

                    var data = (JContainer)JsonConvert.DeserializeObject(jsonResponse);
                    if (Convert.ToBoolean(data["Success"]))
                    {
                        JObject jObject = JObject.Parse(jsonResponse);
                        oReturn = JsonConvert.DeserializeObject<RespuestaCast>(jObject["Message"].ToString());

                        Console.WriteLine("Nro eCas:{0} - Contenedor: {1} - Fecha Emision: {2} - Fecha Caducidad: {3}",
                                         oReturn.eCasNumber, oReturn.Container, oReturn.DateIssue.ToShortDateString(),
                                         oReturn.DateExpiry.ToShortDateString());

                        oReturn.strResultado = jsonResponse;
                    }
                    else
                    {
                        oReturn.isCorrect = false;
                        oReturn.strResultado = "Ocurrio un error en la llamada! :: " + data["Message"];
                    }
                        
                }
            }
            return oReturn;
        }

        private string GetAPISecurityToken()
        {
            var cfgs = dbconfig.GetActiveConfig(null, null, null);
            string tokenJWT = String.Empty;
            HttpClient client = null;
            Task<HttpResponseMessage> response = null;
            
            var v_ref = cfgs.Where(a => a.config_name.Contains("GetTokenSAV")).FirstOrDefault();
            var v_url = v_ref.config_value;

            var v_ref1 = cfgs.Where(a => a.config_name.Contains("BaseAddressSAV")).FirstOrDefault();
            var v_url1 = v_ref1.config_value;

            var v_user = System.Configuration.ConfigurationManager.AppSettings["MARGLOBAL_KEY"];
            var v_pass = System.Configuration.ConfigurationManager.AppSettings["MARGLOBAL_USER_ID"];

            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(v_url1);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var formContent = new FormUrlEncodedContent(new[]
                                                           {
                                                             new KeyValuePair<string, string>("Username", v_user),
                                                             new KeyValuePair<string, string>("Password", v_pass),
                                                             new KeyValuePair<string, string>("Company", "0"),
                                                             new KeyValuePair<string, string>("Localidad","0"),
                                                            }
                                                          );

                response = client.PostAsync(v_url, formContent);
                
                //GRABA LOG DE LLAMADO A SW (jsonObject)
                Graba_Log(string.Format("Username={0} Password={1}",v_user,v_pass), "GetTokenSAV", "SOLICITUD", Usuario);

                if (response.Result.IsSuccessStatusCode)
                {
                    tokenJWT = response.Result.Content.ReadAsStringAsync().Result.ToString();
                    tokenJWT = tokenJWT.Replace("\"", "");

                    //GRABA LOG DE RESPUESTA A SW (_exit.strResultado)
                    Graba_Log(tokenJWT, "GetTokenSAV", "RESPUESTA", Usuario);
                }
                else if (response.Result.StatusCode == HttpStatusCode.Unauthorized)
                    tokenJWT= string.Empty;
            }
            catch (Exception ex)
            {
                tokenJWT = string.Empty;
                var t = log_csl.save_log<Exception>(ex, "ServicioMarGlobal", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
            }
            finally
            {
                client.Dispose();
                response.Dispose();
            }

            return tokenJWT;
        }
*/
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
                            comando.Parameters.AddWithValue("@i_opcion", "MARGLOBAL_" + opcion);
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
                            var t = log_csl.save_log<Exception>(ex, "ServicioMarGlobal", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "ServicioMarGlobal", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);

                return false;
            }
        }

        public ServicioMarGlobal()
        {
            try
            {
                var user = new usuario();
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "Servicio Web MarGlobal", "Tracker", "No disponible", "No disponible");
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

            request.Headers["Authorization"] = token;//"Basic " + authInfo;

            var response = (HttpWebResponse)request.GetResponse();

            string strResponse = "";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                strResponse = sr.ReadToEnd();

            }

            return strResponse;
        }

        public RespuestaCast Peticion(string opcion, string container, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

            RespuestaCast _exit = null;
            string jsonObject = string.Empty;
            
            var cfgs = dbconfig.GetActiveConfig(null, null, null);

            if (opcion == "TOKEN")
            {
                HttpWebRequest webRequest = null;
                var referencia = cfgs.Where(a => a.config_name.Contains("GetTokenSAV")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);

                var v_user = System.Configuration.ConfigurationManager.AppSettings["MARGLOBAL_KEY"];
                var v_pass = System.Configuration.ConfigurationManager.AppSettings["MARGLOBAL_USER_ID"];

                SolicitudMG<DatosCredencialesMarGlobal> COP = new SolicitudMG<DatosCredencialesMarGlobal>();
                SolicitudMGF<DatosCredencialesMarGlobal> COP_ = new SolicitudMGF<DatosCredencialesMarGlobal>();
                DatosCredencialesMarGlobal F = new DatosCredencialesMarGlobal();

                COP.COP = COP_;
                COP.COP.F = F;
                COP.COP.F.Username = v_user;
                COP.COP.F.Password = v_pass;
                COP.COP.F.Company = "0";
                COP.COP.F.Localidad = "ECUADOR";
                jsonObject = JsonConvert.SerializeObject(COP);

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
                                string v_result = peticionResult.ToString();
                                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                doc.LoadXml(v_result);
                                var resultadoXml = doc.InnerText;
                                _exit = new RespuestaCast { isCorrect = true, strResultado = resultadoXml };

                                //GRABA LOG DE RESPUESTA A SW (_exit.strResultado)
                                Graba_Log(_exit.strResultado, opcion, "RESPUESTA", Usuario);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _exit = new RespuestaCast();
                    _exit.isCorrect = false;
                    _exit.strResultado = string.Format("Opcion : {0} - Error no controlado por sw opacific: {1} ", opcion, ex.Message);

                    var number = log_csl.save_log<Exception>(ex, "ServicioMarGlobal", "Peticion() - Opcion=TOKEN", DateTime.Now.ToShortDateString(), Usuario);
                }
            }

            if (opcion == "VALIDA_CAST")
            {
                try
                {
                    var referencia = cfgs.Where(a => a.config_name.Contains("GeteCasDataSAV")).FirstOrDefault();
                    var jsonResponse = HttpGetByWebRequ(referencia.config_value + "?Container=" + container, token);

                    //GRABA LOG DE LLAMADO A SW (jsonObject)
                    Graba_Log(string.Format("Container={0} - Url={1} - Token={1} ", container, referencia.config_value + "?Container=" + container, token), opcion, "SOLICITUD", Usuario);

                    _exit = new RespuestaCast();
                    var data = (JContainer)JsonConvert.DeserializeObject(jsonResponse);
                    if (Convert.ToBoolean(data["Success"]))
                    {
                        JObject jObject = JObject.Parse(jsonResponse);
                        _exit = JsonConvert.DeserializeObject<RespuestaCast>(jObject["Message"].ToString());
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
                    _exit = new RespuestaCast();
                    _exit.isCorrect = false;
                    _exit.strResultado = string.Format("Opcion : {0} - Error no controlado por sw opacific: {1} ", opcion, ex.Message);
                    var number = log_csl.save_log<Exception>(ex, "ServicioMarGlobal", "Peticion() - Opcion=VALIDA_CAST", DateTime.Now.ToShortDateString(), Usuario);
                }
            }

            return _exit;
        }

    }

    public class RespuestaCast
    {
        public bool isCorrect { get; set; }
        public string strResultado { get; set; }
        public int eCasNumber { get; set; }
        public string Container { get; set; }
        public DateTime DateIssue { get; set; }
        public DateTime DateExpiry { get; set; }

        public string CustomsLineCode { get; set; }
        /// <summary>
        /// Constructor eCasReturnData
        /// </summary>
        public RespuestaCast()
        {
            this.isCorrect = true;
            this.strResultado = string.Empty;
            this.eCasNumber = 0;
            this.Container = String.Empty;
            this.DateIssue = new DateTime(1900, 1, 1);
            this.DateExpiry = new DateTime(1900, 1, 1);
        }
    }

    public class DatosCredencialesMarGlobal
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public string Localidad { get; set; }
    }

    public class SolicitudMG<T>
    {
        public SolicitudMGF<T> COP;
    }

    public class SolicitudMGF<T>
    {
        public T F;
    }

}