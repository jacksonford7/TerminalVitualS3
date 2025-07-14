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
using System.Net.Http;

namespace CSLSite.app_start
{
    public class ServicioHamburg
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
                            comando.Parameters.AddWithValue("@i_opcion", "HAMBURGSUD_" + opcion);
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
                            var t = log_csl.save_log<Exception>(ex, "ServicioHamburg", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "ServicioHamburg", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);

                return false;
            }
        }

        public ServicioHamburg()
        {
            try
            {
                var user = new usuario();
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "Servicio Web HamburgSud", "Tracker", "No disponible", "No disponible");
                    HttpContext.Current.Response.Redirect("~/csl/login", true);
                }
                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                Usuario = user.loginname;
            }
            catch
            {
                Usuario = string.Empty;
            }

        }

     
        public string HttpGetByWebRequ(string uri, string EntityId,string User, string Pass, string ContainerNumber)
        {

#if !DEBUG
            proxy = null;
            //proxy = new WebProxy("192.168.0.80:8080", false);
            //proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
#else
            proxy = new WebProxy("192.168.0.80:8080", false);
            proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
#endif

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Accept = "application/json; charset=utf-8";
            request.Proxy = proxy;

            request.Headers["EntityId"] = EntityId;
            request.Headers["User"] = User;
            request.Headers["Pass"] = Pass;
            request.Headers["ContainerNumber"] = ContainerNumber;


            var response = (HttpWebResponse)request.GetResponse();

            string strResponse = "";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                strResponse = sr.ReadToEnd();

            }

            return strResponse;
        }

        public RespuestaHamburgCast<MessageHS> Peticion(string opcion, string container)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

            RespuestaHamburgCast<MessageHS> _exit = null;
            string jsonObject = string.Empty;

            var cfgs = dbconfig.GetActiveConfig(null, null, null);

            var v_entityId = System.Configuration.ConfigurationManager.AppSettings["HAMBURG_ENTITYID"];
            var v_user = System.Configuration.ConfigurationManager.AppSettings["HAMBURG_USER"];
            var v_pass = System.Configuration.ConfigurationManager.AppSettings["HAMBURG_PASS"];


            if (opcion == "VALIDA_CAST")
            {
                try
                {
                    var referencia = cfgs.Where(a => a.config_name.Contains("GetCasDataHSudSAV")).FirstOrDefault();
                    var jsonResponse = HttpGetByWebRequ(referencia.config_value, v_entityId, v_user, v_pass,container);

                    //GRABA LOG DE LLAMADO A SW (jsonObject)
                    Graba_Log(string.Format("Container={0} - Url={1} - Token={1} ", container, referencia.config_value + "?Container=" + container, v_entityId), opcion, "SOLICITUD", Usuario);

                    _exit = new RespuestaHamburgCast<MessageHS>();
                    var data = (JContainer)JsonConvert.DeserializeObject(jsonResponse);
                    if (Convert.ToBoolean(data["Success"]))
                    {
                        JObject jObject = JObject.Parse(jsonResponse);

                        MessageHS msgHS = new MessageHS();
                        msgHS = JsonConvert.DeserializeObject<MessageHS>(jObject["Message"].ToString());
                        _exit.Message = msgHS;
                        _exit.strResultado = jsonResponse;
                    }
                    else
                    {
                        var arraymsg = data["MessageException"].ToString();
                        _exit.Success = false;
                        _exit.MessageException = new string[] { arraymsg.Replace("[","").Replace("]","").ToString().TrimEnd().TrimStart().Trim()};//{ arraymsg[0].ToString(); arraymsg[1].ToString(); };
                        _exit.strResultado = "Ocurrio un error en la llamada! :: " + _exit.MessageException[0];
                    }

                    //GRABA LOG DE RESPUESTA A SW (_exit.strResultado)
                    Graba_Log(_exit.strResultado, opcion, "RESPUESTA", Usuario);
                }
                catch (Exception ex)
                {
                    _exit = new RespuestaHamburgCast<MessageHS>();
                    _exit.Success = false;
                    _exit.strResultado = string.Format("Opcion : {0} - Error no controlado por sw hamburgsud: {1} ", opcion, ex.Message);
                    var number = log_csl.save_log<Exception>(ex, "ServicioHamburg", "Peticion() - Opcion=VALIDA_CAST", DateTime.Now.ToShortDateString(), Usuario);
                }
            }

          

            return _exit;
        }
    }
    public class RespuestaHamburgCast<T>
    {
        public bool Success { get; set; }
        public string[] MessageException { get; set; }
        public T Message { get; set; }
        public string strResultado { get; set; }


        public RespuestaHamburgCast()
        {
            this.Success = true;
            this.MessageException = new string[] {""};
            //this.Message =  new MessageHS();
            this.strResultado = string.Empty;
        }
    }

    public class MessageHS
    {
        public int eCasNumber { get; set; }
        public string Container { get; set; }
        public DateTime DateIssue { get; set; }
        public DateTime DateExpiry { get; set; }

        public MessageHS()
        {
            this.eCasNumber = 0;
            this.Container = String.Empty;
            this.DateIssue = new DateTime(1900, 1, 1);
            this.DateExpiry = new DateTime(1900, 1, 1);
        }
    }

    public class DatosSolicitudHamburg
    {
        public string EntityId { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string ContainerNumber { get; set; }
    }
}