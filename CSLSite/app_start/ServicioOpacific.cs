using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ConectorN4;
using csl_log;

namespace CSLSite.app_start.MRSK
{
    public class ServicioOpacific
    {
        IWebProxy proxy = null ;

        public string Usuario { get; set; }

        public ServicioOpacific()
        {
            try
            {
                var user = new usuario();
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "Servicio Web Opacific", "Tracker", "No disponible", "No disponible");
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

        
        private  HttpWebRequest CrearPeticionJSON(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ProtocolVersion = HttpVersion.Version11;
            webRequest.ContentType = "application/json";
            webRequest.Accept = "text/xml";
            webRequest.Method = WebRequestMethods.Http.Post;
            //comentar
#if !DEBUG
            proxy = null;
            //proxy = new WebProxy("192.168.0.80:8080", false);
            //proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
#else
            proxy = new WebProxy("192.168.0.80:8080", false);
            proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
#endif

            //****
            webRequest.Proxy = proxy;
            webRequest.Timeout = 6000000;

            return webRequest;
        }

        private static void InsertarSobreEnPeticionJSON(string jsonSerialObject, HttpWebRequest webRequest)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // .NET 4.5

            // Remove insecure protocols (SSL3, TLS 1.0, TLS 1.1)
            //ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            //ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
            //ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
            //// Add TLS 1.2
            //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var postString = jsonSerialObject;

        
            var postData = Encoding.UTF8.GetBytes(postString);
            webRequest.ContentLength = postData.Length;
            using (Stream stream = webRequest.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
                stream.Close();
            }
        }

        public Respuesta Peticion(string opcion, Dictionary<string,string> objeto)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

            HttpWebRequest webRequest = null;
            string jsonObject = string.Empty;
            //string Usuario = string.Empty;

            var cfgs = dbconfig.GetActiveConfig(null, null, null);

            if (opcion == "VALIDAR_BOOKING")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("validaBooking")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);

                Solicitud <DatosSolicitudValidaStock> COP = new Solicitud<DatosSolicitudValidaStock>();
                SolicitudF<DatosSolicitudValidaStock> COP_ = new SolicitudF<DatosSolicitudValidaStock>();
                COP.COP = COP_;
                DatosSolicitudValidaStock F = new DatosSolicitudValidaStock();
                COP.COP.F = F;
                COP.COP.F.key = objeto["key"];
                COP.COP.F.usuario_id = objeto["usuario_id"];
                COP.COP.F.fecha = objeto["fecha"];
                //COP.COP.F.hora = objeto["hora"];
                COP.COP.F.booking = objeto["booking"];
                COP.COP.F.accion = objeto["accion"];
                //Usuario = objeto["usuario_id"].ToString().Trim();
                jsonObject = JsonConvert.SerializeObject(COP);
            }
            if (opcion == "CREAR_CITA")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("creaCita")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);
                Solicitud<DatosSolicitudGuardaCita> COP = new Solicitud<DatosSolicitudGuardaCita>();
                SolicitudF<DatosSolicitudGuardaCita> COP_ = new SolicitudF<DatosSolicitudGuardaCita>();
                COP.COP = COP_;
                DatosSolicitudGuardaCita F = new DatosSolicitudGuardaCita();
                COP.COP.F = F;
                COP.COP.F.key = objeto["key"];
                COP.COP.F.usuario_id = objeto["usuario_id"];
                COP.COP.F.fecha = objeto["fecha"];
                COP.COP.F.hora = objeto["hora"];
                COP.COP.F.transportista = objeto["transportista"];
                COP.COP.F.ruc = objeto["ruc"];
                COP.COP.F.booking = objeto["booking"];
                COP.COP.F.linea_naviera = objeto["linea_naviera"];
                COP.COP.F.cedula = objeto["cedula"];
                COP.COP.F.chofer = objeto["chofer"];
                COP.COP.F.placa = objeto["placa"];
                COP.COP.F.cita_contecon = objeto["cita_contecon"];
                COP.COP.F.accion = objeto["accion"];
                COP.COP.F.usuario_id = objeto["usuario_id"];
                //Usuario = objeto["usuario_id"].ToString().Trim();
                jsonObject = JsonConvert.SerializeObject(COP);
            }

            if (opcion == "CANCELA_CITA")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("cancelaCita")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);
                Solicitud <DatosSolicitudCancelaCita> COP = new Solicitud<DatosSolicitudCancelaCita>();
                SolicitudF<DatosSolicitudCancelaCita> COP_ = new SolicitudF<DatosSolicitudCancelaCita>();
                COP.COP = COP_;
                DatosSolicitudCancelaCita F = new DatosSolicitudCancelaCita();
                COP.COP.F = F;
                COP.COP.F.key = objeto["key"];
                COP.COP.F.cita_id = objeto["cita_id"];
                jsonObject = JsonConvert.SerializeObject(COP);
            }

            if (opcion == "ACTUALIZA_CITA")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("actualizaCita")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);
                Solicitud<DatosSolicitudActualizaCita> COP = new Solicitud<DatosSolicitudActualizaCita>();
                SolicitudF<DatosSolicitudActualizaCita> COP_ = new SolicitudF<DatosSolicitudActualizaCita>();
                COP.COP = COP_;
                DatosSolicitudActualizaCita F = new DatosSolicitudActualizaCita();
                COP.COP.F = F;
                COP.COP.F.key = objeto["key"];
                COP.COP.F.placa = objeto["placa"];
                COP.COP.F.cedula = objeto["cedula"];
                COP.COP.F.conductor = objeto["conductor"];
                COP.COP.F.cita_id = objeto["cita_id"];
                COP.COP.F.cita_contecon = objeto["cita_contecon"]; ;
                jsonObject = JsonConvert.SerializeObject(COP);
            }

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // .NET 4.5

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
            Respuesta _exit = null;
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
                            _exit = JsonConvert.DeserializeObject<Respuesta>(peticionResult.ToString());
                            _exit.strResultado = peticionResult.ToString();

                            //GRABA LOG DE RESPUESTA A SW (_exit.strResultado)
                            Graba_Log(_exit.strResultado, opcion,"RESPUESTA",  Usuario);

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _exit = new Respuesta();
                _exit.isCorrect = false;
                Result v_result = new Result(); _exit.result = v_result;
                _exit.result.estado = 0;
                _exit.result.mensaje = " Error no controlado por sw opacific: " + ex.Message;
                _exit.strResultado = " Error no controlado por sw opacific: " + ex.Message;
            }
            return _exit;
        }

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
                            comando.CommandText = "VBS_P_LOG_SW_DEPOT";
                            comando.Parameters.AddWithValue("@i_opcion", opcion);
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
                            var t = log_csl.save_log<Exception>(ex, "ServicioOpacific", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "ServicioOpacific", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                
                return false;
            }
        }
   
    }

    public class Respuesta
    {
        public bool isCorrect { get; set; }
        public string strResultado { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public int estado { get; set; }
        public string mensaje { get; set; }
    }

    public class Solicitud<T>
    {
        public SolicitudF<T> COP;
    }

    public class SolicitudF<T>
    {
        public T F;
    }

    public class DatosSolicitudValidaStock
    {
        public string key { get; set; }
        public string usuario_id { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string booking { get; set; }
        public string accion { get; set; }
    }

    public class DatosSolicitudGuardaCita
    {
        public string key { get; set; }
        public string usuario_id { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string transportista { get; set; }
        public string ruc { get; set; }
        public string booking { get; set; }
        public string linea_naviera { get; set; }
        public string cedula { get; set; }
        public string chofer { get; set; }
        public string placa { get; set; }
        public string cita_contecon { get; set; }
        public string accion { get; set; }
    }

    public class DatosSolicitudCancelaCita
    {
        public string key { get; set; }
        public string cita_id { get; set; }
    }

    public class DatosSolicitudActualizaCita
    {
        public string key { get; set; }
        public string placa { get; set; }
        public string cedula { get; set; }
        public string cita_id { get; set; }
        public string cita_contecon { get; set; }
        public string conductor { get; set; }
    }
}

//namespace CSLSite.app_start
//{
//    public class ServicioOpacific
//    {
//        IWebProxy proxy = null;

//        public string Usuario { get; set; }
//        public string Token { get; set; }

//        public ServicioOpacific()
//        {
//            try
//            {
//                var user = new usuario();
//                if (HttpContext.Current.Session["control"] == null)
//                {
//                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "Servicio Web Opacific", "Tracker", "No disponible", "No disponible");
//                    HttpContext.Current.Response.Redirect("../login.aspx", true);
//                }
//                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
//                Usuario = user.loginname;

//                //OBTIENE TOKEN
//                var respuesta = Peticion("LOGIN", null);

//                if (!respuesta.isCorrect)
//                {
//                    throw new Exception("SW OPACIFIC: " + respuesta.message);
//                }
//                else
//                {
//                    Token = respuesta.result.token;
//                }
//            }
//            catch
//            {
//                Usuario = string.Empty;
//                throw;
//            }
//        }

//        private HttpWebRequest CrearPeticionJSON(string url)
//        {
//            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
//            webRequest.ProtocolVersion = HttpVersion.Version11;
//            webRequest.ContentType = "application/json";
//            webRequest.Accept = "*/*";
//            webRequest.Method = WebRequestMethods.Http.Post;
//            if (!string.IsNullOrEmpty(Token))
//            {
//                webRequest.Headers.Add("Authorization", "Bearer " + Token);
//            }
//            //comentar
//#if !DEBUG
//            proxy = null;
//            //proxy = new WebProxy("192.168.0.80:8080", false);
//            //proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
//#else
//            proxy = new WebProxy("192.168.0.80:8080", false);
//            proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
//            proxy = null;
//#endif

//            //****
//            webRequest.Proxy = proxy;
//            webRequest.Timeout = 6000000;

//            return webRequest;
//        }

//        private static void InsertarSobreEnPeticionJSON(string jsonSerialObject, HttpWebRequest webRequest)
//        {
//            try
//            {
//                var postString = jsonSerialObject;
//                var postData = Encoding.UTF8.GetBytes(postString);
//                webRequest.ContentLength = postData.Length;
//                using (Stream stream = webRequest.GetRequestStream())
//                {
//                    stream.Write(postData, 0, postData.Length);
//                    stream.Close();
//                }
//            }catch(Exception ex)
//            {
//                var t = log_csl.save_log<Exception>(ex, "ServicioOpacific", "InsertarSobreEnPeticionJSON", DateTime.Now.ToShortDateString(), "sistema");
//                throw;
//            }
//        }

//        public Respuesta Peticion(string opcion, Dictionary<string, string> objeto)
//        {
//            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

//            HttpWebRequest webRequest = null;
//            string jsonObject = string.Empty;

//            var cfgs = dbconfig.GetActiveConfig(null, null, null);

//            if (opcion == "LOGIN")
//            {
//                var referencia = cfgs.Where(a => a.config_name.Contains("Login")).FirstOrDefault();
//                var ParmID = cfgs.Where(a => a.config_name.Contains("ParmID")).FirstOrDefault();
//                var ParmPassw = cfgs.Where(a => a.config_name.Contains("ParmPassw")).FirstOrDefault();
//                var ParmSystemId = cfgs.Where(a => a.config_name.Contains("ParmSystemId")).FirstOrDefault();
//                var ParmClientId = cfgs.Where(a => a.config_name.Contains("ParmClientId")).FirstOrDefault();
//                var ParmClientSecret = cfgs.Where(a => a.config_name.Contains("ParmClientSecret")).FirstOrDefault();

//                webRequest = CrearPeticionJSON(referencia.config_value);
                
//                SolicitudSEG<DatosLogin,DatosAutorizacion> SEG = new SolicitudSEG<DatosLogin, DatosAutorizacion>();
//                SolicitudF<DatosLogin> SEG_ = new SolicitudF<DatosLogin>();
//                SolicitudF<DatosAutorizacion> AUT_ = new SolicitudF<DatosAutorizacion>();
//                SEG.SEG = SEG_;
//                SEG.AUT = AUT_;
//                DatosLogin F = new DatosLogin();
//                SEG.SEG.F = F;
//                SEG.SEG.F.id = ParmID.config_value;
//                SEG.SEG.F.password = ParmPassw.config_value;
//                SEG.SEG.F.sistem_id = ParmSystemId.config_value;

//                DatosAutorizacion F_ = new DatosAutorizacion();
//                SEG.AUT.F = F_;
//                SEG.AUT.F.clientId = ParmClientId.config_value;
//                SEG.AUT.F.clientSecret = ParmClientSecret.config_value;
                
//                jsonObject = JsonConvert.SerializeObject(SEG);
//            }

//            if (opcion == "VALIDAR_BOOKING")
//            {
//                var referencia = cfgs.Where(a => a.config_name.Contains("validaBookingNew")).FirstOrDefault();
//                webRequest = CrearPeticionJSON(referencia.config_value);

//                Solicitud<DatosSolicitudValidaStock> COP = new Solicitud<DatosSolicitudValidaStock>();
//                SolicitudF<DatosSolicitudValidaStock> COP_ = new SolicitudF<DatosSolicitudValidaStock>();
//                COP.APS = COP_;
//                DatosSolicitudValidaStock F = new DatosSolicitudValidaStock();
//                COP.APS.F = F;
//                COP.APS.F.usuario_id = objeto["usuario_id"];
//                try { COP.APS.F.fecha = objeto["fecha"].ToString().Substring(6, 4) + objeto["fecha"].ToString().Substring(3, 2) + objeto["fecha"].ToString().Substring(0, 2); } catch { };
//                COP.APS.F.booking = objeto["booking"];
//                COP.APS.F.accion = objeto["accion"];
//                jsonObject = JsonConvert.SerializeObject(COP);
//            }
//            if (opcion == "CREAR_CITA")
//            {
//                var referencia = cfgs.Where(a => a.config_name.Contains("creaCitaNew")).FirstOrDefault();
//                webRequest = CrearPeticionJSON(referencia.config_value);
//                Solicitud<DatosSolicitudGuardaCita> COP = new Solicitud<DatosSolicitudGuardaCita>();
//                SolicitudF<DatosSolicitudGuardaCita> COP_ = new SolicitudF<DatosSolicitudGuardaCita>();
//                COP.APS = COP_;
//                DatosSolicitudGuardaCita F = new DatosSolicitudGuardaCita();
//                COP.APS.F = F;
//                COP.APS.F.usuario_id = objeto["usuario_id"];
//                try { COP.APS.F.fecha = objeto["fecha"].ToString().Substring(6, 4) + objeto["fecha"].ToString().Substring(3, 2) + objeto["fecha"].ToString().Substring(0, 2); } catch { };
//                COP.APS.F.hora = objeto["hora"];
//                COP.APS.F.transportista = objeto["transportista"];
//                COP.APS.F.ruc = objeto["ruc"];
//                COP.APS.F.booking = objeto["booking"];
//                COP.APS.F.linea_naviera = objeto["linea_naviera"];
//                COP.APS.F.cedula = objeto["cedula"];
//                COP.APS.F.chofer = objeto["chofer"];
//                COP.APS.F.placa = objeto["placa"];
//                COP.APS.F.cita_contecon = objeto["cita_contecon"];
//                COP.APS.F.accion = objeto["accion"];
//                COP.APS.F.usuario_id = objeto["usuario_id"];
//                jsonObject = JsonConvert.SerializeObject(COP);
//            }

//            if (opcion == "CANCELA_CITA")
//            {
//                var referencia = cfgs.Where(a => a.config_name.Contains("cancelaCitaNew")).FirstOrDefault();
//                webRequest = CrearPeticionJSON(referencia.config_value);
//                Solicitud<DatosSolicitudCancelaCita> COP = new Solicitud<DatosSolicitudCancelaCita>();
//                SolicitudF<DatosSolicitudCancelaCita> COP_ = new SolicitudF<DatosSolicitudCancelaCita>();
//                COP.APS = COP_;
//                DatosSolicitudCancelaCita F = new DatosSolicitudCancelaCita();
//                COP.APS.F = F;
//                COP.APS.F.cita_id = objeto["cita_id"];
//                jsonObject = JsonConvert.SerializeObject(COP);
//            }

//            if (opcion == "ACTUALIZA_CITA")
//            {
//                var referencia = cfgs.Where(a => a.config_name.Contains("actualizaCitaNew")).FirstOrDefault();
//                webRequest = CrearPeticionJSON(referencia.config_value);
//                Solicitud<DatosSolicitudActualizaCita> COP = new Solicitud<DatosSolicitudActualizaCita>();
//                SolicitudF<DatosSolicitudActualizaCita> COP_ = new SolicitudF<DatosSolicitudActualizaCita>();
//                COP.APS = COP_;
//                DatosSolicitudActualizaCita F = new DatosSolicitudActualizaCita();
//                COP.APS.F = F;
//               // COP.APS.F.key = objeto["key"];
//                COP.APS.F.placa = objeto["placa"];
//                COP.APS.F.cedula = objeto["cedula"];
//                COP.APS.F.conductor = objeto["conductor"];
//                COP.APS.F.cita_id = objeto["cita_id"];
//                COP.APS.F.cita_contecon = objeto["cita_contecon"];
//                jsonObject = JsonConvert.SerializeObject(COP);
//            }

//            //GRABA LOG DE LLAMADO A SW (jsonObject)
//            Graba_Log(jsonObject, opcion, "SOLICITUD", Usuario);

//            InsertarSobreEnPeticionJSON(jsonObject, webRequest);
//            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
//            if (!asyncResult.AsyncWaitHandle.WaitOne())
//            {
//                if (webRequest != null)
//                {
//                    webRequest.Abort();
//                    throw new TimeoutException("Se excedió el tiempo de espera permitido para esta petición");
//                }
//            }
//            StringBuilder peticionResult = new StringBuilder();
//            Respuesta _exit = null;
//            try
//            {
//                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
//                {
//                    if (webResponse != null)
//                    {
//                        StreamReader rd = new StreamReader(webResponse.GetResponseStream());
//                        peticionResult.Append(rd.ReadToEnd());
//                        rd.Close();
//                        if (peticionResult.Length > 0)
//                        {
//                            _exit = JsonConvert.DeserializeObject<Respuesta>(peticionResult.ToString());
//                            _exit.message = peticionResult.ToString();
                   
//                            //GRABA LOG DE RESPUESTA A SW (_exit.strResultado)
//                            Graba_Log(_exit.message, opcion, "RESPUESTA", Usuario);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _exit = new Respuesta();
//                _exit.isCorrect = false;
//                Result v_result = new Result(); _exit.result = v_result;
//                _exit.result.estado = 0;
//                _exit.result.mensaje = " Error no controlado por sw opacific: " + ex.Message;
//                _exit.message = " Error no controlado por sw opacific: " + ex.Message;
//            }
//            return _exit;
//        }

//        private static SqlConnection conexionmidle()
//        {
//            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
//        }
//        public bool Graba_Log(string xml, string opcion, string tipo, string usuario)
//        {
//            string mensaje = "";

//            try
//            {
//                using (var conn = conexionmidle())
//                {
//                    try
//                    {
//                        using (var comando = conn.CreateCommand())
//                        {
//                            comando.CommandType = CommandType.StoredProcedure;
//                            comando.CommandTimeout = 8000;
//                            comando.CommandText = "VBS_P_LOG_SW_DEPOT";
//                            comando.Parameters.AddWithValue("@i_opcion", opcion);
//                            comando.Parameters.AddWithValue("@i_tipo", tipo);
//                            comando.Parameters.AddWithValue("@i_trace", xml);
//                            comando.Parameters.AddWithValue("@i_usuario", usuario);
//                            conn.Open();
//                            var result = comando.ExecuteNonQuery().ToString();
//                            conn.Close();
//                            mensaje = result;

//                            return true;
//                        }
//                    }
//                    catch (SqlException ex)
//                    {
//                        if (ex.Number.ToString() == "50000" && ex.State.ToString() == "1")
//                        {
//                            mensaje = ex.Message.ToString();
//                        }
//                        else
//                        {
//                            StringBuilder sb = new StringBuilder();
//                            foreach (SqlError e in ex.Errors)
//                            {
//                                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
//                            }
//                            var t = log_csl.save_log<Exception>(ex, "ServicioOpacific", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
//                            mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
//                        }
//                        return false;
//                    }
//                    finally
//                    {
//                        if (conn.State == ConnectionState.Open)
//                        {
//                            conn.Close();
//                        }
//                        conn.Dispose();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                //todo loguear que pasó!
//                var t = log_csl.save_log<Exception>(ex, "ServicioOpacific", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
//                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);

//                return false;
//            }
//        }
//    }

//    public class Respuesta
//    {
//        public Result result { get; set; }
//        public bool isCorrect { get; set; }
//        public string message { get; set; }
//    }

//    public class RespuestaGen
//    {
//        public string[] result { get; set; }
//        public bool isCorrect { get; set; }
//        public string message { get; set; }
//    }


//    public class Result
//    {
//        public int estado { get; set; }
//        public string mensaje { get; set; }

//        public string company_document_value { get; set; }
//        public string user_id { get; set; }
//        public string company_name { get; set; }
//        public string key_change { get; set; }
//        public string name { get; set; }
//        public string father_last_name { get; set; }
//        public string id { get; set; }
//        public string mother_last_name { get; set; }
//        public string key_change_days { get; set; }
//        public string token { get; set; }
//    }

//    public class SolicitudSEG<T,E>
//    {
//        public SolicitudF<T> SEG;
//        public SolicitudF<E> AUT { get; set; }
//    }

//    public class Solicitud<T>
//    {
//        public SolicitudF<T> APS;
//    }

//    public class SolicitudF<T>
//    {
//        public T F;
//    }

//    public class DatosLogin
//    {
//        public string id { get; set; }
//        public string password { get; set; }
//        public string sistem_id { get; set; }
//    }

//    public class DatosAutorizacion
//    {
//        public string clientId { get; set; }
//        public string clientSecret { get; set; }
//    }

//    public class DatosSolicitudValidaStock
//    {
//        public string usuario_id { get; set; }
//        public string fecha { get; set; }
//        public string hora { get; set; }
//        public string transportista { get; set; }
//        public string ruc { get; set; }
//        public string booking { get; set; }
//        public string linea_naviera { get; set; }
//        public string cedula { get; set; }
//        public string chofer { get; set; }
//        public string placa { get; set; }
//        public string contenedor { get; set; }
//        public string accion { get; set; }
//        public string cita_contecon { get; set; }
//    }

//    public class DatosSolicitudGuardaCita
//    {
//        public string usuario_id { get; set; }
//        public string fecha { get; set; }
//        public string hora { get; set; }
//        public string transportista { get; set; }
//        public string ruc { get; set; }
//        public string booking { get; set; }
//        public string linea_naviera { get; set; }
//        public string cedula { get; set; }
//        public string chofer { get; set; }
//        public string placa { get; set; }
//        public string contenedor { get; set; }
//        public string accion { get; set; }
//        public string cita_contecon { get; set; }
//    }

//    public class DatosSolicitudCancelaCita
//    {
//        public string cita_id { get; set; }
//    }

//    public class DatosSolicitudActualizaCita
//    {
//        public string cita_id { get; set; }
//        public string cita_contecon { get; set; }
//        public string cedula { get; set; }
//        public string conductor { get; set; }
//        public string placa { get; set; }
//    }
//}


namespace CSLSite.app_start.RepContver
{
    public class ServicioCISE
    {
        IWebProxy proxy = null;

        public string Usuario { get; set; }
        public string Token { get; set; }

        public ServicioCISE()
        {
            try
            {
                var user = new usuario();
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "Servicio Web Opacific", "Tracker", "No disponible", "No disponible");
                    HttpContext.Current.Response.Redirect("../login.aspx", true);
                }
                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                Usuario = user.loginname;

                //OBTIENE TOKEN
                var respuesta = Peticion("LOGIN", null);

                if (!respuesta.isCorrect)
                {
                    throw new Exception("SW OPACIFIC: " + respuesta.message);
                }
                else
                {
                    Token = respuesta.result.token;
                }
            }
            catch
            {
                Usuario = string.Empty;
                throw;
            }
        }

        private HttpWebRequest CrearPeticionJSON(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ProtocolVersion = HttpVersion.Version11;
            webRequest.ContentType = "application/json";
            webRequest.Accept = "*/*";
            webRequest.Method = WebRequestMethods.Http.Post;
            if (!string.IsNullOrEmpty(Token))
            {
                //webRequest.Headers.Add("Authorization",  Token);
                webRequest.Headers["Authorization"] = Token;
            }
            //comentar
#if !DEBUG
            proxy = null;
            //proxy = new WebProxy("192.168.0.80:8080", false);
            //proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
#else
            //proxy = new WebProxy("192.168.0.80:8080", false);
            //proxy.Credentials = new NetworkCredential("jgusqui", "P@ssword", "CGSA");
            proxy = null;
#endif

            //****
            webRequest.Proxy = proxy;
            webRequest.Timeout = 6000000;

            return webRequest;
        }

        private static void InsertarSobreEnPeticionJSON(string jsonSerialObject, HttpWebRequest webRequest)
        {
            try
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
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "ServicioCISE", "InsertarSobreEnPeticionJSON", DateTime.Now.ToShortDateString(), "sistema");
                throw;
            }
        }

        public Respuesta Peticion(string opcion, Dictionary<string, string> objeto)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

            HttpWebRequest webRequest = null;
            string jsonObject = string.Empty;

            var cfgs = dbconfig.GetActiveConfig(null, null, null);

            if (opcion == "LOGIN")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("RCVLogin")).FirstOrDefault();
                var ParmID = cfgs.Where(a => a.config_name.Contains("RCVUser")).FirstOrDefault();
                var ParmPassw = cfgs.Where(a => a.config_name.Contains("RCVPassw")).FirstOrDefault();
                var ParmSystemId = cfgs.Where(a => a.config_name.Contains("RCVDepotId")).FirstOrDefault();

                webRequest = CrearPeticionJSON(referencia.config_value);

                DatosLogin SEG = new DatosLogin();
            
                SEG.username = ParmID.config_value;
                SEG.passwd = ParmPassw.config_value;
                SEG.id_deposito = ParmSystemId.config_value;

                jsonObject = JsonConvert.SerializeObject(SEG);
            }

           
            if (opcion == "CREAR_CITA")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("RCVcreaCita")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);
                DatosSolicitudGuardaCita COP = new DatosSolicitudGuardaCita();
               
                              
                COP.AV_ID_TIPO_ORDEN = objeto["AV_ID_TIPO_ORDEN"];
                COP.AN_ID_TURNO = objeto["AN_ID_TURNO"];
                try { COP.AD_FECHA_TURNO = objeto["AD_FECHA_TURNO"].ToString(); } catch { };//objeto["AD_FECHA_TURNO"].ToString().Substring(6, 4) + objeto["AD_FECHA_TURNO"].ToString().Substring(3, 2) + objeto["AD_FECHA_TURNO"].ToString().Substring(0, 2); } catch { };
                COP.AN_ID_NAVIERA = objeto["AN_ID_NAVIERA"];
                COP.AV_NAVIERA_DESCRIPCION = objeto["AV_NAVIERA_DESCRIPCION"];
                COP.AV_BOOKING = objeto["AV_BOOKING"];
                COP.AV_BL = objeto["AV_BL"];
                COP.AV_ID_TIPO_CTNER = objeto["AV_ID_TIPO_CTNER"];
                COP.AV_CONTENEDOR = objeto["AV_CONTENEDOR"];
                COP.AV_CLIENTE_RUC = objeto["AV_CLIENTE_RUC"];
                COP.AV_CLIENTE_NOMBRE = objeto["AV_CLIENTE_NOMBRE"];
                COP.AV_TRANSPORTISTA_RUC = objeto["AV_TRANSPORTISTA_RUC"];
                COP.AV_TRANSPORTISTA_DESCRIPCION = objeto["AV_TRANSPORTISTA_DESCRIPCION"];
                COP.AV_CHOFER_CEDULA = objeto["AV_CHOFER_CEDULA"];
                COP.AV_CHOFER_NOMBRE = objeto["AV_CHOFER_NOMBRE"];
                COP.AV_PLACAS = objeto["AV_PLACAS"];
                COP.AV_BUQUE = objeto["AV_BUQUE"];
                COP.AV_VIAJE = objeto["AV_VIAJE"];
                COP.AV_ESTADO = objeto["AV_ESTADO"];
                jsonObject = JsonConvert.SerializeObject(COP);
            }

            if (opcion == "CANCELA_CITA")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("RCVcancelaCita")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);
                DatosSolicitudCancelaCita COP = new DatosSolicitudCancelaCita();
                COP.AN_ID_TURNO_REFERENCIA = objeto["AN_ID_TURNO_REFERENCIA"];
                COP.AV_ESTADO = objeto["AV_ESTADO"];
                jsonObject = JsonConvert.SerializeObject(COP);
            }

            if (opcion == "ACTUALIZA_CITA")
            {
                var referencia = cfgs.Where(a => a.config_name.Contains("RCVactualizaCita")).FirstOrDefault();
                webRequest = CrearPeticionJSON(referencia.config_value);
                DatosSolicitudActualizaCita COP = new DatosSolicitudActualizaCita();
                COP.AN_ID_TURNO_REFERENCIA = objeto["AN_ID_TURNO_REFERENCIA"];
                COP.AV_ID_TIPO_ORDEN = objeto["AV_ID_TIPO_ORDEN"];
                COP.AN_ID_TURNO = objeto["AN_ID_TURNO"];
                COP.AD_FECHA_TURNO = objeto["AD_FECHA_TURNO"];
                COP.AN_ID_NAVIERA = objeto["AN_ID_NAVIERA"];
                COP.AV_NAVIERA_DESCRIPCION = objeto["AV_NAVIERA_DESCRIPCION"];
                COP.AV_BOOKING = objeto["AV_BOOKING"];
                COP.AV_BL = objeto["AV_BL"];
                COP.AV_ID_TIPO_CTNER = objeto["AV_ID_TIPO_CTNER"];
                COP.AV_CONTENEDOR = objeto["AV_CONTENEDOR"];
                COP.AV_CLIENTE_RUC = objeto["AV_CLIENTE_RUC"];
                COP.AV_CLIENTE_NOMBRE = objeto["AV_CLIENTE_NOMBRE"];
                COP.AV_TRANSPORTISTA_RUC = objeto["AV_TRANSPORTISTA_RUC"];
                COP.AV_TRANSPORTISTA_DESCRIPCION = objeto["AV_TRANSPORTISTA_DESCRIPCION"];
                COP.AV_CHOFER_CEDULA = objeto["AV_CHOFER_CEDULA"];
                COP.AV_CHOFER_NOMBRE = objeto["AV_CHOFER_NOMBRE"];
                COP.AV_PLACAS = objeto["AV_PLACAS"];
                COP.AV_BUQUE = objeto["AV_BUQUE"];
                COP.AV_VIAJE = objeto["AV_VIAJE"];
                jsonObject = JsonConvert.SerializeObject(COP);
            }

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
            Respuesta _exit = null;
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
                            _exit = JsonConvert.DeserializeObject<Respuesta>(peticionResult.ToString());
                            _exit.message = peticionResult.ToString();

                            //GRABA LOG DE RESPUESTA A SW (_exit.strResultado)
                            Graba_Log(_exit.message, opcion, "RESPUESTA", Usuario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _exit = new Respuesta();
                _exit.isCorrect = false;
                Result v_result = new Result(); _exit.result = v_result;
                _exit.result.estado = 0;
                _exit.result.mensaje = " Error no controlado por sw opacific: " + ex.Message;
                _exit.message = " Error no controlado por sw opacific: " + ex.Message;
            }
            return _exit;
        }

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
                            comando.CommandText = "VBS_P_LOG_SW_DEPOT";
                            comando.Parameters.AddWithValue("@i_opcion", opcion);
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
                            var t = log_csl.save_log<Exception>(ex, "ServicioCISE", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "ServicioCISE", "Graba_Log", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);

                return false;
            }
        }
    }

    public class Respuesta
    {
        public Result result { get; set; }
        public bool isCorrect { get; set; }
        public string message { get; set; }
    }

   

    public class Result
    {
        public int estado { get; set; }
        public string mensaje { get; set; }
        //public string company_document_value { get; set; }
        //public string user_id { get; set; }
        //public string company_name { get; set; }
        //public string key_change { get; set; }
        //public string name { get; set; }
        //public string father_last_name { get; set; }
        //public string id { get; set; }
        //public string mother_last_name { get; set; }
        //public string key_change_days { get; set; }


        public int code { get; set; }
        public string token { get; set; }
        public long id_turno_referencia { get; set; }
        
    }

    public class DatosLogin
    {
        public string username { get; set; }
        public string passwd { get; set; }
        public string id_deposito { get; set; }
    }

   
    public class DatosSolicitudGuardaCita
    {
        public string AV_ID_TIPO_ORDEN { get; set; }
        public string AN_ID_TURNO { get; set; }
        public string AD_FECHA_TURNO { get; set; }
        public string AN_ID_NAVIERA { get; set; }
        public string AV_NAVIERA_DESCRIPCION { get; set; }
        public string AV_BOOKING { get; set; }
        public string AV_BL { get; set; }
        public string AV_ID_TIPO_CTNER { get; set; }
        public string AV_CONTENEDOR { get; set; }
        public string AV_CLIENTE_RUC { get; set; }
        public string AV_CLIENTE_NOMBRE { get; set; }
        public string AV_TRANSPORTISTA_RUC { get; set; }
        public string AV_TRANSPORTISTA_DESCRIPCION { get; set; }
        public string AV_CHOFER_CEDULA { get; set; }
        public string AV_CHOFER_NOMBRE { get; set; }
        public string AV_PLACAS { get; set; }
        public string AV_BUQUE { get; set; }
        public string AV_VIAJE { get; set; }
        public string AV_ESTADO { get; set; }
    }

    public class DatosSolicitudCancelaCita
    {
        public string AN_ID_TURNO_REFERENCIA { get; set; }
        public string AV_ESTADO { get; set; }
    }

    public class DatosSolicitudActualizaCita
    {
        public string AN_ID_TURNO_REFERENCIA { get; set; }
        public string AV_ID_TIPO_ORDEN { get; set; }
        public string AN_ID_TURNO { get; set; }
        public string AD_FECHA_TURNO { get; set; }
        public string AN_ID_NAVIERA { get; set; }
        public string AV_NAVIERA_DESCRIPCION { get; set; }
        public string AV_BOOKING { get; set; }
        public string AV_BL { get; set; }
        public string AV_ID_TIPO_CTNER { get; set; }
        public string AV_CONTENEDOR { get; set; }
        public string AV_CLIENTE_RUC { get; set; }
        public string AV_CLIENTE_NOMBRE { get; set; }
        public string AV_TRANSPORTISTA_RUC { get; set; }
        public string AV_TRANSPORTISTA_DESCRIPCION { get; set; }
        public string AV_CHOFER_CEDULA { get; set; }
        public string AV_CHOFER_NOMBRE { get; set; }
        public string AV_PLACAS { get; set; }
        public string AV_BUQUE { get; set; }
        public string AV_VIAJE { get; set; }
    }
}
