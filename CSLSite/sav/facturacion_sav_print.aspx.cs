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




namespace CSLSite.reportes
{
    public partial class facturacion_sav_print : System.Web.UI.Page
    {

        usuario ClsUsuario;
       

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

                id_comp = Request.QueryString["id_comprobante"];
              
               
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
                        id_comp = id_comp.Trim().Replace("\0", string.Empty);

                        List<SAV_Imprimir_Factura> ListFactura = SAV_Imprimir_Factura.Carga_Factura_Sav(id_comp, out cMensaje);
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
                                    String.IsNullOrEmpty(pf.ID_SERVICIO) ? "..." : pf.ID_SERVICIO,
                                    String.IsNullOrEmpty(pf.DESC_SERVICIO) ? "..." : pf.DESC_SERVICIO,
                                     pf.CANTIDAD.Value == 0 ? "..." : string.Format("{0:N2}", pf.CANTIDAD.Value),
                                     pf.PRECIO.Value == 0 ? "..." : string.Format("{0:N3}", pf.PRECIO.Value),
                                     pf.SUBTOTAL.Value == 0 ? "..." : string.Format("{0:c}", pf.SUBTOTAL.Value)
                                    );


                                if (n_fila == 1)
                                {
                                    c_Subtotal = pf.CAB_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.CAB_SUBTOTAL);
                                    c_Iva = pf.CAB_IVA == 0 ? "..." : string.Format("{0:c}", pf.CAB_IVA);
                                    c_Total = pf.CAB_TOTAL == 0 ? "..." : string.Format("{0:c}", pf.CAB_TOTAL);

                                    cliente.InnerText = String.Format("{0}",  pf.DESC_FACTURADO);
                                    direccion.InnerText = String.Format("{0}", pf.DIR_FACTURADO);
                                    ciudad.InnerText = String.Format("{0}", pf.CIUDAD_FACTURADO);
                                    provincia.InnerText = String.Format("{0}", string.Empty);

                                    cajas_bodega.InnerText = String.Format("{0}", pf.TOTAL_CONTENEDORES);
                                 
                                    referencia.InnerText = String.Format("{0}", pf.CONTENEDORES);

                                    ruc.InnerText = String.Format("{0}", pf.ID_CLIENTE);

                                    

                                    numero_factura.InnerText = String.Format("{0}", pf.NUMERO_FACTURA);
                                    numero_liquidacion.InnerText = String.Format("{0}", pf.NUMERO_FACTURA);

                                    fecha_factura.InnerText = String.Format("{0}", pf.FECHA.HasValue ? pf.FECHA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                                    fecha_vencimiento.InnerText = String.Format("{0}", pf.FECHA.HasValue ? pf.FECHA.Value.ToString("dd/MM/yyyy HH:mm") : "...");

                                 

                                    this.fechagenera.InnerText = string.Format("{0}", pf.FECHA);
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