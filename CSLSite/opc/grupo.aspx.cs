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
using System.Threading;

namespace CSLSite
{
    public partial class grupo : System.Web.UI.Page
    {
        //AntiXRCFG
        Opc opc = null;
        OPCGrupo grp;
        string sid;
        usuario user;
        string sg;
        Int64 idgrupo = 0;
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
                    this.AbortResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
                }
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (!string.IsNullOrEmpty(sid))
                {
                    Int64.TryParse(sid, out idgrupo);
                    edit = true;
                }

                if (edit)
                {
                    Page.Title = "Editando Grupo";
                }
                else
                {
                    Page.Title = "Nuevo Grupo";
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
                        this.AbortResponse(string.Format("El RUC [{0}] no corresponde al de una OPC registrada, por favor comuníquese con credenciales y permisos", user.ruc));
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
                    this.operator_no.InnerText = opc.Nombre;
                    //cargar los datos guardados, recuperar toda la cuadrilla y ponerla en el repeater

                    populatecombo();

                    grp = new OPCGrupo();
                    //si esta editando
                    grp.id = edit ? idgrupo : 0;
                    if (edit)
                    {
                        //recargar la cabecera del grupo
                        if (!grp.PopulateMyData(out sg))
                        {
                            sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se pudo cargar grupo editado"), "grupo", "Page_Load", "Hubo un error al editar", user != null ? user.loginname : "Nologin"));
                            this.AbortResponse(sg);
                            return;
                        }
                        grp.PopulateMyOperators();
                        this.btsalvar.Text = "Actualizar";

                        rp_turno.DataSource = grp.Operadores.Where(o => o.active.HasValue && o.active.Value);
                        rp_turno.DataBind();

                        txtnume.Text = grp.grupo_name;

                        if (dpopc.Items.FindByValue(grp.opc_id) != null)
                        {
                            dpopc.SelectedIndex = -1;
                            dpopc.Items.FindByValue(grp.opc_id).Selected = true;
                        }
                    }
                    Session["grupo"] = grp;
                }
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "grupo", "Page_Load", "Hubo un error al cargar GrupoOPC", user != null ? user.loginname : "Nologin"));
                this.AbortResponse(sg);
                return;
            }

        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            try
            {
                user = Page.getUserBySesion();
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
                grp = Session["grupo"] as OPCGrupo;
                if (grp.Operadores == null)
                {
                    this.Alerta( "Fue imposible recuperar el Grupo");
                    return;
                }
                if (string.IsNullOrEmpty(txtnume.Text))
                {
                    txtnume.Text = grp.next_value(dpopc.SelectedValue);
                }
                //verificar que exista y que este activo
                if (grp.Operadores.Where(i => i.ope_id.Equals(this.identificacion.Value) && i.active.HasValue && i.active.Value).Count() > 0)
                {
                    this.Alerta(string.Format("Usuario con identificación {0} ya esta en la lista", this.identificacion.Value));
                    this.identificacion.Value = string.Empty;
                    return;
                }
                opc = Session["opc"] as Opc;
                if (opc == null)
                {
                    this.AbortResponse(string.Format("El RUC [{0}] no corresponde al de una OPC registrada, por favor comuníquese con credenciales y permisos", user.ruc));
                    return;
                }

                var nu = OPCOperador.OperarioEnGrupo(this.identificacion.Value);
                if (!nu.HasValue)
                {
                    this.Alerta("Hubo un problema al validar el operario en grupos");
                    return;
                }

                if (nu.Value)
                {
                    this.Alerta(string.Format("Usuario con identificación {0} ya se encuentra registrado en otro grupo", this.identificacion.Value));
                    this.identificacion.Value = string.Empty;
                    return;
                }



                var opr = new OPCOperador();
                opr.ope_id = this.identificacion.Value;
                opr.ope_nombre = this.nombres.Value;
                opr.ope_apellido = this.apellidos.Value;
                opr.Create_user = user.loginname;
                opr.active = true;
                grp.Operadores.Add(opr);
                Session["grupo"] = grp;
                this.apellidos.Value = this.nombres.Value = this.identificacion.Value = string.Empty;
                rp_turno.DataSource = grp.Operadores.Where(d=>d.active.HasValue && d.active.Value);
                rp_turno.DataBind();
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "grupo", "btAdd_Click", "Hubo un error al agregar operario a  lista", user != null ? user.loginname : "Nologin"));
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
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo el usuario de sesion"), "grupo", "Item_comand", "Hubo un error al remover la asignación",user!=null? user.loginname:"nologin"));
                this.Alerta(sg);
                return;
            }
            try
            {
                grp = Session["grupo"] as OPCGrupo;
                if (grp == null)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo Session[grupo]"), "grupo", "Item_comand", "Hubo un error al remover la asignación", user != null ? user.loginname : "Nologin"));
                    this.Alerta(sg);
                    return;
                }

                var ci = e.CommandArgument as string;
                if (string.IsNullOrEmpty(ci))
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se pudo obtener commandArgument"), "grupo", "Item_comand", "Hubo un error al remover la asignación", user!=null? user.loginname:"nologin"));
                    this.Alerta(sg);
                    return;
                }
                //No remover sino mas bien inactivar
                var operario = grp.Operadores.FirstOrDefault(f=>f.ope_id.Equals(ci));
                if (operario != null)
                {
                    operario.active = false;
                    operario.Create_user = user.loginname;
                }
                rp_turno.DataSource = grp.Operadores.Where(o=>o.active.HasValue && o.active.Value);
                rp_turno.DataBind();
                Session["grupo"] = grp;
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "grupo", "Item_comand", "Hubo un error general",user!=null? user.loginname:"Nologin"));
                this.Alerta(sg);
                return;
            }
        }
        protected string unir(object s1, object s2)
        {
            return string.Format("{1} {0}",s1,s2);
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo el usuario de sesion"), "turno_opc", "Item_comand", "Hubo un error al remover la asignación", user != null ? user.loginname : "nologin"));
                        this.Alerta(sg);
                        return;
                    }

                    grp = Session["grupo"] as OPCGrupo;
                    if (grp == null)
                    {
                        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo Session[turno_opc]"), "asignar_op", "Salvar_Click", string.Format("Hubo un error al grabar:{0}", idgrupo), user != null ? user.loginname : "Nologin"));
                        this.Alerta(sg);
                        return;
                    }
                    grp.Create_user = user.loginname;
                    grp.grupo_name = string.IsNullOrEmpty(txtnume.Text) ? "NA" : txtnume.Text;
                    grp.opc_id = dpopc.SelectedValue;
                    var si = grp.SaveTransaction(out sg);
                    if(!si.HasValue || si.Value <=0)
                    {
                        this.Alerta(sg);
                        return;
                    }
                    var s = string.Format(" alert('Éxito al {0}');",edit?"Actualizar":"Guardar");
                    if (!edit) { restart(); }
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", s, true);
                }
                catch (Exception ex)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "grupo", "btsalvar_Click", "Hubo un error al Guardar", user != null ? user.loginname : "Nologin"));
                    this.Alerta(sg);
                    return;
                }
            }
        }
        private void populatecombo()
        {
            //nuevo traer datos de OPC'S
            var li = Vessel_Operator.get_local_opcs();
            if (li == null)
            {
                this.PersonalResponse("Se ha producido una novedad cargando la lista de OPCs", "../cuenta/menu.aspx", true);
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
        }
        private void restart()
        {
            dpopc.SelectedIndex = -1;
            dpopc.Items.FindByValue("0").Selected = true;
            txtnume.Text = string.Empty;
            grp = new OPCGrupo();
            Session["grupo"] = grp;
            rp_turno.DataSource = grp.Operadores;
            rp_turno.DataBind();
        }
    }
}