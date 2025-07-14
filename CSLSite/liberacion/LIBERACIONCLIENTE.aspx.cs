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
using csl_log;
using CSLSite.bloqueo;

namespace CSLSite.liberacion
{
    public partial class LIBERACIONCLIENTE : System.Web.UI.Page
    {


        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;

        private DataTable p_drCliente
        {
            get
            {
                return (DataTable)Session["drClienteLib"];
            }
            set
            {
                Session["drClienteLib"] = value;
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
                return (DataTable)Session["dvResult"];
            }
            set
            {
                Session["dvResult"] = value;
            }

        }
        private void IniDsCliente()
        {

            try
            {

                XmlDocument docXml = new XmlDocument();
                XmlElement elem;
                DataSet dsRetorno = new DataSet();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

                docXml.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><PASEPUERTA/>");
                elem = docXml.CreateElement("GetEmpresa");
                elem.SetAttribute("CLNT_TYPE", null);
                //  elem.SetAttribute("CLNT_TYPE", "OTHR");
                elem.SetAttribute("CLNT_ACTIVE", "Y");


                docXml.DocumentElement.AppendChild(elem);
                dsRetorno = WPASEPUERTA.GetEmpresainfo(docXml.OuterXml);
                p_drCliente = dsRetorno.Tables[0];

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "LIBERACIONCLIENTE", "IniDsCliente", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                utilform.MessageBox(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} - {1}", number, ex.Message), this);
            }

        }
        protected void Page_init(object sender, EventArgs e)
        {
            //this.Master.Titulo = "LIBERACION DE CLIENTE";
            //this.Master.FavName = "Liberar Cliente";
            if (IsPostBack != true)
            {
                p_user = User.Identity.Name;
                IniDsCliente();
                Limpiar();
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void CMDBUSCAR_Click(object sender, EventArgs e)
        {
            DataSet WRESUTL = new DataSet();
            if (!String.IsNullOrEmpty(TXTCLIENTE.Text.ToString()))
            {
                String Widem;
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
                    utilform.MessageBox("Empresa no valida", this);
                }
                else
                {
                    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                    //metodo wcf
                    try
                    {
                        System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("GetClientes", new System.Xml.Linq.XElement("GetCliente",
                        new System.Xml.Linq.XAttribute("ID_Cliente", Widem))));

                        WRESUTL = WPASEPUERTA.GetLibClientesinfo(docXML.ToString());
                        p_result = WRESUTL.Tables[0];
                        GVRESULT.DataSource = p_result;
                        GVRESULT.DataBind();
                    }
                    catch (Exception EX)
                    {
                        utilform.MessageBox(EX.Message, this);
                    }

                }

            }
        }
        public void Limpiar()
        {
            p_result = null;
            TXTCLIENTE.Text = null;
            TXTCOMENTARIO.Text = null;
            TXTFECHAINICIO.Text = DateTime.Now.ToString("MM/dd/yyyy");
            TXTFECHAFIN.Text = DateTime.Now.ToString("MM/dd/yyyy");
            GVRESULT.DataSource = p_result;
            GVRESULT.DataBind();

        }
        protected void CMDLIMPIAR_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        protected void CMDADD_Click(object sender, EventArgs e)
        {
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            Boolean wresult = true;
            String Widem = "";

            try
            {

                // Valida cpmentario
                if (String.IsNullOrEmpty(TXTCOMENTARIO.Text))
                {
                    utilform.MessageBox("Ingrese el Comentario...", this);
                    wresult = false;
                }

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
                else
                {
                    utilform.MessageBox("Ingrese el Cliente...", this);
                    wresult = false;
                }

                // Valida wfecha
                DateTime Wfechas;
                DateTime WfechasIni;


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
                    else
                    {

                        System.Xml.Linq.XDocument docXMLValida = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                new System.Xml.Linq.XElement("GetClientes",
                                    new System.Xml.Linq.XElement("GetCliente",
                                    new System.Xml.Linq.XAttribute("ID_Cliente", Widem),
                                    new System.Xml.Linq.XAttribute("Fecha", TXTFECHAFIN.Text.ToString()),
                                    new System.Xml.Linq.XAttribute("Tipo", "LIB"))));
                        if (!Boolean.Parse(WPASEPUERTA.ISDateValidoLiblockCliente(docXMLValida.ToString()).ToString()))
                        {

                            utilform.MessageBox("Ya existe un Rango para el Cliente..", this);
                            wresult = false;
                        }
                    }

                }
                if (wresult)
                {
                    System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                          new System.Xml.Linq.XElement("LIBERATION_CLIENTS",
                          new System.Xml.Linq.XElement("FNA_P_LIBERATION_CLIENTS",
                          new System.Xml.Linq.XAttribute("CUSTOMER", Widem),
                          new System.Xml.Linq.XAttribute("START_DATE", TXTFECHAINICIO.Text.ToString()),
                          new System.Xml.Linq.XAttribute("END_DATE", TXTFECHAFIN.Text.ToString()),
                          new System.Xml.Linq.XAttribute("STATUS", "S"),
                          new System.Xml.Linq.XAttribute("USER", p_user),
                          new System.Xml.Linq.XAttribute("TERMINAL", utilform.Obtenerip().ToString()),
                          new System.Xml.Linq.XAttribute("USER_UPDATE", p_user),
                          new System.Xml.Linq.XAttribute("TERMINAL_UPDATE", utilform.Obtenerip().ToString()),
                          new System.Xml.Linq.XAttribute("COMMENTS", TXTCOMENTARIO.Text.ToUpper().ToString()),
                          new System.Xml.Linq.XAttribute("flag", "I")
                          )));

                    WPASEPUERTA.SaveLiberacion_Clientes(docXML.ToString());

                    System.Xml.Linq.XDocument docXMLConsulta = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                 new System.Xml.Linq.XElement("GetClientes", new System.Xml.Linq.XElement("GetCliente",
                 new System.Xml.Linq.XAttribute("ID_Cliente", Widem))));
                    DataSet WRESUTL = new DataSet();
                    WRESUTL = WPASEPUERTA.GetLibClientesinfo(docXMLConsulta.ToString());
                    p_result = WRESUTL.Tables[0];
                    GVRESULT.DataSource = p_result;
                    GVRESULT.DataBind();

                    utilform.MessageBox("Liberacion de Cliente generado Exitosamente... ", this);

                }



            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "LIBERACIONCLIENTE", "CMDADD_Click", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                utilform.MessageBox(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} - {1}", number, ex.Message), this);

            }



        }
        protected void GVRESULT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVRESULT.PageIndex = e.NewPageIndex;
            GVRESULT.DataSource = p_result.AsDataView();
            GVRESULT.DataBind();
        }
        protected void CHKSTATUS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                String id = GVRESULT.DataKeys[row.RowIndex].Value.ToString();
                String status;
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                CheckBox CHKSTATUS = (CheckBox)row.FindControl("CHKSTATUS");
                if (CHKSTATUS.Checked)
                {
                    status = "S";
                }
                else
                {
                    status = "N";

                }

                var currentStatRow = (from currentStat in p_result.AsEnumerable()
                                      where currentStat.Field<String>("SECUENCIAL") == id.ToString()
                                      select currentStat).FirstOrDefault();


                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                           new System.Xml.Linq.XElement("LIBERATION_CLIENTS",
                           new System.Xml.Linq.XElement("FNA_P_LIBERATION_CLIENTS",
                           new System.Xml.Linq.XAttribute("SECUENCIAL", id),
                           new System.Xml.Linq.XAttribute("CUSTOMER", currentStatRow["CUSTOMER"]),
                           new System.Xml.Linq.XAttribute("STATUS", status),
                           new System.Xml.Linq.XAttribute("USER_UPDATE", p_user),
                           new System.Xml.Linq.XAttribute("TERMINAL_UPDATE", utilform.Obtenerip().ToString()),
                            new System.Xml.Linq.XAttribute("flag", "U")
                           )));

                WPASEPUERTA.SaveLiberacion_Clientes(docXML.ToString());
                currentStatRow["STATUS"] = CHKSTATUS.Checked;
                currentStatRow.AcceptChanges();
                GVRESULT.DataSource = p_result;
                GVRESULT.DataBind();

                utilform.MessageBox("Registro actualizado con éxito", this);


            }
            catch (Exception exc)
            {
                var number = log_csl.save_log<Exception>(exc, "LOCKCLIENTES", "CHKSTATUS_CheckedChanged(1)", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                utilform.MessageBox(string.Format("Error Al Actualizar el Cliente - {0} - Codigo de error {1}", exc.Message.ToString(), number), this);
            }

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
            DTRESULT = (DataTable)HttpContext.Current.Session["drClienteLib"];
            var list = (from currentStat in DTRESULT.Select("EMPRESA like '%" + prefixText + "%'").AsEnumerable()
                        select currentStat.Field<String>("EMPRESA")).ToList();
            string[] prefixTextArray = list.Take(5).ToArray<string>();
            return prefixTextArray;
        }

        protected void TXTFECHAINICIO_TextChanged(object sender, EventArgs e)
        {

        }

    }
}