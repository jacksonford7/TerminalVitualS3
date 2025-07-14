using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AccesoDatos;
using Configuraciones;
using Respuesta;


namespace N4.Entidades
{
    [Serializable]
    [XmlRoot(ElementName = "Certificado")]
   public class Certificado:ModuloBase
    {

        [XmlAttribute(AttributeName = nameof(ruc_exportador))]
        public string ruc_exportador { get; set; }

        [XmlAttribute(AttributeName = nameof(email_exportador))]
        public string email_exportador { get; set; }
        [XmlAttribute(AttributeName = nameof(email_exportador1))]
        public string email_exportador1 { get; set; }
        [XmlAttribute(AttributeName = nameof(email_exportador2))]
        public string email_exportador2 { get; set; }
        [XmlAttribute(AttributeName = nameof(email_exportador3))]
        public string email_exportador3 { get; set; }
        [XmlAttribute(AttributeName = nameof(email_exportador4))]
        public string email_exportador4 { get; set; }
        [XmlAttribute(AttributeName = nameof(nombres_exportador))]
        public string nombres_exportador { get; set; }
        [XmlAttribute(AttributeName = nameof(aisv_numero))]
        public string aisv_numero { get; set; }
        [XmlAttribute(AttributeName = nameof(aisv_contenedor))]
        public string aisv_contenedor { get; set; }
        [XmlAttribute(AttributeName = nameof(aisv_proforma))]
        public string aisv_proforma { get; set; }
        [XmlAttribute(AttributeName = nameof(aisv_login))]
        public string aisv_login { get; set; }
        [XmlAttribute(AttributeName = nameof(aisv_referencia))]
        public string aisv_referencia { get; set; }
        [XmlAttribute(AttributeName = nameof(aisv_booking))]
        public string aisv_booking { get; set; }
        [XmlAttribute(AttributeName = nameof(aisv_producto))]
        public string aisv_producto { get; set; }
        [XmlAttribute(AttributeName = nameof(cert_tipo))]
        public string cert_tipo { get; set; }
        [XmlAttribute(AttributeName = nameof(cert_secuencia))]
        public string cert_secuencia { get; set; }
        [XmlAttribute(AttributeName = nameof(cert_numero))]
        public Int64? cert_numero { get; set; }
        [XmlAttribute(AttributeName = nameof(cert_proceso_estado))]
        public string cert_proceso_estado { get; set; }
        [XmlAttribute(AttributeName = nameof(cert_valido))]
        public bool cert_valido { get; set; }
        [XmlAttribute(AttributeName = nameof(cert_generado))]
        public DateTime? cert_generado { get; set; }
        [XmlAttribute(AttributeName = nameof(unidad_gkey))]
        public Int64? unidad_gkey { get; set; }
        [XmlAttribute(AttributeName = nameof(unidad_buque))]
        public string unidad_buque { get; set; }
        [XmlAttribute(AttributeName = nameof(unidad_viaje))]
        public string unidad_viaje { get; set; }
        [XmlAttribute(AttributeName = nameof(unidad_fecha_embarque))]
        public DateTime? unidad_fecha_embarque { get; set; }
        [XmlAttribute(AttributeName = nameof(unidad_fecha_ingreso))]
        public DateTime? unidad_fecha_ingreso { get; set; }

        [XmlAttribute(AttributeName = nameof(notas))]
        public string notas { get; set; }


        [XmlAttribute(AttributeName = nameof(id))]
        public Int64 id { get; set; }

        public Certificado() : base()
        {


        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "Certificado";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public static ResultadoOperacion<List<Certificado>> PendientesAISV(int maxregs)
        {
            var p = new Certificado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Certificado>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add(nameof(maxregs), maxregs);
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
            var rp = BDOpe.ComandoSelectALista<Certificado>(bcon, "[dbo].[aisv_carbono_certificado_pendientes]", p.Parametros);
            return rp.Exitoso?ResultadoOperacion<List<Certificado>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Certificado>>.CrearFalla(rp.MensajeProblema);
        }
        public static string CertificadosAlista(List<Certificado> lista)
        {
            if(lista==null || lista.Count<=0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<cntrs>");
            lista.ForEach(s => {
                sb.AppendFormat("<cntr id=\"{0}\" proforma=\"{1}\" aisv=\"{2}\" />",s.aisv_contenedor,s.aisv_proforma, s.aisv_numero);
            });
            sb.Append("</cntrs>");
            return sb.ToString();
        }
        public static ResultadoOperacion<List<Certificado>> PendientesN4(string unidadesXML)
        {
            var p = new Certificado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                ResultadoOperacion<List<Certificado>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("cntr_xml", unidadesXML);

            var rp = BDOpe.ComandoSelectALista<Certificado>(bcon, "[dbo].[aisv_carbono_certificado_pendientes]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Certificado>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Certificado>>.CrearFalla(rp.MensajeProblema);
        }





        public Respuesta.ResultadoOperacion<bool> Actualizar()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, "CGSA");
            if (this.id <=0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(id)));
            }

            if (string.IsNullOrEmpty(this.cert_secuencia))
            {
               return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1,nameof(cert_secuencia)));
            }
            if (!this.cert_numero.HasValue)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(cert_numero)));
            }
            if (!this.unidad_gkey.HasValue)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(unidad_gkey)));
            }
            if (string.IsNullOrEmpty(this.unidad_viaje))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(unidad_viaje)));
            }
            if (string.IsNullOrEmpty(this.unidad_buque))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(unidad_buque)));
            }

            if (!this.unidad_fecha_embarque.HasValue)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(unidad_fecha_embarque)));
            }
            if (!this.unidad_fecha_ingreso.HasValue)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(unidad_fecha_ingreso)));
            }

            this.Parametros.Clear();
            this.Parametros.Add(nameof(id),id);
            this.Parametros.Add(nameof(cert_secuencia), cert_secuencia);
            this.Parametros.Add(nameof(cert_numero), cert_numero);
           
            this.Parametros.Add(nameof(unidad_gkey), unidad_gkey);
            this.Parametros.Add(nameof(unidad_viaje), unidad_viaje);
            this.Parametros.Add(nameof(unidad_buque), unidad_buque);

            this.Parametros.Add(nameof(unidad_fecha_embarque), unidad_fecha_embarque);
            this.Parametros.Add(nameof(unidad_fecha_ingreso), unidad_fecha_ingreso);

            if (!string.IsNullOrEmpty(this.cert_proceso_estado))
            {
                this.Parametros.Add(nameof(cert_proceso_estado), cert_proceso_estado);
            }
            if (!string.IsNullOrEmpty(this.notas))
            {
                this.Parametros.Add(nameof(notas), notas);
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("Portal")?.valor;
            var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[dbo].[aisv_carbono_certificado_actualiza]", this.Parametros);
           
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);

            }
            this.cert_generado = DateTime.Now;
            this.cert_valido = true;
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(result.Resultado.HasValue?true:false);
        }


        public static ResultadoOperacion<Int64?> SiguienteCertificado()
        {
            var p = new Certificado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Int64?>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("Portal")?.valor;
            var rp = BDOpe.ComandoSelectEscalar<Int64>(bcon, "select next value for [dbo].[id_certificado]", p.Parametros);

            if (!rp.Exitoso)
            {
                p.LogError<ApplicationException>(new ApplicationException(rp.MensajeProblema), p.actualMetodo, "");
                return ResultadoOperacion<Int64?>.CrearFalla(rp.MensajeProblema);
            }
            return ResultadoOperacion<Int64?>.CrearResultadoExitoso(rp.Resultado);
        }


        public static Stream BarcodeStream(Int64 secuencia)
        {
            try
            {

                var p = new Certificado();
                p.actualMetodo = MethodBase.GetCurrentMethod().Name;
                string pv;
                
                if (!p.Accesorio.Inicializar(out pv))
                {
                    return null;
                }

                var ap_url = p.Accesorio.ObtenerConfiguracion("URL_SERVER");
                var ap_destino = p.Accesorio.ObtenerConfiguracion("DESTINO");
                if (ap_url == null || ap_destino == null)
                {
                    return null;
                }
                string url_destino = string.Format(ap_destino.valor, secuencia);
                string server_url = string.Format("http://{0}/barcode/handler/qr.ashx?code={1}&format=E9", ap_url.valor , url_destino);
                Stream stream = null;
                string url = server_url;
                System.Net.HttpWebRequest fileReq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse fileResp = (System.Net.HttpWebResponse)fileReq.GetResponse();
                if (fileReq.ContentLength > 0)
                { fileResp.ContentLength = fileReq.ContentLength; }
                stream = fileResp.GetResponseStream();
                return stream;
            }
            catch
            {
                return null;
            }
        }

        public static ResultadoOperacion<transportista> obtenerChofer(string idChofer)
        {
            var p = new transportista();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                ResultadoOperacion<transportista>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("i_idChofer", idChofer);

            var rp = BDOpe.ComandoSelectAEntidad<transportista>(bcon, "[dbo].[aisv_escaner_transportista]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<transportista>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<transportista>.CrearFalla(rp.MensajeProblema);
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "transportista")]
    public class transportista : ModuloBase
    {
        [XmlAttribute(AttributeName = nameof(CHOFER))]
        public string CHOFER { get; set; }

        [XmlAttribute(AttributeName = nameof(NOMBRE))]
        public string NOMBRE { get; set; }

        [XmlAttribute(AttributeName = nameof(EMPRESA))]
        public string EMPRESA { get; set; }

        [XmlAttribute(AttributeName = nameof(NAME_EMPRESA))]
        public string NAME_EMPRESA { get; set; }

        public override void OnInstanceCreate()
        {
            this.alterClase = "Certificado";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

    }
}
