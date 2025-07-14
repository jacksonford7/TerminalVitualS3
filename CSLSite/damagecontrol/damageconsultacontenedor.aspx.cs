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
using System.IO;
using System.IO.Compression;

namespace CSLSite
{


    public partial class damageconsultacontenedor : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
       
        private string cMensajes;

        private Damage_Descuentos_Cab objCab = new Damage_Descuentos_Cab();
        private Damage_Descuentos_Det objDet = new Damage_Descuentos_Det();
        private Damage_Contenedor_Cliente objContainer = new Damage_Contenedor_Cliente();
        private Damage_Detalle_Contenedor objDetalleContainer = new Damage_Detalle_Contenedor();
        #endregion

        #region "Variables"

        //private string numero_carga = string.Empty;
        //private DateTime fechadesde = new DateTime();
        //private DateTime fechahasta = new DateTime();

        private string LoginName = string.Empty;
        

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

            this.UPCONTENEDOR.Update();
            this.UPMENSAJE.Update();
            this.UPINFOCONTENEDOR.Update();
           
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
                OcultarLoading("2");

            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje_Pendiente(string Mensaje)
        {

          
         
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;

          
            this.Actualiza_Paneles();

        }

        private void Limpiar_Campos() 
        {
            this.TxtSize.Text =string.Empty;
            this.TxtReferencia.Text = string.Empty;
            this.TxtTipoIso.Text = string.Empty;
            this.TxtFechaCas.Text = string.Empty;
            this.TxtLinea.Text = string.Empty;
            this.TxtCategoria.Text = string.Empty;
            this.TxtFreightKind.Text = string.Empty;

            this.TxtPeso.Text = string.Empty;
            this.TxtDocumento.Text = string.Empty;


            rptImagenes.DataSource = null;
            rptImagenes.DataBind();


        }



        private void Crear_Sesion()
        {
            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCab = new Damage_Descuentos_Cab();           
            Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCab;
            
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

#if !DEBUG
              //  this.IsAllowAccess();
#endif

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
                Server.HtmlEncode(this.TxtContenedor.Text.Trim());
             

                if (!Page.IsPostBack)
                {
                    

                    this.Crear_Sesion();

                   

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

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        OcultarLoading("2");
                        return;
                    }

                    this.acepta_servicio_aceptado.Visible = false;
                    this.texto_servicio_aceptado.InnerHtml = string.Empty;
                    this.BtnDescargar.Visible = false;
                    this.LinkDescargarZip.Visible = false;

                    this.UPACEPTA.Update();

                    CultureInfo enUS = new CultureInfo("en-US");

                    if (string.IsNullOrEmpty(TxtContenedor.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe ingresa el contenedor a buscar..."));
                        this.TxtContenedor.Focus();
                        return;

                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    var Tabla = Damage_Consulta_Contenedor.Consulta_Contenedor(TxtContenedor.Text.Trim(), ClsUsuario.ruc,  out cMensajes);
                    if (Tabla == null)
                    {
                        this.Limpiar_Campos();

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con el número de contenedor ingresado. {0} - {1}", TxtContenedor.Text.Trim(),cMensajes));
                        return;
                    }

                    if (Tabla.Count <= 0)
                    {
                        this.Limpiar_Campos();

                      
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con el número de contenedor ingresado.. {0}", TxtContenedor.Text.Trim()));
                        return;
                    }

                    //linq consulta
                    var LinResultado = Tabla.FirstOrDefault();
                    if (LinResultado != null) 
                    {
                        this.btnAccordion1.Text = LinResultado.CNTR_CONTAINER;
                      
                        this.TxtSize.Text = string.IsNullOrEmpty(LinResultado.CNTR_TYSZ_SIZE) ? "..." : LinResultado.CNTR_TYSZ_SIZE;
                        this.TxtReferencia.Text = string.IsNullOrEmpty(LinResultado.CNTR_VEPR_REFERENCE) ? "..." : LinResultado.CNTR_VEPR_REFERENCE;
                        this.TxtTipoIso.Text = string.IsNullOrEmpty(LinResultado.CNTR_TYSZ_ISO) ? "..." : LinResultado.CNTR_TYSZ_ISO;
                        this.TxtFechaCas.Text = LinResultado.FECHA_CAS.HasValue ? LinResultado.FECHA_CAS.Value.ToString("dd/MM/yyyy") : "...";
                        this.TxtLinea.Text = string.IsNullOrEmpty(LinResultado.CNTR_CLNT_CUSTOMER_LINE) ? "..." : LinResultado.CNTR_CLNT_CUSTOMER_LINE;
                        this.TxtCategoria.Text = string.IsNullOrEmpty(LinResultado.CNTR_TYPE) ? "..." : LinResultado.CNTR_TYPE;
                        this.TxtFreightKind.Text = string.IsNullOrEmpty(LinResultado.CNTR_LCL_FCL) ? "..." : LinResultado.CNTR_LCL_FCL;

                        this.TxtPeso.Text = LinResultado.CNTR_TARE.HasValue ? LinResultado.CNTR_TARE.Value.ToString() : "...";
                        this.TxtDocumento.Text = string.IsNullOrEmpty(LinResultado.CNTR_DOCUMENT) ? "..." : LinResultado.CNTR_DOCUMENT;

                        if (LinResultado.TIENE_SERVICIO)
                        {
                            this.acepta_servicio.Visible = false;
                            this.texto_servicio.InnerHtml = string.Empty;

                            this.BtnDescargar.Visible = true;
                            this.LinkDescargarZip.Visible = false;

                            this.UPACEPTA.Update();
                        }
                        else 
                        {
                            this.acepta_servicio.Visible = true;
                            this.texto_servicio.InnerHtml = LinResultado.MENSAJE;
                            this.BtnDescargar.Visible = false;
                            this.LinkDescargarZip.Visible = false;
                            this.UPACEPTA.Update();
                        }


                        rptImagenes.DataSource = Tabla;
                        rptImagenes.DataBind();

                        objCab = Session["Descuentos" + this.hf_BrowserWindowName.Value] as Damage_Descuentos_Cab;
                        objCab.Detalle_Contenedores.Clear();

                        foreach (var Det in Tabla)
                        {
                            objDetalleContainer = new Damage_Detalle_Contenedor();
                            objDetalleContainer.CraneName = Det.CraneName;
                            objDetalleContainer.Year = Det.Year;
                            objDetalleContainer.Month = Det.Month;
                            objDetalleContainer.Id = Det.Id;
                            objDetalleContainer.Unit = Det.Unit;
                            objDetalleContainer.View = Det.View;
                            objDetalleContainer.Url = Det.Url;
                            objDetalleContainer.Url_Large = Det.Url_Large;
                            objDetalleContainer.Ruta_Fisica = Det.Ruta_Fisica;
                            objCab.Detalle_Contenedores.Add(objDetalleContainer);
                        }

                        Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCab;

                        //eliminar archivos existentes
                        string directoryPath = Server.MapPath("~/damagecontrol/");

                        if (Directory.Exists(directoryPath))
                        {
                            // Obtener todos los archivos .zip en el directorio
                            string[] zipFiles = Directory.GetFiles(directoryPath, "*.zip");

                            foreach (string file in zipFiles)
                            {
                                // Eliminar el archivo
                                File.Delete(file);
                            }

                        }
                        //fin

                    }



                    this.Actualiza_Paneles();
                    this.Ocultar_Mensaje();


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "damageconsultacontenedor", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                }
            }

        }

        protected void grilla_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
               
            }

        }

      
  
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {

            try
            {
                Response.Redirect("~/damagecontrol/damageconsultacontenedor.aspx", false);
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "damageconsultacontenedor", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }

        protected void BtnExcel_Click(object sender, EventArgs e)
        {

          
        }


        protected void BtnConfirmar_Click(object sender, EventArgs e) 
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
                        OcultarLoading("1");
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objContainer = new Damage_Contenedor_Cliente();
                    objContainer.LINEA = ClsUsuario.ruc;
                    objContainer.CONTENEDOR = this.btnAccordion1.Text.Trim();
                    objContainer.DESC_USER_CREA = ClsUsuario.loginname;

                    var nProceso = objContainer.SaveTransaction(out cMensajes);
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo grabar datos de la aceptación del servicio de Damage Control..{0}</b>", cMensajes));
                        return;
                    }
                    else
                    {

                        BtnBuscar_Click(sender,e);

                        this.acepta_servicio_aceptado.Visible = true;
                        this.texto_servicio_aceptado.InnerHtml = string.Format("Estimada Línea Naviera {0}, gracias por aceptar el servicio de visualización de fotos de daños de contenedores.", ClsUsuario.ruc);
                        this.UPACEPTA.Update();
                    }



                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnConfirmar_Click), "damageconsultacontenedor", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }


        protected void BtnDescargar_Click(object sender, EventArgs e)
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
                        OcultarLoading("1");
                        OcultarLoading("2");
                        return;
                    }

                    objCab = Session["Descuentos" + this.hf_BrowserWindowName.Value] as Damage_Descuentos_Cab;
                    if (objCab == null)
                    {
                        OcultarLoading("2");
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe seleccionar el contenedor a exportar las imagenes."));
                        return;
                    }
                    else
                    {
                        string tempFolder = Server.MapPath("~/damagecontrol/");
                        string zipFileName = string.Format("{0}_{1}.zip", this.btnAccordion1.Text, System.DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss"));
                        string zipFilePath = Path.Combine(tempFolder, zipFileName);

                        // Crear la carpeta Temp si no existe
                        if (!Directory.Exists(tempFolder))
                        {
                            Directory.CreateDirectory(tempFolder);
                        }

                        if (File.Exists(string.Format("{0}{1}", tempFolder, zipFileName)))
                        {
                            File.Delete(string.Format("{0}{1}", tempFolder, zipFileName));
                        }


                        using (FileStream zipStream = new FileStream(zipFilePath, FileMode.Create))
                        {
                            using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                            {
                                foreach (var Det in objCab.Detalle_Contenedores)
                                {

                                    string imageUrl = Det.Ruta_Fisica;
                                    if (File.Exists(imageUrl))
                                    {
                                        zip.CreateEntryFromFile(imageUrl, Path.GetFileName(imageUrl));

                                    }
                                }


                            }
                        }



                        // Mostrar el enlace de descarga
                        LinkDescargarZip.NavigateUrl = string.Format("~/damagecontrol/{0}", zipFileName);
                        LinkDescargarZip.Text = "Descargar ZIP";
                        LinkDescargarZip.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "autoDownload", "autoDownload();", true);

                        OcultarLoading("2");

                        this.Actualiza_Paneles();



                    }

                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnDescargar_Click), "damageconsultacontenedor", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }



    }
}