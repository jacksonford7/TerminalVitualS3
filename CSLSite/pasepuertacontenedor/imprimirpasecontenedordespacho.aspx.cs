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

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Net;
using Microsoft.Reporting.WebForms;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using CSLSite;

namespace CSLSite
{

    public partial class imprimirpasecontenedordespacho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idPase = Request.QueryString["id_pase"];
                if (string.IsNullOrEmpty(idPase))
                {
                    LblMensaje.Text = "No se encontró pase para impresión.";
                    LblMensaje.Visible = true;
                    return;
                }

            this.banmsg.Visible = IsPostBack;
            this.banmsg_det.Visible = IsPostBack;

            ClsUsuario = Page.Tracker();
            if (ClsUsuario != null)
            {
                if (!Page.IsPostBack)
                {
                    this.Limpia_Campos();


                    rwReporte.Visible = false;
                    this.imagen.Visible = true;
                    this.Actualiza_Paneles();
                }

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    numero_carga = QuerySegura.DecryptQueryString(Request.QueryString["id_carga"]);
                    if (numero_carga == null || string.IsNullOrEmpty(numero_carga))
                    {
                        this.TXTMRN.Text = string.Empty;
                        this.TXTMSN.Text = string.Empty;
                        if (string.IsNullOrEmpty(this.TXTHSN.Text))
                        { this.TXTHSN.Text = string.Format("{0}", "0000"); }
                    }
                    else
                    {
                        numero_carga = numero_carga.Trim().Replace("\0", string.Empty);
                        if (numero_carga.Split('+').ToList().Count > 0)
                        {
                            this.TXTMRN.Text = numero_carga.Split('-').ToList()[0].Trim();
                            this.TXTMSN.Text = numero_carga.Split('-').ToList()[1].Trim();
                            this.TXTHSN.Text = numero_carga.Split('-').ToList()[2].Trim();

                            this.BtnBuscar_Click(sender, e);

                            this.ChkTodos_CheckedChanged(sender, e);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "Eventos Botones"
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    BindPreview(idPase);
                }
                        Session["ImprimirPaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;

                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", Contenedores.MensajeProblema));

                        return;
                    }

                    this.Ocultar_Mensaje();


                }
                catch (Exception ex)
                {
                    LblMensaje.Text = ex.Message;
                    LblMensaje.Visible = true;
                }
            }




        }

        private void BindPreview(string idPase)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BILLION"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("[dbo].[lista_pase_despacho_por_idpase]", conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pase", idPase);
                da.Fill(dt);
            }
            if (objPaseContenedor.Detalle.Count == 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>No existe detalle de contenedores, para poder imprimir los pase a puerta"));
                return;
            }

            if (dt.Rows.Count == 0)
            {
                LblMensaje.Text = "No se encontró pase para impresión.";
                LblMensaje.Visible = true;
            }
            else
            {
                GridPreview.DataSource = dt;
                GridPreview.DataBind();
            }

        #endregion






        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }






    }

}
