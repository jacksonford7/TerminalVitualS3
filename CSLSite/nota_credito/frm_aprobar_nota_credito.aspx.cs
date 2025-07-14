using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
//using CSLSite.N4Object;
//using ConectorN4;
using System.Globalization;
using ControlOPC.Entidades;
using ClsNotasCreditos;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Web.Script.Services;
using csl_log;
using System.Data;

using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Collections;

//using CSLSite.N4Object;
using ConectorBilling;

//using ConectorN4;

namespace CSLSite
{
    public partial class frm_aprobar_nota_credito : System.Web.UI.Page
    {
        //AntiXRCFG
        private credit_head objcredit_head = new credit_head();

        private string pAccion
        {
            get
            {
                return (string)Session["pAccion"];
            }
            set
            {
                Session["pAccion"] = value;
            }

        }

        private Int64 pId
        {
            get
            {
                return (Int64)Session["pId"];
            }
            set
            {
                Session["pId"] = value;
            }

        }

        private Int64 pid_level
        {
            get
            {
                return (Int64)Session["pid_level"];
            }
            set
            {
                Session["pid_level"] = value;
            }

        }

        private Int64 pid_group
        {
            get
            {
                return (Int64)Session["pid_group"];
            }
            set
            {
                Session["pid_group"] = value;
            }

        }

        private Int32 pIdUsuario
        {
            get
            {
                return (Int32)Session["pIdUsuario"];
            }
            set
            {
                Session["pIdUsuario"] = value;
            }

        }

        private Int16 plevel
        {
            get
            {
                return (Int16)Session["plevel"];
            }
            set
            {
                Session["plevel"] = value;
            }

        }

        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        private String cRutaArchivo = string.Empty;

        private string xmlDocumentos
        {
            get
            {
                return (string)Session["xmlDocumentos"];
            }
            set
            {
                Session["xmlDocumentos"] = value;
            }

        }

        private void Carga_ListadoConceptos()
        {
            try
            {

                List<concepts> ListConceptos = concepts.ListConceptosGeneral();
                if (ListConceptos != null)
                {
                    this.CboConcepto.DataSource = ListConceptos;
                    this.CboConcepto.DataBind();
                }
                else
                {
                    this.CboConcepto.DataSource = null;
                    this.CboConcepto.DataBind();
                }



            }
            catch (Exception ex)
            {
                //sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "CboConcepto", "Carga_ListadoConceptos", "Hubo un error al cargar conceptos", user2 != null ? user2.loginname : "Nologin"));
                //this.Alerta(sg);
                //return;
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_listado_res_nota_credito", "Carga_ListadoConceptos", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;



            }

        }


        private void Limpiar()
        {
            this.pAccion = "N";
            this.pId = 0;
            this.pIdUsuario = 0;
            this.pid_group = 0;
            this.pid_level = 0;
            this.plevel = 0;
            xmlDocumentos = null;
            this.TxtArchivo.Text = null;

        }

       


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //this.IsAllowAccess();
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
                this.TxtNumero.Text =Server.HtmlEncode(this.TxtNumero.Text);
                this.desded.Text = Server.HtmlEncode(this.desded.Text);
                this.desded.Text = Server.HtmlEncode(this.hastad.Text);
                this._nc_id.Value = Server.HtmlEncode(this._nc_id.Value);
                this.TxtMotivo.Text = Server.HtmlEncode(this.TxtMotivo.Text);

            //Session["objeto"] = this.fsuploadarchivo;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;
            }

            if (!IsPostBack)
            {
                /*oculta paneles de motivo de anulacion*/
                this.botones.Visible = false;
                this.motivo.Visible = false;
                this.BtnAgregar.Visible = false;
                this.LblRuta.Visible = false;
                this.TxtArchivo.Enabled = false;
                this.Limpiar();

                this.Carga_ListadoConceptos();
                this.CboConcepto.SelectedIndex = 0;

                populate();
               
            }
            
        }

       /* protected void btnTransmitir_Click(object sender, EventArgs e)
        {

           
            var webService = new wsN4Billing();
            var strinb = new StringBuilder();
            string oerror = string.Empty;
            string estado_transaccion = string.Empty;   
            string numero_transaccion = string.Empty;
            var ss = new StringBuilder();

            var user = Page.getUserBySesion();
            var userk = new ObjectSesion2();

            strinb.Append("<ol style='margin:0;padding:0;' >");
            bool huboerror = false;


            userk.clase = "APROBAR";
            userk.metodo = "btnTransmitir_Click";
            userk.usuario = user.loginname;
            userk.token = HttpContext.Current.Request.Cookies["token"].Value;
            String CXML = "<custom class='CGSACreditArgoWsAPI' type='extension'><cgsa>";
            CXML = CXML + "<credit action = 'CREATE' create-type = 'PARTIAL' date = '2019-27-09' customer-id = '1791297385001' customer-role = 'SHIPPER' credit-type ='OAC' currency-id = 'usd' inv-final-nbr = '1018000396436'>";
            CXML = CXML + "<item inv-item-gkey = '31332734' credit-qty = '1' />";
            CXML = CXML + "</credit>";
            CXML = CXML + "</cgsa>";
            CXML = CXML + "</custom>";
    
            if (webService.InvokeN4Service(userk, CXML, ref oerror, "1", ref estado_transaccion, ref numero_transaccion) > 1)
            {
                strinb.AppendFormat("<li>{0}</li>", oerror);
                ss.AppendLine(oerror);
                huboerror = true;
            }
            else
            {
                if (estado_transaccion == "ERROR")
                {
                    strinb.AppendFormat("<li>{0}</li>", oerror);
                    ss.AppendLine(oerror);
                    huboerror = true;
                }
                else
                {
                    CXML = "<custom class='CGSACreditArgoWsAPI' type='extension'><cgsa>";
                    CXML = CXML + "<credit action='FINALIZE' draft-nbr='" + numero_transaccion.Trim() + "' finalized-date='2019-27-09'/>";
                    CXML = CXML + "</cgsa></custom>";
                    if (webService.InvokeN4Service(userk, CXML, ref oerror, "1", ref estado_transaccion, ref numero_transaccion) > 1)
                    {
                        strinb.AppendFormat("<li>{0}</li>", oerror);
                        ss.AppendLine(oerror);
                        huboerror = true;
                    }
                    else
                    {
                        if (estado_transaccion == "ERROR")
                        {
                            strinb.AppendFormat("<li>{0}</li>", oerror);
                            ss.AppendLine(oerror);
                            huboerror = true;
                        }
                        else
                        {
                            oerror = string.Format("Se genero la siguiente Nota de Crédito en N4 Billing #: {0}", numero_transaccion);
                            strinb.AppendFormat("<li>{0}</li>", oerror);
                            ss.AppendLine(oerror);
                        }
                    }
                }
            }


            strinb.Append("</ol>");
            if (huboerror)
            {
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerHtml = string.Format("<span>{0}</span><br/>{1}", "Los comandos fueron ejecutados, pero hubieron los siguientes problemas:", strinb.ToString());
                sinresultado.Visible = true;
                return;
            }
            else
            {
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-info";
                sinresultado.InnerHtml = string.Format("<span>{0}</span><br/>{1}", "Los comandos fueron ejecutados sin problemas:", strinb.ToString());
                sinresultado.Visible = true;
                return;

            }

        }
        */

        protected void btbuscar_Click(object sender, EventArgs e)
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
                        return;
                    }
                    this.botones.Visible = false;
                    this.motivo.Visible = false;
                    this.UdBotones.Update();

                    this.Limpiar();

                    populate();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }


        protected void btnprocesar_Click(object sender, EventArgs e)
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
                        return;
                    }

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "btnprocesar_Click", "btnprocesar_Click", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        this.botones.Visible = false;
                        this.motivo.Visible = false;
                        this._nc_id.Value = string.Empty;
                        return;
                    }

                    /*SI LA ACCION A EJECUTAR ES ANULAR LA TRANSACCION*/
                    if (this.pAccion =="I")
                    {
                        if (this.TxtMotivo.Text.Trim() == string.Empty)
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Ingrese el motivo de la anulación");
                            sinresultado.Visible = true;
                            return;
                        }

                        if (this.pId == 0 )
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("ID NO CONVERTIBLE"), "btnprocesar_Click", "btnprocesar_Click", this.pId.ToString(), user.loginname));
                            sinresultado.Visible = true;
                            this.botones.Visible = false;
                            this.motivo.Visible = false;
                            this._nc_id.Value = string.Empty;
                            return;

                        }

                        /*anulacion*/
                        if (this.pId != 0)
                        {

                            objcredit_head = new credit_head();
                            objcredit_head.nc_id = this.pId;
                            objcredit_head.nc_anulacion = this.TxtMotivo.Text.Trim();
                            objcredit_head.Create_user = user.loginname;

                            if (objcredit_head.Anular())
                            {

                                sinresultado.Attributes["class"] = string.Empty;
                                sinresultado.Attributes["class"] = "msg-info";
                                sinresultado.InnerText = string.Format("Anulación de nota de crédito No. Interno {0} ,realizada con éxito.", this.pId);
                                sinresultado.Visible = true;
                                this.botones.Visible = false;
                                this.motivo.Visible = false;
                                this._nc_id.Value = string.Empty;
                                this.TxtMotivo.Text = string.Empty;
                                this.Limpiar();

                            }
                            else
                            {
                                sinresultado.Attributes["class"] = string.Empty;
                                sinresultado.Attributes["class"] = "msg-critico";
                                sinresultado.InnerText = string.Format("ERROR AL realizar anulación de nota de crédito No. Interno {0}", this.pId);
                                sinresultado.Visible = true;
                                this.botones.Visible = false;
                                this.motivo.Visible = false;
                                this._nc_id.Value = string.Empty;
                                this.Limpiar();
                                return;

                            }
                        }
                        else
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("No existe ID interno nota de crédito, volver a intentar");
                            sinresultado.Visible = true;
                            this.botones.Visible = false;
                            this.motivo.Visible = false;
                            this._nc_id.Value = string.Empty;
                            this.Limpiar();
                            return;
                        }

                    }
                    /*SI LA ACCION A EJECUTAR ES PROCESAR U APROBAR LA TRANSACCION*/
                    if (this.pAccion == "P")
                    {
                        objcredit_head = new credit_head();
                        objcredit_head.nc_id = this.pId;
                        objcredit_head.id_level = this.pid_level;
                        objcredit_head.id_group = this.pid_group;
                        objcredit_head.IdUsuario = this.pIdUsuario ;
                        objcredit_head.level = this.plevel  ;
                        objcredit_head.Create_user = user.loginname;
                        if (this.xmlDocumentos == null)
                        {
                            objcredit_head.ruta_documento = string.Empty;
                        }
                        else {
                            objcredit_head.ruta_documento = this.xmlDocumentos;

                        }

                        if (objcredit_head.Aprobar())
                        {

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            sinresultado.InnerText = string.Format("Aprobación de nota de crédito No. Interno {0} ,realizada con éxito.", this.pId.ToString());
                            sinresultado.Visible = true;
                            this.botones.Visible = false;
                            this.motivo.Visible = false;
                            this._nc_id.Value = string.Empty;
                            this.TxtMotivo.Text = string.Empty;

                            //valida si es la ultima aprobacion, para poder generar el xml a transmitir
                            string _Es_final = objcredit_head.Validate_credit_head_Pendiente(this.pId);
                            string _XML = string.Empty;
                            string _XML_Grabar = string.Empty;

                            if (_Es_final.Trim() == string.Empty)

                            {   //XML para transmitir por primera vez la nota de credito
                                _XML = objcredit_head.Genera_XML_credit(this.pId,"DRAFT","");
                                if (_XML.Trim() == "ERROR")
                                {
                                    sinresultado.InnerText = sinresultado.InnerText + string.Format("\n\r Error al generar XML, por favor notificar a Dpto. Sistemas [Nota crédito ]: {0} ..Se reverso la aprobación", this.pId.ToString());
                                    sinresultado.Visible = true;

                                    if (!objcredit_head.Reverso())
                                    {
                                        sinresultado.InnerText = sinresultado.InnerText + string.Format("\n\r Error al reversar estado de aprobación [Nota crédito ]: {0}", this.pId.ToString());
                                    }

                                    return;
                                }
                                else
                                {
                                    var webService = new wsN4Billing();
                                    var strinb = new StringBuilder();
                                    string oerror = string.Empty;
                                    string estado_transaccion = string.Empty;
                                    string numero_transaccion = string.Empty;
                                    var ss = new StringBuilder();
                                    var userk = new ObjectSesion2();
                                    bool huboerror = false;

                                    strinb.Append("<ol style='margin:0;padding:0;' >");       
                                    userk.clase = "APROBAR";
                                    userk.metodo = "btnprocesar_Click";
                                    userk.usuario = user.loginname;
                                    userk.token = HttpContext.Current.Request.Cookies["token"].Value;

                                    /*genera el draft*/
                                    if (webService.InvokeN4Service(userk, _XML.Trim(), ref oerror, this.pId.ToString(), ref estado_transaccion, ref numero_transaccion, ref _XML_Grabar) > 1)
                                    {
                                        strinb.AppendFormat("<li>{0}</li>", oerror);
                                        ss.AppendLine(oerror);
                                        huboerror = true;

                                        if (_XML_Grabar != string.Empty)
                                        {
                                            objcredit_head.tipo = "D";
                                            objcredit_head.nc_xml_draft = _XML_Grabar;
                                            objcredit_head.nc_number = "";
                                            if (objcredit_head.Actualizar_XML())
                                            {

                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (estado_transaccion == "ERROR")
                                        {
                                            strinb.AppendFormat("<li>{0}</li>", oerror);
                                            ss.AppendLine(oerror);
                                            huboerror = true;
                                        }
                                        else
                                        {
                                            if (_XML_Grabar != string.Empty)
                                            {
                                                objcredit_head.tipo = "D";
                                                objcredit_head.nc_xml_draft = _XML_Grabar;
                                                objcredit_head.nc_number = "";
                                                if (objcredit_head.Actualizar_XML())
                                                {

                                                }
                                            }

                                            //XML para finalizar la nota de credito
                                            _XML = objcredit_head.Genera_XML_credit(this.pId, "FINALIZAR", numero_transaccion);

                                            if (webService.InvokeN4Service(userk, _XML, ref oerror, this.pId.ToString(), ref estado_transaccion, ref numero_transaccion, ref _XML_Grabar) > 1)
                                            {
                                                strinb.AppendFormat("<li>{0}</li>", oerror);
                                                ss.AppendLine(oerror);
                                                huboerror = true;

                                                objcredit_head.tipo = "F";
                                                objcredit_head.nc_xml_draft = _XML_Grabar;
                                                objcredit_head.nc_number = "";
                                                if (objcredit_head.Actualizar_XML())
                                                {

                                                }
                                            }
                                            else
                                            {
                                                if (estado_transaccion == "ERROR")
                                                {
                                                    strinb.AppendFormat("<li>{0}</li>", oerror);
                                                    ss.AppendLine(oerror);
                                                    huboerror = true;
                                                }
                                                else
                                                {
                                                    objcredit_head.tipo = "F";
                                                    objcredit_head.nc_xml_draft = _XML_Grabar;
                                                    objcredit_head.nc_number = numero_transaccion;
                                                    if (objcredit_head.Actualizar_XML())
                                                    {

                                                    }
                                                    oerror = string.Format("Se genero la siguiente Nota de Crédito en N4 Billing #: {0}", numero_transaccion);
                                                    strinb.AppendFormat("<li>{0}</li>", oerror);
                                                    ss.AppendLine(oerror);
                                                }
                                            }

                                        }
                                    }

                                    strinb.Append("</ol>");

                                    if (huboerror)
                                    {
                                        sinresultado.Attributes["class"] = string.Empty;
                                        sinresultado.Attributes["class"] = "msg-critico";
                                        string msg_anterior = sinresultado.InnerText;
                                        sinresultado.InnerHtml = string.Format("{0} <span>{1}</span><br/>{2}", msg_anterior, ".....La transmisión a N4 Billing fue ejecuta, pero hubieron los siguientes problemas:", strinb.ToString());
                                        sinresultado.Visible = true;

                                        if (!objcredit_head.Reverso())
                                        {
                                            sinresultado.InnerText = sinresultado.InnerText + string.Format("\n\r Error al reversar estado de aprobación [Nota crédito ]: {0}", this.pId.ToString());
                                        }

                                        return;
                                    }
                                    else
                                    {
                                        string msg_anterior = sinresultado.InnerText;
                                        sinresultado.Attributes["class"] = string.Empty;
                                        sinresultado.Attributes["class"] = "msg-info";
                                        sinresultado.InnerHtml = string.Format("{0} {1} {2}", msg_anterior, ".....La transmisión a N4 Billing fue ejecuta sin problemas: ", strinb.ToString());
                                        sinresultado.Visible = true;
                                        

                                    }
                                }
                            }


                        }
                        else
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("ERROR AL realizar aprobación de nota de crédito No. Interno {0}", this.pId.ToString());
                            sinresultado.Visible = true;
                            return;

                        }
                    }

                    populate();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                    this.botones.Visible = false;
                    this.motivo.Visible = false;
                    this._nc_id.Value = string.Empty;
                }
            }
        }


        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                    var user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta_pro", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta_pro", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("ID NO CONVERTIBLE"), "consulta_pro", "tablePagination_ItemCommand",e.CommandArgument.ToString(), user.loginname));
                        sinresultado.Visible = true;
                        return;

                    }

                    Int64 nid_level = 0;
                    Int64 nid_group = 0;
                    Int32 nIdUsuario = 0;
                    Int16 nlevel = 0;

                    if (e.CommandName == "Aprobar")
                    {
                        Label lbl_id_level = e.Item.FindControl("lbl_id_level") as Label;
                        Label lbl_id_group = e.Item.FindControl("lbl_id_group") as Label;
                        Label lbl_IdUsuario = e.Item.FindControl("lbl_IdUsuario") as Label;
                        Label lbl_level = e.Item.FindControl("lbl_level") as Label;

                        if (lbl_id_level.Text != string.Empty) { nid_level = Convert.ToInt64(lbl_id_level.Text); } else { nid_level = 0; }
                        if (lbl_id_group.Text != string.Empty) { nid_group = Convert.ToInt64(lbl_id_group.Text); } else { nid_group = 0; }
                        if (lbl_IdUsuario.Text != string.Empty){ nIdUsuario = Convert.ToInt32(lbl_IdUsuario.Text); } else { nIdUsuario = 0; }
                        if (lbl_level.Text != string.Empty){ nlevel = Convert.ToInt16(lbl_level.Text); } else { nlevel = 0; }

                        this.Limpiar();

                        this.pAccion = "P";
                        this.pId = id;
                        this.pIdUsuario = nIdUsuario;
                        this.pid_group = nid_group;
                        this.pid_level = nid_level;
                        this.plevel = nlevel;

                        this.botones.Visible = true;
                        this.motivo.Visible = false;
                        this.BtnAgregar.Visible = true;
                        this.LblRuta.Visible = true;
                        this.TxtArchivo.Visible = true;

                        this.UdBotones.Update();
                     
                        /*
                        objcredit_head = new credit_head();
                        objcredit_head.nc_id = id;
                        objcredit_head.id_level = nid_level;
                        objcredit_head.id_group = nid_group;
                        objcredit_head.IdUsuario = nIdUsuario;
                        objcredit_head.level = nlevel;
                        objcredit_head.Create_user = user.loginname;

                        if (objcredit_head.Aprobar())
                        {
                            
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            sinresultado.InnerText = string.Format("Aprobación de nota de crédito No. Interno {0} ,realizada con éxito.", e.CommandArgument);
                            sinresultado.Visible = true;
                            this.botones.Visible = false;
                            this.motivo.Visible = false;
                            this._nc_id.Value = string.Empty;
                            this.TxtMotivo.Text = string.Empty;
                        }
                        else
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("ERROR AL realizar aprobación de nota de crédito No. Interno {0}", e.CommandArgument);
                            sinresultado.Visible = true;
                            return;

                        }*/

                    }
                    if (e.CommandName == "Anular")
                    {

                        /*mostar paneles de motivo de anulacion*/
                        this.TxtMotivo.Text = null; 
                        this.botones.Visible = true;
                        this.motivo.Visible = true;
                        this._nc_id.Value = id.ToString();

                        this.BtnAgregar.Visible = false;
                        this.LblRuta.Visible = false;
                        this.TxtArchivo.Visible = false;

                        this.UdBotones.Update();

                        this.Limpiar();

                        this.pAccion = "I";
                        this.pId = id;
                       

                    }
                   
                    populate();

                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al anular", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }

       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
            Int64 nc_id = 0;
            string vr = string.Empty;
            try
           {
               DateTime desde;
               DateTime hasta;
                
                if (this.desded.Text != string.Empty)
                {
                    if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha desde";
                        sinresultado.Visible = true;
                        return;
                    }
                }
                else { desde = DateTime.Parse("01/01/1999");   }

                if (this.hastad.Text != string.Empty)
                {
                    if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha hasta";
                        sinresultado.Visible = true;
                        return;
                    }
                }
                else { hasta = DateTime.Parse("01/01/1999"); }

                if (string.IsNullOrEmpty(TxtNumero.Text))
                {
                    this.TxtNumero.Text = "";
                    nc_id = 0;
                }
                else
                {
                    if (!Int64.TryParse(this.TxtNumero.Text, out nc_id))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Error al convertir el número de N/C";
                        sinresultado.Visible = true;
                        return;
                    }
                }
                
          
               var user = Page.getUserBySesion();

                var table = credit_head.List_Nota_Credito_Pendientes(user.id, user.loginname, desde,hasta, nc_id, int.Parse(this.CboConcepto.SelectedValue) ,out vr);
                if (table == null)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = vr;
                    sinresultado.Visible = true;
                    return;
                }
                if (table.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = string.Format("{0} {1}",this.sinresultado.InnerText,"... No se encontraron resultados, revise los parámetros");
                   sinresultado.Visible = true;
                   return;
               }

               Session["resultado"] = table;
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;

           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
                sinresultado.Visible = true;
           }

       }

     
        #region "metodos_repeater"

        public static string formatPro(object id)
        {
            Int64 idm = 0;
            if (id != null)
            {
                if (Int64.TryParse(id.ToString(), out idm))
                {
                    return idm.ToString("D8");
                }
            }
            return "undefined";
        }
        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            var es = estado.ToString();
            es = es.ToLower();

            if (es.Equals("n")) {
                return "<span class='azul' >Generada</span>";
            }
            if (es.Equals("f"))
            {
                return "<span class='naranja' >Facturada</span>";
            }
            if (es.Equals("a"))
            {
                return "<span class='red' >Anulada</span>";
            }
            return "<span>sin estado!</span>";
        }
        public static bool boton(object estado)
        {
            var t = estado as string;
            if (!string.IsNullOrEmpty(t))
            {
                if (t.ToLower().Contains("a") || t.ToLower().Contains("f"))
                {
                    return false;
                }
            }

            return true;
        }
        public static string securetext(object number)
        {
            if (number == null )
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        public static string formatProDate(object fecha)
        {
            DateTime dt;
            if (fecha != null)
            {
                if (DateTime.TryParse(fecha.ToString(), out dt))
                {
                    return dt.ToString("dd/MM/yyyy HH:mm");
                }
            }

            return "undefined";
        }
        public static string xver(object est)
        {
            if (est != null)
            {
                return est.ToString().ToLower().Equals("a") ? "ocultar" : "mostrar";
            }
            return null;
        }


        #endregion
    }
}