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
using VBSEntidades.BananoMuelle;

namespace CSLSite
{
    public partial class VBS_BAN_Loading : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private BAN_Loading_Program_Cab objLoadingCab = new BAN_Loading_Program_Cab();
        private List<BAN_Loading_Program_Det> objLoadingDet = new List<BAN_Loading_Program_Det>();
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
            this.txtFechaProceso.Text = string.Empty;
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
            objLoadingCab = new BAN_Loading_Program_Cab();
            objLoadingDet = new List<BAN_Loading_Program_Det>();
            objLoadingCab.Detalle = objLoadingDet;
            Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] = objLoadingCab;
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboLinea), "VBS_BAN_Loading.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboHold()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Hold.ConsultarListaHold(out oError); //ds.Tables[0].DefaultView;
                cmbHold.DataSource = oEntidad;
                cmbHold.DataValueField = "ID";
                cmbHold.DataTextField = "nombre";
                cmbHold.DataBind();

                cmbDetHold.DataSource = oEntidad;
                cmbDetHold.DataValueField = "ID";
                cmbDetHold.DataTextField = "nombre";
                cmbDetHold.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboHold), "VBS_BAN_Loading.LlenaComboHold", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboPiso()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Piso.ConsultarListaPisos(out oError); //ds.Tables[0].DefaultView;
                cmbPiso.DataSource = oEntidad;
                cmbPiso.DataValueField = "ID";
                cmbPiso.DataTextField = "nombre";
                cmbPiso.DataBind();

                cmbDetPiso.DataSource = oEntidad;
                cmbDetPiso.DataValueField = "ID";
                cmbDetPiso.DataTextField = "nombre";
                cmbDetPiso.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboPiso), "VBS_BAN_Loading.LlenaComboPiso", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboCargo()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Cargo.ConsultarListaCargos(out oError); //ds.Tables[0].DefaultView;
                cmbCargo.DataSource = oEntidad;
                cmbCargo.DataValueField = "ID";
                cmbCargo.DataTextField = "nombre";
                cmbCargo.DataBind();

                cmbDetCargo.DataSource = oEntidad;
                cmbDetCargo.DataValueField = "ID";
                cmbDetCargo.DataTextField = "nombre";
                cmbDetCargo.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboCargo), "VBS_BAN_Loading.LlenaComboCargo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboExportador), "VBS_BAN_Loading.LlenaComboExportador", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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

                cmbDetMarca.DataSource = oEntidad;
                cmbDetMarca.DataValueField = "ID";
                cmbDetMarca.DataTextField = "nombre";
                cmbDetMarca.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboMarcas), "VBS_BAN_Loading.LlenaComboMarcas", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboConsignatario()
        {
            try
            {
                string oError;
                var oLinea = BAN_Catalogo_Linea.GetLinea(int.Parse(cmbLinea.SelectedValue));
                var oEntidad = BAN_Catalogo_Consignatario.ConsultarListaConsignatarios(oLinea.ruc, out oError); //ds.Tables[0].DefaultView;
                cmbConsignatario.DataSource = oEntidad;
                cmbConsignatario.DataValueField = "ID";
                cmbConsignatario.DataTextField = "nombre";
                cmbConsignatario.DataBind();

                cmbDetConsignatario.DataSource = oEntidad;
                cmbDetConsignatario.DataValueField = "ID";
                cmbDetConsignatario.DataTextField = "nombre";
                cmbDetConsignatario.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboExportador), "VBS_BAN_Loading.LlenaComboConsignatario", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        private void LlenarComboFechaInicial()
        {
            try
            {
                List<BAN_HorarioInicial> Listado = BAN_HorarioInicial.ConsultarHorariosIniciales(out cMensajes);
                var idhora = Listado.FirstOrDefault().Id_Hora;

                //Carga_CboHorarioFinal(idhora);
                this.cboHorarioInicial.DataSource = Listado;
                this.cboHorarioInicial.DataTextField = "Desc_Hora";
                this.cboHorarioInicial.DataValueField = "Id_Hora";
                this.cboHorarioInicial.DataBind();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_cboHorarioInicials", "Hubo un error al cargar Tipo de cargas", t.loginname));
                this.Mostrar_Mensaje(Error);
            }
        }
        private void LlenarComboDetFechaInicial()
        {
            try
            {
                List<BAN_HorarioInicial> Listado = BAN_HorarioInicial.ConsultarHorariosIniciales(out cMensajes);
                var idhora = Listado.FirstOrDefault().Id_Hora;
               // Carga_CboHorarioFinal(idhora);
                this.cmbDetHoraInicio.DataSource = Listado;
                this.cmbDetHoraInicio.DataTextField = "Desc_Hora";
                this.cmbDetHoraInicio.DataValueField = "Id_Hora";
                this.cmbDetHoraInicio.DataBind();
                UPEDIT.Update();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_cboHorarioInicials", "Hubo un error al cargar Tipo de cargas", t.loginname));
                this.Mostrar_Mensaje(Error);
            }
        }
        private void ConsultarDataLoading()
        {
            try
            {
                DateTime fecha = new DateTime();
                CultureInfo enUS = new CultureInfo("en-US");
                if (!DateTime.TryParseExact(txtFechaProceso.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA PROCESO DEBE SER Mes/dia/Anio {0}", txtFechaProceso.Text));
                    txtFechaProceso.Focus();
                    this.btnGrabar.Attributes.Remove("disabled");
                    return;
                }
                int fechaDocumento = int.Parse(fecha.ToString("yyyyMMdd"));
                //###########################################################
                // Asignar los datos de capacidad por hora al grid del popup
                //###########################################################
                var oCapacidaPorHora = BAN_Capacidad_Hora.ConsultarConfiguracionCapacidadPorNave(txtNave.Text, out OError);

                if (oCapacidaPorHora.Where(p => p.idNave == txtNave.Text).Count() == 0)
                {
                    this.Alerta("No se ha configurado la capacidad por hora de la nave " + txtNave.Text + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se ha configurado la capacidad por hora de la nave " + txtNave.Text + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                    cmbConsignatario.Focus();
                    return;
                }
                else
                {
                    dgvCapacidadHora.DataSource = oCapacidaPorHora;
                    dgvCapacidadHora.DataBind();
                    UPMENSAJE2.Update();
                    //this.btnInfoCapacidadHora.Attributes.Remove("disabled");
                }

                var ResultadoCab = BAN_Loading_Program_Cab.GetLoadingProgramCabEspecifico(null ,txtNave.Text, int.Parse(cmbLinea.SelectedValue), fechaDocumento);
                if (ResultadoCab != null)
                {
                    var ResultadoDet = BAN_Loading_Program_Det.ConsultarListadoDetalleLoading(ResultadoCab.idLoadingCab, out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TXTMRN.Focus();
                        return;
                    }
                    
                    var oHold = BAN_Catalogo_Hold.ConsultarListaHold(out OError);
                    var oPiso = BAN_Catalogo_Piso.ConsultarListaPisos(out OError);
                    var oCargo = BAN_Catalogo_Cargo.ConsultarListaCargos(out OError);
                    var oMarca = BAN_Catalogo_Marca.ConsultarListaMarca("CGSA", out OError);
                    var oExportador = BAN_Catalogo_Exportador.ConsultarListaExportador("CGSA", out OError);
                    var oConsignatario = BAN_Catalogo_Consignatario.ConsultarListaConsignatarios("CGSA", out OError);


                    foreach (var a in ResultadoDet)
                    {
                        //a.HoraInicio = BAN_HorarioInicial.GetHorarioInicio(a.idHoraInicio);
                        //a.HoraFin = BAN_HorarioFinal.GetHorarioFinal(a.idHoraFin);
                        a.oHold = oHold.Where(p => p.id == a.idHold).FirstOrDefault();
                        a.oPiso = oPiso.Where(p => p.id == a.idPiso).FirstOrDefault();
                        a.oCargo = oCargo.Where(p => p.id == a.idCargo).FirstOrDefault();
                        a.oMarca = oMarca.Where(p => p.id == a.idMarca).FirstOrDefault();
                        a.oExportador = oExportador.Where(p => p.id == a.idExportador).FirstOrDefault();
                        a.oConsignatario = oConsignatario.Where(p => p.id == a.idConsignatario).FirstOrDefault();
                        a.ListaAISV = BAN_Loading_Program_Aisv.ConsultarListadoAISV(long.Parse(a.idLoadingDet.ToString()), out OError);
                        a.Cabecera = ResultadoCab;
                    }
                    ResultadoCab.Detalle = ResultadoDet;
                    ResultadoCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusiones(ResultadoCab.idLoadingCab, out OError);

                    objLoadingCab = ResultadoCab;
                    Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] = objLoadingCab;

                    string Valor;
                    try { Valor = BAN_Loading_Program_Cab.GetConfiguracion("BAN", "HORA_CIERRE_VBS_BAN"); } catch { Response.Redirect("../login.aspx", false); return; }

                    if (string.IsNullOrEmpty(Valor))
                    {
                        this.btnAutorizar.Attributes["disabled"] = "disabled";
                        this.Alerta("No se puede editar, el parametro de hora permitida para actualizar no esta configurada");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el parametro de hora permitida para actualizar no esta configurada."));
                        this.txtDetFecha.Focus();

                        this.Actualiza_Paneles();
                        return;
                    }
                    else
                    {
                        long v_FechaHoraActual = long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"));
                        long v_FechaHoraPermitida = long.Parse(objLoadingCab.fechaDocumento.ToString() + Valor);

                        if (v_FechaHoraActual > v_FechaHoraPermitida)
                        {
                            this.btnAutorizar.Attributes.Remove("disabled");
                            this.Alerta("El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                            this.btnAutorizarEdicion.Focus();
                            this.Actualiza_Paneles();
                        }
                        else
                        {
                            this.btnAutorizar.Attributes["disabled"] = "disabled";
                        }
                    }

                    //##########################################
                    // Asignar los datos agrupados al Repeater
                    //##########################################
                    var datosAgrupados = ResultadoCab.Detalle.GroupBy(o => o.oHold.nombre)
                                                        .Select(group => new
                                                        {
                                                            Hold = group.Key,
                                                            TotalBoxes = group.Sum(o => o.box)
                                                        });

                    
                    // Calcular y mostrar el total de "BOXES"
                    int totalBoxes = datosAgrupados.Sum(group => group.TotalBoxes);
                    Session["Transaccion_BAN_Loading_Program_Cab_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

                    if (datosAgrupados != null && datosAgrupados.Count() > 0)
                    {
                        dgvTotales.DataSource = datosAgrupados;
                        dgvTotales.DataBind();
                        //this.btnGrabar.Attributes.Remove("disabled");
                    }
                    else
                    {
                        dgvTotales.DataSource = null;
                        dgvTotales.DataBind();
                        //this.btnGrabar.Attributes["disabled"] = "disabled";
                    }

                    //##########################################
                    // Asignar los datos grabados al Repeater
                    //##########################################
                    var LinqQuery = from Tbl in ResultadoCab.Detalle.Where(Tbl => !String.IsNullOrEmpty(Tbl.horaInicio))
                                    select new
                                    {
                                        id = Tbl.idLoadingDet,
                                        fecha = Tbl.fecha.ToString("dd/MM/yyyy"),
                                        time = string.Format("{0} - {1}", Tbl.horaInicio.Trim(), Tbl.horaFin.Trim()),
                                        box = Tbl.box,
                                        deck = Tbl.oPiso?.nombre,
                                        idHold = Tbl.idHold,
                                        Hold = Tbl.oHold?.nombre.Trim(),
                                        cargo = Tbl.oCargo?.nombre.Trim(),
                                        marca = Tbl.oMarca?.nombre.Trim(),
                                        exportador = Tbl.oExportador?.nombre.Trim(),
                                        consignatario = Tbl.oConsignatario?.nombre.Trim(),
                                        aisv = Tbl.aisv,
                                        estado = Tbl.estado,
                                        usuarioCrea = Tbl.usuarioCrea.Trim(),
                                        fechaCreacion = Tbl.fechaCreacion.ToString("dd/MM/yyyy HH:mm"),
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

                    dgvTotales.DataSource = null;
                    dgvTotales.DataBind();

                    sinresultado.Visible = true;
                }
                this.btnInfoCapacidadHora.Attributes.Remove("disabled");
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
        private void ConsultarDataAISVGenerados(long idTurno)
        {
            try
            {
                SinResultadoAISV.Visible = false;
                var ResultadoDet = BAN_AISV_Generados.ConsultarListadoAISVGenerados(idTurno, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    string Mensaje = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.msjErrorLiquidacion.Visible = true;
                    this.msjErrorLiquidacion.InnerHtml = Mensaje;
                    UPAISV.Update();
                    return;
                }

                if (ResultadoDet != null)
                {
                    objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                    objLoadingCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusiones(objLoadingCab.idLoadingCab, out OError);

                    Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] = objLoadingCab;

                    if (ResultadoDet.Count == 0)
                    {
                        dgvAisv.DataSource = null;
                        dgvAisv.DataBind();
                        SinResultadoAISV.Visible = true;
                    }
                    else
                    {
                        dgvAisv.DataSource = ResultadoDet;
                        dgvAisv.DataBind();
                    }
                }
                else
                {
                    dgvAisv.DataSource = null;
                    dgvAisv.DataBind();
                    SinResultadoAISV.Visible = true;
                }

                
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataAISVGenerados), "ConsultarListadoAISVGenerados", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
            UPAISV.Update();
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
        private void Carga_CboHorarioFinal(int idcarga)
        {
            try
            {
                List<BAN_HorarioFinal> Listado = BAN_HorarioFinal.ConsultarHorarioFinal(out cMensajes);
                var list = Listado.Where(x => x.Id_HorarioIni >= idcarga).ToList();
                this.cboHorarioFinal.DataSource = list;
                this.cboHorarioFinal.DataTextField = "Desc_Hora";
                this.cboHorarioFinal.DataValueField = "Id_Hora";
                this.cboHorarioFinal.DataBind();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboHorarioFinal", "Hubo un error al cargar Tipo Contenedor", t.loginname));
                this.Mostrar_Mensaje( Error);
            }
        }
        private void Carga_cmbDetHoraFin(int idcarga)
        {
            try
            {
                List<BAN_HorarioFinal> Listado = BAN_HorarioFinal.ConsultarHorarioFinal(out cMensajes);

                var list = Listado.Where(x => x.Id_HorarioIni == idcarga).ToList();

                this.cmbDetHoraFin.DataSource = list;
                this.cmbDetHoraFin.DataTextField = "Desc_Hora";
                this.cmbDetHoraFin.DataValueField = "Id_Hora";
                this.cmbDetHoraFin.DataBind();
                UPEDIT.Update();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_cmbDetHoraFin", "Hubo un error al cargar Tipo Contenedor", t.loginname));
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
                    this.btnAutorizar.Attributes["disabled"] = "disabled";
                    this.btnGrabar.Attributes["disabled"] = "disabled";
                    this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled"; 
                    sinresultado.Visible = false;
                    SinResultadoAISV.Visible = false;
                    
                    LlenaComboLinea();
                    LlenaComboHold();
                    LlenaComboPiso();
                    LlenaComboCargo();
                    LlenaComboMarcas();
                    LlenaComboExportador();
                    LlenaComboConsignatario();
                    this.LlenarComboFechaInicial();
                    this.LlenarComboDetFechaInicial();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";
                    cmbCargo.Items.Add(item);
                    cmbConsignatario.Items.Add(item);
                    cmbExportador.Items.Add(item);
                    cmbHold.Items.Add(item);
                    //cmbLinea.Items.Add(item);
                    cmbMarca.Items.Add(item);
                    cmbPiso.Items.Add(item);
                    cboHorarioInicial.Items.Add(item);

                    cmbCargo.SelectedValue = "0";
                    cmbConsignatario.SelectedValue = "0";
                    cmbExportador.SelectedValue = "0";
                    cmbHold.SelectedValue = "0";
                    //cmbLinea.SelectedValue = "0";
                    cmbMarca.SelectedValue = "0";
                    cmbPiso.SelectedValue = "0";
                    cboHorarioInicial.SelectedValue = "0";
                    

                    if (Txtruc.Text != "0992506717001") //ruc de cgsa
                    {
                        btnAutorizar.Visible = false;
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
                        objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                        if (objLoadingCab == null) {
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

                        //var oDet = objLoadingCab.Detalle.Where(a => a.idLoadingDet == id).FirstOrDefault();
                        var oDet = BAN_Loading_Program_Det.GetDetalleEspecifico(long.Parse(id.ToString()));

                        hdf_CodigoCab.Value = objLoadingCab.idLoadingCab.ToString();
                        hdf_CodigoDet.Value = id.ToString();
                        txtDetFecha.Text = oDet.fecha.ToString("MM/dd/yyyy");
                        cmbDetHoraInicio.SelectedIndex = 0;
                        try { cmbDetHoraInicio.SelectedValue = oDet.idHoraInicio.ToString(); } catch { }
                        cmbDetHoraInicio_SelectedIndexChanged(null, null);
                        cmbDetHoraFin.SelectedValue = oDet.idHoraFin.ToString();
                        cmbDetHold.SelectedValue = oDet.idHold.ToString();
                        txtDetCantidad.Text = oDet.box.ToString();
                        cmbDetCargo.SelectedValue = oDet.idCargo.ToString();
                        cmbDetConsignatario.SelectedValue = oDet.idConsignatario.ToString();
                        cmbDetExportador.SelectedValue = oDet.idExportador.ToString();
                        cmbDetMarca.SelectedValue = oDet.idMarca.ToString();
                        cmbDetPiso.Text = oDet.idPiso.ToString();
                        txtDetobservacion.Text = oDet.comentario.ToString();

                        string Valor;
                        try { Valor = BAN_Loading_Program_Cab .GetConfiguracion("BAN", "HORA_CIERRE_VBS_BAN"); } catch { Response.Redirect("../login.aspx", false); return; }

                        if (string.IsNullOrEmpty(Valor))
                        {
                            this.btnActualizar.Attributes["disabled"] = "disabled";
                            this.Alerta("No se puede editar, el parametro de hora permitida para actualizar no esta configurada");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el parametro de hora permitida para actualizar no esta configurada."));
                            this.txtDetFecha.Focus();

                            UPEDIT.Update();
                            return;
                        }
                        else
                        {
                            long v_FechaHoraActual = long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"));
                            long v_FechaHoraPermitida = long.Parse(objLoadingCab.fechaDocumento.ToString() + Valor);

                            if (v_FechaHoraActual > v_FechaHoraPermitida)
                            {
                                this.btnActualizar.Attributes["disabled"] = "disabled";
                                this.Alerta("El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                                this.txtDetFecha.Focus();
                                UPEDIT.Update();
                                return;
                            }
                            else
                            {
                                this.btnActualizar.Attributes.Remove("disabled");
                            }
                        }

                        msjErrorDetalle.Visible = false;
                        UPEDIT.Update();

                        //Actualiza_Paneles();
                    }

                    if (e.CommandName == "Quitar")
                    {
                        Ocultar_Mensaje();
                        msjErrorDetalle.Visible = false;

                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                        var oDet = BAN_Loading_Program_Det.GetDetalleEspecifico(long.Parse(v_ID.ToString()));
                        Session["TransaccionBAN_Loading_Program_Det" + this.hf_BrowserWindowName.Value] = oDet;
                        //Actualiza_Paneles();
                    }

                    if (e.CommandName == "Aisv")
                    {
                        //Ocultar_Mensaje();
                        msjErrorLiquidacion.Visible = false;
                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                        objLoadingCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusiones(objLoadingCab.idLoadingCab, out OError);

                        var oDet = BAN_Loading_Program_Det.GetDetalleEspecifico(long.Parse(v_ID.ToString()));
                        oDet.Cabecera = objLoadingCab;
                        Session["TransaccionBAN_Loading_Program_Det" + this.hf_BrowserWindowName.Value] = oDet;

                        if (objLoadingCab == null) {
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
                        txtIdDetalle.Text = oDet.idLoadingDet.ToString();
                        txtDetAisvFecha.Text = oDet.fecha.ToString("MM/dd/yyyy");
                        txtDetAisvTime.Text = oDet.horaInicio.ToString() + " - " + oDet.horaFin.ToString();
                        oDet.oHold = BAN_Catalogo_Hold.GetHold(oDet.idHold);
                        txtDetAisvHold.Text = oDet.oHold?.nombre.ToString();
                        txtDetAisvCantidad.Text = oDet.box.ToString();
                        UPAISV.Update();
                        ConsultarDataAISVGenerados(v_ID);
                        UPAISV.Update();
                        //Actualiza_Paneles();
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
                totalBoxes = int.Parse(Session["Transaccion_BAN_Loading_Program_Cab_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int holdValue = int.Parse(DataBinder.Eval(e.Item.DataItem, "idHold").ToString());
                Label lblHold = e.Item.FindControl("lblHold") as Label;
                Button btnServicioDet = e.Item.FindControl("btnServicio") as Button;
                lblHold.Font.Bold = true;

                if (holdValue % 2 == 0)
                {
                    lblHold.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblHold.ForeColor = System.Drawing.Color.Blue;
                }

                if (Txtruc.Text != "0992506717001") //ruc de cgsa
                {
                    btnServicioDet.Visible = false;
                }

                //#######################################################################
                // VALIDACION DE QUE SI HAY AISV GENERADOS NO PERMITA EDITAR NI ELIMINAR
                //#######################################################################
                Button btnEdicion = e.Item.FindControl("btnEditar") as Button;
                Button btnbtnQuitar = e.Item.FindControl("btnQuitar") as Button;
                Label lblAisv = e.Item.FindControl("lblAisv") as Label;

                objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;

                int v_id = int.Parse(DataBinder.Eval(e.Item.DataItem, "id").ToString());

                var oDetalle = objLoadingCab.Detalle.Where(p => p.idLoadingDet == v_id).FirstOrDefault();

                if (oDetalle.ListaAISV.Count > 0)
                {
                    btnEdicion.Enabled = false;
                    btnbtnQuitar.Enabled = false;
                    lblAisv.Text = string.Empty;
                    foreach (var item in oDetalle.ListaAISV)
                    {
                        lblAisv.Text = lblAisv.Text + "  " + item.aisv;
                    }
                }
                
                this.Actualiza_Panele_Detalle();
            }
        }

        protected void dgvAisv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    this.msjErrorLiquidacion.Visible = false;
                    //UPAISV.Update();
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

                    
                    if (e.CommandName == "Autorizar")
                    {
                        //Ocultar_Mensaje();
                        var coman = e.CommandArgument.ToString();


                        string[] v_valores = coman.Split(',');


                        long v_ID = long.Parse(v_valores[0]);
                        this.lblAISV.InnerText = v_valores[1];
                        Session["lblAISV" + this.hf_BrowserWindowName.Value] = v_valores[1];

                        if (v_ID <= 0) { return; }
                        objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                        var oDet = BAN_Loading_Program_Det.GetDetalleEspecifico(long.Parse(v_ID.ToString()));
                        Session["TransaccionBAN_Loading_Program_Det" + this.hf_BrowserWindowName.Value] = oDet;

                        if (objLoadingCab == null)
                        {
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

                        //UPAUTORIZAR.Update();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "Editar", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    this.msjErrorLiquidacion.Visible = true;
                    this.msjErrorLiquidacion.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                    return;
                }
            }
        }

        protected void dgvAisv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnAutorizarIngreso = (Button)e.Row.FindControl("btnAutorizarIng");

                objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;

                string v_id = DataBinder.Eval(e.Row.DataItem, "aisv_codigo").ToString().Trim();

                if (string.IsNullOrEmpty(v_id))
                {
                    btnAutorizarIngreso.Enabled = false;
                    return;
                }

                if (objLoadingCab.Exclusiones?.Where(p=> p.aisv == v_id).Count() > 0) 
                {
                    btnAutorizarIngreso.Enabled = false;
                }
                else
                {
                    e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                }

                this.Actualiza_Panele_Detalle();
            }
        }
        #endregion
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes["disabled"] = "disabled";
            this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled";
            Limpia_Datos_cliente();
            //objDetalle.Clear();
            objLoadingCab = new BAN_Loading_Program_Cab();
            objLoadingDet = new List<BAN_Loading_Program_Det>();
            objLoadingCab.Detalle = objLoadingDet;
            Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] = objLoadingCab;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            dgvTotales.DataSource = null;
            dgvTotales.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes["disabled"] = "disabled";
            this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled";
            sinresultado.Visible = false;
            ValidarDatosNave();

            //objDetalle.Clear();
            //objLoadingCab.Detalle = objDetalle;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            dgvTotales.DataSource = null;
            dgvTotales.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
            if (Response.IsClientConnected)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.txtFechaProceso.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar la fecha proceso"));
                        this.txtNave.Focus();
                        return;
                    }

                    DateTime fecha = new DateTime();
                    CultureInfo enUS = new CultureInfo("en-US");
                    if (!DateTime.TryParseExact(txtFechaProceso.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                    {
                        this.Alerta(string.Format("EL FORMATO DE FECHA PROCESO DEBE SER Mes/dia/Anio {0}", txtFechaProceso.Text));
                        txtFechaProceso.Focus();
                        this.btnGrabar.Attributes.Remove("disabled");
                        return;
                    }

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

                    objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                    if (objLoadingCab != null)
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

                        if (string.IsNullOrEmpty(this.txtBox.Text) || (txtBox.Text == "0"))
                        {
                            this.Alerta("Ingrese la cantidad.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese la cantidad."));
                            this.txtBox.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(this.TxtFechaDesde.Text))
                        {
                            this.Alerta("Ingrese el campo fecha.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la fecha"));
                            this.TxtFechaDesde.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        DateTime fechaProceso = new DateTime();
                        CultureInfo enUS = new CultureInfo("en-US");
                        if (!DateTime.TryParseExact(txtFechaProceso.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechaProceso))
                        {
                            this.Alerta(string.Format("EL FORMATO DE FECHA POCESO DEBE SER Mes/dia/Anio {0}", txtFechaProceso.Text));
                            txtFechaProceso.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        DateTime fecha = new DateTime();
                        //CultureInfo enUS = new CultureInfo("en-US");
                        if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                        {
                            this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER Mes/dia/Anio {0}", TxtFechaDesde.Text));
                            TxtFechaDesde.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cboHorarioInicial.SelectedValue) || (cboHorarioInicial.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un horario inicial.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un horario inicial."));
                            cboHorarioInicial.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cboHorarioFinal.SelectedValue) || (cboHorarioFinal.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un horario final.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un horario final."));
                            cboHorarioFinal.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbHold.SelectedValue) || (cmbHold.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un hold.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un hold."));
                            cmbHold.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbPiso.SelectedValue) || (cmbPiso.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un piso.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un piso."));
                            cmbPiso.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbCargo.SelectedValue) || (cmbCargo.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un cargo.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un cargo."));
                            cmbCargo.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbMarca.SelectedValue) || (cmbMarca.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione una marca.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una marca."));
                            cmbMarca.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbExportador.SelectedValue) || (cmbExportador.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un exportador.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un exportador."));
                            cmbExportador.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbConsignatario.SelectedValue) || (cmbConsignatario.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un consignatario.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un consignatario."));
                            cmbConsignatario.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }
                        //##########################################################################
                        //VALIDACIONES GENERALES : 1.- EXISTA CONFIGURACIÓN DE CAPACIDAD POR HORA
                        //##########################################################################
                        var oCapacidaPorHora = BAN_Capacidad_Hora.ConsultarConfiguracionCapacidadPorNave(txtNave.Text, out OError);

                        if (oCapacidaPorHora.Where(p => p.idNave == txtNave.Text).Count() == 0)
                        {
                            this.Alerta("No se ha configurado la capacidad por hora de la nave " + txtNave.Text + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se ha configurado la capacidad por hora de la nave " + txtNave.Text + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                            cmbConsignatario.Focus();
                            this.btnGrabar.Attributes.Remove("disabled");
                            this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled";
                            return;
                        }

                        //#################################################################################################
                        //VALIDACIONES GENERALES : 2.1.- EXISTA CONFIGURACIÓN DE CAPACIDAD POR HORA PARA EL HOLD ELEGIDO
                        //#################################################################################################
                        try
                        {
                            var oCapacidadHoraEsp = oCapacidaPorHora.Where(p => p.idNave == txtNave.Text && p.idHold == int.Parse(cmbHold.SelectedValue)).FirstOrDefault();

                            if(oCapacidadHoraEsp == null)
                            {
                                this.Alerta("No se ha configurado la capacidad por hora para el HOLD [" + cmbHold.SelectedItem.ToString() + "]. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se ha configurado la capacidad por hora para el HOLD " + cmbHold.SelectedItem.ToString() + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                                cmbConsignatario.Focus();
                                this.btnGrabar.Attributes.Remove("disabled");
                                return;
                            }

                        } catch { }

                        
                        List<BAN_HorarioFinal> Listado = BAN_HorarioFinal.ConsultarHorarioFinal(out cMensajes);
                        var list = Listado.Where(x => x.Id_HorarioIni >= int.Parse(cboHorarioInicial.SelectedValue.ToString()) && x.Id_HorarioIni <= int.Parse(cboHorarioFinal.SelectedValue.ToString())).ToList();
                        foreach (var oEntidad in list)
                        {
                            oEntidad.oHoraInicial = BAN_HorarioInicial.GetHorarioInicio(oEntidad.Id_HorarioIni);
                            //##########################################################################
                            //VALIDACIONES GENERALES : 2.- NO SOBREPASE LA CAPACIDAD HORA POR HOLD
                            //##########################################################################
                            var oCapacidadHoraXHold = oCapacidaPorHora.Where(p => p.idNave == txtNave.Text && p.idHold == int.Parse(cmbHold.SelectedValue)).FirstOrDefault().box;
                            var oDetalleAgregados = objLoadingCab.Detalle.Where(p => p.idHold == int.Parse(cmbHold.SelectedValue) && p.fecha == fecha && p.idHoraInicio == oEntidad.Id_HorarioIni).Sum(item => item.box);

                            var oDisponibles = oDetalleAgregados;
                            oDetalleAgregados = oDetalleAgregados + int.Parse(txtBox.Text);

                            if (oDetalleAgregados > oCapacidadHoraXHold)
                            {
                                this.Alerta("Ha sobrepasado la capacidad por hora del Hold [Reservado " + oDisponibles.ToString() + " de " + oCapacidadHoraXHold.ToString() + " Cajas por hora]. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Ha sobrepasado la capacidad por hora del  Hold [Reservado " + oDisponibles.ToString() + " de " + oCapacidadHoraXHold.ToString() + " Cajas por hora]. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                                cmbConsignatario.Focus();
                                this.btnGrabar.Attributes.Remove("disabled");
                                return;
                            }

                            //VALIDACIONES GENERALES : 2.- NO INGRESE REGISTROS REPETIDO
                            //var oRepetidos = objLoadingCab.Detalle.Where(p => p.idHold == int.Parse(cmbHold.SelectedValue) && p.idHoraInicio == int.Parse(cboHorarioInicial.SelectedValue) && p.fecha.ToString("MM/dd/yyyy") == TxtFechaDesde.Text && p.idExportador == int.Parse(cmbExportador.SelectedValue) ).Count();

                            //if (oRepetidos > 0)
                            //{

                            //}

                            if (objLoadingCab.idLoadingCab > 0)
                            {
                                //###########################################################################################
                                //VALIDACIONES GENERALES : VALIDA QUE EL TIEMPO PERMITIDO PARA ACT EL LOADING ESTE VIGENTE
                                //###########################################################################################
                                string Valor;
                                try { Valor = BAN_Loading_Program_Cab.GetConfiguracion("BAN", "HORA_CIERRE_VBS_BAN"); } catch { Response.Redirect("../login.aspx", false); return; }

                                if (string.IsNullOrEmpty(Valor))
                                {
                                    this.btnActualizar.Attributes["disabled"] = "disabled";
                                    this.Alerta("No se puede editar, el parametro de hora permitida para actualizar no esta configurada");
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el parametro de hora permitida para actualizar no esta configurada."));
                                    this.txtDetFecha.Focus();

                                    UPEDIT.Update();
                                    return;
                                }
                                else
                                {
                                    //var oDet = objLoadingCab.Detalle.Where(a => a.idLoadingDet == long.Parse(hdf_CodigoDet.Value)).FirstOrDefault();
                                    long v_FechaHoraActual = long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"));
                                    long v_FechaHoraPermitida = long.Parse(objLoadingCab.fechaDocumento.ToString() + Valor);

                                    if (v_FechaHoraActual > v_FechaHoraPermitida)
                                    {
                                        this.btnActualizar.Attributes["disabled"] = "disabled";
                                        this.Alerta("El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                                        this.txtDetFecha.Focus();
                                        UPEDIT.Update();
                                        return;
                                    }
                                    else
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
                                    }
                                }

                                if (objLoadingCab.Detalle.Count > -1)
                                {
                                    BAN_Loading_Program_Det oBAN_LoadingDet = new BAN_Loading_Program_Det();
                                    oBAN_LoadingDet.idLoadingCab = objLoadingCab.idLoadingCab;
                                    oBAN_LoadingDet.fecha = fecha;
                                    oBAN_LoadingDet.idHoraInicio = oEntidad.Id_HorarioIni;// int.Parse(cboHorarioInicial.SelectedValue);
                                    oBAN_LoadingDet.horaInicio = oEntidad?.oHoraInicial?.Desc_Hora;//cboHorarioInicial.SelectedItem.Text;
                                    oBAN_LoadingDet.idHoraFin = int.Parse(oEntidad.Id_Hora);//int.Parse(cboHorarioFinal.SelectedValue);
                                    oBAN_LoadingDet.horaFin = oEntidad.Desc_Hora;//cboHorarioFinal.SelectedItem.Text;
                                    oBAN_LoadingDet.idHold = int.Parse(cmbHold.SelectedValue);
                                    oBAN_LoadingDet.idPiso = int.Parse(cmbPiso.SelectedValue);
                                    oBAN_LoadingDet.box = int.Parse(txtBox.Text);
                                    oBAN_LoadingDet.idCargo = int.Parse(cmbCargo.SelectedValue);
                                    oBAN_LoadingDet.idExportador = int.Parse(cmbExportador.SelectedValue);
                                    oBAN_LoadingDet.idMarca = int.Parse(cmbMarca.SelectedValue);
                                    oBAN_LoadingDet.idConsignatario = int.Parse(cmbConsignatario.SelectedValue);
                                    oBAN_LoadingDet.comentario = string.Empty;
                                    oBAN_LoadingDet.estado = true;
                                    oBAN_LoadingDet.fechaDocumento = objLoadingCab.fechaDocumento;
                                    oBAN_LoadingDet.usuarioCrea = ClsUsuario.loginname;
                                    oBAN_LoadingDet.idLoadingDet = oBAN_LoadingDet.Save_Update(out OError);

                                    if (OError != string.Empty)
                                    {
                                        this.Alerta(OError);
                                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                        this.TXTMRN.Focus();
                                        this.btnGrabar.Attributes.Remove("disabled");
                                        return;
                                    }
                                    else
                                    {
                                        btnBuscar_Click(null, null);
                                        this.Alerta("Transacción exitosa");
                                        TxtFechaDesde.Focus();
                                    }
                                }
                            }
                            else
                            {
                                objLoadingCab.idLoadingCab = 0;
                                objLoadingCab.idNave = txtNave.Text;
                                objLoadingCab.nave = txtDescripcionNave.Text;
                                objLoadingCab.idLinea = int.Parse(cmbLinea.SelectedValue);
                                objLoadingCab.linea = cmbLinea.SelectedItem.ToString();
                                objLoadingCab.estado = true;
                                objLoadingCab.fechaDocumento = int.Parse(fechaProceso.ToString("yyyyMMdd"));
                                objLoadingCab.usuarioCrea = ClsUsuario.loginname;


                                using (var scope = new TransactionScope())
                                {
                                    var id = objLoadingCab.Save_Update(out OError);

                                    if (string.IsNullOrEmpty(OError))
                                    {
                                        BAN_Loading_Program_Det oBAN_LoadingDet = new BAN_Loading_Program_Det();
                                        oBAN_LoadingDet.idLoadingCab = id;
                                        oBAN_LoadingDet.fecha = fecha;
                                        oBAN_LoadingDet.idHoraInicio = oEntidad.Id_HorarioIni;//int.Parse(cboHorarioInicial.SelectedValue);
                                        oBAN_LoadingDet.horaInicio = oEntidad?.oHoraInicial?.Desc_Hora; //cboHorarioInicial.SelectedItem.Text;
                                        oBAN_LoadingDet.idHoraFin = int.Parse(oEntidad.Id_Hora); //int.Parse(cboHorarioFinal.SelectedValue);
                                        oBAN_LoadingDet.horaFin = oEntidad.Desc_Hora; //cboHorarioFinal.SelectedItem.Text;
                                        oBAN_LoadingDet.idHold = int.Parse(cmbHold.SelectedValue);
                                        oBAN_LoadingDet.idPiso = int.Parse(cmbPiso.SelectedValue);
                                        oBAN_LoadingDet.box = int.Parse(txtBox.Text);
                                        oBAN_LoadingDet.idCargo = int.Parse(cmbCargo.SelectedValue);
                                        oBAN_LoadingDet.idExportador = int.Parse(cmbExportador.SelectedValue);
                                        oBAN_LoadingDet.idMarca = int.Parse(cmbMarca.SelectedValue);
                                        oBAN_LoadingDet.idConsignatario = int.Parse(cmbConsignatario.SelectedValue);
                                        oBAN_LoadingDet.comentario = string.Empty;
                                        oBAN_LoadingDet.estado = true;
                                        oBAN_LoadingDet.fechaDocumento = objLoadingCab.fechaDocumento;
                                        oBAN_LoadingDet.usuarioCrea = ClsUsuario.loginname;
                                        oBAN_LoadingDet.idLoadingDet = oBAN_LoadingDet.Save_Update(out OError);

                                        if (string.IsNullOrEmpty(OError))
                                        {
                                            objLoadingCab.Detalle.Add(oBAN_LoadingDet);
                                            scope.Complete();
                                        }
                                    }
                                }

                                if (OError != string.Empty)
                                {
                                    this.Alerta(OError);
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                    this.TXTMRN.Focus();
                                    this.btnGrabar.Attributes.Remove("disabled");
                                    return;
                                }
                                else
                                {
                                    btnBuscar_Click(null, null);
                                    this.Alerta("Transacción exitosa");
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Configuración del loading de la nave {0} almacenada con exito", txtNave.Text.ToString()));
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
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e) 
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {
                    objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;

                    
                    if (objLoadingCab != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtDetFecha.Text))
                        {
                            this.Alerta("Ingrese el campo fecha.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la fecha"));
                            this.TxtFechaDesde.Focus();
                            return;
                        }

                        DateTime fecha = new DateTime();
                        CultureInfo enUS = new CultureInfo("en-US");
                        if (!DateTime.TryParseExact(txtDetFecha.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                        {
                            this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER Mes/dia/Anio {0}", fecETA.Text));
                            fecETA.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetHoraInicio.SelectedValue) || (cmbDetHoraInicio.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un horario inicial.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un horario inicial."));
                            cmbDetHoraInicio.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetHoraFin.SelectedValue) || (cmbDetHoraFin.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un horario final.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un horario final."));
                            cmbDetHoraFin.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetHold.SelectedValue) || (cmbDetHold.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un hold.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un hold."));
                            cmbDetHold.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(cmbDetPiso.SelectedValue) || (cmbDetPiso.SelectedValue == "0"))
                        {
                            this.Alerta("Seleccione un piso.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un piso."));
                            cmbDetPiso.Focus();
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


                        //##########################################################################
                        //VALIDACIONES GENERALES : 1.- EXISTA CONFIGURACIÓN DE CAPACIDAD POR HORA
                        //##########################################################################
                        var oCapacidaPorHora = BAN_Capacidad_Hora.ConsultarConfiguracionCapacidadPorNave(txtNave.Text, out OError);

                        if (oCapacidaPorHora.Where(p => p.idNave == txtNave.Text).Count() == 0)
                        {
                            this.Alerta("No se ha configurado la capacidad por hora de la nave " + txtNave.Text + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se ha configurado la capacidad por hora de la nave " + txtNave.Text + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                            cmbConsignatario.Focus();
                            this.btnActualizar.Attributes.Remove("disabled");
                            return;
                        }

                        //#################################################################################################
                        //VALIDACIONES GENERALES : 2.1.- EXISTA CONFIGURACIÓN DE CAPACIDAD POR HORA PARA EL HOLD ELEGIDO
                        //#################################################################################################
                        try
                        {
                            var oCapacidadHoraEsp = oCapacidaPorHora.Where(p => p.idNave == txtNave.Text && p.idHold == int.Parse(cmbDetHold.SelectedValue)).FirstOrDefault();

                            if (oCapacidadHoraEsp == null)
                            {
                                this.Alerta("No se ha configurado la capacidad por hora para el HOLD [" + cmbDetHold.SelectedItem.ToString() + "]. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se ha configurado la capacidad por hora para el HOLD " + cmbDetHold.SelectedItem.ToString() + ". Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                                cmbConsignatario.Focus();
                                this.btnActualizar.Attributes.Remove("disabled");
                                return;
                            }

                        }
                        catch { }

                        //##########################################################################
                        //VALIDACIONES GENERALES : 2.- NO SOBREPASE LA CAPACIDAD HORA POR HOLD
                        //##########################################################################
                        BAN_Loading_Program_Det oDetalle = new BAN_Loading_Program_Det();
                        oDetalle = objLoadingCab.Detalle.Where(a => a.idLoadingDet == long.Parse(hdf_CodigoDet.Value)).FirstOrDefault();

                        if (oDetalle == null) { Response.Redirect("../login.aspx", false); return; }
                        var oCapacidadHoraXHold = oCapacidaPorHora.Where(p => p.idNave == txtNave.Text && p.idHold == oDetalle.idHold).FirstOrDefault().box;
                        var oDetalleAgregados = objLoadingCab.Detalle.Where(p => p.idHold == oDetalle.idHold && p.fecha == fecha && p.idHoraInicio == oDetalle.idHoraInicio).Sum(item => item.box);

                        var oDisponibles = oDetalleAgregados;
                        oDetalleAgregados = (oDetalleAgregados - oDetalle.box) + int.Parse(txtDetCantidad.Text);

                        if (oDetalleAgregados > oCapacidadHoraXHold)
                        {
                            this.Alerta("Ha sobrepasado la capacidad por hora del Hold [Reservado " + oDisponibles.ToString() + " de " + oCapacidadHoraXHold.ToString() + " Cajas por hora]. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Ha sobrepasado la capacidad por hora del Hold [Reservado " + oDisponibles.ToString() + " de " + oCapacidadHoraXHold.ToString() + " Cajas por hora]. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                            cmbConsignatario.Focus();
                            this.btnActualizar.Attributes.Remove("disabled");
                            return;
                        }
                        

                        if (oDetalle != null)
                        {
                            var oDet = BAN_Loading_Program_Det.GetDetalleEspecifico(long.Parse(oDetalle.idLoadingDet.ToString()));
                            oDetalle = oDet;

                            string Valor;
                            try { Valor = BAN_Loading_Program_Cab.GetConfiguracion("BAN", "HORA_CIERRE_VBS_BAN"); } catch { Response.Redirect("../login.aspx", false); return; }

                            if (string.IsNullOrEmpty(Valor))
                            {
                                this.btnActualizar.Attributes["disabled"] = "disabled";
                                this.Alerta("No se puede editar, el parametro de hora permitida para actualizar no esta configurada");
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el parametro de hora permitida para actualizar no esta configurada."));
                                this.txtDetFecha.Focus();

                                UPEDIT.Update();
                                return;
                            }
                            else
                            {
                                //var oDet = objLoadingCab.Detalle.Where(a => a.idLoadingDet == long.Parse(hdf_CodigoDet.Value)).FirstOrDefault();
                                long v_FechaHoraActual = long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"));
                                long v_FechaHoraPermitida = long.Parse(objLoadingCab.fechaDocumento.ToString() + Valor);

                                if (v_FechaHoraActual > v_FechaHoraPermitida)
                                {
                                    this.btnActualizar.Attributes["disabled"] = "disabled";
                                    this.Alerta("El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                                    this.txtDetFecha.Focus();
                                    UPEDIT.Update();
                                    return;
                                }
                                else
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

                                    oDetalle.fecha = fecha;
                                    oDetalle.idHoraInicio = int.Parse(cmbDetHoraInicio.SelectedValue);
                                    oDetalle.horaInicio = cmbDetHoraInicio.SelectedItem.Text;
                                    oDetalle.idHoraFin = int.Parse(cmbDetHoraFin.SelectedValue);
                                    oDetalle.horaFin = cmbDetHoraFin.SelectedItem.Text;
                                    oDetalle.idHold = int.Parse(cmbDetHold.SelectedValue);
                                    oDetalle.idPiso = int.Parse(cmbDetPiso.SelectedValue);
                                    oDetalle.box = int.Parse(txtDetCantidad.Text);
                                    oDetalle.idCargo = int.Parse(cmbDetCargo.SelectedValue);
                                    oDetalle.idExportador = int.Parse(cmbDetExportador.SelectedValue);
                                    oDetalle.idMarca = int.Parse(cmbDetMarca.SelectedValue);
                                    oDetalle.idConsignatario = int.Parse(cmbDetConsignatario.SelectedValue);
                                    oDetalle.comentario = txtDetobservacion.Text;
                                    oDetalle.estado = true;
                                    oDetalle.fechaDocumento = objLoadingCab.fechaDocumento;
                                    oDetalle.usuarioModifica = ClsUsuario.loginname;
                                    oDetalle.idLoadingDet = oDetalle.Save_Update(out OError);
                                    msjErrorDetalle.Visible = false;

                                    oDetalle.idLoadingDet = oDetalle.Save_Update(out OError);

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
                                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el registro  {0} ", oDetalle.idLoadingDet.ToString()));
                                        this.btnActualizar.Attributes["disabled"] = "disabled";
                                        //Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                                        hdf_CodigoCab.Value = null;
                                        hdf_CodigoDet.Value = null;
                                    }

                                }
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

            string Valor;
            try { Valor = BAN_Loading_Program_Cab.GetConfiguracion("BAN", "HORA_CIERRE_VBS_BAN"); } catch { Response.Redirect("../login.aspx", false); return; }

            if (string.IsNullOrEmpty(Valor))
            {
                this.btnActualizar.Attributes["disabled"] = "disabled";
                this.Alerta("No se puede editar, el parametro de hora permitida para actualizar no esta configurada");
                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el parametro de hora permitida para actualizar no esta configurada."));
                this.txtDetFecha.Focus();

                UPEDIT.Update();
                return;
            }
            else
            {
                objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                long v_FechaHoraActual = long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"));
                long v_FechaHoraPermitida = long.Parse(objLoadingCab.fechaDocumento.ToString() + Valor);

                if (v_FechaHoraActual > v_FechaHoraPermitida)
                {
                    //this.btnQuitar.Attributes["disabled"] = "disabled";
                    this.Alerta("El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El tiempo permitido para configurar el Loading ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                    OcultarLoading("1");
                    OcultarLoading("2");
                    Actualiza_Paneles();
                    return;
                }
                //else
                //{
                //    this.btnQuitar.Attributes.Remove("disabled");
                //}
            }

            var obj = Session["TransaccionBAN_Loading_Program_Det" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Det;
            BAN_Loading_Program_Det oDet = new BAN_Loading_Program_Det();
            oDet = BAN_Loading_Program_Det.GetDetalleEspecifico(long.Parse(obj.idLoadingDet.ToString()));
            Session["TransaccionBAN_Loading_Program_Det" + this.hf_BrowserWindowName.Value] = null;

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
            TxtFechaDesde.Text = string.Empty;
            txtBox.Text = string.Empty;
            cboHorarioInicial.SelectedValue = "0";
            cboHorarioInicial_SelectedIndexChanged(null,null);
            cmbCargo.SelectedValue = "0";
            cmbConsignatario.SelectedValue = "0";
            cmbExportador.SelectedValue = "0";
            cmbHold.SelectedValue = "0";
            //cmbLinea.SelectedValue = "0";
            cmbMarca.SelectedValue = "0";
            cmbPiso.SelectedValue = "0";
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


                   
                    objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                    if (objLoadingCab != null)
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
                    BAN_Loading_Program_Cab oCab = new BAN_Loading_Program_Cab();
                    oCab.idLoadingCab = objLoadingCab.idLoadingCab;
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
                        this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se autorizo la edición del loading exitosamente, usuario: {0} ", oCab.idLoadingCab.ToString()));
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

        protected void btnAutorizarIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    //sinresultado.Visible = false;
                    //Ocultar_Mensaje();
                    //UPAUTORIZAR.Update();
                    this.msjErrorLiquidacion.Visible = false;
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

                    if (string.IsNullOrEmpty(this.txtComentarioAutoriza.Text))
                    {
                        this.Alerta("Ingrese un comentario.");
                        this.txtComentarioAutoriza.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.lblAISV.InnerText))
                    {
                        this.Alerta("debe seleccionar un AISV.");
                        this.txtComentarioAutoriza.Focus();
                        return;
                    }

                    objLoadingCab = Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Cab;
                    if (objLoadingCab != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                           // this.btnGrabar.Attributes.Remove("disabled");
                            return;
                        }
                    }

                    OError = string.Empty;

                    BAN_Loading_Program_Det oDet = new BAN_Loading_Program_Det();
                    oDet = Session["TransaccionBAN_Loading_Program_Det" + this.hf_BrowserWindowName.Value] as BAN_Loading_Program_Det;
                    oDet.Cabecera = objLoadingCab;

                    lblAISV.InnerText = Session["lblAISV" + this.hf_BrowserWindowName.Value] as string;

                    BAN_Exclusion oExclusiones = new BAN_Exclusion();
                    oExclusiones.codigo = string.Empty;
                    oExclusiones.idLoadingCab = objLoadingCab.idLoadingCab;
                    oExclusiones.idLoadingDet = oDet.idLoadingDet;
                    oExclusiones.idMotivo = 0;
                    oExclusiones.aisv = lblAISV.InnerText;
                    oExclusiones.comentario = txtComentarioAutoriza.Text ;
                    oExclusiones.estado = true;
                    oExclusiones.usuarioCrea = ClsUsuario.loginname;
                        
                    var result = oExclusiones.Save_Autorizacion(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.msjErrorLiquidacion.Visible = true;
                        this.msjErrorLiquidacion.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                        return;
                    }
                    else
                    {
                        //btnBuscar_Click(null, null);
                        this.Alerta("Transacción exitosa");
                        ////this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el BL  {0} ", oDetalle.bl.ToString()));
                        //this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se autorizo la edición del loading exitosamente, usuario: {0} ", oCab.idLoadingCab.ToString()));
                        //this.btnActualizar.Attributes["disabled"] = "disabled";
                        Session["TransaccionBAN_Loading_Program_Det" + this.hf_BrowserWindowName.Value] = null;
                        //hdf_CodigoCab.Value = null;
                        //hdf_CodigoDet.Value = null;
                    }
                    ConsultarDataAISVGenerados(long.Parse(oDet.idLoadingDet.ToString()));
                }
                lblAISV.InnerText = string.Empty;
                txtComentarioAutoriza.Text = string.Empty;
                //UPAUTORIZAR.Update();
                
                UPAISV.Update();
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAutorizarIngreso_Click), "btnAutorizarIngreso_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                //this.Mostrar_MensajeDet(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        protected void cmbLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaComboConsignatario();
                LlenaComboExportador();
                LlenaComboMarcas();
                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";

                cmbConsignatario.Items.Add(item);
                cmbExportador.Items.Add(item);
                cmbMarca.Items.Add(item);

                cmbConsignatario.SelectedValue = "0";
                cmbExportador.SelectedValue = "0";
                cmbCargo.SelectedValue = "0";

                btnBuscar_Click(null, null);
            }
            catch
            {
                cmbExportador.SelectedValue = "0";
                cmbMarca.SelectedValue = "0";
                cmbConsignatario.SelectedValue = "0";
            }
        }

        protected void cboHorarioInicial_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCarga = int.Parse(cboHorarioInicial.SelectedValue);
            Carga_CboHorarioFinal(idCarga);
            this.Actualiza_Paneles();
        }

        protected void cmbDetHoraInicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCarga = int.Parse(cmbDetHoraInicio.SelectedValue);
            Carga_cmbDetHoraFin(idCarga);
            this.Actualiza_Paneles();
        }


        #endregion

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
                btnBuscar_Click(null,null);
                txtBox.Focus();
            }
            catch
            {

            }
        }
    }
}