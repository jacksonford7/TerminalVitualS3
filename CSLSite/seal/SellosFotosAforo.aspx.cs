using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConectorN4;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Xml.Linq;
using ReceptioMtyStock;
using CSLSite.app_start;

namespace CSLSite
{

    public partial class SellosFotosAforo : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        #endregion

        #region "Variables"
        private string cMensajes;
        private static Int64? lm = -3;
        private string OError;
        #endregion

        #region "Propiedades"
        private Int64? nSesion { get { return (Int64)Session["nSesion"]; } set { Session["nSesion"] = value; } }
        #endregion

        #region "Metodos"
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
            this.txtContenedor.Text = string.Empty;
            this.dtpFechadesde.Text = string.Empty;
            this.txtFechaHasta.Text = string.Empty;
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
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }
        private void Mostrar_MensajeDet(string Mensaje)
        {
            OcultarLoading("1");
            OcultarLoading("2");

            UPMODAL.Update();
        }
        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

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
        
        private void ConsultarData(DateTime? _desde, DateTime? _hasta, string _container)
        {
            try
            {
                var oDet = SelloAforo.ListSellos(_desde, _hasta, _container);
                //var oDet = Sellos.ListSellos(_desde, _hasta, _container); //tarjaDet.GetTarjaDet( _mrn,  _msn,  _hsn);

                if (oDet == null)
                {
                    this.Alerta("Búsqueda sin resultados, verifique los filtros de consulta");
                    //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Búsqueda sin resultados, verifique el No. Carga"));
                    sinresultado.Visible = true;
                    this.txtContenedor.Focus();
                    return;
                }
                else
                {
                    if (oDet.Count > 0)
                    {
                        tablePagination.DataSource = oDet;
                        tablePagination.DataBind();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        sinresultado.Visible = true;
                    }
                }
               
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarData), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        #endregion

        #region "Forma"
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }
#if !DEBUG
                this.IsAllowAccess();
#endif
                this.banmsg.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    ClsUsuario = Page.Tracker();
                    if (ClsUsuario != null)
                    {
                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                    }

                    this.txtContenedor.Text = string.Empty;
                    this.dtpFechadesde.Text = string.Empty;
                    this.txtFechaHasta.Text = string.Empty;
                }

                ClsUsuario = Page.Tracker();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode(this.txtContenedor.Text.Trim());
                Server.HtmlEncode(this.dtpFechadesde.Text.Trim());
                Server.HtmlEncode(this.txtFechaHasta.Text.Trim());

                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    sinresultado.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "Eventos"
        #region "Gridview Cabecera"

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

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "FotosEmbarque")
                    {
                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        Ocultar_Mensaje();

                        int v_contador = 0;

                        //se obtiene la lista de imagenes de la recepción seleccionada
                        var eSello = SelloAforo.Sello(v_ID, out OError);

                        if (OError != string.Empty)
                        {
                            sinresultadoFotos.Visible = true;
                            this.htmlImagenes.InnerHtml = "";
                            UPMODAL.Update();
                            this.Alerta(OError);
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                            this.txtContenedor.Focus();
                            return;
                        }

                        if (eSello == null)
                        {
                            sinresultadoFotos.Visible = true;
                            this.htmlImagenes.InnerHtml = "";
                            UPMODAL.Update();
                            return;
                        }

                        //DataTable TablaImagenes = new DataTable();
                        //TablaImagenes.Columns.Add("ruta");

                        //TablaImagenes.Rows.Add(new Object[] { eSello.PHOTO1 });
                        //TablaImagenes.Rows.Add(new Object[] { eSello.PHOTO2 });
                        //TablaImagenes.Rows.Add(new Object[] { eSello.PHOTO3 });
                        //TablaImagenes.Rows.Add(new Object[] { eSello.PHOTO4 });

                        var oImagenes = eSello.fotos;// SelloPreEmbarqueFotoPAN.ListFotosSellos(v_ID);

                        string v_divImagenes = string.Empty;

                        foreach(SelloAforoFoto oFoto in oImagenes)//for (int i = 0; i < 4; i++)
                        {
                            if (string.IsNullOrEmpty(oFoto.ruta))//(TablaImagenes.Rows[i].ItemArray[0].ToString()))
                            {
                                continue;
                            }

                            if (v_contador == 0)
                            {
                                v_contador = 1;
                                v_divImagenes += @"<div class='carousel-item active'>
                                                  <img src = '" + oFoto.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                <div class='carousel-caption d-none d-md-block'>
                                                    <!-- <h5>Second slide label</h5>
                                                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                </div>
                                            </div> ";
                            }
                            else
                            {
                                v_divImagenes += @"<div class='carousel-item'>
                                                <img src = '" + oFoto.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                <div class='carousel-caption d-none d-md-block'>
                                                    <!-- <h5>Second slide label</h5>
                                                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                </div>
                                            </div> ";
                            }
                        }

                        StringBuilder tab = new StringBuilder();

                        string v_cuerpo = string.Empty;
                        v_cuerpo = v_cuerpo + @"<div class='mb-5'>
                                        <div id='carouselExampleCaptions' class='carousel slide' data-ride='carousel'>
                                        <ol class='carousel-indicators'>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='0' class='active'></li>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='1'></li>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='2'></li>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='3'></li>
                                        </ol>
                                        <div class='carousel-inner'>
                                           
                                            " + v_divImagenes + @"
                                        </div>
                                        <a class='carousel-control-prev' href='#carouselExampleCaptions' role='button' data-slide='prev'>
                                            <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                                            <span class='sr-only'>Previous</span>
                                        </a>
                                        <a class='carousel-control-next' href='#carouselExampleCaptions' role='button' data-slide='next'>
                                            <span class='carousel-control-next-icon' aria-hidden='true'></span>
                                            <span class='sr-only'>Next</span>
                                        </a>
                                    </div>
                                </div>
                                ";

                        string v_html = string.Empty;
                        v_html = v_html + v_cuerpo;

                        tab.Append(v_html);
                        this.htmlImagenes.InnerHtml = tab.ToString();
                        xfinde2.Visible = true;
                        sinresultadoFotos.Visible = false;
                        UPMODAL.Update();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "SellosFotos.tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
                
            }
        }
        
        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpia_Datos_cliente();
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            sinresultado.Visible = false;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
            if (Response.IsClientConnected)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.txtContenedor.Text) && (string.IsNullOrEmpty(this.dtpFechadesde.Text) || string.IsNullOrEmpty(this.txtFechaHasta.Text)))
                    {
                        this.Alerta("Por favor ingresar el rango de fechas o el contenedor");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar un rango de fechas y el numero del contenedor"));
                        this.txtContenedor.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(dtpFechadesde.Text) && string.IsNullOrEmpty(txtFechaHasta.Text))
                    {
                        ConsultarData(null, null, txtContenedor.Text);
                        return;
                    }

                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechaini;
                    
                    if (!DateTime.TryParseExact(dtpFechadesde.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaini))
                    {
                        this.Response.ClearContent();
                        return;
                    }
                    DateTime fechafin;
                    if (!DateTime.TryParseExact(txtFechaHasta.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechafin))
                    {
                        this.Response.ClearContent();
                        return;
                    }

                    ConsultarData(fechaini, fechafin, string.IsNullOrEmpty(txtContenedor.Text)? null: txtContenedor.Text);
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }
       
        #endregion
    }
}