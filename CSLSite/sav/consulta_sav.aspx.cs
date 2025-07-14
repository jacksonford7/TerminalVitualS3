using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using csl_log;
using ClsAisvSav;

namespace CSLSite
{
    public partial class consultasav : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
#if !DEBUG
            this.IsAllowAccess();
#endif
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
             
                this.aisvn.Text =Server.HtmlEncode(this.aisvn.Text);
                this.cntrn.Text =Server.HtmlEncode(this.cntrn.Text);
              
                this.desded.Text = Server.HtmlEncode(this.desded.Text);
                this.desded.Text = Server.HtmlEncode(this.hastad.Text);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;

                if (!IsPostBack)
                {
                    this.mensaje.Visible = false;
                    this.saldo.InnerText = string.Empty;
                    this.LlenaComboDepositos();


                }
            }
        }

        #region "DEPOSITO"
        protected void cmbDeposito_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbDeposito.SelectedIndex != -1)
                {
                    Int64 ID_DEPORT;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                    {
                        ID_DEPORT = 0;
                    }

                    Session["resultado"] = null;
                    this.tablePagination.DataSource = null;
                    this.tablePagination.DataBind();

                    this.mensaje.Visible = false;
                    this.saldo.InnerText = string.Empty;
                }


            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consulta_sav.aspx", "cmbDeposito_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);


            }
        }
        #endregion
        public void LlenaComboDepositos()
        {
            try
            {
                cmbDeposito.DataSource = man_pro_expo.consultaDepositosFiltrado("1"); //ds.Tables[0].DefaultView;
                cmbDeposito.DataValueField = "ID";
                cmbDeposito.DataTextField = "DESCRIPCION";
                cmbDeposito.DataBind();
                cmbDeposito.Enabled = true;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consulta_sav", "LlenaComboDepositos()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }
                    populate();

                    //clientes de contado
                    var user = this.getUserBySesion();
                    bool Credito = usuario.Credito(user.ruc);
                    if (!Credito)
                    {
                        Int64 ID_DEPORT;

                        if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                        {
                            ID_DEPORT = 0;
                        }


                        /*muestra mensaje de saldo a favor*/
                        List<SaldoProforma> Lista = SaldoProforma.Get_Saldo_Repcontver(user.ruc, ID_DEPORT);

                        if (Lista != null) 
                        {
                            var xList = Lista.FirstOrDefault();
                            if (xList != null)
                            {
                                decimal nSaldo = xList.saldo_final;
                                if (nSaldo != 0)
                                {
                                    this.mensaje.Visible = true;
                                    this.saldo.InnerText = xList.leyenda;
                                }
                                else
                                {
                                    this.mensaje.Visible = false;
                                    this.saldo.InnerText = string.Empty;
                                }
                            }
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            if (estado.ToString().ToLower() == "true")
            {
                return "<span>Activo</span>";
            }
            else
            {
                return "<span class='red' >Expirado</span>";
            }
        }

        public static string boton(object estado)
        { 
          return  estado.ToString().ToLower() == "true" ? "ver":"xver";
        }
        public static string securetext(object number)
        {
            if (number == null || number.ToString().Length <= 2)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }
                    var xpars = e.CommandArgument.ToString().Split(';');
                    if (xpars.Length <= 0 || xpars.Length < 2)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    if (string.IsNullOrEmpty(xpars[0]) || string.IsNullOrEmpty(xpars[1]))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = "No se puede anular este AISV ya que su estado ha cambiado, es posible que la carga ya se encuentre patios.";
                        sinresultado.Visible = true;
                        return;
                    }

                    if (e.CommandName.Contains("eliminar")) 
                    {
                        var sc = new TurnosSAV.ServicioPreavisosClient();

                        

                        var tok = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                        //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                        var vt = sc.cancelar_turno_preavisado_simple(tok, xpars[0], xpars[1], user.loginname, "ANULACION POR AISV");
                        //AQUI LA ANULACION DEL AISV Y TURNO
                        if (vt.resultado != 1)
                        {

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = vt.mensaje_principal;
                            sinresultado.Visible = true;
                            populate();
                            return;
                        }
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        sinresultado.InnerText = string.Format("La anulación del AISV  No.{0} ha resultado exitosa.", xpars[0]);
                        sinresultado.Visible = true;

                        long IdDepot = long.Parse(cmbDeposito.SelectedValue);

                        if (IdDepot == 5)//OPACIFIC
                        {
                            if (!string.IsNullOrEmpty(xpars[2]))
                            {
                                if (!xpars[2].ToString().Equals("0"))
                                {
                                    btnCancelaPase(xpars[2].ToString());
                                }
                            }
                        }

                        


                        //<20210408> REACTIVACION DE ASIGNACION DE PAGO ASUMIDO
                        var oAsume = new ControlPagos.Importacion.PagoAsignado();
                        long v_idAsignado = oAsume.obtener_IdAsignado(xpars[0], xpars[1]).Resultado;
                        oAsume.id_asignacion = v_idAsignado;
                        oAsume.login_modifica = user.loginname;
                        oAsume.ActivarAsignacionSAV();
                        //</20210408>

                        populate();

                        //clientes de contado
                        bool Credito = usuario.Credito(user.ruc);
                        if (!Credito)
                        {
                            Int64 ID_DEPORT;

                            if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                            {
                                ID_DEPORT = 0;
                            }

                            /*muestra mensaje de saldo a favor*/
                            List<SaldoProforma> Lista = SaldoProforma.Get_Saldo_Repcontver(user.ruc, ID_DEPORT);
                            var xList = Lista.FirstOrDefault();
                            if (xList != null)
                            {
                                decimal nSaldo = xList.saldo_final;
                                if (nSaldo != 0)
                                {
                                    this.mensaje.Visible = true;
                                    this.saldo.InnerText = xList.leyenda;
                                }
                                else
                                {
                                    this.mensaje.Visible = false;
                                    this.saldo.InnerText = string.Empty;
                                }
                            }
                        }

                    }

                    else if (e.CommandName == "Imprimir_proforma") //imprime proforma o liquidacion
                    {
                       
                        Int64 value = Convert.ToInt64(xpars[1].ToString());

                        /*se imprime proforma de contado*/
                        var sid = QuerySegura.EncryptQueryString(value.ToString());
                        this.Popup(string.Format("../sav/printproforma_sav.aspx?sid={0}", sid));

                    }
                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consultasav", "Item_comand", "Hubo un error al anular", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }

        protected void tablePagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            /*valida cotizacion*/
            var usn = new usuario();
            usn = this.getUserBySesion();

            if (usn == null)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "consulta_sav", "tablePagination_ItemDataBound", "No usuario", Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            //bool Credito = usuario.Credito(usn.ruc);     
            bool Credito = usn.IsCredito;
           
            if (Credito)  //validaciones usuario de credito
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    Label lbl_tit_imprimir = (Label)e.Item.FindControl("lbl_tit_imprimir");
                    lbl_tit_imprimir.Visible = false;

                    var col = e.Item.FindControl("imprimir_prof");
                    if (col != null)
                    { col.Visible = false; }

                }
                if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                {

                    ImageButton btnImprimirProforma = (ImageButton)e.Item.FindControl("btnImprimirProforma");
                    btnImprimirProforma.Visible = false;

                    Label LblEstadoPago = (Label)e.Item.FindControl("LblEstadoPago");
                    LblEstadoPago.Visible = false;

                    var col = e.Item.FindControl("imprimir_prof_det");
                    if (col != null)
                    { col.Visible = false; }
                }

            }
            else
            {
                if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                {
                    Label lbl_estado = (Label)e.Item.FindControl("lblEstado");
                    string estado = lbl_estado.Text.Trim().Replace("<span class='red' >Expirado</span>", "Expirado");

                    if (estado.Equals("Expirado"))
                    {
                        ImageButton btnImprimirProforma = (ImageButton)e.Item.FindControl("btnImprimirProforma");
                        btnImprimirProforma.Enabled = false;
                    }

                    Label LblEstadoPago = (Label)e.Item.FindControl("LblEstadoPago");
                    LblEstadoPago.Visible = true;
                }

                
            }
        }

        protected string jsarguments(object aisv,  object unidad, object paseLinea)
        {
            return string.Format("{0};{1};{2}", aisv != null ? aisv.ToString().Trim() : "0", unidad != null ? unidad.ToString().Trim() : "NI", paseLinea != null ? paseLinea.ToString().Trim() : "0");
        }
        private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
            var table = new Catalogos.sav_listar_preaviso_repcontverDataTable();// sav_listar_preavisoDataTable();
            var ta = new CatalogosTableAdapters.sav_listar_preaviso_repcontverTableAdapter();//new CatalogosTableAdapters.sav_listar_preavisoTableAdapter();
           try
           {
               DateTime desde;
               DateTime hasta;
               if (!DateTime.TryParseExact(  desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                   sinresultado.Visible = true;
                   return;
               }
               if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                   sinresultado.Visible = true;
                   return;
               }
               if (desde.Year != hasta.Year)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-alerta";
                   this.sinresultado.InnerText = "El rango máximo de consulta es de 1 año, gracias por entender.";
                   sinresultado.Visible = true;
                   return;
               }
                //TimeSpan ts = desde - hasta;
                //// Difference in days.
                //if (ts.Days > 30)
                //{
                //    xfinder.Visible = false;
                //    sinresultado.Attributes["class"] = string.Empty;
                //    sinresultado.Attributes["class"] = "msg-alerta";
                //    this.sinresultado.InnerText = "El rango máximo de consulta es de 1 mes, gracias por entender.";
                //    sinresultado.Visible = true;
                //    return;
                //}

                Int64 ID_DEPORT;

                if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                {
                    ID_DEPORT = 0;
                }

                var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
               //user.loginname = "botrosa";
               ta.Fill(table,   desde, hasta, cntrn.Text,aisvn.Text, user.loginname, ID_DEPORT);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la unidad, documento o # aisv";
                   sinresultado.Visible = true;
                   return;
               }

               Session["resultado"] = table;
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;
           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "populate", "Hubo un error al buscar", t.loginname));
               sinresultado.Visible = true;
           }
           finally
           {
               ta.Dispose();
               table.Dispose();
           }
       }

        protected void btnCancelaPase(string idPase)
        {
            try
            {
                app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                Dictionary<string, string> obj = new Dictionary<string, string>();

                obj.Add("AV_ESTADO", "X");
                obj.Add("AN_ID_TURNO_REFERENCIA", idPase.ToString());

                CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("CANCELA_CITA", obj);
                if (_result.result.estado != 1)
                {
                    Exception ex;
                    ex = new Exception("Error al cancelarr cita en OPACIFIC con Servicio Web; Cita: " + idPase.ToString());
                    var number = log_csl.save_log<Exception>(ex, "consulta_sav", "btncancelar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnCancelaPase()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

    }
}