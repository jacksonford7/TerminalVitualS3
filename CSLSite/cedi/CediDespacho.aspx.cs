using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Core.Cedi;
using Core.Cedi.Models;
using Infra.Cedi;

namespace CSLSite.Cedi
{
    // Replica del flujo de despachovehiculos adaptado para CEDI.
    // Entrada de filtros -> consulta servicio -> bind de grilla -> acciones -> feedback al usuario.
    public partial class CediDespacho : Page
    {
        private ICediDespachoService _service;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }
            }

            if (_service == null)
            {
                var repo = new CediDespachoRepository(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
                _service = new CediDespachoService(repo);
            }
        }

        protected void BtnCediBuscar_Click(object sender, EventArgs e)
        {
            lblCediMensaje.Visible = false;
            var filtro = new CediDespachoFiltro
            {
                Mrn = TXTCediMRN.Text.Trim(),
                Msn = TXTCediMSN.Text.Trim(),
                Hsn = TXTCediHSN.Text.Trim()
            };
            BindGridCedi(filtro);
        }

        protected void BtnCediLimpiar_Click(object sender, EventArgs e)
        {
            TXTCediMRN.Text = string.Empty;
            TXTCediMSN.Text = string.Empty;
            TXTCediHSN.Text = string.Empty;
            gvCediContenedores.DataSource = null;
            gvCediContenedores.DataBind();
            lblCediMensaje.Visible = false;
        }

        private void BindGridCedi(CediDespachoFiltro filtro)
        {
            try
            {
                var datos = _service.Buscar(filtro);
                gvCediContenedores.DataSource = datos;
                gvCediContenedores.DataBind();
            }
            catch (Exception ex)
            {
                lblCediMensaje.Text = ex.Message;
                lblCediMensaje.CssClass = "alert alert-danger";
                lblCediMensaje.Visible = true;
            }
        }

        protected void gvCediContenedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Asignar")
                {
                    _service.Asignar(id, User.Identity.Name);
                }
                else if (e.CommandName == "Despachar")
                {
                    _service.Despachar(id, User.Identity.Name);
                }
                lblCediMensaje.Text = "Acción ejecutada correctamente.";
                lblCediMensaje.CssClass = "alert alert-success";
                lblCediMensaje.Visible = true;
                BtnCediBuscar_Click(sender, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                lblCediMensaje.Text = ex.Message;
                lblCediMensaje.CssClass = "alert alert-danger";
                lblCediMensaje.Visible = true;
            }
        }

        protected void BtnCediExportar_Click(object sender, EventArgs e)
        {
            try
            {
                var filtro = new CediDespachoFiltro
                {
                    Mrn = TXTCediMRN.Text.Trim(),
                    Msn = TXTCediMSN.Text.Trim(),
                    Hsn = TXTCediHSN.Text.Trim()
                };
                var archivo = _service.Exportar(filtro);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "attachment;filename=DespachoCedi.xlsx");
                Response.BinaryWrite(archivo);
                Response.End();
            }
            catch (Exception ex)
            {
                lblCediMensaje.Text = ex.Message;
                lblCediMensaje.CssClass = "alert alert-danger";
                lblCediMensaje.Visible = true;
            }
        }
    }
}
