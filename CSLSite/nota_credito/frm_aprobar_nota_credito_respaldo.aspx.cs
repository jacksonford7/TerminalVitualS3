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
using ControlOPC.Entidades;
using ClsNotasCreditos;

namespace CSLSite
{
    public partial class frm_aprobar_nota_credito_respaldo : System.Web.UI.Page
    {
        //AntiXRCFG
        private credit_head objcredit_head = new credit_head();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            this.IsAllowAccess();
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
                this.TxtNumero.Text =Server.HtmlEncode(this.TxtNumero.Text);
                this.desded.Text = Server.HtmlEncode(this.desded.Text);
                this.desded.Text = Server.HtmlEncode(this.hastad.Text);
                this._nc_id.Value = Server.HtmlEncode(this._nc_id.Value);
                this.TxtMotivo.Text = Server.HtmlEncode(this.TxtMotivo.Text);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;
            }

            if (!IsPostBack)
            {
                /*oculta paneles de motivo de anulacion*/
                this.botones.Visible = false;
                this.motivo.Visible = false;

                populate();
               
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
                    this.botones.Visible = false;
                    this.motivo.Visible = false;

                    populate();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }


        protected void btnprocesar_Click(object sender, EventArgs e)
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

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "btnprocesar_Click", "btnprocesar_Click", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        this.botones.Visible = false;
                        this.motivo.Visible = false;
                        this._nc_id.Value = string.Empty;
                        return;
                    }

                    if (this.TxtMotivo.Text.Trim() == string.Empty)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Ingrese el motivo de la anulación");
                        sinresultado.Visible = true;
                        return;
                    }

                    Int64 id;
                    var t = this._nc_id.Value.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("ID NO CONVERTIBLE"), "btnprocesar_Click", "btnprocesar_Click", id.ToString(), user.loginname));
                        sinresultado.Visible = true;
                        this.botones.Visible = false;
                        this.motivo.Visible = false;
                        this._nc_id.Value = string.Empty;
                        return;

                    }
                    /*anulacion*/
                    if (id != 0)
                    {
                        
                        objcredit_head = new credit_head();
                        objcredit_head.nc_id = id;
                        objcredit_head.nc_anulacion = this.TxtMotivo.Text.Trim();
                        objcredit_head.Create_user = user.loginname;

                        if (objcredit_head.Anular())
                        {

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            sinresultado.InnerText = string.Format("Anulación de nota de crédito No. Interno {0} ,realizada con éxito.", id);
                            sinresultado.Visible = true;
                            this.botones.Visible = false;
                            this.motivo.Visible = false;
                            this._nc_id.Value = string.Empty;
                            this.TxtMotivo.Text = string.Empty;

                        }
                        else
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("ERROR AL realizar anulación de nota de crédito No. Interno {0}", id);
                            sinresultado.Visible = true;
                            this.botones.Visible = false;
                            this.motivo.Visible = false;
                            this._nc_id.Value = string.Empty;
                            return;

                        }
                    }
                    else {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("No existe ID interno nota de crédito, volver a intentar");
                        sinresultado.Visible = true;
                        this.botones.Visible = false;
                        this.motivo.Visible = false;
                        this._nc_id.Value = string.Empty;
                        return;
                    }
                   

                    populate();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                    this.botones.Visible = false;
                    this.motivo.Visible = false;
                    this._nc_id.Value = string.Empty;
                }
            }
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
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta_pro", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta_pro", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("ID NO CONVERTIBLE"), "consulta_pro", "tablePagination_ItemCommand",e.CommandArgument.ToString(), user.loginname));
                        sinresultado.Visible = true;
                        return;

                    }

                    Int64 nid_level = 0;
                    Int64 nid_group = 0;
                    Int32 nIdUsuario = 0;
                    Int16 nlevel = 0;

                    if (e.CommandName == "Aprobar")
                    {
                        Label lbl_id_level = e.Item.FindControl("lbl_id_level") as Label;
                        Label lbl_id_group = e.Item.FindControl("lbl_id_group") as Label;
                        Label lbl_IdUsuario = e.Item.FindControl("lbl_IdUsuario") as Label;
                        Label lbl_level = e.Item.FindControl("lbl_level") as Label;

                        if (lbl_id_level.Text != string.Empty) { nid_level = Convert.ToInt64(lbl_id_level.Text); } else { nid_level = 0; }
                        if (lbl_id_group.Text != string.Empty) { nid_group = Convert.ToInt64(lbl_id_group.Text); } else { nid_group = 0; }
                        if (lbl_IdUsuario.Text != string.Empty){ nIdUsuario = Convert.ToInt32(lbl_IdUsuario.Text); } else { nIdUsuario = 0; }
                        if (lbl_level.Text != string.Empty){ nlevel = Convert.ToInt16(lbl_level.Text); } else { nlevel = 0; }

                        
                        objcredit_head = new credit_head();
                        objcredit_head.nc_id = id;
                        objcredit_head.id_level = nid_level;
                        objcredit_head.id_group = nid_group;
                        objcredit_head.IdUsuario = nIdUsuario;
                        objcredit_head.level = nlevel;
                        objcredit_head.Create_user = user.loginname;

                        if (objcredit_head.Aprobar())
                        {
                            
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            sinresultado.InnerText = string.Format("Aprobación de nota de crédito No. Interno {0} ,realizada con éxito.", e.CommandArgument);
                            sinresultado.Visible = true;
                            this.botones.Visible = false;
                            this.motivo.Visible = false;
                            this._nc_id.Value = string.Empty;
                            this.TxtMotivo.Text = string.Empty;
                        }
                        else
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("ERROR AL realizar aprobación de nota de crédito No. Interno {0}", e.CommandArgument);
                            sinresultado.Visible = true;
                            return;

                        }

                    }
                    if (e.CommandName == "Anular")
                    {

                        /*mostar paneles de motivo de anulacion*/
                        this.botones.Visible = true;
                        this.motivo.Visible = true;
                        this._nc_id.Value = id.ToString();

                        //upresult.Update();

                        /*
                        objcredit_head = new credit_head();
                        objcredit_head.nc_id = id;
                        objcredit_head.Create_user = user.loginname;

                        if (objcredit_head.Anular())
                        {

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            sinresultado.InnerText = string.Format("Anulación de nota de crédito No. Interno {0} ,realizada con éxito.", e.CommandArgument);
                            sinresultado.Visible = true;
                        }
                        else
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("ERROR AL realizar anulación de nota de crédito No. Interno {0}", e.CommandArgument);
                            sinresultado.Visible = true;
                            return;

                        }*/
                    }
                   
                    populate();

                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al anular", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }

       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
            Int64 nc_id = 0;
            string vr = string.Empty;
            try
           {
               DateTime desde;
               DateTime hasta;
                
                if (this.desded.Text != string.Empty)
                {
                    if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha desde";
                        sinresultado.Visible = true;
                        return;
                    }
                }
                else { desde = DateTime.Parse("01/01/1999");   }

                if (this.hastad.Text != string.Empty)
                {
                    if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha hasta";
                        sinresultado.Visible = true;
                        return;
                    }
                }
                else { hasta = DateTime.Parse("01/01/1999"); }

                if (string.IsNullOrEmpty(TxtNumero.Text))
                {
                    this.TxtNumero.Text = "";
                    nc_id = 0;
                }
                else
                {
                    if (!Int64.TryParse(this.TxtNumero.Text, out nc_id))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No se encontraron resultados, revise el número de proforma";
                        sinresultado.Visible = true;
                        return;
                    }
                }
                
          
               var user = Page.getUserBySesion();

                var table = credit_head.List_Nota_Credito_Pendientes(user.id, user.loginname, desde,hasta, nc_id,0, out vr);
                if (table == null)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = vr;
                    sinresultado.Visible = true;
                    return;
                }
                if (table.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise los parámetros";
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
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
                sinresultado.Visible = true;
           }

       }

     
        #region "metodos_repeater"

        public static string formatPro(object id)
        {
            Int64 idm = 0;
            if (id != null)
            {
                if (Int64.TryParse(id.ToString(), out idm))
                {
                    return idm.ToString("D8");
                }
            }
            return "undefined";
        }
        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            var es = estado.ToString();
            es = es.ToLower();

            if (es.Equals("n")) {
                return "<span class='azul' >Generada</span>";
            }
            if (es.Equals("f"))
            {
                return "<span class='naranja' >Facturada</span>";
            }
            if (es.Equals("a"))
            {
                return "<span class='red' >Anulada</span>";
            }
            return "<span>sin estado!</span>";
        }
        public static bool boton(object estado)
        {
            var t = estado as string;
            if (!string.IsNullOrEmpty(t))
            {
                if (t.ToLower().Contains("a") || t.ToLower().Contains("f"))
                {
                    return false;
                }
            }

            return true;
        }
        public static string securetext(object number)
        {
            if (number == null )
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        public static string formatProDate(object fecha)
        {
            DateTime dt;
            if (fecha != null)
            {
                if (DateTime.TryParse(fecha.ToString(), out dt))
                {
                    return dt.ToString("dd/MM/yyyy HH:mm");
                }
            }

            return "undefined";
        }
        public static string xver(object est)
        {
            if (est != null)
            {
                return est.ToString().ToLower().Equals("a") ? "ocultar" : "mostrar";
            }
            return null;
        }


        #endregion
    }
}