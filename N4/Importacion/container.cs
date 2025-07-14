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
    [XmlRoot(ElementName = "container")]
    public class container : ModuloBase
    {

        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public container() : base()
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
        public Int32? CNTR_TEMPERATURE { get; set; }

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

        [XmlAttribute(AttributeName = "CNTR_CATY_CARGO_TYPE")]
        public string CNTR_CATY_CARGO_TYPE { get; set; }

        [XmlAttribute(AttributeName = "CNTR_FREIGHT_KIND")]
        public string CNTR_FREIGHT_KIND { get; set; }

        [XmlAttribute(AttributeName = "CNTR_DD")]
        public Int32? CNTR_DD { get; set; }

        [XmlAttribute(AttributeName = "CNTR_BKNG_BOOKIN")]
        public string CNTR_BKNG_BOOKIN { get; set; }

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

        [XmlAttribute(AttributeName = "CNTR_VEPR_ACTUAL_DEPARTED")]
        public DateTime CNTR_VEPR_ACTUAL_DEPARTED { get; set; }


        [XmlAttribute(AttributeName = "CNTR_DESCARGA")]
        public DateTime CNTR_DESCARGA { get; set; }

        [XmlAttribute(AttributeName = "CNTR_DESCARGA")]
        public string CAS_MOVE { get; set; }

        /*
        Fecha: 13/02/2025
        Autor: drodriguez
        Objetivo: se agregan propiedades para las consultas de carga de contenedores
        */
        public decimal CNTR_GROSS_WEIGHT { get; set; }
        public string CNTR_BKNG_BOOKING { get; set; }
        public string IB_CARRIER { get; set; }
        public string IB_ACTUAL_VISIT { get; set; }
        public string POD { get; set; }
        public DateTime? TIME_IN { get; set; }
        public double PESO_MANIFIESTO_TARA { get; set; }
        public double PESO_IMDT { get; set; }

        #endregion

        public ResultadoOperacion<List<container>> CargaPorKeys(string usuario, List<string> ukeys)
        {

            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            this.alterClase = "N4";
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;
            var sbb = new StringBuilder();
            sbb.Append("<UNIT>");
            foreach (var s in ukeys)
            {
                sbb.AppendFormat("<VALOR ID_GKEY=\"{0}\"/>", s);

            }
            sbb.Append("</UNIT>");
            this.Parametros.Add("unit_all", sbb.ToString());
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_impo]", this.Parametros);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, sbb.ToString());
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }
      

        // derco

        public ResultadoOperacion<List<container>> CargaPorKeysDerco(string usuario, List<string> ukeys, DateTime dtfecha_desde, DateTime dtfecha_hasta, string scntr, string snumero_carga, string smanifiesto)
        {

            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            this.alterClase = "N4";
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(pv);
            }

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;
            var sbb = new StringBuilder();

            this.Parametros.Add("numero_carga", string.IsNullOrEmpty(snumero_carga) ? null : snumero_carga);
            this.Parametros.Add("contenedor", scntr);
            this.Parametros.Add("fecha_desde", dtfecha_desde);
            this.Parametros.Add("fecha_hasta", dtfecha_hasta);
            this.Parametros.Add("bl_manifisesto", string.IsNullOrEmpty(smanifiesto) ? null : smanifiesto);
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_impo_sac]", this.Parametros);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, sbb.ToString());
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }
        public ResultadoOperacion<List<container>> PasePuertaInfo(string usuario, List<string> ukeys)
        {

            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;
            var sbb = new StringBuilder();
            sbb.Append("<UNIT>");
            foreach (var s in ukeys)
            {
                sbb.AppendFormat("<VALOR ID_GKEY=\"{0}\"/>", s);
            }
            sbb.Append("</UNIT>");
            this.Parametros.Add("unit_all", sbb.ToString());
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_info_pase]", this.Parametros);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, sbb.ToString());
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }

        public static ResultadoOperacion<DateTime?> ObtenerFechaCas(Int64 gkey, string usuario)
        {
            var p = new container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<DateTime?>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            //[Bill].[fx_unit_cas_date]
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("gkey", gkey);
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, gkey.ToString());
#endif
            var rp = BDOpe.ComandoSelectEscalar<DateTime>(bcon, "select [Bill].[fx_unit_cas_date](@gkey)", p.Parametros);
            if (!rp.Exitoso)
            {
                p.LogError<ApplicationException>(new ApplicationException(rp.MensajeProblema), p.actualMetodo, usuario);
                return ResultadoOperacion<DateTime?>.CrearFalla(rp.MensajeProblema);
            }
            return ResultadoOperacion<DateTime?>.CrearResultadoExitoso(rp.Resultado);
        }

        public static ResultadoOperacion<string> ValidarEventos(string invoice_call, Dictionary<Int64, string> contenedores, string usuario)
        {
            //retorna resultado de validacion de eventos de las unidades en containers
            Int64? header_id = -1;

            var p = new Importacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<string>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<string>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (string.IsNullOrEmpty(invoice_call))
            {
                return ResultadoOperacion<string>.CrearFalla(string.Format(tt.Item1, nameof(invoice_call)));
            }
            if (contenedores == null || contenedores.Count <= 0)
            {
                return ResultadoOperacion<string>.CrearFalla(string.Format(tt.Item1, nameof(contenedores)));
            }

            StringBuilder containers = new StringBuilder();
            containers.Append("<CONTENEDORES>");
            foreach (var cc in contenedores)
            {
                containers.AppendFormat("<CONTAINERS><ID>{0}</ID><CONTAINER>{1}</CONTAINER></CONTAINERS>", cc.Key, cc.Value);
            }
            containers.Append("</CONTENEDORES>");

            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(containers), containers.ToString());
            p.Parametros.Add(nameof(invoice_call), invoice_call);
            p.Parametros.Add(nameof(header_id), header_id);
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[unit_evento_validador](@containers,@invoice_call,@header_id);", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<string>.CrearFalla(rsql.MensajeProblema);
            }

            if (!XmlTools.IsXml(rsql.Resultado))
            {
                p.LogError<ApplicationException>(new ApplicationException(rsql.Resultado), p.actualMetodo, usuario);
                return ResultadoOperacion<string>.CrearFalla("El contenido de la validación no es un xml");
            }

            var xdoc = XDocument.Parse(rsql.Resultado);
            containers.Clear();
            bool? rs = null;
            foreach (var cntr in contenedores)
            {
                XElement cntrs_att = (from el in xdoc.Root.Elements("cntr")
                                      where (string)el.Attribute("gkey") == cntr.Key.ToString()
                                      select el).FirstOrDefault();

                if (!rs.HasValue)
                {
                    if (cntrs_att.Attribute("msg_duplicado")?.Value.Trim().Length > 0 || cntrs_att.Attribute("msg_faltante")?.Value.Trim().Length > 0 || cntrs_att.Attribute("msg_otros")?.Value.Trim().Length > 0)
                    {
                        rs = false;
                    }
                }
                string m;
                if (cntrs_att != null)
                {
                    {
                        m = string.Format("DUPLICADOS:{0} | FALTANTES:{1} | OTROS:{2}<br/>",
                            cntrs_att.Attribute("msg_duplicado")?.Value.Trim().Length > 0 ? cntrs_att.Attribute("msg_duplicado")?.Value : "NO",
                             cntrs_att.Attribute("msg_faltante")?.Value.Trim().Length > 0 ? cntrs_att.Attribute("msg_faltante")?.Value : "NO",
                            cntrs_att.Attribute("msg_otros")?.Value.Trim().Length > 0 ? cntrs_att.Attribute("msg_otros")?.Value : "NO");
                    }
                    containers.AppendLine(String.Format("{0}->{1}", cntr.Value, m));
                }
            }
            return !rs.HasValue ? ResultadoOperacion<string>.CrearResultadoExitoso(rsql.Resultado, "Validación Aprobada") : ResultadoOperacion<string>.CrearFalla(containers.ToString()); ;
        }

        public static ResultadoOperacion<List<Tuple<string, string, string, bool>>> ValidacionReeferImpo(List<N4.Importacion.container> containers, string usuario)
        {
            var res = new List<Tuple<string, string, string, bool>>();
            var p = new Exportacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Tuple<string, string, string, bool>>>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<Tuple<string, string, string, bool>>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (containers == null || containers.Count <= 0)
            {
                return ResultadoOperacion<List<Tuple<string, string, string, bool>>>.CrearFalla(string.Format(tt.Item1, nameof(containers)));
            }

            StringBuilder bb = new StringBuilder();
            bb.Append("<containers>");
            foreach (var c in containers)
            {
                bb.AppendFormat("<container referencia=\"{0}\" linea=\"{1}\"  categoria=\"IMPORT\"  id=\"{2}\"/>", c.CNTR_VEPR_REFERENCE, c.CNTR_CLNT_CUSTOMER_LINE, c.CNTR_CONTAINER);
            }
            bb.Append("</containers>");
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(containers), bb.ToString());
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[unit_reefer_validador](@containers);", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<List<Tuple<string, string, string, bool>>>.CrearFalla(rsql.MensajeProblema);
            }
            if (!XmlTools.IsXml(rsql.Resultado))
            {
                p.LogError<ApplicationException>(new ApplicationException(rsql.Resultado), p.actualMetodo, usuario);
                return ResultadoOperacion<List<Tuple<string, string, string, bool>>>.CrearFalla("El contenido de la variable respuesta no corresponde a un documento XML");
            }
            var xm = XDocument.Parse(rsql.Resultado);
            var cntrs = xm.Root.Elements().Where(e => e.Name.LocalName == "cntr");
            if (cntrs == null || cntrs.Count() <= 0)
            {
                return ResultadoOperacion<List<Tuple<string, string, string, bool>>>.CrearFalla("No se encontraron elementos cntr");
            }

            //item1=>CNTR, item2=>mensaje_horas, item3=>mensaje_otros
            foreach (var cr in cntrs)
            {
                res.Add(Tuple.Create(cr.Attribute("contenedor")?.Value, cr.Attribute("mensaje_horas")?.Value, cr.Attribute("mensaje_otros")?.Value, !string.IsNullOrEmpty(cr.Attribute("valido")?.Value) && cr.Attribute("valido").Value.Equals("1") ? true : false));
            }



            return ResultadoOperacion<List<Tuple<string, string, string, bool>>>.CrearResultadoExitoso(res);
        }

        public static ResultadoOperacion<List<Tuple<Int64, string, string, bool>>> ValidarEstadoContenedor(List<Int64> containers, string usuario)
        {
            var res = new List<Tuple<Int64, string, string, bool>>();
            var p = new Exportacion.container();
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
            if (containers == null || containers.Count <= 0)
            {
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla(string.Format(tt.Item1, nameof(containers)));
            }

            StringBuilder bb = new StringBuilder();
            bb.Append("<containers>");
            foreach (var c in containers)
            {
                bb.AppendFormat("<container gkey=\"{0}\"/>", c);
            }
            bb.Append("</containers>");
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(containers), bb.ToString());
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[container_estado](@containers);", p.Parametros);
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
            var cntrs = xm.Root.Elements().Where(e => e.Name.LocalName == "cntr");
            if (cntrs == null || cntrs.Count() <= 0)
            {
                return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearFalla("No se encontraron elementos cntr");
            }

            //item1=>CNTR, item2=>mensaje_horas, item3=>mensaje_otros
            Int64 key;
            foreach (var cr in cntrs)
            {
                if (Int64.TryParse(cr.Attribute("gkey")?.Value, out key))
                {
                    res.Add(Tuple.Create(key, cr.Attribute("transito")?.Value, cr.Attribute("descripcion")?.Value, !string.IsNullOrEmpty(cr.Attribute("valido")?.Value) && cr.Attribute("valido").Value.Equals("1") ? true : false));
                }
            }
            return ResultadoOperacion<List<Tuple<Int64, string, string, bool>>>.CrearResultadoExitoso(res);
        }


        public static Dictionary<string, string> AutoRefeerHours(string line, string referencia, string categoria, Dictionary<Int64, string> contenedores)
        {



            return null;
        }

    }



}

