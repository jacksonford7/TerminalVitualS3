using System;
using BillionEntidades;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using BillionAutenticacion;
//using BillionEntidades;
using BillionReglasNegocio;
using System.Data;
using N4.Entidades;
using N4Ws.Entidad;
using CSLSite;

namespace CSLSite.contenedorexpo
{
    public partial class contenedorexportacionConsola : System.Web.UI.Page
    {

        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private List<Cls_Bill_CabeceraExpo> objCabecera = new List<Cls_Bill_CabeceraExpo>();
        private Cls_Bill_Container_Expo objDetalle = new Cls_Bill_Container_Expo();
        private List<Cls_Bill_Container_Expo_Det_Validacion> objValidacionesDet = new List<Cls_Bill_Container_Expo_Det_Validacion>();
        #endregion

        #region "Variables"
        private string cMensajes;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        private string TipoServicio = string.Empty;
        private string LoginName = string.Empty;

        /*variables control*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private string codCab = string.Empty;
        #endregion

        #region "Propiedades"
        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }
        }
        #endregion

        #region "Metodos"

        private void Actualiza_Paneles()
        {
            //UPDETALLE.Update();
            UPCARGA.Update();
            UPBOTONES.Update();
        }

        private void Actualiza_Panele_Detalle()
        {
            //UPDETALLE.Update();
        }

        private void Limpia_Datos_cliente()
        {
            this.TXTNAVE.Text = string.Empty;
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
            objCabecera = new List<Cls_Bill_CabeceraExpo>();
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
            Session["TransaccionContDet"] = objCabecera;
        }

        protected string jsarguments(object CNTR_BKNG_BOOKING, object CNTR_ID)
        {
            return string.Format("{0};{1}", CNTR_BKNG_BOOKING != null ? CNTR_BKNG_BOOKING.ToString().Trim() : "0", CNTR_ID != null ? CNTR_ID.ToString().Trim() : "0");
        }

        #endregion

        #region "Form"
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
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
                Server.HtmlEncode(this.TXTNAVE.Text.Trim());

                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    rbBooking.Checked = true;
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "Grilla de Facturas"

        protected void GridView3_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (GridView3.Rows.Count > 0)
                {
                    GridView3.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    GridView3.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                var v_objConsolidado = Session["GridView3" + this.hf_BrowserWindowName.Value];
                
                if (objCabecera == null)
                {
                    //this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    GridView3.PageIndex = e.NewPageIndex;
                    GridView3.DataSource = v_objConsolidado;
                    GridView3.DataBind();
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void GridView3_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Imprimir")
            {
                Session["NumeroFacturaExpo" + this.hf_BrowserWindowName.Value] = e.CommandArgument.ToString() ;

                string cId = Session["NumeroFacturaExpo" + this.hf_BrowserWindowName.Value] as string;
                var pag = new CSLSite.facturaimportacion();
                ScriptManager.RegisterStartupScript(pag, pag.GetType(), "popup", "popOpen('../reportes/factura_preview.aspx?id_comprobante=" + cId + "');", true);

                //UPFAC.Update();

            }
        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //bool estado = (bool)DataBinder.Eval(e.Row.DataItem, "ESTADO_ERROR");
                //long idCab = (long)DataBinder.Eval(e.Row.DataItem, "CNTR_CAB_ID");
                //long consecutivo = (long)DataBinder.Eval(e.Row.DataItem, "CNTR_CONSECUTIVO");

                //if (idCab == 0) { return; }
                //objValidacionesDet = Session["DetalleValidacion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_Container_Expo_Det_Validacion>;
                //if (objValidacionesDet.Where(p => p.CNTR_CAB_ID == idCab && p.CNTR_CONSECUTIVO == consecutivo).Count() > 0)
                //{
                //    if (estado.Equals(false))
                //    {
                //        e.Row.ForeColor = System.Drawing.Color.Green;
                //    }
                //    else
                //    {
                //        e.Row.ForeColor = System.Drawing.Color.Red;
                //    }

                //}

                UPFAC.Update();
            }
        }
        #endregion

        #region "Grilla de SubDetalle Validacion"
        protected void GridView2_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (GridView2.Rows.Count > 0)
                {
                    GridView2.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;

                if (objCabecera == null)
                {
                    //this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    GridView2.PageIndex = e.NewPageIndex;

                    var v_objConsolidado = Session["GridView2" + this.hf_BrowserWindowName.Value];
                    GridView2.DataSource = v_objConsolidado;
                    GridView2.DataBind();
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void GridView2_RowCommand(object source, GridViewCommandEventArgs e)
        {
    
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //bool estado = (bool)DataBinder.Eval(e.Row.DataItem, "ESTADO_ERROR");
                //long idCab = (long)DataBinder.Eval(e.Row.DataItem, "CNTR_CAB_ID");
                //long consecutivo = (long)DataBinder.Eval(e.Row.DataItem, "CNTR_CONSECUTIVO");

                //if (idCab == 0) { return; }
                //objValidacionesDet = Session["DetalleValidacion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_Container_Expo_Det_Validacion>;
                //if (objValidacionesDet.Where(p => p.CNTR_CAB_ID == idCab && p.CNTR_CONSECUTIVO == consecutivo).Count() > 0)
                //{
                //    if (estado.Equals(false))
                //    {
                //        e.Row.ForeColor = System.Drawing.Color.Green;
                //    }
                //    else
                //    {
                //        e.Row.ForeColor = System.Drawing.Color.Red;
                //    }

                //}

                //UPDET.Update();
            }
        }
        #endregion

        #region "Grilla de Detalle Booking"
        protected void GrillaDetalle_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (GrillaDetalle.Rows.Count > 0)
                {
                    GrillaDetalle.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    GrillaDetalle.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void GrillaDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;

                if (objCabecera == null)
                {
                    //this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    GrillaDetalle.PageIndex = e.NewPageIndex;
                    var objdetalle = objCabecera.Where(p => p.CNTR_BKNG_BOOKING == txtID.Text).FirstOrDefault().Detalle.OrderByDescending(p => p.ESTADO_ERROR).ToList();
                    GrillaDetalle.DataSource = objdetalle;
                    GrillaDetalle.DataBind();
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void GrillaDetalle_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                string v_consecutivo = e.CommandArgument.ToString();
                string[] v_parametros = v_consecutivo.Split(',');
                //objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;

                //if (objCabecera == null) { return; }

                this.txtContainers.Text = v_parametros.GetValue(2).ToString();
                string _error = string.Empty;

                ////RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
                ////var ListaSubdetalle = _obj.consultaSubDetalle(long.Parse(v_parametros.GetValue(0).ToString()), long.Parse(v_parametros.GetValue(1).ToString()), out _error);
                //objValidacionesDet = Session["DetalleValidacion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_Container_Expo_Det_Validacion> ;

                //var ListaSubdetalle = objValidacionesDet.Where(p=> p.CNTR_CAB_ID == long.Parse(v_parametros.GetValue(0).ToString()) && p.CNTR_CONSECUTIVO == long.Parse(v_parametros.GetValue(1).ToString())).ToList();
                //GridView1.DataSource = ListaSubdetalle;
                //GridView1.DataBind();
                //UPMODAL.Update();
                //UPMODAL.Update();

                //string CNTR_BKNG_BOOKING = e.CommandArgument.ToString();
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null) { return; }

                var v_objdetalle = objCabecera.Where(p => p.CNTR_ID == long.Parse(v_parametros.GetValue(0).ToString())).FirstOrDefault().Detalle.Where(q => q.CNTR_CONSECUTIVO == long.Parse(v_parametros.GetValue(1).ToString())).ToList();
                
                GridView1.DataSource = v_objdetalle;
                GridView1.DataBind();


                //CARGA DETALLE DE HISTORIAL
                
                objValidacionesDet = Session["DetalleValidacion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_Container_Expo_Det_Validacion>;
                int v_value = 0;
                var v_objConsolidado = (from A in v_objdetalle
                                        join B in objValidacionesDet on new { A.CNTR_CAB_ID, A.CNTR_CONSECUTIVO } equals new { B.CNTR_CAB_ID, B.CNTR_CONSECUTIVO }
                                        select new
                                        {
                                            //A.CNTR_CAB_ID,
                                            //A.CNTR_CONSECUTIVO,
                                            //A.CNTR_CONTAINER,
                                            //B.SERVICIOS_ACTUALES,
                                            //B.TOTAL,
                                            //B.MSG_DUPLICADO,
                                            //B.MSG_FALTANTE,
                                            //B.MSG_OTROS,
                                            //B.FALTANTES,
                                            //B.FECHA_REGISTRO,
                                            //B.USUARIO_REGISTRA,
                                            //A.ESTADO_ERROR
                                            CNTR_CAB_ID = A.CNTR_CAB_ID,
                                            CNTR_CONSECUTIVO = A.CNTR_CONSECUTIVO,
                                            TIPO_EVENTO = (v_value++).ToString() == "0" ? "PROCESO INICIAL":"REPROCESO",
                                          FECHA_REGISTRO = B.FECHA_REGISTRO,
                                          USUARIO_REGISTRA = B.USUARIO_REGISTRA,
                                          NOVEDADES = "SERVICIOS ACTUALES: " + B.SERVICIOS_ACTUALES + Environment.NewLine + " *** "
                                          + "SERVICIOS DUPLICADOS: " + B.MSG_DUPLICADO + Environment.NewLine + " *** "
                                          + "SERVICIOS FALTANTES: " + B.FALTANTES + " - "+ B.MSG_FALTANTE+ Environment.NewLine+ " *** "
                                          + "OTROS SERVICIOS: " + B.MSG_OTROS + "  "
                                      }).ToList();//.OrderBy(q=> q.ESTADO_ERROR).ToList();


                Session["GridView2" + this.hf_BrowserWindowName.Value] = v_objConsolidado;

                GridView2.DataSource = v_objConsolidado;
                GridView2.DataBind();
                UPDET.Update();



                UPMODAL.Update();
                UPMODAL.Update();
                UPDET.Update();
                UPCAB.Update();
            }
        }
        protected void GrillaDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool estado = (bool)DataBinder.Eval(e.Row.DataItem, "ESTADO_ERROR");
                long idCab = (long)DataBinder.Eval(e.Row.DataItem, "CNTR_CAB_ID");
                long consecutivo = (long)DataBinder.Eval(e.Row.DataItem, "CNTR_CONSECUTIVO");

                if (idCab == 0) { return; }
                objValidacionesDet = Session["DetalleValidacion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_Container_Expo_Det_Validacion>;
                if (objValidacionesDet.Where(p=> p.CNTR_CAB_ID == idCab && p.CNTR_CONSECUTIVO == consecutivo).Count() > 0)
                {
                    if (estado.Equals(false))
                    {
                        e.Row.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }

                }

                UPDET.Update();
            }
        }
        #endregion

        #region "Gridview Cabecera"
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");
                Button btnRW = (Button)e.Row.FindControl("IncreaseButton3");
                string estado = DataBinder.Eval(e.Row.DataItem, "CNTR_ESTADO").ToString().Trim();
                btnRW.Enabled = false;

                if (Chk.Checked)
                {
                    if (estado.Equals("N"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.Peru;
                    }
                    if (estado.Equals("V") || (estado.Equals("F")))
                    {
                        e.Row.ForeColor = System.Drawing.Color.YellowGreen;
                    }

                    if (estado.Equals("E"))
                    {
                        btnRW.Enabled = true;
                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }
                }

                this.Actualiza_Panele_Detalle();
            }
        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                var xpars = e.CommandArgument.ToString().Split(';');

                string v_CNTR_BKNG_BOOKING = (string.IsNullOrEmpty(xpars[0]) ? string.Empty : xpars[0]);
                Int64 v_CNTR_CAB_ID = (string.IsNullOrEmpty(xpars[1]) ? 0 : Int64.Parse(xpars[1]));

                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null) { return; }
                this.txtID.Text = v_CNTR_BKNG_BOOKING;

                var v_objdetalle = objCabecera.Where(p => p.CNTR_BKNG_BOOKING == v_CNTR_BKNG_BOOKING && p.CNTR_ID == v_CNTR_CAB_ID).FirstOrDefault().Detalle.OrderByDescending(p => p.ESTADO_ERROR).ToList();

                RN_Bill_InvoiceContainerExpo _objValidacion = new RN_Bill_InvoiceContainerExpo();
                string _error = string.Empty;
                var v_objRankValidaciones = _objValidacion.consultaSubDetalle(v_objdetalle.FirstOrDefault().CNTR_CAB_ID,null, v_objdetalle.FirstOrDefault().CNTR_VEPR_REFERENCE.ToString(), out _error);

               /* var v_objConsolidado = (from A in v_objdetalle
                                      join B in v_objRankValidaciones on new { A.CNTR_CAB_ID, A.CNTR_CONSECUTIVO} equals new { B.CNTR_CAB_ID, B.CNTR_CONSECUTIVO }
                                      select new
                                      {
                                          A.CNTR_CAB_ID,
                                          A.CNTR_CONSECUTIVO,
                                          A.CNTR_CONTAINER,
                                          B.SERVICIOS_ACTUALES,
                                          B.TOTAL,
                                          B.MSG_DUPLICADO,
                                          B.MSG_FALTANTE,
                                          B.MSG_OTROS,
                                          B.FALTANTES,
                                          B.FECHA_REGISTRO,
                                          B.USUARIO_REGISTRA,
                                          A.ESTADO_ERROR
                                      }).ToList();*/

                var v_objConsolidado = (from B in v_objRankValidaciones.Where(B => B.CNTR_CAB_ID != 0)
                                 join A in v_objdetalle on new { B.CNTR_CAB_ID, B.CNTR_CONSECUTIVO } equals new { A.CNTR_CAB_ID, A.CNTR_CONSECUTIVO }   into TmpFinal
                                 from Final in TmpFinal.DefaultIfEmpty()
                                        select new
                                        {
                                            B.CNTR_CAB_ID,
                                            CNTR_CONSECUTIVO = (Final==null ? 0 : Final.CNTR_CONSECUTIVO),
                                            CNTR_CONTAINER  = (Final==null? string.Empty : Final.CNTR_CONTAINER),
                                            B.SERVICIOS_ACTUALES,
                                            B.TOTAL,
                                            B.MSG_DUPLICADO,
                                            B.MSG_FALTANTE,
                                            B.MSG_OTROS,
                                            B.FALTANTES,
                                            B.FECHA_REGISTRO,
                                            B.USUARIO_REGISTRA,
                                            ESTADO_ERROR = (Final==null ?  B.ERROR : Final.ESTADO_ERROR)
                                        }).ToList();

                GrillaDetalle.DataSource = v_objConsolidado;
                GrillaDetalle.DataBind();
                UPDET.Update();
                //UPDET.Update();
            }

            if (e.CommandName == "Actualizar")
            {

                codCab = e.CommandArgument.ToString();
                Session["CodigoCabecera" + this.hf_BrowserWindowName.Value] = codCab;
                UPCAB.Update();
                
            }

            if (e.CommandName == "Factura")
            {
                string v_CNTR_CNTR_ID = e.CommandArgument.ToString();
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null) { return; }
                TXTBOOK.Text = objCabecera.FirstOrDefault().CNTR_VEPR_REFERENCE.ToString();
                RN_Bill_InvoiceContainerExpo _objFacturas = new RN_Bill_InvoiceContainerExpo();
                string _error = string.Empty;
                var v_objFacturas= _objFacturas.consultaFacturas (objCabecera?.FirstOrDefault().CNTR_VEPR_REFERENCE.ToString(), objCabecera?.Where(x=> x.CNTR_ID == long.Parse(v_CNTR_CNTR_ID))?.FirstOrDefault().CNTR_BKNG_BOOKING.ToString(), out _error);

                Session["GridView3" + this.hf_BrowserWindowName.Value] = v_objFacturas;
                GridView3.DataSource = v_objFacturas;
                GridView3.DataBind();
                UPFAC.Update();

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
                objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_CabeceraExpo>;

                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    tablePagination.PageIndex = e.NewPageIndex;
                    tablePagination.DataSource = objCabecera;
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

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            objCabecera.Clear();
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
            Session["TransaccionContDet"] = objCabecera;
            this.Ocultar_Mensaje();
            UPCAB.Update();

            GrillaDetalle.DataSource = null;
            GrillaDetalle.DataBind();
            UPDET.Update();

            //        return;
            //this.BtnFacturar.Attributes.Add("disabled", "disabled");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader('" + 1 + "');", true);
            if (Response.IsClientConnected)
            {
                try
                {
                    string _ERROR = string.Empty;
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTNAVE.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.TXTNAVE.Focus();
                        return;
                    }

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    //Carga de data
                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //resultado de entidad contenedor
                    var Contenedor = new N4.Exportacion.container();
                    var ListaContenedores = Contenedor.CargaporReferencia(ClsUsuario.loginname, this.TXTNAVE.Text.ToString());

                 

                    //INFORMACION DE BOOKING YA PROCESADOS(ya almacenados en base de datos)
                    RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
                    var ListaBookingProcesado = _obj.consultaEntidad(this.TXTNAVE.Text.ToString(), out _ERROR);

                    //INFORMACION DE VALIDACIONES DE CONTENEDORES YA PROCESADOS(ya almacenados en base de datos)
                    RN_Bill_InvoiceContainerExpo _objValidacion = new RN_Bill_InvoiceContainerExpo();
                    objValidacionesDet = _objValidacion.consultaSubDetalle(null,null, TXTNAVE.Text, out _ERROR);
                    Session["DetalleValidacion" + this.hf_BrowserWindowName.Value] = objValidacionesDet;

                    if (ListaContenedores.Exitoso)
                    {
                        //agrupa el detalle por booking
                        var listaAgrupada = (from tbl in ListaContenedores.Resultado
                                             group tbl by tbl.CNTR_BKNG_BOOKING into tblbook
                                             select tblbook.FirstOrDefault()
                                            ).ToList();

                       
                        ////////////////////////////
                        //    ARMA LA CABECERA
                        ////////////////////////////
                        foreach (var lista in listaAgrupada)
                        {
                            Cls_Bill_CabeceraExpo objCab ;
                            var resultado = InvoiceTypeConfig.ObtenerInvoicetypes();
                            int v_contador = 0;
                            int v_indice = 0;

                            //SI HAY MAS DE UNA FACTURA CON EL MISMO BOOKING
                            if (ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).ToList().Count > 0)
                            {
                                foreach (var lista1 in ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING ))
                                {
                                    objCab = new Cls_Bill_CabeceraExpo();
                                    objCab.CNTR_VEPR_REFERENCE = lista.CNTR_VEPR_REFERENCE;//cab.NAVE_REF.ToString();
                                    objCab.CNTR_BKNG_BOOKING = lista.CNTR_BKNG_BOOKING;//cab.BOOKING;

                                    try { objCab.CNTR_CONTADO = !objCab.CNTR_CREDITO; } catch { }

                                    resultado = InvoiceTypeConfig.ObtenerInvoicetypes();
                                    if (resultado.Exitoso)
                                    {
                                        var invoiceType = resultado.Resultado.Where(p => p.codigo == (objCab.CNTR_CREDITO ? "EXPORCREFULL" : "EXPOCONFULL"));
                                        objCab.CNTR_INVOICE_TYPE = invoiceType.FirstOrDefault().valor.ToString();
                                        objCab.CNTR_INVOICE_TYPE_NAME = objCab.CNTR_CREDITO ? "EXPORCREFULL" : "EXPOCONFULL";
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    /*if (lista1.CNTR_BKNG_BOOKING == "149IEC0097300")
                                    {
                                        return;
                                    }*/
                                    objCab.LLAVE = string.Format("{0};{1}", lista.CNTR_BKNG_BOOKING, lista1.CNTR_ID);
                                    objCab.CNTR_CONTAINERS = "";
                                    objCab.CNTR_FECHA = DateTime.Now;
                                    objCab.CNTR_ESTADO = "N";
                                    objCab.CNTR_VEPR_VSSL_NAME = lista.CNTR_VEPR_VSSL_NAME;//cab.NAVE;
                                    objCab.CNTR_VEPR_VOYAGE = lista.CNTR_VEPR_VOYAGE;// cab.VIAJE;
                                    objCab.CNTR_VEPR_ACTUAL_ARRIVAL = lista.CNTR_VEPR_ACTUAL_ARRIVAL;//cab.LLEGADA;
                                    objCab.CNTR_VEPR_ACTUAL_DEPARTED = lista.CNTR_VEPR_ACTUAL_DEPARTED;//cab.SALIDA;
                                    objCab.CNTR_USUARIO_CREA = ClsUsuario.loginname;

                                    objCab.CNTR_CLNT_CUSTOMER_LINE = lista.CNTR_CLNT_CUSTOMER_LINE;
                                    objCab.CNTR_BKNG_BOOKING = lista.CNTR_BKNG_BOOKING;
                                    objCab.CNTR_CONTADO = false;
                                    objCab.CNTR_CREDITO = false;
                                    objCab.BOOKINGLINE = string.Format("{0} | {1} | {2}",lista.CNTR_CLNT_CUSTOMER_LINE,lista.CNTR_BKNG_BOOKING, lista1.CNTR_INVOICE_TYPE.Replace("2DA_MAN_EXPO_CNTRS_",""));
                                    objCab.CNTR_SIZE_RF = lista.CNTR_TYSZ_SIZE.ToString() + " | REEF:" + lista.CNTR_REEFER_CONT;

                                    objCab.CNTR_PROCESADO = true;
                                    objCab.CNTR_ID = lista1.CNTR_ID;
                                    objCab.CNTR_ESTADO = lista1.CNTR_ESTADO;

                                    v_contador = 0;
                                    v_indice = 0;
                                    //var listaDetalle = ListaContenedores.Resultado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).ToList();

                                    RN_Bill_InvoiceContainerExpo _objDet = new RN_Bill_InvoiceContainerExpo();
                                    string _error = string.Empty;
                                    var listaDetalle = _objValidacion.consultaDetalle(objCab.CNTR_ID, out _error);

                                    ////////////////////////////
                                    //    AGREGA EL DETALLE
                                    ////////////////////////////
                                    foreach (var Det in listaDetalle)
                                    {
                                        v_contador += 1;
                                        v_indice += 1;
                                        objCab.CNTR_CONTAINERS = objCab.CNTR_CONTAINERS == string.Empty ? Det.CNTR_CONTAINER : objCab.CNTR_CONTAINERS + "," + Det.CNTR_CONTAINER;
                                        if (v_contador == 15) { objCab.CNTR_CONTAINERS = objCab.CNTR_CONTAINERS + " "; v_contador = 0; }

                                        objCab.CNTR_CONTENEDOR20 += Det.CNTR_TYSZ_SIZE == "20" ? 1 : 0;
                                        objCab.CNTR_CONTENEDOR40 += Det.CNTR_TYSZ_SIZE == "40" ? 1 : 0;
                                        objCab.CNTR_SIZE = Det.CNTR_TYSZ_SIZE;

                                        objDetalle = new Cls_Bill_Container_Expo();
                                        objDetalle.VISTO = false;
                                        objDetalle.CNTR_CAB_ID = objCab.CNTR_ID;
                                        objDetalle.CNTR_ID = objCab.CNTR_ID;
                                        objDetalle.CNTR_CONSECUTIVO = Det.CNTR_CONSECUTIVO;
                                        objDetalle.CNTR_CONTAINER = Det.CNTR_CONTAINER;
                                        objDetalle.CNTR_TYPE = Det.CNTR_TYPE;
                                        objDetalle.CNTR_TYSZ_SIZE = Det.CNTR_TYSZ_SIZE;
                                        objDetalle.CNTR_TYSZ_ISO = Det.CNTR_TYSZ_ISO;
                                        objDetalle.CNTR_TYSZ_TYPE = Det.CNTR_TYSZ_TYPE;
                                        objDetalle.CNTR_FULL_EMPTY_CODE = Det.CNTR_FULL_EMPTY_CODE;
                                        objDetalle.CNTR_YARD_STATUS = Det.CNTR_YARD_STATUS;
                                        try { objDetalle.CNTR_TEMPERATURE = (decimal)Det.CNTR_TEMPERATURE; } catch { objDetalle.CNTR_TEMPERATURE = 0; }
                                        objDetalle.CNTR_TYPE_DOCUMENT = Det.CNTR_TYPE_DOCUMENT;
                                        objDetalle.CNTR_DOCUMENT = Det.CNTR_DOCUMENT;
                                        objDetalle.CNTR_VEPR_REFERENCE = Det.CNTR_VEPR_REFERENCE;
                                        objDetalle.CNTR_CLNT_CUSTOMER_LINE = Det.CNTR_CLNT_CUSTOMER_LINE;
                                        objDetalle.CNTR_LCL_FCL = Det.CNTR_LCL_FCL;
                                        objDetalle.CNTR_CATY_CARGO_TYPE = Det.CNTR_CATY_CARGO_TYPE;
                                        objDetalle.CNTR_FREIGHT_KIND = Det.CNTR_FREIGHT_KIND;
                                        objDetalle.CNTR_DD = (int)Det.CNTR_DD;
                                        objDetalle.CNTR_BKNG_BOOKING = Det.CNTR_BKNG_BOOKING;
                                        objDetalle.FECHA_CAS = Det.FECHA_CAS;
                                        objDetalle.CNTR_AISV = Det.CNTR_AISV;
                                        objDetalle.CNTR_HOLD = (int)Det.CNTR_HOLD;
                                        objDetalle.CNTR_REEFER_CONT = Det.CNTR_REEFER_CONT;
                                        objDetalle.CNTR_VEPR_VSSL_NAME = Det.CNTR_VEPR_VSSL_NAME;
                                        objDetalle.CNTR_VEPR_VOYAGE = Det.CNTR_VEPR_VOYAGE;
                                        objDetalle.CNTR_VEPR_ACTUAL_ARRIVAL = Det.CNTR_VEPR_ACTUAL_ARRIVAL;
                                        objDetalle.CNTR_VEPR_ACTUAL_DEPARTED = Det.CNTR_VEPR_ACTUAL_DEPARTED;
                                        try { objDetalle.ESTADO_ERROR = objValidacionesDet.Where(p => p.CNTR_CONSECUTIVO == Det.CNTR_CONSECUTIVO).LastOrDefault().ERROR; } catch { }
                                        objCab.Detalle.Add(objDetalle);
                                    }
                                    //v_ds.Tables.Add(v_dt);
                                    //objCab.CNTR_CONTAINERSXML = v_ds.GetXml(); 
                                    objCabecera.Add(objCab);
                                }

                            }
                            else
                            {
                                objCab = new Cls_Bill_CabeceraExpo();
                                objCab.CNTR_VEPR_REFERENCE = lista.CNTR_VEPR_REFERENCE;//cab.NAVE_REF.ToString();
                                objCab.CNTR_BKNG_BOOKING = lista.CNTR_BKNG_BOOKING;//cab.BOOKING;
                                //try { objCab.CNTR_CLIENT_ID = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault().Ruc; } catch { }
                                //try { objCab.CNTR_CLIENT = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault().Cliente; } catch { }
                                //try { objCab.CNTR_CLIENTE = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault(); } catch { }
                                //try { objCab.CNTR_CLIENTES = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).ToList(); } catch { }
                                //try { objCab.CNTR_CREDITO = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault().DatoCliente.DIAS_CREDITO > 0 ? true : false; } catch { }
                                try { objCab.CNTR_CONTADO = !objCab.CNTR_CREDITO; } catch { }

                                resultado = InvoiceTypeConfig.ObtenerInvoicetypes();
                                if (resultado.Exitoso)
                                {
                                    var invoiceType = resultado.Resultado.Where(p => p.codigo == (objCab.CNTR_CREDITO ? "EXPORCREFULL" : "EXPOCONFULL"));
                                    objCab.CNTR_INVOICE_TYPE = invoiceType.FirstOrDefault().valor.ToString();
                                    objCab.CNTR_INVOICE_TYPE_NAME = objCab.CNTR_CREDITO ? "EXPORCREFULL" : "EXPOCONFULL";
                                }
                                else
                                {
                                    continue;
                                }
                                objCab.LLAVE = string.Format("{0};{1}", lista.CNTR_BKNG_BOOKING, 0);
                                objCab.CNTR_CONTAINERS = "";
                                objCab.CNTR_FECHA = DateTime.Now;
                                objCab.CNTR_ESTADO = "N";
                                objCab.CNTR_VEPR_VSSL_NAME = lista.CNTR_VEPR_VSSL_NAME;//cab.NAVE;
                                objCab.CNTR_VEPR_VOYAGE = lista.CNTR_VEPR_VOYAGE;// cab.VIAJE;
                                objCab.CNTR_VEPR_ACTUAL_ARRIVAL = lista.CNTR_VEPR_ACTUAL_ARRIVAL;//cab.LLEGADA;
                                objCab.CNTR_VEPR_ACTUAL_DEPARTED = lista.CNTR_VEPR_ACTUAL_DEPARTED;//cab.SALIDA;
                                objCab.CNTR_USUARIO_CREA = ClsUsuario.loginname;

                                objCab.CNTR_CLNT_CUSTOMER_LINE = lista.CNTR_CLNT_CUSTOMER_LINE;
                                objCab.CNTR_BKNG_BOOKING = lista.CNTR_BKNG_BOOKING;
                                objCab.CNTR_CONTADO = false;
                                objCab.CNTR_CREDITO = false;
                                //objCab.BOOKINGLINE = lista.CNTR_CLNT_CUSTOMER_LINE + " | " + lista.CNTR_BKNG_BOOKING;
                                objCab.BOOKINGLINE = string.Format("{0} | {1} | {2}", lista.CNTR_CLNT_CUSTOMER_LINE, lista.CNTR_BKNG_BOOKING, objCab.CNTR_INVOICE_TYPE.Replace("2DA_MAN_EXPO_CNTRS_", ""));
                                objCab.CNTR_SIZE_RF = lista.CNTR_TYSZ_SIZE.ToString() + " | REEF:" + lista.CNTR_REEFER_CONT;
                                //objCab.CNTR_PROCESADO = ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).Count() > 0 ? true : false;
                                //objCab.CNTR_ID = ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).Count() > 0 ? ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).FirstOrDefault().CNTR_ID:0 ;
                                /*
                                try
                                {
                                    if (ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).Count() > 0)
                                    {
                                        objCab.CNTR_PROCESADO = true;
                                        objCab.CNTR_ID = ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).FirstOrDefault().CNTR_ID;
                                        objCab.CNTR_ESTADO = ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).FirstOrDefault().CNTR_ESTADO;
                                    }
                                }
                                catch { }
                                */
                                v_contador = 0;
                                v_indice = 0;
                                var listaDetalle = ListaContenedores.Resultado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).ToList();
                                //crea detalle
                                //System.Data.DataSet v_ds = new System.Data.DataSet("CONTENEDORES");
                                //System.Data.DataTable v_dt = new System.Data.DataTable("CONTAINERS");
                                //v_dt.Columns.Add("ID", typeof(Int64));
                                //v_dt.Columns.Add("CONTAINER", typeof(String));

                                ////////////////////////////
                                //    AGREGA EL DETALLE
                                ////////////////////////////
                                foreach (var Det in listaDetalle)
                                {
                                    v_contador += 1;
                                    v_indice += 1;
                                    objCab.CNTR_CONTAINERS = objCab.CNTR_CONTAINERS == string.Empty ? Det.CNTR_CONTAINER : objCab.CNTR_CONTAINERS + "," + Det.CNTR_CONTAINER;
                                    if (v_contador == 15) { objCab.CNTR_CONTAINERS = objCab.CNTR_CONTAINERS + " "; v_contador = 0; }

                                    //System.Data.DataRow v_dr = v_dt.NewRow();
                                    //v_dr["ID"] = Det.CNTR_CONSECUTIVO;
                                    //v_dr["CONTAINER"] = Det.CNTR_CONTAINER;
                                    //v_dt.Rows.Add(v_dr);

                                    objCab.CNTR_CONTENEDOR20 += Det.CNTR_TYSZ_SIZE == "20" ? 1 : 0;
                                    objCab.CNTR_CONTENEDOR40 += Det.CNTR_TYSZ_SIZE == "40" ? 1 : 0;
                                    objCab.CNTR_SIZE = Det.CNTR_TYSZ_SIZE;
                                   

                                    objDetalle = new Cls_Bill_Container_Expo();
                                    objDetalle.VISTO = false;
                                    if (objCab.CNTR_PROCESADO)
                                    {
                                        objDetalle.CNTR_CAB_ID = objCab.CNTR_ID;
                                    }
                                    objDetalle.CNTR_ID = objCab.CNTR_ID;
                                    objDetalle.CNTR_CONSECUTIVO = Det.CNTR_CONSECUTIVO;
                                    objDetalle.CNTR_CONTAINER = Det.CNTR_CONTAINER;
                                    objDetalle.CNTR_TYPE = Det.CNTR_TYPE;
                                    objDetalle.CNTR_TYSZ_SIZE = Det.CNTR_TYSZ_SIZE;
                                    objDetalle.CNTR_TYSZ_ISO = Det.CNTR_TYSZ_ISO;
                                    objDetalle.CNTR_TYSZ_TYPE = Det.CNTR_TYSZ_TYPE;
                                    objDetalle.CNTR_FULL_EMPTY_CODE = Det.CNTR_FULL_EMPTY_CODE;
                                    objDetalle.CNTR_YARD_STATUS = Det.CNTR_YARD_STATUS;
                                    try { objDetalle.CNTR_TEMPERATURE = (decimal)Det.CNTR_TEMPERATURE; } catch { objDetalle.CNTR_TEMPERATURE = 0; }
                                    objDetalle.CNTR_TYPE_DOCUMENT = Det.CNTR_TYPE_DOCUMENT;
                                    objDetalle.CNTR_DOCUMENT = Det.CNTR_DOCUMENT;
                                    objDetalle.CNTR_VEPR_REFERENCE = Det.CNTR_VEPR_REFERENCE;
                                    objDetalle.CNTR_CLNT_CUSTOMER_LINE = Det.CNTR_CLNT_CUSTOMER_LINE;
                                    objDetalle.CNTR_LCL_FCL = Det.CNTR_LCL_FCL;
                                    objDetalle.CNTR_CATY_CARGO_TYPE = Det.CNTR_CATY_CARGO_TYPE;
                                    objDetalle.CNTR_FREIGHT_KIND = Det.CNTR_FREIGHT_KIND;
                                    objDetalle.CNTR_DD = (int)Det.CNTR_DD;
                                    objDetalle.CNTR_BKNG_BOOKING = Det.CNTR_BKNG_BOOKING;
                                    objDetalle.FECHA_CAS = Det.FECHA_CAS;
                                    objDetalle.CNTR_AISV = Det.CNTR_AISV;
                                    objDetalle.CNTR_HOLD = (int)Det.CNTR_HOLD;
                                    objDetalle.CNTR_REEFER_CONT = Det.CNTR_REEFER_CONT;
                                    objDetalle.CNTR_VEPR_VSSL_NAME = Det.CNTR_VEPR_VSSL_NAME;
                                    objDetalle.CNTR_VEPR_VOYAGE = Det.CNTR_VEPR_VOYAGE;
                                    objDetalle.CNTR_VEPR_ACTUAL_ARRIVAL = Det.CNTR_VEPR_ACTUAL_ARRIVAL;
                                    objDetalle.CNTR_VEPR_ACTUAL_DEPARTED = Det.CNTR_VEPR_ACTUAL_DEPARTED;
                                    try { objDetalle.ESTADO_ERROR = objValidacionesDet.Where(p => p.CNTR_CONSECUTIVO == Det.CNTR_CONSECUTIVO).LastOrDefault().ERROR; } catch { }
                                    objCab.Detalle.Add(objDetalle);
                                }
                                //v_ds.Tables.Add(v_dt);
                                //objCab.CNTR_CONTAINERSXML = v_ds.GetXml(); 
                                objCabecera.Add(objCab);
                            }
                        }

                        foreach (Cls_Bill_CabeceraExpo _objCab in objCabecera)
                        {
                            if (_objCab.CNTR_ESTADO == "E")
                            {
                                _objCab.ORDEN = 1;
                            }
                            if (_objCab.CNTR_ESTADO == "N")
                            {
                                if (_objCab.CNTR_PROCESADO)
                                {
                                    _objCab.ORDEN = 2;
                                }
                                else
                                {
                                    _objCab.ORDEN = 3;
                                }
                            }
                            if (_objCab.CNTR_ESTADO == "V" || _objCab.CNTR_ESTADO == "F")
                            {
                                _objCab.ORDEN = 4;
                            }
                        }

                        var objCabceraOrdenada = objCabecera.OrderBy(q=> q.ORDEN).ToList();

                        tablePagination.DataSource = objCabceraOrdenada;
                        tablePagination.DataBind();
                       

                        Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabceraOrdenada;
                        Session["TransaccionContDet"] = objCabceraOrdenada;
                        TXTNAVE.Text = string.Empty;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se encontraron datos, revise el criterio de consulta"));
                        return;
                    }
                    this.Ocultar_Mensaje();
                    UPCAB.Update();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }

        protected void BtnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null) { return; }

                List<Cls_Bill_CabeceraExpo> objCabceraOrdenada = new List<Cls_Bill_CabeceraExpo>();
                objCabceraOrdenada = objCabecera;
                if (txtFiltro.Text != string.Empty)
                {
                
                    if (rbBooking.Checked)
                    {
                        objCabceraOrdenada = (from A in objCabecera
                                             where  A.CNTR_BKNG_BOOKING.ToUpper().Contains(txtFiltro.Text.ToUpper())
                                              select A).ToList();
                    }

                    if (rbLinea.Checked)
                    {
                        objCabceraOrdenada = (from A in objCabecera
                                              where A.CNTR_CLNT_CUSTOMER_LINE.ToUpper().Contains(txtFiltro.Text.ToUpper())
                                              select A).ToList();
                    }

                    if (rbContenedor.Checked)
                    {
                        objCabceraOrdenada = (from A in objCabecera
                                              where A.CNTR_CONTAINERS.ToUpper().Contains(txtFiltro.Text.ToUpper())
                                              select A).ToList();
                    }
                }

                tablePagination.DataSource = objCabceraOrdenada;
                tablePagination.DataBind();

                this.Ocultar_Mensaje();
                UPCAB.Update();

                GrillaDetalle.DataSource = null;
                GrillaDetalle.DataBind();
                txtFiltro.Text = string.Empty;
                UPDET.Update();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        protected void BtnActualizar_Click(object sender, EventArgs e)
        {
            try {
                string COD_CABECERA = Session["CodigoCabecera" + this.hf_BrowserWindowName.Value] as string;

                if (COD_CABECERA == null) { return; }
                if (COD_CABECERA == "0") { return; }

                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera.Where(p => p.CNTR_ID == long.Parse(COD_CABECERA)).FirstOrDefault().CNTR_ESTADO != "E") { return; }

                RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
                string v_error = _obj.actualizarStatus(long.Parse(COD_CABECERA), "N");

                if (v_error != string.Empty)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", v_error));
                }
                else
                {
                    this.Mostrar_Mensaje(string.Format("<b>Información! </b> Transacción exitosa"));

                    objCabecera.Where(p => p.CNTR_ID == long.Parse(COD_CABECERA)).FirstOrDefault().CNTR_ESTADO = "N";
                    objCabecera.Where(p => p.CNTR_ID == long.Parse(COD_CABECERA)).FirstOrDefault().ORDEN = 2;

                    objCabecera = objCabecera.OrderBy(p => p.ORDEN).ToList();
                    Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                    Session["TransaccionContDet"] = objCabecera;
                    tablePagination.DataSource = objCabecera;
                    tablePagination.DataBind();
                    UPCAB.Update();
                    UPMENSAJE.Update();
                }
            }
            catch
            {

            }
        }

        protected void BtnFiltrar1_Click(object sender, EventArgs e)
        {
            FiltrarPorColores(1);
        }
        protected void BtnFiltrar2_Click(object sender, EventArgs e)
        {
            FiltrarPorColores(2);
        }
        protected void BtnFiltrar3_Click(object sender, EventArgs e)
        {
            FiltrarPorColores(3);
        }
        protected void BtnFiltrar4_Click(object sender, EventArgs e)
        {
            FiltrarPorColores(4);
        }

        public void FiltrarPorColores(int valor)
        {
            try
            {
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null) { return; }

                List<Cls_Bill_CabeceraExpo> objCabceraOrdenada = new List<Cls_Bill_CabeceraExpo>();
                objCabceraOrdenada = objCabecera;
                if (objCabceraOrdenada.Count > 0)
                {
                    objCabceraOrdenada = (from A in objCabecera
                                          where A.ORDEN == valor
                                          select A).ToList();
                }

                tablePagination.DataSource = objCabceraOrdenada;
                tablePagination.DataBind();

                this.Ocultar_Mensaje();
                UPCAB.Update();

                GrillaDetalle.DataSource = null;
                GrillaDetalle.DataBind();
                txtFiltro.Text = string.Empty;
                UPDET.Update();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }
    }
}