using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;
using VBSEntidades;
using BillionEntidades;

namespace CSLSite
{
    public partial class impresion : System.Web.UI.Page
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

                if (!string.IsNullOrEmpty(sid))
                {
                    sid = sid.Replace("\0", string.Empty).Trim();
                }
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
                    this.tbaduana.Visible = false;
                    this.tbchofer.Visible = false;
                    this.tbsellos.Visible = false;
                    this.tbpeso.Visible = false;

                    var HorallegadaAntes = string.Empty;
                    var HorallegadaDespues = string.Empty;

                    try
                    {
                        VBS_CabeceraPlantilla conexionVBS = new VBS_CabeceraPlantilla();

                        var tabla = new Catalogos.AisvResultDataTable();
                        var ta = new CatalogosTableAdapters.AisvResultTableAdapter();
                        sid = sid.Trim().Replace("\0", string.Empty);
            //            sid = "320170501035";
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

                            if (fila.estado.Contains("A"))
                            {
                                this.hoja.Attributes["class"] = "agua-nulo";
                            }
                            else
                            {
                                this.hoja.Attributes["class"] = "agua-copia";
                            }
                      

                        if (fila.IsdetalleNull() || fila.detalle==false)
                        {
                            if (fila.tipo == "C" && fila.mov == "E")
                            {
                                full.InnerText = "( X )";

                                

                            }
                            else if (fila.tipo == "S" && fila.mov == "E")
                            {
                                csuelta.InnerText = "( X )";

                               

                            }
                            else if (fila.tipo == "S" && fila.mov == "C")
                            {
                                consolidado.InnerText = "( X )";

                             
                            }
                        }
                        else
                        {
                            this.multiple.InnerText = "( X )";
                          
                        }

                        if (!fila.IslateNull())
                        {
                            this.tardio.InnerText = fila.late ? "( X )" : "(   )"; ;
                        }


                        HorallegadaAntes = conexionVBS.GetParametrosValida("holgura_ini");
                        HorallegadaDespues = conexionVBS.GetParametrosValida("holgura_fin");

                        /*var HorallegadaAntes = conexionVBS.GetParametrosValida("holgura_ini");
                        var HorallegadaDespues = conexionVBS.GetParametrosValida("holgura_fin");*/

                        //HorallegadaAntes = fila.vbs_destino.ToUpper() == "MUELLE" ? conexionVBS.GetParametrosValida("holgura_ini_mue") : conexionVBS.GetParametrosValida("holgura_ini_bod");
                        //HorallegadaDespues = fila.vbs_destino.ToUpper() == "MUELLE" ? conexionVBS.GetParametrosValida("holgura_fin_mue") : conexionVBS.GetParametrosValida("holgura_fin_bod");


                        TimeSpan tiempoRestado = DateTime.Now.TimeOfDay;
                        TimeSpan tiempoAgregado = DateTime.Now.TimeOfDay;
                        TimeSpan horaLlegadaDespues;
                        TimeSpan horaLlegadaAntes;
                        try
                        {
                            horaLlegadaAntes = fila.aisv_hora_llegada_turno;
                            tiempoRestado = horaLlegadaAntes.Subtract(TimeSpan.FromMinutes(Convert.ToInt32(HorallegadaAntes)));
                            if (tiempoRestado.TotalHours < 0) // Si el resultado es negativo, agregamos un día (24 horas)
                            {
                                tiempoRestado = tiempoRestado.Add(TimeSpan.FromHours(24));
                            }

                            horaLlegadaDespues = fila.aisv_hora_llegada_turno;
                            tiempoAgregado = horaLlegadaDespues.Add(TimeSpan.FromMinutes(Convert.ToInt32(HorallegadaDespues)));
                            tiempoAgregado = tiempoAgregado.Add(TimeSpan.FromHours(-24 * Math.Floor(tiempoAgregado.TotalDays))); // Ajustamos el resultado a un máximo de 24 horas

                            this.horaLlegadAntes.InnerText = tiempoRestado.ToString(@"hh\:mm\:ss");
                            this.horaLlegadaDespues.InnerText = tiempoAgregado.ToString(@"hh\:mm\:ss");

                            this.FechLlegaTurno.InnerText = string.Format("{0}", !fila.IsfechainNull() ? fila.aisv_fecha_llegada_turno1.ToString("dd/MM/yyyy") : "-");
                        }
                        catch
                        {
                            this.horaLlegadAntes.InnerText = "";
                            this.horaLlegadaDespues.InnerText = "";
                            this.FechLlegaTurno.InnerText = "";
                        }

                        //TimeSpan horaLlegadaAntes = fila.aisv_hora_llegada_turno;
                        //TimeSpan tiempoRestado = horaLlegadaAntes.Subtract(TimeSpan.FromMinutes(Convert.ToInt32(HorallegadaAntes)));
                        //TimeSpan horaLlegadaDespues = fila.aisv_hora_llegada_turno;
                        //TimeSpan tiempoAgregado = horaLlegadaDespues + TimeSpan.FromMinutes(Convert.ToInt32(HorallegadaDespues));

                        //turnos banano
                        if (fila.vbs_id_hora_cita != 0)
                        {
                            if (fila.vbs_destino.ToUpper() == "MUELLE")
                            {
                                string cMensajes;
                                Cls_CFS_Turnos_Banano ObjTurnos = new Cls_CFS_Turnos_Banano();
                                ObjTurnos.idLoadingDet = fila.vbs_id_hora_cita;

                                if (!ObjTurnos.PopulateMyData(out cMensajes))
                                {
                                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Sin turno de banano", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                                    var number = log_csl.save_log<Exception>(ex, "printaisv", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                                    this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                                    return;
                                }
                                else
                                {
                                    this.horaLlegadAntes.InnerText = string.Format("{0}       \t {1}", fila.vbs_destino, ObjTurnos.horaInicio);
                                    this.horaLlegadaDespues.InnerText = ObjTurnos.horaFin;
                                    this.FechLlegaTurno.InnerText = ObjTurnos.fecha.Value.ToString("dd/MM/yyyy");
                                }
                            }

                            if (fila.vbs_destino.ToUpper() == "BODEGA")
                            {
                                string cMensajes;
                                Cls_CFS_Turnos_Banano ObjTurnos = new Cls_CFS_Turnos_Banano();
                                ObjTurnos.idStowagePlanTurno = fila.vbs_id_hora_cita;

                                if (!ObjTurnos.PopulateMyDataBodega(out cMensajes))
                                {
                                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Sin turno de banano", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                                    var number = log_csl.save_log<Exception>(ex, "printaisv", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                                    this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                                    return;
                                }
                                else
                                {
                                    //this.horaLlegadAntes.InnerText = ObjTurnos.horaInicio;
                                    string v_Ubicacion = string.Empty;
                                    try
                                    {
                                        var oTurno = VBSEntidades.BananoBodega.BAN_Stowage_Plan_Turno.GetEntidad(fila.vbs_id_hora_cita);
                                        var oDetalle = VBSEntidades.BananoBodega.BAN_Stowage_Plan_Det.GetEntidad(oTurno.idStowageDet);
                                        oDetalle.oBloque = VBSEntidades.BananoBodega.BAN_Catalogo_Bloque.GetEntidad(int.Parse(oDetalle.idBloque.ToString()));
                                        oDetalle.oBodega = VBSEntidades.BananoBodega.BAN_Catalogo_Bodega.GetEntidad(int.Parse(oDetalle.idBodega.ToString()));
                                        v_Ubicacion = oDetalle.oBodega.nombre.Trim() + "|" + oDetalle.oBloque.nombre.Trim();
                                    }
                                    catch
                                    {
                                        v_Ubicacion = string.Empty;
                                    }

                                    this.horaLlegadAntes.InnerText = string.Format("{0}       \t {1} \n {2}", fila.vbs_destino, ObjTurnos.horaInicio, v_Ubicacion);
                                    this.horaLlegadaDespues.InnerText = ObjTurnos.horaFin;
                                    this.FechLlegaTurno.InnerText = ObjTurnos.fecha.Value.ToString("dd/MM/yyyy");
                                }
                            }
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
                        //this.FechLlegaTurno.InnerText = string.Format("{0}", !fila.IsfechainNull() ? fila.aisv_fecha_llegada_turno1.ToString("dd/MM/yyyy") : "-");
                        //this.horaLlegadAntes.InnerText = tiempoRestado.ToString();
                        //this.horaLlegadaDespues.InnerText = tiempoAgregado.ToString();
                        this.tipo.InnerText = !fila.IsisoNull() ? fila.iso : "-";
                        this.imos.InnerText = !fila.IsimoNull() && fila.imo ? "( X )" : "(  )";
                        this.refers.InnerText = !fila.IsreeferNull() && fila.reefer ? "( X )" : "(  )";
                        this.sobredimension.InnerText = !fila.IsoverdimNull() && fila.overdim ? "( X )" : "(  )";
                        this.notas.InnerText = !fila.IsnotaNull() ? fila.nota : string.Empty;
                        this.agente.InnerText = !fila.IsagentIdNull() ? fila.agentId : "No encontrado!";
                        //2017-->PRINT
                        this.diplo.InnerText = !fila.IsdiplomaticoNull() && fila.diplomatico ? "( X )" : "(  )";
                        this.consignata.InnerText = fila.IsconsignatarioNull() ? "-" : fila.consignatario;
                        //---------->
                        
                        this.regla.InnerText = !fila.IsreglaNull() ? fila.regla : "-";
                        this.container.InnerText = !fila.IscontainerNull() ? fila.container : "-";
                        this.tara.InnerText = !fila.IstaraNull() ? fila.tara.ToString() : "-";
                        this.payload.InnerText = !fila.IspayloadNull() ? fila.payload.ToString() : "-";
                        this.producto.InnerText = fila.comodity;

                        this.tipocarga.InnerText = !fila.IsisoNull() ? fila.id_tipocarga : "-";
                        this.numero_bl.InnerText = !fila.IsisoNull() ? fila.aisv_numero_bl : "-";

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
                       
                        
                        this.seal1.InnerText = !fila.IssagenciaNull() ?  DataTransformHelper.hideData(fila.sagencia)   : "-";
                        this.seal2.InnerText = !fila.IssventilacionNull() ? DataTransformHelper.hideData(fila.sventilacion) : "-";
                        this.seal3.InnerText = !fila.Issop1Null() ? DataTransformHelper.hideData(fila.sop1) : "-";
                        this.seal4.InnerText = !fila.Issop2Null() ? DataTransformHelper.hideData(fila.sop2) : "-";
                      
                        
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

                     
                        this.zonaespecial.InnerText = !fila.IsimoNull() && fila.aisv_servicio_especial ? "( SI )" : "( NO )";

                        this.barcode2.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", pie);
                        anumber.InnerText = sid;
                        numcontenedor.InnerText = pie;

                        this.fechagenera.InnerText = !fila.IsfechainNull() ? fila.fechain.ToString("dd/MM/yyyy HH:mm") : "-";
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                        //nuevo fechas y pesos...
                      
                        this.fechaptoin.InnerText = !fila.IsfechaentNull() ? fila.fechaent.ToString("dd/MM/yyyy HH:mm") : "-"; 
                        this.fechaptoout.InnerText = !fila.IsfechaoutNull() ? fila.fechaout.ToString("dd/MM/yyyy HH:mm") : "-";


                        this.tecacert.InnerText = !fila.IscertfumigaNull() ? fila.certfumiga : "-";
                        this.tecafecha.InnerText = !fila.IsfecFumigaNull() ? fila.fecFumiga.ToString("dd/MM/yyyy") : "-";

                       
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
                      //  this.entrega.InnerText = string.Format("{0}",!fila.IsedocNull() ?   fila.edoc +" | ["+  fila.edoc_estado+"]" : "Pendiente");
                        this.diferencia.InnerText = (fila.payload - (fila.tara / 1000) - ((fila.pesoin - fila.pesoout) / 1000)).ToString();
                        this.pesobruto.InnerText = ((fila.pesoin - fila.pesoout) / 1000).ToString();
                        this.pesobrutomax.InnerText = (fila.payload - (fila.tara / 1000)).ToString();
                        this.sresponde.InnerText = fila.IssellorNull()?string.Empty:fila.sellor;
                        this.trasnport.InnerText = fila.IstransporNull() ? string.Empty : fila.transpor;

                        if (!fila.IsproformaNull() && !string.IsNullOrEmpty(fila.proforma))
                        { 
                             //existe una proforma entonces traer la data y mostrar las tablas adicionales
                            if (!fila.IscontainerNull() && !string.IsNullOrEmpty(fila.container))
                            {
                                var lst = jAisvContainer.UnitRealInfo(sid, fila.container);
                                if (lst != null && lst.Count >  0)
                                {
                                    //empecemos con los sellos
                                    this.cgsas1.InnerText = lst.ContainsKey("sello1") ? lst["sello1"] : "...";
                                    this.cgsas2.InnerText = lst.ContainsKey("sello2") ? lst["sello2"] : "...";
                                    this.cgsas3.InnerText = lst.ContainsKey("sello3") ? lst["sello3"] : "...";
                                    this.cgsas4.InnerText = lst.ContainsKey("sello4") ? lst["sello4"] : "...";
                                    this.sellocgsa.InnerText = lst.ContainsKey("sealcgsa") ? lst["sealcgsa"] : "...";
                                    //datos del chofer
                                    this.cgsachofer.InnerText = lst.ContainsKey("conductor") ? lst["conductor"] : "...";
                                    this.cgsalicencia.InnerText = lst.ContainsKey("licencia") ? lst["licencia"] : "...";
                                    this.cgsaplaca.InnerText = lst.ContainsKey("placa") ? lst["placa"] : "...";
                                    //aduana
                                    this.cgsaentrega.InnerText = lst.ContainsKey("noentrega") ? lst["noentrega"] : "...";
                                    this.tbaduana.Visible = true;
                                    this.tbchofer.Visible = true;
                                    this.tbsellos.Visible = true;
                                    this.tbpeso.Visible = true;
                                }
                            }
                        }
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