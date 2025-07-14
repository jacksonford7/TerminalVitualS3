using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;

namespace CSLSite
{
    public partial class printproforma_sav : System.Web.UI.Page
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
                var number = log_csl.save_log<Exception>(ex, "printproforma_sav", "Init", sid, Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                return;
            }

            try
            {
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de Proforma nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "printproforma_sav", "Init", sid, Request.UserHostAddress);
                    this.PersonalResponse( string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}",Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "printproforma_sav", "Page_Init", sid, User.Identity.Name);
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
                        usuario usn = new usuario();
                        usn = this.getUserBySesion();
                        if (usn == null)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "printproforma_sav", "Page_Load", sid, Request.UserHostAddress);
                            this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                            return;
                        }
                  
                        var tabla = new Catalogos.proformaResultDataTable();
                        var ta = new CatalogosTableAdapters.proformaResultTableAdapter();
                        sid = sid.Trim().Replace("\0", string.Empty);
                        Int64 secuencia = 0;
                        if(!Int64.TryParse(sid,out secuencia))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "printproforma_sav", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                            this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                            return;
                        }
                        ta.Fill(tabla, secuencia);
                        if (tabla.Rows.Count <= 0)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "printproforma_sav", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                            this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                            return;
                        }

                                             
                        var fila = tabla.FirstOrDefault();
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", fila.secuencia);
                        full.InnerText = "( X )";
                        this.boref.InnerText = fila.referencia;
                        this.bonum.InnerText = fila.bl;
                        this.bonom.InnerText = fila.contenedor;
                        this.depot.InnerText = fila.deposito_nombre;
                        


                        //nuevo 2017 si el usuario es de contado el nuevo numero.
                        if(!fila.IsEstadoNull())
                        {
                           
                            if (fila.Estado)
                            {
                                this.hoja.Attributes["class"] = "agua-copia";
                            }
                            else
                            {
                                this.hoja.Attributes["class"] = "agua-nulo";
                            }
                        }
                      
                        if ((!fila.IsliquidacionNull() && fila.liquidacion.Length > 1))
                        {
                            this.pro_li.InnerText = "LIQUIDACIÓN POR RETORNO DE CONTENEDORES VACÍOS";
                            this.servicio.InnerText = "Liquidación #:";
                        }

                        this.numprofpie.InnerHtml = string.Format("<strong>{0}</strong>", (!fila.IsliquidacionNull() && fila.liquidacion.Length > 1) ? fila.liquidacion : fila.secuencia);

                        this.bkcntr.InnerHtml = string.Format("<strong>{0}</strong>",fila.IscantidadNull()?0 : fila.cantidad);
                        this.bkres.InnerText = fila.reservas.ToString();
                        //Obtiene la informacion del cliente en forma de diccionario
                        var clienteInfo = app_start.Proforma.GetClientInfo(fila.ruc);
                        this.cliruc.InnerText = fila.ruc;
                        this.clinombre.InnerText = (clienteInfo!= null && clienteInfo.ContainsKey("nombre")) ? clienteInfo["nombre"] : "Desconocido";
                        this.tablaNueva.DataSource = tabla;
                        this.tablaNueva.DataBind();
                        this.barcode2.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", fila.secuencia);
                          //nuevo 2017 si el usuario es de contado el nuevo numero.
                        
                        anumber.InnerText = (!fila.IsliquidacionNull() && fila.liquidacion.Length>1)? fila.liquidacion:fila.secuencia;
                        this.sfecha_cliente.InnerText = fila.IsfechClienteNull() ? "-" : fila.fechCliente.ToString("dd/MM/yyyy");
                        //this.sfecha_zarpe.InnerText = fila.IsfechaZarpeNull()? "-" : fila.fechaZarpe.ToString("dd/MM/yyyy HH:mm");
                        
                        this.fechagenera.InnerText = string.Format("{0}", fila.fecha.ToString("dd/MM/yyyy HH:mm"));
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        var total = tabla.Sum(d => d.totalfila);
                        //stunit.InnerText = total.ToString("C");
                        this.subtotal.InnerText = string.Format("{0:c}", fila.subtotal);
                        this.desiva.InnerText = string.Format("IVA {0}%(+)", fila.IspcIVANull() ? 0 : (int)fila.pcIVA);
                        this.sttal.InnerText = string.Format("{0:c}",fila.totalpro);
                        //siva.InnerHtml = string.Format("{0:c}", fila.IsivaNull()? 0:fila.iva);
                        iva.InnerHtml = string.Format("{0:c}", fila.IsivaNull() ? 0 : fila.iva);

                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log<Exception>(ex , "printproforma_sav", "Page_Load", sid, User.Identity.Name);
                        string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        base.Response.Write(close);
                    }
                }
            }
        }
    }
}