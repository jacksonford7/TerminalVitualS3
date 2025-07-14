using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using csl_log;
using System.Net;
using BillionEntidades;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
namespace CSLSite
{
    public partial class aceptar : System.Web.UI.Page
    {
       

        public static usuario sUser = null;
        public static UsuarioSeguridad usAutenticado;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

      
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                try
                {
                    string cMensaje = string.Empty;
                    string cRuc = string.Empty;
                    string cNombre = string.Empty;
                    string cTelefono = string.Empty;
                    string cMail = string.Empty;

                    string Problema_Servicio = string.Empty;

                    string ID = Request.QueryString["id"];
                    if (ID == null || string.IsNullOrEmpty(ID))
                    {
                        this.alerta.Visible = false;
                    }
                    else
                    {
                        List<Cls_STC_LeerRuc> LeerRuc = Cls_STC_LeerRuc.Leer(ID, out cMensaje);
                        if (!String.IsNullOrEmpty(cMensaje))
                        {
                            this.alerta.Visible = true;
                            this.alerta.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en </br> {0} ", cMensaje);
                           
                            return;

                        }
                        foreach (var Det in LeerRuc)
                        {
                            cRuc = Det.ruc;
                        }

                        //informacion del cliente
                        List<Cls_STC_LeerRuc> LeerInformacion = Cls_STC_LeerRuc.Info_Cliente(cRuc, out cMensaje);
                        if (!String.IsNullOrEmpty(cMensaje))
                        {
                            this.alerta.Visible = true;
                            this.alerta.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en </br> {0} ", cMensaje);

                            return;

                        }
                        foreach (var Det in LeerInformacion)
                        {
                            cNombre = Det.CLNT_NAME.Replace("&"," Y");
                            cTelefono = Det.telefono;
                            cMail = Det.CLNT_EMAIL;
                        }


                        var usuario_sna = System.Configuration.ConfigurationManager.AppSettings["usuario_sna"];
                        var clave_sna = System.Configuration.ConfigurationManager.AppSettings["clave_sna"];
                        string Estado_Servicio = "0";
                        //xml a consultar
                        string XMLCna = string.Format("<existe><ruc>{0}</ruc></existe>", cRuc);


                        var WSCNA = new SNA.CRMService();
                        var Resultado = WSCNA.Invocar(usuario_sna.ToString(), clave_sna.ToString(), XMLCna.ToString());

                        if (Resultado != null)
                        {
                            string Res = Resultado.ToString();
                            var XMLResult = new XDocument();
                            try
                            {
                                XMLResult = XDocument.Parse(Res);
                            }
                            catch (Exception ex)
                            {
                                this.alerta.Visible = true;
                                var number = log_csl.save_log<Exception>(ex, "aceptar", "Page_Load()", DateTime.Now.ToShortDateString(), "sistemas");
                                var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                                this.alerta.InnerHtml = error;
                                return;
                            }

                            XElement XElemResult = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("resultado")).FirstOrDefault();
                            if (XElemResult != null)
                            {
                                XElement estado;
                                estado = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("estado")).FirstOrDefault();
                                Estado_Servicio = estado.Value;
                            }
                        }

                        //levanta popup si no tiene el servicio
                        if (Estado_Servicio.Equals("0"))
                        {
                            //grabar servicio
                            string XMLSna = string.Format("<cliente><ruc>{0}</ruc><nombre>{1}</nombre><activar>{2}</activar><categoria>{3}</categoria><telefono>{4}</telefono><email>{5}</email><parametros/></cliente>",
                             cRuc.Trim(), (string.IsNullOrEmpty(cNombre) ? "CLIENTE" : cNombre), 1, "IMPO",  (string.IsNullOrEmpty(cTelefono) ? "000000000" : cTelefono), (string.IsNullOrEmpty(cMail) ? "mail@mail.com" : cMail));

                            var WSCNA2 = new SNA.CRMService();
                            var Grabar = WSCNA2.Invocar(usuario_sna.ToString(), clave_sna.ToString(), XMLSna.ToString());
                            if (Grabar != null)
                            {
                                string Res = Grabar.ToString();
                                var XMLResult = new XDocument();
                                try
                                {
                                    XMLResult = XDocument.Parse(Res);
                                }
                                catch (Exception ex)
                                {

                                    this.alerta.Visible = true;
                                    var number = log_csl.save_log<Exception>(ex, "aceptar", "Page_Load()", DateTime.Now.ToShortDateString(), "sistemas");
                                    var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                                    this.alerta.InnerHtml = error;
                                    return;
                                }

                                XElement XElemResult = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("resultado")).FirstOrDefault();
                                if (XElemResult != null)
                                {
                                    XElement estado;
                                    estado = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("estado")).FirstOrDefault();
                                    Estado_Servicio = estado.Value;

                                    XElement problema;
                                    problema = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("problema")).FirstOrDefault();
                                    Problema_Servicio = problema?.Value;

                                    //levanta popup si no tiene el servicio
                                    if (Estado_Servicio.Equals("1"))
                                    {
                                        this.alerta.Visible = true;
                                        this.alerta.InnerHtml = "Proceso terminado con éxito.";
                                    }
                                    else
                                    {
                                        this.alerta.Visible = true;
                                        this.alerta.InnerHtml = "Proceso terminado con éxito.";

                                    }
                                }
                            }
                        }
                        else
                        {
                            this.alerta.Visible = true;
                            this.alerta.InnerHtml = "Proceso terminado con éxito.";
                        }


                       
                    }

                    

                }
                catch (Exception ex)
                {
                    this.alerta.Visible = true;
                    var number = log_csl.save_log<Exception>(ex, "aceptar", "Page_Load()", DateTime.Now.ToShortDateString(), "sistemas");
                    var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                    this.alerta.InnerHtml = error; 
                }

               

                
            }

        }

   
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

   
              

            }
        }
        
      
        protected void btexit_Click(object sender, EventArgs e)
        {
            
            FormsAuthentication.SignOut();
            this.Session.Abandon();
            this.Session.Clear();
            Response.RedirectPermanent("../login.aspx");
        }

       

        public string LoginPage
        {
            get { return this.namelogin.InnerText; }
        }


    }
}