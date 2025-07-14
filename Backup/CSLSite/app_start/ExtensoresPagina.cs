using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSLSite.cuenta;
using System.Text;

namespace CSLSite
{
    public static class extensores
    {
        //revisa si este usurio puede acceder a esta opción->webform
        public static bool IsAutorized(this System.Web.UI.Page page, usuario user)
        {
            //aqui la implementación para saber si este usuario tiene autorizada esta opción.
            return true;
        }
        /*
        //retorna un arreglo de opciones basado en un servicio
        public static string Opciones(this System.Web.UI.Page page, usuario user, ref HashSet<string> p_lista)
        {
            var div = new StringBuilder();
            if (user != null)
            {
                var permisos = user.autorized_access();
                if (permisos == null)
                {
                    return string.Format("No tiene accesos definidos");
                }
                //las zonas autorizadas de este usuario
                var i = 0;
                foreach (var acceso in user.autorized_zones())
                {
                    var option_zone = permisos.Where(a => a.idservicio == acceso.idservicio).FirstOrDefault();
                    div.AppendFormat("<div class=\"contorno\"><div class=\"cleft\"><table cellspacing =\"2\" cellpadding=\"1\"><tr><td><img class=\"icono\" src=\"{0}\" alt=\"NoIcon\" /></td><td class=\"arriba\" ><p class=\"t-left\">{1}</p></td></tr></table></div><div class= \"content-opciones\">", acceso.icono, acceso.titulo);
                    //si tiene permisos en esta área.
                    if (option_zone != null)
                    {
                        var indice = 0;
                        foreach (var item in CLSData.ValorLecturas("sp_user_options", tComando.Procedure, new Dictionary<string, string>() { { "idservicio", acceso.idservicio.ToString() } }))
                        {
                            var z = new opcion();
                            z.idservicio = item[0] as int?;
                            z.idopcion = item[1] as int?;
                            z.descripcion = item[2] as string;
                            z.icono = item[3] as string;
                            z.textointro = item[4] as string;
                            z.url = item[5] as string;
                            if (option_zone.opciones != null)
                            {
                                if (option_zone.opciones.Length > indice)
                                {
                                    z.activa = option_zone.opciones[indice] == '1' ? true : false;
                                }
                            }
                            if (z.activa && !string.IsNullOrEmpty(z.url))
                            {
                                if (p_lista != null)
                                {
                                    p_lista.Add(z.url.Split('/').LastOrDefault());
                                }
                                div.AppendFormat("<a href=\"{0}\" xtitle=\"{1}\"   onmouseover=\"displaytext(this,descripciones{2});\">{3}</a>", z.url, z.textointro, i, z.descripcion);
                            }
                            indice++;
                        }
                    }
                    div.Append("</div>");
                    div.AppendFormat("<div class=\"content-descripcion\">Descripción <p class=\"p-descrip\" id=\"descripciones{0}\">Ponga el mouse sobre una de las opciones para ver su descripción</p></div>", i);
                    div.Append("</div>");
                    i++;
                }
            }
            return div.ToString();
        }
        //siembra el token
      */

         //retorna un arreglo de opciones basado en un servicio
        public static string Opciones(this System.Web.UI.Page page, usuario user, ref HashSet<string> p_lista)
        {
            int id_Servicio = 0;
            int id_opcion = 0;

            var div = new StringBuilder();
            if (user != null)
            {
                var permisos = user.autorized_access();
                if (permisos == null)
                {
                    return string.Format("No tiene accesos definidos");
                }
                /*Aqui bloquear los accesos al AISV compañia*/
                //Eliminar los accesos que esten en la tabla de bloqueos, cliente, rol, pago
                //lista de opciones bloqueadas
                var lockp = user.block_options();

               

                /*------------------------------------------*/
                //las zonas autorizadas de este usuario
                var i = 0;
                foreach (var acceso in user.autorized_zones())
                {
                    var option_zone = permisos.Where(a => a.idservicio == acceso.idservicio).FirstOrDefault();
                    div.AppendFormat("<div class=\"contorno\"><div class=\"cleft\"><table cellspacing =\"2\" cellpadding=\"1\"><tr><td><img class=\"icono\" src=\"{0}\" alt=\"NoIcon\" /></td><td class=\"arriba\" ><p class=\"t-left\">{1}</p></td></tr></table></div><div class= \"content-opciones\">", acceso.icono, acceso.titulo);
                    //si tiene permisos en esta área.
                    if (option_zone != null)
                    {
                        var indice = 0;
                        foreach (var item in CLSData.ValorLecturas("sp_user_options", tComando.Procedure, new Dictionary<string, string>() { { "idservicio", acceso.idservicio.ToString() } }))
                        {
                            var z = new opcion();
                            z.idservicio = item[0] as int?;
                            z.idopcion = item[1] as int?;
                            z.descripcion = item[2] as string;
                            z.icono = item[3] as string;
                            z.textointro = item[4] as string;
                            z.url = item[5] as string;
                            if (option_zone.opciones != null)
                            {
                                if (option_zone.opciones.Length > indice)
                                {
                                    z.activa = option_zone.opciones[indice] == '1' ? true : false;
                                }
                            }

                            //-->VERIFICAR QUE EXISTAN BLOQUEOS EN TABLA BLOQUEOS
                            if (lockp != null && lockp.Count > 0)
                            {
                                id_Servicio = acceso.idservicio.Value;
                                id_opcion = z.idopcion.Value;
                                //APLICAR REGLAS DE CLIENTE
                                var bloqueada = lockp.Where(d => d.Item1 == id_Servicio && d.Item2 == id_opcion && d.Item3.Contains("C")).Count() > 0 ? true : false;
                                if (bloqueada)
                                {
                                    z.activa = false;
                                }
                                //APLICAR REGLAS DE CLIENTE
                                bloqueada = lockp.Where(d => d.Item1 == id_Servicio && d.Item2 == id_opcion && d.Item3.Contains("R")).Count() > 0 ? true : false;
                                if (bloqueada)
                                {
                                    z.activa = false;
                                }
                                //APLICAR REGLAS DE PAGO
                                if (user.IsPaidLock)
                                {
                                    bloqueada = lockp.Where(d => d.Item1 == id_Servicio && d.Item2 == id_opcion && d.Item3.Contains("G") && d.Item4.Contains("P")).Count() > 0 ? true : false;
                                    if (bloqueada)
                                    {
                                        z.activa = false;
                                    }
                                }
                            }

                            if (z.activa && !string.IsNullOrEmpty(z.url))
                            {
                                if (p_lista != null)
                                {
                                    p_lista.Add(z.url.Split('/').LastOrDefault());
                                }
                                div.AppendFormat("<a href=\"{0}\" xtitle=\"{1}\"   onmouseover=\"displaytext(this,descripciones{2});\">{3}</a>", z.url, z.textointro, i, z.descripcion);
                            }
                            indice++;
                        }
                    }
                    div.Append("</div>");
                    div.AppendFormat("<div class=\"content-descripcion\">Descripción <p class=\"p-descrip\" id=\"descripciones{0}\">Ponga el mouse sobre una de las opciones para ver su descripción</p></div>", i);
                    div.Append("</div>");
                    i++;
                }
            }
            return div.ToString();
        }
        
        public static usuario Tracker(this System.Web.UI.Page page)
        {
            var user = new usuario();
            //obtener la sesion
            if (!page.IsPostBack)
            {
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), page.GetType().ToString(), "Tracker", "No disponible", "No disponible");
                    HttpContext.Current.Response.Redirect("~/csl/login", true);
                    user = null;
                    return user;
                }
                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //creo la cokie token
                var ck = new HttpCookie("token");
                // la expiro en 45 minutos
                ck.Expires = DateTime.Now.AddMinutes(30);
                //le digo que solo es usable via http
                ck.HttpOnly = true;
                //HABILITAR Y PROBAR VIA HTTP->COKIE
             
                var desa = System.Configuration.ConfigurationManager.AppSettings["DESARROLLO"];
                if (desa == null || desa.Contains("0"))
                {
                    ck.Secure = true;
                }
                //Si es la primera vez para este formulario entonces siembre el token
                var tk = string.Empty;
                if (csl_log.log_csl.createToken(user.loginname, page.GetType().ToString(), out tk))
                {
                    ck.Value = tk;
                }
                HttpContext.Current.Response.Cookies.Add(ck);
                //AÑADO EL TOKEN
                HttpContext.Current.Session["tokenID"] = QuerySegura.EncryptQueryString(string.Format("{0}.{1}.{2}", user.idcorporacion.HasValue ? user.idcorporacion : 0, user.id, HttpContext.Current.Session.SessionID));
                csl_log.log_csl.saveTracking_det(HttpContext.Current.Session.SessionID, HttpContext.Current.Request.Url.AbsoluteUri, tk);
            }
            else
            {
                user = null;
            }
            return user;
        }
        //revisa que todo el tiempo estemos en HTTPS
        public static void SslOn(this System.Web.UI.Page page)
        {
            if (!HttpContext.Current.Request.IsSecureConnection)
            {

                           var valor = System.Configuration.ConfigurationManager.AppSettings["DESARROLLO"];
                           if (valor == null || valor.Contains("0"))
                           {
                               page.Response.Clear();
                               string close = @"<script type='text/javascript'>  alert('La conexión establecida no es segura no podrá continuar, Gracias por comprender.');   window.returnValue = true; window.close();</script>";
                               csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, intento de acceso no permitido", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url, HttpContext.Current.Request.HttpMethod)), page.GetType().ToString(), "SslOn", "Fallo de socket HTTPS", "Espía");
                               page.Response.Write(string.Format("<img src='{0}'/><hr/>Sistema de Solicitud de Servicios<br/><div>Por su protección se ha denegado el acceso, por favor cierre esta ventana y luego intente abrir sesión nuevamente.</div><hr/>", VirtualPathUtility.ToAbsolute("~/shared/imgs/logocontecon.png")));
                               page.Response.Write(close);
                               page.Response.End();
                           }
                 return;
            }
        }
        //revisa que todos los parametros de QS esten seteados
        public static void ReviewQS(this System.Web.UI.Page page, string[] parametros, string mensaje, bool qs = true)
        {
            for (int i = 0; i <= parametros.Length - 1; i++)
            {
                if (qs)
                {
                    if (HttpContext.Current.Request.QueryString[parametros[i]] == null)
                    {
                        page.Response.Clear();
                        string close = string.Format(@"<script type='text/javascript'>alert('{0}'); window.returnValue = true; window.close();</script>", mensaje);
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, No se encontó una variable en el QS", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url, HttpContext.Current.Request.HttpMethod)), page.GetType().ToString(), "ReviewQS", parametros[i], "Espía");
                        page.Response.Write(string.Format("<img src='{0}'/><hr/>Sistema de Solicitud de Servicios<br/><div>Hubo un problema durante la solicitud por favor reintente en unos minutos..</div><hr/>", VirtualPathUtility.ToAbsolute("~/shared/imgs/logocontecon.png")));
                        page.Response.Write(close);
                        page.Response.End();
                        return;
                    }
                }
                else
                {
                    if (page.RouteData.Values[parametros[i]] == null)
                    {
                        page.Response.Clear();
                        string close = string.Format(@"<script type='text/javascript'>alert('{0}'); window.returnValue = true; window.close();</script>", mensaje);
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, No se encontó una variable en el QS", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url, HttpContext.Current.Request.HttpMethod)), page.GetType().ToString(), "ReviewQS", parametros[i], "Espía");
                        page.Response.Write("<img src='../shared/imgs/logocontecon.png'/><hr/>Sistema de Solicitud de Servicios<br/><div>Hubo un problema durante la solicitud por favor reintente en unos minutos..</div><hr/>");
                        page.Response.Write(close);
                        page.Response.End();
                        return;
                    }
                }
            }
        }
        //retorna el usuario de la sesión actual.
        public static usuario getUserBySesion (this System.Web.UI.Page page)
        {
                var user = new usuario();
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), page.GetType().ToString(), "Tracker", "No disponible", "No disponible");
                    HttpContext.Current.Response.Redirect("~/csl/login", true);
                    return user;
                }
                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                return user;
        }
        //soporto Javascript!
        public static void IsJSSuport(this System.Web.UI.Page page)
        {
            //navegador no soporta javascript!
            if (!(HttpContext.Current.Request.Browser.EcmaScriptVersion.Major < 1))
            {
                page.Response.Clear();
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, No se encontó una variable en el QS", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url, HttpContext.Current.Request.HttpMethod)), page.GetType().ToString(), "IsJSSuport", "NavegadorCliente", "Monitoreo");
                page.Response.Write(string.Format("<img src='{0}'/><hr/>Sistema de Solicitud de Servicios<br/><div>Para que el Sistema de Solicitud de Servicios funcione correctamente su navegador debe soportar Javascript, gracias por entender.<br/>Para mas información comuníquese con Contecon, área Desarrollo.</div><hr/>", VirtualPathUtility.ToAbsolute("~/shared/imgs/logocontecon.png")));
                page.Response.End();
                return;
            }
        }
        public static void PersonalResponse(this System.Web.UI.Page page,string mensaje,string newurl = null, bool redirect=false)
        {
            page.Response.Clear();
            var response = string.Empty;
            response = string.Format("<img src='{1}'/><hr/>Sistema de Solicitud de Servicios<br/><div>{0}</div><hr/>", mensaje, VirtualPathUtility.ToAbsolute("~/shared/imgs/logocontecon.png"));
            if (redirect)
            {
                response = string.Concat(string.Format("<meta http-equiv=\"refresh\" content=\"5; url={0}\" />",newurl),response);
            }
            page.Response.Write(response);
            page.Response.End();
        }
        //comprobar la versión del navegador del cliente
        public static void IsCompatibleBrowser(this System.Web.UI.Page page )
        {
            var bw = HttpContext.Current.Request.Browser;
            var fg = false;
            decimal version=0;
            var valor = bw.Version.Replace(".", ",");
            var zar = valor.Split(',');
            if (zar.Length > 0)
            {
                if (!decimal.TryParse(zar[0], out version))
                {
                   version = 8;
                }
            }
            if (bw.Browser.ToLower().Contains("ie") && version <= 9)
            {
                fg = true;
            }
            if (bw.Browser.ToLower().Contains("firefox") && version < 22)
            {
                fg = true;
            }
              //HABILITAR Y PROBAR VIA HTTP->COKIE
#if DEBUG
            fg = false;
#endif
            if (fg)
            {
                var st = string.Empty;
                st = string.Concat(st, string.Format("<img src='{0}'/><hr/>Sistema de Solicitud de Servicios<br/><div>Para el correcto funcionamiento del Sistema de Solicitud de Servicios, necesita un navegador con una versión superior a la que usted posee instalado:</div><hr/>", VirtualPathUtility.ToAbsolute("~/shared/imgs/logocontecon.png")));
                st = string.Concat(st, "<table cellpading='2' cellspacing='2'>");
                st = string.Concat(st, "<tr><th>Detalle</th><th>Versión</th></tr>");
                st = string.Concat(st, string.Format("<tr><td>Navegador:</td><td>{0}</td></tr>", bw.Browser));
                st = string.Concat(st, string.Format("<tr><td>Versión:</td><td>{0}</td></tr>", bw.Version));
                st = string.Concat(st, string.Format("<tr><td>Versión Mayor:</td><td>{0}</td></tr>", bw.MajorVersion));
                st = string.Concat(st, string.Format("<tr><td>Versión Menor:</td><td>{0}</td></tr>", bw.MinorVersion));
                st = string.Concat(st, "</table>");
                st = string.Concat(st, "<p><strong>Nota:</strong> Si utliza internet explorer asegúrese que el <a href='http://windows.microsoft.com/es-xl/internet-explorer/use-compatibility-view#ie=ie-9'>MODO COMPATIBILIDAD</a> esta desactivado.</p>");
                st = string.Concat(st, "<p> Comience descargando la última versión de su navegador favorito, desde los siguientes enlaces: </p>");
                st = string.Concat(st, string.Format("<img src='{0}' width='20px' height='20px' alt='' />&nbsp;<a href='http://windows.microsoft.com/es-419/internet-explorer/ie-9-worldwide-languages' >Internet Explorer</a><br/>", VirtualPathUtility.ToAbsolute("~/shared/imgs/explore.png")));
                st = string.Concat(st, string.Format("<img src='{0}' width='20px' height='20px' alt='' />&nbsp;<a href='https://www.mozilla.org/es-ES/firefox/new/' >Mozilla Firefox</a><br />", VirtualPathUtility.ToAbsolute("~/shared/imgs/firefox.jpg")));
                st = string.Concat(st, string.Format("<img src='{0}' width='20px'  height='20px' alt='' />&nbsp;<a href='http://www.google.com.ec/intl/es-419/chrome/' >Google Chrome</a><br/>", VirtualPathUtility.ToAbsolute("~/shared/imgs/chrome.png")));
                st = string.Concat(st, string.Format("<img src='{0}' width='20px'  height='20px' alt='' />&nbsp;<a href='http://support.apple.com/kb/dl1531?viewlocale=es_ES' >Safari</a><br/>", VirtualPathUtility.ToAbsolute("~/shared/imgs/safari.png")));
                st = string.Concat(st, "<hr />Gracias por entender..");
                page.Response.Write(st);
                page.Response.End();
            }
        }
        //comprueba si tiene acceso a un área especifica
        public static void IsAllowAccess(this System.Web.UI.Page page)
        {
            if (HttpContext.Current.Session["acceso"] == null)
            {
                var ticket=  csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La variable de session [acceso], no existe"), page.GetType().ToString(), "IsAllowAccess", "No disponible", "No disponible");
                PersonalResponse(page, string.Format("La sesión de usuario ha expirado, será redireccionado a la pantalla de inicio. Ticket [{0}]",ticket), "~/csl/login", true);
                return;
            }
            var opt =(HashSet<string>) HttpContext.Current.Session["acceso"];
            if (opt.Count <= 0)
            {
                var ticket = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La variable de session [acceso], no tiene items"), page.GetType().ToString(), "IsAllowAccess", "No disponible", "No disponible");
                PersonalResponse(page, string.Format("La sesión de usuario ha expirado, será redireccionado a la pantalla de inicio. Ticket [{0}]", ticket), "~/csl/login", true);
                return;
            }
            var t = page.Request.RawUrl.Split('/').LastOrDefault();
            if (!opt.Contains(t))
            {
                var ticket = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("La variable de session [acceso], no contiene a {0}",page.Request.RawUrl)), page.GetType().ToString(), "IsAllowAccess", "No disponible", "No disponible");
                PersonalResponse(page, string.Format("Esta intentado acceder a una zona que requiere otros privilegios, por favor comuníquese con CGSA, Fecha:[{2}], Ticket [{0}], IP [{1}]", ticket,page.Request.UserHostAddress, DateTime.Now));
                return;
            }
        }
        public static void IsTokenAlive(this System.Web.UI.Page page)
        {
            if (HttpContext.Current.Request.Cookies["token"] == null)
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Session.Clear();
                }
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            }
        }
        public static void Alerta(this System.Web.UI.Page page, string mensaje, bool exit=false)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(mensaje))
            {
                if (AjaxControlToolkit.ToolkitScriptManager.GetCurrent(page) != null)
                {
                    if (AjaxControlToolkit.ToolkitScriptManager.GetCurrent(page).IsInAsyncPostBack)
                    {
                        sb.AppendFormat("alert('{0}');", mensaje);
                        if (exit)
                        {
                            sb.Append("window.location='../csl/login'");
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), (new Guid()).ToString(), sb.ToString(), true);
                    }
                    else
                    {
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("window.onload=function() {");
                        sb.AppendFormat("alert('{0}');", mensaje);
                        if (exit)
                        {
                            sb.Append("window.location='../csl/login'");
                        }
                        sb.Append("}</script>");
                        page.ClientScript.RegisterClientScriptBlock(page.GetType(), (new Guid()).ToString(), sb.ToString(), false);
                    }
                }
                else
                {
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("window.onload=function() {");
                    sb.AppendFormat("alert('{0}');", mensaje);
                    if (exit)
                    {
                        sb.Append("window.location='../csl/login'");
                    }
                    sb.Append("}</script>");
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), (new Guid()).ToString(), sb.ToString(),false);
                }
            }
            if (exit)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            }

        }
        public static void Popup(this System.Web.UI.Page page, string url)
        {
            var sb = new StringBuilder();

           
            if (!string.IsNullOrEmpty(url))
            {
                if (AjaxControlToolkit.ToolkitScriptManager.GetCurrent(page) != null)
                {
                    if (AjaxControlToolkit.ToolkitScriptManager.GetCurrent(page).IsInAsyncPostBack)
                    {
                        sb.AppendFormat("window.open('{0}','Impresion','width=850,height=580,scrollbars=yes,resizable=yes');", url);
                        System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), (new Guid()).ToString(), sb.ToString(), true);
                    }
                    else
                    {
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("window.onload=function() {");
                        sb.AppendFormat("window.open('{0}','Impresion','width=850,height=580,scrollbars=yes,resizable=yes');", url);
                        sb.Append("}</script>");
                        page.ClientScript.RegisterClientScriptBlock(page.GetType(), (new Guid()).ToString(), sb.ToString(),false);
                    }
                }
                else
                {
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("window.onload=function() {");
                    sb.AppendFormat("window.open('{0}','Impresion','width=850,height=580,scrollbars=yes,resizable=yes');", url);
                    sb.Append("}</script>");
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), (new Guid()).ToString(), sb.ToString(), false);
                }
            }
        }
        public static void Addscript(this System.Web.UI.Page page, string script)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(script))
            {
                if (AjaxControlToolkit.ToolkitScriptManager.GetCurrent(page) != null)
                {
                    if (AjaxControlToolkit.ToolkitScriptManager.GetCurrent(page).IsInAsyncPostBack)
                    {
                        sb.Append(script);
                        System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), (new Guid()).ToString(), sb.ToString(), true);
                    }
                    else
                    {
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("window.onload=function() {");
                        sb.AppendFormat("alert('{0}');", script);
                        sb.Append("}</script>");
                        page.ClientScript.RegisterClientScriptBlock(page.GetType(), (new Guid()).ToString(), sb.ToString(), false);
                    }
                }
                else
                {
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("window.onload=function() {");
                    sb.AppendFormat("alert('{0}');", script);

                    sb.Append("}</script>");
                    page.ClientScript.RegisterClientScriptBlock(page.GetType(), (new Guid()).ToString(), sb.ToString(), false);
                }
            }

        }
        //retorna un arreglo de opciones basado en un servicio
        public static string OpcionesAdministrativas(this System.Web.UI.Page page, usuario user, ref HashSet<string> p_lista)
        {
            var div = new StringBuilder();
            if (user != null)
            {
                var permisos = user.autorized_access_admin();
                if (permisos == null)
                {
                    return string.Format("No tiene accesos definidos");
                }
                //las zonas autorizadas de este usuario
                var i = 0;
                foreach (var acceso in user.autorized_zones_admin())
                {
                    var option_zone = permisos.Where(a => a.idservicio == acceso.idservicio).FirstOrDefault();
                    div.AppendFormat("<div class=\"contorno\"><div class=\"cleft\"><table cellspacing =\"2\" cellpadding=\"1\"><tr><td><img class=\"icono\" src=\"{0}\" alt=\"NoIcon\" /></td><td class=\"arriba\" ><p class=\"t-left\">{1}</p></td></tr></table></div><div class= \"content-opciones\">", acceso.icono, acceso.titulo);
                    //si tiene permisos en esta área.
                    if (option_zone != null)
                    {
                        var indice = 0;
                        foreach (var item in CLSData.ValorLecturas("sp_user_options", tComando.Procedure, new Dictionary<string, string>() { { "idservicio", acceso.idservicio.ToString() } }))
                        {
                            var z = new opcion();
                            z.idservicio = item[0] as int?;
                            z.idopcion = item[1] as int?;
                            z.descripcion = item[2] as string;
                            z.icono = item[3] as string;
                            z.textointro = item[4] as string;
                            z.url = item[5] as string;
                            if (option_zone.opciones != null)
                            {
                                if (option_zone.opciones.Length > indice)
                                {
                                    z.activa = option_zone.opciones[indice] == '1' ? true : false;
                                }
                            }
                            if (z.activa && !string.IsNullOrEmpty(z.url))
                            {
                                if (p_lista != null)
                                {
                                    p_lista.Add(z.url.Split('/').LastOrDefault());
                                }
                                div.AppendFormat("<a href=\"{0}\" xtitle=\"{1}\"   onmouseover=\"displaytext(this,descripciones{2});\">{3}</a>", z.url, z.textointro, i, z.descripcion);
                            }
                            indice++;
                        }
                    }
                    div.Append("</div>");
                    div.AppendFormat("<div class=\"content-descripcion\">Descripción <p class=\"p-descrip\" id=\"descripciones{0}\">Ponga el mouse sobre una de las opciones para ver su descripción</p></div>", i);
                    div.Append("</div>");
                    i++;
                }
            }
            return div.ToString();
        }
    }

}