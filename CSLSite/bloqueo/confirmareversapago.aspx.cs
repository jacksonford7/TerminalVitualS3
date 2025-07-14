using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CgsaMaster.facturacion.bloqueo
{
    public partial class confirmareversapago : System.Web.UI.Page
    {
        ServicioFacturacion.ServicioFacturacionClient ServicioFacturacion = new ServicioFacturacion.ServicioFacturacionClient();
        public DataTable dtExpoToExcel
        {
            get { return (DataTable)Session["dtExpoToExcel"]; }
            set { Session["dtExpoToExcel"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Titulo = "Confirmación/Reverso de Pago";
            this.Master.FavName = "Confirmación/Reverso de Pago";
            if (!IsPostBack)
            {
                lblError.Text = "";
            }
        }
        protected void rpDetalleVGM_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                var numeroliq = e.CommandArgument.ToString();
                var comando = Convert.ToBoolean(e.CommandName.ToString());
                if (comando) //Confirma
                {
                    var obtenerestado = ServicioFacturacion.CambiarEstadoFactura(numeroliq.Trim(), comando,User.Identity.Name);
                    if (obtenerestado.FueOk)
                    {
                        UIHelper.Sindatos(this, "Su Liquidación ha sido reversada exitosamente.\\n* Número de Liquidación:  " + numeroliq);
                    }
                }
                else //Reverso
                {
                    var obtenerestado = ServicioFacturacion.CambiarEstadoFactura(numeroliq.Trim(), comando, User.Identity.Name);
                    if (obtenerestado.FueOk)
                    {
                        UIHelper.Sindatos(this, "Su Liquidación ha sido confirmada exitosamente.\\n* Número de Liquidación:  " + numeroliq);
                    }
                    
                }
                PoblarGridView(txtnumliq.Text.Trim());//numeroliq.Trim());
                //     args[0].ToString(),
                //     Page.User.Identity.Name.ToUpper(),
            }
            catch (Exception ex)
            {
                //                Response.Write("<script language='JavaScript'>alert('Hubo un problema con la transacción: " + ex.Message + ".');</script>");
                UIHelper.Sindatos(this, "Hubo un problema con la transacción: " + ex.Message);
            }
        }
        private void PoblarGridView(string numliq)
        {
            DataTable dt = new DataTable();
            using (var cx = getConex())
            {
                try
                {
                    numliq = "9025" + numliq;
                    System.Data.SqlClient.SqlDataAdapter t = new System.Data.SqlClient.SqlDataAdapter();
                    t.SelectCommand = new System.Data.SqlClient.SqlCommand();
                    t.SelectCommand.CommandTimeout = 0;
                    t.SelectCommand.Connection = cx;
                    t.SelectCommand.CommandText = "SP_CONSULTA_ECU_LIQUIDACION_SENAE";
                    t.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    t.SelectCommand.Parameters.AddWithValue("@NUMEROLIQ", numliq);
                    t.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        UIHelper.Sindatos(this, "No se encontraron datos:\\n* Número de Liquidación: " + numliq);
                        dt = null;
                        rpDetalleVGM.DataSource = dt;
                        rpDetalleVGM.DataBind();
                        txtnumliq.Focus();
                        return;
                    }
                    var obtenerestado = ServicioFacturacion.ObtenerEstadoFactura(numliq, Page.User.Identity.Name.ToUpper());
                    if (obtenerestado.FueOk)
                    {
                        var mensaje = "* Valor de la Factura: " + obtenerestado.ValorFactura + "\\n";
                        mensaje = mensaje + "* Valor Pagado: " + obtenerestado.ValorPagado + "\\n";
                        mensaje = mensaje + "* Valor Pendiente: " + obtenerestado.ValorPendiente;
                    }
                    else
                    {
                        UIHelper.Sindatos(this, "No se encontraron datos en WebServive ObtenerEstadoFactura:\\n* Número de Liquidación: " + numliq);
                        txtnumliq.Focus();
                        return;
                    }
                    dt.Columns.Add("VALOR_FACTURA");
                    dt.Columns.Add("VALOR_PAGADO");
                    dt.Columns.Add("VALOR_PENDIENTE");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["VALOR_FACTURA"] = obtenerestado.ValorFactura;
                        dt.Rows[i]["VALOR_PAGADO"] = obtenerestado.ValorPagado;
                        dt.Rows[i]["VALOR_PENDIENTE"] = obtenerestado.ValorPendiente;
                    }
                    dtExpoToExcel = dt;
                    rpDetalleVGM.DataSource = dt;
                    rpDetalleVGM.DataBind();
                    lblError.Text = "";
                }
                catch (Exception ex)
                {
                    dt = null;
                    rpDetalleVGM.DataSource = dt;
                    rpDetalleVGM.DataBind();
                    UIHelper.Sindatos(this, "Hubo un problema con la consulta: " + ex.Message);
                }
            }
        }
        public static bool ActualizarVGM(
        string codcertificado,
        string usuario,
        out string mensaje)
        {
            try
            {
                string valor = string.Empty;
                using (var conn = getConex())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "dbo.FNA_FUN_VGM_UPDATE_EXPORTADOR_MAN";
                            comando.Parameters.AddWithValue("@CODIGO", codcertificado);
                            comando.Parameters.AddWithValue("@USUARIO", usuario);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        //var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", sb);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                //var t = log_csl.save_log<Exception>(ex, "credenciales", "ApruebaSolicitudColaborador", DateTime.Now.ToShortDateString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", ex.Message);
                return false;
            }
        }
        public static SqlConnection getConex()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["liquidacion"].ConnectionString);
        }
        protected void btsave_Click(object sender, ImageClickEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                PoblarGridView(txtnumliq.Text.Trim());
                return;
            }
        }
    }
}