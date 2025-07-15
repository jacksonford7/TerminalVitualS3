using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillionEntidades;

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

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TXTMRN.Text))
            {
                this.banmsg.InnerText = "Debe ingresar el número de la carga MRN";
                this.banmsg.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(this.TXTMSN.Text))
            {
                this.banmsg.InnerText = "Debe ingresar el número de la carga MSN";
                this.banmsg.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                this.banmsg.InnerText = "Debe ingresar el número de la carga HSN";
                this.banmsg.Visible = true;
                return;
            }
            this.banmsg.Visible = false;

            try
            {
                ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
                var resultado = Validacion.CargaPorManifiestoImpo(ClsUsuario.loginname, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                if (!resultado.Exitoso)
                {
                    this.banmsg.InnerText = resultado.MensajeProblema;
                    this.banmsg.Visible = true;
                    return;
                }
                var gkeys = Aduana.Importacion.ecu_validacion_cntr.CargaToListString(resultado.Resultado);
                if (!gkeys.Exitoso)
                {
                    this.banmsg.InnerText = gkeys.MensajeProblema;
                    this.banmsg.Visible = true;
                    return;
                }
                var contenedor = new N4.Importacion.container();
                var lista = contenedor.CargaPorKeys(ClsUsuario.loginname, gkeys.Resultado);
                if (!lista.Exitoso)
                {
                    this.banmsg.InnerText = lista.MensajeProblema;
                    this.banmsg.Visible = true;
                    return;
                }
                var query = from c in lista.Resultado
                            select new Cls_Bil_Detalle
                            {
                                CONTENEDOR = c.CNTR_CONTAINER,
                                IN_OUT = c.CNTR_YARD_STATUS,
                                DOCUMENTO = c.CNTR_DOCUMENT,
                                CAS = c.FECHA_CAS,
                                FECHA_HASTA = null,
                                FECHA_ULTIMA = null,
                                NUMERO_FACTURA = string.Empty,
                                IDPLAN = "0",
                                TURNO = "0",
                                VISTO = false
                            };
                Int16 sec = 1;
                var datos = query.ToList();
                foreach (var d in datos)
                {
                    d.SECUENCIA = sec++;
                }
                gvContenedores.DataSource = datos;
                gvContenedores.DataBind();
            }
            catch (Exception ex)
            {
                this.banmsg.InnerText = ex.Message;
                this.banmsg.Visible = true;
            }
        }
    }
}
