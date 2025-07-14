using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using ControlPagos.Importacion;
using Salesforces;
using System.Data;
using System.Net;
using SqlConexion;
using CasManual;

using System.Reflection;
using System.ComponentModel;

namespace CSLSite
{


    public partial class brbk_generar_turnos : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_CasManual objDetalleCas = new Cls_Bil_CasManual();
        private string cMensajes;

        private brbk_turnos_cab objCab = new brbk_turnos_cab();
        private brbk_turnos_det objDet = new brbk_turnos_det();

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
       
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
        private static Int64? lm = -3;
        private string OError;

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
           
            this.UPCARGA.Update();
            this.UPDATOSCLIENTE.Update();

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

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
           
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
          
        }

        private void Carga_CboBodega()
        {
            try
            {
                List<brbk_depositos> Listado = brbk_depositos.CboBodega(false, out cMensajes);

                this.CboBodega.DataSource = Listado;
                this.CboBodega.DataTextField = "DESCRIPCION";
                this.CboBodega.DataValueField = "CODIGO";
                this.CboBodega.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_generar_turnos", "Carga_CboBodega", "Hubo un error al cargar bodegas", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

        private void Carga_CboTipoProducto()
        {
            try
            {
                List<brbk_tipoproducto> Listado = brbk_tipoproducto.CboTipoProducto(false,out cMensajes);

                this.CboTipoProducto.DataSource = Listado;
                this.CboTipoProducto.DataTextField = "DESCRIPCION";
                this.CboTipoProducto.DataValueField = "CODIGO";
                this.CboTipoProducto.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_generar_turnos", "Carga_CboTipoProducto", "Hubo un error al cargar tipo productos", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

     
        

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCab = new brbk_turnos_cab();           
            Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;
            
        }

        #endregion



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        protected void Page_Init(object sender, EventArgs e)
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

            this.IsAllowAccess();

            if (!Page.IsPostBack)
            {

                this.banmsg.InnerText = string.Empty;

                ClsUsuario = Page.Tracker();
                
            }

        }

       

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

               
                Server.HtmlEncode(this.TxtCapacidad.Text.Trim());
                Server.HtmlEncode(this.TxtFrecuencia.Text.Trim());
                Server.HtmlEncode(this.TxtFechaDesde.Text.Trim());
                Server.HtmlEncode(this.TxtFechaHasta.Text.Trim());

               
                if (!Page.IsPostBack)
                {     
                   this.Crear_Sesion();

                    this.Carga_CboBodega();

                    this.Carga_CboTipoProducto();

                }
                else
                {
                  

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

 
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            
            if (Response.IsClientConnected)
            {
                try
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime Fecha_Desde;
                    DateTime Fecha_Hasta;

                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFechaDesde.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la fecha de inicio del turno"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la fecha de finalización del turno"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (!DateTime.TryParseExact(this.TxtFechaDesde.Text.Trim(), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out Fecha_Desde))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de inicio del turno</b>"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }

                    if (!DateTime.TryParseExact(this.TxtFechaHasta.Text.Trim(), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out Fecha_Hasta))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de finalización del turno</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (this.CboTipoProducto.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar el tipo de producto"));
                        this.CboTipoProducto.Focus();
                        return;
                    }

                    if (this.CboBodega.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar la bodega"));
                        this.CboBodega.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtCapacidad.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la capacidad para cada turno"));
                        this.TxtCapacidad.Focus();
                        return;
                    }

                    Int64 Capacidad = 0;
                    if (!Int64.TryParse(this.TxtCapacidad.Text, out Capacidad))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la capacidad para cada turno"));
                        this.TxtCapacidad.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFrecuencia.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la frecuencia en minutos para cada turno"));
                        this.TxtFrecuencia.Focus();
                        return;
                    }

                    Int64 Frecuencia = 0;
                    if (!Int64.TryParse(this.TxtFrecuencia.Text, out Frecuencia))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la frecuencia en minutos para cada turno"));
                        this.TxtFrecuencia.Focus();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objCab = Session["Turnos" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    //valida turnos
                    objCab.FECHA_DESDE = Fecha_Desde;
                    objCab.FECHA_HASTA = Fecha_Hasta;
                    objCab.ID_TIPO_PRODUCTO = Int64.Parse(this.CboTipoProducto.SelectedValue.ToString());
                    objCab.ID_BODEGA = this.CboBodega.SelectedValue.ToString();
                    if (objCab.Valida_Existe_Turnos(out OError))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No se puede generar turno: {0}", OError));
                        this.TxtFrecuencia.Focus();
                        return;
                    }


                  

                    List<brbk_turnos_det> Listado = brbk_turnos_det.Generar_Turnos(Fecha_Desde, Fecha_Hasta, Int64.Parse(this.CboTipoProducto.SelectedValue.ToString()),
                            this.CboBodega.SelectedValue.ToString(), Frecuencia,Capacidad, out cMensajes);


                   
                    objCab.FECHA_DESDE = Fecha_Desde;
                    objCab.FECHA_HASTA = Fecha_Hasta;
                    objCab.ID_TIPO_PRODUCTO = Int64.Parse(this.CboTipoProducto.SelectedValue.ToString());
                    
                    var Existe = CboTipoProducto.Items.FindByValue(objCab.ID_TIPO_PRODUCTO.ToString());
                    if (Existe != null)
                    {
                        objCab.DESC_TIPO_PRODUCTO = Existe.Text;
                    }

                    objCab.ID_BODEGA = this.CboBodega.SelectedValue.ToString();

                    var ExisteBod = CboBodega.Items.FindByValue(objCab.ID_BODEGA.ToString());
                    if (ExisteBod != null)
                    {
                        objCab.DESC_BODEGA = ExisteBod.Text;
                    }

                    objCab.FRECUENCIA = Frecuencia;
                    objCab.CAPACIDAD = Capacidad;
                    objCab.CONCEPTO = this.Txtcomentario.Text;
                    objCab.USUARIO_CREA = ClsUsuario.loginname.Trim();

                    objCab.Detalle.Clear();

                    int i = 0;
                    foreach (var Det in Listado)
                    {
                        objDet = new brbk_turnos_det();
                        objDet.SECUENCIA = Det.SECUENCIA;
                        objDet.FECHA_TURNO = Det.FECHA_TURNO;
                        objDet.ID_TIPO_PRODUCTO = Det.ID_TIPO_PRODUCTO;
                        objDet.ID_BODEGA = Det.ID_BODEGA;
                        objDet.CAPACIDAD = Det.CAPACIDAD;
                        objDet.FRECUENCIA = Det.FRECUENCIA;
                        objDet.ESTADO = true;
                        objDet.DESC_TIPO_PRODUCTO = Det.DESC_TIPO_PRODUCTO;
                        objDet.DESC_BODEGA = Det.DESC_BODEGA;
                        objDet.USUARIO_CREA = ClsUsuario.loginname;
                        objDet.FINSEMANA = Det.FINSEMANA;
                        objCab.Detalle.Add(objDet);
                        i++;
                    }

                    if (i > 0)
                    {
                        tablePagination.DataSource = objCab.Detalle;
                        tablePagination.DataBind();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen turnos que mostrar..{0}", cMensajes));
                        return;
                    }

                   

                    Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;

                    this.Actualiza_Paneles();

                    this.Ocultar_Mensaje();

                }
                catch (Exception ex)
                {
                 
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "BtnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }    
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    this.Ocultar_Mensaje();
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Eliminar")
                    {
                        objCab = Session["Turnos" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                        Int64 SECUENCIA = Int64.Parse(t.ToString());

                        //existe pase a remover
                        var Detalle = objCab.Detalle.FirstOrDefault(f => f.SECUENCIA.Equals(SECUENCIA));
                        if (Detalle != null)
                        {
                           
                            //remover pase
                            objCab.Detalle.Remove(objCab.Detalle.Where(p => p.SECUENCIA == SECUENCIA).FirstOrDefault());

                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información del turno a eliminar: {0} </b>", t.ToString()));
                            return;
                        }

                        tablePagination.DataSource = objCab.Detalle;
                        tablePagination.DataBind();


                      

                        Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;


                        this.Actualiza_Paneles();

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                    

                }
            }

        }


        protected void TxtNewCantidad_TextChanged(object sender, EventArgs e)
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

                    TextBox TxtCapacidad = ((TextBox)(sender));
                    //TxtCapacidad.ForeColor = System.Drawing.Color.Blue;

                    RepeaterItem RpDatos = ((RepeaterItem)(TxtCapacidad.NamingContainer));
                   
                    Label SECUENCIA = (Label)RpDatos.FindControl("SECUENCIA");
                    //SECUENCIA.ForeColor = System.Drawing.Color.Blue;

                    if (string.IsNullOrEmpty(SECUENCIA.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos para actualizar capacidad de turnos"));
                        return;
                    }

                    Int64 nSECUENCIA = Int64.Parse(SECUENCIA.Text);

                    objCab = Session["Turnos" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    //existe pase a remover
                    var Detalle = objCab.Detalle.FirstOrDefault(f => f.SECUENCIA.Equals(nSECUENCIA));
                    if (Detalle != null)
                    {
                        Int64 CAPACIDAD = 0;

                        if (Int64.TryParse(TxtCapacidad.Text, out CAPACIDAD))
                        {
                            Detalle.CAPACIDAD = CAPACIDAD;
                        }
                        
                        //remover pase
                      
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información del turno, para actualizar capacitdad del mismo: {0} </b>", SECUENCIA.Text));
                        return;
                    }

                    tablePagination.DataSource = objCab.Detalle;
                    tablePagination.DataBind();


                    Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;


                    this.Actualiza_Paneles();


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(TxtNewCantidad_TextChanged), "TxtNewCantidad_TextChanged", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));



                }
            }

          

        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {

            try
            {
                this.BtnGrabar.Attributes.Remove("disabled");

           

                this.TxtFechaDesde.Text = string.Empty;
                this.TxtFechaHasta.Text = string.Empty;
                this.CboTipoProducto.SelectedIndex = -1;
                this.CboBodega.SelectedIndex = -1;
                this.TxtCapacidad.Text = string.Empty;
                this.TxtFrecuencia.Text = string.Empty;
                this.Txtcomentario.Text = string.Empty;

                this.banmsg.InnerText = string.Empty;
                this.banmsg.Visible = false;

                objCab = Session["Turnos" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;
                objCab.Detalle.Clear();

                tablePagination.DataSource = null;
                tablePagination.DataBind();

                Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;

                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "brbk_generar_turnos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }

        protected void BtnExcel_Click(object sender, EventArgs e)
        {

            try
            {
                objCab = Session["Turnos" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                List<brbk_turnos_det> lst = objCab.Detalle;
                DataTable tb = ConvertToDataTable(lst);

                if (tb.Rows.Count > 0)
                {
                    string fname = string.Format("TURNOS{0}", DateTime.Now.ToString("ddMMyyyHHmmss"));

                    Session["turnos"] = tb;

                    string llamada = string.Format("'descarga(\"{0}\",\"{1}\",\"{2}\");'", fname, "turnos", "turnos");
                    banmsg.InnerHtml = string.Format("Se ha generado el archivo {0}.xlsx, con {1} filas<br/><a class='btn btn-link' href='#' onclick={2} >Clic Aquí para descargarlo</a>", fname, tb.Rows.Count, llamada);
                    banmsg.Visible = true;
                }
                else
                {
                   
                    banmsg.InnerHtml = "No existen registros para exportar";
                    banmsg.Visible = true;
                }


                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnExcel_Click), "brbk_generar_turnos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();

            try
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
               
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;

            }
            catch (Exception)
            {
                throw;
            }

        }



        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdDesConsolidadora = string.Empty;

                    this.BtnGrabar.Attributes["disabled"] = "disabled";
                    this.Actualiza_Paneles();

                    objCab = Session["Turnos" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    if (objCab == null)
                    {
                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar los turnos, para poder poder grabar los mismos.. </b>"));
                        return;
                    }

                    if (objCab.Detalle.Count == 0)
                    {
                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen turnos generados, debe generar los mismos...</b>"));
                        return;
                    }

                    System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("TURNOS", from p in objCab.Detalle.AsEnumerable().AsParallel()
                                                                     select new System.Xml.Linq.XElement("DETALLE",
                                                                     new System.Xml.Linq.XAttribute("SECUENCIA", p.SECUENCIA ),
                                                                     new System.Xml.Linq.XAttribute("FECHA_TURNO", p.FECHA_TURNO),
                                                                     new System.Xml.Linq.XAttribute("ID_TIPO_PRODUCTO", p.ID_TIPO_PRODUCTO),
                                                                     new System.Xml.Linq.XAttribute("ID_BODEGA", p.ID_BODEGA ),
                                                                     new System.Xml.Linq.XAttribute("CAPACIDAD", p.CAPACIDAD),
                                                                     new System.Xml.Linq.XAttribute("FRECUENCIA", p.FRECUENCIA),
                                                                     new System.Xml.Linq.XAttribute("flag", "I"))));

                    objCab.xmlTurnos = XMLTurnos.ToString();

                    var nProceso = objCab.SaveTransaction(out cMensajes);
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {
                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo grabar los turnos...{0}</b>", cMensajes));
                        return;
                    }
                    else
                    {
                        this.TxtFechaDesde.Text = string.Empty;
                        this.TxtFechaHasta.Text = string.Empty;
                        this.CboTipoProducto.SelectedIndex = -1;
                        this.CboBodega.SelectedIndex = -1;
                        this.TxtCapacidad.Text = string.Empty;
                        this.TxtFrecuencia.Text = string.Empty;
                        this.Txtcomentario.Text = string.Empty;

                        objCab.Detalle.Clear();
                        Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;

                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;

                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(1, string.Format("<b>OK! se generaron turnos con éxito....</b>"));

                        this.Actualiza_Paneles();

                    }

                
                    OcultarLoading("1");
                    OcultarLoading("2");


                }


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "brbk_generar_turnos.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
   }
}