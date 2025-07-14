using System;
using BillionEntidades;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BillionReglasNegocio;
using System.Data;
using N4.Entidades;
using N4Ws.Entidad;
using CSLSite;
using System.IO;

namespace CSLSite.contenedorexpo
{
    public partial class transp_actualizadocumentos : System.Web.UI.Page
    {

        #region "Clases"

        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private List<Cls_Bill_CabeceraExpo> objCabecera = new List<Cls_Bill_CabeceraExpo>();
        private Cls_Bill_Container_Expo objDetalle = new Cls_Bill_Container_Expo();
        private List<Cls_Bill_Container_Expo_Det_Validacion> objValidacionesDet = new List<Cls_Bill_Container_Expo_Det_Validacion>();

        private Cls_TRANSP_Cab_Documentos objDocumento = new Cls_TRANSP_Cab_Documentos();
        private Cls_TRANSP_Colaborador objDetalleColaborador = new Cls_TRANSP_Colaborador();
        private Cls_TRANSP_Vehiculo objDetalleVehiculo = new Cls_TRANSP_Vehiculo();

        private Cls_TRANSP_Doc_Colaborador objDocumentoColaborador = new Cls_TRANSP_Doc_Colaborador();
        private Cls_TRANSP_Doc_Vehiculo objDocumentosVehiculo = new Cls_TRANSP_Doc_Vehiculo();

        private Cls_TRANSP_Cab_Documentos objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();

        private static Int64? lm = -3;
        private string OError;

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

        private DataTable dtDocumentos = new DataTable();
        private string xmlDocumentos
        {
            get
            {
                return (string)Session["xmlDocumentos"];
            }
            set
            {
                Session["xmlDocumentos"] = value;
            }

        }

        private string SsTipo
        {
            get
            {
                return (string)Session["SsTipo"];
            }
            set
            {
                Session["SsTipo"] = value;
            }

        }

        private Int64 SsDocumento
        {
            get
            {
                return (Int64)Session["SsDocumento"];
            }
            set
            {
                Session["SsDocumento"] = value;
            }

        }

        private string SsCodigo
        {
            get
            {
                return (string)Session["SsCodigo"];
            }
            set
            {
                Session["SsCodigo"] = value;
            }

        }

        private string SsDescripcion
        {
            get
            {
                return (string)Session["SsDescripcion"];
            }
            set
            {
                Session["SsDescripcion"] = value;
            }

        }

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
            UPCOLABORADOR.Update();
            UPCARGA.Update();
            UPBOTONES.Update();
            UPVEHICULO.Update();
        }

        private void Actualiza_Panele_Detalle()
        {
            //UPDETALLE.Update();
        }

        private void Limpia_Datos_cliente()
        {
            //this.TXTNAVE.Text = string.Empty;
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
            //objCabecera = new List<Cls_Bill_CabeceraExpo>();
            //Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
            //Session["TransaccionContDet"] = objCabecera;

         

            objDocumento = new Cls_TRANSP_Cab_Documentos();
            Session["Documento" + this.hf_BrowserWindowName.Value] = objDocumento;
          

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

//#if !DEBUG
//                this.IsAllowAccess();
//#endif

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    ClsUsuario = Page.Tracker();

                    if (ClsUsuario != null)
                    {

                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.nombres);
                    }

                }

               
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
                Server.HtmlEncode("");

                if (Response.IsClientConnected && !IsPostBack)
                {
                    sinresultado.Visible = false;
                }

                if (!Page.IsPostBack)
                {
                    if (Response.IsClientConnected)
                    {
                        xmlDocumentos = null;

                        sinresultado.Visible = true;
                        this.ruta_completa.Value = string.Empty;
                        this.nombre_archivo1.Value = string.Empty;

                        this.BtnGenerar.Attributes["disabled"] = "disabled";
                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Crear_Sesion();

                    objDocumento = new Cls_TRANSP_Cab_Documentos();
                    Session["Documento" + this.hf_BrowserWindowName.Value] = objDocumento;

                    this.Cargar_Colaboradores();
                    this.Cargar_Chofer();

                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "Colaboradores"
        protected void BtnFiltrarColaborador_Click(object sender, EventArgs e)
        {
            try
            {
                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                if (objDocumento == null)
                { return; }


                List<Cls_TRANSP_Colaborador> objCabceraOrdenada = new List<Cls_TRANSP_Colaborador>();
                objCabceraOrdenada = objDocumento.Colaborador;

                if (txtFiltro.Text != string.Empty)
                {
                    objCabceraOrdenada = (from A in objDocumento.Colaborador
                                          where A.COLABORADOR.ToUpper().Contains(txtFiltro.Text.ToUpper())
                                          select A).ToList();

                    tablePagination.DataSource = objCabceraOrdenada;
                    tablePagination.DataBind();
                }
                else
                {
                    tablePagination.DataSource = objDocumento.Colaborador;
                    tablePagination.DataBind();
                }

                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
            }
        }

        private void Cargar_Colaboradores()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                tablePagination.DataSource = null;
                tablePagination.DataBind();

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                List<Cls_TRANSP_Colaborador> ListColaborador= Cls_TRANSP_Colaborador.Carga_Colaborador(ClsUsuario.ruc, out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {

                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener colaboradores...{0}</b>", cMensajes));
                    this.Actualiza_Paneles();
                    return;
                }


                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;
                objDocumento.Colaborador.Clear();
                foreach (var Det in ListColaborador)
                {
                    objDetalleColaborador = new Cls_TRANSP_Colaborador();
                    objDetalleColaborador.RUC_EMPRESA = Det.RUC_EMPRESA;
                    objDetalleColaborador.NOMBRE_EMPRESA = Det.NOMBRE_EMPRESA;
                    objDetalleColaborador.NOMINA_COD = Det.NOMINA_COD;
                    objDetalleColaborador.COLABORADOR = Det.COLABORADOR;
                    objDetalleColaborador.NOMBRES = Det.NOMBRES;
                    objDetalleColaborador.FECHA_CADUCIDAD = Det.FECHA_CADUCIDAD;
                    objDetalleColaborador.ESTADO = Det.ESTADO;
                    objDetalleColaborador.NOVEDAD = Det.NOVEDAD;
                    objDetalleColaborador.ESTADO2 = Det.ESTADO2;
                    objDetalleColaborador.ORDEN = Det.ORDEN;

                    objDetalleColaborador.APELLIDOS = Det.APELLIDOS;
                    objDetalleColaborador.TIPOSANGRE = Det.TIPOSANGRE;
                    objDetalleColaborador.DIRECCIONDOM = Det.DIRECCIONDOM;
                    objDetalleColaborador.TELFDOM = Det.TELFDOM;
                    objDetalleColaborador.FECHANAC = Det.FECHANAC;
                    objDetalleColaborador.CARGO = Det.CARGO;

                    objDocumento.Colaborador.Add(objDetalleColaborador);
                }

                tablePagination.DataSource = objDocumento.Colaborador;
                tablePagination.DataBind();


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
            }
        }
        #endregion

        #region "Choferes"

        protected void BtnFiltrarVehiculo_Click(object sender, EventArgs e)
        {
            try
            {
                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                if (objDocumento == null)
                { return; }


                List<Cls_TRANSP_Vehiculo> objCabceraOrdenada = new List<Cls_TRANSP_Vehiculo>();
                objCabceraOrdenada = objDocumento.Vehiculo;

                if (TxtFiltrarVehiculo.Text != string.Empty)
                {
                    objCabceraOrdenada = (from A in objDocumento.Vehiculo
                                          where A.PLACA.ToUpper().Contains(TxtFiltrarVehiculo.Text.ToUpper())
                                          select A).ToList();

                    tableVehiculos.DataSource = objCabceraOrdenada;
                    tableVehiculos.DataBind();
                }
                else
                {
                    tableVehiculos.DataSource = objDocumento.Vehiculo;
                    tableVehiculos.DataBind();
                }

                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
            }
        }

        private void Cargar_Chofer()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                tableVehiculos.DataSource = null;
                tableVehiculos.DataBind();

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                List<Cls_TRANSP_Vehiculo> ListVehiculo = Cls_TRANSP_Vehiculo.Carga_Vehiculo(ClsUsuario.ruc, out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {

                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener vehículos...{0}</b>", cMensajes));
                    this.Actualiza_Paneles();
                    return;
                }


                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;
                objDocumento.Vehiculo.Clear();
                foreach (var Det in ListVehiculo)
                {
                    objDetalleVehiculo = new Cls_TRANSP_Vehiculo();
                    objDetalleVehiculo.RUC_EMPRESA = Det.RUC_EMPRESA;
                    objDetalleVehiculo.NOMBRE_EMPRESA = Det.NOMBRE_EMPRESA;
                    objDetalleVehiculo.PLACA = Det.PLACA;
                    objDetalleVehiculo.VE_POLIZA = Det.VE_POLIZA;
                    objDetalleVehiculo.ESTADO = Det.ESTADO;
                    objDetalleVehiculo.NOVEDAD = Det.NOVEDAD;
                    objDetalleVehiculo.ORDEN = Det.ORDEN;

                    objDetalleVehiculo.CLASETIPO = Det.CLASETIPO;
                    objDetalleVehiculo.MARCA = Det.MARCA;
                    objDetalleVehiculo.MODELO = Det.MODELO;
                    objDetalleVehiculo.COLOR = Det.COLOR;
                    objDetalleVehiculo.TIPOCERTIFICADO = Det.TIPOCERTIFICADO;
                    objDetalleVehiculo.CERTIFICADO = Det.CERTIFICADO;
                    objDetalleVehiculo.CATEGORIA = Det.CATEGORIA;
                    objDetalleVehiculo.DESCRIPCIONCATEGORIA = Det.DESCRIPCIONCATEGORIA;
                    objDetalleVehiculo.FECHAPOLIZA = Det.FECHAPOLIZA;
                    objDetalleVehiculo.FECHAMTOP = Det.FECHAMTOP;
                    objDetalleVehiculo.TIPO = Det.TIPO;

                    objDocumento.Vehiculo.Add(objDetalleVehiculo);
                }

                tableVehiculos.DataSource = objDocumento.Vehiculo;
                tableVehiculos.DataBind();


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
            }
        }
        #endregion




        #region "Cargar Documento"
        private void New_ExportFileUpload()
        {
            string crutaCompleta = string.Empty;

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("Ruta");
            bool agrego = false;

            if (fsuploadarchivo.HasFile)
            {
                string rutafile = Server.MapPath(fsuploadarchivo.FileName);
                string finalname;
                crutaCompleta = fsuploadarchivo.PostedFile.FileName.ToString();
                var p = CSLSite.app_start.CredencialesHelper.UploadFile_Transportistas(Server.MapPath(fsuploadarchivo.FileName), fsuploadarchivo.PostedFile.InputStream, out finalname);
                if (!p)
                {
                    sinresultado.Visible = true;
                    sinresultado.InnerText = string.Format("Error..{0}", finalname);
                    return;
                }
                else
                {
                    this.ruta_completa.Value = finalname;
                    agrego = true;
                    dtDocumentos.Rows.Add(finalname);
                }

            }


            if (agrego)
            {
                dtDocumentos.AcceptChanges();
                dtDocumentos.TableName = "Documentos";
                StringWriter sw = new StringWriter();
                dtDocumentos.WriteXml(sw);
                xmlDocumentos = sw.ToString();

                sinresultado.Visible = true;
                sinresultado.InnerText = string.Format("Archivo agregado exitosamente..{0}", crutaCompleta);

            }
            else
            {
                xmlDocumentos = null;
            }

        }

        protected void find_Click(object sender, EventArgs e)
        {
            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                if (Response.IsClientConnected)
                {
                    if (!this.fsuploadarchivo.HasFile)
                    {
                        sinresultado.Visible = true;
                        sinresultado.InnerText = string.Format("Por favor debe cargar por lo menos un archivo (.PDF)");
                        return;
                    }

                    if (this.fsuploadarchivo.HasFile)
                    {
                        var nombrefile = fsuploadarchivo.PostedFile.FileName;
                        if (!string.IsNullOrEmpty(nombrefile))
                        {
                            if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".PDF")
                            {
                                sinresultado.Visible = true;
                                sinresultado.InnerText = string.Format("La extensión del archivo debe ser PDF");
                            }
                        }

                        this.ruta_completa.Value = fsuploadarchivo.PostedFile.FileName;//ESTE USAR
                        this.nombre_archivo1.Value = fsuploadarchivo.PostedFile.FileName;

                        if (fsuploadarchivo.PostedFile.ContentLength > 2500000)
                        {

                            sinresultado.Visible = true;
                            sinresultado.InnerText = string.Format("El archivo pdf excede el tamaño límite 2.5 megabyte");

                            return;
                        }
                    }

                    this.New_ExportFileUpload();

                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.nombres);

                    UPCARGA.Update();
                    sinresultado.Visible = true;

                    if (this.SsTipo.Equals("COLABORADOR"))
                    {
                        objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                        if (objDocumento != null)
                        {
                            var Detalle = objDocumento.Documento_Colaborador.FirstOrDefault(f => f.ID_DOCUMENTO == this.SsDocumento);
                            if (Detalle != null)
                            {
                                this.txtContainers.Text = Detalle.DESC_DOCUMENTO;
                                this.txtID.Text = this.SsDescripcion;
                                Detalle.RUTA_FINAL = this.ruta_completa.Value;
                                Detalle.RUTA = this.nombre_archivo1.Value;
                            }

                            GrillaDetalle.DataSource = objDocumento.Documento_Colaborador;
                            GrillaDetalle.DataBind();

                            UPDET.Update();

                            this.ruta_completa.Value = string.Empty;
                            this.nombre_archivo1.Value = string.Empty;

                        }

                    }
                    if (this.SsTipo.Equals("VEHICULOS"))
                    {
                        objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                        if (objDocumento != null)
                        {
                            var Detalle = objDocumento.Documento_Vehiculo.FirstOrDefault(f => f.ID_DOCUMENTO == this.SsDocumento);
                            if (Detalle != null)
                            {
                                this.txtContainers.Text = Detalle.DESC_DOCUMENTO;
                                this.txtID.Text = this.SsDescripcion;
                                Detalle.RUTA_FINAL = this.ruta_completa.Value;
                                Detalle.RUTA = this.nombre_archivo1.Value;
                            }

                            GrillaDetalle.DataSource = objDocumento.Documento_Vehiculo;
                            GrillaDetalle.DataBind();

                            UPDET.Update();

                            this.ruta_completa.Value = string.Empty;
                            this.nombre_archivo1.Value = string.Empty;
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                this.sinresultado.InnerText = string.Format("Se produjo un error durante la carga de archivo, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "transp_actualizadocumentos", 
                    "find_Click", "", ClsUsuario != null ? ClsUsuario.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }
        #endregion


        #region "Grilla de Detalle Documentos"

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
           
        }

        protected void GrillaDetalle_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Buscar")
            {
                string v_consecutivo = e.CommandArgument.ToString();


                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                if (objDocumento != null)
                {
                    if (this.SsTipo.Equals("COLABORADOR"))
                    {
                        var Detalle = objDocumento.Documento_Colaborador.FirstOrDefault(f => f.ID_DOCUMENTO == Int64.Parse(v_consecutivo));
                        if (Detalle != null)
                        {
                            this.txtContainers.Text = Detalle.DESC_DOCUMENTO;
                            this.SsDocumento = Detalle.ID_DOCUMENTO;
                        }
                    }
                    if (this.SsTipo.Equals("VEHICULOS"))
                    {
                        var Detalle = objDocumento.Documento_Vehiculo.FirstOrDefault(f => f.ID_DOCUMENTO == Int64.Parse(v_consecutivo));
                        if (Detalle != null)
                        {
                            this.txtContainers.Text = Detalle.DESC_DOCUMENTO;
                            this.SsDocumento = Detalle.ID_DOCUMENTO;
                        }
                    }
                        

                }

                UPDET.Update();

            }
        }

        protected void GrillaDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string estado = (string)DataBinder.Eval(e.Row.DataItem, "ESTADO");
                Button btnRW = (Button)e.Row.FindControl("IncreaseButton");

                if (estado.Equals("NO EXISTE") )
                {
                    btnRW.Enabled = false;
                    return;
                }

                if (estado.Equals("VENCIDO") || estado.Equals("MODIFICAR"))
                {
                    btnRW.Enabled = true;
                }
                else
                {
                    btnRW.Enabled = false;
                }
              
            }
        }

        #endregion


        #region "Gridview Colaborador"

        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
    
                string estado = DataBinder.Eval(e.Row.DataItem, "ESTADO2").ToString().Trim();
               

                if (estado.Equals("ACTIVO"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Black;
                }
                   

                if (estado.Equals("CADUCADO"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
                

                this.Actualiza_Paneles();

            }
        }

        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
         

            if (e.CommandName == "Actualizar")
            {

                codCab = e.CommandArgument.ToString();

                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                if (objDocumento != null)
                {
                    var Detalle = objDocumento.Colaborador.FirstOrDefault(f => f.NOMINA_COD == codCab);
                    if (Detalle != null)
                    {
                        this.txtID.Text = Detalle.COLABORADOR;
                        this.SsCodigo = Detalle.NOMINA_COD;
                        this.SsTipo = "COLABORADOR";
                        this.SsDescripcion = Detalle.COLABORADOR;

                        //cargar documentos del colaborar
                        GrillaDetalle.DataSource = null;
                        GrillaDetalle.DataBind();

                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        List<Cls_TRANSP_Doc_Colaborador> DocumentoColaborador = Cls_TRANSP_Doc_Colaborador.Carga_Documento_Colaborador(out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener documentos del colaborador...{0}-{1}</b>", this.txtID.Text, cMensajes));
                            this.Actualiza_Paneles();
                            return;
                        }

                        objDocumento.Documento_Colaborador.Clear();

                        foreach (var Det in DocumentoColaborador)
                        {
                            objDocumentoColaborador = new Cls_TRANSP_Doc_Colaborador();                          
                            objDocumentoColaborador.NOMINA_COD = Detalle.NOMINA_COD;
                            objDocumentoColaborador.COD_SOLICITUD = Det.COD_SOLICITUD;
                            objDocumentoColaborador.DESC_SOLICITUD = Det.DESC_SOLICITUD;
                            objDocumentoColaborador.TIPO_SOLCIITUD = Det.TIPO_SOLCIITUD;
                            objDocumentoColaborador.ID_DOCUMENTO = Det.ID_DOCUMENTO;
                            objDocumentoColaborador.COD_DOCUMENTO = Det.COD_DOCUMENTO;
                            objDocumentoColaborador.DESC_DOCUMENTO = Det.DESC_DOCUMENTO;
                            objDocumentoColaborador.EXT_DOCUMENTO = Det.EXT_DOCUMENTO;
                            objDocumentoColaborador.RUTA = "";

                            //Copia a color de Licencia de Conducir
                            if (Det.COD_DOCUMENTO.Equals("TR2"))
                            {
                                List<Cls_TRANSP_Validaciones> Licencia = Cls_TRANSP_Validaciones.Valida_Licencia(Detalle.NOMINA_COD, Detalle.RUC_EMPRESA, out cMensajes);
                                if (!String.IsNullOrEmpty(cMensajes))
                                {

                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error al validar licencia...{0}-{1}</b>", this.txtID.Text, cMensajes));
                                    this.Actualiza_Paneles();
                                    return;
                                }

                                var xLic = Licencia.FirstOrDefault();
                                if (xLic != null)
                                {
                                    objDocumentoColaborador.FECHA_CADUCA = xLic.EXPIRACION_LICENCIA;
                                    objDocumentoColaborador.ESTADO = xLic.STATUS;
                                }
                                else
                                {
                                    objDocumentoColaborador.ESTADO ="NO EXISTE";
                                }
                            }

                            //Detalle de Comprobante de Pago, Planilla IESS.
                            if (Det.COD_DOCUMENTO.Equals("TR1"))
                            {
                                objDocumentoColaborador.ESTADO = "MODIFICAR";
                            }
                            //Copia a color de Cedula de Identidad
                            if (Det.COD_DOCUMENTO.Equals("TR3"))
                            {
                                objDocumentoColaborador.ESTADO = "MODIFICAR";
                            }

                            objDocumento.Documento_Colaborador.Add(objDocumentoColaborador);
                        }

                        GrillaDetalle.DataSource = objDocumento.Documento_Colaborador;
                        GrillaDetalle.DataBind();

                        if (Detalle.ESTADO2.Equals("CADUCADO"))
                        {
                            this.BtnGenerar.Attributes["disabled"] = "disabled";
                        }
                        else
                        {
                            this.BtnGenerar.Attributes.Remove("disabled");
                        }

                        
                        this.Ocultar_Mensaje();
                    }
                        
                }

                UPDET.Update();

             
                
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
                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                if (objDocumento == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existen colaboradores de la empresa de transporte"));
                    return;
                }
                else
                {
                    tablePagination.PageIndex = e.NewPageIndex;
                    tablePagination.DataSource = objDocumento.Colaborador;
                    tablePagination.DataBind();
                    this.Actualiza_Paneles();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        #endregion

        #region "Gridview Vehiculos"
        protected void tableVehiculos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string estado = DataBinder.Eval(e.Row.DataItem, "ESTADO").ToString().Trim();


                if (estado.Equals("ACTIVO"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Black;
                }


                if (estado.Equals("CADUCADO"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }


                this.Actualiza_Paneles();

            }
        }

        protected void tableVehiculos_RowCommand(object source, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Actualizar")
            {

                codCab = e.CommandArgument.ToString();

                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                if (objDocumento != null)
                {
                    var Detalle = objDocumento.Vehiculo.FirstOrDefault(f => f.PLACA == codCab);
                    if (Detalle != null)
                    {
                        this.txtID.Text = Detalle.PLACA;
                        this.SsCodigo = Detalle.PLACA;
                        this.SsTipo = "VEHICULOS";
                        this.SsDescripcion = Detalle.PLACA;

                        //cargar documentos del vehiculo
                        GrillaDetalle.DataSource = null;
                        GrillaDetalle.DataBind();

                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        List<Cls_TRANSP_Doc_Vehiculo> DocumentoVehiculo = Cls_TRANSP_Doc_Vehiculo.Carga_Documento_Vehiculo(out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener documentos del colaborador...{0}-{1}</b>", this.txtID.Text, cMensajes));
                            this.Actualiza_Paneles();
                            return;
                        }

                        objDocumento.Documento_Vehiculo.Clear();

                        foreach (var Det in DocumentoVehiculo)
                        {
                            objDocumentosVehiculo = new Cls_TRANSP_Doc_Vehiculo();
                            objDocumentosVehiculo.PLACA = Detalle.PLACA;
                            objDocumentosVehiculo.COD_SOLICITUD = Det.COD_SOLICITUD;
                            objDocumentosVehiculo.DESC_SOLICITUD = Det.DESC_SOLICITUD;
                            objDocumentosVehiculo.TIPO_SOLCIITUD = Det.TIPO_SOLCIITUD;
                            objDocumentosVehiculo.ID_DOCUMENTO = Det.ID_DOCUMENTO;
                            objDocumentosVehiculo.COD_DOCUMENTO = Det.COD_DOCUMENTO;
                            objDocumentosVehiculo.DESC_DOCUMENTO = Det.DESC_DOCUMENTO;
                            objDocumentosVehiculo.EXT_DOCUMENTO = Det.EXT_DOCUMENTO;
                            objDocumentosVehiculo.RUTA = "";

                            //Póliza de Seguro de Responsabilidad Civil por daños a terceros, para vehículos de carga pesada por un monto mínimo de USD. $10.000 y para vehículos bananeros $ 4.000 dólares; con cobertura de servicio para el Terminal Marítimo Simón Bolívar de la ciudad de la Guayaquil.
                            if (Det.COD_DOCUMENTO.Equals("POL"))
                            {
                                List<Cls_TRANSP_Validaciones> Poliza = Cls_TRANSP_Validaciones.Valida_Poliza(Detalle.PLACA, Detalle.RUC_EMPRESA, out cMensajes);
                                if (!String.IsNullOrEmpty(cMensajes))
                                {

                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error al validar Póliza...{0}-{1}</b>", this.txtID.Text, cMensajes));
                                    this.Actualiza_Paneles();
                                    return;
                                }

                                var xPol = Poliza.FirstOrDefault();
                                if (xPol != null)
                                {
                                    objDocumentosVehiculo.FECHA_CADUCA = xPol.EXPIRACION_POLIZA;
                                    objDocumentosVehiculo.ESTADO = xPol.STATUS;
                                }
                                else
                                {
                                    objDocumentosVehiculo.ESTADO = "NO EXISTE";
                                }
                            }
                            //Copìa a colo del Certificado de Pesos y Medidas emitido por el MTOP.
                            if (Det.COD_DOCUMENTO.Equals("CDP"))
                            {
                                List<Cls_TRANSP_Validaciones> Mtop = Cls_TRANSP_Validaciones.Valida_Mtop(Detalle.PLACA, Detalle.RUC_EMPRESA, out cMensajes);
                                if (!String.IsNullOrEmpty(cMensajes))
                                {

                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error al validar Certificado de Pesos...{0}-{1}</b>", this.txtID.Text, cMensajes));
                                    this.Actualiza_Paneles();
                                    return;
                                }

                                var xMtop = Mtop.FirstOrDefault();
                                if (xMtop != null)
                                {
                                    objDocumentosVehiculo.FECHA_CADUCA = xMtop.EXPIRACION_MTOP;
                                    objDocumentosVehiculo.ESTADO = xMtop.STATUS;
                                }
                                else
                                {
                                    objDocumentosVehiculo.ESTADO = "NO EXISTE";
                                }
                            }
                            //Copia a color de la matrícula del vehículo
                            if (Det.COD_DOCUMENTO.Equals("TR4"))
                            {
                                objDocumentosVehiculo.ESTADO = "MODIFICAR";
                            }
                            //Copia a color de la Revisión técnica vehicular.
                            if (Det.COD_DOCUMENTO.Equals("TR5"))
                            {
                                objDocumentosVehiculo.ESTADO = "MODIFICAR";
                            }
                            objDocumento.Documento_Vehiculo.Add(objDocumentosVehiculo);
                        }

                        GrillaDetalle.DataSource = objDocumento.Documento_Vehiculo;
                        GrillaDetalle.DataBind();

                        this.BtnGenerar.Attributes.Remove("disabled");
                        this.Ocultar_Mensaje();
                    }

                }

                UPDET.Update();



            }
        }

        protected void tableVehiculos_PreRender(object sender, EventArgs e)
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

        protected void tableVehiculos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;

                if (objDocumento == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existen vehículos de la empresa de transporte"));
                    return;
                }
                else
                {
                    tableVehiculos.PageIndex = e.NewPageIndex;
                    tableVehiculos.DataSource = objDocumento.Vehiculo;
                    tableVehiculos.DataBind();
                    this.Actualiza_Paneles();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        #endregion

        #region "Grabar Solicitud"

        protected void BtnGenerar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objDocumento = Session["Documento" + this.hf_BrowserWindowName.Value] as Cls_TRANSP_Cab_Documentos;
                    if (objDocumento == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona o el vehículo a generar la solcitud. </b>"));
                        return;
                    }
                    else
                    {
                        if (this.SsTipo.Equals("COLABORADOR"))
                        {

                            if (objDocumento.Documento_Colaborador.Count == 0)
                            {
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el colaborador, para poder generar la solicitud.</b>"));
                                return;
                            }
                            else
                            {
                                //valida que tenga documentos cargados
                                var Existe = objDocumento.Documento_Colaborador.Where(f => !string.IsNullOrEmpty(f.RUTA_FINAL)).Count();
                                if (Existe == 0)
                                {
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe cargar los documentos del colaborador, para poder generar la solicitud.</b>"));
                                    return;
                                }
                            }

                            //proceso para grabar.
                            var x = objDocumento.Colaborador.Where(p => p.NOMINA_COD == this.SsCodigo).FirstOrDefault();

                            objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();
                            objDocumentoGrabar.RUC_EMPRESA = x.RUC_EMPRESA;
                            objDocumentoGrabar.NOMBRE_EMPRESA = x.NOMBRE_EMPRESA;
                            objDocumentoGrabar.NOMINA_COD = x.NOMINA_COD;
                            objDocumentoGrabar.COLABORADOR = x.COLABORADOR;
                            objDocumentoGrabar.NOMBRES = x.NOMBRES;
                            objDocumentoGrabar.ESTADO = x.ESTADO;
                            objDocumentoGrabar.ESTADO2 = x.ESTADO2;
                            objDocumentoGrabar.APELLIDOS = x.APELLIDOS;
                            objDocumentoGrabar.TIPOSANGRE = x.TIPOSANGRE;
                            objDocumentoGrabar.DIRECCIONDOM = x.DIRECCIONDOM;
                            objDocumentoGrabar.TELFDOM = x.TELFDOM;
                            objDocumentoGrabar.FECHANAC = x.FECHANAC;
                            objDocumentoGrabar.CARGO = x.CARGO;
                            objDocumentoGrabar.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objDocumentoGrabar.MAIL = ClsUsuario.email;

                            foreach (var Det in objDocumento.Documento_Colaborador.Where(y => !string.IsNullOrEmpty(y.RUTA_FINAL)))
                            {
                                objDocumentoColaborador = new Cls_TRANSP_Doc_Colaborador();
                                objDocumentoColaborador.ID_DOCUMENTO = Det.ID_DOCUMENTO;
                                objDocumentoColaborador.RUTA_FINAL = Det.RUTA_FINAL;
                                objDocumentoColaborador.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objDocumentoGrabar.Documento_Colaborador.Add(objDocumentoColaborador);
                            }

                            var nIdRegistro = objDocumentoGrabar.SaveTransaction_Colaborador(out cMensajes);
                            if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                            {
                                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar la solicitud de actualización de documentos del colaborador: {0}, error: {0}</b>",x.COLABORADOR, cMensajes));

                                return;
                            }
                            else
                            {
                                this.Ocultar_Mensaje();

                                this.Mostrar_Mensaje(string.Format("<b>Informativo! Se procedió a generar la solicitud # {0} de actualización de documentos del colaborador, con éxito</b>", nIdRegistro.Value));

                                this.BtnGenerar.Attributes["disabled"] = "disabled";
                                this.UPDET.Update();

                            }

                        }
                        if (this.SsTipo.Equals("VEHICULOS"))
                        {
                            if (objDocumento.Documento_Vehiculo.Count == 0)
                            {
                                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el vehículo, para poder generar la solicitud.</b>"));
                                return;
                            }
                            else
                            {
                                //valida que tenga documentos cargados
                                var Existe = objDocumento.Documento_Vehiculo.Where(f => !string.IsNullOrEmpty(f.RUTA_FINAL)).Count();
                                if (Existe == 0)
                                {
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe cargar los documentos del vehículo, para poder generar la solicitud.</b>"));
                                    return;
                                }
                            }



                            //proceso para grabar.
                            var x = objDocumento.Vehiculo.Where(p => p.PLACA == this.SsCodigo).FirstOrDefault();

                            objDocumentoGrabar = new Cls_TRANSP_Cab_Documentos();
                            objDocumentoGrabar.RUC_EMPRESA = x.RUC_EMPRESA;
                            objDocumentoGrabar.PLACA = x.PLACA;
                            objDocumentoGrabar.CLASETIPO = x.CLASETIPO;
                            objDocumentoGrabar.MARCA = x.MARCA;
                            objDocumentoGrabar.MODELO = x.MODELO;
                            objDocumentoGrabar.COLOR = x.COLOR;
                            objDocumentoGrabar.TIPOCERTIFICADO = x.TIPOCERTIFICADO;
                            objDocumentoGrabar.CERTIFICADO = x.CERTIFICADO;
                            objDocumentoGrabar.CATEGORIA = x.CATEGORIA;
                            objDocumentoGrabar.DESCRIPCIONCATEGORIA = x.DESCRIPCIONCATEGORIA;
                            objDocumentoGrabar.FECHAPOLIZA = x.FECHAPOLIZA;
                            objDocumentoGrabar.FECHAMTOP = x.FECHAMTOP;
                            objDocumentoGrabar.TIPO = x.TIPO;

                            objDocumentoGrabar.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objDocumentoGrabar.MAIL = ClsUsuario.email;

                            foreach (var Det in objDocumento.Documento_Vehiculo.Where(y => !string.IsNullOrEmpty(y.RUTA_FINAL)))
                            {
                                objDocumentosVehiculo = new Cls_TRANSP_Doc_Vehiculo();
                                objDocumentosVehiculo.ID_DOCUMENTO = Det.ID_DOCUMENTO;
                                objDocumentosVehiculo.RUTA_FINAL = Det.RUTA_FINAL;
                                objDocumentosVehiculo.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objDocumentoGrabar.Documento_Vehiculo.Add(objDocumentosVehiculo);
                            }

                            var nIdRegistro = objDocumentoGrabar.SaveTransaction_Vehiculo(out cMensajes);
                            if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                            {
                                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar la solicitud de actualización de documentos de Vehículos: {0}, error: {0}</b>", x.PLACA, cMensajes));

                                return;
                            }
                            else
                            {
                                this.Ocultar_Mensaje();

                                this.Mostrar_Mensaje(string.Format("<b>Informativo! Se procedió a generar la solicitud # {0} de actualización de documentos de Vehículos, con éxito</b>", nIdRegistro.Value));

                                this.BtnGenerar.Attributes["disabled"] = "disabled";
                                this.UPDET.Update();

                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGenerar_Click), "Generar Solicitud", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));


                }
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
            UPCOLABORADOR.Update();

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

                    //if (string.IsNullOrEmpty(this.TXTNAVE.Text))
                    //{
                    //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                    //    this.TXTNAVE.Focus();
                    //    return;
                    //}

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    //Carga de data
                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //resultado de entidad contenedor
                    var Contenedor = new N4.Exportacion.container();
                    var ListaContenedores = Contenedor.CargaporReferencia(ClsUsuario.loginname, "");

                 

                    //INFORMACION DE BOOKING YA PROCESADOS(ya almacenados en base de datos)
                    RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
                    var ListaBookingProcesado = _obj.consultaEntidad("", out _ERROR);

                    //INFORMACION DE VALIDACIONES DE CONTENEDORES YA PROCESADOS(ya almacenados en base de datos)
                    RN_Bill_InvoiceContainerExpo _objValidacion = new RN_Bill_InvoiceContainerExpo();
                    objValidacionesDet = _objValidacion.consultaSubDetalle(null,null, "", out _ERROR);
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
                       // TXTNAVE.Text = string.Empty;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se encontraron datos, revise el criterio de consulta"));
                        return;
                    }
                    this.Ocultar_Mensaje();
                    UPCOLABORADOR.Update();
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
                //if (txtFiltro.Text != string.Empty)
                //{
                
                //    if (rbBooking.Checked)
                //    {
                //        objCabceraOrdenada = (from A in objCabecera
                //                             where  A.CNTR_BKNG_BOOKING.ToUpper().Contains(txtFiltro.Text.ToUpper())
                //                              select A).ToList();
                //    }

                //    if (rbLinea.Checked)
                //    {
                //        objCabceraOrdenada = (from A in objCabecera
                //                              where A.CNTR_CLNT_CUSTOMER_LINE.ToUpper().Contains(txtFiltro.Text.ToUpper())
                //                              select A).ToList();
                //    }

                //    if (rbContenedor.Checked)
                //    {
                //        objCabceraOrdenada = (from A in objCabecera
                //                              where A.CNTR_CONTAINERS.ToUpper().Contains(txtFiltro.Text.ToUpper())
                //                              select A).ToList();
                //    }
                //}

                tablePagination.DataSource = objCabceraOrdenada;
                tablePagination.DataBind();

                this.Ocultar_Mensaje();
                UPCOLABORADOR.Update();

                GrillaDetalle.DataSource = null;
                GrillaDetalle.DataBind();
               // txtFiltro.Text = string.Empty;
                UPDET.Update();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        //protected void BtnActualizar_Click(object sender, EventArgs e)
        //{
        //    try {
        //        string COD_CABECERA = Session["CodigoCabecera" + this.hf_BrowserWindowName.Value] as string;

        //        if (COD_CABECERA == null) { return; }
        //        if (COD_CABECERA == "0") { return; }

        //        objCabecera = Session["TransaccionContDet"] as List<Cls_Bill_CabeceraExpo>;
        //        if (objCabecera.Where(p => p.CNTR_ID == long.Parse(COD_CABECERA)).FirstOrDefault().CNTR_ESTADO != "E") { return; }

        //        RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
        //        string v_error = _obj.actualizarStatus(long.Parse(COD_CABECERA), "N");

        //        if (v_error != string.Empty)
        //        {
        //            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", v_error));
        //        }
        //        else
        //        {
        //            this.Mostrar_Mensaje(string.Format("<b>Información! </b> Transacción exitosa"));

        //            objCabecera.Where(p => p.CNTR_ID == long.Parse(COD_CABECERA)).FirstOrDefault().CNTR_ESTADO = "N";
        //            objCabecera.Where(p => p.CNTR_ID == long.Parse(COD_CABECERA)).FirstOrDefault().ORDEN = 2;

        //            objCabecera = objCabecera.OrderBy(p => p.ORDEN).ToList();
        //            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
        //            Session["TransaccionContDet"] = objCabecera;
        //            tablePagination.DataSource = objCabecera;
        //            tablePagination.DataBind();
        //            UPCOLABORADOR.Update();
                   
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}

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
                UPCOLABORADOR.Update();

                GrillaDetalle.DataSource = null;
                GrillaDetalle.DataBind();
               // txtFiltro.Text = string.Empty;
                UPDET.Update();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }
    }
}