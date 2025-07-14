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
using Microsoft.Reporting.WebForms;

namespace CSLSite
{
    public partial class frm_listado_det_nota_credito : System.Web.UI.Page
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
                //this.TxtNumero.Text =Server.HtmlEncode(this.TxtNumero.Text);
                this.desded.Text = Server.HtmlEncode(this.desded.Text);
                this.desded.Text = Server.HtmlEncode(this.hastad.Text);

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
              

                this.CboEstado.Items.Clear();
                this.CboEstado.Items.Add("TODOS");
                this.CboEstado.Items.Add("PENDIENTE");
                this.CboEstado.Items.Add("FINALIZADO");
                this.CboEstado.SelectedIndex = 1;

                string desde = "01/" + DateTime.Today.Month.ToString("D2") + "/"+DateTime.Today.Year.ToString() ;
                

                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                DateTime fdesde;
               
             
                if (!DateTime.TryParseExact(desde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fdesde))
                {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha desde";
                        sinresultado.Visible = true;
                        return;
                }

                this.desded.Text = fdesde.ToString("dd/MM/yyyy");
                this.hastad.Text = DateTime.Today.ToString("dd/MM/yyyy");

                Session["estado"] = Convert.ToInt32(this.CboEstado.SelectedIndex);
                Session["fecha_desde"] = fdesde.ToString("yyyy/MM/dd");
                Session["fecha_hasta"] = DateTime.Today.ToString("yyyy/MM/dd");

                this.Carga_ListadoConceptos();
                this.CboConcepto.SelectedIndex = 0;

                Session["id_concept"] = int.Parse(this.CboConcepto.SelectedValue);

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

       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
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

                Session["estado"] = Convert.ToInt32(this.CboEstado.SelectedIndex);
                Session["fecha_desde"] = desde.ToString("yyyy/MM/dd");
                Session["fecha_hasta"] = hasta.ToString("yyyy/MM/dd");
                Session["id_concept"] = int.Parse(this.CboConcepto.SelectedValue);


                var user = Page.getUserBySesion();

                var table = credit_head.List_Nota_Credito_Resumidas(this.CboEstado.SelectedIndex, desde,hasta, int.Parse(this.CboConcepto.SelectedValue), out vr);
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

                xfinder.Visible = true;

                string cUsuario = string.Format("Usuario: {0} {1} ", user.nombres, user.apellidos);
                ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });

                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.ShowToolBar = true;
                ReportViewer1.ShowPrintButton = true;

                ReportViewer1.LocalReport.Refresh();

            }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "frm_listado_det_nota_credito", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
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