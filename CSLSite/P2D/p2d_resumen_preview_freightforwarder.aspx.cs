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
    public partial class p2d_resumen_preview_freightforwarder : System.Web.UI.Page
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
                       
                        id_comp = id_comp.Trim().Replace("\0", string.Empty);


                        List<P2D_MULTI_cfs_imprimir_pase> ListPase = P2D_MULTI_cfs_imprimir_pase.Datos_Pase(id_comp, ClsUsuario.ruc, System.DateTime.Now, out cMensaje);

                        if (ListPase != null)
                        {
                            if (ListPase.Count <= 0)
                            {
                                // No existen registros de proformas para mostrar
                            }

                            StringBuilder tab = new StringBuilder();
                            tab.Append("<table class='print_table'>");
                            tab.Append("<thead>" +
                                  "<tr>" +
                                  "<td  align='center'><strong># PASE</strong></td> " +
                                  "<td  align='center'><strong>CODIGO BARRA</strong></td> " +
                                  "<td  align='center'><strong># CARGA</strong></td>" +
                                  "<td  align='center'><strong>IMPORTADOR</strong></td>" +
                                  "<td  align='center'><strong>TURNO</strong></td>" +
                                  "<td  align='center'><strong>DIRECCION</strong></td>" +
                                  "</tr>" +
                                "<thead>");
                            tab.Append("<tbody>");

                            foreach (var pf in ListPase)
                            {

                                tab.AppendFormat("<tr>" +
                                    "<td  align='center'>{0}</td>" +
                                    "<td  align='center'><img src='{1}'></td>" +
                                    "<td  align='center'>{2}</td>" +
                                    "<td  align='center'>{3}</td>" +
                                    "<td  align='center'>{4}</td>" +
                                    "<td  align='center'>{5}</td>" +
                                    "</tr>",
                                    pf.ID_PASE==0 ? "..." : pf.ID_PASE.ToString(),
                                    string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=250&height=60&size=50", pf.ID_PASE),
                                    String.IsNullOrEmpty(pf.NUMERO_CARGA) ? "..." : pf.NUMERO_CARGA,
                                    
                                    string.Format("{0} - {1}",pf.IMPORTADOR, pf.IMPORTADOR_DESC),
                                    string.Format("{0} </br> {1}", pf.FECHA_EXPIRACION.ToString("dd/MM/yyyy"), pf.D_TURNO),
                                    pf.DIRECCION
                                    );

                                if (n_fila == 1)
                                {
                                  
                                    cliente.InnerText = String.Format("{0}",  pf.AGENTE_DESC);
                                   
                                    ruc.InnerText = String.Format("{0}", pf.AGENTE);

                                    numero_factura.InnerText = String.Format("{0}", pf.ORDEN);
                                    barra_id.InnerText = String.Format("{0}", pf.ORDEN);

                                    this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=600&height=60&size=50", pf.ORDEN);
                                   
                                    fecha_factura.InnerText = String.Format("{0}", pf.FECHA_EXPIRACION.ToString("dd/MM/yyyy"));
                                    
                                    this.fechagenera.InnerText = string.Format("{0}", pf.FECHA_EXPIRACION.ToString("dd/MM/yyyy"));
                                    this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                                    this.id_transportista.InnerText = string.Format("{0} - {1}", pf.CIATRANS, pf.TRANSPORTISTA_DESC);

                                    this.numero_factura_servicio.InnerText = String.Format("{0}", pf.FACTURA_SERVICIO);
                                }
                                n_fila++;
                            }

                            tab.Append("</tbody>" +
                                  "<tfoot>" +
                                  "<tr><td colspan='5'><div class='lineadiv'><div></td></tr>" +
                                  "<tr><td></td><td></td><td></td><td></td><td></td><td></td></tr>"
                                 );

                          

                            tab.Append("<tr><td></td><td></td></tr></tfoot>");

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