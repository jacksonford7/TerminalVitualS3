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
    public partial class VBS_BAN_OrdenDespacho : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private BAN_Stowage_OrdenDespacho objOrdenDespacho = new BAN_Stowage_OrdenDespacho();
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
            UPEDIT.Update();
        }
        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            Actualiza_Paneles();
        }
        private void Limpia_Datos_cliente()
        {
            this.TXTMRN.Text = string.Empty;
            this.txtFechaDesde.Text = string.Empty;
            this.txtFechaHasta.Text = string.Empty;
            this.txtNave.Text = string.Empty;
            this.txtDescripcionNave.Text = string.Empty;
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
        private void Mostrar_Mensaje(int tipo, string Mensaje)
        {
            if(tipo == 1 ) // nueva orden
            {
                this.banmsg_det.Visible = true;
                //this.banmsg.Visible = true;
                //this.banmsg.InnerHtml = Mensaje;
                this.banmsg_det.InnerHtml = Mensaje;
            }
            else
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
            }

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
                this.Mostrar_Mensaje(1,string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objOrdenDespacho = new BAN_Stowage_OrdenDespacho();
            Session["Transaccion_BAN_Stowage_OrdenDespacho" + this.hf_BrowserWindowName.Value] = objOrdenDespacho;
        }
        public void LlenaComboLinea()
        {
            try
            {
                var oEntidad = BAN_Catalogo_Linea.ConsultarListaLlenaCombo(Txtruc.Text); //ds.Tables[0].DefaultView;
                cmbLinea.DataSource = oEntidad;
                cmbLinea.DataValueField = "ID";
                cmbLinea.DataTextField = "nombre";
                cmbLinea.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboLinea), "VBS_BAN_OrdenDespacho.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
  
        public void LlenaComboExportador()
        {
            try
            {
                string oError;
                var oLinea = BAN_Catalogo_Linea.GetLinea(int.Parse(cmbLinea.SelectedValue));
                var oEntidad = BAN_Catalogo_Exportador.ConsultarListaExportador(oLinea.ruc, out oError); //ds.Tables[0].DefaultView;
                cmbExportador.DataSource = oEntidad;
                cmbExportador.DataValueField = "ID";
                cmbExportador.DataTextField = "nombre";
                cmbExportador.DataBind();

                cmbDetExportador.DataSource = oEntidad;
                cmbDetExportador.DataValueField = "ID";
                cmbDetExportador.DataTextField = "nombre";
                cmbDetExportador.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboExportador), "VBS_BAN_OrdenDespacho.LlenaComboExportador", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboMarcas()
        {
            try
            {
                string oError;
                var oLinea = BAN_Catalogo_Linea.GetLinea(int.Parse(cmbLinea.SelectedValue));
                var oEntidad = BAN_Catalogo_Marca.ConsultarListaMarca(oLinea.ruc, out oError); //ds.Tables[0].DefaultView;
                cmbMarca.DataSource = oEntidad;
                cmbMarca.DataValueField = "ID";
                cmbMarca.DataTextField = "nombre";
                cmbMarca.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboMarcas), "VBS_BAN_PreStowage.LlenaComboMarcas", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboEstado()
        {
            try
            {
                string oError;
                var oEstado = BAN_Catalogo_Estado.ConsultarLista(out oError);
                cmbDetEstado.DataSource = oEstado;
                cmbDetEstado.DataValueField = "ID";
                cmbDetEstado.DataTextField = "nombre";
                cmbDetEstado.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "VBS_BAN_OrdenDespacho.LlenaComboEstado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboFila()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Fila.ConsultarLista(out oError);
                cmbFilaDesde.DataSource = oEntidad;
                cmbFilaDesde.DataValueField = "ID";
                cmbFilaDesde.DataTextField = "descripcion";
                cmbFilaDesde.DataBind();

                cmbFilaHasta.DataSource = oEntidad;
                cmbFilaHasta.DataValueField = "ID";
                cmbFilaHasta.DataTextField = "descripcion";
                cmbFilaHasta.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "VBS_BAN_OrdenDespacho.LlenaComboEstado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboAltura()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Altura.ConsultarLista(out oError);
                cmbAltura.DataSource = oEntidad;
                cmbAltura.DataValueField = "ID";
                cmbAltura.DataTextField = "descripcion";
                cmbAltura.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "VBS_BAN_OrdenDespacho.LlenaComboEstado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboBodega()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Bodega.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbDetBodega.DataSource = oEntidad;
                cmbDetBodega.DataValueField = "id";
                cmbDetBodega.DataTextField = "nombre";
                cmbDetBodega.DataBind();

                cmbBodega.DataSource = oEntidad;
                cmbBodega.DataValueField = "id";
                cmbBodega.DataTextField = "nombre";
                cmbBodega.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void ConsultarDataStowage()
        {
            try
            {
                string desde = null;
                string hasta = null;
                long? idorden = null;
                if (string.IsNullOrEmpty(txtNumeroOrden.Text))
                {
                    if (string.IsNullOrEmpty(this.txtFechaDesde.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Por favor ingresar la fecha desde"));
                        this.txtNave.Focus();
                        return;
                    }

                    DateTime fechaDesde = new DateTime();
                    CultureInfo enUS = new CultureInfo("en-US");
                    if (!DateTime.TryParseExact(txtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechaDesde))
                    {
                        this.Alerta(string.Format("EL FORMATO DE FECHA DESDE DEBE SER Mes/dia/Anio {0}", txtFechaDesde.Text));
                        txtFechaDesde.Focus();
                        //this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Por favor ingresar la fecha hasta"));
                        this.txtNave.Focus();
                        return;
                    }

                    DateTime fechaHasta = new DateTime();

                    if (!DateTime.TryParseExact(txtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechaHasta))
                    {
                        this.Alerta(string.Format("EL FORMATO DE FECHA HASTA DEBE SER Mes/dia/Anio {0}", txtFechaHasta.Text));
                        txtFechaHasta.Focus();
                        //this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }
                    desde = fechaDesde.ToString("yyyyMMdd");
                    hasta = fechaHasta.ToString("yyyyMMdd");
                }
                else
                {
                    idorden = long.Parse(txtNumeroOrden.Text);
                }

                var ResultadoOrdenDespacho = BAN_Stowage_OrdenDespacho.ConsultarOrdenDespacho(desde, hasta, idorden, out OError);// BAN_Stowage_Plan_Cab.GetStowagePlanCabEspecifico(null, txtNave.Text, int.Parse(cmbLinea.SelectedValue), fechaDocumento);
                if (ResultadoOrdenDespacho != null)
                {
                    var oExportador = BAN_Catalogo_Exportador.ConsultarListaExportador("CGSA", out OError);
                    var oBodega = BAN_Catalogo_Bodega.ConsultarLista(out OError);
                    var oBloque = BAN_Catalogo_Bloque.ConsultarLista(null, out OError);
                    var oEstado = BAN_Catalogo_Estado.ConsultarLista(out OError);
                    
                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(2,string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                        return;
                    }


                    foreach (var a in ResultadoOrdenDespacho)
                    {
                        a.oExportador = oExportador.Where(p => p.id == a.idExportador).FirstOrDefault();
                        a.oBloque = oBloque.Where(p => p.id == a.idBloque).FirstOrDefault();
                        a.oBodega = oBodega.Where(p => p.id == int.Parse(a.idBodega.ToString())).FirstOrDefault();
                        a.oBloque.oBodega = a.oBodega;
                        a.oEstado = oEstado.Where(p => p.id == a.estado).FirstOrDefault();
                    }

                    //##########################################
                    // Asignar los datos agrupados al Repeater
                    //##########################################
                    var datosAgrupados = ResultadoOrdenDespacho.GroupBy(o => o.idNave)
                                                        .Select(group => new
                                                        {
                                                            Hold = group.Key,
                                                            TotalBoxes = group.Sum(o => o.cantidadPalets)
                                                        });


                    // Calcular y mostrar el total de "BOXES"
                    int? totalBoxes = datosAgrupados.Sum(group => group.TotalBoxes);
                    Session["Transaccion_BAN_Stowage_OrdenDespacho_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

                    //##########################################
                    // Asignar los datos grabados al Repeater
                    //##########################################
                    var LinqQuery = from Tbl in ResultadoOrdenDespacho.Where(Tbl => !String.IsNullOrEmpty(Tbl.usuarioCrea))
                                    select new
                                    {
                                        id = Tbl.idOrdenDespacho,
                                        //fecha = Tbl.fecha.ToString("dd/MM/yyyy"),
                                        //time = string.Format("{0} - {1}", Tbl.horaInicio.Trim(), Tbl.horaFin.Trim()),
                                        idNave = Tbl.idNave,
                                        exportador = Tbl.oExportador.nombre.Trim(),
                                        bodega = Tbl.oBodega?.nombre + " - " + Tbl.oBloque?.nombre,
                                        bloque = Tbl.oBloque?.nombre,
                                        estado = Tbl.oEstado?.nombre,
                                        box = Tbl.cantidadBox,
                                        pallets = Tbl.cantidadPalets,
                                        arrastre = Tbl.arrastre,
                                        pendiente = Tbl.pendiente,
                                        usuarioCrea = Tbl.usuarioCrea.Trim(),
                                        fechaCreacion = Tbl.fechaCreacion.ToString("dd/MM/yyyy HH:mm"),
                                        booking = Tbl.booking
                                    };

                    try
                    {
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
                    catch
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                    }
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                    sinresultado.Visible = true;
                }
                //this.btnGrabar.Attributes.Remove("disabled");
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataStowage), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(2,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }
        
        protected string jsarguments(object ID_CAB, object ID_DET)
        {
            return string.Format("{0};{1}", ID_CAB != null ? ID_CAB.ToString().Trim() : "0", ID_DET != null ? ID_DET.ToString().Trim() : "0");
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
                this.Mostrar_Mensaje(1, Error);
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
                this.msjErrorDetalle.Visible = IsPostBack;

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
                this.Mostrar_Mensaje(1,string.Format("<b>Error! </b>{0}", ex.Message));
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
                    LlenaComboBodega();
                    LlenaComboExportador();
                    LlenaComboEstado();
                    LlenaComboFila();
                    LlenaComboAltura();
                    LlenaComboMarcas();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";
                    
                    cmbExportador.Items.Add(item);
                    cmbBodega.Items.Add(item);
                    cmbFilaDesde.Items.Add(item);
                    cmbFilaHasta.Items.Add(item);
                    cmbAltura.Items.Add(item);
                    cmbMarca.Items.Add(item);
                    cmbDetBodega.Items.Add(item);
                    
                    cmbExportador.SelectedValue = "0";
                    cmbFilaDesde.SelectedValue = "0";
                    cmbFilaHasta.SelectedValue = "0";
                    cmbAltura.SelectedValue = "0";
                    cmbMarca.SelectedValue = "0";
                    cmbDetBodega.SelectedValue = "0";

                    cmbDetBodega_SelectedIndexChanged(null, null);
                   
                    if (Txtruc.Text != "0992506717001") //ruc de cgsa
                    {
                        this.cmbDetBodega.Attributes["disabled"] = "disabled";
                        this.cmbDetBloque.Attributes["disabled"] = "disabled";
                    }
                }
                Ocultar_Mensaje();
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1,string.Format("<b>Error! </b>{0}", ex.Message));
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

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    long v_ID = long.Parse(e.CommandArgument.ToString());
                    if (v_ID <= 0) { return; }

                    var oOrdenDespacho = BAN_Stowage_OrdenDespacho.GetEntidad(long.Parse(id.ToString()));
                    Session["Transaccion_BAN_Stowage_OrdenDespacho" + this.hf_BrowserWindowName.Value] = oOrdenDespacho;

                    if (oOrdenDespacho is null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen información para mostrar"));
                        return;
                    }

                    if (e.CommandName == "Editar")
                    {
                        txtDetNumeroOrden.Text = "0";
                        txtDetReferencia.Text = string.Empty;
                        txtDetBooking.Text = string.Empty;
                        txtDetCantidad.Text = "0";
                        sinresultadosDet.Visible = true;
                        tablePaginationDetalle.DataSource = null;
                        UPEDIT.Update();


                        if (oOrdenDespacho == null) {
                            try
                            {
                                var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                                ClsUsuario = ClsUsuario_;
                                btnBuscar_Click(null,null);
                            }
                            catch
                            {
                                Response.Redirect("../login.aspx", false);
                                return;
                            }
                        }

                        hdf_CodigoCab.Value = oOrdenDespacho.idOrdenDespacho.ToString();
                        txtDetNumeroOrden.Text = oOrdenDespacho.idOrdenDespacho.ToString();
                        txtDetReferencia.Text = oOrdenDespacho.idNave;
                        txtDetBooking.Text = oOrdenDespacho.booking;
                        try { cmbDetExportador.SelectedValue = oOrdenDespacho.idExportador.ToString(); } catch { }
                        try { cmbDetBodega.SelectedValue = oOrdenDespacho.idBodega.ToString(); } catch { cmbDetBodega.SelectedValue = "0"; }
                        txtDetCantidad.Text = oOrdenDespacho.cantidadPalets.ToString();
                        cmbDetEstado.SelectedValue = oOrdenDespacho.estado;
                        cmbDetBodega_SelectedIndexChanged(null,null);
                        try { cmbDetBloque.SelectedValue = oOrdenDespacho.idBloque.ToString(); } catch { cmbDetBodega.SelectedValue = null; }
                        msjErrorDetalle.Visible = false;
                        if (oOrdenDespacho.estado == "NUE")
                        {
                            this.btnActualizar.Attributes.Remove("disabled");
                        }

                        var ResultadoMovimiento = BAN_Stowage_Movimiento.ConsultarMovimientosXOrdenDespacho(long.Parse(oOrdenDespacho.idOrdenDespacho.ToString()), out OError);// BAN_Stowage_Plan_Cab.GetStowagePlanCabEspecifico(null, txtNave.Text, int.Parse(cmbLinea.SelectedValue), fechaDocumento);
                        if (ResultadoMovimiento != null)
                        {
                            var oExportador = BAN_Catalogo_Exportador.ConsultarListaExportador("CGSA", out OError);
                            var oBodega = BAN_Catalogo_Bodega.ConsultarLista(out OError);
                            var oBloque = BAN_Catalogo_Bloque.ConsultarLista(null, out OError);
                            var oEstado = BAN_Catalogo_Estado.ConsultarLista(out OError);
                            var oUbicacion = BAN_Catalogo_Ubicacion.ConsultarLista(out OError);
                            var oFila = BAN_Catalogo_Fila.ConsultarLista(out OError);
                            var oAltura = BAN_Catalogo_Altura.ConsultarLista(out OError);
                            var oSlot = BAN_Catalogo_Profundidad.ConsultarLista(out OError);
                            var oModalidad = BAN_Catalogo_Modalidad.ConsultarLista(out OError);
                            if (OError != string.Empty)
                            {
                                this.Alerta(OError);
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                this.TXTMRN.Focus();
                                return;
                            }

                            foreach (var a in ResultadoMovimiento)
                            {
                                //a.oExportador = oExportador.Where(p => p.id == a.idExportador).FirstOrDefault();
                                a.oUbicacion = oUbicacion.Where(p => p.id == a.idUbicacion).FirstOrDefault();
                                try { a.oUbicacion.oBloque = oBloque.Where(p => p.id == a.oUbicacion.idBloque).FirstOrDefault(); a.oBloque = a.oUbicacion.oBloque; } catch { }
                                try { a.oUbicacion.oBloque.oBodega = oBodega.Where(p => p.id == a.oUbicacion.idBodega).FirstOrDefault(); a.oBloque.oBodega = a.oUbicacion.oBloque.oBodega; } catch { }
                                a.oModalidad = oModalidad.Where(p => p.id == a.idModalidad).FirstOrDefault();
                                try { a.oUbicacion.oFila = oFila.Where(p => p.id == a.oUbicacion.idFila).FirstOrDefault(); } catch { }
                                try { a.oUbicacion.oAltura = oAltura.Where(p => p.id == a.oUbicacion.idAltura).FirstOrDefault(); } catch { }
                                try { a.oUbicacion.oProfundidad = oSlot.Where(p => p.id == a.oUbicacion.idProfundidad).FirstOrDefault(); } catch { }
                                a.oEstado = oEstado.Where(p => p.id == a.estado).FirstOrDefault();
                                a.oModalidad = oModalidad.Where(p => p.id == a.idModalidad).FirstOrDefault();
                            }

                            //##########################################
                            // Asignar los datos agrupados al Repeater
                            //##########################################
                            var datosAgrupados = ResultadoMovimiento.GroupBy(o => o.idExportador)
                                                                .Select(group => new
                                                                {
                                                                    Hold = group.Key,
                                                                    TotalBoxes = group.Sum(o => o.cantidad)
                                                                });


                            // Calcular y mostrar el total de "BOXES"
                            int? totalBoxes = datosAgrupados.Sum(group => group.TotalBoxes);
                            Session["Transaccion_BAN_Stowage_Movimiento_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

                            //##########################################
                            // Asignar los datos grabados al Repeater
                            //##########################################
                            var LinqQuery = from Tbl in ResultadoMovimiento.Where(Tbl => !String.IsNullOrEmpty(Tbl.usuarioCrea))
                                            select new
                                            {
                                                id = Tbl.idMovimiento,
                                                idOrden = Tbl.idOrdenDespacho,
                                                idUbicacion = Tbl.idUbicacion,
                                                barcodes = Tbl.barcode.Trim(),
                                                bodega = Tbl.oBloque?.oBodega?.nombre + " - " + Tbl.oBloque?.nombre,
                                                bloque = Tbl.oBloque?.nombre,
                                                fila = Tbl.oUbicacion?.oFila?.descripcion,
                                                altura = Tbl.oUbicacion?.oAltura?.descripcion,
                                                slot = Tbl.oUbicacion?.oProfundidad?.descripcion,
                                                estado = Tbl.oEstado?.nombre,
                                                box = Tbl.cantidad,
                                                usuarioCrea = Tbl.usuarioCrea.Trim(),
                                                usuarioPreDespacho = Tbl.usuarioPreDespacho?.Trim(),
                                                usuarioDespacho = Tbl.usuarioDespacho?.Trim(),
                                                isMix = Tbl.isMix,
                                                fechaCreacion = Tbl.fechaCreacion?.ToString("dd/MM/yyyy HH:mm"),
                                                fechaPreDespacho = Tbl.fechaPreDespacho?.ToString("dd/MM/yyyy HH:mm"),
                                                fechaDespacho = Tbl.fechaDespacho?.ToString("dd/MM/yyyy HH:mm")
                                            };

                            try
                            {
                                if (LinqQuery != null && LinqQuery.Count() > 0)
                                {
                                    tablePaginationDetalle.DataSource = LinqQuery;
                                    tablePaginationDetalle.DataBind();
                                    sinresultadosDet.Visible = false;
                                }
                                else
                                {
                                    tablePaginationDetalle.DataSource = null;
                                    tablePaginationDetalle.DataBind();
                                }
                            }
                            catch
                            {
                                tablePaginationDetalle.DataSource = null;
                                tablePaginationDetalle.DataBind();
                            }
                        }
                        else
                        {
                            tablePaginationDetalle.DataSource = null;
                            tablePaginationDetalle.DataBind();
                        }

                        UPEDIT.Update();
                    }

                    if (e.CommandName == "Quitar")
                    {
                        Ocultar_Mensaje();
                        msjErrorDetalle.Visible = false;

                        if (oOrdenDespacho.estado != "NUE")
                        {
                            Mostrar_MensajeDet("No se puede anular orden seleccionada, ya existen ");
                            return;
                        }
                       
                        Actualiza_Paneles();
                    }

                    
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "Editar", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }

        protected void tablePagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                int totalBoxes = 0;
                totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_OrdenDespacho_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //#######################################################################
                // VALIDACION DE QUE SI HAY AISV GENERADOS NO PERMITA EDITAR NI ELIMINAR
                //#######################################################################
                Button btnEdicion = e.Item.FindControl("btnEditar") as Button;
                Button btnbtnQuitar = e.Item.FindControl("btnQuitar") as Button;
                //Label lblAisv = e.Item.FindControl("lblAisv") as Label;

               /* objStowageCab = Session["Transaccion_BAN_Stowage_OrdenDespacho" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

                int v_id = int.Parse(DataBinder.Eval(e.Item.DataItem, "id").ToString());

                var oDetalle = objStowageCab.oDetalle.Where(p => p.idStowageDet == v_id).FirstOrDefault();

                if (oDetalle.ListaAISV.Count > 0)
                {
                    btnEdicion.Enabled = false;
                    btnbtnQuitar.Enabled = false;
                    //lblAisv.Text = string.Empty;
                    //foreach (var item in oDetalle.ListaAISV)
                    //{
                    //    lblAisv.Text = lblAisv.Text + "  " + item.aisv;
                    //}
                }
                */
                this.Actualiza_Panele_Detalle();
            }
        }

      
        #endregion
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes.Remove("disabled");
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
                    ConsultarDataStowage();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscar_Click), "btnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
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

                    this.btnGrabar.Attributes["disabled"] = "disabled";
                    UPDETALLE.Update();

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtNave.Text))
                    {
                        this.Alerta("Ingrese el nombre de la nave.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre de la nave"));
                        this.txtNave.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtBox.Text) || (txtBox.Text == "0"))
                    {
                        this.Alerta("Ingrese la cantidad.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese la cantidad."));
                        this.txtBox.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbLinea.Text))
                    {
                        this.Alerta("Seleccione una linea.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione la linea."));
                        cmbBloque.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbBodega.SelectedValue) || (cmbBodega.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione una bodega.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una bodega."));
                        cmbBodega.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbBloque.Text))
                    {
                        this.Alerta("Seleccione un bloque.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un bloque."));
                        cmbBloque.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbExportador.SelectedValue) || (cmbExportador.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione un exportador.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un exportador."));
                        cmbExportador.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbFilaDesde.SelectedValue) || (cmbFilaDesde.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione una fila desde.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una fila desde."));
                        cmbFilaDesde.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbFilaHasta.SelectedValue) || (cmbFilaHasta.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione una fila hasta.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una fila hasta."));
                        cmbFilaHasta.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
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
                    
                    BAN_Stowage_OrdenDespacho oEntidad = new BAN_Stowage_OrdenDespacho();
                    oEntidad.idOrdenDespacho = null;
                    oEntidad.idNave= txtNave.Text;
                    oEntidad.idExportador = int.Parse(cmbExportador.SelectedValue);
                    oEntidad.cantidadPalets = int.Parse(txtBox.Text);
                    oEntidad.idBodega = int.Parse(cmbBodega.SelectedValue);
                    oEntidad.idBloque= int.Parse(cmbBloque.SelectedValue);
                    oEntidad.filaDesde = int.Parse(cmbFilaDesde.SelectedValue);
                    oEntidad.FilaHasta = int.Parse(cmbFilaHasta.SelectedValue);
                    oEntidad.idAltura = int.Parse(cmbAltura.SelectedValue);
                    oEntidad.idMarca= int.Parse(cmbMarca.SelectedValue);
                    //oEntidad.comentario = string.Empty;
                    oEntidad.estado = "NUE";
                    oEntidad.usuarioCrea = ClsUsuario.loginname;
                    oEntidad.idOrdenDespacho = oEntidad.Save_Update(oEntidad,out OError);

                    if (OError != string.Empty)
                    {
                        string msj = "No se puede crear la orden, es posible de que no existan movimientos con los filtros seleccionados. " + OError;
                        this.Alerta(msj);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}<br>{1}", "No se puede crear la orden, es posible de que no existan movimientos con los filtros seleccionados. " , OError));
                        this.TXTMRN.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }
                    else
                    {
                        btnBuscar_Click(null, null);
                        this.Alerta("Transacción exitosa");
                    }
                }
               
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGrabar_Click), "btnGrabar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
            this.btnGrabar.Attributes.Remove("disabled");
            Actualiza_Paneles();
        }

        protected void btnLimpiarDataAdd_Click(object sender, EventArgs e)
        {
            txtNave.Text = string.Empty;
            txtDescripcionNave.Text = string.Empty;
            txtBox.Text = string.Empty;
            cmbExportador.SelectedValue = "0";
            cmbBodega.SelectedValue = "0";
            cmbBodega_SelectedIndexChanged(null,null);
            cmbFilaDesde.SelectedValue = "0";
            cmbFilaHasta.SelectedValue = "0";
            cmbAltura.SelectedValue = "0";
            cmbMarca.SelectedValue = "0";
            UPBOTONES.Update();
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
                cmbExportador.SelectedValue = "0";
            }
        }

        protected void txtFechaProceso_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //btnBuscar_Click(null, null);
                //txtBox.Focus();
            }
            catch
            {

            }
        }

        protected void cmbDetBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Bloque.ConsultarLista(int.Parse(cmbDetBodega.SelectedValue), out oError); //ds.Tables[0].DefaultView;
                cmbDetBloque.DataSource = oEntidad;
                cmbDetBloque.DataValueField = "id";
                cmbDetBloque.DataTextField = "nombre";
                cmbDetBloque.DataBind();
            }
            catch //(Exception ex)
            {

            }
        }

        #endregion

        protected void tablePaginationDetalle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                int totalBoxes = 0;
                totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_Movimiento_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }
        }

        protected void cmbBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Bloque.ConsultarLista(int.Parse(cmbBodega.SelectedValue), out oError); //ds.Tables[0].DefaultView;
                cmbBloque.DataSource = oEntidad;
                cmbBloque.DataValueField = "id";
                cmbBloque.DataTextField = "nombre";
                cmbBloque.DataBind();
            }
            catch //(Exception ex)
            {

            }
        }

        protected void cmbFilaDesde_SelectedIndexChanged(object sender, EventArgs e)
        {
            string oError;
            var oEntidad = BAN_Catalogo_Fila.ConsultarLista(out oError);
            cmbFilaHasta.DataSource = oEntidad.Where(p=> p.id >= int.Parse(cmbFilaDesde.SelectedValue));
            cmbFilaHasta.DataValueField = "ID";
            cmbFilaHasta.DataTextField = "descripcion";
            cmbFilaHasta.DataBind();
        }
    }
}