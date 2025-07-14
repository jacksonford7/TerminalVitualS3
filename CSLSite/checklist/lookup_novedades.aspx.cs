using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BillionEntidades;
namespace CSLSite
{
    public partial class lookup_novedades : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;

        //public string ID_TIPO_EQUIPO
        //{
        //    get { return (string)Session["ID_TIPO_EQUIPO"]; }
        //    set { Session["ID_TIPO_EQUIPO"] = value; }
        //}

        private usuario u;
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
                sinresultado.Visible = false;
            }

            if (!Page.IsPostBack)
            {

                string ID_TIPO_EQUIPO = Request.QueryString["ID_TIPO_EQUIPO"];

                var lookup = Cls_Novedades.Lista_Novedades(txtfinder.Text, Int64.Parse(ID_TIPO_EQUIPO), out v_mensaje);
                try
                {
                    if (Response.IsClientConnected)
                    {
                        if (lookup != null && lookup.Count > 0)
                        {
                            this.tablePagination.DataSource = lookup;
                            this.tablePagination.DataBind();
                            xfinder.Visible = true;
                            sinresultado.Visible = false;
                            return;
                        }
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lookup_linea_naviera", "find_Click", txtfinder.Text, u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }

            }

        }
        protected void find_Click(object sender, EventArgs e)
        {

            string ID_TIPO_EQUIPO = Request.QueryString["ID_TIPO_EQUIPO"];

            if (Request.QueryString["ID_TIPO_EQUIPO"] == null || string.IsNullOrEmpty(ID_TIPO_EQUIPO))
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Debe seleccionar el tipo de equipo");
                sinresultado.Visible = true;
                return;
            }

            var lookup = Cls_Novedades.Lista_Novedades(txtfinder.Text, Int64.Parse(ID_TIPO_EQUIPO), out v_mensaje);
            try
            {
                if (Response.IsClientConnected)
                {
                    if (lookup != null && lookup.Count > 0)
                    {
                        this.tablePagination.DataSource = lookup;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        return;
                    }
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lookup_linea_naviera", "find_Click", txtfinder.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }
    }
}