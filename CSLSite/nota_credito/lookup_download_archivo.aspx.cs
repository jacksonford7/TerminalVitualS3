using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClsNotasCreditos;




using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using CSLSite.N4Object;
using CSLSite.XmlTool;
using ConectorN4;
using Newtonsoft.Json;
using csl_log;
using System.Text;
using System.Data;
using System.Globalization;

using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Collections;

namespace CSLSite
{
    public partial class lookup_download_archivo : System.Web.UI.Page
    {
        private usuario u;
        private DataTable dtDocumentos = new DataTable();
        private credit_head objcredit_head = new credit_head();
        private string nc_id = string.Empty;

       

        private void populate(Int64 nc_id)
        {
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            Session["resultado"] = null;

            string vr = string.Empty;
            try
            {
               
                var user = Page.getUserBySesion();

                var table = credit_head.List_Archivos(nc_id, out vr);
                if (table == null)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = vr;
                    sinresultado.Visible = true;
                    return;
                }
                if (table.Count <= 0)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = "No se encontraron resultados, no existen archivos subidos en el sistema.";
                    sinresultado.Visible = true;
                    return;
                }

                Session["resultado"] = table;
                this.tablePagination.DataSource = table;
                this.tablePagination.DataBind();
                xfinder.Visible = true;
                sinresultado.Visible = false;
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "lookup_download_archivo", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
                sinresultado.Visible = true;
            }

        }

        public void Descargar(string path)
        {
            try
            {
                System.IO.FileInfo toDownload = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(path));
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                HttpContext.Current.Response.AddHeader("Content-Length", toDownload.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.WriteFile(path);
               
                // HttpContext.Current.Response.End();
               

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "frm_listado_res_nota_credito", "Descargar()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString());
                sinresultado.Visible = true;
                return;
            }
        }

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
                    var user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "lookup_download_archivo", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }

                    if (e.CommandName == "Descargar")
                    {
                        string archivo;
                        archivo = e.CommandArgument.ToString();
                        if (!String.IsNullOrEmpty(archivo))
                        {
                            Descargar(archivo);

                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "lookup_download_archivo", "Item_comand", "Hubo un error al descargar", t.loginname));
                    sinresultado.Visible = true;

                }
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

            try
            {


                nc_id = QuerySegura.DecryptQueryString(Request.QueryString["nc_id"]);
                if (Request.QueryString["nc_id"] == null || string.IsNullOrEmpty(nc_id))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, nc_id de nota de credito no valido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "lookup_download_archivo", "Init", nc_id, Request.UserHostAddress);
                    this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "lookup_download_archivo", "Page_Init", nc_id, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código:{0}", number));
                base.Response.Write(close);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    if (Response.IsClientConnected)
                    {

                        /*Recuperar toda la referencia*/
                        Int64 id = 0;

                        nc_id = nc_id.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(nc_id, out id))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, nc_id no es numerico", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "lookup_download_archivo", "Page_Load", nc_id == null ? "nc_id no es numerico" : nc_id, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }

                     
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                        this.ruta_completa.Value = string.Empty;

                        populate(id);

                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lookup_download_archivo", "Page_Load", "", u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }

            }

        }
    
    }
}