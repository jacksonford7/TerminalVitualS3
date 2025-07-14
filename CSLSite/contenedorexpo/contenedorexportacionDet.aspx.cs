using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.contenedorexpo
{
    public partial class contenedorexportacionDet : System.Web.UI.Page
    {
        private List<Cls_Bill_CabeceraExpo> objCabecera = new List<Cls_Bill_CabeceraExpo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
            if (objCabecera == null) { return; }
                if (!IsPostBack)
            {
                string EID = Request.QueryString["id"];

                txtID.Text = EID;

                tablePagination.DataSource = objCabecera.Where(p => p.CNTR_BKNG_BOOKING == EID).FirstOrDefault().Detalle;
                tablePagination.DataBind();
            }
        }

        

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void tablePagination_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (tablePagination.Rows.Count > 0)
                {
                    tablePagination.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    tablePagination.HeaderRow.TableSection = TableRowSection.TableHeader;

                    // Agrega el elemento TH en la fila de encabezado.               
                    // Agrega la sección TFOOT. 
                    //tablePagination.FooterRow.TableSection = TableRowSection.TableFooter;
                }

            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }
        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;

                if (objCabecera == null)
                {
                    //this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    tablePagination.PageIndex = e.NewPageIndex;
                    tablePagination.DataSource = objCabecera.Where(p => p.CNTR_BKNG_BOOKING == txtID.Text).FirstOrDefault().Detalle;
                    tablePagination.DataBind();
                    //this.Actualiza_Panele_Detalle();
                }

            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }
    }
}