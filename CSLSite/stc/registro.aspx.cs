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
    public partial class registro : System.Web.UI.Page
    {
       

        public static usuario sUser = null;
        private string cMensajes;
        private Cls_STC_Servicios objProcesar = new Cls_STC_Servicios();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }


        private void Mostrar_Mensaje(string Mensaje)
        {

            this.alerta.Visible = true;
            this.alerta.InnerHtml = Mensaje;
            OcultarLoading("1");
           // this.updConsultaUsuarios.Update();


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.txtruccipas.Text = string.Empty;
                this.txtmail.Text = string.Empty;
                this.txtrazonsocial.Text = string.Empty;
                this.txtruccipas.Focus();
            }

        }

   
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.alerta.InnerText = string.Empty;

            }
            this.alerta.Visible = IsPostBack;

        }

        protected void btngrabar_Click(object sender, EventArgs e)
        {
            try
            {
                string cMensaje = string.Empty;
                string cRuc = this.txtruccipas.Text.Trim();
                string cNombre = this.txtrazonsocial.Text.Trim().ToUpper();
                string cTelefono = "9999999999999";
                string cMail = this.txtmail.Text.Trim();

                string Problema_Servicio = string.Empty;

                objProcesar.ruc = cRuc;
                objProcesar.empresa = cNombre;
                objProcesar.email = cMail;
                var nProceso = objProcesar.SaveTransaction(out cMensajes);
                if (!nProceso.HasValue || nProceso.Value <= 0)
                {

                    this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo grabar datos de la empresa..{0}</b>", cMensajes));
                    return;
                }
                else
                {

                    this.txtmail.Text = string.Empty;
                    this.txtruccipas.Text = string.Empty;
                    this.txtrazonsocial.Text = string.Empty;

                    //this.Mostrar_Mensaje(string.Format("<b>Informativo! Empresa registrada con éxito..</b>"));


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
                         cRuc, cNombre, 1, "IMPO", cTelefono, cMail);

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
                                    this.alerta.InnerHtml = "Empresa registrada con éxito..Proceso terminado con éxito.";
                                }
                                else
                                {
                                    this.alerta.Visible = true;
                                    this.alerta.InnerHtml = "Empresa registrada con éxito..Proceso terminado con éxito.";

                                }
                            }
                        }
                    }
                    else
                    {
                        this.alerta.Visible = true;
                        this.alerta.InnerHtml = "Empresa registrada con éxito..Proceso terminado con éxito.";
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
        //protected void btexit_Click(object sender, EventArgs e)
        //{
            
        //    FormsAuthentication.SignOut();
        //    this.Session.Abandon();
        //    this.Session.Clear();
        //    Response.RedirectPermanent("../login.aspx");
        //}

       

        //public string LoginPage
        //{
        //    get { return this.namelogin.InnerText; }
        //}


    }
}