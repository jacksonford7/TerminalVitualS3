

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
using System.Globalization;

namespace CSLSite
{
    public partial class asignar_op : System.Web.UI.Page
    {
        //AntiXRCFG
        Opc opc = null;
        Crane_Turn crt = null;
        string sid;
        usuario user;
        string sg;
        bool edit = false;
        string cError = string.Empty;

        private Vessel_Visit objVesselV = new Vessel_Visit();
        private Vessel_Crane objVesselCrane = new Vessel_Crane();
        private Crane_Turn objVesselTurno = new Crane_Turn();

        private void LlenaGrupos(string pOPC)
        {

            try
            {

                CboGrupos.Items.Clear();

                List<OPCGrupo> Lista = OPCGrupo.ListGrupos(pOPC, out cError);

                if (Lista != null && Lista.Count > 0)
               {
                    CboGrupos.DataSource = Lista;
                    CboGrupos.DataTextField = "grupo_name";
                    CboGrupos.DataValueField = "id";
                    CboGrupos.DataBind();

                }
                else
                {
                    CboGrupos.DataSource = null;
                    CboGrupos.DataBind();
                }
            }
            catch (Exception exc)
            {
                this.Alerta(exc.Message);  
            }


           
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        private void Carga_turnos()
        {
            try
            {
                this.TxtFechaDesde.Text = null;
                this.TxtFechaHasta.Text = null;
                this.vlock.Text = null;
                this.vunlock.Text = null;

                    objVesselV = Session["VesselV"] as Vessel_Visit;
                    objVesselCrane = Session["objVesselCrane"] as Vessel_Crane;

                    if (objVesselV != null)
                    {

                   
                    
                        objVesselV.Turns.Clear();
                    

                        List<Crane_Turn> ListaTurnoOPC = Crane_Turn.ListaTurnoOC(objVesselV.ID, objVesselCrane.Id, this.dpopc.SelectedValue);
                        foreach (Crane_Turn Lista in ListaTurnoOPC)
                        {

                            objVesselTurno = new Crane_Turn();
                            objVesselTurno.id = Lista.id;
                            objVesselTurno.vessel_visit_id = objVesselV.ID;
                            objVesselTurno.turno_number = Lista.turno_number;
                            objVesselTurno.crane_id = Lista.crane_id;
                            objVesselTurno.vessel_crane_id = Lista.vessel_crane_id;
                            objVesselTurno.turn_time_start = Lista.turn_time_start; ;
                            objVesselTurno.turn_time_end = Lista.turn_time_end; ;
                            objVesselTurno.turn_time_meet = Lista.turn_time_start; ;
                            objVesselTurno.crane_name = Lista.crane_name;
                            objVesselTurno.Create_user = Lista.Create_user;
                            objVesselTurno.Mod_user = Lista.Mod_user;
                            objVesselTurno.grupo_id = Lista.grupo_id;
                            objVesselTurno.grupo_name = Lista.grupo_name;
                            objVesselTurno.opc_id = Lista.opc_id;
                            objVesselTurno.opc_name = Lista.opc_name;
                            objVesselTurno.vlock = Lista.vlock;
                            objVesselTurno.vunlock = Lista.vunlock;
                            objVesselTurno.vlock_text = Lista.vlock_text;
                            objVesselTurno.vunlock_text = Lista.vunlock_text;
                            objVesselV.Turns.Add(objVesselTurno);

                        }

                        Session["objVesselCrane"] = objVesselCrane;
                        Session["VesselV"] = objVesselV;

                        TableTurnos.DataSource = objVesselV.Turns;
                        TableTurnos.DataBind();

                    }
    
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover al item comand", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                    this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
                }

                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "QuerySegura", "DecryptQueryString", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }


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

                    Session["objVesselCrane"] = null;
                    Session["VesselV"] = null;

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
                        this.PersonalResponse("Debe seleccionar un turno para el despliegue de datos", "../cuenta/menu.aspx", true);
                        return;
                    }

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

                    this.LlenaGrupos(dpopc.SelectedValue);
                    ///fin combos de opc
                    ///

                    /*llena datos de cabecera*/
                    sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);

                    Int64 id = 0;
                    sid = sid.Trim().Replace("\0", string.Empty);

                    if (!Int64.TryParse(sid, out id))
                    {
                        var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, sid no es numerico", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        var number = log_csl.save_log<Exception>(ex, "plapreview", "Page_Load", sid == null ? "sid is no es numerico" : sid, User.Identity.Name);
                        this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                        return;
                    }

                    /*datos unicos de la grua*/
                    objVesselCrane = new Vessel_Crane();
                    this.TxtIdGrua.Text = id.ToString();
                    objVesselCrane.Id = id;
                    objVesselCrane.PopulateMyData(out cError);
                    if (!objVesselCrane.PopulateMyData(out cError))
                    {
                        var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        var number = log_csl.save_log<Exception>(ex, "plapreview", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                        this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                        return;
                    }
                    objVesselV = new Vessel_Visit(objVesselCrane.VesselVisit_ID);
                    //recupero el objeto
                    if (!objVesselV.PopulateMyData(out cError))
                    {
                        var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        var number = log_csl.save_log<Exception>(ex, "plapreview", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                        this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                        return;
                    }

                    this.buque.InnerText = objVesselV.NAME;
                    this.idreferencia.InnerText = objVesselV.REFERENCE;
                    this.eta.InnerText = objVesselV.ETA.HasValue ? objVesselV.ETA.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                    this.etd.InnerText = objVesselV.ETD.HasValue ? objVesselV.ETD.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                    this.vio.InnerText = String.Format("{0}/{1}", objVesselV.VOYAGE_IN, objVesselV.VOYAGE_OUT);
                    this.fcita.InnerHtml = String.Format("<strong>{0}</strong>", objVesselV.FECHA_CITA.HasValue ? objVesselV.FECHA_CITA.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido");
                    this.grua.InnerHtml = String.Format("<strong>{0}</strong>", objVesselCrane.Crane_name != string.Empty ? objVesselCrane.Crane_name : "No establecido");
                    this.operator_no.InnerHtml = String.Format("<strong>{0}</strong>", opc.Nombre != string.Empty ? opc.Nombre : "No establecido");

                    this.ata.InnerText = objVesselV.ATA.HasValue ? objVesselV.ATA.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                    this.atd.InnerText = objVesselV.ATD.HasValue ? objVesselV.ATD.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                    this.wini.InnerText = objVesselV.START_WORK.HasValue ? objVesselV.START_WORK.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";
                    this.wend.InnerText = objVesselV.END_WORK.HasValue ? objVesselV.END_WORK.Value.ToString("dd/MM/yyyy HH:mm") : "No establecido";

                    /*fin de datos de cabecera*/

                    Session["objVesselCrane"] = objVesselCrane;
                    Session["VesselV"] = objVesselV;

                   
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

                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");

                if (objVesselV == null)
                {
                    this.Alerta("Error, no existe estructura de turnos para agregar (objVesselV)");
                    return;
                }

                if (dpopc.SelectedValue.Equals("0"))
                {
                    this.Alerta("Por favor primero seleccione la operadora de esta cuadrilla");
                    return;
                }
                if (CboGrupos.SelectedValue == string.Empty )
                {
                    this.Alerta("Por favor debe seleccionar el grupo de la cuadrilla");
                    return;
                }


                if (String.IsNullOrEmpty(this.TxtFechaDesde.Text) != false)
                {
                    this.TxtFechaDesde.Focus();
                    this.Alerta("Ingrese una Fecha inicial del turno");
                    return;
                }
                if (String.IsNullOrEmpty(this.TxtFechaHasta.Text) != false)
                {
                    this.TxtFechaHasta.Focus();
                    this.Alerta("Ingrese una Fecha final del turno");
                    return;
                }
#if DEBUG
                if (vlock.Checked && vunlock.Checked)
                {
                    this.Alerta("No puede existir un turno con ambos estados (Amarre y Desamarre)");
                    return;
                }
#endif
                DateTime dfechaDesde;
                DateTime dfechaHasta;
                Int32 nTurno_Number = 1;
               /* dfechaDesde = DateTime.Parse(this.TxtFechaDesde.Text);
                dfechaHasta = DateTime.Parse(this.TxtFechaHasta.Text);*/


                if (!DateTime.TryParseExact(this.TxtFechaDesde.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out dfechaDesde))
                {
                    this.TxtFechaDesde.Focus();
                    this.Alerta("Ingrese una Fecha inicial de trabajo  valida");
                    return;
                }
                if (!DateTime.TryParseExact(this.TxtFechaHasta.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out dfechaHasta))
                {
                    this.TxtFechaHasta.Focus();
                    this.Alerta("Ingrese una Fecha final de trabajo valida");
                    return;
                }


                if (dfechaHasta < dfechaDesde)
                {
                     this.Alerta("La fecha de inicio de turno no puede ser mayor a la fecha final de turno ");
                     return ;
                }
               
                user = Page.getUserBySesion();

                objVesselCrane = Session["objVesselCrane"] as Vessel_Crane;
                objVesselV = Session["VesselV"] as Vessel_Visit;


                if (dfechaDesde < objVesselV.FECHA_CITA)
                {
                    this.TxtFechaDesde.Focus();
                    this.Alerta("El turno inicial no puede ser menor a la fecha de la citación");
                    return;
                }
                if (dfechaHasta < objVesselV.FECHA_CITA)
                {
                    this.TxtFechaDesde.Focus();
                    this.Alerta("El turno final no puede ser menor a la fecha de la citación");
                    return;
                }

                //if (objVesselV.Turns.Where(i => dfechaDesde >= i.turn_time_start.Value && dfechaDesde <= i.turn_time_start.Value).Count() > 0)
                //{
                //    this.Alerta("el turno inicial ya se encuentra registrado");
                //    return;
                //}
                //if (objVesselV.Turns.Where(i => dfechaHasta  >= i.turn_time_end.Value &&   dfechaHasta <=  i.turn_time_end.Value).Count() > 0)
                //{
                //    this.Alerta("el turno final ya se encuentra registrado");
                //    return;
                //}

                objVesselTurno = new Crane_Turn();
                objVesselTurno.id = 0;
                objVesselTurno.vessel_visit_id = objVesselV.ID;
                nTurno_Number = objVesselV.Turns.Count + 1;
                objVesselTurno.turno_number = nTurno_Number;
                objVesselTurno.crane_id = objVesselCrane.crane_id;
                objVesselTurno.vessel_crane_id = objVesselCrane.Id;
                objVesselTurno.turn_time_start = dfechaDesde;
                objVesselTurno.turn_time_end = dfechaHasta;
                objVesselTurno.turn_time_meet = dfechaDesde;
                objVesselTurno.crane_name = objVesselCrane.Crane_name;
                objVesselTurno.Create_user = user.loginname;
                objVesselTurno.Mod_user = string.Empty;
                objVesselTurno.grupo_id = int.Parse(CboGrupos.SelectedValue);
                objVesselTurno.grupo_name = CboGrupos.SelectedItem.ToString();
                objVesselTurno.opc_id = this.dpopc.SelectedValue;
                objVesselTurno.opc_name = this.dpopc.SelectedItem.ToString();
                objVesselTurno.vlock = vlock.Checked;
                objVesselTurno.vunlock = vunlock.Checked;
                objVesselTurno.vlock_text = (vlock.Checked ? "Amarre" : "") ;
                objVesselTurno.vunlock_text = (vunlock.Checked ? "Desamarre" : "");
                objVesselV.Turns.Add(objVesselTurno);

                objVesselTurno.Save(out cError);
                if (cError != string.Empty)
                {
                    objVesselV.Turns.Remove(objVesselV.Turns.Where(p => p.turno_number == nTurno_Number).FirstOrDefault());
                    this.Alerta(cError);
                }

                //salvar objeto
                Session["objVesselCrane"] = objVesselCrane;
                Session["VesselV"] = objVesselV;

          
                this.TxtFechaDesde.Text = null;
                this.TxtFechaHasta.Text = null;
                this.vlock.Checked = false;
                this.vunlock.Checked = false;

                this.Carga_turnos();


            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "btAdd_Click", "Hubo un error al agregar operario", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }
        }
        //protected void rp_turno_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    string sg;
        //    user = Page.getUserBySesion();
        //    if (user == null)
        //    {
        //        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo el usuario de sesion"), "turno_opc", "Item_comand", "Hubo un error al remover la asignación",user!=null? user.loginname:"nologin"));
        //        this.Alerta(sg);
        //        return;
        //    }
        //    try
        //    {
        //        crt = Session["turno_opc"] as Crane_Turn;
        //        var ci = e.CommandArgument as string;
        //        if (string.IsNullOrEmpty(ci))
        //        {
        //            sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se pudo obtener commandArgument"), "asignar_op", "Item_comand", "Hubo un error al remover operario",user!=null? user.loginname:"nologin"));
        //            this.Alerta(sg);
        //            return;
        //        }
        //        crt.turn_operators.Remove(crt.turn_operators.Where(o => o.operator_id.Equals(ci)).FirstOrDefault());
        //        rp_turno.DataSource = crt.turn_operators;
        //        rp_turno.DataBind();
        //        Session["turno_opc"] = crt;

        //        //removió usuarios->SI, por lo tanto hay que limpiar las lista
        //        ViewState["cambios"] = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover al item comand",user!=null? user.loginname:"Nologin"));
        //        this.Alerta(sg);
        //        return;
        //    }
        //}
        protected string join(object s1, object s2)
        {
            return string.Format("{1} {0}",s1,s2);
        }
        //protected void btsalvar_Click(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {
        //           crt = Session["turno_opc"] as Crane_Turn;
        //            if (crt == null)
        //            {
        //                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo Session[turno_opc]"), "asignar_op", "Salvar_Click", string.Format("Hubo un error al grabar:{0}", crt.id), user != null ? user.loginname : "Nologin"));
        //                this.Alerta(sg);
        //                return;
        //            }
        //            if (vlock.Checked || vunlock.Checked)
        //            {
        //                //verifique amarre /desamarre
        //                var tr = Crane_Turn.LockUnlockTurns(crt.id);
        //                var ta = tr.Where(t => t.vlock == true).FirstOrDefault();
        //                var td = tr.Where(t => t.vunlock == true).FirstOrDefault();
        //                //amarre
        //                if (vlock.Checked  && ta!=null)
        //                {
        //                    sg = string.Format("Ya existe turno con operación de amarre [desde: {0}- hasta: {1}]",ta.turn_time_start.Value.ToString("dd/MM/yyyy HH:mm"), ta.turn_time_end.Value.ToString("dd/MM/yyyy HH:mm"));
        //                    this.Alerta(sg);
        //                    return;
        //                } 
        //                //desamarre
        //                if (vunlock.Checked && td!=null)
        //                {
        //                    sg = string.Format("Ya existe turno con operación de des-amarre [desde: {0}- hasta: {1}]", td.turn_time_start.Value.ToString("dd/MM/yyyy HH:mm"), td.turn_time_end.Value.ToString("dd/MM/yyyy HH:mm"));
        //                    this.Alerta(sg);
        //                    return;
        //                }
        //            }
        //            if (!edit)
        //            {
        //                if (crt.IsSetted())
        //                {
        //                    var xs = " alert('Lamentablemente este turno ya fue modificado por otra OPC, la ventana se cerrará'); window.close();window.opener.location.href = window.opener.location.href;";
        //                    ClientScript.RegisterStartupScript(typeof(Page), "closePage_Opc", xs, true);
        //                }
        //            }
        //            //remover todos los cuadrilleros de 1 turno.
        //           var remo= crt.RemoveMyVesselOperators(out sg);
        //            if (!remo.HasValue)
        //            {
        //                this.Alerta(sg);
        //                return;
        //            }
        //            //grabar los cambios, los nuevos vessel_op y los amarre/desamarre
        //            var b = crt.SaveMyVesselOperators(out sg,vlock.Checked,vunlock.Checked);
        //            if (b == null || !b.Value)
        //            {
        //                this.Alerta(sg);
        //                return;
        //            }
        //            var s = string.Format(" alert('Éxito al {0}'); window.close();window.opener.location.href = window.opener.location.href;",edit?"Actualizar":"Guardar");
        //            ClientScript.RegisterStartupScript(typeof(Page), "closePage", s, true);
        //        }
        //        catch (Exception ex)
        //        {
        //            sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover al Salvar", user != null ? user.loginname : "Nologin"));
        //            this.Alerta(sg);
        //            return;
        //        }
        //    }
        //}

        protected void dpopc_SelectedIndexChanged(object sender, EventArgs e)
        {

            try {

                if (this.dpopc.SelectedIndex != -1)
                {
                    LlenaGrupos(dpopc.SelectedValue);

                    this.Carga_turnos();

                }
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover al item comand", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }
           
           
        }

        protected void RemoverTurno_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {

                if (this.dpopc.SelectedIndex != -1)
                {
                   

                    objVesselV = Session["VesselV"] as Vessel_Visit;
                    objVesselCrane = Session["objVesselCrane"] as Vessel_Crane;

                    if (objVesselV != null)
                    {

                        if (objVesselV.Turns.Count != 0)
                        {
                            var result = (from t in objVesselV.Turns
                                          where t.turno_number != 0
                                          orderby t.turno_number descending
                                          select t.id).First();

                            if (result != 0)
                            {
                                long nId = result;
                                user = Page.getUserBySesion();

                                objVesselTurno.id = nId;
                                objVesselTurno.Mod_user = user.loginname;
                                objVesselTurno.EliminarTurnoOPC(out cError);

                                if (cError != string.Empty)
                                {
                                    this.Alerta(cError);
                                }

                                objVesselV.Turns.Remove(objVesselV.Turns.Where(p => p.id == nId).FirstOrDefault());

                                Session["objVesselCrane"] = objVesselCrane;
                                Session["VesselV"] = objVesselV;

                                TableTurnos.DataSource = objVesselV.Turns;
                                TableTurnos.DataBind();


                            }
                        }
                        

                    }

                }
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "turno_opc", "Item_comand", "Hubo un error al remover al item comand", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }
        }
    }
}