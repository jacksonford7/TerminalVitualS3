using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;

namespace CSLSite
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
                var number= log_csl.save_log<Exception>(ex, "printaisv", "Page_Init", sid, User.Identity.Name);
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
                        var tabla = new Catalogos.AisvResultDataTable();
                        var ta = new CatalogosTableAdapters.AisvResultTableAdapter();
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
                        this.propietario.InnerText = string.Format("[{2}]- Reservado por:{0}/{1}", fila.IsshiperNull() ? "No establecido" : fila.shiper, fila.usuario, fila.bookin);
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", sid);

                        if (fila.IsdetalleNull() || fila.detalle==false)
                        {
                            if (fila.tipo == "C" && fila.mov == "E") { full.InnerText = "( X )"; }
                            else if (fila.tipo == "S" && fila.mov == "E") { csuelta.InnerText = "( X )"; }
                            else if (fila.tipo == "S" && fila.mov == "C") { consolidado.InnerText = "( X )"; }
                        }
                        else
                        {
                            this.multiple.InnerText = "( X )";
                        }

                        referencia.InnerText = fila.referencia;
                        buque.InnerText = fila.nave;
                        eta.InnerText = !fila.IsetaNull() ? fila.eta.ToString("dd/MM/yyyy HH:mm") : "-";
                        cutof.InnerText = !fila.IscutoffNull() ? fila.cutoff.ToString("dd/MM/yyyy HH:mm") : "-";
                        uis.InnerText = !fila.IsuisNull() ? fila.uis.ToString("dd/MM/yyyy HH:mm") : "-";
                        agencia.InnerText = !fila.IsagenciaNull() ? fila.agencia : "No encontrado!";
                        this.descarga.InnerText = !fila.IspodNull() ? fila.pod : "-";
                        this.final.InnerText = !fila.Ispod1Null() ? fila.pod1 : "-";
                        this.cubierta.InnerText = !fila.IsdeckNull() && fila.deck ? "SI" : "NO";
                        this.tamano.InnerText = !fila.IstamanoNull() && fila.tamano.Trim().Length > 0 ? fila.tamano + "\"" : "-";
                        this.tipo.InnerText = !fila.IsisoNull() ? fila.iso : "-";
                        this.imos.InnerText = !fila.IsimoNull() && fila.imo ? "( X )" : "(  )";
                        this.refers.InnerText = !fila.IsreeferNull() && fila.reefer ? "( X )" : "(  )";
                        this.sobredimension.InnerText = !fila.IsoverdimNull() && fila.overdim ? "( X )" : "(  )";
                        this.notas.InnerText = !fila.IsnotaNull() ? fila.nota : string.Empty;
                        this.agente.InnerText = !fila.IsagentIdNull() ? fila.agentId : "No encontrado!";
                        
                       this.regla.InnerText = !fila.IsreglaNull() ? fila.regla : "-";
                        this.container.InnerText = !fila.IscontainerNull() ? fila.container : "-";
                        this.tara.InnerText = !fila.IstaraNull() ? fila.tara.ToString() : "-";
                        this.payload.InnerText = !fila.IspayloadNull() ? fila.payload.ToString() : "-";
                        this.producto.InnerText = fila.comodity;
                        var pesso = "-";
                        if (!fila.IspesoNull() && fila.tipo!=null)
                        {
                            if (fila.tipo == "C")
                            {
                                if (fila.detalle)
                                {
                                    pesso = (fila.peso / 1000).ToString("G") + " ton.";
                                }
                                else
                                {
                                    pesso = fila.peso.ToString("G") + " ton.";
                                }
                            }
                            else
                            {
                                pesso = fila.peso.ToString("G") + " kg.";
                            }
                                
                        }
                        this.peso.InnerText = pesso;
                        this.bultos.InnerText = !fila.IsbultosNull() ? fila.bultos.ToString() +" u." : "-";
                        this.peligro.InnerText = !fila.IspeligrosidadNull() ? fila.peligrosidad : "-";
                        this.embalar.InnerText = !fila.IsembalajeNull() ? fila.embalaje : "-";
                        this.refrigera.InnerText = !fila.IsidrefriNull() ? fila.idrefri : "-";
                        this.humedad.InnerText = !fila.IshumeNull() ? fila.hume : "-";
                        this.temp.InnerText = !fila.IstempeNull() ? fila.tempe.ToString("0.0") +"°C" : "-";
                        this.ventilacion.InnerText = !fila.IsventiNull() ? fila.venti : "-";
                        this.depot.InnerText = !fila.IsdepositoNull() ? fila.deposito : "-";
                        this.fechadepot.InnerText = !fila.IsdepositoNull() && !fila.IsdepotimeNull() ? fila.depotime.ToString("dd/MM/yyyy HH:mm") : string.Empty;
                        this.seal1.InnerText = !fila.IssagenciaNull() ? fila.sagencia : "-";
                        this.seal2.InnerText = !fila.IssventilacionNull() ? fila.sventilacion : "-";
                        this.seal3.InnerText = !fila.Issop1Null() ? fila.sop1 : "-";
                        this.seal4.InnerText = !fila.Issop2Null() ? fila.sop2 : "-";
                        this.izquierda.InnerText = !fila.IsexizqNull() ? fila.exizq.ToString(): "-";
                        this.derecha.InnerText = !fila.IsexderNull() ? fila.exder.ToString(): "-";
                        this.frontal.InnerText = !fila.IsexfroNull() ? fila.exfro.ToString(): "-";
                        this.superior.InnerText = !fila.IsextopNull() ? fila.extop.ToString(): "-";
                        this.atras.InnerText = !fila.IsexbackNull() ? fila.exback.ToString(): "-";
                        this.ubicacion.InnerText = !fila.IsplantaNull() ? fila.planta : "No encontrado!";
                        this.salida.InnerText = fila.plantime !=null ? fila.plantime.ToString("dd/MM/yyyy HH:mm") : "!No encontrado";
                        this.cedula.InnerText = !fila.IscedulaNull() ? fila.cedula : "-";
                        this.conductor.InnerText = !fila.IschoferNull() ? fila.chofer : "-";
                        this.placa.InnerText = !fila.IsplacaNull() ? fila.placa : "-";
                        this.cabezal.InnerText = !fila.IscabezalNull() ? fila.cabezal : "-";
                        this.chasis.InnerText = !fila.IschasisNull() ? fila.chasis : "-";
                        this.tviaje.InnerText = !fila.IstviajeNull() ? fila.tviaje.ToString() + " hrs" : "-";
                        this.especial.InnerText = !fila.IsespecialNull() ? fila.especial : "-";
                        var pie = string.Empty;
                        var filaz = string.IsNullOrEmpty( fila.referencia)  ? "xx" : fila.referencia.Trim();
                        if (fila.tipo == "C" || filaz.Contains("CGS2008001"))
                        {
                            pie = fila.container;
                        }
                        else
                        {
                            pie = sid;
                        }
                        this.barcode2.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", pie);
                        anumber.InnerText = sid;
                        numcontenedor.InnerText = pie;

                        this.fechagenera.InnerText = string.Format("{0}", !fila.IsfechainNull() ? fila.fechain.ToString("dd/MM/yyyy HH:mm") : "-");
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));


                        //nuevo fechas y pesos...
                        this.fechaptoin.InnerText = !fila.IsfechainNull() ? fila.fechain.ToString("dd/MM/yyyy HH:mm") : "Pendiente!";
                        this.fechaptoout.InnerText = !fila.IsfechaoutNull() ? fila.fechaout.ToString("dd/MM/yyyy HH:mm") : "Pendiente!";
                        this.pesoneto.InnerText =((fila.pesoin - fila.pesoout - fila.tara)/1000).ToString();
                        this.pesoin.InnerText = (fila.pesoin/1000) .ToString();
                        this.pesoout.InnerText = (fila.pesoout/1000).ToString();
                        this.pesobruto.InnerText = "0";
                        this.pesobrutomax.InnerText = "0";
                        if (fila.estado != null)
                        { 
                           switch (fila.estado.ToLower())
                           {
                               case "a":
                                   xestado.InnerText = "ESTE DOCUMENTO NO ES VÁLIDO FUÉ ANULADO";
                                   break;
                               case "i":
                                   xestado.InnerText = "LA CARGA DE ESTE DOCUMENTO HA INGRESADO A LA TERMINAL";
                                   break;
                               case "r":
                                   xestado.InnerText = string.Format( "RE-IMPRESIÓN VÁLIDA: [{0}]",DateTime.Now);
                                   break;
                               case "s":
                                   xestado.InnerText = "LA UNIDAD DE ESTE DOCUMENTO ESTA FUERA DE LA TERMINAL";
                                   break;
                               default:
                                    xestado.InnerText = "EL ESTADO DE ESTE DOCUMENTO ES INDETERMINADO!";
                                   break;
                           }
                        }
                        if (!fila.IsdetalleNull() && fila.detalle && !fila.IsdocumentoNull())
                        {
                            this.documento.InnerHtml = string.Format("<a href='detalle.aspx?sid={1}' target='_blank'>{0}</a>", fila.documento,Request.QueryString["sid"]);
                        }
                        else
                        {
                            this.documento.InnerText = !fila.IsdocumentoNull() ? fila.documento : "No encontrado!";
                        }
                        this.entrega.InnerText = string.Format("{0}",!fila.IsedocNull() ?   fila.edoc +" | ["+  fila.edoc_estado+"]" : "Pendiente");

                        this.diferencia.InnerText = (fila.payload - (fila.tara / 1000) - ((fila.pesoin - fila.pesoout) / 1000)).ToString();
                        this.pesobruto.InnerText = ((fila.pesoin - fila.pesoout) / 1000).ToString();
                        this.pesobrutomax.InnerText = (fila.payload - (fila.tara / 1000)).ToString();

                        this.sresponde.InnerText = fila.IssellorNull()?string.Empty:fila.sellor;

                        this.trasnport.InnerText = fila.IstransporNull() ? string.Empty : fila.transpor;
                        this.tecacert.InnerText = !fila.IscertfumigaNull() ? fila.certfumiga : "-";
                        this.tecafecha.InnerText = !fila.IsfecFumigaNull() ? fila.fecFumiga.ToString("dd/MM/yyyy") : "-";

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