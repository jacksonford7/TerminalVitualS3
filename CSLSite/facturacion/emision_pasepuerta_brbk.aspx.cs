using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using System.Web.Script.Services;
using System.Configuration;
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
    public partial class emision_pasepuerta_brbk : System.Web.UI.Page
    {
        public List<String> xmllistsubitems
        {
            get { return (List<String>)Session["xmllistsubitems"]; }
            set { Session["xmllistsubitems"] = value; }
        }
        private DataTable dtHorariosBRBK
        {
            get
            {
                return (DataTable)Session["dtHorariosBRBK"];
            }
            set
            {
                Session["dtHorariosBRBK"] = value;
            }

        }
        private String p_breabulkDocumento
        {
            get
            {
                return (String)Session["breabulkdocumentobrbk"];
            }
            set
            {
                Session["breabulkdocumentobrbk"] = value;
            }

        }
        private string sfechasalidabrbk
        {
            get
            {
                return (string)Session["sfechasalidabrbk_"];
            }
            set
            {
                Session["sfechasalidabrbk_"] = value;
            }

        }

        private String p_breabulkconsecutivo
        {
            get
            {
                return (String)Session["breabulkconsecutivobrbk"];
            }
            set
            {
                Session["breabulkconsecutivobrbk"] = value;
            }

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
                return (DataTable)Session["drpasepuertabreakbulk"];
            }
            set
            {
                Session["drpasepuertabreakbulk"] = value;
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

        private DataTable p_gvBreakBulk
        {
            get
            {
                return (DataTable)Session["p_gvBreakBulk"];
            }
            set
            {
                Session["p_gvBreakBulk"] = value;
            }

        }

        private DataTable p_gvBreakBulk_
        {
            get
            {
                return (DataTable)Session["p_gvBreakBulk_"];
            }
            set
            {
                Session["p_gvBreakBulk_"] = value;
            }

        }

        private DataSet p_reportPasePuerta
        {
            get
            {
                return (DataSet)Session["dsPasePuertaBrbk"];
            }
            set
            {
                Session["dsPasePuertaBrbk"] = value;
            }

        }

        private DataTable p_drChofer
        {
            get
            {
                return (DataTable)Session["drChoferbrbk"];
            }
            set
            {
                Session["drChoferbrbk"] = value;
            }

        }

        private DataTable p_drEmpresa
        {
            get
            {
                return (DataTable)Session["drEmpresaPPWebbRBK"];
            }
            set
            {
                Session["drEmpresaPPWebbRBK"] = value;
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
                return (String)Session["breabulkcargabrbk"];
            }
            set
            {
                Session["breabulkcargabrbk"] = value;
            }

        }

        public DataSet ds_report
        {
            get
            {
                return (DataSet)Session["ds_report_cfs"];
            }
            set
            {
                Session["ds_report_cfs"] = value;
            }

        }

        public List<String> listsubitems
        {
            get { return (List<String>)Session["listsubitemscfs"]; }
            set { Session["listsubitemscfs"] = value; }
        }

        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IniDsPasePuertaBreakBulk();
                    IniDsEmpresa();
                    IniDsChofer();
                    lblTotCntr.Text = "";
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                     this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        protected void CHKHORARIOBRBK_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            CheckBox CHKHORARIO = (CheckBox)row.FindControl("CHKHORARIOBRBK");
            Label lblgvBultos = (Label)row.FindControl("lblgvBultos");

            try
            {
                string id = gvHorariosBRBK.DataKeys[row.RowIndex].Value.ToString();
                if (p_drpasepuertabreakbulk != null)
                {
                    if (p_drpasepuertabreakbulk.Rows.Count > 0)
                    {
                        var bodega = pasePuerta.GetBodegaCargaBRBK(p_drpasepuertabreakbulk.Rows[0]["CONSECUTIVO"].ToString());
                        if (bodega == null)
                        {
                             this.Alerta("No se encontraron ubicación o bodega de la carga.");
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                            return;
                        }
                        else if (bodega.Rows.Count == 0)
                        {
                             this.Alerta("No se encontraron ubicación o bodega de la carga.");
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                            return;
                        }

                        dtHorariosBRBK = new DataTable();
                        dtHorariosBRBK = pasePuerta.GetTurnosDisponiblesBRBK(sfechasalidabrbk, bodega.Rows[0]["BODEGA"].ToString());
                        if (dtHorariosBRBK == null)
                        {
                             this.Alerta("No se encontraron horarios, revise la fecha.");
                            gvHorariosBRBK.DataSource = dtHorariosBRBK;
                            gvHorariosBRBK.DataBind();
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                            return;
                        }
                        else if (dtHorariosBRBK.Rows.Count == 0)
                        {
                             this.Alerta("No se encontraron horarios, revise la fecha.");
                            gvHorariosBRBK.DataSource = dtHorariosBRBK;
                            gvHorariosBRBK.DataBind();
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                            return;
                        }
                        if (CHKHORARIO.Checked)
                        {
                            var cuurertrow = (from pi in dtHorariosBRBK.AsEnumerable()
                                              where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == id
                                              select pi).FirstOrDefault();
                            cuurertrow["CHECKED"] = true;
                            dtHorariosBRBK.AcceptChanges();
                            DataTable resultado = new DataTable();
                            var results = (from pi in dtHorariosBRBK.AsEnumerable()
                                           where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == id
                                           select pi);
                            DataView view = results.AsDataView();
                            resultado = view.ToTable();
                            var pasesdisponibles = Convert.ToInt64(resultado.Rows[0]["BULTOS"].ToString());
                            if (pasesdisponibles == 0)
                            {
                                 this.Alerta("El turno " + resultado.Rows[0]["HORADESDE"].ToString() + ", no tiene pases disponibles.");
                                dtHorariosBRBK = new DataTable();
                                dtHorariosBRBK = pasePuerta.GetTurnosDisponiblesBRBK(sfechasalidabrbk, bodega.Rows[0]["BODEGA"].ToString());
                                if (dtHorariosBRBK == null)
                                {
                                     this.Alerta("No se encontraron horarios, revise la fecha.");
                                    gvHorariosBRBK.DataSource = dtHorariosBRBK;
                                    gvHorariosBRBK.DataBind();
                                    //UPMODETALLEBREAK.Update();
                                    MODALBREAKBULK.Show();
                                    return;
                                }
                                else if (dtHorariosBRBK.Rows.Count == 0)
                                {
                                     this.Alerta("No se encontraron horarios, revise la fecha.");
                                    gvHorariosBRBK.DataSource = dtHorariosBRBK;
                                    gvHorariosBRBK.DataBind();
                                    //UPMODETALLEBREAK.Update();
                                    MODALBREAKBULK.Show();
                                    return;
                                }
                                gvHorariosBRBK.DataSource = dtHorariosBRBK;
                                gvHorariosBRBK.DataBind();
                                //UPMODETALLEBREAK.Update();
                                MODALBREAKBULK.Show();
                                return;
                            }
                            Int64 pasesagenerar = 0;
                            for (int i = 0; i < p_drpasepuertabreakbulk.Rows.Count; i++)
                            {
                                pasesagenerar = pasesagenerar + Convert.ToInt64(p_drpasepuertabreakbulk.Rows[i]["CANTPASES"].ToString());
                            }
                            if (pasesagenerar <= pasesdisponibles)
                            {
                                gvHorariosBRBK.DataSource = resultado;
                                gvHorariosBRBK.DataBind();
                            }
                            else
                            {
                                var cuurertrow_ = (from pi in dtHorariosBRBK.AsEnumerable()
                                                   where pi.Field<Int64>("IDDISPONIBLEDET").ToString() == id
                                                   select pi).FirstOrDefault();
                                cuurertrow_["CHECKED"] = false;
                                dtHorariosBRBK.AcceptChanges();
                                gvHorariosBRBK.DataSource = dtHorariosBRBK;
                                gvHorariosBRBK.DataBind();
                                 this.Alerta("La cantidad de pases a generar: " + pasesagenerar.ToString() + ", supera al del turno " + resultado.Rows[0]["HORADESDE"].ToString() + ": " + pasesdisponibles.ToString());
                                //UPMODETALLEBREAK.Update();
                                MODALBREAKBULK.Show();
                                return;
                            }
                        }
                        else
                        {
                            //dtHorariosBRBK = new DataTable();
                            gvHorariosBRBK.DataSource = dtHorariosBRBK;
                            gvHorariosBRBK.DataBind();
                        }
                    }
                    else
                    {
                        dtHorariosBRBK = new DataTable();
                        gvHorariosBRBK.DataSource = dtHorariosBRBK;
                        gvHorariosBRBK.DataBind();
                    }
                }
                else
                {
                    dtHorariosBRBK = new DataTable();
                    gvHorariosBRBK.DataSource = dtHorariosBRBK;
                    gvHorariosBRBK.DataBind();
                }
                //UPMODETALLEBREAK.Update();
                MODALBREAKBULK.Show();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "CHKHORARIOBRBK_CheckedChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                 this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void btConsultarBRBK_Click(object sender, EventArgs e)
        {
            try
            {
                if (p_drpasepuertabreakbulk != null)
                {
                    if (p_drpasepuertabreakbulk.Rows.Count > 0)
                    {
                        var bodega = pasePuerta.GetBodegaCargaBRBK(p_drpasepuertabreakbulk.Rows[0]["CONSECUTIVO"].ToString());
                        if (bodega == null)
                        {
                             this.Alerta("No se encontraron ubicación o bodega de la carga.");
                        }
                        else if (bodega.Rows.Count == 0)
                        {
                             this.Alerta("No se encontraron ubicación o bodega de la carga.");
                        }

                        dtHorariosBRBK = new DataTable();
                        dtHorariosBRBK = pasePuerta.GetTurnosDisponiblesBRBK(sfechasalidabrbk, bodega.Rows[0]["BODEGA"].ToString());
                        if (dtHorariosBRBK == null)
                        {
                             this.Alerta("No se encontraron horarios, revise la fecha.");
                        }
                        else if (dtHorariosBRBK.Rows.Count == 0)
                        {
                             this.Alerta("No se encontraron horarios, revise la fecha.");
                        }
                        gvHorariosBRBK.DataSource = dtHorariosBRBK;
                        gvHorariosBRBK.DataBind();
                    }
                    else
                    {
                         this.Alerta("Ingrese al menos un dato.");
                        dtHorariosBRBK = new DataTable();
                        gvHorariosBRBK.DataSource = dtHorariosBRBK;
                        gvHorariosBRBK.DataBind();
                    }
                }
                else
                {
                    dtHorariosBRBK = new DataTable();
                    gvHorariosBRBK.DataSource = dtHorariosBRBK;
                    gvHorariosBRBK.DataBind();
                }
                //UPMODETALLEBREAK.Update();
                MODALBREAKBULK.Show();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btConsultarBRBK_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                 this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void CMDCANCELARPASEBREAKBULK_Click(object sender, EventArgs e)
        {
            try
            {
                if (p_drpasepuertabreakbulk != null)
                {
                    if (p_drpasepuertabreakbulk.Rows.Count > 0)
                    {
                        if (dtHorariosBRBK == null)
                        {
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                             this.Alerta("Seleccione un Turno.");
                            return;
                        }
                        var result = (from currentStat in dtHorariosBRBK.AsEnumerable()
                                      where Convert.ToBoolean(currentStat.Field<Boolean>("CHECKED")) == true
                                      select currentStat);
                        var count = result.AsDataView().ToTable().Rows.Count;
                        if (count == 0)
                        {
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                             this.Alerta("Seleccione un Turno.");
                            return;
                        }
                    }
                }
                //UPMODETALLEBREAK.Update();
                MODALBREAKBULK.Hide();
                /*GVRESULTBREAKBULK.DataSource = p_gvBreakBulk;
                GVRESULTBREAKBULK.DataBind();
                UPRINCIPAL.Update();*/
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "CMDCANCELARPASEBREAKBULK_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                 this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void GVPASEAPUERTA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Eliminar"))
            {

                try
                {
                    String id = GVPASEAPUERTA.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString();
                    var wpase = (from pase in p_drpasepuertabreakbulk.AsEnumerable()
                                 where pase.Field<int>("ID").Equals(int.Parse(id))
                                 select pase).FirstOrDefault();

                    decimal wpeso = Decimal.Parse(wpase["CANTIDAD"].ToString()) * Decimal.Parse(wpase["CANTPASES"].ToString());

                    var validaCantidad = (from breakb in p_gvBreakBulk.AsEnumerable()
                                          where breakb.Field<String>("CONSECUTIVO").Equals(p_breabulkconsecutivo)
                                          select breakb).FirstOrDefault();
                    validaCantidad["CANTIDAD"] = Decimal.Parse(validaCantidad["CANTIDAD"].ToString()) + wpeso;

                    wpase.Delete();
                    p_gvBreakBulk.AcceptChanges();
                    p_drpasepuertabreakbulk.AcceptChanges();

                    Paserefress();

                    if (p_drpasepuertabreakbulk.Rows.Count == 0)
                    {

                        dtHorariosBRBK = null;
                        gvHorariosBRBK.DataSource = dtHorariosBRBK;
                        gvHorariosBRBK.DataBind();

                    }
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "GVPASEAPUERTA_RowCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                     this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                }
            }
        }

        protected void CMDADDBBREAKBULK_Click(object sender, EventArgs e)
        {
            try
            {
                String wsEmpresa = null;
                String wsChofer = null;
                if (String.IsNullOrEmpty(TxtAddChoferbreakbulk.Text) == true && String.IsNullOrEmpty(TXTADDPLACABREAKBULK.Text.ToString()) == true &&
                    String.IsNullOrEmpty(TXTADDCIABREAKBULK.Text) == true && String.IsNullOrEmpty(TXtCANTPASES.Text) == true && String.IsNullOrEmpty(TXTCANTIDAD.Text) == true)
                {
                     this.Alerta("Ingrese al menos un dato.");
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                    return;
                }
                bool wsresult = true;
                /*
                if (p_breabulkDocumento == null || String.IsNullOrEmpty(p_breabulkDocumento.ToString()))
                {
                     this.Alerta("Ingrese el Documento de la Carga");
                    wsresult = false;
                }
                */
                if (String.IsNullOrEmpty(TXtCANTPASES.Text.ToString()) == true)
                {
                     this.Alerta("Ingrese la Cantidad de Pases");
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                    wsresult = false;
                }
                else
                {
                    if (Decimal.Parse(TXtCANTPASES.Text.ToString()) <= 0)
                    {
                         this.Alerta("la Cantidad de Pases debe ser mayor  a 0");
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        wsresult = false;
                    }
                }

                if (String.IsNullOrEmpty(TXTCANTIDAD.Text.ToString()) == true)
                {
                     this.Alerta("Ingrese la Cantidad");
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                    wsresult = false;
                }
                else
                {
                    if (Decimal.Parse(TXTCANTIDAD.Text.ToString()) <= 0)
                    {
                         this.Alerta("la Cantidad  debe ser mayor a 0");
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        wsresult = false;
                    }
                }

                //Valida Empresa


                if (String.IsNullOrEmpty(TXTADDCIABREAKBULK.Text.ToString()) != true)
                {
                    wsEmpresa = TXTADDCIABREAKBULK.Text.Split('-').ToList()[0];
                    var wEmpresa = (from row in p_drEmpresa.AsEnumerable()
                                    where row.Field<String>("IDEMPRESA") != null && row.Field<String>("IDEMPRESA").Trim().Equals(wsEmpresa.Trim())
                                    select row.Field<String>("EMPRESA")).Count();

                    if ((int)wEmpresa <= 0)
                    {
                         this.Alerta("Empresa no valida");
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        wsresult = false;

                    }
                }

                if (String.IsNullOrEmpty(TxtAddChoferbreakbulk.Text) != true)
                {

                    wsChofer = TxtAddChoferbreakbulk.Text.Split('-').ToList()[0];

                    var wchofer = (from rowChofer in p_drChofer.AsEnumerable()
                                   where rowChofer.Field<String>("IDCHOFER") != null && rowChofer.Field<String>("IDCHOFER").Trim().Equals(wsChofer.Trim())
                                   select new { IDCHOFER = rowChofer.Field<String>("IDCHOFER") }).Count();

                    if ((int)wchofer <= 0)
                    {
                         this.Alerta("Chofer no valida");
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        wsresult = false;

                    }

                }

                if (String.IsNullOrEmpty(TXTADDPLACABREAKBULK.Text.ToString()) != true)
                {
                    var wPlaca = (from row in p_drPlaca.AsEnumerable()
                                  where row.Field<String>("PLACA") != null && row.Field<String>("PLACA").Trim().Equals(TXTADDPLACABREAKBULK.Text.ToString().Trim().ToUpper())
                                  select row.Field<String>("PLACA")).Count();

                    if ((int)wPlaca <= 0)
                    {
                         this.Alerta("Placa no valida");
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        wsresult = false;

                    }
                }

                var validaCantidad = (from breakb in p_gvBreakBulk.AsEnumerable()
                                      where breakb.Field<String>("CONSECUTIVO").Equals(p_breabulkconsecutivo)
                                      select breakb).FirstOrDefault();

                Decimal wpeso = Decimal.Parse(String.IsNullOrEmpty(TXTCANTIDAD.Text.ToString()) == true ? "0" : TXTCANTIDAD.Text.ToString()) * Decimal.Parse(String.IsNullOrEmpty(TXtCANTPASES.Text.ToString()) == true ? "0" : TXtCANTPASES.Text.ToString());
                if (Decimal.Parse(validaCantidad["CANTIDAD"].ToString()) < wpeso)
                {
                     this.Alerta("La cantidad ingresada es mayor que la Cantidad Actual...");
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                    wsresult = false;
                }


                if (wsresult != false)
                {


                    validaCantidad["CANTIDAD"] = Decimal.Parse(validaCantidad["CANTIDAD"].ToString()) - wpeso;

                    p_gvBreakBulk.AcceptChanges();

                    if (p_drpasepuertabreakbulk == null)
                    {

                        IniDsPasePuertaBreakBulk();
                    }

                    p_drpasepuertabreakbulk.Rows.Add(new String[]{null,p_breabulkconsecutivo, TXtCANTPASES.Text,
                                                    TXTADDCIABREAKBULK.Text, 
                                                    wsEmpresa,
                                                    TXTADDPLACABREAKBULK.Text,
                                                     wsChofer, TxtAddChoferbreakbulk.Text, TXTCANTIDAD.Text,p_breabulkcarga});

                    p_drpasepuertabreakbulk.AcceptChanges();

                    Paserefress();

                    TXTCANTIDAD.Text = null;
                    TXtCANTPASES.Text = null;
                    TxtAddChoferbreakbulk.Text = null;
                    TXTADDPLACABREAKBULK.Text = null;
                    TXTADDCIABREAKBULK.Text = null;
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                }

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "CMDADDBBREAKBULK_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                 this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void btclean_Click(object sender, EventArgs e)
        {
            txtmrn.Text = "";
            txtmsn.Text = "";
            txthsn.Text = "";
            txtcntr.Text = "";
            TxtCiaTrans.Text = "";
            lblTotCntr.Text = "Tot. Contenedores: 0";
            //chkAll.Checked = false;
            p_datospp = new DataTable();
            /*
            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();
            */
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dsRetorno = new DataSet();
                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"));
                System.Xml.Linq.XDocument docXMLEXPO = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"));

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
                    var p_datos = pasePuerta.GetInfoPasePuertaBRBKSinRIDT(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
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
                var dtfacturascliente = pasePuerta.GetInfoFacturasClientePPWebBRBKXCarga(rucfaccli, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());

                if (dtfacturascliente.Rows.Count > 0)
                {
                    lbl_facturadoa.Text = TXTFACTURADOA;
                    gvFacturas.DataSource = dtfacturascliente;
                    gvFacturas.DataBind();
                }
                else
                {
                     this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                    return;
                }

                //GridViewRow row = ((GridViewRow)((Button)sender).NamingContainer);
                Label lblFacturaPP = (Label)gvFacturas.Rows[0].Cells[0].FindControl("lblFacturaPP");
                Label lblEstadoPP = (Label)gvFacturas.Rows[0].Cells[1].FindControl("lblEstadoPP");

                if (lblFacturaPP.Text != "0")
                {
                    var v_cliente = pasePuerta.GetValClienteBBK(lblFacturaPP.Text);
                    if (v_cliente.Rows[0]["V_FACTURA"].ToString() == "0")
                    {
                        var v_msg = v_cliente.Rows[0]["MENSAJE"].ToString();
                         this.Alerta(v_msg);
                        return;
                    }
                }
                String MRN = txtmrn.Text.Trim();
                String MSN = txtmsn.Text.Trim();
                String HSN = txthsn.Text.Trim();
                p_gvBreakBulk_ = new DataTable();
                p_gvBreakBulk_ = pasePuerta.GetInfoPasePuertaBRBK(txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim(), lblFacturaPP.Text.Trim());
                /*
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
                */
                if (true)
                {
                    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                    docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                         new System.Xml.Linq.XElement("PASEPUERTA",
                           new System.Xml.Linq.XElement("ConsultaCarga",
                     new System.Xml.Linq.XAttribute("MRN", String.IsNullOrEmpty(txtmrn.Text) == true ? "" : txtmrn.Text),
                     new System.Xml.Linq.XAttribute("MSN", String.IsNullOrEmpty(txtmsn.Text) == true ? "" : txtmsn.Text),
                     new System.Xml.Linq.XAttribute("HSN", String.IsNullOrEmpty(txthsn.Text) == true ? "" : txthsn.Text),
                     new System.Xml.Linq.XAttribute("Type", "IMPO"),
                     new System.Xml.Linq.XAttribute("REFERENCIA", String.IsNullOrEmpty("") == true ? "" : ""),
                     new System.Xml.Linq.XAttribute("CONTENEDOR", String.IsNullOrEmpty("") == true ? "" : "")
                                          )));
                    dsRetorno = WPASEPUERTA.GetBreakBulkN4info(docXML.ToString(), docXMLEXPO.ToString());
                    p_gvBreakBulk = dsRetorno.Tables[0];
                    factura_pp_cfs = lblFacturaPP.Text.Trim();
                    modalFacturasCliente.Hide();

                    var currentStatRow = (from currentStat in p_gvBreakBulk.AsEnumerable()
                                          select currentStat).ToList();

                    foreach (DataRow crow in currentStatRow)
                    {
                        if (String.IsNullOrEmpty(crow["DOCUMENTO"].ToString()))
                        {
                            crow["DOCUMENTO"] = p_gvBreakBulk_.Rows[0]["DOCUMENTO"].ToString();
                        }
                        p_gvBreakBulk.AcceptChanges();
                    }

                    p_breabulkconsecutivo = p_gvBreakBulk.Rows[0]["CONSECUTIVO"].ToString();
                    if (p_drpasepuertabreakbulk != null)
                    {
                        Paserefress();
                    }
                    TXTCANTIDAD.Text = null;
                    TXtCANTPASES.Text = null;
                    TxtAddChoferbreakbulk.Text = null;
                    TXTADDPLACABREAKBULK.Text = null;
                    TXTADDCIABREAKBULK.Text = null;
                    txtmrnmsnhsnppbrbk.Text = txtmrn.Text.ToUpper() + "-" + txtmsn.Text.ToUpper() + "-" + txthsn.Text.ToUpper();
                    p_breabulkcarga = txtmrnmsnhsnppbrbk.Text;
                    txtfecsalppbrbrk.Text = p_gvBreakBulk_.Rows[0]["FECHA_AUT_PPWEB_"].ToString();
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechasal = new DateTime();
                    if (!string.IsNullOrEmpty(txtfecsalppbrbrk.Text))
                    {
                        if (!DateTime.TryParseExact(txtfecsalppbrbrk.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                        {
                            this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsalppbrbrk.Text));
                            return;
                        }
                    }
                    sfechasalidabrbk = fechasal.ToString("MM/dd/yyyy");
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "CMDADDBBREAKBULK_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                 this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void txtfecsalppbrbrk_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!string.IsNullOrEmpty(txtfecsalppbrbrk.Text))
                {
                    if (!DateTime.TryParseExact(txtfecsalppbrbrk.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                         this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsalppbrbrk.Text));
                        txtfecsalppbrbrk.Text = p_gvBreakBulk_.Rows[0]["FECHA_AUT_PPWEB_"].ToString();
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        return;
                    }
                }

                var valcfsdo = pasePuerta.GetValFechaDomingosCFS(fechasal.ToString("yyyy-MM-dd"));
                if (valcfsdo.Rows[0]["VAL"].ToString() == "1")
                {
                    this.Alerta(valcfsdo.Rows[0]["MENSAJE"].ToString());
                    txtfecsalppbrbrk.Text = p_gvBreakBulk_.Rows[0]["FECHA_AUT_PPWEB_"].ToString();
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                    return;
                }
                //-->

                var dtvalfecha = pasePuerta.GetValFechaSalidaBRBK(fechasal.ToString("yyyy-MM-dd"), factura_pp_cfs, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                {
                    if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                    {
                         this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                        txtfecsalppbrbrk.Text = p_gvBreakBulk_.Rows[0]["FECHA_AUT_PPWEB_"].ToString();
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        return;
                    }
                }
                sfechasalidabrbk = fechasal.ToString("MM/dd/yyyy");
                if (p_drpasepuertabreakbulk != null)
                {
                    if (p_drpasepuertabreakbulk.Rows.Count > 0)
                    {
                        var bodega = pasePuerta.GetBodegaCargaBRBK(p_drpasepuertabreakbulk.Rows[0]["CONSECUTIVO"].ToString());
                        if (bodega == null)
                        {
                            this.Alerta("No se encontraron ubicación o bodega de la carga.");
                        }
                        else if (bodega.Rows.Count == 0)
                        {
                            this.Alerta("No se encontraron ubicación o bodega de la carga.");
                        }

                        dtHorariosBRBK = new DataTable();
                        dtHorariosBRBK = pasePuerta.GetTurnosDisponiblesBRBK(sfechasalidabrbk, bodega.Rows[0]["BODEGA"].ToString());
                        if (dtHorariosBRBK == null)
                        {
                            this.Alerta("No se encontraron horarios, revise la fecha.");
                        }
                        else if (dtHorariosBRBK.Rows.Count == 0)
                        {
                            this.Alerta("No se encontraron horarios, revise la fecha.");
                        }
                        gvHorariosBRBK.DataSource = dtHorariosBRBK;
                        gvHorariosBRBK.DataBind();
                    }
                    else
                    {
                        this.Alerta("Ingrese al menos un dato.");
                        dtHorariosBRBK = new DataTable();
                        gvHorariosBRBK.DataSource = dtHorariosBRBK;
                        gvHorariosBRBK.DataBind();
                    }
                }
                else
                {
                    dtHorariosBRBK = new DataTable();
                    gvHorariosBRBK.DataSource = dtHorariosBRBK;
                    gvHorariosBRBK.DataBind();
                }
                //UPMODETALLEBREAK.Update();
                MODALBREAKBULK.Show();
                //UPMODETALLEBREAK.Update();
                MODALBREAKBULK.Show();
               
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "txtfecsalppcfs_TextChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                 this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        public void Paserefress()
        {

            DataRow[] wrows = p_drpasepuertabreakbulk.Select("CONSECUTIVO = '" + p_breabulkconsecutivo + "'");
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("CONSECUTIVO", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTPASES", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("EMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDEMPRESA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("PLACA", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("IDCHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CHOFER", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CARGA", Type.GetType("System.String")));



            foreach (DataRow wrow in wrows)
            {

                dt.Rows.Add(new String[] { wrow["ID"].ToString(), wrow["CONSECUTIVO"].ToString(), wrow["CANTPASES"].ToString(), wrow["EMPRESA"].ToString(), wrow["IDEMPRESA"].ToString(), wrow["PLACA"].ToString(), wrow["IDCHOFER"].ToString(), wrow["CHOFER"].ToString(), wrow["CANTIDAD"].ToString(), wrow["CARGA"].ToString() });
            };

            dt.AcceptChanges();
            GVPASEAPUERTA.DataSource = dt;
            GVPASEAPUERTA.DataBind();

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

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (p_drpasepuertabreakbulk == null)
                {
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                     this.Alerta("No se encontraron datos para procesar el pase de puerta.");
                    return;
                }
                else if (p_drpasepuertabreakbulk.Rows.Count == 0)
                {
                    //UPMODETALLEBREAK.Update();
                    MODALBREAKBULK.Show();
                     this.Alerta("No se encontraron datos para procesar el pase de puerta.");
                    return;
                }
                else if (p_drpasepuertabreakbulk != null)
                {
                    if (p_drpasepuertabreakbulk.Rows.Count > 0)
                    {
                        if (dtHorariosBRBK == null)
                        {
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                             this.Alerta("Seleccione un Turno.");
                            return;
                        }
                        var result = (from currentStat in dtHorariosBRBK.AsEnumerable()
                                      where Convert.ToBoolean(currentStat.Field<Boolean>("CHECKED")) == true
                                      select currentStat);
                        var count = result.AsDataView().ToTable().Rows.Count;
                        if (count == 0)
                        {
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                             this.Alerta("Seleccione un Turno.");
                            return;
                        }
                    }
                }
                //UPMODETALLEBREAK.Update();
                MODALBREAKBULK.Hide();

                var p_user = Page.User.Identity.Name.ToString();
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!string.IsNullOrEmpty(txtfecsalppbrbrk.Text))
                {
                    if (!DateTime.TryParseExact(txtfecsalppbrbrk.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                         this.Alerta(string.Format("! El formato de fecha debe ser dia/Mes/Anio {0}", txtfecsalppbrbrk.Text));
                        txtfecsalppbrbrk.Text = sfechasalidabrbk;
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        return;
                    }
                }

                var dtvalfecha = pasePuerta.GetValFechaSalidaBRBK(fechasal.ToString("yyyy-MM-dd"), factura_pp_cfs, txtmrn.Text.Trim(), txtmsn.Text.Trim(), txthsn.Text.Trim());
                if (dtvalfecha.Rows.Count > 0 || dtvalfecha != null)
                {
                    if (dtvalfecha.Rows[0]["VALOR"].ToString() == "0")
                    {
                         this.Alerta(dtvalfecha.Rows[0]["MENSAJE"].ToString());
                        txtfecsalppbrbrk.Text = sfechasalidabrbk;
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Show();
                        return;
                    }
                }
                //sfechasalidabrbk = fechasal.ToString("dd/MM/yyyy");

                DataTable dt = new DataTable();
                List<String> XMLN4BreakBulk = new List<String>();

                dt.Columns.Add(new DataColumn("CONSECUTIVO", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("IDEMPRESA", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("PLACA", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("IDCHOFER", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("CANTIDAD", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("FECHA_SALIDA", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("CODSUBITEM", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("TIPO_CARGA", Type.GetType("System.String")));
                dt.AcceptChanges();


                var wdatos = (from wdata in p_gvBreakBulk.AsEnumerable().AsParallel()
                              join wdatapase in p_drpasepuertabreakbulk.AsEnumerable().AsParallel()
                              on wdata.Field<String>("CONSECUTIVO") equals wdatapase.Field<String>("CONSECUTIVO")
                              select new
                              {
                                  CONSECUTIVO = wdata.Field<String>("CONSECUTIVO"),
                                  IDEMPRESA = wdatapase.Field<String>("IDEMPRESA"),
                                  PLACA = wdatapase.Field<String>("PLACA"),
                                  IDCHOFER = wdatapase.Field<String>("IDCHOFER"),
                                  CANTIDAD = wdatapase.Field<String>("CANTIDAD"),
                                  FECHA_SALIDA = fechasal.ToString("MM/dd/yyyy"),//wdata.Field<String>("FECHA_SALIDA"),
                                  CARGA = wdatapase.Field<String>("CARGA"),
                                  CANTPASES = wdatapase.Field<String>("CANTPASES")
                              }).ToList();

                foreach (DataRow wrow in LINQToDataTable(wdatos).Rows)
                {
                    for (int i = 0; i < int.Parse(wrow["CANTPASES"].ToString()); i++)
                    {

                        dt.Rows.Add(new String[] { wrow["CONSECUTIVO"] == null ? "" : wrow["CONSECUTIVO"].ToString(),
                                                                   wrow["IDEMPRESA"] == null ? "" : wrow["IDEMPRESA"].ToString(),
                                                                   wrow["PLACA"] == null ? "" : wrow["PLACA"].ToString().ToUpper(),
                                                                   wrow["IDCHOFER"] == null ? "" :wrow["IDCHOFER"].ToString(),
                                                                    wrow["CANTIDAD"] == null ? "" : wrow["CANTIDAD"].ToString(),
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
                                    new System.Xml.Linq.XAttribute("value", fechasal.ToString("yyyy-MM-dd") + " 00:00")),
                                    new System.Xml.Linq.XElement("parameter", new System.Xml.Linq.XAttribute("id", "referencia"),
                                new System.Xml.Linq.XAttribute("value", "")),
                                new System.Xml.Linq.XElement("parameter",
                                    new System.Xml.Linq.XAttribute("id", "BLs"),
                                    new System.Xml.Linq.XAttribute("value", wrow["CARGA"] == null ? "" : wrow["CARGA"].ToString())),
                                    new System.Xml.Linq.XElement("parameter",
                                        new System.Xml.Linq.XAttribute("id", "QTY"),
                                        new System.Xml.Linq.XAttribute("value", wrow["CANTIDAD"] == null ? "" : wrow["CANTIDAD"].ToString())),
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

                    if (ds_report.Tables[0].TableName.Equals("DTError"))
                    {
                        gvHorariosBRBK.DataSource = dtHorarios;
                        gvHorariosBRBK.DataBind();
                        if (ds_report.Tables[0].Rows.Count > 0)
                        {
                             this.Alerta(ds_report.Tables[0].Rows[0]["CARGA"].ToString() + " - " + ds_report.Tables[0].Rows[0]["MENSAJE"]);
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                        }
                        else
                        {
                             this.Alerta("No se pudo Generar el Pase de Puerta, Carga sin Stock o Consulte N4.");
                            //UPMODETALLEBREAK.Update();
                            MODALBREAKBULK.Show();
                        }

                    }
                    else
                    {
                        /*Pase Puerta BRBK*/
                        DataTable resultado = new DataTable();
                        DataView view = new DataView();
                        var resultss = from myRow in dtHorariosBRBK.AsEnumerable()
                                       where Convert.ToBoolean(myRow.Field<Boolean>("CHECKED")) == true
                                       select myRow;
                        view = resultss.AsDataView();
                        resultado = view.ToTable();

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
                        if (!pasePuerta.InsertaReservaDeTurno(writer.ToString(), writerdt.ToString(), sfechasalida, 0, Page.User.Identity.Name, out msjerror))
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

                        var dtPPWebBRBKDet = (from wppweb in p_drpasepuertabreakbulk.AsEnumerable()
                                             select wppweb).AsDataView().ToTable();
                        StringWriter xmlPPWebBRBKDet = new StringWriter();
                        dtPPWebBRBKDet.TableName = "PaseWebDet";
                        dtPPWebBRBKDet.WriteXml(xmlPPWebBRBKDet);

                        String WfACTURADOA = p_gvBreakBulk_.Rows[0]["FACTURADO"].ToString();
                        String wuser = p_user;
                        String wsAgente = p_gvBreakBulk_.Rows[0]["AGENTE"].ToString();
                        String wempr = p_gvBreakBulk_.Rows[0]["FACTURADO"].ToString();

                        msjerror = string.Empty;
                        if (!pasePuerta.AcualizaPaseWebBRBK(xmlPPWeb.ToString(), xmlPPWebBRBK.ToString(), xmlPPWebBRBKDet.ToString(), p_gvBreakBulk_.Rows[0]["FACTURA"].ToString(), wsAgente, wempr, "", "", "", p_user, out msjerror))
                        {
                             this.Alerta(msjerror);
                            return;
                        }

                        EstadoPPSinTurno = true;
                         this.Alerta("Pase a Puerta generado Exitosamente... ");
                        var function = "openPop('" + p_drpasepuertabreakbulk.Rows[0]["CARGA"].ToString() + "','" + ds_report.Tables[0].Rows[0]["PASE"].ToString() + "');";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", function, true);
                        p_drpasepuertabreakbulk = new DataTable();
                        dtHorariosBRBK = new DataTable();
                        IniDsPasePuertaBreakBulk();
                        gvHorariosBRBK.DataSource = null;
                        gvHorariosBRBK.DataBind();
                        gvFacturas.DataSource = null;
                        gvFacturas.DataBind();
                        GVPASEAPUERTA.DataSource = null;
                        GVPASEAPUERTA.DataBind();
                        txtmrn.Text = "";
                        txtmsn.Text = "";
                        txthsn.Text = "";
                        txtcntr.Text = "";
                        TxtCiaTrans.Text = "";
                        lblTotCntr.Text = "Tot. Contenedores: 0";
                        p_datospp = new DataTable();
                        //UPMODETALLEBREAK.Update();
                        MODALBREAKBULK.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "emision_pp_web", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                 this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
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

        //private void IniDsChofer()
        //{
        //    DataSet dsRetorno = new DataSet();
        //    WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
        //    dsRetorno = WPASEPUERTA.GetChoferinfo();
        //    p_drChofer = dsRetorno.Tables[0];

        //}
        private void IniDsChofer(/*string empresa*/)
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

        [System.Web.Services.WebMethod]
        public static string[] GetEmpresaList(string prefix)
        {

            WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            XmlDocument docXml = new XmlDocument();
            XmlElement elem;

            DataTable DTRESULT = new DataTable();

            DTRESULT = (DataTable)HttpContext.Current.Session["drEmpresaPPWebbRBK"];
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
            DTRESULT = (DataTable)HttpContext.Current.Session["drChoferbrbk"];//drChoferPPWeb"];

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
    }
}