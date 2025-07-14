using ControlOPC.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.opc
{
    public partial class inactivar_trabajos : System.Web.UI.Page
    {

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        private Vessel_Visit objVesselV = new Vessel_Visit();

        #region "Propiedades"

        public static string v_mensaje = string.Empty;

        private DataTable pDetalleVesselVisit
        {
            get
            {
                return (DataTable)Session["DtDetalleVesselVisit"];
            }
            set
            {
                Session["DtDetalleVesselVisit"] = value;
            }
        }

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

        private void CargaVesselVisit()
        {

            try
            {
                List<Vessel_Visit> Lista = Vessel_Visit.ListViesselVisitTotal();

                if (Lista != null && Lista.Count > 0)
                {
                    TableTurnos.DataSource = Lista;
                    TableTurnos.DataBind();
                }
                else
                {
                    TableTurnos.DataSource = null;
                    TableTurnos.DataBind();
                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
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
            this.IsAllowAccess();

            var user = Page.Tracker();
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
                CargaVesselVisit();
            }
        }

        //metodo generalizado para controlar el error de esta clase.
        public static string ControlError(string mensaje)
        {
            //paselo a la pantalla una vez controlado y guardado.
            return " mensaje:" + mensaje + ", resultado:false ";
        }

       

        protected string url(object _referece)
        {
            return string.Format("<a href='transaccionopc.aspx?ID={0}' target='_blank'>Visualizar</a>", _referece);
        }

        protected void AnularTrabajo_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    /* if (HttpContext.Current.Request.Cookies["token"] == null)
                     {
                         System.Web.Security.FormsAuthentication.SignOut();
                         Session.Clear();
                         System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                         return;
                     }*/

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "AnularTrabajo_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "AnularTrabajo_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }
                    //
                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "AnularTrabajo_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    int idTransaccion = int.Parse(xpars.ToString());

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());


                    var v_mensaje = Vessel_Visit.Active(idTransaccion, sUser.loginname);
                    if (v_mensaje != string.Empty)
                    {
                        this.MessageBox(v_mensaje.ToString(), this);
                        return;
                    }
                    else
                    {
                        this.MessageBox("Anulación realizada con éxito", this);
                      
                    }

                    CargaVesselVisit();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la anulación de la transacción, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "anular", "Item_comand", "Hubo un error al anular trabajos", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }
    }
}