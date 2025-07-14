using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using System.Web.Script.Services;
using System.Configuration;
using Newtonsoft.Json;
using csl_log;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.IO;
using PasePuerta;

using BillionEntidades;

namespace CSLSite
{
    public partial class asignacion_dae : System.Web.UI.Page
    {
        private static string TextoLeyenda = string.Empty;
        private string cMensajes;
        private Cls_Bil_Log_Carbono_Expo objLogCarbono = new Cls_Bil_Log_Carbono_Expo();
        private string XMLUnidadesCertificado = string.Empty;

        public String emailCliente
        {
            get { return (String)Session["emailClienteDae"]; }
            set { Session["emailClienteDae"] = value; }
        }

        public String NombreCliente
        {
            get { return (String)Session["NombreCliente"]; }
            set { Session["NombreCliente"] = value; }
        }

        private static string leyenda_carbononeutro()
        {
            List<Cls_Bil_Configuraciones> Leyenda = Cls_Bil_Configuraciones.Parametros(out TextoLeyenda);
            if (!String.IsNullOrEmpty(TextoLeyenda))
            {
                return string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", TextoLeyenda);
            }

            var LinqLeyenda = (from Tope in Leyenda.Where(Tope => Tope.NOMBRE.Equals("LEYENDA_CON"))
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

        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
        }

        protected void tablePaginationBkg_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    if (e.Item.ItemType == ListItemType.Header)
                    {

                        var link = e.Item.FindControl("LinkCarbono") as LinkButton;
                        var img = e.Item.FindControl("ImgCarbono") as Image;

                        link.Text = "CERTIFICADO <br>DE CARBONO<br> NEUTRO";
                        link.Attributes["rel"] = "lightbox";
                        link.ForeColor = System.Drawing.Color.White;
                        link.Attributes["href"] = "https://apps.cgsa.com.ec/Terminal";
                        link.Attributes["title"] = string.Format(TextoLeyenda, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine);
                        link.Attributes["target"] = "_blank";


                        img.ImageUrl = "~/img/carbono_neutro.png";
                        img.Attributes["href"] = "https://apps.cgsa.com.ec/Terminal";
                        img.Attributes["title"] = string.Format(TextoLeyenda, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine);
                        img.Attributes["target"] = "_blank";

                    
                    }


             
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "asignacion_dae", "tablePaginationBkg_RowDataBound()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                }
            }
        }
        //SI_Customer_Statement_NAVIS_CGSA.Ws_Sap_EstadoDeCuenta WsSapEstadoDeCuenta = new SI_Customer_Statement_NAVIS_CGSA.Ws_Sap_EstadoDeCuenta();
        //AntiXRCFG.
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
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                    this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
                }

                this.IsAllowAccess();
                var user = Page.Tracker();

                if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
                {
                    var t = CslHelper.getShiperName(user.ruc);
                    if (string.IsNullOrEmpty(user.ruc))
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                        this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                    }
                    this.agencia.Value = user.ruc;
                    this.hfRucUser.Value = user.ruc;
                    Session["RUC_ASIGNACION_DAE"] = this.agencia.Value;
                    this.emailCliente = user.email;
                    this.NombreCliente = user.nombres;

                    if (!IsPostBack)
                    {
                        var email_dae_asignacion = asignacionDae.getEmail(this.agencia.Value, Page.User.Identity.Name.ToUpper());
                        if (email_dae_asignacion.Rows.Count == 0)
                        {
                            this.tmailinfocli.Text = this.emailCliente;
                        }
                        else
                        {
                            this.tmailinfocli.Text = email_dae_asignacion.Rows[0]["EMAILEXPORTADOR"].ToString();
                        }
                    }

                  

                }
                if (!IsPostBack)
                {
                    this.IsCompatibleBrowser();
                    Page.SslOn();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    TextoLeyenda = leyenda_carbononeutro();

                    
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "asignacion_dae", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtRpt = new DataTable();
                string mailContenedores = string.Empty;
                bool Tiene_Servicio = false;
                string MailSelect = string.Empty;

                dtRpt.Columns.Add("GKEY");
                dtRpt.Columns.Add("BKG");
                dtRpt.Columns.Add("CNTR");
                dtRpt.Columns.Add("DAE");
                dtRpt.Columns.Add("CARBONO");

                //valida si tiene seleccionado contenedores
                int Sel = 0;
                foreach (RepeaterItem item in tablePaginationBkg.Items)
                {
                    CheckBox chkElegir = item.FindControl("chkElegir") as CheckBox;
                    if (chkElegir.Checked)
                    {
                        Sel++;
                    }
                }

                if (Sel == 0)
                {
                    OcultarLoading();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta()", true);
                    this.Alerta("¡ No tiene Contenedores seleccionados para Asignar a la DAE.");
                    return;
                }

                if (string.IsNullOrEmpty(this.tmailinfocli.Text) && string.IsNullOrEmpty(this.Txtmail2.Text) && string.IsNullOrEmpty(this.Txtmail3.Text)
                       && string.IsNullOrEmpty(this.Txtmail4.Text) && string.IsNullOrEmpty(this.Txtmail5.Text))
                {
                    OcultarLoading();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta()", true);
                    this.Alerta("¡ Debe ingresar un mail para las alertas de asignaciones..");
                    return;
                }
                else
                {
                    
                    if (!string.IsNullOrEmpty(this.tmailinfocli.Text))
                    {
                        MailSelect = string.Format("{0};",this.tmailinfocli.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(this.Txtmail2.Text))
                    {
                        MailSelect = string.Format("{0}{1};", MailSelect, this.Txtmail2.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(this.Txtmail3.Text))
                    {
                        MailSelect = string.Format("{0}{1};", MailSelect, this.Txtmail3.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(this.Txtmail4.Text))
                    {
                        MailSelect = string.Format("{0}{1};", MailSelect, this.Txtmail4.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(this.Txtmail5.Text))
                    {
                        MailSelect = string.Format("{0}{1};", MailSelect, this.Txtmail5.Text.Trim());
                    }

                    if (!string.IsNullOrEmpty(MailSelect))
                    {
                        MailSelect = MailSelect.Substring(0, MailSelect.Trim().Length - 1);
                    }

                    //if (MailSelect.Split(';').ToList().Count > 5)
                    //{
                    //    OcultarLoading();
                    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta()", true);
                    //    this.Alerta("¡ Puede ingresar hasta 5 direcciones de correo..");
                    //    return;
                    //}
                }

                XMLUnidadesCertificado = "<CONTENEDORES>";

                foreach (RepeaterItem item in tablePaginationBkg.Items)
                {
                    Label lblGkey = item.FindControl("lblGkey") as Label;
                    Label lblBkg = item.FindControl("lblBkg") as Label;
                    Label lblCntr = item.FindControl("lblCntr") as Label;
                    CheckBox chkElegir = item.FindControl("chkElegir") as CheckBox;
                    CheckBox CHKCERTIFICADO = item.FindControl("CHKCERTIFICADO") as CheckBox;

                    var dtResult = asignacionDae.GetValDAE(txtDAE.Text.Trim(), lblCntr.Text.Trim(), txtbkg.Text.Trim(), this.agencia.Value, Page.User.Identity.Name.ToUpper());
                    if (dtResult.Rows[0]["MENSAJE"].ToString() != "1")
                    {
                        OcultarLoading();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta()", true);
                        this.Alerta(dtResult.Rows[0]["MENSAJE"].ToString());
                        return;
                    }
                    if (chkElegir.Checked)
                    {
                        dtRpt.Rows.Add(lblGkey.Text, lblBkg.Text.Trim(), lblCntr.Text.Trim(), txtDAE.Text.Trim(), (CHKCERTIFICADO.Checked ?  1 : 0));
                        mailContenedores = string.Concat(mailContenedores,
                        string.Format("<strong>     Booking: </strong>{0}<strong>     Contenedor: </strong>{1}<br/>",
                                                    txtbkg.Text.Trim(),               lblCntr.Text.Trim()));
                    }

                    //CERTIFICADO CARBONO NEUTRO
                    if (CHKCERTIFICADO.Checked && chkElegir.Checked)
                    {
                        XMLUnidadesCertificado += string.Format("<CONTENEDOR gkey='{0}' contenedor='{1}'/>", lblGkey.Text, lblCntr.Text);
                        Tiene_Servicio = true;
                    }
                    

                    //graba log del servicio
                    objLogCarbono.RUC = this.agencia.Value;
                    objLogCarbono.GKEY = Int64.Parse(lblGkey.Text);
                    objLogCarbono.BKG = lblBkg.Text;
                    objLogCarbono.CNTR = lblCntr.Text;
                    objLogCarbono.DAE = txtDAE.Text.Trim();
                    objLogCarbono.CARBONO = CHKCERTIFICADO.Checked;
                    objLogCarbono.USUARIOING = Page.User.Identity.Name.ToUpper();

                    string xerror;
                    var nProceso = objLogCarbono.SaveTransaction(out xerror);
                    /*fin de nuevo proceso de grabado*/
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {

                    }

                }

                XMLUnidadesCertificado += "</CONTENEDORES>";
                /**********************************************************************************************************************************
                 * activa servicio de carbono neutro
                 * ********************************************************************************************************************************/

                List<Cls_Bil_Valida_Certificado> ListCertificado = Cls_Bil_Valida_Certificado.Validacion_Certificado_Expo(XMLUnidadesCertificado.ToString(), out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, error en obtener datos de certificado carbono neutro....{0}", cMensajes));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    return;
                }

                /*listado de unidades sin el servicio*/
                var LinqCertificado = (from TblFact in ListCertificado.Where(TblFact => TblFact.servicio.Equals(0))
                                       select new
                                       {
                                           gkey = TblFact.gkey,
                                           CONTENEDOR = TblFact.contenedor,
                                           servicio = TblFact.servicio
                                       }).Distinct();

                List<string> ListaCert = new List<string>();
                foreach (var Det in LinqCertificado)
                {
                    ListaCert.Add(Det.CONTENEDOR);

                }

                if (ListaCert.Count != 0)
                {
                    var Resultado = Servicio_Certificado.Marcar_Servicio_Expo(Page.User.Identity.Name.ToUpper(), ListaCert);
                    if (Resultado.Exitoso)
                    {

                    }
                    else
                    {
                        this.Alerta(string.Format("Se presentaron los siguientes problemas, no se pudo generar servicios de carbono neutro: {0}, Existen los siguientes problemas: {1} ", Resultado.MensajeInformacion, Resultado.MensajeProblema));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        return;
                    }
                }
                /**********************************************************************************************************************************
               * fin servicio carbono neutro
               * ********************************************************************************************************************************/


                dtRpt.AcceptChanges();
                dtRpt.TableName = "Dae";
                StringWriter sw = new StringWriter();
                dtRpt.WriteXml(sw);

                string mensaje = null;
                if (!asignacionDae.RegistraAsignacionDAE_New(
                    sw.ToString(),
                    this.agencia.Value,
                    Page.User.Identity.Name.ToUpper(),
                    MailSelect,
                    out mensaje))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    this.Alerta(mensaje);
                    return;
                }
                else
                {
                    string mail = string.Empty;
                    string error = string.Empty;
                    string correoBackUp = string.Empty;
                    string mail_em = string.Empty;
                    string sUser_email = string.Empty;
                    string destinatarios = string.Empty;
                    string mensajeerror = string.Empty;

                    mail = string.Empty;

                    var email_dae_asignacion = asignacionDae.getEmail(this.agencia.Value, Page.User.Identity.Name.ToUpper());
                    if (email_dae_asignacion.Rows.Count == 0)
                    {
                        mail_em = "contecon.it@gmail.com";
                    }
                    else
                    {
                        mail_em = email_dae_asignacion.Rows[0]["EMAILEXPORTADOR"].ToString() + ';' + email_dae_asignacion.Rows[0]["EMAILOPERADOR"].ToString();
                    }

                    if (Tiene_Servicio)
                    {  
                        //graba en tablas de cls_services de carbono neutro
                        objLogCarbono.RUC = this.agencia.Value;
                        objLogCarbono.XMLDAE = sw.ToString();
                        objLogCarbono.nombre = this.NombreCliente;
                        objLogCarbono.email1 = emailCliente;
                        objLogCarbono.email2 = mail_em;
                        objLogCarbono.email3 = null;
                        objLogCarbono.USUARIOING = Page.User.Identity.Name.ToUpper();

                        string xerror;
                        var nProceso = objLogCarbono.SaveTransaction_New(out xerror);
                        /*fin de nuevo proceso de grabado*/
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {

                        }

                    }
                  


                    correoBackUp = "contecon.it@gmail.com";
                    //mail_em = "rreyes@cgsa.com.ec";
                    sUser_email = emailCliente;

                    mail = string.Concat(mail, string.Format("Estimado/a{0} {1}:<br/><br/>Este es un mensaje del Sistema de Terminal Virtual de Contecon Guayaquil S.A, para comunicarle lo siguiente:<br/><br/>", "", ""));
                    mail = string.Concat(mail, string.Format("<br/><strong>Contenedores Asignados a la D.A.E: </strong>" + txtDAE.Text.Trim() + "<br/>"));
                    if (!string.IsNullOrEmpty(mailContenedores))
                    {
                        mail = string.Concat(mail, string.Format("<br/><br/><strong>Detalle: </strong><br/>"));
                        mail = mail + mailContenedores;
                    }
                    error = string.Empty;
                    destinatarios = string.Format("{0};{1}", correoBackUp != null ? correoBackUp : "no_cfg", mail_em != null ? mail_em : "no_cfg");
                    CLSDataCentroSolicitud.addMail(out error, sUser_email, "Asignación D.A.E " + "" + "", mail, destinatarios, Page.User.Identity.Name, "", "");

                    if (!string.IsNullOrEmpty(error))
                    {
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", error, true);
                        return;
                    }

                    //Response.Write("<script language='JavaScript'>var r=alert('D.A.E: '" + txtDAE.Text.Trim() + "'\\nAsignada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "myLoad('" + txtDAE.Text.Trim() + "');", true);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "asignacion_dae", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var c = asignacionDae.GetCntrXBkg(txtbkg.Text.Trim());
                this.tablePaginationBkg.DataSource = c;
                this.tablePaginationBkg.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                if (c.Rows.Count == 0)
                {
                    this.Alerta("El Booking: " + txtbkg.Text.Trim() + ", no cumple los criterios de consulta.");
                    txtbkg.Focus();
                    return;
                }
                this.divnotificacion.InnerText = "Total de Contenedores Asociados al Booking [" + txtbkg.Text.Trim() + "]: #" + c.Rows.Count.ToString();

                XMLUnidadesCertificado = "<CONTENEDORES>";
                foreach (RepeaterItem item in tablePaginationBkg.Items)
                {
                    Label lblGkey = item.FindControl("lblGkey") as Label;
                    Label lblBkg = item.FindControl("lblBkg") as Label;
                    Label lblCntr = item.FindControl("lblCntr") as Label;
                    CheckBox chkElegir = item.FindControl("chkElegir") as CheckBox;
                    CheckBox CHKCERTIFICADO = item.FindControl("CHKCERTIFICADO") as CheckBox;


                    //CERTIFICADO CARBONO NEUTRO
                    
                    XMLUnidadesCertificado += string.Format("<CONTENEDOR gkey='{0}' contenedor='{1}'/>", lblGkey.Text, lblCntr.Text);
                    
                }

                XMLUnidadesCertificado += "</CONTENEDORES>";

                /**********************************************************************************************************************************
                 * activa servicio de carbono neutro
                 * ********************************************************************************************************************************/

                List<Cls_Bil_Valida_Certificado> ListCertificado = Cls_Bil_Valida_Certificado.Validacion_Certificado_Expo(XMLUnidadesCertificado.ToString(), out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, error en obtener datos de certificado carbono neutro....{0}", cMensajes));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    return;
                }

                /*listado de unidades sin el servicio*/
                var LinqCertificado = (from TblFact in ListCertificado.Where(TblFact => TblFact.gkey != 0)
                                       select new
                                       {
                                           gkey = TblFact.gkey,
                                           contenedor = TblFact.contenedor,
                                           servicio = TblFact.servicio
                                       }).Distinct();

                foreach (RepeaterItem item in tablePaginationBkg.Items)
                {
                    Label lblGkey = item.FindControl("lblGkey") as Label;
                    Label lblCntr = item.FindControl("lblCntr") as Label;
                    CheckBox CHKCERTIFICADO = item.FindControl("CHKCERTIFICADO") as CheckBox;
                    Label LblTiene = item.FindControl("LblTiene") as Label;

                    var Existe = LinqCertificado.FirstOrDefault(f => f.gkey.Equals(Int64.Parse(lblGkey.Text)));
                    if (Existe != null)
                    {
                        bool Marcar =(Existe.servicio == 1 ? true : false);
                        CHKCERTIFICADO.Checked = Marcar;
                        LblTiene.Text = (Marcar ? "SI" : "NO");
                        CHKCERTIFICADO.Enabled  = (Marcar ? false : true);

                        if (!Marcar)
                        {
                            CHKCERTIFICADO.Checked = true;
                        }
                    }
                    else
                    {
                        LblTiene.Text = "NO";
                        CHKCERTIFICADO.Checked = true;
                       // CHKCERTIFICADO.Enabled = true;
                    }

                }

                //txtbkg.Text = "";
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "asignacion_dae", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    bool ChkEstado = this.ChkTodos.Checked;

                    foreach (RepeaterItem xitem in tablePaginationBkg.Items)
                    {
                        CheckBox CHKCERTIFICADO = xitem.FindControl("CHKCERTIFICADO") as CheckBox;
                        Label LblTiene = xitem.FindControl("LblTiene") as Label;
                        if (!LblTiene.Text.Equals("SI"))
                        {
                            CHKCERTIFICADO.Checked = ChkEstado;
                        }
                   
                    }

                   // this.UpdatePanel0.Update();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "asignacion_dae", "ChkTodos_CheckedChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                }
            }
        }

        [System.Web.Services.WebMethod]
        public static string IsAvailableBooking(string rucuser, string booking)
        {
            var rucbooking = string.Empty;
            var c = asignacionDae.GetRucXBkg(booking.Trim());
            if (c.Rows.Count == 0)
            {
                return "2";
            }
            else
            {
                rucbooking = c.Rows[0][0].ToString();
#if DEBUG
                rucuser = rucbooking;
#endif
                if (rucuser == rucbooking)
                {
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
        }
    }
}