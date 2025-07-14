using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using CLSiteUnitLogic.FacturaCls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace CLSiteUnitLogic
{
    public class LogicExcel
    {

        public static byte[] CreateExcelBytesHorasReefer()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte_Horas_Reefer");



                var listaContenedores = HttpContext.Current.Session["ContenedorSeleccionado"] as CLSiteUnitLogic.Cls_Container.Cls_Container;
                var datatabl = Cls_Factura.ObtenerSoporteReefer(listaContenedores?.CNTR_BKNG_BOOKING);

                if (datatabl == null || datatabl.Rows.Count == 0)
                {
                    worksheet.Cell(1, 1).Value = "No hay datos disponibles para mostrar.";
                    worksheet.Range(1, 1, 1, 3).Merge().Style.Font.SetBold().Font.FontSize = 12;
                    worksheet.Range(1, 1, 1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }

                var imagePath = HttpContext.Current.Server.MapPath("~/shared/imgs/carbono_img/logoContecon.jpg");
                if (File.Exists(imagePath))
                {
                    IXLPicture picture = worksheet.AddPicture(imagePath)
                                         .MoveTo(worksheet.Cell("A1"))
                                         .WithSize(350, 100);
                }


                // 🔹 Renombrar columnas
                datatabl.Columns["POWER_CONNECT"].ColumnName = "FECHA DE CONEXIÓN";
                datatabl.Columns["POWER_DISCONNECT"].ColumnName = "FECHA DE DESCONEXIÓN";
                datatabl.Columns["HORAS_REEFER_OPE"].ColumnName = "HORAS DE OPERACIÓN REEFER";
                datatabl.Columns["QTY_REFFER_LINE"].ColumnName = "CANTIDAD DE HORAS REEFER LÍNEA";
                datatabl.Columns["QTY_REFFER_OTHER"].ColumnName = "CANTIDAD HORAS REEFER EXPORTADOR";

                // 🔹 Eliminar gkey si existe
                if (datatabl.Columns.Contains("gkey"))
                    datatabl.Columns.Remove("gkey");

                // 🔹 Reordenar columnas
                string[] ordenColumnas = {
                "REFERENCIA", "BUQUE", "CONTENEDOR", "VIAJE", "CATEGORIA", "LINEA",
                "EXPORTADOR", "BOOKING", "FECHA DE CONEXIÓN", "FECHA DE DESCONEXIÓN",
                "HORAS DE OPERACIÓN REEFER", "CANTIDAD DE HORAS REEFER LÍNEA", "CANTIDAD HORAS REEFER EXPORTADOR"
            };

                var columnasValidas = ordenColumnas.Where(c => datatabl.Columns.Contains(c)).ToList();
                datatabl = datatabl.DefaultView.ToTable(false, columnasValidas.ToArray());

                // 🔹 Fecha lado derecho
                worksheet.Cell("M2").Value = $"Fecha de consulta : {DateTime.Now:dd-MM-yyyy, hh.mm tt}";
                worksheet.Cell("M2").Style.Font.Italic = true;
                worksheet.Cell("M2").Style.Font.FontSize = 10;
                worksheet.Cell("M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                // 🔹 Título centrado

                worksheet.Cell("G3").Value = "REPORTE DE HORAS REEFER";
                worksheet.Range("G3:J3").Merge();
                worksheet.Range("G3:J3").Style.Font.SetBold().Font.FontSize = 14;
                worksheet.Range("G3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                // 🔹 Encabezados
                for (int i = 0; i < datatabl.Columns.Count; i++)
                {
                    worksheet.Cell(6, i + 1).Value = datatabl.Columns[i].ColumnName;
                    worksheet.Cell(6, i + 1).Style.Font.SetBold();
                    worksheet.Cell(6, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // 🔹 Datos
                int fila = 7;
                foreach (DataRow dr in datatabl.Rows)
                {
                    for (int col = 0; col < datatabl.Columns.Count; col++)
                    {
                        object valor = dr[col];

                        if (valor is DateTime fecha)
                            worksheet.Cell(fila, col + 1).Value = fecha.ToString("dd/MM/yyyy HH:mm");
                        else if (valor is decimal || valor is double || valor is float)
                            worksheet.Cell(fila, col + 1).Value = Convert.ToDouble(valor);
                        else if (valor != DBNull.Value)
                            worksheet.Cell(fila, col + 1).Value = valor.ToString();
                        else
                            worksheet.Cell(fila, col + 1).Value = "";
                    }
                    fila++;
                }



                worksheet.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

    }
}
