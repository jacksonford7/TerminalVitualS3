using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using csl_log;

namespace CSLSite.app_start
{

        public class EcuapassConector
        {
            string url = string.Empty;
            string action = string.Empty;
        //requestExportDespachoData
        //http://cdes.aduana.gob.ec/cdes_svr/SENAE_ExportDespachoService_EC

        public EcuapassConector(string _url, string _actionmethod)
            {
                this.url = _url;
                this.action = _actionmethod;
            }
            /// <summary>
            /// Para el timer añada una configuración llamada: wsMilisecTimer, que representa los milisegundos de espera.
            /// </summary>
            /// <param name="url"></param>
            /// <param name="action"></param>
            /// <returns></returns>
            private static HttpWebRequest CrearPeticion(string url, string action, long largo = 0)
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ProtocolVersion = HttpVersion.Version11;
                webRequest.Headers.Add("SOAPAction", action);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.Proxy = null;
                if (System.Configuration.ConfigurationManager.AppSettings["wsMilisecTimer"] != null)
                {
                    webRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["wsMilisecTimer"]);
                }
                return webRequest;
            }
            private static void InsertarSobreEnPeticion(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
            {
                var postString = soapEnvelopeXml.OuterXml;
                var postData = Encoding.UTF8.GetBytes(postString);
                webRequest.ContentLength = postData.Length;
                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                    stream.Close();
                }
            }
            public XDocument IniciarPeticion(XmlDocument SobreSoap)
            {
                HttpWebRequest webRequest = CrearPeticion(this.url, this.action, SobreSoap.OuterXml.Length);
                InsertarSobreEnPeticion(SobreSoap, webRequest);
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                if (!asyncResult.AsyncWaitHandle.WaitOne())
                {
                    if (webRequest != null)
                    {
                        webRequest.Abort();
                        throw new TimeoutException("Se excedió el tiempo de espera permitido para esta petición");
                    }
                }
                StringBuilder soapResult = new StringBuilder();
            try
            {
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    XDocument _exit = null;
                    if (webResponse != null)
                    {
                        StreamReader rd = new StreamReader(webResponse.GetResponseStream());
                        soapResult.Append(rd.ReadToEnd());
                        rd.Close();
                        if (soapResult.Length > 0)
                        {
                            _exit = XDocument.Parse(soapResult.ToString());
                        }
                    }
                    return _exit;
                }
            }
            catch (WebException ex)
            {
                var error = ExcepcionWebExterno(ex);
                log_csl.save_log<WebException>(ex, "EcuapassConector", "IniciarPeticion", SobreSoap.ToString(), error);
                throw new ApplicationException(error);

            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "EcuapassConector", "IniciarPeticion", SobreSoap.ToString(), "Error General");
                throw new ApplicationException(ex.Message);
            }

            }


        private static string ExcepcionWebExterno(WebException wex)
        {
            StringBuilder sb = new StringBuilder();
            if (wex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = ((HttpWebResponse)wex.Response);
                try
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    var responsetext = string.Empty;
                    try
                    {
                        responsetext = reader.ReadToEnd();
                        return responsetext;
                    }
                    catch(Exception ex)
                    {
                        log_csl.save_log<Exception>(ex, "EcuapassConector", "ExcepcionWebExterno", wex.Message, "Pidiendo Detalles EXC");
                        return null;
                    }
                }
                catch (WebException ex)
                {
                    log_csl.save_log<Exception>(ex, "EcuapassConector", "ExcepcionWebExterno", wex.Message, "Pidiendo Detalles WEB");
                    return null;
                }
            }
            else
            {
                return (wex.InnerException != null) ? wex.InnerException.Message : wex.Message;
            }
        }

        public static string ExploreEcuapassError(string senae_respuesta)
        {
            if (string.IsNullOrEmpty(senae_respuesta))
            {
                return "La cadena del Error era nulo o vacío";
            }
            try
            {
                XDocument soapex = XDocument.Parse(senae_respuesta);
                var ns = soapex.Root.Name.Namespace;
                var detail = soapex.Root.Descendants(ns + "Fault").Descendants("detail").FirstOrDefault();

                return (detail != null) ? detail.Value : "No se encontró detalles del error.";
            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "EcuapassConector", "ExploreEcuapassError", senae_respuesta, "Pidiendo Detalles EXC");
                return "La cadena no se pudo convertir a un error del senae XML";
            }

        }

        /* ~/ecuapass/publicKey/cert_aduana.cer */
    }
}
