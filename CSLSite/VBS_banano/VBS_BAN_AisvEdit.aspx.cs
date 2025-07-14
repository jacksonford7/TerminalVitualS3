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
    public partial class VBS_BAN_AisvEdit : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private BAN_Stowage_Plan_Cab objStowageCab = new BAN_Stowage_Plan_Cab();
        private List<BAN_Stowage_Plan_Det> objStowagegDet = new List<BAN_Stowage_Plan_Det>();
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
            UPAISV.Update();
        }
        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            Actualiza_Paneles();
        }
        private void Limpia_Datos_cliente()
        {
            this.TXTMRN.Text = string.Empty;
           //this.txtFechaProceso.Text = string.Empty;
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
            //this.banmsg.Visible = true;
            //this.banmsg.InnerHtml = Mensaje;
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
            //this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            //this.banmsg.Visible = false;
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
            objStowagegDet = new List<BAN_Stowage_Plan_Det>();
            objStowageCab.oDetalle = objStowagegDet;
            Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;
        }
        //public void LlenaComboLinea()
        //{
        //    try
        //    {
        //        var oEntidad = BAN_Catalogo_Linea.ConsultarListaLlenaCombo(Txtruc.Text); //ds.Tables[0].DefaultView;
        //        cmbLinea.DataSource = oEntidad;
        //        cmbLinea.DataValueField = "ID";
        //        cmbLinea.DataTextField = "nombre";
        //        cmbLinea.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboLinea), "VBS_BAN_PreStowage.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
        //        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
        //    }
        //}
                
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
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void ConsultarDataLoading()
        {
            try
            {
                //List<BAN_Stowage_Plan_Aisv> oListaAisv = new List<BAN_Stowage_Plan_Aisv>();
                //DateTime fecha = new DateTime();
                //CultureInfo enUS = new CultureInfo("en-US");
                //if (!DateTime.TryParseExact(txtFechaProceso.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                //{
                //    this.Alerta(string.Format("EL FORMATO DE FECHA PROCESO DEBE SER Mes/dia/Anio {0}", txtFechaProceso.Text));
                //    txtFechaProceso.Focus();
                //    this.btnGrabar.Attributes.Remove("disabled");
                //    return;
                //}
                //int fechaDocumento = int.Parse(fecha.ToString("yyyyMMdd"));

                //var ResultadoCab = BAN_Stowage_Plan_Cab.GetStowagePlanCabEspecifico(null ,txtNave.Text, int.Parse(cmbLinea.SelectedValue), fechaDocumento);

                var ListaAISV = BAN_Stowage_Plan_Aisv.ConsultarLista(txtNave.Text, out OError);
                if (ListaAISV != null)
                {
                    //var ResultadoDet = BAN_Stowage_Plan_Det.ConsultarLista(ResultadoCab.idStowageCab, out OError);
                    foreach (var itemAisv in ListaAISV)
                    {
                        itemAisv.oStowage_Plan_Det = BAN_Stowage_Plan_Det.GetEntidad(itemAisv.idStowageDet);
                        itemAisv.oListaStowage_Movimiento = BAN_Stowage_Movimiento.ConsultarLista(long.Parse(itemAisv.idStowageAisv.ToString()), out OError);
                        itemAisv.oStowage_Plan_Det.oMarca = BAN_Catalogo_Marca.GetMarca(itemAisv.oStowage_Plan_Det.idMarca);
                        itemAisv.oStowage_Plan_Det.oExportador = BAN_Catalogo_Exportador.GetExportaador(itemAisv.oStowage_Plan_Det.idExportador);
                    }
                    //oListaAisv.AddRange(ListaAISV);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                        return;
                    }

                    //foreach (var a in ResultadoDet)
                    //{
                    //    a.oHold = BAN_Catalogo_Hold.GetHold(a.idHold);
                    //    a.oEstado = BAN_Catalogo_Estado.GetEntidad(a.estado);
                    //    a.oCargo = BAN_Catalogo_Cargo.GetCargos(a.idCargo);
                    //    a.oMarca = BAN_Catalogo_Marca.GetMarca(a.idMarca);
                    //    a.oExportador = BAN_Catalogo_Exportador.GetExportaador(a.idExportador);
                    //    a.oConsignatario = BAN_Catalogo_Consignatario.GetConsignatario(a.idConsignatario);
                    //    a.ListaAISV = BAN_Stowage_Plan_Aisv.ConsultarLista(long.Parse(a.idStowageDet.ToString()), out OError);
                    //    a.oStowage_Plan_Cab = ResultadoCab;
                    //    try
                    //    {
                    //        a.oBloque = BAN_Catalogo_Bloque.GetEntidad(int.Parse(a.idBloque.ToString()));
                    //        a.oBodega = BAN_Catalogo_Bodega.GetEntidad(int.Parse(a.idBodega.ToString()));
                    //        a.oBloque.oBodega = a.oBodega;

                    //        foreach (var itemAisv in a.ListaAISV)
                    //        {
                    //            itemAisv.oStowage_Plan_Det = a;
                    //            itemAisv.oListaStowage_Movimiento = BAN_Stowage_Movimiento.ConsultarLista(long.Parse(itemAisv.idStowageAisv.ToString()), out OError);
                    //        }
                    //        oListaAisv.AddRange(a.ListaAISV);
                    //    }
                    //    catch { }
                    //}
                    //ResultadoCab.oDetalle = ResultadoDet;
                    //objStowageCab = ResultadoCab;
                    //Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;

                   
                    //##########################################
                    // Asignar los datos grabados al Repeater
                    //##########################################
                    var LinqQuery = from Tbl in ListaAISV.Where(Tbl => Tbl.pendiente < 48)
                                    select new
                                    {
                                        id = Tbl.idStowageAisv,
                                        fecha = Tbl.fecha.ToString("dd/MM/yyyy"),
                                        time = string.Format("{0} - {1}", Tbl.horaInicio.Trim(), Tbl.horaFin.Trim()),
                                        exportador = Tbl.oStowage_Plan_Det?.oExportador?.nombre,
                                        marca = Tbl.oStowage_Plan_Det?.oMarca?.nombre,
                                        box = Tbl.box,
                                        arrastre = Tbl.arrastre,
                                        pendiente = Tbl.pendiente,
                                        aisv = Tbl.aisv,
                                        dae = Tbl.dae,
                                        booking = Tbl.booking,
                                        iieAut = Tbl.IIEAutorizada,
                                        daeAut = Tbl.daeAutorizada,
                                        placa = Tbl.placa,
                                        chofer = string.Format("{0} - {1}", Tbl.idChofer.Trim(), Tbl.chofer.Trim()),
                                        usuarioCrea = Tbl.usuarioCrea.Trim(),
                                        //fechaCreacion = Tbl.fechaCreacion.ToString("dd/MM/yyyy HH:mm"),
                                        comentario = Tbl.comentario
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
                    }catch
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
                this.btnGrabar.Attributes.Remove("disabled");
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataLoading), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
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
                this.Mostrar_Mensaje( Error);
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
                //this.IsAllowAccess();
                this.banmsg.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;
                this.msjErrorLiquidacion.Visible = IsPostBack;
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
                    this.btnGrabar.Attributes["disabled"] = "disabled";
                    //this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled"; 
                    sinresultado.Visible = false;
                    SinResultadoAISV.Visible = false;
                    
                    //LlenaComboLinea();
                    LlenaComboBodega();
                    
                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";
                    cmbDetBodega.Items.Add(item);
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

                    if (e.CommandName == "Editar")
                    {
                        txtDetAisvCantidad.Text = "0";
                        txtDetPiso.Text = string.Empty;
                        txtDetobservacion.Text = string.Empty;
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        if (objStowageCab == null) {
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

                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(id.ToString()));

                        hdf_CodigoCab.Value = objStowageCab.idStowageCab.ToString();
                        hdf_CodigoDet.Value = id.ToString();
                        txtDetPiso.Text = oDet.piso;
                        cmbDetHold.SelectedValue = oDet.idHold.ToString();
                        txtDetCantidad.Text = oDet.boxSolicitado.ToString();
                        cmbDetCargo.SelectedValue = oDet.idCargo.ToString();
                        try { cmbDetConsignatario.SelectedValue = oDet.idConsignatario.ToString(); } catch { }
                        try { cmbDetExportador.SelectedValue = oDet.idExportador.ToString(); } catch { }
                        try { cmbDetMarca.SelectedValue = oDet.idMarca.ToString(); } catch { }
                        try { cmbDetBodega.SelectedValue = oDet.idBodega.ToString(); } catch { cmbDetBodega.SelectedValue = "0"; }
                        cmbDetBodega_SelectedIndexChanged(null,null);
                        try { cmbDetBloque.SelectedValue = oDet.idBloque.ToString(); } catch { cmbDetBodega.SelectedValue = null; }
                        txtDetPiso.Text = oDet.piso.ToString();
                        txtDetobservacion.Text = oDet.comentario.ToString();
                        msjErrorDetalle.Visible = false;
                        this.btnActualizar.Attributes.Remove("disabled");
                        UPEDIT.Update();

                        //Actualiza_Paneles();
                    }

                    if (e.CommandName == "Quitar")
                    {
                        Ocultar_Mensaje();
                        msjErrorDetalle.Visible = false;

                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(v_ID.ToString()));
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDet;
                        //Actualiza_Paneles();
                    }

                    if (e.CommandName == "Aisv")
                    {
                        //Ocultar_Mensaje();
                        msjErrorLiquidacion.Visible = false;
                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        objStowageCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusiones(objStowageCab.idStowageCab, out OError);

                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(v_ID.ToString()));
                        oDet.oStowage_Plan_Cab = objStowageCab;
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDet;

                        if (objStowageCab == null) {
                            try
                            {
                                var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                                ClsUsuario = ClsUsuario_;
                                btnBuscar_Click(null, null);
                            }
                            catch
                            {
                                Response.Redirect("../login.aspx", false);
                                return;
                            }
                        }
                        txtIdDetalle.Text = oDet.idStowageDet.ToString();
                        oDet.oHold = BAN_Catalogo_Hold.GetHold(oDet.idHold);
                        txtDetAisvHold.Text = oDet.oHold?.nombre.ToString();
                        txtDetAisvCantidad.Text = oDet.boxSolicitado.ToString();
                        UPAISV.Update();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "Editar", false, null, null, ex.StackTrace, ex);
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
                totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_Plan_Cab_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //this.Actualiza_Panele_Detalle();
            }
        }

       
       
        #endregion
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes["disabled"] = "disabled";
            //this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled";
            Limpia_Datos_cliente();
            //objDetalle.Clear();
            objStowageCab = new BAN_Stowage_Plan_Cab();
            objStowagegDet = new List<BAN_Stowage_Plan_Det>();
            objStowageCab.oDetalle = objStowagegDet;
            Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes["disabled"] = "disabled";
            //this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled";
            sinresultado.Visible = false;
            ValidarDatosNave();

            //objDetalle.Clear();
            //objLoadingCab.Detalle = objDetalle;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
            if (Response.IsClientConnected)
            {
                try
                {
                    //if (string.IsNullOrEmpty(this.txtFechaProceso.Text))
                    //{
                    //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar la fecha proceso"));
                    //    this.txtNave.Focus();
                    //    return;
                    //}

                    //DateTime fecha = new DateTime();
                    //CultureInfo enUS = new CultureInfo("en-US");
                    //if (!DateTime.TryParseExact(txtFechaProceso.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                    //{
                    //    this.Alerta(string.Format("EL FORMATO DE FECHA PROCESO DEBE SER Mes/dia/Anio {0}", txtFechaProceso.Text));
                    //    txtFechaProceso.Focus();
                    //    this.btnGrabar.Attributes.Remove("disabled");
                    //    return;
                    //}

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

                    //if (string.IsNullOrEmpty(cmbLinea.SelectedValue))
                    //{
                    //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Valor de la linea no válido"));
                    //    this.fecETA.Focus();
                    //    return;
                    //}


                    ConsultarDataLoading();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscar_Click), "btnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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

                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                    if (objStowageCab != null)
                    {
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
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre de la nave"));
                            this.txtNave.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }
                       
                        //DateTime fechaProceso = new DateTime();
                        //CultureInfo enUS = new CultureInfo("en-US");
                        //if (!DateTime.TryParseExact(txtFechaProceso.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechaProceso))
                        //{
                        //    this.Alerta(string.Format("EL FORMATO DE FECHA POCESO DEBE SER Mes/dia/Anio {0}", txtFechaProceso.Text));
                        //    txtFechaProceso.Focus();
                        //    this.btnGrabar.Attributes.Remove("disabled");
                        //    return;
                        //}


                       
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

        protected void btnActualizar_Click(object sender, EventArgs e) 
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {
                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

                    
                    if (objStowageCab != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetHold.SelectedValue) || (cmbDetHold.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un hold.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un hold."));
                            cmbDetHold.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(txtDetPiso.Text))
                        {
                            this.Alerta("Seleccione un piso.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un piso."));
                            txtDetPiso.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetCargo.SelectedValue) || (cmbDetCargo.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un cargo.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un cargo."));
                            cmbDetCargo.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetMarca.SelectedValue) || (cmbDetMarca.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione una marca.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una marca."));
                            cmbDetMarca.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetExportador.SelectedValue) || (cmbDetExportador.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un exportador.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un exportador."));
                            cmbDetExportador.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetConsignatario.SelectedValue) || (cmbDetConsignatario.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un consignatario.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un consignatario."));
                            cmbDetConsignatario.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtDetCantidad.Text) || this.txtDetCantidad.Text == "0")
                        {
                            this.Alerta("Ingrese la cantidad.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese la cantidad."));
                            this.txtDetCantidad.Focus();
                            return;
                        }
                        

                        BAN_Stowage_Plan_Det oDetalle = new BAN_Stowage_Plan_Det();
                        oDetalle = objStowageCab.oDetalle.Where(a => a.idStowageDet == long.Parse(hdf_CodigoDet.Value)).FirstOrDefault();
                        if (oDetalle == null) { Response.Redirect("../login.aspx", false); return; }

                        //#########################################
                        //VALIDA QUE NO SE REPITA EL REGISTRO
                        //#########################################
                        var oDet1 = BAN_Stowage_Plan_Det.ConsultarLista(objStowageCab.idStowageCab, out OError);
                        if (OError != string.Empty)
                        {
                            this.Alerta(OError);
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                            this.TXTMRN.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        //VALIDA QUE NO SE REPITA EL MISMO REGISTRO
                        if (oDet1.Where(p => p.idExportador == int.Parse(cmbDetExportador.SelectedValue) 
                                                && p.idMarca == int.Parse(cmbDetMarca.SelectedValue) 
                                                && p.idHold == int.Parse(cmbDetHold.SelectedValue)
                                                && p.idStowageDet != long.Parse(oDetalle.idStowageDet.ToString())).Count() > 0)
                        {
                            this.Alerta("Exportador y marca ya se encuentra registrada en el Hold");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Exportador y marca ya se encuentra registrada en el Hold"));
                            this.TXTMRN.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }
                        //#########################################
                        if (oDetalle != null)
                        {
                            var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(oDetalle.idStowageDet.ToString()));
                            oDetalle = oDet;
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

                            if (Txtruc.Text == "0992506717001") //ruc de cgsa
                            {
                                if (string.IsNullOrEmpty(cmbDetBodega.SelectedValue) || (cmbDetBodega.SelectedValue == "0"))
                                {
                                    this.Alerta("Seleccione una bodega.");
                                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una bodega."));
                                    cmbDetBodega.Focus();
                                    return;
                                }

                                if (string.IsNullOrEmpty(cmbDetBloque.SelectedValue) || (cmbDetBloque.SelectedValue == "0"))
                                {
                                    this.Alerta("Seleccione un bloque.");
                                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un bloque."));
                                    cmbDetBloque.Focus();
                                    return;
                                }

                                oDetalle.idBodega = int.Parse(cmbDetBodega.SelectedValue);
                                oDetalle.idBloque= int.Parse(cmbDetBloque.SelectedValue);
                                oDetalle.disponible = oDetalle.boxAutorizado;
                                oDetalle.estado = oDetalle.estado == "NUE" ? "DRF": oDetalle.estado;
                            }

                            if (int.Parse(txtDetCantidad.Text) < oDetalle.reservado)
                            {
                                this.Alerta("La cantidad no puede ser menor a lo yá reservado - Reservado: " + oDetalle.reservado.ToString());
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor verifique la cantidad. <br>La cantidad no puede ser menor a lo yá reservado - Reservado: " + oDetalle.reservado.ToString()));
                                this.txtDetCantidad.Focus();
                                return;
                            }

                            oDetalle.idHold = int.Parse(cmbDetHold.SelectedValue);
                            oDetalle.piso = txtDetPiso.Text;
                            oDetalle.boxSolicitado = int.Parse(txtDetCantidad.Text);
                            oDetalle.idCargo = int.Parse(cmbDetCargo.SelectedValue);
                            oDetalle.idExportador = int.Parse(cmbDetExportador.SelectedValue);
                            oDetalle.idMarca = int.Parse(cmbDetMarca.SelectedValue);
                            oDetalle.idConsignatario = int.Parse(cmbDetConsignatario.SelectedValue);
                            oDetalle.comentario = txtDetobservacion.Text;
                            //oDetalle.estado = "NUE";
                            oDetalle.fechaDocumento = objStowageCab.fechaDocumento;
                            oDetalle.usuarioModifica = ClsUsuario.loginname;
                            oDetalle.idStowageDet = oDetalle.Save_Update(out OError);
                            msjErrorDetalle.Visible = false;

                            oDetalle.idStowageDet = oDetalle.Save_Update(out OError);

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
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el registro  {0} ", oDetalle.idStowageDet.ToString()));
                                this.btnActualizar.Attributes["disabled"] = "disabled";
                                //Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                                hdf_CodigoCab.Value = null;
                                hdf_CodigoDet.Value = null;
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

        protected void btnQuitar_Click(object sender, EventArgs e)
        {
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

            var obj = Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Det;
            BAN_Stowage_Plan_Det oDet = new BAN_Stowage_Plan_Det();
            oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(obj.idStowageDet.ToString()));
            Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = null;

            oDet.usuarioModifica = ClsUsuario.loginname;
            var item = oDet.Save_Anulacion(out OError);

            if (string.IsNullOrEmpty(OError))
            {
                btnBuscar_Click(null, null);
                this.Alerta("Transacción exitosa");
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se anulo con éxito el item {0} ", item.ToString()));
            }
            else
            {
                btnBuscar_Click(null, null);
                this.Alerta("No se logró completar la anulación");
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se presentó la siguiente novedad:  {0} ", OError.ToString()));
            }
            
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }

        protected void btnLimpiarDataAdd_Click(object sender, EventArgs e)
        {
            //cmbPiso.SelectedValue = "0";
            UPBOTONES.Update();
        }

        protected void btnAutorizarEdicion_Click(object sender, EventArgs e)
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


                   
                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                    if (objStowageCab != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }
                    }

                    OError = string.Empty;
                    BAN_Stowage_Plan_Cab oCab = new BAN_Stowage_Plan_Cab();
                    oCab.idStowageCab = objStowageCab.idStowageCab;
                    oCab.usuarioCrea = ClsUsuario.loginname;
                    var result = oCab.Save_Autorizacion(out OError);

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
                        //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el BL  {0} ", oDetalle.bl.ToString()));
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se autorizo la edición del Stowage exitosamente, usuario: {0} ", oCab.idStowageCab.ToString()));
                        this.btnActualizar.Attributes["disabled"] = "disabled";
                        //Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                        hdf_CodigoCab.Value = null;
                        hdf_CodigoDet.Value = null;
                    }

                }
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAutorizarEdicion_Click), "btnAutorizarEdicion_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                //this.Mostrar_MensajeDet(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        protected void cmbLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";

                btnBuscar_Click(null, null);
            }
            catch
            {
            }
        }

        protected void txtFechaProceso_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //tablePagination.DataSource = null;
                //tablePagination.DataBind();

                //dgvTotales.DataSource = null;
                //dgvTotales.DataBind();

                //sinresultado.Visible = true;
                //this.Actualiza_Paneles();
                btnBuscar_Click(null, null);
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

    }
}