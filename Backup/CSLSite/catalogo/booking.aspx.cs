using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSLSite.CatalogosTableAdapters;

namespace CSLSite
{
    public partial class booking : System.Web.UI.Page
    {
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
                string[] parametros = { "tipo" };
                Page.ReviewQS(parametros, "No se puede buscar un booking sin la línea asociada");
                if (Request.QueryString["tipo"] == "FCL")
                {
                    sinresultado.InnerText = string.Format("No se han encontrado resultados, por favor asegúrese que el booking que está buscando sea de contenedores llenos (FCL){0}",Request.QueryString["linea"] != null ? " y la línea naviera registrada en el booking sea la que usted seleccionó [" + Request.QueryString["linea"] + "]" : "."); 
                }
                else if (Request.QueryString["tipo"] == "LCL")
                {
                    sinresultado.InnerText = string.Format("No se han encontrado resultados, por favor asegúrese que el booking que está buscando sea de contenedores para consolidación (LCL){0}", Request.QueryString["linea"] != null ? " y la línea naviera registrada en el booking sea la que usted seleccionó [" + Request.QueryString["linea"] + "]" : ".");
                }
                else if (Request.QueryString["tipo"] == "BBK")
                {
                    sinresultado.InnerText = string.Format("No se han encontrado resultados, por favor asegúrese que el booking que está buscando sea de carga suelta (B-BULK){0}",Request.QueryString["linea"] != null ? " y la línea naviera registrada en el booking sea la que usted seleccionó [" + Request.QueryString["linea"] + "]" : ".");
                }
                else if (Request.QueryString["tipo"] == "MTY")
                {
                    sinresultado.InnerText = string.Format("No se han encontrado resultados, por favor asegúrese que el booking que está buscando sea de contenedores vacíos (MTY) {0}",Request.QueryString["linea"] != null ? " y la línea naviera registrada en el booking sea la que usted seleccionó [" + Request.QueryString["linea"] + "]" : "."); 
                }
                else
                {
                    sinresultado.InnerText = "Por favor comuníquese de inmediato con el departamento de planificación en CGSA";
                }
             
            }
        }
        protected void find_Click(object sender, EventArgs e)
        {
            var tabla = new Catalogos.bookingDataTable();
            var ta = new CatalogosTableAdapters.bookingTableAdapter();
            var u = this.getUserBySesion();
            try
            {
                ta.ClearBeforeFill = true;
                ta.Fill(tabla, txtfinder.Text.Trim(), Request.QueryString["linea"], Request.QueryString["tipo"], null);
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
                this.referencia.InnerText = !filatop.IsbreferenciaNull() ? filatop.breferencia : string.Empty;
                this.fk.InnerText = !filatop.IsbfkNull() ? filatop.bfk : string.Empty;
                this.imo.InnerText = !filatop.IsbimoNull() ? filatop.bimo : string.Empty;
                this.nave.InnerText = !filatop.IsbnaveNull() ? filatop.bnave : "No establecida";
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
                        propiedad.InnerHtml = !string.IsNullOrEmpty(filatop.shipname) ? string.Format("Booking reservado por: {0}", filatop.shipname) : "Usted no es propietario de esta reserva"; ;
                        AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key_alert", "alert('Se le comunica que usted no es el reservante de este booking');", true);
                    }
                }
              }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "booking", "find_Click", txtfinder.Text, u!=null?u.loginname:"userNull"));
                sinresultado.Visible = true;
            }
            finally
            {
                tabla.Dispose();
                ta.Dispose();
            }
        }
        public static string getClass(object valor, object fk, object inactive)
        {
            if (fk == null || fk.ToString().Trim().Length <= 0 || fk.ToString().Trim().ToUpper().Contains("BBK"))
            {
                return "point";
            }
            if (fk.ToString().Trim().ToUpper().Contains("LCL") && inactive!=null)
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