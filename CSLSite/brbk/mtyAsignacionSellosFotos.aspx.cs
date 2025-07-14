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
using BreakBulk;

namespace CSLSite
{

    public partial class mtyAsignacionSellosFotos : System.Web.UI.Page
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
            UPEDIT.Update();
            UPMODAL.Update();
            UPBOTONES.Update();
        }
        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            UPCAB.Update();
            UPEDIT.Update();
        }
        private void Limpia_Datos_cliente()
        {
            this.Txtcliente.Text = string.Empty;
            this.txtNave.Text = string.Empty;
            this.txtDescripcionNave.Text = string.Empty;
            this.txtContenedor.Text = string.Empty;
            chkDiferentes.Checked = false;
            //this.dtpFechadesde.Text = string.Empty;
            //this.txtFechaHasta.Text = string.Empty;
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
            msjErrorDetalle.Visible = true;
            msjErrorDetalle.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
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
        
        private void ConsultarData(bool? _diferencia, string _nave,string _container)
        {
            try
            {
                var oDet = sellosImpo.listadoSellosAsignados(_diferencia, _nave, _container,out OError);

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
                        Actualiza_Paneles();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        sinresultado.Visible = true;
                    }
                }
               
                this.Actualiza_Paneles();

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtContenedor.Focus();
                    return;
                }


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
                    //this.dtpFechadesde.Text = string.Empty;
                    //this.txtFechaHasta.Text = string.Empty;
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
                //Server.HtmlEncode(this.dtpFechadesde.Text.Trim());
                //Server.HtmlEncode(this.txtFechaHasta.Text.Trim());

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
                Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                CheckBox ChkDIF = (CheckBox)e.Row.FindControl("CHKDIF");
                CheckBox ChkPRO = (CheckBox)e.Row.FindControl("CHKPRO");
                bool v_diferencia = (bool)DataBinder.Eval(e.Row.DataItem, "diferencia");
                bool v_revisado = (bool)DataBinder.Eval(e.Row.DataItem, "revisado");
                long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "id").ToString().Trim());

                if (v_diferencia)
                {
                    btnEditar.Enabled = true;
                    ChkDIF.Checked = true;
                    e.Row.ForeColor = System.Drawing.Color.DarkBlue;
                }
                else
                {
                    btnEditar.Enabled = false;
                    ChkDIF.Checked = false;
                }

                if (v_revisado)
                {
                    ChkPRO.Checked = true;
                }
                else
                {
                    ChkPRO.Checked = false;
                }

                this.Actualiza_Panele_Detalle();
            }
        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }
                this.btnActualizar.Attributes.Remove("disabled");

                var oDet = sellosImpo.GetSelloAsignado(v_ID);
                txtIdSelloMuelle.Text = oDet.id.ToString();
                txtContenedorEdit.Text = oDet.container;
                txtSello1.Text = oDet.sello1;
                txtSello2.Text = oDet.sello2;
                txtSello3.Text = oDet.sello3;
                txtSello4.Text = oDet.sello4;

                msjErrorDetalle.Visible = false;
                UPEDIT.Update();
            }

            if (e.CommandName == "Foto")
            {
                this.htmlImagenes.InnerHtml = string.Empty;
                long v_ID = long.Parse(e.CommandArgument.ToString());
                Ocultar_Mensaje();

                int v_contador = 0;

                //se obtiene la lista de imagenes de la recepción seleccionada
                var TablaImagenes = fotoSellos.listadoFotosSealMuelle(v_ID, out OError);

                if (OError != string.Empty)
                {
                    sinresultadoFotos.Visible = true;
                    UPMODAL.Update();
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtContenedor.Focus();
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

            bool? Diferencias;
            if (Response.IsClientConnected)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.txtContenedor.Text) && string.IsNullOrEmpty(this.txtNave.Text))
                    {
                        chkDiferentes.Checked = true;
                        UPCHK.Update();
                        ConsultarData(true, null, null);
                     
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.txtContenedor.Text) && !string.IsNullOrEmpty(this.txtNave.Text))
                        {
                           

                            if (chkDiferentes.Checked)
                            {
                                Diferencias = true;
                            }
                            else
                            {
                                Diferencias = null;
                            }
                            //chkDiferentes.Checked = false;
                            UPCHK.Update();
                            ConsultarData(Diferencias, this.txtNave.Text, this.txtContenedor.Text);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.txtContenedor.Text) && string.IsNullOrEmpty(this.txtNave.Text))
                            {
                                if (chkDiferentes.Checked)
                                {
                                    Diferencias = true;
                                }
                                else
                                {
                                    Diferencias = null;
                                }

                               // chkDiferentes.Checked = false;
                                UPCHK.Update();
                                ConsultarData(Diferencias, null, this.txtContenedor.Text);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(this.txtContenedor.Text) && !string.IsNullOrEmpty(this.txtNave.Text))
                                {
                                    if (chkDiferentes.Checked)
                                    {
                                        Diferencias = true;
                                    }
                                    else
                                    {
                                        Diferencias = null;
                                    }

                                    //chkDiferentes.Checked = false;
                                    UPCHK.Update();
                                    ConsultarData(Diferencias, this.txtNave.Text, null);
                                }
                            }
                        }
                    }
                    //if (string.IsNullOrEmpty(dtpFechadesde.Text) && string.IsNullOrEmpty(txtFechaHasta.Text))
                    //{
                    //    ConsultarData(null, null, txtContenedor.Text);
                    //    return;
                    //}

                    //CultureInfo enUS = new CultureInfo("en-US");
                    //DateTime fechaini;
                    
                    //if (!DateTime.TryParseExact(dtpFechadesde.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaini))
                    //{
                    //    this.Response.ClearContent();
                    //    return;
                    //}
                    //DateTime fechafin;
                    //if (!DateTime.TryParseExact(txtFechaHasta.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechafin))
                    //{
                    //    this.Response.ClearContent();
                    //    return;
                    //}

                    
                    
                   //ConsultarData(fechaini, fechafin, string.IsNullOrEmpty(txtContenedor.Text)? null: txtContenedor.Text);
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {

                    if (!string.IsNullOrEmpty(txtIdSelloMuelle.Text))
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtSello1.Text))
                        {
                            this.Alerta("Ingrese el Sello 1");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el Sello 1"));
                            this.txtSello1.Focus();
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

                        string v_xmlSet = string.Empty;
                        var n4Result = actualizarSelloN4(txtContenedorEdit.Text.ToUpper(), txtSello1.Text.ToUpper(), txtSello2.Text.ToUpper(), txtSello3.Text.ToUpper(), txtSello4.Text.ToUpper(), out v_xmlSet);

                        if (n4Result != "OK")
                        {
                            this.Alerta(n4Result);
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", n4Result));
                            this.txtSello1.Focus();
                            return;
                        }

                        sellosImpo oSello = new sellosImpo();
                        oSello.id = long.Parse(txtIdSelloMuelle.Text);
                        oSello.sello1 = txtSello1.Text;
                        oSello.sello2 = txtSello2.Text;
                        oSello.sello3 = txtSello3.Text;
                        oSello.sello4 = txtSello4.Text;
                        oSello.usuarioModifica = ClsUsuario.loginname;
                        oSello.xmlN4 = v_xmlSet;
                        oSello.xmlResult = n4Result;
                        oSello.estado = false;
                        oSello.diferencia = false;
                        oSello.revisado = true;
                        msjErrorDetalle.Visible = false;

                        oSello.id = oSello.Save_Update(out OError);

                        if (OError != string.Empty)
                        {
                            this.Alerta(OError);
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                            this.txtSello1.Focus();
                            return;
                        }
                        else
                        {
                            btnBuscar_Click(null, null);
                            this.Alerta("Transacción exitosa");
                            //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el BL  {0} ", oDetalle.bl.ToString()));
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente - [{0}] ", oSello.id.ToString()));
                            this.btnActualizar.Attributes["disabled"] = "disabled";
                            Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnActualizar_Click), "btnActualizar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                //this.Mostrar_MensajeDet(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        public static string actualizarSelloN4(string cntr,string sello1, string sello2, string sello3, string sello4, out string _n4)
        {
            ObjectSesion usuario = new ObjectSesion();
            string result = string.Empty;
            var webService = new n4WebService();
            var msf = string.Empty;
            StringBuilder n4 = new StringBuilder();
            n4.AppendFormat("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\" /></units><properties>", cntr);
            n4.AppendFormat("<property tag=\"SealNbr1\" value=\"{0}\" />",sello1);
            if (!string.IsNullOrEmpty(sello2)) { n4.AppendFormat("<property tag=\"SealNbr2\" value=\"{0}\" />", sello2); }
            if (!string.IsNullOrEmpty(sello3)) { n4.AppendFormat("<property tag=\"SealNbr3\" value=\"{0}\" />", sello3); }
            if (!string.IsNullOrEmpty(sello4)) { n4.AppendFormat("<property tag=\"SealNbr4\" value=\"{0}\" />", sello4); }
            n4.Append("</properties></icu>");
            _n4 = n4.ToString();
            var i = webService.InvokeN4Service(usuario, n4.ToString(), ref msf, usuario.usuario);
            if (i < 0 || i > 1)
            {
                result = "ERROR N4: " + msf;
                csl_log.log_csl.save_log<Exception>(new ApplicationException("No se pudo actualizar el sello"), "mtySellosFotos", "actualizarSelloN4", cntr, n4.ToString());
            }
            else { result = "OK"; }
            return result;
        }

        #endregion
    }
}