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
    public partial class VBS_BAN_StowageAisvAdd : System.Web.UI.Page
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
            this.msjErrorAisv.Visible = true;
            this.msjErrorAisv.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }
        
        private void Ocultar_Mensaje()
        {
            this.msjErrorAisv.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.msjErrorAisv.Visible = false;
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
            Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] = objStowageCab;
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboLinea), "VBS_BAN_PreStowage.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
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
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboHold), "VBS_BAN_PreStowage.LlenaComboHold", false, null, null, ex.StackTrace, ex);
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
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboCargo), "VBS_BAN_PreStowage.LlenaComboCargo", false, null, null, ex.StackTrace, ex);
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
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboExportador), "VBS_BAN_Loading.LlenaComboConsignatario", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboBodega()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Bodega.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbBodega.DataSource = oEntidad;
                cmbBodega.DataValueField = "id";
                cmbBodega.DataTextField = "nombre";
                cmbBodega.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        private void ConsultarDataStowage()
        {
            try
            {
                DateTime fecha = new DateTime();
                CultureInfo enUS = new CultureInfo("en-US");
                if (!DateTime.TryParseExact(txtFechaProceso.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA PROCESO DEBE SER Mes/dia/Anio {0}", txtFechaProceso.Text));
                    txtFechaProceso.Focus();
                    return;
                }
                int fechaDocumento = int.Parse(fecha.ToString("yyyyMMdd"));
                //###########################################################
                // Asignar los datos de capacidad por hora al grid del popup
                //###########################################################
                /*                var oCapacidaPorHora = BAN_Capacidad_Hora.ConsultarConfiguracionCapacidadPorNave(txtNave.Text, out OError);

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
                */
                var ResultadoCab = BAN_Stowage_Plan_Cab.GetStowagePlanCabEspecifico(null, txtNave.Text, int.Parse(cmbLinea.SelectedValue), fechaDocumento);
                if (ResultadoCab != null)
                {
                    var ResultadoDet = BAN_Stowage_Plan_Det.ConsultarLista(ResultadoCab.idStowageCab, out OError);

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

                    foreach (var a in ResultadoDet)
                    {
                        //a.HoraInicio = BAN_HorarioInicial.GetHorarioInicio(a.idHoraInicio);
                        //a.HoraFin = BAN_HorarioFinal.GetHorarioFinal(a.idHoraFin);
                        a.oHold = oHold.Where(p => int.Parse(p.id.ToString()) == a.idHold).FirstOrDefault();
                        a.oEstado = oEstado.Where(p => p.id == a.estado).FirstOrDefault();
                        a.oCargo = oCargo.Where(p => p.id == a.idCargo).FirstOrDefault() ;
                        a.oMarca = oMarca.Where( p=> p.id == a.idMarca).FirstOrDefault();
                        a.oExportador = oExportador.Where(p=> p.id == a.idExportador).FirstOrDefault();
                        a.oConsignatario = oConsignatario.Where(p=> p.id == a.idConsignatario).FirstOrDefault();
                        a.ListaAISV = BAN_Stowage_Plan_Aisv.ConsultarLista(long.Parse(a.idStowageDet.ToString()), out OError);
                        try
                        {
                            a.oBloque = oBloque.Where(p=> p.id == a.idBloque).FirstOrDefault();
                            a.oBodega = oBodega.Where(p=> p.id == a.idBodega).FirstOrDefault();
                            a.oBloque.oBodega = a.oBodega;
                        }
                        catch { }
                        a.oStowage_Plan_Cab = ResultadoCab;
                    }
                    ResultadoCab.oDetalle = ResultadoDet;
                    ResultadoCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusionesST(ResultadoCab.idStowageCab, out OError);

                    objStowageCab = ResultadoCab;
                    Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] = objStowageCab;
                    /*
                    string Valor;
                    try { Valor = BAN_Stowage_Plan_Cab.GetConfiguracion("BAN", "HORA_CIERRE_VBS_BAN_BOD"); } catch { Response.Redirect("../login.aspx", false); return; }

                    if (string.IsNullOrEmpty(Valor))
                    {
                        this.Alerta("No se puede editar, el parametro de hora permitida para actualizar no esta configurada");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede actualizar, el parametro de hora permitida para actualizar no esta configurada."));
                        //this.txtDetFecha.Focus();

                        this.Actualiza_Paneles();
                        return;
                    }
                    else
                    {
                        long v_FechaHoraActual = long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"));
                        long v_FechaHoraPermitida = long.Parse(objStowageCab.fechaDocumento.ToString() + Valor);

                        if (v_FechaHoraActual > v_FechaHoraPermitida)
                        {
                            this.btnAutorizar.Attributes.Remove("disabled");
                            this.Alerta("El tiempo permitido para configurar el Stowage ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El tiempo permitido para configurar el Stowage ha concluido. Favor comunicarse a las casillas de multipropósito o redireccionar su requerimiento a breakbulk@cgsa.com.ec y dataentrycfs@cgsa.com.ec"));
                            this.btnAutorizarEdicion.Focus();
                            this.Actualiza_Paneles();
                        }
                        else
                        {
                            this.btnAutorizar.Attributes["disabled"] = "disabled";
                        }
                    }
                    */
                    //##########################################
                    // Asignar los datos agrupados al Repeater
                    //##########################################
                    var datosAgrupados = ResultadoCab.oDetalle.GroupBy(o => o.oHold.nombre)
                                                        .Select(group => new
                                                        {
                                                            Hold = group.Key,
                                                            TotalBoxes = group.Sum(o => o.boxSolicitado)
                                                        });


                    // Calcular y mostrar el total de "BOXES"
                    int? totalBoxes = datosAgrupados.Sum(group => group.TotalBoxes);
                    Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

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
                    var LinqQuery = from Tbl in ResultadoCab.oDetalle.Where(Tbl => !String.IsNullOrEmpty(Tbl.usuarioCrea))
                                    select new
                                    {
                                        id = Tbl.idStowageDet,
                                        //fecha = Tbl.fecha.ToString("dd/MM/yyyy"),
                                        //time = string.Format("{0} - {1}", Tbl.horaInicio.Trim(), Tbl.horaFin.Trim()),
                                        box = Tbl.boxSolicitado,
                                        deck = Tbl.piso,
                                        idHold = Tbl.idHold,
                                        Hold = Tbl.oHold?.nombre.Trim(),
                                        cargo = Tbl.oCargo?.nombre.Trim(),
                                        marca = Tbl.oMarca?.nombre.Trim(),
                                        exportador = Tbl.oExportador?.nombre.Trim(),
                                        consignatario = Tbl.oConsignatario?.nombre.Trim(),
                                        bodega = Tbl.oBodega?.nombre +" - " + Tbl.oBloque?.nombre,
                                        bloque = Tbl.oBloque?.nombre,
                                        estado = Tbl.oEstado?.nombre,
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
                    }
                    catch(Exception ex)
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

                    dgvTotales.DataSource = null;
                    dgvTotales.DataBind();

                    sinresultado.Visible = true;
                }
                //this.btnInfoCapacidadHora.Attributes.Remove("disabled");
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
        private void ConsultarDataAISVGenerados(long idStowageDet)
        {
            try
            {
                SinResultadoAISV.Visible = false;
                //var ResultadoDet = BAN_AISV_Generados.ConsultarListadoAISVGeneradosST(idTurno, out OError);
                var ResultadoDet = BAN_Stowage_Plan_Aisv.ConsultarLista(idStowageDet, out OError);
                var oEstado = BAN_Catalogo_Estado.ConsultarLista(out OError);
                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    string Mensaje = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = Mensaje;
                    UPAISV.Update();
                    return;
                }

                if (ResultadoDet != null)
                {
                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                    objStowageCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusionesST(objStowageCab.idStowageCab, out OError);

                    Session["Transaccion_BAN_Loading_Program_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;

                    if (ResultadoDet.Count == 0)
                    {
                        dgvAisv.DataSource = null;
                        dgvAisv.DataBind();
                        SinResultadoAISV.Visible = true;
                    }
                    else
                    {
                        var LinqQuery = from Tbl in ResultadoDet.Where(Tbl => !String.IsNullOrEmpty(Tbl.usuarioCrea))
                                        select new
                                        {
                                            vbs_id_hora_cita = Tbl.idStowagePlanTurno,
                                            aisv_codigo = Tbl.aisv,
                                            vbs_box = Tbl.box,
                                            aisv_numero_booking = Tbl.booking,
                                            aisv_dae = Tbl.dae,
                                            fecha = Tbl.fecha.ToShortDateString(),
                                            time = Tbl.horaInicio + " - " + Tbl.horaFin,
                                            aisv_cedul_chof = Tbl.idChofer,
                                            aisv_nombr_chof = Tbl.chofer,
                                            aisv_placa_vehi = Tbl.placa,
                                            aisv_estado = oEstado?.Where(p=> p.id == Tbl.estado).FirstOrDefault().nombre,
                                            usuarioCrea = Tbl.usuarioCrea.Trim()/*,
                                            fechaCreacion = Tbl.fechaCreacion.ToString("dd/MM/yyyy HH:mm"),*/
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            dgvAisv.DataSource = LinqQuery;
                            dgvAisv.DataBind();
                        }
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

        private void ConsultarDataAISV(string NumeroAISV)
        {
            try
            {
                SinResultadoAISV.Visible = false;
                var ResultadoDet = BAN_AISV_Generados.ConsultarListadoAISV(NumeroAISV, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    string Mensaje = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = Mensaje;
                    UPAISV.Update();
                    return;
                }

                if (ResultadoDet != null)
                {
                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

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

                        var oExportador = BAN_Catalogo_Exportador.GetExportaadorPorRucLinea(int.Parse(cmbLinea.SelectedValue), ResultadoDet.FirstOrDefault().aisv_codig_clte);

                        cmbExportador.SelectedValue = oExportador?.id.ToString();
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
                this.msjErrorAisv.Visible = IsPostBack;

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
                    SinResultadoAISV.Visible = false;

                    LlenaComboLinea();
                    LlenaComboHold();
                    LlenaComboCargo();
                    LlenaComboCargo();
                    LlenaComboMarcas();
                    LlenaComboExportador();
                    LlenaComboConsignatario();
                    LlenaComboBodega();
                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbCargo.Items.Add(item);
                    cmbConsignatario.Items.Add(item);
                    cmbExportador.Items.Add(item);
                    cmbHold.Items.Add(item);
                    cmbMarca.Items.Add(item);
                    cmbBodega.Items.Add(item);

                    cmbCargo.SelectedValue = "0";
                    cmbConsignatario.SelectedValue = "0";
                    cmbExportador.SelectedValue = "0";
                    cmbHold.SelectedValue = "0";
                    cmbMarca.SelectedValue = "0";
                    cmbBodega.SelectedValue = "0";
                    cmbBodega_SelectedIndexChanged(null, null);
                    if (Txtruc.Text != "0992506717001") //ruc de cgsa
                    {
                        this.cmbBodega.Attributes["disabled"] = "disabled";
                        this.cmbBloque.Attributes["disabled"] = "disabled";
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

                   
                    if (e.CommandName == "Aisv")
                    {
                        //Ocultar_Mensaje();
                        msjErrorAisv.Visible = false;
                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        objStowageCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusionesST(objStowageCab.idStowageCab, out OError);

                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(v_ID.ToString()));
                        oDet.oStowage_Plan_Cab = objStowageCab;
                        oDet.oExportador = BAN_Catalogo_Exportador.GetExportaador(oDet.idExportador);

                        int vReservado = 0;
                        try
                        {
                            var oTurnos = BAN_Stowage_Plan_Turno.ConsultarLista(long.Parse(oDet.idStowageDet.ToString()), out OError);
                            vReservado = oTurnos.Sum(P => P.box);
                            oDet.reservado = vReservado;
                            oDet.disponible = oDet.boxAutorizado - oDet.reservado;
                        }
                        catch
                        {
                            vReservado = 0;
                        }
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDet;

                        if (objStowageCab == null)
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

                        ConsultarDataAISVGenerados(v_ID);
                        UPAISV.Update();
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
                totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int holdValue = int.Parse(DataBinder.Eval(e.Item.DataItem, "idHold").ToString());
                Label lblHold = e.Item.FindControl("lblHold") as Label;
                Button btnServicioDet = e.Item.FindControl("btnServicios") as Button;
                Button btnPublica = e.Item.FindControl("btnPublicar") as Button;

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
                Button btnTurno = e.Item.FindControl("btnDetalle") as Button;

                objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

                int v_id = int.Parse(DataBinder.Eval(e.Item.DataItem, "id").ToString());
                var oDetalle = objStowageCab.oDetalle.Where(p => p.idStowageDet == v_id).FirstOrDefault();

                if (string.IsNullOrEmpty(oDetalle.oBloque?.nombre))
                {
                    btnTurno.Enabled = false;
                }

                if (oDetalle?.reservado == 0 || oDetalle?.reservado is null)
                {
                    btnPublica.Enabled = false;
                }

                if (oDetalle.estado == "PBL")
                {
                    btnPublica.Enabled = false;
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
                    this.msjErrorAisv.Visible = false;
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

                    if (e.CommandName == "Agregar")
                    {
                        var coman = e.CommandArgument.ToString();
                        string[] v_valores = coman.Split(',');
                        string v_exportador = v_valores[0];
                        string vAisv = v_valores[1];
                        string vCant = v_valores[2];
                        string vDae = v_valores[3];
                        string vBook = v_valores[4];
                        string vPlaca = v_valores[5];
                        string vChofer = v_valores[6];
                        string vIdChofer = v_valores[7];
                        Session["lblAISV" + this.hf_BrowserWindowName.Value] = v_valores[1];

                        if (string.IsNullOrEmpty(v_exportador)) { return; }

                        //VALIDA EXPORTADOR Y QUE EL AISV NO ESTE PREVIAMENTE INGRESADO.
                        var oExportador = BAN_Catalogo_Exportador.GetExportaadorPorRucLinea(int.Parse(cmbLinea.SelectedValue), v_exportador);

                        if (oExportador == null)
                        {
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>El exportador del AISV selecconado no existe,  ....{0}", "favor verificar"));
                            return;
                        }

                        var oAisv = BAN_Stowage_Plan_Aisv.ConsultarListaXAISV(vAisv, out OError);
                        if (oAisv?.Count > 0)
                        {
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>El AISV seleccionado ya ha sido registrado,  ....{0}", "favor verificar"));
                            return;
                        }

                        btnAutorizarIngreso_Click(vAisv,int.Parse(vCant),vDae,vBook,vPlaca,vChofer,vIdChofer);
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(dgvAisv_RowCommand), "dgvTurnos_RowCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                    return;
                }
            }
        }
        private int totalBoxes = 0;
        protected void dgvAisv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnAutorizarIngreso = (Button)e.Row.FindControl("btnAgregar");

                objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

                string v_id = DataBinder.Eval(e.Row.DataItem, "aisv_codigo").ToString().Trim();

                if (string.IsNullOrEmpty(v_id))
                {
                    btnAutorizarIngreso.Enabled = false;
                    return;
                }

                var oAisv = BAN_Stowage_Plan_Aisv.ConsultarListaXAISV(txtAISV.Text, out OError);

                if (oAisv?.Count > 0)
                {
                    btnAutorizarIngreso.Enabled = false;
                }
                else
                {
                    e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                }

                //CALCULA TOTALES
                int totalBoxesItem = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "vbs_box"));
                totalBoxes += totalBoxesItem;

                this.Actualiza_Panele_Detalle();
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalBoxesFooter = (Label)e.Row.FindControl("lblTotalBoxesFooter");
                if (lblTotalBoxesFooter != null)
                {
                    lblTotalBoxesFooter.Text = totalBoxes.ToString();
                }
            }
            UPAISV.Update();
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
            Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] = objStowageCab;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            dgvTotales.DataSource = null;
            dgvTotales.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //this.btnInfoCapacidadHora.Attributes["disabled"] = "disabled";
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
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtNave.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.txtNave.Focus();
                        return;
                    }


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
                btnBuscar_Click(null, null);
            }
            catch { }
        }
        #endregion

        protected void txtFechaProceso_TextChanged(object sender, EventArgs e)
        {
            try
            {
                tablePagination.DataSource = null;
                tablePagination.DataBind();

                dgvTotales.DataSource = null;
                dgvTotales.DataBind();

                sinresultado.Visible = true;
                this.Actualiza_Paneles();
                btnBuscar_Click(null, null);
            }
            catch
            {

            }
        }

        protected void btnConsultarAisv_Click(object sender, EventArgs e)
        {
            msjErrorAisv.Visible = false;
            objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
            //objStowageCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusionesST(objStowageCab.idStowageCab, out OError);

            ConsultarDataAISV(txtAISV.Text);
            txtAISV.Text = string.Empty;
            UPAISV.Update();
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

        protected void btnAutorizarIngreso_Click(string aisv, int cantidad, string dae, string booking, string placa, string chofer, string idChofer)
        {
            try
            {
                //if (Response.IsClientConnected)
                //{
                    sinresultado.Visible = false;
                    Ocultar_Mensaje();
                    UPAISV.Update();
                    this.msjErrorAisv.Visible = false;
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

                    if (string.IsNullOrEmpty(aisv))
                    {
                        this.Alerta("debe seleccionar un AISV.");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbHold.SelectedValue) || cmbHold.SelectedValue == "0")
                    {
                        this.Alerta("Debe seleccionar el hold.");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtPiso.Text))
                    {
                        this.Alerta("Debe seleccionar el piso.");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbCargo.SelectedValue) || cmbCargo.SelectedValue == "0")
                    {
                        this.Alerta("Debe seleccionar el cargo.");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbExportador.SelectedValue) || cmbExportador.SelectedValue == "0")
                    {
                        this.Alerta("Debe seleccionar el exportador.");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbMarca.SelectedValue) || cmbMarca.SelectedValue == "0")
                    {
                        this.Alerta("Debe seleccionar la marca.");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbConsignatario.SelectedValue) || cmbConsignatario.SelectedValue == "0")
                    {
                        this.Alerta("Debe seleccionar el consignatario.");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbBodega.SelectedValue) || cmbBodega.SelectedValue == "0")
                    {
                        this.Alerta("Debe seleccionar la bodega.");
                        return;
                    }
                   
                    if (string.IsNullOrEmpty(cmbBloque.SelectedValue) || cmbBloque.SelectedValue == "0")
                    {
                        this.Alerta("Debe seleccionar el bloque.");
                        return;
                    }

                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab_ADD_AISV" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
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
                    }

                    OError = string.Empty;
                    long? result = -1;
                    try
                    {
                        //grabar transaccion.
                        using (var scope = new System.Transactions.TransactionScope())
                        {
                            BAN_Stowage_Plan_Det oDet = new BAN_Stowage_Plan_Det();
                            oDet.oStowage_Plan_Cab = objStowageCab;
                            oDet.idStowageCab = objStowageCab.idStowageCab;
                            oDet.idHold = int.Parse(cmbHold.SelectedValue);
                            oDet.piso = txtPiso.Text;
                            oDet.boxSolicitado = cantidad;
                            oDet.boxAutorizado = cantidad;
                            oDet.idCargo = int.Parse(cmbCargo.SelectedValue);
                            oDet.idExportador = int.Parse(cmbExportador.SelectedValue);
                            oDet.idMarca = int.Parse(cmbMarca.SelectedValue);
                            oDet.idConsignatario = int.Parse(cmbConsignatario.SelectedValue);
                            oDet.idBodega = int.Parse(cmbBodega.SelectedValue);
                            oDet.idBloque = int.Parse(cmbBloque.SelectedValue);
                            oDet.reservado = 0;
                            oDet.disponible = cantidad;
                            oDet.comentario = "INGRESO AUTOMÁTICO DE AISV EXTERNO";
                            oDet.fechaDocumento = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                            oDet.estado = "PBL";
                            oDet.usuarioCrea = ClsUsuario.loginname;

                            result = oDet.Save_Update(out OError);
                            if (result != null) 
                            { 
                                if (result > 0)
                                {
                                    BAN_Stowage_Plan_Aisv oAisv = new BAN_Stowage_Plan_Aisv();
                                    oAisv.idStowageDet = long.Parse(result.ToString());
                                    oAisv.fecha = DateTime.Now;
                                    oAisv.idHoraInicio = 1;
                                    oAisv.horaInicio = "00:00";
                                    oAisv.idHoraFin = 1;
                                    oAisv.horaFin = "01:00";
                                    oAisv.box = cantidad;
                                    oAisv.comentario = "INGRESO AUTOMÁTICO DE AISV DE OTRA PROCEDENCIA";
                                    oAisv.aisv = aisv;
                                    oAisv.dae = dae.Trim();
                                    oAisv.booking = booking.Trim();
                                    oAisv.IIEAutorizada = true;
                                    oAisv.daeAutorizada = true;
                                    oAisv.placa = placa;
                                    oAisv.idChofer = idChofer;
                                    oAisv.chofer = chofer;
                                    //oAisv.idCapacidadHoraBodega = 
                                    oAisv.estado = "ING";
                                    oAisv.usuarioCrea = ClsUsuario.loginname;
                                    oAisv.idStowageAisv = oAisv.Save_Update(out OError);

                                    if (OError != string.Empty)
                                    {
                                        throw new Exception(OError);
                                    }
                                    scope.Complete();
                                }
                            }
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAutorizarIngreso_Click), "btnAutorizarIngreso_Click", false, null, null, ex.StackTrace, ex);
                        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    }
                    

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.msjErrorAisv.Visible = true;
                        this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                        return;
                    }
                    else
                    {
                        this.Alerta("Transacción exitosa");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-info'></i><b> Información! </b>El AISV seleccionado se ha migrado con éxito."));
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = null;
                    }
                //ConsultarDataAISVGenerados(long.Parse(result.ToString()));

                dgvAisv.DataSource = null;
                dgvAisv.DataBind();
                
                btnBuscar_Click(null, null);
                UPDETALLE.Update();
                //ConsultarDataAISV(aisv);
                //UPAISV.Update();

                //}
                //else
                //{
                //    Response.Redirect("../login.aspx", false);
                //    return;
                //}
                //lblAISV.InnerText = string.Empty;
                //UPAISV.Update();
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAutorizarIngreso_Click), "btnAutorizarIngreso_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
    }
}
