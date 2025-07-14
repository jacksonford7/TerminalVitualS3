using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using Salesforces;
using System.Data;
using System.Web.Script.Services;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using PasePuerta;
using CSLSite;

using ClsOrdenesP2D;

namespace CSLSite
{
  

    public partial class emisionpasebrbk : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaBRBK_Cabecera objPaseBRBK = new Cls_Bil_PasePuertaBRBK_Cabecera();
        private Cls_Bil_PasePuertaBRBK_SubItems objPaseBRBKTarja = new Cls_Bil_PasePuertaBRBK_SubItems();
        private Cls_Bil_PasePuertaBRBK_Detalle objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();

    
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }

        private Cls_Bil_PasePuertaBRBK_Temporal objReserva ;
        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();
        private brbk_valida_solicitud objValidaSolicitud = new brbk_valida_solicitud();

        //private P2D_Traza_Liftif objLogLiftif = new P2D_Traza_Liftif();
        //private P2D_Actualiza_PasePuerta objActualiza_Pase = new P2D_Actualiza_PasePuerta();
        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;
        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;

        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        private string TipoServicio = string.Empty;
        private bool EsPasesinTurno = false;
       
        private DateTime FechaFacturaHasta;
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
        private string Tipo_Contenedor = string.Empty;
        private DateTime FechaActualSalida;
        private string ContenedorSelec = string.Empty;
        private string EmpresaSelect = string.Empty;
        private string ChoferSelect = string.Empty;
        private string PlacaSelect = string.Empty;
        private string TurnoSelect = string.Empty;
        private DateTime FechaTurnoInicio;
        private DateTime FechaTurnoFinal;
        private Int64 ConsecutivoSelec = 0;
        private static string TextoLeyenda = string.Empty;

        #endregion

        #region "Propiedades"

        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }

        }

       
        #endregion

        #region "Metodos Web Services"


        [System.Web.Services.WebMethod]
        public static string[] GetEmpresas(string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Transportista = N4.Entidades.CompaniaTransporte.ObtenerCompanias(ClsUsuario.loginname, prefix);
                if (Transportista.Exitoso)
                {
                    var LinqQuery = (from Tbl in Transportista.Resultado.Where(Tbl => Tbl.ruc != null)
                                        select new
                                        {
                                            EMPRESA = string.Format("{0} - {1}", Tbl.ruc.Trim(), Tbl.razon_social.Trim()),
                                            RUC = Tbl.ruc.Trim(),
                                            NOMBRE = Tbl.razon_social.Trim(),
                                            ID = Tbl.ruc.Trim()
                                        });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        [System.Web.Services.WebMethod]
        public static string[] GetChofer(string idempresa, string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Chofer = N4.Entidades.Chofer.ObtenerChoferes(ClsUsuario.loginname, String.Empty, idempresa);
                if (Chofer.Exitoso)
                {
                    var LinqQuery = (from Tbl in Chofer.Resultado.Where(Tbl => Tbl.numero != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0} - {1}", Tbl.numero.Trim(), Tbl.nombres.Trim()),
                                         RUC = Tbl.numero.Trim(),
                                         NOMBRE = Tbl.nombres.Trim(),
                                         ID = Tbl.numero.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }
      
                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        [System.Web.Services.WebMethod]
        public static string[] GetPlaca(string idempresa, string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Camion = N4.Entidades.Camion.ObtenerCamiones(ClsUsuario.loginname, prefix, idempresa);
                if (Camion.Exitoso)
                {
                    var LinqQuery = (from Tbl in Camion.Resultado.Where(Tbl => Tbl.numero != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0}", Tbl.numero.Trim()),
                                         RUC = Tbl.numero.Trim(),
                                         NOMBRE = Tbl.numero.Trim(),
                                         ID = Tbl.numero.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        #endregion


        #region "Metodos"

        private void Enviar_Caso_Salesforce(string pUsuario, string pruc, string pModulo, string pNovedad, string pErrores, string pValor1, string pValor2, string pValor3, out string Mensaje, bool bloqueo = false)
        {
            /*************************************************************************************************************************************
            * crear caso salesforce
            * **********************************************************************************************************************************/
            Mensaje = string.Empty;

            try
            {

                Salesforces.Ticket tk = new Ticket();

                tk.Tipo = "ERROR"; //debe ser: Error, Sugerencia, Queja, Problema, Otros
                tk.Categoria = "IMPO"; //solo puede ser: Impo,Expo,Otros
                tk.Usuario = pUsuario.Trim(); //login
                tk.Ruc = pruc.Trim(); //login ruc
                tk.PalabraClave = "CasoImpo"; //Opcional es una palabra clave para agrupar
                tk.Copias = "desarrollo@cgsa.com.ec";//opcional es para enviar copia de respaldo
                tk.Aplicacion = "Billion"; //obligatorio
                tk.Modulo = pModulo;//opcional

                var detalle_carga = new SaleforcesContenido();
                detalle_carga.Categoria = TipoCategoria.Importacion; //opcional
                detalle_carga.Tipo = TipoCarga.BRBK; //opcional
                detalle_carga.Titulo = "Modulo de Facturación Importación BREAK BULK"; //opcional
                detalle_carga.Novedad = pNovedad; //mensaje del modulo o error

                detalle_carga.Detalles.Add(new DetalleCarga("Errores:", MensajesErrores));

                if (!string.IsNullOrEmpty(pValor1)) { detalle_carga.Detalles.Add(new DetalleCarga("BL", pValor1)); }
                if (!string.IsNullOrEmpty(pValor2)) { detalle_carga.Detalles.Add(new DetalleCarga("Cliente", pValor2)); }
                if (!string.IsNullOrEmpty(pValor3)) { detalle_carga.Detalles.Add(new DetalleCarga("Agente", pValor3)); }

                //asi puedes poner los campos que desees o se necesiten sobre la carga

                tk.Contenido = detalle_carga.ToString();

                var rt = tk.NuevoCaso();
                if (rt.Exitoso)
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de Tesorería  para que realicen las respectivas revisiones del caso generado #  {0} ...Casilla de atención: tesoreia@cgsa.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del caso generado #  {0} ...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                }
                else
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de Tesorería  para que realicen las respectivas revisiones del problema {0} ...Casilla de atención: tesoreia@cgsa.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.", rt.MensajeProblema);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del problema {0} ....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00. ", rt.MensajeProblema);
                    }
                }

            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;

            }


            /*************************************************************************************************************************************
            * fin caso salesforce
            * **********************************************************************************************************************************/

        }


        private void Actualiza_Paneles()
        {
            UPCARGA.Update();
            UPCANTIDADRETIRAR.Update();
            
            UPBOTONES.Update();
            UPCONTENEDOR.Update();
            UPRETIRADOS.Update();
            UPSALDO.Update();
            UPBODEGA.Update();
            UPPRODUCTO.Update();
            UPFECHASALIDA.Update();
            UPTURNO.Update();
            UPCAS.Update();
            this.UPSALDOFINAL.Update();
            this.UPACTUAL.Update();
            UPAGREGATURNO.Update();
            this.UPPAGADO.Update();
            this.UPTEXTO.Update();

        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
            this.TXTMSN.Text = string.Empty;
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                this.TXTHSN.Text = string.Format("{0}", "0000");
            }
            this.Txtempresa.Text = string.Empty;
            this.TxtChofer.Text = string.Empty;
            this.TxtPlaca.Text = string.Empty;
            this.TxtPagado.Text = string.Empty;
            this.TxtFechaHasta.Text = string.Empty;
            this.TxtContenedorSeleccionado.Text = string.Empty;
            this.TxtRetirados.Text = string.Empty;
            this.TxtSaldo.Text = string.Empty;
            this.TxtBodega.Text = string.Empty;
            this.TxtTipoProducto.Text = string.Empty;
            this.TxtSaldoFinal.Text = string.Empty;
            this.TxtActual.Text = string.Empty;

            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();

            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });

            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();

            this.Actualiza_Paneles();

        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");
           
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("2");
      
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objPaseBRBK = new Cls_Bil_PasePuertaBRBK_Cabecera();
            Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
        }

        private void Turno_Default()
        {
            //turno por defecto
            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Seleccione *" });
            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();
        }

        private void Pase_Sin_Turno_Default()
        {
            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Pase Sin turno *" });
            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();
        }

       
    

        #endregion

      

        #region "Eventos del Formulario"

        #region "Seleccionar todos"
        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {

            //if (Response.IsClientConnected)
            //{
            //    try
            //    {
            //        if (HttpContext.Current.Request.Cookies["token"] == null)
            //        {
            //            System.Web.Security.FormsAuthentication.SignOut();
            //            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            //            Session.Clear();
            //            OcultarLoading("1");
            //            return;
            //        }

            //        bool ChkEstado = this.ChkTodos.Checked;
            //        CultureInfo enUS = new CultureInfo("en-US");
            //        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

            //        objPaseBRBK = Session["PaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
            //        if (objPaseBRBK == null)
            //        {
            //            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todos los subitems o bultos... </b>"));
            //            return;
            //        }
            //        if (objPaseBRBK.DetalleSubItem.Count == 0)
            //        {
            //            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, para poder seleccionar... </b>"));
            //            return;
            //        }

            //        var LinqValidaSubItemsFaltantes = (from p in objPaseBRBK.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI"))
            //                                           select p.CONSECUTIVO).ToList();

            //        if (LinqValidaSubItemsFaltantes.Count > 0)
            //        {
            //            //proceso de marcar subitems
            //            foreach (var Det in objPaseBRBK.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI")))
            //            {
            //                ConsecutivoSelec = Det.CONSECUTIVO.Value;
            //                var Detalle = objPaseBRBK.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
            //                if (Detalle != null)
            //                {
            //                    Detalle.VISTO = ChkEstado;
                               
            //                }
            //            }

            //            //tablePagination_Tarja.DataSource = objPaseBRBK.DetalleSubItem.Where(p => string.IsNullOrEmpty(p.MARCADO_SUBITEMS));
            //            //tablePagination_Tarja.DataBind();

            //        }


            //        Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

            //        //this.Pintar_Grilla();

            //    }
            //    catch (Exception ex)
            //    {
            //        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ChkTodos_CheckedChanged), "ChkTodos_CheckedChanged", false, null, null, ex.StackTrace, ex);
            //        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
            //        this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
            //        return;

            //    }
            //}
               
        }
        #endregion


        #region "Eventos del turno"

        protected void TxtFechaHasta_TextChanged(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    CultureInfo enUS = new CultureInfo("en-US");

                    this.Ocultar_Mensaje();

                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();

                    if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        this.Turno_Default();
                        this.Actualiza_Paneles();
                        return;
                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    objPaseBRBK = Session["PaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar la fecha del turno... </b>"));
                        return;
                    }
                    if (objPaseBRBK.CNTR_CANTIDAD_SALDO == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe saldo de bultos disponibles, para poder seleccionar la fecha del turno... </b>"));
                        return;
                    }

                    HoraHasta = "00:00";
                    Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase.. Mes/Día/año</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    FechaFacturaHasta = objPaseBRBK.FECHA_SALIDA.Value;

                    if (FechaActualSalida.Date < System.DateTime.Now.Date)
                    {
                        this.Turno_Default();
                       // this.TxtFechaHasta.Text = objPaseBRBK.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), System.DateTime.Now.ToString("MM/dd/yyyy")));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (FechaActualSalida.Date > FechaFacturaHasta.Date)
                    {
                        this.Turno_Default();
                        //this.TxtFechaHasta.Text = objPaseBRBK.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), FechaFacturaHasta.ToString("MM/dd/yyyy")));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (FechaFacturaHasta.Date == System.DateTime.Today.Date)
                    {
                        this.Turno_Default();
                        //this.TxtFechaHasta.Text = System.DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha del turno, no puede ser igual a la fecha actual</b>" ));
                        this.TxtFechaHasta.Focus();
                    }

                    //if (FechaActualSalida.Date == FechaFacturaHasta.Date)
                    //{
                    //    this.Turno_Default();
                    //    this.TxtFechaHasta.Text = objPaseBRBK.FECHA_SALIDA.Value.AddDays(-1).ToString("MM/dd/yyyy");
                    //    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, debe ser un día antes que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), FechaFacturaHasta.ToString("MM/dd/yyyy")));
                    //    this.TxtFechaHasta.Focus();
                    //    return;
                    //}


                    //verificar si es pase sin turno
                    var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, objPaseBRBK.MRN, objPaseBRBK.MSN, objPaseBRBK.HSN);
                    if (PaseSinturno.Exitoso)
                    {
                        if (PaseSinturno.Resultado)
                        {
                            EsPasesinTurno = true;
                        }
                        else
                        {
                            EsPasesinTurno = false;
                        }

                    }
                    else
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseBRBK.CARGA, PaseSinturno.MensajeProblema));
                    }

                    //si es pase sin turno
                    if (EsPasesinTurno)
                    {
                        this.Pase_Sin_Turno_Default();
                    }
                    else
                    {
                       
                        var Turnos = PasePuerta.TurnoBRBK.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, objPaseBRBK.CNTR_UBICACION, objPaseBRBK.idProducto.Value);
                        if (Turnos.Exitoso)
                        {
                            //turno por defecto
                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Seleccione *" });
                            var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                             select new
                                             {
                                                 IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm"), (Tbl.Bultos==null ? 0 : Tbl.Bultos)),
                                                 Turno = string.Format("{0}", Tbl.Turno)
                                             }).ToList().OrderBy(x => x.Turno);

                            foreach (var Items in LinqQuery)
                            {
                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                            }

                            this.CboTurnos.DataSource = List_Turnos;
                            this.CboTurnos.DataTextField = "Turno";
                            this.CboTurnos.DataValueField = "IdPlan";
                            this.CboTurnos.DataBind();

                        }
                        else
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                            return;

                        }
                    }
                    

                    this.Actualiza_Paneles();

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(TxtFechaHasta_TextChanged), "TxtFechaHasta_TextChanged", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                    return;

                }
            }


        }

        protected void BtnAgregaTruno_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    string Tarjas = string.Empty;
                    int TotalBultos = 0;
                    int SaldoActual = 0;
                    int TotalDetalle = 0;

                    string MRN = string.Empty;
                    string MSN = string.Empty;
                    string HSN = string.Empty;
                    string BODEGA = string.Empty;

                    string IdEmpresa = string.Empty;
                    string DesEmpresa = string.Empty;
                    string IdChofer = string.Empty;
                    string DesChofer = string.Empty;

                    CultureInfo enUS = new CultureInfo("en-US");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    this.Ocultar_Mensaje();

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    objPaseBRBK = Session["PaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder agregar un turno... </b>"));
                        return;
                    }

                    int Capacidad = 0;
                    if (!int.TryParse(this.TxtCantidadRetirar.Text, out Capacidad))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar la cantidad a retirar... </b>"));
                        this.TxtCantidadRetirar.Focus();
                        return;
                    }

                  
                    if (Capacidad==0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar la cantidad a retirar... </b>"));
                        this.TxtCantidadRetirar.Focus();
                        return;
                    }

                    //verificar si es pase sin turno
                    var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, objPaseBRBK.MRN, objPaseBRBK.MSN, objPaseBRBK.HSN);
                    if (PaseSinturno.Exitoso)
                    {
                        if (PaseSinturno.Resultado)
                        {
                            EsPasesinTurno = true;
                        }
                        else
                        {
                            EsPasesinTurno = false;
                        }

                    }
                    else
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseBRBK.CARGA, PaseSinturno.MensajeProblema));
                        return;
                    }

                    //pase con turno
                    if (!EsPasesinTurno)
                    {
                        if (this.CboTurnos.SelectedIndex == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un turno para poder agregar la información </b>"));
                            this.CboTurnos.Focus();
                            return;
                        }
                        else
                        {
                            TurnoSelect = this.CboTurnos.SelectedValue;
                        }
                    }
                    

                    HoraHasta = "00:00";
                    Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase.. Mes/Día/año</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(TurnoSelect))
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un turno para poder generar el pase de puerta..</b>"));
                        this.CboTurnos.Focus();
                        return;
                    }

                    //pase con turno
                    //if (!EsPasesinTurno)
                    //{
                    string FechaIni = TurnoSelect.Split('-').ToList()[2].Trim();

                    
                    if (!DateTime.TryParseExact(FechaIni, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaTurnoInicio))
                    {

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de inicio del turno seleccionado no es valida, Mes/Día/año </b>"));
                        this.CboTurnos.Focus();
                        return;
                    }
                    string FechaFin = TurnoSelect.Split('-').ToList()[3].Trim();
                    if (!DateTime.TryParseExact(FechaFin, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaTurnoFinal))
                    {

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha final del turno seleccionado no es valida, Mes/Día/año </b>"));
                        this.CboTurnos.Focus();
                        return;
                    }

                    //}


                    List<Cls_Bil_Configuraciones> ValidaFinSemana = Cls_Bil_Configuraciones.Get_Validacion("FINSEMANABRBK", out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", cMensajes));
                        return;

                    }

                    bool Valida_FinSemana = false;
                    if (ValidaFinSemana.Count != 0)
                    {
                        Valida_FinSemana = true;
                    }
                    if (Valida_FinSemana)
                    {
                        if (FechaTurnoFinal.DayOfWeek != DayOfWeek.Saturday && FechaTurnoFinal.DayOfWeek != DayOfWeek.Sunday)
                        {
                        }
                        else
                        {
                        
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede generar pase de puerta para los días: sábados y Domingos.</b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }

                    //valida turno seleccionado, tomado o capacidad
                    if (!EsPasesinTurno)
                    {
                        //validar stock
                        Int64 IDX_ROW = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());

                        var Msg = PasePuerta.ValidaCargaBRBK.ValidaTurno(ClsUsuario.ruc, IDX_ROW, objPaseBRBK.MRN, objPaseBRBK.MSN, objPaseBRBK.HSN);
                        if (Msg.Exitoso)
                        {
                            var LinqQuery = (from Tbl in Msg.Resultado.Where(p => !string.IsNullOrEmpty(p.MENSAJE))
                                             select new
                                             {
                                                 MENSAJE = Tbl.MENSAJE

                                             }).FirstOrDefault();

                            if (LinqQuery != null)
                            {
                                if (!string.IsNullOrEmpty(LinqQuery.MENSAJE))
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0}</b>", LinqQuery.MENSAJE));
                                    
                                    //vuelve a cargar turnos
                                    //valida turno
                                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                                    {
                                        HoraHasta = "00:00";
                                        Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                                        if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase Mes/Día/año</b>"));
                                            this.TxtFechaHasta.Focus();
                                        }


                                        //EsPasesinTurno = false;


                                        List_Turnos = new List<Cls_Bil_Turnos>();
                                        List_Turnos.Clear();

                                        var Turnos = PasePuerta.TurnoBRBK.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, objPaseBRBK.CNTR_UBICACION, objPaseBRBK.idProducto.Value);
                                        if (Turnos.Exitoso)
                                        {
                                            //turno por defecto
                                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "", "0"), Turno = "* Seleccione *" });
                                            var LinqQTurnos = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                               select new
                                                               {
                                                                   IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm"), (Tbl.Bultos == null ? 0 : Tbl.Bultos)),
                                                                   Turno = string.Format("{0}", Tbl.Turno)
                                                               }).ToList().OrderBy(x => x.Turno);

                                            foreach (var Items in LinqQTurnos)
                                            {
                                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                            }

                                            this.CboTurnos.DataSource = List_Turnos;
                                            this.CboTurnos.DataTextField = "Turno";
                                            this.CboTurnos.DataValueField = "IdPlan";
                                            this.CboTurnos.DataBind();

                                        }
                                        else
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                                            return;

                                        }

                                        //si es pase sin turno
                                        //if (EsPasesinTurno)
                                        //{
                                        //    this.Pase_Sin_Turno_Default();
                                        //}
                                        //else
                                        //{
                                            
                                        //}

                                    }

                                    this.CboTurnos.Focus();
                                    return;
                                }
                            }
                        }
                        else
                        {

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No puede validar turnos...No existe información de mensajes de la carga...</b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }



                    //validar stock
                    var Saldo = PasePuerta.SaldoCargaBRBK.SaldoPendienteBRBK(ClsUsuario.ruc, objPaseBRBK.CARGA);
                    if (Saldo.Exitoso)
                    {
                        var LinqQuery = (from Tbl in Saldo.Resultado.Where(p => !string.IsNullOrEmpty(p.NUMERO_CARGA))
                                         select new
                                         {
                                             SALDO_FINAL = Tbl.SALDO_FINAL

                                         }).FirstOrDefault();

                        if (LinqQuery != null)
                        {
                            if (Capacidad > LinqQuery.SALDO_FINAL)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede retirar la cantidad de: {0}, el saldo pendiente es de: {1}, se excede el stock</b>", Capacidad, LinqQuery.SALDO_FINAL));
                                this.TxtCantidadRetirar.Text = LinqQuery.SALDO_FINAL.ToString();
                                this.TxtCantidadRetirar.Focus();
                                return;
                            }

                            TotalBultos = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0).Sum(x => x.CANTIDAD.Value) + Capacidad;

                            TotalDetalle = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0).Sum(x => x.CANTIDAD.Value);

                            this.TxtActual.Text = TotalDetalle.ToString();

                            if (TotalBultos > LinqQuery.SALDO_FINAL)
                            {
                                int nSbubtotal = (LinqQuery.SALDO_FINAL - TotalDetalle);
                                int nSaldoFin = (nSbubtotal < 0 ? 0 : nSbubtotal);

                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede retirar la cantidad de: {0}, el saldo pendiente es de: {1}, se excede el stock</b>", Capacidad, nSaldoFin));
                                this.TxtCantidadRetirar.Text = nSaldoFin.ToString();
                                this.TxtCantidadRetirar.Focus();
                                return;
                            }

                            
                            if (!int.TryParse(this.TxtSaldo.Text, out SaldoActual))
                            {
                               
                            }

                            this.TxtSaldoFinal.Text = (SaldoActual - TotalDetalle).ToString();

                            int TotalPases = objPaseBRBK.Detalle.Count() + 1;

                            this.text_detalle.InnerHtml = "DETALLE DE PASES A EMITIR";
                            //this.text_detalle.InnerHtml = string.Format("DETALLE DE PASES A EMITIR:  &nbsp;&nbsp;&nbsp;&nbsp;T/PASES: {0}  &nbsp;&nbsp;&nbsp;&nbsp;T/BULTOS: {1} &nbsp;&nbsp;&nbsp;&nbsp;S/PENDIENTE: {2}", TotalPases, TotalBultos, this.TxtSaldoFinal.Text);

                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No puede validar stock...No existe información del saldo de la carga...</b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }
                    else
                    {

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No puede validar stock...No existe información del saldo de la carga...</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                   

                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar la Compañía de Transporte para poder agregar la información </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }

                    //valida que exista Empresa Transporte 
                    if (!string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        EmpresaSelect = this.Txtempresa.Text.Trim();
                        if (EmpresaSelect.Split('-').ToList().Count > 1)
                        {
                          
                            IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();

                            DesEmpresa = (EmpresaSelect.Split('-').ToList().Count >= 3 ? string.Format("{0} - {1}", EmpresaSelect.Split('-').ToList()[1].Trim(), EmpresaSelect.Split('-').ToList()[2].Trim()) : EmpresaSelect.Split('-').ToList()[1].Trim());

                            var EmpresaTransporte = N4.Entidades.CompaniaTransporte.ObtenerCompania(ClsUsuario.loginname, IdEmpresa);
                            if (!EmpresaTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información </b>"));
                                this.Txtempresa.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }

                    //valida que exista un chofer
                    if (!string.IsNullOrEmpty(TxtChofer.Text))
                    {
                        ChoferSelect = this.TxtChofer.Text.Trim();
                        if (ChoferSelect.Split('-').ToList().Count > 1)
                        {
                         
                            IdChofer = ChoferSelect.Split('-').ToList()[0].Trim();
                            DesChofer = ChoferSelect.Split('-').ToList()[1].Trim();
                            var ChoferTransporte = N4.Entidades.Chofer.ObtenerChofer(ClsUsuario.loginname, IdChofer);
                            if (!ChoferTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                                this.TxtChofer.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                            this.TxtChofer.Focus();
                            return;
                        }
                    }

                    //valida que exista una placa
                    if (!string.IsNullOrEmpty(TxtPlaca.Text))
                    {
                        PlacaSelect = this.TxtPlaca.Text.Trim();
                        if (PlacaSelect.Split('-').ToList().Count > 1)
                        {
                            string IdPlaca = PlacaSelect.Split('-').ToList()[0].Trim();
                            var PlacaTransporte = N4.Entidades.Camion.ObtenerCamion(ClsUsuario.loginname, IdPlaca);
                            if (!PlacaTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                                this.TxtPlaca.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                            this.TxtPlaca.Focus();
                            return;
                        }
                    }

                    string CIATRANS = string.Empty;
                    string ID_CIATRANS = string.Empty;
                    string TRANSPORTISTA_DESC = string.Empty;

                    //recorre para actualizar datos del transportista
                    Tarjas = (objPaseBRBK.Detalle.Count()+1).ToString();

                    objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();
                    objDetallePaseBRBK.FECHA = objPaseBRBK.FECHA;
                    objDetallePaseBRBK.MRN = objPaseBRBK.MRN;
                    objDetallePaseBRBK.MSN = objPaseBRBK.MSN;
                    objDetallePaseBRBK.HSN = objPaseBRBK.HSN;
                    objDetallePaseBRBK.IV_USUARIO_CREA = objPaseBRBK.IV_USUARIO_CREA;
                    objDetallePaseBRBK.SESION = objPaseBRBK.SESION;

                    objDetallePaseBRBK.FACTURA = objPaseBRBK.FACTURA;
                    objDetallePaseBRBK.CARGA = objPaseBRBK.CARGA;
                    objDetallePaseBRBK.AGENTE = objPaseBRBK.AGENTE;
                    objDetallePaseBRBK.FACTURADO = objPaseBRBK.FACTURADO;
                    objDetallePaseBRBK.PAGADO = objPaseBRBK.PAGADO;
                    objDetallePaseBRBK.GKEY = objPaseBRBK.GKEY;
                    objDetallePaseBRBK.REFERENCIA = objPaseBRBK.REFERENCIA;
                    objDetallePaseBRBK.CONTENEDOR = objPaseBRBK.CONTENEDOR;
                    objDetallePaseBRBK.DOCUMENTO = objPaseBRBK.DOCUMENTO;
                    objDetallePaseBRBK.PRIMERA = objPaseBRBK.PRIMERA;
                    objDetallePaseBRBK.MARCA = objPaseBRBK.MARCA;
                    objDetallePaseBRBK.CANTIDAD = Capacidad;
                    objDetallePaseBRBK.CIATRANS = objPaseBRBK.CIATRANS;
                    objDetallePaseBRBK.CHOFER = objPaseBRBK.CHOFER;
                    objDetallePaseBRBK.PLACA = objPaseBRBK.PLACA;
                    objDetallePaseBRBK.FECHA_SALIDA = objPaseBRBK.FECHA_SALIDA;
                    objDetallePaseBRBK.CNTR_DD = objPaseBRBK.CNTR_DD;
                    objDetallePaseBRBK.AGENTE_DESC = objPaseBRBK.AGENTE_DESC;
                    objDetallePaseBRBK.FACTURADO_DESC = objPaseBRBK.FACTURADO_DESC;
                    objDetallePaseBRBK.IMPORTADOR = objPaseBRBK.IMPORTADOR;
                    objDetallePaseBRBK.IMPORTADOR_DESC = objPaseBRBK.IMPORTADOR_DESC;
                    objDetallePaseBRBK.FECHA_SALIDA_PASE = objPaseBRBK.FECHA_SALIDA_PASE;
                    objDetallePaseBRBK.FECHA_AUT_PPWEB = objPaseBRBK.FECHA_AUT_PPWEB;
                    objDetallePaseBRBK.HORA_AUT_PPWEB = objPaseBRBK.HORA_AUT_PPWEB;
                    objDetallePaseBRBK.TIPO_CNTR = objPaseBRBK.TIPO_CNTR;

                    //actualizar datos pase sin turno
                    if (EsPasesinTurno)
                    {
                        objDetallePaseBRBK.D_TURNO = string.Empty;
                        objDetallePaseBRBK.TURNO = 0;
                        objDetallePaseBRBK.ID_TURNO = null;
                        objDetallePaseBRBK.TURNO_DESDE = null;
                        objDetallePaseBRBK.TURNO_HASTA = null;
                        objDetallePaseBRBK.FECHA_SALIDA_PASE = FechaActualSalida;
                        objDetallePaseBRBK.BULTOS_HORARIOS = Capacidad;

                    }
                    else
                    {
                        objDetallePaseBRBK.D_TURNO = this.CboTurnos.SelectedItem.ToString().Substring(0, 5);
                        objDetallePaseBRBK.TURNO = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());//ID TURNO
                        objDetallePaseBRBK.ID_TURNO = Convert.ToInt16(TurnoSelect.Split('-').ToList()[1].Trim());//secuencia turno
                        objDetallePaseBRBK.TURNO_DESDE = FechaTurnoInicio;
                        objDetallePaseBRBK.TURNO_HASTA = FechaTurnoFinal;
                        objDetallePaseBRBK.BULTOS_HORARIOS = Capacidad;

                        HoraHasta = objDetallePaseBRBK.D_TURNO;
                        Fecha = string.Format("{0} {1}", FechaActualSalida.Date.ToString("MM/dd/yyyy"), HoraHasta);
                        if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para poder agregar la información del turno, Mes/Día/año </b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        objDetallePaseBRBK.FECHA_SALIDA_PASE = FechaActualSalida;
                    }


                    
                    objDetallePaseBRBK.ID_PASE = objPaseBRBK.ID_PASE;
                    objDetallePaseBRBK.ESTADO = objPaseBRBK.ESTADO;
                    objDetallePaseBRBK.ENVIADO = objPaseBRBK.ENVIADO;
                    objDetallePaseBRBK.AUTORIZADO = objPaseBRBK.AUTORIZADO;
                    objDetallePaseBRBK.VENTANILLA = objPaseBRBK.VENTANILLA;
                    objDetallePaseBRBK.USUARIO_ING = objPaseBRBK.USUARIO_ING;
                    objDetallePaseBRBK.FECHA_ING = objPaseBRBK.FECHA_ING;
                    objDetallePaseBRBK.USUARIO_MOD = objPaseBRBK.USUARIO_MOD;
                    objDetallePaseBRBK.ESTADO_PAGO = objPaseBRBK.ESTADO_PAGO;
                    objDetallePaseBRBK.ID_PPWEB = objPaseBRBK.ID_PPWEB;

                    objDetallePaseBRBK.CANTIDAD = Capacidad;


                    objDetallePaseBRBK.ID_CIATRANS = IdEmpresa;
                    objDetallePaseBRBK.ID_CHOFER = IdChofer;
                    objDetallePaseBRBK.CIATRANS = EmpresaSelect;
                    objDetallePaseBRBK.CHOFER = ChoferSelect;
                    objDetallePaseBRBK.PLACA = PlacaSelect;
                    objDetallePaseBRBK.TRANSPORTISTA_DESC = DesEmpresa;
                    objDetallePaseBRBK.CHOFER_DESC = DesChofer;


                    objDetallePaseBRBK.SUB_SECUENCIA = Tarjas;
                    objDetallePaseBRBK.LLAVE = Tarjas;
                    objDetallePaseBRBK.ID_UNIDAD = objPaseBRBK.ID_UNIDAD;
                    objPaseBRBK.Detalle.Add(objDetallePaseBRBK);

                   

                    tablePagination.DataSource = objPaseBRBK.Detalle;
                    tablePagination.DataBind();

                    Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;


                    TotalDetalle = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0).Sum(x => x.CANTIDAD.Value);

                    this.TxtActual.Text = TotalDetalle.ToString();

                    if (!int.TryParse(this.TxtSaldo.Text, out SaldoActual))
                    {

                    }

                    this.TxtSaldoFinal.Text = (SaldoActual - TotalDetalle).ToString();

                    this.Actualiza_Paneles();

                    //grabar turno en tablas temporales
                    //inserta en tabla temporal
                    //if (!EsPasesinTurno)
                    //{
                        objReserva = new Cls_Bil_PasePuertaBRBK_Temporal();
                        objReserva.IDX_ROW = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());
                        objReserva.MRN = objPaseBRBK.MRN;
                        objReserva.MSN = objPaseBRBK.MSN;
                        objReserva.HSN = objPaseBRBK.HSN;
                        objReserva.USUARIO_CREA = ClsUsuario.loginname;

                        var nProceso = objReserva.SaveTransaction(out cMensajes);
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {

                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo grabar datos del turno.{0}</b>", cMensajes));
                            return;
                        }
                       
                    //}


                    //valida turno
                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        HoraHasta = "00:00";
                        Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                        if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase Mes/Día/año</b>"));
                            this.TxtFechaHasta.Focus();
                        }


                        //verificar si es pase sin turno
                        var PaseSinturnoNew = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, objPaseBRBK.MRN, objPaseBRBK.MSN, objPaseBRBK.HSN);
                        if (PaseSinturnoNew.Exitoso)
                        {
                            if (PaseSinturnoNew.Resultado)
                            {
                                EsPasesinTurno = true;
                            }
                            else
                            {
                                EsPasesinTurno = false;
                            }

                        }
                        else
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseBRBK.CARGA, PaseSinturnoNew.MensajeProblema));
                        }


                        //si es pase sin turno
                        if (EsPasesinTurno)
                        {
                            this.Pase_Sin_Turno_Default();
                        }
                        else
                        {
                            List_Turnos = new List<Cls_Bil_Turnos>();
                            List_Turnos.Clear();

                            var Turnos = PasePuerta.TurnoBRBK.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, objPaseBRBK.CNTR_UBICACION, objPaseBRBK.idProducto.Value);
                            if (Turnos.Exitoso)
                            {
                                //turno por defecto
                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "", "0"), Turno = "* Seleccione *" });
                                var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                 select new
                                                 {
                                                     IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm"), (Tbl.Bultos == null ? 0 : Tbl.Bultos)),
                                                     Turno = string.Format("{0}", Tbl.Turno)
                                                 }).ToList().OrderBy(x => x.Turno);

                                foreach (var Items in LinqQuery)
                                {
                                    List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                }

                                this.CboTurnos.DataSource = List_Turnos;
                                this.CboTurnos.DataTextField = "Turno";
                                this.CboTurnos.DataValueField = "IdPlan";
                                this.CboTurnos.DataBind();

                            }
                            else
                            {
                                this.Turno_Default();
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                                return;

                            }
                        }

                    }

                    


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAgregaTruno_Click), "BtnAgregaTruno_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar turno valido.. {0} </b>", OError));
                }
               
            }
        }
        #endregion



        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");

                    int SALDO_FINAL = 0;


                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de la carga MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de la carga MSN"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<b><b>Informativo! </b>Por favor ingresar el número de la carga HSN"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                   

                   

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //valida información de la carga en N4
                    var Contenedor = new N4.Importacion.container_brbk();
                    var ListaContenedores = Contenedor.CargaPorBL(ClsUsuario.loginname, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());//resultado de entidad contenedor y brbk
                    if (ListaContenedores.Exitoso)
                    {

                        //contenedores con carga cfs
                        var LinqPartidadN4 = (from Tbl in ListaContenedores.Resultado.Where(Tbl => Tbl.CNTR_CANTIDAD != 0)
                                              select new
                                              {
                                                  CNTR_CONTAINER = (Tbl.CNTR_CONTAINER == null ? string.Empty : Tbl.CNTR_CONTAINER),
                                                  CNTR_VEPR_REFERENCE = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                  CNTR_TYPE = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                  CNTR_TYSZ_SIZE = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                  CNTR_CATY_CARGO_TYPE = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                                  FECHA_CAS = (DateTime?)(Tbl.FECHA_CAS.HasValue ? Tbl.FECHA_CAS : null),
                                                  BLOQUEOS = Tbl.CNTR_HOLD,
                                                  CNTR_YARD_STATUS = (Tbl.CNTR_YARD_STATUS == null) ? string.Empty : Tbl.CNTR_YARD_STATUS,
                                                  CNTR_TYSZ_TYPE = (Tbl.CNTR_TYSZ_TYPE == null) ? string.Empty : Tbl.CNTR_TYSZ_TYPE,
                                                  CNTR_CLNT_CUSTOMER_LINE = (Tbl.CNTR_CLNT_CUSTOMER_LINE == null) ? string.Empty : Tbl.CNTR_CLNT_CUSTOMER_LINE,
                                                  CNTR_DOCUMENT = (Tbl.CNTR_DOCUMENT == null ? string.Empty : Tbl.CNTR_DOCUMENT),
                                                  CNTR_FULL_EMPTY_CODE = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                                  CNTR_CONSECUTIVO = Tbl.CNTR_CONSECUTIVO,
                                                  CNTR_AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                                  CNTR_HOLD = (Tbl.CNTR_HOLD == 0) ? false : true,
                                                  CNTR_VEPR_VOYAGE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                  CNTR_VEPR_VSSL_NAME = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                  CNTR_VEPR_ACTUAL_ARRIVAL = (Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.HasValue ? Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.Value.ToString("dd/MM/yyyy") : ""),
                                                  CNTR_DD = (Tbl.CNTR_DD == null) ? false : Tbl.CNTR_DD.Value,
                                                  CNTR_DESCARGA = (Tbl.CNTR_DESCARGA == null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                  CNTR_VEPR_ACTUAL_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                                  CNTR_CANTIDAD_SALDO = (Tbl.CNTR_CANTIDAD == null ? 0 : Tbl.CNTR_CANTIDAD),
                                                  CNTR_PESO = (Tbl.CNTR_PESO == null ? 0 : Tbl.CNTR_PESO),
                                                  CNTR_OPERACION = (Tbl.CNTR_OPERACION == null ? string.Empty : Tbl.CNTR_OPERACION),
                                                  CNTR_DESCRIPCION = (Tbl.CNTR_DESCRIPCION == null ? string.Empty : Tbl.CNTR_DESCRIPCION),
                                                  CNTR_EXPORTADOR = (Tbl.CNTR_EXPORTADOR == null ? string.Empty : Tbl.CNTR_EXPORTADOR),
                                                  CNTR_AGENCIA = (Tbl.CNTR_AGENCIA == null ? string.Empty : Tbl.CNTR_AGENCIA),
                                                  CARGA = string.Format("{0}-{1}-{2}", Tbl.CNTR_MRN, Tbl.CNTR_MSN, Tbl.CNTR_HSN),
                                                  CNTR_REEFER_CONT = (Tbl.CNTR_REEFER_CONT == null ? string.Empty : Tbl.CNTR_REEFER_CONT),
                                                  ID_UNIDAD = (Tbl.ID_UNIDAD == null ? 0 : Tbl.ID_UNIDAD),
                                                  CNTR_CANTIDAD_TOTAL = (Tbl.CNTR_TEMPERATURE == null ? 0 : Tbl.CNTR_TEMPERATURE),
                                                  CNTR_UBICACION = string.IsNullOrEmpty(Tbl.CNTR_POSITION) ? "" : Tbl.CNTR_POSITION,
                                                  UNIDAD_GKEY = (Tbl.UNIDAD_GKEY == null ? 0 : Tbl.UNIDAD_GKEY),
                                                  CNTR_BODEGA = string.IsNullOrEmpty(Tbl.CNTR_BODEGA) ? "" : Tbl.CNTR_BODEGA,
                                              });

                       
                        //informacion de N4Middleware
                        var Carga = PasePuerta.PaseWebBRBK.ObtenerCargaPaseBRBK(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc);
                        if (Carga.Exitoso)
                        {
                            /*query contenedores*/
                            var LinqQuery = (from Tbl in Carga.Resultado.Where(Tbl => Tbl.ID_PPWEB != 0)
                                             select new
                                             {
                                                 ID_PPWEB = Tbl.ID_PPWEB,
                                                 CARGA = string.Format("{0}-{1}-{2}", Tbl.MRN, Tbl.MSN, Tbl.HSN),
                                                 MRN = Tbl.MRN,
                                                 MSN = Tbl.MSN,
                                                 HSN = Tbl.HSN,
                                                 FACTURA = (Tbl.FACTURA == null) ? string.Empty : Tbl.FACTURA,
                                                 AGENTE = (Tbl.AGENTE == null) ? string.Empty : Tbl.AGENTE,
                                                 FACTURADO = (Tbl.FACTURADO == null) ? string.Empty : Tbl.FACTURADO,
                                                 PAGADO = Tbl.PAGADO,
                                                 GKEY = (Tbl.GKEY == null) ? 0 : Tbl.GKEY,
                                                 REFERENCIA = (Tbl.REFERENCIA == null) ? string.Empty : Tbl.REFERENCIA,
                                                 CONTENEDOR = (Tbl.CONTENEDOR == null) ? string.Empty : Tbl.CONTENEDOR,
                                                 DOCUMENTO = (Tbl.DOCUMENTO == null) ? string.Empty : Tbl.DOCUMENTO,
                                                 PRIMERA = (Tbl.PRIMERA == null) ? string.Empty : Tbl.PRIMERA,
                                                 MARCA = (Tbl.MARCA == null) ? string.Empty : Tbl.MARCA,
                                                 CANTIDAD = (Tbl.CANTIDAD == null) ? 0 : Tbl.CANTIDAD,
                                                 CIATRANS = (Tbl.CIATRANS == null) ? string.Empty : Tbl.CIATRANS,
                                                 CHOFER = (Tbl.CHOFER == null) ? string.Empty : Tbl.CHOFER,
                                                 PLACA = (Tbl.PLACA == null) ? string.Empty : Tbl.PLACA,
                                                 FECHA_SALIDA = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                                 FECHA_SALIDA_PASE = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                                 FECHA_AUT_PPWEB = (Tbl.FECHA_AUT_PPWEB.HasValue ? Tbl.FECHA_AUT_PPWEB : null),
                                                 HORA_AUT_PPWEB = (Tbl.HORA_AUT_PPWEB == null) ? string.Empty : Tbl.HORA_AUT_PPWEB,
                                                 CNTR_DD = (Tbl.CNTR_DD == null) ? false : Tbl.CNTR_DD,
                                                 AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? string.Empty : Tbl.AGENTE_DESC,
                                                 FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                                 IMPORTADOR = (Tbl.IMPORTADOR == null) ? string.Empty : Tbl.IMPORTADOR,
                                                 IMPORTADOR_DESC = (Tbl.IMPORTADOR_DESC == null) ? string.Empty : Tbl.IMPORTADOR_DESC,
                                                 TIPO_CNTR = (Tbl.TIPO_CNTR == null) ? string.Empty : Tbl.TIPO_CNTR,
                                                 ID_TURNO = (Tbl.ID_TURNO == null) ? 0 : Tbl.ID_TURNO,
                                                 TURNO = (Tbl.TURNO == null) ? 0 : Tbl.TURNO,
                                                 D_TURNO = (Tbl.D_TURNO == null) ? string.Empty : Tbl.D_TURNO,
                                                 ID_PASE = (Tbl.ID_PASE == null) ? 0 : Tbl.ID_PASE,
                                                 ESTADO = Tbl.ESTADO,
                                                 ENVIADO = Tbl.ENVIADO,
                                                 AUTORIZADO = Tbl.AUTORIZADO,
                                                 VENTANILLA = Tbl.VENTANILLA,
                                                 USUARIO_ING = ClsUsuario.loginname,
                                                 USUARIO_MOD = ClsUsuario.loginname,
                                                 ESTADO_PAGO = (Tbl.PAGADO == true ? "SI" : "NO"),
                                                 ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD,
                                                 idProducto = (Tbl.idProducto == null) ? 0 : Tbl.idProducto,
                                                 idItem = (Tbl.idItem == null) ? 0 : Tbl.idItem,
                                                 Tipo_Producto = (Tbl.Tipo_Producto == null) ? string.Empty : Tbl.Tipo_Producto,
                                             });

                            if (LinqQuery != null)
                            {

                                /*left join de contenedores*/
                                var LinqFinal = (from Tbl in LinqQuery
                                                 join Tbl2 in LinqPartidadN4 on Tbl.GKEY equals Tbl2.CNTR_CONSECUTIVO //into TmpFinal
                                                 //from Final in TmpFinal.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     Tbl.ID_PPWEB,
                                                     Tbl.CARGA,
                                                     Tbl.MRN,
                                                     Tbl.MSN,
                                                     Tbl.HSN,
                                                     Tbl.FACTURA,
                                                     Tbl.AGENTE ,
                                                     Tbl.FACTURADO,
                                                     Tbl.PAGADO ,
                                                     Tbl.GKEY ,
                                                     Tbl.REFERENCIA,
                                                     Tbl.CONTENEDOR ,
                                                     Tbl.DOCUMENTO ,
                                                     Tbl.PRIMERA ,
                                                     Tbl.MARCA ,
                                                     Tbl.CANTIDAD ,
                                                     Tbl.CIATRANS ,
                                                     Tbl.CHOFER ,
                                                     Tbl.PLACA ,
                                                     Tbl.FECHA_SALIDA,
                                                     Tbl.FECHA_SALIDA_PASE,
                                                     Tbl.FECHA_AUT_PPWEB ,
                                                     Tbl.HORA_AUT_PPWEB,
                                                     Tbl.CNTR_DD ,
                                                     Tbl.AGENTE_DESC,
                                                     Tbl.FACTURADO_DESC ,
                                                     Tbl.IMPORTADOR ,
                                                     Tbl.IMPORTADOR_DESC ,
                                                     Tbl.TIPO_CNTR ,
                                                     Tbl.ID_TURNO ,
                                                     Tbl.TURNO ,
                                                     Tbl.D_TURNO,
                                                     Tbl.ID_PASE ,
                                                     Tbl.ESTADO ,
                                                     Tbl.ENVIADO ,
                                                     Tbl.AUTORIZADO ,
                                                     Tbl.VENTANILLA ,
                                                     Tbl.USUARIO_ING ,
                                                     Tbl.USUARIO_MOD ,
                                                     Tbl.ESTADO_PAGO ,
                                                     Tbl.ID_UNIDAD ,
                                                     CNTR_CANTIDAD_SALDO = (Tbl2.CNTR_CANTIDAD_SALDO == null ? 0 : Tbl2.CNTR_CANTIDAD_SALDO),
                                                     CNTR_PESO = (Tbl2.CNTR_PESO == null ? 0 : Tbl2.CNTR_PESO),
                                                     CNTR_CANTIDAD_TOTAL = (Tbl2.CNTR_CANTIDAD_TOTAL == null ? 0 : Tbl2.CNTR_CANTIDAD_TOTAL),
                                                     CNTR_UBICACION = string.IsNullOrEmpty(Tbl2.CNTR_UBICACION)  ? "" : Tbl2.CNTR_UBICACION,
                                                     UNIDAD_GKEY = (Tbl2.UNIDAD_GKEY == null ? 0 : Tbl2.UNIDAD_GKEY),
                                                     CANTIDAD_EMITIDOS = ((Tbl2.CNTR_CANTIDAD_TOTAL == null ? 0 : Tbl2.CNTR_CANTIDAD_TOTAL) - (Tbl2.CNTR_CANTIDAD_SALDO == null ? 0 : Tbl2.CNTR_CANTIDAD_SALDO)),
                                                     CNTR_BODEGA = string.IsNullOrEmpty(Tbl2.CNTR_BODEGA) ? "" : Tbl2.CNTR_BODEGA,
                                                     Tbl.idProducto,
                                                     Tbl.idItem ,
                                                     Tbl.Tipo_Producto ,
                                                 }).Where(p => p.CNTR_CANTIDAD_SALDO.Value > 0).FirstOrDefault();

                                if (LinqFinal != null)
                                {

                                    //valido saldo restante + lo reservado
                                    //validar stock
                                    string _BL = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());

                                    var Saldo = PasePuerta.SaldoCargaBRBK.SaldoPendienteBRBK(ClsUsuario.ruc, _BL);
                                    if (Saldo.Exitoso)
                                    {
                                        var LinqSaldo = (from Tbl in Saldo.Resultado.Where(p => !string.IsNullOrEmpty(p.NUMERO_CARGA))
                                                         select new
                                                         {
                                                             SALDO_FINAL = Tbl.SALDO_FINAL

                                                         }).FirstOrDefault();

                                        if (LinqSaldo != null)
                                        {
                                            SALDO_FINAL = LinqSaldo.SALDO_FINAL;

                                            if (LinqSaldo.SALDO_FINAL == 0)
                                            {
                                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe saldo de la carga: {0} pendiente para emitir pase de puerta..Saldo actual: {1}</b>", _BL, LinqSaldo.SALDO_FINAL));
                                                return;
                                            }

                                        }
                                        else
                                        {
                                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! al validar saldo de la carga...No existe información del saldo de la carga...</b>"));
                                            this.TxtFechaHasta.Focus();
                                            return;
                                        }
                                    }
                                    else
                                    {

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! al validar saldo de la carga...No existe información del saldo de la carga...</b>"));
                                        return;
                                    }


                                    //valida el tipo de carga, para que emita pases en base a solicitudes
                                    //valido ubicacion
                                    var LinqTipo = !LinqFinal.idProducto.HasValue ? 0 : LinqFinal.idProducto.Value;

                                    objValidaSolicitud = new brbk_valida_solicitud();
                                    objValidaSolicitud.MRN = this.TXTMRN.Text.Trim();
                                    objValidaSolicitud.MSN = this.TXTMSN.Text.Trim();
                                    objValidaSolicitud.HSN = this.TXTHSN.Text.Trim();
                                    objValidaSolicitud.RUC = ClsUsuario.ruc;

                                    //objValidaSolicitud.IDPRODUCTO = LinqTipo;

                                    if (objValidaSolicitud.Valida_Tipo_Carga(out OError))
                                    {

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", objValidaSolicitud.MENSAJE));
                                        return;
                                    }


                                    //valida si tiene servicios pendientes de facturar, para que emita la factura
                                    string Rubro = string.Empty;
                                    Configuraciones.ModuloAccesorios Cfgs = new Configuraciones.ModuloAccesorios("N4_SERVICE");

                                    Cfgs.ConfiguracionBase = "DATACON";
                                    string pv = string.Empty;
                                    if (!Cfgs.Inicializar(out pv))
                                    {
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener configuraciones BREAK BULK....{0}</b>", pv));
                                        return;
                                    }

                                    var pRubro = Cfgs.ObtenerConfiguracion("PASE_VENCIDO_BRBK");

                                    Rubro = pRubro == null ? string.Empty : pRubro.valor;

                                    XDocument XMLRubros = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                               new XElement("CONTENEDORES", 
                                                                           new XElement("CONTENEDOR",
                                                                    new XAttribute("gkey", LinqFinal.ID_UNIDAD == 0 ? "0" : LinqFinal.ID_UNIDAD.ToString()),
                                                                    new XAttribute("Servicio", Rubro)
                                                                    )));

                                    var Msg = PasePuerta.ValidaFacturaBRBK.ValidaRubrosPendientes(ClsUsuario.ruc, XMLRubros.ToString());
                                    if (Msg.Exitoso)
                                    {
                                        var LinqResp = (from Tbl in Msg.Resultado
                                                         select new
                                                         {
                                                             VALOR = Tbl.resultado

                                                         }).FirstOrDefault();

                                        if (LinqResp != null)
                                        {
                                            if (LinqResp.VALOR != 0)
                                            {
                                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede generar los pase de puerta, debido a que tiene rubros pendientes de facturar.. Debe emitir las respectivas facturas.</b>"));

                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No puede validar rubros pendientes de facturar...No existe información de mensajes de la carga...</b>")); 
                                        return;
                                    }

                                   



                                    //fin de validacion


                                    //agrego todos los contenedores a la clase cabecera
                                    objPaseBRBK = Session["PaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                                    objPaseBRBK.FECHA = DateTime.Now;
                                    objPaseBRBK.MRN = LinqFinal.MRN;
                                    objPaseBRBK.MSN = LinqFinal.MSN;
                                    objPaseBRBK.HSN = LinqFinal.HSN;
                                    objPaseBRBK.IV_USUARIO_CREA = ClsUsuario.loginname;
                                    objPaseBRBK.SESION = this.hf_BrowserWindowName.Value;

                                    objPaseBRBK.FACTURA = LinqFinal.FACTURA;
                                    objPaseBRBK.CARGA = LinqFinal.CARGA;
                                    objPaseBRBK.AGENTE = LinqFinal.AGENTE;
                                    objPaseBRBK.FACTURADO = LinqFinal.FACTURADO;
                                    objPaseBRBK.PAGADO = LinqFinal.PAGADO;
                                    
                                    objPaseBRBK.GKEY = LinqFinal.GKEY;
                                    objPaseBRBK.REFERENCIA = LinqFinal.REFERENCIA;
                                    objPaseBRBK.CONTENEDOR = LinqFinal.CONTENEDOR;
                                    objPaseBRBK.DOCUMENTO = LinqFinal.DOCUMENTO;
                                    objPaseBRBK.PRIMERA = LinqFinal.PRIMERA;
                                    objPaseBRBK.MARCA = LinqFinal.MARCA;
                                    objPaseBRBK.CANTIDAD = int.Parse(LinqFinal.CNTR_CANTIDAD_TOTAL.Value.ToString());
                                    objPaseBRBK.CIATRANS = string.Empty;
                                    objPaseBRBK.CHOFER = string.Empty;
                                    objPaseBRBK.PLACA = string.Empty;
                                    objPaseBRBK.FECHA_SALIDA = LinqFinal.FECHA_SALIDA;
                                    objPaseBRBK.CNTR_DD = LinqFinal.CNTR_DD.Value;
                                    objPaseBRBK.AGENTE_DESC = LinqFinal.AGENTE_DESC;
                                    objPaseBRBK.FACTURADO_DESC = LinqFinal.FACTURADO_DESC;
                                    objPaseBRBK.IMPORTADOR = LinqFinal.IMPORTADOR;
                                    objPaseBRBK.IMPORTADOR_DESC = LinqFinal.IMPORTADOR_DESC;

                                    //objPaseBRBK.CANTIDAD = LinqFinal.CANTIDAD;
                                    objPaseBRBK.CNTR_CANTIDAD_SALDO = LinqFinal.CNTR_CANTIDAD_SALDO.Value;
                                    objPaseBRBK.CANTIDAD_EMITIDOS = LinqFinal.CANTIDAD_EMITIDOS.Value;
                                    objPaseBRBK.CNTR_UBICACION = LinqFinal.CNTR_BODEGA;

                                    objPaseBRBK.FECHA_SALIDA_PASE = LinqFinal.FECHA_SALIDA_PASE;

                                    objPaseBRBK.idProducto = LinqFinal.idProducto;
                                    objPaseBRBK.idItem = LinqFinal.idItem;
                                    objPaseBRBK.Tipo_Producto = LinqFinal.Tipo_Producto;

                                    this.TxtFechaHasta.Text = objPaseBRBK.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                    this.TxtFechaCas.Text = objPaseBRBK.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                    

                                    this.TxtContenedorSeleccionado.Text = objPaseBRBK.CANTIDAD.ToString();
                                    this.TxtSaldo.Text = objPaseBRBK.CNTR_CANTIDAD_SALDO.ToString();
                                    this.TxtRetirados.Text = objPaseBRBK.CANTIDAD_EMITIDOS.ToString();
                                    this.TxtBodega.Text = objPaseBRBK.CNTR_UBICACION;
                                    this.TxtTipoProducto.Text = objPaseBRBK.Tipo_Producto;
                                    this.TxtCantidadRetirar.Text = SALDO_FINAL.ToString();
                                    this.TxtSaldo.Text = SALDO_FINAL.ToString();

                                    if (LinqFinal.PAGADO == false)
                                    {
                                        this.TxtPagado.Text = "NO";
                                        this.BtnGrabar.Attributes.Add("disabled", "disabled");
                                        this.BtnAgregaTruno.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        this.BtnGrabar.Attributes.Remove("disabled");
                                        this.BtnAgregaTruno.Attributes.Remove("disabled");
                                        this.TxtPagado.Text = "SI";
                                    }


                                    objPaseBRBK.FECHA_AUT_PPWEB = LinqFinal.FECHA_AUT_PPWEB;
                                    objPaseBRBK.HORA_AUT_PPWEB = LinqFinal.HORA_AUT_PPWEB;

                                    objPaseBRBK.TIPO_CNTR = LinqFinal.TIPO_CNTR;
                                    objPaseBRBK.ID_TURNO = LinqFinal.ID_TURNO;
                                    objPaseBRBK.TURNO = LinqFinal.TURNO;
                                    objPaseBRBK.D_TURNO = LinqFinal.D_TURNO;
                                    objPaseBRBK.ID_PASE = double.Parse(LinqFinal.ID_PASE.Value.ToString());
                                    objPaseBRBK.ESTADO = LinqFinal.ESTADO;
                                    objPaseBRBK.ENVIADO = LinqFinal.ENVIADO;
                                    objPaseBRBK.AUTORIZADO = LinqFinal.AUTORIZADO;
                                    objPaseBRBK.VENTANILLA = LinqFinal.VENTANILLA;
                                    objPaseBRBK.USUARIO_ING = LinqFinal.USUARIO_ING;
                                    objPaseBRBK.FECHA_ING = System.DateTime.Now.Date;
                                    objPaseBRBK.USUARIO_MOD = LinqFinal.USUARIO_MOD;
                                    objPaseBRBK.ESTADO_PAGO = LinqFinal.ESTADO_PAGO;

                                    objPaseBRBK.ID_PPWEB = LinqFinal.ID_PPWEB;
                                    objPaseBRBK.ID_UNIDAD = LinqFinal.ID_UNIDAD;

                                    objPaseBRBK.TIPO_CNTR = LinqFinal.TIPO_CNTR;

                                    //if (LinqFinal.CNTR_DD.Value)
                                    //{
                                    //    objPaseBRBK.TIPO_CNTR = string.Format("{0} - {1}", LinqQuery.TIPO_CNTR, "Desaduanamiento Directo");
                                    //}

                                    //detalle de pases
                                    objPaseBRBK.Detalle.Clear();


                                    //valido ubicacion
                                    var LinqExiste = (from Tbl in ListaContenedores.Resultado.Where(Tbl => Tbl.CNTR_CANTIDAD != 0)
                                                      select new
                                                      {
                                                          CNTR_UBICACION = string.IsNullOrEmpty(Tbl.CNTR_BODEGA) ? "" : Tbl.CNTR_BODEGA
                                                      }).FirstOrDefault();

                                    if (LinqExiste != null)
                                    {
                                        if (string.IsNullOrEmpty(LinqExiste.CNTR_UBICACION))
                                        {
                                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: La carga no presenta ubicación..{0}", "Sin ubicación");

                                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Emisión Pase BREAK BULK", "La carga no presenta ubicación..", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                                objPaseBRBK.FACTURADO_DESC, objPaseBRBK.AGENTE_DESC, out MensajeCasos, false);

                                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", MensajeCasos));
                                            return;
                                        }
                                    }


                                    //nuevo codigo
                                    //datos del turno
                                    CultureInfo enUS = new CultureInfo("en-US");

                                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                                    {
                                        HoraHasta = "00:00";
                                        Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                                        if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase Mes/Día/año</b>"));
                                            this.TxtFechaHasta.Focus();
                                        }




                                        List_Turnos = new List<Cls_Bil_Turnos>();
                                        List_Turnos.Clear();

                                        var Turnos = PasePuerta.TurnoBRBK.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, objPaseBRBK.CNTR_UBICACION, objPaseBRBK.idProducto.Value);
                                        if (Turnos.Exitoso)
                                        {
                                            //turno por defecto
                                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "", "0"), Turno = "* Seleccione *" });
                                            var LinqQueryTur = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                                select new
                                                                {
                                                                    IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm"), (Tbl.Bultos == null ? 0 : Tbl.Bultos)),
                                                                    Turno = string.Format("{0}", Tbl.Turno)
                                                                }).ToList().OrderBy(x => x.Turno);

                                            foreach (var Items in LinqQueryTur)
                                            {
                                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                            }

                                            this.CboTurnos.DataSource = List_Turnos;
                                            this.CboTurnos.DataTextField = "Turno";
                                            this.CboTurnos.DataValueField = "IdPlan";
                                            this.CboTurnos.DataBind();
                                        }
                                        else
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                                            return;

                                        }


                                    }



                                    Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                                  
                                }
                                else
                                {
                                   

                                    this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", Carga.MensajeProblema));
                                    return;
                                }



                            }
                            else
                            {
                              
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", Carga.MensajeProblema));
                            return;
                        }

                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", ListaContenedores.MensajeProblema));
                        return;

                    }
                   

                    this.Ocultar_Mensaje();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                }
            }

        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            CultureInfo enUS = new CultureInfo("en-US");
            string id_carga = string.Empty;
            EsPasesinTurno = false;

            if (Response.IsClientConnected)
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                try
                {

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Por favor ingresar el número de la carga MRN </b>"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Por favor ingresar el número de la carga MSN </b>"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b><b>Informativo! Por favor ingresar el número de la carga HSN </b>"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    //instancia sesion
                    objPaseBRBK = Session["PaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar los pase a puerta de carga break bulk. </b>"));
                        return;
                    }

                   

                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen pases de puertas de break bulk pendientes para generar. </b>"));
                        return;
                    }

                    LoginName = objPaseBRBK.IV_USUARIO_CREA.Trim();

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    ////pase sin turno.
                    ////verificar si es pase sin turno
                    //var PaseSinturno = Pase_CFS.EsPaseSinTurno(LoginName, objPaseBRBK.MRN, objPaseBRBK.MSN, objPaseBRBK.HSN);
                    //if (PaseSinturno.Exitoso)
                    //{
                    //    if (PaseSinturno.Resultado)
                    //    {
                    //        EsPasesinTurno = true;
                    //    }
                    //    else
                    //    {
                    //        EsPasesinTurno = false;
                    //    }

                    //}
                    //else
                    //{
                     
                    //    this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseBRBK.CARGA, PaseSinturno.MensajeProblema));
                    //    return;
                    //}

                    ////pase con turno
                    //if (!EsPasesinTurno)
                    //{
                        /***********************************************************************************************************************************************
                        *valida que tenga un turno ingresado
                        **********************************************************************************************************************************************/
                    foreach (var Det in objPaseBRBK.Detalle)
                    {
                            
                        if (string.IsNullOrEmpty(Det.D_TURNO))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se ha ingresado el turno para poder generar el pase de puerta de la carga: {0} </b>", Det.CARGA));
                            this.CboTurnos.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(Det.ID_CIATRANS))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se ha ingresado la empresa de transporte para poder generar el pase de puerta de la carga: {0} </b>", Det.CARGA));
                            this.IdTxtempresa.Focus();
                            return;
                        }

                    }
                    //}


                    

                    /*contenedores seleccionados para emitir pase*/
                    var LinqQuery = (from Tbl in objPaseBRBK.Detalle.Where(Tbl => Tbl.CANTIDAD != 0)
                                     select new
                                     {
                                         ID_PPWEB = Tbl.ID_PPWEB,
                                         CARGA = string.Format("{0}-{1}-{2}", Tbl.MRN, Tbl.MSN, Tbl.HSN),
                                         MRN = Tbl.MRN,
                                         MSN = Tbl.MSN,
                                         HSN = Tbl.HSN,
                                         FACTURA = (Tbl.FACTURA == null) ? string.Empty : Tbl.FACTURA,
                                         AGENTE = (Tbl.AGENTE == null) ? string.Empty : Tbl.AGENTE,
                                         FACTURADO = (Tbl.FACTURADO == null) ? string.Empty : Tbl.FACTURADO,
                                         PAGADO = Tbl.PAGADO,
                                         GKEY = (Tbl.GKEY == null) ? 0 : Tbl.GKEY,
                                         REFERENCIA = (Tbl.REFERENCIA == null) ? string.Empty : Tbl.REFERENCIA,
                                         CONTENEDOR = (Tbl.CONTENEDOR == null) ? string.Empty : Tbl.CONTENEDOR,
                                         DOCUMENTO = (Tbl.DOCUMENTO == null) ? string.Empty : Tbl.DOCUMENTO,
                                         PRIMERA = (Tbl.PRIMERA == null) ? string.Empty : Tbl.PRIMERA,
                                         MARCA = (Tbl.MARCA == null) ? string.Empty : Tbl.MARCA,
                                         CANTIDAD = (Tbl.CANTIDAD == null) ? 0 : Tbl.CANTIDAD,
                                         BULTOS_HORARIOS = (Tbl.BULTOS_HORARIOS == null) ? 0 : Tbl.BULTOS_HORARIOS,
                                         CIATRANS = (Tbl.CIATRANS == null) ? string.Empty : Tbl.CIATRANS,
                                         CHOFER = (Tbl.CHOFER == null) ? string.Empty : Tbl.CHOFER,
                                         ID_CHOFER = (Tbl.ID_CHOFER == null) ? string.Empty : Tbl.ID_CHOFER,
                                         ID_CIATRANS = (Tbl.ID_CIATRANS == null) ? string.Empty : Tbl.ID_CIATRANS,
                                         PLACA = (Tbl.PLACA == null) ? string.Empty : Tbl.PLACA,
                                         FECHA_SALIDA = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                         FECHA_SALIDA_PASE = (Tbl.FECHA_SALIDA_PASE.HasValue ? Tbl.FECHA_SALIDA_PASE : null),
                                         FECHA_AUT_PPWEB = (Tbl.FECHA_AUT_PPWEB.HasValue ? Tbl.FECHA_AUT_PPWEB : null),
                                         HORA_AUT_PPWEB = (Tbl.HORA_AUT_PPWEB == null) ? string.Empty : Tbl.HORA_AUT_PPWEB,
                                         TIPO_CNTR = (Tbl.TIPO_CNTR == null) ? string.Empty : Tbl.TIPO_CNTR,
                                         ID_TURNO = (Tbl.ID_TURNO == null) ? 0 : Tbl.ID_TURNO,
                                         TURNO = (Tbl.TURNO == null) ? 0 : Tbl.TURNO,
                                         D_TURNO = (Tbl.D_TURNO == null) ? string.Empty : Tbl.D_TURNO,
                                         ID_PASE = (Tbl.ID_PASE == null) ? 0 : Tbl.ID_PASE,
                                         ESTADO = Tbl.ESTADO,
                                         ENVIADO = Tbl.ENVIADO,
                                         AUTORIZADO = Tbl.AUTORIZADO,
                                         VENTANILLA = Tbl.VENTANILLA,                                       
                                         USUARIO = Tbl.USUARIO_ING,
                                         TURNO_DESDE = (Tbl.TURNO_DESDE.HasValue ? Tbl.TURNO_DESDE : null),
                                         TURNO_HASTA = (Tbl.TURNO_HASTA.HasValue ? Tbl.TURNO_HASTA : null),
                                         CNTR_DD = (EsPasesinTurno ? true : Tbl.CNTR_DD),
                                         AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? string.Empty : Tbl.AGENTE_DESC,
                                         FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                         IMPORTADOR = (Tbl.IMPORTADOR == null) ? string.Empty : Tbl.IMPORTADOR,
                                         IMPORTADOR_DESC = (Tbl.IMPORTADOR_DESC == null) ? string.Empty : Tbl.IMPORTADOR_DESC,
                                         TRANSPORTISTA_DESC = (Tbl.TRANSPORTISTA_DESC == null) ? string.Empty : Tbl.TRANSPORTISTA_DESC,
                                         CHOFER_DESC = (Tbl.CHOFER_DESC == null) ? string.Empty : Tbl.CHOFER_DESC,
                                         SUB_SECUENCIA = (Tbl.SUB_SECUENCIA == null) ? string.Empty : Tbl.SUB_SECUENCIA,
                                         ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD

                                     }).ToList().OrderBy(x => x.CONTENEDOR);


                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/


                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {

                        //valida saldo
                        var Saldo = PasePuerta.SaldoCargaBRBK.SaldoPendienteBRBK(ClsUsuario.ruc, Det.CARGA);
                        if (Saldo.Exitoso)
                        {
                            var LinqSaldo = (from Tbl in Saldo.Resultado.Where(p => !string.IsNullOrEmpty(p.NUMERO_CARGA))
                                             select new
                                             {
                                                 SALDO_FINAL = Tbl.SALDO_FINAL

                                             }).FirstOrDefault();

                            if (LinqSaldo != null)
                            {
                                if (LinqSaldo.SALDO_FINAL == 0)
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe saldo de la carga: {0} pendiente para emitir pase de puerta..Saldo actual: {1}</b>", Det.CARGA, LinqSaldo.SALDO_FINAL));
                                    return;
                                }

                            }
                            else
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! al validar saldo de la carga...No existe información del saldo de la carga...</b>"));
                                this.TxtFechaHasta.Focus();
                                return;
                            }
                        }
                        else
                        {

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! al validar saldo de la carga...No existe información del saldo de la carga...</b>"));
                            return;
                        }

                        Pase_BRBK pase = new Pase_BRBK();
                        pase.ID_CARGA = Det.GKEY;
                        pase.ESTADO = "GN";
                        pase.FECHA_EXPIRACION = Det.FECHA_SALIDA_PASE;
                        
                        pase.ID_PLACA = Det.PLACA;
                        pase.ID_CHOFER = Det.ID_CHOFER;
                        pase.ID_EMPRESA = Det.ID_CIATRANS;
                        pase.CANTIDAD_CARGA = Det.CANTIDAD;
                        pase.USUARIO_REGISTRO = Det.USUARIO;
                        pase.TIPO_CARGA = "BRBK";
                     
                        pase.CONSIGNATARIO_ID = Det.IMPORTADOR;
                        pase.CONSIGNARIO_NOMBRE = Det.IMPORTADOR_DESC;
                        pase.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                        pase.CHOFER_DESC = Det.CHOFER_DESC;
                        pase.PPW = Det.ID_PPWEB;
                        pase.ID_UNIDAD = Det.ID_UNIDAD;


                       
                        var Resultado = pase.Insertar(Det.TURNO.Value, Det.CONTENEDOR, Det.BULTOS_HORARIOS.Value,Det.MRN, Det.MSN, Det.HSN, Det.CNTR_DD);
                        if (Resultado.Exitoso)
                        {
                            nTotal++;
                            if (nTotal == 1)
                            {
                                id_carga = securetext(Det.CARGA);
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar el pase de puerta para la carga: {0}, Total bultos {1} Existen los siguientes problemas: {2}, {3} </b>", Det.CARGA,Det.CANTIDAD , Resultado.MensajeInformacion, Resultado.MensajeProblema));
                            return;
                        }

                    }

                    if (nTotal != 0)
                    {
                        string link = string.Format("<a href='../pasepuertabrbk/imprimirpasebrbk.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta</a>", id_carga);

                        //limpiar
                        objPaseBRBK.Detalle.Clear();
                        objPaseBRBK.DetalleSubItem.Clear();

                        Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                       
                       
                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        this.Limpia_Campos();


                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar {0} pase de puerta con éxito, para proceder a imprimir los mismo, <br/>por favor dar click en el siguiente link: {1} </b>", nTotal, link));
                        return;
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Grabar Pase BREAK BULK", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }
           


        }

        #region "Eventos de la grilla de pases de puerta break bulk"

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    int TotalDetalle = 0;
                    int SaldoActual = 0;

                    this.Ocultar_Mensaje();
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Eliminar")
                    {
                        objPaseBRBK = Session["PaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;

                        //existe pase a remover
                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));
                        if (Detalle != null)
                        {
                            string Llave = Detalle.LLAVE;
                            Int64 IDX_ROW = Detalle.TURNO.Value;
                            Int64 SECUENCIA = Detalle.ID_TURNO.Value;

                            objReserva = new Cls_Bil_PasePuertaBRBK_Temporal();
                            objReserva.IDX_ROW = IDX_ROW;
                            objReserva.SECUENCIA = SECUENCIA;
                            objReserva.USUARIO_CREA = Detalle.USUARIO_ING;

                            objReserva.Delete(out cMensajes);
                            if (cMensajes != string.Empty)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Error! Al eliminar reserva de turno..{0}</b>", cMensajes));
                                return;

                            }


                            //remover pase
                            objPaseBRBK.Detalle.Remove(objPaseBRBK.Detalle.Where(p => p.LLAVE == Llave).FirstOrDefault());

                            TotalDetalle = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0).Sum(x => x.CANTIDAD.Value);

                            this.TxtActual.Text = TotalDetalle.ToString();

                            if (!int.TryParse(this.TxtSaldo.Text, out SaldoActual))
                            {

                            }

                            this.TxtSaldoFinal.Text = (SaldoActual - TotalDetalle).ToString();

                            //valida turno
                            if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                            {
                                CultureInfo enUS = new CultureInfo("en-US");
                                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                                HoraHasta = "00:00";
                                Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                                if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                                {
                                    this.Turno_Default();
                                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase Mes/Día/año</b>"));
                                    this.TxtFechaHasta.Focus();
                                }


                                //verificar si es pase sin turno
                                var PaseSinturnoNew = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, objPaseBRBK.MRN, objPaseBRBK.MSN, objPaseBRBK.HSN);
                                if (PaseSinturnoNew.Exitoso)
                                {
                                    if (PaseSinturnoNew.Resultado)
                                    {
                                        EsPasesinTurno = true;
                                    }
                                    else
                                    {
                                        EsPasesinTurno = false;
                                    }

                                }
                                else
                                {
                                    this.Turno_Default();
                                    this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseBRBK.CARGA, PaseSinturnoNew.MensajeProblema));
                                }


                                //si es pase sin turno
                                if (EsPasesinTurno)
                                {
                                    this.Pase_Sin_Turno_Default();
                                }
                                else
                                {
                                    List_Turnos = new List<Cls_Bil_Turnos>();
                                    List_Turnos.Clear();

                                    var Turnos = PasePuerta.TurnoBRBK.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, objPaseBRBK.CNTR_UBICACION, objPaseBRBK.idProducto.Value);
                                    if (Turnos.Exitoso)
                                    {
                                        //turno por defecto
                                        List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "", "0"), Turno = "* Seleccione *" });
                                        var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                         select new
                                                         {
                                                             IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm"), (Tbl.Bultos == null ? 0 : Tbl.Bultos)),
                                                             Turno = string.Format("{0}", Tbl.Turno)
                                                         }).ToList().OrderBy(x => x.Turno);

                                        foreach (var Items in LinqQuery)
                                        {
                                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                        }

                                        this.CboTurnos.DataSource = List_Turnos;
                                        this.CboTurnos.DataTextField = "Turno";
                                        this.CboTurnos.DataValueField = "IdPlan";
                                        this.CboTurnos.DataBind();

                                    }
                                    else
                                    {
                                        this.Turno_Default();
                                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                                        return;

                                    }
                                }

                            }

                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información temporal de los turnos a REMOVER: {0} </b>", t.ToString()));
                            return;
                        }

                      
                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                       

                        Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;


                        this.Actualiza_Paneles();

                    }

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                    return;

                }
            }

        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
           
        }

        #endregion

        #region "Eventos Grilla Tarja"

        protected void chkPaseTarja_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPaseTarja = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPaseTarja.NamingContainer;
                Label LblConsecutivo = (Label)item.FindControl("LblConsecutivo");

                if (!Int64.TryParse(LblConsecutivo.Text, out ConsecutivoSelec))
                {
                    //campo no es numerico
                    this.Mostrar_Mensaje(2,string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error al convertir consecutivo del sub.ítem: {0}", LblConsecutivo.Text));
                    return;
                }

                //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;

                //actualiza datos del contenedor
                objPaseBRBK = Session["PaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                var Detalle = objPaseBRBK.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPaseTarja.Checked;
                    //si desmarca, limpia los campos
                    if (!Detalle.VISTO)
                    {
                        Detalle.CIATRANS = string.Empty;
                        Detalle.CHOFER = string.Empty;
                        Detalle.PLACA = string.Empty;
                        Detalle.TRANSPORTISTA_DESC = string.Empty;
                        Detalle.CHOFER_DESC = string.Empty;
                    }
                }

               

                Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

              



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }

        protected void tablePagination_Tarja_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void tablePagination_Tarja_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox Chk = e.Item.FindControl("chkPaseTarja") as CheckBox;
                Label Estado = e.Item.FindControl("LblEstado") as Label;
               
                if (Estado.Text.Equals("NO"))
                {
                    Chk.Enabled = false;
                   
                }
            }
        }

        #endregion

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);
               
                return;
            }

            this.IsAllowAccess();

            this.banmsg.Visible = IsPostBack;
            this.banmsg_det.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                this.banmsg_det.InnerText = string.Empty;
                
            }

            ClsUsuario = Page.Tracker();
            if (ClsUsuario != null)
            {
                if (!Page.IsPostBack)
                {
                    this.Limpia_Campos();
                }
                   
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                if (Response.IsClientConnected)
                {
                   
                    //banmsg.Visible = false;
                }

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());
                Server.HtmlEncode(this.Txtempresa.Text.Trim());
                Server.HtmlEncode(this.TxtChofer.Text.Trim());
                Server.HtmlEncode(this.TxtPlaca.Text.Trim());
                Server.HtmlEncode(this.TxtFechaHasta.Text.Trim());
                Server.HtmlEncode(this.TxtContenedorSeleccionado.Text.Trim());
                Server.HtmlEncode(this.TxtRetirados.Text.Trim());
                Server.HtmlEncode(this.TxtSaldo.Text.Trim());
                Server.HtmlEncode(this.TxtBodega.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                   

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}