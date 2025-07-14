using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CLSiteUnitLogic.Cls_Peso
{
    public class Cls_Peso_Expo
    {

        public decimal CODIGO_CERTIFICADO { get; set; }
        public string CONTENEDOR { get; set; }
        public string PLACA { get; set; }
        public string REFERENCIA { get; set; }
        public string NAVE { get; set; }
        public decimal PESO_BALANZA { get; set; }
        public decimal PESO_VEHICULO { get; set; }
        public decimal PESO_BRUTO { get; set; }
        public decimal TARA { get; set; }
        public decimal PESO_NETO { get; set; }
        public string USUARIO { get; set; }
        public DateTime FECHA { get; set; }
        public string VIAJE { get; set; }
        public string SERVERQR { get; set; }
        public byte[] GenerarPdf()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                var cb = writer.DirectContent;

                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var fontLabel = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                var fontValor = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                // HEADER LINEA SUPERIOR
                cb.MoveTo(doc.Left, doc.Top - 50f);
                cb.Stroke();

                // HEADER CON LOGO + BARRA + QR (TAMAÑO TRIPLICADO)
                PdfPTable encabezado = new PdfPTable(3);
                encabezado.WidthPercentage = 100;
                encabezado.SetWidths(new float[] { 25, 40, 25 });

                string logoPath = HttpContext.Current.Server.MapPath("~/shared/imgs/logoContecon.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleAbsolute(145f, 55f);
                var cellLogo = new PdfPCell(logo)
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 10f // Espacio superior para bajar el logo
                };



                encabezado.AddCell(cellLogo);

                var barcodeIni = new Barcode128
                {
                    Code = CONTENEDOR + "FIN",
                    BarHeight = 50f,
                    X = 1.2f,
                    Font = null
                };


                var barraImg = barcodeIni.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                var cellBarra = new PdfPCell(barraImg)
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 10f, // Baja un poco la barra,
                    PaddingLeft = 20f,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                encabezado.AddCell(cellBarra);

                var qr = new BarcodeQRCode("https://tu-enlace.com/ver?cont=", 0, 0, null);
                var qrImg = qr.GetImage();
                qrImg.ScaleAbsolute(110, 62f);
                encabezado.AddCell(new PdfPCell(qrImg)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingTop = 4f
                });

                doc.Add(encabezado);

                //// LINEA HORIZONTAL DEBAJO DEL HEADER
                //cb.MoveTo(doc.Left, doc.Top - 130f);
                //cb.LineTo(doc.Right, doc.Top - 130f);
                //cb.Stroke();

                // TITULO
                var titulo = new Paragraph("CERTIFICADO DE PESO", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 2f,
                    SpacingAfter = 10f
                };
                doc.Add(titulo);

                // Línea horizontal como tabla fluida debajo del título
                PdfPTable lineaTitulo = new PdfPTable(1);
                lineaTitulo.WidthPercentage = 100;
                PdfPCell celdaLinea = new PdfPCell(new Phrase(""))
                {
                    Border = Rectangle.TOP_BORDER,
                    BorderWidthTop = 1f,
                    PaddingTop = 5f,
                    PaddingBottom = 10f
                };
                lineaTitulo.AddCell(celdaLinea);
                doc.Add(lineaTitulo);
                doc.Add(new Paragraph("\n"));

                PdfPTable lineaTitulo2 = new PdfPTable(1);
                lineaTitulo2.WidthPercentage = 100;
                PdfPCell celdaLinea2 = new PdfPCell(new Phrase(""))
                {
                    Border = Rectangle.TOP_BORDER,
                    BorderWidthTop = 1f,
                    PaddingTop = 5f,
                    PaddingBottom = 10f
                };
                lineaTitulo2.AddCell(celdaLinea2);
                doc.Add(lineaTitulo2);
                doc.Add(new Paragraph("\n"));


                // CAMPOS GENERALES
                AgregarCampoConSalto(cb, doc, "Fecha Impresión:", FECHA.ToString("dd/MM/yyyy hh:mm tt"), fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Contenedor:", CONTENEDOR, fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Vessel Reference:", REFERENCIA, fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Navío:", NAVE, fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Viaje:", VIAJE, fontLabel, fontValor);
                PdfPTable lineaNextViaje = new PdfPTable(1);
                lineaNextViaje.WidthPercentage = 100;
                PdfPCell celdaLineaViaje = new PdfPCell(new Phrase(""))
                {
                    Border = Rectangle.TOP_BORDER,
                    BorderWidthTop = 1f,
                    PaddingTop = 5f,
                    PaddingBottom = 10f
                };
                lineaNextViaje.AddCell(celdaLineaViaje);
                doc.Add(lineaNextViaje);

                doc.Add(new Paragraph("\n"));

                PdfPTable lineaNextViaje2 = new PdfPTable(1);
                lineaNextViaje2.WidthPercentage = 100;
                PdfPCell celdaLineaViaje2 = new PdfPCell(new Phrase(""))
                {
                    Border = Rectangle.TOP_BORDER,
                    BorderWidthTop = 1f,
                    PaddingTop = 5f,
                    PaddingBottom = 10f
                };
                lineaNextViaje2.AddCell(celdaLineaViaje2);
                doc.Add(lineaNextViaje2);
                doc.Add(new Paragraph("\n"));
                //// LINEA HORIZONTAL DEBAJO DE VIAJE
                //cb.MoveTo(doc.Left, doc.Top - 350f);
                //cb.LineTo(doc.Right, doc.Top - 350f);
                //cb.Stroke();

                // BLOQUE DE PESOS
                AgregarCampoConSalto(cb, doc, "Peso Balanza:", PESO_BALANZA.ToString(), fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Peso Vehículo (-):", PESO_VEHICULO.ToString(), fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Peso Bruto:", PESO_BRUTO.ToString(), fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Tara (-):", TARA.ToString(), fontLabel, fontValor);
                AgregarCampoConSalto(cb, doc, "Peso Neto:", PESO_NETO.ToString(), fontLabel, fontValor);

                PdfPTable lineaPesoNeto = new PdfPTable(1);
                lineaPesoNeto.WidthPercentage = 100;
                PdfPCell celdaLineaPesoNeto = new PdfPCell(new Phrase(""))
                {
                    Border = Rectangle.TOP_BORDER,
                    BorderWidthTop = 1f,
                    PaddingTop = 5f,
                    PaddingBottom = 10f
                };
                lineaPesoNeto.AddCell(celdaLineaPesoNeto);
                doc.Add(lineaPesoNeto);
                doc.Add(new Paragraph("\n"));

                // LINEA HORIZONTAL DEBAJO DE PESO NETO
                cb.MoveTo(doc.Left, 200f);
                cb.LineTo(doc.Right, 200f);
                cb.Stroke();

                // CODIGO DE BARRAS FINAL (DOBLE TAMAÑO)
                var barcodeFin = new Barcode128
                {
                    Code = CONTENEDOR + "FIN",
                    BarHeight = 50f,
                    X = 1.5f,
                    Font = null
                };
                var barraFinal = barcodeFin.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                barraFinal.ScalePercent(160);
                barraFinal.SetAbsolutePosition((PageSize.A4.Width - barraFinal.ScaledWidth) / 2, 60f);
                doc.Add(barraFinal);

                // INFORMACIÓN DE SEGURIDAD DEBAJO DEL CÓDIGO
                doc.Add(new Paragraph("INFORMACIÓN DE SEGURIDAD", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 25f
                });

                doc.Close();
                return ms.ToArray();
            }
        }

        private void AgregarCampoConSalto(PdfContentByte cb, Document doc, string label, string value, Font fontLabel, Font fontValor)
        {
            PdfPTable tabla = new PdfPTable(2);
            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] { 25, 75 });

            PdfPCell cellLabel = new PdfPCell(new Phrase(label, fontLabel))
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 5f,
                PaddingBottom = 4f
            };

            PdfPCell cellValor = new PdfPCell(new Phrase(value, fontValor))
            {
                Border = Rectangle.BOX,
                PaddingTop = 6f,
                PaddingBottom = 6f
            };

            tabla.AddCell(cellLabel);
            tabla.AddCell(cellValor);
            doc.Add(tabla);

            doc.Add(new Paragraph("\n"));
        }

        public PdfPCell GenerarCelda(string texto, Font font, BaseColor bgColor, int align = Element.ALIGN_LEFT)
        {
            return new PdfPCell(new Phrase(texto, font))
            {
                BackgroundColor = bgColor,
                HorizontalAlignment = align,
                Padding = 5,
                Border = Rectangle.BOX
            };
        }



    }
}
