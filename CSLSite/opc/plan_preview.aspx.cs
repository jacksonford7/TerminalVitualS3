using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;
using ControlOPC.Entidades;
using System.Text;

namespace CSLSite
{
    public partial class plan_preview : System.Web.UI.Page
    {

        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        private string sid = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "printplan", "Init", sid, Request.UserHostAddress);
                this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                return;
            }

            try
            {


                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de plan no valido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "printplan", "Init", sid, Request.UserHostAddress);
                    this.AbortResponse( string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}",Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                    return;
                }

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "printplan", "Page_Init", sid, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código:{0}", number));
                base.Response.Write(close);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (!IsPostBack)
                {
                    try
                    {

                        Int64 id = 0;
                        sid = sid.Trim().Replace("\0", string.Empty);
                        if (!Int64.TryParse(sid, out id))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, sid no es numerico", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "plapreview", "Page_Load", sid == null ? "sid is no es numerico" : sid, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }
                        var vv = new Vessel_Visit(id);
                        var msg = string.Empty;
                        //recupero el objeto
                        if ( !vv.PopulateMyData(out msg))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "plapreview", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }
                        //recupero la grúas
                        vv.LoadCranes();
                        //recupero los turnos
                        vv.LoadTurns();
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=200&height=60&size=50", vv.ID.ToString("D8"));
                        if (!string.IsNullOrEmpty( vv.TIPO_CARGA))
                        {
                            if (vv.TIPO_CARGA.Equals("C")) { full.InnerText = "( X )"; }
                            else if (vv.TIPO_CARGA.Equals("B")) { csuelta.InnerText = "( X )"; }

                            full.Visible = false;
                            csuelta.Visible = false;

                        }
                        referencia.InnerText = vv.REFERENCE;
                        buque.InnerText = vv.NAME;

                        eta.InnerText = vv.ETA.HasValue? vv.ETA.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                        etd.InnerText = vv.ETD.HasValue? vv.ETD.Value.ToString("dd/MM/yyyy HH:mm") : "...";

                        //->pendiente traer datos  de N4 actualizados.
                        var vs = Vessel.ListaVessel(vv.REFERENCE).FirstOrDefault();
                        if(vs!=null)
                        {
                            //se recupero la data de N4 actualizada
                            vv.END_WORK = vv.END_WORK.HasValue ? vv.END_WORK : vs.END_WORK;
                            vv.START_WORK = vv.START_WORK.HasValue ? vv.START_WORK : vs.START_WORK;
                            vv.ATA = vv.ATA.HasValue ? vv.ATA : vs.ATA;
                            vv.ATD = vv.ATD.HasValue ? vv.ATD : vs.ATD;
                            vv.BERTH = string.IsNullOrEmpty(vv.BERTH) ? vs.BERTH : vv.BERTH;
                        }

                        ata.InnerText = vv.ATA.HasValue? vv.ATA.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                        atd.InnerText = vv.ATD.HasValue? vv.ATD.Value.ToString("dd/MM/yyyy HH:mm") : "...";

                        wini.InnerText = vv.START_WORK.HasValue ? vv.START_WORK.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                        wend.InnerText = vv.END_WORK.HasValue ? vv.END_WORK.Value.ToString("dd/MM/yyyy HH:mm") : "...";

                        citacion.InnerText = vv.FECHA_CITA.Value.ToString("dd/MM/yyyy HH:mm");

                        this.barcode2.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=200&height=60&size=50", vv.ID.ToString("D8"));
                        anumber.InnerText = vv.REFERENCE;
                        this.grua_data.InnerHtml =  grua_table(vv.Cranes);
                        this.turno_data.InnerHtml = turno_table(vv.Turns);
                        this.fechagenera.InnerText = string.Format("{0}", !vv.Create_date.HasValue ? vv.Create_date.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
 
                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log<Exception>(ex , "planpreview", "Page_Load", sid, User.Identity.Name);
                        string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        base.Response.Write(close);
                    }
                }
            }
        }



        private static string grua_table(List<Vessel_Crane> gruas)
        {
            if (gruas == null)
            {
                return "<p>Hubo un error al Cargar los datos comuniquese con el área de IT de Cgsa</p>";
            }

            if (gruas.Count <=0)
            {
                return "<p><strong> No existen registros de grúas que mostrar.</strong></p>";
            }

            StringBuilder tab = new StringBuilder();
            tab.Append("<table class='print_table'>");
            tab.Append("<thead><tr><th>Grúa</th> <th>Trabajo (Hrs)</th><th>Inicio</th><th>Fin</th></tr><thead>");
            tab.Append("<tbody>");
            foreach (var f in gruas)
            {
                tab.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                    string.IsNullOrEmpty( f.Crane_name)?"...":f.Crane_name,
                    f.Crane_time_qty,
                    f.StarWork.HasValue?f.StarWork.Value.ToString("dd/MMM/yyyy HH:mm"):  "...",
                    f.EndWork.HasValue ? f.EndWork.Value.ToString("dd/MMM/yyyy HH:mm") : "..."
                    );
            }
            tab.Append("<tbody>");
            tab.Append("</table>");
            return tab.ToString();
        }
        private static string turno_table(List<Crane_Turn> turnos)
        {
            if (turnos == null)
            {
                return "<p>Hubo un error al cargar los datos comuniquese con el área de IT de Cgsa</p>";
            }

            if (turnos.Count <= 0)
            {
                return "<p><strong> No existen registros de turnos que mostrar.</strong></p>";
            }

            StringBuilder tab = new StringBuilder();
            tab.Append("<table class='print_table'>");
            tab.Append("<thead><tr><th>Grúa</th> <th>Numero</th> <th>Inicio</th><th>Fin</th><th>OPC</th></tr><thead>");
            tab.Append("<tbody>");
            var tu = turnos.Where(s=>s.active.HasValue && s.active.Value);

            foreach (var f in tu)
            {
                tab.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                    string.IsNullOrEmpty(f.crane_name) ? "..." : f.crane_name,
                    f.turno_number,
                    f.turn_time_start.HasValue ? f.turn_time_start.Value.ToString("dd/MMM/yyyy HH:mm") : "...",
                    f.turn_time_end.HasValue ? f.turn_time_end.Value.ToString("dd/MMM/yyyy HH:mm") : "...",
                    string.IsNullOrEmpty(f.opc_name) ? "..." : f.opc_name
                    );
            }
            tab.Append("<tbody>");
            tab.Append("</table>");
            return tab.ToString();
        }
    }
}