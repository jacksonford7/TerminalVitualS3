using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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

                try
                {
                    BindPreview(idPase);
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
        }
    }
}
