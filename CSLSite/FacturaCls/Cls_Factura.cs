using BillionEntidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CSLSite.FacturaCls
{

	public class Cls_Factura : Cls_Bil_Base
	{

        public string id { get; set; }
        public Int64 documento { get; set; }
        public DateTime fecha { get; set; }
        public string contenido { get; set; }
        public int tipo { get; set; }
        public string clave { get; set; }
        public string autorizacion { get; set; }
        public DateTime fechaAutoriza { get; set; }
        public string IVA { get; set; }
        public string IV_FACTURA { get; set; }
        public string IV_DESC_CLIENTE { get; set; }
        public decimal IV_TOTAL { get; set; }
        public string IV_NUMERO_CARGA { get; set; }
        public DateTime IV_FECHA { get; set; }


        private BillionEntidades.Factura.Factura innerFactura;
        public BillionEntidades.Factura.Factura InnerFactura { get { return innerFactura; } }

        public Cls_Factura(string docXml) { this.contenido = docXml; innerFactura = null; }

        public bool Serializar(out string novedad)
        {
            this.innerFactura = null;
            novedad = null;
            try
            {

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.contenido);
                using (TextReader textReader = new StringReader(doc.OuterXml))
                {
                    using (XmlTextReader reader = new XmlTextReader(textReader))
                    {
                        reader.Namespaces = false;
                        XmlSerializer serializer = new XmlSerializer(typeof(BillionEntidades.Factura.Factura));
                        this.innerFactura = (BillionEntidades.Factura.Factura)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception e)
            {
                novedad = e.Message;
                return false;
            }
            return true;
        }
        public byte[] GenerarPdf()
        {
            //FACTURA
            //7 TABLAS-->Formato
            byte[] result = null;
            using (var ms = new MemoryStream())
            {
                try
                {
                    int style = 0;
                    string familia = "Open Sans";
                    string subfamilia = "sans-serif";
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    writer.CloseStream = false;
                    PdfPTable table = new PdfPTable(3);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.PaddingTop = 2;
                    var mitad = document.PageSize.Width / 2;
                    mitad = mitad - 5;
                    table.SetWidths(new float[] { mitad, 10, mitad });
                    table.TotalWidth = document.PageSize.Width;
                    PdfPCell cell = null;
                    Paragraph parrafo = null;
                    document.Open();
                    Font fuente = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                    Font peque = FontFactory.GetFont(familia, subfamilia, true, 8, style, BaseColor.BLACK);
                    #region"CABECERA"

                    // fila 1;

                    //DESCOMENTAR AL SUBIR
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                    //iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance("~/shared/imgs/carbono_img/logoContecon.jpg");
                    imagen.ScaleToFit(170, 80);
                    cell = new PdfPCell(imagen);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = 0;
                    cell.Rowspan = 5;
                    table.AddCell(cell);



                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------



                    parrafo = new Paragraph("R.U.C.: 0992506717001", fuente);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);


                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------

                    // 3 texto
                    parrafo = new Paragraph("FACTURA", fuente);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------
                    //4 numero
                    parrafo = new Paragraph(string.Format("No. {0}", this.id), peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);


                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------
                    //5 texto
                    parrafo = new Paragraph("NÚMERO DE AUTORIZACIÓN", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);


                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------

                    //6 numero auto
                    parrafo = new Paragraph(this.autorizacion, peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);
                    //fin de Fila 1

                    //7 texto --- fila 2
                    parrafo = new Paragraph("CONTECON GUAYAQUIL SA", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------

                    //feha aut
                    parrafo = new Paragraph(string.Format("FECHA Y HORA DE AUTORIZACIÓN: {0}", this.fechaAutoriza.ToString("dd-MM-yyyyy HH:mm")), peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //fin fila 2


                    parrafo = new Paragraph("Dir Matriz: AV. DE LA MARINA S/N VIA AL PUERTO MARITIMO", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------

                    //texto
                    parrafo = new Paragraph("AMBIENTE: PRODUCCION", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //fila 3

                    parrafo = new Paragraph("Dir Sucursal: VIA PUERTO MARITIMO AV. DE LA MARINA S/N", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------

                    //texto
                    parrafo = new Paragraph("EMSION: NORMAL", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //fila 4

                    parrafo = new Paragraph("Contribuyente Especial Nro 870", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------


                    parrafo = new Paragraph("CLAVE ACCESO:", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);




                    parrafo = new Paragraph("OBLIGADO A LLEVAR CONTABILIDAD: SI", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);


                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------
                    #region "PDF BARCODE"
                    Barcode128 _bc = new Barcode128();
                    _bc.CodeType = Barcode128.CODE128;
                    _bc.ChecksumText = true;
                    _bc.GenerateChecksum = true;
                    _bc.Code = clave;
                    var bImg = _bc.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);
                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(bImg, System.Drawing.Imaging.ImageFormat.Jpeg);
                    pic.ScaleToFit(mitad - 50, 45);
                    cell = new PdfPCell(pic);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);
                    #endregion

                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);

                    //Celda centro----------------------
                    parrafo = new Paragraph("   ", peque);
                    cell = new PdfPCell(parrafo);
                    cell.Border = 0;
                    table.AddCell(cell);
                    //----------------------------------
                    parrafo = new Paragraph(clave, peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);





                    document.Add(table);
                    #endregion
                    parrafo = new Paragraph("   ", peque);
                    document.Add(parrafo);
                    #region "Tabls 2"

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.PaddingTop = 2;
                    mitad = document.PageSize.Width / 2;
                    mitad = mitad - 1;

                    table.SetWidths(new float[] { mitad, mitad });
                    table.TotalWidth = document.PageSize.Width;


                    parrafo = new Paragraph(string.Format("Razón Social / Nombres y Apellidos: {0}", this.InnerFactura.InfoFactura.RazonSocialComprador), peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);


                    parrafo = new Paragraph(string.Format("Identificación: {0}", this.InnerFactura.InfoFactura.IdentificacionComprador), peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);



                    parrafo = new Paragraph(string.Format("Fecha Emisión: {0}", this.InnerFactura.InfoFactura.FechaEmision), peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);



                    parrafo = new Paragraph("Guía Remisión:", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);
                    document.Add(table);
                    #endregion
                    parrafo = new Paragraph("   ", peque);
                    document.Add(parrafo);
                    #region "Tabla detalle"
                    table = new PdfPTable(10);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.PaddingTop = 2;
                    table.SetWidths(new float[] { 18, 18, 22, 80, 22, 22, 22, 22, 22, 22 });
                    table.TotalWidth = document.PageSize.Width;




                    parrafo = new Paragraph("Cod. Principal", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);

                    parrafo = new Paragraph("Cod. Auxiliar", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);

                    parrafo = new Paragraph("Cant.", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);



                    parrafo = new Paragraph("Descripción", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);


                    parrafo = new Paragraph("D.Adicional", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);

                    parrafo = new Paragraph("D.Adicional", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);

                    parrafo = new Paragraph("D.Adicional", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);

                    parrafo = new Paragraph("Precio Unitario", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);

                    parrafo = new Paragraph("Descuento", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                    table.AddCell(cell);

                    parrafo = new Paragraph("Precio Total", peque);
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.BorderColor = Color.BLACK;
                    cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                    table.AddCell(cell);

                    //Bucle de items:
                    int it = 0;
                    if (this.InnerFactura.Detalles.Detalle != null && this.InnerFactura.Detalles.Detalle.Count > 0)
                    {
                        foreach (var fi in this.InnerFactura.Detalles.Detalle)
                        {

                            it++;
                            parrafo = new Paragraph(fi.CodigoPrincipal, peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);

                            parrafo = new Paragraph("", peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);


                            parrafo = new Paragraph(fi.Cantidad, peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);


                            parrafo = new Paragraph(fi.Descripcion, peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);


                            parrafo = new Paragraph("", peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;

                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);

                            parrafo = new Paragraph("", peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);


                            parrafo = new Paragraph("", peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);

                            parrafo = new Paragraph(fi.PrecioUnitario, peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);


                            parrafo = new Paragraph(fi.Descuento, peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
                            }
                            table.AddCell(cell);

                            parrafo = new Paragraph(fi.PrecioTotalSinImpuesto, peque);
                            cell = new PdfPCell(parrafo);
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = Element.ALIGN_TOP;
                            cell.BorderColor = Color.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                            if (it == this.innerFactura.Detalles.Detalle.Count)
                            {
                                cell.Border = Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                            }
                            table.AddCell(cell);



                        }
                    }




                    document.Add(table);
                    #endregion
                    parrafo = new Paragraph("   ", peque);
                    document.Add(parrafo);
                    //---------------------------------------------------//
                    var adicional = new PdfPTable(3);
                    adicional.WidthPercentage = 100;
                    adicional.PaddingTop = 2;
                    mitad = document.PageSize.Width / 2;
                    mitad = mitad - 5;
                    adicional.SetWidths(new float[] { 120, 30, 55 });
                    adicional.TotalWidth = document.PageSize.Width;
                    //--------------------------------------------------------//
                    #region "Celda IZQ"

                    if (this.innerFactura?.InfoAdicional?.CampoAdicional != null && this.innerFactura.InfoAdicional.CampoAdicional.Count > 0)
                    {

                        //ADITIONAL
                        var Ltable = new PdfPTable(2);
                        Ltable.SetWidths(new float[] { 18, 50 });
                        parrafo = new Paragraph("Información Adicional", peque);
                        var cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Colspan = 2;
                        cellInterna.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        Ltable.AddCell(cellInterna);


                        //direccionCliente
                        parrafo = new Paragraph("Dirección", peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.LEFT_BORDER;

                        Ltable.AddCell(cellInterna);
                        var pun = this.innerFactura.InfoAdicional.CampoAdicional.Where(f => f.Nombre.Equals("direccionCliente")).FirstOrDefault();
                        parrafo = new Paragraph(pun != null ? pun.Text : string.Empty, peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.RIGHT_BORDER;
                        Ltable.AddCell(cellInterna);

                        //Email
                        parrafo = new Paragraph("Email", peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.LEFT_BORDER;
                        Ltable.AddCell(cellInterna);

                        pun = this.innerFactura.InfoAdicional.CampoAdicional.Where(f => f.Nombre.Equals("Email")).FirstOrDefault();
                        parrafo = new Paragraph(pun != null ? pun.Text : string.Empty, peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.RIGHT_BORDER;
                        Ltable.AddCell(cellInterna);

                        //Buque
                        parrafo = new Paragraph("Buque", peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.LEFT_BORDER;
                        Ltable.AddCell(cellInterna);

                        pun = this.innerFactura.InfoAdicional.CampoAdicional.Where(f => f.Nombre.Equals("Buque")).FirstOrDefault();
                        parrafo = new Paragraph(pun != null ? pun.Text : string.Empty, peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.RIGHT_BORDER;
                        Ltable.AddCell(cellInterna);

                        //Viaje
                        parrafo = new Paragraph("Viaje", peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.LEFT_BORDER;
                        Ltable.AddCell(cellInterna);

                        pun = this.innerFactura.InfoAdicional.CampoAdicional.Where(f => f.Nombre.Equals("Viaje")).FirstOrDefault();
                        parrafo = new Paragraph(pun != null ? pun.Text : string.Empty, peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.RIGHT_BORDER;
                        Ltable.AddCell(cellInterna);


                        //Viaje
                        parrafo = new Paragraph("Fecha Arribo", peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.LEFT_BORDER;
                        Ltable.AddCell(cellInterna);

                        pun = this.innerFactura.InfoAdicional.CampoAdicional.Where(f => f.Nombre.Equals("arribo")).FirstOrDefault();
                        parrafo = new Paragraph(pun != null ? pun.Text : string.Empty, peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.RIGHT_BORDER;
                        Ltable.AddCell(cellInterna);


                        //cntrBl
                        parrafo = new Paragraph("Cntr/Bl", peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.LEFT_BORDER;
                        Ltable.AddCell(cellInterna);

                        pun = this.innerFactura.InfoAdicional.CampoAdicional.Where(f => f.Nombre.Equals("cntrBl")).FirstOrDefault();
                        parrafo = new Paragraph(pun != null ? pun.Text : string.Empty, peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.RIGHT_BORDER;
                        Ltable.AddCell(cellInterna);


                        //comentario
                        parrafo = new Paragraph("Comentario", peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
                        Ltable.AddCell(cellInterna);

                        pun = this.innerFactura.InfoAdicional.CampoAdicional.Where(f => f.Nombre.Equals("comentario")).FirstOrDefault();
                        parrafo = new Paragraph(pun != null ? pun.Text : string.Empty, peque);
                        cellInterna = new PdfPCell(parrafo);
                        cellInterna.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellInterna.VerticalAlignment = Element.ALIGN_TOP;
                        cellInterna.Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                        Ltable.AddCell(cellInterna);


                        var ycell = new PdfPCell(Ltable);
                        ycell.Border = 0;
                        adicional.AddCell(ycell);
                    }

                    #endregion
                    parrafo = new Paragraph("   ", peque);
                    var zcell = new PdfPCell(parrafo);
                    zcell.Border = 0;
                    adicional.AddCell(zcell);
                    #region"Celda DER"

                    var Rtable = new PdfPTable(2);
                    Rtable.SetWidths(new float[] { 45, 18 });

                    //SUBTOTAL 12
                    parrafo = new Paragraph(string.Format("SUBTOTAL {0}%", this.IVA), peque);
                    var excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph(this.innerFactura.InfoFactura.TotalSinImpuestos, peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);
                    //----------------------------------------


                    //SUBTOTAL CERO--------------------------
                    parrafo = new Paragraph(string.Format("SUBTOTAL {0}%", 0), peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph("0.00", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);
                    //SUBTOTAL CERO


                    //SUBTOTAL NO OBJETO
                    parrafo = new Paragraph("SUBTOTAL No objeto de IVA", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph("0.00", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);
                    //------------------------------------------------------


                    //SUB TOTAL Exento------------------------------------
                    parrafo = new Paragraph("SUBTOTAL Exento de IVA", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph("0.00", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    //-----------------------------------------------------------



                    //SUBTOTAL AIN IMPUES-----------------------------------
                    parrafo = new Paragraph("SUBTOTAL SIN IMPUESTOS", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph(this.innerFactura.InfoFactura.TotalSinImpuestos, peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);
                    //------------------------------------------------------------


                    //TOTAL DESCUENTO---------------------------------------
                    parrafo = new Paragraph("TOTAL Descuento", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph(this.innerFactura.InfoFactura.TotalDescuento, peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    //-------------------------------------------------------------------------


                    //--------------ice---------------------------------

                    parrafo = new Paragraph("ICE", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph("0.00", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);
                    //--------------------------------------------------------------


                    //iva 12-----------------------------------------
                    parrafo = new Paragraph(string.Format("IVA {0}%", this.IVA), peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph(this.InnerFactura.InfoFactura.TotalConImpuestos.TotalImpuesto.Valor, peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    //------------------------------------------------------



                    //----------------IRBPNR-----------------------------------
                    parrafo = new Paragraph("IRBPNR", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph("0.00", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    //------------------------------------------------------

                    //PROPINA---------------------
                    parrafo = new Paragraph("PROPINA", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);



                    parrafo = new Paragraph(this.InnerFactura.InfoFactura.Propina, peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    //-----------------------------------------------------


                    //VALOR TOTAL-----------------------------------------------
                    parrafo = new Paragraph("VALOR TOTAL", peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_LEFT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);

                    parrafo = new Paragraph(this.InnerFactura.InfoFactura.ImporteTotal, peque);
                    excel = new PdfPCell(parrafo);
                    excel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    excel.VerticalAlignment = Element.ALIGN_TOP;
                    Rtable.AddCell(excel);
                    //-----------------------------------------------------------------------





                    //add rtable
                    var xell = new PdfPCell(Rtable);
                    xell.Border = 0;
                    xell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    adicional.AddCell(xell);

                    #endregion
                    document.Add(adicional);
                    //SALTO
                    parrafo = new Paragraph("   ", peque);
                    document.Add(parrafo);
                    #region "Tabla PIE"
                    //---------------------------------------------------//
                    var pie = new PdfPTable(1);
                    pie.PaddingTop = 2;
                    pie.TotalWidth = 100;
                    //--------------------------------------------------------//


                    var ptable = new PdfPTable(4);
                    ptable.SetWidths(new float[] { 60, 18, 18, 18 });


                    parrafo = new Paragraph("Forma de Pago", peque);
                    var pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_LEFT;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                    ptable.AddCell(pexcel);


                    parrafo = new Paragraph("Total", peque);
                    pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_CENTER;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                    ptable.AddCell(pexcel);


                    parrafo = new Paragraph("Plazo", peque);
                    pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_CENTER;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
                    ptable.AddCell(pexcel);


                    parrafo = new Paragraph("Tiempo", peque);
                    pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_CENTER;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                    ptable.AddCell(pexcel);



                    //valores
                    string valor = string.Empty;
                    //forma de pago
                    if (this.InnerFactura?.InfoFactura?.Pagos?.Pago?.FormaPago != null)
                    {
                        valor = this.InnerFactura.InfoFactura.Pagos.Pago.FormaPago;
                    }

                    if (valor.Equals("20"))
                    {
                        valor = "OTROS CON UTILIZACION DEL SISTEMA FINANCIERO";
                    }
                    else
                    {
                        valor = "Otros";
                    }


                    parrafo = new Paragraph(valor, peque);
                    pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_CENTER;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    ptable.AddCell(pexcel);



                    valor = string.Empty;
                    //forma de pago
                    if (this.InnerFactura?.InfoFactura?.Pagos?.Pago?.Total != null)
                    {
                        valor = this.InnerFactura.InfoFactura.Pagos.Pago.Total;
                    }

                    parrafo = new Paragraph(valor, peque);
                    pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_CENTER;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    ptable.AddCell(pexcel);




                    valor = string.Empty;
                    //forma de pago
                    if (this.InnerFactura?.InfoFactura?.Pagos?.Pago?.Plazo != null)
                    {
                        valor = this.InnerFactura.InfoFactura.Pagos.Pago.Plazo;
                    }

                    parrafo = new Paragraph(valor, peque);
                    pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_CENTER;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                    ptable.AddCell(pexcel);




                    valor = string.Empty;
                    //forma de pago
                    if (this.InnerFactura?.InfoFactura?.Pagos?.Pago?.UnidadTiempo != null)
                    {
                        valor = this.InnerFactura.InfoFactura.Pagos.Pago.UnidadTiempo;
                    }

                    parrafo = new Paragraph(valor, peque);
                    pexcel = new PdfPCell(parrafo);
                    pexcel.HorizontalAlignment = Element.ALIGN_CENTER;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pexcel.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;

                    ptable.AddCell(pexcel);

                    #endregion
                    pexcel = new PdfPCell(ptable);
                    pexcel.Border = 0;
                    pexcel.HorizontalAlignment = Element.ALIGN_LEFT;
                    pexcel.VerticalAlignment = Element.ALIGN_TOP;
                    pie.AddCell(pexcel);
                    document.Add(pie);
                    document.Close();
                    result = ms.GetBuffer();
                    return result;

                }
                catch (Exception ex)
                {

                    result = null;
                }

            }
            return result;
        }
        public Cls_Factura() : base()
		{
			init();
		}

		private static void OnInit(string Base)
		{
			sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
			parametros = new Dictionary<string, object>();
			nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
		}

        public static List<Cls_Factura> ObteneIVXGkey(string id)
        {
            try
            {
                parametros.Clear();

                if (!int.TryParse(id, out int gkey))
                {
                    throw new ArgumentException("El parámetro Gkey debe ser un número entero válido.");
                }

                parametros.Add("Gkey", gkey); 

                string controlError;

                var listaIv = sql_puntero.ExecuteSelectControl<Cls_Factura>(
                    sql_puntero.Conexion_Local,
                    4000,
                    "sp_Consultar_Factura_Por_Gkey",
                    parametros,
                    out controlError
                );

                return listaIv;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Ocurrió un error al consultar las facturas en la base de datos.", sqlEx);
            }
            catch (TimeoutException timeoutEx)
            {
                throw new Exception("El tiempo de espera para la consulta de facturas ha expirado.", timeoutEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al consultar las facturas.", ex);
            }
        }


        public static List<Cls_Factura> ObtenerXNumFact(string id)
		{
			try
			{

				OnInit("APPCGSA");
				parametros.Clear();
				parametros.Add("id", id);


				string controlError;

				var listaFacturas = sql_puntero.ExecuteSelectControl<Cls_Factura>(
					nueva_conexion,
					4000,
					"pdf.ObtenerRegistroFactura",
					parametros,
					out controlError
				);
				return listaFacturas;

			}


			catch (SqlException sqlEx)
			{
				throw new Exception("Ocurrió un error al consultar los sellos en la base de datos.", sqlEx);
			}
			catch (TimeoutException timeoutEx)
			{
				throw new Exception("El tiempo de espera para la consulta de sellos ha expirado.", timeoutEx);
			}
			catch (Exception ex)
			{
				throw new Exception("Ocurrió un error inesperado al consultar los sellos.", ex);
			}

		}

        public static DataTable ObtenerSoporteReefer(string booking)
        {
            try
            {
                OnInit("N5");
                parametros.Clear();


                parametros.Add("booking", booking);

                string controlError;

                var soporteReefer = sql_puntero.ComadoSelectADatatable(
                     nueva_conexion,
                    4000,
                    "[Bill].[soporte_horas_reefer_referencia]",
                    parametros,
                    out controlError
                );

                return soporteReefer;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Ocurrió un error al consultar las facturas en la base de datos.", sqlEx);
            }
            catch (TimeoutException timeoutEx)
            {
                throw new Exception("El tiempo de espera para la consulta de facturas ha expirado.", timeoutEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al consultar las facturas.", ex);
            }

        }
    }
}