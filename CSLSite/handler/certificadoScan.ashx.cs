using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using iTextSharp.text;
using iTextSharp;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Globalization;
using System.Net;

namespace CSLSite.handler
{
    public class certificadoScan : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable => throw new NotImplementedException();
        
        public void ProcessRequest(HttpContext context)
        {
            HttpContext.Current.Response.Clear();
            string sid = HttpContext.Current.Request.QueryString["sid"];
            string lg = HttpContext.Current.Request.QueryString["lg"];
            lg = string.IsNullOrEmpty(lg) ? "E" : lg;
            //s-->español

            if (string.IsNullOrEmpty(sid))
            {
                HttpContext.Current.Response.Write("<p>Por favor seleccione el certificado</p>");
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;
            }
            sid = QuerySegura.DecryptQueryString(sid);

            if (string.IsNullOrEmpty(sid))
            {
                HttpContext.Current.Response.Write("<p>Los datos solicitados no fueron encontrados</p>");
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;
            }
            var tabla = new Catalogos.aisv_escaner_certificado_imprimeDataTable();
            var ta = new CatalogosTableAdapters.aisv_escaner_certificado_imprimeTableAdapter();
            sid = sid.Trim().Replace("\0", string.Empty);
            Int64 id;
            if (!Int64.TryParse(sid, out id))
            {
                string close = CSLSite.CslHelper.ExitForm("Hubo un problema durante la conversión para la búsqueda");
                HttpContext.Current.Response.Write(close);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;
            }
            ta.Fill(tabla, id);
            if (tabla.Rows.Count <= 0)
            {
                string close = CSLSite.CslHelper.ExitForm("La busqueda no obtuvo resultados");
                HttpContext.Current.Response.Write(close);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;

            }
            var fila = tabla.FirstOrDefault();

            if (fila.Iscert_generadoNull())
            {
                string close = CSLSite.CslHelper.ExitForm("El certificado no esta listo para descarga.");
                HttpContext.Current.Response.Write(close);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;
            }

            //obtener configuraciones 
            var cfgs = dbconfig.GetActiveConfig("csl_services", "tv-", null);
            string SCFECHA = TextFinal(nameof(SCFECHA), cfgs, lg);
            string SCTITULO = TextFinal(nameof(SCTITULO), cfgs, lg);
            string SCSUB_P = TextFinal(nameof(SCSUB_P), cfgs, lg);
            string SCTRAFIC = TextFinal(nameof(SCTRAFIC), cfgs, lg);
            string SCTRAFIC_E = TextFinal(nameof(SCTRAFIC_E), cfgs, lg);
            string SCTRAFIC_I = TextFinal(nameof(SCTRAFIC_I), cfgs, lg);

            string SDATO01 = TextFinal(nameof(SDATO01), cfgs, lg);
            string SDATO02 = TextFinal(nameof(SDATO02), cfgs, lg);
            string SDATO03 = TextFinal(nameof(SDATO03), cfgs, lg);
            string SDATO04 = TextFinal(nameof(SDATO04), cfgs, lg);
            string SDATO05 = TextFinal(nameof(SDATO05), cfgs, lg);
            string SDATO06 = TextFinal(nameof(SDATO06), cfgs, lg);
            string SDATO07 = TextFinal(nameof(SDATO07), cfgs, lg);
            string SDATO08 = TextFinal(nameof(SDATO08), cfgs, lg);
            string SDATO09 = TextFinal(nameof(SDATO09), cfgs, lg);
            string SDATO10 = TextFinal(nameof(SDATO10), cfgs, lg);
            string SDATO11 = TextFinal(nameof(SDATO11), cfgs, lg);
            string SDATO12 = TextFinal(nameof(SDATO12), cfgs, lg);
            string SDATO13 = TextFinal(nameof(SDATO13), cfgs, lg);
            string SDATO14 = TextFinal(nameof(SDATO14), cfgs, lg);
            string SDATO15 = TextFinal(nameof(SDATO15), cfgs, lg);
            string SDATO16 = TextFinal(nameof(SDATO16), cfgs, lg);
            string SCSTATUS = TextFinal(nameof(SCSTATUS), cfgs, lg);
            string SCFORMAT = TextFinal(nameof(SCFORMAT), cfgs, lg);

           
            CultureInfo enUS = new CultureInfo(SCFECHA);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            string attachment = string.Format("attachment;filename=CERT_{0}.pdf", fila.cert_secuencia);
            HttpContext.Current.Response.AddHeader("Content-Disposition", attachment);
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            byte[] result = null;
            using (var ms = new MemoryStream())
            {
                try
                {

                    int style = 0;
                    int estilo2 = 1; //negrita
                    string familia = "Open Sans";
                    string subfamilia = "sans-serif";
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/background.jpg"));
                    imagen.ScaleToFit(PageSize.A4);
                    imagen.Alignment = iTextSharp.text.Image.UNDERLYING;
                    imagen.SetAbsolutePosition(0, 0);
                    writer.CloseStream = false;
                    document.Open();
                    document.Add(imagen);
                   

                    //fuente
                    Font times = FontFactory.GetFont(familia, subfamilia, true, 12, style, BaseColor.WHITE);
                    Paragraph fecha = new Paragraph(string.Format("Ecuador, {0}", fila.cert_generado.ToString("D", enUS)), times);
                    fecha.Alignment = Element.ALIGN_LEFT;

                    //fuente
                    times = FontFactory.GetFont(familia, subfamilia, true, 30, estilo2, BaseColor.WHITE);
                    Paragraph titulo = new Paragraph(SCTITULO, times);
                    titulo.Alignment = Element.ALIGN_LEFT;

                    //fuente-----> MODIFICAR ESTE TEXTO
                    times = FontFactory.GetFont(familia, subfamilia, true, 30, estilo2, BaseColor.WHITE);
                    string tipo = SCSUB_P;// fila.cert_tipo.Contains("T") ? S : CSUB_P;

                    Paragraph subtitulo = new Paragraph(tipo, times);
                    subtitulo.Alignment = Element.ALIGN_LEFT;

                    string v_subtitulo1 = string.Empty;
                    if (!string.IsNullOrEmpty(fila.cert_tipo))
                    {
                        v_subtitulo1 = SCTRAFIC + " " + (fila.cert_tipo.Trim().ToUpper().Contains("E") ? SCTRAFIC_E : SCTRAFIC_I);
                    }

                    Paragraph subtitulo2 = new Paragraph(v_subtitulo1, times);
                    subtitulo2.Alignment = Element.ALIGN_LEFT;

                    //interlineado
                    subtitulo2.SetLeading(30, 0);

                    //parte blanca

                    #region "Separador"
                    times = FontFactory.GetFont(familia, subfamilia, true, 8, estilo2, BaseColor.BLACK);
                    Paragraph separador = new Paragraph("         ", times);
                    document.Add(separador);

                    #endregion

                    //ponr en documento
                    document.Add(Chunk.NEWLINE);
                    document.Add(fecha);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(titulo);
                    document.Add(subtitulo);
                    document.Add(subtitulo2);
                    document.Add(Chunk.NEWLINE);
                    //document.Add(Chunk.NEWLINE);
                    //document.Add(endorse);
                    //document.Add(Chunk.NEWLINE);
                    //document.Add(expotador);
                    //document.Add(Chunk.NEWLINE);

                    //document.Add(Following);//new
                    //document.Add(Chunk.NEWLINE);//new

                    #region "Contenidos"
                    PdfPTable table = new PdfPTable(2);
                    table.HorizontalAlignment = 0;
                    var f = new float[] { 225, 450 };
                    table.SetWidthPercentage(f, PageSize.A4);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    PdfPCell cell = new PdfPCell(new Phrase(SDATO01, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.cert_secuencia, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO02, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_booking, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO03, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_contenedor, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO04, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(string.Format("{0} / {1}", fila.unidad_buque, fila.unidad_viaje), times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO05, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(string.Format("{0} {1}", fila.dae, fila.unidad_viaje), times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO06, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_sello1, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO07, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_sello2, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO08, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_sello3, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO09, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_sello4, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    string v_Chofer = string.Empty;
                    string v_Empresatransportista = string.Empty;
                    try
                    {
                        var oTrans = N4.Entidades.Certificado.obtenerChofer(fila.id_chofer);
                        v_Chofer = oTrans.Resultado.NOMBRE.Trim();
                        v_Empresatransportista = oTrans.Resultado.NAME_EMPRESA.Trim();
                    }
                    catch { }

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO10, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(v_Empresatransportista, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO11, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(v_Chofer, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO12, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.id_chofer, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO13, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.placa, times));
                    cell.Border = 0;
                    table.AddCell(cell);


                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO14, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SCSTATUS, times));
                    cell.Border = 0;
                    table.AddCell(cell);


                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO15, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.unidad_fecha_ingreso.ToString(SCFORMAT), times));
                    cell.Border = 0;
                    table.AddCell(cell);


                    //tabla contenido
                    document.Add(table);
                    document.Add(Chunk.NEWLINE);
                    #endregion

                
                    #region "Firma"
                    table = new PdfPTable(1);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 400 };
                    table.SetWidthPercentage(f, PageSize.A4);
                    

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(SDATO16, times));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                  
                    //tabla firma
                    document.Add(table);
                    document.Add(Chunk.NEWLINE);
                    #endregion

                    #region "Logos"
                    iTextSharp.text.Image logo_cgsa = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                    table = new PdfPTable(1);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 200/*, 100*/ };
                    table.SetWidthPercentage(f, PageSize.A4);

                    cell = new PdfPCell();
                    logo_cgsa.ScalePercent(20);
                    cell.AddElement(logo_cgsa);
                    cell.HorizontalAlignment = 2;
                    cell.VerticalAlignment = 3;
                    cell.Border = 0;
                    table.AddCell(cell);

                    document.Add(table);
                    #endregion

                    #region "Separador"
                    times = FontFactory.GetFont(familia, subfamilia, true, 8, estilo2, BaseColor.BLACK);
                    separador = new Paragraph("         ", times);
                    document.Add(separador);
                    separador = new Paragraph("         ", times);
                    document.Add(separador);
                    #endregion


                    #region "TablaPie"
                    #endregion
                    document.Close();
                    result = ms.GetBuffer();

                }
                catch
                {
                    string close = CSLSite.CslHelper.ExitForm("<p>Ocurrió un problema durante la generación del archivo PDF</p>");
                    HttpContext.Current.Response.Write(close);
                    HttpContext.Current.Response.End();
                    HttpContext.Current.Response.Close();
                    return;
                }
            }
            HttpContext.Current.Response.BinaryWrite(result);
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();
        }

        private  string TextFinal(string cfname, List<dbconfig> cfgs, string lan )
        {
            string[] textos;
            string salida;
            bool arl = false;
            var sebusca = cfname.ToString().Trim().ToUpper();
           var cfg = cfgs.Where(a => a.config_name.Trim().ToUpper().Equals(sebusca)).FirstOrDefault();
            salida = cfg != null ? cfg.config_value : "";
            textos = salida.Split('+');
            arl = textos.Length > 1;
            salida = lan == "S" ? textos[0] : arl ? textos[1] : "-";
            return salida.Trim();
        }
    }
}