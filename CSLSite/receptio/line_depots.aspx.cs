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
using ControlOPC.Entidades;
using System.Data;
using System.Globalization;
using ReceptioMtyStock;

namespace CSLSite
{
    public partial class line_depots : System.Web.UI.Page
    {
 
        private Line_Depot objLineDepot = new Line_Depot();
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
           

            CboDeposito.DataSource = null;
            CboDeposito.DataBind();

            TableBodegas.DataSource = null;
            TableBodegas.DataBind();
   
        }

        private void CargaDepositos()
        {
            try
            {
                List<Depot> Lista = Depot.ListDepot();
            
                if (Lista != null && Lista.Count > 0)  
                {
                    this.CboDeposito.DataSource = Lista;
                    CboDeposito.DataBind();
                }
                else
                {
                    CboDeposito.DataSource = null;
                    CboDeposito.DataBind();
                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        private void CargaLineaNaviera(string codigoempresa)
        {
            try
            {
                List<Line> Lista = Line.ListLine(codigoempresa);
                var xList = Lista.FirstOrDefault();

                if (xList != null)
                {
                    this.LblNombre.Text = xList.name;
                    Session["id_line"] = xList.id_line;
                }
                else
                {
                    Session["id_line"] = "0";
                    this.LblNombre.Text = null;

                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        private void CargaLineDepots()
        {
            try
            {

                    List<Line_Depot> ListLineDepots = Line_Depot.ListLineDepot(Convert.ToInt32(Session["id_line"]));
                    if (ListLineDepots != null)
                    {
                        TableBodegas.DataSource = ListLineDepots;
                        TableBodegas.DataBind();
                    }
                    else {
                        TableBodegas.DataSource = null;
                        TableBodegas.DataBind();
                    }
                


            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "line_depots", "CargaLineaNaviera", "Hubo un error al cargar depositos por Línea de Naviera", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        protected void RemoverBodegas_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "RemoverGruas_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "RemoverBodegas_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }
                    //
                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "RemoverBodegas_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    Int64  _Id_Line_Depo = int.Parse(xpars.ToString());

                  
                    Label lbl = (Label)e.Item.FindControl("lblBodega");


                    objLineDepot = new Line_Depot();
                    objLineDepot.Id_line_depo = _Id_Line_Depo;
                    objLineDepot.Mod_user = user.loginname;
                    objLineDepot.active = false;
                    objLineDepot.Id_depot = Convert.ToInt32(lbl.Text);
                    objLineDepot.Id_line = (int)Session["id_line"];

                    if (objLineDepot.ExistMovStock(out sg))
                    {
                        if (objLineDepot.Stock != 0){
                            this.MessageBox("No podrá remover una bodega con saldos, para poder realizar esta acción la bodega debe tener saldo: 0", this);
                            return;
                        }
                       
                    }
                    else
                    {
                        if (sg != string.Empty)
                        {
                            this.MessageBox(sg, this);
                            return;
                        }

                    }


                    objLineDepot.Delete(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.MessageBox(v_mensaje.ToString(), this);
                    }
                    else
                    {
                        this.MessageBox("Se elimino bodega con éxito", this);
                        /*carga depositos por naviera*/
                        this.CargaLineDepots();
                    }

                   

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de grúa, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al eliminar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }

   
        #endregion


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

            //this.IsAllowAccess();

             user = Page.Tracker();
            if (user != null)
            {
               // this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
                /*datos de la linea naviera*/
                this.CargaLineaNaviera(user.codigoempresa);
                if (this.LblNombre.Text == String.Empty) {  
                    this.AbortResponse("Su usuario no tiene una línea de naviera asignada", "../cuenta/menu.aspx", true);
                    return;
                }

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

                /*carga todas las bodegas*/
                CargaDepositos();

                /*carga depositos por naviera*/
                this.CargaLineDepots();
            }
        }


        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            try
            {

                string cMensaje = "";

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());


                if (String.IsNullOrEmpty(this.LblNombre.Text) != false)
                {
                    this.MessageBox("No tiene una línea  de naviera asignada", this);
                    return;
                }

                if (this.CboDeposito.SelectedValue == string.Empty)
                {
                    this.MessageBox("Por favor debe seleccionar la bodega", this);
                    return;
                }

                if ((int)Session["id_line"] == 0)
                {
                    this.MessageBox("Error, no existe una línea naviera para el usuario" + sUser.nombres, this);
                    return;
                }
             
                objLineDepot = new Line_Depot();
                objLineDepot.Id_line = (int)Session["id_line"];
                objLineDepot.Id_depot = Convert.ToInt32(this.CboDeposito.SelectedValue);
                objLineDepot.Create_user = sUser.loginname;
                objLineDepot.active = true;

                if (objLineDepot.ExistLineDepot(out cMensaje))
                {
                    this.MessageBox("El deposito seleccionado ya se encuentra agregado: " + this.CboDeposito.SelectedItem.ToString(), this);
                    return;
                }
                else {
                    if (cMensaje != string.Empty) {
                        this.MessageBox(cMensaje, this);
                        return;
                    }

                }

                objLineDepot.Save(out cMensaje);
                if (cMensaje != string.Empty)
                {
                    this.MessageBox(cMensaje.ToString(), this);
                }  
                else
                {
                    this.MessageBox("Se agrego bodega con éxito", this);
                    this.Limpiar();
                    /*carga depositos por naviera*/
                    this.CargaLineDepots();
                }

          
            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "line_depots", "BtnAgregar_Click", "Hubo un error al agregar bodega", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
              
            }

           
        }

      
    }
}