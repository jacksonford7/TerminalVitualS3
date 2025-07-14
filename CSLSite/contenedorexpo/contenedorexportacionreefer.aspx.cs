using BillionEntidades;
using BillionReglasNegocio;
using CSLSite;
using N4.Entidades;
using N4Ws.Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.contenedorexpo
{
    public partial class contenedorexportacionreefer : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private List<Cls_Bill_CabeceraExpo> objCabecera = new List<Cls_Bill_CabeceraExpo>();
        private Cls_Bill_Container_Expo objDetalle = new Cls_Bill_Container_Expo();
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
        
        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
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
            UPDETALLE.Update();
            UPCARGA.Update();
            UPBOTONES.Update();
        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
        }

        private void Limpia_Datos_cliente()
        {
            this.TXTARRIVAL.Text = string.Empty;
            this.TXTDEPARTED.Text = string.Empty;
            this.txtID.Text = string.Empty;
            this.TXTNAVE.Text = string.Empty;
            this.TXTREFERENCIA.Text = string.Empty;
            this.TXTVOYAGE.Text = string.Empty;
            this.TXTVSSLNAME.Text = string.Empty;
        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
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
                this.Mostrar_Mensaje( string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCabecera = new List<Cls_Bill_CabeceraExpo>();
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
            Session["TransaccionContDet"] = objCabecera;
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
            try {

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
                this.Mostrar_Mensaje( string.Format("<b>Error! </b>{0}", ex.Message));
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
                    //var ClsUsuario = HttpContext.Current.Session["control"] as usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    this.BtnFacturar.Attributes["disabled"] = "disabled";
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
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
                    GrillaDetalle.DataSource = objCabecera.Where(p => p.CNTR_BKNG_BOOKING == txtID.Text).FirstOrDefault().Detalle;
                    GrillaDetalle.DataBind();      
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        #endregion

        #region "Controles"
        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool ChkEstado = this.ChkTodos.Checked;
                objCabecera = Session["Transaccion"] as List<Cls_Bill_CabeceraExpo>;
                objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }

                foreach (var Det in objCabecera)
                {
                    var Detalle = objCabecera.FirstOrDefault(f => f.CNTR_BKNG_BOOKING.Equals(Det.CNTR_BKNG_BOOKING.Trim()));
                    if (Detalle != null)
                    {
                        if (!Detalle.CNTR_PROCESADO)
                            Detalle.VISTO = ChkEstado;
                    }
                }

                tablePagination.DataSource = objCabecera;
                tablePagination.DataBind();

                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                Session["TransaccionContDet"] = objCabecera;

                this.Actualiza_Panele_Detalle();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", ex.Message));
            }
        }
        protected void CHKFA_CheckedChanged(object sender, EventArgs e)
                {
                    try
                    { 
                        GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                        CheckBox CHKFA = (CheckBox)row.FindControl("CHKFA");
                        String BOOKING = tablePagination.DataKeys[row.RowIndex].Value.ToString();

                        objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_CabeceraExpo>;
                        if (objCabecera == null)
                        {
                            return;
                        }
                        var _cabecera = objCabecera.FirstOrDefault(f => f.CNTR_BKNG_BOOKING.Equals(BOOKING.Trim()));
                        if (_cabecera != null)
                        {
                            if (!_cabecera.CNTR_PROCESADO)
                            {
                                _cabecera.VISTO = CHKFA.Checked;
                            }
                        }
            
                        /*tablePagination.DataSource = objCabecera;
                        tablePagination.DataBind();*/

                        Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                        Session["TransaccionContDet"] = objCabecera;

                        List<Cls_Bill_CabeceraExpo> objCabceraOrdenada = new List<Cls_Bill_CabeceraExpo>();
                        objCabceraOrdenada = objCabecera;
                        if (txtFiltro.Text != string.Empty)
                        {

                            if (rbBooking.Checked)
                            {
                                objCabceraOrdenada = (from A in objCabecera
                                                      where A.CNTR_BKNG_BOOKING.ToUpper().Contains(txtFiltro.Text.ToUpper())
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


                            tablePagination.DataSource = objCabceraOrdenada;
                            tablePagination.DataBind();

                        }
                        else
                        {
                            tablePagination.DataSource = objCabecera;
                            tablePagination.DataBind();
                        }

                        this.Actualiza_Panele_Detalle();
                    }
                    catch (Exception ex)
                    {
                        this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
                    }
                }
        protected void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList cmbCliente = (DropDownList)row.FindControl("cmbCliente");
            String wContenedor = tablePagination.DataKeys[row.RowIndex].Value.ToString();

            objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_CabeceraExpo>;

            var currentStatRow = (from objCab in objCabecera.AsEnumerable()
                                  where objCab.CNTR_BKNG_BOOKING == wContenedor
                                  select objCab).FirstOrDefault();

            string EmpresaSelect = cmbCliente.SelectedValue.ToString();
            string IdEmpresa = string.Empty;

            if (EmpresaSelect.Split('-').ToList().Count > 1)
            {
                IdEmpresa = EmpresaSelect.Split('-').ToList()[1].Trim();
            }
            else
            {
                IdEmpresa = cmbCliente.SelectedValue.ToString();
            }
            currentStatRow.CNTR_CLIENT_ID2 = cmbCliente.SelectedValue.ToString();
            currentStatRow.CNTR_CLIENT_ID = IdEmpresa;
            currentStatRow.CNTR_CLIENT = cmbCliente.SelectedItem.Text.ToString();
            currentStatRow.CNTR_CLIENTE = currentStatRow.CNTR_CLIENTES.Where(p => p.Ruc == IdEmpresa.ToString()).FirstOrDefault();

            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
            Session["TransaccionContDet"] = objCabecera;
            this.Actualiza_Panele_Detalle();
        }
        #endregion

        #region "Grid Principal"
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //LLENADO DE COMBO DEL GRID
                    DataTable dt = new DataTable();
                    DropDownList cmbCliente = (DropDownList)e.Row.FindControl("cmbCliente");
                    string row_idcliente = DataBinder.Eval(e.Row.DataItem, "CNTR_CLIENT_ID2").ToString();

                    String wContenedor = tablePagination.DataKeys[e.Row.RowIndex].Value.ToString();
                    var currentStatRow = (from objCab in objCabecera.AsEnumerable()
                                          where objCab.CNTR_BKNG_BOOKING == wContenedor
                                          select objCab).FirstOrDefault();



                    cmbCliente.Items.Clear();
                    dt.Columns.Add(new DataColumn("CNTR_CLIENT_ID2", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("CNTR_CLIENT", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("CNTR_RUC", Type.GetType("System.String")));


                    int i = 1;
                    foreach (var cab in currentStatRow.CNTR_CLIENTES)
                    {
                        String wRuc = cab != null && cab.Ruc == null ? "0" : cab.Ruc.ToString();
                        String wClienteTEXT = string.Empty;
                        try { wClienteTEXT = cab != null && cab.Cliente == null ? "0" : cab.Tipo + " - " + cab.Cliente.ToString() + " - Dias Credito:" + cab.DatoCliente.DIAS_CREDITO; } catch { wClienteTEXT = cab != null && cab.Cliente == null ? "0" : cab.Tipo + " - " + cab.Cliente.ToString(); }

                        if (dt.Select(string.Format(" CNTR_RUC='{0}' and CNTR_CLIENT='{1}'", wRuc, wClienteTEXT)).Count() == 0)
                        {
                            dt.Rows.Add(new String[] { string.Format("{0}-{1}", i, wRuc), wClienteTEXT, wRuc });
                            dt.AcceptChanges();
                            i++;
                        }
                    }


                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    ds.AcceptChanges();
                    cmbCliente.DataSource = ds;

                    if (!string.IsNullOrEmpty(row_idcliente))
                    {
                        cmbCliente.SelectedValue = row_idcliente;
                    }

                    cmbCliente.DataBind();



                    //cambia color de filas
                    //string estado = DataBinder.Eval(e.Row.DataItem, "REEFER").ToString();
                    //string conectado = DataBinder.Eval(e.Row.DataItem, "CONECTADO").ToString();
                    CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");

                    if (Chk.Checked)//(estado.Equals("RF") && conectado.Equals("NO CONECTADO"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.DarkBlue;
                        e.Row.Enabled = false;
                    }

                    this.Actualiza_Panele_Detalle();
                }
            }catch(Exception ex)
            {
                //this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                string CNTR_BKNG_BOOKING = e.CommandArgument.ToString();
                objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null){return;}
                this.txtID.Text = CNTR_BKNG_BOOKING;
                GrillaDetalle.DataSource = objCabecera.Where(p => p.CNTR_BKNG_BOOKING == CNTR_BKNG_BOOKING).FirstOrDefault().Detalle;
                GrillaDetalle.DataBind();
                UPMODAL.Update();
                UPMODAL.Update();
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
                this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }

        }
        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //objCabecera = Session["Transaccion"] as Cls_Bil_Cabecera;
                objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_CabeceraExpo>;

                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje( string.Format("<b>Informativo! </b>Debe generar la consulta"));
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
                this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        #endregion
        
        #region "Botones"
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.BtnFacturar.Attributes.Add("disabled", "disabled");
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
                        this.Mostrar_Mensaje( string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.TXTNAVE.Focus();
                        return;
                    }

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    //Carga de data
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //resultado de entidad contenedor
                    var Contenedor = new N4.Exportacion.container();
                    var ListaContenedores = Contenedor.CargaporReferencia(ClsUsuario.loginname, this.TXTNAVE.Text.ToString());

                    //Lista de AISV para obtener clientes
                    Dictionary<string, string> v_ListaAISV = null;
                    List<string> v_ListaBooking = null;
                    Dictionary<string,string> v_ListaDAE = null;

                    if (ListaContenedores.Exitoso)
                    {
                        //v_ListaAISV = (from A in ListaContenedores.Resultado.Distinct()
                        //               where A.CNTR_AISV != null
                        //               select new
                        //               {
                        //                   A.CNTR_AISV
                        //                   ,A.CNTR_BKNG_BOOKING 
                        //               }
                        //            ).Distinct().ToDictionary(p=> p.CNTR_AISV , p=> p.CNTR_BKNG_BOOKING);

                       var ListaAISV = (from A in ListaContenedores.Resultado.Where(p => p.CNTR_TYSZ_TYPE.Equals("RF"))
                                     where A.CNTR_AISV != null && A.CNTR_BKNG_BOOKING != null 
                                        select new
                                     {
                                         CNTR_AISV = A.CNTR_AISV.Trim(),
                                         CNTR_BKNG_BOOKING = A.CNTR_BKNG_BOOKING.Trim() != null ? A.CNTR_BKNG_BOOKING.Trim() : string.Empty
                                     }
                                    ).Distinct();

                        v_ListaAISV = new Dictionary<string, string>();
                       foreach (var a in ListaAISV)
                        {
                            try {
                                v_ListaAISV.Add(a.CNTR_AISV, a.CNTR_BKNG_BOOKING);
                            }
                            catch { continue; }
                        }

                     
                        v_ListaBooking = (from A in ListaContenedores.Resultado.Where(p => p.CNTR_TYSZ_TYPE.Equals("RF"))
                                          where A.CNTR_BKNG_BOOKING != null 
                                          select A.CNTR_BKNG_BOOKING).Distinct().ToList();

                        //v_ListaDAE = (from A in ListaContenedores.Resultado.Distinct()
                        //              where A.CNTR_DOCUMENT != null

                        //                select new
                        //                {
                        //                    CNTR_DOCUMENT = A.CNTR_DOCUMENT,
                        //                    CNTR_BKNG_BOOKING = A.CNTR_BKNG_BOOKING
                        //                }
                        //              ).Distinct().ToDictionary(p => p.CNTR_DOCUMENT, p => p.CNTR_BKNG_BOOKING);


                        var ListaDAE = (from A in ListaContenedores.Resultado.Where(p => p.CNTR_TYSZ_TYPE.Equals("RF"))
                                        where A.CNTR_DOCUMENT != null && A.CNTR_BKNG_BOOKING != null
                                        select new
                                        {
                                            CNTR_DOCUMENT = A.CNTR_DOCUMENT.Trim(),
                                            CNTR_BKNG_BOOKING = A.CNTR_BKNG_BOOKING.Trim() != null ? A.CNTR_BKNG_BOOKING.Trim(): string.Empty
                                        }
                                    ).Distinct();
                        v_ListaDAE = new Dictionary<string, string>();
                        foreach (var a in ListaDAE)
                        {
                            try
                            {
                                v_ListaDAE.Add(a.CNTR_DOCUMENT, a.CNTR_BKNG_BOOKING);
                            }
                            catch { continue; }
                        }



                    }


                    //INFORMACION DEL CONTENEDOR
                    List<N4.Exportacion.ClientePago> ClientesPago = null;
                    try
                    {
                        ClientesPago = new List<N4.Exportacion.ClientePago>();
                        //var ClientesPago = N4.Exportacion.ClientePago.ObtenerClientesPago(this.TXTNAVE.Text.ToString(), ClsUsuario.Usuario);
                        var ClientesPago1 = N4.Exportacion.ClientePago.ObtenerClientesPagoPorAISV(v_ListaAISV, ClsUsuario.loginname);
                        var ClientesPago2 = N4.Exportacion.ClientePago.ObtenerClientesPagoPorBookings(v_ListaBooking, ClsUsuario.loginname);
                        var ClientesPago3 = N4.Exportacion.ClientePago.ObtenerClientesPagoPorDAE(v_ListaDAE, ClsUsuario.loginname);
                        if (ClientesPago1.Exitoso) { ClientesPago.AddRange(ClientesPago1.Resultado); }
                        if (ClientesPago2.Exitoso) { ClientesPago.AddRange(ClientesPago2.Resultado); }
                        if (ClientesPago3.Exitoso) { ClientesPago.AddRange(ClientesPago3.Resultado); }
                    }
                    catch
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! No se puede realizar la consulta, favor actualice la pagina. </b>"));
                        return;
                    }

                    var ListaClientesPago = ClientesPago;//N4.Exportacion.ClientePago.ObtenerClientesPago(this.TXTNAVE.Text.ToString(), ClsUsuario.Usuario); ;
                    var ListaRuc = N4.Exportacion.ClientePago.PurgarListaClientes(ListaClientesPago);
                    var ListaClientesNombres = N4.Entidades.Cliente.ObtenerClientes(ListaRuc, ClsUsuario.loginname);
                    //crea el listado de clientes generales agrupados por bookimg
                    List<ClientesCombo> listaClientes = new List<ClientesCombo>();

                    //if (ListaClientesPago.Exitoso)
                    // {
                    var v = ListaClientesPago.Where(z => z.BOOKING == "966053739").ToList();
                        foreach (var v_lista in ListaClientesPago)
                        {
                            ClientesCombo v_Clientes;

                            if (v_lista.TERCEROS != null)
                            {
                                v_Clientes = new ClientesCombo();
                                v_Clientes.Orden = 1;
                                v_Clientes.Tipo = "TERCEROS";
                                v_Clientes.Ruc = v_lista.TERCEROS;
                                try { v_Clientes.Cliente = ListaClientesNombres.Resultado.Where(p => p.CLNT_CUSTOMER == v_lista.TERCEROS).ToList().FirstOrDefault().CLNT_NAME; } catch { v_Clientes.Cliente = ""; }
                                try { N4.Entidades.Cliente DatosAdicionales = ListaClientesNombres.Resultado.Where(p => p.CLNT_CUSTOMER == v_lista.TERCEROS).FirstOrDefault(); v_Clientes.DatoCliente = DatosAdicionales; } catch { }
                                v_Clientes.Booking = v_lista.BOOKING;
                                listaClientes.Add(v_Clientes);
                            }

                            if (v_lista.PROFORMA != null)
                            {
                                v_Clientes = new ClientesCombo();
                                v_Clientes.Orden = 2;
                                v_Clientes.Tipo = "PROFORMA";
                                v_Clientes.Ruc = v_lista.PROFORMA;
                                try { v_Clientes.Cliente = ListaClientesNombres.Resultado.Where(p => p.CLNT_CUSTOMER == v_lista.PROFORMA).ToList().FirstOrDefault().CLNT_NAME; } catch { v_Clientes.Cliente = ""; }
                                try { v_Clientes.DatoCliente = ListaClientesNombres.Resultado.Where(p => p.CLNT_CUSTOMER == v_lista.PROFORMA).FirstOrDefault(); } catch { }
                                v_Clientes.Booking = v_lista.BOOKING;
                                listaClientes.Add(v_Clientes);
                            }

                            if (v_lista.AISV != null)
                            {
                                v_Clientes = new ClientesCombo();
                                v_Clientes.Orden = 3;
                                v_Clientes.Tipo = "AISV";
                                v_Clientes.Ruc = v_lista.AISV;
                                try { v_Clientes.Cliente = ListaClientesNombres.Resultado.Where(p => p.CLNT_CUSTOMER == v_lista.AISV).ToList().FirstOrDefault().CLNT_NAME; } catch { v_Clientes.Cliente = ""; }
                                try { v_Clientes.DatoCliente = (Cliente)ListaClientesNombres.Resultado.Where(p => p.CLNT_CUSTOMER == v_lista.AISV).FirstOrDefault(); } catch { }
                                v_Clientes.Booking = v_lista.BOOKING;
                                listaClientes.Add(v_Clientes);
                            }
                        }
                    //}
                    //se almacena en memoria el listado de clientes finales
                    Session["ListaClientes" + this.hf_BrowserWindowName.Value] = listaClientes;

                    //INFORMACION DE BOOKING YA PROCESADOS(ya almacenados en base de datos)
                    RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
                    var ListaBookingProcesado = _obj.consultaEntidad_Reefer(this.TXTNAVE.Text.ToString(), out _ERROR);

                    if (ListaContenedores.Exitoso)
                    {
                        //agrupa el detalle por booking
                        var listaAgrupada = (from tbl in ListaContenedores.Resultado.Where(p => p.CNTR_TYSZ_TYPE.Equals("RF"))
                                             group tbl by tbl.CNTR_BKNG_BOOKING into tblbook
                                             select tblbook.FirstOrDefault()
                                            ).ToList();
                        ////////////////////////////
                        //    ARMA LA CABECERA
                        ////////////////////////////
                        foreach (var lista in listaAgrupada)
                        {
                            Cls_Bill_CabeceraExpo objCab = new Cls_Bill_CabeceraExpo();
                            objCab.CNTR_VEPR_REFERENCE = lista.CNTR_VEPR_REFERENCE;//cab.NAVE_REF.ToString();
                            //objCab.CNTR_BKNG_BOOKING = lista.CNTR_BKNG_BOOKING;//cab.BOOKING;

                            if (string.IsNullOrEmpty(lista.CNTR_BKNG_BOOKING))
                            {
                                string x = "ww";
                                objCab.CNTR_BKNG_BOOKING = "";
                            }
                            else {
                                objCab.CNTR_BKNG_BOOKING = lista.CNTR_BKNG_BOOKING;
                            }

                            
                            try { objCab.CNTR_CLIENT_ID = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault().Ruc; } catch { }
                            try{objCab.CNTR_CLIENT = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault().Cliente; } catch { }
                            try{objCab.CNTR_CLIENTE = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault(); } catch { }
                            try{objCab.CNTR_CLIENTES = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q=> q.Orden).Distinct().ToList(); } catch { }
                            try { objCab.CNTR_CREDITO = listaClientes.Where(p => p.Booking == lista.CNTR_BKNG_BOOKING).OrderBy(q => q.Orden).FirstOrDefault().DatoCliente.DIAS_CREDITO > 0 ? true : false; } catch { }
                            try { objCab.CNTR_CONTADO = !objCab.CNTR_CREDITO; } catch { }

                            var resultado = InvoiceTypeConfig.ObtenerInvoicetypes();
                            if (resultado.Exitoso)
                            {
                                var invoiceType = resultado.Resultado.Where(p=> p.valor.Equals("2DA_MAN_EXPO_CNTRS_BILLION_REEFER"));
                                objCab.CNTR_INVOICE_TYPE = invoiceType.FirstOrDefault().valor.ToString() ;
                                objCab.CNTR_INVOICE_TYPE_NAME = objCab.CNTR_CREDITO ? "EXPOREFER" : "EXPOREFER";
                            }
                            else
                            {
                                continue;
                            }
                            //

                            objCab.CNTR_HOURS = (lista.CNTR_HOURS == null ? 0 : lista.CNTR_HOURS);
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
                            objCab.CNTR_PROCESADO = ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING).Count() > 0 ? true : false;

                            int v_contador = 0;
                            int v_indice = 0;
                            var listaDetalle = ListaContenedores.Resultado.Where(p => p.CNTR_BKNG_BOOKING == lista.CNTR_BKNG_BOOKING &&  p.CNTR_TYSZ_TYPE.Equals("RF")).ToList();
                            //crea detalle
                            System.Data.DataSet v_ds = new System.Data.DataSet("CONTENEDORES");
                            System.Data.DataTable v_dt = new System.Data.DataTable("CONTAINERS");
                            v_dt.Columns.Add("ID", typeof(Int64));
                            v_dt.Columns.Add("CONTAINER", typeof(String));

                            ////////////////////////////
                            //    AGREGA EL DETALLE
                            ////////////////////////////
                            foreach (var Det in listaDetalle)
                            {
                                v_contador +=1;
                                v_indice += 1;
                                objCab.CNTR_CONTAINERS = objCab.CNTR_CONTAINERS == string.Empty? Det.CNTR_CONTAINER: objCab.CNTR_CONTAINERS +  "," + Det.CNTR_CONTAINER;
                                if (v_contador ==15) { objCab.CNTR_CONTAINERS = objCab.CNTR_CONTAINERS + " "; v_contador = 0; }

                                System.Data.DataRow v_dr = v_dt.NewRow();
                                v_dr["ID"] = Det.CNTR_CONSECUTIVO;
                                v_dr["CONTAINER"] = Det.CNTR_CONTAINER;
                                v_dt.Rows.Add(v_dr);

                                objCab.CNTR_CONTENEDOR20 += Det.CNTR_TYSZ_SIZE == "20" ? 1 : 0;
                                objCab.CNTR_CONTENEDOR40 += Det.CNTR_TYSZ_SIZE == "40" ? 1 : 0;

                                objDetalle = new Cls_Bill_Container_Expo();
                                objDetalle.VISTO = false;
                                objDetalle.CNTR_CONSECUTIVO = Det.CNTR_CONSECUTIVO;
                                objDetalle.CNTR_CONTAINER = Det.CNTR_CONTAINER;
                                objDetalle.CNTR_TYPE = Det.CNTR_TYPE;
                                objDetalle.CNTR_TYSZ_SIZE = Det.CNTR_TYSZ_SIZE;
                                objDetalle.CNTR_TYSZ_ISO = Det.CNTR_TYSZ_ISO;
                                objDetalle.CNTR_TYSZ_TYPE = Det.CNTR_TYSZ_TYPE;
                                objDetalle.CNTR_FULL_EMPTY_CODE = Det.CNTR_FULL_EMPTY_CODE;
                                objDetalle.CNTR_YARD_STATUS = Det.CNTR_YARD_STATUS;
                                try { objDetalle.CNTR_TEMPERATURE=  (decimal)Det.CNTR_TEMPERATURE; } catch { objDetalle.CNTR_TEMPERATURE = 0; }
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
                                objDetalle.CNTR_PROFORMA = (Det.CNTR_PROFORMA==null ? string.Empty : Det.CNTR_PROFORMA);
                                objDetalle.CNTR_HOURS = (Det.CNTR_HOURS == null ? 0 : Det.CNTR_HOURS);
                                objCab.Detalle.Add(objDetalle);
                            }
                            v_ds.Tables.Add(v_dt);
                            objCab.CNTR_CONTAINERSXML = v_ds.GetXml();
                           objCabecera.Add(objCab);
                        }

                        var objCabOrdenada = objCabecera.OrderBy(p => p.CNTR_BKNG_BOOKING).ToList();

                        tablePagination.DataSource = objCabOrdenada;
                        tablePagination.DataBind();
                        this.BtnFacturar.Attributes.Remove("disabled");
                        Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabOrdenada;
                        Session["TransaccionContDet"] = objCabOrdenada;
                        TXTNAVE.Text = string.Empty;
                        TXTREFERENCIA.Text = objCabOrdenada.FirstOrDefault()?.CNTR_VEPR_REFERENCE;
                        TXTVOYAGE.Text = objCabOrdenada.FirstOrDefault()?.CNTR_VEPR_VOYAGE;
                        TXTVSSLNAME.Text = objCabOrdenada.FirstOrDefault()?.CNTR_VEPR_VSSL_NAME;

                        if (objCabOrdenada.FirstOrDefault().CNTR_VEPR_ACTUAL_ARRIVAL.HasValue)
                        {
                            TXTARRIVAL.Text = objCabOrdenada.FirstOrDefault()?.CNTR_VEPR_ACTUAL_ARRIVAL.Value.ToString("MM-dd-yyyy");
                        }
                        else
                        {
                            TXTARRIVAL.Text = string.Empty;
                        }

                        //TXTARRIVAL.Text = objCabOrdenada.FirstOrDefault()?.CNTR_VEPR_ACTUAL_ARRIVAL.Value.ToString("MM-dd-yyyy");
                        try { TXTDEPARTED.Text = objCabOrdenada.FirstOrDefault()?.CNTR_VEPR_ACTUAL_DEPARTED.Value.ToString("MM-dd-yyyy"); } catch { TXTDEPARTED.Text = string.Empty; }

#if !DEBUG
                        if ((TXTDEPARTED.Text == null) || (TXTDEPARTED.Text == string.Empty))
                        {
                            this.BtnFacturar.Attributes.Add("disabled", "disabled");
                        }
#endif
                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se encontraron datos, revise el criterio de consulta"));
                        return;
                    }
                    this.Ocultar_Mensaje();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }
        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                this.BtnFacturar.Attributes["disabled"] = "disabled";
                string _ERROR = string.Empty;
                OcultarLoading("2");

                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    OcultarLoading("1");
                    this.BtnFacturar.Attributes.Remove("disabled");
                    return;
                }

                objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as List<Cls_Bill_CabeceraExpo>;
                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje( string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA y consultar"));
                    this.TXTNAVE.Focus();
                    this.BtnFacturar.Attributes.Remove("disabled");
                    return;
                }

                if (objCabecera.Where(p => p.VISTO).Count() == 0)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> Debe seleccionar al menos un item para procesar"));
                    this.TXTNAVE.Focus();
                    this.BtnFacturar.Attributes.Remove("disabled");
                    return;
                }

                //VALIDA SI EL CLIENTE TIENE EMAIL
                string mensaje = string.Empty;
                foreach (var listaCab in objCabecera.Where(p => p.VISTO))
                {
                    if (listaCab.CNTR_CLIENTE != null && listaCab.CNTR_CLIENTE.DatoCliente != null)
                    {
                        if (listaCab.CNTR_CLIENTE.DatoCliente.CLNT_FAX_INVC == null)
                        {
                            mensaje = string.Format("<b>Informativo! </b> El cliente {0}-{1}, no tiene ingresada una dirección de correo..No podrá realizar la factura.{2}", listaCab.CNTR_CLIENT_ID, listaCab.CNTR_CLIENTE.Cliente, System.Environment.NewLine);

                        }
                    }

                }
                if (!string.IsNullOrEmpty(mensaje))
                {
                    this.Mostrar_Mensaje(mensaje);
                }

                RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
                string _error = string.Empty;
                _error = _obj.grabarEntidad(objCabecera.Where(p => p.VISTO == true && p.CNTR_CLIENTE != null).ToList());

                RN_Bill_InvoiceContainerExpo regProcesados = new RN_Bill_InvoiceContainerExpo();
                var ListaBookingProcesado = regProcesados.consultaEntidad_Reefer(this.TXTREFERENCIA.Text.ToString(), out _ERROR);

                foreach (var listaCab in objCabecera.Where(p => p.VISTO))
                {
                    listaCab.VISTO = false;
                    listaCab.CNTR_PROCESADO = ListaBookingProcesado.Where(p => p.CNTR_BKNG_BOOKING == listaCab.CNTR_BKNG_BOOKING).Count() > 0 ? true : false;
                }

                tablePagination.DataSource = objCabecera;
                tablePagination.DataBind();

                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                Session["TransaccionContDet"] = objCabecera;

                if (_error != string.Empty)
                {
                    Mostrar_Mensaje( _error);
                }
                else
                {
                   // Mostrar_Mensaje( "<b>Informativo! </b> Transacción existosa");
                    mensaje = string.Format("{0} <b>Informativo! </b> Transacción existosa", mensaje);
                    Mostrar_Mensaje(mensaje);
                }

                //this.Ocultar_Mensaje();
                this.BtnFacturar.Attributes.Remove("disabled");
            }
            catch(Exception ex)
            {
                this.BtnFacturar.Attributes.Remove("disabled");
                this.Ocultar_Mensaje();
                this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
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
                                              where A.CNTR_BKNG_BOOKING.ToUpper().Contains(txtFiltro.Text.ToUpper())
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
                //txtFiltro.Text = string.Empty;
                this.Ocultar_Mensaje();
                UPDETALLE.Update();
                //UPCAB.Update();

                //GrillaDetalle.DataSource = null;
                //GrillaDetalle.DataBind();
                //UPDET.Update();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }
        #endregion
    }
}