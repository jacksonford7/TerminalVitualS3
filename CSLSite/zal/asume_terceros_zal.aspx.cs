using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using ControlPagos.Importacion;
using SqlConexion;


namespace CSLSite.zal
{
    public partial class asume_terceros_zal : System.Web.UI.Page
    {
        #region "Clases"


        usuario ClsUsuario;

        #endregion

        #region "Variables"

        private static Int64? lm = -3;
        private string OError;

        private string numero_carga = string.Empty;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        private string TipoServicio = string.Empty;
        private string LoginName = string.Empty;

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;

        #endregion

        #region "Propiedades"

        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }
        }

        #endregion

        #region "Metodos"

        public void LlenaComboDepositos()
        {
            try
            {
                cmbDeposito.DataSource = man_pro_expo.consultaDepositos(); //ds.Tables[0].DefaultView;
                cmbDeposito.DataValueField = "ID";
                cmbDeposito.DataTextField = "DESCRIPCION";
                cmbDeposito.DataBind();
                cmbDeposito.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAsumir_Click), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void Listado_Cargas()
        {
            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Resultado = PagoAsignado.ListaAsignacionZAL(ClsUsuario.loginname.Trim(), ClsUsuario.ruc.Trim(), null, null);

                if (Resultado != null)
                {
                    if (Resultado.Exitoso)
                    {
                        var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.booking))
                                        select new
                                        {
                                            id_asignacion = Tbl.id_asignacion,
                                            carga = Tbl.booking,
                                            ruc = Tbl.ruc.Trim(),
                                            nombre = Tbl.nombre.Trim(),
                                            ruc_asumido = Tbl.ruc_asumido.Trim(),
                                            nombre_asumido = Tbl.nombre_asumido.Trim(),
                                            fecha_asignado = Tbl.fecha_asignado.Value.ToString("dd/MM/yyyy HH:mm"),
                                            login_asigna = Tbl.login_asigna
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            tablePagination.DataSource = LinqQuery;
                            tablePagination.DataBind();
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                        }
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                    }
                    this.Actualiza_Paneles();
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Cargas), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "Remover")
                    {
                        PagoAsignado pago = new PagoAsignado(id, ClsUsuario.loginname.Trim());
                        var Resultado = pago.BorrarAsignacionZAL();
                        if (Resultado.Exitoso)
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-info'></i><b> Informativo! </b>Eliminación realizada con éxito...{0}", Resultado.MensajeInformacion));
                            this.Listado_Cargas();
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} , {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "BorrarAsignacion", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }

        private void Actualiza_Paneles()
        {
            this.UPCARGA.Update();
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

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
        }
        #endregion

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

            this.banmsg.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                }

                nbrboo.Value =   string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode(nbrboo.Value.Trim());
                Server.HtmlEncode(this.TXTCLIENTE_RUC.Text.Trim());
                Server.HtmlEncode(this.TXTCLIENTE_ASUMIDO.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Listado_Cargas();
                    LlenaComboDepositos();
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

        protected void cmbDeposito_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                if (cmbDeposito.SelectedValue == "-1")
                {
                    this.Alerta("Seleccione un depósito.");
                    cmbDeposito.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                    return;
                }

                if (IdDepot == 1)//ZAL
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

                if (IdDepot == 2)//OPACIFIC
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPOPA"];
                }
                //06-abr-2020
                if (IdDepot == 3)//ZAL
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }
                if (IdDepot == 4)//ZAL
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }
                if (IdDepot == 5)//SAV
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

                //this.limpiar();
            }
            catch
            {

            }
        }

        protected void BtnAsumir_Click(object sender, EventArgs e)
        {
            nbrboo.Value = nbrboo.Value.ToUpper();
            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (cmbDeposito.SelectedValue == "-1")
                    {
                        this.Alerta("Seleccione un depósito.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un depósito."));
                        cmbDeposito.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(nbrboo.Value))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el Booking"));
                        nbrboo.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTCLIENTE_RUC.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de RUC del cliente que se asumirá el pago"));
                        this.TXTCLIENTE_RUC.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTCLIENTE_ASUMIDO.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor verificar el cliente."));
                        this.TXTCLIENTE_RUC.Focus();
                        return;
                    }

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    PagoAsignado pago = new PagoAsignado(long.Parse(cmbDeposito.SelectedValue), nbrboo.Value.ToString().Trim(), this.TXTCLIENTE_RUC.Text, TXTCLIENTE_ASUMIDO.Text, ClsUsuario.ruc,ClsUsuario.nombres, ClsUsuario.loginname);
                    var Resultado = pago.NuevaAsignacionZAL();

                    TXTCLIENTE_RUC.Text = string.Empty;
                    TXTCLIENTE_ASUMIDO.Text = string.Empty;
                    nbrboo.Value = string.Empty;

                    if (Resultado.Exitoso)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-info'></i><b> Informativo! </b>Asignación realizada con éxito...{0}", Resultado.MensajeInformacion));
                        this.Listado_Cargas();
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} , {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                        return;
                    }

                    this.Ocultar_Mensaje();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAsumir_Click), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var c = N4.Entidades.Cliente.ObtenerClienteSAV(Page.User.Identity.Name, TXTCLIENTE_RUC.Text);// man_pro_expo.get//(TXTCLIENTE_ASUMIDO.Text.Trim());
                if (c.Resultado == null)
                {
                    this.Alerta("No se encontraron datos con el RUC: " + TXTCLIENTE_ASUMIDO.Text.Trim());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    TXTCLIENTE_ASUMIDO.Focus();
                    return;
                }
                TXTCLIENTE_ASUMIDO.Text = c.Resultado.CLNT_NAME;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAsumir_Click), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void TXTCLIENTE_RUC_TextChanged(object sender, EventArgs e)
        {
            this.TXTCLIENTE_ASUMIDO.Text = String.Empty;
        }
    }
}