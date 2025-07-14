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
    public partial class generar_proforma : System.Web.UI.Page
    {
        private Vessel_Visit objVesselV = new Vessel_Visit();
        
        private ProformaCab obProformaCab = new ProformaCab();

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
            

            TablePendientes.DataSource = null;
            TablePendientes.DataBind();

            TableProformas.DataSource = null;
            TableProformas.DataBind();

        }

        private void CargaProformasCab()
        {

            try
            {
                List<Vessel_Visit> Lista = Vessel_Visit.ListViesselVisit("C");

                if (Lista != null && Lista.Count > 0)
                {
                    TablePendientes.DataSource = Lista;
                    TablePendientes.DataBind();
                }
                else
                {
                    TablePendientes.DataSource = null;
                    TablePendientes.DataBind();
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
                Limpiar();
                CargaProformasCab();
              //  Limpiar();

            }
        }

        //metodo generalizado para controlar el error de esta clase.
        public static string ControlError(string mensaje)
        {
            //paselo a la pantalla una vez controlado y guardado.
            return " mensaje:" + mensaje + ", resultado:false ";
        }

       


     

        protected void AgregarProforma_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
     
        protected void GenerarProforma_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
   
                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "GenerarProforma_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "GenerarProforma_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandName == "RefButton")
                    {

                        var xpars = e.CommandArgument.ToString();
                        if (xpars.Length <= 0)
                        {

                            var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "GenerarProforma_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                            this.MessageBox(cMensaje2.ToString(), this);
                            return;
                        }

                        string cReferencia = xpars.ToString();

                        //detalle de proformas por referencia
                        List<ProformaCab> ListaProformaCab = ProformaCab.ListProformasCab(cReferencia, out v_mensaje);
                        if (ListaProformaCab != null && ListaProformaCab.Count > 0)
                        {
                            TableProformas.DataSource = ListaProformaCab;
                            TableProformas.DataBind();

                        }
                        else
                        {
                            TableProformas.DataSource = null;
                            TableProformas.DataBind();
                        }

                        if (v_mensaje != string.Empty)
                        {
                            this.MessageBox(v_mensaje.ToString(), this);
                            return;
                        }

                    }

                    if (e.CommandName == "GenButton")
                    {
                        var xpars = e.CommandArgument.ToString();
                        if (xpars.Length <= 0)
                        {

                            var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "GenerarProforma_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                            this.MessageBox(cMensaje2.ToString(), this);
                            return;
                        }

                        string cReferencia = xpars.ToString();
                        usuario sUser = null;
                        sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());


                        obProformaCab.Vessel_visit_reference = cReferencia;
                        obProformaCab.Create_user = sUser.loginname;

                        obProformaCab.SaveProforma(out v_mensaje);

                        if (v_mensaje != string.Empty)
                        {
                            this.MessageBox(v_mensaje.ToString(), this);
                        }
                        else
                        {
                            string cId = securetext(obProformaCab.Id.ToString());

                            this.MessageBox("Se genero la proforma con éxito", this);

                            CargaProformasCab();

                            //detalle de proformas por referencia
                            List<ProformaCab> ListaProformaCab = ProformaCab.ListProformasCab(cReferencia, out v_mensaje);
                            if (ListaProformaCab != null && ListaProformaCab.Count > 0)
                            {
                                TableProformas.DataSource = ListaProformaCab;
                                TableProformas.DataBind();

                            }
                            else
                            {
                                TableProformas.DataSource = null;
                                TableProformas.DataBind();
                            }

                                                      
                           // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('proforma_preview.aspx?sid=" + cId + "');", true);

                        }
                    }



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la aprobación de la transacción, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consultar", "Item_comand", "Hubo un error al aprobar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }

        }

      

   
    }
}