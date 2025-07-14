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
using System.Text;
using ControlOPC.Entidades;
using System.Data;
using System.Globalization;

namespace CSLSite
{
    public partial class transaccionopc : System.Web.UI.Page
    {
        //
        private Vessel_Visit objVesselV = new Vessel_Visit();
        private Vessel_Crane objVesselCrane = new Vessel_Crane();
        private Crane_Turn objVesselTurno = new Crane_Turn();

        #region "Propiedades"

        public static string v_mensaje = string.Empty;

        private DataTable pDetalleGruas
        {
            get
            {
                return (DataTable)Session["DtDetalleGruas"];
            }
            set
            {
                Session["DtDetalleGruas"] = value;
            }

        }

        private DataTable pDetalleturnos
        {
            get
            {
                return (DataTable)Session["DtDetalleTurnos"];
            }
            set
            {
                Session["DtDetalleTurnos"] = value;
            }

        }

        #endregion

        #region "Metodos"

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }


        private void MessageBox(String msg, Page page)
        {
            StringBuilder s = new StringBuilder();
            msg = msg.Replace("'", " ");
            msg = msg.Replace(System.Environment.NewLine, " ");
            msg = msg.Replace("/", "");
            s.Append(" alertify.alert('").Append(msg).Append("');");
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);

        }

        private void Limpiar()
        {
            //Caja de Texto
            TxtReferencia.Text = null;
            TxtFechacita.Text = null;
            TxtFechaDesde.Text = "";
            TxtHorasTrabajo.Text = "";

            LblNombre.Text = null;
            LblViaje.Text = null;
            LblETA.Text = null;
            LblETD.Text = null;
            LblHoras.Text = null;
            LblVoyageIn.Text = null;
            LblVoyageOut.Text = null;

            CboGruas.DataSource = null;
            CboGruas.DataBind();

            Session["VesselV"] = null;

            TableGruas.DataSource = null;
            TableGruas.DataBind();

          
        }

        private void CargaGruas()
        {

            try
            {

                List<Crane> Lista = Crane.ListCrane();
                //DataTable dsRetorno = App_Extension.LINQToDataTable(Lista);
                if (Lista != null && Lista.Count  > 0)
                // if (dsRetorno != null && dsRetorno.Rows.Count > 0)
                {
                    this.CboGruas.DataSource = Lista;
                    CboGruas.DataBind();

                }
                else
                {
                    CboGruas.DataSource = null;
                    CboGruas.DataBind();
                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }

        }

        protected string jsarguments(object idGrua, object NumberTurno)
        {
            return string.Format("{0};{1}", idGrua != null ? idGrua.ToString().Trim() : "0", NumberTurno != null ? NumberTurno.ToString().Trim() : "0");
        }

        protected void RemoverGruas_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "RemoverGruas_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "RemoverGruas_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }
                    //
                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "RemoverGruas_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    int idGrua = int.Parse(xpars.ToString());
                    objVesselV = Session["VesselV"] as Vessel_Visit;

                    //elimina gruas
                    objVesselV.Cranes.Remove(objVesselV.Cranes.Where(p => p.Crane_Gkey == idGrua).FirstOrDefault());
                  

                    //salvar objeto
                    Session["VesselV"] = objVesselV;
                    //asignar
                    TableGruas.DataSource = objVesselV.Cranes;
                    TableGruas.DataBind();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de grúa, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al eliminar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }

        //protected void RemoverTurno_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {

        //           /* if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                Session.Clear();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                return;
        //            }*/

        //            var user = Page.getUserBySesion();
        //            if (user == null)
        //            {

        //                var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "RemoverTurno_ItemCommand", "No se pudo obtener usuario", "anónimo"));
        //                this.MessageBox(cMensaje2.ToString(), this);
        //                return;
        //            }

        //            if (e.CommandArgument == null)
        //            {

        //                var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "RemoverTurno_ItemCommand", "CommandArgument is NULL", user.loginname));
        //                this.MessageBox(cMensaje2.ToString(), this);
        //                return;
        //            }
        //            //
        //            var xpars = e.CommandArgument.ToString().Split(';');
        //            if (xpars.Length <= 0)
        //            {

        //                var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "RemoverTurno_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
        //                this.MessageBox(cMensaje2.ToString(), this);
        //                return;
        //            }

        //            int idGrua = int.Parse(xpars[0].ToString());
        //            int idNumberturno = int.Parse(xpars[1].ToString());
                    

        //            objVesselV = Session["VesselV"] as Vessel_Visit;
        //            objVesselV.Turns.Remove(objVesselV.Turns.Where(p => p.turno_number == idNumberturno && p.crane_id == idGrua).FirstOrDefault());

        //            Session["VesselV"] = objVesselV;

                  
        //        }
        //        catch (Exception ex)
        //        {
        //            var t = this.getUserBySesion();
        //            var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de turnos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al eliminar turno", t.loginname));
        //            this.MessageBox(cMensaje2.ToString(), this);

        //        }

        //    }
        //}

        #endregion






        //AntiXRCFG
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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            //this.IsAllowAccess();

            var user = Page.Tracker();
            if (user != null)
            {
               // this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
            }
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {

                var sp = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                string t = null;
                if (!string.IsNullOrEmpty(sp))
                {
                    t = CslHelper.getShiperName(sp);
                }
                //this.nomexpo.InnerText = t != null ? t : string.Format("{0} {1}", user.nombres, user.apellidos);
                //this.numexpo.InnerText = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                //this.numexport.Value = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Limpiar();

                CargaGruas();


                var oref = Request.QueryString["ID"] as string;
                if (!string.IsNullOrEmpty(oref))
                {

                    this.BtnBuscar.Enabled = false;
                    this.TxtReferencia.Enabled = false;
                }
   
            }
        }





        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                //bool lTieneFecha = true;
                string cMensaje = "";

                if (String.IsNullOrEmpty(this.TxtReferencia.Text) != false)
                {
                    this.MessageBox("Ingrese la referencia de la nave a buscar", this);
                    return;
                }

                List<Vessel> Lista = Vessel.ListaVessel(this.TxtReferencia.Text.Trim());
                var xList = Lista.FirstOrDefault();

                if (xList != null)
                {
                    //valido que la nave no exista en otra transaccion abierta
                    bool lExisteNave = Vessel_Visit.ReferenceHasActiveTransaction(this.TxtReferencia.Text.Trim(), out cMensaje).Value;
                    if (lExisteNave)
                    {
                        this.MessageBox(cMensaje, this);
                        this.LblNombre.Text = null;
                        this.LblETA.Text = null;
                        this.LblETD.Text = null;
                        this.LblViaje.Text = null;
                        this.LblVoyageIn.Text = null;
                        this.LblVoyageOut.Text = null;
                        return;
                    }

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objVesselV.ETA = xList.ETA;
                    objVesselV.ETD = xList.ETD;
                    objVesselV.NAME = xList.NAME;
                    objVesselV.VOYAGE_IN = xList.VOYAGE_IN;
                    objVesselV.VOYAGE_OUT = xList.VOYAGE_OUT;
                    objVesselV.ATA = xList.ATA;
                    objVesselV.ATD = xList.ATD;
                    objVesselV.Create_user = sUser.loginname;
                    objVesselV.Mod_user = String.Empty;
                    objVesselV.REFERENCE = xList.REFERENCE;
                    objVesselV.GKEY = xList.GKEY;
                    objVesselV.END_WORK = xList.END_WORK;
                    objVesselV.START_WORK = xList.START_WORK;

                    //salvar objeto
                    Session["VesselV"] = objVesselV;

                    this.LblNombre.Text = objVesselV.NAME;
                    this.LblETA.Text = objVesselV.ETA.HasValue ? objVesselV.ETA.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                    this.LblETD.Text = objVesselV.ETD.HasValue ? objVesselV.ETD.Value.ToString("dd/MM/yyyy HH:mm") : "...";  
                    this.LblViaje.Text = xList.VOYAGE.ToString();
                    this.LblVoyageIn.Text = objVesselV.VOYAGE_IN;
                    this.LblVoyageOut.Text = objVesselV.VOYAGE_OUT;

                    if (xList.ETD != null && xList.ETA != null)
                    {
                        var horas = (xList.ETD.Value - xList.ETA.Value).TotalHours;
                        this.LblHoras.Text = horas.ToString();
                    }
                    else { this.LblHoras.Text = "0"; }
                   

                }
                else
                {
                    this.LblNombre.Text = null;
                    this.LblETA.Text = null;
                    this.LblETD.Text = null;
                    this.LblViaje.Text = null;
                    this.LblVoyageIn.Text = null;
                    this.LblVoyageOut.Text = null;
                    this.MessageBox("No existe información de nave con los criterios de búsqueda ingresados", this);
                    return;
                }
                

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }

           
        }

        protected void CboGruas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

 
        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            try
            {
                //recuperar objeto
                objVesselV = Session["VesselV"] as Vessel_Visit;
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                DateTime dfechaDesde;
                DateTime dfechaCita;
                int nHorasTrabajo = 0;
                int nTotal = 0;
                Int32 IdGrua = 0;
                string NombreGrua = string.Empty;

                if (objVesselV == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "container", "ValidateJSON", "Sesión no existe", "No disponible");

                    this.MessageBox("Debe buscar la nave o referencia", this);
                    return;// CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Su sesión ha expirado, sera redireccionado a la página de login");
                }

                //si existen elementos ddel combo
                if (this.CboGruas.SelectedIndex == -1)
                {
                    this.MessageBox("Debe seleccionar la La grúa.", this);
                    return;
                }

                //valida datos requeridos
                if (Convert.ToDouble(this.LblHoras.Text == string.Empty ? "0" : this.LblHoras.Text) == 0)
                {
                    this.MessageBox("No existen horas de trabajo para la Nave" + this.LblNombre.Text, this);
                    return;
                }

                if (String.IsNullOrEmpty(this.TxtHorasTrabajo.Text) != false)
                {
                    this.TxtHorasTrabajo.Focus();
                    this.MessageBox("Debe ingresar las horas de trabajo", this);
                    return;
                }
                if (Convert.ToInt32(this.TxtHorasTrabajo.Text) == 0)
                {
                    this.TxtHorasTrabajo.Focus();
                    this.MessageBox("Debe ingresar las horas de trabajo", this);
                    return;
                }

                if (String.IsNullOrEmpty(this.TxtFechaDesde.Text) != false)
                {
                    this.TxtFechaDesde.Focus();
                    this.MessageBox("Ingrese una Fecha de trabajo de la grúa valida", this);
                    return;
                }
                if (String.IsNullOrEmpty(this.TxtFechacita.Text) != false)
                {
                    this.TxtHorasTrabajo.Focus();
                    this.MessageBox("Ingrese una Fecha para la cita valida", this);
                    return;
                }


                if (!DateTime.TryParseExact(this.TxtFechaDesde.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out dfechaDesde))
                {
                    this.TxtHorasTrabajo.Focus();
                    this.MessageBox("Ingrese una Fecha de trabajo para la grúa valida", this);
                    return;
                }
                if (!DateTime.TryParseExact(this.TxtFechacita.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out dfechaCita))
                {
                    this.TxtHorasTrabajo.Focus();
                    this.MessageBox("Ingrese una Fecha para la cita valida", this);
                    return;
                }

                /*  dfechaDesde = DateTime.Parse(this.TxtFechaDesde.Text);
              dfechaCita = DateTime.Parse(this.TxtFechacita.Text);*/

                objVesselV.FECHA_CITA = dfechaCita;

                //valida si ya fue ingresada
                IdGrua = Convert.ToInt32(this.CboGruas.SelectedValue);
                NombreGrua = this.CboGruas.SelectedItem.ToString();

                if (objVesselV.Cranes.Where(p => p.Crane_Gkey == IdGrua).Count() > 0)
                {
                    this.MessageBox("La grua ya fue ingresada", this);
                    return;
                }
                //valida si existe grua 
                bool lOtrodocumento = Crane.ValidateCraneActiveTransaction(IdGrua, out v_mensaje).Value;
                if (lOtrodocumento)
                {
                    this.MessageBox(v_mensaje, this);
                    return;
                }
                //valida total de horas trabajadas no se mayor al planificado
                int nTotalHoras = (from s in objVesselV.Cranes.AsEnumerable()
                                   select s.Crane_time_qty).Sum();

                Decimal nHoras = Decimal.Parse(this.LblHoras.Text);

                nHorasTrabajo = int.Parse(this.TxtHorasTrabajo.Text);
                nTotal = nTotalHoras + nHorasTrabajo;

                //if (nTotal > nHoras)
                //{
                //    string Mensaje = string.Format("El total de horas de trabajo de las grúas: {0:0.00} no puede mayor al total de horas de trabajo de la nave ", nTotal);
                //    this.MessageBox(Mensaje, this);
                //    return;
                //}


                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                objVesselCrane.Crane_Gkey = IdGrua;
                objVesselCrane.Crane_name = NombreGrua;
                objVesselCrane.Create_user = sUser.loginname;
                objVesselCrane.Mod_user = string.Empty;
                objVesselCrane.Crane_time_qty = nHorasTrabajo;
                objVesselCrane.DateWork = dfechaDesde;
               
                objVesselV.Cranes.Add(objVesselCrane);

                TableGruas.DataSource = objVesselV.Cranes;
                TableGruas.DataBind();
  
                //salvar objeto
                Session["VesselV"] = objVesselV;
               
              

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
      
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            try {
               
                //graba transaccion
                //recuperar objeto

                objVesselV = Session["VesselV"] as Vessel_Visit;

                if (objVesselV != null)
                {
                    if (objVesselV.Cranes.Count == 0)
                    {
                        this.MessageBox("Aún no ha agregado el detalle de las grúas", this);
                        return;
                    }

                 


                    objVesselV.SaveTransaction(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.MessageBox(v_mensaje.ToString(), this);
                    }
                    else
                    {
                        this.MessageBox("Se genero la transacción con éxito", this);
                        this.Limpiar();

                        string cId = securetext(objVesselV.ID.ToString());
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('plan_preview.aspx?sid=" + cId + "');", true);

                    }

                }
                else
                {
                    this.MessageBox("Aún no ha generado la consulta de la nave", this);
                }
              
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }





        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();

        }
    }
}