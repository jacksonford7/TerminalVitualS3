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
using CSLSite;
using BillionEntidades;

namespace CSLSite
{
    public partial class LOCKCLIENTES : System.Web.UI.Page
    {
        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;
        usuario ClsUsuario;
        private Cls_EnviarEmail objProcesar = new Cls_EnviarEmail();

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private bool sap_valida = false;

        private decimal SaldoPendiente = 0;
        private decimal ValorVencido = 0;
        private decimal ValorPendiente = 0;


        private DataTable p_drCliente
        {
            get
            {
                return (DataTable)Session["drClienteLiblock"];
            }
            set
            {
                Session["drClienteLiblock"] = value;
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
                return (DataTable)Session["dvResultlock"];
            }
            set
            {
                Session["dvResultlock"] = value;
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

                utilform.MessageBox(ex.Message.ToString(),this);
               
            }
           


        }
        protected void Page_init(object sender, EventArgs e)
        {
            //this.Master.Titulo = "Bloqueo de  CLIENTE";
            //this.Master.FavName = "Bloquear Cliente";
            if (IsPostBack != true)
            {
                ClsUsuario = Page.Tracker();

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
                    
                    try
                    {
                        app_start.bloqueo oBloqueo = new app_start.bloqueo();
                        var detalle = oBloqueo.GetLockClientes(Widem);
                        p_result = detalle;
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
            String strAisv = "";
            if (ChAISV.Checked)
            {
                strAisv = "Y";
            }
            else
            {
                strAisv = "N";
            }
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
                                    new System.Xml.Linq.XAttribute("Tipo", "lOCK"))));
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
                          new System.Xml.Linq.XElement("LOCK_CLIENTS",
                          new System.Xml.Linq.XElement("FNA_P_LOCK_CLIENTS",
                          new System.Xml.Linq.XAttribute("CUSTOMER", Widem),
                          new System.Xml.Linq.XAttribute("START_DATE", TXTFECHAINICIO.Text.ToString()),
                          new System.Xml.Linq.XAttribute("END_DATE", TXTFECHAFIN.Text.ToString()),
                          new System.Xml.Linq.XAttribute("STATUS", "S"),
                          new System.Xml.Linq.XAttribute("USER", p_user),
                          new System.Xml.Linq.XAttribute("TERMINAL", utilform.Obtenerip().ToString()),
                          new System.Xml.Linq.XAttribute("USER_UPDATE", p_user),
                          new System.Xml.Linq.XAttribute("TERMINAL_UPDATE", utilform.Obtenerip().ToString()),
                          new System.Xml.Linq.XAttribute("COMMENTS", TXTCOMENTARIO.Text.ToUpper().ToString()),
                          new System.Xml.Linq.XAttribute("flag", "I"),
                          new System.Xml.Linq.XAttribute("AISV", strAisv),
                          new System.Xml.Linq.XAttribute("VALOR_MINIMO", txtMontoMinimo.Text.ToString())
                          )));

                    WPASEPUERTA.SaveLock_Clientes(docXML.ToString());

                    System.Xml.Linq.XDocument docXMLConsulta = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                 new System.Xml.Linq.XElement("GetClientes", new System.Xml.Linq.XElement("GetCliente",
                 new System.Xml.Linq.XAttribute("ID_Cliente", Widem))));
                    //DataSet WRESUTL = new DataSet();
                    //WRESUTL = WPASEPUERTA.GetLockClientesinfo(docXMLConsulta.ToString());
                    //p_result = WRESUTL.Tables[0];
                    //GVRESULT.DataSource = p_result;
                    //GVRESULT.DataBind();

                    try
                    {
                        app_start.bloqueo oBloqueo = new app_start.bloqueo();
                        var detalle = oBloqueo.GetLockClientes(Widem);
                        p_result = detalle;
                        GVRESULT.DataSource = p_result;
                        GVRESULT.DataBind();


                        //envio de correo
                        /***********************************************************************************************************************************************
                        *datos del cliente N4, días de crédito 
                        **********************************************************************************************************************************************/
                        if (this.ChkMailCliente.Checked)
                        {
                            string email_usuario = string.Empty;
                            string email_cliente = string.Empty;
                            string cliente = string.Empty;
                            string mensajes = string.Empty;
                            CultureInfo enUS = new CultureInfo("en-US");
                            NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;

                            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                            email_usuario = ClsUsuario.email;

                            var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, Widem.Trim());
                            if (Cliente.Exitoso)
                            {
                                var ListaCliente = Cliente.Resultado;
                                if (ListaCliente != null)
                                {
                                    cliente = ListaCliente.CLNT_NAME;
                                    email_cliente = ListaCliente.CLNT_FAX_INVC;

                                    Cls_Bil_Parametros_Sap p = new Cls_Bil_Parametros_Sap();
                                    List<Cls_Bil_Parametros_Sap> Parametros = Cls_Bil_Parametros_Sap.Parametros(out mensajes);
                                    if (Parametros != null)
                                    {
                                        var User = Parametros.Where(f => !string.IsNullOrEmpty(f.USER)).FirstOrDefault();
                                        if (User != null)
                                        {
                                            sap_usuario = User.USER;
                                            sap_clave = User.PASSWORD;
                                            sap_valida = User.VALIDACION;
                                        }
                                        if (string.IsNullOrEmpty(sap_usuario) || string.IsNullOrEmpty(sap_clave))
                                        {
                                            sap_usuario = "sap";
                                            sap_clave = "sap";
                                        }
                                    }

                                    if (sap_valida)
                                    {
                                        var WsEstadoDeCuenta = new CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoapClient();
                                        var RespEstadoCta = WsEstadoDeCuenta.SI_Customer_Statement_NAVIS_CGSA(Widem.Trim(), sap_usuario, sap_clave);
                                        if (RespEstadoCta == null)
                                        {
                                            utilform.MessageBox("Error! Ocurrió un error en el WEBSERVICE SAP - OBJETO NULO, por favor comunicar a CGSA...", this);
                                        }
                                        var Nodoerror = RespEstadoCta.Descendants("ERROR").FirstOrDefault();
                                        if (Nodoerror != null)
                                        {
                                            string MsgError = string.Format("Error! Ocurrió un error en la validación de SAP: {0}, por favor comunicar a CGSA...", Nodoerror.Value);
                                            utilform.MessageBox(MsgError, this);
                                        }
                                        else
                                        {
                                            var NodoCab = RespEstadoCta.Descendants("CABECERA").FirstOrDefault();
                                            var Tagsaldo = NodoCab.Element("SALDO");

                                            //monto total = SaldoPendiente
                                            if (Tagsaldo == null || string.IsNullOrEmpty(Tagsaldo.Value) || !decimal.TryParse(Tagsaldo.Value, style, enUS, out SaldoPendiente))
                                            {
                                                SaldoPendiente = 0;
                                            }
                                            //si tiene valor pendiente
                                            if (SaldoPendiente > 0)
                                            {
                                                var TagValorVencido = NodoCab.Element("FACTURAS_VENCIDAS");
                                                var TagValorPendiente = NodoCab.Element("FACTURAS_PENDIENTES");

                                                //facturas vencidas
                                                if (TagValorVencido != null && !string.IsNullOrEmpty(TagValorVencido.Value))
                                                {
                                                    if (!decimal.TryParse(TagValorVencido.Value, out ValorVencido))
                                                    {
                                                        ValorVencido = 0;
                                                    }
                                                }
                                                //facturas por vencer
                                                if (TagValorPendiente != null && !string.IsNullOrEmpty(TagValorPendiente.Value))
                                                {
                                                    if (!decimal.TryParse(TagValorPendiente.Value, out ValorPendiente))
                                                    {
                                                        ValorPendiente = 0;
                                                    }
                                                }

                                                //sp enviar correo
                                                objProcesar.mail_cliente = email_cliente;
                                                objProcesar.SaldoPendiente = SaldoPendiente;
                                                objProcesar.ValorVencido = ValorVencido;
                                                objProcesar.ValorPendiente = ValorPendiente;
                                                objProcesar.Cliente = cliente;

                                                var nProceso = objProcesar.SaveTransaction(out mensajes);
                                                /*fin de nuevo proceso de grabado*/
                                                if (!nProceso.HasValue || nProceso.Value <= 0)
                                                {
                                                    utilform.MessageBox(string.Format("No se pudo realizar el envío de correos:{0}", mensajes), this);

                                                }
                                                else
                                                {
                                                    this.ChkMailCliente.Checked = false;
                                                    // Mensaje.Add(string.Format("Éxito al enviar notificaciones de correos con novedades.."));
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //mensaje de que no existe
                                }
                            }
                        }
                        

                    }
                    catch (Exception EX)
                    {
                        utilform.MessageBox(EX.Message, this);
                    }



                    utilform.MessageBox("Bloqueo de Cliente generado Exitosamente... ", this);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "LOCKCLIENTES", "CMDADD_Click", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                utilform.MessageBox(string.Format("Error Al Bloquear el Cliente - {0} - Codigo de error {1}", ex.Message.ToString(), number), this);
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
                String empresa = GVRESULT.DataKeys[row.RowIndex].Value.ToString();
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


                try
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

                    app_start.bloqueo oBloqueo = new app_start.bloqueo();
                    var detalle = oBloqueo.GetLockClientes(Widem);
                    p_result = detalle;
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "LOCKCLIENTES", "CHKSTATUS_CheckedChanged(1)", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                }


                var currentStatRow = (from currentStat in p_result.AsEnumerable()
                                      where currentStat.Field<string>("SECUENCIAL") ==  id.ToString()
                                      select currentStat).FirstOrDefault();


                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                           new System.Xml.Linq.XElement("LOCK_CLIENTS",
                           new System.Xml.Linq.XElement("FNA_P_LOCK_CLIENTS",
                           new System.Xml.Linq.XAttribute("SECUENCIAL", id),
                           new System.Xml.Linq.XAttribute("CUSTOMER", currentStatRow["CUSTOMER"]),
                           new System.Xml.Linq.XAttribute("STATUS", status),
                           new System.Xml.Linq.XAttribute("USER_UPDATE", p_user),
                           new System.Xml.Linq.XAttribute("TERMINAL_UPDATE", utilform.Obtenerip().ToString()),
                            new System.Xml.Linq.XAttribute("flag", "U")
                           )));

                WPASEPUERTA.SaveLock_Clientes(docXML.ToString());
                //currentStatRow["STATUS"] = CHKSTATUS.Checked;
                //currentStatRow.AcceptChanges();
                //GVRESULT.DataSource = p_result;
                //GVRESULT.DataBind();

                utilform.MessageBox("Registro Actualizado", this);
                CMDBUSCAR_Click(null, null);

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
            DTRESULT = (DataTable)HttpContext.Current.Session["drClienteLiblock"];
            var list = (from currentStat in DTRESULT.Select("EMPRESA like '%" + prefixText + "%'").AsEnumerable()
                        select currentStat.Field<String>("EMPRESA")).ToList();
            string[] prefixTextArray = list.Take(5).ToArray<string>();
            return prefixTextArray;
        }
    }
}