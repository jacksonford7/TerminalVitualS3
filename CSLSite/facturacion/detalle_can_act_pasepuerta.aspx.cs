using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Xml;
using csl_log;

namespace CSLSite
{
    public partial class detalle_can_act_pasepuerta : System.Web.UI.Page
    {
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
                return (DataTable)Session["p_datosppWebDAC"];
            }
            set
            {
                Session["p_datosppWebDAC"] = value;
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
/*
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        */
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            //if (Response.IsClientConnected && !IsPostBack)
            //{
                IniPaseWeb();
            //}
        }

        private void IniPaseWeb()
        {
            p_datospp = new DataTable();
            p_datospp = (DataTable)Session["p_datosppWebAC"];
            tablePaginationPPWeb.DataSource = p_datospp;
            tablePaginationPPWeb.DataBind();
            IniDsEmpresa();
            IniDsChofer();
            IniTurno();
            IniDsPlaca();
            //IniDatosPPWeb();
            p_datospp.Columns.Add("PASE");
            p_datospp.Columns.Add("ID_PASE");
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
                dsresult.Rows[0]["TURNO"] = "* ELIJA";
                ddlTurno.DataSource = dsresult.AsDataView();
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
        }

        private void IniDsChofer()
        {
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
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
            dsRetorno.Tables[0].Rows.Add("0", "* ELIJA *");
            DataView myDataView = dsRetorno.Tables[0].DefaultView;
            myDataView.Sort = "CHOFER ASC";

            p_drChofer = myDataView.ToTable();

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                DropDownList ddlChofer = item.FindControl("ddlChofer") as DropDownList;
                ddlChofer.DataSource = myDataView;
                ddlChofer.DataTextField = "CHOFER";
                ddlChofer.DataValueField = "IDCHOFER";
                ddlChofer.DataBind();
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

            for (int i = 0; i < dsRetorno.Tables[0].Rows.Count; i++)
            {
                String value = dsRetorno.Tables[0].Rows[i]["EMPRESA"].ToString();
                Char delimiter = '-';
                List<string> substringemp = value.Split(delimiter).ToList();
                string empresa = string.Empty;
                for (int l = 0; l < substringemp.Count; l++)
                {
                    if (l == 0)
                    {
                        empresa = substringemp[l + 1].Trim();
                    }
                    else
                    {
                        if ((l + 1) < substringemp.Count)
                        {
                            empresa = empresa + " - " + substringemp[l + 1].Trim();
                        }
                    }
                }
                dsRetorno.Tables[0].Rows[i]["EMPRESA"] = empresa;
            }
            dsRetorno.Tables[0].Rows.Add("0", "* ELIJA *");
            DataView myDataView = dsRetorno.Tables[0].DefaultView;
            myDataView.Sort = "EMPRESA ASC";

            p_drEmpresa = myDataView.ToTable();

            foreach (RepeaterItem item in tablePaginationPPWeb.Items)
            {
                DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                ddlEmpresa.DataSource = myDataView;
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

        private void IniDsPlaca()
        {
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            dsRetorno = WPASEPUERTA.GetPlacainfo();
            p_drPlaca = dsRetorno.Tables[0];
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
                if (ddlTurno.SelectedItem.Value == "0")
                {
                    this.Alerta("Elija un Turno para el Contenedor: " + lblCntr.Text);
                    chkPase.Checked = false;
                    return;
                }
                DropDownList ddlEmpresa = item.FindControl("ddlEmpresa") as DropDownList;
                if (ddlEmpresa.SelectedItem.Value == "0")
                {
                    this.Alerta("Elija una Cia. Trans para el Contenedor: " + lblCntr.Text);
                    chkPase.Checked = false;
                    return;
                }
                DropDownList ddlChofer = item.FindControl("ddlChofer") as DropDownList;
                TextBox txtPlaca = item.FindControl("txtPlaca") as TextBox;
                if (ddlChofer.SelectedItem.Value != "0")
                {
                    if (string.IsNullOrEmpty(txtPlaca.Text))
                    {
                        this.Alerta("Elija una Placa para el Contenedor: " + lblCntr.Text);
                        chkPase.Checked = false;
                        return;
                    }
                    else
                    {
                        var wPlaca = (from row in p_drPlaca.AsEnumerable()
                                      where row.Field<String>("PLACA") != null && row.Field<String>("PLACA").Trim().Equals(txtPlaca.Text.ToString().Trim().ToUpper())
                                      select row.Field<String>("PLACA")).Count();

                        if ((int)wPlaca <= 0)
                        {
                            this.Alerta("La Placa no es valida para el Contenedor: " + lblCntr.Text);
                            chkPase.Checked = false;
                            return;
                        }
                        p_datospp.Rows[item.ItemIndex]["CHOFER"] = ddlChofer.SelectedItem.Value;
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
                }
                if (ddlEmpresa.SelectedItem.Value != "0")
                {
                    p_datospp.Rows[item.ItemIndex]["CIATRANS"] = ddlEmpresa.SelectedItem.Value == "0" ? "" : ddlEmpresa.SelectedItem.Value;
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
      
    }
}