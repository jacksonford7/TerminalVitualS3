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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Font = iTextSharp.text.Font;
using CLSiteUnitLogic.Cls_Vgm;
using CSLSite.peso_cls;
using System.Text;
using System.Web.Script.Serialization;
using Image = iTextSharp.text.Image;
using Rectangule = iTextSharp.text.Rectangle;
using CLSiteUnitLogic.Cls_pase_puerta;
using CLSiteUnitLogic.FacturaCls;
using CLSiteUnitLogic.Cls_TranVirtual;
using CLSiteUnitLogic;

namespace CSLSite.unit
{
    public partial class UNIT_Consulta_Contenedor : System.Web.UI.Page
    {
        #region "Clases"


        usuario ClsUsuario;
        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private string fdesde = string.Empty;
        private string fhasta = string.Empty;

        #endregion

        #region "variables"

        private static string TextoLeyenda = string.Empty;


        public string Gkey;
        #endregion "variables"


        #region "Metodos Protegidos"
        private void Actualiza_Paneles()
        {
            UPWUSR.Update();
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {



            this.Actualiza_Paneles();
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
                        /*para almacenar clientes que asumen factura*/

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

                    var listaContenedores = Session["DatosContenedorCntr2"] as IEnumerable<object>;

                    bool estaSuscrito = false;

                    if (listaContenedores != null)
                    {
                        foreach (var item in listaContenedores)
                        {
                            var propContenedor = item.GetType().GetProperty("CONTENEDOR")?.GetValue(item, null);

                            if (propContenedor != null && propContenedor.ToString() == idContenedor)
                            {
                                string gkeyValue = GetPropertyValue(item, "GKEY");
                                Session["Gkey"] = gkeyValue;
                                this.lblidContenedor.InnerText = GetPropertyValue(item, "CONTENEDOR");
                                this.lblCNTR_Size.InnerText = GetPropertyValue(item, "CONTENEDORSIZE");
                                this.lblVessel.InnerText = GetPropertyValue(item, "VESSEL");
                                this.lbltstate.Text = GetPropertyValue(item, "ESTADO");
                                if (lbltstate != null)
                                {
                                    string estado = lbltstate.Text.Trim().ToUpper();

                                    switch (estado)
                                    {
                                        case "DESPACHADO":
                                            lbltstate.ForeColor = System.Drawing.Color.Gray;
                                            break;
                                        case "EN PATIO":
                                            lbltstate.ForeColor = System.Drawing.Color.Red;
                                            break;
                                        case "A BORDO":
                                            lbltstate.ForeColor = System.Drawing.Color.Green;
                                            break;
                                    }
                                }
                                this.lblExportador.InnerText = GetPropertyValue(item, "EXPORTADOR");
                                this.lblFCAS.InnerText = GetPropertyValue(item, "FECHACAS");
                                this.lblSalida.InnerText = GetPropertyValue(item, "FECHASALIDA");
                                this.lblnumberdoc.InnerText = GetPropertyValue(item, "NODOC");
                                var pesoBruto = GetPropertyValue(item, "GROSS");
                                var pesoTara = GetPropertyValue(item, "TARA");
                                var pesoImdt = GetPropertyValue(item, "IMDT");
                                this.lblweight.InnerText = $"{pesoBruto} - {pesoTara} - {pesoImdt} KG";
                                this.lblweight.Attributes["title"] =
                                $"Peso Bruto: {pesoBruto} KG\nPeso Tara: {pesoTara} KG\nPeso IMDT: {pesoImdt} KG";



                                this.lblweight.Attributes["title"] =
                                    $"Peso Manifiesto: {pesoBruto} KG\nPeso Tara: {pesoTara} KG\nPeso IMDT: {pesoImdt} KG";

                                fdesde = GetPropertyValue(item, "FECHADESDE");
                                fhasta = GetPropertyValue(item, "FECHAHASTA");
                                this.lblMrn.InnerText = GetPropertyValue(item, "MRN");
                            }
                        }
                        var gkey = Session["Gkey"] as string;
                        var listaVGM = Cls_VGM.ObtenerRegistroPeso(gkey);
                        Session["PesoSeleccionado"] = listaVGM;
                        hfJsonVgm.Value = ObtenerJsonVgm();
                    }
                    else
                    {
                        Mostrar_Mensaje(1, "No hay datos de contenedores en la sesión.");
                    }
                }


                ConsultarAforo();
                ConsultarBloqueos();
            }
        }


        private string GetPropertyValue(object obj, string propertyName)
        {
            var prop = obj.GetType().GetProperty(propertyName);
            return prop != null ? Convert.ToString(prop.GetValue(obj, null)) : string.Empty;
        }

        protected void gvVGM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DescargarVGM")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvVGM.Rows[index];

                var vgmData = new
                {
                    gkeyValue = gvVGM.DataKeys[index].Value.ToString(),
                    tara = (row.FindControl("taraHidden") as HiddenField)?.Value ?? "0",
                    peso = (row.FindControl("pesoHidden") as HiddenField)?.Value ?? "0",
                    payload = (row.FindControl("payloadHidden") as HiddenField)?.Value ?? "0",
                    equipo = (row.FindControl("equipoHidden") as HiddenField)?.Value ?? "",
                    ruc = (row.FindControl("rucHidden") as HiddenField)?.Value ?? "",
                    fechaEmision = (row.FindControl("fechaRegHidden") as HiddenField)?.Value ?? "",
                    numeroCertificado = row.Cells[0].Text,
                    razonSocial = row.Cells[1].Text, // Exportador
                    nave = row.Cells[2].Text, // Nombre del buque
                    viaje = row.Cells[3].Text // Viaje
                };

                string jsonVgmData = Newtonsoft.Json.JsonConvert.SerializeObject(vgmData);

                ScriptManager.RegisterStartupScript(this, GetType(), "descargarVGM", $"descargarPdfVgm({jsonVgmData});", true);
            }
        }

        protected void gvFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DescargarPDF")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string gkeyValue = gvFacturas.DataKeys[index].Value.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "descargarFactura", $"descargarPdfFact('{gkeyValue}');", true);
            }
        }

        protected void gvSellos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Foto")
            {
                try
                {
                    string gkeySession = Session["Gkey"] as string; ;
                    this.htmlImagenes.InnerHtml = string.Empty;
                    string OError = string.Empty;
                    int contador = 0;

                    var imagenes = fotoSellos.listadoFotosSealMuelleTV(Convert.ToInt64(gkeySession), out OError);

                    if (!string.IsNullOrEmpty(OError) || imagenes == null || imagenes.Count == 0)
                    {
                        sinresultadoFotos.Visible = true;
                        UPMODAL.Update();
                        return;
                    }

                    StringBuilder divImagenes = new StringBuilder();

                    foreach (var item in imagenes)
                    {
                        divImagenes.Append($@"
                    <div class='carousel-item {(contador++ == 0 ? "active" : "")}'>
                        <img src='{item.ruta}' class='d-block w-100' style='height:750px; overflow:auto' alt='Foto de sello' />
                    </div>");
                    }

                    string html = $@"
                <div class='mb-5'>
                    <div id='carouselExampleCaptions' class='carousel slide' data-ride='carousel'>
                        <div class='carousel-inner'>
                            {divImagenes}
                        </div>
                        <a class='carousel-control-prev' href='#carouselExampleCaptions' role='button' data-slide='prev'>
                            <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                            <span class='sr-only'>Previous</span>
                        </a>
                        <a class='carousel-control-next' href='#carouselExampleCaptions' role='button' data-slide='next'>
                            <span class='carousel-control-next-icon' aria-hidden='true'></span>
                            <span class='sr-only'>Next</span>
                        </a>
                    </div>
                </div>";

                    this.htmlImagenes.InnerHtml = html;
                    xfinde2.Visible = true;
                    sinresultadoFotos.Visible = false;
                    UPMODAL.Update();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void ConsultarSellos()
        {
            try
            {
                string gkeySession = Session["Gkey"] as string;
                if (string.IsNullOrEmpty(gkeySession) || !int.TryParse(gkeySession, out int gkey) || gkey <= 0)
                {
                    MostrarGridVacio();
                    return;
                }

                var listaSellos = transacMVirtual.GetSellos(gkey);

                if (listaSellos != null && listaSellos.Count > 0)
                {
                    gvSellos.DataSource = new List<object> { listaSellos.Last() };
                    gvSellos.DataBind();
                }
                else
                {
                    MostrarGridVacio();
                    MostrarError("No se encontraron sellos para este contenedor.");
                }
            }
            catch (Exception ex)
            {
                Mostrar_Mensaje(1, "Ocurrió un error al consultar los sellos. Inténtalo nuevamente.");
                MostrarGridVacio();
            }

            UPWUSR.Update();
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

        protected void gvPasePuerta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DescargarPase")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPasePuerta.Rows[index];

                string idPase = gvPasePuerta.DataKeys[index].Value.ToString();

                ScriptManager.RegisterStartupScript(this, GetType(), "descargarPase", $"descargarCertificadoPasePuerta({idPase});", true);
            }
        }
        #endregion "Metodos Protegidos"

        #region Metodos Publicos
        public void MostrarError(string mensaje)
        {
            string script = $"mostrarError('{mensaje.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mostrarError", script, true);
        }

        public void ConsultarBloqueos()
        {
            try
            {
                string gkeySession = Session["Gkey"] as string;
                if (string.IsNullOrEmpty(gkeySession) || !int.TryParse(gkeySession, out int gkey) || gkey <= 0)
                {
                    Mostrar_Mensaje(1, "El código Gkey no es válido.");
                    return;
                }

                List<transacMVirtual.Bloqueos> bloqueos = transacMVirtual.GetBloqueos(gkey, this.lblidContenedor.InnerText);


                if (bloqueos.Count > 0)
                {
                    string tiposBloqueos = string.Join(" .\n", bloqueos.Select(b => b.Id));
                    this.lblTiposBloqueos.InnerText = tiposBloqueos;

                    // Llenar historial detallado en Repeater
                    this.rptHistorialBloqueos.DataSource = bloqueos;
                    this.rptHistorialBloqueos.DataBind();
                }
                else
                {
                    this.lblTiposBloqueos.InnerText = "Ninguno";

                    this.rptHistorialBloqueos.DataSource = null;
                    this.rptHistorialBloqueos.DataBind();
                }

            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al consultar los bloqueos.");

            }
        }


        public void limpiaGrids()
        {
            gvSellos.Visible = false;
            gvFacturas.Visible = false;
            gvVGM.Visible = false;
            gvFacturas.DataSource = null;
            gvFacturas.DataBind();
            gvSellos.DataSource = null;
            gvSellos.DataBind();
            gvVGM.DataSource = null;
            gvVGM.DataBind();
            gvVGM.DataBind();
            gvPasePuerta.DataSource = null;
            gvPasePuerta.DataBind();
        }
        public void ConsultarAforo()
        {
            try
            {
                string gkeySession = Session["Gkey"] as string;
                if (string.IsNullOrEmpty(gkeySession) || !int.TryParse(gkeySession, out int gkey) || gkey <= 0)
                {

                    MostrarError("El código Gkey no es válido.");

                    return;
                }

                var tieneAforo = transacMVirtual.GetAforo(gkey);

                string mensaje = (tieneAforo == 1) ? this.lblAforo.InnerText = "Tiene aforo" : this.lblAforo.InnerText = "No tiene aforo";

            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al consultar el aforo.");

            }
        }

        public string ObtenerJsonVgm()
        {
            var listaVGM = Session["PesoSeleccionado"] as List<Cls_peso_expo>;

            if (listaVGM != null && listaVGM.Any())
            {
                var peso = listaVGM.LastOrDefault(); // o First si prefieres
                var vgmData = new
                {
                    gkeyValue = Session["Gkey"], // obligatorio
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
        #endregion

        #region PDFS
        [System.Web.Services.WebMethod]
        public static string ObtenerPdfPesoImpo(Cls_peso_expo vgmDato)
        {
            try
            {

                var cnn = Configuracion.ObtenerConfiguracion("N5");

                cnn = Configuracion.ObtenerConfiguracion("SERVER_QR");

                Cls_peso_expo vgmPdf = new Cls_peso_expo
                {
                    CONTENEDOR = vgmDato.CONTENEDOR,
                    PLACA = vgmDato.PLACA,
                    REFERENCIA = vgmDato.REFERENCIA,
                    NAVE = vgmDato.NAVE,
                    VIAJE = vgmDato.VIAJE,
                    PESO_BALANZA = vgmDato.PESO_BALANZA,
                    PESO_VEHICULO = vgmDato.PESO_VEHICULO,
                    PESO_BRUTO = vgmDato.PESO_BRUTO,
                    TARA = vgmDato.TARA,
                    PESO_NETO = vgmDato.PESO_NETO,
                    USUARIO = vgmDato.USUARIO,
                    FECHA = vgmDato.FECHA,
                    SERVERQR = cnn.Value
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
        [System.Web.Services.WebMethod]
        public static string ObtenerPdfPasePuerta(long idPase)
        {
            try
            {
                string gkeySession = HttpContext.Current.Session["Gkey"] as string;
                byte[] pdfBytes = null;
                var dato = ContenedorPase.ObtenePaseRegistroUnificado(idPase, Convert.ToInt64(gkeySession));
                if (dato != null)
                {

                    var cnn = Configuracion.ObtenerConfiguracion("N5");

                    cnn = Configuracion.ObtenerConfiguracion("SERVER_QR");

                    Cls_PasePuertaNew vgmPdf = new Cls_PasePuertaNew();

                    vgmPdf.contenedor = dato.contenedor;
                    vgmPdf.conductor = dato.conductor;
                    vgmPdf.documento = dato.documento;
                    vgmPdf.sello = dato.sello1;
                    vgmPdf.selloGeo = dato.aduana;
                    vgmPdf.serial = dato.sn;
                    vgmPdf.horarioDesde = dato.tinicio;
                    vgmPdf.horarioHasta = dato.tfin;
                    vgmPdf.horarioLlegada = dato.tturno;
                    vgmPdf.Ruc = dato.ruc;
                    vgmPdf.Importador = dato.importador ?? "";
                    vgmPdf.Placa = dato.placa;
                    vgmPdf.Licencia = dato.licencia;
                    vgmPdf.mrn = dato.mrn;
                    vgmPdf.empresaTransporte = dato.empresa ?? "";
                    vgmPdf.tipoIso = dato.iso;
                    vgmPdf.IdPase = idPase;
                    vgmPdf.cf_server_con = cnn.Value;

                    pdfBytes = vgmPdf.GenerarPdf();


                    if (pdfBytes == null || pdfBytes.Length == 0)
                    {
                        return "ERROR: No se pudo generar el PDF.";
                    }

                }

                return Convert.ToBase64String(pdfBytes);

            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
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
        public static void ObtenerPdfBase64(string gkeyValue)
        {
            try
            {
                int gkey;
                if (!int.TryParse(gkeyValue, out gkey) || gkey <= 0)
                {
                    HttpContext.Current.Response.StatusCode = 400;
                    HttpContext.Current.Response.ContentType = "application/json";
                    HttpContext.Current.Response.Write("{\"error\": \"Ha ocurrido un error con el parámetro gkey.\"}");
                    HttpContext.Current.Response.End();
                    return;
                }
                var cp = new CertificadoEIR();
                var ListCertificado = cp.ver_certificado(gkey);

                if (ListCertificado == null || ListCertificado.Count == 0)
                {
                    HttpContext.Current.Response.StatusCode = 404;
                    HttpContext.Current.Response.ContentType = "application/json";
                    HttpContext.Current.Response.Write("{\"error\": \"No se encontró datos para el certificado.\"}");
                    HttpContext.Current.Response.End();
                    return;
                }

                byte[] pdfBytes = LogicPdfs.GenerarPdfImpo(ListCertificado);

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    HttpContext.Current.Response.StatusCode = 500;
                    HttpContext.Current.Response.ContentType = "application/json";
                    HttpContext.Current.Response.Write("{\"error\": \"Error al generar el PDF.\"}");
                    HttpContext.Current.Response.End();
                    return;
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Certificado.pdf");
                HttpContext.Current.Response.BinaryWrite(pdfBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write("{\"error\": \"ERROR: " + ex.Message + "\"}");
                HttpContext.Current.Response.End();
            }
        }


        #endregion

        #region Botones
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session["RetornoDesdeDetalle"] = true;
            Response.Redirect("~/unit/UNIT_Consulta_Carga.aspx");
        }
        protected void btnFacturas_Click(object sender, EventArgs e)
        {
            limpiaGrids();
            string gkeySession = Session["Gkey"] as string;
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
        }


        protected void btnPasePuerta_Click(object sender, EventArgs e)
        {
            limpiaGrids();
            string gkeySession = Session["Gkey"] as string;
            var list = ContenedorPase.ObtenePasesPuerta(Convert.ToInt64(gkeySession));
            if (list != null && list.Count > 0)
            {
                gvPasePuerta.DataSource = new List<object> { list.Last() };
                gvPasePuerta.DataBind();
                gvPasePuerta.Visible = true;
            }
            else
            {
                MostrarError("No se encontró el dato para descargar el Pase Puerta.");
            }

            UPWUSR.Update();
        }


        protected void btnSellos_Click(object sender, EventArgs e)
        {
            limpiaGrids();
            ConsultarSellos();
            gvSellos.Visible = true;
        }


        [System.Web.Services.WebMethod]
        public static string ObtenerFotosContenedor(string idContenedor)
        {
            try
            {
                string OError = string.Empty;
                var imagenes = transacMVirtual.ObtenerFotosContenedor(idContenedor, out OError);

                if (!string.IsNullOrEmpty(OError) || imagenes == null || imagenes.Count == 0)
                {
                    return "{\"error\": \"No se encontraron fotos del contenedor.\"}";
                }

                List<object> imagenesUrls = new List<object>();

                // 📂 Ruta física de la carpeta temporal pública
                string carpetaTemporal = HttpContext.Current.Server.MapPath("~/img/temp/");
                if (!Directory.Exists(carpetaTemporal))
                {
                    Directory.CreateDirectory(carpetaTemporal);
                }

                // 🧹 Eliminar imágenes antiguas (más de 1 hora)
                foreach (string file in Directory.GetFiles(carpetaTemporal, "*.jpg"))
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastWriteTime < DateTime.Now.AddHours(-1))
                    {
                        try { fi.Delete(); } catch { }
                    }
                }

                // 📦 Procesar las imágenes recibidas
                foreach (var item in imagenes)
                {
                    if (string.IsNullOrWhiteSpace(item.UrlWeb)) continue;

                    string rutaUNC = item.UrlWeb.Trim();
                    string nombreArchivo = Path.GetFileName(rutaUNC);
                    string destinoLocal = Path.Combine(carpetaTemporal, nombreArchivo);

                    // Copiar el archivo si no existe o si deseas sobreescribir
                    if (!File.Exists(destinoLocal))
                    {
                        File.Copy(rutaUNC, destinoLocal, true);
                    }

                    // Generar la URL relativa para el navegador
                    string urlWeb = "/img/temp/" + nombreArchivo;

                    imagenesUrls.Add(new { UrlWeb = urlWeb });
                }

                return new JavaScriptSerializer().Serialize(imagenesUrls);
            }
            catch (Exception)
            {
                return "{\"error\": \"Error al cargar imágenes del contenedor.\"}";
            }
        }

        #endregion

    }
}