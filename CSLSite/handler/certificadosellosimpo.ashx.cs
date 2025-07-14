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
    public class certificadosellosimpo : IHttpHandler, IRequiresSessionState
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
            var tabla = new Catalogos.sellos_certificado_imprime_impoDataTable();
            var ta = new CatalogosTableAdapters.sellos_certificado_imprime_impoTableAdapter();
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

            string S_CFORMAT = TextFinal(nameof(S_CFORMAT), cfgs, lg);
            string S_CFECHA = TextFinal(nameof(S_CFECHA), cfgs, lg);
            string S_CTITULO = TextFinal(nameof(S_CTITULO), cfgs, lg);
            string S_CSUB_P = TextFinal(nameof(S_CSUB_P), cfgs, lg);
            string S_CSUB_T = TextFinal(nameof(S_CSUB_T), cfgs, lg);
            string S_CEMISOR_P = TextFinal(nameof(S_CEMISOR_P), cfgs, lg);
            string S_FECHA = TextFinal(nameof(S_FECHA), cfgs, lg);
            string S_IMPORTADOR = TextFinal(nameof(S_IMPORTADOR), cfgs, lg);
            string S_CONTENEDOR = TextFinal(nameof(S_CONTENEDOR), cfgs, lg);
            string S_CARGA = TextFinal(nameof(S_CARGA), cfgs, lg);
            string S_NUMCERTI = TextFinal(nameof(S_NUMCERTI), cfgs, lg);
            string S_SELLO1 = TextFinal(nameof(S_SELLO1), cfgs, lg);
            string S_SELLO2 = TextFinal(nameof(S_SELLO2), cfgs, lg);
            string S_SELLO3 = TextFinal(nameof(S_SELLO3), cfgs, lg);
            string S_CSUB_S = TextFinal(nameof(S_CSUB_S), cfgs, lg);

            CultureInfo enUS = new CultureInfo(CFECHA);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            string attachment = string.Format("attachment;filename=CERT_SELLOS_{0}.pdf",fila.NUMERO_CERTIFICADO);
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

                    Stream stream = app_start.CarbonoNeutro.BarcodeStream(fila.NUMERO_CERTIFICADO,server_consulta,server_qr);


                    if (stream != null)
                    {
                        Image barcode = Image.GetInstance(stream);
                        barcode.ScaleToFit(80, 80);
                        barcode.Alignment = iTextSharp.text.Image.UNDERLYING;
                        int varia = 0;
                        //if (!fila.cert_tipo.Contains("T"))
                        //{
                            varia = 18;
                        //}
                        barcode.SetAbsolutePosition(425, 340 + varia);
                        document.Add(barcode);
                    }
                   // ------------------------------------------------------->


                    //fuente
                    Font times = FontFactory.GetFont(familia, subfamilia, true, 12, style, BaseColor.WHITE);
                    Paragraph fecha = new Paragraph(string.Format("Ecuador, {0}", fila.FECHAING.ToString("D", enUS)), times);
                    fecha.Alignment = Element.ALIGN_LEFT;

                    //fuente
                    times = FontFactory.GetFont(familia, subfamilia, true, 30, estilo2, BaseColor.WHITE);
                    Paragraph titulo = new Paragraph(S_CTITULO, times);
                    titulo.Alignment = Element.ALIGN_LEFT;



                    //parte blanca
                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);

                    var basic_line = S_CEMISOR_P;
                    Paragraph endorse = new Paragraph(basic_line, times);
                    endorse.Alignment = Element.ALIGN_JUSTIFIED;


                    times = FontFactory.GetFont(familia, subfamilia, true, 18, style, BaseColor.BLACK);
                    Paragraph expotador = new Paragraph(fila.NOMBRES, times);
                    expotador.Alignment = Element.ALIGN_CENTER;

                  

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
                    PdfPCell cell = new PdfPCell(new Phrase(S_FECHA, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.FECHAING.ToString(S_CFORMAT), times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    //--------------------------->
                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(S_IMPORTADOR, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.NOMBRES, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    //---------------------------->

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(S_CONTENEDOR, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.CNTR, times));
                    cell.Border = 0;
                    table.AddCell(cell);


                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(S_CARGA, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.NUMERO_CARGA, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(S_NUMCERTI, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.NUMERO_CEROS, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(S_SELLO1, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.SEAL_1, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(S_SELLO2, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.SEAL_2, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(S_SELLO3, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(fila.SEAL_3, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                  

                    //tabla contenido
                    document.Add(table);
                    #endregion

                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    #region "Firma"


                  
                    #endregion

                    #region "Logos"
                    iTextSharp.text.Image logo_cgsa = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
               
                    table = new PdfPTable(1);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 300 };
                    table.SetWidthPercentage(f, PageSize.A4);

                    cell = new PdfPCell();
                    logo_cgsa.ScalePercent(20);
                    cell.AddElement(logo_cgsa);
                    cell.HorizontalAlignment = 1;
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