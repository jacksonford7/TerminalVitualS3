using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Security;
using System.Data;
using System.Globalization;
using BillionEntidades;
using System.Text;




namespace CSLSite
{
    public partial class p2d_facturafreightforwarder_preview : System.Web.UI.Page
    {

        usuario ClsUsuario;
        private Cls_Bil_Invoice_Impresion objFactura = new Cls_Bil_Invoice_Impresion();
      

        private string NombreUsuario = string.Empty;
        private string id_comp = string.Empty;
        string cMensaje = string.Empty;
        private int n_fila = 1;
        private string c_Subtotal = string.Empty;
        private string c_Iva = string.Empty;
        private string c_Total = string.Empty;


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

            if (!IsPostBack)
            {
                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    NombreUsuario = ClsUsuario.nombres + " " + ClsUsuario.apellidos;
                }

                id_comp = QuerySegura.DecryptQueryString(Request.QueryString["id_comprobante"]);
              
               
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
                        /*Recuperar toda la referencia*/
                        Int64 id = 0;

                        id_comp = id_comp.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(id_comp, out id))
                        {
                            return;
                        }

                        List<P2D_MULTI_Imprimir> ListFactura = P2D_MULTI_Imprimir.Datos_Factura(id, out cMensaje);

                        if (ListFactura != null)
                        {
                            if (ListFactura.Count <= 0)
                            {
                                // No existen registros de proformas para mostrar
                            }

                            StringBuilder tab = new StringBuilder();
                            tab.Append("<table class='print_table'>");
                            tab.Append("<thead>" +
                                  "<tr>" +
                                  "<td><strong>Código</strong></td> " +
                                  "<td><strong>Descripción</strong></td> " +
                                  "<td align='right'><strong>Cantidad</strong></td>" +
                                  "<td align='right'><strong>V.Unit.</strong></td>" +
                                  "<td align='right'><strong>V.Total</strong></td>" +
                                  "</tr>" +
                                "<thead>");
                            tab.Append("<tbody>");

                            foreach (var pf in ListFactura)
                            {

                                tab.AppendFormat("<tr>" +
                                    "<td>{0}</td>" +
                                    "<td>{1}</td>" +
                                    "<td align='right'>{2}</td>" +
                                    "<td align='right'>{3}</td>" +
                                    "<td align='right'>{4}</td>" +
                                    "</tr>",
                                    String.IsNullOrEmpty(pf.IV_TIPO) ? "..." : pf.IV_TIPO,
                                    String.IsNullOrEmpty(pf.IV_DESC_SERVICIO) ? "..." : pf.IV_DESC_SERVICIO,
                                    pf.IV_CANTIDAD == 0 ? "..." : string.Format("{0:N2}", pf.IV_CANTIDAD),
                                    pf.IV_PRECIO == 0 ? "..." : string.Format("{0:c}", pf.IV_PRECIO),
                                    pf.IV_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.IV_SUBTOTAL)
                                    );

                                if (n_fila == 1)
                                {
                                    c_Subtotal = pf.TOT_IV_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.TOT_IV_SUBTOTAL);
                                    c_Iva = pf.TOT_IV_IVA == 0 ? "..." : string.Format("{0:c}", pf.TOT_IV_IVA);
                                    c_Total = pf.TOT_IV_TOTAL == 0 ? "..." : string.Format("{0:c}", pf.TOT_IV_TOTAL);

                                    cliente.InnerText = String.Format("{0}",  pf.IV_DESC_FACTURADO);

                                    ciudad.InnerText = String.Format("{0}", pf.IV_CIUDAD);

                                    agente_aduana.InnerText = String.Format("[{0}] - {1}   -   {2}", pf.IV_ID_AGENTE,pf.IV_DESC_AGENTE, pf.IV_DOCUMENTO);

                                    ruc.InnerText = String.Format("{0}", pf.IV_ID_FACTURADO);

                                    ruc.InnerText = String.Format("{0}", pf.IV_ID_FACTURADO);

                                    buque.InnerText = String.Format("{0}", pf.IV_NUMERO_CARGA);

                                    numero_factura.InnerText = String.Format("{0}", pf.IV_COMPROBANTE);
                                    numero_liquidacion.InnerText = String.Format("{0}", pf.IV_LIQUIDACION);

                                    numero_despacho.InnerText = String.Format("{0}", pf.NUMERO_DESPACHO);

                                    fecha_factura.InnerText = String.Format("{0}", pf.IV_FECHA);
                                    

                                    this.fechagenera.InnerText = string.Format("{0}", pf.IV_FECHA);
                                    this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                                }


                                n_fila++;
                            }

                            tab.Append("</tbody>" +
                                  "<tfoot>" +
                                  "<tr><td colspan='5'><div class='lineadiv'><div></td></tr>" +
                                  "<tr><td></td><td></td><td></td><td></td><td></td></tr>" +
                                  "<tr><td colspan='3' rowspan='5' class='auto-style1'>Este documento hace referencia a la FACTURA No "+ numero_factura.InnerText + " y no tiene validez legal alguna. <br/>" +
                                  "La factura autorizada por el SRI la podrá visualizar y descargar en nuestro portal e-billing ingresando al <br/>" +
                                  "siguiente link: http://contecongye.e-custodia.com.ec <br/><br/>" +
                                  "Debo y pagaré incondicionalmente a la orden de Contecon Guayaquil S.A. en el lugar y fecha que se me reconvenga,<br/>" +
                                  "el valor  total expresado  en este documento, más los  impuestos legales respectivos en  Dólares de <br/>" +
                                  "los Estados Unidos de América, por los bienes y/o servicios que he recibido a mi entera satisfacción." +
                                  "</td>" +
                                  "<td align='right'><strong>SUBTOTAL</strong></td>" +
                                  "<td align='right'><strong><span id='Subtotal' runat='server' class='labelprint'>" + c_Subtotal + "</span></strong></td>" +
                                  "</tr>");

                            tab.Append("<tr><td align='right'><strong>IVA 0%</strong></td>"+
                                "<td align='right'><strong><span id='IvaCero' runat='server' class='labelprint'>$0.00</span></strong></td>" +
                                "</tr>"+
                                "<tr><td align='right'><strong>IVA 15%</strong></td>"+
                                "<td align='right'><strong><span id='Iva' runat='server' class='labelprint'>" + c_Iva + "</span></strong></td></tr>");

                            tab.Append("<tr><td align='right'><strong>TOTAL</strong></td>" +
                                   "<td align='right'><strong><span id='Total' runat='server' class='labelprint'>" + c_Total + "</span></strong></td>" +
                                   "</tr><tr><td></td><td></td></tr></tfoot>");

                            tab.Append("</table>");

                            this.detalle_data.InnerHtml = tab.ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

        }
    }
}