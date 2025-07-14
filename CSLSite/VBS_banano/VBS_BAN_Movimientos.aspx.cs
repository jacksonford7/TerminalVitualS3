using BillionEntidades;
using BreakBulk;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VBSEntidades.Banano;
using VBSEntidades.BananoBodega;
using VBSEntidades.BananoMuelle;

namespace CSLSite
{
    public partial class VBS_BAN_Movimientos : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private BAN_Stowage_Plan_Cab objStowageCab = new BAN_Stowage_Plan_Cab();
        private List<BAN_Stowage_Plan_Det> objStowageDet = new List<BAN_Stowage_Plan_Det>();
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
            this.banmsg_det.Visible = true;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }
        private void Mostrar_MensajeDet(string Mensaje)
        {
            OcultarLoading("1");
            OcultarLoading("2");
        }
        private void Ocultar_Mensaje()
        {
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

            //cabecera de transaccion
            objStowageCab = new BAN_Stowage_Plan_Cab();
            objStowageDet = new List<BAN_Stowage_Plan_Det>();
            objStowageCab.oDetalle = objStowageDet;
            Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;
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
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboLinea), "VBS_BAN_PreStowage.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }        
        private void ConsultarDataStowage()
        {
            try
            {
                var a = int.Parse(cmbLinea.SelectedValue);
            }
            catch 
            {
                OError = "Por favor, seleccione la linea";
                this.Alerta(OError);
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                return;
            }

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
                    foreach (var a in ResultadoCab)
                    {
                        oDetalle.AddRange(BAN_Stowage_Plan_Det.ConsultarLista(a.idStowageCab, out OError));
                    }
                    
                    var oUbicaciones = BAN_Catalogo_Ubicacion.ConsultarLista(out OError);
                    var oFila = BAN_Catalogo_Fila.ConsultarLista(out OError);
                    var oAltura = BAN_Catalogo_Altura.ConsultarLista(out OError);
                    var oProfundidad = BAN_Catalogo_Profundidad.ConsultarLista(out OError);

                    foreach (var a in ResultadoDet)
                    {
                        a.oStowageDet = oDetalle.Where(p=> p.idStowageDet == a.idStowageDet).FirstOrDefault();
                        a.idExportador = a.oStowageDet.idExportador;
                        a.oStowageDet.oMarca = oMarca.Where(p => p.id == a.oStowageDet?.idMarca).FirstOrDefault();
                        a.oExportador = oExportador.Where(p => p.id == a.idExportador).FirstOrDefault();
                        a.oEstado = oEstado.Where(p => p.id == a.estado).FirstOrDefault();
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

                    Session["Transaccion_BAN_Stowage_Movimiento" + this.hf_BrowserWindowName.Value] = ResultadoDet;

                    //##########################################
                    // Asignar los datos grabados al Repeater
                    //##########################################
                    var LinqQuery = from Tbl in ResultadoDet.Where(Tbl => Tbl.active)
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
                                        usuarioPreDespacho = Tbl.usuarioPreDespacho?.Trim(),
                                        fechaPreDespacho = Tbl.fechaPreDespacho?.ToString("yyyy-MM-dd HH:mm"),
                                        usuarioDespacho = Tbl.usuarioDespacho?.Trim(),
                                        fechaDespacho = Tbl.fechaDespacho?.ToString("yyyy-MM-dd HH:mm"),
                                        idOrdenDespacho = Tbl.idOrdenDespacho
                                    };


                    try
                    {
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            tablePagination.DataSource = LinqQuery;
                            tablePagination.DataBind();

                            //DataTable resultDataTable = ConvertToDataTable(LinqQuery);
                            //Session["Transaccion_ReporteExportar" + this.hf_BrowserWindowName.Value] = resultDataTable;
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
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

        public static DataTable ConvertToDataTable<T>(IEnumerable<T> query)
        {
            DataTable dataTable = new DataTable();

            // Obtiene las propiedades del tipo T.
            var properties = typeof(T).GetProperties();

            // Crea las columnas en el DataTable con base en las propiedades.
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Agrega las filas con los valores de las propiedades.
            foreach (var item in query)
            {
                var values = properties.Select(prop => prop.GetValue(item, null)).ToArray();
                dataTable.Rows.Add(values);
            }

            return dataTable;
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
        private void FiltrarDatosSegundoDropdown(int idCarga)
        {
            try
            {
                List<BAN_HorarioFinal> ListadoFiltrado = BAN_HorarioFinal.ConsultarHorarioFinal(out cMensajes);

                var list = ListadoFiltrado.Where(x => x.Id_HorarioIni == idCarga).ToList();

                // Crear una cadena de texto con los elementos del DropDownList
                StringBuilder sb = new StringBuilder();
                foreach (var item in list)
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id_Hora, item.Desc_Hora);
                }

                // Generar la respuesta como cadena de texto
                string response = sb.ToString();

                // Enviar la respuesta al cliente
                Response.Write(response);
                Response.End();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "FiltrarDatosSegundoDropdown", "Hubo un error al filtrar datos", t.loginname));
                this.Mostrar_Mensaje(Error);
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
            //if (Response.IsClientConnected)
            //{
            //    try
            //    {
            //        if (HttpContext.Current.Request.Cookies["token"] == null)
            //        {
            //            System.Web.Security.FormsAuthentication.SignOut();
            //            Session.Clear();
            //            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            //            return;
            //        }

            //        //var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

            //        if (e.CommandArgument == null)
            //        {
            //            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
            //            return;
            //        }

            //        Int64 id;
            //        var t = e.CommandArgument.ToString();
            //        if (!Int64.TryParse(t, out id))
            //        {
            //            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
            //            return;
            //        }

            //        if (e.CommandName == "Editar")
            //        {
            //            objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
            //            if (objStowageCab == null)
            //            {
            //                try
            //                {
            //                    var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            //                    ClsUsuario = ClsUsuario_;
            //                    btnBuscar_Click(null, null);
            //                }
            //                catch
            //                {
            //                    Response.Redirect("../login.aspx", false);
            //                    return;
            //                }
            //            }

            //            var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(id.ToString()));
            //            var oConsignatario = BAN_Catalogo_Consignatario.GetConsignatario(oDet.idConsignatario);
            //            var oExportador = BAN_Catalogo_Exportador.GetExportaador(oDet.idExportador);
            //            var oMarca = BAN_Catalogo_Marca.GetMarca(oDet.idMarca);

            //            hdf_CodigoCab.Value = objStowageCab.idStowageCab.ToString();
            //            hdf_CodigoDet.Value = id.ToString();
            //            txtDetPiso.Text = oDet.piso;
            //            cmbDetHold.SelectedValue = oDet.idHold.ToString();
            //            txtDetCantidad.Text = oDet.boxSolicitado.ToString();
            //            cmbDetCargo.SelectedValue = oDet.idCargo.ToString();
            //            txtDetConsignatario.Text = oConsignatario?.nombre;
            //            txtDetExportador.Text= oExportador?.nombre;
            //            txtDetMarca.Text = oMarca?.nombre;
            //            txtDetPiso.Text = oDet.piso.ToString();
            //            txtDetobservacion.Text = oDet.comentario.ToString();
            //            msjErrorDetalle.Visible = false;
            //            this.btnActualizar.Attributes.Remove("disabled");
            //            UPEDIT.Update();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
            //        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
            //        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            //        return;
            //    }
            //}
        }

        protected void tablePagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Footer)
            //{
            //    int totalBoxes = 0;
            //    totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_Plan_Cab_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

            //    // Asigna el total al Label en el FooterTemplate
            //    Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
            //    lblTotalBoxes.Text = totalBoxes.ToString();
            //}

            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    int holdValue = int.Parse(DataBinder.Eval(e.Item.DataItem, "idHold").ToString());
            //    Label lblHold = e.Item.FindControl("lblHold") as Label;
            //    Button btnServicioDet = e.Item.FindControl("btnServicios") as Button;
            //    Button btnPublica = e.Item.FindControl("btnPublicar") as Button;

            //    lblHold.Font.Bold = true;

            //    if (holdValue % 2 == 0)
            //    {
            //        lblHold.ForeColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        lblHold.ForeColor = System.Drawing.Color.Blue;
            //    }

            //    if (Txtruc.Text != "0992506717001") //ruc de cgsa
            //    {
            //        btnServicioDet.Visible = false;
            //    }

            //    //#######################################################################
            //    // VALIDACION DE QUE SI HAY AISV GENERADOS NO PERMITA EDITAR NI ELIMINAR
            //    //#######################################################################
            //    Button btnEdicion = e.Item.FindControl("btnEditar") as Button;
            //    Button btnTurno = e.Item.FindControl("btnDetalle") as Button;

            //    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

            //    int v_id = int.Parse(DataBinder.Eval(e.Item.DataItem, "id").ToString());
            //    var oDetalle = objStowageCab.oDetalle.Where(p => p.idStowageDet == v_id).FirstOrDefault();

            //    if (string.IsNullOrEmpty(oDetalle.oBloque?.nombre))
            //    {
            //        btnTurno.Enabled = false;
            //    }

            //    if (oDetalle?.reservado == 0 || oDetalle?.reservado is null)
            //    {
            //        btnPublica.Enabled = false;
            //    }

            //    if (oDetalle.estado == "PBL")
            //    {
            //        btnPublica.Enabled = false;
            //    }

            //    this.Actualiza_Panele_Detalle();
            //}
        }
        
        #endregion
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled";
            Limpia_Datos_cliente();
            //objDetalle.Clear();
            objStowageCab = new BAN_Stowage_Plan_Cab();
            objStowageDet = new List<BAN_Stowage_Plan_Det>();
            objStowageCab.oDetalle = objStowageDet;
            Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            //dgvTotales.DataSource = null;
            //dgvTotales.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            sinresultado.Visible = false;
            ValidarDatosNave();

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

                    //if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    //{
                    //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>MRN no encontrado"));
                    //    this.TXTMRN.Focus();
                    //    return;
                    //}

                    //if (string.IsNullOrEmpty(this.fecETA.Text))
                    //{
                    //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Valor de ETA no válido"));
                    //    this.fecETA.Focus();
                    //    return;
                    //}
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

        protected void btnExportador_Click(object sender, EventArgs e)
        {
            //sinresultado.Visible = false;
            //Actualiza_Paneles();
            ////ValidarDatosNave();

            //tablePagination.DataSource = null;
            //tablePagination.DataBind();
            //this.Ocultar_Mensaje();
            //UPDETALLE.Update();
            if (Response.IsClientConnected)
            {
                try
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

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

                    if (ClsUsuario != null)
                    {
                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                    }

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    if (string.IsNullOrEmpty(hdf_txtNave.Value))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.txtNave.Focus();
                        return;
                    }

                    var ExistenRegistros = BAN_Stowage_Movimiento.ConsultarListaReporte(hdf_txtNave.Value, int.Parse(cmbLinea.SelectedValue), out OError);//Session["Transaccion_ReporteExportar" + this.hf_BrowserWindowName.Value] as DataTable; 
                   // var ExistenRegistros = Session["Transaccion_ReporteExportar" + this.hf_BrowserWindowName.Value] as DataTable; 
                    if (ExistenRegistros == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }

                    if (ExistenRegistros.Rows.Count <= 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);

                        //DataTable dt;
                        string v_nameTable = "Reporte_movimientos";
                        ExistenRegistros.TableName = v_nameTable;

                        //this.UPDETALLE.Update();
                        try
                        {
                            using (ExcelPackage xp = new ExcelPackage())
                            {
                                {
                                    ExcelWorksheet ws = xp.Workbook.Worksheets.Add(ExistenRegistros.TableName);
                                    int rowstart = 2;
                                    int colstart = 2;
                                    int rowend = rowstart;
                                    int colend = colstart + ExistenRegistros.Columns.Count;

                                    ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                                    ws.Cells[rowstart, colstart, rowend, colend].Value = ExistenRegistros.TableName;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                                    rowstart += 2;
                                    rowend = rowstart + ExistenRegistros.Rows.Count;
                                    ws.Cells[rowstart, colstart].LoadFromDataTable(ExistenRegistros, true);
                                    int i = 1;
                                    foreach (DataColumn dc in ExistenRegistros.Columns)
                                    {
                                        i++;
                                        if (dc.DataType == typeof(decimal))
                                        {
                                            ws.Column(i).Style.Numberformat.Format = "#0.00";
                                        }
                                        if (dc.DataType == typeof(DateTime))
                                        {
                                            ws.Column(i).Style.Numberformat.Format = "dd-MM-yyyy HH:mm";
                                        }
                                    }
                                    ws.Cells[ws.Dimension.Address].AutoFitColumns();



                                    ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                                       ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                                       ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                                       ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                }
                                Response.AddHeader("content-disposition", "attachment;filename=" + v_nameTable + ".xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.BinaryWrite(xp.GetAsByteArray());
                                //Response.Flush();
                                //Response.End();

                                HttpContext.Current.Response.Flush();
                                HttpContext.Current.Response.SuppressContent = true;
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }

                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
                            string eRROR = ex.Message.ToString();
                            this.Actualiza_Paneles();
                        }

                        this.Ocultar_Mensaje();
                        this.Actualiza_Paneles();
                    }

                    this.Actualiza_Paneles();
                    this.Ocultar_Mensaje();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscar_Click), "btnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }
        }

        protected void cmbLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnBuscar_Click(null, null);
            }
            catch
            {

            }
        }
        
        #endregion

    }
}
