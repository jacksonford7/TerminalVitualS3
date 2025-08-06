using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;

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

                BindPreview(idPase);
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            string idPase = Request.QueryString["id_pase"];
            if (!string.IsNullOrEmpty(idPase))
            {
               BindPreview(idPase);
            }
        }

        private void BindPreview(string idPase)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BILLION"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("[dbo].[lista_pase_despacho_por_idpase]", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pase", idPase);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        LblMensaje.Text = "No se encontró pase para impresión.";
                        LblMensaje.Visible = true;
                        GridPreview.Visible = false;
                    }
                    else
                    {
                        GridPreview.DataSource = dt;
                        GridPreview.DataBind();
                        GridPreview.Visible = true;
                        LblMensaje.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LblMensaje.Text = $"Error: {ex.Message}";
                LblMensaje.Visible = true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
    }
}
