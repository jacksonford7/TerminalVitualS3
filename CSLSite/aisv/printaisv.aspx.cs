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
    public partial class printaisv : System.Web.UI.Page
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
                var number = log_csl.save_log<Exception>(ex, "printaisv", "Init", sid, Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            try
            {
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "printaisv", "Init", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "printaisv", "Page_Init", sid, User.Identity.Name);
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
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=400&height=60&size=50", sid);

                        if (fila.IsdetalleNull() || fila.detalle == false)
                        {
                            if (fila.tipo == "C" && fila.mov == "E") { full.InnerText = "( X )"; }
                            else if (fila.tipo == "S" && fila.mov == "E") { csuelta.InnerText = "( X )"; }
                            else if (fila.tipo == "S" && fila.mov == "C") { consolidado.InnerText = "( X )"; }
                        }
                        else
                        {
                            this.multiple.InnerText = "( X )";
                        }

                        if (!fila.IslateNull())
                        {
                            this.tardio.InnerText = fila.late ? "( X )" : "(   )"; ;
                        }

                        VBS_CabeceraPlantilla conexionVBS = new VBS_CabeceraPlantilla();
                       // var HorallegadaAntes = fila.vbs_destino.ToUpper() == "MUELLE"? conexionVBS.GetParametrosValida("holgura_ini"): conexionVBS.GetParametrosValida("holgura_ini_bod");
                       // var HorallegadaDespues = fila.vbs_destino.ToUpper() == "MUELLE" ? conexionVBS.GetParametrosValida("holgura_fin"): conexionVBS.GetParametrosValida("holgura_ini_bod");
                        var HorallegadaAntes = string.Empty;
                        var HorallegadaDespues = string.Empty;

                        HorallegadaAntes = conexionVBS.GetParametrosValida("holgura_ini");
                        HorallegadaDespues = conexionVBS.GetParametrosValida("holgura_fin");

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
                                    //this.horaLlegadAntes.InnerText = ObjTurnos.horaInicio;
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

                                    this.horaLlegadAntes.InnerText = string.Format("[{0}]       \t {1} \n {2}", fila.vbs_destino, ObjTurnos.horaInicio, v_Ubicacion);
                                    this.horaLlegadaDespues.InnerText = ObjTurnos.horaFin;
                                    this.FechLlegaTurno.InnerText = ObjTurnos.fecha.Value.ToString("dd/MM/yyyy");
                                }
                            }
                        }

                        this.dae.InnerText = !fila.IsdocumentoNull() ? fila.documento : "No disponible";
                        referencia.InnerText = fila.referencia;
                        buque.InnerText = fila.nave;
                        eta.InnerText = !fila.IsetaNull() ? fila.eta.ToString("dd/MM/yyyy HH:mm") : "-";
                        cutof.InnerText = !fila.IscutoffNull() ? fila.cutoff.ToString("dd/MM/yyyy HH:mm") : "-";
                        uis.InnerText = !fila.IsuisNull() ? fila.uis.ToString("dd/MM/yyyy HH:mm") : "-";
                        agencia.InnerText = !fila.IsagenciaNull() ? fila.agencia : "No encontrado!";
                        
                        
                        this.tamano.InnerText = !fila.IstamanoNull() && fila.tamano.Trim().Length > 0 ? fila.tamano + "\"" : "-";
                        this.tipo.InnerText = !fila.IsisoNull() ? fila.iso : "-";
                        this.imos.InnerText = !fila.IsimoNull() && fila.imo ? "( X )" : "(  )";
                        this.refers.InnerText = !fila.IsreeferNull() && fila.reefer ? "( X )" : "(  )";
                        this.sobredimension.InnerText = !fila.IsoverdimNull() && fila.overdim ? "( X )" : "(  )";
                        this.container.InnerText = !fila.IscontainerNull() ? fila.container : "-";
                        this.payload.InnerText = !fila.IspayloadNull() ? fila.payload.ToString() : "-";

                        this.tipocarga.InnerText = !fila.IsisoNull() ? fila.id_tipocarga : "-";
                        this.numero_bl.InnerText = !fila.IsisoNull() ? fila.aisv_numero_bl : "-";

                        var pesso = "-";
                        if (!fila.IspesoNull() && fila.tipo != null)
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

                        this.propietario.InnerText = string.Format("* Numero:{2}, Reservado por:{0}/{1}", fila.IsshiperNull() ? "No establecido" : fila.shiper, fila.usuario, fila.bookin);
                        this.peso.InnerText = pesso;
                        this.bultos.InnerText = !fila.IsbultosNull() ? fila.bultos.ToString() + " u." : "-";
                        this.peligro.InnerText = !fila.IspeligrosidadNull() ? fila.peligrosidad : "-";
                        this.embalar.InnerText = !fila.IsembalajeNull() ? fila.embalaje : "-";
                        this.seal1.InnerText = !fila.IssagenciaNull() ? DataTransformHelper.hideData(fila.sagencia) : "-";
                        this.seal2.InnerText = !fila.IssventilacionNull() ? DataTransformHelper.hideData(fila.sventilacion) : "-";
                        this.cedula.InnerText = !fila.IscedulaNull() ? fila.cedula : "-";
                        this.conductor.InnerText = !fila.IschoferNull() ? fila.chofer : "-";
                        this.placa.InnerText = !fila.IsplacaNull() ? fila.placa : "-";

                        var pie = string.Empty;
                        this.producto.InnerText = fila.IscomodityNull() ? "No disponible" : fila.comodity;
                        var filaz = string.IsNullOrEmpty(fila.referencia) ? "xx" : fila.referencia;

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

                        this.fechagenera.InnerText = string.Format("{0}", !fila.IsfechainNull() ? fila.fechain.ToString("dd/MM/yyyy HH:mm") : "-");
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));


                        //this.diferencia.InnerText = (fila.payload - (fila.tara / 1000) - ((fila.pesoin - fila.pesoout) / 1000)).ToString();
                        //this.pesobruto.InnerText = ((fila.pesoin - fila.pesoout) / 1000).ToString();
                        //this.pesobrutomax.InnerText = (fila.payload - (fila.tara / 1000)).ToString();

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