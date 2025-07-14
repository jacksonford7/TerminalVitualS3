using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Configuraciones;
using Respuesta;
using AccesoDatos;
using System.Reflection;

namespace Aduanas.Entidades
{

    #region "Entidad"
    [Serializable]
    [XmlRoot(ElementName = "Manifiesto")]
    public  class Manifiesto:ModuloBase
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "func_code")]
        public string func_code { get; set; }

        [XmlAttribute(AttributeName = "procesado")]
        public string procesado { get; set; }

        [XmlAttribute(AttributeName = "total_gross")]
        public Decimal total_gross { get; set; }

        [XmlAttribute(AttributeName = "cant_abordo")]
        public int? cant_abordo { get; set; }

        [XmlAttribute(AttributeName = "equipos")]
        public int equipos { get; set; }

        [XmlAttribute(AttributeName = "bl_doc")]
        public string bl_doc { get; set; }

        [XmlAttribute(AttributeName = "importador")]
        public string importador { get; set; }

        [XmlAttribute(AttributeName = "importador_id")]
        public string importador_id { get; set; }

        [XmlAttribute(AttributeName = "fecha")]
        public DateTime? fecha { get; set; }

        [XmlArray("Manifiesto_items")]
        [XmlArrayItem("manifiesto_item", typeof(ManifiestoItem))]
        public List<ManifiestoItem> manifiesto_item { get; set; }

        public Manifiesto():base()
        {
            this.manifiesto_item = new List<ManifiestoItem>();
        }

        public override void OnInstanceCreate()
        {
            this.alterClase = "ADUANA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public static ResultadoOperacion<Manifiesto> ObtenerManifiesto(string usuario, string mrn,string msn, string hsn)
        {
            var p = new Manifiesto();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
               return ResultadoOperacion<Manifiesto>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            var bcon = p.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bcon);
#endif
            var rp = BDOpe.ComandoSelectAEntidad<Manifiesto>(bcon, "[Bill].[manifiesto_informacion]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<Manifiesto>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Manifiesto>.CrearFalla(rp.MensajeProblema,rp.MensajeInformacion);
        }

        public static ResultadoOperacion<int> ActualizarManifiestoCFS(string USUARIO, Int64 ID_MANIFIESTO,  string TIPO_CARGA, string DESCONSOLIDADOR_ID)
        {
            var p = new Manifiesto();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<int>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add(nameof(ID_MANIFIESTO), ID_MANIFIESTO);
            p.Parametros.Add(nameof(TIPO_CARGA), TIPO_CARGA);
            p.Parametros.Add(nameof(DESCONSOLIDADOR_ID), DESCONSOLIDADOR_ID);
            p.Parametros.Add(nameof(USUARIO), USUARIO);

            var bcon = p.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
#if DEBUG
            p.LogEvent(USUARIO, p.actualMetodo, bcon);
#endif
            var rp = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[actualizar_manifiesto_informacion]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<int>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<int>.CrearFalla(rp.MensajeProblema, rp.MensajeInformacion);
        }

    }
    #endregion


}
