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
    public partial class VBS_BAN_Stowage : System.Web.UI.Page
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
            UPEDIT.Update();
            UPTURNOS.Update();
            UPCAPACIDADHORA.Update();
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
            objStowageCab = new BAN_Stowage_Plan_Cab();
            objStowageDet = new List<BAN_Stowage_Plan_Det>();
            objStowageCab.oDetalle = objStowageDet;
            Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;
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
                cmbDetHold.DataSource = oEntidad;
                cmbDetHold.DataValueField = "ID";
                cmbDetHold.DataTextField = "nombre";
                cmbDetHold.DataBind();
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
                cmbDetCargo.DataSource = oEntidad;
                cmbDetCargo.DataValueField = "ID";
                cmbDetCargo.DataTextField = "nombre";
                cmbDetCargo.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboCargo), "VBS_BAN_PreStowage.LlenaComboCargo", false, null, null, ex.StackTrace, ex);
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
                    Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;
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
                    Session["Transaccion_BAN_Stowage_Plan_Cab_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

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

        private void ConsultarDataCapacidadHoraTurnos(string fecha, int bloque, string idNave)
        {
            try
            {
                msjSinResultadosDetalle.Visible = false;
                var ResultadoDet = BAN_Capacidad_Hora_Bodega.ConsultarConfiguracionCapacidadPorNave(fecha, bloque, idNave, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    string Mensaje = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = Mensaje;
                    UPTURNOS.Update();
                    return;
                }

                if (ResultadoDet != null)
                {
                    Session["Transaccion_BAN_Capacidad_HoraBodegaST" + this.hf_BrowserWindowName.Value] = ResultadoDet;
                    var oBodega = BAN_Catalogo_Bodega.ConsultarLista(out OError);
                    var oBloque = BAN_Catalogo_Bloque.ConsultarLista(null, out OError);
                    foreach (var a in ResultadoDet)
                    {
                        //a.HoraInicio = BAN_HorarioInicial.GetHorarioInicio(a.idHoraInicio);
                        //a.HoraFin = BAN_HorarioFinal.GetHorarioFinal(a.idHoraFin);
                        try
                        {
                            a.oBloque = oBloque.Where(p => p.id == int.Parse(a.idBloque.ToString())).FirstOrDefault();
                            a.oBloque.oBodega = oBodega.Where(p => p.id == int.Parse(a.idBodega.ToString())).FirstOrDefault();
                        }
                        catch { }
                    }

                    var LinqQuery = from Tbl in ResultadoDet.Where(Tbl => !String.IsNullOrEmpty(Tbl.usuarioCrea))
                                    select new
                                    {
                                        id = Tbl.id,
                                        //time = string.Format("{0} - {1}", Tbl.horaInicio.Trim(), Tbl.horaFin.Trim()),
                                        idNave = Tbl.idNave,
                                        nave = Tbl.nave,
                                        idHoraInicio = Tbl.idHoraInicio,
                                        horaInicio = Tbl.horaInicio,
                                        idHoraFin = Tbl.idHoraFin,
                                        horaFin = Tbl.horaFin,
                                        idbodega = Tbl.idBodega,
                                        bodega = Tbl.oBloque?.oBodega?.nombre,
                                        idBloque = Tbl.idBloque,
                                        Bloque = Tbl.oBloque?.nombre,
                                        bodegaBloque = Tbl.oBloque?.oBodega?.nombre + " - " + Tbl.oBloque?.nombre,
                                        box = Tbl.box,
                                        boxSeleccionado = 0,
                                        reservado = Tbl.reservado,
                                        disponible = Tbl.disponible,
                                    };

                    try
                    {
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            dgvCapacidadHora.DataSource = LinqQuery;
                            dgvCapacidadHora.DataBind();
                        }
                        else
                        {
                            dgvCapacidadHora.DataSource = null;
                            dgvCapacidadHora.DataBind();
                            msjSinResultadosDetalle.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        dgvCapacidadHora.DataSource = null;
                        dgvCapacidadHora.DataBind();
                        this.Alerta(ex.Message);
                    }
                }
                else
                {
                    dgvCapacidadHora.DataSource = null;
                    dgvCapacidadHora.DataBind();
                    msjSinResultadosDetalle.Visible = true;
                }


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataCapacidadHoraTurnos), "ConsultarDataTurnos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
            UPTURNOS.Update();
        }
        private void ConsultarDataTurnosGenerados(long idStowageDet)
        {
            try
            {
                SinResultadoTurnos.Visible = false;
                var ResultadoDet = BAN_Stowage_Plan_Turno.ConsultarLista(idStowageDet, out OError);

                //##########################################
                // Asignar los datos agrupados al Repeater
                //##########################################
                var datosAgrupados = ResultadoDet.GroupBy(o => o.usuarioCrea)
                                                    .Select(group => new
                                                    {
                                                        Hold = group.Key,
                                                        TotalBoxes = group.Sum(o => o.box)
                                                    });


                // Calcular y mostrar el total de "BOXES"
                int? totalBoxes = datosAgrupados.Sum(group => group.TotalBoxes);
                Session["Transaccion_BAN_Stowage_Plan_AISV_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    string Mensaje = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = Mensaje;
                    UPTURNOS.Update();
                    return;
                }

                if (ResultadoDet != null)
                {
                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                    //objStowageCab.Exclusiones = BAN_Exclusion.ConsultarListadoExclusiones(objStowageCab.idStowageCab, out OError);

                    Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;

                    if (ResultadoDet.Count == 0)
                    {
                        dgvTurnos.DataSource = null;
                        dgvTurnos.DataBind();
                        SinResultadoTurnos.Visible = true;
                    }
                    else
                    {
                        dgvTurnos.DataSource = ResultadoDet;
                        dgvTurnos.DataBind();
                    }
                }
                else
                {
                    dgvTurnos.DataSource = null;
                    dgvTurnos.DataBind();
                    SinResultadoTurnos.Visible = true;
                }


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataTurnosGenerados), "ConsultarListadoAISVGenerados", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return;
            }
            UPTURNOS.Update();
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
                    this.msjErrorLiquidacion.Visible = true;
                    this.msjErrorLiquidacion.InnerHtml = Mensaje;
                    UPAISV.Update();
                    return;
                }

                if (ResultadoDet != null)
                {
                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
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
                this.msjErrorDetalle.Visible = IsPostBack;
                this.msjErrorLiquidacion.Visible = IsPostBack;

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
                    SinResultadoTurnos.Visible = false;

                    LlenaComboLinea();
                    LlenaComboHold();
                    LlenaComboServicios();
                    LlenaComboCargo();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";
                    cmbLiqServicio.Items.Add(item);
                    cmbLiqServicio.SelectedValue = "0";
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
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
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

                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(id.ToString()));
                        var oConsignatario = BAN_Catalogo_Consignatario.GetConsignatario(oDet.idConsignatario);
                        var oExportador = BAN_Catalogo_Exportador.GetExportaador(oDet.idExportador);
                        var oMarca = BAN_Catalogo_Marca.GetMarca(oDet.idMarca);

                        hdf_CodigoCab.Value = objStowageCab.idStowageCab.ToString();
                        hdf_CodigoDet.Value = id.ToString();
                        txtDetPiso.Text = oDet.piso;
                        cmbDetHold.SelectedValue = oDet.idHold.ToString();
                        txtDetCantidad.Text = oDet.boxSolicitado.ToString();
                        cmbDetCargo.SelectedValue = oDet.idCargo.ToString();
                        txtDetConsignatario.Text = oConsignatario?.nombre;
                        txtDetExportador.Text= oExportador?.nombre;
                        txtDetMarca.Text = oMarca?.nombre;
                        txtDetPiso.Text = oDet.piso.ToString();
                        txtDetobservacion.Text = oDet.comentario.ToString();
                        msjErrorDetalle.Visible = false;
                        this.btnActualizar.Attributes.Remove("disabled");
                        UPEDIT.Update();
                    }
                   
                    if (e.CommandName == "Aisv")
                    {
                        //Ocultar_Mensaje();
                        msjErrorAisv.Visible = false;
                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
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

                        txtExportadorSeleccionado.Text = oDet.oExportador?.ruc + " - " + oDet.oExportador?.nombre;
                        txtCantidadAutorizada.Text = oDet.boxAutorizado.ToString();
                        txtReservado.Text = oDet.reservado.ToString();
                        txtDisponible.Text = oDet.disponible.ToString();
                        UPBOTONES.Update();
                        ConsultarDataCapacidadHoraTurnos(DateTime.Now.ToString("yyyyMMdd"), int.Parse(oDet.idBloque.ToString()), objStowageCab.idNave);
                        UPCAPACIDADHORA.Update();
                        ConsultarDataTurnosGenerados(v_ID);
                        UPTURNOS.Update();
                        ConsultarDataAISVGenerados(v_ID);
                        UPAISV.Update();
                    }

                    if (e.CommandName == "Servicios")
                     {
                        Ocultar_Mensaje();
                        msjErrorLiquidacion.Visible = false;

                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        if (objStowageCab == null) { return; }

                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(v_ID.ToString()));
                        oDet.oExportador = BAN_Catalogo_Exportador.GetExportaador(oDet.idExportador);

                        LlenaComboAISV(v_ID);
                        ListItem item = new ListItem();
                        item.Text = "-- Seleccionar --";
                        item.Value = "0";
                        cmbLiqAisv.Items.Add(item);

                        hdf_idStowageDet.Value = v_ID.ToString();
                        txtLiqShipper.Text = string.Format("{0}", oDet?.oExportador?.nombre);
                        txtLiqCantidad.Text = "0";
                        cmbLiqServicio.SelectedValue = "0";
                        cmbLiqAisv.SelectedValue = "0";
                        txtLiqComentario.Text = string.Empty;
                        dgvLiquidacion.DataSource = null;
                        dgvLiquidacion.DataBind();
                        ConsultarLiquidacion(v_ID);
                        this.btnAddLiquidacion.Attributes.Remove("disabled");

                        UPLIQ.Update();
                    }

                    if (e.CommandName == "Publicar")
                    {
                        Ocultar_Mensaje();
                        msjErrorDetalle.Visible = false;

                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(v_ID.ToString()));
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDet;
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
                totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_Plan_Cab_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

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

                objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

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
                    //UPTURNOS.Update();
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
                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        var oDetTurno = BAN_Stowage_Plan_Turno.GetEntidad(long.Parse(v_ID.ToString()));

                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(oDetTurno?.idStowageDet.ToString()));
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDet;
                        Session["TransaccionBAN_Stowage_Plan_Turno" + this.hf_BrowserWindowName.Value] = oDetTurno;
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
                        btnAutorizarIngreso_Click(null, null);
                        UPAUTORIZAR.Update();
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
                Button btnAutorizarIngreso = (Button)e.Row.FindControl("btnAutorizarIng");

                objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;

                string v_id = DataBinder.Eval(e.Row.DataItem, "aisv_codigo").ToString().Trim();

                if (string.IsNullOrEmpty(v_id))
                {
                    btnAutorizarIngreso.Enabled = false;
                    return;
                }

                if (objStowageCab.Exclusiones?.Where(p => p.aisv == v_id).Count() > 0)
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

               
                /*if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Asumiendo que tienes una columna llamada "TotalBoxes"
                    int totalBoxesItem = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "vbs_box"));
                    totalBoxes += totalBoxesItem;
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    TableCell footerCell = new TableCell();
                    footerCell.ColumnSpan = e.Row.Cells.Count;
                    footerCell.HorizontalAlign = HorizontalAlign.Right;
                    footerCell.Font.Bold = true;
                    footerCell.Text = "Total: " + totalBoxes.ToString();
                    e.Row.Cells.Clear();
                    e.Row.Cells.Add(footerCell);
                }*/
                
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
            Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] = objStowageCab;
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
                     

                        if (oDetalle != null)
                        {
                            var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(oDetalle.idStowageDet.ToString()));
                            oDetalle = oDet;

                            if (int.Parse(txtDetCantidad.Text) < oDetalle.reservado )
                            {
                                this.Alerta("La cantidad no puede ser menor a lo yá reservado - Reservado: " + oDetalle.reservado.ToString());
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor verifique la cantidad. <br>La cantidad no puede ser menor a lo yá reservado - Reservado: " + oDetalle.reservado.ToString()));
                                this.txtDetCantidad.Focus();
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

                            oDetalle.boxSolicitado = int.Parse(txtDetCantidad.Text);
                            oDetalle.comentario = txtDetobservacion.Text;
                            oDetalle.usuarioModifica = ClsUsuario.loginname;
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
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el registro  {0} ", oDetalle.idStowageDet.ToString()));
                                this.btnActualizar.Attributes["disabled"] = "disabled";
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

            var oDetAisv = Session["TransaccionBAN_Stowage_Plan_Turno" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Turno;

            ////
            ///
            /// 
            BAN_Stowage_Plan_Turno oStowage_Plan_Turno = new BAN_Stowage_Plan_Turno();
            oStowage_Plan_Turno = BAN_Stowage_Plan_Turno.GetEntidad(long.Parse(oDetAisv.idStowagePlanTurno.ToString()));
            oStowage_Plan_Turno.usuarioModifica = ClsUsuario.loginname;

            if (oStowage_Plan_Turno.estado != "NUE") { return; }
            if (!(oStowage_Plan_Turno.isActive)) { return; }

            using (var scope = new TransactionScope())
            {
                var item = oStowage_Plan_Turno.Save_anulacion(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);

                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.TxtFechaDesde.Focus();
                    return;
                }

                var oCapacidadHora = BAN_Capacidad_Hora_Bodega.GetCapacidadHoraEspecifico(long.Parse(oStowage_Plan_Turno.idCapacidadHorafecha.ToString()), long.Parse(oStowage_Plan_Turno.idCapacidadHoraBodega.ToString()));
                BAN_Capacidad_Hora_Bodega oBAN_Capacidad_Hora = new BAN_Capacidad_Hora_Bodega();
                oBAN_Capacidad_Hora = oCapacidadHora;
                oBAN_Capacidad_Hora.reservado = oBAN_Capacidad_Hora.reservado - oStowage_Plan_Turno.box;
                //oBAN_Capacidad_Hora.disponible = oBAN_Capacidad_Hora.disponible + oStowage_Plan_Turno.box;

                BAN_Capacidad_Hora_BodegaFecha oBAN_Capacidad_HoraBodegaFecha = new BAN_Capacidad_Hora_BodegaFecha();
                oBAN_Capacidad_HoraBodegaFecha.idCapacidadHorafecha = long.Parse(oBAN_Capacidad_Hora.idCapacidadHorafecha.ToString());
                oBAN_Capacidad_HoraBodegaFecha.idCapacidadHoraBodega = long.Parse(oBAN_Capacidad_Hora.id.ToString());
                oBAN_Capacidad_HoraBodegaFecha.reservado = oBAN_Capacidad_Hora.reservado;
                oBAN_Capacidad_HoraBodegaFecha.id = oBAN_Capacidad_HoraBodegaFecha.Save_Update(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);

                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.TxtFechaDesde.Focus();
                    return;
                }

                var oDetalle = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(oStowage_Plan_Turno.idStowageDet.ToString()));
                BAN_Stowage_Plan_Det oBAN_StowagePlanDet = new BAN_Stowage_Plan_Det();
                oBAN_StowagePlanDet = oDetalle;
                oBAN_StowagePlanDet.reservado = oBAN_StowagePlanDet.reservado - oStowage_Plan_Turno.box;
                oBAN_StowagePlanDet.disponible = oBAN_StowagePlanDet.disponible + oStowage_Plan_Turno.box;
                oBAN_StowagePlanDet.usuarioModifica = ClsUsuario.loginname;

                oBAN_StowagePlanDet.idStowageDet = oBAN_StowagePlanDet.Save_Update(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);

                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                    this.TxtFechaDesde.Focus();
                    return;
                }
                else
                {
                    scope.Complete();
                }
            }

            DateTime fecha = new DateTime();
            CultureInfo enUS = new CultureInfo("en-US");
            if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                fecha = DateTime.Now;
            }
            string fechaDocumento = fecha.ToString("yyyyMMdd");
            objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
            var oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(oStowage_Plan_Turno.idStowageDet.ToString()));
            oDet.oStowage_Plan_Cab = objStowageCab;
            oDet.oExportador = BAN_Catalogo_Exportador.GetExportaador(oDet.idExportador);
            Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDet;

             
            txtExportadorSeleccionado.Text = oDet.oExportador?.ruc + " - " + oDet.oExportador?.nombre;
            txtCantidadAutorizada.Text = oDet.boxAutorizado.ToString();
            txtReservado.Text = oDet.reservado.ToString();
            txtDisponible.Text = oDet.disponible.ToString();
            UPBOTONES.Update();
            ConsultarDataCapacidadHoraTurnos(fecha.ToString("yyyyMMdd"), int.Parse(oDet.idBloque.ToString()), objStowageCab.idNave);
            UPCAPACIDADHORA.Update();
            ConsultarDataTurnosGenerados(long.Parse(oDet.idStowageDet.ToString()));
            UPTURNOS.Update();
            OcultarLoading("1");
            OcultarLoading("2");
            btnBuscar_Click(null,null);
            //this.Actualiza_Paneles();
            this.Alerta("Transacción exitosa");
            
        }

        protected void btnPublicar_Click(object sender, EventArgs e)
        {
            try
            {
                Ocultar_Mensaje();
                if (Response.IsClientConnected)
                {
                    objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                    BAN_Stowage_Plan_Det oDetalle = new BAN_Stowage_Plan_Det();
                    oDetalle = Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Det;

                    if (objStowageCab != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            OcultarLoading("2");
                            return;
                        }


                        if (oDetalle == null) { Response.Redirect("../login.aspx", false); return; }

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

                            oDetalle.estado = "PBL";
                            oDetalle.usuarioModifica = ClsUsuario.loginname;
                            oDetalle.idStowageDet = oDetalle.Save_Update(out OError);

                            Ocultar_Mensaje();

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
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el registro  {0} ", oDetalle.idStowageDet.ToString()));
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

        protected void btnAutorizarIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    sinresultado.Visible = false;
                    Ocultar_Mensaje();
                    UPAUTORIZAR.Update();
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

                    if (string.IsNullOrEmpty(this.lblAISV.InnerText))
                    {
                        this.Alerta("debe seleccionar un AISV.");
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
                            return;
                        }
                    }

                    OError = string.Empty;

                    BAN_Stowage_Plan_Det oDet = new BAN_Stowage_Plan_Det();
                    oDet = Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Det;
                    oDet.oStowage_Plan_Cab = objStowageCab;
                    var oStowagePlanTurno = Session["TransaccionBAN_Stowage_Plan_Turno" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Turno;

                    lblAISV.InnerText = Session["lblAISV" + this.hf_BrowserWindowName.Value] as string;

                    BAN_Exclusion oExclusiones = new BAN_Exclusion();
                    oExclusiones.codigo = string.Empty;
                    oExclusiones.idStowageCab = objStowageCab.idStowageCab;
                    oExclusiones.idStowageDet = oDet.idStowageDet;
                    oExclusiones.idStowageTurno = oStowagePlanTurno?.idStowagePlanTurno;
                    oExclusiones.idMotivo = 0;
                    oExclusiones.aisv = lblAISV.InnerText;
                    oExclusiones.comentario = string.Empty;// txtComentarioAutoriza1.Text;
                    oExclusiones.estado = true;
                    oExclusiones.usuarioCrea = ClsUsuario.loginname;

                    var result = oExclusiones.Save_AutorizacionST(out OError);

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
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = null;
                    }
                    ConsultarDataAISVGenerados(long.Parse(oDet.idStowageDet.ToString()));
                    UPAISV.Update();
                }
                lblAISV.InnerText = string.Empty;
                UPTURNOS.Update();
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAutorizarIngreso_Click), "btnAutorizarIngreso_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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

        protected void btnAddLiquidacion_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {
                    if (this.cmbLiqServicio.SelectedValue == "0")
                    {
                        this.Mostrar_MensajeLiquidacion(string.Format("<b>Informativo! </b>Por favor seleccione el servicio"));
                        this.cmbLiqServicio.Focus();
                        return;
                    }

                    if (cmbLiqAisv.SelectedValue == "0")
                    {
                        this.Mostrar_MensajeLiquidacion(string.Format("<b>Informativo! </b>Por favor ingrese el No. AISV"));
                        this.cmbLiqServicio.Focus();
                        return;
                    }

                    BAN_Stowage_Plan_Det oDetalle = new BAN_Stowage_Plan_Det();
                    //var obj = Session["TransaccionStowage_Plan_Det" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Det;
                    oDetalle = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(hdf_idStowageDet.Value.ToString()));

                    if (oDetalle != null)
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

                        BAN_Stowage_ServicioAdicional oLiquidacion = new BAN_Stowage_ServicioAdicional();
                        oLiquidacion.oStowageDet = oDetalle;
                        oLiquidacion.cantidad = int.Parse(txtLiqCantidad.Text);
                        oLiquidacion.aisv = cmbLiqAisv.SelectedItem.Text;
                        oLiquidacion.idServicio = int.Parse(cmbLiqServicio.SelectedValue);
                        oLiquidacion.comentario = txtLiqComentario.Text;
                        oLiquidacion.estado = true;
                        oLiquidacion.usuarioCrea = ClsUsuario.loginname;
                        msjErrorAisv.Visible = false;
                        //var gkey = liquidacion.consultaGKeyCarga(string.Format("{0}-{1}-{2}", oLiquidacion.TarjaDet.mrn, oLiquidacion.TarjaDet.msn, oLiquidacion.TarjaDet.hsn));
                        //var oServicio = servicios.GetServicio(int.Parse(cmbLiqServicio.SelectedValue));


                        oLiquidacion.idliquidacion = oLiquidacion.Save_Update(out OError);

                        if (OError != string.Empty)
                        {
                            this.Alerta(OError);
                            this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                            this.txtLiqComentario.Focus();
                            return;
                        }
                        else
                        {
                            ConsultarLiquidacion(long.Parse(oDetalle.idStowageDet.ToString()));
                            this.Alerta("Transacción exitosa");
                        }
                      
                    }
                }
            }
            catch (Exception ex)
            {
                this.btnActualizar.Attributes["disabled"] = "disabled";
                Session["TransaccionStowage_Plan_Det" + this.hf_BrowserWindowName.Value] = null;
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAddLiquidacion_Click), "btnAddLiquidacion_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }
        #endregion

        private void Mostrar_MensajeLiquidacion(string Mensaje)
        {
            this.msjErrorLiquidacion.Visible = true;
            this.msjErrorLiquidacion.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            UPLIQ.Update();
        }

        private void ConsultarLiquidacion(long _idStowageDet)
        {
            try
            {
                var Resultado = BAN_Stowage_ServicioAdicional.Consultar(_idStowageDet, out OError);

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
                        var oDet = BAN_Stowage_Plan_Det.GetEntidad(Resultado.FirstOrDefault().idStowageDet);
                        var oServicios = BAN_Catalogo_Servicio.Consultar(out OError);
                        var oExportador = BAN_Catalogo_Exportador.ConsultarListaExportador("CGSA", out OError);

                        foreach (var a in Resultado)
                        {
                            a.oServicio = oServicios.Where(p => p.id == a.idServicio).FirstOrDefault();
                            a.oExportador = oExportador.Where(p => p.id == int.Parse(a.idExportador.ToString())).FirstOrDefault();
                            a.oStowageDet = oDet;
                        }

                        var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.usuarioCrea))
                                        select new
                                        {
                                            idliquidacion = Tbl.idliquidacion,
                                            idStowageDet = Tbl.idStowageDet.ToString(),
                                            aisv = string.Format("{0}", Tbl.aisv),
                                            cantidad = Tbl.cantidad,
                                            //peso = Tbl.peso,
                                            //cubicaje = Tbl.cubicaje,
                                            exportador = Tbl.oStowageDet?.oExportador?.nombre.Trim(),
                                            //ubicacion = Tbl.ubicacion.Trim(),
                                            Servicio = Tbl.oServicio?.nombre.Trim(),
                                            comentario = Tbl.comentario.Trim(),
                                            estados = Tbl.estado,
                                            //sobredimensionado = Tbl.sobredimensionado,
                                            usuarioCrea = Tbl.usuarioCrea.Trim(),
                                            fechaCreacion = Tbl.fechaCreacion.Value.ToString("dd/MM/yyyy HH:mm"),
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            dgvLiquidacion.DataSource = LinqQuery;
                            dgvLiquidacion.DataBind();
                        }
                    }
                    else
                    {
                        dgvLiquidacion.DataSource = null;
                        dgvLiquidacion.DataBind();
                    }
                }
                else
                {
                    dgvLiquidacion.DataSource = null;
                    dgvLiquidacion.DataBind();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarLiquidacion), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        public void LlenaComboServicios()
        {
            try
            {
                cmbLiqServicio.DataSource = BAN_Catalogo_Servicio.Consultar(out OError); //ds.Tables[0].DefaultView;
                cmbLiqServicio.DataValueField = "ID";
                cmbLiqServicio.DataTextField = "nombre";
                cmbLiqServicio.DataBind();
                cmbLiqServicio.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboServicios), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboAISV(long idStowageDet)
        {
            try
            {
                cmbLiqAisv.DataSource = BAN_Stowage_Plan_Aisv.ConsultarLista(idStowageDet, out OError); //ds.Tables[0].DefaultView;
                cmbLiqAisv.DataValueField = "idStowageAisv";
                cmbLiqAisv.DataTextField = "aisv";
                cmbLiqAisv.DataBind();
                cmbLiqAisv.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboAISV), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        
        protected void txtBoxesDet_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtBox = (TextBox)row.FindControl("txtBoxesDet");
            int wContenedor = int.Parse(dgvCapacidadHora.DataKeys[row.RowIndex].Value.ToString());

            var objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodegaST" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;

            var currentStatRow = (from objCab in objCapacidadHoraDet.AsEnumerable()
                                  where objCab.id == wContenedor
                                  select objCab).FirstOrDefault();

            string naveSelect = txtDescripcionNave.Text.ToString();
            string IdEmpresa = string.Empty;

            if (string.IsNullOrEmpty(txtBox.Text.ToString()))
            {
                txtBox.Text = "0";
            }

            currentStatRow.boxSeleccionado = int.Parse(txtBox.Text.ToString());
            currentStatRow.nave = naveSelect;
            //this.btnGrabar.Attributes.Remove("disabled");
            Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;



            //############################
            TextBox currentTextBox = (TextBox)sender;

            // Obtén el índice de fila actual
            GridViewRow currentRow = (GridViewRow)currentTextBox.Parent.Parent;
            int rowIndex = currentRow.RowIndex;

            // Obtén el índice de columna actual
            int colIndex = currentTextBox.TabIndex;

            // Mueve el foco al siguiente control en la misma columna pero en la siguiente fila
            int nextRowIndex = rowIndex + 1;
            if (nextRowIndex < dgvCapacidadHora.Rows.Count)
            {
                Control nextControl = dgvCapacidadHora.Rows[nextRowIndex].Cells[colIndex].FindControl("txtBoxesDet");
                if (nextControl != null && nextControl is TextBox)
                {
                    ((TextBox)nextControl).Focus();
                }
            }


            this.Actualiza_Paneles();
        }

        protected void dgvCapacidaHora_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    this.msjErrorAisv.Visible = false;
                    //UPTURNOS.Update();
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
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }



                    if (e.CommandName == "Seleccionar")
                    {
                        //Ocultar_Mensaje();
                        var coman = e.CommandArgument.ToString();

                        long v_ID = long.Parse(coman);
                        long v_idCapacidadFecha = 0;
                        string v_cantidad = string.Empty;
                        DateTime? v_fecha = DateTime.Now;
                        int v_boxSeleccionado = 0;

                        //SE OBTIENE LA SESION ALMACENA CON LOS REGISTROS ACTUALIZADOS CON LOS BOX POR PANTALLA 
                        var objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodegaST" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;

                        //SE FILTRA EL REGISTRO ELEGIDO POR EL ID DEL ITEM
                        if (v_ID <= 0) { return; }
                        BAN_Capacidad_Hora_Bodega oCapacidaHoraDet = objCapacidadHoraDet.Where(p => p.id == v_ID).FirstOrDefault();
                        //v_cantidad = objCapacidadHoraDet.Where(p => p.id == v_ID).FirstOrDefault().boxSeleccionado.ToString();
                        //v_fecha = objCapacidadHoraDet.Where(p => p.id == v_ID).FirstOrDefault().fecha;

                        v_cantidad = oCapacidaHoraDet.boxSeleccionado.ToString();
                        v_fecha = oCapacidaHoraDet.fecha;
                        v_idCapacidadFecha = long.Parse(oCapacidaHoraDet.idCapacidadHorafecha.ToString());
                        //SE VALIDA QUE LA CANTIDAD HAYA SIDO INGRESADA Y QUE NO SE SOBREPASE DE LO DISPONIBLE
                        if (string.IsNullOrEmpty(v_cantidad)) { return; }
                        if (int.Parse(v_cantidad) <= 0) { return; }


                        v_boxSeleccionado = int.Parse(v_cantidad);
                        var oCapacidadHora = BAN_Capacidad_Hora_Bodega.GetCapacidadHoraEspecifico(v_idCapacidadFecha, v_ID);

                        if (v_boxSeleccionado > oCapacidadHora.disponible)
                        {
                            this.Alerta(string.Format("La cantidad ingresada => [{0}] es superior a lo disponible de la capacidad por hora [{1}]", v_boxSeleccionado, oCapacidadHora.disponible.ToString()));
                            return;
                        }

                        //SE OBTIENE EL OBJETO DETALLE SELECCIONADO
                        var oDet = Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Det;
                        var oDetalle = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(oDet.idStowageDet.ToString()));

                        int vReservado = 0;
                        try
                        {
                            var oTurnos = BAN_Stowage_Plan_Turno.ConsultarLista(long.Parse(oDet.idStowageDet.ToString()), out OError);
                            vReservado = oTurnos.Sum(P => P.box);
                            oDetalle.reservado = vReservado;
                            oDetalle.disponible = oDet.boxAutorizado - oDet.reservado;
                        }
                        catch
                        {
                            vReservado = 0;
                        }

                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDetalle;

                        //SE VALIDA QUE EL TOTAL DE TURNOS Y CAJAS NO SOBREPASE EL TOTAL AUTORIZADO
                        if (v_boxSeleccionado > oDetalle.disponible)
                        {
                            this.Alerta(string.Format("La cantidad ingresada => [{0}] es superior a la cantidad disponible [{1}] del stowage, superando el total general autorizado [{2}] ", v_boxSeleccionado, oDetalle.disponible.ToString(), oDetalle.boxAutorizado));
                            return;
                        }

                        oCapacidadHora.boxSeleccionado = v_boxSeleccionado;

                        try
                        {
                            var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                            ClsUsuario = ClsUsuario_;

                            var oTurnoAisv = BAN_Stowage_Plan_Turno.ConsultarLista(long.Parse(oDetalle.idStowageDet.ToString()), out OError);

                            //VALIDA QUE NO SE REPITA EL MISMO REGISTRO
                            if (oTurnoAisv.Where(p => p.fecha == DateTime.Parse(v_fecha.ToString()) && p.idHoraInicio == int.Parse(oCapacidadHora.idHoraInicio.ToString())  && p.idCapacidadHoraBodega == oCapacidadHora.id && p.idCapacidadHorafecha == oCapacidadHora.idCapacidadHorafecha).Count() > 0)
                            {
                                this.msjErrorAisv.Visible = true;
                                this.Alerta("Horario seleccionado ya se encuentra registrado");
                                this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Horario seleccionado ya se encuentra registrado");
                                this.TxtFechaDesde.Focus();
                                return;
                            }
                        }
                        catch
                        {
                            Response.Redirect("../login.aspx", false);
                            return;
                        }

                        using (var scope = new TransactionScope())
                        {
                            //######################################
                            //SE REALIZA LA INSERCION DEL TURNO
                            //######################################
                            BAN_Stowage_Plan_Turno oEntidad = new BAN_Stowage_Plan_Turno();
                            oEntidad.idStowageDet = long.Parse(oDetalle.idStowageDet.ToString());
                            oEntidad.fecha = DateTime.Parse(v_fecha.ToString());
                            oEntidad.idHoraInicio = int.Parse(oCapacidadHora.idHoraInicio.ToString());
                            oEntidad.horaInicio = oCapacidadHora.horaInicio;
                            oEntidad.idHoraFin = int.Parse(oCapacidadHora.idHoraFin.ToString());
                            oEntidad.horaFin = oCapacidadHora.horaFin;
                            oEntidad.box = int.Parse(v_cantidad);
                            oEntidad.idCapacidadHorafecha = oCapacidadHora.idCapacidadHorafecha;
                            oEntidad.idCapacidadHoraBodega = oCapacidadHora.id;
                            oEntidad.estado = "NUE";
                            oEntidad.usuarioCrea = ClsUsuario.loginname;
                            oEntidad.idStowagePlanTurno = oEntidad.Save_Update(out OError);

                            if (OError != string.Empty)
                            {
                                this.Alerta(OError);

                                this.msjErrorAisv.Visible = true;
                                this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                                this.TxtFechaDesde.Focus();
                                return;
                            }

                            //#################################################
                            //SE ACTUALIZA LA RESERVA DE LA CAPACIDAD HORA
                            //#################################################
                            BAN_Capacidad_Hora_Bodega oBAN_Capacidad_Hora = new BAN_Capacidad_Hora_Bodega();
                            oBAN_Capacidad_Hora = oCapacidadHora;
                            oBAN_Capacidad_Hora.reservado = oBAN_Capacidad_Hora.reservado + oBAN_Capacidad_Hora.boxSeleccionado;
                           
                            BAN_Capacidad_Hora_BodegaFecha oCapacidadHoraBodegFecha = new BAN_Capacidad_Hora_BodegaFecha();
                            oCapacidadHoraBodegFecha.reservado = oBAN_Capacidad_Hora.reservado;
                            oCapacidadHoraBodegFecha.idCapacidadHorafecha = long.Parse(oBAN_Capacidad_Hora.idCapacidadHorafecha.ToString());
                            oCapacidadHoraBodegFecha.idCapacidadHoraBodega = long.Parse(oBAN_Capacidad_Hora.id.ToString());
                            oCapacidadHoraBodegFecha.id = oCapacidadHoraBodegFecha.Save_Update(out OError);

                            if (OError != string.Empty)
                            {
                                this.Alerta(OError);

                                this.msjErrorAisv.Visible = true;
                                this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                                this.TxtFechaDesde.Focus();
                                return;
                            }

                            //#################################################
                            //SE ACTUALIZA LA RESERVA DEL DETALLE MACRO
                            //#################################################
                            BAN_Stowage_Plan_Det oBAN_StowagePlanDet = new BAN_Stowage_Plan_Det();
                            oBAN_StowagePlanDet = oDetalle;
                            oBAN_StowagePlanDet.reservado = oBAN_StowagePlanDet.reservado + v_boxSeleccionado;
                            oBAN_StowagePlanDet.disponible = oBAN_StowagePlanDet.disponible - v_boxSeleccionado;
                            oBAN_StowagePlanDet.usuarioModifica = ClsUsuario.loginname;

                            oBAN_StowagePlanDet.idStowageDet = oBAN_StowagePlanDet.Save_Update(out OError);

                            if (OError != string.Empty)
                            {
                                this.Alerta(OError);

                                this.msjErrorAisv.Visible = true;
                                this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError);
                                this.TxtFechaDesde.Focus();
                                return;
                            }
                            else
                            {
                                scope.Complete();
                            }
                        }

                        objStowageCab = Session["Transaccion_BAN_Stowage_Plan_Cab" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Cab;
                        oDet = BAN_Stowage_Plan_Det.GetEntidad(long.Parse(oDetalle.idStowageDet.ToString()));
                        oDet.oStowage_Plan_Cab = objStowageCab;
                        oDet.oExportador = BAN_Catalogo_Exportador.GetExportaador(oDet.idExportador);
                        Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] = oDet;
                        
                        txtExportadorSeleccionado.Text = oDet.oExportador?.ruc + " - " + oDet.oExportador?.nombre;
                        txtCantidadAutorizada.Text = oDet.boxAutorizado.ToString();
                        txtReservado.Text = oDet.reservado.ToString();
                        txtDisponible.Text = oDet.disponible.ToString();
                        UPBOTONES.Update();
                        ConsultarDataCapacidadHoraTurnos(v_fecha?.ToString("yyyyMMdd"), int.Parse(oDet.idBloque.ToString()), objStowageCab.idNave);
                        UPCAPACIDADHORA.Update();
                        ConsultarDataTurnosGenerados(long.Parse(oDet.idStowageDet.ToString()));
                        UPTURNOS.Update();
                        btnBuscar_Click(null, null);
                        //this.Actualiza_Paneles();
                        this.Alerta("Transacción exitosa");

                        //this.Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(dgvCapacidaHora_RowCommand), "dgvCapacidaHora_RowCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    this.msjErrorAisv.Visible = true;
                    this.msjErrorAisv.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                    return;
                }
            }
        }

        protected void dgvCapacidaHora_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                //CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");
                TextBox txtBox = (TextBox)e.Row.FindControl("txtBoxesDet");
                //string v_estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString().Trim();
                string v_box = DataBinder.Eval(e.Row.DataItem, "box").ToString().Trim();

                long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "id").ToString().Trim());

                txtBox.Text = "0";


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


                this.UPCAPACIDADHORA.Update();
            }
        }

        protected void dgvTurnos_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        Ocultar_Mensaje();
                        msjErrorDetalle.Visible = false;

                        long v_ID = long.Parse(e.CommandArgument.ToString());
                        if (v_ID <= 0) { return; }
                        var oDet = Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Det;
                        var oDetAisv = BAN_Stowage_Plan_Turno.GetEntidad(v_ID);
                        Session["TransaccionBAN_Stowage_Plan_Turno" + this.hf_BrowserWindowName.Value] = oDetAisv;


                        btnQuitar_Click(null,null);
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(dgvTurnos_ItemCommand), "dgvTurnos_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }

        protected void dgvTurnos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                int totalBoxes = 0;
                totalBoxes = int.Parse(Session["Transaccion_BAN_Stowage_Plan_AISV_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                //#######################################################################
                // VALIDACION DE QUE SI HAY AISV GENERADOS NO PERMITA EDITAR NI ELIMINAR
                //#######################################################################
                Button btnEliminar = e.Item.FindControl("btnQuitar") as Button;

                bool v_aisv = bool.Parse(DataBinder.Eval(e.Item.DataItem, "isActive").ToString());
                //Label lblAISV = e.Item.FindControl("lblAisv") as Label;
                              

                if (!v_aisv)
                {
                    btnEliminar.Enabled = false;
                }
                else
                {
                    btnEliminar.Enabled = true;
                }

                this.Actualiza_Panele_Detalle();
            }
        }
    

        protected void btnInfoCapacidadHora_Click(object sender, EventArgs e)
        {
            DateTime fecha = new DateTime();
            CultureInfo enUS = new CultureInfo("en-US");
            if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                this.Alerta(string.Format("EL FORMATO DE FECHA PROCESO DEBE SER Mes/dia/Anio {0}", TxtFechaDesde.Text));
                TxtFechaDesde.Focus();
                return;
            }
            string fechaDocumento = fecha.ToString("yyyyMMdd");
            var oDet = Session["TransaccionBAN_Stowage_Plan_Det" + this.hf_BrowserWindowName.Value] as BAN_Stowage_Plan_Det;
            ConsultarDataCapacidadHoraTurnos(fechaDocumento, int.Parse(oDet.idBloque.ToString()),oDet.oStowage_Plan_Cab.idNave);
            UPCAPACIDADHORA.Update();
            ConsultarDataTurnosGenerados(long.Parse(oDet.idStowageDet.ToString()));
            UPTURNOS.Update();
        }

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
    }
}
