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
    public partial class cancelacion_actualizacion_pasepuerta_brbk : System.Web.UI.Page
    {
        private DataTable p_gvPase
        {
            get
            {
                return (DataTable)Session["gvPaseCancelarBrbrk"];
            }
            set
            {
                Session["gvPaseCancelarBrbrk"] = value;
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
                return (DataTable)Session["drChoferPPWebACBrbk"];
            }
            set
            {
                Session["drChoferPPWebACBrbk"] = value;
            }

        }

        private DataTable p_drChoferFilter
        {
            get
            {
                return (DataTable)Session["drChoferFilterPPWebCanBrbk"];
            }
            set
            {
                Session["drChoferFilterPPWebCanBrbk"] = value;
            }

        }

        private DataTable p_drEmpresa
        {
            get
            {
                return (DataTable)Session["drEmpresaPPWebACBrbk"];
            }
            set
            {
                Session["drEmpresaPPWebACBrbk"] = value;
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

        private DataTable p_gvBreakBulk
        {
            get
            {
                return (DataTable)Session["p_gvBreakBulkAct"];
            }
            set
            {
                Session["p_gvBreakBulkAct"] = value;
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
                    IniDsEmpresa();
                    IniRpt();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "cancelacion_actualizacion_pasepuerta_brbk", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                var number = log_csl.save_log<Exception>(ex, "cancelacion_actualizacion_pasepuerta_brbk", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                var number = log_csl.save_log<Exception>(ex, "cancelacion_actualizacion_pasepuerta_brbk", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
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
                                             join wdatapase in p_gvBreakBulk.AsEnumerable().AsParallel()
                                             on p.Field<Decimal>("ID_PASE").ToString() equals wdatapase.Field<Decimal>("ID_PASE").ToString()
                                             where p.Field<String>("ACTUALIZAR") == "True"
                                             select new XElement("VBS_P_PASE_PUERTA",
                                             new XAttribute("ID_PASE", p.Field<Decimal>("ID_PASE").ToString()),
                                             new XAttribute("PASE", p.Field<String>("NUMERO_PASE_N4") == null ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()),
                                             new XAttribute("TIPO_CARGA", wdatapase.Field<String>("TIPO_CARGA") == null ? "" : wdatapase.Field<String>("TIPO_CARGA").ToString()),
                                             new XAttribute("ESTADO", "CA"),
                                             new XAttribute("USUARIO_ESTADO", p_user),
                                             new XAttribute("FECHA_ESTADO", DateTime.Now.ToString("MM/dd/yyyy HH:mm")),
                                             new XAttribute("MOTIVO_CANCELACION", MotivoCan),
                                             new XAttribute("flag", "U"),
                                             new XAttribute("RESERVA", 1))));

            String[] docXMLN4 = (from p in p_datospp.AsEnumerable().AsParallel()
                                 where (p.Field<String>("ACTUALIZAR") == "True") && p.Field<String>("TIPO").Equals("CNTR")
                                 select new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                 new XElement("gate",
                                 new XElement("cancel-appointment",
                                 new XElement("appointments",
                                 new XElement("appointment",
                                 new XAttribute("appointment-nbr", (String.IsNullOrEmpty(p.Field<String>("NUMERO_PASE_N4").ToString()) == true ? "" : p.Field<String>("NUMERO_PASE_N4").ToString()))
                                 ))))).ToString()).ToArray();

            String[] docXMLN4brk = (from p in p_datospp.AsEnumerable().AsParallel()
                                    where (p.Field<String>("ACTUALIZAR") == "True")
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
                                    new XAttribute("value", "BRBK"))
                                    ))).ToString()).ToArray();

            String docXMLICU = null;

            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            DataSet ds_report = new DataSet();
            WPASEPUERTA.SaveCancelarPasePuerta(docXML.ToString(), docXMLN4, docXMLN4brk);
            pasePuerta.CANCELA_HORARIOS_PASEPUERTA(docXML.ToString(), Page.User.Identity.Name);
            /*CANCELA PASE DE PUERTA*/

            /*GENERA NUEVO PASE DE PUERTA*/
            DataTable dt = new DataTable();
            
            List<String> XMLN4BreakBulk = new List<String>();

            dt.Columns.Add(new DataColumn("CONSECUTIVO", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDEMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("PLACA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDCHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FECHA_AUT_PPWEB", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FECHA_SALIDA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CODSUBITEM", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("TIPO_CARGA", Type.GetType("System.String")));
            dt.AcceptChanges();
            
            var wdatos = (from wdata in p_gvBreakBulk.AsEnumerable().AsParallel()
                          join wdatapase in p_datospp.AsEnumerable().AsParallel()
                          on wdata.Field<Decimal>("ID_PASE").ToString() equals wdatapase.Field<Decimal>("ID_PASE").ToString()
                          where wdatapase.Field<String>("ACTUALIZAR") == "True"
                          select new
                          {
                              CONSECUTIVO = wdatapase.Field<Int64>("CONSECUTIVO").ToString(),
                              IDEMPRESA = wdatapase.Field<String>("CIATRANS"),
                              PLACA = wdatapase.Field<String>("PLACA"),
                              IDCHOFER = wdatapase.Field<String>("CHOFER"),
                              CANTIDAD = "1",
                              FECHA_AUT_PPWEB = wdatapase.Field<String>("FECHA_AUT_PPWEB"),
                              FECHA_SALIDA = wdatapase.Field<String>("FECHASALIDA"),
                              CARGA = wdata.Field<String>("CARGA"),
                              CANTPASES = wdata.Field<String>("INFORMACION").Substring(0, Convert.ToInt32(wdata.Field<String>("INFORMACION").IndexOf("(").ToString()) - 1)
                          }).ToList();

            foreach (DataRow wrow in LINQToDataTable(wdatos).Rows)
            {
                /*
                for (int i = 0; i < wdatos.Count; i++)
                {*/

                    dt.Rows.Add(new String[] { wrow["CONSECUTIVO"] == null ? "" : wrow["CONSECUTIVO"].ToString(),
                                                                   wrow["IDEMPRESA"] == null ? "" : wrow["IDEMPRESA"].ToString(),
                                                                   wrow["PLACA"] == null ? "" : wrow["PLACA"].ToString().ToUpper(),
                                                                   wrow["IDCHOFER"] == null ? "" :wrow["IDCHOFER"].ToString(),
                                                                    wrow["CANTIDAD"] == null ? "" : wrow["CANTIDAD"].ToString(),
                                                                    wrow["FECHA_AUT_PPWEB"] == null ? "" : wrow["FECHA_AUT_PPWEB"].ToString(),
                                                                    wrow["FECHA_SALIDA"] == null ? "" : wrow["FECHA_SALIDA"].ToString(),
                                                                    "",
                                                                    "BRBK",
                        });
                    dt.AcceptChanges();

                    String XMLN4 = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("groovy",
                        new System.Xml.Linq.XAttribute("class-location", "database"),
                        //    new System.Xml.Linq.XAttribute("class-location", "code-extension"),
                        new System.Xml.Linq.XAttribute("class-name", "CGSADeliveryOrderQty"),
                        //    new System.Xml.Linq.XAttribute("class-name", "CGSADeliveryOrderCreate"),
                            new System.Xml.Linq.XElement("parameters",
                            new System.Xml.Linq.XElement("parameter",
                            new System.Xml.Linq.XAttribute("id", "agencia"),
                            new System.Xml.Linq.XAttribute("value", wrow["IDEMPRESA"] == null ? "" : wrow["IDEMPRESA"].ToString())),
                            new System.Xml.Linq.XElement("parameter",
                            new System.Xml.Linq.XAttribute("id", "camion"),
                            new System.Xml.Linq.XAttribute("value", "")),
                            new System.Xml.Linq.XElement("parameter",
                                new System.Xml.Linq.XAttribute("id", "fecha"),
                                new System.Xml.Linq.XAttribute("value", wrow["FECHA_SALIDA"] + " 00:00")),
                                new System.Xml.Linq.XElement("parameter", new System.Xml.Linq.XAttribute("id", "referencia"),
                            new System.Xml.Linq.XAttribute("value", "")),
                            new System.Xml.Linq.XElement("parameter",
                                new System.Xml.Linq.XAttribute("id", "BLs"),
                                new System.Xml.Linq.XAttribute("value", wrow["CARGA"] == null ? "" : wrow["CARGA"].ToString())),
                                new System.Xml.Linq.XElement("parameter",
                                    new System.Xml.Linq.XAttribute("id", "QTY"),
                                    new System.Xml.Linq.XAttribute("value", wrow["CANTPASES"] == null ? "" : wrow["CANTPASES"].ToString())),
                                        new System.Xml.Linq.XElement("parameter",
                                        new System.Xml.Linq.XAttribute("id", "codsubitem"),
                                    new System.Xml.Linq.XAttribute("value", "")),
                                            new System.Xml.Linq.XElement("parameter",
                                new System.Xml.Linq.XAttribute("id", "placa"),
                                new System.Xml.Linq.XAttribute("value", wrow["PLACA"] == null ? "" : wrow["PLACA"].ToString().ToUpper())),
                                new System.Xml.Linq.XElement("parameter",
                                new System.Xml.Linq.XAttribute("id", "chofer"),
                                new System.Xml.Linq.XAttribute("value", wrow["IDCHOFER"] == null ? "" : wrow["IDCHOFER"].ToString())),
                                new System.Xml.Linq.XElement("parameter",
                                new System.Xml.Linq.XAttribute("id", "tipo_carga"),
                                new System.Xml.Linq.XAttribute("value", "BRBK")),
                                new System.Xml.Linq.XElement("parameter",
                            new System.Xml.Linq.XAttribute("id", "consecutivo"),
                                new System.Xml.Linq.XAttribute("value", wrow["CONSECUTIVO"] == null ? "" : wrow["CONSECUTIVO"].ToString())),
                                    new System.Xml.Linq.XElement("parameter",
                                new System.Xml.Linq.XAttribute("id", "usuer"),
                                new System.Xml.Linq.XAttribute("value", p_user))
                                ))).ToString();
                    XMLN4BreakBulk.Add(XMLN4);
                /*}*/
            }

            docXML = new XDocument();
            docXML = new System.Xml.Linq.XDocument(
            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
            new System.Xml.Linq.XElement("PASEPUERTA",
            from p in dt.AsEnumerable().AsParallel()
            select new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA",
            new System.Xml.Linq.XAttribute("ID_CARGA", p.Field<String>("CONSECUTIVO")),
            new System.Xml.Linq.XAttribute("ESTADO", "GN"),
            new System.Xml.Linq.XAttribute("FECHA_EXPIRACION", p.Field<String>("FECHA_AUT_PPWEB")),
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

            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            ds_report = new DataSet();
            ds_report = WPASEPUERTA.SavePasePuerta(docXML.ToString(), XMLN4BreakBulk.ToArray(), "BRBK");
            p_reportPasePuerta = new DataSet();
            if (ds_report == null || ds_report.Tables[0].Rows.Count < 0)
            {
                if (ds_report.Tables[0].TableName.Equals("DTError"))
                {
                    if (ds_report.Tables[0].Rows.Count > 0)
                    {
                        this.Alerta(ds_report.Tables[0].Rows[0]["CARGA"].ToString() + " - " + ds_report.Tables[0].Rows[0]["MENSAJE"]);
                        return;
                    }
                    else
                    {
                        this.Alerta("No se pudo Generar el Pase de Puerta, Carga sin Stock o Consulte N4.");
                        return;
                    }

                }
            }
            else
            {
                /*Pase Puerta BRBK*/
                DataTable resultado = new DataTable();
                DataView view = new DataView();
                var resultss = from myRow in p_datospp.AsEnumerable()
                               where myRow.Field<String>("ACTUALIZAR") == "True"
                               select myRow;
                view = resultss.AsDataView();
                resultado = view.ToTable();
                resultado.Columns.Add("BODEGA");
                var bodega = pasePuerta.GetBodegaCargaBRBK(p_datospp.Rows[0]["CONSECUTIVO"].ToString());
                for (int ic = 0; ic < resultado.Rows.Count; ic++)
                {
                    resultado.Rows[ic]["BODEGA"] = bodega.Rows[0][0].ToString();
                }

                StringWriter writer = new StringWriter();
                resultado.TableName = "Cfs";
                resultado.WriteXml(writer);

                StringWriter writerdt = new StringWriter();
                int i = 0;
                DataTable Dreporte = new DataTable();
                Dreporte = ds_report.Tables[i].Copy();
                p_reportPasePuerta.Tables.Add(Dreporte);

                Dreporte.TableName = "Cfs";
                Dreporte.WriteXml(writerdt);

                string msjerror = string.Empty;
                if (!pasePuerta.InsertaReservaDeTurnoActualizacion(writer.ToString(), writerdt.ToString(), "", 0, Page.User.Identity.Name, out msjerror))
                {
                    this.Alerta(msjerror);
                    return;
                }

                var dtPPWeb = (from wppweb in ds_report.Tables[0].AsEnumerable()
                               select wppweb).AsDataView().ToTable();
                StringWriter xmlPPWeb = new StringWriter();
                dtPPWeb.TableName = "PaseWeb";
                dtPPWeb.WriteXml(xmlPPWeb);

                var dtPPWebBRBK = (from wppweb in p_gvBreakBulk.AsEnumerable()
                                   select wppweb).AsDataView().ToTable();
                StringWriter xmlPPWebBRBK = new StringWriter();
                dtPPWebBRBK.TableName = "PaseWebBRBK";
                dtPPWebBRBK.WriteXml(xmlPPWebBRBK);
                /*
                var dtPPWebBRBKDet = (from wppweb in p_drpasepuertabreakbulk.AsEnumerable()
                                      select wppweb).AsDataView().ToTable();
                StringWriter xmlPPWebBRBKDet = new StringWriter();
                dtPPWebBRBKDet.TableName = "PaseWebDet";
                dtPPWebBRBKDet.WriteXml(xmlPPWebBRBKDet);
                */
                String WfACTURADOA = p_datospp.Rows[0]["FACTURADO"].ToString();
                String wuser = p_user;
                String wsAgente = p_datospp.Rows[0]["AGENTE"].ToString();
                String wempr = p_datospp.Rows[0]["FACTURADO"].ToString();
                String Factura = p_datospp.Rows[0]["FACTURA"].ToString();
                msjerror = string.Empty;

                if (!pasePuerta.AcualizaPaseWebBRBK(xmlPPWeb.ToString(), xmlPPWebBRBK.ToString(), ""/*xmlPPWebBRBKDet.ToString()*/, Factura, wsAgente, wempr, "", "", "", p_user, out msjerror))
                {
                    this.Alerta(msjerror);
                    return;
                }
            }
            /*GENERA NUEVO PASE DE PUERTA*/
            this.Alerta("e-Pass procesado Exitosamente... ");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            IniPaseWeb();
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
            p_datospp = pasePuerta.GetDatosActualizaCancelaPaseWebBRBKS3(this.agencia.Value, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), txtcntr.Text.Trim(), txtfecsal.Text != "" ? fechasal.ToString("yyyy-MM-dd") : txtfecsal.Text.Trim());
            if (p_datospp.Rows.Count == 0)
            {
                this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                return;
            }

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

            p_datospp.Columns.Add("ACTUALIZAR");
            p_datospp.Columns.Add("CANCELAR");
            p_datospp.Columns.Add("TIPO");
            p_datospp.Columns.Add("FECHA_AUT_PPWEB");
            p_datospp.Columns.Add("FECHASALIDA");
            p_datospp.Columns.Add("CHOFER");
            p_datospp.Columns.Add("PLACA");
            p_datospp.Columns.Add("ID_TURNO");
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
            p_gvBreakBulk = dsRetorno.Tables[0];

            for (int i = 0; i < p_datospp.Rows.Count; i++)
            {
                for (int i2 = 0; i2 < dsRetorno.Tables[0].Rows.Count; i2++)
                {
                    if (p_datospp.Rows[i]["ID_PASE"].ToString() == dsRetorno.Tables[0].Rows[i2]["ID_PASE"].ToString())
                    {
                        fechasal = new DateTime();
                        /*
                        var fecha_expiracion = dsRetorno.Tables[0].Rows[i2]["FECHA_EXPIRACION"].ToString();
                        if (!DateTime.TryParseExact(fecha_expiracion, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                        {
                            this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", fecha_expiracion));
                            return;
                        }
                        */
                        var fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[i2]["FECHA_EXPIRACION"].ToString());
                        fechasal = fechasal_;
                        
                        p_datospp.Rows[i]["FECHA_AUT_PPWEB"] = fechasal.ToString("dd/MM/yyyy");
                    }
                }
            }
            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                LinkButton lnkImprimir = item.FindControl("lnkImprimir") as LinkButton;
                string url = "../facturacion/impresion-pase-de-puerta-carga-breakbull?opcion=ReprePasePuerta&Carga=" + p_datospp.Rows[item.ItemIndex]["CARGA"].ToString() + "&Pase=" + p_datospp.Rows[item.ItemIndex]["NUMERO_PASE_N4"].ToString() + "&Tipo=" + "BRBK";
                lnkImprimir.Attributes.Add("onClick", "JavaScript: window.open('" + url + "','Reimpresion Pase de Puerta','width=700,height=700')");
                p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = "False";
                p_datospp.Rows[item.ItemIndex]["CANCELAR"] = "False";

                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;

                DataTable dtHorarios = new DataTable();
                for (int i4 = 0; i4 < dsRetorno.Tables[0].Rows.Count; i4++)
                {
                    if (p_datospp.Rows[item.ItemIndex]["ID_PASE"].ToString() == dsRetorno.Tables[0].Rows[i4]["ID_PASE"].ToString())
                    {
                        fechasal = new DateTime();
                        
                        var fechasal_ = Convert.ToDateTime(dsRetorno.Tables[0].Rows[i4]["FECHA_EXPIRACION"].ToString());
                        fechasal = fechasal_;
                        
                        /*
                        var fecha_expiracion = dsRetorno.Tables[0].Rows[i4]["FECHA_EXPIRACION"].ToString();
                        if (!DateTime.TryParseExact(fecha_expiracion, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                        {
                            this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", fecha_expiracion));
                            return;
                        }
                        */
                        var bodega = pasePuerta.GetBodegaCargaBRBK(p_datospp.Rows[item.ItemIndex]["CONSECUTIVO"].ToString());
                        dtHorarios = new DataTable();
                        dtHorarios = pasePuerta.GetTurnosDisponiblesBRBK(fechasal.ToString("MM/dd/yyyy"), bodega.Rows[0]["BODEGA"].ToString());
                        dtHorarios.Rows.Add(0, "ELIJA", "", 0, "", false);
                        if (dtHorarios.Rows.Count > 0 || dtHorarios != null)
                        {
                            dtHorarios.DefaultView.Sort = "HORADESDE desc";
                            dtHorarios = dtHorarios.DefaultView.ToTable(true);
                            ddlTurno.DataTextField = "HORADESDE";
                            ddlTurno.DataValueField = "IDDISPONIBLEDET";
                            ddlTurno.DataSource = dtHorarios.AsDataView();
                            ddlTurno.DataBind();
                        }
                        else
                        {
                            ddlTurno.DataSource = dtHorarios.AsDataView();
                            ddlTurno.DataBind();
                        }
                    }
                }
            }

            p_gvPase = dsRetorno.Tables[0];
            lblTotCntr.Text = "Tot. Pases: " + p_datospp.Rows.Count.ToString();
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

                    var dthorasrf = pasePuerta.GetTurnosInfoReefer(xmlTurnos.ToString(), dfechasalida.ToString("yyyy-MM-dd HH:mm") /*p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"].ToString()*/);
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

        private void IniDsChofer(string empresa)
        {
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            dsRetorno = WPASEPUERTA.GetChoferinfo();

            for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
            {
                dsRetorno.Tables[0].Rows[i]["CHOFER"] = dsRetorno.Tables[0].Rows[i]["CHOFER"].ToString().Replace("-  -", "-");
            }

            p_drChofer = dsRetorno.Tables[0];
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
                //DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                CheckBox chkCanPase = item.FindControl("chkCanPase") as CheckBox;

                TextBox txtfecsalpp = item.FindControl("lblFecAutPPWeb") as TextBox;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();

                if (chkPase.Checked)
                {
                    chkCanPase.Checked = false;
                    if (ddlTurno.SelectedItem.Value == "0")
                    {
                        this.Alerta("Seleccione el Turno.");
                        chkPase.Checked = false;
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtfecsalpp.Text))
                    {
                        if (!DateTime.TryParseExact(txtfecsalpp.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                        {
                            this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text));
                            txtfecsal.Focus();
                            txtfecsalpp.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"].ToString();
                            return;
                        }
                    }
                    else if (string.IsNullOrEmpty(txtfecsalpp.Text))
                    {
                        this.Alerta("Seleccione la Fecha.");
                        txtfecsalpp.Focus();
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

                    if (ddlTurno.SelectedItem.Value != "0")
                    {
                        var valcfsdo = pasePuerta.GetValFechaDomingosCFS(fechasal.ToString("yyyy-MM-dd"));
                        if (valcfsdo.Rows[0]["VAL"].ToString() == "1")
                        {
                            this.Alerta(valcfsdo.Rows[0]["MENSAJE"].ToString());
                            lblFecAutPPWeb.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"].ToString();
                            return;
                        }
                        var dtvalfecha = pasePuerta.GetValFechaSalidaBRBK(fechasal.ToString("yyyy-MM-dd"), p_datospp.Rows[item.ItemIndex]["FACTURA"].ToString(), txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                        if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                        {
                            if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                            {
                                this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                                lblFecAutPPWeb.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"].ToString();
                                return;
                            }
                        }
                    }

                    TextBox TxtGEmpresa = item.FindControl("TxtGEmpresa") as TextBox;
                    if (string.IsNullOrEmpty(TxtGEmpresa.Text))//ddlEmpresa.SelectedItem.Value == "0")
                    {
                        this.Alerta("Elija una Cia. Trans.");
                        chkPase.Checked = false;
                        return;
                    }

                    DataTable DTRESULT = new DataTable();
                    DTRESULT = (DataTable)HttpContext.Current.Session["drEmpresaPPWebACBrbk"];
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
                    DTRESULT2 = (DataTable)HttpContext.Current.Session["drChoferPPWebACBrbk"];
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
                    
                    TextBox txtPlaca = item.FindControl("txtPlaca") as TextBox;
                    if (!string.IsNullOrEmpty(TxtGChofer.Text)) //ddlChofer.SelectedItem.Value != "0")
                    {
                        /*if (string.IsNullOrEmpty(txtPlaca.Text))
                        {
                            this.Alerta("Escriba una Placa.");
                            chkPase.Checked = false;
                            return;
                        }
                        else
                        {*/                            
                            //p_datospp.Rows[item.ItemIndex]["CHOFER"] = ddlChofer.SelectedItem.Value;
                            //p_datospp.Rows[item.ItemIndex]["PLACA"] = txtPlaca.Text.Trim();
                            String value_ = TxtGEmpresa.Text.Trim();
                            Char delimiter_ = '-';
                            List<string> substringemp = value_.Split(delimiter_).ToList();
                            //IniDsPlaca(ddlEmpresa.SelectedItem.Value);
                            var wPlaca = (from row in p_drPlaca.AsEnumerable()
                                          where row.Field<String>("EMPRESA") == substringemp[0].Trim() && row.Field<String>("PLACA") != null && row.Field<String>("PLACA").Trim().Equals(txtPlaca.Text.ToString().Trim().ToUpper())
                                          select row.Field<String>("PLACA")).Count();

                            /*if ((int)wPlaca <= 0)
                            {
                                
                                this.Alerta("La Placa " + txtPlaca.Text +  ", no es valida.");
                                chkPase.Checked = false;
                                return;
                                
                            }
                            else
                            {*/
                                String value = TxtGChofer.Text.Trim();
                                Char delimiter = '-';
                                List<string> substringchof = value.Split(delimiter).ToList();
                                p_datospp.Rows[item.ItemIndex]["CHOFER"] = substringchof[0].Trim();
                                p_datospp.Rows[item.ItemIndex]["PLACA"] = txtPlaca.Text.Trim();
                            /*}*/
                        /*}*/
                    }

                    if (ddlTurno.SelectedItem.Value != "0")
                    {
                        //p_datospp.Rows[item.ItemIndex]["TURNO"] = ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[0].Trim();
                        //p_datospp.Rows[item.ItemIndex]["D_TURNO"] = ddlTurno.SelectedItem.Text;
                        p_datospp.Rows[item.ItemIndex]["ID_TURNO"] = ddlTurno.SelectedItem.Value.ToString(); //.Split('-').ToList()[1].Trim();
                    }
                    
                    if (!string.IsNullOrEmpty(TxtGEmpresa.Text.Trim()))//ddlEmpresa.SelectedItem.Value != "0")
                    {
                        String value = TxtGEmpresa.Text.Trim();
                        Char delimiter = '-';
                        List<string> substringemp = value.Split(delimiter).ToList();
                        p_datospp.Columns["CIATRANS"].ReadOnly = false;
                        p_datospp.Rows[item.ItemIndex]["CIATRANS"] = substringemp[0].Trim() == "0" ? "" : substringemp[0].Trim();
                    }
                    p_datospp.Rows[item.ItemIndex]["ACTUALIZAR"] = chkPase.Checked.ToString();
                    if (chkPase.Checked)
                    {
                        p_datospp.Rows[item.ItemIndex]["TIPO"] = "A";
                    }
                    else
                    {
                        p_datospp.Rows[item.ItemIndex]["TIPO"] = "C";
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
            {
                TextBox chkPase = (TextBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;
                TextBox lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as TextBox;

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!string.IsNullOrEmpty(lblFecAutPPWeb.Text))
                {
                    if (!DateTime.TryParseExact(lblFecAutPPWeb.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", lblFecAutPPWeb.Text));
                        lblFecAutPPWeb.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"].ToString();
                        return;
                    }
                }
                else
                {
                    this.Alerta("Ingrese la Fecha de Salida.");
                    return;
                }

                var valcfsdo = pasePuerta.GetValFechaDomingosCFS(fechasal.ToString("yyyy-MM-dd"));
                if (valcfsdo.Rows[0]["VAL"].ToString() == "1")
                {
                    this.Alerta(valcfsdo.Rows[0]["MENSAJE"].ToString());
                    lblFecAutPPWeb.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"].ToString();
                    return;
                }
                //-->

                var dtvalfecha = pasePuerta.GetValFechaSalidaBRBK(fechasal.ToString("yyyy-MM-dd"), p_datospp.Rows[item.ItemIndex]["FACTURA"].ToString(), txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                {
                    if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                    {
                        this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                        lblFecAutPPWeb.Text = p_datospp.Rows[item.ItemIndex]["FECHA_AUT_PPWEB"].ToString();
                        return;
                    }
                }
                var sfechasalidabrbk = fechasal.ToString("MM/dd/yyyy");

                var bodega = pasePuerta.GetBodegaCargaBRBK(p_datospp.Rows[item.ItemIndex]["CONSECUTIVO"].ToString());
                if (bodega == null)
                {
                    this.Alerta("No se encontraron ubicación o bodega de la carga.");
                    return;
                }
                else if (bodega.Rows.Count == 0)
                {
                    this.Alerta("No se encontraron ubicación o bodega de la carga.");
                    return;
                }

                var dtHorarios = new DataTable();
                dtHorarios = pasePuerta.GetTurnosDisponiblesBRBK(sfechasalidabrbk, bodega.Rows[0]["BODEGA"].ToString());
                dtHorarios.Rows.Add(0, "ELIJA", "", 0, "", false);
                if (dtHorarios.Rows.Count > 0 || dtHorarios != null)
                {
                    dtHorarios.DefaultView.Sort = "HORADESDE desc";
                    dtHorarios = dtHorarios.DefaultView.ToTable(true);
                    ddlTurno.DataTextField = "HORADESDE";
                    ddlTurno.DataValueField = "IDDISPONIBLEDET";
                    ddlTurno.DataSource = dtHorarios.AsDataView();
                    ddlTurno.DataBind();
                }
                else
                {
                    this.Alerta("No se encontraron horarios, revise la fecha.");
                    ddlTurno.DataSource = dtHorarios.AsDataView();
                    ddlTurno.DataBind();
                    return;
                }
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

            DTRESULT = (DataTable)HttpContext.Current.Session["drEmpresaPPWebACBrbk"];
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
            DTRESULT = (DataTable)HttpContext.Current.Session["drChoferPPWebACBrbk"];//drChoferPPWeb"];

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

    }
}