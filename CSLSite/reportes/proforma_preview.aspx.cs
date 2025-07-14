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
    public partial class proforma_preview : System.Web.UI.Page
    {

        usuario ClsUsuario;
        private Cls_Bil_Proforma_Impresion objProforma = new Cls_Bil_Proforma_Impresion();
        //private Int64 id_proforma = 0;

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
                            
                        }

                        List<Cls_Bil_Proforma_Impresion> ListProforma = Cls_Bil_Proforma_Impresion.Datos_Proforma(id, out cMensaje);
                        if (ListProforma != null)
                        {
                            if (ListProforma.Count <= 0)
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

                            foreach (var pf in ListProforma)
                            {

                                tab.AppendFormat("<tr>" +
                                    "<td>{0}</td>" +
                                    "<td>{1}</td>" +
                                    "<td align='right'>{2}</td>" +
                                    "<td align='right'>{3}</td>" +
                                    "<td align='right'>{4}</td>" +
                                    "</tr>",
                                    String.IsNullOrEmpty(pf.PF_ID_SERVICIO) ? "..." : pf.PF_ID_SERVICIO,
                                    String.IsNullOrEmpty(pf.PF_DESC_SERVICIO) ? "..." : pf.PF_DESC_SERVICIO,
                                    pf.PF_CANTIDAD == 0 ? "..." : string.Format("{0:N2}", pf.PF_CANTIDAD),
                                    pf.PF_PRECIO == 0 ? "..." : string.Format("{0:c}", pf.PF_PRECIO),
                                    pf.PF_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.PF_SUBTOTAL)
                                    );

                                if (n_fila == 1)
                                {
                                    c_Subtotal = pf.TOT_PF_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.TOT_PF_SUBTOTAL);
                                    c_Iva = pf.TOT_PF_IVA == 0 ? "..." : string.Format("{0:c}", pf.TOT_PF_IVA);
                                    c_Total = pf.TOT_PF_TOTAL == 0 ? "..." : string.Format("{0:c}", pf.TOT_PF_TOTAL);

                                    cliente.InnerText = String.Format("[{0}] - {1}", pf.PF_ID_FACTURADO, pf.PF_DESC_FACTURADO);
                                    observacion.InnerText = String.Format("{0}", pf.PF_GLOSA);
                                    numero_carga.InnerText = String.Format("{0}", pf.PF_NUMERO_CARGA);
                                    contenedores.InnerText = String.Format("{0}", pf.PF_CONTENEDOR);

                                    numero_proforma.InnerText = String.Format("PROFORMA # {0}", pf.PF_NUMERO);
                                    fechaemision.InnerText = String.Format("{0}", pf.PF_FECHA);
                                    this.fechagenera.InnerText = string.Format("{0}", pf.PF_FECHA);
                                    this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                                }


                                n_fila++;
                            }

                            tab.Append("</tbody>" +
                                  "<tfoot>" +
                                  "<tr><td colspan='5'><div class='lineadiv'><div></td></tr>" +
                                  "<tr><td></td><td></td><td></td><td></td><td></td></tr>" +
                                  "<tr><td></td><td>Generado por: "+ NombreUsuario + "</td><td></td>"+ 
                                  "<td align='right'><strong>SUBTOTAL $</strong></td>" +
                                  "<td align='right'><strong><span id='Subtotal' runat='server' class='labelprint'>" + c_Subtotal + "</span></strong></td>" +
                                  "</tr>");

                            tab.Append("<tr><td></td><td></td><td></td><td align='right'><strong>IVA 0%</strong></td>"+
                                "<td align='right'><strong><span id='IvaCero' runat='server' class='labelprint'>$0.00</span></strong></td>" +
                                "</tr>"+
                                "<tr><td></td><td></td><td></td><td align='right'><strong>IVA 15%</strong></td>"+
                                "<td align='right'><strong><span id='Iva' runat='server' class='labelprint'>" + c_Iva + "</span></strong></td></tr>");

                            tab.Append("<tr><td></td><td></td><td></td><td align='right'><strong>TOTAL $</strong></td>" +
                                   "<td align='right'><strong><span id='Total' runat='server' class='labelprint'>" + c_Total + "</span></strong></td>" +
                                   "</tr><td><td></td><td></td><td></td><td></td><td></td></tr></tfoot>");

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