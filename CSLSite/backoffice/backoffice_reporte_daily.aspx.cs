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
using BillionReglasNegocio;

using OfficeOpenXml;
using System.Data;
using System.Threading;

namespace CSLSite.backoffice
{
    public partial class backoffice_reporte_daily : System.Web.UI.Page
    {
        #region "Clases"
        //Cls_Usuario ClsUsuario;
       
        #endregion

        #region "Variables"

        private string cMensajes;


        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();

        #endregion

    
        #region "Metodos"

        private void Actualiza_Paneles()
        {
            //UPDETALLE.Update();
            UPCARGA.Update();
           
        }

        private void Actualiza_Panele_Detalle()
        {
            //UPDETALLE.Update();
        }

        
        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }


        private void Mostrar_Mensaje(string Mensaje)
        {
           
               
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading();
           

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;   
            this.Actualiza_Paneles();
            OcultarLoading();

        }
        #endregion



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

            this.banmsg.Visible = IsPostBack;
          
            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
              
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                   
                    //banmsg.Visible = false;
                }

                this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
                this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);

                if (!Page.IsPostBack)
                {
                    string desde = DateTime.Today.Month.ToString("D2") + "/01/"  + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

        protected void chkFacturar_CheckedChanged(object sender, EventArgs e)
        {

            

        }
       
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            
            if (Response.IsClientConnected)
            {
                try
                {

                    

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFechaDesde.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TxtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    CultureInfo enUS = new CultureInfo("en-US");
                   
                    if (!string.IsNullOrEmpty(TxtFechaDesde.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;
                           
                        }
                    }

                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} \\nNO deber ser mayor a la\\nFecha final: {1}", TxtFechaDesde.Text, TxtFechaDesde.Text));
                        return;
                    }
                    if (diferenciaEnDias > 366)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un año."));
                        return;
                    }

                    var ExistenRegistros = Cls_Reporte_Daily.Reporte_Daily(fechadesde,
                                                  fechahasta,
                                                  int.Parse(this.CboTipo.SelectedValue.ToString()),
                                                  int.Parse( this.CboFactura.SelectedValue.ToString()),
                                                   out cMensajes);


                    if (ExistenRegistros == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                   
                    if (ExistenRegistros.Rows.Count <= 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);



                        //DataTable dt;
                        string v_nameTable = "Reporte_Daily_Billing";
                        ExistenRegistros.TableName = v_nameTable;

                        this.UPCARGA.Update();

                        try
                        {
                            using (ExcelPackage xp = new ExcelPackage())
                            {

                                {
                                    ExcelWorksheet ws = xp.Workbook.Worksheets.Add(ExistenRegistros.TableName);

                                    int rowstart = 2;
                                    int colstart = 2;
                                    int rowend = rowstart;
                                    int colend = colstart + ExistenRegistros.Columns.Count;

                                    ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                                    ws.Cells[rowstart, colstart, rowend, colend].Value = ExistenRegistros.TableName;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                                    rowstart += 2;
                                    rowend = rowstart + ExistenRegistros.Rows.Count;
                                    ws.Cells[rowstart, colstart].LoadFromDataTable(ExistenRegistros, true);
                                    int i = 1;
                                    foreach (DataColumn dc in ExistenRegistros.Columns)
                                    {
                                        i++;
                                        if (dc.DataType == typeof(decimal))
                                        {
                                            ws.Column(i).Style.Numberformat.Format = "#0.00";
                                        }
                                        if (dc.DataType == typeof(DateTime))
                                        {
                                            ws.Column(i).Style.Numberformat.Format = "dd-MM-yyyy HH:mm";
                                        }
                                    }
                                    ws.Cells[ws.Dimension.Address].AutoFitColumns();



                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                                       ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                                       ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                                       ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                }
                                Response.AddHeader("content-disposition", "attachment;filename=" + v_nameTable + ".xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.BinaryWrite(xp.GetAsByteArray());
                                Response.Flush();
                                Response.End();
                            }

                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
                            string eRROR = ex.Message.ToString();
                            this.Actualiza_Paneles();
                        }

                        this.Ocultar_Mensaje();
                        this.Actualiza_Paneles();

                        // CreateExcelFile(ExistenRegistros, "Reporte_Daily_Billing");

                    }

                   
                    this.Actualiza_Paneles();
                    this.Ocultar_Mensaje();
                   

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                  
                    //this.Ocultar_Mensaje();
                    //this.Actualiza_Paneles();

                }
            }



            
        }



        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        private void CreateExcelFile(DataTable dt, string v_nameTable)
        {
            try
            {
                dt.TableName = v_nameTable;
               

                using (ExcelPackage xp = new ExcelPackage())
                {
                   
                    {
                        ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt.TableName);

                        int rowstart = 2;
                        int colstart = 2;
                        int rowend = rowstart;
                        int colend = colstart + dt.Columns.Count;

                        ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                        ws.Cells[rowstart, colstart, rowend, colend].Value = dt.TableName;
                        ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                        ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                        rowstart += 2;
                        rowend = rowstart + dt.Rows.Count;
                        ws.Cells[rowstart, colstart].LoadFromDataTable(dt, true);
                        int i = 1;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            i++;
                            if (dc.DataType == typeof(decimal))
                            {
                                ws.Column(i).Style.Numberformat.Format = "#0.00";
                            }
                            if (dc.DataType == typeof(DateTime))
                            {
                                ws.Column(i).Style.Numberformat.Format = "dd-MM-yyyy";
                            }
                        }
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();



                        ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                           ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                           ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                           ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    }
                    Response.AddHeader("content-disposition", "attachment;filename=" + v_nameTable + ".xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xp.GetAsByteArray());
                    Response.End();
                }
             
            }
            catch
            {
                throw;
            }
        }


     
    }
}