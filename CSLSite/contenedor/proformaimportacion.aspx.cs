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


namespace CSLSite
{
    public partial class proformaimportacion : System.Web.UI.Page
    {
        usuario ClsUsuario;
        private Cls_Bil_Proforma_Consulta objProforma = new Cls_Bil_Proforma_Consulta();
        private Int64 id_proforma = 0;
        private string c_id_proforma = string.Empty;
        private string c_Subtotal = string.Empty;
        private string c_Iva = string.Empty;
        private string c_Total = string.Empty;
        private int n_fila = 1;
        string cMensaje = string.Empty;


        protected string jsarguments(object carga, object facturado, object agente, object cliente, object sesion, object valor)
        {
            return string.Format("{0}+{1}+{2}+{3}+{4}+{5}", carga != null ? carga.ToString().Trim() : string.Empty, facturado != null ? facturado.ToString().Trim() : string.Empty, agente != null ? agente.ToString().Trim() : string.Empty, cliente != null ? cliente.ToString().Trim() : string.Empty, sesion != null ? sesion.ToString().Trim() : "0", valor != null ? valor.ToString().Trim() : "0");
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();

        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);
               
                return;
            }

            this.banmsg.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
            }

            try
            {

                c_id_proforma = QuerySegura.DecryptQueryString(Request.QueryString["id_proforma"]);
                if (Request.QueryString["id_proforma"] == null || string.IsNullOrEmpty(c_id_proforma))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para proformar"));
                    Response.Redirect("~/contenedorimportacion.aspx", false);
                    return;
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));

            }



        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                if (!IsPostBack)
                {
                    try
                    {
                        this.banmsg.InnerText = string.Empty;

                        ClsUsuario = Page.Tracker();
                        if (ClsUsuario == null)
                        {
                            return;
                        }

                        c_id_proforma = c_id_proforma.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(c_id_proforma, out id_proforma))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error al convertir {0}", c_id_proforma));
                            return;
                        }

                        List<Cls_Bil_Proforma_Consulta> ListProforma = Cls_Bil_Proforma_Consulta.List_Proforma(id_proforma, out cMensaje);
                        if (ListProforma != null)
                        {
                            if (ListProforma.Count <= 0)
                            {
                                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para proformar"));
                                return;
                            }

                            StringBuilder tab = new StringBuilder();
                            tab.Append("<table class='table table-bordered invoice'>");
                            tab.Append("<thead>" +
                                  "<tr>" +
                                  "<th class='text-center'>CODIGO</th>" +
                                  "<th class='text-left'>DESCRIPCION</th>" +
                                  "<th class='text-center'>CANTIDAD</th>" +
                                  "<th class='text-right'>V.UNITARIO</th>" +
                                  "<th class='text-right'>V.TOTAL</th>" +
                                  "</tr>" +
                                "<thead>");
                            tab.Append("<tbody>");
                           
                            foreach (var pf in ListProforma)
                            {

                                tab.AppendFormat("<tr>" +
                                    "<td class='text-center'>{0}</td>" +
                                    "<td>{1}</td>" +
                                    "<td class='text-center'>{2}</td>"+
                                    "<td class='text-right'>{3}</td>" +
                                    "<td class='text-right'>{4}</td>" +
                                    "</tr>",
                                    String.IsNullOrEmpty(pf.PF_ID_SERVICIO) ? "..." : pf.PF_ID_SERVICIO,
                                    String.IsNullOrEmpty(pf.PF_DESC_SERVICIO) ? "..." : pf.PF_DESC_SERVICIO,
                                    pf.PF_SER_CANTIDAD == 0 ? "..." : string.Format("{0:N2}", pf.PF_SER_CANTIDAD),
                                    pf.PF_SER_PRECIO == 0 ? "..." : string.Format("{0:c}", pf.PF_SER_PRECIO),
                                    pf.PF_SER_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.PF_SER_SUBTOTAL)
                                    );

                                if (n_fila == 1)
                                {
                                    c_Subtotal = pf.PF_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.PF_SUBTOTAL);
                                    c_Iva = pf.PF_IVA == 0 ? "..." : string.Format("{0:c}", pf.PF_IVA);
                                    c_Total = pf.PF_TOTAL == 0 ? "..." : string.Format("{0:c}", pf.PF_TOTAL);

                                    agente.InnerText = String.Format("AGENTE ADUANERO: [{0}] - {1}", pf.PF_ID_AGENTE,pf.PF_DESC_AGENTE);
                                    hf_idagente.Value = String.Format("{0}", pf.PF_CODIGO_AGENTE);
                                    cliente.InnerText = String.Format("CONSIGNATARIO: [{0}] - {1}", pf.PF_ID_CLIENTE, pf.PF_DESC_CLIENTE);
                                    hf_idcliente.Value = String.Format("{0}", pf.PF_ID_CLIENTE);
                                    facturado.InnerText = String.Format("FACTURADO A: [{0}] - {1}", pf.PF_ID_FACTURADO, pf.PF_DESC_FACTURADO);
                                    hf_idfacturado.Value = String.Format("{0}", pf.PF_ID_FACTURADO);
                                    observacion.InnerText = String.Format("OBSERVACIONES: {0}", pf.PF_GLOSA);
                                    carga.InnerText = String.Format("NUMERO DE CARGA: {0}", pf.PF_NUMERO_CARGA);
                                    hf_idcarga.Value = String.Format("{0}", pf.PF_NUMERO_CARGA);
                                    contenedor.InnerText = String.Format("CONTENEDORES: {0}", pf.PF_CONTENEDOR);

                                    numero.InnerText = String.Format("{0}", pf.PF_ID.ToString("D8"));
                                    fecha.InnerText = String.Format("{0}", pf.PF_FECHA_CREA.HasValue ? pf.PF_FECHA_CREA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                                    fecha_hasta.InnerText = String.Format("{0}", pf.PF_FECHA_HASTA.HasValue ? pf.PF_FECHA_HASTA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                                    fecha_hasta.Visible = false;
                                    total.InnerText = String.Format("{0} USD", c_Total);

                                    hf_BrowserWindowName.Value = String.Format("{0}", pf.PF_SESION);
                                }
                              

                                n_fila++;
                            }
                          
                            tab.Append("<tr><td colspan = '3' rowspan = '4'>" +
                                    "<h4>Términos y condiciones</h4>" +
                                    "<p>Este documento no tiene validez legal alguna, la proforma puede variar costos de los servicios en base a la fecha de generación de la misma.</p>" +
                                    "<td class='text-right'><strong>Subtotal</strong></td>" +
                                    "<td class='text-right'><strong>" + c_Subtotal + "</strong></td></td></tr>");

                            tab.Append("<tr><td class='text-right no-border'><strong>Iva 15%</strong></td>" +
                                   "<td class='text-right'><strong>" + c_Iva + "</strong></td></tr>");

                            tab.Append("<tr><td class='text-right no-border'>" +
                                   "<div class='well well-small rojo'><strong>Total</strong></div>" +
                                   "</td>" +
                                   "<td class='text-right'><strong>" + c_Total + "</strong></td></tr>");

                            tab.Append("<tbody>");
                            tab.Append("</table>");

                            this.detalle.InnerHtml = tab.ToString();
                            this.Actualiza_Paneles();

                        }

                    }
                    catch (Exception ex)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
                    }
                }

            }
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            string ID_CARGA = jsarguments(this.hf_idcarga.Value.Replace("-","+"), this.hf_idfacturado.Value, this.hf_idagente.Value, this.hf_idcliente.Value, this.hf_BrowserWindowName.Value, this.hf_BrowserWindowName.Value);
            Response.Redirect("~/contenedor/contenedorimportacion.aspx?ID_CARGA="+ securetext(ID_CARGA), false);
        }

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {

            
            c_id_proforma = QuerySegura.DecryptQueryString(Request.QueryString["id_proforma"]);

            if (Request.QueryString["id_proforma"] == null || string.IsNullOrEmpty(c_id_proforma))
            {

                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..campo c_id_proforma null {0}", c_id_proforma));
                return;
            }
            else
            {
                c_id_proforma = c_id_proforma.Trim().Replace("\0", string.Empty);

                if (!Int64.TryParse(c_id_proforma, out id_proforma))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error al convertir {0}", c_id_proforma));
                    return;
                }
                else
                {
                    string cId = securetext(id_proforma.ToString());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../reportes/proforma_preview.aspx?id_comprobante=" + cId + "');", true);
                    this.Ocultar_Mensaje();
                }
            }

           

          
        }
    }
}