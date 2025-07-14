using ControlOPC.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.opc
{
    public partial class generar_proforma_adicional : System.Web.UI.Page
    {


        #region "Propiedades"

        private ProformaCab obProformaCab = new ProformaCab();
        private ProformaDet obProformaDet = new ProformaDet();
        private Vessel_Visit objVesselV = new Vessel_Visit();


        public static string v_mensaje = string.Empty;

        private DataTable pDetalleVesselVisit
        {
            get
            {
                return (DataTable)Session["DtDetalleVesselVisit"];
            }
            set
            {
                Session["DtDetalleVesselVisit"] = value;
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
            LblNombre.Text = null;
            LblViaje.Text = null;
            LblETA.Text = null;
            LblETD.Text = null;
            LblHoras.Text = null;

            this.LblProveedor.Text = null;
            this.LblRucProv.Text = null;
            this.LblFechaProforma.Text = null;
            this.TxtCantidad.Text = null;
            this.TxtPrecio.Text = null;
            this.TxtGlosa.Text = null;

          //  this.LblFechaCita.Text = null;
            this.LblTotalProforma.Text = null;

            Session["ProformaCab"] = null;

            TablePendientes.DataSource = null;
            TablePendientes.DataBind();

            TableProformas.DataSource = null;
            TableProformas.DataBind();

        }

        private void CargaConceptos()
        {

            try
            {

                List<Concepto> Lista = Concepto.ListConceptos(out v_mensaje);
                //DataTable dsRetorno = App_Extension.LINQToDataTable(Lista);

                if (Lista != null && Lista.Count > 0)
                //if (dsRetorno != null && dsRetorno.Rows.Count > 0)
                {
                    this.CboConcepto.DataSource = Lista;
                    CboConcepto.DataBind();

                }
                else
                {
                    CboConcepto.DataSource = null;
                    CboConcepto.DataBind();
                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }

        }

        protected void RemoverConceptos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    /* if (HttpContext.Current.Request.Cookies["token"] == null)
                     {
                         System.Web.Security.FormsAuthentication.SignOut();
                         Session.Clear();
                         System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                         return;
                     }*/

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "RemoverConceptos_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "RemoverConceptos_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }
                    //
                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "RemoverConceptos_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }


                    int Linea = int.Parse(xpars.ToString());

                    //recuperar objeto
                    obProformaCab = Session["ProformaCab"] as ProformaCab;

                    //elimina gruas
                    obProformaCab.ProformaDetalle.Remove(obProformaCab.ProformaDetalle.Where(p => p.Line == Linea).FirstOrDefault());

                    var ntotal = obProformaCab.ProformaDetalle.Sum(p => p.Total);
                    this.LblTotalProforma.Text = ntotal.ToString();

                    //salvar objeto
                    Session["ProformaCab"] = obProformaCab;

                    //asignar
                    TableProformas.DataSource = obProformaCab.ProformaDetalle;
                    TableProformas.DataBind();


                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de un concepto, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al eliminar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }


        protected void Opciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Option rowOption = (Option)e.Item.DataItem;
                Label lbl = (Label)e.Item.FindControl("LblTipo");
                if (lbl.Text == "Adicional")
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("LblIdProf")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblFechaProf")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblUsuarioProf")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblBuque")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblRuc")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblProveedor")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblHoras")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblTotal")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("LblTipo")).ForeColor = System.Drawing.Color.Green;
                }

            }


        }


        #endregion

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
            this.IsAllowAccess();

            var user = Page.Tracker();
            if (user != null)
            {
              
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
                CargaConceptos();
                CboConcepto_SelectedIndexChanged(sender, e);

            }
        }

        //metodo generalizado para controlar el error de esta clase.
        public static string ControlError(string mensaje)
        {
            //paselo a la pantalla una vez controlado y guardado.
            return " mensaje:" + mensaje + ", resultado:false ";
        }

       

        protected string url(object _referece)
        {
            return string.Format("<a href='transaccionopc.aspx?ID={0}' target='_blank'>Imprimir</a>", _referece);
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //bool lTieneFecha = true;
                string cMensaje = "";

                if (String.IsNullOrEmpty(this.TxtReferencia.Text) != false)
                {
                    this.MessageBox("Ingrese la referencia del buque a buscar", this);
                    return;
                }

                List<Vessel> Lista = Vessel.ListaVessel(this.TxtReferencia.Text.Trim());
                var xList = Lista.FirstOrDefault();

                if (xList != null)
                {
                   
                    objVesselV.ETA = xList.ETA;
                    objVesselV.ETD = xList.ETD;
                    objVesselV.NAME = xList.NAME;
                    objVesselV.VOYAGE_IN = xList.VOYAGE_IN;
                    objVesselV.VOYAGE_OUT = xList.VOYAGE_OUT;
                    objVesselV.ATA = xList.ATA;
                    objVesselV.ATD = xList.ATD;
                    objVesselV.REFERENCE = xList.REFERENCE;
                    objVesselV.GKEY = xList.GKEY;
                    objVesselV.END_WORK = xList.END_WORK;
                    objVesselV.START_WORK = xList.START_WORK;
                  
                    this.LblNombre.Text = objVesselV.NAME;
                    //this.LblETA.Text = objVesselV.ETA.ToString();
                    //this.LblETD.Text = objVesselV.ETD.ToString();
                    this.LblETA.Text = objVesselV.ETA.HasValue ? objVesselV.ETA.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                    this.LblETD.Text = objVesselV.ETD.HasValue ? objVesselV.ETD.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                    this.LblViaje.Text = xList.VOYAGE.ToString();
                  
                    if (xList.ETD != null && xList.ETA != null)
                    {
                        var horas = (xList.ETD.Value - xList.ETA.Value).TotalHours;
                        this.LblHoras.Text = horas.ToString();
                    }
                    else { this.LblHoras.Text = "0"; }

                    Session["ProformaCab"] = null;

                    TablePendientes.DataSource = null;
                    TablePendientes.DataBind();

                    TableProformas.DataSource = null;
                    TableProformas.DataBind();

                    //detalle de proformas por referencia
                    List<ProformaCab> ListaProformaCab = ProformaCab.ListProformasCab(this.TxtReferencia.Text.Trim(), out cMensaje);
                    if (ListaProformaCab != null && ListaProformaCab.Count > 0)
                    {
                        TablePendientes.DataSource = ListaProformaCab;
                        TablePendientes.DataBind();

                    }
                    else
                    {
                        TablePendientes.DataSource = null;
                        TablePendientes.DataBind();
                    }

             
                }
                else
                {
                    this.LblNombre.Text = null;
                    this.LblETA.Text = null;
                    this.LblETD.Text = null;
                    this.LblViaje.Text = null;
                
                    this.MessageBox("No existe información del buque con los criterios de búsqueda ingresados", this);
                    return;
                }


            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {

                //graba transaccion
                //recuperar objeto
                obProformaCab = Session["ProformaCab"] as ProformaCab;

                if (obProformaCab != null)
                {
                    if (obProformaCab.ProformaDetalle.Count == 0)
                    {
                        this.MessageBox("Aún no ha agregado el detalle de la proforma", this);
                        return;
                    }

                    obProformaCab.Observacion = this.TxtGlosa.Text.Trim();
                    obProformaCab.Total = obProformaCab.ProformaDetalle.Sum(c => c.Total);
                    obProformaCab.SaveTransaction(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.MessageBox(v_mensaje.ToString(), this);
                    }
                    else
                    {
                        string cId = securetext(obProformaCab.Id.ToString());
                        this.MessageBox("Se genero la transacción con éxito", this);
                        this.Limpiar();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('proforma_preview.aspx?sid=" + cId + "');", true);
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
            Limpiar();
        }

        protected void AgregarProforma_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected) {

                try
                {

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "AgregarProforma_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "AgregarProforma_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {
                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "AgregarProforma_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    int idTransaccion = int.Parse(xpars.ToString());

                    var cab = ProformaCab.GetProformasCab(idTransaccion, out v_mensaje);
                    obProformaCab.Opc_id = cab.Opc_id;
                    obProformaCab.Opc_name = cab.Opc_name;
                    obProformaCab.Status = "N";
                    obProformaCab.Active = cab.Active;
                    obProformaCab.Create_user = sUser.loginname;
                    obProformaCab.Vessel_visit_reference = cab.Vessel_visit_reference;
                    obProformaCab.Tipo_carga = cab.Tipo_carga;
                    this.LblFechaProforma.Text = DateTime.Now.ToShortDateString();
                    this.LblRucProv.Text = obProformaCab.Opc_id.ToString();
                    this.LblProveedor.Text = obProformaCab.Opc_name.ToString();
                  

                    Session["ProformaCab"] = obProformaCab;

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la aprobación de la transacción, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "generar", "Item_comand", "Hubo un error al generar nueva proforma", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }
             
            }
               
        }

        protected void BtnAgregarItem_Click(object sender, EventArgs e)
        {
            try
            {

                Int32 IdConcepto = 0;
                string NombreConcepto = string.Empty;
                decimal nCantidad = 0;
                decimal nValor = 0;

                //recuperar objeto
                obProformaCab = Session["ProformaCab"] as ProformaCab;

                if (obProformaCab == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "container", "ValidateJSON", "Sesión no existe", "No disponible");
                    return;// CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Su sesión ha expirado, sera redireccionado a la página de login");
                }
                //si existen elementos ddel combo
                if (this.CboConcepto.SelectedIndex == -1)
                {
                    this.MessageBox("Debe seleccionar el concepto.", this);
                    return;
                }
                //valida datos requeridos
                if (Convert.ToDecimal(this.TxtCantidad.Text == string.Empty ? "0" : this.TxtCantidad.Text) == 0)
                {
                    this.MessageBox("Debe ingresar la cantidad", this);
                    return;
                }
                //valida datos requeridos
                if (Convert.ToDecimal(this.TxtPrecio.Text == string.Empty ? "0" : this.TxtPrecio.Text) == 0)
                {
                    this.MessageBox("Debe ingresar el precio", this);
                    return;
                }

                //valida si ya fue ingresada
                IdConcepto = Convert.ToInt32(this.CboConcepto.SelectedValue);
                NombreConcepto = this.CboConcepto.SelectedItem.ToString();

                if (obProformaCab.ProformaDetalle.Where(p => p.Concepto_id == IdConcepto).Count() > 0)
                {
                    this.MessageBox("El concepto ya fue ingresada", this);
                    return;
                }

                nCantidad = Convert.ToDecimal(this.TxtCantidad.Text == string.Empty ? "0" : this.TxtCantidad.Text);
                nValor = Convert.ToDecimal(this.TxtPrecio.Text == string.Empty ? "0" : this.TxtPrecio.Text);

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                obProformaDet.Line = obProformaCab.ProformaDetalle.Count+1;
                obProformaDet.Concepto_id = IdConcepto;
                obProformaDet.Concepto_name = NombreConcepto;
                obProformaDet.Total_horas = nCantidad;
                obProformaDet.Precio_hora = nValor;
                obProformaDet.Total = Decimal.Round((nCantidad * nValor),2);
                obProformaDet.Valida = false;
                obProformaDet.Create_user = sUser.loginname;
                obProformaDet.Mod_user = string.Empty;
                
                obProformaCab.ProformaDetalle.Add(obProformaDet);

                TableProformas.DataSource = obProformaCab.ProformaDetalle;
                TableProformas.DataBind();

                var ntotal = obProformaCab.ProformaDetalle.Sum(p => p.Total); 
                this.LblTotalProforma.Text = ntotal.ToString();

                //salvar objeto
                Session["ProformaCab"] = obProformaCab;

                this.TxtCantidad.Text = null;
                this.TxtPrecio.Text = null;


            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        protected void CboConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboConcepto.SelectedIndex != -1)
                {
                    List<Concepto> Lista = Concepto.GetConcepto(Int32.Parse(CboConcepto.SelectedValue));
                    var xList = Lista.FirstOrDefault();

                    if (xList != null)
                    {
                        this.TxtPrecio.Text = xList.price.ToString();
                    }
                }
               
                   
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

      
    }
}