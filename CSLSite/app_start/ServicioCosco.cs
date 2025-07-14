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
    public class ServicioCosco
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
                            comando.Parameters.AddWithValue("@i_opcion", "COSCO_" + opcion);
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
                            var t = log_csl.save_log<Exception>(ex, "ServicioCosco", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "ServicioCosco", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);

                return false;
            }
        }

        public ServicioCosco()
        {
            try
            {
                var user = new usuario();
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "Servicio Web Cosco", "Tracker", "No disponible", "No disponible");
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
           // webRequest.Accept = "text/xml";
            webRequest.Accept = "application/json; charset=utf-8";
            webRequest.Method = WebRequestMethods.Http.Post;
            //comentar
#if !DEBUG
            proxy = null;
#else
          
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

            request.Headers["Authorization"] = token;

            var response = (HttpWebResponse)request.GetResponse();

            string strResponse = "";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                strResponse = sr.ReadToEnd();

            }

            return strResponse;
        }

        public RespuestaCosco Peticion(string opcion, string container, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

            RespuestaCosco _exit = null;
            string jsonObject = string.Empty;
            
            var cfgs = dbconfig.GetActiveConfig(null, null, null);

            if (opcion == "TOKEN")
            {
                HttpWebRequest webRequest = null;
                

                var referencia = cfgs.Where(a => a.config_name.Contains("CoscoPostLoginSAV")).FirstOrDefault();
                var v_user = cfgs.Where(a => a.config_name.Contains("CoscoUser")).FirstOrDefault();
                var v_pass = cfgs.Where(a => a.config_name.Contains("CoscoPass")).FirstOrDefault();
                var v_company = cfgs.Where(a => a.config_name.Contains("CoscoCompany")).FirstOrDefault();
                var v_localidad = cfgs.Where(a => a.config_name.Contains("Localidad")).FirstOrDefault();

                webRequest = CrearPeticionJSON(referencia.config_value);

                SolicitudMGCosco<DatosCredencialesCosco> COP = new SolicitudMGCosco<DatosCredencialesCosco>();
                SolicitudMGFCosco<DatosCredencialesCosco> COP_ = new SolicitudMGFCosco<DatosCredencialesCosco>();
                DatosCredencialesCosco F = new DatosCredencialesCosco();

                COP.COP = COP_;
                COP.COP.F = F;
                COP.COP.F.Username = v_user.config_value.ToString();
                COP.COP.F.Password = v_pass.config_value.ToString();
                COP.COP.F.Company = v_company.config_value.ToString();
                COP.COP.F.Localidad = v_localidad.config_value.ToString();
                jsonObject = JsonConvert.SerializeObject(COP);


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
                                ResultadoTokenCosco Token = JsonConvert.DeserializeObject<ResultadoTokenCosco>(peticionResult.ToString());

                                string v_result = peticionResult.ToString();

                                //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                //doc.LoadXml(v_result);
                                //var resultadoXml = doc.InnerText;

                                _exit = new RespuestaCosco { isCorrect = true, strResultado = v_result, strToken = Token };

                                Graba_Log(_exit.strResultado, opcion, "RESPUESTA", Usuario);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _exit = new RespuestaCosco();
                    _exit.isCorrect = false;
                    _exit.strResultado = string.Format("Opcion : {0} - Error no controlado por sw COSCO: {1} ", opcion, ex.Message);

                    var number = log_csl.save_log<Exception>(ex, "ServicioCosco", "Peticion() - Opcion=TOKEN", DateTime.Now.ToShortDateString(), Usuario);
                }
            }

            if (opcion == "VALIDA_CAST")
            {
                try
                {
                  

                    var referencia = cfgs.Where(a => a.config_name.Contains("CoscoGetCasDataSAV")).FirstOrDefault();
                    var jsonResponse = HttpGetByWebRequ(referencia.config_value + "" + container, token);

                    Graba_Log(string.Format("Container={0} - Url={1} - Token={1} ", container, referencia.config_value + "" + container, token), opcion, "SOLICITUD", Usuario);
                   

                    _exit = new RespuestaCosco();
                    var data = (JContainer)JsonConvert.DeserializeObject(jsonResponse);
                    if (Convert.ToBoolean(data["Success"]))
                    {
                        JObject jObject = JObject.Parse(jsonResponse);
                        _exit = JsonConvert.DeserializeObject<RespuestaCosco>(jObject["Message"].ToString());
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
                    _exit = new RespuestaCosco();
                    _exit.isCorrect = false;
                    _exit.strResultado = string.Format("Opcion : {0} - Error no controlado por sw COSCO: {1} ", opcion, ex.Message);
                    var number = log_csl.save_log<Exception>(ex, "ServicioCosco", "Peticion() - Opcion=VALIDA_CAST", DateTime.Now.ToShortDateString(), Usuario);
                }
            }

            return _exit;
        }

    }

    public class RespuestaCosco
    {
        public bool isCorrect { get; set; }
        public string strResultado { get; set; }
        public int eCasNumber { get; set; }
        public string Container { get; set; }
        public string DateIssue { get; set; }
        public DateTime DateExpiry { get; set; }

        public ResultadoTokenCosco strToken { get; set; }
        /// <summary>
        /// Constructor eCasReturnData
        /// </summary>
        public RespuestaCosco()
        {
            this.isCorrect = true;
            this.strResultado = string.Empty;
            this.eCasNumber = 0;
            this.Container = String.Empty;
            this.DateIssue = "1900-01-01";
            this.DateExpiry = new DateTime(1900, 1, 1);
            this.strToken = new ResultadoTokenCosco();
        }
    }

    public class DatosCredencialesCosco
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public string Localidad { get; set; }
    }

    public class SolicitudMGCosco<T>
    {
        public SolicitudMGFCosco<T> COP;
    }

    public class SolicitudMGFCosco<T>
    {
        public T F;
    }

    public class ResultadoTokenCosco
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public string token { get; set; }
        public string status { get; set; }
        public string codigoalmacen { get; set; }
    }
}