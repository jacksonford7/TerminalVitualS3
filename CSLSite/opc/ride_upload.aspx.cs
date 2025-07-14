

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using CSLSite.N4Object;
using CSLSite.XmlTool;
using ConectorN4;
using Newtonsoft.Json;
using csl_log;
using ControlOPC.Entidades;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;


namespace CSLSite
{
    public partial class ride_upload : System.Web.UI.Page
    {
        string sid;
        usuario user;
        string sg;
        Int64 num;
        Int64 id;
        Ride ri;
        ProformaCab pro;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.AbortResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
            if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "QuerySegura", "DecryptQueryString", sid, Request.UserHostAddress);
                this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            //convierto el valor
            if (!Int64.TryParse(sid, out id))
            {
                this.AbortResponse("Debe seleccionar una proforma para el despliegue de datos", "../cuenta/menu.aspx", true);
                return;
            }

            //  this.IsAllowAccess();
            user = Page.Tracker();
            if (user != null)
            {
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
            }
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {

            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Response.IsClientConnected)
            {
                dtlo.InnerHtml = dtlo.InnerText = string.Format("Estimado/a: {0}", user.nombres);

                if (string.IsNullOrEmpty(sid))
                {
                    this.AbortResponse("Debe seleccionar una proforma para subir su digital", "../cuenta/menu.aspx", true);
                    return;
                }
                //cargar la proforma 
                pro = ProformaCab.GetProformasCab(id, out sg);
                if (pro == null)
                {
                    this.AbortResponse(sg);
                    return;
                }
                pro.Mod_user = user.loginname;
                this.pro_id.InnerText = pro.Id.ToString("D8");
                this.pro_razon.InnerText = pro.Opc_name;
                this.pro_ruc.InnerText = pro.Opc_id;
                this.pro_subt.InnerText = pro.Total.ToString("C2");
                this.pro_total.InnerText = pro.Total.ToString("C2");
                this.pro_fecha.InnerText = pro.Create_date.Value.ToString("dd/MM/yyyy");
                this.pro_referencia.InnerHtml = string.Format("<p><strong> Referencia: {0}</strong></p>", pro.Vessel_visit_reference);

                //SI TIENE YA VALORES SUBIDOS
                fac_num.InnerText = string.IsNullOrEmpty(pro.num_xml) ? "..." : pro.num_xml;
                fac_fecha.InnerText = string.IsNullOrEmpty(pro.fecha_xml) ? "..." : pro.fecha_xml;

                 this.fac_razon.InnerText = "...";
                 this.fac_ruc.InnerText = string.IsNullOrEmpty(pro.ruc_xml) ? "..." : pro.ruc_xml;
                this.fac_subtotal.InnerText = pro.subtotal_xml.HasValue ? pro.subtotal_xml.Value.ToString("C2") : "...";
                this.fac_total.InnerText = pro.total_xml.HasValue ? pro.total_xml.Value.ToString("C2") : "...";


                Session["pro"] = pro;
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                var vw = ViewState["cambios"] as string;
                if (string.IsNullOrEmpty(vw))
                {
                    this.Alerta("Debe elegir un archivo antes de actualizar");
                    return;
                }

                try
                {
                    ri = Session["ride"] as Ride;
                    pro = Session["pro"] as ProformaCab; ;
                    
                    if (ri == null || pro==null)
                    {
                        this.Alerta("No se pudo obtener la proforma o el ride");
                        return;
                    }
                    
                    var eje =  pro.update_ride(ri, out sg);
                    if (!eje.HasValue)
                    {
                        this.Alerta(sg);
                        return;
                    }
                    if (!eje.Value)
                    {
                        this.Alerta(sg);
                        return;
                    }

                    var s = string.Format(" alert('Éxito al {0}'); window.close();window.opener.location.href = window.opener.location.href;", "Guardar");
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", s, true);
                }
                catch (Exception ex)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "ride_upload", "Guardar", "Hubo un error al Salvar", user != null ? user.loginname : "Nologin"));
                    this.Alerta(sg);
                    return;
                }
            }
        }

        protected void btsubir_Click(object sender, EventArgs e)
        {
            //pone rusuario

            if (!this.ffile.HasFile)
            {
                this.Alerta("Por favor seleccione el archivo Ride (XML)");
                return;
            }

            var nombrefile = ffile.PostedFile.FileName;
            if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".XML")
            {
                this.Alerta("La extensión del archivo debe ser XML");
                return;
            }
            if (ffile.PostedFile.ContentLength > 1500000)
            {
                this.Alerta("El archivo excede el tamaño límite");
                return;
            }

            //leo toda la cadena como string.
            var str = new StreamReader(ffile.PostedFile.InputStream).ReadToEnd();

            try
            {
                byte[] bytes = Encoding.Default.GetBytes(str);
                str = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                str = Regex.Replace(str, Environment.NewLine, string.Empty);
            }
    
            try
            {
 
                ri = Ride.getForXml(str, id, out sg);
                if (ri == null)
                {
                    return;
                }
                this.fac_fecha.InnerText = ri.fechaEmision;
                this.fac_num.InnerText = ri.numero;
                this.fac_razon.InnerText = ri.razonSocial;
                this.fac_subtotal.InnerText = ri.totalSinImpuestos;
                this.fac_total.InnerText = ri.importeTotal;
                this.fac_ruc.InnerText = ri.ruc;
                //ride
                Session["ride"] = ri;
                ViewState["cambios"] = "1";
                    
            }

            catch (Exception x)
            {
                this.Alerta(string.Format("Ha ocurrido la excepción #{0}",num));
                num = csl_log.log_csl.save_log<Exception>(x, "ride_upload", "btup_Click-exception", str, "sin usuario");
                return;
            }

        }
    }
}