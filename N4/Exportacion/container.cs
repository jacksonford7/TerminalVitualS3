using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Respuesta;
using AccesoDatos;
using Configuraciones;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml.Linq;

namespace N4.Exportacion
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
        public int? CNTR_TEMPERATURE { get; set; }

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

        [XmlAttribute(AttributeName = "CNTR_BKNG_BOOKING")]
        public string CNTR_BKNG_BOOKING  { get; set; }


        [XmlAttribute(AttributeName = "FECHA_CAS")]
        public DateTime? FECHA_CAS { get; set; }

        [XmlAttribute(AttributeName = "CNTR_AISV")]
        public string CNTR_AISV { get; set; }

        [XmlAttribute(AttributeName = "CNTR_HOLD")]
        public Int32? CNTR_HOLD { get; set; }

        [XmlAttribute(AttributeName = "CNTR_REEFER_CONT")]
        public string CNTR_REEFER_CONT { get; set; }


        [XmlAttribute(AttributeName = "CNTR_VEPR_VSSL_NAME")]
        public string CNTR_VEPR_VSSL_NAME   { get; set; }


        [XmlAttribute(AttributeName = "CNTR_VEPR_VOYAGE")]
        public string CNTR_VEPR_VOYAGE   { get; set; }


        [XmlAttribute(AttributeName = "CNTR_VEPR_ACTUAL_ARRIVAL")]
        public DateTime? CNTR_VEPR_ACTUAL_ARRIVAL { get; set; }


        [XmlAttribute(AttributeName = "CNTR_VEPR_ACTUAL_DEPARTED")]
        public DateTime? CNTR_VEPR_ACTUAL_DEPARTED { get; set; }

       [XmlAttribute(AttributeName = "CNTR_PROFORMA")]
        public string CNTR_PROFORMA  { get; set; }
        

       [XmlAttribute(AttributeName = "CNTR_HOURS")]
        public double? CNTR_HOURS { get; set; }

        #endregion

        public ResultadoOperacion<List<container>> CargaporReferencia(string usuario, string referencia)
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
            if (string.IsNullOrEmpty(referencia))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(string.Format(tt.Item1, nameof(referencia)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;
        
     
            this.Parametros.Add("referencia",referencia);
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_expo]", this.Parametros);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, referencia);
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }



        public ResultadoOperacion<List<container>> CargaporReferenciaPan(string usuario, string referencia)
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
            if (string.IsNullOrEmpty(referencia))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(string.Format(tt.Item1, nameof(referencia)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;


            this.Parametros.Add("referencia", referencia);
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_expo_inspeccion]", this.Parametros);
#if DEBUG
            this.LogEvent(usuario, actualMetodo, referencia);
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }

        public ResultadoOperacion<Dictionary<int,Entidades.Cliente>> ObtenerClientesPago(string BOOKING, string USUARIO)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Dictionary<int, Entidades.Cliente>>.CrearFalla(pv);
            }
            //fin inicializa
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO);
            if (string.IsNullOrEmpty(USUARIO))
            {
                return ResultadoOperacion<Dictionary<int, Entidades.Cliente>>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO)));
            }
            if (string.IsNullOrEmpty(BOOKING))
            {
                return ResultadoOperacion<Dictionary<int, Entidades.Cliente>>.CrearFalla(string.Format(tt.Item1, nameof(BOOKING)));
            }
            BOOKING = BOOKING.Trim().ToUpper();

            //paso 1--> Obtener los clientes de pago basado en una lista de items
            //   this.Parametros.Add("referencia", referencia);
            var bcon = this.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
            this.Parametros.Add(nameof(BOOKING),BOOKING);
            var rsql = BDOpe.ComandoSelectALista<ItemRetornable>(bcon, "[Bill].[clientes_pago_booking]", this.Parametros);
            //paso 2-->Crear un list<string> para recuperar los datos de cliente
            if (!rsql.Exitoso)
            {
                 return ResultadoOperacion<Dictionary<int, Entidades.Cliente>>.CrearFalla(string.Format("Fue imposible recuperar los clientes de pago del booking {0}", BOOKING));
            }
            var it = rsql.Resultado.FirstOrDefault();
            //pregunte si es nulo
            if (it == null)
            {
                return ResultadoOperacion<Dictionary<int, Entidades.Cliente>>.CrearFalla(string.Format("Fue imposible recuperar los clientes de pago del booking {0}, el objeto es nulo.", BOOKING));
            }

            var lstr = new List<string>();
            //ruc->pago tercero
            if (!string.IsNullOrEmpty(it.valor1))
            {
                lstr.Add(it.valor1);
            }
            //ruc->dae
            if (!string.IsNullOrEmpty(it.valor2))
            {
                lstr.Add(it.valor2);
            }
            //ruc->proforma
            if (!string.IsNullOrEmpty(it.valor3))
            {
                lstr.Add(it.valor3);
            }
            //obtenga la lista de clientes, porque debo retornar un diccionario
            var llclie = Entidades.Cliente.ObtenerClientes(lstr, USUARIO);
            if (!llclie.Exitoso)
            {
                return ResultadoOperacion<Dictionary<int, Entidades.Cliente>>.CrearFalla(string.Format("Fue imposible recuperar los clientes de N4/BILLING del booking {0}", BOOKING));
            }
            var resultado = new Dictionary<int, N4.Entidades.Cliente>();
            //BARRER LA LISTA DE CLIENTES N4/BILLING UBICAR DE ACUERDO A IMPORTANCIA
            foreach (var pp in llclie.Resultado)
            {
                if (pp.CLNT_CUSTOMER.Equals(it.valor1))
                {
                    resultado.Add(1, pp);
                }
                if (pp.CLNT_CUSTOMER.Equals(it.valor2))
                {
                    if (resultado.Where(f => f.Value.CLNT_CUSTOMER.Equals(pp.CLNT_CUSTOMER)).Count() <= 0)
                        resultado.Add(2, pp);
                }
                if (pp.CLNT_CUSTOMER.Equals(it.valor3))
                {
                    if (resultado.Where(f => f.Value.CLNT_CUSTOMER.Equals(pp.CLNT_CUSTOMER)).Count() <= 0)
                        resultado.Add(3, pp);
                }
            }
            return ResultadoOperacion<Dictionary<int, Entidades.Cliente>>.CrearResultadoExitoso(resultado);
 
        }

        //le paso la lista de gkeys (archivo puro del registro de exportacion), invoice-call-> que pantalla uso, header_id->cabecera
        public static ResultadoOperacion<string> ValidarEventos(string invoice_call, Int64? header_id, string containers, string usuario)
        {
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Exportacion.container();
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
            if (string.IsNullOrEmpty(containers))
            {
                return ResultadoOperacion<string>.CrearFalla(string.Format(tt.Item1, nameof(containers)));
            }
            if (!header_id.HasValue || header_id.Value<=0)
            {
                return ResultadoOperacion<string>.CrearFalla(string.Format(tt.Item1, nameof(header_id)));
            }

            if (!XmlTools.IsXml(containers))
            {
                p.LogError<ApplicationException>(new ApplicationException(containers), p.actualMetodo, usuario);
                return ResultadoOperacion<string>.CrearFalla("El contenido de la variable container no corresponde a un documento XML");
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(containers),containers);
            p.Parametros.Add(nameof(invoice_call), invoice_call);
            p.Parametros.Add(nameof(header_id), header_id);
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[unit_evento_validador](@containers,@invoice_call,@header_id);", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<string>.CrearFalla(rsql.MensajeProblema);
            }
            return ResultadoOperacion<string>.CrearResultadoExitoso(rsql.Resultado);
        }

        //el resultado del metodo Validar Eventos.
        public static ResultadoOperacion<Tuple<string,string>> ActualizaValidacion( string containers, string usuario)
        {
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Exportacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (string.IsNullOrEmpty(containers))
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(string.Format(tt.Item1, nameof(containers)));
            }
            if (!XmlTools.IsXml(containers))
            {
                p.LogError<ApplicationException>(new ApplicationException(containers), p.actualMetodo, usuario);
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla("El contenido de la variable container no corresponde a un documento XML");
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add("xmlref", containers);

            var rsql = BDOpe.ComandoSelectAEntidad<ItemRetornable>(bcon, "[Bill].[validacion_resultado]", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(rsql.MensajeProblema);
            }
            return ResultadoOperacion<Tuple<string, string>>.CrearResultadoExitoso(Tuple.Create(rsql.Resultado.id,rsql.Resultado.valor1));
        }

        public static ResultadoOperacion<List<Tuple<string, string, string,bool>>> ValidacionReeferExpo(List<N4.Exportacion.container> containers, string usuario)
        {

            var res = new List<Tuple<string, string, string,bool>>();

            //string lista-> cntrs, IMPORT, REFERENCIA,LINEA, 
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Exportacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Tuple<string, string, string,bool>>>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<Tuple<string, string, string,bool>>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (containers == null || containers.Count<=0)
            {
                return ResultadoOperacion<List<Tuple<string, string, string,bool>>>.CrearFalla(string.Format(tt.Item1, nameof(containers)));
            }

            StringBuilder bb = new StringBuilder();
            bb.Append("<containers>");
            foreach (var c in containers)
            {
                bb.AppendFormat("<container referencia=\"{0}\" linea=\"{1}\"  categoria=\"EXPORT\"  id=\"{2}\"/>",c.CNTR_VEPR_REFERENCE,c.CNTR_CLNT_CUSTOMER_LINE,c.CNTR_CONTAINER);
            }
            bb.Append("</containers>");
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(containers), bb.ToString());
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[unit_reefer_validador](@containers);", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<List<Tuple<string, string, string,bool>>>.CrearFalla(rsql.MensajeProblema);
            }
            if (!XmlTools.IsXml(rsql.Resultado))
            {
                p.LogError<ApplicationException>(new ApplicationException(rsql.Resultado), p.actualMetodo, usuario);
                return ResultadoOperacion<List<Tuple<string, string, string,bool>>>.CrearFalla("El contenido de la variable respuesta no corresponde a un documento XML");
            }
            var xm = XDocument.Parse(rsql.Resultado);
            var cntrs = xm.Root.Elements().Where(e => e.Name.LocalName == "cntr");
            if (cntrs==null || cntrs.Count()<=0)
            {
                return ResultadoOperacion<List<Tuple<string, string, string,bool>>>.CrearFalla("No se encontraron elementos cntr");
            }

            //item1=>CNTR, item2=>mensaje_horas, item3=>mensaje_otros
            foreach (var cr in cntrs)
            {
                res.Add(Tuple.Create(cr.Attribute("contenedor")?.Value, cr.Attribute("mensaje_horas")?.Value, cr.Attribute("mensaje_otros")?.Value, !string.IsNullOrEmpty(cr.Attribute("valido")?.Value) && cr.Attribute("valido").Value.Equals("1")?true:false   ));
            }
            return ResultadoOperacion<List<Tuple<string, string, string,bool>>>.CrearResultadoExitoso(res);
        }


        public static ResultadoOperacion<string> ValidacionReeferExpo(List<N4.Exportacion.container> containers, Int64 header_id, string usuario)
        {

           //string lista-> cntrs, IMPORT, REFERENCIA,LINEA, 
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Exportacion.container();
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
            if (containers == null || containers.Count <= 0)
            {
                return ResultadoOperacion<string>.CrearFalla(string.Format(tt.Item1, nameof(containers)));
            }

            StringBuilder bb = new StringBuilder();
            bb.Append("<containers>");
            foreach (var c in containers)
            {
                bb.AppendFormat("<container header_id=\"{3}\" gkey=\"{4}\" booking=\"{5}\" referencia=\"{0}\" linea=\"{1}\"  categoria=\"EXPORT\"  id=\"{2}\"/>", c.CNTR_VEPR_REFERENCE, c.CNTR_CLNT_CUSTOMER_LINE, c.CNTR_CONTAINER, header_id, c.CNTR_CONSECUTIVO, c.CNTR_BKNG_BOOKING);
            }
            bb.Append("</containers>");
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(containers), bb.ToString());
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[unit_reefer_validador_full](@containers);", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<string>.CrearFalla(rsql.MensajeProblema);
            }
            if (!XmlTools.IsXml(rsql.Resultado))
            {
                p.LogError<ApplicationException>(new ApplicationException(rsql.Resultado), p.actualMetodo, usuario);
                return ResultadoOperacion<string>.CrearFalla("El contenido de la variable respuesta no corresponde a un documento XML");
            }

            return ResultadoOperacion<string>.CrearResultadoExitoso(rsql.Resultado);
        }


        public static ResultadoOperacion<Tuple<string,string>> ActualizaValidacionReefer( string containers, string usuario)
        {
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Exportacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (string.IsNullOrEmpty(containers))
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(string.Format(tt.Item1, nameof(containers)));
            }
            if (!XmlTools.IsXml(containers))
            {
                p.LogError<ApplicationException>(new ApplicationException(containers), p.actualMetodo, usuario);
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla("El contenido de la variable container no corresponde a un documento XML");
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add("xmlref", containers);

            var rsql = BDOpe.ComandoSelectAEntidad<ItemRetornable>(bcon, "[Bill].[validacion_resultado_reefer]", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<Tuple<string, string>>.CrearFalla(rsql.MensajeProblema);
            }
            return ResultadoOperacion<Tuple<string, string>>.CrearResultadoExitoso(Tuple.Create(rsql.Resultado.id,rsql.Resultado.valor1));
        }

        //Nuevo expo pan, le pasa el XML como una lista de string
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
            this.Parametros.Add("units", sbb.ToString());
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_expo_mas]", this.Parametros);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, sbb.ToString());
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }
        //obtiene los pendientes de PAN, es decir de la tabla de inpecciones->se debe revisar si ya zarpo?
        public  ResultadoOperacion<List<string>> Carga_PAN(string usuario, DateTime? fecha, int maxregs )
        {
            //string lista-> cntrs, IMPORT, REFERENCIA,LINEA, 
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Exportacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<string>>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<string>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (!fecha.HasValue )
            {
                return ResultadoOperacion<List<string>>.CrearFalla(string.Format(tt.Item1, nameof(fecha)));
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("BILLION")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(fecha), fecha);
            p.Parametros.Add(nameof(maxregs), maxregs);
            var rsql = BDOpe.ComandoSelectALista<ItemRetornable>(bcon, "[Bill].[expo_pan_contenedores]", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<List<string>>.CrearFalla(rsql.MensajeProblema);
            }
            //convierto a lista simple de cadenas
            var rp = rsql.Resultado.Select(t => t.id).ToList();
            //retorno lista simple de cadebas
            return ResultadoOperacion<List<string>>.CrearResultadoExitoso(rp);
        }


        public ResultadoOperacion<HashSet<Tuple<string,string>>> Carga_PAN_Tipo(string usuario, DateTime? fecha, int maxregs)
        {
            //string lista-> cntrs, IMPORT, REFERENCIA,LINEA, 
            //retorna resultado de validacion de eventos de las unidades en containers
            var p = new Exportacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<HashSet<Tuple<string, string>>>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<HashSet<Tuple<string, string>>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (!fecha.HasValue)
            {
                return ResultadoOperacion<HashSet<Tuple<string, string>>>.CrearFalla(string.Format(tt.Item1, nameof(fecha)));
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("BILLION")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(fecha), fecha);
            p.Parametros.Add(nameof(maxregs), maxregs);
            var rsql = BDOpe.ComandoSelectALista<ItemRetornable>(bcon, "[Bill].[expo_pan_contenedores_tipo]", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<HashSet<Tuple<string, string>>>.CrearFalla(rsql.MensajeProblema);
            }

            //convierto a hashset simple de cadenas
            var hs = new HashSet<Tuple<string, string>>();
            foreach (var r in rsql.Resultado)
            {
                hs.Add(Tuple.Create(r.id,r.valor1));
            }

            //campo 1: gkey, campo 2: tipo bloqueo.
            //retorno lista simple de cadebas
            return ResultadoOperacion<HashSet<Tuple<string, string>>>.CrearResultadoExitoso(hs);
        }


        //OBTIENE EL PAR DAE-BOOKING
        public static Dictionary<string, string> ObtenerDAExBOOKING(List<container> unidades)
        {
            var dr = new Dictionary<string, string>();
            if (unidades != null && unidades.Count > 0)
            {
                foreach (var u in unidades)
                {
                    if ( !string.IsNullOrEmpty(u.CNTR_DOCUMENT) && !dr.ContainsKey(u.CNTR_DOCUMENT))
                        dr.Add(u.CNTR_DOCUMENT, u.CNTR_BKNG_BOOKING);
                }
                return dr;
            }
            return null;
        }
        //OBTIENE EL PAR AISV-BOOKING
        public static Dictionary<string, string> ObtenerAISVxBOOKING(List<container> unidades)
        {
            var dr = new Dictionary<string, string>();
            if (unidades != null && unidades.Count > 0)
            {
                foreach (var u in unidades)
                {
                    if (!string.IsNullOrEmpty(u.CNTR_AISV) && !dr.ContainsKey(u.CNTR_AISV))
                        dr.Add(u.CNTR_AISV, u.CNTR_BKNG_BOOKING);
                }
                return dr;
            }
            return null;
        }


        public ResultadoOperacion<List<container>> CargaporBooking(string usuario, string booking)
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
            if (string.IsNullOrEmpty(booking))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(string.Format(tt.Item1, nameof(booking)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;


            this.Parametros.Add("booking", booking);
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_expo_booking]", this.Parametros);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, booking);
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }


        public ResultadoOperacion<List<container>> CargaporBooking_NoExportada(string usuario, string booking)
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
            if (string.IsNullOrEmpty(booking))
            {
                return Respuesta.ResultadoOperacion<List<container>>.CrearFalla(string.Format(tt.Item1, nameof(booking)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;


            this.Parametros.Add("booking", booking);
            var rsql = BDOpe.ComandoSelectALista<container>(bcon, "[Bill].[container_noexpo_booking]", this.Parametros);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, booking);
#endif
            return rsql.Exitoso ? ResultadoOperacion<List<container>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<container>>.CrearFalla(rsql.MensajeProblema);
        }


        public static ResultadoOperacion<Int64> InsertarValidacionIndividualExpo(Int64? cntr_consecutivo,Int64 cntr_cab_id,string booking,string servicios_actuales,
                        int total,string msg_duplicado,string msg_faltante,string msg_otros,string faltantes,DateTime? fecha_registro,string usuario_registra,bool error,string referencia
            )
        {
            var p = new Exportacion.container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario_registra);
            if (string.IsNullOrEmpty(usuario_registra))
            {
                return ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(usuario_registra)));
            }
            if (cntr_cab_id<=0)
            {
                return ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(cntr_cab_id)));
            }
            if (string.IsNullOrEmpty(msg_duplicado)&& string.IsNullOrEmpty(msg_faltante) && string.IsNullOrEmpty(msg_otros))
            {
                return ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, "MENSAJES DE ERROR"));
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(cntr_consecutivo),cntr_consecutivo );
            p.Parametros.Add(nameof(cntr_cab_id),cntr_cab_id );
            p.Parametros.Add(nameof(booking),booking );
            p.Parametros.Add(nameof(servicios_actuales),servicios_actuales );
            p.Parametros.Add(nameof(total),total );
            p.Parametros.Add(nameof(msg_duplicado),msg_duplicado );
            p.Parametros.Add(nameof(msg_faltante),msg_faltante );
            p.Parametros.Add(nameof(msg_otros),msg_otros );
            p.Parametros.Add(nameof(faltantes),faltantes );
            p.Parametros.Add(nameof(usuario_registra),usuario_registra );
            p.Parametros.Add(nameof(error),error );
            p.Parametros.Add(nameof(referencia),referencia);
            var rsql = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[agregar_validacion_individual]", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<Int64>.CrearFalla(rsql.MensajeProblema);
            }
            if (!rsql.Resultado.HasValue)
            {
                return ResultadoOperacion<Int64>.CrearFalla("El resultado del ID es nulo");
            }
            return ResultadoOperacion<Int64>.CrearResultadoExitoso(rsql.Resultado.Value);
        }


    }
}
