

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
    public partial class asignar_op_original : System.Web.UI.Page
    {
        //AntiXRCFG
        Opc opc = null;
        Crane_Turn crt = null;
        string sid;
        usuario user;
        string sg;
        bool edit = false;
  

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                    this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
                }

                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "QuerySegura", "DecryptQueryString", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }

                //modo edición.--->
                edit = !string.IsNullOrEmpty(Request.QueryString["op"]) && Request.QueryString["op"].Contains("e");

                if (edit)
                {
                    Page.Title = "Editando Asignación";
                }
                else
                {
                    Page.Title = "Asignando Operario";
                }

                //  this.IsAllowAccess();
                user = Page.Tracker();
                if (user != null)
                {
                    this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
                }
                if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
                {

                    opc = Opc.GetOPC(user.ruc);
                    //simhay problemas al traer el OPC.
                    if (opc == null)
                    {
                        this.PersonalResponse("Hubo un problema al cargar los datos de OPC, favor intente mas tarde", "csl/menu", true);
                        return;
                    }
                    Session["opc"] = opc;
                }
                if (!IsPostBack)
                {
                    this.IsCompatibleBrowser();
                    Page.SslOn();
                }
            }
            catch (Exception ex)
            {

                var t = this.getUserBySesion();
                var cMensaje2 = string.Format("Ha ocurrido la excepción #{0}", csl_log.log_csl.save_log<Exception>(ex, "asignar_opc", "Page_init", "Hubo un problema al init", t.loginname));
                this.PersonalResponse(cMensaje2);
            }



        }
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack && Response.IsClientConnected)
                {
                    dtlo.InnerHtml = dtlo.InnerText = string.Format("Estimado/a: {0}", opc.Nombre);

                    if (string.IsNullOrEmpty(sid))
                    {
                        this.PersonalResponse("Debe seleccionar un turno para el despliegue de datos", "csl/menu", true);
                        return;
                    }

                    //nuevo traer datos de OPC'S
                    var li = Vessel_Operator.get_local_opcs();
                    if (li == null)
                    {
                        this.PersonalResponse("Se ha producido una novedad cargando la lista de OPCs", "csl/menu", true);
                        return;
                    }
                    li.Add(new Opc() { GKey = -1, Ruc = "0", Nombre = "Seleccione" });

                    dpopc.DataSource = li;
                    dpopc.DataTextField = "Nombre";
                    dpopc.DataValueField = "Ruc";
                    dpopc.DataBind();
                    if (dpopc.Items.FindByValue("0") != null)
                    {
                        dpopc.Items.FindByValue("0").Selected = true;
                    }
                    ///fin combos de opc

                    //creo un turno para recuperar info
                    var cr = new Crane_Turn();
                    Int64 id;
                    //convierto el valor
                    if (!Int64.TryParse(sid, out id))
                    {
                        this.PersonalResponse("Debe seleccionar un turno para el despliegue de datos", "csl/menu", true);
                        return;
                    }
                    //seteo el valor
                    cr.id = id;
                    //obtengo la visita de este turno
                    var vv = cr.getVesselVisit();
                    if (vv == null)
                    {
                        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se pudo recuperar el vesel visit de este turno"), "asignar_op", "Page_load", string.Format("turno_id={0}", crt.id), user != null ? user.loginname : "Nologin"));
                        this.Alerta(sg);
                        return;
                    }




                    //cargar turnos
                    vv.LoadTurns();
                    //obtener este turno
                    cr = vv.Turns.Where(d => d.id == id).FirstOrDefault();

                    crt = cr;


                    this.referencia.InnerText = vv.REFERENCE;
                    this.operator_no.InnerText = opc.Nombre;
                    this.grua.InnerText = cr.crane_name;
                    this.turno.InnerHtml = string.Format("<span style='display:inline-block; background-color:Yellow;'><strong>Desde: {0} | Hasta:{1}</strong></span>", cr.turn_time_start.Value.ToString("dd/MM/yyyy HH:mm"), cr.turn_time_end.Value.ToString("dd/MM/yyyy HH:mm"));
                    //cargar los datos guardados, recuperar toda la cuadrilla y ponerla en el repeater
                    if (edit)
                    {
                        //cargar el repeater con los turnos actuales
                        var lop = Vessel_Operator.ListVesselOperators(cr.id, out sg);
                        rp_turno.DataSource = lop;
                        if (!string.IsNullOrEmpty(sg))
                        {
                            //error al editar
                            this.PersonalResponse(sg);
                            return;

                        }
                        this.btsalvar.Text = "Actualizar";
                        rp_turno.DataBind();
                        cr.turn_operators = null;
                        cr.turn_operators = lop;
                        vlock.Checked = cr.vlock;
                        vunlock.Checked = cr.vunlock;

                        if (dpopc.Items.FindByValue(cr.opc_id) != null)
                        {
                            dpopc.SelectedItem.Selected = false;
                            dpopc.Items.FindByValue(cr.opc_id).Selected = true;
                        }
                    }

                    //ponga el turno en sesion
                    Session["turno_opc"] = cr;
                }
            }
            catch (Exception ex)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Page_Load", "Hubo un error al agregar operario", user != null ? user.loginname : "Nologin"));
                this.PersonalResponse(sg);
                return;
            }

        }
        protected void btAdd_Click(object sender, EventArgs e)
        {
            try
            {

                if (dpopc.SelectedValue.Equals("0"))
                {
                    this.Alerta("Por favor primero seleccione la operadora de esta cuadrilla");
                    return;
                }


                if (string.IsNullOrEmpty(this.identificacion.Value))
                {
                    this.Alerta("Debe seleccionar al operario de la lista");
                    return;
                }
                //aqui se añade
                crt = Session["turno_opc"] as Crane_Turn;
                if (crt == null)
                {
                    this.Alerta( "Fue imposible recuperar el turno");
                    return;
                }

                if (!edit)
                {
                    if (crt.IsSetted())
                    {
                        var xs = " alert('Lamentablemente este turno ya fue modificado por otra OPC, la ventana se cerrará'); window.close();window.opener.location.href = window.opener.location.href;";
                        ClientScript.RegisterStartupScript(typeof(Page), "closePage_Opc", xs, true);
                    }
                }

                //verificar que exista
                if (crt.turn_operators.Where(i => i.operator_id.Equals(this.identificacion.Value)).Count() > 0)
                {
                    this.Alerta(string.Format("Usuario con identificación {0} ya esta en la lista", this.identificacion.Value));
                    return;
                }
                opc = Session["opc"] as Opc;
                if (opc == null)
                {
                    this.Alerta("Fue imposible recuperar el OPC");
                    return;
                }
                user = Page.getUserBySesion();
                var opr = new Vessel_Operator();
                opr.turn_id = crt.id;
                opr.opc_id = dpopc.SelectedValue;
                opr.opc_name = dpopc.SelectedItem.ToString();
                opr.operator_id = this.identificacion.Value;
                opr.operator_names = this.nombres.Value;
                opr.operator_apellidos = this.apellidos.Value;
                opr.description = string.Empty;
                opr.Create_user = user.loginname;
               
                string salir;
                if (Vessel_Operator.HasActiveTurns(opr.operator_id, crt.turn_time_start.Value, crt.turn_time_end.Value, out salir))
                {
                    this.Alerta(string.Format("{0}", salir));
                    return;
                }
                crt.turn_operators.Add(opr);
                Session["turno_opc"] = crt;
                //blankeo campos
                this.apellidos.Value = this.nombres.Value = this.identificacion.Value = string.Empty;
                rp_turno.DataSource = crt.turn_operators;
                rp_turno.DataBind();
                
                
                //hizo cambios SI->POR LO TANTO BLANQUEO DE LISTADO
                ViewState["cambios"] = true;
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "btAdd_Click", "Hubo un error al agregar operario", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }
        }
        protected void rp_turno_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string sg;
            user = Page.getUserBySesion();
            if (user == null)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo el usuario de sesion"), "turno_opc", "Item_comand", "Hubo un error al remover la asignación",user!=null? user.loginname:"nologin"));
                this.Alerta(sg);
                return;
            }
            try
            {
                crt = Session["turno_opc"] as Crane_Turn;
                var ci = e.CommandArgument as string;
                if (string.IsNullOrEmpty(ci))
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se pudo obtener commandArgument"), "asignar_op", "Item_comand", "Hubo un error al remover operario",user!=null? user.loginname:"nologin"));
                    this.Alerta(sg);
                    return;
                }
                crt.turn_operators.Remove(crt.turn_operators.Where(o => o.operator_id.Equals(ci)).FirstOrDefault());
                rp_turno.DataSource = crt.turn_operators;
                rp_turno.DataBind();
                Session["turno_opc"] = crt;

                //removió usuarios->SI, por lo tanto hay que limpiar las lista
                ViewState["cambios"] = true;
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover al item comand",user!=null? user.loginname:"Nologin"));
                this.Alerta(sg);
                return;
            }
        }
        protected string join(object s1, object s2)
        {
            return string.Format("{1} {0}",s1,s2);
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                   crt = Session["turno_opc"] as Crane_Turn;
                    if (crt == null)
                    {
                        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo Session[turno_opc]"), "asignar_op", "Salvar_Click", string.Format("Hubo un error al grabar:{0}", crt.id), user != null ? user.loginname : "Nologin"));
                        this.Alerta(sg);
                        return;
                    }
                    if (vlock.Checked || vunlock.Checked)
                    {
                        //verifique amarre /desamarre
                        var tr = Crane_Turn.LockUnlockTurns(crt.id);
                        var ta = tr.Where(t => t.vlock == true).FirstOrDefault();
                        var td = tr.Where(t => t.vunlock == true).FirstOrDefault();
                        //amarre
                        if (vlock.Checked  && ta!=null)
                        {
                            sg = string.Format("Ya existe turno con operación de amarre [desde: {0}- hasta: {1}]",ta.turn_time_start.Value.ToString("dd/MM/yyyy HH:mm"), ta.turn_time_end.Value.ToString("dd/MM/yyyy HH:mm"));
                            this.Alerta(sg);
                            return;
                        } 
                        //desamarre
                        if (vunlock.Checked && td!=null)
                        {
                            sg = string.Format("Ya existe turno con operación de des-amarre [desde: {0}- hasta: {1}]", td.turn_time_start.Value.ToString("dd/MM/yyyy HH:mm"), td.turn_time_end.Value.ToString("dd/MM/yyyy HH:mm"));
                            this.Alerta(sg);
                            return;
                        }
                    }
                    if (!edit)
                    {
                        if (crt.IsSetted())
                        {
                            var xs = " alert('Lamentablemente este turno ya fue modificado por otra OPC, la ventana se cerrará'); window.close();window.opener.location.href = window.opener.location.href;";
                            ClientScript.RegisterStartupScript(typeof(Page), "closePage_Opc", xs, true);
                        }
                    }
                    //remover todos los cuadrilleros de 1 turno.
                   var remo= crt.RemoveMyVesselOperators(out sg);
                    if (!remo.HasValue)
                    {
                        this.Alerta(sg);
                        return;
                    }
                    //grabar los cambios, los nuevos vessel_op y los amarre/desamarre
                    var b = crt.SaveMyVesselOperators(out sg,vlock.Checked,vunlock.Checked);
                    if (b == null || !b.Value)
                    {
                        this.Alerta(sg);
                        return;
                    }
                    var s = string.Format(" alert('Éxito al {0}'); window.close();window.opener.location.href = window.opener.location.href;",edit?"Actualizar":"Guardar");
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", s, true);
                }
                catch (Exception ex)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover al Salvar", user != null ? user.loginname : "Nologin"));
                    this.Alerta(sg);
                    return;
                }
            }
        }
    }
}