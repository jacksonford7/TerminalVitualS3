using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Services;

namespace CSLSite
{
    public partial class turnos_zal : System.Web.UI.Page
    {
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

        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
                DataTable dtTurnosZAL = new DataTable();
                var fechasalida = Convert.ToDateTime(Request.QueryString["fsal"]).ToString("yyyy-MM-dd");
                var linea = Request.QueryString["line"];
                var placa = Request.QueryString["placa"];
                var chofer = Request.QueryString["chofer"];
                var bkg = Request.QueryString["bkg"];
                var pase = Request.QueryString["pase"];
                var turno = Request.QueryString["turno"];
                /*
                dtTurnosZAL = N4_P_Cons_Turnos_Disponibles_ZAL(fechasalida, linea.ToString());
                dtTurnosZAL = dtTurnosZAL.Select("Plan_Id <> 0").CopyToDataTable();
                dtTurnosZAL.Columns.Add("CHECK");
                dtTurnosZAL.Columns.Add("IDPZAL");
                cblturnos.DataSource = dtTurnosZAL;
                cblturnos.DataValueField = "Plan";
                cblturnos.DataTextField = "Inicio";
                cblturnos.DataBind();
                */
                txtChofer.Text = chofer;
                txtPlaca.Text = placa;
                lblBooking.Text = bkg;
                lblPase.Text = pase;
                lblFechaSalida.Text = Request.QueryString["fsal"];
                lblTurnoPase.Text = turno;
                IniDsChofer("");
                IniDsPlaca("");
            }
        }

        private void IniDsPlaca(string empresa)
        {
            p_drPlaca = pasePuerta.GetPlacainfo(empresa);
        }

        private void IniDsChofer(string empresa)
        {
            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            var dtRetorno = pasePuerta.GetChoferinfo(empresa);
            if (dtRetorno.Rows.Count > 0)
            {
                DataView myDataView = dtRetorno.DefaultView;
                myDataView.Sort = "CHOFER ASC";

                p_drChofer = myDataView.ToTable();
                p_drChoferFilter = p_drChofer;
            }
        }

        protected void cblturnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int ind = cblturnos.SelectedIndex;
                //if (string.IsNullOrEmpty(txtqty.Text))
                //{
                //    cblturnos.Items[ind].Selected = false;
                //    this.Alerta("Ingrese la cantidad solicitada.");
                //    txtqty.Focus();
                //    return;
                //}

                //int top = int.Parse(txtqty.Text);
                //if (top == 0)
                //{
                //    cblturnos.Items[ind].Selected = false;
                //    this.Alerta("La cantidad solicitada debe ser mayor a cero.");
                //    txtqty.Focus();
                //    return;
                //}

                //bool val = false;

                //if (ind>=0)
                //    val=cblturnos.Items[cblturnos.SelectedIndex].Selected;
                
                

                //for (var a = 0; a < cblturnos.Items.Count; a++)
                //{
                //    if(dtTurnosZAL.Rows[a]["CHECK"].ToString()=="")
                //        dtTurnosZAL.Rows[a]["CHECK"]="False";

                //    if (dtTurnosZAL.Rows[a]["CHECK"].ToString() != cblturnos.Items[a].Selected.ToString())
                //    {
                //        ind = a;
                //        val = cblturnos.Items[a].Selected;
                //        a = cblturnos.Items.Count;
                //        //numbook.InnerText = "";
                //    }
                //}
                

                //if (cblturnos.Items.Count-1 < top)
                //{
                //    cblturnos.Items[ind].Selected = false;
                //    this.Alerta("La cantidad solicitada: " + top + ", supera los turnos disponibles en la fecha indicada: " + fecsalida.Text);
                //    txtqty.Focus();
                //    return;
                //    //mensaje cantidad solicitada supera los turnos disponibles en la fecha indicada
                //}
                //else
                //{
                //    if (ind == 0)
                //    {
                //        dtTurnosZAL.Rows[ind]["CHECK"] = val;

                //        if (val)
                //        {
                //            var result = (from currentStat in dtTurnosZAL.AsEnumerable()
                //                          where currentStat.Field<string>("CHECK") == "True"
                //                          select currentStat);

                //            var count = result.AsDataView().ToTable().Rows.Count;

                //            if (count < 3)
                //            {
                //                if (count == 2)
                //                {
                //                    cont = 0;

                //                    for (var a = 1; a < cblturnos.Items.Count; a++)
                //                    {
                //                        if (cblturnos.Items[a].Selected)
                //                            cont += 1;

                //                        if (cont > 0 && cont <= top)
                //                        {
                //                            dtTurnosZAL.Rows[a]["CHECK"] = val;
                //                            cblturnos.Items[a].Selected = val;
                //                            cont += 1;
                //                        }
                //                        if (cont > top)
                //                            a = cblturnos.Items.Count;
                //                    }
                //                }
                //                else
                //                {
                //                    for (var a = 1; a <= top; a++)
                //                    {
                //                        dtTurnosZAL.Rows[a]["CHECK"] = val;
                //                        cblturnos.Items[a].Selected = val;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                var d = true.ToString();
                //                cblturnos.Items[ind].Selected = false;
                //                //mensaje ya ha seleccionado 2 o más turnos, no aplica el all
                //            }

                //        }
                //        else
                //        {
                //            for (var a = 1; a < cblturnos.Items.Count; a++)
                //            {
                //                dtTurnosZAL.Rows[a]["CHECK"] = val;
                //                cblturnos.Items[a].Selected = val;
                //            }
                //        }
                //        //cblturnos.DataSource = dtTurnosZAL;
                //        //cblturnos.DataValueField = "Plan";
                //        //cblturnos.DataTextField = "Inicio";
                //        //cblturnos.DataBind();
                //    }
                //    else
                //    {
                //        dtTurnosZAL.Rows[ind]["CHECK"] = val;
                //    }
                //}
                //dtTurnosZAL.AcceptChanges();
            }
            catch(Exception)
            {
                    //cont=0;
                    //mensaje error
            }
        }

        private static SqlConnection ConexionN4Middleware()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }

        public static DataTable N4_P_Cons_Turnos_Disponibles_ZAL(string fecha, string lineapro)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "N4_P_Cons_Turnos_Disponibles_ZAL";
                coman.Parameters.AddWithValue("@FECHA", fecha);
                coman.Parameters.AddWithValue("@LINEA", lineapro);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoEmails", "N4_P_Cons_Turnos_Disponibles_ZAL", DateTime.Now.ToShortDateString());
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }

        protected void find_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                /*
                var u = this.getUserBySesion();
                var t = turno.GetBookingsZAL_(txtname.Text.Trim(), u.ruc);
                */
                try
                {
                    /*
                    if (t != null && t.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(t.Rows[0]["alerta"].ToString()))
                        {
                            var alert = "Usted no es el propietario de este Booking: " + txtname.Text.Trim();
                            xfinder.Visible = false;
                            alerta.Attributes["class"] = string.Empty;
                            alerta.Attributes["class"] = "msg-info";
                            this.alerta.InnerHtml = "";

                            this.tablePagination.DataSource = null;
                            this.tablePagination.DataBind();
                            this.Alerta(alert);
                            return;
                        }

                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";
                        this.alerta.InnerHtml = "Estimado cliente, si el número de booking que esta buscando no aparece en esta lista. <br />" +
                                       "Por favor comunicarse con la Linea Naviera.";
                        Session["s_linea_proforma"] = t.Rows[0]["linea"].ToString();
                        return;
                    }
                    
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                    */
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    /*
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", txtname.Text, u != null ? u.loginname : "userNull"));
                    */
                    sinresultado.Visible = true;
                }
            }
        }

        [WebMethod(EnableSession = true)]
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
    }
}