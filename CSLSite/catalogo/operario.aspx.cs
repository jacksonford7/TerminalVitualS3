using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ControlOPC.Entidades;

namespace CSLSite
{
    public partial class operario : System.Web.UI.Page
    {
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

            if (string.IsNullOrEmpty(Request.QueryString["opc"]))
            {
                this.AbortResponse("Para mostrar este catalogo debe especificar un opc", "../cuenta/menu.aspx", true);
                return;
            }


            u = this.getUserBySesion();
            //me aseguro que que exista un usuario y que tenga un ruc
            if (u != null && !string.IsNullOrEmpty(u.ruc))
            {

                var opc = Opc.GetOPC(u.ruc);
                //simhay problemas al traer el OPC.
                if (opc == null)
                {
                    this.AbortResponse(string.Format("El RUC [{0}] no corresponde al de una OPC registrada, por favor comuníquese con credenciales y permisos", u.ruc));
                    return;
                }
            }
        }
        protected void find_Click(object sender, EventArgs e)
        {

            var t = Request.QueryString["opc"];

            var loper = Operator.ListOperator(t, txtfinder.Text);
            try
            {
                if (Response.IsClientConnected)
                {
                    if (loper!=null && loper.Count > 0)
                    {
                        this.tablePagination.DataSource = loper;
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
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lineas", "find_Click", txtfinder.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
          }
    }
}