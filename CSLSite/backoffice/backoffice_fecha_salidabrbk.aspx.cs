using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using ControlPagos.Importacion;
using Salesforces;
using System.Data;
using System.Net;
using SqlConexion;
using CasManual;

namespace CSLSite
{


    public partial class backoffice_fecha_salidabrbk : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_CasManual objDetalleCas = new Cls_Bil_CasManual();

        private Cls_Bil_Registra_Salida_BRBK Carga = new Cls_Bil_Registra_Salida_BRBK();
        private string cMensajes;

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
       
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;

        private string TipoServicio = string.Empty;

        private string LoginName = string.Empty;
        

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private static Int64? lm = -3;
        private string OError;

        private string HoraHasta = "00:00";
        private DateTime FechaActualSalida;
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

        private void Listado_Cargas()
        {
            try
            {
               
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                var Resultado = PagoAsignado.ListaAsignacion(ClsUsuario.loginname.Trim(), ClsUsuario.ruc.Trim(), null,null);
           
                if (Resultado != null)
                {

                    if (Resultado.Exitoso)
                    {
                        var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.mrn))
                                        select new
                                        {
                                            id_asignacion = Tbl.id_asignacion,
                                            carga = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                            ruc = Tbl.ruc.Trim(),
                                            nombre = Tbl.nombre.Trim(),
                                            fecha_asignado = Tbl.fecha_asignado.Value.ToString("dd/MM/yyyy HH:mm"),
                                            login_asigna = Tbl.login_asigna
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            //tablePagination.DataSource = LinqQuery;
                            //tablePagination.DataBind();
                        }
                        else
                        {
                            //tablePagination.DataSource = null;
                            //tablePagination.DataBind();
                        }
                    }
                    else
                    {
                        //tablePagination.DataSource = null;
                        //tablePagination.DataBind();
                    }
                    this.Actualiza_Paneles();
                  
                }
                else {
                    //tablePagination.DataSource = null;
                    //tablePagination.DataBind();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                return;
            }

        }

        private void Actualiza_Paneles()
        {
           
            this.UPCARGA.Update();
            this.UPDATOSCLIENTE.Update();
           

        }


        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
           
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
          
        }


        //private void Carga_Desconsolidadora()
        //{

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(this.TxtIdAgente.Text))
        //        {
        //            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
        //            //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
        //            var Forwarder = N4.Entidades.Forwarder.ObtenerForwarderPorCodigo(ClsUsuario.loginname, this.TxtIdAgente.Text.Trim());

        //            if (Forwarder.Exitoso)
        //            {
        //                var ListaForwarder = Forwarder.Resultado;
        //                if (ListaForwarder != null)
        //                {
        //                    this.TXTAGENCIA.Text = string.Format("{0}", ListaForwarder.CLNT_NAME.Trim());
        //                }
        //                else
        //                {
        //                    this.TXTAGENCIA.Text = string.Empty;
        //                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos de la desconsolidadora</b>"));
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                this.TXTAGENCIA.Text = string.Empty;
        //                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos de la desconsolidadora, {0}</b>", Forwarder.MensajeProblema));
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            this.TXTAGENCIA.Text = string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Desconsolidadora), "N4.Entidades.Forwarder.ObtenerForwarderPorCodigo", false, null, null, ex.StackTrace, ex);
        //        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
        //    }
        
        //}

       

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
            Session["CasManual" + this.hf_BrowserWindowName.Value] = objCas;
            Session["TransaccionContDet"] = objCas.Detalle_Cas;
        }

        #endregion



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);
              
                return;
            }

            this.banmsg.Visible = IsPostBack;
         

            if (!Page.IsPostBack)
            {

               
            
                this.banmsg.InnerText = string.Empty;

                ClsUsuario = Page.Tracker();
                
                if (ClsUsuario != null)
                {

                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                }

                this.TXTMRN.Text = string.Empty;
                this.TXTMSN.Text = string.Empty;
                if (string.IsNullOrEmpty(this.TXTHSN.Text))
                { this.TXTHSN.Text = string.Format("{0}", "0000"); }

            }

        }

  

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());
                Server.HtmlEncode(this.TxtFactura.Text.Trim());

                Server.HtmlEncode(this.TxtFechaCas.Text.Trim());

                if (!Page.IsPostBack)
                {     
                   this.Crear_Sesion();
                }
                else
                {
                   
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    if (ClsUsuario != null)
                    {

                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                    }

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

 
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            
            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MSN"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga HSN"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    Carga = new Cls_Bil_Registra_Salida_BRBK();
                    Carga.MRN = this.TXTMRN.Text.Trim().ToUpper();
                    Carga.MSN = this.TXTMSN.Text.Trim().ToUpper();
                    Carga.HSN = this.TXTHSN.Text.Trim().ToUpper();

                    if (!Carga.PopulateMyData(out cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existe información con el # de carga ingresado: {0}-{1}-{2}", this.TXTMRN.Text.Trim().ToUpper(), this.TXTMSN.Text.Trim().ToUpper(), this.TXTHSN.Text.Trim().ToUpper()));
                        return;
                    }
                    else
                    {
                        this.TxtFactura.Text = Carga.FACTURA;
                        this.TxtFechaCas.Text = Carga.FECHA_SALIDA.HasValue ? Carga.FECHA_SALIDA.Value.Date.ToString("MM/dd/yyyy") : System.DateTime.Now.Date.ToString("MM/dd/yyyy");

                        Ocultar_Mensaje();
                    }

                    this.Actualiza_Paneles();


                    
                }
                catch (Exception ex)
                {
                 
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "BtnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }    
        }

        protected void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                   
                    CultureInfo enUS = new CultureInfo("en-US");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    string IdDesConsolidadora = string.Empty;

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MSN"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga HSN"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFechaCas.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar la fecha de salida"));
                        this.TxtFechaCas.Focus();
                        return;
                    }

                    HoraHasta = "00:00";
                    Fecha = string.Format("{0} {1}", this.TxtFechaCas.Text.Trim(), HoraHasta);
                    string FechaNew = this.TxtFechaCas.Text.Trim();

                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                    {
                       
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Debe seleccionar una fecha valida de salida.. Mes/Día/año</b>"));
                        this.TxtFechaCas.Focus();
                        return;
                    }

                    if (FechaActualSalida.Date < System.DateTime.Now.Date)
                    {
                      
                        this.TxtFechaCas.Text = FechaNew;
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), System.DateTime.Now.ToString("MM/dd/yyyy")));
                        this.TxtFechaCas.Focus();
                        return;
                    }

                    string numero_carga = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim().ToUpper(), this.TXTMSN.Text.Trim().ToUpper(), this.TXTHSN.Text.Trim().ToUpper());

                    Carga = new Cls_Bil_Registra_Salida_BRBK();
                    Carga.MRN = this.TXTMRN.Text.Trim().ToUpper();
                    Carga.MSN = this.TXTMSN.Text.Trim().ToUpper();
                    Carga.HSN = this.TXTHSN.Text.Trim().ToUpper();
                    Carga.FACTURA = this.TxtFactura.Text.Trim();
                    Carga.FECHA_SALIDA = FechaActualSalida.Date;
                    Carga.IV_USUARIO_CREA = ClsUsuario.loginname;

                    var nProceso = Carga.Save(out cMensajes);
                    /*fin de nuevo proceso de grabado*/
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos de la carga...{0} </b>", cMensajes));
                        return;
                    }
                    else
                    {
                       
                    }

                    OcultarLoading("1");
                    OcultarLoading("2");

                    this.TXTMRN.Text = string.Empty;
                    this.TXTMSN.Text = string.Empty;
                    this.TXTHSN.Text = string.Empty;
                    this.TxtFactura.Text = string.Empty;
                    this.TxtFechaCas.Text = string.Empty;

                    this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Se procedió con el registro de fecha de salida de la carga {0}. Proceso realizado con éxito. </b>", numero_carga));
                    this.Actualiza_Paneles();
                    return;

                   
                }


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnActualizar_Click), "backoffice_descon_manual.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
   }
}