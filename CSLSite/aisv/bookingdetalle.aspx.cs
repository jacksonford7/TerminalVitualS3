using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;
using CSLSite.CatalogosTableAdapters;

namespace CSLSite
{
    public partial class bookingdetalle : System.Web.UI.Page
    {
        string sid = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (!IsPostBack)
            {
                sinresultado.Visible = false;
            }
            try
            {

                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "bookingdetalle", "Init", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }

                find_Click();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "bookingdetalle", "Page_Load", sid, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                base.Response.Write(close);
            }
        }
        private void find_Click()
        {
            var tabla = new Catalogos.BookingDetalleDataTable();
            var ta = new CatalogosTableAdapters.BookingDetalleTableAdapter();
            var u = this.getUserBySesion();
            try
            {
                ta.ClearBeforeFill = true;
                ta.Fill(tabla, sid);
                sinresultado.Visible = false;
                if (tabla.Rows.Count <= 0)
                {
                    sinresultado.Visible = true;
                    tablePagination.Visible = false;
                    xfinder.Visible = false;
                    return;
                }
                //elegir la cabecera.
                var filatop = tabla.FirstOrDefault();
                this.numero.InnerText = !filatop.IsbnumberNull() ? filatop.bnumber.ToUpper() : string.Empty;
                this.referencia.InnerText = filatop.breferencia;
                this.fk.InnerText = !filatop.IsbfkNull() ? filatop.bfk : string.Empty;
                this.imo.InnerText = !filatop.IsbimoNull() ? filatop.bimo : string.Empty;
                this.nave.InnerText = filatop.bnave;
                this.viaje.InnerText = !filatop.IsbviajeNull() ? filatop.bviaje : string.Empty;
                this.eta.InnerText = !filatop.IsbetaNull() ? filatop.beta.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                this.cutoff.InnerText = !filatop.IsbcutOffNull() ? filatop.bcutOff.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                this.refer.InnerText = !filatop.IsbreeferNull() ? filatop.breefer == 1 ? "SI" : "NO" : string.Empty;
                this.pod.InnerText = !filatop.IsbpodNull() ? filatop.bpod : "NA";
                this.pod1.InnerText = !filatop.Isbpod1Null() ? filatop.bpod1 : "NA";
                this.comoditi.InnerText = !filatop.IsbcomodityNull() ? filatop.bcomodity : string.Empty;
                this.notas.Value = !filatop.IsremarksNull() ? filatop.remarks : string.Empty;
                
                sinresultado.Visible = false;
                tablePagination.Visible = true;
                if (Response.IsClientConnected)
                {
                    this.tablePagination.DataSource = tabla;
                    this.tablePagination.DataBind();
                    xfinder.Visible = true;
                    propiedad.InnerHtml = string.Empty;
                    if (!filatop.IsshiperidNull() && u != null && filatop.shiperid != u.ruc)
                    {
                        propiedad.InnerHtml = !string.IsNullOrEmpty(filatop.shipname) ? string.Format("Booking reservado por: {0}", filatop.shipname) : "Usted no es propietario de esta reserva"; 
                        //AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key_alert", "alert('Se le comunica que usted no es el reservante de este booking');", true);

                    }
                }
              }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "bookingDetalle", "find_Click",sid, u!=null?u.loginname:"userNull"));
                sinresultado.Visible = true;
            }
            finally
            {
                tabla.Dispose();
                ta.Dispose();
            }
        }
        public static string getClass(object valor, object fk)
        {
            if (fk == null || fk.ToString().Trim().Length <= 0 || fk.ToString().Trim().ToUpper().Contains("BBK"))
            {
                return "point";
            }
            if (fk.ToString().Trim().ToUpper().Contains("LCL"))
            {
                return "point";
            }
            if (valor == null)
            {
                return "point rowdis";
            }
            int i=0;
            if (!int.TryParse(valor.ToString(), out i))
            {
                return "point rowdis";
            }
            if (i > 0)
            {
                return "point";
            }
            return "point rowdis";
        }
    }
}