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
    public partial class frm_pendientes_nota_credito : System.Web.UI.Page
    {
        //AntiXRCFG
        private credit_head objcredit_head = new credit_head();

        private void Carga_ListadoConceptos()
        {
            try
            {

                List<concepts> ListConceptos = concepts.ListConceptosGeneral();
                if (ListConceptos != null)
                {
                    this.CboConcepto.DataSource = ListConceptos;
                    this.CboConcepto.DataBind();
                }
                else
                {
                    this.CboConcepto.DataSource = null;
                    this.CboConcepto.DataBind();
                }



            }
            catch (Exception ex)
            {
                //sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "CboConcepto", "Carga_ListadoConceptos", "Hubo un error al cargar conceptos", user2 != null ? user2.loginname : "Nologin"));
                //this.Alerta(sg);
                //return;
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_listado_res_nota_credito", "Carga_ListadoConceptos", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;



            }

        }


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
                this.Carga_ListadoConceptos();
                this.CboConcepto.SelectedIndex = 0;

                populate();
               
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
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "frm_pendientes_nota_credito", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                 
                      
                   
                    populate();

                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la seleccion de elementos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_pendientes_nota_credito", "tablePagination_ItemCommand", "Hubo un error", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }

       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
            string vr = string.Empty;
            try
           {
               
               var user = Page.getUserBySesion();

                var table = credit_head.List_Seguimiento_Nota_Credito(user.id, user.loginname, int.Parse(this.CboConcepto.SelectedValue),out vr);
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
                   this.sinresultado.InnerText = "No se encontraron resultados..";
                   sinresultado.Visible = true;
                   return;
               }

               Session["resultado"] = table;
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;


                foreach (RepeaterItem item in tablePagination.Items)
                {
                    Label LblEstado = item.FindControl("lbl_estado") as Label;

                    Label lbl_nc_id = item.FindControl("lbl_nc_id") as Label;
                    Label lbl_nc_date_text = item.FindControl("lbl_nc_date_text") as Label;
                    Label lbl_description = item.FindControl("lbl_description") as Label;
                    Label lbl_num_factura = item.FindControl("lbl_num_factura") as Label;
                    Label lbl_nombre_cliente = item.FindControl("lbl_nombre_cliente") as Label;
                    Label lbl_nc_total = item.FindControl("lbl_nc_total") as Label;
                    Label lbl_usuarios_aprobados = item.FindControl("lbl_usuarios_aprobados") as Label;
                    Label lbl_usuarios_pendientes = item.FindControl("lbl_usuarios_pendientes") as Label;
                    Label lbl_level_text = item.FindControl("lbl_level_text") as Label;
                  
                    if (LblEstado.Text == "APROBADO")
                    {

                        lbl_nc_id.ForeColor = System.Drawing.Color.Blue;
                        lbl_nc_date_text.ForeColor = System.Drawing.Color.Blue;
                        lbl_description.ForeColor = System.Drawing.Color.Blue;
                        lbl_num_factura.ForeColor = System.Drawing.Color.Blue;
                        lbl_nombre_cliente.ForeColor = System.Drawing.Color.Blue;
                        lbl_nc_total.ForeColor = System.Drawing.Color.Blue;
                        lbl_usuarios_aprobados.ForeColor = System.Drawing.Color.Blue;
                        lbl_usuarios_pendientes.ForeColor = System.Drawing.Color.Blue;
                        lbl_level_text.ForeColor = System.Drawing.Color.Blue;
                    }
                }

           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
              vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "frm_pendientes_nota_credito", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
                sinresultado.Visible = true;
                this.sinresultado.InnerText = vr;
           }

       }

     
        #region "metodos_repeater"

       
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
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_listado_res_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
    }
}