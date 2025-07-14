using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using ControlPagos.Importacion;
using Salesforces;
using System.Data;
using System.Net;
using SqlConexion;
using CasManual;

using System.Reflection;
using System.ComponentModel;
using VBSEntidades.Plantilla;
using VBSEntidades;
using VBSEntidades.ClaseEntidades;
using System.Web.Services;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CSLSite
{


    public partial class VBS_Exportar_Reporte_Import : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;

        private string cMensajes;


        #endregion

     

        private string LoginName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);

                return;
            }

            this.IsAllowAccess();

            if (!Page.IsPostBack)
            {
                ClsUsuario = Page.Tracker();

            }

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!Page.IsPostBack)
                {
                  

                }
                else
                {


                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocrurrio un error en la pagina", ex);
            }
        }

      

        public static byte[] CreateExcelBytesFromStoredProcedure(string fechaDesde, string fechaHasta)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            string fechaLocal = fechaDesde;
            DateTime fechaConvertida = DateTime.ParseExact(fechaLocal, "d/M/yyyy", enUS);

            string fechaLocalHasta = fechaHasta;
            DateTime fechaConvertidaHasta = DateTime.ParseExact(fechaLocalHasta, "d/M/yyyy", enUS);

           
            // Conexión a la base de datos
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VBS"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

              
                Color colorPlaneado = ColorTranslator.FromHtml("#59B653");
                Color colorReservado = ColorTranslator.FromHtml("#5AABD9");
                Color colorDisponible = ColorTranslator.FromHtml("#D9DB63");


                // Crear el comando para ejecutar el SP
                using (SqlCommand command = new SqlCommand("VBS_DETALLE_REPORTE_IMPORT_v2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros al comando
                    command.Parameters.AddWithValue("@FechaDesde", fechaConvertida);
                    command.Parameters.AddWithValue("@FechaHasta", fechaConvertidaHasta);

                    // Crear el DataTable para almacenar los datos del SP
                    DataTable dataTable = new DataTable();

                    // Leer los datos del SP y llenar el DataTable
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                    //grupo a recorrer
                    var grupos = from p in dataTable.AsEnumerable()
                                 group p by p.Field<string>("TIPO") into g
                                 select new
                                 {
                                     TIPO = g.Key
                                 };

                    // Crear el archivo Excel con EPPlus
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");


                        worksheet.Cells["B3:B3"].Value = "Fecha Desde:";
                        worksheet.Cells["B3:B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["B3:B3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["B3:B3"].Style.Fill.BackgroundColor.SetColor(colorPlaneado);
                        worksheet.Cells["B3:B3"].Style.Font.Color.SetColor(System.Drawing.Color.White);


                        worksheet.Cells["C3:C3"].Value = fechaDesde;
                        worksheet.Cells["C3:C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                        worksheet.Cells["B4:B4"].Value = "Fecha Hasta:";
                        worksheet.Cells["B4:B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["B4:B4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["B4:B4"].Style.Fill.BackgroundColor.SetColor(colorReservado);
                        worksheet.Cells["B4:B4"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                        worksheet.Cells["C4:C4"].Value = fechaHasta;
                        worksheet.Cells["C4:C4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                        //Asignar los valores de la cabecera
                        int c = 1;
                        int espa = 2;
                        int r = 6;
                        int inicio = 0;
                        foreach (var cab in grupos)
                        {
                            inicio = c;

                            foreach (DataColumn column in dataTable.Columns)  //printing column headings
                            {

                                worksheet.Cells[r, c].Value = column.ColumnName;
                                worksheet.Cells[r, c].Style.Font.Bold = true;
                                worksheet.Cells[r, c].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells[r, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[r, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[r, c].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                                c = c + 1;
                            }

                            c = c + espa;
                            inicio = c;
                        }

                        r = 7;
                        c = 1;
                        string anterior = string.Empty;
                        //detalle de informacion
                        foreach (var cab in grupos)
                        {

                            inicio = c;
                            //filtramos en base al cada tipo
                            var query = from p in dataTable.AsEnumerable().Where(y => y.Field<string>("TIPO") == cab.TIPO)
                                        select p;

                            //se convierte para recorrer
                            DataTable resultado = query.CopyToDataTable<DataRow>();

                            if (resultado.Rows.Count > 0)
                            {
                                int fila = 0;
                                int eachRow = 0;
                                int col = 0;
                                int new_col = c;
                                for (eachRow = 0; eachRow < resultado.Rows.Count;)
                                {
                                    col = 1;
                                    new_col = c;
                                    foreach (DataColumn column in resultado.Columns)
                                    {

                                        worksheet.Cells[r, new_col].Value = resultado.Rows[fila][col - 1];
                                        col++;
                                        new_col++;
                                    }

                                    eachRow++;
                                    r++;
                                    fila++;

                                }

                            }

                            c = c + dataTable.Columns.Count;


                            r = 7;
                            c = c + espa;
                            inicio = c;
                        }

                        /*
                        int col = 0;
                        int FilTitulo = 7;
                        int eachRow = 0;
                        int row = 8;

                        foreach (DataColumn column in dataTable.Columns)  //printing column headings
                        {

                            worksheet.Cells[FilTitulo, ++col].Value = string.Empty;//column.ColumnName;

                            if (col <= dataTable.Columns.Count)
                            {
                                worksheet.Cells[FilTitulo, col].Value = column.ColumnName;
                                worksheet.Cells[FilTitulo, col].Style.Font.Bold = true;
                                worksheet.Cells[FilTitulo, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells[FilTitulo, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[FilTitulo, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[FilTitulo, col].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            }
                        }

                        if (dataTable.Rows.Count > 0)
                        {
                            int fila = 0;
                            for (eachRow = 0; eachRow < dataTable.Rows.Count;)
                            {
                                col = 1;
                                foreach (DataColumn column in dataTable.Columns)
                                {
                                   

                                    if (col <= dataTable.Columns.Count)
                                    {
                                        worksheet.Cells[row, col].Value = dataTable.Rows[fila][col-1];

                                    }
                                    col++;
                                }
                                    
                                eachRow++;
                                row++;
                                fila++;
                            }
                        }
                        */
                            // Ajustar el ancho de las columnas automáticamente
                       worksheet.Cells.AutoFitColumns();

                        // Convertir el archivo Excel a bytes
                        return excelPackage.GetAsByteArray();
                    }
                }
            }
        }


        [WebMethod]
        public static void ExportarExcelImport(string fechaDesde, string fechaHasta)
        {
            try
            {
                byte[] excelBytes = CreateExcelBytesFromStoredProcedure(fechaDesde, fechaHasta);

                string dateTimeFormat = "yyyyMMddHHmmssfff";
                string formattedDateTime = DateTime.Now.ToString(dateTimeFormat);

                string fileName = "Reporte_" + formattedDateTime + ".xlsx";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                HttpContext.Current.Response.BinaryWrite(excelBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error al exportar el archivo Excel";
            }
        }


    }
}