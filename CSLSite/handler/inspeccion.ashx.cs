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
    public class inspeccion : IHttpHandler, IRequiresSessionState
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
            var tabla = new Catalogos.bil_imprime_certificadosDataTable();
            var ta = new CatalogosTableAdapters.bil_imprime_certificadosTableAdapter();
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

           
            //obtener configuraciones 
            var cfgs = dbconfig.GetActiveConfig("billion", "tv", null);
            var cf_server_con = cfgs.Where(a => a.config_name.Contains("carbono_consulta_ins")).FirstOrDefault();
            var cf_server_qr = cfgs.Where(a => a.config_name.Contains("carbono_qr_ins")).FirstOrDefault();

            var server_consulta = cf_server_con != null && !string.IsNullOrEmpty(cf_server_con.config_value) ? cf_server_con.config_value : "https://www.cgsa.com.ec/carbono-neutro/";
            var server_qr = cf_server_qr != null && !string.IsNullOrEmpty(cf_server_qr.config_value) ? cf_server_qr.config_value : "CGINT06:8080";

            //campo de definicion de fecha
            string CFECHA_INS = TextFinal(nameof(CFECHA_INS), cfgs, lg);
            string CTITULO_INS = TextFinal(nameof(CTITULO_INS), cfgs, lg);
            string CCID_INS = TextFinal(nameof(CCID_INS), cfgs, lg);
            string CCOM_P_INS = TextFinal(nameof(CCOM_P_INS), cfgs, lg);
            string CLINEA_INS = TextFinal(nameof(CLINEA_INS), cfgs, lg);
            string CREFERENCIA_INS = TextFinal(nameof(CREFERENCIA_INS), cfgs, lg);
            string CBANDERA_INS = TextFinal(nameof(CBANDERA_INS), cfgs, lg);
            string CINSPE_INS = TextFinal(nameof(CINSPE_INS), cfgs, lg);
            string CEMISOR_P_INS = TextFinal(nameof(CEMISOR_P_INS), cfgs, lg);
            string CCEO_INS = TextFinal(nameof(CCEO_INS), cfgs, lg);
            string CGER_INS = TextFinal(nameof(CGER_INS), cfgs, lg);
            string CFORMAT_INS = TextFinal(nameof(CFORMAT_INS), cfgs, lg);

           

            CultureInfo enUS = new CultureInfo(CFECHA_INS);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            string attachment = string.Format("attachment;filename=CERT_{0}.pdf",fila.SECUENCIA);
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

                    Stream stream = app_start.CarbonoNeutro.BarcodeStream(fila.ID,server_consulta,server_qr);


                    if (stream != null)
                    {
                        Image barcode = Image.GetInstance(stream);
                        barcode.ScaleToFit(80, 80);
                        barcode.Alignment = iTextSharp.text.Image.UNDERLYING;
                        int varia = 0;
                        //if (!fila.cert_tipo.Contains("T"))
                        //{
                        //    varia = 18;
                        //}
                        barcode.SetAbsolutePosition(425, 340 + varia);
                        document.Add(barcode);
                    }
                   // ------------------------------------------------------->


                    //fuente
                    Font times = FontFactory.GetFont(familia, subfamilia, true, 12, style, BaseColor.WHITE);
                    Paragraph fecha = new Paragraph(string.Format("Ecuador, {0}", System.DateTime.Now.ToString("D", enUS)), times);
                    fecha.Alignment = Element.ALIGN_LEFT;

                    //fuente
                    times = FontFactory.GetFont(familia, subfamilia, true, 30, estilo2, BaseColor.WHITE);
                    Paragraph titulo = new Paragraph(CTITULO_INS, times);
                    titulo.Alignment = Element.ALIGN_LEFT;

                   
                    //parte blanca
                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);

                    var basic_line = CEMISOR_P_INS;
                    Paragraph endorse = new Paragraph(basic_line, times);


                    endorse.Alignment = Element.ALIGN_JUSTIFIED;

                    times = FontFactory.GetFont(familia, subfamilia, true, 18, style, BaseColor.BLACK);
                    Paragraph expotador = new Paragraph(fila.DESC_NAVE, times);
                    expotador.Alignment = Element.ALIGN_CENTER;

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);

                    string cco = CCOM_P_INS;
                    Paragraph compesation = new Paragraph(cco, times);
                    compesation.Alignment = Element.ALIGN_JUSTIFIED;

                   

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
                   
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(endorse);
                    document.Add(Chunk.NEWLINE);
                    document.Add(expotador);
                    document.Add(Chunk.NEWLINE);


                    #region "Contenidos"
                    PdfPTable table = new PdfPTable(2);
                    table.HorizontalAlignment = 0;
                    var fix = lg == "S" ? 175 : 125;
                    var f = new float[] { fix, 450 };
                    table.SetWidthPercentage(f, PageSize.A4);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    PdfPCell cell = new PdfPCell(new Phrase(CCID_INS, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.SECUENCIA, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    //--------------------------->
                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CLINEA_INS, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.LINEA, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    //---------------------------->




                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CREFERENCIA_INS, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(fila.REFERENCIA) ? "??????????" : fila.REFERENCIA, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CBANDERA_INS, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(fila.BANDERA) ? "??????????" : fila.BANDERA, times));
                    cell.Border = 0;
                    table.AddCell(cell);


                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(CINSPE_INS, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.IsFECHA_INSPENull() ? "??????????" : fila.FECHA_INSPE.ToString(CFORMAT_INS) , times));
                    cell.Border = 0;
                    table.AddCell(cell);

                  
                    //tabla contenido
                    document.Add(table);
                    #endregion

                    document.Add(Chunk.NEWLINE);
                    document.Add(compesation);


                    #region "Firma"

                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    table = new PdfPTable(1);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 300 };
                    table.SetWidthPercentage(f, PageSize.A4);
                   
                    string var = "CONTECON GUAYAQUIL CGSA";
                   

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(var, times));
                    cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 3;
                    cell.Border = 0;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.BorderWidthBottom = 1;
                    table.AddCell(cell);


                  
                    document.Add(table);
                    #endregion

                    #region "Logos"
                    iTextSharp.text.Image logo_cgsa = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));

                   
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    table = new PdfPTable(1);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 200 };
                    table.SetWidthPercentage(f, PageSize.A4);

                    cell = new PdfPCell();
                    logo_cgsa.ScalePercent(20);
                    cell.AddElement(logo_cgsa);
                    cell.HorizontalAlignment = 2;
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

                    table = new PdfPTable(3);
                    table.HorizontalAlignment = 0;
                    f = new float[] { 250, 125, 125 };
                    table.SetWidthPercentage(f, PageSize.A4);


                    document.Add(table);
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