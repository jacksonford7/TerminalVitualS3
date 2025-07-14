using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;


namespace CSLSite.aisv
{
    public partial class editrefdata : System.Web.UI.Page
    {
        //AntiXRCFG
        app_start.TurnoPan tp;
        CSLSite.usuario user;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        private string sid = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, No autenticado", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "editrefdata", "Init", sid, Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }
            user = Page.Tracker();
            try
            {
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "editrefdata", "Init", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "editrefdata", "Page_Init", sid, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante el proceso de carga de datos, por favor repórtelo con este código: E00-{0}", number));
                base.Response.Write(close);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Response.IsClientConnected)
            {
                this.hro_id.InnerText = "Sin Programar";
                if (!IsPostBack)
                {
                    Session["turno"] = null;
                    //cargar los datos
                    populateDrop(dptipos, app_start.TurnoPan.RetornarTipos());
                    tp = app_start.TurnoPan.ObtenerData(sid);
                    this.statusdoc.InnerText = tp!=null && tp.activo ? " (Programado)" : " (No Programado)";
                    //tiene programacion
                    if (tp != null && !string.IsNullOrEmpty(tp.cntr))
                    {
                        //horario me permite bloquear.
                        //tipo es del combox
                        if (dptipos.Items.Count > 0)
                        {
                            if (dptipos.Items.FindByValue(tp.tipo_id.ToString()) != null)
                            {
                                dptipos.Items.FindByValue(tp.tipo_id.ToString()).Selected = true;
                            }
                        }
                        ckactivo.Checked = tp.activo;
                        this.nota.InnerText = tp.nota;
                        this.cntr_id.InnerText = tp.cntr;
                        this.aisv_id.InnerText = tp.aisv;
                        this.book_id.InnerText = tp.booking;
                        this.user_id.InnerText = tp.usuario;
                        this.fecha_id.InnerText = tp.fecha;
                        this.hro_id.InnerText = tp.horario;
                        this.expo_id.InnerText = tp.expoid;
                        this.name_id.InnerText = tp.exponame;
                        Session["turno"] = tp;
                        //tiene horario entonces no puede modificar
                        if(!string.IsNullOrEmpty(tp.horario))
                        {
                            dptipos.Enabled = false;
                            nota.Disabled = true;
                            ckactivo.Disabled = true;
                            bt_send.Enabled = false;
                            this.problema.InnerHtml = string.Format("El contenedor {0} ya tiene programación de inspección para el día {1} y no puede ser modificado.",tp.cntr,tp.horario);
                        }
                    }
                    else
                    {
                        //no tiene programación obtener datos desde el aisv;
                        var tabla = new Catalogos.AisvResultDataTable();
                        var ta = new CatalogosTableAdapters.AisvResultTableAdapter();
                        sid = sid.Trim().Replace("\0", string.Empty);
                        ta.Fill(tabla, sid);
                        if (tabla.Rows.Count <= 0)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "editrefdata", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                            this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: L00-{0}", number), null);
                            return;
                        }
                        var fila = tabla.FirstOrDefault();
                        this.cntr_id.InnerText = !fila.IscontainerNull() ? fila.container : "....";
                        this.aisv_id.InnerText = sid;
                        this.book_id.InnerText = !fila.IsbookinNull() ? fila.bookin : "....";
                        this.user_id.InnerText = user != null ? user.loginname : "....";
                        this.fecha_id.InnerText = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        this.expo_id.InnerText = fila.expoid;
                        this.name_id.InnerText = !fila.IsexponameNull()?fila.exponame:"...";
                        
                        //es un seco mantenga bloqueada la pantalla
                        if (!fila.IstempeNull() && fila.tempe == 0)
                        {
                            dptipos.Enabled = false;
                            nota.Disabled = true;
                            ckactivo.Disabled = true;
                            bt_send.Enabled = false;
                            this.problema.InnerHtml = string.Format("El contenedor {0} no requiere mantener la cadena de frío", tp.cntr);
                            return;
                        }
                    
                    }

                }
            }
        }
        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }

        protected void bt_send_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                var cbbal = dptipos.SelectedValue;
                if (dptipos.SelectedValue.Contains("**"))
                {
                    cbbal = "0";
                }

                user = Page.getUserBySesion();
                string log = string.Empty;
                if (user != null)
                {
                    log = user.loginname;
                }
                //si recupera al turno es porque es edicion
                tp = Session["turno"] as app_start.TurnoPan;
                if (tp != null)
                {
                    tp.nota = this.nota.InnerText.Trim(); //nota

                   
           

                    tp.tipo_id = ckactivo.Checked ? Convert.ToInt32(cbbal) : 0; //valor selecto
                    tp.activo = ckactivo.Checked; //activo
                    tp.usuario = log;
                    var pt = tp.Actualizar();
                    //diferente de 1 ok entonces error
                    if (pt != 1)
                    {
                        this.problema.InnerHtml = string.Format("Ha ocurrido la novedad n. {0}, favor comuniquese con CGSA", pt * -1);
                        return;
                    }
                }
                //nO RECUPERA EL TURNO ENTONCES ES NUEVO.
                else
                {
                 var po=   app_start.TurnoPan.salvar_refrigerado(this.cntr_id.InnerText.Trim(), this.book_id.InnerText.Trim(), this.aisv_id.InnerText.Trim(), Convert.ToInt32(dptipos.SelectedValue), log, this.expo_id.InnerText, this.name_id.InnerText);
                    if (po < 0)
                    {
                        this.problema.InnerHtml = string.Format("Ha ocurrido la novedad n. {0}, favor comuniquese con CGSA", po * -1);
                        return;
                    }
                }
                //ACTUALIZAR AISV-->SIEMPRE CON LO QUE EL TIPO REFRIGERADO SELECCIONA PERO SI LO ACTIVA.
                //FALTA SOLO ESTO
                app_start.TurnoPan.update_aisv(this.aisv_id.InnerText.Trim(), Convert.ToInt32(cbbal));
                this.ClientScript.RegisterStartupScript(GetType(), "key_close", "closeMe();", true);
            }
        }
    }
}