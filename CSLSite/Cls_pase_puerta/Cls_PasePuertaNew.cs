using AccesoDatos;
using BillionEntidades;
using CSLSite.Cls_pase_puerta;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CSLSite.Cls_pase_puerta
{
    public class Cls_PasePuertaNew : Cls_Bil_Base
    {
        public Int64 IdPase { get; set; }

        public string cf_server_con { get;  set; }
        //qr serach

        public string horarioLlegada { get; set; }
        public string horarioDesde { get; set; }
        public string horarioHasta { get; set; }
        public string contenedor { get; set; }
        public string sello { get; set; }
        public string selloGeo { get; set; }
        public string tipoIso { get; set; }
        public string Importador { get; set; }
        public string Ruc { get; set; }
        public string empresaTransporte { get; set; }
        public string Placa { get; set; }
        public string Licencia { get; set; }
        public string conductor { get; set; }
        public string mrn { get; set; }
        public string documento { get; set; }
        public string serial { get; set; }
        public string entrega { get; set; }
        public string expira { get; set; }
        public string ubicacion { get; set; }
        public string cantidad { get; set; }
        public string descripcion { get; set; }

        public Cls_PasePuertaNew() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static Int64 ObtenerIdPasePorGkey(string id)
        {
            try
            {
                OnInit("APPCGSA");
                parametros.Clear();

                if (!Int64.TryParse(id, out Int64 gkey))
                    throw new ArgumentException("El parámetro Gkey debe ser un número entero válido.");
         //       gkey = 4278284;
                
                parametros.Add("Gkey", Convert.ToDecimal(gkey));

                var idPase = sql_puntero.ExecuteSelectOnlyValue<Int64?>(
                    nueva_conexion,
                    4000,
                    "[pdf].[ObtenerIdPaseXGkey]",
                    parametros
                );
                
                return Convert.ToInt64(idPase);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ID_PASE desde la base de datos.", ex);
            }
        }

      
        public byte[] GenerarPdf()
        {
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
                    writer.CloseStream = false;


                    PdfPTable table = null;
                    PdfPCell cell = null;

                    document.Open();

                    Font fuente = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);
                    Font negrita = FontFactory.GetFont(familia, subfamilia, true, 10, estilo2, BaseColor.BLACK);
                    //TITULO
                    Paragraph parrafo = new Paragraph(string.Format("E-PASS:{0}", IdPase), fuente);
                    parrafo.Alignment = Element.ALIGN_CENTER;
                    document.Add(parrafo);
                    fuente = FontFactory.GetFont(familia, subfamilia, true, 5, style, BaseColor.BLACK);
                    parrafo = new Paragraph("            ", fuente);


                    document.Add(parrafo);
                    fuente = FontFactory.GetFont(familia, subfamilia, true, 10, style, BaseColor.BLACK);

                    table = new PdfPTable(3);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 60f, 120f, 30f });
                    table.TotalWidth = document.PageSize.Width;


                    //LOGO CGSA
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg"));
                    imagen.ScaleToFit(170, 80);
                    cell = new PdfPCell(imagen);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    //CODIGO BARRA SUPERIOR
                    var stream = HttpTool.BarStream(IdPase, cf_server_con);
                  if (stream != null)
                    {
                        imagen = Image.GetInstance(stream);
                        imagen.ScaleToFit(280, 80);
                        cell = new PdfPCell(imagen);
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    //CODIGO QR SUPERIOR
                    stream = HttpTool.BarcodeStream(IdPase, string.Empty, cf_server_con); ;
                    if (stream != null)
                    {
                        imagen = Image.GetInstance(stream);
                        imagen.ScaleToFit(80, 90);
                        cell = new PdfPCell(imagen);
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    //ADD CABECERA
                    document.Add(table);

                    parrafo = new Paragraph("INFORMACIÓN DE E-PASS", negrita);
                    parrafo.Alignment = Element.ALIGN_CENTER;
                    document.Add(parrafo);
                    Chunk linebreak = new Chunk(new LineSeparator(2f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 7));
                    document.Add(linebreak);

                    parrafo = new Paragraph(string.Format("SU FECHA DE LLEGADA ES: {0}", this.horarioLlegada), fuente);
                    parrafo.Alignment = Element.ALIGN_CENTER;
                    document.Add(parrafo);

                    parrafo = new Paragraph(string.Format("USTED PUEDE INGRESA A LA TERMINAL DESDE LAS: {0}", this.horarioDesde), fuente);
                    parrafo.Alignment = Element.ALIGN_CENTER;
                    document.Add(parrafo);

                    parrafo = new Paragraph(string.Format("SU HORA MÁXIMA DE LLEGADA A LA TERMINAL ES: {0}", this.horarioHasta), fuente);
                    parrafo.Alignment = Element.ALIGN_CENTER;
                    document.Add(parrafo);


                    parrafo = new Paragraph("INFORMACIÓN SOBRE LA CARGA", negrita);
                    parrafo.Alignment = Element.ALIGN_CENTER;
                    document.Add(parrafo);
                    linebreak = new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 7));
                    document.Add(linebreak);

                    //Tabla detalles de carga (items)

                    table = new PdfPTable(2);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 60f, 120f });
                    table.TotalWidth = document.PageSize.Width;

                    #region   "Entrega"
                    if (!string.IsNullOrEmpty(entrega))
                    {
                        parrafo = new Paragraph("ENTREGA:", negrita);
                        parrafo.Alignment = Element.ALIGN_RIGHT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);

                        parrafo = new Paragraph(entrega, fuente);
                        parrafo.Alignment = Element.ALIGN_LEFT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region   "Expira"
                    if (!string.IsNullOrEmpty(expira))
                    {
                        parrafo = new Paragraph("EXPIRA:", negrita);
                        parrafo.Alignment = Element.ALIGN_RIGHT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);

                        parrafo = new Paragraph(expira, fuente);
                        parrafo.Alignment = Element.ALIGN_LEFT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region "Contenedor"
                    parrafo = new Paragraph("CONTENEDOR:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(contenedor, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    #endregion
                    #region   "Sello"
                    if (!string.IsNullOrEmpty(sello))
                    {
                        parrafo = new Paragraph("SELLO:", negrita);
                        parrafo.Alignment = Element.ALIGN_RIGHT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);

                        parrafo = new Paragraph(sello, fuente);
                        parrafo.Alignment = Element.ALIGN_LEFT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region "Sello GEO"
                    parrafo = new Paragraph("SELLO GEO:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;

                    table.AddCell(cell);

                    parrafo = new Paragraph(selloGeo, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;

                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region   "Ubicacion"
                    if (!string.IsNullOrEmpty(ubicacion))
                    {
                        parrafo = new Paragraph("UBICACION:", negrita);
                        parrafo.Alignment = Element.ALIGN_RIGHT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);

                        parrafo = new Paragraph(ubicacion, fuente);
                        parrafo.Alignment = Element.ALIGN_LEFT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region   "Descripcion Carga"
                    if (!string.IsNullOrEmpty(ubicacion))
                    {
                        parrafo = new Paragraph("DESCRIPCIÓN:", negrita);
                        parrafo.Alignment = Element.ALIGN_RIGHT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);

                        parrafo = new Paragraph(HttpTool.normalizeWS(descripcion), fuente);
                        parrafo.Alignment = Element.ALIGN_LEFT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region   "CANTIDAD"
                    if (!string.IsNullOrEmpty(cantidad))
                    {
                        parrafo = new Paragraph("CANTIDAD:", negrita);
                        parrafo.Alignment = Element.ALIGN_RIGHT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);

                        parrafo = new Paragraph(cantidad, fuente);
                        parrafo.Alignment = Element.ALIGN_LEFT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region "TIPO/ISO"
                    if (!string.IsNullOrEmpty(tipoIso))
                    {
                        parrafo = new Paragraph("TIPO/ISO:", negrita);
                        parrafo.Alignment = Element.ALIGN_RIGHT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);

                        parrafo = new Paragraph(tipoIso, fuente);
                        parrafo.Alignment = Element.ALIGN_LEFT;
                        cell = new PdfPCell(parrafo);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_TOP;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }
                    #endregion
                    #region "IMPORTADOR"
                    parrafo = new Paragraph("IMPORTADOR:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(HttpTool.normalizeWS(Importador), fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "RUC"
                    parrafo = new Paragraph("RUC (EMP):", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(Ruc, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "EMP. TRANS"
                    parrafo = new Paragraph("EMP. TRANS:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(HttpTool.normalizeWS(empresaTransporte), fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "PLACA:"
                    parrafo = new Paragraph("PLACA:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(Placa, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "LICENCIA:"
                    parrafo = new Paragraph("LICENCIA:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(Licencia, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "CONDUCTOR:"
                    parrafo = new Paragraph("CONDUCTOR:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(conductor, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "MRN:"
                    parrafo = new Paragraph("MRN:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(mrn, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "DOCUMENTO:"
                    parrafo = new Paragraph("DOCUMENTO:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(documento, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "SERIAL:"
                    parrafo = new Paragraph("SERIAL:", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    parrafo = new Paragraph(serial, fuente);
                    parrafo.Alignment = Element.ALIGN_LEFT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);
                    #endregion
                    #region "SERIAL SN BAR"
                    parrafo = new Paragraph("    ", negrita);
                    parrafo.Alignment = Element.ALIGN_RIGHT;
                    cell = new PdfPCell(parrafo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = 0;
                    table.AddCell(cell);

                    stream = HttpTool.BarStream(serial, cf_server_con);
                    if (stream != null)
                    {
                        imagen = Image.GetInstance(stream);
                        imagen.ScaleToFit(280, 60);
                        cell = new PdfPCell(imagen);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Border = 0;
                        table.AddCell(cell);
                    }

                    #endregion
                    document.Add(table);
                    parrafo = new Paragraph("INFORMACIÓN DE SEGURIDAD", negrita);
                    parrafo.Alignment = Element.ALIGN_CENTER;
                    document.Add(parrafo);
                    linebreak = new Chunk(new LineSeparator(2f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 7));
                    document.Add(linebreak);


                    string par = "Estimado Cliente/Empresa de Transporte, a continuación, indicaciones de seguridad que deben ser cumplidas sin excepción al ingresar a nuestras instalaciones portuarias: ";
                    fuente = FontFactory.GetFont(familia, subfamilia, true, 7, style, BaseColor.BLACK);
                    parrafo = new Paragraph(par, fuente);
                    parrafo.Alignment = Element.ALIGN_JUSTIFIED;

                    //PARRAFOS FINALES
                    iTextSharp.text.List list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED, 10f);
                    list.SetListSymbol("\u2022");
                    list.IndentationLeft = 20f;
                    list.Add(new ListItem("El vehículo (camión) y su respectiva plataforma deben estar en buenas condiciones, caso contrario no podrá ingresar", fuente));
                    list.Add(new ListItem("El conductor debe:", fuente));
                    list.Add(new ListItem("No estar bajo la influencia de alcohol y drogas", fuente));
                    list.Add(new ListItem("Portar el Equipo de Protección Personal (EPP) básico para ingresar a la IP: Botas de Seguridad, Casco y Chaleco con reflectivo", fuente));
                    list.Add(new ListItem("No exceder los siguientes límites de velocidad: Calles Principales 30 km/h, Patios 20 km/h y Muelles 10 km/h", fuente));
                    list.Add(new ListItem("Retirar los seguros de los Twist Lock (Piñas) o Pines para ser descargado por las grúas, siguiendo las normas de seguridad internas del puerto", fuente));
                    list.Add(new ListItem("Permanecer en la cabina del vehículo mientras las grúas están en movimiento y realizando maniobras de carga y descarga de los contenedores.", fuente));
                    list.Add(new ListItem("Está prohibido:", fuente));
                    list.Add(new ListItem("Realizar giros en 'U' dentro de los patios y en calles con espacios reducidos", fuente));
                    list.Add(new ListItem("Realizar trabajos de mantenimiento a su vehículo y de presentarse alguna novedad debe coordinar con las áreas de: Seguridad Industrial, Seguridad Física y Mantenimiento.", fuente));
                    parrafo.Add(list);
                    document.Add(parrafo);
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    //MARCA DE AGUA
                    // PdfHelper.AddWaterMark(writer.DirectContent, "CGSAPP", bfTimes, 100, 35, new BaseColor(70, 70, 255), document.PageSize );
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
    }
}