using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BillionEntidades;
using BreakBulk;
using BillionEntidades.Entidades;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Font = iTextSharp.text.Font;
using Rectangule = iTextSharp.text.Rectangle;

using CLSiteUnitLogic.Cls_pase_puerta;
using CSLSite.peso_cls;
using System.Text;
using System.Web.Script.Serialization;
using Image = iTextSharp.text.Image;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ClosedXML.Excel.Drawings;
using CLSiteUnitLogic;
using CLSiteUnitLogic.FacturaCls;
using CLSiteUnitLogic.Cls_Vgm;
using CLSiteUnitLogic.Cls_Container;
using CLSiteUnitLogic.Cls_TranVirtual;

namespace CSLSite.unit
{
    public partial class UNIT_Consulta_Booking : System.Web.UI.Page
    {
        #region "Clases"

        usuario ClsUsuario;
        private string fdesde = string.Empty;
        private string fhasta = string.Empty;
        protected string UrlAISV = "";
        #endregion

        #region "variables"

        private static string TextoLeyenda = string.Empty;

        private string cMensajes;

        public string Gkey;
        #endregion "variables"

        #region "Propiedades"

        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }

        }

        #endregion "Propiedades"


        private void Actualiza_Paneles()
        {
            UPWUSR.Update();
        }


        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    Page.SslOn();
                }

                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);

                    return;
                }



                if (!Page.IsPostBack)
                {

                }
#if !DEBUG
                    this.IsAllowAccess();
#endif

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    if (!Page.IsPostBack)
                    {

                        this.Actualiza_Paneles();
                    }

                }
                else
                {
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));
            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    string idContenedor = Request.QueryString["id"];
                    Session["Contenedor"] = idContenedor;
                    if (Session["DatosContenedorBooking"] is List<Cls_Container> listaBooking)
                    {
                        var seleccionado = listaBooking.FirstOrDefault(x => x.CNTR_CONTAINER == idContenedor);
                        if (seleccionado != null)
                        {
                            Session["ContenedorSeleccionado"] = seleccionado;
                        }
                    }

                    var listaContenedores = Session["ContenedorSeleccionado"] as Cls_Container;

                    if (listaContenedores != null)
                    {
                        Session["GkeyBooking"] = listaContenedores.CNTR_CONSECUTIVO;

                        this.lblidContenedor.Visible = false;
                        this.lblBookingNo.InnerText = listaContenedores.CNTR_BKNG_BOOKING;
                        this.lblVesselName.InnerText = listaContenedores.CNTR_VEPR_VSSL_NAME;
                        this.lblVoyage.InnerText = listaContenedores.CNTR_VEPR_VOYAGE;
                        this.lblPlaceReceipt.InnerText = listaContenedores.CNTR_CITY_LOADED;
                        this.lblPortLoading.InnerText = listaContenedores.CNTR_CITY_UNLOADED;
                        this.lblPortDischarging.InnerText = listaContenedores.CNTR_CITY_ARRIVE;
                        this.lblEstDepArribDate.InnerText = Convert.ToString(listaContenedores.CNTR_VEPR_ESTIMADO_ARRIVAL);
                        this.lblEstDepDate.InnerText = Convert.ToString(listaContenedores.CNTR_VEPR_ACTUAL_DEPARTED);
                        this.lblweight.InnerText = $"{listaContenedores.CNTR_GROSS_WEIGHT} KG";


                        var gkey = listaContenedores.CNTR_CONSECUTIVO;
                        var listaPESO = Cls_VGM.ObtenerRegistroPeso(Convert.ToString(gkey));
                        Session["PesoSeleccionado"] = listaPESO;


                        Session["AISVSeleccionado"] = listaContenedores.CNTR_AISV;

                        if (Session["AISVSeleccionado"] != null)
                        {
                            string sid = Session["AISVSeleccionado"].ToString();


                            string limpioEncriptado = QuerySegura.EncryptQueryString(sid);

                         
                            AISV.Value = ResolveUrl("~/aisv/impresion.aspx?sid=" + HttpUtility.UrlEncode(limpioEncriptado));
                        }


                        var gkeyVGM = listaContenedores.CNTR_CONSECUTIVO;

                        var listaVGM = Cls_VGM.ObtenerRegistroVGM(Convert.ToString(gkey));
                        Session["PesoSeleccionadoVGM"] = listaVGM;

                  //      hfJsonPeso.Value = ObtenerJsonPeso();
                        hfJsonVgm.Value = ObtenerJsonVGM();

                    }
                    else
                    {
                        MostrarError("No hay datos de contenedores en la sesión.");
                    }
                }

                ConsultarBloqueos();
            }
        }

        #region "Metodos Protegidos"
        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            this.Actualiza_Paneles();
        }
    
    
        public void limpiarDivs()
        {
            gvSellos.Visible = false;
            gvFacturas.Visible = false;

            gvPeso.Visible = false;
            gvFacturas.DataSource = null;
            gvFacturas.DataBind();
            gvSellos.DataSource = null;
            gvSellos.DataBind();
            gvPeso.DataSource = null;
            gvPeso.DataBind();
        }
        protected void gvPeso_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showLoader", "mostrarLoaderSwal()();", true);
            if (e.CommandName == "DescargarPeso")
            {
                int index;

                if (!int.TryParse(e.CommandArgument.ToString(), out index) || index < 0 || index >= gvPeso.Rows.Count)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorPeso", "Swal.fire('Error', 'Índice fuera de rango.', 'error');", true);
                    return;
                }

                GridViewRow row = gvPeso.Rows[index];

                if (gvPeso.DataKeys[index] == null || gvPeso.DataKeys[index].Value == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorPeso", "Swal.fire('Error', 'No se encontró la clave de datos.', 'error');", true);
                    return;
                }

                if (!decimal.TryParse(gvPeso.DataKeys[index].Value.ToString(), out decimal codigoCertificado))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorPeso", "Swal.fire('Error', 'Error en la conversión de la clave.', 'error');", true);
                    return;
                }

                var listaVGM = Session["PesoSeleccionado"] as List<Cls_peso_expo>;

                if (listaVGM != null)
                {
                    var vgmFiltrado = listaVGM.FirstOrDefault(vgm => vgm.CODIGO_CERTIFICADO == codigoCertificado);

                    if (vgmFiltrado != null)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                        string jsonVgmData = Newtonsoft.Json.JsonConvert.SerializeObject(vgmFiltrado);
                        string script = $"setTimeout(function() {{ descargarPdfPeso({jsonVgmData}); }}, 500);";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "descargarPeso", script, true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "errorPeso", "Swal.fire('Error', 'No se encontraron datos para el certificado.', 'error');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorPeso", "Swal.fire('Error', 'No hay datos de peso en la sesión.', 'error');", true);
                }
            }
        }
        protected void gvFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DescargarPDF")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showLoader", "mostrarLoaderSwal()();", true);
                int index = Convert.ToInt32(e.CommandArgument);
                string gkeyValue = gvFacturas.DataKeys[index].Value.ToString();

                ScriptManager.RegisterStartupScript(this, GetType(), "descargarFactura", $"descargarPdfFact('{gkeyValue}');", true);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
            }
        }

        private void MostrarGridVacio()
        {
            var dt = new DataTable();
            dt.Columns.Add("SEAL_1");
            dt.Columns.Add("SEAL_2");
            dt.Columns.Add("SEAL_3");
            dt.Columns.Add("SEAL_4");
            dt.Columns.Add("FECHA");

            dt.Rows.Add(dt.NewRow());

            gvSellos.DataSource = dt;
            gvSellos.DataBind();

            if (gvSellos.Rows.Count > 0)
            {
                gvSellos.Rows[0].Visible = false;
            }
        }


        #endregion "Metodos Protegidos"

        #region Metodos Publicos
        public void ConsultarSellos(string gkeySession)
        {
            try
            {

                if (string.IsNullOrEmpty(gkeySession) || !int.TryParse(gkeySession, out int gkey) || gkey <= 0)
                {
                    MostrarError("No se encontraron sellos para este contenedor.");

                    MostrarGridVacio();
                    return;
                }
                var listaSellos = transacMVirtual.GetSellos(gkey);

                if (listaSellos != null && listaSellos.Count > 0)
                {
                    gvSellos.DataSource = new List<object> { listaSellos.Last() };
                    gvSellos.DataBind();
                    UPWUSR.Update();
                }
                else
                {
                    MostrarError("No se encontraron sellos para este contenedor.");

                    MostrarGridVacio();
                }
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al consultar los sellos. Inténtalo nuevamente.");

                MostrarGridVacio();
            }
        }

        public void ConsultarBloqueos()
        {
            try
            {
                var listaContenedores = Session["ContenedorSeleccionado"] as Cls_Container;
                string gkeySession = listaContenedores.CNTR_CONSECUTIVO.ToString();

                if (string.IsNullOrEmpty(gkeySession) || !int.TryParse(gkeySession, out int gkey) || gkey <= 0)
                {
                    Mostrar_Mensaje(1, "El código Gkey no es válido.");
                    return;
                }

                List<transacMVirtual.Bloqueos> bloqueos = transacMVirtual.GetBloqueos(gkey, this.lblidContenedor.InnerText);

                if (bloqueos.Count > 0)
                {
                    // Llenar texto resumen de tipos
                    string tiposBloqueos = string.Join(" .\n", bloqueos.Select(b => b.Id));
                    this.lblTiposBloqueos.InnerText = tiposBloqueos;

                    // Llenar historial detallado en Repeater
                    this.rptHistorialBloqueos.DataSource = bloqueos;
                    this.rptHistorialBloqueos.DataBind();
                }
                else
                {
                    this.lblTiposBloqueos.InnerText = "Ninguno";

                    // Limpiar repeater si no hay datos
                    this.rptHistorialBloqueos.DataSource = null;
                    this.rptHistorialBloqueos.DataBind();
                }
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al consultar los bloqueos.");
            }
        }


        public void MostrarError(string mensaje)
        {
            string script = $"mostrarError('{mensaje.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mostrarError", script, true);

        }


        [System.Web.Services.WebMethod]
        public static void ExportarExcel()
        {
            try
            {
                byte[] excelBytes = LogicExcel.CreateExcelBytesHorasReefer();

                string dateTimeFormat = "yyyyMMddHHmmssfff";
                string formattedDateTime = DateTime.Now.ToString(dateTimeFormat);

                string fileName = "Reporte_" + formattedDateTime + ".xlsx";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                HttpContext.Current.Response.BinaryWrite(excelBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error al exportar el archivo Excel";
            }
        }
        public string ObtenerJsonPeso()
        {
            var listaVGM = Session["PesoSeleccionado"] as List<Cls_peso_expo>;

            if (listaVGM != null && listaVGM.Any())
            {
                var peso = listaVGM.LastOrDefault();
                var vgmData = new
                {
                    gkeyValue = Session["GkeyBooking"],
                    peso.CONTENEDOR,
                    peso.PLACA,
                    peso.REFERENCIA,
                    peso.NAVE,
                    peso.VIAJE,
                    peso.PESO_BALANZA,
                    peso.PESO_VEHICULO,
                    peso.PESO_BRUTO,
                    peso.TARA,
                    peso.PESO_NETO,
                    peso.USUARIO,
                    FECHA = peso.FECHA.ToString("yyyy-MM-ddTHH:mm:ss")

                };
                return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(vgmData);
            }

            return "{}";
        }

        public string ObtenerJsonVGM()
        {
            var listaVGM = Session["PesoSeleccionadoVGM"] as List<Cls_VGM>;

            if (listaVGM != null && listaVGM.Any())
            {
                var peso = listaVGM.LastOrDefault();
                int anio = peso.fecha?.Year ?? 0;
                var certificado = "VGM-" + anio + "-" + "00" + peso.codigo;
                var vgmData = new
                {
                    gkeyValue = Session["GkeyBooking"],
                    cntr = peso.cntr,
                    tara = peso.tara?.ToString() ?? "0",
                    peso = peso.peso?.ToString() ?? "0",
                    payload = peso.payload?.ToString() ?? "0",
                    equipo = peso.equipo ?? "",
                    ruc = peso.ruc ?? "",
                    fechaEmision = peso.fecha_reg?.ToString("yyyy-MM-dd") ?? "",
                    certificado = certificado ?? "",
                    export = peso.export ?? "",
                    nave = peso.NOMBRE_BUQUE ?? "",
                    viaje = peso.VIAJE ?? ""

                };
                return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(vgmData);
            }

            return "{}";
        }


        #endregion

        #region PDFS

        // ***EIR PDF***

        [System.Web.Services.WebMethod]
        public static string ObtenerPdfBase64(string gkeyValue)
        {
            try
            {
                int gkey;
                if (!int.TryParse(gkeyValue, out gkey) || gkey <= 0)
                    return "ERROR: Ha ocurrido un error con el parámetro gkey.";

                var cp = new CertificadoEIR();
                var ListCertificado = cp.ver_certificado(gkey);

                if (ListCertificado == null || ListCertificado.Count == 0)
                    return "ERROR: No se encontraron datos para el certificado.";

                byte[] pdfBytes = LogicPdfs.GenerarPdfEirExpo(ListCertificado);

                if (pdfBytes == null || pdfBytes.Length == 0)
                    return "ERROR: No se pudo generar el PDF.";

                return Convert.ToBase64String(pdfBytes);
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
        }
        [System.Web.Services.WebMethod]
        public static string ObtenerPdfFact(string gkeyValue)
        {
            try
            {
                Byte[] pdf = null;


                if (gkeyValue != null)
                {

                    var fac = Cls_Factura.ObtenerXNumFact(gkeyValue).FirstOrDefault();

                    if (fac != null)
                    {

                        var pc = new Cls_Factura(fac.contenido);
                        pc.documento = fac.documento;
                        pc.fecha = fac.fecha;
                        pc.id = fac.id;
                        pc.autorizacion = fac.autorizacion;
                        pc.clave = fac.clave;
                        pc.fechaAutoriza = fac.fechaAutoriza;
                        pc.tipo = fac.tipo;
                        pc.IVA = fac.IVA;
                        string xt;
                        if (!pc.Serializar(out xt))
                        {
                        }
                        else
                        {
                            pdf = pc.GenerarPdf();
                        }

                    }
                    else
                    {

                        return "ERROR: Error al generar el archivo PDF.";
                    }
                }

                string base64Pdf = Convert.ToBase64String(pdf);
                return base64Pdf;
            }
            catch (Exception ex)
            {
                return "ERROR: Ocurrió un error inesperado al procesar la solicitud.";
            }
        }

        [System.Web.Services.WebMethod]
        public static string ObtenerPdfVgm(Cls_VGM vgmDato)
        {
            try
            {

                var cnn = Configuracion.ObtenerConfiguracion("N5");

                cnn = Configuracion.ObtenerConfiguracion("SERVER_QR");

               Cls_VGMExpo vgmPdf = new Cls_VGMExpo();

                vgmPdf.Contenedor = vgmDato.cntr;
                vgmPdf.CertificadoEquipo = vgmDato.certificado;
                vgmPdf.Equipo = vgmDato.equipo;
                vgmPdf.fechaEmision = vgmDato.fecha.HasValue ? vgmDato.fecha.Value.ToString("dd/MM/yyyy HH:mm") : vgmDato.fecha_reg.HasValue ? vgmDato.fecha_reg.Value.ToString("dd/MM/yyyy HH:mm") : DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                vgmPdf.numeroCertificado = vgmDato.certificado;
                vgmPdf.Nave = vgmDato.nave;
                vgmPdf.RazonSocial = vgmDato.export;
                vgmPdf.Ruc = vgmDato.ruc;
                vgmPdf.Viaje = vgmDato.VIAJE;

                vgmPdf.PesoVGM = vgmDato.peso.HasValue ? vgmDato.peso.Value.ToString() : "00.00";
                vgmPdf.Tara = vgmDato.tara.HasValue ? vgmDato.tara.Value.ToString() : "00.00";
                vgmPdf.PayLoad = vgmDato.payload.HasValue ? vgmDato.payload.Value.ToString() : "00.00";
                vgmPdf.ServerQR = cnn.Value;
                decimal neto = 0;
                if (vgmDato.peso.HasValue && vgmDato.tara.HasValue)
                {
                    neto = vgmDato.peso.Value - vgmDato.tara.Value;
                }
                vgmPdf.PesoNeto = neto.ToString();

                byte[] pdfBytes = vgmPdf.GenerarPdf();


                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return "ERROR: No se pudo generar el PDF.";
                }


                return Convert.ToBase64String(pdfBytes);
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        public void DescargarDirectoVGM()
        {
            try
            {
                var listaVGM = Session["PesoSeleccionadoVGM"] as List<Cls_VGM>;
                if (listaVGM != null && listaVGM.Any())
                {
                    var peso = listaVGM.Last();
                    int anio = peso.fecha?.Year ?? 0;
                    var certificado = "VGM-" + anio + "-" + "00" + peso.codigo;

                    var vgmData = new
                    {
                        cntr = peso.cntr,
                        tara = peso.tara?.ToString() ?? "0",
                        peso = peso.peso?.ToString() ?? "0",
                        payload = peso.payload?.ToString() ?? "0",
                        equipo = peso.equipo ?? "",
                        ruc = peso.ruc ?? "",
                        fechaEmision = peso.fecha_reg?.ToString("yyyy-MM-dd") ?? "",
                        certificado = certificado,
                        export = peso.export ?? "",
                        nave = peso.NOMBRE_BUQUE ?? "",
                        viaje = peso.VIAJE ?? ""
                    };

                    string jsonVgmData = JsonConvert.SerializeObject(vgmData);

                    string script = $"descargarPdfVgm({jsonVgmData});";
                    ScriptManager.RegisterStartupScript(this, GetType(), "descargarVGM", script, true);
                }
                else
                {
                    MostrarError("No hay datos de VGM en la sesión.");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al preparar la descarga del VGM: " + ex.Message);
            }
        }
        protected void btnDescargarVGM_Click(object sender, EventArgs e)
        {
            DescargarDirectoVGM();
        }

        [System.Web.Services.WebMethod]
        public static string ObtenerPdfPeso(Cls_peso_expo vgmDato)
        {
            try
            {
                Cls_peso_expo vgmPdf = new Cls_peso_expo
                {
                    CONTENEDOR = vgmDato.CONTENEDOR,
                    PLACA = vgmDato.PLACA,
                    REFERENCIA = vgmDato.REFERENCIA,
                    NAVE = vgmDato.NAVE,
                    PESO_BALANZA = vgmDato.PESO_BALANZA,
                    PESO_VEHICULO = vgmDato.PESO_VEHICULO,
                    PESO_BRUTO = vgmDato.PESO_BRUTO,
                    TARA = vgmDato.TARA,
                    PESO_NETO = vgmDato.PESO_NETO,
                    USUARIO = vgmDato.USUARIO,
                    FECHA = vgmDato.FECHA
                };

                byte[] pdfBytes = vgmPdf.GenerarPdf();

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return "ERROR: No se pudo generar el PDF.";
                }

                return Convert.ToBase64String(pdfBytes);
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

 
        #endregion

        #region Botones

        public void btnSellos_Click(object sender, EventArgs e)
        {
            var listaContenedores = Session["ContenedorSeleccionado"] as Cls_Container;
            string gkeySession = listaContenedores.CNTR_CONSECUTIVO.ToString();

            limpiarDivs();
            ConsultarSellos(gkeySession);
            gvSellos.Visible = true;
        }
        protected void btnFacturas_Click(object sender, EventArgs e)
        {
            limpiarDivs();

        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showLoader", "mostrarLoaderSwal()();", true);
            var listaContenedores = Session["ContenedorSeleccionado"] as Cls_Container;
            string gkeySession = listaContenedores.CNTR_CONSECUTIVO.ToString();

            var listaFacturas = Cls_Factura.ObteneIVXGkey(gkeySession);
            if (listaFacturas != null && listaFacturas.Count > 0)
            {
                gvFacturas.DataSource = new List<object> { listaFacturas.Last() };
                gvFacturas.DataBind();
                gvFacturas.Visible = true;
            }
            else
            {
                MostrarError("No hay facturas disponibles.");
            }

            UPWUSR.Update();

       //     ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
        }

        protected void btnFactExcel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "exportarExcel", "exportar();", true);
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session["RetornoDesdeDetalle"] = true;
            Response.Redirect("~/unit/UNIT_Consulta_Carga.aspx");
        }

        #endregion
    }
}