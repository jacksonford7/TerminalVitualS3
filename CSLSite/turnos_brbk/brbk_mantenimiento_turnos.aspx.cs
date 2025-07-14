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


    public partial class brbk_mantenimiento_turnos : System.Web.UI.Page
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

        private void Mostrar_Mensaje(string Mensaje)
        {
           
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");
        
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
                List<brbk_depositos> Listado = brbk_depositos.CboBodega(true, out cMensajes);

                this.CboBodega.DataSource = Listado;
                this.CboBodega.DataTextField = "DESCRIPCION";
                this.CboBodega.DataValueField = "CODIGO";
                this.CboBodega.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_mantenimiento_turnos", "Carga_CboBodega", "Hubo un error al cargar bodegas", t.loginname));
                this.Mostrar_Mensaje(Error);
            }
        }

        private void Carga_CboTipoProducto()
        {
            try
            {
                List<brbk_tipoproducto> Listado = brbk_tipoproducto.CboTipoProducto(true, out cMensajes);

                this.CboTipoProducto.DataSource = Listado;
                this.CboTipoProducto.DataTextField = "DESCRIPCION";
                this.CboTipoProducto.DataValueField = "CODIGO";
                this.CboTipoProducto.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_mantenimiento_turnos", "Carga_CboTipoProducto", "Hubo un error al cargar tipo productos", t.loginname));
                this.Mostrar_Mensaje( Error);
            }
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
            objCab = new brbk_turnos_cab();           
            Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;
            
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

            this.IsAllowAccess();

            this.banmsg.Visible = IsPostBack;

         

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
                //Server.HtmlEncode(this.TxtFrecuencia.Text.Trim());
                Server.HtmlEncode(this.TxtFechaDesde.Text.Trim());
               // Server.HtmlEncode(this.TxtFechaHasta.Text.Trim());

               
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
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
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
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la fecha del turno"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }

                   

                    if (!DateTime.TryParseExact(this.TxtFechaDesde.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.AdjustToUniversal, out Fecha_Desde))
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida  del turno</b>"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }


                    if (this.CboTipoProducto.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar el tipo de producto"));
                        this.CboTipoProducto.Focus();
                        return;
                    }

                    if (this.CboBodega.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar la bodega"));
                        this.CboBodega.Focus();
                        return;
                    }



                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                  

                    List<brbk_turnos_det> Listado = brbk_turnos_det.Detalle_Turnos_Capacidad(Fecha_Desde, Int64.Parse(this.CboTipoProducto.SelectedValue.ToString()),
                            this.CboBodega.SelectedValue.ToString(),  out cMensajes);


                   
                    objCab.FECHA_DESDE = Fecha_Desde;
                    objCab.FECHA_HASTA = Fecha_Desde;
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

                    objCab.FRECUENCIA = 0;
                    objCab.CAPACIDAD = 0;
                    objCab.CONCEPTO = this.Txtcomentario.Text;
                    objCab.USUARIO_CREA = ClsUsuario.loginname.Trim();

                    objCab.Detalle.Clear();

                    int i = 0;
                    foreach (var Det in Listado)
                    {
                        objDet = new brbk_turnos_det();
                        objDet.ID_TURNO = Det.ID_TURNO;
                        objDet.SECUENCIA = Det.SECUENCIA;
                        objDet.FECHA_TURNO = Det.FECHA_TURNO;
                        objDet.ID_TIPO_PRODUCTO = Det.ID_TIPO_PRODUCTO;
                        objDet.ID_BODEGA = Det.ID_BODEGA;
                        objDet.CAPACIDAD = Det.CAPACIDAD;
                        objDet.FRECUENCIA = Det.FRECUENCIA;
                        objDet.ESTADO = Det.ESTADO;
                        objDet.DESC_TIPO_PRODUCTO = Det.DESC_TIPO_PRODUCTO;
                        objDet.DESC_BODEGA = Det.DESC_BODEGA;
                        objDet.USUARIO_CREA = ClsUsuario.loginname;
                        objDet.FINSEMANA = Det.FINSEMANA;
                        objDet.IDX_ROW = Det.IDX_ROW;
                        objDet.PASES = Det.PASES;
                        objDet.USADOS = Det.USADOS;
                        objDet.SALDO = Det.SALDO;
                        objDet.CHECK = false;
                        objDet.NUEVA_CAPACIDAD = 0;
                        objDet.SALDO_CAPACIDAD = Det.SALDO;
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

                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen turnos que mostrar..{0}", cMensajes));
                        return;
                    }

                   

                    Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;

                    this.Actualiza_Paneles();

                    this.Ocultar_Mensaje();

                }
                catch (Exception ex)
                {
                 
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "BtnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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
                        this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Eliminar")
                    {
                        objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

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
                            this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo encontrar información del turno a eliminar: {0} </b>", t.ToString()));
                            return;
                        }

                        tablePagination.DataSource = objCab.Detalle;
                        tablePagination.DataBind();


                      

                        Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;


                        this.Actualiza_Paneles();

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                    

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

                    objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    if (objCab == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder poder actualizar la capacidad de los turnos...</b>"));
                        return;
                    }

                    if (objCab.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen turnos generados, debe generar los mismos...</b>"));
                        return;
                    }

                    TextBox TxtCapacidad = ((TextBox)(sender));
                    //TxtCapacidad.ForeColor = System.Drawing.Color.Blue;

                    RepeaterItem RpDatos = ((RepeaterItem)(TxtCapacidad.NamingContainer));
                   
                    Label IDX_ROW = (Label)RpDatos.FindControl("IDX_ROW");
                    //SECUENCIA.ForeColor = System.Drawing.Color.Blue;

                    if (string.IsNullOrEmpty(IDX_ROW.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos para actualizar capacidad de turnos"));
                        return;
                    }

                    Int64 nIDX_ROW = Int64.Parse(IDX_ROW.Text);

                    int CAPACIDAD = 0;

                    if (!int.TryParse(TxtCapacidad.Text, out CAPACIDAD))
                    {
                        CAPACIDAD = 0;
                    }

                    if (CAPACIDAD < 0)
                    {
                        CAPACIDAD = Math.Abs(CAPACIDAD);

                        //recorro para validar
                        foreach (var Det in objCab.Detalle.Where(p => p.IDX_ROW == nIDX_ROW))
                        {
                            var _SALDO = (Det.SALDO - CAPACIDAD);
                            if (_SALDO < 0)
                            {
                                objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.NUEVA_CAPACIDAD = (Det.SALDO * -1));
                               // objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.SALDO_CAPACIDAD = (int.Parse(f.CAPACIDAD.ToString()) + f.NUEVA_CAPACIDAD));
                            }
                            else
                            {
                                objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.NUEVA_CAPACIDAD = (CAPACIDAD * -1));
                                //objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.SALDO_CAPACIDAD = (int.Parse(f.CAPACIDAD.ToString()) + f.NUEVA_CAPACIDAD));
                            }

                            objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.SALDO_CAPACIDAD = (int.Parse(f.CAPACIDAD.ToString()) + f.NUEVA_CAPACIDAD));
                        }

                    }
                    else
                    {
                        objCab.Detalle.Where(w => w.IDX_ROW == nIDX_ROW).ToList().ForEach(f => f.NUEVA_CAPACIDAD = CAPACIDAD);
                        objCab.Detalle.Where(w => w.IDX_ROW == nIDX_ROW).ToList().ForEach(f => f.SALDO_CAPACIDAD = (int.Parse(f.CAPACIDAD.ToString()) + f.NUEVA_CAPACIDAD));

                    }


                    tablePagination.DataSource = objCab.Detalle;
                    tablePagination.DataBind();


                    Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;


                    this.Actualiza_Paneles();


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(TxtNewCantidad_TextChanged), "TxtNewCantidad_TextChanged", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));



                }
            }

          

        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {

            try
            {
                this.BtnGrabar.Attributes.Remove("disabled");

           

                this.TxtFechaDesde.Text = string.Empty;
   
                this.CboTipoProducto.SelectedIndex = 0;
                this.CboBodega.SelectedIndex = 0;
                this.TxtCapacidad.Text = string.Empty;
                this.ChkTodos.Checked = false;

                this.Txtcomentario.Text = string.Empty;

                this.banmsg.InnerText = string.Empty;
                this.banmsg.Visible = false;

                objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;
                objCab.Detalle.Clear();

                tablePagination.DataSource = null;
                tablePagination.DataBind();

                Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;

                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "brbk_mantenimiento_turnos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }

        protected void BtnExcel_Click(object sender, EventArgs e)
        {

            try
            {
                objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnExcel_Click), "brbk_mantenimiento_turnos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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

                    objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    if (objCab == null)
                    {
                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar los turnos, para poder poder actualizar la capacidad de los mismos.. </b>"));
                        return;
                    }

                    if (objCab.Detalle.Count == 0)
                    {
                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen turnos generados, debe generar los mismos...</b>"));
                        return;
                    }

                    var Cantidad = objCab.Detalle.Where(p => p.CHECK == true).Count();

                    if (Cantidad == 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los turnos, para poder poder actualizar la capacidad de los mismos...</b>"));
                        return;

                    }


                    System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("TURNOS", from p in objCab.Detalle.Where(p => p.CHECK==true).AsEnumerable().AsParallel()
                                                                     select new System.Xml.Linq.XElement("DETALLE",
                                                                     new System.Xml.Linq.XAttribute("IDX_ROW", p.IDX_ROW),
                                                                     new System.Xml.Linq.XAttribute("FECHA_TURNO", p.FECHA_TURNO),
                                                                     new System.Xml.Linq.XAttribute("ID_TIPO_PRODUCTO", p.ID_TIPO_PRODUCTO),
                                                                     new System.Xml.Linq.XAttribute("ID_BODEGA", p.ID_BODEGA ),
                                                                     new System.Xml.Linq.XAttribute("CAPACIDAD", p.SALDO_CAPACIDAD),
                                                                     new System.Xml.Linq.XAttribute("ID_TURNO", p.ID_TURNO),
                                                                     new System.Xml.Linq.XAttribute("flag", "I"))));

                    objCab.xmlTurnos = XMLTurnos.ToString();

                    var nProceso = objCab.SaveTransaction_Update_Mante(out cMensajes);
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {
                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo actualizar los turnos...{0}</b>", cMensajes));
                        return;
                    }
                    else
                    {
                        this.TxtFechaDesde.Text = string.Empty;
                     
                        this.CboTipoProducto.SelectedIndex = -1;
                        this.CboBodega.SelectedIndex = -1;
                        this.TxtCapacidad.Text = string.Empty;
                        this.Txtcomentario.Text = string.Empty;

                        objCab.Detalle.Clear();
                        Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;

                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;

                        this.BtnGrabar.Attributes.Remove("disabled");

                        this.Mostrar_Mensaje(string.Format("<b>OK! se actualizaron capacidad de turnos con éxito....</b>"));

                        this.Actualiza_Paneles();

                    }

                
                    OcultarLoading("1");
                    OcultarLoading("2");


                }


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "brbk_mantenimiento_turnos.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }


        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
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

                    objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    if (objCab == null)
                    {
                        this.ChkTodos.Checked = false;

                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder poder seleccionar los turnos.. </b>"));
                        return;
                    }

                    if (objCab.Detalle.Count == 0)
                    {
                        this.ChkTodos.Checked = false;

                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen turnos generados, debe generar los mismos...</b>"));
                        return;
                    }

                    objCab.Detalle.Where(w => w.IDX_ROW > 0).ToList().ForEach(f => f.CHECK = this.ChkTodos.Checked);

                    Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;

                    tablePagination.DataSource = objCab.Detalle;
                    tablePagination.DataBind();

                    this.Actualiza_Paneles();

                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ChkTodos_CheckedChanged), "brbk_mantenimiento_turnos.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void BtnActualizar_Click(object sender, EventArgs e)
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

                    objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    if (objCab == null)
                    {     
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder poder actualizar la capacidad de los turnos...</b>"));
                        return;
                    }

                    if (objCab.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen turnos generados, debe generar los mismos...</b>"));
                        return;
                    }

                    var Cantidad = objCab.Detalle.Where(p => p.CHECK == true).Count();

                    if (Cantidad == 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los turnos a modificar la capacidad...</b>"));
                        return;

                    }

                    int Capacidad = 0;
                    if (!int.TryParse(this.TxtCapacidad.Text, out Capacidad))
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la capacidad"));
                        this.TxtCapacidad.Focus();
                        return;
                    }

                    if (Capacidad == 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! La capacidad a modificar no puede ser cero...</b>"));
                        return;

                    }

                    if (Capacidad < 0)
                    {
                        Capacidad = Math.Abs(Capacidad);

                        //recorro para validar
                        foreach (var Det in objCab.Detalle.Where(p => p.CHECK == true))
                        {
                            var _SALDO = (Det.SALDO - Capacidad);
                            if (_SALDO < 0)
                            {
                                objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.NUEVA_CAPACIDAD = (Det.SALDO * -1));
                                objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.SALDO_CAPACIDAD = 0);
                            }
                            else
                            {
                                objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.NUEVA_CAPACIDAD = (Capacidad * -1));
                                objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.SALDO_CAPACIDAD = _SALDO);
                            }

                            //objCab.Detalle.Where(w => w.IDX_ROW == Det.IDX_ROW).ToList().ForEach(f => f.SALDO_CAPACIDAD = (int.Parse(f.CAPACIDAD.ToString()) + f.NUEVA_CAPACIDAD));
                        }

                    }
                    else
                    {
                        objCab.Detalle.Where(w => w.CHECK == true).ToList().ForEach(f => f.NUEVA_CAPACIDAD = Capacidad);

                        objCab.Detalle.Where(w => w.CHECK == true).ToList().ForEach(f => f.SALDO_CAPACIDAD = (int.Parse(f.CAPACIDAD.ToString()) + f.NUEVA_CAPACIDAD));

                    }
                  
                    //objCab.Detalle.Where(w => w.CHECK == true).ToList().ForEach(f => f.NUEVA_CAPACIDAD = Capacidad);

                    Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;

                    tablePagination.DataSource = objCab.Detalle;
                    tablePagination.DataBind();

                    this.banmsg.InnerText = string.Empty;
                    this.banmsg.Visible = false;

                    this.Actualiza_Paneles();

                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnActualizar_Click), "brbk_mantenimiento_turnos.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void CHKMARCAR_CheckedChanged(object sender, EventArgs e)
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

                    CheckBox CHKMARCAR = ((CheckBox)(sender));


                    RepeaterItem RpDatos = ((RepeaterItem)(CHKMARCAR.NamingContainer));

                    Label IDX_ROW = (Label)RpDatos.FindControl("IDX_ROW");

                    if (string.IsNullOrEmpty(IDX_ROW.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos para actualizar capacidad de turnos"));
                        return;
                    }

                    Int64 nIDX_ROW = Int64.Parse(IDX_ROW.Text);

                    objCab = Session["UpdTurno" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;

                    if (objCab == null)
                    {
                        this.ChkTodos.Checked = false;

                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder poder seleccionar los turnos.. </b>"));
                        return;
                    }

                   
                    objCab.Detalle.Where(w => w.IDX_ROW == nIDX_ROW).ToList().ForEach(f => f.CHECK = CHKMARCAR.Checked);

                    Session["UpdTurno" + this.hf_BrowserWindowName.Value] = objCab;

                    tablePagination.DataSource = objCab.Detalle;
                    tablePagination.DataBind();

                    this.Actualiza_Paneles();

                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(CHKMARCAR_CheckedChanged), "brbk_mantenimiento_turnos.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

    }
}