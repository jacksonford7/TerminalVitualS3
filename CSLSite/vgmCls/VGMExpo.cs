using BillionEntidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;

namespace CSLSite.vgmCls
{
    public class VGMExpo
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

        public byte[] GenerarPdf()
        {
            //  CultureInfo enUS = new CultureInfo(CFECHA);
            byte[] result = null;
            using (var ms = new MemoryStream())
            {
                try
                {
                    Document document = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.AddTitle("VERIFICACIÓN DE MASA BRUTA VGM");
                    document.AddCreator("CONTECON GUAYAQUIL ECUADOR AN ICTSI GROUP COMPANY");

                    #region "Logo CGSA"
                    iTextSharp.text.Image _logoCabecera = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                    _logoCabecera.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    float percentage = 0.0f;
                    percentage = 200 / _logoCabecera.Width;
                    _logoCabecera.ScalePercent(percentage * 100);
                    #endregion
                    // Formato Texto
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _standardFontTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _standardFontVGM = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                    document.Open();
                    #region "Crear Tabla PDF"
                    PdfPTable tblCabecera = new PdfPTable(2);
                    tblCabecera.WidthPercentage = 100;

                    PdfPTable tblBarraCertificadoDePeso = new PdfPTable(1);
                    tblBarraCertificadoDePeso.WidthPercentage = 100;

                    PdfPTable tblTitulo = new PdfPTable(1);
                    tblTitulo.WidthPercentage = 100;

                    PdfPTable tblTitulo2 = new PdfPTable(1);
                    tblTitulo2.WidthPercentage = 100;

                    PdfPTable tblTitulo3 = new PdfPTable(1);
                    tblTitulo3.WidthPercentage = 100;

                    PdfPTable tblTitulo4 = new PdfPTable(1);
                    tblTitulo4.WidthPercentage = 100;

                    PdfPTable tblTitulo5 = new PdfPTable(1);
                    tblTitulo5.WidthPercentage = 100;

                    PdfPTable tblCertificadoFechaEmision = new PdfPTable(4);
                    tblCertificadoFechaEmision.WidthPercentage = 100;

                    PdfPTable tblNombreBuque = new PdfPTable(2);
                    tblNombreBuque.WidthPercentage = 100;

                    PdfPTable tblNombreExportador = new PdfPTable(2);
                    tblNombreExportador.WidthPercentage = 100;

                    PdfPTable tblInformacionBuque = new PdfPTable(1);
                    tblInformacionBuque.WidthPercentage = 100;

                    PdfPTable tblDatosInformacionBuque = new PdfPTable(2);
                    tblDatosInformacionBuque.WidthPercentage = 100;

                    PdfPTable tblDatosContenedor = new PdfPTable(2);
                    tblDatosContenedor.WidthPercentage = 100;

                    PdfPTable tblNombreTexto = new PdfPTable(1);
                    tblNombreTexto.WidthPercentage = 100;

                    // Configuramos el título de las columnas de la tabla
                    PdfPCell clNombreEncabezado = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clLogoEncabezado = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreBarraCertificadoDePeso = new PdfPCell(new Phrase("", _standardFontVGM));
                    PdfPCell clLogo = new PdfPCell(new Phrase("", _standardFontVGM));
                    PdfPCell clLogo1 = new PdfPCell(new Phrase("", _standardFontVGM));
                    PdfPCell clTitulo1 = new PdfPCell(new Phrase("", _standardFontTitle));
                    PdfPCell clTitulo2 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTitulo3 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTitulo4 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTitulo5 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTitulo6 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTitulo7 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell NumCertificado = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell ValorNumCertificado = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell FechaEmision = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell ValorFechaEmision = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTituloInformacionBuque = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clBuque = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreBuque = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clViaje = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreViaje = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTexto1 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTexto2 = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTexto3 = new PdfPCell(new Phrase("", _standardFont));

                    PdfPCell clRuc = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreRuc = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clRazon = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreRazon = new PdfPCell(new Phrase("", _standardFont));

                    PdfPCell clContenedor = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreContenedor = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clPeso = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombrePeso = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clTara = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreTara = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreNeto = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clVGM = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreVGM = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clPay = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombrePay = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clEquipo = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreEquipo = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clCertificado = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreCertificado = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clCertificadoF = new PdfPCell(new Phrase("", _standardFont));
                    PdfPCell clNombreCertificadoF = new PdfPCell(new Phrase("", _standardFont));

                    #endregion
                    #region "Agregar Datos"
                    // Llenamos la tabla con información
                    clLogoEncabezado = new PdfPCell(_logoCabecera);
                    clLogoEncabezado.BorderWidth = 0;
                    //clNombreEncabezado = new PdfPCell(new Phrase("VERIFICACIÓN DE MASA BRUTA VGM", _standardFont));
                    clNombreEncabezado = new PdfPCell(new Phrase(" ", _standardFont));
                    clNombreEncabezado.BorderWidth = 0;
                    clNombreEncabezado.HorizontalAlignment = Element.ALIGN_CENTER;

                    clLogo = new PdfPCell(new Phrase("", _standardFont));
                    clLogo.BorderWidth = 0;

                    clTitulo1 = new PdfPCell(new Phrase("CERTIFICADO DE PESO", _standardFontTitle));
                    clTitulo1.BorderWidth = 0;
                    clTitulo1.HorizontalAlignment = Element.ALIGN_CENTER;
                    clTitulo2 = new PdfPCell(new Phrase("VERIFIED GROSS MASS", _standardFont));
                    clTitulo2.BorderWidth = 0;
                    clTitulo2.HorizontalAlignment = Element.ALIGN_CENTER;
                    clTitulo3 = new PdfPCell(new Phrase(" ", _standardFont));
                    clTitulo3.BorderWidth = 0;
                    clTitulo3.HorizontalAlignment = Element.ALIGN_CENTER;

                    NumCertificado = new PdfPCell(new Phrase("Certificado No.:", _standardFont));
                    NumCertificado.BorderWidth = 0;
                    ValorNumCertificado = new PdfPCell(new Phrase(numeroCertificado, _standardFont));
                    ValorNumCertificado.BorderWidth = 0;
                    ValorNumCertificado.HorizontalAlignment = Element.ALIGN_LEFT;
                    FechaEmision = new PdfPCell(new Phrase("Fecha de Consulta:", _standardFont));
                    FechaEmision.BorderWidth = 0;
                    FechaEmision.HorizontalAlignment = Element.ALIGN_RIGHT;
                    ValorFechaEmision = new PdfPCell(new Phrase(fechaEmision, _standardFont));
                    ValorFechaEmision.BorderWidth = 0;
                    ValorFechaEmision.HorizontalAlignment = Element.ALIGN_RIGHT;

                    clTitulo4 = new PdfPCell(new Phrase("INFORMACIÓN BUQUE", _standardFont));
                    clTitulo4.BorderWidth = 1;
                    clTitulo4.HorizontalAlignment = Element.ALIGN_CENTER;

                    clBuque = new PdfPCell(new Phrase("Nombre del Buque:", _standardFont));
                    clBuque.BorderWidth = 0;
                    clNombreBuque = new PdfPCell(new Phrase(Nave, _standardFont));
                    clNombreBuque.BorderWidth = 0;
                    clNombreBuque.HorizontalAlignment = Element.ALIGN_LEFT;

                    clViaje = new PdfPCell(new Phrase("No. de Viaje:", _standardFont));
                    clViaje.BorderWidth = 0;
                    clNombreViaje = new PdfPCell(new Phrase(Viaje, _standardFont));
                    clNombreViaje.BorderWidth = 0;
                    clNombreViaje.HorizontalAlignment = Element.ALIGN_LEFT;

                    clTitulo5 = new PdfPCell(new Phrase("INFORMACIÓN EXPORTADOR", _standardFont));
                    clTitulo5.BorderWidth = 1;
                    clTitulo5.HorizontalAlignment = Element.ALIGN_CENTER;

                    clRuc = new PdfPCell(new Phrase("RUC:", _standardFont));
                    clRuc.BorderWidth = 0;
                    clNombreRuc = new PdfPCell(new Phrase(Ruc, _standardFont));
                    clNombreRuc.BorderWidth = 0;
                    clNombreRuc.HorizontalAlignment = Element.ALIGN_LEFT;

                    clRazon = new PdfPCell(new Phrase("Razón Social:", _standardFont));
                    clRazon.BorderWidth = 0;
                    clNombreRazon = new PdfPCell(new Phrase(RazonSocial, _standardFont));
                    clNombreRazon.BorderWidth = 0;
                    clNombreRazon.HorizontalAlignment = Element.ALIGN_LEFT;

                    clTitulo6 = new PdfPCell(new Phrase("INFORMACIÓN CONTENEDOR", _standardFont));
                    clTitulo6.BorderWidth = 1;
                    clTitulo6.HorizontalAlignment = Element.ALIGN_CENTER;

                    clContenedor = new PdfPCell(new Phrase("Contenedor No.:", _standardFont));
                    clContenedor.BorderWidth = 0;
                    clNombreContenedor = new PdfPCell(new Phrase(Contenedor, _standardFont));
                    clNombreContenedor.BorderWidth = 0;
                    clNombreContenedor.HorizontalAlignment = Element.ALIGN_LEFT;

                    clTara = new PdfPCell(new Phrase("Tara (Kg):", _standardFont));
                    clTara.BorderWidth = 0;
                    clNombreTara = new PdfPCell(new Phrase(Tara, _standardFont));
                    clNombreTara.BorderWidth = 0;
                    clNombreTara.HorizontalAlignment = Element.ALIGN_LEFT;

                    clPeso = new PdfPCell(new Phrase("Peso Neto (Kg):", _standardFont));
                    clPeso.BorderWidth = 0;
                    clNombrePeso = new PdfPCell(new Phrase(PesoNeto, _standardFont));
                    clNombrePeso.BorderWidth = 0;
                    clNombrePeso.HorizontalAlignment = Element.ALIGN_LEFT;

                    clVGM = new PdfPCell(new Phrase("Peso VGM (Kg):", _standardFont));
                    clVGM.BorderWidth = 0;
                    clNombreVGM = new PdfPCell(new Phrase(PesoVGM, _standardFont));
                    clNombreVGM.BorderWidth = 0;
                    clNombreVGM.HorizontalAlignment = Element.ALIGN_LEFT;

                    clPay = new PdfPCell(new Phrase("Max. Payload (Kg):", _standardFont));
                    clPay.BorderWidth = 0;
                    clNombrePay = new PdfPCell(new Phrase(PayLoad, _standardFont));
                    clNombrePay.BorderWidth = 0;
                    clNombrePay.HorizontalAlignment = Element.ALIGN_LEFT;

                    clEquipo = new PdfPCell(new Phrase("Equipo de Pesaje:", _standardFont));
                    clEquipo.BorderWidth = 0;
                    clNombreEquipo = new PdfPCell(new Phrase(Equipo, _standardFont));
                    clNombreEquipo.BorderWidth = 0;
                    clNombreEquipo.HorizontalAlignment = Element.ALIGN_LEFT;

                    clCertificado = new PdfPCell(new Phrase("No. Certificado Equipo:", _standardFont));
                    clCertificado.BorderWidth = 0;
                    clNombreCertificado = new PdfPCell(new Phrase(CertificadoEquipo, _standardFont));
                    clNombreCertificado.BorderWidth = 0;
                    clNombreCertificado.HorizontalAlignment = Element.ALIGN_LEFT;

                    clTitulo7 = new PdfPCell(new Phrase("MARCO LEGAL", _standardFont));
                    clTitulo7.BorderWidth = 1;
                    clTitulo7.HorizontalAlignment = Element.ALIGN_CENTER;

                    clTexto1 = new PdfPCell(new Phrase("RESOLUCIÓN MTOP-SPTM-2016-0088-R", _standardFont));
                    clTexto1.BorderWidth = 0;
                    clTexto2 = new PdfPCell(new Phrase("Importante:", _standardFont));
                    clTexto2.BorderWidth = 0;
                    clTexto3 = new PdfPCell(new Phrase("Este certificado tiene validez si el sello de seguridad no ha perdido su integridad", _standardFont));
                    clTexto3.BorderWidth = 0;
                    #endregion
                    #region "Agregar Celdas"
                    // Añadimos las celdas a la tabla
                    tblCabecera.AddCell(clLogoEncabezado);
                    tblCabecera.AddCell(clNombreEncabezado);
                    tblTitulo.AddCell(clTitulo1);
                    tblTitulo.AddCell(clTitulo2);
                    tblTitulo.AddCell(clTitulo3);
                    tblCertificadoFechaEmision.AddCell(NumCertificado);
                    tblCertificadoFechaEmision.AddCell(ValorNumCertificado);
                    tblCertificadoFechaEmision.AddCell(FechaEmision);
                    tblCertificadoFechaEmision.AddCell(ValorFechaEmision);
                    tblTitulo2.AddCell(clTitulo4);
                    tblNombreBuque.AddCell(clBuque);
                    tblNombreBuque.AddCell(clNombreBuque);
                    tblNombreBuque.AddCell(clViaje);
                    tblNombreBuque.AddCell(clNombreViaje);
                    tblTitulo3.AddCell(clTitulo5);
                    tblNombreExportador.AddCell(clRuc);
                    tblNombreExportador.AddCell(clNombreRuc);
                    tblNombreExportador.AddCell(clRazon);
                    tblNombreExportador.AddCell(clNombreRazon);
                    tblTitulo4.AddCell(clTitulo6);
                    tblDatosContenedor.AddCell(clContenedor);
                    tblDatosContenedor.AddCell(clNombreContenedor);
                    tblDatosContenedor.AddCell(clTara);
                    tblDatosContenedor.AddCell(clNombreTara);
                    tblDatosContenedor.AddCell(clPeso);
                    tblDatosContenedor.AddCell(clNombrePeso);
                    tblDatosContenedor.AddCell(clVGM);
                    tblDatosContenedor.AddCell(clNombreVGM);
                    tblDatosContenedor.AddCell(clPay);
                    tblDatosContenedor.AddCell(clNombrePay);
                    tblDatosContenedor.AddCell(clEquipo);
                    tblDatosContenedor.AddCell(clNombreEquipo);
                    tblDatosContenedor.AddCell(clCertificado);
                    tblDatosContenedor.AddCell(clNombreCertificado);
                    tblTitulo5.AddCell(clTitulo7);
                    tblNombreTexto.AddCell(clTexto1);
                    tblNombreTexto.AddCell(clTexto2);
                    tblNombreTexto.AddCell(clTexto3);

                    // Añadimos la tabla al documento PDF
                    document.Add(tblCabecera);

                    // Agregamos salto de linea
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(new Paragraph(Chunk.NEWLINE));

                    // Añadimos la tabla al documento PDF
                    document.Add(tblTitulo);
                    document.Add(tblCertificadoFechaEmision);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblTitulo2);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblNombreBuque);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblTitulo3);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblNombreExportador);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblTitulo4);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblDatosContenedor);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblTitulo5);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(tblNombreTexto);
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    // Agregamos salto de linea
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(new Paragraph(Chunk.NEWLINE));
                    document.Add(new Paragraph(Chunk.NEWLINE));


                    #endregion

                    document.Close();
                    result = ms.GetBuffer();
                    // result = PDFSignature.SignPDFStream(ms.GetBuffer(), rutaCert, claveCert, certID);
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }


            }

        }
    }
}