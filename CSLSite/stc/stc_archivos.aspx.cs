using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using MSCaptcha;
using System.Text;
using System.Drawing.Imaging;
using csl_log;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BillionEntidades;

namespace CSLSite
{
    public partial class stc_archivos : System.Web.UI.Page
    {
        
        private string cMensajes;

        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
       
        private String xmlDocumentos;
        private Cls_STC_Imagenes objProcesar = new Cls_STC_Imagenes();



        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        private void Mostrar_Mensaje( string Mensaje)
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
            OcultarLoading("2");
        }

        private void Actualiza_Paneles()
        {
            //UPIMAGEN.Update();
           //UPDETALLE.Update();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            //this.IsAllowAccess();
            var user = Page.Tracker();

            this.banmsg.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;

            }

            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();

            }



        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                this.mrn.Value = string.Empty;
                this.id_importador.Value = string.Empty;
                this.unidad.Value = string.Empty;
                this.msn.Value = string.Empty;
                this.hsn.Value = string.Empty;
                this.gkey.Value = "0";
                this.btsalvar.Attributes["disabled"] = "disabled";
                this.ls6.Visible = false;
            }
        }
        


   
       
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.btsalvar.Attributes["disabled"] = "disabled";
                this.ls6.Visible = false;

                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    OcultarLoading("1");
                    return;
                }

                if (string.IsNullOrEmpty(this.TxtContenedor.Text))
                {
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de contenedor</b>"));
                    this.TxtContenedor.Focus();
                    return;
                }


                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                /*ultima factura*/
                List<Cls_STC_Imagenes> STC = Cls_STC_Imagenes.Info_Aforo(this.TxtContenedor.Text.Trim(), out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {

                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener información de la carga....{0}</b>", cMensajes));
                    this.Actualiza_Paneles();
                    return;
                }

                var LinqDatos = (from Tbl in STC.Where(Tbl => !string.IsNullOrEmpty(Tbl.unidad))
                                  select new
                                  {
                                      mrn = Tbl.mrn,
                                      id_importador = Tbl.id_importador,
                                      unidad = Tbl.unidad,
                                      msn = Tbl.msn,
                                      hsn = Tbl.hsn,
                                      gkey = Tbl.gkey,
                                  }).FirstOrDefault();


                if (LinqDatos != null)
                {

                    this.mrn.Value = LinqDatos.mrn.ToString();
                    this.id_importador.Value = LinqDatos.id_importador;
                    this.unidad.Value = LinqDatos.unidad;
                    this.msn.Value = LinqDatos.msn;
                    this.hsn.Value = LinqDatos.hsn;
                    this.gkey.Value = LinqDatos.gkey.ToString();
                    this.Ocultar_Mensaje();

                    var table = Cls_STC_Imagenes.Consulta_Imagenes(LinqDatos.gkey, LinqDatos.mrn, LinqDatos.msn, LinqDatos.hsn, out cMensajes);
                    if (table == null)
                    {
                        this.btsalvar.Attributes.Remove("disabled");
                        xfinder.Visible = false;
                    }
                    else
                    {
                        if (table.Count <= 0)
                        {
                            this.btsalvar.Attributes.Remove("disabled");
                            this.tablePagination.DataSource = null;
                            this.tablePagination.DataBind();
                            xfinder.Visible = false;
                        }
                        else
                        {
                            xfinder.Visible = true;
                            this.tablePagination.DataSource = table;
                            this.tablePagination.DataBind();

                            OcultarLoading("1");
                            this.Actualiza_Paneles();

                            return;
                        }
                    } 

                }
                else
                {
                    tablePaginationDocumentos.DataSource = null;
                    tablePaginationDocumentos.DataBind();

                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información con el número  de contenedor ingresado..</b>"));
                    this.Actualiza_Paneles();
                    return;
                }



                List<Cls_STC_Imagenes> Imagenes = new List<Cls_STC_Imagenes>();
                for (int i = 1; i <= 4; i++)
                {
                    Imagenes.Add(new Cls_STC_Imagenes { id = i, imagen = string.Empty, nombre = string.Format("Imagen {0}",i), extension = ".jpg" });

                }

                this.ls6.Visible = true;
                tablePaginationDocumentos.DataSource = Imagenes;
                tablePaginationDocumentos.DataBind();

                OcultarLoading("1");
                this.Actualiza_Paneles();
                
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "stc_archivos", "btsalvar_Click()", DateTime.Now.ToShortDateString(), "sistemas");
                var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", error));
               
            }
        }



        private void New_ExportFileUpload()
        {

            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("ruc");
            dtDocumentos.Columns.Add("gkey");
            dtDocumentos.Columns.Add("contenedor");
            dtDocumentos.Columns.Add("mrn");
            dtDocumentos.Columns.Add("msn");
            dtDocumentos.Columns.Add("hsn");
            dtDocumentos.Columns.Add("imagen");
            dtDocumentos.Columns.Add("usuario");
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string finalname;
                     var p = CSLSite.app_start.CredencialesHelper.UploadFile_Stc(Server.MapPath(fsupload.FileName), fsupload.PostedFile.InputStream,out finalname);
                    if (!p)
                    {
                        this.Alerta(finalname);
                        return;
                    }
                    dtDocumentos.Rows.Add(this.id_importador.Value, Convert.ToInt64(this.gkey.Value), this.unidad.Value, this.mrn.Value, this.msn.Value, this.hsn.Value,finalname, ClsUsuario.loginname);
                }
            }
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
        }




        protected void btsalvar_Click(object sender, EventArgs e)
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

                try
                {
                    this.New_ExportFileUpload();

                    objProcesar.xmlCabecera = xmlDocumentos.ToString();

                    var nProceso = objProcesar.SaveTransaction(out cMensajes);
                    /*fin de nuevo proceso de grabado*/
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {

                        this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo grabar datos de las imagenes de contenedor..{0}</b>", cMensajes));
                        return;
                    }
                    else
                    {

                        this.btsalvar.Attributes["disabled"] = "disabled";
                        this.ls6.Visible = false;

                        OcultarLoading("2");
                        this.mrn.Value = string.Empty;
                        this.id_importador.Value = string.Empty;
                        this.unidad.Value = string.Empty;
                        this.msn.Value = string.Empty;
                        this.hsn.Value = string.Empty;
                        this.gkey.Value = "0";
                        tablePaginationDocumentos.DataSource = null;
                        tablePaginationDocumentos.DataBind();
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Imágenes subidas con éxito..</b>"));
                        this.Actualiza_Paneles();
                    }

                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "stc_archivos", "btsalvar_Click()", DateTime.Now.ToShortDateString(), "sistemas");
                    var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", error));
                }
            }
              
        }

       

       

      
    }
}