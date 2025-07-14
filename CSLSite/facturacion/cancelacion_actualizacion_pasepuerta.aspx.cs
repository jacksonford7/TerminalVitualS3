using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using System.Web.Script.Services;
using System.Configuration;
using Newtonsoft.Json;
using csl_log;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Collections;

namespace CSLSite.facturacion
{
    public partial class cancelacion_actualizacion_pasepuerta : System.Web.UI.Page
    {
        private DataTable p_gvPase
        {
            get
            {
                return (DataTable)Session["gvPaseCancelar"];
            }
            set
            {
                Session["gvPaseCancelar"] = value;
            }

        }

        private DataTable p_gvContenedor
        {
            get
            {
                return (DataTable)Session["gvDetallePPWebAC"];
            }
            set
            {
                Session["gvDetallePPWebAC"] = value;
            }

        }

        private DataTable p_datospp
        {
            get
            {
                return (DataTable)Session["p_datosppWebAC"];
            }
            set
            {
                Session["p_datosppWebAC"] = value;
            }

        }

        private DataSet p_reportPasePuerta
        {
            get
            {
                return (DataSet)Session["dsPasePuertaAC"];
            }
            set
            {
                Session["dsPasePuertaAC"] = value;
            }

        }

        private DataTable p_drChofer
        {
            get
            {
                return (DataTable)Session["drChoferPPWebAC"];
            }
            set
            {
                Session["drChoferPPWebAC"] = value;
            }

        }

        private DataTable p_drChoferFilter
        {
            get
            {
                return (DataTable)Session["drChoferFilterPPWebCan"];
            }
            set
            {
                Session["drChoferFilterPPWebCan"] = value;
            }

        }

        private DataTable p_drEmpresa
        {
            get
            {
                return (DataTable)Session["drEmpresaPPWebAC"];
            }
            set
            {
                Session["drEmpresaPPWebAC"] = value;
            }

        }

        public String emailClientePPWeb
        {
            get { return (String)Session["emailClientePPWebAC"]; }
            set { Session["emailClientePPWebAC"] = value; }
        }

        private DataTable p_drPlaca
        {
            get
            {
                return (DataTable)Session["drPlacaPPWebAC"];
            }
            set
            {
                Session["drPlacaPPWebAC"] = value;
            }

        }

        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }

                /*PARA PRUEBAS*/
                //this.agencia.Value = "0926860396001"; ;
                this.agencia.Value = user.ruc;
                this.hfRucUser.Value = user.ruc;
                this.emailClientePPWeb = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IniRpt();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "cancelacion_actualizacion_pasepuerta", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                IniPaseWeb();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "cancelacion_actualizacion_pasepuerta", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ActualizaOCancelaPasePuerta();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "cancelacion_actualizacion_pasepuerta", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
        }

        private void ActualizaOCancelaPasePuerta()
        {
            string cContenedor = string.Empty;
            /*CANCELA PASE DE PUERTA*/
            var dtPPWebVal = (from p in p_datospp.AsEnumerable()
                              where p.Field<String>("CANCELAR") == "False" && p.Field<String>("ACTUALIZAR") == "False"
                              select p).AsDataView().ToTable();
            if (dtPPWebVal.Rows.Count == p_datospp.Rows.Count)
            {
                this.Alerta("No tiene datos para procesar.");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                return;
            }

            /*todo: agregado 15-07-2019*/
            int nContador = 0;
            DateTime fechasal = new DateTime();
            CultureInfo enUS = new CultureInfo("en-US");
            bool bError = false;
            string cError = string.Empty;

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                CheckBox chkPase = item.FindControl("chkPase") as CheckBox;
                Label lblCntr = item.FindControl("lblCntr") as Label;
                cContenedor = lblCntr.Text;
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;
                TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;

                if (chkPase.Checked)
                {

                    if (ddlTurno.SelectedItem.Value == "0")
                    {   
                        bError = true;
                        cError = String.Format("Debe seleccionar el turno para el contenedor: {0}", cContenedor);
                        break;
                    } 

                    if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        bError = true;
                        cError =string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text);
                        txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB_"].ToString();
                        break;
                    }

                    if (ddlTurno.SelectedItem.Value != "0")
                    {//ACTUALIZA NUEVOS DATOS
                        p_datospp.Rows[item.ItemIndex]["TURNO"] = ddlTurno.SelectedItem.Value == "0" ? "" : ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[0].Trim();
                        p_datospp.Rows[item.ItemIndex]["ID_TURNO"] = ddlTurno.SelectedItem.Value == "0" ? "" : ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[1].Trim();
                        p_datospp.Rows[item.ItemIndex]["D_TURNO"] = ddlTurno.SelectedItem.Text;
                        p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = fechasal.ToString("MM/dd/yyyy");
                        p_datospp.Rows[item.ItemIndex]["FECHASALIDA"] = fechasal.ToString("yyyy-MM-dd");
                        p_datospp.AcceptChanges();
                    }

                }
            }

            if (bError)
            {
                this.Alerta(cError);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                return;
            }
            /*fin*/
            


            String MotivoCan = "9"; //"9 = CAMBIO DE TURNO" //DrObser vaciones.SelectedValue.ToString();
            var p_user = Page.User.Identity.Name;
            XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                             new XElement("PASEPUERTA", from p in p_datospp.AsEnumerable().AsParallel()
                                             where p.Field<String>("CANCELAR") == "True" || p.Field<String>("ACTUALIZAR") == "True"
                                             select new XElement("VBS_P_PASE_PUERTA",
                                             new XAttribute("ID_PASE", p.Field<Decimal>("ID_PASE").ToString()),
                                             new XAttribute("PASE", p.Field<String>("NUMERO_PASE_N4") == null ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()),
                                             new XAttribute("TIPO_CARGA", p.Field<String>("TIPO_CARGA") == null ? "" : p.Field<String>("TIPO_CARGA").ToString()),
                                             new XAttribute("ESTADO", "CA"),
                                             new XAttribute("USUARIO_ESTADO", p_user),
                                             new XAttribute("FECHA_ESTADO", DateTime.Now.ToString("MM/dd/yyyy HH:mm")),
                                             new XAttribute("MOTIVO_CANCELACION", MotivoCan),
                                             new XAttribute("flag", "U"),
                                             new XAttribute("RESERVA", p.Field<Boolean>("RESERVA") == Boolean.Parse("false") ? 0 : 1))));

            String[] docXMLN4 = (from p in p_datospp.AsEnumerable().AsParallel()
                                 where (p.Field<String>("CANCELAR") == "True" || p.Field<String>("ACTUALIZAR") == "True") && p.Field<String>("TIPO_CARGA").Equals("CNTR")
                                 select new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                 new XElement("gate",
                                 new XElement("cancel-appointment",
                                 new XElement("appointments",
                                 new XElement("appointment",
                                 new XAttribute("appointment-nbr", (String.IsNullOrEmpty(p.Field<String>("NUMERO_PASE_N4").ToString()) == true ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()))
                                 ))))).ToString()).ToArray();

            String[] docXMLN4brk = (from p in p_datospp.AsEnumerable().AsParallel()
                                    where (p.Field<String>("CANCELAR") == "True" || p.Field<String>("ACTUALIZAR") == "True") && (p.Field<String>("TIPO_CARGA").Equals("BRBK") || p.Field<String>("TIPO_CARGA").Equals("CFS"))
                                    select new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                    new XElement("groovy",
                                    new XAttribute("class-location", "database"),
                                    new XAttribute("class-name", "CGSADeliveryOrderCancel"),
                                    new XElement("parameters",
                                    new XElement("parameter",
                                    new XAttribute("id", "OrderNbr"),
                                    new XAttribute("value", (String.IsNullOrEmpty(p.Field<String>("NUMERO_PASE_N4").ToString()) == true ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()))),
                                    new XElement("parameter",
                                    new XAttribute("id", "Nota"),
                                    new XAttribute("value", "pruebas")),
                                    new XElement("parameter",
                                    new XAttribute("id", "fecha"),
                                    new XAttribute("value", DateTime.Now.ToString("yyyy-MM-dd HH:mm"))),
                                    new XElement("parameter",
                                    new XAttribute("id", "tipo_carga"),
                                    new XAttribute("value", p.Field<String>("TIPO_CARGA")))
                                    ))).ToString()).ToArray();

            String docXMLICU = null;

            if (((from p in p_datospp.AsEnumerable().AsParallel()
                  where (p.Field<String>("CANCELAR") == "True" || p.Field<String>("ACTUALIZAR") == "True") && p.Field<String>("TIPO_CARGA").Equals("CNTR")
                  select p).Count() > 0))
            {
                docXMLICU = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                   new XElement("icus",
                                       from p in p_datospp.AsEnumerable().AsParallel()
                                       where (p.Field<String>("CANCELAR") == "True" || p.Field<String>("ACTUALIZAR") == "True") && p.Field<String>("TIPO_CARGA").Equals("CNTR")
                                       select new XElement("icu",
                                            new XAttribute("CARGA", p.Field<String>("CONTENEDOR").ToString()),
                                             new XAttribute("USUARIO", (String.IsNullOrEmpty(p_user) == true ? "" : p_user))
                                                   ))).ToString();
            }
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            DataSet ds_report = new DataSet();
            WPASEPUERTA.SaveCancelarPasePuerta(docXML.ToString(), docXMLN4, docXMLN4brk);
            /*
            if (docXMLICU != null)
            {
                // WPASEPUERTA.EXECICU(docXMLICU);
                if (MotivoCan.Equals("3"))
                    WPASEPUERTA.EXECICU_tmp(docXMLICU, 1);
                else
                    if (!MotivoCan.Equals("11"))
                        WPASEPUERTA.EXECICU_tmp(docXMLICU, 2);
            }
            */
            /*CANCELA PASE DE PUERTA*/

            /*GENERA NUEVO PASE DE PUERTA*/
            docXML = new XDocument();
            docXML = new System.Xml.Linq.XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement("PASEPUERTA", from p in p_datospp.AsEnumerable().AsParallel()
                                       where p.Field<String>("ACTUALIZAR") == "True"
                                       select new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA",
                                       new System.Xml.Linq.XAttribute("ID_CARGA", p.Field<Int64>("GKEY").ToString() == null ? "" : p.Field<Int64>("GKEY").ToString()),
                                       new System.Xml.Linq.XAttribute("ESTADO", "GN"),
                                       new System.Xml.Linq.XAttribute("FECHA_EXPIRACION", p.Field<String>("FECHA_AUT_PPWEB")),
                                       new System.Xml.Linq.XAttribute("ID_PLACA", p.Field<String>("PLACA") == null ? "" : p.Field<String>("PLACA").ToString().ToUpper()),
                                       new System.Xml.Linq.XAttribute("ID_CHOFER", p.Field<String>("CHOFER") == null ? "" : p.Field<String>("CHOFER").ToString()),
                                       new System.Xml.Linq.XAttribute("ID_EMPRESA", p.Field<String>("CIATRANS").ToString()),
                                       new System.Xml.Linq.XAttribute("CANTIDAD_CARGA", ""),
                                       new System.Xml.Linq.XAttribute("USUARIO_REGISTRO", p_user),
                                       new System.Xml.Linq.XAttribute("FECHA_REGISTRO", DateTime.Now.ToString("MM/dd/yyyy")),
                                       new System.Xml.Linq.XAttribute("USUARIO_ESTADO", p_user),
                                       new System.Xml.Linq.XAttribute("FECHA_ESTADO", DateTime.Now.ToString("MM/dd/yyyy")),
                                       new System.Xml.Linq.XAttribute("ID_RESERVA", ""),
                                       new System.Xml.Linq.XAttribute("ID_PLAN", p.Field<Int64>("TURNO").ToString()),
                                       new System.Xml.Linq.XAttribute("ID_PLAN_SECUENCIA", p.Field<Int32>("ID_TURNO").ToString()),
                                       new System.Xml.Linq.XAttribute("TIPO_CARGA", "CNTR"),
                                       new System.Xml.Linq.XAttribute("CONTENEDOR", p.Field<String>("CONTENEDOR").ToString()),
                                       new System.Xml.Linq.XAttribute("flag", "I"))));

            String[] docXMLN4_ = (from p in p_datospp.AsEnumerable().AsParallel()
                                 where p.Field<String>("ACTUALIZAR") == "True"
                                 select new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                 new System.Xml.Linq.XElement("gate", new System.Xml.Linq.XElement("create-appointment",
                                 new System.Xml.Linq.XElement("appointment-date", p.Field<String>("FECHASALIDA")),
                                 new System.Xml.Linq.XElement("gate-id", "CONTENEDORES"),
                                 new System.Xml.Linq.XElement("driver", new System.Xml.Linq.XAttribute("card-id", "")),
                                 new System.Xml.Linq.XElement("truck", new System.Xml.Linq.XAttribute("license-nbr", "")),
                                 new System.Xml.Linq.XElement("tran-type", "DI"),
                                 new System.Xml.Linq.XElement("container", new System.Xml.Linq.XAttribute("eqid", (p.Field<String>("CONTENEDOR").ToString())))
                                     ))).ToString()).ToArray();

            String[] docXMLN4DAI = (from p in p_datospp.AsEnumerable().AsParallel()
                                    where p.Field<String>("ACTUALIZAR") == "True"
                                    select new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                    new System.Xml.Linq.XElement("icu", new System.Xml.Linq.XElement("units",
                                    new System.Xml.Linq.XElement("unit-identity", new System.Xml.Linq.XAttribute("id", p.Field<String>("CONTENEDOR")),
                                    new System.Xml.Linq.XAttribute("type", "CONTAINERIZED"),
                                    new System.Xml.Linq.XElement("carrier",
                                    new System.Xml.Linq.XAttribute("direction", "IB"),
                                    new System.Xml.Linq.XAttribute("mode", "VESSEL"),
                                    new System.Xml.Linq.XAttribute("id", p.Field<String>("REFERENCIA"))
                                    ))),
                                    new System.Xml.Linq.XElement("properties",
                                    new System.Xml.Linq.XElement("property",
                                    new System.Xml.Linq.XAttribute("tag", "UnitFlexString01"),
                                    new System.Xml.Linq.XAttribute("value", p.Field<String>("DOCUMENTO"))))
                                        )).ToString()).ToArray();

            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            WPASEPUERTA.SaveDAI(docXMLN4DAI);

            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            ds_report = new DataSet();
            ds_report = WPASEPUERTA.SavePasePuerta(docXML.ToString(), docXMLN4_, "CNTR");
            if (ds_report == null || ds_report.Tables[0].Rows.Count < 0)
            {
                p_reportPasePuerta = null;
                string sdocXMLN4 = string.Empty;
                foreach (var item in docXMLN4_)
                {
                    sdocXMLN4 += item;
                }
                var number = "ds_report.Tables['DTError'], Metodo: WPASEPUERTA.SavePasePuerta(" + docXML.ToString() + ", " + sdocXMLN4 + ", " + "'CNTR')";
                throw new System.ArgumentException(number, "original");
            }
            /*GENERA NUEVO PASE DE PUERTA*/

            var dtPPWeb = (from p in p_datospp.AsEnumerable()
                           where p.Field<String>("CANCELAR") == "True" || p.Field<String>("ACTUALIZAR") == "True"
                           select p).AsDataView().ToTable();

            StringWriter xmlPPWeb = new StringWriter();
            dtPPWeb.TableName = "PaseWeb";
            dtPPWeb.WriteXml(xmlPPWeb);

            StringWriter xmlSN = new StringWriter();
            ds_report.Tables[0].TableName = "SN";
            ds_report.Tables[0].WriteXml(xmlSN);

            string msjerror = string.Empty;
            if (!pasePuerta.ActualizaOCancelaPasePuertaWeb(xmlPPWeb.ToString(), xmlSN.ToString(), Page.User.Identity.Name, out msjerror))
            {
                this.Alerta(msjerror);
                return;
            }
            this.Alerta("e-Pass procesado Exitosamente... ");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            IniPaseWeb();
           /*
            p_datospp = new DataTable();
            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();
            */
        }
        
        private void IniPaseWeb()
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fechasal = new DateTime();
            if (!string.IsNullOrEmpty(txtfecsal.Text))
            {
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                {
                    this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text));
                    txtfecsal.Focus();
                    return;
                }
            }

            p_datospp = new DataTable();
            p_datospp = pasePuerta.GetDatosActualizaCancelaPaseWeb(this.agencia.Value, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), txtcntr.Text.Trim(), txtfecsal.Text != "" ? fechasal.ToString("yyyy-MM-dd") : txtfecsal.Text.Trim());
            if (p_datospp.Rows.Count == 0)
            {
                this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                return;
            }

            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();

            p_datospp.Columns.Add("ACTUALIZAR");
            p_datospp.Columns.Add("CANCELAR");
            p_datospp.Columns.Add("TIPO");
            p_datospp.Columns.Add("FECHA_AUT_PPWEB");
            p_datospp.Columns.Add("FECHASALIDA");
            //IniDsEmpresaLoad();
            IniDsEmpresa();
            IniDsChofer("");
           
            IniDsPlaca("");

            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            var docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                     new XElement("PASEPUERTA",
                       new XElement("ConsultaCancelarPN",
                 new XAttribute("MRN", String.IsNullOrEmpty(txtmrn.Text) == true ? "" : txtmrn.Text),
                 new XAttribute("MSN", String.IsNullOrEmpty(txtmsn.Text) == true ? "" : txtmsn.Text),
                 new XAttribute("HSN", String.IsNullOrEmpty(txthsn.Text) == true ? "" : txthsn.Text),
                 new XAttribute("PN", String.IsNullOrEmpty("") == true ? "" : ""),
                 new XAttribute("REFERENCIA", String.IsNullOrEmpty("") == true ? "" : ""),
                 new XAttribute("CONTENEDOR", String.IsNullOrEmpty("") == true ? "" : "")

                 )));
            var dsRetorno = WPASEPUERTA.GetPNinfo(docXML.ToString());

           
            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;
                LinkButton lnkImprimir = item.FindControl("lnkImprimir") as LinkButton;
                string url = "../facturacion/impresion-pase-de-puerta?opcion=reimpresion&carga=" + p_datospp.Rows[item.ItemIndex]["CONTENEDOR"].ToString() + "&pase=" + p_datospp.Rows[item.ItemIndex]["NUMERO_PASE_N4"].ToString() + "&tipo=" + "CNTR";
                lnkImprimir.Attributes.Add("onClick", "JavaScript: window.open('" + url + "','Reimpresion Pase de Puerta','width=700,height=700')");
                p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = "False";
                p_datospp.Rows[item.ItemIndex]["CANCELAR"] = "False";

                for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
                {

                        if (dsRetorno.Tables[0].Rows[i]["CARGA"].ToString() == p_datospp.Rows[item.ItemIndex]["CONTENEDOR"].ToString())
                        {
                            
                            txtfecsalpp.Text = Convert.ToDateTime(dsRetorno.Tables[0].Rows[i]["FECHA_EXPIRACION"]).ToString("dd/MM/yyyy");
                        }
                   
                }

               
            }

            IniTurno();

            lblTotCntr.Text = "Tot. Contenedores: " + p_datospp.Rows.Count.ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
        }

        private void IniTurno()
        {
            XmlDocument docXmlTurno = new XmlDocument();
            DataSet dsRetornoturno = new DataSet();
            XmlElement elemturno;
            docXmlTurno.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><N4/>");
            elemturno = docXmlTurno.CreateElement("Turnos");

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                Label lblCntr = item.FindControl("lblCntr") as Label;
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;


                CultureInfo enUS = new CultureInfo("en-US");
                DateTime dfechaini;
                if (!DateTime.TryParseExact(lblFecAutPPWeb.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                {
                }

                elemturno.SetAttribute("Container", lblCntr.Text);
                elemturno.SetAttribute("Fecha", dfechaini.ToString("MM/dd/yyyy"));
                elemturno.SetAttribute("Consecutivo", p_datospp.Rows[item.ItemIndex]["GKEY"].ToString());
                docXmlTurno.DocumentElement.AppendChild(elemturno);
                dsRetornoturno = WPASEPUERTA.GetTurnoinfo(docXmlTurno.OuterXml, Page.User.Identity.Name);
                var dsresult = dsRetornoturno.Tables[0].DefaultView.ToTable();//.Rows[0]["TURNO"] = "* ELIJA";

                if (p_datospp.Rows[item.ItemIndex]["TIPO_CNTR"].ToString() == "RF")
                {
                    var dtTurnos = (from p in dsresult.AsDataView().ToTable().AsEnumerable()
                                    select p).AsDataView().ToTable();

                    StringWriter xmlTurnos = new StringWriter();
                    dtTurnos.TableName = "Turnos";
                    dtTurnos.WriteXml(xmlTurnos);

                    DateTime dfechasalida = new DateTime();
                    if (!DateTime.TryParseExact(lblFecAutPPWeb.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out dfechasalida))
                    {
                    }

                    //var dthorasrf = pasePuerta.GetTurnosInfoReefer(xmlTurnos.ToString(), Convert.ToDateTime(dfechasalida).ToString("yyyy-MM-dd HH:mm") );
                    //ddlTurno.DataSource = dthorasrf.DefaultView.ToTable().AsDataView();

                    /*nuevo*/
                    var dthorasrf = pasePuerta.GetTurnosInfoReeferContenedor(Convert.ToInt32(p_datospp.Rows[item.ItemIndex]["ID_PPWEB"]), xmlTurnos.ToString(), Convert.ToDateTime(dfechasalida).ToString("yyyy-MM-dd"));
                    ddlTurno.DataSource = dthorasrf.DefaultView.ToTable().AsDataView();
                }
                else
                {
                    ddlTurno.DataSource = dsresult.AsDataView();
                }
                dsresult.Rows[0]["TURNO"] = "* Seleccione *";
                ddlTurno.DataTextField = "TURNO";
                ddlTurno.DataValueField = "IDTURNO";
                ddlTurno.DataBind();
            }
        }

        private void IniRpt()
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("CARGA");
            dtNew.Columns.Add("CONTENEDOR");
            dtNew.Columns.Add("FECHA_AUT_PPWEB");
            tablePaginationPPWeb.DataSource = dtNew;
            tablePaginationPPWeb.DataBind();
            lblTotCntr.Text = "";
        }

        private void IniDsChofer(string empresa)
        {
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            var dtRetorno = pasePuerta.GetChoferinfo(empresa);
            /*
            dsRetorno = WPASEPUERTA.GetChoferinfo();
            for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
            {
                String value = dsRetorno.Tables[0].Rows[i]["CHOFER"].ToString();
                Char delimiter = '-';
                List<string> substringchf = value.Split(delimiter).ToList();
                string chofer = string.Empty;
                for (int l = 0; l < substringchf.Count; l++)
                {
                    if (l == 0)
                    {
                        chofer = substringchf[l + 1].Trim();
                    }
                    else
                    {
                        if ((l + 1) < substringchf.Count)
                        {
                            chofer = chofer + " - " + substringchf[l + 1].Trim();
                        }
                    }
                }
                value = chofer;
                substringchf = value.Split(delimiter).ToList();
                dsRetorno.Tables[0].Rows[i]["CHOFER"] = substringchf[1].Trim();
            }
            */
            //dtRetorno.Rows.Add("0", "* ELIJA *");
            /*
            if (dtRetorno.Rows.Count > 0)
            {
                DataView myDataView = dtRetorno.DefaultView;
                myDataView.Sort = "CHOFER ASC";

                p_drChofer = myDataView.ToTable();
            }
            */
            p_drChofer = dtRetorno;
            /*
            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                DropDownList ddlChofer = item.FindControl("ddlChofer") as DropDownList;
                ddlChofer.DataSource = myDataView;
                ddlChofer.DataTextField = "CHOFER";
                ddlChofer.DataValueField = "IDCHOFER";
                ddlChofer.DataBind();
            }
            */
        }

        //private void IniDsChofer(string empresa)
        //{
        //    DataSet dsRetorno = new DataSet();
        //    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
        //    var dtRetorno = pasePuerta.GetChoferinfo(empresa);
        //    /*
        //    dsRetorno = WPASEPUERTA.GetChoferinfo();
        //    for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
        //    {
        //        String value = dsRetorno.Tables[0].Rows[i]["CHOFER"].ToString();
        //        Char delimiter = '-';
        //        List<string> substringchf = value.Split(delimiter).ToList();
        //        string chofer = string.Empty;
        //        for (int l = 0; l < substringchf.Count; l++)
        //        {
        //            if (l == 0)
        //            {
        //                chofer = substringchf[l + 1].Trim();
        //            }
        //            else
        //            {
        //                if ((l + 1) < substringchf.Count)
        //                {
        //                    chofer = chofer + " - " + substringchf[l + 1].Trim();
        //                }
        //            }
        //        }
        //        value = chofer;
        //        substringchf = value.Split(delimiter).ToList();
        //        dsRetorno.Tables[0].Rows[i]["CHOFER"] = substringchf[1].Trim();
        //    }
        //    */
        //    dtRetorno.Rows.Add("0", "* Seleccione *");
        //    DataView myDataView = dtRetorno.DefaultView;
        //    myDataView.Sort = "CHOFER ASC";

        //    p_drChofer = myDataView.ToTable();

        //    foreach (RepeaterItem item in tablePaginationPPWeb.Items)
        //    {
        //        DropDownList ddlChofer = item.FindControl("ddlChofer") as DropDownList;
        //        ddlChofer.DataSource = myDataView;
        //        ddlChofer.DataTextField = "CHOFER";
        //        ddlChofer.DataValueField = "IDCHOFER";
        //        ddlChofer.DataBind();
        //    }
        //}
        
        //private void IniDsEmpresa()
        //{
        //    XmlDocument docXml = new XmlDocument();
        //    XmlElement elem;
        //    DataSet dsRetorno = new DataSet();
        //    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

        //    docXml.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><PASEPUERTA/>");
        //    elem = docXml.CreateElement("GetEmpresa");
        //    elem.SetAttribute("CLNT_TYPE", "TRCO");
        //    elem.SetAttribute("CLNT_ACTIVE", "Y");

        //    docXml.DocumentElement.AppendChild(elem);
        //    dsRetorno = WPASEPUERTA.GetEmpresainfo(docXml.OuterXml);
        //    /*
        //    for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
        //    {
        //        String value = dsRetorno.Tables[0].Rows[i]["EMPRESA"].ToString();
        //        Char delimiter = '-';
        //        List<string> substringemp = value.Split(delimiter).ToList();
        //        string empresa = string.Empty;
        //        for (int l = 0; l < substringemp.Count; l++)
        //        {
        //            if (l == 0)
        //            {
        //                empresa = substringemp[l + 1].Trim();
        //            }
        //            else
        //            {
        //                if ((l + 1) < substringemp.Count)
        //                {
        //                    empresa = empresa + " - " + substringemp[l + 1].Trim();
        //                }
        //            }
        //        }
        //        dsRetorno.Tables[0].Rows[i]["EMPRESA"] = empresa;
        //    }
        //    */
        //    dsRetorno.Tables[0].Rows.Add("0", "* Seleccione *");
        //    DataView myDataView = dsRetorno.Tables[0].DefaultView;
        //    myDataView.Sort = "IDEMPRESA ASC";

        //    p_drEmpresa = myDataView.ToTable();

        //    foreach (RepeaterItem item in tablePaginationPPWeb.Items)
        //    {
        //        DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
        //        Label lblciatrans = item.FindControl("lblciatrans") as Label;
        //        if (!string.IsNullOrEmpty(lblciatrans.Text))
        //        {
        //            lblciatrans.Text = (from row in p_drEmpresa.AsEnumerable()
        //                                where row.Field<String>("IDEMPRESA") == lblciatrans.Text
        //                                select row).AsDataView().ToTable().Rows[0]["EMPRESA"].ToString();
        //        }
        //        ddlEmpresa.DataSource = myDataView;
        //        ddlEmpresa.DataTextField = "EMPRESA";
        //        ddlEmpresa.DataValueField = "IDEMPRESA";
        //        ddlEmpresa.DataBind();
        //    }
        //}
        
        private void IniDsEmpresaLoad()
        {
            DataTable dtRetorno = new DataTable();
            dtRetorno.Columns.Add("IDEMPRESA");
            dtRetorno.Columns.Add("EMPRESA");
            dtRetorno.Rows.Add("0", "* Seleccione *");

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                ddlEmpresa.DataSource = dtRetorno.DefaultView;
                ddlEmpresa.DataTextField = "EMPRESA";
                ddlEmpresa.DataValueField = "IDEMPRESA";
                ddlEmpresa.DataBind();
            }
        }

        private void IniDsEmpresa()
        {



            XmlDocument docXml = new XmlDocument();
            XmlElement elem;
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            docXml.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><PASEPUERTA/>");
            elem = docXml.CreateElement("GetEmpresa");
            elem.SetAttribute("CLNT_TYPE", "TRCO");
            elem.SetAttribute("CLNT_ACTIVE", "Y");


            docXml.DocumentElement.AppendChild(elem);
            dsRetorno = WPASEPUERTA.GetEmpresainfo(docXml.OuterXml);
            p_drEmpresa = dsRetorno.Tables[0];

        }

        //private void IniDsEmpresa()
        //{
        //    XmlDocument docXml = new XmlDocument();
        //    XmlElement elem;
        //    DataSet dsRetorno = new DataSet();
        //    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
        //    //var dtRetorno = pasePuerta.GetEmpresainfo();

        //    docXml.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><PASEPUERTA/>");
        //    elem = docXml.CreateElement("GetEmpresa");
        //    elem.SetAttribute("CLNT_TYPE", "TRCO");
        //    elem.SetAttribute("CLNT_ACTIVE", "Y");

        //    docXml.DocumentElement.AppendChild(elem);
        //    dsRetorno = WPASEPUERTA.GetEmpresainfo(docXml.OuterXml);

        //    /*
        //    for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
        //    {
        //        String value = dsRetorno.Tables[0].Rows[i]["EMPRESA"].ToString();
        //        Char delimiter = '-';
        //        List<string> substringemp = value.Split(delimiter).ToList();
        //        string empresa = string.Empty;
        //        for (int l = 0;  l < substringemp.Count;  l++)
        //        {
        //            if (l == 0)
        //            {
        //                empresa = substringemp[l + 1].Trim();
        //            }
        //            else
        //            {
        //                if ((l + 1) < substringemp.Count)
        //                {
        //                    empresa = empresa + " - " + substringemp[l + 1].Trim();
        //                }
        //            }
        //        }
        //        dsRetorno.Tables[0].Rows[i]["EMPRESA"] = empresa;
        //    }
        //    */
        //    dsRetorno.Tables[0].Rows.Add("0", "* Seleccione *");
        //    //dtRetorno.Rows.Add("0", "* Seleccione *", "", "", "0", "", "", "");
        //    DataView myDataView = //dtRetorno.DefaultView;
        //    dsRetorno.Tables[0].DefaultView;
        //    myDataView.Sort = "IDEMPRESA ASC";

        //    /*
        //    for (int i2 = 0; i2 < myDataView.ToTable().Rows.Count; i2++)
        //    {
        //        myDataView.ToTable().Rows[i2]["EMPRESA"] = myDataView.ToTable().Rows[i2]["IDEMPRESA"] + " - " + myDataView.ToTable().Rows[i2]["EMPRESA"]; 
        //    }
        //    */

        //    p_drEmpresa = myDataView.ToTable();

        //    foreach (RepeaterItem item in tablePaginationPPWeb.Items)
        //    {
        //        DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
        //        Label lblciatrans = item.FindControl("lblciatrans") as Label;
        //        if (!string.IsNullOrEmpty(lblciatrans.Text))
        //        {
        //            lblciatrans.Text = (from row in p_drEmpresa.AsEnumerable()
        //                                where row.Field<String>("IDEMPRESA") == lblciatrans.Text
        //                                select row).AsDataView().ToTable().Rows[0]["EMPRESA"].ToString();
        //        }
        //    }
        //}

        private void IniDatosPPWeb()
        {
            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                Label lblCarga = item.FindControl("lblCarga") as Label;
                Label lblCntr = item.FindControl("lblCntr") as Label;
                Label lblReserva = item.FindControl("lblReserva") as Label;

                String value = lblCarga.Text;
                Char delimiter = '-';
                List<string> substringcarga = value.Split(delimiter).ToList();

                DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));
                XDocument docXMLEXPO = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));
                DataSet dsRetorno = new DataSet();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("PASEPUERTA",
                              new XElement("ConsultaCarga",
                        new XAttribute("MRN", substringcarga[0].Trim()),
                        new XAttribute("MSN", substringcarga[1].Trim()),
                        new XAttribute("HSN", substringcarga[2].Trim()),
                        new XAttribute("Type", "IMPO"),
                        new XAttribute("REFERENCIA", ""),
                        new XAttribute("CONTENEDOR", "")
                                             )));
                dsRetorno = WPASEPUERTA.GetContainerN4info(docXML.ToString(), docXMLEXPO.ToString());
                p_gvContenedor = dsRetorno.Tables[0];
                var result = (from hi in p_gvContenedor.AsEnumerable()
                              where hi.Field<String>("CONTENEDOR") == lblCntr.Text
                              select hi);
                lblReserva.Text = result.AsDataView().ToTable().Rows[item.ItemIndex]["RESERVA"].ToString();
            }
        }

        private void IniDsPlaca(string empresa)
        {
            /*
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            dsRetorno = WPASEPUERTA.GetPlacainfo();
            p_drPlaca = dsRetorno.Tables[0];
            */
            p_drPlaca = pasePuerta.GetPlacainfo(empresa);
        }

        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label lblCntr = (Label)item.FindControl("lblCntr");
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;
                Label lbldturno = item.FindControl("lbldturno") as Label;
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
               
                CheckBox chkCanPase = item.FindControl("chkCanPase") as CheckBox;

                TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();

                if (chkPase.Checked)
                {
                    chkCanPase.Checked = false;
                   
                    if (string.IsNullOrEmpty(txtfecsalpp.Text))
                    {
                        this.Alerta("Seleccione la Fecha..");
                        txtfecsalpp.Focus();
                        return;
                    }

                    if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text));
                        txtfecsal.Focus();
                        txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB_"].ToString();
                        return;
                    }

                    XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));
                    DataSet dsRetorno = new DataSet();
                    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                    docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                        new XElement("PASEPUERTA",
                                          new XElement("ConsultaCancelarPN",
                                    new XAttribute("MRN", String.IsNullOrEmpty(txtmrn.Text) == true ? "" : txtmrn.Text),
                                    new XAttribute("MSN", String.IsNullOrEmpty(txtmsn.Text) == true ? "" : txtmsn.Text),
                                    new XAttribute("HSN", String.IsNullOrEmpty(txthsn.Text) == true ? "" : txthsn.Text),
                                    new XAttribute("PN", String.IsNullOrEmpty("") == true ? "" : ""),
                                    new XAttribute("REFERENCIA", String.IsNullOrEmpty("") == true ? "" : ""),
                                    new XAttribute("CONTENEDOR", String.IsNullOrEmpty("") == true ? "" : "")

                                    )));
                    dsRetorno = WPASEPUERTA.GetPNinfo(docXML.ToString());
                    var fechaultimoase = "";
                    for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
                    {
                        if (p_datospp.Rows[item.ItemIndex]["CONTENEDOR"].ToString() == dsRetorno.Tables[0].Rows[i]["CARGA"].ToString())
                        {
                            //p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = Convert.ToDateTime(txtfecsalpp.Text.Trim()).ToString("yyyy-MM-dd");
                            fechaultimoase = /*Convert.ToDateTime(*/dsRetorno.Tables[0].Rows[i]["FECHA_EXPIRACION"].ToString();//).ToString("yyyy-MM-dd");
                        }
                    }

                    var dtvalfecha = pasePuerta.GetValFechaSalidaCntr(fechaultimoase.Trim(), p_datospp.Rows[item.ItemIndex]["FACTURA"].ToString(), txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblCntr.Text);
                    if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                    {
                        if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                        {
                            this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                            txtfecsal.Focus();
                            txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB_"].ToString();
                            return;
                        }
                    }

                    if (ddlTurno.SelectedItem.Value != "0")
                    {
                        var v_turno = pasePuerta.GetValTurno(Convert.ToDateTime(fechaultimoase.Trim()).ToString("yyyy-MM-dd"), lbldturno.Text.Trim());
                        if (v_turno.Rows[0]["V_TURNO"].ToString() == "1")
                        {
                            var v_msg = v_turno.Rows[0]["MENSAJE"].ToString();
                            this.Alerta(v_msg);
                            chkPase.Checked = false;
                            return;
                        }
                    }
                    TextBox TxtGEmpresa = item.FindControl("TxtGEmpresa") as TextBox;
                    //DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                    if (string.IsNullOrEmpty(TxtGEmpresa.Text))//ddlEmpresa.SelectedItem.Value == "0")
                    {
                        this.Alerta("Elija una Cia. Trans para el Contenedor: " + lblCntr.Text);
                        chkPase.Checked = false;
                        return;
                    }

                    DataTable DTRESULT = new DataTable();
                    DTRESULT = (DataTable)HttpContext.Current.Session["drEmpresaPPWebAC"];
                    var valemp = (from currentStat in DTRESULT.Select("EMPRESA = '" + TxtGEmpresa.Text.Trim() + "'").AsEnumerable()
                                  select currentStat.Field<String>("EMPRESA")).ToList().Take(5);
                    string[] aEmp = valemp.ToArray<string>();
                    if (aEmp.Count() > 0)
                    {
                        TxtGEmpresa.Text = aEmp[0].ToString();
                    }
                    else
                    {
                        this.Alerta("La Cia. Trans: " + TxtGEmpresa.Text + " no es valida.");
                        chkPase.Checked = false;
                        return;
                    }

                    TextBox TxtGChofer = item.FindControl("TxtGChofer") as TextBox;
                    DataTable DTRESULT2 = new DataTable();
                    DTRESULT2 = (DataTable)HttpContext.Current.Session["drChoferPPWebAC"];
                    if (DTRESULT2.Rows.Count > 0)
                    {
                        var valcho = (from currentStat in DTRESULT2.Select("CHOFER = '" + TxtGChofer.Text.Trim() + "'").AsEnumerable()
                                      select currentStat.Field<String>("CHOFER")).ToList().Take(5);
                        string[] aChof = valcho.ToArray<string>();
                        if (aChof.Count() > 0)
                        {
                            TxtGChofer.Text = aChof[0].ToString();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(TxtGChofer.Text))
                            {
                                this.Alerta("El Chofer: " + TxtGChofer.Text + " no es valido.");
                                chkPase.Checked = false;
                                return;
                            }
                        }
                    }
                    //DropDownList ddlChofer = item.FindControl("ddlChofer") as DropDownList;
                    TextBox txtPlaca = item.FindControl("txtPlaca") as TextBox;
                    if (!string.IsNullOrEmpty(TxtGChofer.Text)) //ddlChofer.SelectedItem.Value != "0")
                    {
                        if (string.IsNullOrEmpty(txtPlaca.Text))
                        {
                            this.Alerta("Escriba una Placa para el Contenedor: " + lblCntr.Text);
                            chkPase.Checked = false;
                            return;
                        }
                        else
                        {
                           
                            String value_ = TxtGEmpresa.Text.Trim();
                            Char delimiter_ = '-';
                            List<string> substringemp = value_.Split(delimiter_).ToList();
                            //IniDsPlaca(ddlEmpresa.SelectedItem.Value);
                            var wPlaca = (from row in p_drPlaca.AsEnumerable()
                                          where row.Field<String>("EMPRESA") == substringemp[0].Trim() && row.Field<String>("PLACA") != null && row.Field<String>("PLACA").Trim().Equals(txtPlaca.Text.ToString().Trim().ToUpper())
                                          select row.Field<String>("PLACA")).Count();

                            if ((int)wPlaca <= 0)
                            {
                                this.Alerta("La Placa no es valida para el Contenedor: " + lblCntr.Text);
                                chkPase.Checked = false;
                                return;
                            }
                            String value = TxtGChofer.Text.Trim();
                            Char delimiter = '-';
                            List<string> substringchof = value.Split(delimiter).ToList();
                            p_datospp.Rows[item.ItemIndex]["CHOFER"] = substringchof[0].Trim();
                            p_datospp.Rows[item.ItemIndex]["PLACA"] = txtPlaca.Text.Trim();
                        }
                    }
                    
                    if (ddlTurno.SelectedItem.Value != "0")
                    {
                        p_datospp.Rows[item.ItemIndex]["TURNO"] = ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[0].Trim();
                        p_datospp.Rows[item.ItemIndex]["ID_TURNO"] = ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[1].Trim();
                        p_datospp.Rows[item.ItemIndex]["D_TURNO"] = ddlTurno.SelectedItem.Text;
                    }
                  
                    if (!string.IsNullOrEmpty(TxtGEmpresa.Text.Trim()))//ddlEmpresa.SelectedItem.Value != "0")
                    {
                        String value = TxtGEmpresa.Text.Trim();
                        Char delimiter = '-';
                        List<string> substringemp = value.Split(delimiter).ToList();
                        p_datospp.Rows[item.ItemIndex]["CIATRANS"] = substringemp[0].Trim() == "0" ? "" : substringemp[0].Trim();
                    }
                    p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = chkPase.Checked.ToString();
                    p_datospp.Rows[item.ItemIndex]["CANCELAR"] = chkCanPase.Checked.ToString();
                    if (chkPase.Checked)
                    {
                        p_datospp.Rows[item.ItemIndex]["TIPO"] = "A";
                    }
                    else
                    {
                        p_datospp.Rows[item.ItemIndex]["TIPO"] = "";
                    }

                    p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = fechasal.ToString("MM/dd/yyyy");
                    p_datospp.Rows[item.ItemIndex]["FECHASALIDA"] = fechasal.ToString("yyyy-MM-dd");

                    p_datospp.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "detalle_can_act_pasepuerta", "chkPase_CheckedChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void chkCanPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkCanPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkCanPase.NamingContainer;
                CheckBox chkPase = item.FindControl("chkPase") as CheckBox;
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                //DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                //DropDownList ddlChofer = item.FindControl("ddlChofer") as DropDownList;
                TextBox txtPlaca = item.FindControl("txtPlaca") as TextBox;
                TextBox TxtGEmpresa = item.FindControl("TxtGEmpresa") as TextBox;
                TextBox TxtGChofer = item.FindControl("TxtGChofer") as TextBox;
                ddlTurno.SelectedValue = "0";
                //ddlEmpresa.SelectedValue = "0";
                //ddlChofer.SelectedValue = "0";
                txtPlaca.Text = "";
                TxtGChofer.Text = "";
                TxtGEmpresa.Text = "";
                chkPase.Checked = false;
                p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = chkPase.Checked.ToString();
                p_datospp.Rows[item.ItemIndex]["CANCELAR"] = chkCanPase.Checked.ToString();
                if (chkCanPase.Checked)
                {
                    p_datospp.Rows[item.ItemIndex]["TIPO"] = "C";
                }
                else
                {
                    p_datospp.Rows[item.ItemIndex]["TIPO"] = "";
                }
                p_datospp.AcceptChanges();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "detalle_can_act_pasepuerta", "chkPase_CheckedChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlTurno = (DropDownList)sender;
                RepeaterItem item = (RepeaterItem)ddlTurno.NamingContainer;
                ddlTurno.ToolTip = ddlTurno.SelectedItem.Text;
                /*
                if (ddlTurno.SelectedItem.Value != "0")
                {
                    DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                    ddlEmpresa.DataSource = p_drEmpresa.AsDataView();
                    ddlEmpresa.DataTextField = "EMPRESA";
                    ddlEmpresa.DataValueField = "IDEMPRESA";
                    ddlEmpresa.DataBind();
                }
                */
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "detalle_can_act_pasepuerta", "ddlChofer_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlEmpresa = (DropDownList)sender;
                RepeaterItem item = (RepeaterItem)ddlEmpresa.NamingContainer;
                ddlEmpresa.ToolTip = ddlEmpresa.SelectedItem.Text;

                if (ddlEmpresa.SelectedItem.Value != "0")
                {
                    DropDownList ddlChofer = (DropDownList)sender;
                    RepeaterItem item2 = (RepeaterItem)ddlChofer.NamingContainer;
                    ddlChofer = item2.FindControl("ddlChofer") as DropDownList;
                    DataSet dsRetorno = new DataSet();
                    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                    var dtRetorno = pasePuerta.GetChoferinfo(ddlEmpresa.SelectedItem.Value);
                    dtRetorno.Rows.Add("0", "* Seleccione *");
                    DataView myDataView = dtRetorno.DefaultView;
                    myDataView.Sort = "CHOFER ASC";
                    p_drChofer = myDataView.ToTable();
                    ddlChofer.DataSource = myDataView;
                    ddlChofer.DataTextField = "CHOFER";
                    ddlChofer.DataValueField = "IDCHOFER";
                    ddlChofer.DataBind();
                    //IniDsChofer(ddlEmpresa.SelectedItem.Value);
                    //IniDsPlaca(ddlEmpresa.SelectedItem.Value);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "detalle_can_act_pasepuerta", "ddlChofer_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void ddlChofer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlChofer = (DropDownList)sender;
                RepeaterItem item = (RepeaterItem)ddlChofer.NamingContainer;
                CheckBox chkPase = item.FindControl("chkPase") as CheckBox;
                if (ddlChofer.SelectedItem.Value != "0")
                {
                    chkPase.Checked = false;
                }
                ddlChofer.ToolTip = ddlChofer.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "detalle_can_act_pasepuerta", "ddlChofer_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void TxtGEmpresa_TextChanged(object sender, EventArgs e)
        {
            TextBox TxtGEmpresa = (TextBox)sender;
            TxtGEmpresa.ToolTip = TxtGEmpresa.Text.Trim();

            String value = TxtGEmpresa.Text.Trim();
            Char delimiter = '-';
            List<string> substringemp = value.Split(delimiter).ToList();
            IniDsChofer(substringemp[0].Trim());
            IniDsPlaca(substringemp[0].Trim());
        }

        protected void lblFecAutPPWeb_TextChanged(object sender, EventArgs e)
        {
            try
            {//lblFecAutPPWeb
                TextBox chkPase = (TextBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                Label lblCntr = item.FindControl("lblCntr") as Label;
                
                if (!string.IsNullOrEmpty(txtfecsalpp.Text))
                {
                    if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text));
                        txtfecsal.Focus();
                        txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB_"].ToString();
                        return;
                    }
                }
                
                //txtfecsalpp.Text = txtfecsalpp.Text.Replace("/", "-");

                var dtvalfecha = pasePuerta.GetValFechaSalidaCntr(fechasal.ToString("yyyy-MM-dd"), p_datospp.Rows[item.ItemIndex]["FACTURA"].ToString(), txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblCntr.Text);
                if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                {
                    if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                    {
                        this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                        txtfecsal.Focus();
                        txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB_"].ToString();
                        return;
                    }
                }

                //p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = Convert.ToDateTime(txtfecsalpp.Text.Trim()).ToString("yyyy-MM-dd");

                XmlDocument docXmlTurno = new XmlDocument();
                DataSet dsRetornoturno = new DataSet();
                XmlElement elemturno;
                docXmlTurno.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><N4/>");
                elemturno = docXmlTurno.CreateElement("Turnos");
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;
                elemturno.SetAttribute("Container", lblCntr.Text);
                elemturno.SetAttribute("Fecha", fechasal.ToString("MM/dd/yyyy"));
                elemturno.SetAttribute("Consecutivo", p_datospp.Rows[item.ItemIndex]["GKEY"].ToString());
                docXmlTurno.DocumentElement.AppendChild(elemturno);
                dsRetornoturno = WPASEPUERTA.GetTurnoinfo(docXmlTurno.OuterXml, Page.User.Identity.Name);
                var dsresult = dsRetornoturno.Tables[0].DefaultView.ToTable();//.Rows[0]["TURNO"] = "* ELIJA";

                if (p_datospp.Rows[item.ItemIndex]["TIPO_CNTR"].ToString() == "RF")
                {
                    var dtTurnos = (from p in dsresult.AsDataView().ToTable().AsEnumerable()
                                    select p).AsDataView().ToTable();

                    StringWriter xmlTurnos = new StringWriter();
                    dtTurnos.TableName = "Turnos";
                    dtTurnos.WriteXml(xmlTurnos);

                    DateTime fechasalidapp = new DateTime();
                    if (!DateTime.TryParseExact(p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"].ToString(), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechasalidapp))
                    {
                    }
                    //var fechasalidapp = Convert.ToDateTime(p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"].ToString());
                    var fechacambio = fechasal;
                    if (fechacambio.ToString("yyyy-MM-dd") == fechasalidapp.ToString("yyyy-MM-dd"))
                    {
                        //var dthorasrf = pasePuerta.GetTurnosInfoReefer(xmlTurnos.ToString(), Convert.ToDateTime(fechasalidapp).ToString("yyyy-MM-dd HH:mm") );
                        //ddlTurno.DataSource = dthorasrf.DefaultView.ToTable().AsDataView();

                        var dthorasrf = pasePuerta.GetTurnosInfoReeferContenedor(Convert.ToInt32(p_datospp.Rows[item.ItemIndex]["ID_PPWEB"]), xmlTurnos.ToString(), Convert.ToDateTime(fechasalidapp).ToString("yyyy-MM-dd"));
                        ddlTurno.DataSource = dthorasrf.DefaultView.ToTable().AsDataView();

                    }
                    else
                    {
                        DateTime FecAutPPWeb = new DateTime();
                        if (!DateTime.TryParseExact(lblFecAutPPWeb.Text + " 23:59", "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out FecAutPPWeb))
                        {
                        }

                        //var dthorasrf = pasePuerta.GetTurnosInfoReefer(xmlTurnos.ToString(), FecAutPPWeb.ToString("yyyy-MM-dd HH:mm"));
                        //ddlTurno.DataSource = dthorasrf.DefaultView.ToTable().AsDataView();

                        var dthorasrf = pasePuerta.GetTurnosInfoReeferContenedor(Convert.ToInt32(p_datospp.Rows[item.ItemIndex]["ID_PPWEB"]), xmlTurnos.ToString(), FecAutPPWeb.ToString("yyyy-MM-dd"));
                        ddlTurno.DataSource = dthorasrf.DefaultView.ToTable().AsDataView();
                    }
                }
                else
                {
                    ddlTurno.DataSource = dsresult.AsDataView();
                }
                dsresult.Rows[0]["TURNO"] = "* Seleccione *";
                ddlTurno.DataTextField = "TURNO";
                ddlTurno.DataValueField = "IDTURNO";
                ddlTurno.DataBind();

                //IniTurno();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "txtfecsalpp_TextChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        [System.Web.Services.WebMethod]
        public static string[] GetEmpresaList(string prefix)
        {

            WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            XmlDocument docXml = new XmlDocument();
            XmlElement elem;

            DataTable DTRESULT = new DataTable();

            DTRESULT = (DataTable)HttpContext.Current.Session["drEmpresaPPWebAC"];
            if (DTRESULT != null)
            {
                var list = (from currentStat in DTRESULT.Select("EMPRESA like '%" + prefix + "%'").AsEnumerable()
                            select currentStat.Field<String>("EMPRESA")).ToList().Take(5);
                string[] prefixTextArray = list.ToArray<string>();
                return prefixTextArray;
            }
            else
            {
                ArrayList myAL = new ArrayList();
                // Add stuff to the ArrayList.
                string[] myArr = (String[])myAL.ToArray(typeof(string));
                string[] prefixTextArray2 = myArr.ToArray<string>();
                return prefixTextArray2;
            }
            //Return Selected Products
            //return prefixTextArray;
        }

        [System.Web.Services.WebMethod]
        public static string[] GetChoferList(string prefix)
        {

            WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            DataTable DTRESULT = new DataTable();
            DTRESULT = (DataTable)HttpContext.Current.Session["drChoferFilterPPWebCan"];//drChoferPPWeb"];

            if (DTRESULT != null)
            {
                var list = /*(from currentStat in DTRESULT.AsEnumerable()
                        where currentStat.Field<String>("CHOFER") != null && currentStat.Field<String>("CHOFER").Contains(prefixText.ToUpper())
                        select currentStat.Field<String>("IDCHOFER") + " - " + currentStat.Field<String>("CHOFER")).ToList().Take(5);*/

                (from currentStat in DTRESULT.Select("CHOFER like '%" + prefix + "%'").AsEnumerable()
                 select currentStat.Field<String>("CHOFER")).ToList().Take(5);

                string[] prefixTextArray = list.ToArray<string>();
                return prefixTextArray;
            }
            else
            {
                ArrayList myAL = new ArrayList();
                // Add stuff to the ArrayList.
                string[] myArr = (String[])myAL.ToArray(typeof(string));
                string[] prefixTextArray2 = myArr.ToArray<string>();
                return prefixTextArray2;
            }
            //Return Selected Products
        }

        [System.Web.Services.WebMethod]
        public static string GetFilterChoferList(string prefix)
        {
            DataTable DTRESULT = new DataTable();
            DTRESULT = (DataTable)HttpContext.Current.Session["drChoferPPWebAC"];

            /*
             var list = (from currentStat in DTRESULT.Select("EMPRESA like '%" + prefix + "%'").AsEnumerable()
                         select currentStat);
            */

            String value = prefix.Trim();
            Char delimiter = '-';
            List<string> substringemp = value.Split(delimiter).ToList();

            var rucciatrans = substringemp[0].Trim();

            var list = (from hi in DTRESULT.AsEnumerable()
                        where hi.Field<String>("EMPRESA") == rucciatrans
                        select hi);

            DataTable dtResultado = new DataTable();
            dtResultado = list.AsDataView().ToTable();

            if (dtResultado != null)
            {
                if (dtResultado.Rows.Count > 0)
                {
                    HttpContext.Current.Session["drChoferFilterPPWebCan"] = dtResultado;
                }
            }

            return "1";
        }
    }
}