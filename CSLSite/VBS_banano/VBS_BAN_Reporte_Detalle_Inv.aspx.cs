﻿using System;
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
using VBSEntidades.BananoBodega;

namespace CSLSite
{


    public partial class VBS_BAN_Reporte_Detalle_Inv : System.Web.UI.Page
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
                if (ClsUsuario != null)
                {
                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {
                    LlenaComboBodega();
                }
                else
                {
                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;

                        if (ClsUsuario != null)
                        {
                            this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                            this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                            this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                        }
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocrurrio un error en la pagina", ex);
            }
        }

        public static byte[] CreateExcelBytesFromStoredProcedure(string _ruc, string _bodega)
        {
            //CultureInfo enUS = new CultureInfo("en-US");
            //string fechaLocal = fechaDesde;
            //DateTime fechaConvertida = DateTime.ParseExact(fechaLocal, "d/M/yyyy", enUS);

            //string fechaLocalHasta = fechaHasta;
            //DateTime fechaConvertidaHasta = DateTime.ParseExact(fechaLocalHasta, "d/M/yyyy", enUS);


            // var idTipoCarga = Convert.ToInt32(tipoCargaId);
            // Conexión a la base de datos
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VBS"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Color colorPlaneado = ColorTranslator.FromHtml("#59B653");
                Color colorReservado = ColorTranslator.FromHtml("#5AABD9");
                Color colorDisponible = ColorTranslator.FromHtml("#D9DB63");


                // Crear el comando para ejecutar el SP
                using (SqlCommand command = new SqlCommand("VBS_BAN_REPORTE_STOCK_UBICACION", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros al comando
                    command.Parameters.AddWithValue("@LV_BODEGA", _bodega);
                    command.Parameters.AddWithValue("@LV_LINEA", _ruc);

                    // Crear el DataTable para almacenar los datos del SP
                    DataTable dataTable = new DataTable();

                    // Leer los datos del SP y llenar el DataTable
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                    if (dataTable is null || dataTable.Rows.Count == 0)
                    {
                        return null;
                    }

                    // Ordenar los datos por la columna "Inicio"
                    DataView dataView = dataTable.DefaultView;
                    //dataView.Sort = "Inicio ASC";
                    DataTable sortedTable = dataView.ToTable();

                    //grupo a recorrer
 /*                   var grupos = from p in dataTable.AsEnumerable()
                                 group p by p.Field<string>("Exportador") into g
                                 select new
                                 {
                                     Exportador = g.Key
                                 };*/
                    //fin grupo

                    //detalle


                    //var detalle = from p in dataTable.AsEnumerable()
                    //                  select new
                    //                  {
                    //                      TIPO = p.Field<string>("TIPO"),
                    //                      TURNO = p.Field<string>("TURNO"),
                    //                      ISO = p.Field<int>("ISO TANQUE"),
                    //                      SECO = p.Field<int>("SECO"),
                    //                      TODOS = p.Field<int>("TODOS"),
                    //                      REFEER = p.Field<int>("REFEER"),
                    //                  };
                    //    //fin detalle

                    // Crear el archivo Excel con EPPlus
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");


                        worksheet.Cells["B3:B3"].Value = "Linea:";
                        worksheet.Cells["B3:B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["B3:B3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["B3:B3"].Style.Fill.BackgroundColor.SetColor(colorPlaneado);
                        worksheet.Cells["B3:B3"].Style.Font.Color.SetColor(System.Drawing.Color.White);


                        worksheet.Cells["C3:C3"].Value = _ruc;
                        worksheet.Cells["C3:C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                        worksheet.Cells["B4:B4"].Value = "Bodega:";
                        worksheet.Cells["B4:B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["B4:B4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["B4:B4"].Style.Fill.BackgroundColor.SetColor(colorReservado);
                        worksheet.Cells["B4:B4"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                        worksheet.Cells["C4:C4"].Value = _bodega;
                        worksheet.Cells["C4:C4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Asignar los valores de la cabecera
                        int c = 1;
                        int espa = 2;
                        int r = 6;
                        int inicio = 0;
                        //foreach (var cab in grupos)
                        //{
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



                        //    c = c + espa;
                        //    inicio = c;
                        //}


                        r = 7;
                        c = 1;
                        string anterior = string.Empty;
                        //detalle de informacion
                        //foreach (var cab in grupos)
                        //{

                            inicio = c;
                            //filtramos en base al cada tipo
                            var query = from p in dataTable.AsEnumerable()//dataTable.AsEnumerable().Where(y => y.Field<string>("Exportador") == cab.Exportador)
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

                        //    c = c + dataTable.Columns.Count;


                        //    r = 7;
                        //    c = c + espa;
                        //    inicio = c;
                        //}



                        // Ajustar el ancho de las columnas automáticamente
                        worksheet.Cells.AutoFitColumns();

                        // Convertir el archivo Excel a bytes
                        return excelPackage.GetAsByteArray();
                    }
                }
            }
        }


        [WebMethod]
        public static void ExportarExcel(string fechaDesde, string fechaHasta)
        {
            try
            {
                byte[] excelBytes = CreateExcelBytesFromStoredProcedure(fechaDesde, fechaHasta);
                if (excelBytes is null)
                {
                    HttpContext.Current.Response.StatusCode = 204; // No Content
                    HttpContext.Current.Response.StatusDescription = "No hay datos disponibles para exportar.";
                    //HttpContext.Current.Response.End();
                    return;
                }

                string dateTimeFormat = "yyyyMMddHHmmssfff";
                string formattedDateTime = DateTime.Now.ToString(dateTimeFormat);
                
                string fileName = "Reporte_Stock" + formattedDateTime + ".xlsx";
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

                // Define el código de estado HTTP y la descripción del error.
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error al exportar el archivo Excel";


            }
        }

        public void LlenaComboBodega()
        {
            try
            {
                string oError = string.Empty;
                var oEntidad = BAN_Catalogo_Bodega.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbBodega.DataSource = oEntidad;
                cmbBodega.DataValueField = "codigo";
                cmbBodega.DataTextField = "nombre";
                cmbBodega.DataBind();
            }
            catch (Exception ex)
            {
                var lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_PreStowage.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
                var OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Alerta(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

            }
        }

    }
}