using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace CSLSite.bloqueo
{
    public partial class WRPTLOCKCLIENTES : System.Web.UI.Page
    {
        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;

        private DataTable p_drCliente
        {
            get
            {
                return (DataTable)Session["drClienteLOCKRpt"];
            }
            set
            {
                Session["drClienteLOCKRpt"] = value;
            }

        }
        private String p_user
        {
            get
            {
                return (String)Session["puser"];
            }
            set
            {
                Session["puser"] = value;
            }

        }
        private DataTable p_result
        {
            get
            {
                return (DataTable)Session["dvResultllockrpt"];
            }
            set
            {
                Session["dvResultllockrpt"] = value;
            }

        }
        private void IniDsCliente()
        {
            XmlDocument docXml = new XmlDocument();
            XmlElement elem;
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            docXml.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><PASEPUERTA/>");
            elem = docXml.CreateElement("GetEmpresa");
            elem.SetAttribute("CLNT_TYPE", null);
            elem.SetAttribute("CLNT_ACTIVE", "Y");
            docXml.DocumentElement.AppendChild(elem);
            dsRetorno = WPASEPUERTA.GetEmpresainfo(docXml.OuterXml);
            p_drCliente = dsRetorno.Tables[0];
        }
        protected void Page_init(object sender, EventArgs e)
        {
            //this.Master.Titulo = "Reporte Bloqueo de  CLIENTE";
            //this.Master.FavName = "Reporte Bloquear Cliente";
            if (IsPostBack != true)
            {
                p_user = User.Identity.Name;
                IniDsCliente();
                Limpiar();
            }

        }
        public void Limpiar()
        {
            p_result = null;
            TXTCLIENTE.Text = null;
            TXTFECHAINICIO.Text = DateTime.Now.ToString("MM/dd/yyyy");
            TXTFECHAFIN.Text = DateTime.Now.ToString("MM/dd/yyyy");
            rwReporte.LocalReport.DataSources.Clear();
        }
        protected void CMDADD_Click(object sender, EventArgs e)
        {
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            Boolean wresult = true;
            String Widem = "";
            try
            {
                // Valida cpmentario


                // Valida Cliente
                if (!String.IsNullOrEmpty(TXTCLIENTE.Text.ToString()))
                {

                    if (TXTCLIENTE.Text.Split('-').ToList().Count() > 0)
                    {
                        Widem = TXTCLIENTE.Text.Split('-').ToList()[0];
                    }
                    else
                    {
                        Widem = TXTCLIENTE.Text;

                    }

                    var wEmpresa = (from row in p_drCliente.AsEnumerable()
                                    where row.Field<String>("IDEMPRESA").Trim().Equals(Widem.Trim())
                                    select row.Field<String>("EMPRESA")).Count();


                    if ((int)wEmpresa <= 0)
                    {
                        utilform.MessageBox("Cliente no valida", this);
                        wresult = false;
                    }
                }
                

                // Valida wfecha
                DateTime Wfechas;
                DateTime WfechasIni;

                if (DateTime.TryParseExact(TXTFECHAINICIO.Text.ToString(), "MM/dd/yyyy", new CultureInfo("es-EC"), DateTimeStyles.None, out Wfechas) != true)
                {

                    utilform.MessageBox("Ingrese una Fecha de Inicio Valida", this);
                    wresult = false;
                }



                if (DateTime.TryParseExact(TXTFECHAFIN.Text.ToString(), "MM/dd/yyyy", new CultureInfo("es-EC"), DateTimeStyles.None, out Wfechas) != true)
                {

                    utilform.MessageBox("Ingrese una Fecha de Fin Valida", this);
                    wresult = false;
                }
                else
                {
                    DateTime.TryParseExact(TXTFECHAINICIO.Text.ToString(), "MM/dd/yyyy", new CultureInfo("es-EC"), DateTimeStyles.None, out WfechasIni);

                    if (Wfechas < WfechasIni)
                    {
                        utilform.MessageBox("Ingrese una Fecha de Fin debe ser mayor o igual a la Fecha Inicial", this);
                        wresult = false;
                    }

                }
                if (wresult)
                {
                    //System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    //      new System.Xml.Linq.XElement("REPORT_CLIENTS",
                    //      new System.Xml.Linq.XElement("REPORT_CLIENT",
                    //      new System.Xml.Linq.XAttribute("ID_Cliente", Widem),
                    //      new System.Xml.Linq.XAttribute("Fecha", TXTFECHAINICIO.Text.ToString()),
                    //      new System.Xml.Linq.XAttribute("FechaFin", TXTFECHAFIN.Text.ToString()),
                    //      new System.Xml.Linq.XAttribute("Tipo", "LOCK")


                    //      )));


                    //DataSet WRESUTL = new DataSet();
                    app_start.bloqueo oBloqueo = new app_start.bloqueo();
                    //WRESUTL = WPASEPUERTA.RImpLibLockCliente(docXML.ToString());
                    DataTable dt = new DataTable();
                    DateTime wFecha;
                    DateTime wFechaFin;


                    DateTime.TryParseExact(TXTFECHAINICIO.Text.ToString(), "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out wFecha);
                    DateTime.TryParseExact(TXTFECHAFIN.Text.ToString(), "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out wFechaFin);

                    dt = oBloqueo.RLibLockCliente(wFecha, wFechaFin, cmbEstado.SelectedValue, Widem);

                    p_result = dt;
                    LoadReport();
                }

            }
            catch (Exception ex)
            {

                utilform.MessageBox(ex.Message, this);
            }

        }
        public void LoadReport()
        {
            String Reporte = "RptLockCliente.rdlc";
            ReportDataSource wdatasourc;
            ReportParameter wrparameter = new ReportParameter();

            Reporte = this.Server.MapPath(@"..\bloqueo\" + Reporte);
            if (inicializaReporte(Reporte) != true)
            {
                return;
            }

            else
            {
                wdatasourc = new ReportDataSource("dsLockReporte", p_result);
                AñadeDatasorurce(wdatasourc);

                rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter });
            }

            rwReporte.LocalReport.Refresh();
        }
        public void AñadeDatasorurce(ReportDataSource wdatasourc)
        {
            rwReporte.LocalReport.DataSources.Add(wdatasourc);
        }
        public Boolean inicializaReporte(String Reporte)
        {
            ReportParameter wrparameter = new ReportParameter();
            String wuser = p_user;
            if (System.IO.File.Exists(Reporte) != true)
            {
                utilform.MessageBox("El Reporte No Existe..", this);
                return false;
            }
            wrparameter = new ReportParameter("rp_user", wuser);
            rwReporte.ProcessingMode = ProcessingMode.Local;
            rwReporte.LocalReport.ReportPath = Reporte;
            rwReporte.LocalReport.DataSources.Clear();
            rwReporte.LocalReport.Refresh();
            rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter });
            rwReporte.Visible = true;


            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [System.Web.Script.Services.ScriptMethod]
        [System.Web.Services.WebMethod]
        public static string[] GetClienteList(String prefixText, int count)
        {
            WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            prefixText = prefixText.ToUpper();
            XmlDocument docXml = new XmlDocument();
            XmlElement elem;
            DataTable DTRESULT = new DataTable();
            DTRESULT = (DataTable)HttpContext.Current.Session["drClienteLOCKRpt"];
            var list = (from currentStat in DTRESULT.Select("EMPRESA like '%" + prefixText + "%'").AsEnumerable()
                        select currentStat.Field<String>("EMPRESA")).ToList();
            string[] prefixTextArray = list.Take(5).ToArray<string>();
            return prefixTextArray;
        }

        protected void CMDLIMPIAR_Click(object sender, System.EventArgs e)
        {
            Limpiar();
        }
    }
}