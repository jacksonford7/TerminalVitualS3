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

namespace CSLSite
{
    public partial class consulta_trabajos : System.Web.UI.Page
    {
        //AntiXRCFG
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
                this._referencia.Text =Server.HtmlEncode(this._referencia.Text);
               
                this.ruc_opc.Text =Server.HtmlEncode(this.ruc_opc.Text);
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
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
       


 

       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
            Int64 idpro = 0;
            string vr = string.Empty;
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

               var user = Page.getUserBySesion();
                var table = Vessel_Visit.ListViesselVisit(desde, hasta, ruc_opc.Text, _referencia.Text);
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
                //cargarle todas las gruas y los turnos de estos documentos.
                table.ForEach(t => { t.LoadCranes(); t.LoadTurns(); });

                var lfinal = table.Select(g => new {
                    id= g.ID,
                    referencia = g.REFERENCE,
                    nave = g.NAME,
                    atraque = g.ATA.HasValue ? g.ATA : g.ETA,
                    zarpe = g.ATD.HasValue ? g.ATD : g.ETD,
                    opcs = g.Turns.Select(t=>t.opc_id).Distinct().Count(),
                    gruas=g.Cranes.Select(c=>c.Crane_name).Distinct().Count(),
                    turnos = g.Turns.Where(t=>t.active.HasValue && t.active.Value).Count(),
                    creada = g.Create_date,
                    status= g.STATUS
                });



               Session["resultado"] = table;
               this.tablePagination.DataSource = lfinal;
               this.tablePagination.DataBind();
               xfinder.Visible = true;
           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "consulta_pro", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
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
                return "<span class='azul' >Nueva</span>";
            }
            if (es.Equals("w"))
            {
                return "<span class='naranja' >Publicada</span>";
            }
            if (es.Equals("s"))
            {
                return "<span class='naranja' >Turnos asignados</span>";
            }
            if (es.Equals("p")|| es.Equals("c"))
            {
                return "<span class='red' >Aprobada</span>";
            }

            if (es.Equals("g"))
            {
                return "<span class='red' >Proformada</span>";
            }
            if (es.Equals("f"))
            {
                return "<span class='red' >Facturada</span>";
            }
            return "<span>sin estado!</span>";
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
   


        #endregion
    }
}