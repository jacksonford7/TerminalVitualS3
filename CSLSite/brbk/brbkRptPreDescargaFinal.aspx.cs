using BillionEntidades;
using BreakBulk;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CSLSite.brbk
{
    public partial class brbkRptPreDescargaFinal : System.Web.UI.Page
    {
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        #region "Variables"
        private string cMensajes;
        //private static Int64? lm = -3;
        private string OError;
        #endregion

        #region "Propiedades"
        private Int64? nSesion { get { return (Int64)Session["nSesion"]; } set { Session["nSesion"] = value; } }
        #endregion

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();
        }

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCAB.Update();
            UPBOTONES.Update();
        }
        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
        }
        private void Limpia_Datos_cliente()
        {
            this.TXTMRN.Text = string.Empty;
            this.txtNave.Text = string.Empty;
            this.txtDescripcionNave.Text = string.Empty;
        }
        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }
        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg_det.Visible = true;
            //this.banmsg.Visible = true;
            //this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            //this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            //this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                Page.Tracker();
                if (!IsPostBack)
                {
                    this.IsCompatibleBrowser();
                    Page.SslOn();
                }

                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }

               // this.banmsg.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                   // this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    ClsUsuario = Page.Tracker();
                    if (ClsUsuario != null)
                    {
                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                    }

                    this.txtNave.Text = string.Empty;
                    this.TXTMRN.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var t = this.getUserBySesion();

            try
            {
                if (!IsPostBack)
                {
                    if (Response.IsClientConnected)
                    {
                        try
                        {
                            if (HttpContext.Current.Request.Cookies["token"] == null)
                            {
                                System.Web.Security.FormsAuthentication.SignOut();
                                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                                Session.Clear();
                                return;
                            }

                            ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                            this.Crear_Sesion();
                            sinresultado.Visible = false;
                        }
                        catch 
                        {
                            throw;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        protected void ValidarDatosNave()
        {
            try
            {
                var oNave = nave.GetNave(txtNave.Text);
                txtDescripcionNave.Text = oNave.name;
                TXTMRN.Text = oNave.in_customs_voy_nbr;
            }
            catch { }

        }

        protected void btnconsultar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNave.Text))
            {
                return;
            }

            var t = this.getUserBySesion();
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }
                populate(txtNave.Text, t.loginname);
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }

      

        private void populate(string ruc, string login)
        {
            grilla.DataSource = null;
            grilla.DataBind();

            sinresultado.Visible = false;
          
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
            if (Response.IsClientConnected)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.txtNave.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.txtNave.Focus();
                        return;
                    }

                    ValidarDatosNave();

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    var Resultado = tarjaDet.rptConsultaPredescargaFinal(txtNave.Text, out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                    }

                    if (Resultado != null)
                    {
                        if (Resultado.Rows.Count > 0)
                        {
                            grilla.DataSource = Resultado;
                            grilla.DataBind();
                        }
                        else
                        {
                            sinresultado.Visible = true;
                        }
                    }
                    else
                    {
                        
                        sinresultado.Visible = true;
                    }
                    this.Actualiza_Paneles();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
           
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpia_Datos_cliente();
            grilla.DataSource = null;
            grilla.DataBind();
            this.Actualiza_Paneles();
        }

    }
}