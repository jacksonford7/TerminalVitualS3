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
    public partial class brbkDescarga : System.Web.UI.Page
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
        private Int64? nSesion { get { return (Int64)Session["nSesion"]; } set { Session["nSesion"] = value; } }
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

        private void Mostrar_MensajeLiquidacion(string Mensaje)
        {
            this.msjErrorLiquidacion.Visible = true;
            this.msjErrorLiquidacion.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            UPLIQ.Update();
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

        public void LlenaComboUbicacion()
        {
            try
            {
                cmbUbicacion.DataSource = ubicacion.consultaUbicacion();
                cmbUbicacion.DataValueField = "ID";
                cmbUbicacion.DataTextField = "nombre";
                cmbUbicacion.DataBind();
                cmbUbicacion.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboUbicacion), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        public void LlenaComboUbicacionLiquidacion()
        {
            try
            {
                cmbLiqUbicacion.DataSource = ubicacion.consultaUbicacion();
                cmbLiqUbicacion.DataValueField = "ID";
                cmbLiqUbicacion.DataTextField = "nombre";
                cmbLiqUbicacion.DataBind();
                
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
                var oManiobra = maniobra.consultaManiobras(); //ds.Tables[0].DefaultView;
                cmbManiobra.DataSource = oManiobra;
                cmbManiobra.DataValueField = "ID";
                cmbManiobra.DataTextField = "nombre";
                cmbManiobra.DataBind();
                cmbManiobra.Enabled = true;

                cmbManiobra2.DataSource = oManiobra;
                cmbManiobra2.DataValueField = "ID";
                cmbManiobra2.DataTextField = "nombre";
                cmbManiobra2.DataBind();
                cmbManiobra2.Enabled = true;
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
        public void LlenaComboServicios()
        {
            try
            {
                cmbLiqServicio.DataSource = servicios.consultaServicios(); //ds.Tables[0].DefaultView;
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
        public void LlenaComboLinea(string nave)
        {
            try
            {
                cmbLineas.DataSource = lineaNaviera.consultaLineas(nave); //ds.Tables[0].DefaultView;
                cmbLineas.DataValueField = "ID";
                cmbLineas.DataTextField = "nombre";
                cmbLineas.DataBind();

                ListItem item = new ListItem();
                item.Text = "-- Seleccionar --";
                item.Value = "0";
                cmbLineas.Items.Add(item);
                cmbLineas.SelectedValue = "0";
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
                cmbFiltroEstados.Items.Add(new ListItem("CONFIRMADOS","CON"));
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboCondicion), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
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
                    this.Alerta(OError);
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
                    this.Alerta(OError);
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

                        var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.estado))
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
        private void ConsultarLiquidacion(long _idTarjaDet)
        {
            try
            {
                var Resultado = liquidacion.listadoLiquidacion(_idTarjaDet, out OError);

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
                        var oDet = tarjaDet.GetTarjaDet(Resultado.FirstOrDefault().idTarjaDet);

                        foreach (var a in Resultado)
                        {
                            a.Servicio = servicios.GetServicio(a.idServicio);
                            a.Estados = estados.GetEstado(a.estado);
                            a.TarjaDet = oDet;
                        }

                        var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.estado))
                                        select new
                                        {
                                            idliquidacion = Tbl.idliquidacion,
                                            idTarjaDet = Tbl.idTarjaDet.ToString(),
                                            carga = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                            cantidad = Tbl.cantidad,
                                            peso = Tbl.peso,
                                            cubicaje = Tbl.cubicaje,
                                            consignatario = Tbl.TarjaDet.Consignatario.Trim(),
                                            ubicacion = Tbl.ubicacion.Trim(),
                                            Servicio = Tbl.Servicio?.nombre.Trim(),
                                            comentario = Tbl.comentario.Trim(),
                                            estados = Tbl.Estados?.nombre,
                                            sobredimensionado = Tbl.sobredimensionado,
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataTarjaN4Middle), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

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
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    sinresultado.Visible = false;
                    objCabecera.Detalle = objDetalle;
                    LlenaComboEstado();
                    LlenaComboProductos();
                    LlenaComboManiobras();
                    LlenaComboItems();
                    LlenaComboCondicion();
                    LlenaComboUbicacion();
                    LlenaComboUbicacionLiquidacion();
                    LlenaComboServicios();
                    LlenarFiltroEstados();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbProducto.Items.Add(item);
                    cmbManiobra.Items.Add(item);
                    cmbManiobra2.Items.Add(item);
                    cmbItem.Items.Add(item);
                    cmbCondicion.Items.Add(item);
                    cmbUbicacion.Items.Add(item);
                    cmbLiqUbicacion.Items.Add(item);
                    cmbLiqServicio.Items.Add(item);
                    cmbEstado.SelectedValue = "NUE";
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "N4"
        public bool GeneraBL(tarjaDet oDet, string usuario,string linea , ref string me)
        {
            try
            {
                string nbr = oDet.mrn + "-" + oDet.msn + "-" + oDet.hsn;
                //XmlDocument xmlDoc = new XmlDocument();

                //XmlElement c1 = xmlDoc.CreateElement("argo", "snx", "otro");//<argo:snx >
                //XmlElement c2 = xmlDoc.CreateElement("bill-of-lading");
                //XmlElement c3 = xmlDoc.CreateElement("items");
                //XmlElement c4 = xmlDoc.CreateElement("item");
                //XmlElement c5 = xmlDoc.CreateElement("cargo-lots");
                //XmlElement c6 = xmlDoc.CreateElement("cargo-lot");
                //XmlElement c7 = xmlDoc.CreateElement("position");
                //XmlElement c8 = xmlDoc.CreateElement("goods-bl");
                //c1.SetAttribute("xmlns:argo", "http://www.navis.com/argo");
                //c3.SetAttribute("update-mode", "MERGE");
                //c2.SetAttribute("nbr", nbr);
                //c2.SetAttribute("category", "IMPORT");
                //c2.SetAttribute("line", linea);//cont.CNTR_CLNT_CUSTOMER_LINE);
                //c2.SetAttribute("carrier-visit", oDet.tarjaCab.idNave/*cont.CNTR_VEPR_REFERENCE*/);
                //c2.SetAttribute("released-quantity", "0.0");
                //c2.SetAttribute("entered-quantity", "0.0");
                //c4.SetAttribute("nbr", Convert.ToString(oDet.idTarjaDet/*item.CODIGO_TARJA_ITEM*/));
                //c4.SetAttribute("is-bulk", "N");
                //c4.SetAttribute("piece-is-bulk", "N");
                //c4.SetAttribute("quantity", Convert.ToString(oDet.cantidad/*item.CANTIDAD_INGRESO*/));

                //c4.SetAttribute("weight-total-kg", Convert.ToString(oDet.kilos));
                //c4.SetAttribute("notes", string.Format("Creado en TV, por usuario:{0} IMO:{1}", usuario, oDet.imo));

                //c4.SetAttribute("commodity-id", "SENAE");
                //string lot = Convert.ToString(oDet.tarjaCab.idTarja/*tarja.CODIGO_TARJA*/) + "-" + Convert.ToString(oDet.idTarjaDet/*item.CODIGO_TARJA_ITEM*/);
                //c6.SetAttribute("lot-id", lot);
                //c6.SetAttribute("is-default-lot", "N");
                //c6.SetAttribute("quantity", Convert.ToString(oDet.cantidad/*item.CANTIDAD_INGRESO*/));
                //c6.SetAttribute("quantity-manifiested", Convert.ToString(oDet.cantidad/*item.CANTIDAD_INGRESO*/));

                //c6.SetAttribute("lot-weight-total-kg", Convert.ToString(oDet.kilos));

                //c6.SetAttribute("brand", "A");//no va 
                //c6.SetAttribute("unit-id", lot);
                //c7.SetAttribute("loc-type", "VESSEL");
                //c7.SetAttribute("location", oDet.tarjaCab.idNave/*cont.CNTR_VEPR_REFERENCE*/);
                //string slot = Convert.ToString(oDet.tarjaCab.idTarja/*tarja.CODIGO_TARJA*/) + "-" + Convert.ToString(oDet.idTarjaDet/*item.CODIGO_TARJA_ITEM*/);
                //c7.SetAttribute("slot", slot);
                //c8.SetAttribute("unit-id", lot);
                //c6.AppendChild(c7);
                //c5.AppendChild(c6);
                //c4.AppendChild(c5);
                //c3.AppendChild(c4);
                //c2.AppendChild(c3);
                //c2.AppendChild(c8);
                //c1.AppendChild(c2);

                //xmlDoc.AppendChild(c1);
                //wsN4 g = new wsN4();

                var webService = new n4WebService();
                string lot = Convert.ToString(oDet.tarjaCab.idTarja) + "-" + Convert.ToString(oDet.idTarjaDet) +"-"+ oDet.bl;
                string kilos = oDet.kilos.ToString();

                var oRecepcion = recepcion.listadoRecepcion(long.Parse(oDet.idTarjaDet.ToString()), out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return false;
                }

                string v_xml = string.Format(@"
                                <groovy class-location=""code-extension"" class-name=""CGSABillOfLadingUnload"">
                                  <parameters>
                                <!--Numero de BL -->
                                               <parameter id=""BL"" value=""{0}"" />
                                                <!--Vessel visit N4-->
                                                  <parameter id=""VISIT"" value=""{1}"" />
                                                                <!--LINE Operator N4-->
                                                  <parameter id=""LINE"" value=""{2}"" />
                                                                <!--BODEGA N4: B6,B7,B8-->
                                      <parameter id=""POSITION"" value=""{3}"" />
                                                   <!--COMMODIT: SENAE / CFS-->
                                                  <parameter id=""COMMODITY"" value=""{4}"" />
                                                  <!--CANTIDAD-->
                                                  <parameter id=""QTY"" value=""{5}"" />
                                                   <!--PESO-->
                                                  <parameter id=""WEIGHT"" value=""{6}"" />
                                                   <!--VOLUMEN:OPCIONAL-->
                                      <parameter id=""VOLUME"" value=""{16}"" />
                                                  <!--USUARIO:OPCIONAL-->
                                                  <parameter id=""USER"" value=""{7}"" />
                                                   <!--PESABLE Y/N:OPCIONAL-->
                                                  <parameter id=""CANWEIGHT"" value=""N"" />
                                                     <!--CARGO 1ERA MANIOBRA CONVENTIONAL- NO CONVENTIONAL- VEHICLE:OPCIONAL-->
                                                  <parameter id=""CARGOFIRST"" value=""{8}"" />
                                                      <!--CARGO 2DA MANIOBRA CONVENTIONAL- NO CONVENTIONAL- VEHICLE:OPCIONAL-->
                                                  <parameter id=""CARGOSECOND"" value=""{9}"" />
                                                      <!--CARGO PIN REBATE:OPCIONAL-->
                                      <parameter id=""CARGOPIN"" value="""" />
                                                    <!--CARGO OPERATION FIOS-HH-SG-SY:OPCIONAL-->
                                                   <parameter id=""OPERATION"" value=""{10}"" />
                                                    <!--CARGO BL ITEM NBR STRING:OPCIONAL-->
                                      <parameter id=""ITEMNBR"" value=""{11}"" />
                                      <parameter id=""SHIPPER"" value=""{12}"" />
                                                <!--SHIPPER OPCIONAL-->
                                      <parameter id=""CONSIGNEE"" value=""{13}"" />
                                                <!--MARSANDNUMBER DEL ITEM OPCIONAL-->
                                      <parameter id=""MARKS"" value=""{14}"" />
                                                 <!--MOTES DEL BL-->
                                    <parameter id=""NOTES"" value=""{15}"" />


                                  </parameters>
                                </groovy>
                                ", nbr //{0}
                                , oDet.tarjaCab.idNave //{1}
                                , linea //{2}
                                , oRecepcion.FirstOrDefault().ubicacion//{3}
                                , "SENAE" //{4}
                                , Convert.ToString(oDet.cantidad) //{5}
                                , int.Parse(kilos)//{6}
                                , usuario //{7}
                                , oDet.maniobra?.codigo //{8}
                                , oDet.maniobra2?.codigo //{9}
                                , oDet.condicion?.codigo //{10}
                                , lot //{11}
                                , "" //{12}
                                , oDet.idConsignatario //{13}
                                , oDet.productoEcuapass.Length > 50 ? oDet.productoEcuapass.Substring(0, 50) : oDet.productoEcuapass  //{14}
                                , string.Format("Creado en TV, por usuario:{0}", usuario)//{15}
                                , oDet.cubicaje); //{16}
                //int c = 0;//g.CallBasicService(@"ICT/ECU/GYE/CGSA",/* xmlDoc.OuterXml*/v_xml, ref me);

                ObjectSesion user = new ObjectSesion();

                var gkey = webService.InvokeN4Service(user, v_xml, ref me);

                try
                {
                    Int64 vGkey = long.Parse(gkey);
                    oDet.n4 = true;
                    oDet.usuario_n4 = usuario;
                    oDet.gKey_unit_BL = gkey;
                    oDet.Update_BL_N4(v_xml, me, out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta("BL generado en N4 sin embargo no se logró actualizar el status del BL en la TV: " + OError + ", se genero el Unit GKey:" + gkey);
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    }
                    me = "I-OK";

                    return true;
                }
                catch
                {
                    me = "ERROR";
                    oDet.n4 = false;
                    oDet.usuario_n4 = usuario;
                    oDet.gKey_unit_BL = string.Empty;
                    oDet.Update_BL_N4(v_xml, gkey, out OError);

                    this.Alerta(string.Format("No se puede generar el BL.: {0} - Mensaje N4 {1}",me,gkey) );
                    this.Mostrar_Mensaje(string.Format("No se puede generar el BL.: {0} - Mensaje N4 {1}", me, gkey));
                }
                //if (!me.ToUpper().Contains("ERROR"))
                //{
                    
                    //oDet.n4 = true;
                    //oDet.usuario_n4= usuario;
                    //oDet.gKey_unit_BL = gkey;
                    //oDet.Update_BL_N4(out OError);

                    //if (OError != string.Empty)
                    //{
                    //    this.Alerta("BL generado en N4 sin embargo no se logró actualizar el status del BL en la TV: " + OError);
                    //    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    //}
                    //me = "I-OK";

                    //return true;
                //}

                //return (c == 0) ? true : false;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(GeneraBL), "GeneraBL", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                this.Alerta("No se puede generar el BL.: " + ex.Message );
                //this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
            return false;
        }
        public bool Envio_IMDT(tarjaDet tarjaDet, string user, ref  string me)
        {
            try
            {
                string wxml = GeneraXml_IMDT(tarjaDet,user);

                WebRef_IngresoIEE.n4ServiceSoapClient ser = new WebRef_IngresoIEE.n4ServiceSoapClient();

                string resul = ser.basicInvoke("", wxml);  //IMDT
                me = resul;

                XDocument _exit = new XDocument();
                if (resul.Length > 0)
                {
                    _exit = XDocument.Parse(resul.ToString());
                }

                var Det = (from t in _exit.Descendants("ticket")
                           select new { t.Value });

                string IMDT = (Det != null) ? Det.ToList().First().Value : null;

                if (IMDT != null)
                {//modifica imdt del item
                    
                    string msg = "";

                    tarjaDet.imdt = true;
                    tarjaDet.imdt_num = IMDT;
                    tarjaDet.usuario_imdt = user;
                    tarjaDet.Update_IMDT(out msg);
                    if (msg!= string.Empty)
                    {
                        me = "Se envió el IMDT y no se logró actualizar el estado del IMDT del item: " + msg;
                        return false;
                    }
                    else
                    {
                        me = IMDT;
                    }
                }
                else { return false; }
                return true;
            }
            catch (Exception ex)
            {
                me= "Error al enviar IMDT: "+ me + " - "+ ex.Message;
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Envio_IMDT), "Envio_IMDT", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                //this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                return false;
            }
        }
        public String GeneraXml_IMDT(tarjaDet oDet, string user)
        {
            string wresult = "";
            try
            {              
                if (oDet == null)
                {
                    return null;
                }
                //carga secuencia de imdt
                int v_secu = tarjaDet.consultaSecuenciaIMDT(oDet.mrn, oDet.msn, oDet.hsn);

                //des aqui solo usa cabecera y detalle
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement transaction = xmlDoc.CreateElement("Transaction");
                XmlElement gkey = xmlDoc.CreateElement("Gkey"); gkey.InnerText = string.Format("{0}{1}00{2}7", DateTime.Now.Year, DateTime.Now.Month, oDet.idTarjaDet);
                XmlElement entity = xmlDoc.CreateElement("Entity"); entity.InnerText = "CFS";
                XmlElement Category = xmlDoc.CreateElement("Category"); Category.InnerText = "IMPRT";
                XmlElement User = xmlDoc.CreateElement("User"); User.InnerText = user;
                XmlElement TransType = xmlDoc.CreateElement("TransactionType"); TransType.InnerText = "022";
                XmlElement TransPlay = xmlDoc.CreateElement("TransactionPayload");
                XmlElement Mrn = xmlDoc.CreateElement("mrn"); Mrn.InnerText = oDet.mrn;
                XmlElement Msn = xmlDoc.CreateElement("msn"); Msn.InnerText = oDet.msn;
                XmlElement Hsn = xmlDoc.CreateElement("hsn"); Hsn.InnerText = oDet.hsn;
                XmlElement Sequen = xmlDoc.CreateElement("secuence"); Sequen.InnerText = Convert.ToString(v_secu);
                XmlElement Cqtty = xmlDoc.CreateElement("CountQuantity"); Cqtty.InnerText = Convert.ToString(Math.Truncate(oDet.cantidad));
                XmlElement Cpeso = xmlDoc.CreateElement("CommoditySizeMeasure"); Cpeso.InnerText = Convert.ToString(oDet.kilos);

                TransPlay.AppendChild(Mrn);
                TransPlay.AppendChild(Msn);
                TransPlay.AppendChild(Hsn);
                TransPlay.AppendChild(Sequen);
                TransPlay.AppendChild(Cqtty);
                TransPlay.AppendChild(Cpeso);

                transaction.AppendChild(gkey);
                transaction.AppendChild(entity);
                transaction.AppendChild(Category);
                transaction.AppendChild(User);
                transaction.AppendChild(TransType);
                transaction.AppendChild(TransPlay);

                xmlDoc.AppendChild(transaction);
                wresult = xmlDoc.InnerXml;
            }

            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                throw new DataException(string.Format("Error en Consulta de Datos IMDT {0} ", sb.ToString()));


                //Display or log the error based on your application.
            }

            catch (Exception)
            {
                throw;
            }
            return wresult;
        }
        #endregion

        #region "Eventos"
        #region "Gridview Cabecera"
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                Button btnGeneraIMDT = (Button)e.Row.FindControl("btnIMDT");
                Button btnGeneraBL = (Button)e.Row.FindControl("btnN4");
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
                btnGeneraIMDT.Enabled = false;
                btnGeneraBL.Enabled = false;

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
                        else { btnGeneraIMDT.Enabled = true; }

                        if (v_n4)
                        {
                            ChkBL.Checked = true;
                        }
                        else { btnGeneraBL.Enabled = true; }
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
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
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
                txtDescripcion.Text = oDet.descripcion;
                txtContenido.Text = oDet.contenido;
                txtobservacion.Text = oDet.observacion;
                cmbUbicacion.SelectedValue = oDet.ubicacion == null ? "0" : oDet.ubicacion;
                cmbEstado.SelectedValue = oDet.estado;
                cmbCondicion.SelectedValue = oDet.idCondicion.ToString();
                cmbProducto.SelectedValue = oDet.idProducto.ToString();
                if (cmbProducto.SelectedValue == "0")
                {
                    cmbManiobra.SelectedValue = "0";
                    cmbManiobra2.SelectedValue = "0";
                    cmbItem.SelectedValue = "0";
                    cmbCondicion.SelectedValue = "0";
                }
                else
                {
                    oDet.producto = productos.GetProducto(oDet.idProducto);
                    cmbManiobra.SelectedValue = oDet.producto?.Maniobra?.id.ToString();
                    cmbManiobra2.SelectedValue = oDet.producto?.Maniobra2?.id.ToString();
                    cmbItem.SelectedValue = oDet.producto?.Items.id.ToString();
                }

                ConsultarRecepcion(v_ID);
                UPEDIT.Update(); 
                nave oNave;
                try { oNave = nave.GetNave(objCabecera.idNave); } catch { Response.Redirect("../login.aspx", false); return; }
                if (string.IsNullOrEmpty(oNave.ata?.ToString()))
                {
                    this.btnActualizar.Attributes["disabled"] = "disabled";
                    this.Alerta("La nave aún no ha arribado a la terminal y no consta con DRM.");
                    this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "La nave aún no ha arribado a la terminal y no consta con DRM."));
                    this.txtBL.Focus();
                    UPEDIT.Update();
                    return;
                }
                else
                {
                    //se valida que el Bl se encuentre en status permitidos
                    var oDeta = tarjaDet.GetTarjaDet(long.Parse(oDet.idTarjaDet.ToString()));
                    if (oDeta.estado == "CON")
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
                        else
                        {
                            this.Alerta("No se puede proceder a realizar la descarga definitiva porqué el BL tiene posiciones en bodega pendiente de confirmar.");
                            this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede proceder a realizar la descarga definitiva porqué el BL tiene posiciones en bodega pendiente de confirmar."));
                        }
                        this.txtBL.Focus();
                        UPEDIT.Update();
                        return;
                    }
                }
            }

            if ((e.CommandName == "IMDT") || (e.CommandName == "N4"))
            {
                Ocultar_Mensaje();
                msjErrorDetalle.Visible = false;

                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }

                var oDet = tarjaDet.GetTarjaDet(long.Parse(v_ID.ToString()));
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = oDet;
            }

            if (e.CommandName == "Liquidacion")
            {
                Ocultar_Mensaje();
                msjErrorLiquidacion.Visible = false;

                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }
                objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;
                if (objCabecera == null) { return; }

                var oDet = tarjaDet.GetTarjaDet(long.Parse(v_ID.ToString()));
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = oDet;

                txtLiqCargaNum.Text = string.Format("{0}-{1}-{2}", oDet.mrn, oDet.msn, oDet.hsn);
                txtLiqCantidad.Text = oDet.cantidad.ToString();
                txtLiqPeso.Text = oDet.kilos.ToString();
                txtLiqCubicaje.Text = oDet.cubicaje.ToString();
                txtLiqConsignatario.Text = string.Format("{0} - {1}", oDet.idConsignatario, oDet.Consignatario);
                cmbLiqUbicacion.SelectedValue = "0";
                cmbLiqServicio.SelectedValue = "0";
                txtLiqComentario.Text = string.Empty;
                chkSobredimensionado.Checked = false;

                dgvLiquidacion.DataSource = null;
                dgvLiquidacion.DataBind();

                if (oDet.n4)
                {
                    ConsultarLiquidacion(v_ID);
                    this.btnAddLiquidacion.Attributes.Remove("disabled");
                }
                else
                {
                    this.btnAddLiquidacion.Attributes["disabled"] = "disabled";
                    this.Alerta("Está pendiente la creación del BL en N4.");
                    this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Está pendiente la creación del BL en N4."));
                    this.txtLiqComentario.Focus();
                    UPLIQ.Update();
                    return;
                }

                UPLIQ.Update();
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

        protected void btnLimpiar_Click(object sender, EventArgs e)
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
        protected void btnBuscar_Click(object sender, EventArgs e)
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
                    LlenaComboLinea(txtNave.Text);
                    UPMENSAJE2.Update();
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
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {
                    objCabecera = Session["TransaccionTarjaCab" + this.hf_BrowserWindowName.Value] as tarjaCab;

                    nave oNave;
                    try { oNave = nave.GetNave(objCabecera.idNave); } catch { Response.Redirect("../login.aspx", false); return; }
                    
                    if (string.IsNullOrEmpty(oNave.ata?.ToString()))
                    {
                        this.btnActualizar.Attributes["disabled"] = "disabled";
                        this.Alerta("La nave aún no ha arribado a la terminal y no consta con DRM.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "La nave aún no ha arribado a la terminal y no consta con DRM."));
                        this.txtBL.Focus();
                        UPEDIT.Update();
                        return;
                    }
                    else
                    {
                        this.btnActualizar.Attributes.Remove("disabled");
                        UPEDIT.Update();
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
                                               

                        tarjaDet oDetalle = new tarjaDet();
                        var obj= Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] as tarjaDet;
                        oDetalle = tarjaDet.GetTarjaDet(long.Parse(obj.idTarjaDet.ToString()));
                        

                        if (oDetalle.estado.ToString() != "CON")
                        {
                            this.btnActualizar.Attributes["disabled"] = "disabled";
                            if (oDetalle.estado.ToString() == "DES")
                            {
                                this.Alerta("El BL ya está en descarga definitiva.");
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "El BL está en descarga definitiva."));
                            }
                            else
                            {
                                this.Alerta("No se puede proceder a realizar la descarga definitiva porqué el BL tiene posiciones en bodega pendiente de confirmar.");
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede proceder a realizar la descarga definitiva porqué el BL tiene posiciones en bodega pendiente de confirmar."));
                            }
                            this.txtBL.Focus();
                            UPEDIT.Update();
                            return;
                        }
                        else
                        {
                            this.btnActualizar.Attributes.Remove("disabled");
                            UPEDIT.Update();
                        }

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

                            oDetalle.usuarioModifica = ClsUsuario.loginname;
                            oDetalle.estado = "DES";
                            msjErrorDetalle.Visible = false;

                            oDetalle.idTarjaDet = oDetalle.Save_Update(out OError);

                            if (OError != string.Empty)
                            {
                                this.Alerta(OError);
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                this.TXTMRN.Focus();
                                UPEDIT.Update();
                                return;
                            }
                            else
                            {
                                btnBuscar_Click(null, null);
                                this.Alerta("Transacción exitosa");
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el BL  {0} ", oDetalle.bl.ToString()));
                                this.Mostrar_MensajeDet(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se actualizó exitosamente el BL  {0} ", oDetalle.bl.ToString()));
                                this.btnActualizar.Attributes["disabled"] = "disabled";
                                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                            }
                        }
                    }
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
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }
        protected void btnGenerarBL_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbLineas.SelectedValue == "0")
                {
                    this.Alerta("Seleccione la linea");
                    return;
                }

                string me = string.Empty;
                var obj= Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] as tarjaDet;
                tarjaDet oDet = new tarjaDet();
                oDet = tarjaDet.GetTarjaDet(long.Parse(obj.idTarjaDet.ToString()));
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = oDet;
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
                if (GeneraBL(oDet, ClsUsuario.loginname,cmbLineas.SelectedItem.ToString(), ref me))
                {
                    btnBuscar_Click(null, null);
                    this.Alerta("Transacción exitosa");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se generó con éxito el BL  {0} ", oDet.carga.ToString()));
                }
                else
                {
                    btnBuscar_Click(null, null);
                    this.Alerta("Proceso de generación de BL no se logró completar");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se presentó la siguiente novedad:  {0} ", me.ToString()));
                }
            }
            catch(Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGenerarBL_Click), "btnGenerarBL_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                btnBuscar_Click(null, null);
            }
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }
        protected void btnGenerarIMDT_Click(object sender, EventArgs e)
        {
            try
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
                var obj = Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] as tarjaDet;
                tarjaDet oDet = new tarjaDet();
                oDet = tarjaDet.GetTarjaDet(long.Parse(obj.idTarjaDet.ToString()));
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                string me = string.Empty;
                if (Envio_IMDT(oDet, ClsUsuario.loginname, ref me))
                {
                    btnBuscar_Click(null, null);
                    this.Alerta("Transacción exitosa");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se generó con éxito el IMDT  {0} ",me.ToString()));
                }
                else
                {
                    btnBuscar_Click(null, null);
                    this.Alerta("El envio del IMDT no se logró completar");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se presentó la siguiente novedad:  {0} ", me.ToString()));
                }
                
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGenerarIMDT_Click), "btnGenerarIMDT_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje( string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                btnBuscar_Click(null, null);
                return;
            }
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }
        protected void btnAddLiquidacion_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrorDetalle.Visible = false;
                if (Response.IsClientConnected)
                {
                    //if (this.cmbLiqUbicacion.SelectedValue =="0")
                    //{
                    //    this.Mostrar_MensajeLiquidacion(string.Format("<b>Informativo! </b>Por favor seleccione una ubicación"));
                    //    this.cmbLiqUbicacion.Focus();
                    //    return;
                    //}

                    if (this.cmbLiqServicio.SelectedValue == "0")
                    {
                        this.Mostrar_MensajeLiquidacion(string.Format("<b>Informativo! </b>Por favor seleccione el servicio"));
                        this.cmbLiqServicio.Focus();
                        return;
                    }

                    tarjaDet oDetalle = new tarjaDet();
                    var obj = Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] as tarjaDet;
                    oDetalle = tarjaDet.GetTarjaDet(long.Parse(obj.idTarjaDet.ToString()));

                    if (oDetalle != null)
                    {
                        if (oDetalle.n4)
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

                            liquidacion oLiquidacion = new liquidacion();
                            oLiquidacion.TarjaDet = oDetalle;
                            oLiquidacion.ubicacion = cmbLiqUbicacion.SelectedValue;
                            oLiquidacion.idServicio = int.Parse(cmbLiqServicio.SelectedValue);
                            oLiquidacion.comentario = txtLiqComentario.Text;
                            oLiquidacion.estado = "NUE";
                            oLiquidacion.sobredimensionado = chkSobredimensionado.Checked;
                            oLiquidacion.usuarioCrea = ClsUsuario.loginname;
                            msjErrorLiquidacion.Visible = false;
                            //var gkey = liquidacion.consultaGKeyCarga(string.Format("{0}-{1}-{2}", oLiquidacion.TarjaDet.mrn, oLiquidacion.TarjaDet.msn, oLiquidacion.TarjaDet.hsn));
                            var oServicio = servicios.GetServicio(int.Parse(cmbLiqServicio.SelectedValue));


                            oLiquidacion.idliquidacion = oLiquidacion.Save_Update(long.Parse(oDetalle.gKey_unit_BL),oServicio,out OError);

                            if (OError != string.Empty)
                            {
                                this.Alerta(OError);
                                this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                this.txtLiqComentario.Focus();
                                return;
                            }
                            else
                            {
                                ConsultarLiquidacion(long.Parse(oDetalle.idTarjaDet.ToString()));
                                this.Alerta("Transacción exitosa");
                            }
                        }
                        else
                        {
                            this.btnAddLiquidacion.Attributes["disabled"] = "disabled";
                            this.Alerta("Está pendiente la creación del BL en N4.");
                            this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "Está pendiente la creación del BL en N4."));
                            this.txtLiqComentario.Focus();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.btnActualizar.Attributes["disabled"] = "disabled";
                Session["TransaccionTarjaDet" + this.hf_BrowserWindowName.Value] = null;
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAddLiquidacion_Click), "btnAddLiquidacion_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }
        protected void cmbProducto_SelectedIdexChange(object sender, EventArgs e)
        {
            try
            {
                var oProducto = productos.GetProducto(int.Parse(cmbProducto.SelectedValue));
                cmbManiobra.SelectedValue = oProducto.idManiobra.ToString();
                cmbManiobra2.SelectedValue = oProducto.idManiobra2.ToString();
                cmbItem.SelectedValue = oProducto.item.ToString();
                msjErrorDetalle.Visible = false;
            }
            catch
            {
                cmbManiobra.SelectedValue = "0";
                cmbManiobra2.SelectedValue = "0";
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
                btnFiltar_Click(null,null);
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

        #endregion
    }
}