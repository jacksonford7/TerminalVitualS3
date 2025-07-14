using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Configuraciones;
using AccesoDatos;
using Respuesta;
using System.Reflection;
using System.Xml.Linq;

namespace N4.Importacion
{
    [Serializable]
    [XmlRoot(ElementName = "container_brbk")]
    public class container_brbk : ModuloBase
    {

        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public container_brbk() : base()
        {

        }
        #region "Entidad"

        [XmlAttribute(AttributeName = "CNTR_CONSECUTIVO")]
        public Int64 CNTR_CONSECUTIVO { get; set; }

        [XmlAttribute(AttributeName = "CNTR_CONTAINER")]
        public string CNTR_CONTAINER { get; set; }

        [XmlAttribute(AttributeName = "CNTR_TYPE")]
        public string CNTR_TYPE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_TYSZ_SIZE")]
        public string CNTR_TYSZ_SIZE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_TYSZ_ISO")]
        public string CNTR_TYSZ_ISO { get; set; }

        [XmlAttribute(AttributeName = "CNTR_TYSZ_TYPE")]
        public string CNTR_TYSZ_TYPE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_FULL_EMPTY_CODE")]
        public string CNTR_FULL_EMPTY_CODE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_YARD_STATUS")]
        public string CNTR_YARD_STATUS { get; set; }

        [XmlAttribute(AttributeName = "CNTR_TEMPERATURE")]
        public double? CNTR_TEMPERATURE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_TYPE_DOCUMENT")]
        public string CNTR_TYPE_DOCUMENT { get; set; }

        [XmlAttribute(AttributeName = "CNTR_DOCUMENT")]
        public string CNTR_DOCUMENT { get; set; }

        [XmlAttribute(AttributeName = "CNTR_VEPR_REFERENCE")]
        public string CNTR_VEPR_REFERENCE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_CLNT_CUSTOMER_LINE")]
        public string CNTR_CLNT_CUSTOMER_LINE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_LCL_FCL")]
        public string CNTR_LCL_FCL { get; set; }
        //aqui cambie//

        [XmlAttribute(AttributeName = "CNTR_CATY_CARGO_TYPE")]
        public string CNTR_CATY_CARGO_TYPE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_FREIGHT_KIND")]
        public string CNTR_FREIGHT_KIND { get; set; }

        [XmlAttribute(AttributeName = "CNTR_DD")]
        public bool? CNTR_DD { get; set; }

        [XmlAttribute(AttributeName = "FECHA_CAS")]
        public DateTime? FECHA_CAS { get; set; }

        [XmlAttribute(AttributeName = "CNTR_AISV")]
        public string CNTR_AISV { get; set; }

        [XmlAttribute(AttributeName = "CNTR_HOLD")]
        public Int32? CNTR_HOLD { get; set; }

        [XmlAttribute(AttributeName = "CNTR_REEFER_CONT")]
        public string CNTR_REEFER_CONT { get; set; }

        [XmlAttribute(AttributeName = "CNTR_VEPR_VSSL_NAME")]
        public string CNTR_VEPR_VSSL_NAME { get; set; }

        [XmlAttribute(AttributeName = "CNTR_VEPR_VOYAGE")]
        public string CNTR_VEPR_VOYAGE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_VEPR_ACTUAL_ARRIVAL")]
        public DateTime? CNTR_VEPR_ACTUAL_ARRIVAL { get; set; }

        [XmlAttribute(AttributeName = "CNTR_DESCARGA")]
        public DateTime CNTR_DESCARGA { get; set; }

        [XmlAttribute(AttributeName = "CNTR_VEPR_ACTUAL_DEPARTED")]
        public DateTime CNTR_VEPR_ACTUAL_DEPARTED { get; set; }

        [XmlAttribute(AttributeName = "CNTR_CANTIDAD")]
        public double? CNTR_CANTIDAD { get; set; }

        [XmlAttribute(AttributeName = "CNTR_PESO")]
        public decimal? CNTR_PESO { get; set; }


        [XmlAttribute(AttributeName = "CNTR_MANIOBRA")]
        public string CNTR_MANIOBRA { get; set; }

        [XmlAttribute(AttributeName = "CNTR_OPERACION")]
        public string CNTR_OPERACION { get; set; }

        [XmlAttribute(AttributeName = "CNTR_DESCRIPCION")]
        public string CNTR_DESCRIPCION { get; set; }

        [XmlAttribute(AttributeName = "CNTR_CONSIGNATARIO")]
        public string CNTR_CONSIGNATARIO { get; set; }

        [XmlAttribute(AttributeName = "CNTR_EXPORTADOR")]
        public string CNTR_EXPORTADOR { get; set; }

        [XmlAttribute(AttributeName = "CNTR_AGENCIA")]
        public string CNTR_AGENCIA { get; set; }

        [XmlAttribute(AttributeName = "CNTR_MRN")]
        public string CNTR_MRN { get; set; }

        [XmlAttribute(AttributeName = "CNTR_MSN")]
        public string CNTR_MSN { get; set; }

         [XmlAttribute(AttributeName = "CNTR_HSN")]
        public string CNTR_HSN { get; set; }


        [XmlAttribute(AttributeName = "CNTR_POSITION")]
        public string CNTR_POSITION { get; set; }

        [XmlAttribute(AttributeName = "BILL_ITEM")]
        public Int64? BILL_ITEM { get; set; }


        [XmlAttribute(AttributeName = "ID_UNIDAD")]
        public Int64? ID_UNIDAD { get; set; }

        [XmlAttribute(AttributeName = "UNIDAD_GKEY")]
        public Int64? UNIDAD_GKEY { get; set; }

        [XmlAttribute(AttributeName = "CNTR_BODEGA")]
        public string CNTR_BODEGA { get; set; }

        [XmlAttribute(AttributeName = "TIME_IN")]
        public DateTime? TIME_IN { get; set; }

        [XmlAttribute(AttributeName = "VOLUMEN")]
        public double? VOLUMEN { get; set; }

        #endregion

        public ResultadoOperacion<List<container_brbk>> CargaPorBL(string usuario, string mrn,string msn, string hsn)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            this.alterClase = "N4";
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<container_brbk>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<container_brbk>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<container_brbk>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<container_brbk>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;
            this.Parametros.Clear();
            this.Parametros.Add(nameof(mrn),mrn);
            this.Parametros.Add(nameof(msn), msn);
            this.Parametros.Add(nameof(hsn), hsn);
            var rsql = BDOpe.ComandoSelectALista<container_brbk>(bcon, "[Bill].[container_brbk_impo]", this.Parametros);
#if DEBUG
            this.LogEvent(usuario, actualMetodo, string.Format("{0}/{1}/{2}",mrn,msn,hsn));
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container_brbk>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container_brbk>>.CrearFalla(rsql.MensajeProblema);
        }

        public static ResultadoOperacion<List<Tuple<Int64, string, string, bool>>> ValidarEstadoTransaccion(List<Int64> pases, string usuario)
        {
            var res = new List<Tuple<Int64, string, string, bool>>();
            //string lista-> cntrs, IMPORT, REFERENCIA,LINEA, 
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Importacion.container_brbk();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (pases == null || pases.Count <= 0)
            {
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla(string.Format(tt.Item1, nameof(pases)));
            }

            StringBuilder bb = new StringBuilder();
            bb.Append("<pases>");
            foreach (var c in pases)
            {
                bb.AppendFormat("<pase id=\"{0}\"/>", c);
            }
            bb.Append("</pases>");
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(pases), bb.ToString());
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[transaccion_brbk_estado](@pases);", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla(rsql.MensajeProblema);
            }
            if (!XmlTools.IsXml(rsql.Resultado))
            {
                p.LogError<ApplicationException>(new ApplicationException(rsql.Resultado), p.actualMetodo, usuario);
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla("El contenido de la variable respuesta no corresponde a un documento XML");
            }
            var xm = XDocument.Parse(rsql.Resultado);
            var cntrs = xm.Root.Elements().Where(e => e.Name.LocalName == "pase");
            if (cntrs == null || cntrs.Count() <= 0)
            {
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla("No se encontraron elementos pase");
            }

            //item1=>pase_id, item2=>estado_n4, item3=>estado_descritto, item4->puede usarse
            Int64 key;
            foreach (var cr in cntrs)
            {
                if (Int64.TryParse(cr.Attribute("id")?.Value, out key))
                {
                    res.Add(Tuple.Create(key, cr.Attribute("estado")?.Value, cr.Attribute("descripcion")?.Value, !string.IsNullOrEmpty(cr.Attribute("valido")?.Value) && cr.Attribute("valido").Value.Equals("1") ? true : false));
                }
            }
            return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearResultadoExitoso(res);
        }


        public static ResultadoOperacion<int> ProcesoRetiro(Int64 gkey, string usuario)
        {
            var p = new container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<int>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            //[Bill].[fx_unit_cas_date]
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("gkey", gkey);
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, gkey.ToString());
#endif
            var rp = BDOpe.ComandoSelectEscalar<int>(bcon, "select [Bill].[transaccion_brbk_estado_gkey](@gkey)", p.Parametros);
            if (!rp.Exitoso)
            {
                p.LogError<ApplicationException>(new ApplicationException(rp.MensajeProblema), p.actualMetodo, usuario);
                return ResultadoOperacion<int>.CrearFalla(rp.MensajeProblema);
            }
            return ResultadoOperacion<int>.CrearResultadoExitoso(rp.Resultado);
        }
    }



}

