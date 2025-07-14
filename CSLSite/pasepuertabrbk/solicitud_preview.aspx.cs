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




namespace CSLSite.pasepuertabrbk
{
    public partial class solicitud_preview : System.Web.UI.Page
    {

        usuario ClsUsuario;
        private Cls_Bil_Invoice_Impresion objFactura = new Cls_Bil_Invoice_Impresion();
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
                            return;
                        }

                        List<brbk_imprime_solicitud> ListFactura = brbk_imprime_solicitud.Imprime_Solicitud(id, out cMensaje);
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
                                  "<td align='center'><strong>FECHA</strong></td>" +
                                  "<td align='center'><strong>TURNO</strong></td>" +
                                  "<td align='center'><strong># VEHICULOS</strong></td>" +
                                  "<td align='center'><strong>CANT. X VEH</strong></td>" +
                                  "<td align='center'><strong>TOTAL BULTOS</strong></td>" +
                                  "<td align='center'><strong>TRANSPORTISTA</strong></td>" +
                                  "</tr>" +
                                "<thead>");
                            tab.Append("<tbody>");

                            foreach (var pf in ListFactura)
                            {

                                tab.AppendFormat("<tr>" +
                                    "<td align='center'>{0}</td>" +
                                    "<td align='center'>{1}</td>" +
                                    "<td align='center'>{2}</td>" +
                                    "<td align='center'>{3}</td>" +
                                    "<td align='center'>{4}</td>" +
                                    "<td align='center'>{5}</td>" +
                                    "</tr>",
                                    !pf.FECHA_EXPIRACION.HasValue ? "..." : pf.FECHA_EXPIRACION.Value.ToString("dd/MM/yyyy"),
                                    String.IsNullOrEmpty(pf.HORARIO) ? "..." : pf.HORARIO,
                                    pf.CANTIDAD_VEHICULOS == 0 ? "..." : string.Format("{0:N0}", pf.CANTIDAD_VEHICULOS),
                                    pf.CANTIDAD_CARGA == 0 ? "..." : string.Format("{0:N0}", pf.CANTIDAD_CARGA),
                                    pf.TOTAL_CARGA == 0 ? "..." : string.Format("{0:N0}", pf.TOTAL_CARGA),
                                    String.IsNullOrEmpty(pf.ID_EMPRESA) ? "..." : string.Format("{0} - {1}",pf.ID_EMPRESA, pf.TRANSPORTISTA_DESC)
                                    );

                                if (n_fila == 1)
                                {
                                    numero_factura.InnerText = String.Format("{0}", pf.NUMERO_SOLICITUD);
                                    fecha_factura.InnerText = String.Format("{0}", pf.FECHA_SOL.Value.ToString("dd/MM/yyyy"));
                                    numero_carga.InnerText = String.Format("{0}", pf.NUMERO_CARGA);
                                    agente.InnerText = String.Format("{0}", pf.AGENTE_DESC);
                                    importador.InnerText = String.Format("{0}", pf.IMPORTADOR_DESC);
                                    estado.InnerText = String.Format("{0}", pf.ESTADO);
                                    tipo_turno.InnerText = String.Format("{0}", pf.TIPO_TURNO);

                                    total_pases.InnerText = pf.CANTIDAD_VEHI == 0 ? "..." : string.Format("{0:N0}", pf.CANTIDAD_VEHI);
                                    total_bultos.InnerText = pf.TOTAL_BUTLOS == 0 ? "..." : string.Format("{0:N0}", pf.TOTAL_BUTLOS);

                                
                                 
                                    //agente_aduana.InnerText = String.Format("[{0}] - {1}   -   {2}", pf.IV_ID_AGENTE,pf.IV_DESC_AGENTE, pf.IV_DOCUMENTO);

                                    this.fechagenera.InnerText = string.Format("{0}", pf.FECHA_SOL);
                                    this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                                }


                                n_fila++;
                            }

                            tab.Append("</tbody>" +
                                  "<tfoot>" +
                                  "<tr><td colspan='6'><div class='lineadiv'><div></td></tr>" +
                                  "<tr><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                                 

                            tab.Append("</tfoot>");

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