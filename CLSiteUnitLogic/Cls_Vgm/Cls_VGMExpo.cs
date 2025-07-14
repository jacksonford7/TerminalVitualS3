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

namespace CLSiteUnitLogic.Cls_Vgm
{
   public class Cls_VGMExpo
    {
        public string numeroCertificado { get; set; }
        public string fechaEmision { get; set; }
        public string Nave { get; set; }
        public string Viaje { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Contenedor { get; set; }
        public string Tara { get; set; }
        public string PesoNeto { get; set; }
        public string PesoVGM { get; set; }
        public string PayLoad { get; set; }
        public string Equipo { get; set; }
        public string CertificadoEquipo { get; set; }
        public string ServerQR { get; set; }

        public byte[] GenerarPdf()
        {
            using (var ms = new MemoryStream())
            {
                try
                {
                    Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();
                    var cb = writer.DirectContent;

                    var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
                    var fontSmall = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.BLACK);

                    var fullUrl = ServerQR.StartsWith("http") ? ServerQR : "http://" + ServerQR;
                    if (!fullUrl.EndsWith("/")) fullUrl += "/";
                    fullUrl += CertificadoEquipo;

                    var fecha = new Paragraph("Ecuador, " + DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy"), fontSmall)
                    {
                        Alignment = Element.ALIGN_LEFT,
                        SpacingAfter = 3f
                    };
                    document.Add(fecha);

                    PdfPTable header = new PdfPTable(3);
                    header.WidthPercentage = 100;
                    header.SetWidths(new float[] { 30, 50, 22 });
                    header.DefaultCell.Border = Rectangle.NO_BORDER;

                    var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                    logo.ScaleAbsolute(170f, 60f);
                    header.AddCell(new PdfPCell(logo)
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        PaddingTop = 5f
                    });

                    var streamBarra = HttpTool.BarStream(CertificadoEquipo, ServerQR);
                    if (streamBarra != null)
                    {
                        var barraImg = Image.GetInstance(streamBarra);
                        barraImg.ScaleToFit(280f, 60f);
                        header.AddCell(new PdfPCell(barraImg)
                        {
                            Border = Rectangle.NO_BORDER,
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

                        header.AddCell(new PdfPCell(barraGenerica)
                        {
                            Border = Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }





                    var qr = new BarcodeQRCode(fullUrl, 135, 135, null);
                    var qrImg = qr.GetImage();
                    qrImg.ScaleToFit(70f, 70f);
                    header.AddCell(new PdfPCell(qrImg)
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        PaddingRight = 80f
                    });

                    document.Add(header);

                    cb.MoveTo(document.Left, document.Top - 83f); cb.LineTo(document.Right, document.Top - 83f); cb.Stroke();
                    document.Add(new Paragraph("CERTIFICADO DE PESO\nVERIFIED GROSS MASS", fontTitulo)
                    {
                        Alignment = Element.ALIGN_CENTER
                    });
                    cb.MoveTo(document.Left, document.Top - 132f); cb.LineTo(document.Right, document.Top - 132f); cb.Stroke();

                    PdfPTable filaCabecera = new PdfPTable(2);
                    filaCabecera.WidthPercentage = 100;
                    filaCabecera.SetWidths(new float[] { 50, 50 });
                    filaCabecera.AddCell(new PdfPCell(new Phrase("Certificado No.: " + numeroCertificado, fontSmall))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        PaddingTop = 16f,
                        PaddingBottom = 12f
                    });
                    filaCabecera.AddCell(new PdfPCell(new Phrase("Fecha de Emisión: " + fechaEmision, fontSmall))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        PaddingTop = 16f,
                        PaddingBottom = 12f
                    });
                    document.Add(filaCabecera);
                    cb.MoveTo(document.Left, writer.GetVerticalPosition(true) - 1f); cb.LineTo(document.Right, writer.GetVerticalPosition(true) - 1f); cb.Stroke();

                    AgregarSeccion(document, "INFORMACIÓN BUQUE", fontSmall, ("Nombre del Buque:", Nave), ("No. de Viaje:", Viaje));
                    AgregarSeccion(document, "INFORMACIÓN EXPORTADOR", fontSmall, ("RUC:", Ruc), ("Razón Social:", RazonSocial));
                    AgregarSeccion(document, "INFORMACIÓN CONTENEDOR", fontSmall,
                        ("Contenedor No.:", Contenedor), ("Tara (Kg):", Tara),
                        ("Peso Neto (Kg):", PesoNeto), ("Peso VGM (Kg):", PesoVGM),
                        ("Max. Payload (Kg):", PayLoad), ("Equipo de Pesaje:", Equipo),
                        ("No. Certificado Equipo:", CertificadoEquipo));

                    document.Add(SeccionTitulo("MARCO LEGAL", fontSmall));
                    document.Add(new Paragraph("RESOLUCIÓN MTOP-SPTM-2016-0088-R", fontSmall));
                    document.Add(new Paragraph("Importante:", fontSmall));
                    document.Add(new Paragraph("Este certificado tiene validez si el sello de seguridad no ha perdido su integridad", fontSmall));
                    document.Add(Chunk.NEWLINE);

                    // 🔹 Código de barras inferior
                    var streamBarraInferior = HttpTool.BarStream(CertificadoEquipo, ServerQR);
                    if (streamBarraInferior != null)
                    {
                        var imgBarraFinal = Image.GetInstance(streamBarraInferior);
                        imgBarraFinal.ScaleToFit(300f, 60f);

                        var tablaCodigoFinal = new PdfPTable(1) { WidthPercentage = 100 };
                        tablaCodigoFinal.AddCell(new PdfPCell(imgBarraFinal)
                        {
                            Border = Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            PaddingTop = 10f,
                            PaddingBottom = 10f
                        });
                        document.Add(tablaCodigoFinal);
                    }
                    else
                    {
                        // Fallback si falla el stream
                        var barraFallback = new Barcode128 { Code = CertificadoEquipo, BarHeight = 60f, X = 1.4f, Font = null };
                        var fallbackImg = barraFallback.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                        fallbackImg.ScaleToFit(300f, 60f);

                        var tablaFallback = new PdfPTable(1) { WidthPercentage = 100 };
                        tablaFallback.AddCell(new PdfPCell(fallbackImg)
                        {
                            Border = Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            PaddingTop = 10f,
                            PaddingBottom = 10f
                        });
                        document.Add(tablaFallback);
                    }

                    document.Add(new Paragraph("INFORMACIÓN DE SEGURIDAD", fontSmall)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 5f
                    });

                    document.Close();
                    return ms.ToArray();
                }
                catch
                {
                    return null;
                }
            }
        }

        private void AgregarSeccion(Document doc, string titulo, Font fuente, params (string, string)[] pares)
        {
            doc.Add(SeccionTitulo(titulo, fuente));
            foreach (var (label, valor) in pares)
                doc.Add(DatosFila(label, valor, fuente));
        }

        private PdfPTable SeccionTitulo(string texto, Font fuente)
        {
            var tabla = new PdfPTable(1) { WidthPercentage = 100 };
            tabla.AddCell(new PdfPCell(new Phrase(texto, fuente))
            {
                Border = Rectangle.BOX,
                BorderWidth = 0.5f,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingTop = 4f,
                PaddingBottom = 4f
            });
            tabla.SpacingBefore = 10f;
            tabla.SpacingAfter = 4f;
            return tabla;
        }

        private PdfPTable DatosFila(string label, string valor, Font fuente)
        {
            var tabla = new PdfPTable(2);
            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] { 25, 75 });
            tabla.AddCell(new PdfPCell(new Phrase(label, fuente))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 4f
            });
            tabla.AddCell(new PdfPCell(new Phrase(valor, fuente))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 4f
            });
            return tabla;
        }

     
    }
}
