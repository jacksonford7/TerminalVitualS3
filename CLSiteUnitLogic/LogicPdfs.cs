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
using Font = iTextSharp.text.Font;
using Rectangule = iTextSharp.text.Rectangle;

namespace CLSiteUnitLogic
{
   public class LogicPdfs
    {

        public static byte[] GenerarPdfEirExpo(List<CertificadoEIR> Detalle)
        {
            var configQR = Configuracion.ObtenerConfiguracion("SERVER_QR");
            string serverQR = configQR?.Value ?? "";
            var fila = Detalle.LastOrDefault();
            if (fila == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                var cb = writer.DirectContent;

                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                var fontLabel = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
                var fontValor = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                var fontFecha = FontFactory.GetFont(FontFactory.HELVETICA, 8);

                // 🔹 Fecha
                var fecha = new Paragraph("Ecuador, " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy"), fontFecha)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                doc.Add(fecha);

                // 🔹 URL QR
                string fullUrl = serverQR.StartsWith("http") ? serverQR : "http://" + serverQR;
                if (!fullUrl.EndsWith("/")) fullUrl += "/";
                fullUrl += fila.Container;

                // 🔹 Encabezado
                PdfPTable encabezado = new PdfPTable(3);
                encabezado.WidthPercentage = 100;
                encabezado.SetWidths(new float[] { 30, 50, 22 });

                var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                logo.ScaleAbsolute(170f, 60f);
                encabezado.AddCell(new PdfPCell(logo)
                {
                    Border = Rectangule.NO_BORDER,
                    PaddingTop = 5f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                var streamBarra = HttpTool.BarStream(fila.Container, serverQR);
                if (streamBarra != null)
                {
                    var barraImg = Image.GetInstance(streamBarra);
                    barraImg.ScaleToFit(280f, 60f);
                    encabezado.AddCell(new PdfPCell(barraImg)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else
                {
                    var barcode = new Barcode128
                    {
                        Code = "0000000000000000000000",
                        StartStopText = false,
                        CodeType = Barcode128.CODE128,
                        Font = null,
                        BarHeight = 60f,
                        X = 1.5f
                    };

                    var barraGenerica = barcode.CreateImageWithBarcode(cb, null, null);
                    barraGenerica.ScaleToFit(360f, 60f);

                    encabezado.AddCell(new PdfPCell(barraGenerica)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }

                var qr = new BarcodeQRCode(fullUrl, 135, 135, null);
                var qrImg = qr.GetImage();
                qrImg.ScaleToFit(70f, 70f);
                encabezado.AddCell(new PdfPCell(qrImg)
                {
                    Border = Rectangule.NO_BORDER,
                    PaddingRight = 80f,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });

                doc.Add(encabezado);
                doc.Add(LineaSeparadora());
                // 🔹 Título y líneas
                doc.Add(new Paragraph("ENTREGA DE CONTENEDOR EXPORTACIÓN", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 5f
                });

                doc.Add(new Paragraph("INFORMACIÓN SOBRE LA CARGA:", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 5f,
                    SpacingAfter = 5f
                });
                doc.Add(LineaSeparadora());
                doc.Add(Chunk.NEWLINE);

                // 🔹 Datos
                AgregarCampo(doc, "Placa Carga:", fila.placa, fontLabel, fontValor);
                AgregarCampo(doc, "Cédula Chofer:", fila.cedula_chofer, fontLabel, fontValor);
                AgregarCampo(doc, "Nombre Chofer:", fila.nombre_chofer, fontLabel, fontValor);
                AgregarCampo(doc, "Contenedor:", fila.Container, fontLabel, fontValor);
                AgregarCampo(doc, "Número Carga:", fila.numero_carga, fontLabel, fontValor);
                AgregarCampo(doc, "Cliente:", fila.cliente, fontLabel, fontValor);
                AgregarCampo(doc, "Iso:", fila.iso, fontLabel, fontValor);
                AgregarCampo(doc, "Línea Naviera:", fila.linea_naviera, fontLabel, fontValor);
                AgregarCampo(doc, "Nave:", fila.nave, fontLabel, fontValor);
                AgregarCampo(doc, "Referencia:", fila.referencia, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 1:", fila.seal1, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 2:", fila.seal2, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 3:", fila.seal3, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 4:", fila.seal4, fontLabel, fontValor);
                AgregarCampo(doc, "Salida Camión:", fila.EventDate.ToString("dd/MM/yyyy HH:mm"), fontLabel, fontValor);
                AgregarCampo(doc, "Peso Contenedor (KG):", fila.peso, fontLabel, fontValor);

                // 🔹 Código de barras inferior
                var streamBarraInferior = HttpTool.BarStream(fila.Container, serverQR);
                if (streamBarraInferior != null)
                {
                    var imgBarraFinal = Image.GetInstance(streamBarraInferior);
                    imgBarraFinal.ScaleToFit(300f, 60f);

                    var tablaCodigoFinal = new PdfPTable(1) { WidthPercentage = 100 };
                    tablaCodigoFinal.AddCell(new PdfPCell(imgBarraFinal)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        PaddingTop = 10f,
                        PaddingBottom = 10f
                    });
                    doc.Add(tablaCodigoFinal);
                }
                else
                {
                    // Fallback si falla el stream
                    var barraFallback = new Barcode128 { Code = fila.Container, BarHeight = 60f, X = 1.4f, Font = null };
                    var fallbackImg = barraFallback.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                    fallbackImg.ScaleToFit(300f, 60f);

                    var tablaFallback = new PdfPTable(1) { WidthPercentage = 100 };
                    tablaFallback.AddCell(new PdfPCell(fallbackImg)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        PaddingTop = 10f,
                        PaddingBottom = 10f
                    });
                    doc.Add(tablaFallback);
                }
                doc.Add(new Paragraph("INFORMACIÓN DE SEGURIDAD", fontLabel)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 5f
                });

                doc.Close();

                return ms.ToArray();
            }
        }

        public static byte[] GenerarPdfImpo(List<CertificadoEIR> Detalle)
        {
            var configQR = Configuracion.ObtenerConfiguracion("SERVER_QR");
            string serverQR = configQR?.Value ?? "";
            var fila = Detalle.LastOrDefault();
            if (fila == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                var cb = writer.DirectContent;

                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                var fontLabel = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
                var fontValor = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                var fontFecha = FontFactory.GetFont(FontFactory.HELVETICA, 8);

                // 🔹 Fecha
                var fecha = new Paragraph("Ecuador, " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy"), fontFecha)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                doc.Add(fecha);

                // 🔹 URL QR
                string fullUrl = serverQR.StartsWith("http") ? serverQR : "http://" + serverQR;
                if (!fullUrl.EndsWith("/")) fullUrl += "/";
                fullUrl += fila.Container;

                // 🔹 Encabezado
                PdfPTable encabezado = new PdfPTable(3);
                encabezado.WidthPercentage = 100;
                encabezado.SetWidths(new float[] { 30, 50, 22 });

                var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                logo.ScaleAbsolute(170f, 60f);
                encabezado.AddCell(new PdfPCell(logo)
                {
                    Border = Rectangule.NO_BORDER,
                    PaddingTop = 5f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                var streamBarra = HttpTool.BarStream(fila.Container, serverQR);
                if (streamBarra != null)
                {
                    var barraImg = Image.GetInstance(streamBarra);
                    barraImg.ScaleToFit(280f, 60f);
                    encabezado.AddCell(new PdfPCell(barraImg)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }
                else
                {
                    var barcode = new Barcode128
                    {
                        Code = "0000000000000000000000",
                        StartStopText = false,
                        CodeType = Barcode128.CODE128,
                        Font = null,
                        BarHeight = 60f,
                        X = 1.5f
                    };

                    var barraGenerica = barcode.CreateImageWithBarcode(cb, null, null);
                    barraGenerica.ScaleToFit(360f, 60f);

                    encabezado.AddCell(new PdfPCell(barraGenerica)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });
                }

                var qr = new BarcodeQRCode(fullUrl, 135, 135, null);
                var qrImg = qr.GetImage();
                qrImg.ScaleToFit(70f, 70f);
                encabezado.AddCell(new PdfPCell(qrImg)
                {
                    Border = Rectangule.NO_BORDER,
                    PaddingRight = 80f,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });

                doc.Add(encabezado);
                doc.Add(LineaSeparadora());
                // 🔹 Título y líneas
                doc.Add(new Paragraph("ENTREGA DE CONTENEDOR EXPORTACIÓN", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 5f
                });

                doc.Add(new Paragraph("INFORMACIÓN SOBRE LA CARGA:", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 5f,
                    SpacingAfter = 5f
                });
                doc.Add(LineaSeparadora());
                doc.Add(Chunk.NEWLINE);

                // 🔹 Datos
                AgregarCampo(doc, "Placa Carga:", fila.placa, fontLabel, fontValor);
                AgregarCampo(doc, "Cédula Chofer:", fila.cedula_chofer, fontLabel, fontValor);
                AgregarCampo(doc, "Nombre Chofer:", fila.nombre_chofer, fontLabel, fontValor);
                AgregarCampo(doc, "Contenedor:", fila.Container, fontLabel, fontValor);
                AgregarCampo(doc, "Número Carga:", fila.numero_carga, fontLabel, fontValor);
                AgregarCampo(doc, "Cliente:", fila.cliente, fontLabel, fontValor);
                AgregarCampo(doc, "Iso:", fila.iso, fontLabel, fontValor);
                AgregarCampo(doc, "Línea Naviera:", fila.linea_naviera, fontLabel, fontValor);
                AgregarCampo(doc, "Nave:", fila.nave, fontLabel, fontValor);
                AgregarCampo(doc, "Referencia:", fila.referencia, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 1:", fila.seal1, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 2:", fila.seal2, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 3:", fila.seal3, fontLabel, fontValor);
                AgregarCampo(doc, "Sello 4:", fila.seal4, fontLabel, fontValor);
                AgregarCampo(doc, "Salida Camión:", fila.EventDate.ToString("dd/MM/yyyy HH:mm"), fontLabel, fontValor);
                AgregarCampo(doc, "Peso Contenedor (KG):", fila.peso, fontLabel, fontValor);

                // 🔹 Código de barras inferior
                var streamBarraInferior = HttpTool.BarStream(fila.Container, serverQR);
                if (streamBarraInferior != null)
                {
                    var imgBarraFinal = Image.GetInstance(streamBarraInferior);
                    imgBarraFinal.ScaleToFit(300f, 60f);

                    var tablaCodigoFinal = new PdfPTable(1) { WidthPercentage = 100 };
                    tablaCodigoFinal.AddCell(new PdfPCell(imgBarraFinal)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        PaddingTop = 10f,
                        PaddingBottom = 10f
                    });
                    doc.Add(tablaCodigoFinal);
                }
                else
                {
                    // Fallback si falla el stream
                    var barraFallback = new Barcode128 { Code = fila.Container, BarHeight = 60f, X = 1.4f, Font = null };
                    var fallbackImg = barraFallback.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                    fallbackImg.ScaleToFit(300f, 60f);

                    var tablaFallback = new PdfPTable(1) { WidthPercentage = 100 };
                    tablaFallback.AddCell(new PdfPCell(fallbackImg)
                    {
                        Border = Rectangule.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        PaddingTop = 10f,
                        PaddingBottom = 10f
                    });
                    doc.Add(tablaFallback);
                }
                doc.Add(new Paragraph("INFORMACIÓN DE SEGURIDAD", fontLabel)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 5f
                });

                doc.Close();

                return ms.ToArray();
            }
        }



        public static  void ExportarPDF(string tipo)
        {
            MemoryStream ms = new MemoryStream();

            var doc = new Document();
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            if (tipo == "Cntr")
            {
                var lista = HttpContext.Current.Session["DatosContenedorCntr"] as List<ContenedorVista>;
                if (lista == null || lista.Count == 0) return;

                doc.Add(new Paragraph("REPORTE DE CONTENEDORES"));
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f });

                string[] headers = { "Vigencia De Cas", "Contenedor", "Categoría", "Estado", "Naviera", "Referencia Nave", "Tipo de Carga" };
                foreach (var h in headers) table.AddCell(new Phrase(h));

                foreach (var item in lista)
                {
                    table.AddCell(item.CAS?.ToString("dd/MM/yyyy") ?? "");
                    table.AddCell(item.CONTENEDOR ?? "");
                    table.AddCell(item.Category ?? "");
                    table.AddCell(item.TIPO_STATE ?? "");
                    table.AddCell(item.LINE_OP ?? "");
                    table.AddCell(item.IB_ACTUAL_VISIT ?? "");
                    table.AddCell(item.FRGHT_KIND ?? "");
                }

                doc.Add(table);
            }
            else if (tipo == "Book")
            {
                var lista = HttpContext.Current.Session["DatosContenedorBooking"] as List<Cls_Container.Cls_Container>;
                if (lista == null || lista.Count == 0) return;

                doc.Add(new Paragraph("REPORTE DE BOOKING"));
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 2, 2, 2, 2, 2, 2 });

                string[] headers = { "Contenedor", "Tráfico", "Estado", "Naviera", "Booking", "Temperatura", "Tipo de Carga" };
                foreach (var h in headers) table.AddCell(new Phrase(h));

                foreach (var item in lista)
                {
                    table.AddCell(item.CNTR_CONTAINER ?? "");
                    table.AddCell(item.CNTR_TYPE ?? "");
                    table.AddCell(item.CNTR_YARD_STATUS ?? "");
                    table.AddCell(item.CNTR_CLNT_CUSTOMER_LINE ?? "");
                    table.AddCell(item.CNTR_BKNG_BOOKING ?? "");
                    table.AddCell(item.CNTR_TEMPERATURE == 0 ? "Sin temperatura" : item.CNTR_TEMPERATURE.ToString());
                    table.AddCell(item.CNTR_LCL_FCL ?? "");
                }

                doc.Add(table);
            }
            else if (tipo == "CargaSuelta")
            {
                var lista = HttpContext.Current.Session["DatosContenedorCargaSuelta"] as List<CargaSueltaVista>;
                if (lista == null || lista.Count == 0) return;

                doc.Add(new Paragraph("REPORTE DE CARGA SUELTA"));
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(8);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1.5f, 2, 1.5f, 1.5f, 1.5f, 2, 2, 2 });

                string[] headers = { "Línea", "Nave", "DAI", "Bultos", "Estado", "Ingreso", "Desconsolidación", "Despacho" };
                foreach (var h in headers) table.AddCell(new Phrase(h));

                foreach (var item in lista)
                {
                    table.AddCell(item.LINEA ?? "");
                    table.AddCell(item.NAVE ?? "");
                    table.AddCell(item.DAI ?? "");
                    table.AddCell(item.BULTOS.ToString());
                    table.AddCell(item.ESTADO ?? "");
                    table.AddCell(item.FECHAINGRESO?.ToString("dd/MM/yyyy") ?? "");
                    table.AddCell(item.FECHADESCONSOLIDA?.ToString("dd/MM/yyyy") ?? "");
                    table.AddCell(item.FECHADESPACHO?.ToString("dd/MM/yyyy") ?? "");
                }

                doc.Add(table);
            }



            doc.Close();

            byte[] bytes = ms.ToArray();
            ms.Close();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment;filename={tipo}_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }



        // Reutilizable

        private static PdfPTable LineaSeparadora()
        {
            var linea = new PdfPTable(1) { WidthPercentage = 100 };
            linea.AddCell(new PdfPCell(new Phrase(""))
            {
                Border = Rectangule.TOP_BORDER,
                BorderWidthTop = 1f,
                PaddingTop = 5f
            });
            return linea;
        }
        private static void AgregarCampo(Document doc, string label, string value, Font fontLabel, Font fontValor)
        {
            PdfPTable tabla = new PdfPTable(2);
            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] { 25, 75 });

            PdfPCell cellLabel = new PdfPCell(new Phrase(label, fontLabel))
            {
                Border = Rectangule.NO_BORDER,
                PaddingTop = 3f,
                PaddingBottom = 2f
            };

            PdfPCell cellValor = new PdfPCell(new Phrase(value, fontValor))
            {
                Border = Rectangule.BOX,
                PaddingTop = 4f,
                PaddingBottom = 4f
            };

            tabla.AddCell(cellLabel);
            tabla.AddCell(cellValor);
            doc.Add(tabla);
            doc.Add(new Paragraph("\n"));
        }

    }
}
