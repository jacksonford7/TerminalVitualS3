using csl_log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.sav
{
    public partial class turnosSav : System.Web.UI.Page
    {
        TurnoSav pb = new TurnoSav();
        usuario ClsUsuario;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);
                return;
            }

            if (!Page.IsPostBack)
            {
                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    this.txtUsuario.Text = string.Format("{0}", ClsUsuario.loginname);
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           // this.Master.Titulo = "HORARIOS CAMARA FRIA";
            //this.Master.FavName = "[HORARIOS]";

            if (!Page.IsPostBack)
            {
                pError.InnerText = string.Empty;
                LlenaComboDepositos();

                Parameter p = SqlDataSource1.SelectParameters["ParameterDeposito"];
                SqlDataSource1.SelectParameters.Remove(p);
                SqlDataSource1.SelectParameters.Add("ParameterDeposito", cmbDepositoList.SelectedValue.ToString() );

                Parameter p1 = SqlDataSource1.UpdateParameters["ParameterUsuario"];
                SqlDataSource1.UpdateParameters.Remove(p1);
                SqlDataSource1.UpdateParameters.Add("ParameterUsuario", txtUsuario.Text.ToString());
            }
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            //aqui controlo y valido todo

        }
        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            //Parameter p1 = SqlDataSource1.UpdateParameters["ParameterUsuario"];
            //SqlDataSource1.UpdateParameters.Remove(p1);
            //SqlDataSource1.UpdateParameters.Add("ParameterUsuario", txtUsuario.Text.ToString());

            if (e.Exception != null)
            {

                pError.InnerText = e.Exception.Message;
                e.ExceptionHandled = true;
                return;
            }

            pError.InnerText = "Registro actualizado exitosamente";
        }

        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                pError.InnerText = e.Exception.Message;
                e.ExceptionHandled = true;
                return;
            }
            pError.InnerText = "Registro ingresado exitosamente";
        }

        protected void Button2_Click(object sender, EventArgs e) //actualiza estado detalle
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }


        protected void btnNuevo_Click(object sender, EventArgs e) //registra cabecera
        {
            try
            {
                TextBox Hora = (TextBox)DetailsView1.FindControl("TextBox1");
                TextBox Unidades = (TextBox)DetailsView1.FindControl("TextBox2");
                DropDownList Dia = (DropDownList)DetailsView1.FindControl("DropDownList1");
                if (string.IsNullOrEmpty(Hora.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta("El campo 'HORA', no debe ser nulo.").ToString(), true);
                    Hora.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(Unidades.Text) || Unidades.Text == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta("El campo 'UNIDADES DISPONIBLES', no debe ser nulo o cero.").ToString(), true);
                    Unidades.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(Dia.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta("El campo 'DIA', no debe ser nulo.").ToString(), true);
                    Dia.Focus();
                    return;
                }
                pb.RegistraDetalleDeHorarios(Hora.Text, "00", Convert.ToInt32(Unidades.Text), Dia.SelectedValue, Convert.ToInt32(cmbDeposito.SelectedValue), Dia.SelectedItem.Text,txtUsuario.Text);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta("Transacciòn exitosa.").ToString(), true);
                SqlDataSource1.SelectCommand = "SELECT A.[HORA], A.[CNTR_DISP], A.[DIA], A.[ID_HORARIO], A.ESTADO,B.tipo_descripcion AS TIPO , B.id AS ID_TIPO FROM [PNA_HORARIOS_CAMARA_FRIA] A JOIN PNA_TIPO_REFRIGERACION B ON (A.ID_TIPO = B.ID ) WHERE A.[ESTADO] = 'A' AND  B.id = " + cmbDepositoList.SelectedValue.ToString() + "  ORDER BY 3,1";
                SqlDataSource1.DataBind();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta(ex.Message).ToString(), true);
            }

        }

        protected void SqlDataSource2_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                pError.InnerText = e.Exception.Message;
                e.ExceptionHandled = true;
                return;
            }

            pError.InnerText = "Registro actualizado exitosamente";
        }

        protected void btnIngresarCab_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SAV_DIA"] = "NULL";
                Session["SAV_HORA"] = "NULL";

                if (cmbDeposito.SelectedValue == "-1")
                {
                    this.Alerta("Seleccione un depósito.");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                    cmbDeposito.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtCantDisp.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta("El campo 'UNIDADES DISPONIBLES', no debe ser nulo o cero.").ToString(), true);
                    txtCantDisp.Focus();
                    return;
                }
                pb.RegistraDetalleDeHorarios(ddlHora.SelectedValue, ddlMinuto.SelectedValue, Convert.ToInt32(txtCantDisp.Text), ddlDiaCab.SelectedValue, Convert.ToInt32(cmbDeposito.SelectedValue), ddlDiaCab.SelectedItem.Text, txtUsuario.Text);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta("Transacciòn exitosa.").ToString(), true);
                SqlDataSource1.SelectCommand = "SELECT A.[HORA], A.[CNTR_DISP], A.[DIA], A.[ID_HORARIO], A.ESTADO,B.descripcion AS DEPOSITO_DESC , B.id AS DEPOSITO  FROM [SAV_HORARIOS] A JOIN ZAL_DEPOT B ON (A.DEPOSITO = B.ID ) WHERE A.[ESTADO] = 'A' AND  B.id = " + cmbDepositoList.SelectedValue.ToString() + " ORDER BY 3,1";
                SqlDataSource1.DataBind();
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                if (ex.Message == "YA EXISTE UN HORARIO ACTIVO")
                {
                    SqlDataSource1.SelectCommand = "SELECT A.[HORA], A.[CNTR_DISP], A.[DIA], A.[ID_HORARIO], A.ESTADO,B.descripcion AS DEPOSITO_DESC , B.id AS DEPOSITO FROM [SAV_HORARIOS] A JOIN ZAL_DEPOT B ON (A.DEPOSITO = B.ID ) WHERE A.[ESTADO] = 'A'  AND A.DIA='" + ddlDiaCab.SelectedValue + "' AND A.HORA='" + (ddlHora.SelectedValue + ":" + ddlMinuto.SelectedValue) + "' ORDER BY 3,1";
                    SqlDataSource1.DataBind();
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta(ex.Message).ToString(), true);
            }
        }

        protected void btnCancelarCab_Click(object sender, EventArgs e)
        {
            ddlDiaCab.SelectedValue = "1";
            ddlHora.SelectedValue = "00";
            ddlMinuto.SelectedValue = "00";
            txtCantDisp.Text = "";
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Select") == true)
                {
                    GridViewRow wRow = GridView1.Rows[int.Parse(e.CommandArgument.ToString())];

                    Label Hora = (Label)GridView1.Rows[wRow.RowIndex].FindControl("Label1");
                    DropDownList Dia = (DropDownList)GridView1.Rows[wRow.RowIndex].FindControl("DropDownList1");
                    Label IdTipo = (Label)GridView1.Rows[wRow.RowIndex].FindControl("Label7");
                    pb.ActualizaDetalleDeHorarios(Hora.Text, Dia.SelectedValue, Convert.ToInt32(IdTipo.Text), Dia.SelectedItem.Text, txtUsuario.Text);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta("Transacciòn exitosa.").ToString(), true);

                    SqlDataSource1.SelectCommand = "SELECT A.[HORA], A.[CNTR_DISP], A.[DIA], A.[ID_HORARIO], A.ESTADO,B.descripcion AS DEPOSITO_DESC , B.id AS DEPOSITO  FROM [SAV_HORARIOS] A JOIN ZAL_DEPOT B ON (A.DEPOSITO = B.ID ) WHERE A.[ESTADO] = 'A' AND  B.id = " + cmbDepositoList.SelectedValue.ToString() + " ORDER BY 3,1";
                    SqlDataSource1.DataBind();
                    GridView1.DataBind();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", pb.Alerta(ex.Message).ToString(), true);
            }

        }
        
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Parameter p1 = SqlDataSource1.UpdateParameters["ParameterUsuario"];
            SqlDataSource1.UpdateParameters.Remove(p1);
            SqlDataSource1.UpdateParameters.Add("ParameterUsuario", txtUsuario.Text.ToString());
        }

        protected void btnCancelar__Click(object sender, EventArgs e)
        {

        }

        protected void btnAceptar__Click(object sender, EventArgs e)
        {

        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = "SELECT A.[HORA], A.[CNTR_DISP], A.[DIA], A.[ID_HORARIO], A.ESTADO,B.descripcion AS DEPOSITO_DESC , B.id AS DEPOSITO  FROM [SAV_HORARIOS] A JOIN ZAL_DEPOT B ON (A.DEPOSITO = B.ID ) WHERE A.[ESTADO] = 'A' AND  B.id = " + cmbDepositoList.SelectedValue.ToString() + " ORDER BY 3,1";
            SqlDataSource1.DataBind();
        }

        public void LlenaComboDepositos()
        {
            try
            {
                cmbDeposito.DataSource = man_pro_expo.consultaDepositosFiltrado("1"); //ds.Tables[0].DefaultView;
                cmbDeposito.DataValueField = "ID";
                cmbDeposito.DataTextField = "DESCRIPCION";
                cmbDeposito.DataBind();
                cmbDeposito.Enabled = true;

                cmbDepositoList.DataSource = man_pro_expo.consultaDepositosFiltrado("2"); //ds.Tables[0].DefaultView;
                cmbDepositoList.DataValueField = "ID";
                cmbDepositoList.DataTextField = "DESCRIPCION";
                cmbDepositoList.DataBind();
                cmbDepositoList.Enabled = true;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pases_zal", "LlenaComboDepositos()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void cmbDeposito_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = "SELECT A.[HORA], A.[CNTR_DISP], A.[DIA], A.[ID_HORARIO], A.ESTADO,B.descripcion AS DEPOSITO_DESC , B.id AS DEPOSITO  FROM [SAV_HORARIOS] A JOIN ZAL_DEPOT B ON (A.DEPOSITO = B.ID ) WHERE A.[ESTADO] = 'A' AND  B.id = "+ cmbDepositoList.SelectedValue.ToString() +"  ORDER BY 3,1";
            SqlDataSource1.DataBind();
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //SqlDataSource1.SelectParameters.Add("ParameterDeposito", cmbDepositoList.SelectedValue.ToString());
            Parameter p = SqlDataSource1.SelectParameters["ParameterDeposito"];
            SqlDataSource1.SelectParameters.Remove(p);
            SqlDataSource1.SelectParameters.Add("ParameterDeposito", cmbDepositoList.SelectedValue.ToString());

            //Parameter p1 = SqlDataSource1.UpdateParameters["ParameterUsuario"];
            //SqlDataSource1.UpdateParameters.Remove(p1);
            //SqlDataSource1.UpdateParameters.Add("ParameterUsuario", txtUsuario.Text.ToString());
        }
    }
}