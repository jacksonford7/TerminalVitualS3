using BillionEntidades;
using BreakBulk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.brbk
{
    public partial class brbkReversoTransaccion : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private tarjaCab objCabecera = new tarjaCab();
        private List<tarjaDet> objDetalle = new List<tarjaDet>();
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
            this.txtNave.Text = string.Empty;
            this.txtDescripcionNave.Text = string.Empty;
            this.fecETA.Text = string.Empty;

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

            //cabecera de transaccion
            objCabecera = new tarjaCab();
            Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] = objCabecera;
        }
        public void LlenaComboEstado()
        {
            try
            {
                cmbEstado.DataSource = estados.consultaEstados();
                cmbEstado.DataValueField = "ID";
                cmbEstado.DataTextField = "nombre";
                cmbEstado.DataBind();
                cmbEstado.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
       
        private void ConsultarDataTarjaN4Middle()
        {
            try
            {
                var Resultado = tarjaDet.ConsultaDataEcuapassCGSA(TXTMRN.Text, out OError);

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

                        objCabecera.idNave = txtNave.Text;
                        objCabecera.nave = txtDescripcionNave.Text;
                        objCabecera.mrn = TXTMRN.Text;
                        objCabecera.Detalle = Resultado;
                        Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] = objCabecera;

                        tablePagination.DataSource = Resultado;
                        tablePagination.DataBind();

                        //this.btnGrabar.Attributes["disabled"] = "disabled";
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataTarjaN4Middle), "ConsultarDataTarjaN4Middle", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }
        protected string jsarguments(object ID_CAB, object ID_DET)
        {
            return string.Format("{0};{1}", ID_CAB != null ? ID_CAB.ToString().Trim() : "0", ID_DET != null ? ID_DET.ToString().Trim() : "0");
        }
        protected void ValidarDatosNave()
        {
            try
            {
                var oNave = nave.GetNave(txtNave.Text);
                txtDescripcionNave.Text = oNave.name;
                fecETA.Text = oNave.published_eta.ToString("dd/MM/yyyy");
                TXTMRN.Text = oNave.in_customs_voy_nbr;
                UPCAB.Update();
            }
            catch { }

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

                    this.txtNave.Text = string.Empty;
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
                Server.HtmlEncode(this.txtNave.Text.Trim());


                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    //var ClsUsuario = HttpContext.Current.Session["control"] as usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    //this.btnGrabar.Attributes["disabled"] = "disabled";
                    sinresultado.Visible = false;
                    objCabecera.Detalle = objDetalle;
                    LlenaComboEstado();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbEstado.SelectedValue = "NUE";
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
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnGenerar = (Button)e.Row.FindControl("btnGenerar");
                    Button btnReversar = (Button)e.Row.FindControl("btnReversar");
                    //CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");
                    string v_estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString().Trim();
                    string v_idAgente = DataBinder.Eval(e.Row.DataItem, "idAgente").ToString().Trim();

                    //if (v_id <= 0)
                    //{
                    //    btnEditar.Enabled = false;
                    //}
                    //else
                    //{
                    //    if (v_estado != "NUE")
                    //    {
                    //        Chk.Checked = true;
                    //        e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                    //    }
                    //}
                    var Resultado = tarjaCab.GetTarjaCab(txtNave.Text, v_idAgente, TXTMRN.Text, out OError);
                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                    }

                    if (Resultado != null)
                    {
                        if (Resultado.Detalle.Count > 0)
                        {
                            var oDet = Resultado.Detalle.Where(a => a.estado != "NUE");
                            if (oDet.Count() == 0)
                            {
                                btnReversar.Enabled = true;
                                btnGenerar.Enabled = false;
                            }
                            else
                            {
                                btnReversar.Enabled = false;
                                btnGenerar.Enabled = false;
                            }
                        }
                        else
                        {
                            btnReversar.Enabled = false;
                            btnGenerar.Enabled = true;
                        }
                    }
                    else
                    {
                        btnReversar.Enabled = false;
                        btnGenerar.Enabled = true;
                    }

                    this.Actualiza_Panele_Detalle();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataTarjaN4Middle), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            Session["IdAgente" + this.hf_BrowserWindowName.Value] = string.Empty;
            if (e.CommandName == "Generar")
            {
                Session["IdAgente" + this.hf_BrowserWindowName.Value] = e.CommandArgument.ToString();
                return;
            }

            if (e.CommandName == "Reversar")
            {
                Session["IdAgente" + this.hf_BrowserWindowName.Value] = e.CommandArgument.ToString();
                return;
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
                objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;

                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    tablePagination.PageIndex = e.NewPageIndex;
                    tablePagination.DataSource = objCabecera.Detalle;
                    tablePagination.DataBind();
                    this.Actualiza_Panele_Detalle();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        #endregion

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
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "Modificar")
                    {
                        //PagoAsignado pago = new PagoAsignado(id, ClsUsuario.loginname.Trim());
                        /*var Resultado = productos.GetProducto(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oProducto = Resultado;
                            txtNombre.Text = Resultado.nombre;
                            cmbEstado.SelectedValue = Resultado.estado.ToString();
                            cmbManiobra.SelectedValue = Resultado.Maniobra.id.ToString();
                            cmbItem.SelectedValue = Resultado.Items.id.ToString();
                        }
                        else
                        {
                            oProducto = null;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }*/
                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "BorrarAsignacion", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //this.btnGrabar.Attributes["disabled"] = "disabled";
            Limpia_Datos_cliente();
            objDetalle.Clear();
            objCabecera = new tarjaCab();
            Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] = objCabecera;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //this.btnGrabar.Attributes["disabled"] = "disabled";
            sinresultado.Visible = false;
            ValidarDatosNave();

            objDetalle.Clear();
            objCabecera.Detalle = objDetalle;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
            if (Response.IsClientConnected)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.txtNave.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.txtNave.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>MRN no encontrado"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.fecETA.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Valor de ETA no válido"));
                        this.fecETA.Focus();
                        return;
                    }

                    ConsultarDataTarjaN4Middle();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }

        protected void btnGrabar_Click(tarjaCab objCabecera)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    if (objCabecera != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtNave.Text))
                        {
                            this.Alerta("Ingrese el nombre de la nave.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre de la nave"));
                            this.txtNave.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.TXTMRN.Text))
                        {
                            this.Alerta("Ingrese el MRN.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el MRN"));
                            this.TXTMRN.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.fecETA.Text))
                        {
                            this.Alerta("Ingrese ETA.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el ETA"));
                            this.TXTMRN.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbEstado.SelectedValue))
                        {
                            this.Alerta("Seleccione un estado.");
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un estado."));
                            cmbEstado.Focus();
                            return;
                        }

                        if (objCabecera.Detalle != null)
                        {
                            if (objCabecera.Detalle.Count > 0)
                            {
                                objCabecera.idTarja = objCabecera.Save_Update(out OError);

                                if (OError != string.Empty)
                                {
                                    this.Alerta(OError);
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                    this.TXTMRN.Focus();
                                    return;
                                }
                                else
                                {
                                    btnBuscar_Click(null, null);
                                    this.Alerta("Transacción exitosa");
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se generó exitosamente la transacción No {0} ", objCabecera.idTarja.ToString()));
                                }
                            }

                        }
                    }
                    else
                    {
                        this.Alerta("No se pudo generar la transacción.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se pudo generar la transacción.", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGrabar_Click), "btnGrabar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void btnGenerarTransaccion_Click(object sender, EventArgs e)
        {
           
            try
            {
                string v_agente = Session["IdAgente" + this.hf_BrowserWindowName.Value] as string;
                var Resultado = tarjaDet.ConsultaDataEcuapass(v_agente, TXTMRN.Text, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                    return;
                }
                if (Resultado != null)
                {
                    if (Resultado != null && Resultado.Count() > 0)
                    {
                        tarjaCab oTarjaCab = new tarjaCab();
                        oTarjaCab.idTarja = 0;
                        oTarjaCab.idAgente = v_agente;
                        oTarjaCab.Agente = Txtempresa.Text + " - " + Txtcliente.Text;
                        oTarjaCab.mrn = TXTMRN.Text;
                        oTarjaCab.carrier_id = string.Empty;//oCarrier.carrier_id;
                        oTarjaCab.idNave = txtNave.Text;
                        oTarjaCab.nave = txtDescripcionNave.Text;

                        DateTime fecha = new DateTime();
                        CultureInfo enUS = new CultureInfo("en-US");
                        if (!DateTime.TryParseExact(fecETA.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
                        {
                            this.Alerta(string.Format("EL FORMATO DE FECHA ETA DEBE SER dia/Mes/Anio {0}", fecETA.Text));
                            fecETA.Focus();
                            return;
                        }
                        oTarjaCab.eta = fecha;

                        cmbEstado.SelectedValue = "NUE";
                        oTarjaCab.fecha = DateTime.Now;
                        oTarjaCab.estado = cmbEstado.SelectedValue;

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
                        oTarjaCab.usuarioCrea = ClsUsuario.loginname;

                        oTarjaCab.Detalle = Resultado;
                        objCabecera = oTarjaCab;

                        btnGrabar_Click(objCabecera);
                        //Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] = objCabecera;

                        //tablePagination.DataSource = Resultado;
                        //tablePagination.DataBind();
                    }
                    else
                    {
                        //tablePagination.DataSource = null;
                        //tablePagination.DataBind();
                        sinresultado.Visible = true;
                    }
                }
                else
                {
                    //tablePagination.DataSource = null;
                    //tablePagination.DataBind();
                    sinresultado.Visible = true;
                }
                this.Actualiza_Paneles();
                //btnBuscar_Click(null, null);
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGenerarTransaccion_Click), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
            Session["IdAgente" + this.hf_BrowserWindowName.Value] = "";
        }

        protected void btnReversarTrasaccion_Click(object sender, EventArgs e)
        {
            try
            {
                string v_agente = Session["IdAgente" + this.hf_BrowserWindowName.Value] as string;
                var Resultado = tarjaCab.GetTarjaCab(txtNave.Text, v_agente, TXTMRN.Text, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                }

                if (Resultado != null)
                {
                    Resultado.Save_Delete(out OError);
                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                        return;
                    }
                    else
                    {
                        btnBuscar_Click(null, null);
                        this.Alerta("Transacción exitosa");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se proceso exitosamente la transacción No {0} ", Resultado.idTarja.ToString()));
                    }
                }
                else
                {
                    this.Alerta("No existe Transacción");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se pudo encontrar la transacción No {0} ", Resultado.idTarja.ToString()));
                }
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGenerarTransaccion_Click), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
            Session["IdAgente" + this.hf_BrowserWindowName.Value] = "";
        }
        #endregion
    }
}