using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using N4;
using System.Configuration;
using N4Ws;
using N4Ws.Entidad;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using N4.Entidades;
using System.Xml.Linq;
using ControlPagos.Importacion;
using CasManual;
using System.Web.UI.WebControls;
using System.Globalization;

namespace CLSiteUnitLogic.Cls_CargaSuelta
{
    public class Cls_CargaSuelta
    {
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        private Cls_Bil_Proforma_Cabecera objProforma = new Cls_Bil_Proforma_Cabecera();
        private Cls_Bil_Proforma_Detalle objDetalleProforma = new Cls_Bil_Proforma_Detalle();
        private Cls_Bil_Proforma_Servicios objServicios = new Cls_Bil_Proforma_Servicios();

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
        private Cls_Bil_Invoice_Validaciones objValidacion = new Cls_Bil_Invoice_Validaciones();

        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
        private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
        //private List<Cls_Bil_Turnos> List_Turnos { set; get; }
        private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }
        private Cls_Bil_Log_Appcgsa objLogAppCgsa = new Cls_Bil_Log_Appcgsa();
        //private List<N4.Importacion.container> ContainersReefer { set; get; }
        //  private MantenimientoPaqueteCliente obj = new MantenimientoPaqueteCliente();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

        private P2D_Proforma_Cabecera objProformaCab = new P2D_Proforma_Cabecera();
        private P2D_Proforma_Detalle objProformaDet = new P2D_Proforma_Detalle();
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();
        private Cls_Bil_PasePuertaCFS_SubItems objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
        private cfs_procesa_subsecuencias objGrabaSubSecuencias = new cfs_procesa_subsecuencias();
        private static string TextoLeyenda = string.Empty;
        private int NDiasLibreas = 0;
        private static string TextoProforma = string.Empty;
        private static string TextoServicio = string.Empty;


        #region "Clases"
        //private static Int64? lm = -3;
        //private string OError;

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

        //private Int64 Gkey = 0;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Cliente_CodigoSap = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        private int Fila = 1;
        private string TipoServicio = string.Empty;
        private DateTime FechaFactura;
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;

        //private decimal NEstadoCuenta = 0;

        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;


        private string gkeyBuscado = string.Empty;

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
        private bool SinDesconsolidar = false;
        private bool SinAutorizacion = false;
        private bool Bloqueos = false;
        #endregion

        #region "Propiedades"




        #endregion





        public ResultadoCargaSuelta ConsultarCargaSuelta(string mrn, string msn, string hsn, string usuarioLogin, string usuarioRuc, out string mensaje)
        {



            mensaje = string.Empty;

            var resultado = new ResultadoCargaSuelta
            {
                Cabecera = new Cls_Bil_Cabecera(),
                TieneErrores = false
            };
            var result = new ResultadoCargaSuelta();
            if (string.IsNullOrWhiteSpace(mrn) || string.IsNullOrWhiteSpace(msn) || string.IsNullOrWhiteSpace(hsn))
            {
                mensaje = "Debe ingresar MRN, MSN y HSN.";
                resultado.TieneErrores = true;
                return resultado;
            }
            try
            {

                bool Ocultar_Mensaje = true;
                bool cancelado = false;
                bool Tiene_Servicio_p2d = false;
                string Msg = string.Empty;

                string cMensajes = string.Empty;


                /*saco los dias libre como parametros generales*/
                List<Cls_Bil_Configuraciones> Parametros = Cls_Bil_Configuraciones.Parametros(out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    return result;
                }

                var LinqDiasLibres = (from Dias in Parametros.Where(Dias => Dias.NOMBRE.Equals("DIASLIBRES"))
                                      select new
                                      {
                                          VALOR = Dias.VALOR == null ? string.Empty : Dias.VALOR
                                      }).FirstOrDefault();



                /***************************fin de dias libres***************************/


                /*para almacenar clientes que asumen factura*/
                var List_Asume = new List<Cls_Bil_AsumeFactura>();
                List_Asume.Clear();
                List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                //busca contenedores por ruc de usuario
                string IdAgenteCodigo = string.Empty;
                var AgenteCod = N4.Entidades.Agente.ObtenerAgentePorRuc(usuarioLogin, usuarioRuc);
                if (AgenteCod.Exitoso)
                {
                    var ListaAgente = AgenteCod.Resultado;
                    if (ListaAgente != null)
                    {
                        IdAgenteCodigo = ListaAgente.codigo;
                    }
                }

                var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                var EcuaContenedores = Validacion.CargaPorManifiestoImpo(usuarioLogin, usuarioRuc, IdAgenteCodigo, mrn, msn, hsn, true);
                if (EcuaContenedores.Exitoso)
                {
                    //DATOS DEL AGENTE PARA BUSCAR INFORMACION
                    var LinqAgente = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                      select new
                                      {
                                          ID_AGENTE = Tbl.agente_id,
                                          ID_CLIENTE = Tbl.importador_id,
                                          DESC_CLIENTE = (Tbl.importador == null ? string.Empty : Tbl.importador)
                                      }).FirstOrDefault();

                    var hf_idagente = LinqAgente.ID_AGENTE;
                    var hf_idcliente = LinqAgente.ID_CLIENTE;
                    var hf_idasume = LinqAgente.ID_CLIENTE;

                    var hf_rucagente = string.Empty;
                    var hf_descagente = string.Empty;
                    var hf_descasume = string.Empty;
                    var hf_desccliente = LinqAgente.DESC_CLIENTE;

                    //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
                    //    var Agente = N4.Entidades.Agente.ObtenerAgente(usuarioLogin, hf_idagente);



                    //LLENADO DE CAMPOS DE PANTALLA DEL CLIENTE


                    /*verifica si la carga tiene mas personas que van asumir la carga*/
                    var Resultado = PagoAsignado.ListaAsignacionPartida(usuarioLogin, mrn, msn, hsn);
                    if (Resultado != null)
                    {
                        if (Resultado.Exitoso)
                        {
                            var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.ruc))
                                            select new
                                            {
                                                ruc = Tbl.ruc,
                                                nombre = Tbl.nombre,
                                                mostrar = string.Format("{0} - {1}", Tbl.ruc, Tbl.nombre)
                                            };
                            foreach (var Items in LinqQuery)
                            {
                                List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = Items.ruc.Trim(), nombre = Items.nombre.Trim(), mostrar = Items.mostrar });
                            }

                        }

                    }

                    List<Cls_Bil_Configuraciones> ListValidaClientes = Cls_Bil_Configuraciones.Get_Validacion("CLIENTE_FF", out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        result.TieneErrores = true;
                        return result;

                    }

                    bool ValidaClientes = false;

                    if (ListValidaClientes.Count != 0)
                    {
                        ValidaClientes = true;
                    }


                    List<Cls_Bil_Configuraciones> ValidaCarbono = Cls_Bil_Configuraciones.Get_Validacion("CARBONO_NEUTRO", out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        result.TieneErrores = true;
                        return result;

                    }
                    else
                    {
                        //si esta activo el servicio de carbono neutro
                        if (ValidaCarbono != null)
                        {
                            if (ValidaCarbono.Count != 0)
                            {
                                //si es cliente de cabono Neutro
                                bool Existe_cliente = Cls_ImpoContenedor.ExisteUsuarioCarbono(hf_idcliente, out cMensajes);
                                if (!string.IsNullOrEmpty(cMensajes))
                                {
                                    result.TieneErrores = true;
                                    return result;
                                }

                                //si no existe, muestra marcado
                                if (!Existe_cliente)
                                {
                                    //valida si esta inactivo, para dejar sin efecto el visto
                                    bool Cliente_Inactivo = Cls_ImpoContenedor.ExisteUsuarioCarbonoInactivo(hf_idcliente, out cMensajes);
                                    if (!string.IsNullOrEmpty(cMensajes))
                                    {
                                        result.TieneErrores = true;
                                        return result;
                                    }


                                }
                                //else
                                //{
                                //    //si existe el cliente con el servicio, muestra bloqueado
                                //    this.LblCarbono.Visible = true;
                                //    this.ChkCarbono.Attributes["disabled"] = "disabled";
                                //    this.ChkCarbono.Checked = true;
                                //    this.LblTituloCarbono.InnerText = "Certificado de Carbono Neutro (ACTIVADO)";
                                //    this.UPCARBONO.Update();
                                //}
                            }

                        }


                    }
                    /*************************************************************************************************************
                    *fin carbono neutro
                    **************************************************************************************************************/




                    //INFORMACION DEL CONTENEDOR
                    var Contenedor = new N4.Importacion.container_cfs();
                    var ListaContenedores = Contenedor.CargaPorBL(usuarioLogin, mrn, msn, hsn);//resultado de entidad contenedor y cfs
                    if (ListaContenedores.Exitoso)
                    {



                        var List_Autorizacion = new List<Cls_Bil_Cas_Manual>();
                        List_Autorizacion.Clear();


                        //autorizacion de salida de la carga cfs
                        var Autorizacion = CasBBK.ListaCasPartida(usuarioLogin, mrn, msn, hsn);
                        if (Autorizacion.Exitoso)
                        {
                            var LinqAut = (from Tbl in Autorizacion.Resultado.Where(Tbl => Tbl.activo)
                                           select new
                                           {
                                               CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                               FECHA_AUTORIZACION = Tbl.fecha_registro,
                                               AUTORIZADO = true,
                                               USUARIO_AUTORIZA = (Tbl.usuario_libera == null ? string.Empty : Tbl.usuario_libera),
                                               CONSIGNATARIO = (Tbl.consignatario_manifiesto == null ? string.Empty : Tbl.consignatario_manifiesto)
                                           }).FirstOrDefault();

                            List_Autorizacion.Add(new Cls_Bil_Cas_Manual { CARGA = LinqAut.CARGA, FECHA_AUTORIZACION = LinqAut.FECHA_AUTORIZACION, AUTORIZADO = LinqAut.AUTORIZADO, USUARIO_AUTORIZA = LinqAut.USUARIO_AUTORIZA, CONSIGNATARIO = LinqAut.CONSIGNATARIO });


                        }
                        else
                        {
                            List_Autorizacion.Add(new Cls_Bil_Cas_Manual
                            {
                                CARGA = string.Format("{0}-{1}-{2}", mrn, msn, hsn),
                                FECHA_AUTORIZACION = (DateTime?)null,
                                AUTORIZADO = false,
                                USUARIO_AUTORIZA = string.Empty,
                                CONSIGNATARIO = string.Empty
                            });
                        }

                        var LinqAutorizacion = (from Tbl in List_Autorizacion
                                                select new
                                                {
                                                    CARGA = Tbl.CARGA,
                                                    FECHA_AUTORIZACION = Tbl.FECHA_AUTORIZACION,
                                                    AUTORIZADO = Tbl.AUTORIZADO,
                                                    USUARIO_AUTORIZA = Tbl.USUARIO_AUTORIZA,
                                                    CONSIGNATARIO = Tbl.CONSIGNATARIO
                                                });


                        //informacion ecuapass     
                        var LinqPartidas = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                            select new
                                            {
                                                MRN = Tbl.mrn,
                                                MSN = Tbl.msn,
                                                HSN = Tbl.hsn,
                                                IMDT = (Tbl.imdt_id == null) ? "" : Tbl.imdt_id,
                                                GKEY = (Tbl.gkey == null ? 0 : Tbl.gkey),
                                                CONTENEDOR = (Tbl.cntr == null) ? "" : Tbl.cntr,
                                                BL = (Tbl.documento_bl == null) ? "" : Tbl.documento_bl,
                                                DECLARACION = (Tbl.declaracion == null) ? "" : Tbl.declaracion,
                                                ESTADO_RIDT = (Tbl.ridt_estado == null ? "" : Tbl.ridt_estado),
                                                CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn)
                                            }).Distinct();

                        //contenedores con carga cfs
                        var LinqPartidadN4 = (from Tbl in ListaContenedores.Resultado.Where(Tbl => Tbl.CNTR_CANTIDAD != 0)
                                              select new
                                              {
                                                  CNTR_CONTAINER = (Tbl.CNTR_AISV == null ? string.Empty : Tbl.CNTR_AISV),
                                                  CNTR_VEPR_REFERENCE = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                  CNTR_TYPE = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                  CNTR_TYSZ_SIZE = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                  CNTR_CATY_CARGO_TYPE = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                                  FECHA_CAS = (DateTime?)(Tbl.FECHA_CAS.HasValue ? Tbl.FECHA_CAS : null),
                                                  BLOQUEOS = Tbl.CNTR_HOLD,
                                                  CNTR_YARD_STATUS = (Tbl.CNTR_YARD_STATUS == null) ? string.Empty : Tbl.CNTR_YARD_STATUS,
                                                  CNTR_TYSZ_TYPE = (Tbl.CNTR_TYSZ_TYPE == null) ? string.Empty : Tbl.CNTR_TYSZ_TYPE,
                                                  CNTR_CLNT_CUSTOMER_LINE = (Tbl.CNTR_CLNT_CUSTOMER_LINE == null) ? string.Empty : Tbl.CNTR_CLNT_CUSTOMER_LINE,
                                                  CNTR_DOCUMENT = (Tbl.CNTR_DOCUMENT == null ? string.Empty : Tbl.CNTR_DOCUMENT),
                                                  CNTR_FULL_EMPTY_CODE = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                                  CNTR_CONSECUTIVO = Tbl.CNTR_CONSECUTIVO,
                                                  CNTR_AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                                  CNTR_HOLD = (Tbl.CNTR_HOLD == 0) ? false : true,
                                                  CNTR_VEPR_VOYAGE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                  CNTR_VEPR_VSSL_NAME = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                  CNTR_VEPR_ACTUAL_ARRIVAL = (Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.HasValue ? Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.Value.ToString("dd/MM/yyyy") : ""),
                                                  CNTR_DD = (Tbl.CNTR_DD == null) ? false : Tbl.CNTR_DD.Value,
                                                  CNTR_DESCARGA = (Tbl.CNTR_DESCARGA == null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                  CNTR_VEPR_ACTUAL_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                                  CNTR_CANTIDAD = (Tbl.CNTR_CANTIDAD == null ? 0 : Tbl.CNTR_CANTIDAD),
                                                  CNTR_PESO = (Tbl.CNTR_PESO == null ? 0 : Tbl.CNTR_PESO),
                                                  CNTR_OPERACION = (Tbl.CNTR_OPERACION == null ? string.Empty : Tbl.CNTR_OPERACION),
                                                  CNTR_DESCRIPCION = (Tbl.CNTR_DESCRIPCION == null ? string.Empty : Tbl.CNTR_DESCRIPCION),
                                                  CNTR_EXPORTADOR = (Tbl.CNTR_EXPORTADOR == null ? string.Empty : Tbl.CNTR_EXPORTADOR),
                                                  CNTR_AGENCIA = (Tbl.CNTR_AGENCIA == null ? string.Empty : Tbl.CNTR_AGENCIA),
                                                  CARGA = string.Format("{0}-{1}-{2}", Tbl.CNTR_MRN, Tbl.CNTR_MSN, Tbl.CNTR_HSN),
                                                  CNTR_REEFER_CONT = (Tbl.CNTR_REEFER_CONT == null ? string.Empty : Tbl.CNTR_REEFER_CONT),
                                                  ID_UNIDAD = (Tbl.ID_UNIDAD == null ? 0 : Tbl.ID_UNIDAD),
                                              }).OrderBy(x => x.CNTR_CANTIDAD).ThenBy(x => x.CNTR_CONTAINER);

                        /*ultima factura*/
                        List<Cls_Bil_Invoice_Ultima_Factura> ListUltimaFactura = Cls_Bil_Invoice_Ultima_Factura.List_Ultima_Factura_cfs(mrn + "-" + msn + "-" + hsn, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            result.TieneErrores = true;
                            return result;
                        }
                        /*ultima factura en caso de tener*/
                        var LinqUltimaFactura = (from TblFact in ListUltimaFactura.Where(TblFact => !string.IsNullOrEmpty(TblFact.IV_FACTURA))
                                                 select new
                                                 {
                                                     FT_NUMERO_CARGA = TblFact.IV_NUMERO_CARGA,
                                                     FT_FECHA = (TblFact.IV_FECHA == null ? null : TblFact.IV_FECHA),
                                                     FT_FACTURA = TblFact.IV_FACTURA,
                                                     FT_FECHA_HASTA = (TblFact.IV_FECHA_HASTA == null ? null : TblFact.IV_FECHA_HASTA),
                                                     FT_ID = TblFact.IV_ID,
                                                     FT_MODULO = TblFact.IV_MODULO
                                                 }).Distinct();


                        /*pase puerta*/
                        List<Cls_Bil_Invoice_Pase_Puerta> ListPasePuerta = Cls_Bil_Invoice_Pase_Puerta.List_Pase_Puerta_cfs(mrn + "-" + msn + "-" + hsn, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            result.TieneErrores = true;
                            return result;
                        }
                        /*ultima factura en caso de tener*/
                        var LinqListPasePuerta = (from TblPase in ListPasePuerta.Where(TblPase => TblPase.GKEY != 0)
                                                  select new
                                                  {
                                                      ID_PPWEB = TblPase.ID_PPWEB,
                                                      ID_PASE = TblPase.ID_PASE,
                                                      GKEY = TblPase.GKEY,
                                                      CONTENEDOR = TblPase.CONTENEDOR,
                                                      NUMERO_PASE_N4 = TblPase.NUMERO_PASE_N4,
                                                  }).Distinct();


                        /*verificar si tiene servicio de carbono neutro*/
                        XDocument XMLContenedores = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                               new XElement("CONTENEDORES", from p in LinqPartidadN4.AsEnumerable().AsParallel()
                                                            select new XElement("CONTENEDOR",
                                                    new XAttribute("gkey", p.ID_UNIDAD == null ? "0" : p.ID_UNIDAD.ToString()),
                                                    new XAttribute("contenedor", p.CNTR_CONTAINER == null ? "" : p.CNTR_CONTAINER.Trim())
                                                    )));

                        List<Cls_Bil_Valida_Certificado> ListCertificado = Cls_Bil_Valida_Certificado.Validacion_Certificado_Cfs(XMLContenedores.ToString(), out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            result.TieneErrores = true;
                            return result;
                        }

                        /*listado de unidades con o sin el servicio*/
                        var LinqCertificado = (from TblFact in ListCertificado.Where(TblFact => TblFact.gkey != 0)
                                               select new
                                               {
                                                   gkey = TblFact.gkey,
                                                   contenedor = TblFact.contenedor,
                                                   servicio = TblFact.servicio
                                               }).Distinct();


                        /*consulto si tiene proforma emitida*/
                        List<Cls_Bil_Configuraciones> ListP2D = Cls_Bil_Configuraciones.Get_Validacion("P2D", out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            result.TieneErrores = true;
                            return result;

                        }

                        bool Valida_P2D = false;

                        if (ListP2D.Count != 0)
                        {
                            Valida_P2D = true;
                        }

                        //si tiene activo para el servicio de PORT TO DOOR
                        //if (Valida_P2D && (ClsUsuario.loginname.Trim().Equals("TER_INCALINES") || ClsUsuario.loginname.Trim().Equals("TER_COKA") || ClsUsuario.loginname.Trim().Equals("TER_UNI")
                        //     || ClsUsuario.loginname.Trim().Equals("TER_SACO") || ClsUsuario.loginname.ToUpper().Trim().Equals("TERMINAL")))
                        if (Valida_P2D)
                        {


                            //valido que no tenga una cotizacion activa
                            objValida = new P2D_Valida_Proforma();
                            objValida.MRN = mrn;
                            objValida.MSN = msn;
                            objValida.HSN = hsn;
                            objValida.RUC = string.IsNullOrEmpty(usuarioLogin) ? "" : usuarioRuc;
                            if (objValida.PopulateMyData(out cMensajes))
                            {

                                Tiene_Servicio_p2d = true;

                                Msg = string.IsNullOrEmpty(TextoProforma) ? "" : string.Format(TextoProforma, objValida.ID_PROFORMA, objValida.FECHA_EMISION.ToString("dd/MM/yyyy"), objValida.TOTAL_PAGAR);

                                if (string.IsNullOrEmpty(Msg))
                                {
                                    Msg = string.Format("<b>Estimado cliente,</b> Ud tiene una cotización activa del servicio de transporte P2D, cotización # {0}, emitida el {1} por un valor de {2:c}, el cual le será cargado a su factura de carga suelta.",
                                        objValida.ID_PROFORMA, objValida.FECHA_EMISION.ToString("dd/MM/yyyy"), objValida.TOTAL_PAGAR);
                                }


                            }
                            else
                            {

                            }
                            //valida si tiene ya cargado el servicio de transporte, ya no muestra el mensaje de que tiene cotizaciones.
                            //List<P2D_Valida_Proforma> ListTransp = P2D_Valida_Proforma.Validacion_Servicio_Transporte_Cfs(XMLContenedores.ToString(), out cMensajes);
                            //if (!String.IsNullOrEmpty(cMensajes))
                            //{

                            //    result.TieneErrores = true;
                            //    return result;
                            //}
                            /*listado de unidades con o sin el servicio*/
                            //var LinqTransp = (from TblFact in ListTransp.Where(TblFact => TblFact.gkey != 0 && TblFact.servicio == 1)
                            //                  select new
                            //                  {
                            //                      gkey = TblFact.gkey,
                            //                      contenedor = TblFact.contenedor,
                            //                      servicio = TblFact.servicio
                            //                  }).Distinct();

                            //if (LinqTransp.Count() != 0)
                            //{
                            //    Tiene_Servicio_p2d = true;



                            //}

                            //si no tiene el servicio, muestro para que pueda seleccionar
                            if (!Tiene_Servicio_p2d)
                            {
                                objTarja = new P2D_Tarja_Cfs();
                                objTarja.mrn = mrn;
                                objTarja.msn = msn;
                                objTarja.hsn = hsn;
                                objTarja.ruc = string.IsNullOrEmpty(usuarioLogin) ? "" : usuarioRuc;
                                objTarja.apilable = false;
                                if (!objTarja.PopulateMyData(out cMensajes))
                                {
                                    result.TieneErrores = true;
                                    return result;
                                }

                                //busco si existe en el tarifario
                                objTarifa = new P2D_Tarifario();
                                objTarifa.M3 = objTarja.m3;
                                objTarifa.PESO = objTarja.pesokg;

                                if (!objTarifa.PopulateMyData(out cMensajes))
                                {
                                    result.TieneErrores = true;
                                    return result;
                                }

                                Msg = string.IsNullOrEmpty(TextoServicio) ? "" : string.Format(TextoServicio, objTarja.pesokg.ToString("N2"), objTarja.m3.ToString("N2"), objTarifa.TOTAL_PAGAR);

                            }
                        }

                        //valida multidespacho, para el tema de las medidas de las tarjas
                        /*consulto si tiene proforma emitida*/
                        List<Cls_Bil_Configuraciones> MultiDespacho = Cls_Bil_Configuraciones.Get_Validacion("MULTIDESPACHO", out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            result.TieneErrores = true;
                            return result;

                        }

                        bool Valida_MultiDespacho = false;

                        if (MultiDespacho.Count != 0)
                        {
                            Valida_MultiDespacho = true;
                        }



                        //fin multidespacho


                        /*left join de contenedores*/
                        var LinqQuery = (from Tbl in LinqPartidadN4
                                         join EcuaPartidas in LinqPartidas on Tbl.CARGA equals EcuaPartidas.CARGA into TmpFinal
                                         join Factura in LinqUltimaFactura on Tbl.CARGA equals Factura.FT_NUMERO_CARGA into TmpFactura
                                         join AutCas in LinqAutorizacion on Tbl.CARGA equals AutCas.CARGA into TmpAutorizacion
                                         join Certificado in LinqCertificado on Tbl.ID_UNIDAD equals Certificado.gkey into TmpCertificado
                                         from Final in TmpFinal.DefaultIfEmpty()
                                         from FinalFT in TmpFactura.DefaultIfEmpty()
                                         from FinalAut in TmpAutorizacion.DefaultIfEmpty()
                                         from FinalCT in TmpCertificado.DefaultIfEmpty()
                                         select new
                                         {
                                             CONTENEDOR = Tbl.CNTR_CONTAINER,
                                             REFERENCIA = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                             TRAFICO = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                             TAMANO = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                             TIPO = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                             FECHA_CAS = (DateTime?)(FinalAut == null ? null : FinalAut.FECHA_AUTORIZACION),
                                             AUTORIZADO = (bool?)(FinalAut == null ? false : FinalAut.AUTORIZADO),
                                             BLOQUEOS = (Tbl.CNTR_HOLD == false) ? string.Empty : "SI",
                                             IN_OUT = (Tbl.CNTR_YARD_STATUS == null) ? string.Empty : Tbl.CNTR_YARD_STATUS,
                                             TIPO_CONTENEDOR = (Tbl.CNTR_TYSZ_TYPE == null) ? string.Empty : Tbl.CNTR_TYSZ_TYPE,//REEFER
                                             CONECTADO = ((Tbl.CNTR_TYSZ_TYPE == "RF") ? ((Tbl.CNTR_REEFER_CONT == "N") ? "NO CONECTADO" : "CONECTADO") : string.Empty),
                                             LINEA = (Tbl.CNTR_CLNT_CUSTOMER_LINE == null) ? string.Empty : Tbl.CNTR_CLNT_CUSTOMER_LINE,
                                             DOCUMENTO = (string.IsNullOrEmpty(Tbl.CNTR_DOCUMENT) ? ((Final == null) ? string.Empty : Final.DECLARACION) : Tbl.CNTR_DOCUMENT),
                                             IMDT = (Final == null) ? string.Empty : Final.IMDT,
                                             BL = (Final == null) ? string.Empty : Final.BL,
                                             FULL_VACIO = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                             GKEY = Tbl.CNTR_CONSECUTIVO,
                                             AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                             DECLARACION = (Final == null) ? string.Empty : Final.DECLARACION,
                                             BLOQUEADO = Tbl.CNTR_HOLD,
                                             FECHA_ULTIMA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA.HasValue ? FinalFT.FT_FECHA : null),
                                             NUMERO_FACTURA = (FinalFT == null) ? string.Empty : FinalFT.FT_FACTURA,
                                             ID_FACTURA = (FinalFT == null) ? 0 : FinalFT.FT_ID,
                                             VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                             NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                             FECHA_ARRIBO = Tbl.CNTR_VEPR_ACTUAL_ARRIVAL,
                                             CNTR_DD = Tbl.CNTR_DD,
                                             FECHA_HASTA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA_HASTA.HasValue ? FinalFT.FT_FECHA_HASTA : null),
                                             ESTADO_RIDT = (Final.ESTADO_RIDT == null) ? string.Empty : Final.ESTADO_RIDT,
                                             CNTR_DESCARGA = (Tbl.CNTR_DESCARGA == null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                             MODULO = (FinalFT == null) ? string.Empty : FinalFT.FT_MODULO,
                                             CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                             CANTIDAD = Tbl.CNTR_CANTIDAD,
                                             PESO = Tbl.CNTR_PESO,
                                             OPERACION = Tbl.CNTR_OPERACION,
                                             DESCRIPCION = Tbl.CNTR_DESCRIPCION,
                                             EXPORTADOR = Tbl.CNTR_EXPORTADOR,
                                             AGENCIA = Tbl.CNTR_AGENCIA,
                                             CARGA = Tbl.CARGA,
                                             ID_UNIDAD = Tbl.ID_UNIDAD,
                                             //CERTIFICADO = ((FinalCT == null) ? false : (FinalCT.servicio == 0 ? true : false)),
                                             CERTIFICADO = ((FinalCT == null) ? false : (FinalCT.servicio == 0 && true ? true : (FinalCT.servicio == 1 ? true : false))),
                                             TIENE_CERTIFICADO = ((FinalCT == null) ? "NO" : (FinalCT.servicio == 0 ? "NO" : "SI")),
                                         }).OrderBy(x => x.IN_OUT).ThenBy(x => x.CONTENEDOR);

                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objCabecera = new Cls_Bil_Cabecera();

                            objCabecera.ID_CLIENTE = hf_idcliente;
                            objCabecera.DESC_CLIENTE = hf_desccliente;
                            objCabecera.ID_FACTURADO = hf_idasume;
                            objCabecera.DESC_FACTURADO = hf_descasume;
                            objCabecera.ID_UNICO_AGENTE = hf_idagente;
                            objCabecera.ID_AGENTE = hf_rucagente;
                            objCabecera.DESC_AGENTE = hf_descagente;
                            objCabecera.FECHA = DateTime.Now;
                            objCabecera.TIPO_CARGA = "CFS";
                            objCabecera.NUMERO_CARGA = mrn + "-" + msn + "-" + hsn;
                            objCabecera.IV_USUARIO_CREA = usuarioLogin;
                            objCabecera.SESION = "";
                            objCabecera.HORA_HASTA = "00:00";

                            objCabecera.Detalle.Clear();
                            objCabecera.DetalleSubItem.Clear();

                            Int16 Secuencia = 1;
                            Int64 _GKEY = 0;

                            foreach (var Det in LinqQuery)
                            {
                                /*datos nuevos para imprimir factura*/
                                objCabecera.BL = Det.BL;
                                objCabecera.BUQUE = Det.NAVE;
                                objCabecera.VIAJE = Det.VIAJE;
                                objCabecera.FECHA_ARRIBO = Det.FECHA_ARRIBO;


                                objDetalle = new Cls_Bil_Detalle();
                                objDetalle.VISTO = false;
                                objDetalle.ID = Det.ID_FACTURA;
                                objDetalle.SECUENCIA = Secuencia;
                                objDetalle.GKEY = Det.GKEY;
                                _GKEY = Det.GKEY;
                                objDetalle.MRN = mrn;
                                objDetalle.MSN = msn;
                                objDetalle.HSN = hsn;
                                objDetalle.CONTENEDOR = Det.CONTENEDOR;
                                objDetalle.TRAFICO = Det.TRAFICO;
                                objDetalle.DOCUMENTO = Det.DOCUMENTO;
                                objDetalle.DES_BLOQUEO = Det.BLOQUEOS;
                                objDetalle.CONECTADO = Det.CONECTADO;
                                objDetalle.REFERENCIA = Det.REFERENCIA;
                                objDetalle.TAMANO = Det.TAMANO;
                                objDetalle.TIPO = "CFS";
                                objDetalle.CAS = Det.FECHA_CAS;
                                objDetalle.AUTORIZADO = (Det.AUTORIZADO == true ? "SI" : "NO");
                                objDetalle.BOOKING = "";

                                objDetalle.IMDT = Det.IMDT;
                                objDetalle.BLOQUEO = Det.BLOQUEADO;
                                // objDetalle.FECHA_ULTIMA = Det.FECHA_ULTIMA;
                                objDetalle.FECHA_ULTIMA = Det.FECHA_HASTA;
                                objDetalle.IN_OUT = Det.IN_OUT;
                                objDetalle.FULL_VACIO = Det.FULL_VACIO;
                                objDetalle.AISV = Det.AISV;
                                objDetalle.REEFER = Det.TIPO_CONTENEDOR;
                                objDetalle.IV_USUARIO_CREA = usuarioLogin;
                                objDetalle.IV_FECHA_CREA = DateTime.Now;
                                objDetalle.NUMERO_FACTURA = Det.NUMERO_FACTURA;
                                objDetalle.CNTR_DD = Det.CNTR_DD;
                                objDetalle.FECHA_HASTA = Det.FECHA_HASTA;
                                objDetalle.ESTADO_RDIT = Det.ESTADO_RIDT.Trim();
                                objDetalle.CNTR_DESCARGA = Det.CNTR_DESCARGA;
                                objDetalle.MODULO = Det.MODULO;
                                objDetalle.CNTR_DEPARTED = Det.CNTR_DEPARTED;
                                objDetalle.LINEA = Det.LINEA;

                                //nuevos campos
                                objDetalle.CANTIDAD = decimal.Parse(Det.CANTIDAD.Value.ToString());
                                objDetalle.PESO = decimal.Parse(Det.PESO.ToString());
                                objDetalle.OPERACION = Det.OPERACION;
                                objDetalle.DESCRIPCION = Det.DESCRIPCION;
                                objDetalle.EXPORTADOR = Det.EXPORTADOR;
                                objDetalle.AGENCIA = Det.AGENCIA;
                                //nuevo
                                objDetalle.ID_UNIDAD = Det.ID_UNIDAD;

                                if (!objDetalle.ESTADO_RDIT.Equals("A"))
                                {
                                    cancelado = true;
                                }

                                if (NDiasLibreas != 0)
                                {
                                    objDetalle.FECHA_TOPE_DLIBRE = objDetalle.CNTR_DESCARGA.Value.AddDays(NDiasLibreas);
                                }
                                else
                                {
                                    objDetalle.FECHA_TOPE_DLIBRE = objDetalle.CNTR_DESCARGA;
                                }


                                objDetalle.IDPLAN = "0";
                                objDetalle.TURNO = "* Seleccione *";

                                var Pase = LinqListPasePuerta.FirstOrDefault(f => f.ID_PPWEB != 0);
                                if (Pase != null)
                                {
                                    objDetalle.NUMERO_PASE_N4 = Pase.NUMERO_PASE_N4;
                                }

                                objDetalle.CERTIFICADO = Det.CERTIFICADO;
                                objDetalle.TIENE_CERTIFICADO = Det.TIENE_CERTIFICADO;

                                objCabecera.Detalle.Add(objDetalle);
                                Secuencia++;
                            }

                            /****************************************************************************************************************
                            //agrega multidespacho
                            ****************************************************************************************************************/
                            if (Valida_MultiDespacho)
                            {
                                //consulta detalle de subitems
                                var Tarja = PasePuerta.Pase_WebCFS.ObtenerTarjaCFS_CERTIFICADOS(mrn, msn, hsn, _GKEY);
                                if (Tarja.Exitoso)
                                {
                                    var LinqDetTarja = (from Tbl in Tarja.Resultado.Where(Tbl => Tbl.CONSECUTIVO != 0)
                                                        select new
                                                        {
                                                            CONSECUTIVO = Tbl.CONSECUTIVO,
                                                            CARGA = objCabecera.NUMERO_CARGA,
                                                            CANTIDAD = Tbl.CANTIDAD,
                                                            MRN = mrn,
                                                            MSN = msn,
                                                            HSN = hsn,
                                                            P2D_ALTO = Tbl.P2D_ALTO == null ? 0 : Tbl.P2D_ALTO.Value,
                                                            P2D_ANCHO = Tbl.P2D_ANCHO == null ? 0 : Tbl.P2D_ANCHO.Value,
                                                            P2D_LARGO = Tbl.P2D_LARGO == null ? 0 : Tbl.P2D_LARGO.Value,
                                                            PESO = Tbl.PESO == null ? 0 : Tbl.PESO.Value,
                                                            P2D_VOLUMEN = Tbl.P2D_VOLUMEN == null ? 0 : Tbl.P2D_VOLUMEN.Value,
                                                            IMO = string.IsNullOrEmpty(Tbl.IMO) ? "NO APLICA" : Tbl.IMO.Trim(),
                                                            NUMERO_CERTIFICADO = string.IsNullOrEmpty(Tbl.NUMERO_CERTIFICADO) ? "NO APLICA" : Tbl.NUMERO_CERTIFICADO.Trim()
                                                        }).ToList().OrderBy(x => x.CONSECUTIVO);

                                    List<Int64> Lista = new List<Int64>();
                                    foreach (var Det in LinqDetTarja.Where(p => p.P2D_ALTO != 0 && p.P2D_ANCHO != 0 && p.P2D_LARGO != 0))
                                    {

                                        objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
                                        objPaseCFSTarja.CARGA = Det.CARGA;
                                        objPaseCFSTarja.MRN = Det.MRN;
                                        objPaseCFSTarja.MSN = Det.MSN;
                                        objPaseCFSTarja.HSN = Det.HSN;
                                        objPaseCFSTarja.CONSECUTIVO = Det.CONSECUTIVO;
                                        objPaseCFSTarja.CANTIDAD = Det.CANTIDAD.Value;
                                        objPaseCFSTarja.CIATRANS = string.Empty;
                                        objPaseCFSTarja.CHOFER = string.Empty;
                                        objPaseCFSTarja.ID_CIATRANS = string.Empty;
                                        objPaseCFSTarja.ID_CHOFER = string.Empty;
                                        objPaseCFSTarja.PLACA = string.Empty;
                                        objPaseCFSTarja.VISTO = false;
                                        objPaseCFSTarja.TRANSPORTISTA_DESC = string.Empty;
                                        objPaseCFSTarja.CHOFER_DESC = string.Empty;
                                        objPaseCFSTarja.ESTADO_PAGO = "NO";
                                        objPaseCFSTarja.MARCADO_SUBITEMS = string.Empty;

                                        objPaseCFSTarja.P2D_ALTO = Det.P2D_ALTO;
                                        objPaseCFSTarja.P2D_ANCHO = Det.P2D_ANCHO;
                                        objPaseCFSTarja.P2D_LARGO = Det.P2D_LARGO;
                                        objPaseCFSTarja.PESO = Det.PESO;
                                        objPaseCFSTarja.P2D_VOLUMEN = Det.P2D_VOLUMEN;
                                        objPaseCFSTarja.IMO = Det.IMO;
                                        objPaseCFSTarja.NUMERO_CERTIFICADO = Det.NUMERO_CERTIFICADO;

                                        Lista.Add(Det.CONSECUTIVO.Value);

                                        objCabecera.DetalleSubItem.Add(objPaseCFSTarja);


                                    }

                                }
                            }

                            /***************************************************************************************************************
                            //fin
                            ****************************************************************************************************************/


                            //total de bultos
                            var TotalBultos = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                            objCabecera.TOTAL_BULTOS = TotalBultos;

                            ////agrega a la grilla
                            //tablePagination.DataSource = objCabecera.Detalle;
                            //tablePagination.DataBind();


                            result.Cabecera = objCabecera;

                            return result;
                        }
                        else
                        {
                            result.TieneErrores = true;
                            return result;
                        }
                    }
                    else
                    {
                        result.TieneErrores = true;
                        return result;
                    }

                }
                else
                {
                    result.TieneErrores = true;
                    return result;
                }



            }
            catch (Exception ex)
            {
                result.TieneErrores = true;
                return result;
            }

        }

        public void cotizarProforma(string mrn, string msn, string hsn, string usuarioLogin, string usuarioRuc)
        {


        }
        private static string leyenda_proforma_p2d()
        {
            List<Cls_Bil_Configuraciones> Leyenda = Cls_Bil_Configuraciones.Parametros(out TextoLeyenda);
            if (!String.IsNullOrEmpty(TextoLeyenda))
            {
                return string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", TextoLeyenda);
            }

            var LinqLeyenda = (from Tope in Leyenda.Where(Tope => Tope.NOMBRE.Equals("TEXTO_P2D_FAC"))
                               select new
                               {
                                   VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                               }).FirstOrDefault();

            if (LinqLeyenda != null)
            {
                return LinqLeyenda.VALOR == null ? "" : LinqLeyenda.VALOR;
            }


            return "";
        }

    }
}
