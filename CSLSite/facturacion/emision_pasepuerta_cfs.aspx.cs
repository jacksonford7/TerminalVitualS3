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
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.Reflection;

namespace CSLSite.facturacion
{
    public partial class emision_pasepuerta_cfs : System.Web.UI.Page
    {
        public List<String> xmllistsubitems
        {
            get { return (List<String>)Session["xmllistsubitems"]; }
            set { Session["xmllistsubitems"] = value; }
        }

        private DataTable p_drCliente
        {
            get
            {
                return (DataTable)Session["drClientecfs"];
            }
            set
            {
                Session["drClientecfs"] = value;
            }

        }

        private String factura_pp_cfs
        {
            get
            {
                return (String)Session["factura_pp_cfs"];
            }
            set
            {
                Session["factura_pp_cfs"] = value;
            }

        }

        private String id_empresa
        {
            get
            {
                return (String)Session["id_empresaCFS"];
            }
            set
            {
                Session["id_empresaCFS"] = value;
            }

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
                return (DataTable)Session["gvDetallePPWeb"];
            }
            set
            {
                Session["gvDetallePPWeb"] = value;
            }

        }

        private DataTable p_datospp
        {
            get
            {
                return (DataTable)Session["p_datosppWeb"];
            }
            set
            {
                Session["p_datosppWeb"] = value;
            }

        }

        private DataSet p_reportPasePuerta
        {
            get
            {
                return (DataSet)Session["dsPasePuerta"];
            }
            set
            {
                Session["dsPasePuerta"] = value;
            }

        }

        private DataTable p_drChofer
        {
            get
            {
                return (DataTable)Session["drChoferPPWeb"];
            }
            set
            {
                Session["drChoferPPWeb"] = value;
            }

        }

        private DataTable p_drChoferFilter
        {
            get
            {
                return (DataTable)Session["drChoferFilterPPWeb"];
            }
            set
            {
                Session["drChoferFilterPPWeb"] = value;
            }

        }

        private DataTable p_drEmpresa
        {
            get
            {
                return (DataTable)Session["drEmpresaPPWeb"];
            }
            set
            {
                Session["drEmpresaPPWeb"] = value;
            }

        }

        public String emailClientePPWeb
        {
            get { return (String)Session["emailClientePPWeb"]; }
            set { Session["emailClientePPWeb"] = value; }
        }

        private DataTable p_drPlaca
        {
            get
            {
                return (DataTable)Session["drPlacaPPWeb"];
            }
            set
            {
                Session["drPlacaPPWeb"] = value;
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

        public List<String> listsubitems
        {
            get { return (List<String>)Session["listsubitemscfs"]; }
            set { Session["listsubitemscfs"] = value; }
        }


        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;

        protected void btnSeguir_Click(object sender, EventArgs e)
        {
            modalFacturasCliente.Hide();
            //UPMODETALLECFS.Update();
            //MODALCFS.Show();
        }

        protected void btnSalirFac_Click(object sender, EventArgs e)
        {
            modalFacturasCliente.Hide();
        }

        protected void txtfecsalppcfs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtfecsalppcfs.Text))
                {
                    this.Alerta("Ingrese una fecha de salida..");
                    MODALCFS.Show();
                    return;
                }

                //UPMODETALLECFS.Update();
                
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!string.IsNullOrEmpty(txtfecsalppcfs.Text))
                {
                    if (!DateTime.TryParseExact(txtfecsalppcfs.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsalppcfs.Text));
                        MODALCFS.Show();
                        txtfecsal.Focus();
                        return;
                    }
                }

                var dtvalfecha = pasePuerta.GetValFechaSalida(fechasal.ToString("yyyy-MM-dd"), factura_pp_cfs, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                {
                    if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                    {
                        this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                        txtfecsalppcfs.Text = p_gvBreakBulk.Rows[0]["FECHA_AUT_PPWEB_"].ToString();
                        MODALCFS.Show();
                        return;
                    }
                }

                //p_gvBreakBulk.Rows[0]["FECHA_AUT_PPWEB"] = fechasal.ToString("MM/dd/yyyy");
                /*
                DateTime fechasalida = new DateTime();
                if (!DateTime.TryParseExact(txtfecsalppcfs.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasalida))
                {
                }
                p_gvBreakBulk.Rows[0]["FECHA_SALIDA"] = fechasalida; //.ToString("MM/dd/yyyy"); //Convert.ToDateTime(txtfecsalppcfs.Text).ToString("dd/MM/yyyy"); //+ " 00:00:00";
                p_gvBreakBulk.Rows[0]["FECHA_AUT_PPWEB"] = fechasalida; //.ToString("MM/dd/yyyy"); 
                */
                //btnConsultar.Visible = true;
                gvHorarios.DataSource = null;
                gvHorarios.DataBind();
                MODALCFS.Show();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "txtfecsalppcfs_TextChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void btnContinuarPP_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                //GridViewRow row = ((GridViewRow)((Button)sender).NamingContainer);
                //Label lblFacturaPP = row.FindControl("lblFacturaPP") as Label;
                Label lblFacturaPP = (Label)gvFacturas.Rows[0].Cells[0].FindControl("lblFacturaPP");
                //Label lblEstadoPP = row.FindControl("lblEstadoPP") as Label;
                Label lblEstadoPP = (Label)gvFacturas.Rows[0].Cells[1].FindControl("lblEstadoPP");

                String MRN = txtmrn.Text.Trim();
                String MSN = txtmsn.Text.Trim();
                String HSN = txthsn.Text.Trim();
                p_gvBreakBulk = new DataTable();
                p_gvBreakBulk = pasePuerta.GetInfoPasePuertaCFS(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFacturaPP.Text.Trim());
                String ID_CARGA = p_gvBreakBulk.Rows[0]["CONSECUTIVO"].ToString();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                   new System.Xml.Linq.XElement("GetCfsSubItems", new System.Xml.Linq.XElement("GetCfsSubItem",
                                                   new System.Xml.Linq.XAttribute("MRN", String.IsNullOrEmpty(MRN) != true ? MRN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("MSN", String.IsNullOrEmpty(MSN) != true ? MSN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("HSN", String.IsNullOrEmpty(HSN) != true ? HSN.ToString().Trim() : "0000"),
                                                   new System.Xml.Linq.XAttribute("ID_CARGA", String.IsNullOrEmpty(ID_CARGA) != true ? ID_CARGA.ToString().Trim() : "")
                                                   )));
                DataSet dsRetorno = WPASEPUERTA.GetCFSSubIteminfo(docXML.ToString());
                var p_gvCfsTemp_ = dsRetorno.Tables[0];
                if (p_gvCfsTemp_ != null)
                {
                    if (p_gvCfsTemp_.Rows.Count > 0)
                    {
                        factura_pp_cfs = lblFacturaPP.Text.Trim();
                        modalFacturasCliente.Hide();
                        REFRESSCFS(factura_pp_cfs);
                        IniCfs();
                        //UPMODETALLECFS.Update();
                        MODALCFS.Show();
                        return;
                    }
                }


                if (lblEstadoPP.Text == "GENERADO")
                {
                    this.Alerta("Pase a Puerta ya se encuentra GENERADO... ");
                    modalFacturasCliente.Show();
                    return;
                }
                else if (lblEstadoPP.Text == "CANCELADO")
                {
                    this.Alerta("Pase a Puerta ya se encuentra CANCELADO... ");
                    modalFacturasCliente.Show();
                    return;
                }
                else
                {

                    factura_pp_cfs = lblFacturaPP.Text.Trim();
                    modalFacturasCliente.Hide();
                    IniDsEmpresa();
                    IniDsChofer();
                    IniDsPlaca();
                    REFRESSCFS(factura_pp_cfs);
                    IniCfs();
                    //UPMODETALLECFS.Update();
                    MODALCFS.Show();
                }
                */
            }
            catch (Exception ex)
            {
                //utilform.MessageBox(ex.Message, this);
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btnConsultar_Click", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        private void IniCfs()
        {

            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID", typeof(int));
            dcID.AutoIncrement = true;
            dcID.AutoIncrementSeed = 1;
            dcID.AutoIncrementStep = 1;

            dt.Columns.Add(dcID);


            dt.Columns.Add(new DataColumn("CONSECUTIVO", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("EMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDEMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("PLACA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDCHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CARGA", Type.GetType("System.String")));

            dt.AcceptChanges();

            p_gvCfs = dt;
        }

        public void REFRESSCFS(String lblFactura)
        {
            try
            {
                p_gvBreakBulk = new DataTable();
                p_gvBreakBulk = pasePuerta.GetInfoPasePuertaCFSS3(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFactura);
                if (p_gvBreakBulk.Rows.Count == 0)
                {
                    //utilform.MessageBox("No se encontraron datos, revise los criterios de consulta.", this);
                    this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    return;
                }
                p_gvBreakBulk.Columns.Add("FECHA_AUT_PPWEB");
           
                p_gvBreakBulk.Columns.Add("FECHA_SALIDA", typeof(DateTime));
                String ID_CARGA = p_gvBreakBulk.Rows[0]["CONSECUTIVO"].ToString();

                var wcarga = (from carga in p_gvBreakBulk.AsEnumerable()
                              where carga.Field<String>("CONSECUTIVO") == ID_CARGA
                              select new
                              {
                                  SCARGA = carga.Field<String>("MRN") + "-" + carga.Field<String>("MSN") + "-" + carga.Field<String>("HSN"),
                                  DOCUMENTO = carga.Field<String>("DOCUMENTO"),
                                  TIPO_CARGA = carga.Field<String>("SEGUNDA"),
                                  MRN = carga.Field<String>("MRN"),
                                  MSN = carga.Field<String>("MSN"),
                                  HSN = carga.Field<String>("HSN"),

                              }).FirstOrDefault();

                p_breabulkcarga = wcarga.SCARGA.ToString();


                String MRN = p_breabulkcarga.Split('-').ToList()[0].ToString();
                String MSN = p_breabulkcarga.Split('-').ToList()[1].ToString();
                String HSN = p_breabulkcarga.Split('-').ToList()[2].ToString();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                   new System.Xml.Linq.XElement("GetCfsSubItems", new System.Xml.Linq.XElement("GetCfsSubItem",
                                                   new System.Xml.Linq.XAttribute("MRN", String.IsNullOrEmpty(MRN) != true ? MRN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("MSN", String.IsNullOrEmpty(MSN) != true ? MSN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("HSN", String.IsNullOrEmpty(HSN) != true ? HSN.ToString().Trim() : "0000"),
                                                   new System.Xml.Linq.XAttribute("ID_CARGA", String.IsNullOrEmpty(ID_CARGA) != true ? ID_CARGA.ToString().Trim() : "")
                                                   )));
                DataSet dsRetorno = WPASEPUERTA.GetCFSSubIteminfo(docXML.ToString());
                p_gvCfsTemp = dsRetorno.Tables[0];

                if (p_gvCfs != null)
                {

                    var listPN = (from re in p_gvCfs.AsEnumerable().Where(TR => TR.Field<String>("CARGA") == p_breabulkcarga)
                                  select re).ToList();
                    if (listPN != null && listPN.Count > 0)
                    {
                        gvCFS.DataSource = listPN.CopyToDataTable<DataRow>();

                        gvCFS.DataBind();
                    }
                    else
                    {
                        gvCFS.DataSource = null;
                        gvCFS.DataBind();
                    }

                    var list = (from re in p_gvCfs.AsEnumerable().Where(TR => TR.Field<String>("CARGA") == p_breabulkcarga).SelectMany(X => X.Field<String>("CONSECUTIVO").Split(','))
                                select re).ToArray();

                    if (list.Count() > 0)
                    {
                        var resul = (from te in p_gvCfsTemp.AsEnumerable().Where(fu => !list.Contains(fu.Field<Decimal>("CONSECUTIVO").ToString()))
                                     select te).ToList();

                        if (resul != null && resul.Count > 0)
                        {
                            p_gvCfsTemp = resul.CopyToDataTable<DataRow>();
                        }
                        else
                        {
                            p_gvCfsTemp = null;
                        }
                    }


                }

                if (p_gvCfsTemp != null)
                {
                    if (p_gvCfsTemp.Rows.Count > 0)
                    {
                        p_gvCfsTemp.Columns.Add("CONSECUTIVO_VEH");
                        for (int i = 0; i < p_gvCfsTemp.Rows.Count; i++)
                        {
                            var vehiculo = pasePuerta.GetVehiculoSubSec(p_gvCfsTemp.Rows[i]["CONSECUTIVO"].ToString());
                            if (vehiculo != null)
                            {
                                if (vehiculo.Rows.Count > 0)
                                {
                                    if (vehiculo.Rows[0]["VEHICULO"].ToString() == "1")
                                    {
                                        p_gvCfsTemp.Rows[i]["CONSECUTIVO_VEH"] = p_gvCfsTemp.Rows[0]["CONSECUTIVO"].ToString() + " (" + vehiculo.Rows[0]["MENSAJE"].ToString() + ")";
                                    }
                                    else
                                    {
                                        p_gvCfsTemp.Rows[i]["CONSECUTIVO_VEH"] = p_gvCfsTemp.Rows[i]["CONSECUTIVO"].ToString();
                                    }
                                }
                                else
                                {
                                    p_gvCfsTemp.Rows[i]["CONSECUTIVO_VEH"] = p_gvCfsTemp.Rows[i]["CONSECUTIVO"].ToString();
                                }
                            }
                        }
                    }
                }

                GvCfsTemp.DataSource = p_gvCfsTemp;
                GvCfsTemp.DataBind();

                if (p_gvCfsTemp != null)
                {
                    DataTable resultadotrue = new DataTable();
                    DataView viewtrue = new DataView();
                    var resulttrue = from myRow in p_gvCfsTemp.AsEnumerable()
                                     where myRow.Field<bool>("ASIGNADOPN") == true
                                     select myRow;
                    viewtrue = resulttrue.AsDataView();
                    resultadotrue = viewtrue.ToTable();
                    for (int i = 0; i <= resultadotrue.Rows.Count - 1; i++)
                    {
                        listsubitems.Add(resultadotrue.Rows[i][1].ToString());
                    }

                    DataTable resultadofalse = new DataTable();
                    DataView viewfalse = new DataView();
                    var resultsfalse = from myRow in p_gvCfsTemp.AsEnumerable()
                                       where myRow.Field<bool>("ASIGNADOPN") == false
                                       select myRow;
                    viewfalse = resultsfalse.AsDataView();
                    resultadofalse = viewfalse.ToTable();

                    for (int i = 0; i <= resultadofalse.Rows.Count - 1; i++)
                    {
                        listsubitems.Remove(resultadofalse.Rows[i][1].ToString());
                    }
                }

                txtfecsalppcfs.Text = p_gvBreakBulk.Rows[0]["FECHA_AUT_PPWEB_"].ToString();
                
                txtmrnmsnhsnppcfs.Text = wcarga.SCARGA.ToString();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btnBuscar_Click();REFRESSCFS", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));   
            }

        }

        public void REFRESSCFSSALIR(String lblFactura)
        {
            try
            {
                p_gvBreakBulk = new DataTable();
                p_gvBreakBulk = pasePuerta.GetInfoPasePuertaCFS(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFactura);
                if (p_gvBreakBulk.Rows.Count == 0)
                {
                    //utilform.MessageBox("No se encontraron datos, revise los criterios de consulta.", this);
                    //this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    return;
                }

                String ID_CARGA = p_gvBreakBulk.Rows[0]["CONSECUTIVO"].ToString();
                IniCfs();
                var wcarga = (from carga in p_gvBreakBulk.AsEnumerable()
                              where carga.Field<String>("CONSECUTIVO") == ID_CARGA
                              select new
                              {
                                  SCARGA = carga.Field<String>("MRN") + "-" + carga.Field<String>("MSN") + "-" + carga.Field<String>("HSN"),
                                  DOCUMENTO = carga.Field<String>("DOCUMENTO"),
                                  TIPO_CARGA = carga.Field<String>("SEGUNDA"),
                                  MRN = carga.Field<String>("MRN"),
                                  MSN = carga.Field<String>("MSN"),
                                  HSN = carga.Field<String>("HSN"),

                              }).FirstOrDefault();

                p_breabulkcarga = wcarga.SCARGA.ToString();

                String MRN = p_breabulkcarga.Split('-').ToList()[0].ToString();
                String MSN = p_breabulkcarga.Split('-').ToList()[1].ToString();
                String HSN = p_breabulkcarga.Split('-').ToList()[2].ToString();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                   new System.Xml.Linq.XElement("GetCfsSubItems", new System.Xml.Linq.XElement("GetCfsSubItem",
                                                   new System.Xml.Linq.XAttribute("MRN", String.IsNullOrEmpty(MRN) != true ? MRN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("MSN", String.IsNullOrEmpty(MSN) != true ? MSN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("HSN", String.IsNullOrEmpty(HSN) != true ? HSN.ToString().Trim() : "0000"),
                                                   new System.Xml.Linq.XAttribute("ID_CARGA", String.IsNullOrEmpty(ID_CARGA) != true ? ID_CARGA.ToString().Trim() : "")
                                                   )));
                DataSet dsRetorno = WPASEPUERTA.GetCFSSubIteminfo(docXML.ToString());
                p_gvCfsTemp = dsRetorno.Tables[0];

                if (p_gvCfs != null)
                {

                    var listPN = (from re in p_gvCfs.AsEnumerable().Where(TR => TR.Field<String>("CARGA") == p_breabulkcarga)
                                  select re).ToList();
                    if (listPN != null && listPN.Count > 0)
                    {
                        gvCFS.DataSource = listPN.CopyToDataTable<DataRow>();

                        gvCFS.DataBind();
                    }
                    else
                    {
                        gvCFS.DataSource = null;
                        gvCFS.DataBind();
                    }

                    var list = (from re in p_gvCfs.AsEnumerable().Where(TR => TR.Field<String>("CARGA") == p_breabulkcarga).SelectMany(X => X.Field<String>("CONSECUTIVO").Split(','))
                                select re).ToArray();

                    if (list.Count() > 0)
                    {
                        var resul = (from te in p_gvCfsTemp.AsEnumerable().Where(fu => !list.Contains(fu.Field<Decimal>("CONSECUTIVO").ToString()))
                                     select te).ToList();

                        if (resul != null && resul.Count > 0)
                        {
                            p_gvCfsTemp = resul.CopyToDataTable<DataRow>();
                        }
                        else
                        {
                            p_gvCfsTemp = null;
                        }
                    }


                }

                if (p_gvCfsTemp != null)
                {
                    if (p_gvCfsTemp.Rows.Count > 0)
                    {
                        p_gvCfsTemp.Columns.Add("CONSECUTIVO_VEH");
                    }
                }

                GvCfsTemp.DataSource = p_gvCfsTemp;
                GvCfsTemp.DataBind();

                if (p_gvCfsTemp != null)
                {
                    DataTable resultadotrue = new DataTable();
                    DataView viewtrue = new DataView();
                    var resulttrue = from myRow in p_gvCfsTemp.AsEnumerable()
                                     where myRow.Field<bool>("ASIGNADOPN") == true
                                     select myRow;
                    viewtrue = resulttrue.AsDataView();
                    resultadotrue = viewtrue.ToTable();
                    for (int i = 0; i <= resultadotrue.Rows.Count - 1; i++)
                    {
                        listsubitems.Add(resultadotrue.Rows[i][1].ToString());
                    }

                    DataTable resultadofalse = new DataTable();
                    DataView viewfalse = new DataView();
                    var resultsfalse = from myRow in p_gvCfsTemp.AsEnumerable()
                                       where myRow.Field<bool>("ASIGNADOPN") == false
                                       select myRow;
                    viewfalse = resultsfalse.AsDataView();
                    resultadofalse = viewfalse.ToTable();

                    for (int i = 0; i <= resultadofalse.Rows.Count - 1; i++)
                    {
                        listsubitems.Remove(resultadofalse.Rows[i][1].ToString());
                    }
                }

                System.Xml.Linq.XDocument docXMLridt = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                       new System.Xml.Linq.XElement("CRIDT",
                       new System.Xml.Linq.XElement("RIDT",
                       new System.Xml.Linq.XAttribute("MRN", String.IsNullOrEmpty(txtmrn.Text.Trim()) == true ? "" : txtmrn.Text.Trim()),
                       new System.Xml.Linq.XAttribute("MSN", String.IsNullOrEmpty(txtmsn.Text.Trim()) == true ? "" : txtmsn.Text.Trim()),
                       new System.Xml.Linq.XAttribute("HSN", String.IsNullOrEmpty(txthsn.Text.Trim()) == true ? "" : txthsn.Text.Trim()),
                       new System.Xml.Linq.XAttribute("CONTENEDOR", String.IsNullOrEmpty(txtcntr.Text.Trim()) == true ? "" : txtcntr.Text.Trim()))));

                dsRetorno = new DataSet();
                dsRetorno = WPASEPUERTA.GetRIDT(docXMLridt.ToString());

                var TXTFACTURADOA = "";
                if (dsRetorno != null && dsRetorno.Tables.Count > 0 && dsRetorno.Tables[0].Rows.Count > 0)
                {
                    var wresulta = (from wrow in dsRetorno.Tables[0].AsEnumerable()
                                    select wrow).FirstOrDefault();
                    TXTFACTURADOA = wresulta["ID_IMPORTADOR"].ToString() + " - " + wresulta["NOMBRE_IMPORTADOR"].ToString();

                }

                String value2 = TXTFACTURADOA;
                Char delimiter2 = '-';
                List<string> substringfaccli = value2.Split(delimiter2).ToList();
                var rucfaccli = substringfaccli[0].Trim();
                var dtfacturascliente = pasePuerta.GetInfoFacturasClientePPWebCFSXCarga(rucfaccli, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), this.agencia.Value);

                if (dtfacturascliente.Rows.Count > 0)
                {
                    lbl_facturadoa.Text = TXTFACTURADOA;
                    gvFacturas.DataSource = dtfacturascliente;
                    gvFacturas.DataBind();
                    //modalFacturasCliente.Show();
                }

                //txtfecsalppcfs.Text = Convert.ToDateTime(p_gvBreakBulk.Rows[0]["FECHA_AUT_PPWEB_"].ToString()).ToString("dd/MM/yyyy");
                txtmrnmsnhsnppcfs.Text = wcarga.SCARGA.ToString();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btnBuscar_Click();REFRESSCFSSALIR", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));   
            }

        }

        protected void chkTodosSubItems_CheckedChanged(Object sender, EventArgs args)
        {
            if (chkTodosSubItems.Checked)
            {
                var table = p_gvCfsTemp;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i]["ASIGNADOPN"] = true;
                }
                table.AcceptChanges();
                GvCfsTemp.DataSource = table;
                GvCfsTemp.DataBind();
            }
            else
            {
                var table = p_gvCfsTemp;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i]["ASIGNADOPN"] = false;
                }
                table.AcceptChanges();
                GvCfsTemp.DataSource = table;
                GvCfsTemp.DataBind();

            }
            //UPMODETALLECFS.Update();
            MODALCFS.Show();
        }

        protected void CHKPNCFS_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            CheckBox CHKPN = (CheckBox)row.FindControl("CHKPNCFS");
            //ASIGNADOPN CONSECUTIVO
            Boolean wsresult = true;

            String id = GvCfsTemp.DataKeys[row.RowIndex].Value.ToString();

            var cuurertrow = (from pi in p_gvCfsTemp.AsEnumerable()
                              where pi.Field<Decimal>("CONSECUTIVO").ToString() == id
                              select pi).FirstOrDefault();

            //Label LBLGCONSECUTIVO = (Label)row.FindControl("LBLGCONSECUTIVO");
            if (CHKPN.Checked)
            {
                cuurertrow["ASIGNADOPN"] = true;

            }
            else
            {
                cuurertrow["ASIGNADOPN"] = false;
            }

            p_gvCfsTemp.AcceptChanges();
            //UPMODETALLECFS.Update();
            MODALCFS.Show();
        }

        protected void GvCfsTemp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvCfsTemp.PageIndex = e.NewPageIndex;
            GvCfsTemp.DataSource = p_gvCfsTemp.AsDataView();
            GvCfsTemp.DataBind();
            //UPMODETALLECFS.Update();
            MODALCFS.Show();
        }

        protected void CMDADDCFS_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtfecsalppcfs.Text))
                {
                    this.Alerta("Ingrese una fecha de salida..");
                    MODALCFS.Show();
                    return;
                }

                String wsEmpresa = null;
                String wsChofer = null;

                DataView viewv = new DataView();
                var resultssv = from myRow in p_gvCfsTemp.AsEnumerable()
                                where myRow.Field<bool>("ASIGNADOPN") == true
                                select myRow;
                viewv = resultssv.AsDataView();
                var resultadov = viewv.ToTable();
                for (int i = 0; i <= resultadov.Rows.Count - 1; i++)
                {
                    listsubitems.Add(resultadov.Rows[i][1].ToString());
                }
                if (listsubitems.Count == 0)
                {
                    this.Alerta("Seleccione al menos un Codigo Sub. Item.");
                    MODALCFS.Show();
                    return;
                }

                if (string.IsNullOrEmpty(TxtGEmpresa.Text))
                {
                    this.Alerta("Escriba la Cia. de Transporte.");
                    TxtGEmpresa.Focus();
                    MODALCFS.Show();
                    return;
                }

                var valemp = TxtGEmpresa.Text.Split('-').ToList()[0].Trim();
                if (valemp == "0000000000000" || valemp == "0000000000001" || valemp == "0000000000002" || valemp == "0000000000005" || valemp == "0000000000008")
                {
                    if (string.IsNullOrEmpty(TxtChoferCFS.Text))
                    {
                        this.Alerta("Escriba el Chofer.");
                        TxtChoferCFS.Focus();
                        MODALCFS.Show();
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(TxtChoferCFS.Text) && string.IsNullOrEmpty(TxtPlacaCFS.Text))
                    {
                        this.Alerta("Escriba la Placa.");
                        TxtPlacaCFS.Focus();
                        MODALCFS.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtChoferCFS.Text) && !string.IsNullOrEmpty(TxtPlacaCFS.Text))
                    {
                        this.Alerta("Escriba el Chofer.");
                        TxtChoferCFS.Focus();
                        MODALCFS.Show();
                        return;
                    }
                }
                bool wsresult = true;

                //if (p_breabulkDocumento == null || String.IsNullOrEmpty(p_breabulkDocumento.ToString()))
                //{
                //    utilform.MessageBox("Ingrese el Documento de la Carga", this);
                //    wsresult = false;
                //}


                //var wconcfs = (from p in p_gvCfsTemp.AsEnumerable()
                //               where p.Field<Boolean>("ASIGNADOPN").Equals(true)
                //               select p).Count();

                //if ((int)wconcfs <= 0)
                //{
                //    utilform.MessageBox("Seleccione la Carga", this);
                //    wsresult = false;
                //}

                //DateTime WFECHAHOY;
                //DateTime.TryParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", new CultureInfo("es-EC"), DateTimeStyles.None, out WFECHAHOY);

                //Valida Empresa
                if (String.IsNullOrEmpty(TxtGEmpresa.Text.ToString()) != true)
                {
                    wsEmpresa = TxtGEmpresa.Text.Split('-').ToList()[0];
                    var wEmpresa = (from row in p_drEmpresa.AsEnumerable()
                                    where row.Field<String>("IDEMPRESA") != null && row.Field<String>("IDEMPRESA").Trim().Equals(wsEmpresa.Trim())
                                    select row.Field<String>("EMPRESA")).Count();

                    if ((int)wEmpresa <= 0)
                    {
                        //utilform.MessageBox("Empresa no valida", this);
                        this.Alerta("Empresa no valida.");
                        //UPMODETALLECFS.Update();
                        MODALCFS.Show();
                        wsresult = false;

                    }
                    id_empresa = wsEmpresa;
                }

                if (String.IsNullOrEmpty(TxtChoferCFS.Text) != true)
                {

                    wsChofer = TxtChoferCFS.Text.Split('-').ToList()[0];

                    var wchofer = (from rowChofer in p_drChofer.AsEnumerable()
                                   where rowChofer.Field<String>("IDCHOFER").Trim().Equals(wsChofer.Trim())
                                   select new { IDCHOFER = rowChofer.Field<String>("IDCHOFER") }).Count();

                    if ((int)wchofer <= 0)
                    {
                        //utilform.MessageBox("Chofer no valida", this);
                        this.Alerta("Chofer no es valido.");
                        //UPMODETALLECFS.Update();
                        MODALCFS.Show();
                        wsresult = false;

                    }

                }

                if (String.IsNullOrEmpty(TxtPlacaCFS.Text.ToString()) != true)
                {
                    var wPlaca = (from row in p_drPlaca.AsEnumerable()
                                  where row.Field<String>("PLACA") != null && row.Field<String>("PLACA").Trim().Equals(TxtPlacaCFS.Text.ToString().Trim().ToUpper())
                                  select row.Field<String>("PLACA")).Count();

                    if ((int)wPlaca <= 0)
                    {
                        //utilform.MessageBox("Placa no valida", this);
                        this.Alerta("Placa no valida.");
                        //UPMODETALLECFS.Update();
                        MODALCFS.Show();
                        wsresult = false;

                    }
                }

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!DateTime.TryParseExact(txtfecsalppcfs.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                {
                }
                sfechasalida = fechasal.ToString("MM-dd-yyyy");
                if (sfechasalida == null)
                {
                    //utilform.MessageBox("Seleccione una Fec. Salida.", this);
                    this.Alerta("Seleccione una Fec. Salida.");
                    //UPMODETALLECFS.Update();
                    MODALCFS.Show();
                    wsresult = false;
                }

                if (wsresult != false)
                {
                    DataTable resultado = new DataTable();
                    DataView view = new DataView();
                    var resultss = from myRow in p_gvCfsTemp.AsEnumerable()
                                   where myRow.Field<bool>("ASIGNADOPN") == true
                                   select myRow;
                    view = resultss.AsDataView();
                    resultado = view.ToTable();
                    for (int i = 0; i <= resultado.Rows.Count - 1; i++)
                    {
                        listsubitems.Add(resultado.Rows[i][1].ToString());
                    }

                    xmllistsubitems = listsubitems;

                    String wconsecutivo = "";
                    Decimal wcantidad = 0;
                    var currentStatRow = (from p in p_gvCfsTemp.AsEnumerable()
                                          where p.Field<Boolean>("ASIGNADOPN").Equals(true)
                                          select p);
                    foreach (DataRow crow in currentStatRow)
                    {

                        wconsecutivo = wconsecutivo + crow["CONSECUTIVO"].ToString() + ",";
                        wcantidad = (Decimal)crow["CANTIDAD"] + wcantidad;
                        crow.Delete();

                    }
                    if (!string.IsNullOrEmpty(wconsecutivo))
                    {
                        wconsecutivo = wconsecutivo.Substring(0, wconsecutivo.Length - 1).ToString();
                    }


                    p_gvCfsTemp.AcceptChanges();
                    p_gvCfs.Rows.Add(new String[] { null, wconsecutivo, TxtGEmpresa.Text.ToString(), wsEmpresa, TxtPlacaCFS.Text.ToUpper(), wsChofer, TxtChoferCFS.Text, wcantidad.ToString(), p_breabulkcarga });
                    p_gvCfs.AcceptChanges();



                    GvCfsTemp.DataSource = p_gvCfsTemp;
                    GvCfsTemp.DataBind();


                    var listPN = (from re in p_gvCfs.AsEnumerable().Where(TR => TR.Field<String>("CARGA") == p_breabulkcarga)
                                  select re).ToList();

                    gvCFS.DataSource = listPN.CopyToDataTable<DataRow>();
                    gvCFS.DataBind();
                    TxtChoferCFS.Text = null;
                    TxtPlacaCFS.Text = null;
                    TxtGEmpresa.Text = null;

                    EstadoPPSinTurno = pasePuerta.CONSULTA_ESTADO_PASE_SIN_TURNO(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                    if (EstadoPPSinTurno == false)
                    {
                        btnConsultar.Visible = true;
                    }

                    chkTodosSubItems.Checked = false;
                    //UPMODETALLECFS.Update();
                    MODALCFS.Show();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "CMDADDCFS_Click();REFRESSCFSSALIR", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void gvCFS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Eliminar"))
            {

                try
                {
                    var fechasalida = txtfecsalppcfs.Text.Trim();
                    String id = gvCFS.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString();
                    var wpase = (from pase in p_gvCfs.AsEnumerable()
                                 where pase.Field<int>("ID").Equals(int.Parse(id))
                                 select pase).FirstOrDefault();

                    wpase.Delete();
                    p_gvCfs.AcceptChanges();
                    gvHorarios.DataSource = new DataTable();
                    gvHorarios.DataBind();
                    btnConsultar.Visible = true;
                    REFRESSCFS(factura_pp_cfs);

                    //modalFacturasCliente.Hide();
                    txtfecsalppcfs.Text = fechasalida;
                    //UPMODETALLECFS.Update();
                    MODALCFS.Show();
                }
                catch (Exception ex)
                {

                    //utilform.MessageBox(exc.Message, this);
                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "gvCFS_RowCommand", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));   

                }
            }
        }

        protected void gvCFS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCFS.PageIndex = e.NewPageIndex;

            if (p_gvCfs != null)
            {

                var listPN = (from re in p_gvCfs.AsEnumerable().Where(TR => TR.Field<String>("CARGA") == p_breabulkcarga)
                              select re).ToList();
                if (listPN != null && listPN.Count > 0)
                {
                    gvCFS.DataSource = listPN.CopyToDataTable<DataRow>();
                    gvCFS.DataBind();
                }
                else
                {
                    gvCFS.DataSource = null;
                    gvCFS.DataBind();
                }
            }

            //UPMODETALLECFS.Update();
            MODALCFS.Show();
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtfecsalppcfs.Text))
                {
                    this.Alerta("Ingrese una fecha de salida..");
                           MODALCFS.Show();
                           return;
                }

                
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!string.IsNullOrEmpty(txtfecsalppcfs.Text))
                {
                    if (!DateTime.TryParseExact(txtfecsalppcfs.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsalppcfs.Text));
                        MODALCFS.Show();
                        txtfecsal.Focus();
                        return;
                    }
                }

                string xml = "";
                GenXml(listsubitems, out xml);
                
                //if (listsubitems.Count > 0)
                //{
                //fac_business fac = new fac_business();
                dtHorarios = new DataTable();
                dtHorariosfull = new DataTable();



                sfechasalida = fechasal.ToString("yyyy-MM-dd");
                dtHorarios = pasePuerta.CONSULTA_HORARIOS_DISPONIBLES(sfechasalida/*.Date.ToString("yyyy-MM-dd")*/, xml).ToTable();
                dtHorariosfull = pasePuerta.CONSULTA_HORARIOS_DISPONIBLES_FULL(sfechasalida/*.Date.ToString("yyyy-MM-dd")*/, xml).ToTable();
                if (dtHorarios == null)
                {
                    this.Alerta("No se encontraron horarios, revise la fecha..");
                    gvHorarios.DataSource = dtHorarios;
                    gvHorarios.DataBind();
                    btnConsultar.Visible = true;
                    MODALCFS.Show();
                    return;
                }
                if (dtHorarios.Rows.Count == 0)
                {
                    //utilform.MessageBox("No se encontraron horarios, revise la fecha.", this);
                    this.Alerta("No se encontraron horarios, revise la fecha..");
                    gvHorarios.DataSource = dtHorarios;
                    gvHorarios.DataBind();
                    btnConsultar.Visible = true;
                    MODALCFS.Show();
                    return;
                    //wsresult = false;
                }
                gvHorarios.DataSource = dtHorarios;
                gvHorarios.DataBind();
                btnConsultar.Visible = true;
                //}
                
                MODALCFS.Show();
            }
            catch (Exception ex)
            {
                //utilform.MessageBox(ex.Message, this);
                if (ex.Message == "No se puede generar pase de puerta los domingos.")
                {
                    txtfecsalppcfs.Text = "";
                    this.Alerta(ex.Message);
                    MODALCFS.Show();
                }
                else
                {
                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btnConsultar_Click", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                }
            }
        }

        protected void CHKHORARIO_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            CheckBox CHKHORARIO = (CheckBox)row.FindControl("CHKHORARIO");
            Label lblgvTb = (Label)row.FindControl("lblgvTb");
            Label lblgvBultos = (Label)row.FindControl("lblgvBultos");

            Decimal TotBultos = Convert.ToDecimal(lblgvTb.Text);
            //Decimal Bultos = Convert.ToDecimal(lblgvBultos.Text);

            string id = gvHorarios.DataKeys[row.RowIndex].Value.ToString();

            if (CHKHORARIO.Checked)
            {
                var cuurertrow = (from pi in dtHorarios.AsEnumerable()
                                  where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == id
                                  //&     pi.Field<bool>("CHECKED") == false
                                  select pi).FirstOrDefault();
                cuurertrow["CHECKED"] = true;
                //cuurertrow["IDEMPRESA"] = id_empresa;
                tot_bultos = TotBultos;
                //sum_bultos = sum_bultos + Bultos;

                dtHorarios.AcceptChanges();
                DataTable resultado = new DataTable();
                var results = (from pi in dtHorarios.AsEnumerable()
                               where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == id
                               select pi);
                DataView view = results.AsDataView();
                resultado = view.ToTable();
                gvHorarios.DataSource = resultado;
                gvHorarios.DataBind();

                bool valida = false;
                int sumtot = Convert.ToInt32(dtHorariosfull.Rows[row.RowIndex][4]);
                int rowi = 0;
                rowi = Convert.ToInt32(dtHorariosfull.Rows[row.RowIndex][1]);
                var cuurertrowidd = (from pi in dtHorariosfull.AsEnumerable()
                                     where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == rowi.ToString()
                                     select pi).FirstOrDefault();
                cuurertrowidd["CHECKED"] = true;
                //cuurertrowid["IDEMPRESA"] = id_empresa;
                dtHorariosfull.AcceptChanges();

                if (sumtot < tot_bultos)
                {
                    for (int i = row.RowIndex + 1; i <= dtHorariosfull.Rows.Count - 1; i++)
                    {
                        if (sumtot < tot_bultos)
                        {
                            rowi = Convert.ToInt32(dtHorariosfull.Rows[i][1]);
                            var cuurertrowid = (from pi in dtHorariosfull.AsEnumerable()
                                                where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == rowi.ToString()
                                                select pi).FirstOrDefault();
                            cuurertrowid["CHECKED"] = true;
                            //cuurertrowid["IDEMPRESA"] = id_empresa;
                            dtHorariosfull.AcceptChanges();
                            valida = true;
                        }
                        sumtot = sumtot + Convert.ToInt32(dtHorariosfull.Rows[i][4]);
                    }
                    if (valida == false)
                    {
                        rowi = Convert.ToInt32(dtHorariosfull.Rows[row.RowIndex][1]);
                        var cuurertrowid = (from pi in dtHorariosfull.AsEnumerable()
                                            where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == rowi.ToString()
                                            select pi).FirstOrDefault();
                        cuurertrowid["CHECKED"] = true;
                        //cuurertrowid["IDEMPRESA"] = id_empresa;
                        dtHorariosfull.AcceptChanges();
                    }
                }
                //UPMODETALLECFS.Update();
                MODALCFS.Show();
            }
            else
            {
                //cuurertrow["CHECKED"] = false;
                //dtHorarios.AcceptChanges();
                int rowi = 0;
                for (int i = 0; i <= dtHorarios.Rows.Count - 1; i++)
                {
                    rowi = Convert.ToInt32(dtHorarios.Rows[i][1]);
                    var cuurertrowid = (from pi in dtHorarios.AsEnumerable()
                                        where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == rowi.ToString()
                                        select pi).FirstOrDefault();
                    cuurertrowid["CHECKED"] = false;
                    //cuurertrowid["IDEMPRESA"] = "";
                    dtHorarios.AcceptChanges();
                }
                rowi = 0;
                for (int i = 0; i <= dtHorariosfull.Rows.Count - 1; i++)
                {
                    rowi = Convert.ToInt32(dtHorariosfull.Rows[i][1]);
                    var cuurertrowid = (from pi in dtHorariosfull.AsEnumerable()
                                        where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == rowi.ToString()
                                        select pi).FirstOrDefault();
                    cuurertrowid["CHECKED"] = false;
                    //cuurertrowid["IDEMPRESA"] = "";
                    dtHorariosfull.AcceptChanges();
                }
                gvHorarios.DataSource = dtHorarios;
                gvHorarios.DataBind();
                //UPMODETALLECFS.Update();
                MODALCFS.Show();
                /*
                StringWriter objSW = new StringWriter();
                dtHorarios.WriteXml(objSW);
                string result = objSW.ToString();*/
            }
        }

        protected void CMDCANCELARPASECFS_Click(object sender, EventArgs e)
        {
            try
            {
                SalirBtn();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "CMDCANCELARPASECFS_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
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

        public void Consultar()
        {
            
        }

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

        private void IniDsCliente()
        {



            XmlDocument docXml = new XmlDocument();
            XmlElement elem;
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            docXml.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><PASEPUERTA/>");
            elem = docXml.CreateElement("GetEmpresa");
            elem.SetAttribute("CLNT_TYPE", "OTHR,SHLN");
            elem.SetAttribute("CLNT_ACTIVE", "Y");


            docXml.DocumentElement.AppendChild(elem);
            dsRetorno = WPASEPUERTA.GetEmpresainfo(docXml.OuterXml);
            p_drCliente = dsRetorno.Tables[0];


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
                this.agencia.Value = "0904032331001";
              //  this.agencia.Value = "0991370226001";//AVILES TORRES
               // this.agencia.Value = user.ruc;
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
                    listsubitems = new List<string>();
                    IniDsPasePuertaBreakBulk();
                    //IniDsCliente();
                    IniRpt();
                    btnConsultar.Visible = false;
                    IniDsEmpresa();
                    IniDsChofer();
                    IniDsPlaca();
                    gvCFS.DataSource = null;
                    gvCFS.DataBind();
                    GvCfsTemp.DataSource = null;
                    GvCfsTemp.DataBind();
                    SalirBtn();
                    p_gvCfs = null;
                    p_gvCfsTemp = null;
                    //IniDsEmpresa();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //IniPaseWeb();
                //Consultar();

                DataSet dsRetorno = new DataSet();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                System.Xml.Linq.XDocument docXMLridt = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                          new System.Xml.Linq.XElement("CRIDT",
                          new System.Xml.Linq.XElement("RIDT",
                          new System.Xml.Linq.XAttribute("MRN", String.IsNullOrEmpty(txtmrn.Text.Trim()) == true ? "" : txtmrn.Text.Trim()),
                          new System.Xml.Linq.XAttribute("MSN", String.IsNullOrEmpty(txtmsn.Text.Trim()) == true ? "" : txtmsn.Text.Trim()),
                          new System.Xml.Linq.XAttribute("HSN", String.IsNullOrEmpty(txthsn.Text.Trim()) == true ? "" : txthsn.Text.Trim()),
                          new System.Xml.Linq.XAttribute("CONTENEDOR", String.IsNullOrEmpty(txtcntr.Text.Trim()) == true ? "" : txtcntr.Text.Trim()))));

                dsRetorno = WPASEPUERTA.GetRIDT(docXMLridt.ToString()) as DataSet;

                var TXTFACTURADOA = "";
                if (dsRetorno != null && dsRetorno.Tables.Count > 0 && dsRetorno.Tables[0].Rows.Count > 0)
                {
                    var wresulta = (from wrow in dsRetorno.Tables[0].AsEnumerable()
                                    select wrow).FirstOrDefault();
                    TXTFACTURADOA = wresulta["ID_IMPORTADOR"].ToString() + " - " + wresulta["NOMBRE_IMPORTADOR"].ToString();
                }
                else
                {
                    var p_datos = pasePuerta.GetInfoPasePuertaCFSSinRIDT(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                    if (p_datos.Rows.Count == 0)
                    {
                        this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                        return;
                    }
                    TXTFACTURADOA = p_datos.Rows[0]["ID_IMPORTADOR"].ToString() + " - " + p_datos.Rows[0]["NOMBRE_IMPORTADOR"].ToString();
                }

                String value2 = TXTFACTURADOA;
                Char delimiter2 = '-';
                List<string> substringfaccli = value2.Split(delimiter2).ToList();
                var rucfaccli = substringfaccli[0].Trim();
                var dtfacturascliente = pasePuerta.GetInfoFacturasClientePPWebCFSXCarga(rucfaccli, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), this.agencia.Value);

                if (dtfacturascliente.Rows.Count > 0)
                {
                    lbl_facturadoa.Text = TXTFACTURADOA;
                    gvFacturas.DataSource = dtfacturascliente;
                    gvFacturas.DataBind();
                    /*
                    var v_cliente = pasePuerta.GetValCliente(p_datospp.Rows[0]["FACTURA"].ToString());
                    if (v_cliente.Rows[0]["V_FACTURA"].ToString() == "0")
                    {
                        Label lblFacturaPPVal = (Label)gvFacturas.Rows[0].Cells[0].FindControl("lblFacturaPP");
                        var v_msg = lblFacturaPPVal.Text.ToString();
                        this.Alerta(v_msg);
                        return;
                    }
                    */
                    //modalFacturasCliente.Show();
                }
                else
                {
                    this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                    return;
                }
                //else
                //{
                //    var dtfacturasclientesub = pasePuerta.GetInfoFacturasClientePPWebCFSXCargaSubSec(rucfaccli, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), this.agencia.Value);
                //    if (dtfacturasclientesub.Rows.Count > 0)
                //    {
                //        /*
                //        IniDsEmpresa();
                //        IniDsChofer();
                //        IniDsPlaca();
                //        */
                //        lbl_facturadoa.Text = TXTFACTURADOA;
                //        gvFacturas.DataSource = dtfacturasclientesub;
                //        gvFacturas.DataBind();

                //        /*
                //           var v_cliente = pasePuerta.GetValCliente(p_datospp.Rows[0]["FACTURA"].ToString());
                //           if (v_cliente.Rows[0]["V_FACTURA"].ToString() == "0")
                //           {
                //               Label lblFacturaPPVal = (Label)gvFacturas.Rows[0].Cells[0].FindControl("lblFacturaPP");
                //               var v_msg = lblFacturaPPVal.Text.ToString();
                //               this.Alerta(v_msg);
                //               return;
                //           }
                //        */
                //        //modalFacturasCliente.Show();
                //    }
                //    else
                //    {
                //        this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                //        return;
                //    }
                //    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                //}

                //GridViewRow row = ((GridViewRow)((Button)sender).NamingContainer);
                Label lblFacturaPP = (Label)gvFacturas.Rows[0].Cells[0].FindControl("lblFacturaPP");
                Label lblEstadoPP = (Label)gvFacturas.Rows[0].Cells[1].FindControl("lblEstadoPP");

                var v_cliente = pasePuerta.GetValClienteCFS(lblFacturaPP.Text);
                if (v_cliente.Rows[0]["V_FACTURA"].ToString() == "0")
                {
                    var v_msg = v_cliente.Rows[0]["MENSAJE"].ToString();
                    this.Alerta(v_msg);
                    return;
                }

                String MRN = txtmrn.Text.Trim();
                String MSN = txtmsn.Text.Trim();
                String HSN = txthsn.Text.Trim();
                p_gvBreakBulk = new DataTable();
                p_gvBreakBulk = pasePuerta.GetInfoPasePuertaCFS_2019(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFacturaPP.Text.Trim(), this.agencia.Value);//modificado
                //p_gvBreakBulk = pasePuerta.GetInfoPasePuertaCFS(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFacturaPP.Text.Trim());//error
                String ID_CARGA = string.Empty;

                if (p_gvBreakBulk != null)
                {
                    if (p_gvBreakBulk.Rows.Count != 0)
                    {
                        ID_CARGA = p_gvBreakBulk.Rows[0]["CONSECUTIVO"].ToString();
                    }
                    else {
                        ID_CARGA = "0";
                        this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                        return;
                    }
               
                }
                else {
                    ID_CARGA = "0";
                    this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                    return;
                }

                //String ID_CARGA = p_gvBreakBulk.Rows[0]["CONSECUTIVO"].ToString();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                   new System.Xml.Linq.XElement("GetCfsSubItems", new System.Xml.Linq.XElement("GetCfsSubItem",
                                                   new System.Xml.Linq.XAttribute("MRN", String.IsNullOrEmpty(MRN) != true ? MRN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("MSN", String.IsNullOrEmpty(MSN) != true ? MSN.ToString().Trim() : ""),
                                                   new System.Xml.Linq.XAttribute("HSN", String.IsNullOrEmpty(HSN) != true ? HSN.ToString().Trim() : "0000"),
                                                   new System.Xml.Linq.XAttribute("ID_CARGA", String.IsNullOrEmpty(ID_CARGA) != true ? ID_CARGA.ToString().Trim() : "")
                                                   )));
                dsRetorno = new DataSet();
                dsRetorno = WPASEPUERTA.GetCFSSubIteminfo(docXML.ToString());
                var p_gvCfsTemp_ = dsRetorno.Tables[0];
                if (p_gvCfsTemp_ != null)
                {
                    if (p_gvCfsTemp_.Rows.Count > 0)
                    {
                        factura_pp_cfs = lblFacturaPP.Text.Trim();
                        modalFacturasCliente.Hide();
                        REFRESSCFS(factura_pp_cfs);
                        IniCfs();
                        //UPMODETALLECFS.Update();
                        MODALCFS.Show();
                        return;
                    }
                }


                if (lblEstadoPP.Text == "GENERADO")
                {
                    this.Alerta("Pase a Puerta ya se encuentra GENERADO... ");
                    //modalFacturasCliente.Show();
                    return;
                }
                else if (lblEstadoPP.Text == "CANCELADO")
                {
                    this.Alerta("Pase a Puerta ya se encuentra CANCELADO... ");
                    //modalFacturasCliente.Show();
                    return;
                }
                else
                {

                    factura_pp_cfs = lblFacturaPP.Text.Trim();
                    modalFacturasCliente.Hide();
                    IniDsEmpresa();
                    IniDsChofer();
                    IniDsPlaca();
                    REFRESSCFS(factura_pp_cfs);
                    IniCfs();
                    //UPMODETALLECFS.Update();
                    MODALCFS.Show();
                }
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                IniRpt();
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                
                DateTime fechasal = new DateTime();
                if (!string.IsNullOrEmpty(txtfecsalppcfs.Text))
                {
                    if (!DateTime.TryParseExact(txtfecsalppcfs.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsalppcfs.Text));
                        MODALCFS.Show();
                        txtfecsal.Focus();
                        return;
                    }
                }

                var dtvalfecha = pasePuerta.GetValFechaSalida(fechasal.ToString("yyyy-MM-dd"), factura_pp_cfs, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                {
                    if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                    {
                        this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                        MODALCFS.Show();
                        return;
                    }
                }
                //p_gvBreakBulk.Rows[0]["FECHA_SALIDA"] = txtfecsalppcfs.Text.Trim(); //Convert.ToDateTime(txtfecsalppcfs.Text).ToString("dd/MM/yyyy"); //+ " 00:00:00";
                //p_gvBreakBulk.Rows[0]["FECHA_AUT_PPWEB"] = txtfecsalppcfs.Text.Trim(); //Convert.ToDateTime(txtfecsalppcfs.Text).ToString("dd/MM/yyyy");// +" 00:00:00";
                sfechasalida = fechasal.ToString("yyyy-MM-dd");

                if (p_gvCfs != null && p_gvCfs.Rows.Count > 0)
                {
                    if (listsubitems.Count == 0)
                    {
                        this.Alerta("Seleccione al menos un Codigo Sub. Item.");
                        MODALCFS.Show();
                        return;
                    }
                    if (dtHorariosfull == null)
                    {
                        this.Alerta("Seleccione un Turno.");
                        MODALCFS.Show();
                        return;
                    }
                    else
                    {
                        EstadoPPSinTurno = pasePuerta.CONSULTA_ESTADO_PASE_SIN_TURNO(txtmrn.Text, txtmsn.Text, txthsn.Text);
                        if (EstadoPPSinTurno == false)
                        {
                            DataTable resultado = new DataTable();
                            DataView view = new DataView();
                            var resultss = from myRow in dtHorariosfull.AsEnumerable()
                                           where myRow.Field<bool>("CHECKED") == true
                                           select myRow;
                            view = resultss.AsDataView();
                            resultado = view.ToTable();
                            if (resultado.Rows.Count == 0)
                            {
                                this.Alerta("Seleccione un Turno.");
                                MODALCFS.Show();
                                return;
                            }
                        }
                    }
                    Session["ptypeReport"] = "CFS";

                    DateTime fechasalida = new DateTime();
                    if (!DateTime.TryParseExact(txtfecsalppcfs.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasalida))
                    {
                    }
                    p_gvBreakBulk.Rows[0]["FECHA_SALIDA"] = fechasalida; //.ToString("MM/dd/yyyy"); //Convert.ToDateTime(txtfecsalppcfs.Text).ToString("dd/MM/yyyy"); //+ " 00:00:00";
                    p_gvBreakBulk.Rows[0]["FECHA_AUT_PPWEB"] = fechasalida; //.ToString("MM/dd/yyyy"); 
                    GPasePuerta();
                }
                else
                {
                    this.Alerta("No se encontraron datos para procesar el Pase a Puerta, revise por favor... ");
                    MODALCFS.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
        }

        protected void btnAddCiatrans_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in tablePaginationPPWeb.Items)
                {
                    TextBox TxtGEmpresa = item.FindControl("TxtGEmpresa") as TextBox;
                    CheckBox chkPase = item.FindControl("chkPase") as CheckBox;
                    if (!chkPase.Checked)
                    {
                        TxtGEmpresa.Text = TxtGEmpresaAdd.Text.Trim();
                        TxtGEmpresa.ToolTip = TxtGEmpresaAdd.Text.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btnAddCiatrans_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);

            }
        }

        private void IniPaseWeb()
        {
            /*
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fechasal = new DateTime();
            if (!string.IsNullOrEmpty(txtfecsal.Text))
            {
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                {
                    this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsal.Text));
                    MODALCFS.Show();
                    txtfecsal.Focus();
                    return;
                }
            }
            */
            p_datospp = new DataTable();
            p_datospp = pasePuerta.GetDatosGeneraPaseWeb(this.agencia.Value, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), txtcntr.Text.Trim());
            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();
            if (p_datospp.Rows.Count == 0)
            {
                this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                return;
            }
            //IniDsEmpresaLoad();
            //IniDsEmpresa();
            //IniDsChofer();
            IniTurno();
            //IniDsPlaca();
            //IniDatosPPWeb();
            p_datospp.Columns.Add("PASE");
            p_datospp.Columns.Add("D_TURNO");
            lblTotCntr.Text = "Tot. Contenedores: " + p_datospp.Rows.Count.ToString();
            TxtGEmpresaAdd.Text = "";
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
                Label lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as Label;
                elemturno.SetAttribute("Container", lblCntr.Text);
                elemturno.SetAttribute("Fecha", Convert.ToDateTime(lblFecAutPPWeb.Text).ToString("MM/dd/yyyy"));
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
                    var dthorasrf = pasePuerta.GetTurnosInfoReefer(xmlTurnos.ToString(), p_datospp.Rows[item.ItemIndex]["FECHA_SALIDA"].ToString());
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

        private void IniDsChofer(/*string empresa*/)
        {
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            //var dtRetorno = pasePuerta.GetChoferinfo(empresa);
            
            dsRetorno = WPASEPUERTA.GetChoferinfo();
            p_drChofer = dsRetorno.Tables[0];
            
            for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
            {
                dsRetorno.Tables[0].Rows[i]["CHOFER"] = dsRetorno.Tables[0].Rows[i]["CHOFER"].ToString().Replace("-  -", "-");
                
                /*
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
                dsRetorno.Tables[0].Rows[i]["CHOFER"] = substringchf[1].Trim();*/
            }
            
            //dtRetorno.Rows.Add("0", "* ELIJA *");
            /*
            if (dtRetorno.Rows.Count > 0)
            {
                DataView myDataView = dtRetorno.DefaultView;
                myDataView.Sort = "CHOFER ASC";

                p_drChofer = myDataView.ToTable();
            }
            */
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
            p_drCliente = dsRetorno.Tables[0];

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
            
        //    /*
        //    foreach (RepeaterItem item in tablePaginationPPWeb.Items)
        //    {
        //        DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
        //        ddlEmpresa.DataSource = myDataView;
        //        ddlEmpresa.DataTextField = "EMPRESA";
        //        ddlEmpresa.DataValueField = "IDEMPRESA";
        //        ddlEmpresa.DataBind();
        //    }
        //    */
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
                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"));
                System.Xml.Linq.XDocument docXMLEXPO = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"));
                DataSet dsRetorno = new DataSet();
                WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new System.Xml.Linq.XElement("PASEPUERTA",
                              new System.Xml.Linq.XElement("ConsultaCarga",
                        new System.Xml.Linq.XAttribute("MRN", substringcarga[0].Trim()),
                        new System.Xml.Linq.XAttribute("MSN", substringcarga[1].Trim()),
                        new System.Xml.Linq.XAttribute("HSN", substringcarga[2].Trim()),
                        new System.Xml.Linq.XAttribute("Type", "IMPO"),
                        new System.Xml.Linq.XAttribute("REFERENCIA", ""),
                        new System.Xml.Linq.XAttribute("CONTENEDOR", "")
                                             )));
                dsRetorno = WPASEPUERTA.GetContainerN4info(docXML.ToString(), docXMLEXPO.ToString());
                p_gvContenedor = dsRetorno.Tables[0];
                var result = (from hi in p_gvContenedor.AsEnumerable()
                              where hi.Field<String>("CONTENEDOR") == lblCntr.Text
                              select hi);
                lblReserva.Text = result.AsDataView().ToTable().Rows[item.ItemIndex]["RESERVA"].ToString();
            }
        }

        private void IniDsPlaca(/*string empresa*/)
        {
            
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            dsRetorno = WPASEPUERTA.GetPlacainfo();
            p_drPlaca = dsRetorno.Tables[0];
            
            /*p_drPlaca = pasePuerta.GetPlacainfo(empresa);*/
        }

        private void GPasePuerta()
        {

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

                                     select new
                                     {
                                         CONSECUTIVO = wdata.Field<String>("CONSECUTIVO"),
                                         IDEMPRESA = wdatapase.Field<String>("IDEMPRESA"),
                                         PLACA = wdatapase.Field<String>("PLACA"),
                                         IDCHOFER = wdatapase.Field<String>("IDCHOFER"),
                                         CANTIDAD = wdatapase.Field<String>("CANTIDAD"),
                                         FECHA_SALIDA = wdata.Field<DateTime>("FECHA_SALIDA").ToString("MM/dd/yyyy"),
                                         FECHA_AUT = wdata.Field<DateTime>("FECHA_SALIDA").ToString("yyyy-MM-dd") + " 00:00",
                                         CARGA = wdatapase.Field<String>("CARGA"),
                                         CODSUBITEMS = wdatapase.Field<String>("CONSECUTIVO")
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
                        if (ds_report.Tables[0].ToString() == "DTError")
                        {
                            if (ds_report.Tables[0].Rows.Count > 0)
                            {
                                this.Alerta(ds_report.Tables[0].Rows[0][1].ToString());
                                MODALCFS.Show();
                                return;
                            }
                            else
                            {
                                this.Alerta("Hubo un error con la carga.");
                                MODALCFS.Show();
                                return;
                            }
                        }
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
                            gvHorarios.DataSource = dtHorarios;
                            gvHorarios.DataBind();
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
                                    DataTable resultado = new DataTable();
                                    DataView view = new DataView();
                                    var resultss = from myRow in dtHorariosfull.AsEnumerable()
                                                   where myRow.Field<bool>("CHECKED") == true
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
                                    dtHorarios = new DataTable();
                                    dtHorariosfull = new DataTable();
                                    gvHorarios.DataSource = dtHorarios;
                                    gvHorarios.DataBind();
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

                                    var Empresa = (from currentStat in p_drCliente.AsEnumerable()
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

                                    string msjerror = string.Empty;
                                    if (!pasePuerta.AcualizaPaseWebCFS(xmlPPWeb.ToString(), xmlPPWebCFS.ToString(), xmlPPWebCFSDet.ToString(), p_gvBreakBulk.Rows[0]["FACTURA"].ToString(), wsAgente, wempr, "", "", "", p_user, out msjerror))
                                    {
                                        this.Alerta(msjerror);
                                        return;
                                    }
                                    this.Alerta("Pase a Puerta generado Exitosamente... ");
                                    var function = "openPop('" + p_gvCfs.Rows[0]["CARGA"].ToString() + "','" + ds_report.Tables[0].Rows[0]["PASE"].ToString() + "');";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", function, true);
                                    SalirBtn();
                                }
                                catch (Exception ex)
                                {
                                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                                    return;
                                }
                           /* }
                            else
                            {
                                DataTable Dreporte = new DataTable();
                                Dreporte = ds_report.Tables[i].Copy();
                                p_reportPasePuerta.Tables.Add(Dreporte);
                                //utilform.MessageBox("Pase a Puerta generado Exitosamente... ", this);
                                this.Alerta("Pase a Puerta generado Exitosamente... ");
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

        public void SalirBtn()
        {
            //lista_subitems = "";
            listsubitems = new List<string>();
            //btnConsultar.Visible = false;
            chkTodosSubItems.Checked = false;
            dtHorariosfull = new DataTable();
            dtHorarios = new DataTable();
            gvHorarios.DataSource = new DataTable();
            gvHorarios.DataBind();
            //btnConsultar.Visible = false;
            TxtChoferCFS.Text = "";
            TxtGEmpresa.Text = "";
            TxtPlacaCFS.Text = "";
            //UPMODETALLECFS.Update();
            //MODALCFS.Hide();
            //modalFacturasCliente.Hide();
            REFRESSCFSSALIR(factura_pp_cfs);

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

        [System.Web.Services.WebMethod]
        public static string IsAvailableBooking(string rucuser, string booking)
        {
            var rucbooking = string.Empty;
            var c = asignacionDae.GetRucXBkg(booking.Trim());
            if (c.Rows.Count == 0)
            {
                return "2";
            }
            else
            {
                rucbooking = c.Rows[0][0].ToString();
                if (rucuser == rucbooking)
                {
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
        }

        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label lblCntr = (Label)item.FindControl("lblCntr");
                Label lblFecAutPPWeb = item.FindControl("lblFecAutPPWeb") as Label;
                DropDownList ddlTurno = item.FindControl("ddlTurno") as DropDownList;

                if (chkPase.Checked)
                {
                    var v_cliente = pasePuerta.GetValCliente(p_datospp.Rows[item.ItemIndex]["FACTURA"].ToString());
                    if (v_cliente.Rows[0]["V_FACTURA"].ToString() == "0")
                    {
                        var v_msg = v_cliente.Rows[0]["MENSAJE"].ToString();
                        this.Alerta(v_msg);
                        chkPase.Checked = false;
                        return;
                    }
                    if (ddlTurno.SelectedItem.Value == "0")
                    {
                        this.Alerta("Elija un Turno para el Contenedor: " + lblCntr.Text);
                        chkPase.Checked = false;
                        return;
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
                    DTRESULT = (DataTable)HttpContext.Current.Session["drEmpresaPPWeb"];
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
                    DTRESULT2 = (DataTable)HttpContext.Current.Session["drChoferFilterPPWeb"];
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
                    else
                    {
                        p_datospp.Rows[item.ItemIndex]["CHOFER"] = "NULL";
                        p_datospp.Rows[item.ItemIndex]["PLACA"] = "NULL";
                    }
                    p_datospp.Rows[item.ItemIndex]["PASE"] = chkPase.Checked.ToString();
                    if (ddlTurno.SelectedItem.Value != "0")
                    {
                        p_datospp.Rows[item.ItemIndex]["TURNO"] = ddlTurno.SelectedItem.Value == "0" ? "" : ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[0].Trim();
                        p_datospp.Rows[item.ItemIndex]["ID_TURNO"] = ddlTurno.SelectedItem.Value == "0" ? "" : ddlTurno.SelectedItem.Value.ToString().Split('-').ToList()[1].Trim();
                        p_datospp.Rows[item.ItemIndex]["D_TURNO"] = ddlTurno.SelectedItem.Text;
                    }
                    if (!string.IsNullOrEmpty(TxtGEmpresa.Text.Trim()))//ddlEmpresa.SelectedItem.Value != "0")
                    {
                        String value = TxtGEmpresa.Text.Trim();
                        Char delimiter = '-';
                        List<string> substringemp = value.Split(delimiter).ToList();
                        p_datospp.Rows[item.ItemIndex]["CIATRANS"] = substringemp[0].Trim() == "0" ? "" : substringemp[0].Trim();
                    }
                    p_datospp.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "chkPase_CheckedChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "ddlChofer_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "ddlChofer_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "ddlChofer_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void btgcodebar_Click(object sender, EventArgs e)
        {
            
            string barcode = "BARCODE12345";
            Bitmap bitmap = new Bitmap(barcode.Length * 40, 150);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                PrivateFontCollection privateFonts = new PrivateFontCollection();
                var ruta = Server.MapPath("../fonts/IDAutomationHC39M.ttf");
                privateFonts.AddFontFile(ruta); //("C:\\IDAutomationHC39M.ttf");
                Font oFont = new Font(privateFonts.Families[0], 20);
                PointF point = new PointF(2f, 2f);
                SolidBrush black = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);
                graphics.FillRectangle(white, 0, 0, bitmap.Width, bitmap.Height);
                graphics.DrawString("*" + barcode + "*", oFont, black, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                var base64Data = Convert.ToBase64String(ms.ToArray());
                //imap.ImageUrl = "data:image/gif;base64," + base64Data;
            }
            
        }

        protected void TxtGEmpresa_TextChanged(object sender, EventArgs e)
        {
            try
            {
                /*
                TextBox TxtGEmpresa = (TextBox)sender;
                TxtGEmpresa.ToolTip = TxtGEmpresa.Text.Trim();

                String value = TxtGEmpresa.Text.Trim();
                Char delimiter = '-';
                List<string> substringemp = value.Split(delimiter).ToList();
                IniDsChofer(substringemp[0].Trim());
                IniDsPlaca(substringemp[0].Trim());
                TxtChoferCFS.Focus();
                MODALCFS.Show();
                */
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "TxtGEmpresa_TextChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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

            DTRESULT = (DataTable)HttpContext.Current.Session["drEmpresaPPWeb"];
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
            DTRESULT = (DataTable)HttpContext.Current.Session["drChoferFilterPPWeb"];//drChoferPPWeb"];

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
            DTRESULT = (DataTable)HttpContext.Current.Session["drChoferPPWeb"];
            
            /*
             var list = (from currentStat in DTRESULT.Select("EMPRESA like '%" + prefix + "%'").AsEnumerable()
                         select currentStat);
            */
            /*
            String value = prefix.Trim();
            Char delimiter = '-';
            List<string> substringemp = value.Split(delimiter).ToList();

            var rucciatrans = substringemp[0].Trim();
            */
            var list = (from hi in DTRESULT.AsEnumerable()
                        select hi);
            
            DataTable dtResultado = new DataTable();
            dtResultado = list.AsDataView().ToTable();

            if (dtResultado != null)
            {
                if (dtResultado.Rows.Count > 0)
                {
                    HttpContext.Current.Session["drChoferFilterPPWeb"] = dtResultado;
                }
            }
            
            return "1";
        }
    }
}