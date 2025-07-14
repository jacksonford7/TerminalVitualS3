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
    public partial class anulacion_proformas : System.Web.UI.Page
    {


        #region "Propiedades"

        private ProformaCab obProformaCab = new ProformaCab();
        private ProformaDet obProformaDet = new ProformaDet();
        private Vessel_Visit objVesselV = new Vessel_Visit();


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

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

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
            //Caja de Texto
            TxtReferencia.Text = null;
         
            TablePendientes.DataSource = null;
            TablePendientes.DataBind();
            UpdatePanel1.Update();



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
                Limpiar();
       
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
            return string.Format("<a href='transaccionopc.aspx?ID={0}' target='_blank'>Imprimir</a>", _referece);
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //bool lTieneFecha = true;
                string cMensaje = "";

                if (String.IsNullOrEmpty(this.TxtReferencia.Text) != false)
                {
                   
                    ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('Ingrese la referencia del buque a buscar')", true);
                    return;
                }
       
                    //detalle de proformas por referencia
                    Int32 nTipo = 0;
                    if (this.RdbProceso.Checked) { nTipo = 1; }
                    if (this.RdbAdicionales.Checked) { nTipo = 0; }

                    List<ProformaCab> ListaProformaCab = ProformaCab.ListProformasCabFiltros(this.TxtReferencia.Text.Trim(), nTipo, out cMensaje);
                    if (ListaProformaCab != null && ListaProformaCab.Count > 0)
                    {
                        TablePendientes.DataSource = ListaProformaCab;
                        TablePendientes.DataBind();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('No existe información con los criterios de búsqueda')", true);
                        TablePendientes.DataSource = null;
                        TablePendientes.DataBind();
                    }
                UpdatePanel1.Update();

            }
            catch (Exception exc)
            {
                string mensaje = @"alert('" + exc.Message.ToString() + "')";

                ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", mensaje, true);
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {


                if (String.IsNullOrEmpty(this.TxtReferencia.Text) != false)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('Ingrese la referencia del buque a buscar')", true);
                    return;
                }

                if (TablePendientes.Items.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('No existen proformas para anular')", true);
                    return;
                }

                obProformaCab = new ProformaCab();
                obProformaCab.Vessel_visit_reference = this.TxtReferencia.Text.Trim();
                if (this.RdbProceso.Checked) {
                    obProformaCab.Active = this.RdbProceso.Checked;
                }
                if (this.RdbAdicionales.Checked)
                {
                    obProformaCab.Active = false;
                }

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                obProformaCab.Mod_user = sUser.loginname;

                obProformaCab.AnularProforma(out v_mensaje);

                if (v_mensaje != string.Empty)
                {
                    this.MessageBox(v_mensaje.ToString(), this);
                }
                else
                {
                   
                    this.MessageBox("Se genero la anulación con éxito", this);
                    BtnBuscar_Click(sender, e);


                }

               

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }

        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        

      
    }
}