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
    public class certificadoimpo : IHttpHandler, IRequiresSessionState
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
            var tabla = new Catalogos.aisv_carbono_certificado_imprimeDataTable();
            var ta = new CatalogosTableAdapters.aisv_carbono_certificado_imprimeTableAdapter();
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
            var cfgs = dbconfig.GetActiveConfig("csl_services", "tv", null);
            var cf_server_con = cfgs.Where(a => a.config_name.Contains("carbono_consulta")).FirstOrDefault();
            var cf_server_qr = cfgs.Where(a => a.config_name.Contains("carbono_qr")).FirstOrDefault();

            var server_consulta = cf_server_con != null && !string.IsNullOrEmpty(cf_server_con.config_value) ? cf_server_con.config_value : "https://www.cgsa.com.ec/carbono-neutro/";
            var server_qr = cf_server_qr != null && !string.IsNullOrEmpty(cf_server_qr.config_value) ? cf_server_qr.config_value : "CGINT06:8080";

            //campo de definicion de fecha
            string CFECHA = TextFinal(nameof(CFECHA), cfgs, lg);
            string CTITULO = TextFinal(nameof(CTITULO), cfgs, lg);
            string CSUB_P = TextFinal(nameof(CSUB_P), cfgs, lg);
            string CSUB_T = TextFinal(nameof(CSUB_T), cfgs, lg);
            string CEMISOR_P = TextFinal(nameof(CEMISOR_P), cfgs, lg);
            string CEMISOR_T = TextFinal(nameof(CEMISOR_T), cfgs, lg);
            string CCOM_T = TextFinal(nameof(CCOM_T), cfgs, lg);
            string CCOM_P = TextFinal(nameof(CCOM_P), cfgs, lg);
            string CNUM = TextFinal(nameof(CNUM), cfgs, lg);
            CNUM = fila.notas == "CFS" ? CNUM == "Number:" ? "Load Number:" : "Número Carga:" : CNUM;
            string CCID = TextFinal(nameof(CCID), cfgs, lg);
            string CIN = TextFinal(nameof(CIN), cfgs, lg);
            string CLOAD = TextFinal(nameof(CLOAD), cfgs, lg);
            string CTRIP = TextFinal(nameof(CTRIP), cfgs, lg);
            string CCOM = TextFinal(nameof(CCOM), cfgs, lg);
            string CCEO = TextFinal(nameof(CCEO), cfgs, lg);
            string CGER = TextFinal(nameof(CGER), cfgs, lg);
            string CCERTI = TextFinal(nameof(CCERTI), cfgs, lg);
            string CFORMAT = TextFinal(nameof(CFORMAT), cfgs, lg);

            string CDES = TextFinal(nameof(CDES), cfgs, lg);
            string CFESAL = TextFinal(nameof(CFESAL), cfgs, lg);

            string CSUB_S = TextFinal(nameof(CSUB_S), cfgs, lg);

            CultureInfo enUS = new CultureInfo(CFECHA);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            string attachment = string.Format("attachment;filename=CERT_{0}.pdf",fila.cert_secuencia);
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
                    //CODIGO QR------------------------------------>

                    Stream stream = app_start.CarbonoNeutro.BarcodeStream(fila.cert_numero,server_consulta,server_qr);


                    if (stream != null)
                    {
                        Image barcode = Image.GetInstance(stream);
                        barcode.ScaleToFit(80, 80);
                        barcode.Alignment = iTextSharp.text.Image.UNDERLYING;
                        int varia = 0;
                        if (!fila.cert_tipo.Contains("T"))
                        {
                            varia = 18;
                        }
                        barcode.SetAbsolutePosition(425, 340 + varia);
                        document.Add(barcode);
                    }
                   // ------------------------------------------------------->


                    //fuente
                    Font times = FontFactory.GetFont(familia, subfamilia, true, 12, style, BaseColor.WHITE);
                    Paragraph fecha = new Paragraph(string.Format("Ecuador, {0}", fila.cert_generado.ToString("D", enUS)), times);
                    fecha.Alignment = Element.ALIGN_LEFT;

                    //fuente
                    times = FontFactory.GetFont(familia, subfamilia, true, 30, estilo2, BaseColor.WHITE);
                    Paragraph titulo = new Paragraph(CTITULO, times);
                    titulo.Alignment = Element.ALIGN_LEFT;

                    //fuente-----> MODIFICAR ESTE TEXTO
                    times = FontFactory.GetFont(familia, subfamilia, true, 30, estilo2, BaseColor.WHITE);
                    string tipo = fila.cert_tipo.Contains("T") ? CSUB_T : CSUB_P;

                    Paragraph subtitulo = new Paragraph(tipo, times);
                    subtitulo.Alignment = Element.ALIGN_LEFT;
                    //interlineado
                    subtitulo.SetLeading(30, 0);

                    //parte blanca
                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);

                    var basic_line = fila.cert_tipo.Contains("T") ? CEMISOR_T :CEMISOR_P;
                    Paragraph endorse = new Paragraph(basic_line, times);


                    endorse.Alignment = Element.ALIGN_JUSTIFIED;

                    times = FontFactory.GetFont(familia, subfamilia, true, 18, style, BaseColor.BLACK);
                    Paragraph expotador = new Paragraph(fila.nombres_exportador, times);
                    expotador.Alignment = Element.ALIGN_CENTER;

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);

                    string cco = fila.cert_tipo.Contains("T") ? CCOM_T :CCOM_P;
                    Paragraph compesation = new Paragraph(cco, times);

                    compesation.Alignment = Element.ALIGN_JUSTIFIED;

                    //NUEVO TEXTO
                    string siguiente = CSUB_S;
                    Paragraph psiguiente = new Paragraph(siguiente, times);
                    psiguiente.Alignment = Element.ALIGN_LEFT;


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
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(endorse);
                    document.Add(Chunk.NEWLINE);
                    document.Add(expotador);
                    document.Add(Chunk.NEWLINE);

                    document.Add(psiguiente);
                    document.Add(Chunk.NEWLINE);

                    #region "Contenidos"
                    PdfPTable table = new PdfPTable(2);
                    table.HorizontalAlignment = 0;
                    var fix = lg == "S" ? 175 : 125;
                    var f = new float[] { fix, 450 };
                    table.SetWidthPercentage(f, PageSize.A4);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    PdfPCell cell = new PdfPCell(new Phrase(CNUM, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_contenedor, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    //--------------------------->
                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CCID, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.cert_secuencia, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    //---------------------------->




                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CDES, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.unidad_fecha_ingreso.ToString(CFORMAT), times));
                    cell.Border = 0;
                    table.AddCell(cell);


                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CFESAL, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.unidad_fecha_embarque.ToString(CFORMAT), times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CTRIP, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(string.Format("{0} {1}", fila.unidad_buque, fila.unidad_viaje), times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    /*
                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CCOM, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.aisv_producto, times));
                    cell.Border = 0;
                    table.AddCell(cell);
                    */

                    //tabla contenido
                    document.Add(table);
                    #endregion

                    document.Add(Chunk.NEWLINE);
                    document.Add(compesation);
                   // document.Add(Chunk.NEWLINE);

                    // document.Add(Chunk.NEWLINE);

                    #region "Firma"
                    table = new PdfPTable(1);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 300 };
                    table.SetWidthPercentage(f, PageSize.A4);
                    iTextSharp.text.Image firma = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/signature.png"));
                   // iTextSharp.text.Image firma = iTextSharp.text.Image.GetInstance(dataDir + "signature.png");

                    cell = new PdfPCell();
                    cell.AddElement(firma);
                    cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 3;
                    cell.Border = 0;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.BorderWidthBottom = 1;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CGER, times));
                    cell.Border = 0;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CCEO, times));
                    cell.VerticalAlignment = 1;
                    cell.HorizontalAlignment = 1;
                    cell.Border = 0;
                    table.AddCell(cell);
                    //tabla firma
                    document.Add(table);
                    #endregion

                    #region "Logos"
                    iTextSharp.text.Image logo_cgsa = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                    //iTextSharp.text.Image logo_cgsa = iTextSharp.text.Image.GetInstance(dataDir + "logoContecon.jpg");
                    iTextSharp.text.Image logo_neutral = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoSeal.png"));
                   // iTextSharp.text.Image logo_neutral = iTextSharp.text.Image.GetInstance(dataDir + "logoSeal.png");
                    table = new PdfPTable(2);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 200, 100 };
                    table.SetWidthPercentage(f, PageSize.A4);

                    cell = new PdfPCell();
                    logo_cgsa.ScalePercent(20);
                    cell.AddElement(logo_cgsa);
                    cell.HorizontalAlignment = 2;
                    cell.VerticalAlignment = 3;
                    cell.Border = 0;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    logo_neutral.ScalePercent(20);
                    cell.AddElement(logo_neutral);
                    cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 3;
                    cell.Border = 0;
                    table.AddCell(cell);
                    //tabla logos
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

                    //table = new PdfPTable(3);
                    //table.HorizontalAlignment = 0;
                    //f = new float[] { 250, 125, 125 };
                    //table.SetWidthPercentage(f, PageSize.A4);

                    //iTextSharp.text.Image logo_tuv = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoTUV.png"));
                
                    //iTextSharp.text.Image logo_sambito = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoSambito.png"));
                 

                    //times = FontFactory.GetFont(familia, subfamilia, true, 8, style, BaseColor.BLACK);
                    //cell = new PdfPCell(new Phrase(CCERTI, times));
                    //cell.Border = 0;
                    //cell.HorizontalAlignment = 2;

                    //table.AddCell(cell);

                    //cell = new PdfPCell();
                    //logo_tuv.ScalePercent(15);
                    //cell.AddElement(logo_tuv);
                    //cell.HorizontalAlignment = 2;
                    //cell.VerticalAlignment = 3;

                    //cell.Border = 0;
                    //table.AddCell(cell);

                    //cell = new PdfPCell();
                    //logo_sambito.ScalePercent(15);
                    //cell.AddElement(logo_sambito);
                    //cell.HorizontalAlignment = 2;
                    //cell.VerticalAlignment = 3;

                    //cell.Border = 0;
                    //table.AddCell(cell);

                    //document.Add(table);
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