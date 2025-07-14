using BillionEntidades.Entidades;
using CLSiteUnitLogic.Cls_CargaSuelta;
using CLSiteUnitLogic.Cls_Container;
using CLSiteUnitLogic.Cls_pase_puerta;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Font = iTextSharp.text.Font;
using Rectangule = iTextSharp.text.Rectangle;


namespace CLSiteUnitLogic
{
    public class LogicXml
    {


        public static void ExportarXML(string tipo)
        {
            XElement root;

            if (tipo == "Cntr")
            {
                var lista = HttpContext.Current.Session["DatosContenedorCntr"] as List<ContenedorVista>;
                if (lista == null || lista.Count == 0) return;

                root = new XElement("Contenedores",
                    lista.Select(c => new XElement("Contenedor",
                        new XElement("VigenciaDeCas", c.CAS?.ToString("dd/MM/yyyy") ?? ""),
                        new XElement("Contenedor", c.CONTENEDOR),
                        new XElement("Categoria", c.Category),
                        new XElement("Estado", c.TIPO_STATE),
                        new XElement("Naviera", c.LINE_OP),
                        new XElement("ReferenciaNave", c.IB_ACTUAL_VISIT),
                        new XElement("TipoCarga", c.FRGHT_KIND)
                    ))
                );
            }
            else if (tipo == "CargaSuelta")
            {
                var lista = HttpContext.Current.Session["DatosContenedorCargaSuelta"] as List<CargaSueltaVista>;
                if (lista == null || lista.Count == 0) return;

                root = new XElement("CargaSuelta",
                    lista.Select(c => new XElement("Item",
                        new XElement("Linea", c.LINEA),
                        new XElement("Nave", c.NAVE),
                        new XElement("DAI", c.DAI),
                        new XElement("Bultos", c.BULTOS),
                        new XElement("Estado", c.ESTADO),
                        new XElement("Ingreso", c.FECHAINGRESO?.ToString("dd/MM/yyyy")),
                        new XElement("Desconsolidacion", c.FECHADESCONSOLIDA?.ToString("dd/MM/yyyy")),
                        new XElement("Despacho", c.FECHADESPACHO?.ToString("dd/MM/yyyy"))
                    ))
                );
            }

            else
            {
                var lista = HttpContext.Current.Session["DatosContenedorBooking"] as List<Cls_Container.Cls_Container>;
                if (lista == null || lista.Count == 0) return;

                root = new XElement("Bookings",
                    lista.Select(c => new XElement("Booking",
                        new XElement("Contenedor", c.CNTR_CONTAINER),
                        new XElement("Tráfico", c.CNTR_TYPE),
                        new XElement("Estado", c.CNTR_YARD_STATUS),
                        new XElement("Naviera", c.CNTR_CLNT_CUSTOMER_LINE),
                        new XElement("Booking", c.CNTR_BKNG_BOOKING),
                        new XElement("Temperatura", c.CNTR_TEMPERATURE == 0 ? "Sin temperatura" : c.CNTR_TEMPERATURE.ToString()),
                        new XElement("TipoCarga", c.CNTR_LCL_FCL)
                    ))
                );
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/xml";
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment;filename=Export_{DateTime.Now:yyyyMMddHHmmss}.xml");
            HttpContext.Current.Response.Write(root.ToString());
            HttpContext.Current.Response.End();
        }


    }
}
