using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using System.Web.Script.Services;
using System.Configuration;
using BillionEntidades;
using N4Ws.Entidad;
using PasePuerta;

namespace CSLSite
{
    public partial class registro_inspeccion : System.Web.UI.Page
    {
        private Cls_Inspeccion_Buques obj = new Cls_Inspeccion_Buques();
        private Cls_Valida_Servicio_Naves ObjValida = new Cls_Valida_Servicio_Naves();
        private string OError;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "registro_inspeccion", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
       
            var user = Page.Tracker();

            if (user != null /*&& !string.IsNullOrEmpty(user.nombregrupo)*/)
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "registro_inspeccion", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                //this.agencia.Value = user.ruc;
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
               sinresultado.Visible = false;
                alerta.Visible = false;
            }
           
        }

        private void LImpiar()
        {
            this.tbooking.Text = string.Empty;
            this.tfechaini.Text = string.Empty;
            this.TxtRuta1.Text = string.Empty;
            this.ruta_completa.Value = string.Empty;
            this.TxtObservacion.Text = string.Empty;
            this.id_linea.Value = string.Empty;
        }

        private void JsLimpiar()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "limpiar('');", true);
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {

                this.LImpiar();


            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "registro_inspeccion", "BtnNuevo_Click", "Hubo un error al limpiar", t.loginname));
                sinresultado.Visible = true;
            }

        }

        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime Fecha_Desde;

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;

                    }
                    sinresultado.Visible = false;

                    if (string.IsNullOrEmpty(this.tbooking.Text))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor debe seleccionar la Nave";
                        this.tbooking.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.tfechaini.Text))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor debe ingresar la fecha de la inspección";
                        this.tfechaini.Focus();
                        return;
                    }

                    if (!DateTime.TryParseExact(this.tfechaini.Text.Trim(), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out Fecha_Desde))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor debe ingresar una fecha valida, para la inspección";
                        this.tfechaini.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtRuta1.Text))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor debe cargar un archivo pdf de la inspección";
                        this.TxtRuta1.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.ruta_completa.Value))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor debe cargar un archivo pdf de la inspección";
                        this.TxtRuta1.Focus();
                        return;
                    }

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //valido si ya tiene el servicio tomado

                    /*saco el invoice type*/
                    string pInvoiceType = string.Empty;
                    int existe = 0;

                    var InvoiceType = InvoiceTypeConfig.ObtenerInvoicetypes();
                    if (InvoiceType.Exitoso)
                    {
                        var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("SUB_INSPEC"))
                                               select new { valor = p.valor }).FirstOrDefault();

                        pInvoiceType = LinqInvoiceType.valor == null ? "SERVICIO_SUB_INSPEC " : LinqInvoiceType.valor;
                    }
                    /*fin invoice type*/

                    ObjValida = new Cls_Valida_Servicio_Naves();
                    ObjValida.NAVE = this.tbooking.Text;
                    ObjValida.EVENTO = pInvoiceType;
                    if (ObjValida.PopulateMyData(out OError))
                    {
                        existe = ObjValida.resultado;
                    }
                    else
                    {
                        existe = 0;
                        alerta.Visible = true;
                        alerta.InnerText = OError;
                        return;
                    }

                    bool Carga_Servicio = false;

                    //carga servicio
                    if (existe == 0)
                    {
                        var ResultadoEvt = Servicio_Inspeccion.Marcar_Servicio(this.tbooking.Text, pInvoiceType, ClsUsuario.loginname.Trim(), 1);
                        if (ResultadoEvt.Exitoso)
                        {
                            Carga_Servicio = true;
                        }
                        else
                        {
                            Carga_Servicio = false;
                            alerta.Visible = true;
                            alerta.InnerText = string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar servicios de Inspección Subacuática: {0}, Existen los siguientes problemas: {1} </b>", ResultadoEvt.MensajeInformacion, ResultadoEvt.MensajeProblema);
                            return;

                        }
                    }
                    else
                    {
                        Carga_Servicio = true;
                    }
                    //fin validacion
                    if (Carga_Servicio)
                    {
                        obj = new Cls_Inspeccion_Buques();
                        obj.NAVE = this.tbooking.Text;
                        obj.DESC_NAVE = this.txtNaveDescrip.Text.Trim();
                        obj.REFERENCIA = this.TxtReferencia.Text.Trim();
                        obj.LINEA = this.TxtLineaNaviera.Text.Trim();
                        obj.ID_LINEA = this.id_linea.Value;
                        obj.BANDERA = TxtBandera.Text.Trim();
                        obj.FECHA_INSPE = Fecha_Desde;
                        obj.OBSERVACION = this.TxtObservacion.Text.Trim();

                        obj.USUARIO_CREA = ClsUsuario.loginname.Trim();
                        obj.RUC_USUARIO = ClsUsuario.ruc;
                        obj.RUTA_PDF = this.ruta_completa.Value;


                        var nIdRegistro = obj.Save(out OError);

                        if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                        {
                            alerta.Visible = true;
                            alerta.InnerText = string.Format("Error! No se pudo registrar inspección..{0}", OError);
                            return;
                        }
                        else
                        {
                            this.LImpiar();
                            alerta.Visible = true;
                            alerta.InnerText = string.Format("Se procedió a registrar la inspección # {0} ", nIdRegistro.Value.ToString("D8"));

                            JsLimpiar();
                        }
                    }
                   



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "registro_inspeccion", "btbuscar_Click", "Hubo un error al registrar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
     
    }
}