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
    public partial class brbkDatosDescarga : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private tarjaCab objCabecera= new tarjaCab();
        private List<tarjaDet> objDetalle = new List<tarjaDet>();
        #endregion

        #region "Variables"
        private string cMensajes;
        private static Int64? lm = -3;
        private string OError;
        #endregion

        #region "Propiedades"
        private Int64? nSesion { get { return (Int64)Session["nSesion"];} set { Session["nSesion"] = value;}}
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
        private void Mostrar_MensajeDet(string Mensaje)
        {
            this.msjErrorDetalle.Visible = true;
            this.msjErrorDetalle.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            UPEDIT.Update();
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
                this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboProductos()
        {
            try
            {
                cmbProducto.DataSource = productos.consultaProductos(); 
                cmbProducto.DataValueField = "ID";
                cmbProducto.DataTextField = "nombre";
                cmbProducto.DataBind();
                cmbProducto.Enabled = true;

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboManiobras), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboManiobras()
        {
            try
            {
                cmbManiobra.DataSource = maniobra.consultaManiobras(); //ds.Tables[0].DefaultView;
                cmbManiobra.DataValueField = "ID";
                cmbManiobra.DataTextField = "nombre";
                cmbManiobra.DataBind();
                cmbManiobra.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboManiobras), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboItems()
        {
            try
            {
                cmbItem.DataSource = items.consultaItems(); //ds.Tables[0].DefaultView;
                cmbItem.DataValueField = "ID";
                cmbItem.DataTextField = "nombre";
                cmbItem.DataBind();
                cmbItem.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboItems), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboCondicion()
        {
            try
            {
                cmbCondicion.DataSource = condicion.consultaCondicion(); //ds.Tables[0].DefaultView;
                cmbCondicion.DataValueField = "ID";
                cmbCondicion.DataTextField = "nombre";
                cmbCondicion.DataBind();
                cmbCondicion.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboCondicion), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboIMO()
        {
            try
            {
                cmbImo.DataSource = imos.consultaImos(); //ds.Tables[0].DefaultView;
                cmbImo.DataValueField = "ID";
                cmbImo.DataTextField = "imo";
                cmbImo.DataBind();
                cmbImo.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboIMO), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenarFiltroEstados()
        {
            try
            {


                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";
                cmbFiltroEstados.Items.Add(item);
                cmbFiltroEstados.Items.Add(new ListItem("VERIFICADO", "ACT"));
                cmbFiltroEstados.Items.Add(new ListItem("VERIFCACIÓN PENDIENTE", "NO_ACT"));
                cmbFiltroEstados.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboCondicion), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        private void ConsultarDataEcuapass()
        {
            try
            {
                //var oCarrier = carrier.GetCurrier(Txtruc.Text);

                //if (oCarrier == null)
                //{
                //    if (string.IsNullOrEmpty(oCarrier.carrier_id))
                //    {
                //        this.Alerta("No se encontro el Carrier Id del cliente");
                //        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se encontro el Carrier Id del cliente"));
                //        this.TXTMRN.Focus();
                //        return;
                //    }
                //}

                var Resultado = tarjaDet.ConsultaDataEcuapass(Txtruc.Text,TXTMRN.Text,out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                    return;
                }

                if (Resultado != null)
                {
                   
                    if (Resultado != null && Resultado.Count() > 0)
                    {
                        tarjaCab oTarjaCab = new tarjaCab();
                        oTarjaCab.idTarja = 0;
                        oTarjaCab.idAgente = Txtruc.Text;
                        oTarjaCab.Agente = Txtempresa.Text+ " - "+ Txtcliente.Text;
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
                        Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] = objCabecera;

                        tablePagination.DataSource = Resultado;
                        tablePagination.DataBind();

                        this.btnGrabar.Attributes.Remove("disabled");
                        
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        sinresultado.Visible = true;
                        this.btnGrabar.Attributes["disabled"] = "disabled";
                    }

                    
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                    sinresultado.Visible = true;
                    this.btnGrabar.Attributes["disabled"] = "disabled";
                }
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataEcuapass), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }
        private void ConsultarDataTarjaN4Middle()
        {
            try
            {
                var Resultado = tarjaCab.GetTarjaCab(txtNave.Text,Txtruc.Text,TXTMRN.Text, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                }

                if (Resultado != null )
                {
                    if (Resultado.Detalle != null)
                    {
                        foreach (var a in Resultado.Detalle)
                        {
                            a.tarjaCab = Resultado;
                            a.producto = productos.GetProducto(a.idProducto);
                            a.condicion = condicion.GetCondicion(a.idCondicion);
                            a.Estados = estados.GetEstado(a.estado);
                        }

                        objCabecera = Resultado;
                        Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] = objCabecera;

                        txtNave.Text = objCabecera.idNave;
                        txtDescripcionNave.Text = objCabecera.nave;
                        TXTMRN.Text = objCabecera.mrn;
                        fecETA.Text = objCabecera.eta.ToString("dd/MM/yyyy");
                        cmbEstado.SelectedValue = objCabecera.estado;

                        if (Resultado.Detalle.Count() > 0)
                        {
                            tablePagination.DataSource = Resultado.Detalle;
                            tablePagination.DataBind();
                        }
                        this.btnGrabar.Attributes["disabled"] = "disabled";
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
                    ConsultarDataEcuapass();
                   
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
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    this.btnGrabar.Attributes["disabled"] = "disabled";
                    sinresultado.Visible = false;
                    objCabecera.Detalle = objDetalle;
                    LlenaComboEstado();
                    LlenaComboProductos();
                    LlenaComboManiobras();
                    LlenaComboItems();
                    LlenaComboCondicion();
                    LlenaComboIMO();
                    LlenarFiltroEstados();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbProducto.Items.Add(item);
                    cmbManiobra.Items.Add(item);
                    cmbItem.Items.Add(item);
                    cmbCondicion.Items.Add(item);
                    cmbImo.Items.Add(item);
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");
                string v_estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString().Trim();
                long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "idTarjaDet").ToString().Trim());

                if (v_id <= 0)
                {
                    btnEditar.Enabled = false;
                }
                else
                {
                    if (v_estado != "NUE")
                    {
                        Chk.Checked = true;
                        e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                    }
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
                objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;
                if (objCabecera == null) { return; }

                var oDet = objCabecera.Detalle.Where(a => a.idTarjaDet == v_ID).FirstOrDefault();
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = oDet;

                txtBL.Text = oDet.bl;
                txtmrnDet.Text = oDet.mrn;
                txtmsnDet.Text = oDet.msn;
                txthsnDet.Text = oDet.hsn;
                txtRucConsignatario.Text = oDet.idConsignatario;
                txtConsignatario.Text = oDet.Consignatario;
                txtProductoEcuapass.Text = oDet.productoEcuapass;
                txtCantidad.Text = oDet.cantidad.ToString();
                txtKilos.Text = oDet.kilos.ToString();
                txtCubicaje.Text = oDet.cubicaje.ToString();
                txtTonelaje.Text = oDet.tonelaje.ToString();
                txtDescripcion.Text = oDet.descripcion;
                txtContenido.Text = oDet.contenido;
                txtobservacion.Text = oDet.observacion;
                cmbImo.SelectedValue= oDet.imo;

                cmbProducto.SelectedValue = oDet.idProducto.ToString();
                if (cmbProducto.SelectedValue =="0")
                {
                    cmbManiobra.SelectedValue = "0";
                    cmbItem.SelectedValue = "0";
                    cmbCondicion.SelectedValue = "0";
                }
                else
                {
                    cmbManiobra.SelectedValue = oDet.producto?.Maniobra.id.ToString();
                    cmbItem.SelectedValue = oDet.producto?.Items.id.ToString();
                    cmbCondicion.SelectedValue = oDet.idCondicion.ToString();
                }
                nave oNave;
                try { oNave = nave.GetNave(oDet.tarjaCab.idNave); } catch { Response.Redirect("../login.aspx", false); return; }

                if (string.IsNullOrEmpty(oNave.ata?.ToString()))
                {
                    //se valida que el Bl no se encuentre en status PRE - CON - DES
                    var oDeta = tarjaDet.GetTarjaDet(long.Parse(oDet.idTarjaDet.ToString()));
                    if (oDeta.estado == "NUE" || oDeta.estado == "ACT")
                    {
                        this.btnActualizar.Attributes.Remove("disabled");
                    }
                    else
                    {
                        this.btnActualizar.Attributes["disabled"] = "disabled";
                        this.Alerta("No se puede actualizar, el BL se encuentra en status " + oDeta.Estados.nombre);
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el BL se encuentra en status " + oDeta.Estados.nombre));
                        this.txtBL.Focus();
                        
                        UPEDIT.Update();
                        return;
                    }
                }
                else
                {
                    this.btnActualizar.Attributes["disabled"] = "disabled";
                    this.Alerta("La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                    this.txtBL.Focus();
                    UPEDIT.Update();
                    return;
                }

                msjErrorDetalle.Visible = false;
                UPEDIT.Update();
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

                    //var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

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
                    this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes["disabled"] = "disabled";
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
            this.btnGrabar.Attributes["disabled"] = "disabled";
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

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;
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
                            this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre de la nave"));
                            this.txtNave.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.TXTMRN.Text))
                        {
                            this.Alerta("Ingrese el MRN.");
                            this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el MRN"));
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
                            this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un estado."));
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
                                    this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
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
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGrabar_Click), "btnGrabar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                //this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {
                    objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;
                    nave oNave;
                    try { oNave = nave.GetNave(objCabecera.idNave);} catch { Response.Redirect("../login.aspx", false); return; }

                    if (string.IsNullOrEmpty(oNave.ata?.ToString()))
                    {
                        this.btnActualizar.Attributes.Remove("disabled");
                    }
                    else
                    {
                        this.btnActualizar.Attributes["disabled"] = "disabled";
                        this.Alerta("La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                        this.txtBL.Focus();
                        return;
                    }

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

                        if (string.IsNullOrEmpty(this.txtBL.Text))
                        {
                            this.Alerta("Ingrese el BL.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el BL"));
                            this.txtBL.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtmrnDet.Text))
                        {
                            this.Alerta("Ingrese el MRN.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el MRN"));
                            this.txtmrnDet.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtmsnDet.Text))
                        {
                            this.Alerta("Ingrese el MSN.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el MSN"));
                            this.txtmsnDet.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txthsnDet.Text))
                        {
                            this.Alerta("Ingrese el HSN.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el HSN"));
                            this.txthsnDet.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtRucConsignatario.Text))
                        {
                            this.Alerta("Ingrese consignatario.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el consignatario"));
                            this.txtRucConsignatario.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtConsignatario.Text))
                        {
                            this.Alerta("Ingrese nombre consignatario.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre consignatario"));
                            this.txtConsignatario.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtProductoEcuapass.Text))
                        {
                            this.Alerta("Ingrese la descripción del producto.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese la descripción del producto."));
                            this.txtProductoEcuapass.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbProducto.SelectedValue) || (cmbProducto.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un producto.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un producto."));
                            cmbProducto.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbManiobra.SelectedValue) || (cmbManiobra.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione una maniobra.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una maniobra."));
                            cmbManiobra.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbItem.SelectedValue) || (cmbItem.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un Item.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un Item."));
                            cmbItem.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbCondicion.SelectedValue) || (cmbCondicion.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione una condición.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una condición."));
                            cmbCondicion.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtCantidad.Text))
                        {
                            this.Alerta("Ingrese la cantidad.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese la cantidad."));
                            this.txtCantidad.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtKilos.Text))
                        {
                            this.Alerta("Ingrese los kilos.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese los kilos."));
                            this.txtKilos.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtCubicaje.Text))
                        {
                            this.Alerta("Ingrese cubicaje.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese cubicaje."));
                            this.txtCubicaje.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtTonelaje.Text))
                        {
                            this.Alerta("Ingrese tonelaje.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese tonelaje."));
                            this.txtTonelaje.Focus();
                            return;
                        }

                        tarjaDet oDetalle = new tarjaDet();
                        oDetalle = Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] as tarjaDet;

                        if (oDetalle != null)
                        {
                            //se valida que el Bl no se encuentre en status PRE - CON - DES
                            var oDet = tarjaDet.GetTarjaDet(long.Parse(oDetalle.idTarjaDet.ToString()));
                            if (oDet.estado == "NUE" || oDet.estado == "ACT")
                            {
                                this.btnActualizar.Attributes.Remove("disabled");
                                UPEDIT.Update();
                            }
                            else
                            {
                                this.btnActualizar.Attributes["disabled"] = "disabled";
                                this.Alerta("No se puede actualizar, el BL se encuentra en status "+ oDet.Estados.nombre );
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el BL se encuentra en status " + oDet.Estados.nombre));
                                this.txtBL.Focus();
                                UPEDIT.Update();
                                return;
                            }

                            try {
                                var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                                ClsUsuario = ClsUsuario_;
                            }
                            catch
                            {
                                Response.Redirect("../login.aspx", false);
                                return;
                            }

                            oDetalle.bl = txtBL.Text;
                            oDetalle.mrn = txtmrnDet.Text;
                            oDetalle.msn = txtmsnDet.Text;
                            oDetalle.hsn = txthsnDet.Text;
                            oDetalle.idConsignatario = txtRucConsignatario.Text;
                            oDetalle.Consignatario = txtConsignatario.Text;
                            oDetalle.productoEcuapass = txtProductoEcuapass.Text;
                            oDetalle.cantidad = decimal.Parse(txtCantidad.Text);
                            oDetalle.kilos = int.Parse(txtKilos.Text);
                            oDetalle.cubicaje = decimal.Parse(txtCubicaje.Text);
                            oDetalle.tonelaje = decimal.Parse(txtTonelaje.Text);
                            oDetalle.descripcion = txtDescripcion.Text;
                            oDetalle.contenido = txtContenido.Text;
                            oDetalle.observacion = txtobservacion.Text;
                            oDetalle.imo = cmbImo.SelectedValue;

                            oDetalle.idProducto = int.Parse(cmbProducto.SelectedValue);
                            oDetalle.idManiobra = int.Parse(cmbManiobra.SelectedValue);
                            oDetalle.idItem = int.Parse(cmbItem.SelectedValue);
                            oDetalle.idCondicion = int.Parse(cmbCondicion.SelectedValue);
                            oDetalle.usuarioModifica = ClsUsuario.loginname;
                            oDetalle.estado = "ACT";
                            msjErrorDetalle.Visible = false;

                            oDetalle.idTarjaDet = oDetalle.Save_Update(out OError);

                            if (OError != string.Empty)
                            {
                                this.Alerta(OError);
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                this.TXTMRN.Focus();
                                return;
                            }
                            else
                            {
                                btnBuscar_Click(null, null);
                                this.Alerta("Transacción exitosa");
                                //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el BL  {0} ", oDetalle.bl.ToString()));
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el BL  {0} ", oDetalle.bl.ToString()));
                                this.btnActualizar.Attributes["disabled"] = "disabled";
                                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                            }
                        }
                        else { Response.Redirect("../login.aspx", false); return; }
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

        protected void cmbProducto_SelectedIdexChange(object sender, EventArgs e)
        {
            try
            {
                var oProducto = productos.GetProducto(int.Parse(cmbProducto.SelectedValue));
                cmbManiobra.SelectedValue = oProducto.idManiobra.ToString();
                cmbItem.SelectedValue = oProducto.item.ToString();
                msjErrorDetalle.Visible = false;
            }
            catch
            {
                cmbManiobra.SelectedValue = "0";
                cmbItem.SelectedValue = "0";
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                txtConsignatario.Text = string.Empty;
                msjErrorDetalle.Visible = false;
                if (string.IsNullOrEmpty(this.txtRucConsignatario.Text))
                {
                    this.Alerta("Ingrese el ruc.");
                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese el ruc del consignatario."));
                    this.txtKilos.Focus();
                    return;
                }

                var c = N4.Entidades.Cliente.ObtenerClienteSAV(Page.User.Identity.Name, txtRucConsignatario.Text);
                if (c.Resultado == null)
                {
                    this.Alerta("No se encontraron datos con el RUC: " + txtRucConsignatario.Text.Trim());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    txtConsignatario.Focus();
                    return;
                }
                txtConsignatario.Text = c.Resultado.CLNT_NAME;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscarCliente_Click), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            try
            {
                objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;
                if (txtFiltroMSN.Text == string.Empty && txtFiltroHSN.Text == string.Empty)
                {
                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                        return;
                    }
                    else
                    {
                        tablePagination.DataSource = objCabecera.Detalle;
                        tablePagination.DataBind();
                    }
                }
                else
                {
                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                        return;
                    }
                    else
                    {
                        if (txtFiltroMSN.Text != string.Empty && txtFiltroHSN.Text != string.Empty)
                        {
                            var oFiltro = objCabecera.Detalle.Where(a => a.mrn == TXTMRN.Text && a.msn == txtFiltroMSN.Text && a.hsn == txtFiltroHSN.Text);
                            tablePagination.DataSource = oFiltro.ToList();
                            
                        }
                        else
                        {
                            if (txtFiltroMSN.Text != string.Empty && txtFiltroHSN.Text == string.Empty)
                            {
                                var oFiltro = objCabecera.Detalle.Where(a => a.mrn == TXTMRN.Text && a.msn == txtFiltroMSN.Text);
                                tablePagination.DataSource = oFiltro.ToList();
                            }
                            else
                            {
                                tablePagination.DataSource = objCabecera?.Detalle;
                            }
                        }

                        tablePagination.DataBind();
                    }
                }
                txtFiltroMSN.Text = string.Empty;
                txtFiltroHSN.Text = string.Empty;
                this.Actualiza_Panele_Detalle();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }

        protected void cmbFiltroEstados_SelectedIdexChange(object sender, EventArgs e)
        {
            try
            {
                objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;

                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }

                if (cmbFiltroEstados.SelectedValue == "ACT")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.estado != "NUE");
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "NO_ACT")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.estado == "NUE");
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else
                {
                    tablePagination.DataSource = objCabecera.Detalle;
                }
                tablePagination.DataBind();
            }
            catch
            {
                btnFiltar_Click(null, null);
            }
        }
        #endregion
    }
}