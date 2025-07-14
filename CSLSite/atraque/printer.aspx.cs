using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;

namespace CSLSite.atraque
{
public partial class printer : System.Web.UI.Page
    {
        private string sid = string.Empty;
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //todo ver si esta autenticada
            try
            {

                if (!Request.IsAuthenticated)
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "printaisv", "Init", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "QuerySegura", "DecryptQueryString", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "print_solicitud", "Page_Init", sid, User.Identity.Name);
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
                        var tabla = new Catalogos.obtenerSolicitudDataTable();
                        var ta = new CatalogosTableAdapters.obtenerSolicitudTableAdapter();
                        sid = sid.Trim().Replace("\0", string.Empty);
                        ta.Fill(tabla, sid);
                        if (tabla.Rows.Count <= 0)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "printaisv", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                            this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                            return;
                        }
                        var fila = tabla.FirstOrDefault();
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", sid);
                        this.anumber.InnerText = fila.referencia;
                        this.agencia.InnerText = fila.agencia;
                        this.sservicio.InnerText = fila.servicio;
                        this.nave_nombre.InnerText = fila.nave;
                        this.num_imo.InnerText = fila.imo;
                        this.nave_flag.InnerText = fila.bandera;
                        this.nave_eslora.InnerText = fila.eslora;
                        this.nave_neto.InnerText = fila.neto;
                        this.nave_bruto.InnerText = fila.bruto;
                        this.nave_sign.InnerText = fila.signal;
                        this.nave_tipo.InnerText = fila.tipo;
                        this.nave_in.InnerText = fila.viajeIn;
                        this.nave_out.InnerText = fila.viajeOut;
                        pbip_num.InnerText = fila.pnumero;
                        pbip_hasta.InnerText = fila.validez;
                        pbip_pro.InnerText = fila.provisonal;
                        pbip_seguridad.InnerText = fila.nivel;
                        pto_ultimo.InnerText = fila.ultimo;
                        pto_proximo.InnerText = fila.proximo;
                        adu_impo.InnerText = fila.imrn;
                        adu_expo.InnerText = fila.emrn;
                        apg_anio.InnerText = fila.anio;
                        apg_registro.InnerText = fila.registro;

                       // ope_etb.InnerText = fila.etb;
                        //ope_eta.InnerText = fila.eta;

                        ope_etb.InnerText = fila.eta;
                        ope_eta.InnerText = fila.etb;

                        ope_etd.InnerText = fila.ets;

                        ope_ata.InnerText = fila.ata;
                        ope_atd.InnerText = fila.ats;
                        ope_uso.InnerText = fila.uso;
                        this.xestado.InnerText = string.Empty;
                  
                        this.fechagenera.InnerText = string.Format("{0}", fila.fecha);
                        this.fechaimprime.InnerText = string.Format("{0}, {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());

                        this.operacion.InnerText = fila.tipooperacion;
                        this.fecembarque.InnerText = fila.embarqueplanificado;
                        this.fecbanano.InnerText = fila.cutoffbbk;


                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log<Exception>(ex, "printaisv", "Page_Load", sid, User.Identity.Name);
                        string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        base.Response.Write(close);
                    }
                }
            }
        }
    }
}