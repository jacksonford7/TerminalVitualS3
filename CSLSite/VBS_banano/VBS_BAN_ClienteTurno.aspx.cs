using BillionEntidades;
using BreakBulk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBSEntidades.Banano;
using VBSEntidades.BananoMuelle;

namespace CSLSite
{
    public partial class VBS_BAN_ClienteTurno : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private BAN_TurnosCab objCabecera= new BAN_TurnosCab();
        private List<BAN_TurnosDet> objDetalle = new List<BAN_TurnosDet>();
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
            UPEDIT_DET.Update();
            UPEDIT.Update();
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
            UPEDIT_DET.Update();
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
            objCabecera = new BAN_TurnosCab();
            Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] = objCabecera;
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
        //public void LlenaComboProductos()
        //{
        //    try
        //    {
        //        cmbProducto.DataSource = productos.consultaProductos(); 
        //        cmbProducto.DataValueField = "ID";
        //        cmbProducto.DataTextField = "nombre";
        //        cmbProducto.DataBind();
        //        cmbProducto.Enabled = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboManiobras), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
        //        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
        //    }
        //}
        
        public void LlenarFiltroEstados()
        {
            try
            {


                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";
                //cmbFiltroEstados.Items.Add(item);
                //cmbFiltroEstados.Items.Add(new ListItem("VERIFICADO", "ACT"));
                //cmbFiltroEstados.Items.Add(new ListItem("VERIFCACIÓN PENDIENTE", "NO_ACT"));
                //cmbFiltroEstados.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenarFiltroEstados), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        /*
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
                        Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] = objCabecera;

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
        }*/
        private void ConsultarDataTurnoDet()
        {
            try
            {
                var Resultado = BAN_TurnosCab.GetTurnoCab(txtNave.Text,Txtruc.Text,TXTMRN.Text, out OError);

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
                            a.TurnoCab = Resultado;
                            a.HorarioInicial = BAN_HorarioInicial.GetHorarioInicio(int.Parse(a.idHoraInicio));
                            a.HorarioFinal = BAN_HorarioFinal.GetHorarioFinal(int.Parse(a.idHoraFinal));
                            a.SubDetalle = BAN_TurnosCliente.listadoTurnosCliente(a.IdTurno, out OError);
                        }

                        objCabecera = Resultado;
                        Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] = objCabecera;

                        txtNave.Text = objCabecera.idNave;
                        txtDescripcionNave.Text = objCabecera.nave;
                        TXTMRN.Text = objCabecera.mrn;
                        fecETA.Text = objCabecera.eta.ToString("dd/MM/yyyy");
                        //cmbEstado.SelectedValue = objCabecera.estado.ToString();

                        if (Resultado.Detalle.Count() > 0)
                        {
                            tablePagination.DataSource = Resultado.Detalle;
                            tablePagination.DataBind();
                        }
                        //////////////////////this.btnGrabar.Attributes["disabled"] = "disabled";
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                    }

                    this.Actualiza_Paneles();
                }
                //else
                //{
                //    ConsultarDataEcuapass();
                   
                //}
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataTurnoDet), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
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
                    //this.btnGrabar.Attributes["disabled"] = "disabled";
                    sinresultado.Visible = false;
                    objCabecera.Detalle = objDetalle;
                    LlenaComboEstado();                   
                    LlenarFiltroEstados();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbEstado.SelectedValue = "A";
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion



        #region "Eventos"
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //this.btnGrabar.Attributes["disabled"] = "disabled";
            Limpia_Datos_cliente();
            objDetalle.Clear();
            objCabecera = new BAN_TurnosCab();
            Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] = objCabecera;
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

                    ConsultarDataTurnoDet();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }


        }

        #region "Gridview Cabecera"
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");
                string v_estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString().Trim();
                long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "IdTurno").ToString().Trim());

                if (v_id <= 0)
                {
                    btnEditar.Enabled = false;
                }
                else
                {
                    if (v_estado != "A")
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
                objCabecera = Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] as BAN_TurnosCab;
                if (objCabecera == null) { return; }

                var oDet = objCabecera.Detalle.Where(a => a.IdTurno == v_ID).FirstOrDefault();
                Session["TransaccionTurnoDet" + this.hf_BrowserWindowName.Value] = oDet;

                hdf_CodigoCab.Value = oDet.IdTurnoCab.ToString();
                hdf_CodigoDet.Value = oDet.IdTurno.ToString();

                txtHorarioInicioDet.Text = oDet.HoraInicio;
                txtHorarioFinDet.Text = oDet.HoraFinal;
                txtCantidadDet.Text = oDet.Cantidad.ToString();
                txtVigenciaInicialdet.Text = oDet.VigenciaInicial.ToShortDateString();
                txtVigenciaFinalDet.Text = oDet.VigenciaFinal.ToShortDateString();
                txtHorarioDet.Text = oDet.Horario.ToString();
                txtDisponibledet.Text = oDet.Disponible.ToString();
                txtAsignadosDet.Text = oDet.Asignados.ToString();
                txtLineaDet.Text = oDet.LineaNaviera;

                if (oDet.SubDetalle?.Count > 0)
                {
                    tablePaginationEdit.DataSource = oDet.SubDetalle;
                    tablePaginationEdit.DataBind();

                    sinresultadoEdit.Visible = false;
                    UPEDIT_DET.Update();
                }

                nave oNave;
                try { oNave = nave.GetNave(oDet.TurnoCab.idNave); } catch { Response.Redirect("../login.aspx", false); return; }

                if (string.IsNullOrEmpty(oNave.ata?.ToString()))
                {
                    //se valida que el Bl no se encuentre en status PRE - CON - DES
                    var oDeta = BAN_TurnosDet.GetTurnoDet(long.Parse(oDet.IdTurno.ToString()));
                    if (oDeta.Estado == "A" )
                    {
                        this.btnAdd.Attributes.Remove("disabled");
                    }
                    else
                    {
                        this.btnAdd.Attributes["disabled"] = "disabled";
                        this.Alerta("No se puede asignar, el Turno ya fue usado y se encuentra en status INACTIVO." );
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el Turno ya fue usado y se encuentra en status INACTIVO."));
                        this.txtLineaDet.Focus();
                        
                        UPEDIT.Update();
                        return;
                    }
                }
                else
                {
                    this.btnAdd.Attributes["disabled"] = "disabled";
                    this.Alerta("La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                    this.txtLineaDet.Focus();
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
                objCabecera = Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] as BAN_TurnosCab;

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

        #region "Controles Detalle"
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {
                    objCabecera = Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] as BAN_TurnosCab;
                    nave oNave;
                    try { oNave = nave.GetNave(objCabecera.idNave); } catch { Response.Redirect("../login.aspx", false); return; }

                    //if (string.IsNullOrEmpty(oNave.ata?.ToString()))
                    //{
                    //    this.btnAdd.Attributes.Remove("disabled");
                    //}
                    //else
                    //{
                    //    this.btnAdd.Attributes["disabled"] = "disabled";
                    //    this.Alerta("La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                    //    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                    //    this.txtLineaDet.Focus();
                    //    return;
                    //}

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

                        if (string.IsNullOrEmpty(this.txtConsignatario.Text))
                        {
                            this.Alerta("Ingrese el Cliente.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el cliente"));
                            this.txtLineaDet.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtRucConsignatario.Text))
                        {
                            this.Alerta("Ingrese el Ruc del Cliente.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el ruc del cliente"));
                            this.txtLineaDet.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtCantidadAsignada.Text))
                        {
                            this.Alerta("Ingrese la cantidad.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la cantidad"));
                            this.txtLineaDet.Focus();
                            return;
                        }

                        BAN_TurnosDet oDetalle = new BAN_TurnosDet();
                        oDetalle = Session["TransaccionTurnoDet" + this.hf_BrowserWindowName.Value] as BAN_TurnosDet;

                        if (oDetalle != null)
                        {
                            //se valida que el Bl no se encuentre en status PRE - CON - DES
                            var oDet = BAN_TurnosDet.GetTurnoDet(long.Parse(oDetalle.IdTurno.ToString()));
                            if (oDet.Estado == "A")
                            {
                                this.btnAdd.Attributes.Remove("disabled");
                                UPEDIT.Update();
                            }
                            else
                            {
                                this.btnAdd.Attributes["disabled"] = "disabled";
                                this.Alerta("No se puede actualizar, el Turno se encuentra en status INACTIVO");
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el Turno se encuentra en status INACTIVO"));
                                this.txtLineaDet.Focus();
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


                            BAN_TurnosCliente oSubDetalle = new BAN_TurnosCliente();
                            long v_secuencia = -1;

                            oSubDetalle.cantidad = int.Parse(txtCantidadAsignada.Text.ToString());
                            oSubDetalle.estado = true;
                            oSubDetalle.idcliente = this.txtRucConsignatario.Text.Trim();
                            oSubDetalle.cliente = this.txtConsignatario.Text.Trim();
                            oSubDetalle.idTurnoDet = oDetalle.IdTurno;
                            oSubDetalle.usuarioCrea = ClsUsuario.loginname;

                            var oRespuesta = oSubDetalle.Save_Update(out v_secuencia);
                            oSubDetalle.secuencia = v_secuencia;


                            hdf_CodigoCab.Value = oRespuesta.Resultado.FirstOrDefault().TurnoDet.IdTurnoCab.ToString();
                            hdf_CodigoCab.Value = oRespuesta.Resultado.FirstOrDefault().TurnoDet.IdTurno.ToString();
                            txtHorarioInicioDet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.HoraInicio;
                            txtHorarioFinDet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.HoraFinal;
                            txtCantidadDet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.Cantidad.ToString();
                            txtVigenciaInicialdet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.VigenciaInicial.ToShortDateString();
                            txtVigenciaFinalDet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.VigenciaFinal.ToShortDateString();
                            txtHorarioDet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.Horario.ToString();
                            txtDisponibledet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.Disponible.ToString();
                            txtAsignadosDet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.Asignados.ToString();
                            txtLineaDet.Text = oRespuesta.Resultado.FirstOrDefault().TurnoDet.LineaNaviera;


                            if (!oRespuesta.Exitoso)
                            {
                                this.Alerta(OError);
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", oRespuesta.MensajeProblema));
                                this.TXTMRN.Focus();
                                return;
                            }
                            else
                            {
                                tablePaginationEdit.DataSource = oRespuesta.Resultado;
                                tablePaginationEdit.DataBind();

                                this.Alerta("Transacción exitosa");
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se asignó exitosamente el Turno  {0} ", oDetalle.HoraInicio.ToString()));
                                this.btnAdd.Attributes["disabled"] = "disabled";
                                Session["TransaccionTurnoDet" + this.hf_BrowserWindowName.Value] = null;
                            }
                        }
                        else { Response.Redirect("../login.aspx", false); return; }
                    }
                }
                Actualiza_Panele_Detalle();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAdd_Click), "btnAdd_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                //this.Mostrar_MensajeDet(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
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
                    this.txtRucConsignatario.Focus();
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
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        #endregion
        #endregion


        protected void tablePaginationEdit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                objDetalle = Session["TransaccionTurnoDet" + this.hf_BrowserWindowName.Value] as List<BAN_TurnosDet>;

                if (objDetalle == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    tablePaginationEdit.PageIndex = e.NewPageIndex;
                    tablePaginationEdit.DataSource = objDetalle.Where(p=> p.IdTurno == long.Parse( hdf_CodigoDet.Value)).FirstOrDefault().SubDetalle;
                    tablePaginationEdit.DataBind();
                    this.Actualiza_Panele_Detalle();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }

        protected void tablePaginationEdit_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (tablePaginationEdit.Rows.Count > 0)
                {
                    tablePaginationEdit.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    tablePaginationEdit.HeaderRow.TableSection = TableRowSection.TableHeader;

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

        protected void tablePaginationEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }


                //objCabecera = Session["TransaccionTurnoCab" + this.hf_BrowserWindowName.Value] as BAN_TurnosCab;
                //if (objCabecera == null) { return; }

                //var oDet = objCabecera.Detalle.Where(a => a.IdTurno == v_ID).FirstOrDefault();
                //Session["TransaccionTurnoDet" + this.hf_BrowserWindowName.Value] = oDet;

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

                BAN_TurnosCliente oTurnosCliente = new BAN_TurnosCliente();
                var oSubDet = oTurnosCliente.GetListaTurnosCliente(v_ID, 0).Resultado.FirstOrDefault();
                long v_sec = -1;
                
                oSubDet.estado = false;
                oSubDet.usuarioModifica = ClsUsuario.loginname;
                oSubDet.Save_Update(out v_sec);

                /*
                if (oDet.SubDetalle?.Count > 0)
                {
                    tablePaginationEdit.DataSource = oDet.SubDetalle;
                    tablePaginationEdit.DataBind();

                    sinresultadoEdit.Visible = false;
                    UPEDIT_DET.Update();
                }

                nave oNave;
                try { oNave = nave.GetNave(oDet.TurnoCab.idNave); } catch { Response.Redirect("../login.aspx", false); return; }

                if (string.IsNullOrEmpty(oNave.ata?.ToString()))
                {
                    //se valida que el Bl no se encuentre en status PRE - CON - DES
                    var oDeta = BAN_TurnosDet.GetTurnoDet(long.Parse(oDet.IdTurno.ToString()));
                    if (oDeta.Estado == "A")
                    {
                        this.btnAdd.Attributes.Remove("disabled");
                    }
                    else
                    {
                        this.btnAdd.Attributes["disabled"] = "disabled";
                        this.Alerta("No se puede asignar, el Turno ya fue usado y se encuentra en status INACTIVO.");
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el Turno ya fue usado y se encuentra en status INACTIVO."));
                        this.txtLineaDet.Focus();

                        UPEDIT.Update();
                        return;
                    }
                }
                else
                {
                    this.btnAdd.Attributes["disabled"] = "disabled";
                    this.Alerta("La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "La nave ha arribado a la terminal y consta con DRM. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                    this.txtLineaDet.Focus();
                    UPEDIT.Update();
                    return;
                }

                msjErrorDetalle.Visible = false;

                UPEDIT.Update();
                */
            }
        }

        protected void tablePaginationEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                //CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");
                //string v_estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString().Trim();
                //long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "IdTurno").ToString().Trim());

                //if (v_id <= 0)
                //{
                //    btnEditar.Enabled = false;
                //}
                //else
                //{
                //    if (v_estado != "A")
                //    {
                //        Chk.Checked = true;
                //        e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                //    }
                //}

                //this.Actualiza_Panele_Detalle();
            }
        }
    }
}