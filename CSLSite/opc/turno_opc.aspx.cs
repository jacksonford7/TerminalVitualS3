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


namespace CSLSite
{
    public partial class turno_opc : System.Web.UI.Page
    {

        //AntiXRCFG
        string sid;
        Vessel_Visit vv;
        usuario user;

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        static Opc opc = null;
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
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
            user = Page.Tracker();
            if (user != null)
            {
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
            }
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                opc = Opc.GetOPC(user.ruc);
                if (opc == null)
                {
                    this.AbortResponse(string.Format("El RUC [{0}] no corresponde al de una OPC registrada, por favor comuníquese con credenciales y permisos", user.ruc));
                    return;
                }
           }
            try
            {

                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de plan no valido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "turno_opc", "Init", sid, Request.UserHostAddress);
                    this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "turno_opc", "Page_Init", sid, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la carga de datos, por favor repórtelo con este código:{0}", number));
                base.Response.Write(close);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Response.IsClientConnected)
            {
                string sg;
                try
                {
                    dtlo.InnerHtml = dtlo.InnerText = string.Format("Estimado/a: {0}", opc.Nombre);
                    Int64 i = 0;
                    if (string.IsNullOrEmpty(sid))
                    {
                        this.AbortResponse("Debe seleccionar una referencia para el despliegue de datos", "../cuenta/menu.aspx", true);
                        return;
                    }
                    if (!Int64.TryParse(sid, out i))
                    {
                        var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de plan no valido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        var number = log_csl.save_log<Exception>(ex, "turno_opc", "Init", sid, Request.UserHostAddress);
                        this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                        return;
                    }

                    //la primera vez pongo todo lo nuevo en pantalla
                    vv = new Vessel_Visit(i);
                    string error = string.Empty;
                    vv.PopulateMyData(out error);
                    vv.LoadCranes();
                    vv.LoadTurns();
                    if (!string.IsNullOrEmpty(error))
                    {
                        this.PersonalResponse(error, "../cuenta/menu.aspx", true);
                        return;
                    }
                    //ya tengo la nave.
                    var v = Vessel.ListaVessel(vv.REFERENCE).FirstOrDefault();
                    //presentar datos basicos, porque se debe cargar los turnos de esta nave
                    this.buque.InnerText = vv.NAME;
                    this.referencia.InnerText = vv.REFERENCE;
                    this.eta.InnerText = vv.ETA.HasValue ? vv.ETA.Value.ToString("dd/MM/yyyy HH:mm") : "-";
                    this.etd.InnerText = vv.ETD.HasValue ? vv.ETD.Value.ToString("dd/MM/yyyy HH:mm") : "-";
                    this.vio.InnerText = String.Format("{0}/{1}", vv.VOYAGE_IN, vv.VOYAGE_OUT);
                    this.fcita.InnerHtml = String.Format("<strong>{0}</strong>", vv.FECHA_CITA.HasValue ? vv.FECHA_CITA.Value.ToString("dd/MM/yyyy HH:mm") : "-");
                    if (v != null)
                    {
                        //de la visita activa
                        this.ata.InnerText = v.ATA.HasValue ? v.ATA.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                        this.atd.InnerText = v.ATD.HasValue ? v.ATD.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";

                        this.wini.InnerText = v.START_WORK.HasValue ? v.START_WORK.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                        this.wend.InnerText = v.END_WORK.HasValue ? v.END_WORK.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                        //de la visita activa en n4
                    }

                    this.rp_grua.DataSource = vv.Cranes;
                    var turno_sin = vv.Turns;

                    this.rp_grua.DataBind();

                    this.rp_grua.Visible = vv.Cranes.Count > 0;

                    sin_gruas.Visible = !this.rp_grua.Visible;

                    Session["vv"] = vv;
                }
                catch (Exception ex)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Page_Load", "Hubo un error al carga pagina de asignación",user!=null? user.loginname:"Sin Login"));
                    this.AbortResponse(sg);
                    return;
                }
            }
        }
        public static string testado(object opc_name )
        {
            var s = opc_name as string;
            if(string.IsNullOrEmpty(s))
            {
                return "<span class='disponible'>NO ASIGNADA</span>";
            }
            return string.Format("<span class='asignado'>{0}</span>", s.ToUpper());
        }
        //grabar en una variable el ruc de la opc logueada y compararla con cada fila
        public static string controles(object opc_id)
        {
            var o = opc_id as string;
            if (!string.IsNullOrEmpty(o))
            {
                return  !o.ToUpper().Equals("N") ?    "mostrar":"ocultar";
            }
            return "ocultar";
        }
        public static string toSet(object opc_id)
        {
            if (opc == null)
            {
                return "mostrar";
            }
            var o = opc_id as string;
            if (!string.IsNullOrEmpty(o))
            {
                return "ocultar";
            }
            return "mostrar";
        }
        //protected void rp_turno_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    string sg;
        //    user = Page.getUserBySesion();
        //    if (user == null)
        //    {
        //        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo el usuario de sesion"), "turno_opc", "Item_comand", "Hubo un error al remover la asignación", user!=null? user.loginname:"nologin"));
        //        this.Alerta(sg);
        //        return;
        //    }
        //    try
        //    {
        //        if (HttpContext.Current.Request.Cookies["token"] == null)
        //        {
        //            System.Web.Security.FormsAuthentication.SignOut();
        //            Session.Clear();
        //            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //            return;
        //        }
        //        var cs = e.CommandArgument as string;
        //        Int64 ca;
        //        if (!Int64.TryParse(cs, out ca))
        //        {
        //            sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo CommandArgument"), "turno_opc", "Item_comand", "Hubo un error al remover la asignación", user.loginname));
        //            this.Alerta(sg);
        //            return;
        //        }
        //        vv = Session["vv"] as Vessel_Visit;
        //        var t = vv.Turns.Find(d => d.id == ca);
        //        t.Mod_user = user.loginname;
        //        var r = t.LiberarTurno();
        //        if (!r.HasValue || r.Value <= 0)
        //        {
        //            sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Vessel Liberar turno Falló"), "turno_opc", "Item_comand", "Hubo un error al remover la asignación", user.loginname));
        //            this.Alerta(sg);
        //            return;
        //        }
        //        //en teoria volver a cargar.
        //        string er;
        //        if (!vv.LoadTurns(out er))
        //        {
        //            sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Vessel: Recargar turnos falló"), "turno_opc", "Item_comand", "Hubo un error al remover la asignación", user.loginname));
        //            this.Alerta(sg);
        //            return;
        //        }
        //        //rp_turno.DataSource = null;
        //        //rp_turno.DataSource = vv.Turns;
        //        //rp_turno.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover la asignación", user.loginname));
        //        this.Alerta(sg);
        //        return;
        //    }
        //}
        //protected void rp_turno_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    //sino son cabeceras o pies de tabla
        //    var vvv = vv == null ? Session["vv"] as Vessel_Visit : vv;
        //    if (vvv != null)
        //    {
        //        if (vvv.STATUS.Contains("P") || vvv.STATUS.Contains("C"))
        //        {
        //            if (e?.Item != null)
        //            {
        //                if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        //                {
        //                    var div = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("div_control");
        //                    if (div != null)
        //                    {
        //                        div.InnerHtml = string.Empty;
        //                        div.InnerHtml = "<span class='nodisponible'>No disponible</span>";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}