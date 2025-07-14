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

namespace CSLSite
{
    public partial class booking_autoriza : System.Web.UI.Page
    {
        public DataTable dtListaAsumeCliente
        {
            get { return (DataTable)Session["dtListaAsumeCliente"]; }
            set { Session["dtListaAsumeCliente"] = value; }
        }

        public DataTable dtListaBooking
        {
            get { return (DataTable)Session["dtListaBooking"]; }
            set { Session["dtListaBooking"] = value; }
        }

        public String emailCliente
        {
            get { return (String)Session["emailCliente"]; }
            set { Session["emailCliente"] = value; }
        }

        public Boolean ClienteBloqueado
        {
            get { return (Boolean)Session["ValClienteBloqueado"]; }
            set { Session["ValClienteBloqueado"] = value; }
        }

        public Boolean ClienteTipo
        {
            get { return (Boolean)Session["ValClienteTipo"]; }
            set { Session["ValClienteTipo"] = value; }
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
                this.emailCliente = user.email;
                this.ClienteBloqueado = user.bloqueo_cartera; // si es true esta bloqueado si es false no esta bloqueado
                this.ClienteTipo = user.IsCredito;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    /*
                    dtListaAsumeCliente = new DataTable();
                    dtListaAsumeCliente.Columns.Add("REFERENCIA");
                    dtListaAsumeCliente.Columns.Add("NAVE");
                    dtListaAsumeCliente.Columns.Add("BOOKING");
                    dtListaAsumeCliente.Columns.Add("CODIGOSAP");
                    dtListaAsumeCliente.Columns.Add("RUC");
                    dtListaAsumeCliente.Columns.Add("RAZSOCIAL");
                    dtListaAsumeCliente.Columns.Add("EMAIL");
                    dtListaAsumeCliente.Columns.Add("TIPO");
                    */

                    dtListaBooking = new DataTable();
                    dtListaBooking.Columns.Add("BOOKING");
                    dtListaBooking.Columns.Add("REFERENCIA");
                    dtListaBooking.Columns.Add("LINEA");
                    dtListaBooking.Columns.Add("MENSAJE");
                    RepeaterBooking.DataSource = dtListaBooking;
                    RepeaterBooking.DataBind();

                    //var r = man_pro_expo.GetCatalagoReferenciasPagoTerceros();

                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "pago_terceros", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {

                if (dtListaBooking.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + "Aun no Agrega Bookings a la lista." + "');getGifOcultaBuscar();", true);
                    return;
                }

                string mail = string.Empty;
                string error = string.Empty;
                string correoBackUp = string.Empty;
                string mail_em = string.Empty;
                string sUser_email = string.Empty;
                string destinatarios = string.Empty;
                string mensajeerror = string.Empty;

                string mailContenedores = string.Empty;
                int cont = 0;
                int add = 0;
                string mensaje = null;
                string mensajeu = null;

                for (int i = 0; i < dtListaBooking.Rows.Count; i++)
                {
                    if (!man_pro_expo.AddBookAutoriza(
                        dtListaBooking.Rows[i]["BOOKING"].ToString(),
                        dtListaBooking.Rows[i]["REFERENCIA"].ToString(),
                        dtListaBooking.Rows[i]["LINEA"].ToString(),
                        Page.User.Identity.Name.ToUpper(),
                        out mensaje))
                    {
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + mensaje + "');", true);
                        cont += 1;
                        mensajeu = mensaje;
                        if (mensaje.Contains("ya se encuentra autorizado"))
                            dtListaBooking.Rows[i]["MENSAJE"] = "Registro duplicado";
                        else
                            dtListaBooking.Rows[i]["MENSAJE"] = mensaje.Length > 30 ? mensaje.Substring(1, 30) : mensaje;
                    }
                    else
                    {


                        //if (!string.IsNullOrEmpty(error))
                        //{
                        //    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", error, true);
                        //    return;
                        //}
                        add += 1;
                        dtListaBooking.Rows[i]["MENSAJE"] = "Registrado con éxito";
                    }
                }

                RepeaterBooking.DataSource = dtListaBooking;
                RepeaterBooking.DataBind();

                if (cont == 0)
                    Response.Write("<script language='JavaScript'>var r=alert('Transacción exitosa todos los bookings fueron registrados.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                else
                {
                    this.Alerta(string.Format("Algunos registros reportaron error verifique los mensajes: bookings registrados {0}, bookings con error {1}.", add,cont));
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + mensajeu + "');", true);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "booking_atoriza", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + ex.Message + "');", true);
            }
        }

        protected void RepeaterBooking_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete") //breakpoint on this line
            {
                dtListaBooking.Rows.RemoveAt(e.Item.ItemIndex);
                dtListaBooking.AcceptChanges();
                RepeaterBooking.DataSource = dtListaBooking;
                RepeaterBooking.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }
        }

        protected void btnAsumirBook_Click(object sender, EventArgs e)
        {
            try
            {
                var query = from myRow in dtListaBooking.AsEnumerable()
                            where myRow.Field<string>("REFERENCIA") == xreferencia.Value.ToString() && myRow.Field<string>("BOOKING") == nbrboo.Value.ToString()
                            select myRow;
                DataTable tbresult = query.AsDataView().ToTable();

                if (tbresult.Rows.Count == 0)
                {

                    dtListaBooking.Rows.Add(nbrboo.Value, xreferencia.Value, xlinea.Value);
                    RepeaterBooking.DataSource = dtListaBooking;
                    RepeaterBooking.DataBind();
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnRemove_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

    }
}