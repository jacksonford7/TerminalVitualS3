using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using ControlPagos.Importacion;
using SqlConexion;


namespace CSLSite.pagosterceros
{


    public partial class asumirpagocontenedor : System.Web.UI.Page
    {

        #region "Clases"


        usuario ClsUsuario;

        #endregion

        #region "Variables"

        private static Int64? lm = -3;
        private string OError;

        private string numero_carga = string.Empty;
        //private string cMensajes;

        //private Int64 Gkey = 0;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        //private int Fila = 1;
        private string TipoServicio = string.Empty;
        //private DateTime FechaFactura;
        //private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
        

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        //private bool sap_valida = false;
        //private bool tieneBloqueo = false;
        //private decimal SaldoPendiente = 0;
        //private decimal ValorVencido = 0;
        //private decimal ValorPendiente = 0;
        //private Int64 DiasCredito = 0;

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

        private void Listado_Cargas()
        {
            try
            {
            
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Resultado = PagoAsignado.ListaAsignacion(ClsUsuario.loginname.Trim(), ClsUsuario.ruc.Trim(), null,null);
           
                if (Resultado != null)
                {

                    if (Resultado.Exitoso)
                    {
                        var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.mrn))
                                        select new
                                        {
                                            id_asignacion = Tbl.id_asignacion,
                                            carga = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                            ruc = Tbl.ruc.Trim(),
                                            nombre = Tbl.nombre.Trim(),
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
                else {
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
                        var Resultado = pago.BorrarAsignacion();
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
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
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

                this.TXTMRN.Text = string.Empty;
                this.TXTMSN.Text = string.Empty;
                if (string.IsNullOrEmpty(this.TXTHSN.Text))
                { this.TXTHSN.Text = string.Format("{0}", "0000"); }

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());

                if (!Page.IsPostBack)
                {

                    this.Listado_Cargas();
             
                }

            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

 
        protected void BtnAsumir_Click(object sender, EventArgs e)
        {
            
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

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MSN"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga HSN"));
                        this.TXTHSN.Focus();
                        return;
                    }




                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    PagoAsignado pago = new PagoAsignado(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc, ClsUsuario.loginname);
                    var Resultado = pago.NuevaAsignacion();
                    if (Resultado.Exitoso)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-info'></i><b> Informativo! </b>Asignación realizada con éxito...{0}", Resultado.MensajeInformacion));
                        this.Listado_Cargas();
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} , {1}",  Resultado.MensajeProblema, Resultado.QueHacer));
                        return;
                    }

                    this.Ocultar_Mensaje();
                 

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAsumir_Click), "PagoAsignado",false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));   
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }



            
        }

      
      
        


    }
}