using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using CSLSite.N4Object;
using CSLSite.XmlTool;
using ConectorN4;
using Newtonsoft.Json;
using csl_log;
using System.Text;
using ClsNotasCreditos;
using System.Data;
using System.Globalization;


namespace CSLSite
{
    public partial class frm_concepts : System.Web.UI.Page
    {

        private concepts objConceptos = new concepts();
        usuario user;
        string sg;

        #region "Propiedades"

        public static string v_mensaje = string.Empty;

        #endregion

        #region "Metodos"


        private void MessageBox(String msg, Page page)
        {
            StringBuilder s = new StringBuilder();
            msg = msg.Replace("'", " ");
            msg = msg.Replace(System.Environment.NewLine, " ");
            msg = msg.Replace("/", "");
            s.Append(" alert('").Append(msg).Append("');");
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);

        }

        private void Limpiar()
        {

            Session["Action"] = "I";//NUEVO INGRESO 
            this.TxtDescripcion.Text = null;

            Tableconceptos.DataSource = null;
            Tableconceptos.DataBind();

            this.Genera_Id_Concepto();

            this.CboEstado.Items.Clear();
            this.CboEstado.Items.Add("Activo");
            this.CboEstado.Items.Add("Inactivo");

            this.TxtDescripcion.Focus();

        }


        private void Genera_Id_Concepto()
        {
            try
            {
                List<concepts> Lista = concepts.Max_Id_Concepto();
                if (Lista != null)
                {
                    var xList = Lista.FirstOrDefault();

                    if (xList != null)
                    {
                        this.LblID.Text = xList.id_concept.ToString("D3");
                    }
                    else
                    {
                        this.LblID.Text = "0";
                    }
                }
                else { this.LblID.Text = "0"; }

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        private void Carga_ListadoConceptos()
        {
            try
            {

                List<concepts> ListConceptos = concepts.ListConceptos();
                if (ListConceptos != null)
                {
                    Tableconceptos.DataSource = ListConceptos;
                    Tableconceptos.DataBind();
                }
                else
                {
                    Tableconceptos.DataSource = null;
                    Tableconceptos.DataBind();
                }



            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "line_depots", "CargaLineaNaviera", "Hubo un error al cargar depositos por Línea de Naviera", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        


        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }



        #region "Eventos del Form"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            user = Page.Tracker();
            if (user != null)
            {

                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);

            }
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {

                var sp = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                string t = null;
                if (!string.IsNullOrEmpty(sp))
                {
                    t = CslHelper.getShiperName(sp);
                }

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

                Limpiar();


                /*carga listado de conceptos*/
                this.Carga_ListadoConceptos();
            }
        }


        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            try
            {

                string cMensaje = "";

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                int id_concept = int.Parse(this.LblID.Text);
                objConceptos = new concepts();
                objConceptos.id_concept = id_concept;
                objConceptos.description = this.TxtDescripcion.Text;
                objConceptos.Create_user = sUser.loginname;
                objConceptos.Mod_user = sUser.loginname;
                objConceptos.state = this.CboEstado.SelectedIndex == 0 ? true : false;
                objConceptos.Action = Session["Action"].ToString();

                objConceptos.Save(out cMensaje);
                if (cMensaje != string.Empty)
                {
                    this.MessageBox(cMensaje.ToString(), this);
                }
                else
                {
                    this.MessageBox("Se grabo concepto de cartera con éxito", this);
                    this.Limpiar();
                    this.Carga_ListadoConceptos();
                }




            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "conceptos", "BtnGrabar_Click", "Hubo un error al grabar concepto", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;

            }


        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();
            this.Carga_ListadoConceptos();
        }

        protected void Opciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                Label lbl = (Label)e.Item.FindControl("lbl_estado");
                if (lbl.Text == "INACTIVO")
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_id_concept")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_description")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_usuario_crea")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_usuario_mod")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_estado")).ForeColor = System.Drawing.Color.Red;
    
                }
            }
        }


        protected void Seleccionar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "Seleccionar_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "Seleccionar_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "Seleccionar_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    Int16 _Id_Line_Depo = Int16.Parse(xpars.ToString());
                    List<concepts> Lista = concepts.Get_Concepto(_Id_Line_Depo);
                    var xList = Lista.FirstOrDefault();
                    if (xList != null)
                    {
                        this.LblID.Text = xList.id_concept.ToString("D3");
                        this.TxtDescripcion.Text = xList.description;
                        this.CboEstado.SelectedIndex = xList.Indice;

                        Session["Action"] = "U";//SE VISUALIZA PARA ACTUALIZAR
                    }
                    else
                    {
                        Session["Action"] = "I";
                        this.MessageBox("No existe el concepto seleccionado, no podra realizar modificaciones..", this);
                        return;
                    }



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la selección de concepto, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "concepts", "Seleccionar_ItemCommand", "Hubo un error al seleccionar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }

        #endregion


    } 
}