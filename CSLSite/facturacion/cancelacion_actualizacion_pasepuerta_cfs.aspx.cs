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
using System.Reflection;

namespace CSLSite.facturacion
{
    public partial class cancelacion_actualizacion_pasepuerta_cfs : System.Web.UI.Page
    {
        public List<String> listsubitems
        {
            get { return (List<String>)Session["listsubitemscfscan"]; }
            set { Session["listsubitemscfscan"] = value; }
        }

        private DataTable p_drpasepuertabreakbulk
        {
            get
            {
                return (DataTable)Session["drpasepuertabreakbulkcfs"];
            }
            set
            {
                Session["drpasepuertabreakbulkcfs"] = value;
            }

        }

        private DataTable p_dtturnos
        {
            get
            {
                return (DataTable)Session["p_dtturnos"];
            }
            set
            {
                Session["p_dtturnos"] = value;
            }

        }


        private Boolean EstadoPPSinTurno
        {
            get
            {
                return (Boolean)Session["EstadoPPSinTurno"];
            }
            set
            {
                Session["EstadoPPSinTurno"] = value;
            }

        }

        public string lista_subitems
        {
            get { return (string)Session["lista_subitemscfs"]; }
            set { Session["lista_subitemscfs"] = value; }
        }

        private Decimal tot_bultos
        {
            get
            {
                return (Decimal)Session["tot_bultoscfs"];
            }
            set
            {
                Session["tot_bultoscfs"] = value;
            }

        }

        private string sfechasalida
        {
            get
            {
                return (string)Session["sfechasalidacfs"];
            }
            set
            {
                Session["sfechasalidacfs"] = value;
            }

        }

        private DataTable dtHorarios
        {
            get
            {
                return (DataTable)Session["dtHorarios"];
            }
            set
            {
                Session["dtHorarios"] = value;
            }

        }

        private DataTable dtHorariosfull
        {
            get
            {
                return (DataTable)Session["dtHorariosfull"];
            }
            set
            {
                Session["dtHorariosfull"] = value;
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

        private DataTable p_gvBreakBulk
        {
            get
            {
                return (DataTable)Session["p_gvBreakBulkCFSPP"];
            }
            set
            {
                Session["p_gvBreakBulkCFSPP"] = value;
            }

        }

        private DataTable p_gvCfsTemp
        {
            get
            {
                return (DataTable)Session["gvCfstempPP"];
            }
            set
            {
                Session["gvCfstempPP"] = value;
            }

        }

        private DataTable p_gvCfs
        {
            get
            {
                return (DataTable)Session["gvCfsPP"];
            }
            set
            {
                Session["gvCfsPP"] = value;
            }

        }

        private String p_breabulkcarga
        {
            get
            {
                return (String)Session["breabulkcargacfs"];
            }
            set
            {
                Session["breabulkcargacfs"] = value;
            }

        }

    

        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;

        private void IniDsPasePuertaBreakBulk()
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds = new DataSet();
            DataColumn dcID = new DataColumn("ID", typeof(int));
            dcID.AutoIncrement = true;
            dcID.AutoIncrementSeed = 1;
            dcID.AutoIncrementStep = 1;

            dt.Columns.Add(dcID);
            dt.Columns.Add(new DataColumn("CONSECUTIVO", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTPASES", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("EMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDEMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("PLACA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDCHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CARGA", Type.GetType("System.String")));

            ds.Tables.Add(dt);

            p_drpasepuertabreakbulk = ds.Tables[0];

        }

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

               this.agencia.Value = "0991370226001";//
                //this.agencia.Value = user.ruc;
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
                    /*
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechasal = new DateTime();
                    //fechasal = dsRetorno.Tables[0].Rows[i2]["FECHA_EXPIRACION"].ToString();
                    if (!DateTime.TryParseExact("25/6/2018 0:00:00", "d/M/yyyy h:m:s", enUS, DateTimeStyles.None, out fechasal))
                    {
                    }
                    */
                    IniRpt();
                    IniDsPasePuertaBreakBulk();
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
                //ActualizaOCancelaPasePuerta();
                GPasePuerta_CFS();
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
                                       new System.Xml.Linq.XAttribute("FECHA_EXPIRACION", p.Field<String>("FECHA_AUT_PPWEB") == null ? "" : Convert.ToDateTime(p.Field<String>("FECHA_AUT_PPWEB")).ToString("MM/dd/yyyy")),
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
                                 new System.Xml.Linq.XElement("appointment-date", p.Field<DateTime>("FECHA_SALIDA").ToString("yyyy-MM-dd")),
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
            p_datospp = new DataTable();
            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();
        }

        private void GPasePuerta()
        {
            EstadoPPSinTurno = false;
            Session["ptypeReport"] = "CFS";
            DataTable resultado = new DataTable();
            DataView view = new DataView();


            /*agregado 08-13-2019
                 jalvarado
           
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fechasal = new DateTime();
            bool bError = false;
            string cError = string.Empty;

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                CheckBox chkPase = item.FindControl("chkPase") as CheckBox;
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;
                TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;

                if (chkPase.Checked)
                {


                    if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        bError = true;
                        cError = string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text);
                        txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB_"].ToString();
                        break;
                    }

                    if (ddlTurno.SelectedItem.Value != "0")
                    {//ACTUALIZA NUEVOS DATOS
                        //p_datospp.Rows[item.ItemIndex]["TURNO"] = ddlTurno.SelectedItem.Value == "0" ? "" : ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[0].Trim();
                        p_datospp.Rows[item.ItemIndex]["ID_TURNO"] = ddlTurno.SelectedItem.Value == "0" ? "" : ddlTurno.SelectedItem.Value.ToString();
                        p_datospp.Rows[item.ItemIndex]["D_TURNO"] = ddlTurno.SelectedItem.Text;
                        p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = fechasal.ToString("MM/dd/yyyy");
                        p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"] = fechasal.ToString("yyyy-MM-dd");
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
            */
            /*fin*/



            p_gvCfs = p_datospp;
            /*
            p_gvBreakBulk = new DataTable();
            var lblFactura = p_gvCfs.Rows[0]["FACTURA"].ToString();
            p_gvBreakBulk = pasePuerta.GetInfoPasePuertaCFSGPase(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFactura);
            p_gvBreakBulk.Columns.Add("FECHA_SALIDA");
            
            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;
                CheckBox chkPases = item.FindControl("chkPase") as CheckBox;
                if (chkPases.Checked)
                {
                    p_gvBreakBulk.Rows[0]["FECHA_SALIDA"] = lblFecAutPPWeb.Text.Trim(); //Convert.ToDateTime(lblFecAutPPWeb.Text.Trim()).ToString("yyyy-MM-dd");
                }
            }
            */
            /*14/08/2019 jalvarado*/
            p_gvBreakBulk = new DataTable();
            var lblFactura = p_gvCfs.Rows[0]["FACTURA"].ToString();
            p_gvBreakBulk = pasePuerta.GetInfoPasePuertaCFSGPase(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFactura);



            DataTable dt = new DataTable();
            List<String> XMLN4BreakBulk = new List<String>();
            var p_user = Page.User.Identity.Name;

            dt.Columns.Add(new DataColumn("CONSECUTIVO", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDEMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("PLACA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDCHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FECHA_SALIDA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FECHA_AUT", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CODSUBITEM", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("TIPO_CARGA", Type.GetType("System.String")));
            dt.AcceptChanges();

            if (p_drpasepuertabreakbulk != null || (p_gvCfs != null && p_gvCfs.Rows.Count > 0))
            {


                


                int vrbreak = 0;

                if (p_drpasepuertabreakbulk != null)
                {
                    var wcount_break = (from hi in p_drpasepuertabreakbulk.AsEnumerable()
                                        select hi).Count();
                    vrbreak = (int)wcount_break;
                }

                if (p_gvCfs != null && p_gvCfs.Rows.Count > 0)
                {
                    var wdatoscfs = (from wdata in p_gvBreakBulk.AsEnumerable().AsParallel()
                                     join wdatapase in p_gvCfs.AsEnumerable().AsParallel()
                                     on wdata.Field<String>("MRN") + "-" + wdata.Field<String>("MSN") + "-" + wdata.Field<String>("HSN") equals wdatapase.Field<String>("CARGA")
                                     where wdatapase.Field<String>("ACTUALIZAR") == "True"
                                     select new
                                     {
                                         CONSECUTIVO = wdata.Field<String>("CONSECUTIVO"),
                                         IDEMPRESA = wdatapase.Field<String>("CIA_TRANS"),
                                         PLACA = wdatapase.Field<String>("PLACA"),
                                         IDCHOFER = wdatapase.Field<String>("CHOFER"),
                                         CANTIDAD = wdatapase.Field<String>("CANTIDAD"),
                                         FECHA_SALIDA = wdatapase.Field<DateTime>("FECHA_SALIDA").ToString("MM/dd/yyyy"),
                                         FECHA_AUT = wdatapase.Field<DateTime>("FECHA_SALIDA").ToString("yyyy-MM-dd") + " 00:00",
                                         CARGA = wdatapase.Field<String>("CARGA"),
                                         CODSUBITEMS = wdatapase.Field<String>("SUB_SECUENCIA")
                                     }).ToList();

                    foreach (DataRow wrow in LINQToDataTable(wdatoscfs).Rows)
                    {
                        dt.Rows.Add(new String[] {  wrow["CONSECUTIVO"] == null ? "" : wrow["CONSECUTIVO"].ToString(),
                                                            wrow["IDEMPRESA"] == null ? "" : wrow["IDEMPRESA"].ToString(),
                                                            wrow["PLACA"] == null ? "" : wrow["PLACA"].ToString(),
                                                            wrow["IDCHOFER"] == null ? "" :wrow["IDCHOFER"].ToString(),
                                                            wrow["CANTIDAD"] == null ? "" : wrow["CANTIDAD"].ToString(),
                                                             wrow["FECHA_SALIDA"] == null ? "" : wrow["FECHA_SALIDA"].ToString(),
                                                            wrow["FECHA_AUT"] == null ? "" : wrow["FECHA_AUT"].ToString(),
                                                            wrow["CODSUBITEMS"] == null ? "" : wrow["CODSUBITEMS"].ToString(),
                                                            "CFS",
                                    });
                        dt.AcceptChanges();

                        String XMLN4 = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("groovy",
                        new System.Xml.Linq.XAttribute("class-location", "database"),
                            //new System.Xml.Linq.XAttribute("class-location", "code-extension"),
                        new System.Xml.Linq.XAttribute("class-name", "CGSADeliveryOrderQty"),
                            //new System.Xml.Linq.XAttribute("class-name", "CGSADeliveryOrderCreate"),
                        new System.Xml.Linq.XElement("parameters",
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "agencia"),
                        new System.Xml.Linq.XAttribute("value", wrow["IDEMPRESA"] == null ? "" : wrow["IDEMPRESA"].ToString())),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "camion"),
                        new System.Xml.Linq.XAttribute("value", "")),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "fecha"),
                        new System.Xml.Linq.XAttribute("value", wrow["FECHA_AUT"].ToString()) /*== null ? "" : DateTime.ParseExact(wrow["FECHA_SALIDA"].ToString().Substring(0, 10), "MM/dd/yyyy", new CultureInfo("es-EC")).ToString("yyyy-MM-dd") + " 00:00")*/),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "referencia"),
                        new System.Xml.Linq.XAttribute("value", "")),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "BLs"),
                        new System.Xml.Linq.XAttribute("value", wrow["CARGA"] == null ? "" : wrow["CARGA"].ToString())),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "QTY"),
                        new System.Xml.Linq.XAttribute("value", wrow["CANTIDAD"] == null ? "" : wrow["CANTIDAD"].ToString())),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "codsubitem"),
                        new System.Xml.Linq.XAttribute("value", wrow["CODSUBITEMS"] == null ? "" : wrow["CODSUBITEMS"].ToString())),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "placa"),
                        new System.Xml.Linq.XAttribute("value", wrow["PLACA"] == null ? "" : wrow["PLACA"].ToString())),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "chofer"),
                        new System.Xml.Linq.XAttribute("value", wrow["IDCHOFER"] == null ? "" : wrow["IDCHOFER"].ToString())),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "tipo_carga"),
                        new System.Xml.Linq.XAttribute("value", "CFS")),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "consecutivo"),
                        new System.Xml.Linq.XAttribute("value", wrow["CONSECUTIVO"] == null ? "" : wrow["CONSECUTIVO"].ToString())),
                        new System.Xml.Linq.XElement("parameter",
                        new System.Xml.Linq.XAttribute("id", "usuer"),
                        new System.Xml.Linq.XAttribute("value", p_user))))).ToString();
                        XMLN4BreakBulk.Add(XMLN4);
                    }
                }

                System.Xml.Linq.XDocument docXML =
                new System.Xml.Linq.XDocument(
                new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                new System.Xml.Linq.XElement("PASEPUERTA",
                from p in dt.AsEnumerable().AsParallel()
                select new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA",
                new System.Xml.Linq.XAttribute("ID_CARGA", p.Field<String>("CONSECUTIVO")),
                new System.Xml.Linq.XAttribute("ESTADO", "GN"),
                new System.Xml.Linq.XAttribute("FECHA_EXPIRACION", p.Field<String>("FECHA_SALIDA")),
                new System.Xml.Linq.XAttribute("ID_PLACA", p.Field<String>("PLACA")),
                new System.Xml.Linq.XAttribute("ID_CHOFER", p.Field<String>("IDCHOFER")),
                new System.Xml.Linq.XAttribute("ID_EMPRESA", p.Field<String>("IDEMPRESA")),
                new System.Xml.Linq.XAttribute("CANTIDAD_CARGA", p.Field<String>("CANTIDAD")),
                new System.Xml.Linq.XAttribute("USUARIO_REGISTRO", p_user),
                new System.Xml.Linq.XAttribute("FECHA_REGISTRO", DateTime.Now.ToString("MM/dd/yyyy")),
                new System.Xml.Linq.XAttribute("USUARIO_ESTADO", p_user),
                new System.Xml.Linq.XAttribute("FECHA_ESTADO", DateTime.Now.ToString("MM/dd/yyyy")),
                new System.Xml.Linq.XAttribute("ID_RESERVA", ""),
                new System.Xml.Linq.XAttribute("ID_PLAN", ""),
                new System.Xml.Linq.XAttribute("ID_PLAN_SECUENCIA", ""),
                new System.Xml.Linq.XAttribute("TIPO_CARGA", p.Field<String>("TIPO_CARGA")),
                new System.Xml.Linq.XAttribute("CONTENEDOR", ""),
                new System.Xml.Linq.XAttribute("flag", "I"))));

                try
                {
                    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                    DataSet ds_report = new DataSet();
                    ds_report = WPASEPUERTA.SavePasePuerta(docXML.ToString(), XMLN4BreakBulk.ToArray(), "BRBK");

                    if (ds_report == null)
                    {
                        p_reportPasePuerta = null;
                    }
                    else
                    {
                        p_reportPasePuerta = new DataSet();

                        //for (int i = 0; i <= ds_report.Tables.Count - 1; i++)
                        //{
                        int i = 0;
                        if (ds_report.Tables[0].TableName.Equals("DTError"))
                        {
                            //GVERRORPASES.DataSource = ds_report.Tables[i];
                            //GVERRORPASES.DataBind();
                            dtHorarios = new DataTable();
                            dtHorariosfull = new DataTable();
                            //gvHorarios.DataSource = dtHorarios;
                            //gvHorarios.DataBind();
                            //UPMOFACTURA.Update();
                            //utilform.MessageBox("Pase a Puerta NO GENERADO, revise informaciòn... ", this);
                        }
                        else
                        {/*
                            if (Session["EstadoPPSinTurnoCFS"] == null)
                            {
                                EstadoPPSinTurno = true;
                            }
                            if (EstadoPPSinTurno == false)
                            {*/
                                try
                                {
                                    EstadoPPSinTurno = false;
                                    resultado = new DataTable();
                                    view = new DataView();
                                    p_dtturnos.Columns.Add("VALIDA");

                                    var resultsst = from myRow in p_datospp.AsEnumerable()
                                                   where myRow.Field<String>("TIPO") == "A"
                                                   select myRow;
                                    view = resultsst.AsDataView();
                                    resultado = view.ToTable();
                                    if (resultado != null)
                                    {
                                        if (resultado.Rows.Count > 0)
                                        {
                                            for (int i3 = 0; i3 < resultado.Rows.Count; i3++)
                                            {
                                                for (int i4 = 0; i4 < p_dtturnos.Rows.Count; i4++)
                                                {
                                                    if (resultado.Rows[i3]["ID_TURNO"].ToString() == p_dtturnos.Rows[i4]["IDDISPONIBLEDET"].ToString())
                                                    {
                                                        p_dtturnos.Rows[i4]["VALIDA"] = "1";
                                                    }
                                                }
                                            }

                                            resultado = new DataTable();
                                            view = new DataView();
                                            var resultss = from myRow in p_dtturnos.AsEnumerable()
                                                           where myRow.Field<String>("VALIDA") == "1"
                                                           select myRow;
                                            view = resultss.AsDataView();
                                            resultado = view.ToTable();

                                            StringWriter writer = new StringWriter();
                                            resultado.TableName = "Cfs";
                                            resultado.WriteXml(writer);

                                            StringWriter writerdt = new StringWriter();
                                            //dt.TableName = "Cfs";
                                            //dt.WriteXml(writerdt);

                                            DataTable Dreporte = new DataTable();
                                            Dreporte = ds_report.Tables[i].Copy();
                                            p_reportPasePuerta.Tables.Add(Dreporte);

                                            //fac_business fac = new fac_business();
                                            Dreporte.TableName = "Cfs";
                                            Dreporte.WriteXml(writerdt);

                                            //for (int index = 0; index <= Dreporte.Rows.Count - 1; index++)
                                            //{
                                            //for (int indexd = 0; indexd <= resultado.Rows.Count - 1; indexd++)
                                            //{
                                            pasePuerta.REGISTRA_HORARIOS_PASEPUERTA(writer.ToString(), writerdt.ToString(), sfechasalida/*.Date*/, /*Convert.ToInt64(Dreporte.Rows[index][23].ToString())*/0, Page.User.Identity.Name);

                                            //}
                                            //}
                                        }
                                    }
                                    dtHorarios = new DataTable();
                                    dtHorariosfull = new DataTable();
                                    //gvHorarios.DataSource = dtHorarios;
                                    //gvHorarios.DataBind();
                                    listsubitems = new List<string>();

                                    var dtPPWeb = (from wppweb in ds_report.Tables[0].AsEnumerable()
                                                   select wppweb).AsDataView().ToTable();

                                    StringWriter xmlPPWeb = new StringWriter();
                                    dtPPWeb.TableName = "PaseWeb";
                                    dtPPWeb.WriteXml(xmlPPWeb);

                                    String WfACTURADOA = p_gvBreakBulk.Rows[0]["FACTURADO"].ToString();
                                    String wuser = (String)HttpContext.Current.Session["puser"];
                                    String wsAgente = p_gvBreakBulk.Rows[0]["AGENTE"].ToString();
                                    String wempr = p_gvBreakBulk.Rows[0]["FACTURADO"].ToString();

                                    var Empresa = (from currentStat in p_drEmpresa.AsEnumerable()
                                                   where currentStat.Field<String>("IDEMPRESA") == WfACTURADOA
                                                   select currentStat).FirstOrDefault();

                                    //XElement xmlElements = new XElement("SUBITEMS", xmllistsubitems.Select(i2 => new XElement("SECUENCIA", i2)));
                                    var dtPPWebCFS = (from wppweb in ds_report.Tables[0].AsEnumerable()
                                                      select wppweb).AsDataView().ToTable();
                                    StringWriter xmlPPWebCFS = new StringWriter();
                                    dtPPWebCFS.TableName = "PaseWeb";
                                    dtPPWebCFS.WriteXml(xmlPPWebCFS);

                                    var p_cfs = p_gvCfs;
                                    var dtPPWebCFSDet = (from wppweb in p_cfs.AsEnumerable()
                                                         select wppweb).AsDataView().ToTable();

                                    StringWriter xmlPPWebCFSDet = new StringWriter();
                                    dtPPWebCFSDet.TableName = "PaseWebDet";
                                    dtPPWebCFSDet.WriteXml(xmlPPWebCFSDet);
                                // Empresa["ROLE"].ToString()
                                string msjerror = string.Empty;
                                    if (!pasePuerta.AcualizaPaseWebCFSCA(xmlPPWeb.ToString(), xmlPPWebCFS.ToString(), xmlPPWebCFSDet.ToString(), p_gvBreakBulk.Rows[0]["FACTURA"].ToString(), wsAgente, wempr, "SHIPPER", "", "", p_user, out msjerror))
                                    {
                                        this.Alerta(msjerror);
                                        return;
                                    }
                                    this.Alerta("e-Pass procesado Exitosamente... ");
                                    IniPaseWeb();
                                    /*
                                    var function = "openPop('" + p_gvCfs.Rows[0]["CARGA"].ToString() + "','" + ds_report.Tables[0].Rows[0]["PASE"].ToString() + "');";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", function, true);
                                    */
                                }
                                catch (Exception ex)
                                {
                                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                                    return;
                                }
                            /*}
                            else
                            {
                                DataTable Dreporte = new DataTable();
                                Dreporte = ds_report.Tables[i].Copy();
                                p_reportPasePuerta.Tables.Add(Dreporte);
                                //utilform.MessageBox("Pase a Puerta generado Exitosamente... ", this);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                                p_datospp = new DataTable();
                                tablePaginationPPWeb.DataSource = p_datospp;
                                tablePaginationPPWeb.DataBind();
                                this.Alerta("e-Pass procesado Exitosamente... ");
                               
                            }*/
                        }
                        //}

                    }
                }
                catch (Exception exc)
                {
                    var number = log_csl.save_log<Exception>(exc, "emision_pp_web", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    return;
                }
            }
        }

        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        private void GPasePuerta_CFS()
        {

            try
            {

                var dtPPWebVal = (from p in p_datospp.AsEnumerable()
                                  where p.Field<String>("CANCELAR") == "False" && p.Field<String>("ACTUALIZAR") == "False"
                                  select p).AsDataView().ToTable();

                if (dtPPWebVal.Rows.Count == p_datospp.Rows.Count)
                {
                    this.Alerta("No tiene datos para procesar.");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    return;
                }


                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                bool bError = false;
                string cError = string.Empty;

                foreach (RepeaterItem item in tablePaginationPPWeb.Items)
                {
                    DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                    CheckBox chkPase = item.FindControl("chkPase") as CheckBox;
                    TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;
                    TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;

                    if (chkPase.Checked)
                    {


                        if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                        {
                            bError = true;
                            cError = string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text);
                            txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB_"].ToString();
                            break;
                        }

                        if (ddlTurno.SelectedItem.Value != "0")
                        {//ACTUALIZA NUEVOS DATOS
                            p_datospp.Rows[item.ItemIndex]["ID_TURNO"] = ddlTurno.SelectedItem.Value == "0" ? "" : ddlTurno.SelectedItem.Value.ToString();
                            p_datospp.Rows[item.ItemIndex]["D_TURNO"] = ddlTurno.SelectedItem.Text;
                            p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = fechasal.ToString("MM/dd/yyyy");
                            p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"] = fechasal.ToString("yyyy-MM-dd");
                            p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = chkPase.Checked.ToString();
                            p_datospp.AcceptChanges();
                        }
                        else
                        {
                            bError = true;
                            cError = "! Elija un Turno.";
                        }

                    }
                    else
                    {
                        p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = chkPase.Checked.ToString();
                        p_datospp.AcceptChanges();
                    }
                }

                if (bError)
                {
                    this.Alerta(cError);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    return;
                }

                //cancela pase

                var p_user = Page.User.Identity.Name;
                String MotivoCan = "9";
                /*XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                                 new XElement("PASEPUERTA", from p in p_gvPase.AsEnumerable().AsParallel()
                                                                            where p.Field<Boolean>("CANCELAR").Equals(true) 
                                                                            select new XElement("VBS_P_PASE_PUERTA",
                                                                            new XAttribute("ID_PASE", p.Field<Decimal>("ID_PASE") == null ? "" : p.Field<Decimal>("ID_PASE").ToString()),
                                                                            new XAttribute("PASE", p.Field<String>("NUMERO_PASE_N4") == null ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()),
                                                                            new XAttribute("TIPO_CARGA", p.Field<String>("TIPO_CARGA") == null ? "" : p.Field<String>("TIPO_CARGA").ToString()),
                                                                            new XAttribute("ESTADO", "CA"),
                                                                            new XAttribute("USUARIO_ESTADO", Page.User.Identity.Name),
                                                                            new XAttribute("FECHA_ESTADO", DateTime.Now.ToString("MM/dd/yyyy HH:mm")),
                                                                            new XAttribute("MOTIVO_CANCELACION", MotivoCan),
                                                                            new XAttribute("flag", "U"),
                                                                            new XAttribute("RESERVA", p.Field<Boolean>("RESERVA") == Boolean.Parse("false") ? 0 : 1))));*/

                XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                                                new XElement("PASEPUERTA", from p in p_gvPase.AsEnumerable().AsParallel()
                                                                                           join wdatapase in p_datospp.AsEnumerable().AsParallel()
                                                                                           on new { a = p.Field<String>("CARGA"), b = p.Field<Decimal>("ID_PASE") } equals new { a = wdatapase.Field<String>("CARGA"), b = wdatapase.Field<Decimal>("ID_PASE") }
                                                                                           where p.Field<Boolean>("CANCELAR").Equals(true) && wdatapase.Field<String>("ACTUALIZAR") == "True"
                                                                                           select new XElement("VBS_P_PASE_PUERTA",
                                                                                           new XAttribute("ID_PASE", p.Field<Decimal>("ID_PASE") == null ? "" : p.Field<Decimal>("ID_PASE").ToString()),
                                                                                           new XAttribute("PASE", p.Field<String>("NUMERO_PASE_N4") == null ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()),
                                                                                           new XAttribute("TIPO_CARGA", p.Field<String>("TIPO_CARGA") == null ? "" : p.Field<String>("TIPO_CARGA").ToString()),
                                                                                           new XAttribute("ESTADO", "CA"),
                                                                                           new XAttribute("USUARIO_ESTADO", Page.User.Identity.Name),
                                                                                           new XAttribute("FECHA_ESTADO", DateTime.Now.ToString("MM/dd/yyyy HH:mm")),
                                                                                           new XAttribute("MOTIVO_CANCELACION", MotivoCan),
                                                                                           new XAttribute("flag", "U"),
                                                                                           new XAttribute("RESERVA", p.Field<Boolean>("RESERVA") == Boolean.Parse("false") ? 0 : 1))));

                /*String[] docXMLN4 = (from p in p_gvPase.AsEnumerable().AsParallel()
                                     where p.Field<Boolean>("CANCELAR").Equals(true) && p.Field<String>("TIPO_CARGA").Equals("CNTR")
                                     select new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                     new XElement("gate",
                                        new XElement("cancel-appointment",
                                            new XElement("appointments",
                                                new XElement("appointment",
                                                      new XAttribute("appointment-nbr", (String.IsNullOrEmpty(p.Field<String>("NUMERO_PASE_N4").ToString()) == true ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()))
                                                    ))))).ToString()).ToArray();*/

                String[] docXMLN4 = (from p in p_gvPase.AsEnumerable().AsParallel()
                                     join wdatapase in p_datospp.AsEnumerable().AsParallel()
                                     on new { a = p.Field<String>("CARGA"), b = p.Field<Decimal>("ID_PASE") } equals new { a = wdatapase.Field<String>("CARGA"), b = wdatapase.Field<Decimal>("ID_PASE") }
                                     where p.Field<Boolean>("CANCELAR").Equals(true) && p.Field<String>("TIPO_CARGA").Equals("CNTR") && wdatapase.Field<String>("ACTUALIZAR") == "True"
                                     select new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                     new XElement("gate",
                                        new XElement("cancel-appointment",
                                            new XElement("appointments",
                                                new XElement("appointment",
                                                      new XAttribute("appointment-nbr", (String.IsNullOrEmpty(p.Field<String>("NUMERO_PASE_N4").ToString()) == true ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()))
                                                    ))))).ToString()).ToArray();


                /* String[] docXMLN4brk = (from p in p_gvPase.AsEnumerable().AsParallel()
                                         where p.Field<Boolean>("CANCELAR").Equals(true) && (p.Field<String>("TIPO_CARGA").Equals("BRBK") || p.Field<String>("TIPO_CARGA").Equals("CFS"))
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
                                         ))).ToString()).ToArray();*/

                String[] docXMLN4brk = (from p in p_gvPase.AsEnumerable().AsParallel()
                                        join wdatapase in p_datospp.AsEnumerable().AsParallel()
                                        on new { a = p.Field<String>("CARGA"), b = p.Field<Decimal>("ID_PASE") } equals new { a = wdatapase.Field<String>("CARGA"), b = wdatapase.Field<Decimal>("ID_PASE") }
                                        where wdatapase.Field<String>("ACTUALIZAR") == "True" && p.Field<Boolean>("CANCELAR").Equals(true) && (p.Field<String>("TIPO_CARGA").Equals("BRBK") || p.Field<String>("TIPO_CARGA").Equals("CFS"))
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

                if (((from p in p_gvPase.AsEnumerable().AsParallel()
                      where p.Field<Boolean>("CANCELAR").Equals(true) && p.Field<String>("TIPO_CARGA").Equals("CNTR")
                      select p).Count() > 0))
                {
                    docXMLICU = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                       new XElement("icus",
                                           from p in p_gvPase.AsEnumerable().AsParallel()
                                           where p.Field<Boolean>("CANCELAR").Equals(true) && p.Field<String>("TIPO_CARGA").Equals("CNTR")
                                           select new XElement("icu",
                                                new XAttribute("CARGA", (String.IsNullOrEmpty(p.Field<String>("CARGA").ToString()) == true ? "" : p.Field<String>("CARGA").ToString())),
                                                 new XAttribute("USUARIO", (String.IsNullOrEmpty(p_user) == true ? "" : p_user))
                                                       ))).ToString();
                }


                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                DataSet ds_report = new DataSet();
                WPASEPUERTA.SaveCancelarPasePuerta(docXML.ToString(), docXMLN4, docXMLN4brk);
                if (docXMLICU != null)
                {
                    // WPASEPUERTA.EXECICU(docXMLICU);
                    if (MotivoCan.Equals("3"))
                        WPASEPUERTA.EXECICU_tmp(docXMLICU, 1);
                    else
                        if (MotivoCan.Equals("8"))
                            WPASEPUERTA.EXECICU_tmp(docXMLICU, 1);
                }
                //fac_business fac = new fac_business();
                pasePuerta.CANCELA_HORARIOS_PASEPUERTA(docXML.ToString(), Page.User.Identity.Name);

                var dtPPWeb_CFS = (from p in p_gvPase.AsEnumerable()
                                   where p.Field<Boolean>("CANCELAR").Equals(true) && p.Field<String>("TIPO_CARGA").Equals("CFS")
                                   select p).AsDataView().ToTable();

                //var dtPPWeb_CFS = (from p in p_gvPase.AsEnumerable()
                //                   join wdatapase in p_datospp.AsEnumerable().AsParallel() on p.Field<String>("CARGA") equals wdatapase.Field<String>("CARGA")
                //                   where p.Field<Boolean>("CANCELAR").Equals(true) && p.Field<String>("TIPO_CARGA").Equals("CFS")
                //                   select p).AsDataView().ToTable();

                var msjerror = string.Empty;

                StringWriter xmlPPWeb = new StringWriter();
                dtPPWeb_CFS.TableName = "PaseWeb";
                dtPPWeb_CFS.WriteXml(xmlPPWeb);
                if (!pasePuerta.CancelaPasePuertaWebCFS(xmlPPWeb.ToString(), p_user, out msjerror))
                {
                    this.Alerta(msjerror);
                }


                //generar nuevo pase
                GPasePuerta();

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "cancelacion_actualizacion_pasepuerta_cfs", "GpasePuerta_CFS()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
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
            p_datospp = pasePuerta.GetDatosActualizaCancelaPaseWebCFSS3_2019(this.agencia.Value, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), txtcntr.Text.Trim(), txtfecsal.Text != "" ? fechasal.ToString("yyyy-MM-dd") : txtfecsal.Text.Trim());
            if (p_datospp.Rows.Count == 0)
            {
                this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                return;
            }
            IniDsEmpresa();
            p_datospp.Columns.Add("CIATRANSDES");
            for (int i = 0; i < p_datospp.Rows.Count; i++)
            {
                var ruccitrans = p_datospp.Rows[i]["CIATRANS"].ToString().Trim();
                var desEmpresa = (from row in p_drEmpresa.AsEnumerable()
                                  where row.Field<String>("IDEMPRESA").Trim() == ruccitrans
                                  select row).AsDataView().ToTable();
                if (desEmpresa != null || desEmpresa.Rows.Count > 0)
                {
                    p_datospp.Rows[i]["CIATRANSDES"] = desEmpresa.Rows[0]["EMPRESA"];
                }
            }

            //IniDsEmpresaLoad();
            
            IniDsChofer("");
            //IniTurno(); 
            IniDsPlaca("");
            //IniDatosPPWeb();
            p_datospp.Columns.Add("ACTUALIZAR");
            p_datospp.Columns.Add("CANCELAR");
            p_datospp.Columns.Add("TIPO");
            p_datospp.Columns.Add("ID_TURNO");
            p_datospp.Columns.Add("CIA_TRANS");
            p_datospp.Columns.Add("CHOFER");
            p_datospp.Columns.Add("PLACA");
            p_datospp.Columns.Add("FECHA_AUT_PPWEB");
            p_datospp.Columns.Add("FECHA_SALIDA", typeof(DateTime));

            Char delimiter = ',';
            new List<string>();
            List<string> lsubstring = new List<string>();
            string xml = "";

            /*
            for (int i2 = 0; i2 < p_datospp.Rows.Count; i2++)
            {
                var substrings = p_datospp.Rows[i2]["SUB_SECUENCIA"].ToString().Split(delimiter);
                foreach (var substring in substrings)
                {
                    lsubstring.Add(substring);
                }
            }
            
            string xml = "";
            GenXml(lsubstring, out xml);
            */

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
            DataTable dtturnos = new DataTable();
            p_dtturnos = new DataTable();
            p_dtturnos.Columns.Add("TOTBULTOS");
            p_dtturnos.Columns.Add("IDDISPONIBLEDET");
            p_dtturnos.Columns.Add("HORADESDE");
            p_dtturnos.Columns.Add("HORAHASTA");
            p_dtturnos.Columns.Add("BULTOS");
            p_dtturnos.Columns.Add("CHECKED");

            for (int i = 0; i < p_datospp.Rows.Count; i++)
            {
                for (int i2 = 0; i2 < dsRetorno.Tables[0].Rows.Count; i2++)
                {
                    if (p_datospp.Rows[i]["ID_PASE"].ToString() == dsRetorno.Tables[0].Rows[i2]["ID_PASE"].ToString())
                    {
                        fechasal = new DateTime();
                        //fechasal = dsRetorno.Tables[0].Rows[i2]["FECHA_EXPIRACION"].ToString();
                        var fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[i2]["FECHA_EXPIRACION"].ToString());
                        fechasal = fechasal_;
                        /*
                        if (!DateTime.TryParseExact(dsRetorno.Tables[0].Rows[i2]["FECHA_EXPIRACION"].ToString(), "dd/mm/yyyy hh:mm:ss", enUS, DateTimeStyles.None, out fechasal))
                        {
                        }*/
                        
                        p_datospp.Rows[i]["FECHA_AUT_PPWEB"] = fechasal.ToString("dd/MM/yyyy");
                        new List<string>();
                        lsubstring = new List<string>();
                        var substrings = p_datospp.Rows[i]["SUB_SECUENCIA"].ToString().Split(delimiter);
                        foreach (var substring in substrings)
                        {
                            lsubstring.Add(substring);
                        }
                        xml = "";
                        GenXml(lsubstring, out xml);
                        var fechasalidad = fechasal.ToString("MM-dd-yyyy");
                        dtturnos = pasePuerta.CONSULTA_HORARIOS_DISPONIBLES_(fechasalidad, xml);
                        for (int i3 = 0; i3 < dtturnos.Rows.Count; i3++)
                        {
                            p_dtturnos.Rows.Add(dtturnos.Rows[i3][0], dtturnos.Rows[i3][1], dtturnos.Rows[i3][2], dtturnos.Rows[i3][3], dtturnos.Rows[i3][4], dtturnos.Rows[i3][5]);
                            p_dtturnos.AcceptChanges();
                        }
                    }
                }
            }

            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();

            DataView vista = new DataView(p_dtturnos);
            DataTable dtsindupl = vista.ToTable(true, "TOTBULTOS", "IDDISPONIBLEDET", "HORADESDE", "HORAHASTA", "BULTOS", "CHECKED");
            p_dtturnos = new DataTable();
            p_dtturnos = dtsindupl;

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                LinkButton lnkImprimir = item.FindControl("lnkImprimir") as LinkButton;
                string url = "../facturacion/impresion-pase-de-puerta-carga-suelta-cfs?opcion=ReprePasePuerta&Carga=" + p_datospp.Rows[item.ItemIndex]["CARGA"].ToString() + "&Pase=" + p_datospp.Rows[item.ItemIndex]["NUMERO_PASE_N4"].ToString() + "&Tipo=" + "BRBK";
                lnkImprimir.Attributes.Add("onClick", "JavaScript: window.open('" + url + "','Reimpresion Pase de Puerta','width=700,height=700')");
                p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = "False";
                p_datospp.Rows[item.ItemIndex]["CANCELAR"] = "False";


                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                
                new List<string>();
                var substrings = p_datospp.Rows[item.ItemIndex]["SUB_SECUENCIA"].ToString().Split(delimiter);
                lsubstring = new List<string>();
                foreach (var substring in substrings)
                {
                    lsubstring.Add(substring);
                }
                xml = "";
                GenXml(lsubstring, out xml);
                DataTable dtHorarios = new DataTable();
                for (int i4 = 0; i4 < dsRetorno.Tables[0].Rows.Count; i4++)
                {
                    if (p_datospp.Rows[item.ItemIndex]["ID_PASE"].ToString() == dsRetorno.Tables[0].Rows[i4]["ID_PASE"].ToString())
                    {
                        fechasal = new DateTime();
                        var fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[i4]["FECHA_EXPIRACION"].ToString());
                        fechasal = fechasal_;
                        /*
                        if (!DateTime.TryParseExact(dsRetorno.Tables[0].Rows[i4]["FECHA_EXPIRACION"].ToString(), "d/M/yyyy h:m:s", enUS, DateTimeStyles.None, out fechasal))
                        {
                        }
                        */
                        var fechasalida = fechasal.ToString("MM-dd-yyyy");
                        dtHorarios = pasePuerta.CONSULTA_HORARIOS_DISPONIBLES_(fechasalida, xml);
                        dtHorarios.Rows.Add(0, 0, "ELIJA", "", 0, false);
                        dtHorarios.DefaultView.Sort = "HORADESDE desc";
                        dtHorarios = dtHorarios.DefaultView.ToTable(true);
                        if (dtHorarios.Rows.Count > 0 || dtHorarios != null)
                        {
                            ddlTurno.DataTextField = "HORADESDE";
                            ddlTurno.DataValueField = "IDDISPONIBLEDET";
                            ddlTurno.DataSource = dtHorarios.AsDataView();
                            ddlTurno.DataBind();
                        }
                    }
                }
                /*
                var cichofer = p_datospp.Rows[item.ItemIndex]["CHOFER"].ToString().Trim();
                var desChofer = (from row in p_drChofer.AsEnumerable()
                                 where row.Field<String>("IDCHOFER").Trim() == cichofer
                                 select row).AsDataView().ToTable();
                if (desChofer != null || desChofer.Rows.Count > 0)
                {
                    p_datospp.Rows[item.ItemIndex]["CHOFER"] = desChofer.Rows[item.ItemIndex]["IDCHOFER"] + " - " + desChofer.Rows[item.ItemIndex]["CHOFER"];
                }
                */
            }

            dsRetorno.Tables[0].Columns.Add("ACTEPASS");
            dsRetorno.Tables[0].Columns.Add("FECHA_SALIDA");

            for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
            {
                fechasal = new DateTime();
                fechasal = new DateTime();
                var fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[i]["FECHA_EXPIRACION"].ToString());
                fechasal = fechasal_;
                /*
                if (!DateTime.TryParseExact(dsRetorno.Tables[0].Rows[i]["FECHA_EXPIRACION"].ToString(), "d/M/yyyy h:m:s", enUS, DateTimeStyles.None, out fechasal))
                {
                }
                */
                dsRetorno.Tables[0].Rows[i]["ACTEPASS"] = false;
                dsRetorno.Tables[0].Rows[i]["FECHA_SALIDA"] = fechasal.ToString("dd/MM/yyyy");
            }

            p_gvPase = dsRetorno.Tables[0];

            lblTotCntr.Text = "Tot. Pases: " + p_datospp.Rows.Count.ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
        }

        private void IniTurno()
        {
            try
            {
                foreach (RepeaterItem item in tablePaginationPPWeb.Items)
                {
                   

                    //var dsresult = dsRetornoturno.Tables[0].DefaultView.ToTable();//.Rows[0]["TURNO"] = "* ELIJA";


                    //if (p_datospp.Rows[item.ItemIndex]["TIPO_CNTR"].ToString() == "RF")
                    //{
                    //    var dtTurnos = (from p in dsresult.AsDataView().ToTable().AsEnumerable()
                    //                    select p).AsDataView().ToTable();

                    //    StringWriter xmlTurnos = new StringWriter();
                    //    dtTurnos.TableName = "Turnos";
                    //    dtTurnos.WriteXml(xmlTurnos);
                    //    var dthorasrf = pasePuerta.GetTurnosInfoReefer(xmlTurnos.ToString(), p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"].ToString());
                    //    ddlTurno.DataSource = dthorasrf.DefaultView.ToTable().AsDataView();
                    //}
                    //else
                    //{
                    //    ddlTurno.DataSource = dsresult.AsDataView();
                    //}
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "detalle_can_act_pasepuerta", "chkPase_CheckedChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }


        public void GenXml(List<String> listasubitems, out String xml)
        {
            xml = "";
            String xmvar = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
            new System.Xml.Linq.XElement("CFS",
            from p in listasubitems
            select new System.Xml.Linq.XElement("SUB",
            new System.Xml.Linq.XAttribute("SUBITEMS", p)))).ToString();
            xml = xmvar;
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
            //Session["EstadoPPSinTurnoCFS"] = false;
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



        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                int bandera = 0;
                foreach (RepeaterItem items in tablePaginationPPWeb.Items)
                {
                    CheckBox chkPases = items.FindControl("chkPase") as CheckBox;
                    if (chkPases.Checked)
                    {
                        bandera = bandera + 1;
                    }
                    if (bandera == 2)
                    {
                        this.Alerta("Solo puede actualizar un Pase de Puerta a la vez.");
                        chkPase.Checked = false;
                       
                        return;
                    }
                }


                Label lblCntr = (Label)item.FindControl("lblCntr");
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;
                Label lbldturno = item.FindControl("lbldturno") as Label;
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                //DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                CheckBox chkCanPase = item.FindControl("chkCanPase") as CheckBox;

                 TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (chkPase.Checked)
                {
                    chkCanPase.Checked = false;
                    //if (ddlTurno.SelectedItem.Value == "0" && ddlEmpresa.SelectedItem.Value == "0")
                    //{
                    //    this.Alerta("Seleccione turno o Cía de transporte a actualizar para el contenedor : " + lblCntr.Text);
                    //    chkPase.Checked = false;
                    //    return;
                    //}
                    
                    if (ddlTurno.SelectedItem.Value == "0")
                    {
                        this.Alerta("Elija un Turno.");
                        chkPase.Checked = false;
                        return;
                    }
                    
                    /*
                    if (ddlEmpresa.SelectedItem.Value == "0")
                    {
                        this.Alerta("Elija una Cia. Trans para el Contenedor: " + lblCntr.Text);
                        chkPase.Checked = false;
                        return;
                    }
                    */
                    if (ddlTurno.SelectedItem.Value != "0")
                    {
                        /*
                        var strturno = lbldturno.Text.Trim().Substring(0, 5);
                        var v_turno = pasePuerta.GetValTurno(Convert.ToDateTime(lblFecAutPPWeb.Text.Trim()).ToString("yyyy-MM-dd"), strturno.Trim());
                        if (v_turno.Rows[0]["V_TURNO"].ToString() == "1")
                        {
                            var v_msg = v_turno.Rows[0]["MENSAJE"].ToString().Trim();
                            this.Alerta(v_msg);
                            chkPase.Checked = false;
                            return;
                        }
                        */
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
                        DateTime  fechasal_ = new DateTime();
                        if (!string.IsNullOrEmpty(txtfecsalpp.Text))
                        {
                            if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                            {
                                this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text));
                                txtfecsal.Focus();
                                fechasal_ = new DateTime();
                                var _fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString());
                                fechasal_ = _fechasal_;
                                /*
                                if (!DateTime.TryParseExact(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString(), "d/M/yyyy h:m:s", enUS, DateTimeStyles.None, out fechasal_))
                                {
                                }
                                */
                                txtfecsalpp.Text = fechasal_.ToString("dd/MM/yyyy");
                                return;
                            }
                            
                        }
                        
                        
                        DateTime fechaultimoase = new DateTime();
                        for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
                        {
                            if (p_datospp.Rows[item.ItemIndex]["ID_PASE"].ToString() == dsRetorno.Tables[0].Rows[i]["ID_PASE"].ToString())
                            {
                                //p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = Convert.ToDateTime(txtfecsalpp.Text.Trim()).ToString("yyyy-MM-dd");
                                fechasal_ = new DateTime();
                                var _fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString());
                                fechasal_ = _fechasal_;
                                /*
                                if (!DateTime.TryParseExact(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString(), "d/M/yyyy h:m:s", enUS, DateTimeStyles.None, out fechasal_))
                                {
                                }
                                */
                                fechaultimoase = fechasal_;
                            }
                        }

                        var strturno = lbldturno.Text.Trim().Substring(0, 5);
                        var v_turno = pasePuerta.GetValTurno(fechaultimoase.ToString("yyyy-MM-dd"), strturno.Trim());
                        if (v_turno.Rows[0]["V_TURNO"].ToString() == "1")
                        {
                            var v_msg = v_turno.Rows[0]["MENSAJE"].ToString().Trim();
                            this.Alerta(v_msg);
                            chkPase.Checked = false;
                            return;
                        }
                    }
                    TextBox TxtGEmpresa = item.FindControl("TxtGEmpresa") as TextBox;
                    //DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                    if (string.IsNullOrEmpty(TxtGEmpresa.Text))//ddlEmpresa.SelectedItem.Value == "0")
                    {
                        this.Alerta("Elija una Cia. Transporte.");
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
                    DTRESULT2 = (DataTable)HttpContext.Current.Session["drChoferFilterPPWebCan"];
                    if (DTRESULT2 != null)
                    {
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
                    }
                    //DropDownList ddlChofer = item.FindControl("ddlChofer") as DropDownList;
                    TextBox txtPlaca = item.FindControl("txtPlaca") as TextBox;
                    if (!string.IsNullOrEmpty(TxtGChofer.Text)) //ddlChofer.SelectedItem.Value != "0")
                    {
                        if (string.IsNullOrEmpty(txtPlaca.Text))
                        {
                            this.Alerta("Escriba una Placa." + lblCntr.Text);
                            chkPase.Checked = false;
                            return;
                        }
                        else
                        {
                            //IniDsPlaca(ddlEmpresa.SelectedItem.Value);
                            //var wPlaca = (from row in p_drPlaca.AsEnumerable()
                            //              where row.Field<String>("PLACA") != null && row.Field<String>("PLACA").Trim().Equals(txtPlaca.Text.ToString().Trim().ToUpper())
                            //              select row.Field<String>("PLACA")).Count();

                            //if ((int)wPlaca <= 0)
                            //{
                            //    this.Alerta("La Placa no es valida para el Contenedor: " + lblCntr.Text);
                            //    chkPase.Checked = false;
                            //    return;
                            //}
                            //p_datospp.Rows[item.ItemIndex]["CHOFER"] = ddlChofer.SelectedItem.Value;
                            //p_datospp.Rows[item.ItemIndex]["PLACA"] = txtPlaca.Text.Trim();
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
                    /*
                    else
                    {
                        p_datospp.Rows[item.ItemIndex]["CHOFER"] = "NULL";
                        p_datospp.Rows[item.ItemIndex]["PLACA"] = "NULL";
                    }
                    */
                    if (ddlTurno.SelectedItem.Value != "0")
                    {
                        //p_datospp.Rows[item.ItemIndex]["TURNO"] = ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[0].Trim();
                        p_datospp.Rows[item.ItemIndex]["ID_TURNO"] = ddlTurno.SelectedItem.Value;
                        //p_datospp.Rows[item.ItemIndex]["D_TURNO"] = ddlTurno.SelectedItem.Text;
                    }
                    //if (ddlEmpresa.SelectedItem.Value != "0")
                    //{
                    //    p_datospp.Rows[item.ItemIndex]["CIATRANS"] = ddlEmpresa.SelectedItem.Value;
                    //}
                    if (!string.IsNullOrEmpty(TxtGEmpresa.Text.Trim()))//ddlEmpresa.SelectedItem.Value != "0")
                    {
                        String value = TxtGEmpresa.Text.Trim();
                        Char delimiter = '-';
                        List<string> substringemp = value.Split(delimiter).ToList();
                        p_datospp.Rows[item.ItemIndex]["CIA_TRANS"] = substringemp[0].Trim() == "0" ? "" : substringemp[0].Trim();
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
                    for (int i = 0; i < p_gvPase.Rows.Count; i++)
                    {
                        if (p_gvPase.Rows[i]["ID_PASE"].ToString() == p_datospp.Rows[item.ItemIndex]["ID_PASE"].ToString())
                        {
                            p_gvPase.Rows[item.ItemIndex]["CANCELAR"] = true;
                        }
                    }

                    DateTime fechasalida = new DateTime();
                    if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasalida))
                    {
                    }

                    p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"] = fechasalida; //.ToString("MM/dd/yyyy"); //Convert.ToDateTime(txtfecsalppcfs.Text).ToString("dd/MM/yyyy"); //+ " 00:00:00";
                    p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"] = fechasalida; //.ToString("MM/dd/yyyy"); 
                    sfechasalida = fechasalida.ToString("yyyy-MM-dd"); //txtfecsalpp.Text; //Convert.ToDateTime(lblFecAutPPWeb.Text.Trim()).ToString("yyyy-MM-dd");
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
            {
                TextBox chkPase = (TextBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                Label lblCntr = item.FindControl("lblCntr") as Label;

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
                DateTime fechasal_ = new DateTime();
                if (!string.IsNullOrEmpty(txtfecsalpp.Text))
                {
                    if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text));
                        txtfecsal.Focus();
                        fechasal_ = new DateTime();
                        var _fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString());
                        fechasal_ = _fechasal_;
                        /*
                        if (!DateTime.TryParseExact(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString(), "d/M/yyyy h:m:s", enUS, DateTimeStyles.None, out fechasal_))
                        {
                        }
                        */
                        txtfecsalpp.Text = fechasal_.ToString("dd/MM/yyyy");
                        return;
                    }
                }
                

                //txtfecsalpp.Text = txtfecsalpp.Text.Replace("/", "-");
                var dtvalfecha = pasePuerta.GetValFechaSalidaCFS(fechasal.ToString("yyyy-MM-dd"), p_datospp.Rows[item.ItemIndex]["FACTURA"].ToString(), txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblCntr.Text);
                if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                {
                    if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                    {
                        this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                        txtfecsal.Focus();
                        fechasal_ = new DateTime();
                        var _fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString());
                        fechasal_ = _fechasal_;
                        /*
                        if (!DateTime.TryParseExact(dsRetorno.Tables[0].Rows[item.ItemIndex]["FECHA_EXPIRACION"].ToString(), "d/M/yyyy h:m:s", enUS, DateTimeStyles.None, out fechasal_))
                        {
                        }
                        */
                        txtfecsalpp.Text = fechasal_.ToString("dd/MM/yyyy");
                        return;
                    }
                }
                
                
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                Char delimiter = ',';
                new List<string>();
                List<string> lsubstring = new List<string>();
                string xml = "";
                new List<string>();
                var substrings = p_datospp.Rows[item.ItemIndex]["SUB_SECUENCIA"].ToString().Split(delimiter);
                lsubstring = new List<string>();
                foreach (var substring in substrings)
                {
                    lsubstring.Add(substring);
                }
                xml = "";
                GenXml(lsubstring, out xml);
                DataTable dtHorarios = new DataTable();
                for (int i4 = 0; i4 < dsRetorno.Tables[0].Rows.Count; i4++)
                {
                    if (p_datospp.Rows[item.ItemIndex]["ID_PASE"].ToString() == dsRetorno.Tables[0].Rows[i4]["ID_PASE"].ToString())
                    {
                        var fechasalida = fechasal.ToString("yyyy-MM-dd");
                        
                        dtHorarios = pasePuerta.CONSULTA_HORARIOS_DISPONIBLES_(fechasalida, xml);
                        dtHorarios.Rows.Add(0, 0, "ELIJA", "", 0, false);
                        dtHorarios.DefaultView.Sort = "HORADESDE desc";
                        dtHorarios = dtHorarios.DefaultView.ToTable(true);
                        if (dtHorarios.Rows.Count > 0 || dtHorarios != null)
                        {
                            ddlTurno.DataTextField = "HORADESDE";
                            ddlTurno.DataValueField = "IDDISPONIBLEDET";
                            ddlTurno.DataSource = dtHorarios.AsDataView();
                            ddlTurno.DataBind();
                        }
                    }
                }
                //IniTurno();
            }
            catch (Exception ex)
            {
                if (ex.Message == "No se puede generar pase de puerta los domingos.")
                {
                    IniPaseWeb();
                    this.Alerta(ex.Message);
                }
                else
                {
                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "txtfecsalpp_TextChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                }
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