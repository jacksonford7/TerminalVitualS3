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
    public partial class brbkRecepcion : System.Web.UI.Page
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
        private recepcion oRecepcion
        {
            get
            {
                return (recepcion)Session["SessionRecepcion"];
            }
            set
            {
                Session["SessionRecepcion"] = value;
            }
        }

        private Int64? nSesion { get { return (Int64)Session["nSesion"]; } set { Session["nSesion"] = value; } }
        #endregion

        #region "Metodos"
        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCAB.Update();
            UPEDIT.Update();
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
            this.txtFechaAtA.Text = string.Empty;
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
            this.msjErrorDetalle.Visible = false;
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

        public void LlenaComboUbicacion()
        {
            try
            {
                cmbUbicacion.DataSource = ubicacion.consultaUbicacion();
                cmbUbicacion.DataValueField = "ID";
                cmbUbicacion.DataTextField = "nombre";
                cmbUbicacion.DataBind();
                //cmbUbicacion.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboUbicacion), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboProductos), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
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
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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
        public void LlenarFiltroEstados()
        {
            try
            {
                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";
                cmbFiltroEstados.Items.Add(item);
                cmbFiltroEstados.Items.Add(new ListItem("CONFIRMADOS", "CON"));
                cmbFiltroEstados.Items.Add(new ListItem("CONFIRMACIÓN PENDIENTE", "NO_CON"));
                cmbFiltroEstados.Items.Add(new ListItem("DESCARGADOS", "DES"));
                cmbFiltroEstados.Items.Add(new ListItem("DESCARGA PENDIENTE", "NO_DES"));
                cmbFiltroEstados.Items.Add(new ListItem("BL GENERADO", "N4"));
                cmbFiltroEstados.Items.Add(new ListItem("BL PENDIENTE", "NO_N4"));
                cmbFiltroEstados.Items.Add(new ListItem("IMDT GENERADO", "ECU"));
                cmbFiltroEstados.Items.Add(new ListItem("IMDT PENDIENTE", "NO_ECU"));
                cmbFiltroEstados.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenarFiltroEstados), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void ConsultarDataTarjaN4Middle()
        {
            try
            {
                var Resultado = tarjaDet.GetTarjaDetXNave(txtNave.Text, out OError);

                if (OError != string.Empty)
                {
                    //this.Alerta(OError);
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataTarjaN4Middle), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
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
                    //this.Alerta(OError);
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

                        var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.estado) && Tbl.lugar =="BODEGA")
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
                            dgvRecepcion.DataSource = LinqQuery;
                            dgvRecepcion.DataBind();
                        }
                    }
                    else
                    {
                        dgvRecepcion.DataSource = null;
                        dgvRecepcion.DataBind();
                    }
                }
                else
                {
                    dgvRecepcion.DataSource = null;
                    dgvRecepcion.DataBind();
                }

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarRecepcion), "ConsultarRecepcion", false, null, null, ex.StackTrace, ex);
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
                txtFechaAtA.Text = oNave.ata?.ToString("dd/MM/yyyy");
                TXTMRN.Text = oNave.in_customs_voy_nbr;

                if (oNave.ata != null)
                {
                    cmbEstado.SelectedValue = "PRE";
                }
                else
                {
                    cmbEstado.SelectedValue = "NUE";
                }
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

                    //ClsUsuario = Page.Tracker();
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
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    sinresultado.Visible = false;
                    objCabecera.Detalle = objDetalle;
                    LlenaComboEstado();
                    LlenaComboProductos();
                    LlenaComboManiobras();
                    LlenaComboItems();
                    LlenaComboCondicion();
                    LlenaComboUbicacion();
                    LlenarFiltroEstados();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbProducto.Items.Add(item);
                    cmbManiobra.Items.Add(item);
                    cmbItem.Items.Add(item);
                    cmbCondicion.Items.Add(item);
                    cmbUbicacion.Items.Add(item);
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
        protected void dgvRecepcion_RowCommand(object source, GridViewCommandEventArgs e)
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
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "Modificar")
                    {
                        //PagoAsignado pago = new PagoAsignado(id, ClsUsuario.loginname.Trim());
                        var Resultado = recepcion.GetRecepcion(long.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oRecepcion = Resultado;
                            txtCantidadReceptada.Text = Resultado.cantidad.ToString();
                            if(Resultado.ubicacion?.ToString().Trim() =="")
                            {
                                Resultado.ubicacion = "0";
                            }
                            cmbUbicacion.SelectedValue = Resultado.ubicacion.ToString().Trim();
                            txtobservacion.Text = "";
                        }
                        else
                        {
                            oRecepcion = null;
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }
                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(dgvRecepcion_RowCommand), "Modificar", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_MensajeDet( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                    CheckBox ChkConfirmado = (CheckBox)e.Row.FindControl("CHKPRO");
                    CheckBox ChkDescargado = (CheckBox)e.Row.FindControl("CHKDES");
                    CheckBox ChkIMDT = (CheckBox)e.Row.FindControl("ChkIMDT");
                    CheckBox ChkBL = (CheckBox)e.Row.FindControl("CHKBL");
                    string v_estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString().Trim();
                    long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "idTarjaDet").ToString().Trim());

                    bool v_imdt = bool.Parse(DataBinder.Eval(e.Row.DataItem, "imdt").ToString().Trim());
                    bool v_n4 = bool.Parse(DataBinder.Eval(e.Row.DataItem, "n4").ToString().Trim());

                    ChkConfirmado.Checked = false;
                    ChkDescargado.Checked = false;
                    ChkIMDT.Checked = false;
                    ChkBL.Checked = false;
                    btnEditar.Enabled = false;


                    if (v_id > 0)
                    {
                        btnEditar.Enabled = true;
                        if (v_estado == "DES")
                        {
                            ChkConfirmado.Checked = true;
                            ChkDescargado.Checked = true;

                            if (v_imdt)
                            {
                                ChkIMDT.Checked = true;
                            }

                            if (v_n4)
                            {
                                ChkBL.Checked = true;
                            }
                        }
                        else
                        {
                            if (v_estado == "CON")
                            {
                                ChkConfirmado.Checked = true;
                            }
                        }

                        if (v_estado == "DES" || v_estado == "CON")
                        {
                            e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                        }
                    }

                    this.Actualiza_Panele_Detalle();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_RowDataBound), "tablePagination_RowDataBound", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }

        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Editar")
                {
                    Ocultar_Mensaje();
                    msjErrorDetalle.Visible = false;

                    long v_ID = long.Parse(e.CommandArgument.ToString());
                    if (v_ID <= 0) { return; }
                    objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;
                    if (objCabecera == null) { return; }

                    var oDet = tarjaDet.GetTarjaDet(long.Parse(v_ID.ToString()));
                    Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = oDet;

                    txtBL.Text = oDet.bl;
                    txtCarga.Text = string.Format("{0} - {1} - {2}", oDet.mrn, oDet.msn, oDet.hsn);
                    txtConsignatario.Text = string.Format("{0} - {1}", oDet.idConsignatario, oDet.Consignatario);
                    txtProductoEcuapass.Text = oDet.productoEcuapass;
                    txtCantidad.Text = oDet.cantidad.ToString();
                    txtKilos.Text = oDet.kilos.ToString();
                    txtCubicaje.Text = oDet.cubicaje.ToString();
                    txtTonelaje.Text = oDet.tonelaje.ToString();

                    cmbUbicacion.SelectedValue = "0";//oDet.ubicacion == null ? "0" : oDet.ubicacion;
                    cmbEstado.SelectedValue = oDet.estado;
                    cmbCondicion.SelectedValue = oDet.idCondicion.ToString();
                    cmbProducto.SelectedValue = oDet.idProducto.ToString();

                    txtCantidadReceptada.Text = "";
                    txtobservacion.Text = "";

                    hdf_CodigoDet.Value = v_ID.ToString();
                    if (cmbProducto.SelectedValue == "0")
                    {
                        cmbManiobra.SelectedValue = "0";
                        cmbItem.SelectedValue = "0";
                        cmbCondicion.SelectedValue = "0";
                    }
                    else
                    {
                        cmbManiobra.SelectedValue = oDet.producto?.Maniobra.id.ToString();
                        cmbItem.SelectedValue = oDet.producto?.Items.id.ToString();
                    }

                    ConsultarRecepcion(v_ID);
                    UPEDIT.Update();

                    //se valida que el Bl se encuentre en status permitidos
                    var oDeta = tarjaDet.GetTarjaDet(long.Parse(oDet.idTarjaDet.ToString()));
                    if (oDeta.estado == "PRE" || oDeta.estado == "CON")
                    {
                        this.btnActualizar.Attributes.Remove("disabled");
                        UPEDIT.Update();
                    }
                    else
                    {
                        this.btnActualizar.Attributes["disabled"] = "disabled";
                        if (oDet.estado.ToString() == "DES")
                        {
                            this.Alerta("El BL está en descarga definitiva.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El BL está en descarga definitiva."));
                        }
                        this.txtCantidadReceptada.Focus();
                        UPEDIT.Update();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_RowCommand), "tablePagination_RowCommand", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_PreRender), "tablePagination_PreRender", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_PageIndexChanging), "tablePagination_PageIndexChanging", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
        }
        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                Limpia_Datos_cliente();
                objDetalle.Clear();
                objCabecera = new tarjaCab();
                Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] = objCabecera;
                tablePagination.DataSource = null;
                tablePagination.DataBind();
                this.Ocultar_Mensaje();
                OcultarLoading("1");
                OcultarLoading("2");
                Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnLimpiar_Click), "btnLimpiar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
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
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar MRN"));
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
                OcultarLoading("1");
                OcultarLoading("2");
                Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscar_Click), "btnBuscar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
        }
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    try
                    {
                        
                        Ocultar_Mensaje();
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            OcultarLoading("2");
                            return;
                        }

                        if (string.IsNullOrEmpty(txtCantidadReceptada.Text))
                        {
                            //this.Alerta("Ingrese la cantidad.");
                            this.Mostrar_MensajeDet( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Ingrese la cantidad."));
                            txtCantidadReceptada.Focus();
                            UPEDIT.Update();
                            return;
                        }

                        if (decimal.Parse(txtCantidadReceptada.Text) <= 0)
                        {
                            //this.Alerta("Ingrese la cantidad.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Ingrese la cantidad."));
                            txtCantidadReceptada.Focus();
                            UPEDIT.Update();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbUbicacion.SelectedValue))
                        {
                            //this.Alerta("Seleccione la ubicación.");
                            this.Mostrar_MensajeDet( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione la ubicación."));
                            cmbUbicacion.Focus();
                            UPEDIT.Update();
                            return;
                        }

                        if (cmbUbicacion.SelectedValue =="0")
                        {
                            //this.Alerta("Seleccione la ubicación.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione la ubicación."));
                            cmbUbicacion.Focus();
                            UPEDIT.Update();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtobservacion.Text))
                        {
                            //this.Alerta("Ingrese una observación.");
                            this.Mostrar_MensajeDet( string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor Ingrese una observación"));
                            this.txtobservacion.Focus();
                            UPEDIT.Update();
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
                        recepcion oRec = new recepcion();
                        oRec.lugar = "BODEGA";
                        oRec.estado = "CON";
                        if (oRecepcion != null)
                        {
                            oRec.idRecepcion = oRecepcion.idRecepcion;
                            oRec.idTarjaDet = oRecepcion.idTarjaDet;
                            oRec.cantidad = decimal.Parse(txtCantidadReceptada.Text.ToString());
                            oRec.ubicacion = cmbUbicacion.SelectedValue.ToString();
                            oRec.observacion = txtobservacion.Text;
                            oRec.usuarioModifica = ClsUsuario.loginname;
                        }
                        else
                        {
                            oRec.idTarjaDet = long.Parse(hdf_CodigoDet.Value);
                            oRec.cantidad = decimal.Parse(txtCantidadReceptada.Text);
                            oRec.ubicacion = cmbUbicacion.SelectedValue.ToString();
                            oRec.observacion = txtobservacion.Text;
                            oRec.usuarioCrea = ClsUsuario.loginname;
                        }

                        oRec.Save_Update(out OError);

                        if (OError != string.Empty)
                        {
                            //this.Alerta(OError);
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b> Verificar la cantidad ingresada: {0}", OError));
                            this.txtCantidadReceptada.Focus();
                            oRecepcion = null;
                            ConsultarRecepcion(oRec.idTarjaDet);
                            return;
                        }
                        else
                        {
                            this.Alerta("Registro Almacenado");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Registro actualizado"));
                        }
                        ConsultarRecepcion(oRec.idTarjaDet);

                        btnLimpiar2_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnActualizar_Click), "InsertaRecepcion", false, null, null, ex.StackTrace, ex);
                        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    }
                    oRecepcion = null;
                    btnBuscar_Click(null,null);
                }
            }
            catch (Exception ex)
            {
                this.btnActualizar.Attributes["disabled"] = "disabled";
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnActualizar_Click), "btnActualizar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
            Actualiza_Paneles();
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

                if (cmbFiltroEstados.SelectedValue == "CON")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.estado == "CON" || a.estado == "DES");
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "NO_CON")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.estado != "CON" && a.estado != "DES");
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "DES")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.estado == "DES");
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "NO_DES")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.estado != "DES");
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "N4")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.n4 == true);
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "NO_N4")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.n4 == false);
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "ECU")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.imdt == true);
                    tablePagination.DataSource = oFiltro.ToList();
                }
                else if (cmbFiltroEstados.SelectedValue == "NO_ECU")
                {
                    var oFiltro = objCabecera.Detalle.Where(a => a.imdt == false);
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
        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            try
            {
                objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;
                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }

                if (txtFiltroMSN.Text == string.Empty && txtFiltroHSN.Text == string.Empty)
                {
                    tablePagination.DataSource = objCabecera.Detalle;
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
                }
                tablePagination.DataBind();
                txtFiltroMSN.Text = string.Empty;
                txtFiltroHSN.Text = string.Empty;
                this.Actualiza_Panele_Detalle();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void btnLimpiar2_Click(object sender, EventArgs e)
        {
            try
            {
                oRecepcion = null;
                txtCantidadReceptada.Text = string.Empty;
                txtobservacion.Text = string.Empty;
                cmbUbicacion.SelectedValue = "0";
                Ocultar_Mensaje();
                OcultarLoading("1");
                OcultarLoading("2");
                Actualiza_Paneles();
                Actualiza_Panele_Detalle();
            }
            catch (Exception ex)
            {
                this.btnActualizar.Attributes["disabled"] = "disabled";
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnLimpiar2_Click), "btnLimpiar2_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        #endregion
    }
}