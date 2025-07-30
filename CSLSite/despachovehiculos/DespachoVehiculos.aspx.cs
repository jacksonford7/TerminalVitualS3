using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillionEntidades;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CSLSite
{
    public partial class DespachoVehiculos : System.Web.UI.Page
    {
        private usuario ClsUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }
                ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            int grabados = 0;
            int indexMRN = 11;
            int indexMSN = 12;
            int indexHSN = 13;

            foreach (GridViewRow row in gvContenedores.Rows)
            {
                CheckBox chk = row.FindControl("CHKFA") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    string contenedor = row.Cells[2].Text.Trim();
                    DropDownList ddl = row.FindControl("CboTurno") as DropDownList;
                    int idPlan = 0;
                    if (ddl != null)
                    {
                        int.TryParse(ddl.SelectedValue, out idPlan);
                    }
                    string mrn = ((Label)row.FindControl("lblMRN"))?.Text.Trim();
                    string msn = ((Label)row.FindControl("lblMSN"))?.Text.Trim();
                    string hsn = ((Label)row.FindControl("lblHSN"))?.Text.Trim();

                    string uuid = ((Label)row.FindControl("lblIdentificadorUnico"))?.Text.Trim();
                    string nombreNave = ((Label)row.FindControl("lblNombreNave"))?.Text.Trim();
                    int clienteId = int.TryParse(((Label)row.FindControl("lblClienteID"))?.Text.Trim(), out int cli) ? cli : 0;
                    string linea = ((Label)row.FindControl("lblLinea"))?.Text.Trim();
                    string tamano = ((Label)row.FindControl("lblTamano"))?.Text.Trim();
                    string ruc = ((Label)row.FindControl("lblImportadorID"))?.Text.Trim();
                    string nombreImportador = ((Label)row.FindControl("lblImportadorNombre"))?.Text.Trim();
                    string tipo = ((Label)row.FindControl("lblTipoContenedor"))?.Text.Trim();
                    decimal bultos = decimal.TryParse(((Label)row.FindControl("lblBultos"))?.Text, out decimal bul) ? bul : 0;
                    bool ridt = ((Label)row.FindControl("lblRIDT"))?.Text.Trim() == "1";
                    string manifiesto = ((Label)row.FindControl("lblManifiesto"))?.Text.Trim();
                    string bl = ((Label)row.FindControl("lblBL"))?.Text.Trim();
                    string buque = ((Label)row.FindControl("lblBuque"))?.Text.Trim();


                    if (InsertarContenedor(uuid, contenedor, nombreNave, clienteId, null, null, null, ClsUsuario?.loginname ?? "",
    linea, tamano, ruc, nombreImportador, null, tipo, bultos, ridt, manifiesto, bl, buque, mrn, msn, hsn)
 > 0)
                    {
                        grabados++;
                    }
                }
            }

            lblMensaje.Text = $"Se grabaron {grabados} contenedores.";
            lblMensaje.CssClass = "alert alert-success";
            lblMensaje.Visible = true;

            if (grabados > 0)
            {
                BtnBuscar_Click(sender, e);
            }
        }

        private int InsertarContenedor(
      string uuid,
      string numeroContenedor,
      string nombreNave,
      int clienteId,
      int? ordenTrabajoId,
      string vehiculo,
      string estado,
      string usuarioCreacion,
      string linea,
      string tamano,
      string ruc,
      string nombreImportador,
      DateTime? fechaCas,
      string tipoContenedor,
      decimal bultos,
      bool ridt,
      string manifiesto,
      string bl,
      string buque,
      string mrn,
      string msn,
      string hsn)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("[vhs].[insertar_contenedor]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdentificadorUnico", (object)uuid ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NumeroContenedor", (object)numeroContenedor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NombreNave", (object)nombreNave ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ClienteID", clienteId);
                cmd.Parameters.AddWithValue("@OrdenTrabajoID", (object)ordenTrabajoId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Vehiculo", (object)vehiculo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", (object)estado ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UsuarioCreacion", (object)usuarioCreacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Linea", (object)linea ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tamaño", (object)tamano ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ruc_importador", (object)ruc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nombre_importador", (object)nombreImportador ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fecha_cas", (object)fechaCas ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tipo_contenedor", (object)tipoContenedor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@bultos", bultos);
                cmd.Parameters.AddWithValue("@ridt", ridt);
                cmd.Parameters.AddWithValue("@manifiesto", (object)manifiesto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@bl", (object)bl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@buque", (object)buque ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MRN", (object)mrn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MSN", (object)msn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@HSN", (object)hsn ?? DBNull.Value);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null && int.TryParse(result.ToString(), out int id) ? id : 0;
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;

            if (string.IsNullOrEmpty(this.TXTMRN.Text))
            {
                lblMensaje.Text = "Debe ingresar el número de la carga MRN";
                lblMensaje.CssClass = "alert alert-danger";
                lblMensaje.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(this.TXTMSN.Text))
            {
                lblMensaje.Text = "Debe ingresar el número de la carga MSN";
                lblMensaje.CssClass = "alert alert-danger";
                lblMensaje.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                lblMensaje.Text = "Debe ingresar el número de la carga HSN";
                lblMensaje.CssClass = "alert alert-danger";
                lblMensaje.Visible = true;
                return;
            }

            try
            {
                ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
                var resultado = Validacion.CargaPorManifiestoImpo(ClsUsuario.loginname, TXTMRN.Text.Trim(), TXTMSN.Text.Trim(), TXTHSN.Text.Trim());

                if (!resultado.Exitoso)
                {
                    lblMensaje.Text = resultado.MensajeProblema;
                    lblMensaje.CssClass = "alert alert-danger";
                    lblMensaje.Visible = true;
                    return;
                }

                var datosOriginales = resultado.Resultado; // 🔸 Guardamos para join posterior

                var gkeys = Aduana.Importacion.ecu_validacion_cntr.CargaToListString(datosOriginales);
                if (!gkeys.Exitoso)
                {
                    lblMensaje.Text = gkeys.MensajeProblema;
                    lblMensaje.CssClass = "alert alert-danger";
                    lblMensaje.Visible = true;
                    return;
                }

                var contenedor = new N4.Importacion.container();
                var lista = contenedor.CargaPorKeys(ClsUsuario.loginname, gkeys.Resultado);
                if (!lista.Exitoso)
                {
                    lblMensaje.Text = lista.MensajeProblema;
                    lblMensaje.CssClass = "alert alert-danger";
                    lblMensaje.Visible = true;
                    return;
                }

                // 🔁 Hacemos el join entre datos de validación y de carga
                var query = from c in lista.Resultado
                            join r in datosOriginales on c.CNTR_CONTAINER equals r.cntr
                            select new Cls_Bil_Detalle
                            {
                                GKEY = (long)r.uuid,
                                MRN = r.mrn,
                                MSN = r.msn,
                                HSN = r.hsn,
                                CONTENEDOR = c.CNTR_CONTAINER,
                                REFERENCIA = r.referencia,
                                LINEA = r.agente,
                                TIPO = r.tipo,
                               
                                DOCUMENTO = r.documento_bl,
                                DESCRIPCION = r.descripcion,
                                CANTIDAD = Convert.ToDecimal(r.total_partida),
                                CNTR_VEPR_VSSL_NAME = r.referencia,
                                CNTR_VEPR_VOYAGE = "", // Puedes ajustar si tienes el valor
                                IDPLAN = "0",
                                TURNO = "0",
                                NUMERO_FACTURA = "",
                                VISTO = false,
                                FECHA_HASTA = null,
                                FECHA_ULTIMA = null
                            };

                Int16 sec = 1;
                var datos = query.ToList();

                foreach (var d in datos)
                {
                    d.SECUENCIA = sec++;
                }

                gvContenedores.DataSource = datos;
                gvContenedores.DataBind();
                CargarContenedoresDesdeVHS(TXTMRN.Text.Trim(), TXTMSN.Text.Trim(), TXTHSN.Text.Trim());

            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                lblMensaje.CssClass = "alert alert-danger";
                lblMensaje.Visible = true;
            }
        }



        protected void gvContenedoresVHS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                try
                {
                    long contenedorId = Convert.ToInt64(e.CommandArgument);
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                    using (SqlCommand cmd = new SqlCommand("vhs.eliminar_contenedor", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContenedorID", contenedorId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    // Refrescar la grilla
                    BtnBuscar_Click(sender, e);

                    lblMensaje.Text = "Contenedor eliminado correctamente.";
                    lblMensaje.CssClass = "alert alert-success";
                    lblMensaje.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error al eliminar contenedor: " + ex.Message;
                    lblMensaje.CssClass = "alert alert-danger";
                    lblMensaje.Visible = true;
                }
            }
        }

        private void CargarContenedoresDesdeVHS(string mrn, string msn, string hsn)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("vhs.validacion_cntr_impo_mrn_disponibles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mrn", mrn);
                    cmd.Parameters.AddWithValue("@msn", msn);
                    cmd.Parameters.AddWithValue("@hsn", hsn);
                

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    gvContenedoresVHS.DataSource = dt;
                    gvContenedoresVHS.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        panelContenedoresVHS.Visible = true;
                        gvContenedoresVHS.DataSource = dt;
                        gvContenedoresVHS.DataBind();
                    }
                    else
                    {
                        panelContenedoresVHS.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar datos VHS: " + ex.Message;
                lblMensaje.CssClass = "alert alert-danger";
                lblMensaje.Visible = true;
            }
        }

    }
}
