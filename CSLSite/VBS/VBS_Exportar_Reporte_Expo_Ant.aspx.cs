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


    public partial class VBS_Exportar_Reporte_Expo_Ant : System.Web.UI.Page
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
                    this.Carga_CboTipoCargas();
                    //  this.Crear_Sesion();

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

        private void Carga_CboTipoCargas()
        {
            try
            {
                List<VBS_ConsultarTipoCargas> Listado = VBS_ConsultarTipoCargas.ConsultarTipoCargas(out cMensajes);

                // Agregar el registro "TODOS" al inicio de la lista
                VBS_ConsultarTipoCargas todos = new VBS_ConsultarTipoCargas()
                {
                    Id_Carga = 0,
                    Desc_Carga = "TODOS"
                };
                Listado.Insert(0, todos);

                var idcarga = Listado.FirstOrDefault().Id_Carga;

                this.cboTipoCarga.DataSource = Listado;
                this.cboTipoCarga.DataTextField = "Desc_Carga";
                this.cboTipoCarga.DataValueField = "Id_Carga";
                this.cboTipoCarga.DataBind();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboTipoCargas", "Hubo un error al cargar Tipo de cargas", t.loginname));
                //      this.Mostrar_Mensaje(1, Error);
            }
        }

        public static byte[] CreateExcelBytesFromStoredProcedure(string fechaDesde, string tipoCargaId)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            string fechaLocal = fechaDesde; 
            DateTime fechaConvertida = DateTime.ParseExact(fechaLocal, "d/M/yyyy", enUS);


          
            var idTipoCarga = Convert.ToInt32(tipoCargaId);
            // Conexión a la base de datos
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["VBS"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var cabeceraReport = objCab.CabeceraExpoReporte(fechaConvertida, idTipoCarga);// Consulta de la cabecera
                var fechaDesdes = cabeceraReport.Rows[0]["FechaDesde"]; // Acceder al valor del parámetro FechaDesde
                var fechaHasta = cabeceraReport.Rows[0]["FechaHasta"]; // Acceder al valor del parámetro FechaHasta
                var totalPlaneados = cabeceraReport.Rows[0]["TotalPlaneados"]; // Acceder al valor del campo TotalPlaneados
                var totalReservados = cabeceraReport.Rows[0]["TotalReservados"]; // Acceder al valor del campo TotalReservados
                var totalDisponibles = cabeceraReport.Rows[0]["TotalDisponibles"]; // Acceder al valor del campo TotalDisponibles
                Color colorPlaneado = ColorTranslator.FromHtml("#59B653");
                Color colorReservado = ColorTranslator.FromHtml("#5AABD9");
                Color colorDisponible = ColorTranslator.FromHtml("#D9DB63");


                // Crear el comando para ejecutar el SP
                using (SqlCommand command = new SqlCommand("VBS_DETALLE_REPORTE_EXPO", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros al comando
                    command.Parameters.AddWithValue("@FechaDesde", fechaConvertida);
                    command.Parameters.AddWithValue("@FechaHasta", fechaConvertida);
                    command.Parameters.AddWithValue("@idTipoCarga", idTipoCarga);
                    // Crear el DataTable para almacenar los datos del SP
                    DataTable dataTable = new DataTable();

                    // Leer los datos del SP y llenar el DataTable
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                    // Ordenar los datos por la columna "Inicio"
                    DataView dataView = dataTable.DefaultView;
                    dataView.Sort = "Inicio ASC";
                    DataTable sortedTable = dataView.ToTable();

                    // Crear el archivo Excel con EPPlus
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");


                        worksheet.Cells["G3:G3"].Value = "Total Citas Exportación: Planeados";
                        worksheet.Cells["G3:G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["G3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["G3:G3"].Style.Fill.BackgroundColor.SetColor(colorPlaneado);
                        worksheet.Cells["G3:G3"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                        // Establecer valor y estilo para Total Citas Exportación: Planeados
                        worksheet.Cells["H3:H3"].Value = totalPlaneados;
                        worksheet.Cells["H3:H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Establecer estilo y color para Total Citas Exportación: Reservados
                        worksheet.Cells["I3:I3"].Value = "Total Citas Exportación: Reservados";
                        worksheet.Cells["I3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["I3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["I3:I3"].Style.Fill.BackgroundColor.SetColor(colorReservado);
                        worksheet.Cells["I3:I3"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                        // Establecer valor y estilo para Total Citas Exportación: Reservados
                        worksheet.Cells["J3:J3"].Value = totalReservados;
                        worksheet.Cells["J3:J3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Establecer estilo y color para Total Citas Exportación: Disponible
                        worksheet.Cells["K3:K3"].Value = "Total Citas Exportación: Disponible";
                        worksheet.Cells["K3:K3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["K3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["K3:K3"].Style.Fill.BackgroundColor.SetColor(colorDisponible);
                        worksheet.Cells["K3:K3"].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                        // Establecer valor y estilo para Total Citas Exportación: Disponible
                        worksheet.Cells["L3:L3"].Value = totalDisponibles;
                        worksheet.Cells["L3:L3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                        // Asignar los valores de la cabecera
                        worksheet.Cells[6, 1].Value = "Inicio";
                        worksheet.Cells[6, 2].Value = "Fin";
                        worksheet.Cells[6, 3].Value = "Planeado";
                        worksheet.Cells[6, 4].Value = "Reservado";
                        worksheet.Cells[6, 5].Value = "Disponible";
                        worksheet.Cells[6, 6].Value = "Tipo_Carga";
                        worksheet.Cells[6, 7].Value = "Tipo_contenedor";

                        worksheet.Cells["A6:G6"].Style.Font.Bold = true;
                        worksheet.Cells["A6:G6"].Style.Font.Size = 12;

                        int rowIndex = 7; // Índice de la fila actual en el archivo Excel
                        TimeSpan lastHour = TimeSpan.MinValue; // Última hora procesada

                        // Recorrer los registros ordenados
                        foreach (DataRow row in sortedTable.Rows)
                        {
                            TimeSpan inicio = TimeSpan.Parse(row["Inicio"].ToString());
                            TimeSpan fin = TimeSpan.Parse(row["Fin"].ToString());

                            // Verificar si la hora de inicio ha cambiado
                            if (inicio > lastHour)
                            {
                                // Asignar la hora en la primera columna
                                worksheet.Cells[rowIndex, 1].Value = inicio.ToString(@"hh\:mm\:ss");

                                // Asignar los valores de inicio y fin en la segunda columna
                                worksheet.Cells[rowIndex, 2].Value = fin.ToString(@"hh\:mm\:ss");

                                // Recorrer los registros correspondientes a la hora actual y asignar los valores horizontalmente
                                int columnIndex = 3; // Índice de la columna actual en el archivo Excel
                                foreach (DataRow horaRow in sortedTable.Rows)
                                {
                                    TimeSpan horaInicio = TimeSpan.Parse(horaRow["Inicio"].ToString());
                                    TimeSpan horaFin = TimeSpan.Parse(horaRow["Fin"].ToString());

                                    if (horaInicio == inicio && horaFin == fin)
                                    {
                                        //Obtener valores de la tabla
                                        string planeado = horaRow["Planeado"].ToString();
                                        string reservado = horaRow["Reservado"].ToString();
                                        string disponible = horaRow["Disponible"].ToString();
                                        string tipoCarga = horaRow["Tipo_Carga"].ToString();
                                        string tipoContenedor = horaRow["Tipo_contenedor"].ToString();


                                        //Crear cabeceras por cada nuevo array
                                        worksheet.Cells[6, columnIndex].Value = "Planeado";
                                        worksheet.Cells[6, columnIndex + 1].Value = "Reservado";
                                        worksheet.Cells[6, columnIndex + 2].Value = "Disponible";
                                        worksheet.Cells[6, columnIndex + 3].Value = "Tipo_Carga";
                                        worksheet.Cells[6, columnIndex + 4].Value = "Tipo_contenedor";

                                        //Modificar las celdas de titulos con Negrita y aumento de tamaño en la letra

                                     
                                        worksheet.Cells[6, columnIndex].Style.Font.Bold = true;
                                        worksheet.Cells[6, columnIndex].Style.Font.Size = 12;
                                        worksheet.Cells[6, columnIndex + 1].Style.Font.Bold = true;
                                        worksheet.Cells[6, columnIndex + 1].Style.Font.Size = 12;
                                        worksheet.Cells[6, columnIndex + 2].Style.Font.Bold = true;
                                        worksheet.Cells[6, columnIndex + 2].Style.Font.Size = 12;
                                        worksheet.Cells[6, columnIndex + 3].Style.Font.Bold = true;
                                        worksheet.Cells[6, columnIndex + 3].Style.Font.Size = 12;
                                        worksheet.Cells[6, columnIndex + 4].Style.Font.Bold = true;
                                        worksheet.Cells[6, columnIndex + 4].Style.Font.Size = 12;

                                        //agregar los valores a las columnas
                                        worksheet.Cells[rowIndex, columnIndex].Value = planeado;
                                        worksheet.Cells[rowIndex, columnIndex + 1].Value = reservado;
                                        worksheet.Cells[rowIndex, columnIndex + 2].Value = disponible;
                                        worksheet.Cells[rowIndex, columnIndex + 3].Value = tipoCarga;
                                        worksheet.Cells[rowIndex, columnIndex + 4].Value = tipoContenedor;
                                       

                                        // Pintar las celdas "Planeado" de verde claro
                                        using (var range = worksheet.Cells[rowIndex, columnIndex])
                                        {
                                            var fill = range.Style.Fill;
                                            fill.PatternType = ExcelFillStyle.Solid;

                                            Color color = ColorTranslator.FromHtml("#59B653"); // Establece el color hexadecimal aquí
                                            fill.BackgroundColor.SetColor(color);
                                        }

                                        using (var range = worksheet.Cells[rowIndex, columnIndex + 1])
                                        {
                                            var fill = range.Style.Fill;
                                            fill.PatternType = ExcelFillStyle.Solid;

                                            Color color = ColorTranslator.FromHtml("#5AABD9"); // Establece el color hexadecimal aquí
                                            fill.BackgroundColor.SetColor(color);
                                        }

                                        // Pintar las celdas "Disponible" de amarillo claro
                                        using (var range = worksheet.Cells[rowIndex, columnIndex + 2])
                                        {
                                            
                                            var fill = range.Style.Fill;
                                            fill.PatternType = ExcelFillStyle.Solid;

                                            Color color = ColorTranslator.FromHtml("#D9DB63"); // Establece el color hexadecimal aquí
                                            fill.BackgroundColor.SetColor(color);
                                        }

                                        worksheet.Cells["A5:AA5"].Merge = true;
                                        worksheet.Cells["A5:AA5"].Value = "Disponibilidad de citas Exportación";
                                        worksheet.Cells["A5:AA5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        worksheet.Cells["A5:AA5"].Style.Font.Size = 14;
                                        worksheet.Cells["A5:AA5"].Style.Font.Bold = true;

                                        columnIndex += 5; // Incrementar el índice de columna
                                    }
                                }

                                rowIndex++; // Incrementar el índice de fila
                                lastHour = inicio; // Actualizar la última hora procesada
                            }
                        }

                        // Ajustar el ancho de las columnas automáticamente
                        worksheet.Cells.AutoFitColumns();

                        // Convertir el archivo Excel a bytes
                        return excelPackage.GetAsByteArray();
                    }
                }
            }
        }


        [WebMethod]

        public static void ExportarExcel(string fechaDesde, string tipoCargaId)
        {
            try
            {
                byte[] excelBytes = CreateExcelBytesFromStoredProcedure(fechaDesde, tipoCargaId);

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

                // Define el código de estado HTTP y la descripción del error.
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error al exportar el archivo Excel";

              
            }
        }



    }
}