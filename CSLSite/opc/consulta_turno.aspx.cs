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
    public partial class consulta_turno : System.Web.UI.Page
    {
        private Vessel_Visit objVesselV = new Vessel_Visit();
        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

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
                List<Vessel_Visit> Lista = Vessel_Visit.ListViesselVisit("W");

                if (Lista != null && Lista.Count > 0)
                {
                    TableTurnos.DataSource = Lista;
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
                // this.textbox1.Value = user.email != null ? user.email : string.Empty;
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
                //this.nomexpo.InnerText = t != null ? t : string.Format("{0} {1}", user.nombres, user.apellidos);
                //this.numexpo.InnerText = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                //this.numexport.Value = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
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

        protected void BtnActualizar_Click(object sender, EventArgs e)
        {
                CargaVesselVisit();
       
        }

  
     
    }
}