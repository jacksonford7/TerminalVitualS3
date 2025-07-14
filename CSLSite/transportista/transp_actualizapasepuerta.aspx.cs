using System;
using BillionEntidades;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BillionReglasNegocio;
using System.Data;
using N4.Entidades;
using N4Ws.Entidad;
using CSLSite;
using System.IO;
using System.Web.Script.Services;
using PasePuerta;

namespace CSLSite.contenedorexpo
{
    public partial class transp_actualizapasepuerta : System.Web.UI.Page
    {

        #region "Clases"

        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private List<Cls_Bill_CabeceraExpo> objCabecera = new List<Cls_Bill_CabeceraExpo>();
        private Cls_Bill_Container_Expo objDetalle = new Cls_Bill_Container_Expo();
        private List<Cls_Bill_Container_Expo_Det_Validacion> objValidacionesDet = new List<Cls_Bill_Container_Expo_Det_Validacion>();

        private Cls_TRANSP_Cab_Documentos objDocumento = new Cls_TRANSP_Cab_Documentos();
        private Cls_TRANSP_Colaborador objDetalleColaborador = new Cls_TRANSP_Colaborador();
        private Cls_TRANSP_Vehiculo objDetalleVehiculo = new Cls_TRANSP_Vehiculo();

        private Cls_TRANSP_Doc_Colaborador objDocumentoColaborador = new Cls_TRANSP_Doc_Colaborador();
        private Cls_TRANSP_Doc_Vehiculo objDocumentosVehiculo = new Cls_TRANSP_Doc_Vehiculo();

        private Cls_TRANSP_Cab_Documentos objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();

        private static Int64? lm = -3;
        private string OError;

        private Cls_TRANSP_PasePuertaImpo objPaseImpo = new Cls_TRANSP_PasePuertaImpo();
        private Cls_TRANSP_PasePuertaImpo_Detalle objDetallePaseImpo = new Cls_TRANSP_PasePuertaImpo_Detalle();
        private Cls_TRANSP_PasePuertaExpo_Detalle objDetallePaseExpo = new Cls_TRANSP_PasePuertaExpo_Detalle();
        #endregion

        #region "Variables"
        private string cMensajes;
       

        private string ChoferSelect = string.Empty;
        private string PlacaSelect = string.Empty;

      

        #endregion

        #region "Propiedades"
        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }
        }
        #endregion

        #region "Metodos"
        public static string tipos(object tipo, object movi)
        {
            if (tipo == null || movi == null)
            {
                return "!error";
            }

            if (tipo.ToString().Trim().Length < 1 || movi.ToString().Trim().Length < 1)
            {
                return "!error";
            }

            if (movi.ToString().Trim() == "E")
            {
                if (tipo.ToString().Trim() == "C")
                {
                    return "Full";
                }
                else
                {
                    return "C. Suelta";
                }
            }
            else
            {
                return "Consolidación";
            }
        }

        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            if (estado.ToString().ToLower() == "r")
            {
                return "<span>Registrado</span>";
            }
            if (estado.ToString().ToLower() == "a")
            {
                return "<span class='red' >Anulado</span>";
            }
            if (estado.ToString().ToLower() == "i")
            {
                return "<span class='azul' >Ingresado</span>";
            }
            if (estado.ToString().ToLower() == "s")
            {
                return "<span class='naranja' >Salida</span>";
            }
            return "<span>sin estado!</span>";
        }


        private void Actualiza_Paneles()
        {
           
            UPDETALLE_TRANSP_IMPO.Update();
            UPBOTONES_TRANSP_IMPO.Update();
            UPDETALLE_IMPO.Update();
            UPDETALLE_EXPO.Update();

            EMPRESA_TRANSPORTE_IMPO.Update();
            UPTIPOCARGA.Update();
            UPNUMEROPASE.Update();
            UPCONTENEDOR.Update();
            UPNUMEROCARGA.Update();
            UPCHOFER_ANTERIOR.Update();
            UPPLACA_ANTERIOR.Update();

            EMPRESA_TRANSPORTE_EXPO.Update();
            UPCHOFER_ANTERIOR_EXPO.Update();
            UPPLACA_ANTERIOR_EXPO.Update();
            UPTIPOCARGA_EXPO.Update();
            UPNUMEROPASE_EXPO.Update();
            UPCONTENEDOR_EXPO.Update();
            UPNUMEROCARGA_EXPO.Update();
            UPDETALLE_TRANSP_EXPO.Update();
            UPBOTONES_TRANSP_EXPO.Update();
            UPDATOS_TRANSP_EXPO.Update();
        }

        private void Actualiza_Panele_Detalle()
        {
            //UPDETALLE.Update();
        }

        private void Limpia_Datos_cliente()
        {
            //this.TXTNAVE.Text = string.Empty;
        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje_Actualizar(int Tipo, string Mensaje)
        {
            if (Tipo == 1)
            {
                this.sinresultado.Visible = true;
                this.sinresultado.InnerHtml = Mensaje;

                OcultarLoading("1");

            }

            if (Tipo == 2)
            {
                this.sinresultado_expo.Visible = true;
                this.sinresultado_expo.InnerHtml = Mensaje;

                OcultarLoading("2");

            }


            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();


            objPaseImpo = new Cls_TRANSP_PasePuertaImpo();
            Session["Documento" + this.hf_BrowserWindowName.Value] = objPaseImpo;
          

    }

        protected string jsarguments(object CNTR_BKNG_BOOKING, object CNTR_ID)
        {
            return string.Format("{0};{1}", CNTR_BKNG_BOOKING != null ? CNTR_BKNG_BOOKING.ToString().Trim() : "0", CNTR_ID != null ? CNTR_ID.ToString().Trim() : "0");
        }

        #endregion

        #region "Metodos Web Services"
        [System.Web.Services.WebMethod]
        public static string[] GetChofer(string idempresa, string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                if (string.IsNullOrEmpty(idempresa))
                {
                    idempresa = ClsUsuario.ruc.Trim();
                }

                var Chofer = N4.Entidades.Chofer.ObtenerChoferes(ClsUsuario.loginname, String.Empty, idempresa);
                if (Chofer.Exitoso)
                {
                    var LinqQuery = (from Tbl in Chofer.Resultado.Where(Tbl => Tbl.numero != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0} - {1}", Tbl.numero.Trim(), Tbl.nombres.Trim()),
                                         RUC = Tbl.numero.Trim(),
                                         NOMBRE = Tbl.nombres.Trim(),
                                         ID = Tbl.numero.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        [System.Web.Services.WebMethod]
        public static string[] GetPlaca(string idempresa, string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                if (string.IsNullOrEmpty(idempresa)) 
                {
                    idempresa = ClsUsuario.ruc.Trim();
                }

               
                var Camion = N4.Entidades.Camion.ObtenerCamiones(ClsUsuario.loginname, prefix, idempresa);
                if (Camion.Exitoso)
                {
                    var LinqQuery = (from Tbl in Camion.Resultado.Where(Tbl => Tbl.numero != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0}", Tbl.numero.Trim()),
                                         RUC = Tbl.numero.Trim(),
                                         NOMBRE = Tbl.numero.Trim(),
                                         ID = Tbl.numero.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        #endregion

        #region "Form"
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }

                this.banmsg.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;

//#if !DEBUG
//                this.IsAllowAccess();
//#endif

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    ClsUsuario = Page.Tracker();

                    if (ClsUsuario != null)
                    {

                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.nombres);
                    }

                }

               
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode("");

                if (Response.IsClientConnected && !IsPostBack)
                {
                    sinresultado.Visible = false;
                    sinresultado_expo.Visible = false;
                }

                if (!Page.IsPostBack)
                {
                    if (Response.IsClientConnected)
                    {
                      
                        sinresultado.Visible = true;
                        sinresultado_expo.Visible = true;
                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Crear_Sesion();

                    objPaseImpo = new Cls_TRANSP_PasePuertaImpo();
                    Session["Documento" + this.hf_BrowserWindowName.Value] = objPaseImpo;

                 

                    this.Cargar_PasePuerta_Impo();

                    this.Cargar_PasePuerta_Expo();


                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

       




        


     

      

        #region "Grabar Solicitud"

        protected void BtnGenerar_Click(object sender, EventArgs e)
        {
            //if (Response.IsClientConnected)
            //{
            //    try
            //    {
            //        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

            //        objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;
            //        if (objDocumento == null)
            //        {
            //            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona o el vehículo a generar la solcitud. </b>"));
            //            return;
            //        }
            //        else
            //        {
            //            if (this.SsTipo.Equals("COLABORADOR"))
            //            {

            //                if (objDocumento.Documento_Colaborador.Count == 0)
            //                {
            //                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el colaborador, para poder generar la solicitud.</b>"));
            //                    return;
            //                }
            //                else
            //                {
            //                    //valida que tenga documentos cargados
            //                    var Existe = objDocumento.Documento_Colaborador.Where(f => !string.IsNullOrEmpty(f.RUTA_FINAL)).Count();
            //                    if (Existe == 0)
            //                    {
            //                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe cargar los documentos del colaborador, para poder generar la solicitud.</b>"));
            //                        return;
            //                    }
            //                }

            //                //proceso para grabar.
            //                var x = objDocumento.Colaborador.Where(p => p.NOMINA_COD == this.SsCodigo).FirstOrDefault();

            //                objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();
            //                objDocumentoGrabar.RUC_EMPRESA = x.RUC_EMPRESA;
            //                objDocumentoGrabar.NOMBRE_EMPRESA = x.NOMBRE_EMPRESA;
            //                objDocumentoGrabar.NOMINA_COD = x.NOMINA_COD;
            //                objDocumentoGrabar.COLABORADOR = x.COLABORADOR;
            //                objDocumentoGrabar.NOMBRES = x.NOMBRES;
            //                objDocumentoGrabar.ESTADO = x.ESTADO;
            //                objDocumentoGrabar.ESTADO2 = x.ESTADO2;
            //                objDocumentoGrabar.APELLIDOS = x.APELLIDOS;
            //                objDocumentoGrabar.TIPOSANGRE = x.TIPOSANGRE;
            //                objDocumentoGrabar.DIRECCIONDOM = x.DIRECCIONDOM;
            //                objDocumentoGrabar.TELFDOM = x.TELFDOM;
            //                objDocumentoGrabar.FECHANAC = x.FECHANAC;
            //                objDocumentoGrabar.CARGO = x.CARGO;
            //                objDocumentoGrabar.IV_USUARIO_CREA = ClsUsuario.loginname;
            //                objDocumentoGrabar.MAIL = ClsUsuario.email;

            //                foreach (var Det in objDocumento.Documento_Colaborador.Where(y => !string.IsNullOrEmpty(y.RUTA_FINAL)))
            //                {
            //                    objDocumentoColaborador = new Cls_TRANSP_Doc_Colaborador();
            //                    objDocumentoColaborador.ID_DOCUMENTO = Det.ID_DOCUMENTO;
            //                    objDocumentoColaborador.RUTA_FINAL = Det.RUTA_FINAL;
            //                    objDocumentoColaborador.IV_USUARIO_CREA = ClsUsuario.loginname;
            //                    objDocumentoGrabar.Documento_Colaborador.Add(objDocumentoColaborador);
            //                }

            //                var nIdRegistro = objDocumentoGrabar.SaveTransaction_Colaborador(out cMensajes);
            //                if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
            //                {
            //                    this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar la solicitud de actualización de documentos del colaborador: {0}, error: {0}</b>",x.COLABORADOR, cMensajes));

            //                    return;
            //                }
            //                else
            //                {
            //                    this.Ocultar_Mensaje();

            //                    this.Mostrar_Mensaje(string.Format("<b>Informativo! Se procedió a generar la solicitud # {0} de actualización de documentos del colaborador, con éxito</b>", nIdRegistro.Value));

            //                    this.BtnGenerar.Attributes["disabled"] = "disabled";
            //                    this.UPDET.Update();

            //                }

            //            }
            //            if (this.SsTipo.Equals("VEHICULOS"))
            //            {
            //                if (objDocumento.Documento_Vehiculo.Count == 0)
            //                {
            //                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el vehículo, para poder generar la solicitud.</b>"));
            //                    return;
            //                }
            //                else
            //                {
            //                    //valida que tenga documentos cargados
            //                    var Existe = objDocumento.Documento_Vehiculo.Where(f => !string.IsNullOrEmpty(f.RUTA_FINAL)).Count();
            //                    if (Existe == 0)
            //                    {
            //                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe cargar los documentos del vehículo, para poder generar la solicitud.</b>"));
            //                        return;
            //                    }
            //                }



            //                //proceso para grabar.
            //                var x = objDocumento.Vehiculo.Where(p => p.PLACA == this.SsCodigo).FirstOrDefault();

            //                objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();
            //                objDocumentoGrabar.RUC_EMPRESA = x.RUC_EMPRESA;
            //                objDocumentoGrabar.PLACA = x.PLACA;
            //                objDocumentoGrabar.CLASETIPO = x.CLASETIPO;
            //                objDocumentoGrabar.MARCA = x.MARCA;
            //                objDocumentoGrabar.MODELO = x.MODELO;
            //                objDocumentoGrabar.COLOR = x.COLOR;
            //                objDocumentoGrabar.TIPOCERTIFICADO = x.TIPOCERTIFICADO;
            //                objDocumentoGrabar.CERTIFICADO = x.CERTIFICADO;
            //                objDocumentoGrabar.CATEGORIA = x.CATEGORIA;
            //                objDocumentoGrabar.DESCRIPCIONCATEGORIA = x.DESCRIPCIONCATEGORIA;
            //                objDocumentoGrabar.FECHAPOLIZA = x.FECHAPOLIZA;
            //                objDocumentoGrabar.FECHAMTOP = x.FECHAMTOP;
            //                objDocumentoGrabar.TIPO = x.TIPO;

            //                objDocumentoGrabar.IV_USUARIO_CREA = ClsUsuario.loginname;
            //                objDocumentoGrabar.MAIL = ClsUsuario.email;

            //                foreach (var Det in objDocumento.Documento_Vehiculo.Where(y => !string.IsNullOrEmpty(y.RUTA_FINAL)))
            //                {
            //                    objDocumentosVehiculo = new Cls_TRANSP_Doc_Vehiculo();
            //                    objDocumentosVehiculo.ID_DOCUMENTO = Det.ID_DOCUMENTO;
            //                    objDocumentosVehiculo.RUTA_FINAL = Det.RUTA_FINAL;
            //                    objDocumentosVehiculo.IV_USUARIO_CREA = ClsUsuario.loginname;
            //                    objDocumentoGrabar.Documento_Vehiculo.Add(objDocumentosVehiculo);
            //                }

            //                var nIdRegistro = objDocumentoGrabar.SaveTransaction_Vehiculo(out cMensajes);
            //                if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
            //                {
            //                    this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar la solicitud de actualización de documentos de Vehículos: {0}, error: {0}</b>", x.PLACA, cMensajes));

            //                    return;
            //                }
            //                else
            //                {
            //                    this.Ocultar_Mensaje();

            //                    this.Mostrar_Mensaje(string.Format("<b>Informativo! Se procedió a generar la solicitud # {0} de actualización de documentos de Vehículos, con éxito</b>", nIdRegistro.Value));

            //                    this.BtnGenerar.Attributes["disabled"] = "disabled";
            //                    this.UPDET.Update();

            //                }
            //            }

            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGenerar_Click), "Generar Solicitud", false, null, null, ex.StackTrace, ex);
            //        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
            //        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));


            //    }
            //}
        }

        #endregion

        #region "Cargar Pase Puerta Importacion"

        private void Cargar_PasePuerta_Impo()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                grilla_importacion.DataSource = null;
                grilla_importacion.DataBind();

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                var TablaPase = Cls_TRANSP_PasePuertaImpo.Listado_Pases_Impo( ClsUsuario.ruc,  out cMensajes);

                if (!String.IsNullOrEmpty(cMensajes))
                {

                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener pase puerta impo...{0}</b>", cMensajes));
                    this.Actualiza_Paneles();
                    return;
                }

                if (TablaPase != null)
                {
                    var Transp = TablaPase.FirstOrDefault();
                    if (Transp != null)
                    {
                        this.TxtEmpresaTransporte.Text = string.Format("{0} - {1}", Transp.ID_EMPRESA, Transp.CIATRASNSP);
                        this.IdTxtempresa.Value = Transp.ID_EMPRESA;

                        objPaseImpo = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_PasePuertaImpo;
                        objPaseImpo.Detalle.Clear();

                        foreach (var Det in TablaPase)
                        {
                            objDetallePaseImpo = new Cls_TRANSP_PasePuertaImpo_Detalle();
                            objDetallePaseImpo.ID_PASE = Det.ID_PASE;
                            objDetallePaseImpo.NUMERO_CARGA = Det.NUMERO_CARGA;
                            objDetallePaseImpo.CONTENEDOR = Det.CONTENEDOR;
                            objDetallePaseImpo.ID_PPWEB = Det.ID_PPWEB;
                            objDetallePaseImpo.MRN = Det.MRN;
                            objDetallePaseImpo.HSN = Det.HSN;
                            objDetallePaseImpo.MSN = Det.MSN;
                            objDetallePaseImpo.CONDUCTOR = Det.CONDUCTOR;
                            objDetallePaseImpo.PLACA = Det.PLACA;
                            objDetallePaseImpo.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;
                            objDetallePaseImpo.FECHA_TURNO = Det.FECHA_TURNO;
                            objDetallePaseImpo.ID_EMPRESA = Det.ID_EMPRESA;
                            objDetallePaseImpo.CIATRASNSP = Det.CIATRASNSP;
                            objDetallePaseImpo.TURNO = Det.TURNO;
                            objDetallePaseImpo.TIPO_CARGA = Det.TIPO_CARGA;
                            objDetallePaseImpo.ID_CHOFER = Det.ID_CHOFER;
                            objPaseImpo.Detalle.Add(objDetallePaseImpo);
                        }

                        Session["Documento" + this.hf_BrowserWindowName.Value] = objPaseImpo;

                        EMPRESA_TRANSPORTE_IMPO.Update();
                    }

                    grilla_importacion.DataSource = TablaPase;
                    grilla_importacion.DataBind();
                }

                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
            }
        }

        #endregion

        #region "Cargar Pase Puerta Exportaciones"

        private void Cargar_PasePuerta_Expo()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                grilla_exportacion.DataSource = null;
                grilla_exportacion.DataBind();

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                var TablaPase = Cls_TRANSP_PasePuertaExpo_Detalle.Listado_Pases_Expo(ClsUsuario.ruc, out cMensajes);

                if (!String.IsNullOrEmpty(cMensajes))
                {

                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener pase puerta expo...{0}</b>", cMensajes));
                    this.Actualiza_Paneles();
                    return;
                }

                if (TablaPase != null)
                {
                    var Transp = TablaPase.FirstOrDefault();
                    if (Transp != null)
                    {

                        this.TxtEmpresaTransporteExpo.Text = string.Format("{0} - {1}", Transp.aisv_tran_ruc, Transp.aisv_tran_cia);

                        this.IdTxtempresaExpo.Value = Transp.aisv_tran_ruc;

                        objPaseImpo = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_PasePuertaImpo;
                        objPaseImpo.Detalle_Expo.Clear();

                        foreach (var Det in TablaPase)
                        {
                            objDetallePaseExpo = new Cls_TRANSP_PasePuertaExpo_Detalle();

                            objDetallePaseExpo = new Cls_TRANSP_PasePuertaExpo_Detalle();
                            objDetallePaseExpo.aisv_fecha_ing = Det.aisv_fecha_ing;
                            objDetallePaseExpo.item = Det.item;
                            objDetallePaseExpo.aisv = Det.aisv;
                            objDetallePaseExpo.tipo = Det.tipo;
                            objDetallePaseExpo.movi = Det.movi;
                            objDetallePaseExpo.boking = Det.boking;
                            objDetallePaseExpo.referencia = Det.referencia;
                            objDetallePaseExpo.fk = Det.fk;
                            objDetallePaseExpo.agencia = Det.agencia;
                            objDetallePaseExpo.pod = Det.pod;
                            objDetallePaseExpo.dae = Det.dae;
                            objDetallePaseExpo.carga = Det.carga;
                            objDetallePaseExpo.fecha = Det.fecha;
                            objDetallePaseExpo.estado = Det.estado;
                            objDetallePaseExpo.cntr = Det.cntr;
                            objDetallePaseExpo.aisv_fecha_llegada_turno = Det.aisv_fecha_llegada_turno;
                            objDetallePaseExpo.aisv_cedul_chof = Det.aisv_cedul_chof;
                            objDetallePaseExpo.aisv_nombr_chof = Det.aisv_nombr_chof;
                            objDetallePaseExpo.aisv_placa_vehi = Det.aisv_placa_vehi;
                            objDetallePaseExpo.aisv_tran_cia = Det.aisv_tran_cia;
                            objDetallePaseExpo.aisv_tran_ruc = Det.aisv_tran_ruc;
                            objDetallePaseExpo.FECHA_VALIDA = Det.FECHA_VALIDA;
                            objDetallePaseExpo.turno = Det.turno;
                            objDetallePaseExpo.chofer = Det.chofer;
                            objPaseImpo.Detalle_Expo.Add(objDetallePaseExpo);
                        }

                        Session["Documento" + this.hf_BrowserWindowName.Value] = objPaseImpo;

                        EMPRESA_TRANSPORTE_EXPO.Update();
                    }

                    grilla_exportacion.DataSource = TablaPase;
                    grilla_exportacion.DataBind();
                }

                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
            }
        }

        #endregion

        #region "Actualizar Datos Transportista"
        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtEmpresaTransporte.Text))
                    {
                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Informativo! Debe seleccionar la Compañía de Transporte para poder agregar la información </b>"));
                        this.TxtEmpresaTransporte.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtChofer.Text) && string.IsNullOrEmpty(TxtPlaca.Text))
                    {
                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Informativo! Debe ingresar el chofer o la placa a realizar la actualización.</b>"));
                        this.TxtChofer.Focus();
                        return;
                    }


                    string IdChofer = string.Empty;
                    string DesChofer = string.Empty;

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //valida que exista un chofer
                    if (!string.IsNullOrEmpty(TxtChofer.Text))
                    {
                        ChoferSelect = this.TxtChofer.Text.Trim();
                        if (ChoferSelect.Split('-').ToList().Count > 1)
                        {
                            IdChofer = ChoferSelect.Split('-').ToList()[0].Trim();
                            DesChofer = ChoferSelect.Split('-').ToList()[1].Trim();

                            var ChoferTransporte = N4.Entidades.Chofer.ObtenerChofer(ClsUsuario.loginname, IdChofer);
                            if (!ChoferTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                                this.TxtChofer.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                            this.TxtChofer.Focus();
                            return;
                        }
                    }


                    //valida que exista una placa
                    if (!string.IsNullOrEmpty(TxtPlaca.Text))
                    {
                        PlacaSelect = this.TxtPlaca.Text.Trim();
                        if (PlacaSelect.Length > 1)
                        {
                            string IdPlaca = PlacaSelect.Trim();
                            var PlacaTransporte = N4.Entidades.Camion.ObtenerCamion(ClsUsuario.loginname, IdPlaca);
                            if (!PlacaTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                                this.TxtPlaca.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                            this.TxtPlaca.Focus();
                            return;
                        }
                    }

                    Int64 PASE = 0;
                    if (!Int64.TryParse(this.TxtPase.Text, out PASE))
                    {
                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "# pase null"));
                        return;
                    }

                    Pase_Transportista pase = new Pase_Transportista();
                    pase.ID_PASE = PASE;
                    pase.ID_PLACA = (string.IsNullOrEmpty(TxtPlaca.Text.Trim()) ? null : TxtPlaca.Text.Trim());
                    pase.ID_CHOFER = (string.IsNullOrEmpty(IdChofer) ? null : IdChofer);
                    pase.ID_EMPRESA = this.IdTxtempresa.Value;
                    pase.CHOFER_DESC = (string.IsNullOrEmpty(DesChofer) ? null : DesChofer);

                    var Resultado = pase.Actualizar_Transportista(ClsUsuario.loginname);

                    if (Resultado.Exitoso)
                    {
                        BtnAgregar.Attributes["disabled"] = "disabled";

                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Informativo! Se procedió  con la actualización de pase de puerta  {0}  con éxito.</b>", PASE));

                        objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();
                        objDocumentoGrabar.TIPO = "PASE";
                        objDocumentoGrabar.PASE = PASE;
                        objDocumentoGrabar.AISV = "";
                        objDocumentoGrabar.EMPRESA = this.TxtEmpresaTransporte.Text.Trim();
                        objDocumentoGrabar.CHOFER = (string.IsNullOrEmpty(IdChofer) ? TxtChoferAnterior.Text : string.Format("{0} - {1}", IdChofer, DesChofer));
                        objDocumentoGrabar.PLACA = (string.IsNullOrEmpty(TxtPlaca.Text.Trim()) ? TxtPlacaAnterior.Text : TxtPlaca.Text.Trim());
                        objDocumentoGrabar.MAIL = ClsUsuario.email;
                        objDocumentoGrabar.NUMERO_CARGA = this.TxtNumeroCarga.Text;
                        objDocumentoGrabar.USUARIOING = ClsUsuario.loginname;

                        string cError = string.Empty;
                        var Mail = objDocumentoGrabar.Enviar_Mail_PasePuerta(out cError);
                        if (!Mail.HasValue)
                        {

                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo actualizar el pase de puerta para la carga: {0}, pase # {1} Existen los siguientes problemas: {2}, {3} </b>", 
                            this.TxtNumeroCarga.Text, PASE, Resultado.MensajeInformacion, Resultado.MensajeProblema));
                        return;
                    }

                    Cargar_PasePuerta_Impo();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
                }
            }
        }

        protected void BtnAgregarExpo_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtEmpresaTransporteExpo.Text))
                    {
                        this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Informativo! Debe seleccionar la Compañía de Transporte para poder agregar la información </b>"));
                        this.TxtEmpresaTransporteExpo.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtChoferExpo.Text) && string.IsNullOrEmpty(TxtPlacaExpo.Text))
                    {
                        this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Informativo! Debe ingresar el chofer o la placa a realizar la actualización.</b>"));
                        this.TxtChoferExpo.Focus();
                        return;
                    }


                    string IdChofer = string.Empty;
                    string DesChofer = string.Empty;

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //valida que exista un chofer
                    if (!string.IsNullOrEmpty(TxtChoferExpo.Text))
                    {
                        ChoferSelect = this.TxtChoferExpo.Text.Trim();
                        if (ChoferSelect.Split('-').ToList().Count > 1)
                        {
                            IdChofer = ChoferSelect.Split('-').ToList()[0].Trim();
                            DesChofer = ChoferSelect.Split('-').ToList()[1].Trim();

                            var ChoferTransporte = N4.Entidades.Chofer.ObtenerChofer(ClsUsuario.loginname, IdChofer);
                            if (!ChoferTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                                this.TxtChoferExpo.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                            this.TxtChoferExpo.Focus();
                            return;
                        }
                    }


                    //valida que exista una placa
                    if (!string.IsNullOrEmpty(TxtPlacaExpo.Text))
                    {
                        PlacaSelect = this.TxtPlacaExpo.Text.Trim();
                        if (PlacaSelect.Length > 1)
                        {
                            string IdPlaca = PlacaSelect.Trim();
                            var PlacaTransporte = N4.Entidades.Camion.ObtenerCamion(ClsUsuario.loginname, IdPlaca);
                            if (!PlacaTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                                this.TxtPlacaExpo.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                            this.TxtPlacaExpo.Focus();
                            return;
                        }
                    }

                

                    Pase_Transportista pase = new Pase_Transportista();
                    pase.aisv = this.TxtPaseExpo.Text.Trim();
                    pase.ID_PLACA = (string.IsNullOrEmpty(TxtPlacaExpo.Text.Trim()) ? null : TxtPlacaExpo.Text.Trim());
                    pase.ID_CHOFER = (string.IsNullOrEmpty(IdChofer) ? null : IdChofer);
                    pase.ID_EMPRESA = this.IdTxtempresaExpo.Value;
                    pase.CHOFER_DESC = (string.IsNullOrEmpty(DesChofer) ? null : DesChofer);

                    var Resultado = pase.Actualizar_Transportista_Expo(ClsUsuario.loginname);

                    if (Resultado.Exitoso)
                    {
                        BtnAgregarExpo.Attributes["disabled"] = "disabled";

                        this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Informativo! Se procedió  con la actualización de pase de puerta (AISV)  {0}  con éxito.</b>", this.TxtPaseExpo.Text.Trim()));

                        objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();
                        objDocumentoGrabar.TIPO = "AISV";
                        objDocumentoGrabar.PASE = 0;
                        objDocumentoGrabar.AISV = this.TxtPaseExpo.Text.Trim();
                        objDocumentoGrabar.EMPRESA = this.TxtEmpresaTransporteExpo.Text.Trim();
                        objDocumentoGrabar.CHOFER = (string.IsNullOrEmpty(IdChofer) ? TxtChoferAnteriorExpo.Text : string.Format("{0} - {1}", IdChofer, DesChofer));
                        objDocumentoGrabar.PLACA = (string.IsNullOrEmpty(TxtPlacaExpo.Text.Trim()) ? TxtPlacaAnteriorExpo.Text : TxtPlacaExpo.Text.Trim());
                        objDocumentoGrabar.MAIL = ClsUsuario.email;
                        objDocumentoGrabar.NUMERO_CARGA = this.TxtNumeroCargaExpo.Text;
                        objDocumentoGrabar.USUARIOING = ClsUsuario.loginname;

                        string cError = string.Empty;
                        var Mail = objDocumentoGrabar.Enviar_Mail_PasePuerta(out cError);
                        if (!Mail.HasValue)
                        {

                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje_Actualizar(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo actualizar el pase de puerta AISV: {0} Existen los siguientes problemas: {1}, {2} </b>",
                           this.TxtPaseExpo.Text.Trim(), Resultado.MensajeInformacion, Resultado.MensajeProblema));
                        return;
                    }

                    this.Cargar_PasePuerta_Expo();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje_Actualizar(2,string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
                }
            }
        }
        #endregion

        #region "Grilla Transportistas de Importacion"
        protected void grilla_importacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    Int64 PASE = 0;
                    if (!Int64.TryParse(t, out PASE))
                    {
                        this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;
                    }

                    objPaseImpo = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_PasePuertaImpo;
                    var Detalle = objPaseImpo.Detalle.FirstOrDefault(f => f.ID_PASE == PASE);
                    if (Detalle != null)
                    {
                        this.TxtTipoCarga.Text = Detalle.TIPO_CARGA;
                        this.TxtNumeroPase.Text = Detalle.ID_PASE.ToString();
                        this.TxtNumeroContenedor.Text = Detalle.CONTENEDOR;
                        this.TxtNumeroCarga.Text = Detalle.NUMERO_CARGA;
                        this.TxtChoferAnterior.Text = !string.IsNullOrEmpty(Detalle.ID_CHOFER) ? string.Format("{0} - {1}", Detalle.ID_CHOFER, Detalle.CONDUCTOR) : "";
                        this.TxtPlacaAnterior.Text = Detalle.PLACA;
                        this.TxtPase.Text = Detalle.ID_PASE.ToString();

                    }

                    this.sinresultado.InnerHtml = string.Empty;
                    this.BtnAgregar.Attributes.Remove("disabled");

                    this.Actualiza_Paneles();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
                }

            }
        }
        #endregion

        #region "Grilla Transportistas de Exportacion"
        protected void grilla_exportacion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    return;
                }

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                if (e.CommandArgument == null)
                {
                    this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                    return;
                }

                var t = e.CommandArgument.ToString();
                if (String.IsNullOrEmpty(t))
                {
                    this.Mostrar_Mensaje_Actualizar(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                    return;

                }

                objPaseImpo = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_PasePuertaImpo;
                var Detalle = objPaseImpo.Detalle_Expo.FirstOrDefault(f => f.aisv == t);
                if (Detalle != null)
                {
                    this.TxtTipoCargaExpo.Text = tipos(Detalle.tipo, Detalle.movi);
                    this.TxtNumeroPaseExpo.Text = Detalle.aisv;
                    this.TxtNumeroContenedorExpo.Text = Detalle.boking;
                    this.TxtNumeroCargaExpo.Text = Detalle.carga;
                    this.TxtChoferAnteriorExpo.Text = !string.IsNullOrEmpty(Detalle.aisv_nombr_chof) ? string.Format("{0} - {1}", Detalle.aisv_cedul_chof, Detalle.aisv_nombr_chof) : "";
                    this.TxtPlacaAnteriorExpo.Text = Detalle.aisv_placa_vehi;
                    this.TxtPaseExpo.Text = Detalle.aisv;

                }

                this.sinresultado_expo.InnerHtml = string.Empty;
                this.BtnAgregarExpo.Attributes.Remove("disabled");

                this.Actualiza_Paneles();

            }
        }
        #endregion




    }
}