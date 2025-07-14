using BillionEntidades;
using BreakBulk;
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
using CSLSite.WebRef_IngresoIEE;
using System.Xml.Linq;

namespace CSLSite.brbk
{

    public partial class brbkRecepciones : System.Web.UI.Page
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
            this.TXTMRN.Text = string.Empty;
            this.txtMSN.Text = string.Empty;
            this.txtHSN.Text = string.Empty;
            this.txtConsignatario.Text = string.Empty;
            this.txtCantidad.Text = string.Empty;
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
        
        private void ConsultarDataRecepcion(string _mrn, string _msn, string _hsn)
        {
            try
            {
                var oDet = tarjaDet.GetTarjaDet( _mrn,  _msn,  _hsn);

                if (oDet == null)
                {
                    this.Alerta("Búsqueda sin resultados, verifique el No. Carga");
                    //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Búsqueda sin resultados, verifique el No. Carga"));
                    sinresultado.Visible = true;
                    this.TXTMRN.Focus();
                    return;
                }

                var Resultado = recepcion.listadoRecepcion(long.Parse(oDet.idTarjaDet.ToString()), out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                    return;
                }

                if (Resultado != null)
                {
                    if (Resultado.Count > 0)
                    {

                        txtConsignatario.Text = oDet.consigna;
                        txtCantidad.Text = oDet.cantidad.ToString();
                        tablePagination.DataSource = Resultado;
                        tablePagination.DataBind();
                        Actualiza_Paneles();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        sinresultado.Visible = true;
                    }
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                    sinresultado.Visible = true;
                }
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataRecepcion), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        private void ConsultarRecepcion(long _idTarjaDet)
        {
            try
            {
                var Resultado = recepcion.listadoRecepcion(_idTarjaDet, out OError);//(txtNave.Text, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                }

                if (Resultado != null)
                {
                    if (Resultado.Count > 0)
                    {
                        var oDet = tarjaDet.GetTarjaDet(Resultado.FirstOrDefault().idTarjaDet);

                        foreach (var a in Resultado)
                        {
                            a.Grupo = grupos.GetGrupos(a.idGrupo);
                            a.Estados = estados.GetEstado(a.estado);
                            a.Ubicaciones = ubicacion.GetUbicacion(a.ubicacion);
                            a.TarjaDet = oDet;
                        }

                        var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.estado))
                                        select new
                                        {
                                            idRecepcion = Tbl.idRecepcion,
                                            idTarjaDet = Tbl.idTarjaDet.ToString(),
                                            consignatario = Tbl.TarjaDet.Consignatario.Trim(),
                                            producto = Tbl.TarjaDet.producto?.nombre.Trim(),
                                            grupo = Tbl.Grupo?.nombre.Trim(),
                                            lugar = Tbl.lugar.Trim(),
                                            cantidades = Tbl.cantidad,
                                            ubicacion = Tbl.Ubicaciones?.nombre.Trim() == null ? string.Empty : Tbl.Ubicaciones?.nombre.Trim(),
                                            observaciones = Tbl.observacion,
                                            estados = Tbl.Estados?.nombre,
                                            usuarioCrea = Tbl.usuarioCrea.Trim(),
                                            fechaCreacion = Tbl.fechaCreacion.Value.ToString("dd/MM/yyyy HH:mm"),
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            //dgvRecepcion.DataSource = LinqQuery;
                            //dgvRecepcion.DataBind();
                        }
                    }
                    else
                    {
                        //dgvRecepcion.DataSource = null;
                        //dgvRecepcion.DataBind();
                    }
                }
                else
                {
                    //dgvRecepcion.DataSource = null;
                    //dgvRecepcion.DataBind();
                }

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarRecepcion), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        protected string jsarguments(object ID_CAB, object ID_DET)
        {
            return string.Format("{0};{1}", ID_CAB != null ? ID_CAB.ToString().Trim() : "0", ID_DET != null ? ID_DET.ToString().Trim() : "0");
        }
       
        private string ConsultarFotosNovedad(long idNovedad)
        {
            int v_contador = 0;
            //se obtiene la lista de imagenes de la recepción seleccionada
            var TablaImagenes = fotoNovedad.listadoFotosNovedad(idNovedad, out OError);

            if (OError != string.Empty)
            {
                sinresultadoNovedad.Visible = true;
                this.Alerta(OError);
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                this.TXTMRN.Focus();
                return string.Empty;
            }

            if (TablaImagenes == null)
            {
                sinresultadoFotos.Visible = true;
                return string.Empty;
            }

            if (TablaImagenes.Count == 0)
            {
                sinresultadoFotos.Visible = true;
                return string.Empty;
            }

            string v_divImagenes = string.Empty;
            foreach (var Det in TablaImagenes)
            {
                if (v_contador == 0)
                {
                    v_contador = 1;
                    v_divImagenes += @"<div class='carousel-item active'>
                                            <img src = '" + Det.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                            <div class='carousel-caption d-none d-md-block'>
                                                <!-- <h5>Second slide label</h5>
                                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                            </div>
                                        </div> ";
                }
                else
                {
                    v_divImagenes += @"<div class='carousel-item'>
                                            <img src = '" + Det.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
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
                                    <div id='carouselExampleCaptions{X1}' class='carousel slide' data-ride='carousel'>
                                    <ol class='carousel-indicators'>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='0' class='active'></li>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='1'></li>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='2'></li>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='3'></li>
                                    </ol>
                                    <div class='carousel-inner'>
                                           
                                        " + v_divImagenes + @"
                                    </div>
                                    <a class='carousel-control-prev' href='#carouselExampleCaptions{X1}' role='button' data-slide='prev'>
                                        <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Previous</span>
                                    </a>
                                    <a class='carousel-control-next' href='#carouselExampleCaptions{X1}' role='button' data-slide='next'>
                                        <span class='carousel-control-next-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Next</span>
                                    </a>
                                </div>
                            </div>
                            ";
            v_cuerpo = v_cuerpo.Replace("{X1}", idNovedad.ToString());

            string v_html = string.Empty;
            v_html = v_html + v_cuerpo;

            tab.Append(v_html);
            return tab.ToString();
        }

        private string ConsultarFotosDespacho(long idDespacho)
        {
            int v_contador = 0;
            //se obtiene la lista de imagenes de la recepción seleccionada
            var TablaImagenes = fotoDespacho.listadoFotosDespacho(idDespacho, out OError);

            if (OError != string.Empty)
            {
                //sinresultadoDespacho.Visible = true;
                this.Alerta(OError);
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                this.TXTMRN.Focus();
                return string.Empty;
            }

            if (TablaImagenes == null)
            {
                //sinresultadoDespacho.Visible = true;
                return string.Empty;
            }

            if (TablaImagenes.Count == 0)
            {
                //sinresultadoFotos.Visible = true;
                return string.Empty;
            }

            string v_divImagenes = string.Empty;
            foreach (var Det in TablaImagenes)
            {
                if (v_contador == 0)
                {
                    v_contador = 1;
                    v_divImagenes += @"<div class='carousel-item active'>
                                            <img src = '" + Det.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                            <div class='carousel-caption d-none d-md-block'>
                                                <!-- <h5>Second slide label</h5>
                                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                            </div>
                                        </div> ";
                }
                else
                {
                    v_divImagenes += @"<div class='carousel-item'>
                                            <img src = '" + Det.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
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
                                    <div id='carouselExampleCaptions{X1}' class='carousel slide' data-ride='carousel'>
                                    <ol class='carousel-indicators'>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='0' class='active'></li>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='1'></li>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='2'></li>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='3'></li>
                                    </ol>
                                    <div class='carousel-inner'>
                                           
                                        " + v_divImagenes + @"
                                    </div>
                                    <a class='carousel-control-prev' href='#carouselExampleCaptions{X1}' role='button' data-slide='prev'>
                                        <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Previous</span>
                                    </a>
                                    <a class='carousel-control-next' href='#carouselExampleCaptions{X1}' role='button' data-slide='next'>
                                        <span class='carousel-control-next-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Next</span>
                                    </a>
                                </div>
                            </div>
                            ";
            v_cuerpo = v_cuerpo.Replace("{X1}", idDespacho.ToString());

            string v_html = string.Empty;
            v_html = v_html + v_cuerpo;

            tab.Append(v_html);
            return tab.ToString();
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

                    this.txtHSN.Text = string.Empty;
                    this.txtMSN.Text = string.Empty;
                    this.TXTMRN.Text = string.Empty;
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
                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.txtMSN.Text.Trim());
                Server.HtmlEncode(this.txtHSN.Text.Trim());

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
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "idRecepcion").ToString().Trim());

                var TablaCasos = novedad.listadoNovedades(v_id, out OError);

                if (TablaCasos.Count > 0)
                {
                    e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                }

                this.Actualiza_Panele_Detalle();
            }
        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Foto")
            {
                long v_ID = long.Parse(e.CommandArgument.ToString());
                Ocultar_Mensaje();

                int v_contador = 0;

                //se obtiene la lista de imagenes de la recepción seleccionada
                var TablaImagenes = fotoRecepcion.listadoFotoRecepcion(v_ID, out OError);

                if (OError != string.Empty)
                {
                    sinresultadoFotos.Visible = true;
                    UPMODAL.Update();
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                    return;
                }

                if (TablaImagenes == null)
                {
                    sinresultadoFotos.Visible = true;
                    UPMODAL.Update();
                    return;
                }

                if (TablaImagenes.Count == 0)
                {
                    sinresultadoFotos.Visible = true;
                    UPMODAL.Update();
                    return;
                }

                string v_divImagenes = string.Empty;
                foreach (var Det in TablaImagenes)
                {
                    if (v_contador == 0)
                    {
                        v_contador = 1;
                        v_divImagenes += @"<div class='carousel-item active'>
                                                  <img src = '" + Det.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                <div class='carousel-caption d-none d-md-block'>
                                                    <!-- <h5>Second slide label</h5>
                                                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                </div>
                                            </div> ";
                    }
                    else
                    {
                        v_divImagenes += @"<div class='carousel-item'>
                                                <img src = '" + Det.ruta + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
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

            if (e.CommandName == "Novedad") 
            {
                Ocultar_Mensaje();

                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }

                if (Response.IsClientConnected)
                {
                    try
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            Session.Clear();
                            Response.Redirect("../login.aspx", false);
                            OcultarLoading("1");
                            return;
                        }

                        var TablaCasos = novedad.listadoNovedades(v_ID, out OError);

                        if (OError != string.Empty)
                        {
                            sinresultadoNovedad.Visible = true;
                            xfinderNov.Visible = false;
                            UPNOV.Update();
                            this.Alerta(OError);
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                            this.TXTMRN.Focus();
                            return;
                        }

                        if (TablaCasos == null)
                        {
                            sinresultadoNovedad.Visible = true;
                            xfinderNov.Visible = false;
                            UPNOV.Update();
                            return;
                        }

                        if (TablaCasos.Count <= 0)
                        {
                            sinresultadoNovedad.Visible = true;
                            xfinderNov.Visible = false;
                            UPNOV.Update();
                            return;
                        }

                        StringBuilder tab = new StringBuilder();

                        string v_cuerpo = string.Empty;
                        string v_detalle = string.Empty;
                        string v_html = @"  <div class='bs-example'>
                                            <div class='accordion' id='accordionExample'>";

                        foreach (var Det in TablaCasos)
                        {
                            v_detalle = string.Empty;
                            v_detalle = v_detalle + "<p>" + ConsultarFotosNovedad(long.Parse(Det.idNovedad.ToString())) + "</p>";
                            v_detalle = v_detalle + "<p><span class='text-muted'>Descripción :</span> " + Det.descripcion + " <br/> <span class='text-muted'>Fecha Registro :</span> " + Det.fecha.Value.ToString("dd/MM/yyyy hh:mm");
                          
                            string barra = @"  &nbsp;  &nbsp;";

                            v_cuerpo = v_cuerpo + @"
                                                    <div class='card'>
                                                        <div class='card-header' id='heading{X1}'>
                                                            <h6 class='mb-0'>
                                                                <a class='collapsed card-link' data-toggle='collapse' data-target='#collapse{X1}'>
                                                                    <i class='fa fa-plus'></i> "
                                                                        + string.Format("<span class='text-muted'>Fecha y hora de novedad :</span> {0}  {1} " +
                                                                                        "<span class='text-muted'>Usuario responsable:</span> {2}  {3} <br/>" +
                                                                                        "<span class='text-muted'>Descripción :</span>   {4} "
                                                                                        , Det.fecha?.ToString("dd/MM/yyyy hh:mm"), barra
                                                                                        , Det.usuarioCrea?.ToUpper(), barra
                                                                                        , Det.descripcion?.ToUpper()) 
                                                                + @"</a>
                                                            </h6>
                                                        </div>
                                                        <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                            <div class='card-body'>
                                                                " + v_detalle + @"
                                                            </div>
                                                        </div>
                                                    </div>
                                                    ";

                            v_cuerpo = v_cuerpo.Replace("{X1}", Det.idNovedad.ToString());
                        }

                        v_html = v_html + v_cuerpo;
                        v_html = v_html + @" </div>
                            </div>";
                        tab.Append(v_html);
                        this.htmlcasos.InnerHtml = tab.ToString();
                        xfinderNov.Visible = true;
                        sinresultadoNovedad.Visible = false;
                        this.Actualiza_Panele_Detalle();
                        this.Ocultar_Mensaje();
                        UPNOV.Update();
                    }
                    catch (Exception ex)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                    }
                }
            }
        }
        protected void tablePagination_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (tablePagination.Rows.Count > 0)
                {
                    tablePagination.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    tablePagination.HeaderRow.TableSection = TableRowSection.TableHeader;

                    // Agrega el elemento TH en la fila de encabezado.               
                    // Agrega la sección TFOOT. 
                    //tablePagination.FooterRow.TableSection = TableRowSection.TableFooter;
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }

        }
        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
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
                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Alerta("Por favor ingresar el MRN");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar el MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtMSN.Text))
                    {
                        this.Alerta("Por favor ingresar el MSN");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar el MSN"));
                        this.txtMSN.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtHSN.Text))
                    {
                        this.Alerta("Por favor ingresar el HSN");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar el HSN"));
                        this.txtHSN.Focus();
                        return;
                    }

                    ConsultarDataRecepcion(TXTMRN.Text,txtMSN.Text, txtHSN.Text);
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }
        protected void btnVerDespachos_Click(object sender, EventArgs e)
        {
            
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        Response.Redirect("../login.aspx", false);
                        OcultarLoading("1");
                        return;
                    }

                    if (TXTMRN.Text == "" || txtMSN.Text == "" || txtHSN.Text == "")
                    {
                        sinresultadoDespacho.Visible = true;
                        xfinderDes.Visible = false;
                        UPDES.Update();
                        this.Alerta("Debe ingresar el número de carga");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Debe ingresar el número de carga"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    var oTarjaDet = tarjaDet.GetTarjaDet(TXTMRN.Text, txtMSN.Text, txtHSN.Text);

                    if (oTarjaDet == null)
                    {
                        sinresultadoDespacho.Visible = true;
                        xfinderDes.Visible = false;
                        UPDES.Update();
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se encontró resultados con el número de carga ingresado"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    var TablaCasos = despacho.listadoDespacho(long.Parse(oTarjaDet.idTarjaDet.ToString()), out OError);

                    if (OError != string.Empty)
                    {
                        sinresultadoDespacho.Visible = true;
                        xfinderDes.Visible = false;
                        UPDES.Update();
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                        return;
                    }

                    if (TablaCasos == null)
                    {
                        sinresultadoDespacho.Visible = true;
                        xfinderDes.Visible = false;
                        UPDES.Update();
                        return;
                    }

                    if (TablaCasos.Count <= 0)
                    {
                        sinresultadoDespacho.Visible = true;
                        xfinderDes.Visible = false;
                        UPDES.Update();
                        return;
                    }

                    StringBuilder tab = new StringBuilder();

                    string v_cuerpo = string.Empty;
                    string v_detalle = string.Empty;
                    string v_html = @"  <div class='bs-example'>
                                        <div class='accordion' id='accordionExample'>";

                    foreach (var Det in TablaCasos)
                    {
                        Det.tarjaDet = tarjaDet.GetTarjaDet(Det.idTarjaDet);
                        v_detalle = string.Empty;
                        v_detalle = v_detalle + "<p>" + ConsultarFotosDespacho(long.Parse(Det.idDespacho.ToString())) + "</p>";
                        v_detalle = v_detalle + "<p><span class='text-muted'>EPass :</span> " + Det.pase +
                        " <br/> <span class='text-muted'>Producto :</span> " + Det.tarjaDet.producto.nombre +
                        " <br/> <span class='text-muted'>Bodega :</span> " + Det.tarjaDet.ubicacion +
                        " <br/> <span class='text-muted'>Fecha Registro :</span> " + Det.fechaCreacion.Value.ToString("dd/MM/yyyy hh:mm");

                        string barra = @"  &nbsp;  &nbsp;";

                        v_cuerpo = v_cuerpo + @"
                                                <div class='card'>
                                                    <div class='card-header' id='heading{X1}'>
                                                        <h6 class='mb-0'>
                                                            <a class='collapsed card-link' data-toggle='collapse' data-target='#collapse{X1}'>
                                                                <i class='fa fa-plus'></i> "
                                                                    + string.Format("<span class='text-muted'>BL :</span>   {0} &nbsp;  &nbsp; &nbsp;  &nbsp;" +
                                                                                    "<span class='text-muted'>F/H DESPACHO :</span> {1}  {2} " +
                                                                                    "<span class='text-muted'>USUARIO :</span> {3}  {4} <br/>" +
                                                                                    "<span class='text-muted'>EPASS :</span>   {5}  &nbsp;  &nbsp; &nbsp;  &nbsp; " + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
                                                                                    "<span class='text-muted'>PLACA :</span>   {6}  &nbsp;  &nbsp; &nbsp;  &nbsp; " + "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
                                                                                    "<span class='text-muted'>CANTIDAD : </span>{8}  <br/>" +
                                                                                    "<span class='text-muted'>CHOFER :</span>  {7}  &nbsp;  &nbsp; <br/>" +
                                                                                    "<span class='text-muted'>NOTAS : </span>   {9}  " 
                                                                                    , Det?.mrn.ToUpper() + "-" + Det?.msn + "-" + Det?.hsn
                                                                                    , Det?.fechaCreacion?.ToString("dd/MM/yyyy HH:mm"), barra
                                                                                    , Det?.usuarioCrea?.ToUpper(), barra
                                                                                    , Det?.pase
                                                                                    , Det?.placa.ToUpper()
                                                                                    , Det?.idchofer?.ToUpper() +" - "+ Det.chofer?.ToUpper()
                                                                                    , Det?.cantidad
                                                                                    , Det?.observacion.ToUpper())
                                                            + @"</a>
                                                        </h6>
                                                    </div>
                                                    <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                        <div class='card-body'>
                                                            " + v_detalle + @"
                                                        </div>
                                                    </div>
                                                </div>
                                                ";

                        v_cuerpo = v_cuerpo.Replace("{X1}", Det.idDespacho.ToString());
                    }

                    v_html = v_html + v_cuerpo;
                    v_html = v_html + @" </div>
                        </div>";
                    tab.Append(v_html);
                    this.htmlDespachos.InnerHtml = tab.ToString();
                    xfinderDes.Visible = true;
                    sinresultadoDespacho.Visible = false;
                    this.Actualiza_Panele_Detalle();
                    this.Ocultar_Mensaje();
                    UPDES.Update();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataRecepcion), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                    return;
                }
            }
        }
        #endregion
    }
}