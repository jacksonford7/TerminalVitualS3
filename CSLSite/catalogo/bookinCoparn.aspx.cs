using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CSLSite
{
    public partial class bookinCoparn : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                var t = EDI_bookingCab.DtBookingCabFilter(Page.User.Identity.Name, txtname.Text.Trim());
                try
                {
                    if (t != null && t.Rows.Count > 0)
                    {
                        //if (!string.IsNullOrEmpty(t.Rows[0]["alerta"].ToString()))
                        //{
                        //    var alert = "Usted no es el propietario de este Booking: " + txtname.Text.Trim();
                        //    xfinder.Visible = false;
                        //    alerta.Attributes["class"] = string.Empty;
                        //    alerta.Attributes["class"] = "msg-info";
                        //    this.alerta.InnerHtml = "";

                        //    this.tablePagination.DataSource = null;
                        //    this.tablePagination.DataBind();
                        //    this.Alerta(alert);
                        //    return;
                        //}

                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        //Session["ReferenciaZAL"] = null;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "alert alert-info";
                        this.alerta.InnerHtml = "Estimado cliente, si el número de booking que esta buscando no aparece en esta lista. <br />" +
                                       "Por favor comunicarse con la Linea Naviera.";
                        Session["s_linea_proforma1"] = t.Rows[0]["lineOperator"].ToString();
                        return;
                    }

                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "alert alert-danger";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", txtname.Text, Page.User != null ?Page.User.Identity.Name : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }
        protected void find_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
//#if !DEBUG
  
//             u = this.getUserBySesion();
//#endif


                //string v_referencia = Session["ReferenciaZAL"] as string;
                //if (v_referencia is null)
                //{
                //    xfinder.Visible = false;
                //    this.sinresultado.Attributes["class"] = string.Empty;
                //    this.sinresultado.Attributes["class"] = "msg-critico";
                //    this.sinresultado.InnerText = "Se debe seleccionar un deposito ";
                //    sinresultado.Visible = true;
                //    return;
                //}
                
                var t = EDI_bookingCab.DtBookingCabFilter(Page.User.Identity.Name, txtname.Text.Trim());
                try
                {
                    if (t != null && t.Rows.Count > 0)
                    {
                        //if (!string.IsNullOrEmpty(t.Rows[0]["alerta"].ToString()))
                        //{
                        //    var alert = "Usted no es el propietario de este Booking: " + txtname.Text.Trim();
                        //    xfinder.Visible = false;
                        //    alerta.Attributes["class"] = string.Empty;
                        //    alerta.Attributes["class"] = "msg-info";
                        //    this.alerta.InnerHtml = "";

                        //    this.tablePagination.DataSource = null;
                        //    this.tablePagination.DataBind();
                        //    this.Alerta(alert);
                        //    return;
                        //}

                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        //Session["ReferenciaZAL"] = null;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "alert alert-info";
                        this.alerta.InnerHtml = "Estimado cliente, si el número de booking que esta buscando no aparece en esta lista. <br />" +
                                       "Por favor comunicarse con la Linea Naviera.";
                        Session["s_linea_proforma"] = t.Rows[0]["lineOperator"].ToString();
                        return;
                    }
                    
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "alert alert-danger";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", txtname.Text, Page.User != null ? Page.User.Identity.Name : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }


    }
}