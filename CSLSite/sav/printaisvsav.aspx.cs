using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;

namespace CSLSite
{
    public partial class printaisvsav : System.Web.UI.Page
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
                var number = log_csl.save_log<Exception>(ex, "printaisvsav", "Init", sid, Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                return;
            }

            try
            {
              
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "printaisvsav", "Init", sid, Request.UserHostAddress);
                    this.PersonalResponse( string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}",Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "printaisvsav", "Page_Init", sid, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
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
                        var tabla = new Catalogos.sav_imprimir_preavisoDataTable();
                        var ta = new CatalogosTableAdapters.sav_imprimir_preavisoTableAdapter();
                        sid = sid.Trim().Replace("\0", string.Empty);
                        ta.Fill(tabla, sid);
                        if (tabla.Rows.Count <= 0)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "printaisvsav", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                            this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                            return;
                        }
                        var fila = tabla.FirstOrDefault();
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", sid);
                        this.referencia.InnerText = !fila.Isunidad_referenciaNull() ?fila.unidad_referencia : "-";
                        this.booking.InnerText = fila.unidad_booking;
                        this.fecha.InnerText = fila.turno_fecha.ToString("dd/MM/yyyy");
                        this.hora.InnerText = fila.turno_hora;
                        this.container.InnerText = fila.unidad_id;
                        this.tamano.InnerText = fila.unidad_tamano;
                        this.opera.InnerText = fila.unidad_linea;
                        this.conductor.InnerText = fila.chofer_nombre;
                        this.cedula.InnerText = fila.chofer_licencia;
                        this.placa.InnerText = fila.vehiculo_placa;
                        this.depot.InnerText = fila.deposito_nombre;

                        this.barcode2.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", sid);
                        anumber.InnerText = sid;
                        numcontenedor.InnerText = fila.unidad_id;

                        this.fechagenera.InnerText = string.Format("{0}",  fila.creado_fecha.ToString("dd/MM/yyyy HH:mm"));
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
      
                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log<Exception>(ex , "printaisvsav", "Page_Load", sid, User.Identity.Name);
                        string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        base.Response.Write(close);
                    }
                }
            }
        }
    }
}