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
using BillionEntidades;

namespace CSLSite.handler
{
    public class pesoscfs : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable => throw new NotImplementedException();

        public void ProcessRequest(HttpContext context)
        {
            HttpContext.Current.Response.Clear();

            
            string _mrn = HttpContext.Current.Request.QueryString["mrn"];
            string _msn = HttpContext.Current.Request.QueryString["msn"];
            string _hsn = HttpContext.Current.Request.QueryString["hsn"];
            string cMensajes;


            if (string.IsNullOrEmpty(_mrn))
            {
                HttpContext.Current.Response.Write("<p>Por favor seleccione el certificado</p>");
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;
            }


            var tabla = cfs_ver_certificado.ver_certificado(_mrn, _msn, _hsn, out cMensajes);
            if (tabla == null)
            {
                string close = CSLSite.CslHelper.ExitForm("No existe información que mostrar");
                HttpContext.Current.Response.Write(close);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;
            }

            if (tabla.Count <= 0)
            {
                string close = CSLSite.CslHelper.ExitForm("La busqueda no obtuvo resultados");
                HttpContext.Current.Response.Write(close);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
                return;

            }

            var fila = tabla.FirstOrDefault();

            int pagina = 1;
            int paginafin = 1;

            if (tabla.Count > 20)
            {
                paginafin = 2;
            }
             
            //obtener configuraciones 
            var cfgs = dbconfig.GetActiveConfig("billion", "tv", null);
            var cf_server_con = cfgs.Where(a => a.config_name.Contains("carbono_consulta_ins")).FirstOrDefault();
            var cf_server_qr = cfgs.Where(a => a.config_name.Contains("carbono_qr_ins")).FirstOrDefault();

            var server_consulta = cf_server_con != null && !string.IsNullOrEmpty(cf_server_con.config_value) ? cf_server_con.config_value : "https://www.cgsa.com.ec/carbono-neutro/";
            var server_qr = cf_server_qr != null && !string.IsNullOrEmpty(cf_server_qr.config_value) ? cf_server_qr.config_value : "CGINT06:8080";

           
            string NUMERO_CARGA = string.Format("{0}-{1}-{2}", fila.MRN, fila.MSN, fila.HSN);
            string AGENTE = string.Format("{0} - {1}", fila.AGENTE, fila.AGENTE_DESC);
            string CLIENTE = string.Format("{0} - {1}", fila.FACTURADO, fila.FACTURADO_DESC);

            CultureInfo enUS = new CultureInfo("es-ES");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            string attachment = string.Format("attachment;filename=CERT_{0}.pdf", NUMERO_CARGA);
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
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/background_pesos.jpg"));
                    imagen.ScaleToFit(PageSize.A4);
                    imagen.Alignment = iTextSharp.text.Image.UNDERLYING;
                    imagen.SetAbsolutePosition(0, 0);
                    writer.CloseStream = false;
                    document.Open();
                    document.Add(imagen);


                    //fuente
                    Font times = FontFactory.GetFont(familia, subfamilia, true, 12, style, BaseColor.WHITE);
                    Paragraph fecha = new Paragraph(string.Format("Ecuador, {0}", System.DateTime.Now.ToString("D", enUS)), times);
                    fecha.Alignment = Element.ALIGN_LEFT;

                    //fuente
                    times = FontFactory.GetFont(familia, subfamilia, true, 25, estilo2, BaseColor.WHITE);
                    Paragraph titulo = new Paragraph(fila.TITULO1, times);
                    titulo.Alignment = Element.ALIGN_CENTER;

                    times = FontFactory.GetFont(familia, subfamilia, true, 25, estilo2, BaseColor.WHITE);
                    Paragraph titulo_dos = new Paragraph(fila.TITULO2, times);
                    titulo_dos.Alignment = Element.ALIGN_CENTER;


                    #region "Separador"

                    times = FontFactory.GetFont(familia, subfamilia, true, 8, estilo2, BaseColor.BLACK);
                    Paragraph separador = new Paragraph("         ", times);
                    //document.Add(separador);

                    #endregion


                    document.Add(fecha);
                    document.Add(titulo);
                    document.Add(titulo_dos);
                    document.Add(separador);
                    document.Add(separador);
                    document.Add(separador);
                    //document.Add(Chunk.NEWLINE);


                    #region "Contenidos"
                    PdfPTable table = new PdfPTable(2);
                    table.HorizontalAlignment = 0;
                    
                    var fix =  165;
                    var f = new float[] { fix, 470 };
                    table.SetWidthPercentage(f, PageSize.A4);

                    times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);
                    PdfPCell cell = new PdfPCell(new Phrase("Número Carga:", times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(NUMERO_CARGA, times));
                    cell.Border = 0;
                    table.AddCell(cell);

        
                    times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase("Agente de Aduana:", times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(AGENTE, times));
                    cell.Border = 0;
                    table.AddCell(cell);

          

                    times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase("Cliente:", times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                    cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(CLIENTE) ? "??????????" : CLIENTE, times));
                    cell.Border = 0;
                    table.AddCell(cell);

                    //tabla contenido
                    document.Add(table);
                    #endregion

                    document.Add(Chunk.NEWLINE);
              
                    #region "Tabla detalle"

                    iTextSharp.text.Rectangle page = document.PageSize; //Obtenemos el tamaño
                    table = new PdfPTable(6);
                    table.WidthPercentage = 80;//Le damos un tamaño a la tabla, esta tomara en porcierto el ancho que ucupara
                    table.TotalWidth = page.Width - 90; //Le damos el tamaño de la tabla
                    table.LockedWidth = true;//Decimos que se bloque el tamaño de la tabla, esto para que no creesca dependiendo de la información
                    float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f }; //Declaramos un array con los tamaños de nuestras columnas deben de coincidir con el tamaño de columnas
                    table.SetWidths(widths); //Agregamos los anchos a nuestra tabla

                    //Agregamos una valor a nuestra celda de nuestra tabla
                    times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);

                    cell = new PdfPCell(new Phrase("# CERTIFICADO", times));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("ALTO", times));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("ANCHO", times));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("LARGO", times));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("PESO", times));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("VOLUMEN", times));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                   

                    times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);

                    int i = 0;
                    foreach (var Det in tabla)
                    {
                        if (i <= 20)
                        {
                            cell = new PdfPCell(new Phrase(String.IsNullOrEmpty(Det.NUMERO_CERTIFICADO) ? "..." : Det.NUMERO_CERTIFICADO, times));
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Det.P2D_ALTO == 0 ? "..." : Det.P2D_ALTO.ToString("N2"), times));
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Det.P2D_ANCHO == 0 ? "..." : Det.P2D_ANCHO.ToString("N2"), times));
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Det.P2D_LARGO == 0 ? "..." : Det.P2D_LARGO.ToString("N2"), times));
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Det.PESO == 0 ? "..." : Det.PESO.ToString("N2"), times));
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(Det.P2D_VOLUMEN == 0 ? "..." : Det.P2D_VOLUMEN.ToString("N2"), times));
                            cell.HorizontalAlignment = 1;
                            table.AddCell(cell);

                            
                        }
                            

                        i++;
                    }

                    //tabla contenido
                    document.Add(table);

                    document.Add(Chunk.NEWLINE);
                    #endregion


                    #region "Firma"

                    table = new PdfPTable(1);
                    table.HorizontalAlignment = 1;
                    f = new float[] { 300 };
                    table.SetWidthPercentage(f, PageSize.A4);
                    iTextSharp.text.Image firma = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/signature.png"));

                    if (tabla.Count > 20)
                    {
                       

                        times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase("", times));
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase("", times));
                        cell.VerticalAlignment = 1;
                        cell.HorizontalAlignment = 1;
                        cell.Border = 0;
                        table.AddCell(cell);
                        //tabla firma
                        document.Add(table);

                        document.Add(Chunk.NEWLINE);
                        document.Add(Chunk.NEWLINE);
                        document.Add(Chunk.NEWLINE);
                    }
                    else
                    {
                        cell = new PdfPCell();
                        cell.AddElement(firma);

                        cell.HorizontalAlignment = 1;
                        cell.VerticalAlignment = 3;
                        cell.Border = 0;
                        cell.BorderColorBottom = BaseColor.BLACK;
                        cell.BorderWidthBottom = 1;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase(fila.TEXTO1, times));
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase(fila.TEXTO2, times));
                        cell.VerticalAlignment = 1;
                        cell.HorizontalAlignment = 1;
                        cell.Border = 0;
                        table.AddCell(cell);
                        //tabla firma
                        document.Add(table);
                    }

                    //cell = new PdfPCell();
                    //cell.AddElement(firma);

                    //cell.HorizontalAlignment = 1;
                    //cell.VerticalAlignment = 3;
                    //cell.Border = 0;
                    //cell.BorderColorBottom = BaseColor.BLACK;
                    //cell.BorderWidthBottom = 1;
                    //table.AddCell(cell);

                    //times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                    //cell = new PdfPCell(new Phrase("Javier Lancha de Micheo", times));
                    //cell.Border = 0;
                    //cell.HorizontalAlignment = 1;
                    //table.AddCell(cell);

                    //times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                    //cell = new PdfPCell(new Phrase("C.E.O.", times));
                    //cell.VerticalAlignment = 1;
                    //cell.HorizontalAlignment = 1;
                    //cell.Border = 0;
                    //table.AddCell(cell);
                    ////tabla firma
                    //document.Add(table);
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

                 
                    separador = new Paragraph(string.Format("Página {0} de {1}", pagina, paginafin), times);
                    separador.Alignment = Element.ALIGN_CENTER;
                    document.Add(separador);

                    separador = new Paragraph("         ", times);
        
                    #endregion


                    #region "TablaPie"

                    table = new PdfPTable(3);
                    table.HorizontalAlignment = 0;
                    f = new float[] { 250, 125, 125 };
                    table.SetWidthPercentage(f, PageSize.A4);


                    document.Add(table);
                    #endregion

                    #region "segunda pagina"

                    if (tabla.Count > 20)
                    {
                        //nueva pagina
                        document.NewPage();

                        document.Add(imagen);

                        document.Add(fecha);
                        document.Add(titulo);
                        document.Add(titulo_dos);
                        document.Add(separador);
                        document.Add(separador);
                        document.Add(separador);

                        #region "Contenidos"

                        table = new PdfPTable(2);
                        table.HorizontalAlignment = 0;

                        fix = 165;
                        f = new float[] { fix, 470 };
                        table.SetWidthPercentage(f, PageSize.A4);

                        times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase("Número Carga:", times));
                        cell.Border = 0;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase(NUMERO_CARGA, times));
                        cell.Border = 0;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase("Agente de Aduana:", times));
                        cell.Border = 0;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase(AGENTE, times));
                        cell.Border = 0;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase("Cliente:", times));
                        cell.Border = 0;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(CLIENTE) ? "??????????" : CLIENTE, times));
                        cell.Border = 0;
                        table.AddCell(cell);

                        //tabla contenido
                        document.Add(table);
                        #endregion

                        document.Add(Chunk.NEWLINE);

                        #region "Tabla detalle"

                        page = document.PageSize; //Obtenemos el tamaño
                        table = new PdfPTable(6);
                        table.WidthPercentage = 80;//Le damos un tamaño a la tabla, esta tomara en porcierto el ancho que ucupara
                        table.TotalWidth = page.Width - 90; //Le damos el tamaño de la tabla
                        table.LockedWidth = true;//Decimos que se bloque el tamaño de la tabla, esto para que no creesca dependiendo de la información
                        widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f }; //Declaramos un array con los tamaños de nuestras columnas deben de coincidir con el tamaño de columnas
                        table.SetWidths(widths); //Agregamos los anchos a nuestra tabla

                        //Agregamos una valor a nuestra celda de nuestra tabla
                        times = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);

                        cell = new PdfPCell(new Phrase("# CERTIFICADO", times));
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("ALTO", times));
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("ANCHO", times));
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("LARGO", times));
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("PESO", times));
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("VOLUMEN", times));
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F3F3F3"));
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);


                        times = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);

                        i = 21;
                        foreach (var Det in tabla)
                        {
                            if (i <= tabla.Count)
                            {
                                cell = new PdfPCell(new Phrase(String.IsNullOrEmpty(Det.NUMERO_CERTIFICADO) ? "..." : Det.NUMERO_CERTIFICADO, times));
                                cell.HorizontalAlignment = 1;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Det.P2D_ALTO == 0 ? "..." : Det.P2D_ALTO.ToString("N2"), times));
                                cell.HorizontalAlignment = 1;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Det.P2D_ANCHO == 0 ? "..." : Det.P2D_ANCHO.ToString("N2"), times));
                                cell.HorizontalAlignment = 1;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Det.P2D_LARGO == 0 ? "..." : Det.P2D_LARGO.ToString("N2"), times));
                                cell.HorizontalAlignment = 1;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Det.PESO == 0 ? "..." : Det.PESO.ToString("N2"), times));
                                cell.HorizontalAlignment = 1;
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase(Det.P2D_VOLUMEN == 0 ? "..." : Det.P2D_VOLUMEN.ToString("N2"), times));
                                cell.HorizontalAlignment = 1;
                                table.AddCell(cell);


                            }


                            i++;
                        }

                        //tabla contenido
                        document.Add(table);

                        document.Add(Chunk.NEWLINE);
                        #endregion

                        #region "Firma"

                        table = new PdfPTable(1);
                        table.HorizontalAlignment = 1;
                        f = new float[] { 300 };
                        table.SetWidthPercentage(f, PageSize.A4);
                        firma = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/signature.png"));


                        cell = new PdfPCell();
                        cell.AddElement(firma);
                        cell.HorizontalAlignment = 1;
                        cell.VerticalAlignment = 3;
                        cell.Border = 0;
                        cell.BorderColorBottom = BaseColor.BLACK;
                        cell.BorderWidthBottom = 1;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 11, style, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase(fila.TEXTO1, times));
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        times = FontFactory.GetFont(familia, subfamilia, true, 11, estilo2, BaseColor.BLACK);
                        cell = new PdfPCell(new Phrase(fila.TEXTO2, times));
                        cell.VerticalAlignment = 1;
                        cell.HorizontalAlignment = 1;
                        cell.Border = 0;
                        table.AddCell(cell);
                        //tabla firma
                        document.Add(table);

                        #endregion

                        #region "Logos"

                        logo_cgsa = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));


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

                        pagina = 2;

                        separador = new Paragraph(string.Format("Página {0} de {1}", pagina, paginafin), times);
                        separador.Alignment = Element.ALIGN_CENTER;
                        document.Add(separador);

                        separador = new Paragraph("         ", times);
                        #endregion

                        #region "TablaPie"

                        table = new PdfPTable(3);
                        table.HorizontalAlignment = 0;
                        f = new float[] { 250, 125, 125 };
                        table.SetWidthPercentage(f, PageSize.A4);


                        document.Add(table);
                        #endregion

                    }
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