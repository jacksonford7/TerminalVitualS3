using BillionEntidades;
using BreakBulk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VBSEntidades.Banano;
using VBSEntidades.BananoBodega;
using VBSEntidades.BananoMuelle;

namespace CSLSite
{
    public partial class VBS_BAN_Roleo : System.Web.UI.Page
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
            UPRoleo.Update();
        }
        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            Actualiza_Paneles();
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
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }
        private void Mostrar_MensajeDet(string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");
        }
        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.InnerText = string.Empty;
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
        public void LlenaComboLinea()
        {
            try
            {
                var oEntidad = BAN_Catalogo_Linea.ConsultarListaLlenaCombo(Txtruc.Text); 
                cmbLinea.DataSource = oEntidad;
                cmbLinea.DataValueField = "ID";
                cmbLinea.DataTextField = "nombre";
                cmbLinea.DataBind();

                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";

                cmbLinea.Items.Add(item);
                cmbLinea.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboLinea), "VBS_BAN_PreStowage.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboExportador()
        {
            if (cmbLinea.SelectedValue.Equals("0")) { return; }
            try
            {
                string oError;
                var oLinea = BAN_Catalogo_Linea.GetLinea(int.Parse(cmbLinea.SelectedValue));
                var oEntidad = BAN_Catalogo_Exportador.ConsultarListaExportador(oLinea.ruc, out oError); //ds.Tables[0].DefaultView;
                cmbExportador.DataSource = oEntidad;
                cmbExportador.DataValueField = "ID";
                cmbExportador.DataTextField = "nombre";
                cmbExportador.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboExportador), "VBS_BAN_PreStowage.LlenaComboExportador", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        private void ConsultarDataStowage()
        {
            try
            {
                var ResultadoDet = BAN_Stowage_Movimiento.ConsultarLista(txtNave.Text, int.Parse(cmbLinea.SelectedValue), out OError);

                if (ResultadoDet != null && ResultadoDet.Count > 0)
                {
                    var ResultadoCab = BAN_Stowage_Plan_Cab.ConsultarLista(txtNave.Text, int.Parse(cmbLinea.SelectedValue), out OError);//.GetEntidad(int.Parse(ResultadoDet?.FirstOrDefault()?.idStowageCab.ToString()));

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                        return;
                    }

                    var oHold = BAN_Catalogo_Hold.ConsultarListaHold(out OError);
                    var oEstado = BAN_Catalogo_Estado.ConsultarLista(out OError);
                    var oCargo = BAN_Catalogo_Cargo.ConsultarListaCargos(out OError);
                    var oMarca = BAN_Catalogo_Marca.ConsultarListaMarca("CGSA", out OError);
                    var oExportador = BAN_Catalogo_Exportador.ConsultarListaExportador("CGSA", out OError);
                    var oConsignatario = BAN_Catalogo_Consignatario.ConsultarListaConsignatarios("CGSA", out OError);
                    var oBodega = BAN_Catalogo_Bodega.ConsultarLista(out OError);
                    var oBloque = BAN_Catalogo_Bloque.ConsultarLista(null, out OError);

                    List<BAN_Stowage_Plan_Det> oDetalle = new List<BAN_Stowage_Plan_Det>();
                    List<BAN_Stowage_Plan_Aisv> oDetalleAisv = new List<BAN_Stowage_Plan_Aisv>();
                    foreach (var a in ResultadoCab)
                    {
                        oDetalle.AddRange(BAN_Stowage_Plan_Det.ConsultarLista(a.idStowageCab, out OError));
                        oDetalleAisv.AddRange(BAN_Stowage_Plan_Aisv.ConsultarLista(a.idNave.ToString(), out OError));
                    }
                    
                    var oUbicaciones = BAN_Catalogo_Ubicacion.ConsultarLista(out OError);
                    var oFila = BAN_Catalogo_Fila.ConsultarLista(out OError);
                    var oAltura = BAN_Catalogo_Altura.ConsultarLista(out OError);
                    var oProfundidad = BAN_Catalogo_Profundidad.ConsultarLista(out OError);

                    foreach (var a in ResultadoDet)
                    {
                        
                        //a.oStowageCab = ResultadoCab;
                        a.oStowageDet = oDetalle.Where(p=> p.idStowageDet == a.idStowageDet).FirstOrDefault();
                        a.idExportador = a.oStowageDet.idExportador;
                        a.oStowageDet.oMarca = oMarca.Where(p => p.id == a.oStowageDet?.idMarca).FirstOrDefault();
                        a.oExportador = oExportador.Where(p => p.id == a.idExportador).FirstOrDefault();
                        //a.oStowageTurno = BAN_Stowage_Plan_Turno.GetEntidad(a.idStowagePlanTurno);
                        a.oEstado = oEstado.Where(p => p.id == a.estado).FirstOrDefault();
                        //a.oConsignatario = BAN_Catalogo_Consignatario.GetConsignatario(a.idConsignatario);
                        a.oStowage_Plan_Aisv = oDetalleAisv.Where(p=> p.idStowageAisv == long.Parse(a.idStowageAisv.ToString()))?.FirstOrDefault();//BAN_Stowage_Plan_Aisv.GetEntidad(long.Parse(a.idStowageAisv.ToString()));
                        try
                        {
                            a.oUbicacion = oUbicaciones.Where(p=> p.id == a.idUbicacion).FirstOrDefault();
                            if (!(a.oUbicacion is null))
                            {
                                a.oUbicacion.oBloque = oBloque.Where(p => p.id == a.oUbicacion?.idBloque).FirstOrDefault();
                                a.oBloque = a.oUbicacion?.oBloque;
                                a.oUbicacion.oBloque.oBodega = oBodega.Where(p => p.id == a.oUbicacion?.idBodega).FirstOrDefault(); 
                                a.oBloque.oBodega = a.oUbicacion?.oBloque?.oBodega;
                                a.oUbicacion.oFila = oFila.Where(p=> p.id == a.oUbicacion?.idFila).FirstOrDefault();
                                a.oUbicacion.oAltura = oAltura.Where(p=> p.id ==a.oUbicacion?.idAltura).FirstOrDefault();
                                a.oUbicacion.oProfundidad = oProfundidad.Where(p => p.id == a.oUbicacion?.idProfundidad).FirstOrDefault();
                            }
                            
                        }
                        catch { }
                        
                    }
                    //ResultadoCab.oDetalle = ResultadoDet;
                    //ResultadoCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusionesST(ResultadoCab.idStowageCab, out OError);

                    //objStowageCab = ResultadoCab;
                    Session["Transaccion_BAN_Stowage_Movimiento" + this.hf_BrowserWindowName.Value] = ResultadoDet;

                    //##########################################
                    // Asignar los datos agrupados al Repeater
                    //##########################################

                    // Calcular y mostrar el total de "BOXES"
                     int? totalBoxes = ResultadoDet.Where(Tbl => Tbl.active && Tbl.estado == "CON" && Tbl.idExportador == int.Parse(cmbExportador.SelectedValue) && Tbl.oStowage_Plan_Aisv?.booking == (string.IsNullOrEmpty(txtBookingFiltro.Text) ? Tbl.oStowage_Plan_Aisv?.booking : txtBookingFiltro.Text)).Sum(group => group.cantidad);
                    Session["Transaccion_BAN_Stowage_Roleos_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

                    //if (datosAgrupados != null && datosAgrupados.Count() > 0)
                    //{
                    //    dgvTotales.DataSource = datosAgrupados;
                    //    dgvTotales.DataBind();
                    //}
                    //else
                    //{
                    //    dgvTotales.DataSource = null;
                    //    dgvTotales.DataBind();
                    //}

                    //##########################################
                    // Asignar los datos grabados al Repeater
                    //##########################################
                    var LinqQuery = from Tbl in ResultadoDet.Where(Tbl => Tbl.active && Tbl.estado == "CON" && Tbl.idExportador == int.Parse(cmbExportador.SelectedValue) && Tbl.oStowage_Plan_Aisv?.booking == (string.IsNullOrEmpty(txtBookingFiltro.Text)? Tbl.oStowage_Plan_Aisv?.booking : txtBookingFiltro.Text) )
                                    orderby Tbl.barcode//, Tbl.fechaCreacion // Ordena por barcode y fecha
                                    select new
                                    {
                                        id = Tbl.idMovimiento,
                                        box = Tbl.cantidad,
                                        barcode = Tbl.barcode,
                                        idHold = Tbl.oUbicacion?.idBodega,
                                        Hold = Tbl.oUbicacion?.oBloque?.oBodega?.nombre,
                                        fecha = Tbl.fechaCreacion?.ToString("yyyy-MM-dd HH:mm"),
                                        marca = Tbl.oStowageDet?.oMarca?.nombre.Trim(),
                                        exportador = Tbl.oExportador?.nombre.Trim(),
                                        tipo = Tbl.tipo.Trim(),
                                        bodega = Tbl.oUbicacion?.oBloque?.oBodega?.nombre + " - " + Tbl.oBloque?.nombre + " - " + Tbl.oUbicacion?.oFila?.descripcion + " - " + Tbl.oUbicacion?.oAltura?.descripcion + " - " + Tbl.oUbicacion?.oProfundidad?.descripcion,
                                        bloque = Tbl.oUbicacion?.oBloque?.nombre,
                                        estado = Tbl.oEstado?.nombre,
                                        usuarioCrea = Tbl.usuarioCrea.Trim(),
                                        usuarioModifica = Tbl.usuarioModifica?.Trim(),
                                        fechaModifica = Tbl.fechaModifica?.ToString("yyyy-MM-dd HH:mm"),
                                        usuarioDespacho = Tbl.usuarioDespacho?.Trim(),
                                        fechaDespacho = Tbl.fechaDespacho?.ToString("yyyy-MM-dd HH:mm"),
                                        idOrdenDespacho = Tbl.idOrdenDespacho,
                                        bookings = Tbl.oStowage_Plan_Aisv?.booking,
                                        check = true
                                    };


                    try
                    {
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            tablePagination.DataSource = LinqQuery;
                            tablePagination.DataBind();

                            // Convertir los resultados de LINQ a una lista
                            var dataList = LinqQuery.ToList();

                            // Crear un DataTable y agregar las columnas correspondientes
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("id", typeof(int));
                            dataTable.Columns.Add("box", typeof(decimal));
                            dataTable.Columns.Add("barcode", typeof(string));
                            dataTable.Columns.Add("idHold", typeof(int));
                            dataTable.Columns.Add("Hold", typeof(string));
                            dataTable.Columns.Add("fecha", typeof(string));
                            dataTable.Columns.Add("marca", typeof(string));
                            dataTable.Columns.Add("exportador", typeof(string));
                            dataTable.Columns.Add("tipo", typeof(string));
                            dataTable.Columns.Add("bodega", typeof(string));
                            dataTable.Columns.Add("bloque", typeof(string));
                            dataTable.Columns.Add("estado", typeof(string));
                            dataTable.Columns.Add("usuarioCrea", typeof(string));
                            dataTable.Columns.Add("usuarioModifica", typeof(string));
                            dataTable.Columns.Add("fechaModifica", typeof(string));
                            dataTable.Columns.Add("usuarioDespacho", typeof(string));
                            dataTable.Columns.Add("fechaDespacho", typeof(string));
                            dataTable.Columns.Add("idOrdenDespacho", typeof(int));
                            dataTable.Columns.Add("bookings", typeof(string));
                            dataTable.Columns.Add("check", typeof(bool));

                            // Llenar el DataTable con los datos
                            foreach (var item in dataList)
                            {
                                dataTable.Rows.Add(
                                    item.id,
                                    item.box,
                                    item.barcode,
                                    item.idHold,
                                    item.Hold,
                                    item.fecha,
                                    item.marca,
                                    item.exportador,
                                    item.tipo,
                                    item.bodega,
                                    item.bloque,
                                    item.estado,
                                    item.usuarioCrea,
                                    item.usuarioModifica,
                                    item.fechaModifica,
                                    item.usuarioDespacho,
                                    item.fechaDespacho,
                                    item.idOrdenDespacho,
                                    item.bookings,
                                    true
                                );
                            }

                            // Crear un DataSet y agregar el DataTable
                            DataSet dataSet = new DataSet();
                            dataSet.Tables.Add(dataTable);
                            Session["Transaccion_BAN_DataParaRoleo" + this.hf_BrowserWindowName.Value] = dataSet;
                            Session["Transaccion_BAN_ListParaRoleo" + this.hf_BrowserWindowName.Value] = LinqQuery;

                            this.btnRolear.Attributes.Remove("disabled");
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                            sinresultado.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.Alerta(ex.Message);
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataStowage), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
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

                this.IsAllowAccess();
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
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    sinresultado.Visible = false;
                    LlenaComboLinea();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    this.btnRolear.Attributes["disabled"] = "disabled";
                }
                Ocultar_Mensaje();
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "Eventos"
        #region "Gridview Detalle"
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

                    if (e.CommandName == "Quitar")
                    {
                        DataSet DsResultados = Session["Transaccion_BAN_DataParaRoleo" + this.hf_BrowserWindowName.Value] as DataSet;
                        if (DsResultados == null)
                        {
                            try
                            {
                                var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                                ClsUsuario = ClsUsuario_;
                                btnBuscar_Click(null, null);
                                return;
                            }
                            catch
                            {
                                Response.Redirect("../login.aspx", false);
                                return;
                            }
                        }

                        if (DsResultados.Tables.Count > 0)
                        {
                           // DsResultados.Tables[0].Select(p=> p.id)
                        }


                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }

        protected void tablePagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                int totalBoxes = 0;
                totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_Roleos_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }
          
        }
        
        #endregion
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnRolear.Attributes["disabled"] = "disabled";
            Limpia_Datos_cliente();
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            Actualiza_Paneles();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.btnRolear.Attributes["disabled"] = "disabled";
            sinresultado.Visible = false;

            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            Actualiza_Paneles();
            if (Response.IsClientConnected)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.txtNave.Text))
                    {
                        this.OcultarLoading("1");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.txtNave.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.cmbLinea.SelectedValue))
                    {
                        this.OcultarLoading("1");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la linea"));
                        return;
                    }

                    if (cmbLinea.SelectedValue.Equals("0"))
                    {
                        this.OcultarLoading("1");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la linea"));
                        return;
                    }

                    if (string.IsNullOrEmpty(this.cmbExportador.SelectedValue))
                    {
                        this.OcultarLoading("1");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione el exportador"));
                        this.txtNave.Focus();
                        return;
                    }
                    ValidarDatosNave();
                    ConsultarDataStowage();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscar_Click), "btnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }
        }
        protected void btnRolear_Click(object sender, EventArgs e)
        {
            this.btnRolear.Attributes["disabled"] = "disabled";
            UPRoleo.Update();
            //UPRoleo.Update();

            //System.Threading.Thread.Sleep(10000);

            try
            {
                if (Response.IsClientConnected)
                {
                    sinresultado.Visible = false;
                    Ocultar_Mensaje();
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
                    this.btnRolear.Attributes["disabled"] = "disabled";
                    Actualiza_Paneles();

                    DataSet DsResultados = Session["Transaccion_BAN_DataParaRoleo" + this.hf_BrowserWindowName.Value] as DataSet;
                    Session["Transaccion_BAN_DataParaRoleo" + this.hf_BrowserWindowName.Value] = null;
                    if (DsResultados != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("2");
                            this.btnRolear.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtNaveRoleo.Text))
                        {
                            this.Alerta("Ingrese el nombre de la nave.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre de la nave para el roleo"));
                            this.txtNave.Focus();
                            OcultarLoading("2");
                            this.btnRolear.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtBooking.Text))
                        {
                            this.Alerta("Ingrese el booking.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese el booking para el roleo."));
                            this.txtBooking.Focus();
                            OcultarLoading("2");
                            this.btnRolear.Attributes.Remove("disabled");
                            return;
                        }


                        if (DsResultados.Tables.Count > 0)
                        {
                            if (DsResultados.Tables[0].Rows.Count > 0)
                            {
                                DsResultados.DataSetName = "RESULTADO";
                                DsResultados.Tables[0].TableName = "ROLEO";
                                string v_xml = DsResultados.GetXml();
                                var resultado = BAN_Stowage_Movimiento.Save_Roleo(txtNaveRoleo.Text, txtDescripcionNaveRoleo.Text, txtBooking.Text, v_xml, ClsUsuario.loginname, out OError);

                                if (OError != string.Empty)
                                {
                                    this.Alerta(OError);
                                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                    this.TXTMRN.Focus();
                                    this.btnRolear.Attributes.Remove("disabled");
                                    return;
                                }
                                else
                                {
                                    btnBuscar_Click(null, null);
                                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-info'></i><b> Informativo! </b>{0}", "Transacción ejecutada con éxito"));
                                    this.Alerta("Transacción exitosa");
                                }
                            }
                        }
                    }
                    else
                    {
                        btnBuscar_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnRolear_Click), "btnGrabar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
            this.btnRolear.Attributes.Remove("disabled");
            this.Actualiza_Paneles();
        }

        protected void cmbLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaComboExportador();
                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";
                cmbExportador.Items.Add(item);
                cmbExportador.SelectedValue = "0";
            }
            catch
            {

            }
        }

        #endregion

        protected void CHKPRO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // Obtener el checkbox que disparó el evento
                CheckBox chk = sender as CheckBox;
                if (chk != null)
                {
                    // Obtener el contenedor del elemento actual (Item del Repeater)
                    RepeaterItem item = chk.NamingContainer as RepeaterItem;

                    if (item != null)
                    {
                        // Buscar el control Label "lblId" dentro del RepeaterItem
                        Label lblId = item.FindControl("lblId") as Label;

                        if (lblId != null)
                        {
                            // Obtener el ID del elemento
                            int id = int.Parse(lblId.Text);

                            // Obtener el estado del checkbox
                            bool isChecked = chk.Checked;

                            // Actualizar el servidor con el nuevo estado
                            ActualizarEstadoCheckbox(id, isChecked);
                            //this.Actualiza_Paneles();
                            // Mensaje opcional para confirmar
                            //this.Mostrar_Mensaje($"El estado del ID {id} ha sido actualizado a {(isChecked ? "Seleccionado" : "No Seleccionado")}.");
                        }
                    }
                }
                txtNave.Text = "11111";
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje($"Error al actualizar el estado: {ex.Message}");
            }
        }

        // Método para realizar la actualización en la base de datos o lógica de negocio
        private void ActualizarEstadoCheckbox(int id, bool isChecked)
        {
            try
            {
                var dataSet = Session["Transaccion_BAN_DataParaRoleo" + this.hf_BrowserWindowName.Value] as DataSet ;
                var lstData = Session["Transaccion_BAN_ListParaRoleo" + this.hf_BrowserWindowName.Value];

                string filtro = string.Format("id={0}", id);
                var dr = dataSet.Tables[0].Select(filtro).FirstOrDefault();
                dr["check"] = isChecked;

                Session["Transaccion_BAN_DataParaRoleo" + this.hf_BrowserWindowName.Value] = dataSet;

                return;
                // Lógica para actualizar el estado del checkbox en el servidor
                // Ejemplo: Llamar a una capa de servicio o directamente a la base de datos
                //BAN_Stowage_Movimiento.ActualizarEstadoCheckbox(id, isChecked, out string error);

                //if (!string.IsNullOrEmpty(error))
                //{
                //    throw new Exception(error);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo actualizar el estado del checkbox. {ex.Message}");
            }
        }

    }
}
