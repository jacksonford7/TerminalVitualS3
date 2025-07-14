using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class empresas : System.Web.UI.Page
    {
        public usuario sUser = null;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        /// <summary>
        /// Código que se ejecuta cuando se carga la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
#if !DEBUG
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
#endif
            }
        }

        /// <summary>
        /// Código que realiza la carga de los resultados de la consulta en el grid
        /// </summary>
        /// <param name="nombreEmpresa">Nombre de la empresa a buscar</param>
        /// <param name="identificacionEmpresa">Identificación de la empresa a buscar</param>
        private void cargarResultados(string nombreEmpresa, string identificacionEmpresa)
        {
            try
            {
                List<Empresa> empresas = new List<Empresa>();
                Seguridad seguridad = new Seguridad();
                empresas = seguridad.consultarEmpresas(nombreEmpresa, identificacionEmpresa);
                if (empresas != null)
                {
                    if (empresas.Count > 0)
                    {
                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        tablePagination.DataSource = empresas;
                        tablePagination.DataBind();
                    }
                    else
                    {
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                        sinresultado.InnerText = "No hay información relacionada según los criterios de búsqueda establecidos";
                    }
                }
            }
            catch (Exception ex)
            {
                this.sinresultado.Visible = true;
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "empresas", "consultar_empresas", txtfinder.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Código que realiza la búsqueda de las empresas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void find_Click(object sender, EventArgs e)
        {
            var u = this.getUserBySesion();
            try
            {
                if (Response.IsClientConnected)
                {
                    cargarResultados(txtfinder.Text.Trim(), txtIdentificacion.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "empresas", "find_Click", txtfinder.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }

        }
    }
}