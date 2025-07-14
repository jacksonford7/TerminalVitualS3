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
    public partial class p2d_proforma_preview : System.Web.UI.Page
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

                        List<P2D_Imprimir_Proforma> ListProforma = P2D_Imprimir_Proforma.Datos_Proforma(id, out cMensaje);
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
                                  "<td><strong>PESO TOTAL TONLEADAS</strong></td> " +
                                  "<td><strong>PESO VOLÚMETRICO METROS</strong></td> " +
                                  "<td><strong></strong></td> " +
                                  "<td><strong></strong></td> " +
                                  "<td align='right'><strong>TOTAL A PAGAR</strong></td>" +
                                  "</tr>" +
                                "<thead>");
                            tab.Append("<tbody>");

                            foreach (var pf in ListProforma)
                            {

                                tab.AppendFormat("<tr>" +
                                   "<td>{0}</td>" +
                                   "<td>{1}</td>" +
                                   "<td>{2}</td>" +
                                   "<td>{3}</td>" +
                                   "<td align='right'>{4}</td>" +
                                   "</tr>",
                                  pf.TOTAL_TN == 0 ? "..." : string.Format("{0:N2}", pf.TOTAL_TN),
                                   pf.TOTAL_M3 == 0 ? "..." : string.Format("{0:N2}", pf.TOTAL_M3),
                                   "",
                                   "",
                                   pf.TOTAL_PAGAR == 0 ? "..." : string.Format("{0:c}", pf.TOTAL_PAGAR)
                                   );

                                if (n_fila == 1)
                                {
                                    c_Total = pf.TOTAL_PAGAR == 0 ? "..." : string.Format("{0:c}", pf.TOTAL_PAGAR);

                                    cliente.InnerText = String.Format("{0} {1}", pf.NOMBRES, pf.APELLIDOS);
                                    tiposervicio.InnerText = String.Format("{0}", (pf.EXPRESS == true ? "EXPRESS" : "NORMAL"));
                                    ciudad.InnerText = String.Format("CIUDAD: {0}", pf.DESC_CIUDAD);
                                    zona.InnerText = String.Format("ZONA: {0}", pf.ZONA);
                                    direccion.InnerText = String.Format("DIRECCIÓN: {0}", pf.DIR_ENTREGA);
                                    numero_carga.InnerText = String.Format("{0}-{1}-{2}", pf.MRN, pf.MSN, pf.HSN);
                                    //contenedores.InnerText = String.Format("{0}", pf.PF_CONTENEDOR);

                                    numero_proforma.InnerText = String.Format("PROFORMA # {0}", pf.ID_PROFORMA);
                                    fechaemision.InnerText = String.Format("{0}", pf.FECHA);
                                    this.fechagenera.InnerText = string.Format("{0}", pf.FECHA);
                                    this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                                }


                                n_fila++;
                            }

                            tab.Append("</tbody>" +
                                  "<tfoot>" +
                                  "<tr><td colspan='5'><div class='lineadiv'><div></td></tr>" +
                                  "<tr><td></td><td></td><td></td><td></td><td></td></tr>" +
                                  "<tr><td></td><td>Generado por: "+ NombreUsuario + "</td><td></td>"+ 
                                  "<td align='right'><strong></strong></td>" +
                                  "<td align='right'><strong><span id='Subtotal' runat='server' class='labelprint'></span></strong></td>" +
                                  "</tr>");

                         
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