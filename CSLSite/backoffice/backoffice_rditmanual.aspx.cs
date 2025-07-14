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

namespace CSLSite.backoffice
{


    public partial class backoffice_rditmanual : System.Web.UI.Page
    {

        #region "Clases"


        usuario ClsUsuario;
       
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
            this.UPAGENTE.Update();

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


        private void Carga_Agente()
        {

            try
            {
                if (!string.IsNullOrEmpty(this.TxtIdAgente.Text))
                {
                   
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
                    var Agente = N4.Entidades.Agente.ObtenerAgente(ClsUsuario.loginname, this.TxtIdAgente.Text.Trim());
                    if (Agente.Exitoso)
                    {
                        var ListaAgente = Agente.Resultado;
                        if (ListaAgente != null)
                        {
                            this.TXTAGENCIA.Text = string.Format("{0}", ListaAgente.nombres.Trim());
                        }
                        else
                        {
                            this.TXTAGENCIA.Text = string.Empty;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente</b>"));
                            return;
                        }
                    }
                    else
                    {
                        this.TXTAGENCIA.Text = string.Empty;
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente, {0}</b>", Agente.MensajeProblema));
                        return;
                    }
                }
                else
                {
                    this.TXTAGENCIA.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Agente), "N4.Entidades.Agente.ObtenerAgente", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        
        }

        private void Carga_Importador()
        {

            try
            {
                if (!string.IsNullOrEmpty(this.TxtIdImportador.Text))
                {
                   
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, this.TxtIdImportador.Text.Trim());
                    if (Cliente.Exitoso)
                    {
                        var ListaCliente = Cliente.Resultado;
                        if (ListaCliente != null)
                        {
                            this.TxtDescImportador.Text = string.Format("{0}", ListaCliente.CLNT_NAME.Trim());
                        }
                        else
                        {
                            this.TxtDescImportador.Text = string.Empty;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente</b>"));
                            return;
                        }
                    }
                    else
                    {
                        this.TxtDescImportador.Text = string.Empty;
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del cliente, {0}</b>", Cliente.MensajeProblema));
                        return;
                    }
                }
                else
                {
                    this.TxtDescImportador.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Importador), "N4.Entidades.Cliente.ObtenerCliente", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

            
        }

        private void HandleCustomPostbackEvent(string ctrlName, string args)
        {
            try
            {
                this.Ocultar_Mensaje();

                if (ctrlName == TxtIdAgente.UniqueID && args == "OnBlur")
                {
                    this.Carga_Agente();
                    this.Carga_Importador();
                }
                if (ctrlName == TxtIdImportador.UniqueID && args == "OnBlur")
                {
                    this.Carga_Importador();
                    this.Carga_Agente();
                }
       

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                return;

            }
            
        }

        private void HandleCustomPostbackEvent2(string ctrlName, string args)
        {
            try
            {

                if (ctrlName == TxtIdImportador.UniqueID && args == "OnBlur")
                {
                    this.Carga_Importador();
                }
                if (ctrlName == TxtIdAgente.UniqueID && args == "OnBlur")
                {
                    this.Carga_Agente();
                }
               
                this.Ocultar_Mensaje();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                return;

            }

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

                var onBlurScript = Page.ClientScript.GetPostBackEventReference(TxtIdAgente, "OnBlur");
                TxtIdAgente.Attributes.Add("onblur", onBlurScript);

                var onBlurScript2 = Page.ClientScript.GetPostBackEventReference(TxtIdImportador, "OnBlur");
                TxtIdImportador.Attributes.Add("onblur", onBlurScript2);

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

                Server.HtmlEncode(this.TXTAGENCIA.Text.Trim());
                Server.HtmlEncode(this.TxtDescImportador.Text.Trim());

                if (!Page.IsPostBack)
                {

                    this.Listado_Cargas();

                }
                else
                {
                    var ctrlName = Request.Params[Page.postEventSourceID];
                    var args = Request.Params[Page.postEventArgumentID];

                    HandleCustomPostbackEvent(ctrlName, args);

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

 
        protected void BtnAsumir_Click(object sender, EventArgs e)
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
                    if (string.IsNullOrEmpty(this.TxtIdAgente.Text) || string.IsNullOrEmpty(this.TXTAGENCIA.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el código del Agente de Aduana"));
                        this.TxtIdAgente.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TxtIdImportador.Text) || string.IsNullOrEmpty(this.TxtDescImportador.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el Ruc del Importador"));
                        this.TxtIdImportador.Focus();
                        return;
                    }


                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    Aduanas.Entidades.AduanaRidt p = new Aduanas.Entidades.AduanaRidt();
                    p.mrn = this.TXTMRN.Text.Trim();
                    p.msn = this.TXTMSN.Text.Trim();
                    p.hsn = this.TXTHSN.Text.Trim();
                    p.id_agente = this.TxtIdAgente.Text.Trim();
                    p.id_importador = this.TxtIdImportador.Text.Trim();
                    p.nombre_importador = this.TxtDescImportador.Text.Trim();
                    p.usuario_registra = ClsUsuario.loginname.Trim();
                    p.comentarios = this.Txtcomentario.Text.Trim();
                    p.numero_declaracion = this.TxtNumeroDeclaracion.Text.Trim();

                    var Resultado = p.NuevoRegistro();
                    if (Resultado.Exitoso)
                    {

                        this.TxtIdImportador.Text = string.Empty;
                        this.TxtIdAgente.Text = string.Empty;
                        this.Txtcomentario.Text = string.Empty;
                        this.TXTAGENCIA.Text = string.Empty;
                        this.TxtDescImportador.Text = string.Empty;
                        this.TxtNumeroDeclaracion.Text = string.Empty;

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-info'></i><b> Informativo! </b>RIDT registrado con éxito...{0}", Resultado.MensajeInformacion));
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} , {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                        return;
                    }
              

                }
                catch (Exception ex)
                {
                 
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAsumir_Click), " Aduanas.Entidades.AduanaRidt", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }



            
        }


    }
}