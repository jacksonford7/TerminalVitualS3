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
  

    public partial class emisionpasecfs : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objPaseCFS = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_PasePuertaCFS_SubItems objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
        private Cls_Bil_PasePuertaCFS_Detalle objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();

    
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }

        private Cls_Bil_Stock_Pases_CFS objCtock = new Cls_Bil_Stock_Pases_CFS();
        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

        private P2D_Traza_Liftif objLogLiftif = new P2D_Traza_Liftif();
        private P2D_Actualiza_PasePuerta objActualiza_Pase = new P2D_Actualiza_PasePuerta();
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
                detalle_carga.Tipo = TipoCarga.CFS; //opcional
                detalle_carga.Titulo = "Modulo de Facturación Importación CFS"; //opcional
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
            UPDATOSCLIENTE.Update();
            UPDESADUANAMIENTO.Update();
            UPBOTONES.Update();
            UPCONTENEDOR.Update();
            UPFECHASALIDA.Update();
            UPTURNO.Update();
            UPCAS.Update();
            UPACEPTA.Update();
           
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
            this.TxtFechaHasta.Text = string.Empty;
            this.TxtContenedorSeleccionado.Text = string.Empty;
            this.TxtDesaduanamiento.Text = string.Empty;

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
            objPaseCFS = new Cls_Bil_PasePuertaCFS_Cabecera();
            Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;
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

        private void Pintar_Grilla()
        {
            foreach (RepeaterItem xitem in tablePagination_Tarja.Items)
            {
                CheckBox chkPaseTarja = xitem.FindControl("chkPaseTarja") as CheckBox;

                Label LblCarga = xitem.FindControl("LblCarga") as Label;
                Label LblConsecutivo = xitem.FindControl("LblConsecutivo") as Label;
                Label LblCantidad = xitem.FindControl("LblCantidad") as Label;
                Label LblEmpresa = xitem.FindControl("LblEmpresa") as Label;
                Label LblChofer = xitem.FindControl("LblChofer") as Label;
                Label LblPlaca = xitem.FindControl("LblPlaca") as Label;
                Label LblEstado = xitem.FindControl("LblEstado") as Label;
                if (chkPaseTarja.Checked == true)
                {

                    LblCarga.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblConsecutivo.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblCantidad.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEmpresa.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblChofer.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblPlaca.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEstado.ForeColor = System.Drawing.Color.PaleVioletRed;
                }

            }
        }

        private void Carga_CboCiudad()
        {
            try
            {
                List<P2D_Ciudad> Listado = P2D_Ciudad.CboCiudad(out cMensajes);

                this.CboCiudad.DataSource = Listado;
                this.CboCiudad.DataTextField = "DESC_CIUDAD";
                this.CboCiudad.DataValueField = "ID_CIUDAD";
                this.CboCiudad.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboCiudad", "Hubo un error al cargar ciudades", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

        private void Carga_CboZonas()
        {
            try
            {
                List<P2D_Zona> Listado = P2D_Zona.CboZona(out cMensajes);

                this.CboZonas.DataSource = Listado;
                this.CboZonas.DataTextField = "ZONA";
                this.CboZonas.DataValueField = "ID_ZONA";
                this.CboZonas.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboZonas", "Hubo un error al cargar zonas", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

        private static string leyenda_servicio_p2d()
        {
            List<Cls_Bil_Configuraciones> Leyenda = Cls_Bil_Configuraciones.Parametros(out TextoLeyenda);
            if (!String.IsNullOrEmpty(TextoLeyenda))
            {
                return string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", TextoLeyenda);
            }

            var LinqLeyenda = (from Tope in Leyenda.Where(Tope => Tope.NOMBRE.Equals("TEXTO_P2D_PASE_PUERTA"))
                               select new
                               {
                                   VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                               }).FirstOrDefault();

            if (LinqLeyenda != null)
            {
                return LinqLeyenda.VALOR == null ? "" : LinqLeyenda.VALOR;
            }


            return "";
        }

        #endregion

      

        #region "Eventos del Formulario"

        #region "Seleccionar todos"
        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
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
                        OcultarLoading("1");
                        return;
                    }

                    bool ChkEstado = this.ChkTodos.Checked;
                    CultureInfo enUS = new CultureInfo("en-US");
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todos los subitems o bultos... </b>"));
                        return;
                    }
                    if (objPaseCFS.DetalleSubItem.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, para poder seleccionar... </b>"));
                        return;
                    }

                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI"))
                                                       select p.CONSECUTIVO).ToList();

                    if (LinqValidaSubItemsFaltantes.Count > 0)
                    {
                        //proceso de marcar subitems
                        foreach (var Det in objPaseCFS.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI")))
                        {
                            ConsecutivoSelec = Det.CONSECUTIVO.Value;
                            var Detalle = objPaseCFS.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
                            if (Detalle != null)
                            {
                                Detalle.VISTO = ChkEstado;
                               
                            }
                        }

                        tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem.Where(p => string.IsNullOrEmpty(p.MARCADO_SUBITEMS));
                        tablePagination_Tarja.DataBind();

                    }


                    Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                    this.Pintar_Grilla();

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ChkTodos_CheckedChanged), "ChkTodos_CheckedChanged", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                    return;

                }
            }
               
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
                    objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar la fecha del turno... </b>"));
                        return;
                    }
                    if (objPaseCFS.DetalleSubItem.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, para poder seleccionar la fecha del turno... </b>"));
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

                    FechaFacturaHasta = objPaseCFS.FECHA_SALIDA.Value;

                    if (FechaActualSalida.Date < System.DateTime.Now.Date)
                    {
                        this.Turno_Default();
                        this.TxtFechaHasta.Text = objPaseCFS.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), System.DateTime.Now.ToString("MM/dd/yyyy")));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (FechaActualSalida.Date > FechaFacturaHasta.Date)
                    {
                        this.Turno_Default();
                        this.TxtFechaHasta.Text = objPaseCFS.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), FechaFacturaHasta.ToString("MM/dd/yyyy")));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    //servicio de port to door
                    if (this.acepta_servicio.Visible)
                    {
                        if (!objPaseCFS.EXPRESS)
                        {
                            if (FechaActualSalida.Date == System.DateTime.Now.Date)
                            {
                                this.Turno_Default();
                                this.TxtFechaHasta.Text = objPaseCFS.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser igual que la fecha actual: {1}, sólo el servicio EXPRESS, puede seleccionar esta fecha.. </b>", FechaActualSalida.ToString("MM/dd/yyyy"), System.DateTime.Now.ToString("MM/dd/yyyy")));
                                this.TxtFechaHasta.Focus();
                                return;
                            }

                        }
                    }


                    //verificar si es pase sin turno
                    var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, objPaseCFS.MRN, objPaseCFS.MSN, objPaseCFS.HSN);
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
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseCFS.CARGA, PaseSinturno.MensajeProblema));
                    }

                    //si es pase sin turno
                    if (EsPasesinTurno)
                    {
                        this.Pase_Sin_Turno_Default();
                    }
                    else
                    {
                        List<Int64> Lista = new List<Int64>();
                        foreach (var Det in objPaseCFS.DetalleSubItem)
                        {
                            Lista.Add(Det.CONSECUTIVO.Value);
                        }

                        var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista,this.acepta_servicio.Visible, objPaseCFS.EXPRESS);
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

                    this.CboTurnos.Focus();
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
                    Int64 IDDISPONIBLEDET = 0;
                    string MRN = string.Empty;
                    string MSN = string.Empty;
                    string HSN = string.Empty;
                    string BODEGA = string.Empty;

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
                    objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder agregar un turno... </b>"));
                        return;
                    }
                    if (objPaseCFS.DetalleSubItem.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, para poder agregar un turno... </b>"));
                        return;
                    }

                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == false || string.IsNullOrEmpty(x.MARCADO_SUBITEMS))
                                                       select p.CONSECUTIVO).ToList();

                    if (LinqValidaSubItemsFaltantes.Count > 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Existen bultos o subitems sin seleccionar, debe completar toda la partida para poder agregar el turno. </b>"));
                        return;
                    }


                    //verificar si es pase sin turno
                    var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, objPaseCFS.MRN, objPaseCFS.MSN, objPaseCFS.HSN);
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
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseCFS.CARGA, PaseSinturno.MensajeProblema));
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

                    //pase con turno
                    if (!EsPasesinTurno)
                    {
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

                    }


                    List<Cls_Bil_Configuraciones> ValidaFinSemana = Cls_Bil_Configuraciones.Get_Validacion("FINSEMANA", out cMensajes);
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

                    //validar stock
                    
                    if (!EsPasesinTurno)
                    {
                        Tarjas = string.Join(",", objPaseCFS.DetalleSubItem.Where(x => x.CONSECUTIVO != 0).Select(kvp => kvp.CONSECUTIVO));
                        TotalBultos = objPaseCFS.DetalleSubItem.Where(x => x.CONSECUTIVO != 0).Sum(x => x.CANTIDAD);

                        var lst = new StringBuilder();
                        lst.Append("<TARJA>");
                        foreach (var i in objPaseCFS.DetalleSubItem.Where(x => x.CONSECUTIVO != 0))
                        {
                            lst.AppendFormat("<VALOR CONSECUTIVO=\"{0}\"/>", i.CONSECUTIVO);
                        }
                        lst.Append("</TARJA>");

                        //saca bultos
                        List<Cls_Bil_Stock_Pases_CFS> Bultos = Cls_Bil_Stock_Pases_CFS.Carga_Cantidad_Bultos_Pase(lst.ToString(), out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.Error en cantidad de bultos....{0}</b>", cMensajes));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                        foreach (var Det in Bultos)
                        {
                            TotalBultos = Det.TOTAL;
                        }

                         Int64 Id_Turno = Convert.ToInt16(TurnoSelect.Split('-').ToList()[0].Trim());
                        //validacion de stock
                        List<Cls_Bil_Stock_Pases_CFS> Stock = Cls_Bil_Stock_Pases_CFS.Carga_Stock_Turno(FechaActualSalida, lst.ToString(), Id_Turno, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.Error en Stock....{0}</b>", cMensajes));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                        foreach (var Det in Stock)
                        {
                            int stock_actual = Det.Bultos - TotalBultos;
                            if (stock_actual < 0)
                            {

                                /*
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede seleccionar el turno {0}, no existe disponibilidad de retiro para este horario..Intente con otro turno....</b>", this.CboTurnos.SelectedItem.ToString().Substring(0,5)));
                                this.TxtFechaHasta.Focus();

                                //vuelve a cargar turnos
                                List<Int64> Lista = new List<Int64>();
                                foreach (var Tur in objPaseCFS.DetalleSubItem)
                                {
                                    Lista.Add(Tur.CONSECUTIVO.Value);
                                }

                                var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista);
                                if (Turnos.Exitoso)
                                {
                                    List_Turnos = new List<Cls_Bil_Turnos>();
                                    List_Turnos.Clear();
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
                                   
                                    return;

                                }
                                */
                                break;
                                //return;

                            }
                            else
                            {
                                break;
                            }
                        }


                    }
                        

                    //actualizado datos de fecha de salida y horqa del turno, para el pase
                    foreach (var Det in objPaseCFS.Detalle)
                    {
                        string LLAVE = Det.LLAVE;
                        var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.LLAVE.Equals(LLAVE));
                        if (Detalle != null)
                        {
                            //actualizar datos pase sin turno
                            if (EsPasesinTurno)
                            {
                                Detalle.D_TURNO = string.Empty;
                                Detalle.TURNO = 0;
                                Detalle.ID_TURNO = 0;
                                Detalle.TURNO_DESDE = null;
                                Detalle.TURNO_HASTA = null;
                                Detalle.FECHA_SALIDA_PASE = FechaActualSalida;
                                Detalle.BULTOS_HORARIOS = 0;

                            }
                            else
                            {
                                Detalle.D_TURNO = this.CboTurnos.SelectedItem.ToString().Substring(0,5);
                                Detalle.TURNO = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());//ID TURNO
                                Detalle.ID_TURNO = Convert.ToInt16(TurnoSelect.Split('-').ToList()[1].Trim());//secuencia turno
                                Detalle.TURNO_DESDE = FechaTurnoInicio;
                                Detalle.TURNO_HASTA = FechaTurnoFinal;
                                Detalle.BULTOS_HORARIOS = Convert.ToInt16(TurnoSelect.Split('-').ToList()[4].Trim());

                                HoraHasta = Detalle.D_TURNO;
                                Fecha = string.Format("{0} {1}", FechaActualSalida.Date.ToString("MM/dd/yyyy"), HoraHasta);
                                if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                                {
                                    this.Turno_Default();
                                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para poder agregar la información del turno, Mes/Día/año </b>"));
                                    this.TxtFechaHasta.Focus();
                                    return;
                                }

                                Detalle.FECHA_SALIDA_PASE = FechaActualSalida;
                                
                                IDDISPONIBLEDET = Detalle.TURNO.Value;//ID TURNO
                                MRN = Detalle.MRN;
                                MSN = Detalle.MSN;
                                HSN = Detalle.HSN;
                                BODEGA = "TMP";
                                
                            }

                        }
                    }

                    tablePagination.DataSource = objPaseCFS.Detalle;
                    tablePagination.DataBind();

                    Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                    this.Actualiza_Paneles();


                    //inserta en tabla temporal
                    if (!EsPasesinTurno)
                    {
                        var lst = new StringBuilder();
                        lst.Append("<TARJA>");
                        foreach (var i in objPaseCFS.DetalleSubItem.Where(x => x.CONSECUTIVO != 0))
                        {
                            lst.AppendFormat("<VALOR CONSECUTIVO=\"{0}\"/>", i.CONSECUTIVO);
                        }
                        lst.Append("</TARJA>");

                        objCtock.IDDISPONIBLEDET = IDDISPONIBLEDET;
                        objCtock.FECHA = FechaActualSalida;
                        objCtock.Bultos = TotalBultos;
                        objCtock.MRN = MRN;
                        objCtock.MSN = MSN;
                        objCtock.HSN = HSN;
                        objCtock.BODEGA = BODEGA;
                        objCtock.subitems = lst.ToString();
                        objCtock.USUARIOING = ClsUsuario.loginname;

                        var nProceso = objCtock.SaveTransaction(out cMensajes);
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {
                            
                            this.Mostrar_Mensaje(2,string.Format("<b>Error! No se pudo grabar datos del turno.{0}</b>", cMensajes));
                            return;
                        }
                        else
                        {
                            List<Int64> Lista = new List<Int64>();
                            foreach (var Det in objPaseCFS.DetalleSubItem)
                            {
                                Lista.Add(Det.CONSECUTIVO.Value);
                            }

                            var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista, this.acepta_servicio.Visible, objPaseCFS.EXPRESS);
                            if (Turnos.Exitoso)
                            {
                                //  Turno = string.Format("{0} - Puede Retirar: {1} Bultos", Tbl.Turno, Tbl.Bultos)
                                List_Turnos = new List<Cls_Bil_Turnos>();
                                List_Turnos.Clear();
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

                            this.Actualiza_Paneles();
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



        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                
                try
                {

                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }


                    CultureInfo enUS = new CultureInfo("en-US");

                    this.Ocultar_Mensaje();


                    string IdEmpresa = string.Empty;
                    string DesEmpresa = string.Empty;
                    string IdChofer = string.Empty;
                    string DesChofer = string.Empty;
                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();


                    if (string.IsNullOrEmpty(Txtempresa.Text) && !this.acepta_servicio.Visible)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar la Compañía de Transporte para poder agregar la información </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }

                    objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar el o los pases de puerta... </b>"));
                        return;
                    }
                    if (objPaseCFS.DetalleSubItem.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, para poder generar el o los pases de puerta </b>"));
                        return;
                    }

                    var LinqValidaSubItems = (from p in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true)
                                              select p.CONSECUTIVO).ToList();

                    if (LinqValidaSubItems.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los subitems a relacionar con una empresa de transporte. </b>"));
                        return;
                    }

                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true && string.IsNullOrEmpty(x.MARCADO_SUBITEMS))
                                                       select p.CONSECUTIVO).ToList();

                    if (LinqValidaSubItemsFaltantes.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen subitems pendientes para realizar pases de puerta CFS. </b>"));
                        return;
                    }

                    //si tiene el servicio de p2d, debe seleciconar todos los subitems
                    if (this.acepta_servicio.Visible)
                    {
                        var LinqValidaSeleccion = (from p in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == false)
                                                  select p.CONSECUTIVO).ToList();

                        if (LinqValidaSeleccion.Count != 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar todos los subitems a relacionar con una empresa de transporte. (Servicio PORT TO DOOR)</b>"));
                            return;
                        }
                    }

                   

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //valida que exista Empresa Transporte 
                    if (!string.IsNullOrEmpty(Txtempresa.Text) && !this.acepta_servicio.Visible)
                    {
                        EmpresaSelect = this.Txtempresa.Text.Trim();
                        if (EmpresaSelect.Split('-').ToList().Count > 1)
                        {
                            //Int32 p = EmpresaSelect.Split('-').ToList().Count;
                            IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();
                            //DesEmpresa = EmpresaSelect.Split('-').ToList()[1].Trim();
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
                    if (!string.IsNullOrEmpty(TxtChofer.Text) && !this.acepta_servicio.Visible)
                    {
                        ChoferSelect = this.TxtChofer.Text.Trim();
                        if (ChoferSelect.Split('-').ToList().Count > 1)
                        {
                            //Int32 p = ChoferSelect.Split('-').ToList().Count;
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
                    if (!string.IsNullOrEmpty(TxtPlaca.Text) && !this.acepta_servicio.Visible)
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
                    string Tarjas = string.Join(",", objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true && string.IsNullOrEmpty(x.MARCADO_SUBITEMS)).Select(kvp => kvp.CONSECUTIVO));
                    //total de bultos
                    var TotalBultosSelect = objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true && string.IsNullOrEmpty(x.MARCADO_SUBITEMS)).Sum(p => p.CANTIDAD);
                    //actualizado datos de chofer y subitems seleccionados, por cada vez que se agrega 
                    foreach (var Det in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true && string.IsNullOrEmpty(x.MARCADO_SUBITEMS)))
                    {
                        ConsecutivoSelec = Det.CONSECUTIVO.Value;
                        var Detalle = objPaseCFS.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
                        if (Detalle != null)
                        {
                            if (this.acepta_servicio.Visible)
                            {
                                Detalle.CHOFER = string.Empty;
                                Detalle.PLACA = string.Empty;
                                Detalle.ID_CHOFER = string.Empty;
                                Detalle.TRANSPORTISTA_DESC = string.Empty;
                                Detalle.CHOFER_DESC = string.Empty;

                                CIATRANS = Detalle.CIATRANS;
                                ID_CIATRANS = Detalle.ID_CIATRANS;

                                TRANSPORTISTA_DESC = (CIATRANS.Split('-').ToList().Count >= 3 ? string.Format("{0} - {1}", CIATRANS.Split('-').ToList()[1].Trim(), CIATRANS.Split('-').ToList()[2].Trim()) : CIATRANS.Split('-').ToList()[1].Trim());
                                Detalle.TRANSPORTISTA_DESC = TRANSPORTISTA_DESC;

                            }
                            else
                            {
                                Detalle.CIATRANS = EmpresaSelect;
                                Detalle.CHOFER =  ChoferSelect;
                                Detalle.PLACA = PlacaSelect;
                                Detalle.ID_CIATRANS =  IdEmpresa;
                                Detalle.ID_CHOFER =IdChofer;
                                Detalle.TRANSPORTISTA_DESC =  DesEmpresa;
                                Detalle.CHOFER_DESC =DesChofer;
                            }
                           
                            Detalle.MARCADO_SUBITEMS = Tarjas;
                        }
                    }

                    tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem.Where(p => p.VISTO == false && string.IsNullOrEmpty(p.MARCADO_SUBITEMS));
                    tablePagination_Tarja.DataBind();


                    //si exsiten cantidades pendientes o seleccionadas
                    if (TotalBultosSelect != 0)
                    {
                        objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();
                        objDetallePaseCFS.FECHA = objPaseCFS.FECHA;
                        objDetallePaseCFS.MRN = objPaseCFS.MRN;
                        objDetallePaseCFS.MSN = objPaseCFS.MSN;
                        objDetallePaseCFS.HSN = objPaseCFS.HSN;
                        objDetallePaseCFS.IV_USUARIO_CREA = objPaseCFS.IV_USUARIO_CREA;
                        objDetallePaseCFS.SESION = objPaseCFS.SESION;

                        objDetallePaseCFS.FACTURA = objPaseCFS.FACTURA;
                        objDetallePaseCFS.CARGA = objPaseCFS.CARGA;
                        objDetallePaseCFS.AGENTE = objPaseCFS.AGENTE;
                        objDetallePaseCFS.FACTURADO = objPaseCFS.FACTURADO;
                        objDetallePaseCFS.PAGADO = objPaseCFS.PAGADO;
                        objDetallePaseCFS.GKEY = objPaseCFS.GKEY;
                        objDetallePaseCFS.REFERENCIA = objPaseCFS.REFERENCIA;
                        objDetallePaseCFS.CONTENEDOR = objPaseCFS.CONTENEDOR;
                        objDetallePaseCFS.DOCUMENTO = objPaseCFS.DOCUMENTO;
                        objDetallePaseCFS.PRIMERA = objPaseCFS.PRIMERA;
                        objDetallePaseCFS.MARCA = objPaseCFS.MARCA;
                        objDetallePaseCFS.CANTIDAD = objPaseCFS.CANTIDAD;
                        objDetallePaseCFS.CIATRANS = objPaseCFS.CIATRANS;
                        objDetallePaseCFS.CHOFER = objPaseCFS.CHOFER;
                        objDetallePaseCFS.PLACA = objPaseCFS.PLACA;
                        objDetallePaseCFS.FECHA_SALIDA = objPaseCFS.FECHA_SALIDA;
                        objDetallePaseCFS.CNTR_DD = objPaseCFS.CNTR_DD;
                        objDetallePaseCFS.AGENTE_DESC = objPaseCFS.AGENTE_DESC;
                        objDetallePaseCFS.FACTURADO_DESC = objPaseCFS.FACTURADO_DESC;
                        objDetallePaseCFS.IMPORTADOR = objPaseCFS.IMPORTADOR;
                        objDetallePaseCFS.IMPORTADOR_DESC = objPaseCFS.IMPORTADOR_DESC;
                        objDetallePaseCFS.FECHA_SALIDA_PASE = objPaseCFS.FECHA_SALIDA_PASE;
                        objDetallePaseCFS.FECHA_AUT_PPWEB = objPaseCFS.FECHA_AUT_PPWEB;
                        objDetallePaseCFS.HORA_AUT_PPWEB = objPaseCFS.HORA_AUT_PPWEB;
                        objDetallePaseCFS.TIPO_CNTR = objPaseCFS.TIPO_CNTR;
                        objDetallePaseCFS.ID_TURNO = objPaseCFS.ID_TURNO;
                        objDetallePaseCFS.TURNO = objPaseCFS.TURNO;
                        objDetallePaseCFS.D_TURNO = objPaseCFS.D_TURNO;
                        objDetallePaseCFS.ID_PASE = objPaseCFS.ID_PASE;
                        objDetallePaseCFS.ESTADO = objPaseCFS.ESTADO;
                        objDetallePaseCFS.ENVIADO = objPaseCFS.ENVIADO;
                        objDetallePaseCFS.AUTORIZADO = objPaseCFS.AUTORIZADO;
                        objDetallePaseCFS.VENTANILLA = objPaseCFS.VENTANILLA;
                        objDetallePaseCFS.USUARIO_ING = objPaseCFS.USUARIO_ING;
                        objDetallePaseCFS.FECHA_ING = objPaseCFS.FECHA_ING;
                        objDetallePaseCFS.USUARIO_MOD = objPaseCFS.USUARIO_MOD;
                        objDetallePaseCFS.ESTADO_PAGO = objPaseCFS.ESTADO_PAGO;
                        objDetallePaseCFS.ID_PPWEB = objPaseCFS.ID_PPWEB;

                        objDetallePaseCFS.CANTIDAD = TotalBultosSelect;

                        if (this.acepta_servicio.Visible)
                        {
                            objDetallePaseCFS.ID_CIATRANS = ID_CIATRANS;
                            objDetallePaseCFS.CIATRANS = CIATRANS;
                            objDetallePaseCFS.ID_CHOFER = string.Empty;
                            objDetallePaseCFS.CHOFER =string.Empty;
                            objDetallePaseCFS.PLACA = string.Empty;
                            objDetallePaseCFS.TRANSPORTISTA_DESC = TRANSPORTISTA_DESC;
                            objDetallePaseCFS.CHOFER_DESC = string.Empty;
                        }
                        else
                        {
                            objDetallePaseCFS.ID_CIATRANS = IdEmpresa;
                            objDetallePaseCFS.ID_CHOFER = IdChofer;
                            objDetallePaseCFS.CIATRANS = EmpresaSelect;
                            objDetallePaseCFS.CHOFER = ChoferSelect;
                            objDetallePaseCFS.PLACA = PlacaSelect;
                            objDetallePaseCFS.TRANSPORTISTA_DESC = DesEmpresa;
                            objDetallePaseCFS.CHOFER_DESC = DesChofer;
                        }

                        objDetallePaseCFS.SUB_SECUENCIA = Tarjas;
                        objDetallePaseCFS.LLAVE = Tarjas;
                        objDetallePaseCFS.ID_UNIDAD = objPaseCFS.ID_UNIDAD;
                        objPaseCFS.Detalle.Add(objDetallePaseCFS);
                    }
                    else
                    {
                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: No se pudo presentar por pantalla el detalle de pases a emitir: {0}", objPaseCFS.CARGA);

                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Problemas al presentar detalle de pases a emitir CFS", MensajesErrores.Trim(), objPaseCFS.CARGA,
                            objPaseCFS.FACTURADO_DESC, objPaseCFS.AGENTE_DESC, out MensajeCasos, false);

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} {1} </b>", MensajesErrores, MensajeCasos));

                    }

                    tablePagination.DataSource = objPaseCFS.Detalle;
                    tablePagination.DataBind();

                    //turno cfs
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
                        var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, objPaseCFS.MRN, objPaseCFS.MSN, objPaseCFS.HSN);
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
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseCFS.CARGA, PaseSinturno.MensajeProblema));
                        }


                        //si es pase sin turno
                        if (EsPasesinTurno)
                        {
                            this.Pase_Sin_Turno_Default();
                        }
                        else
                        {
                            //agrega turnos
                            List<Int64> Lista = new List<Int64>();
                            foreach (var Det in objPaseCFS.DetalleSubItem)
                            {
                                Lista.Add(Det.CONSECUTIVO.Value);
                            }

                            var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista, this.acepta_servicio.Visible, objPaseCFS.EXPRESS);
                            if (Turnos.Exitoso)
                            {
                                //turno por defecto
                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Seleccione *" });
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




                    Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                    this.Pintar_Grilla();
                    this.Actualiza_Paneles();

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAgregar_Click), "AgregarTransportista", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));

                }

            }

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");


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

                    tablePagination_Tarja.DataSource = null;
                    tablePagination_Tarja.DataBind();

                    this.acepta_servicio.Visible = false;
                    this.UPACEPTA.Update();

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Carga = PasePuerta.Pase_WebCFS.ObtenerCargaPaseCFS(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc);
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
                                             CANTIDAD =  (Tbl.CANTIDAD == null) ? 0 : Tbl.CANTIDAD,
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
                                             ESTADO_PAGO = (Tbl.PAGADO==true ? "SI" : "NO"),
                                             ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD
                                         }).FirstOrDefault();

                        if (LinqQuery != null)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                            objPaseCFS.FECHA = DateTime.Now;
                            objPaseCFS.MRN = LinqQuery.MRN;
                            objPaseCFS.MSN = LinqQuery.MSN;
                            objPaseCFS.HSN = LinqQuery.HSN;
                            objPaseCFS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseCFS.SESION = this.hf_BrowserWindowName.Value;

                            objPaseCFS.FACTURA = LinqQuery.FACTURA;
                            objPaseCFS.CARGA = LinqQuery.CARGA;
                            objPaseCFS.AGENTE = LinqQuery.AGENTE;
                            objPaseCFS.FACTURADO = LinqQuery.FACTURADO;
                            objPaseCFS.PAGADO = LinqQuery.PAGADO;
                            objPaseCFS.GKEY = LinqQuery.GKEY;
                            objPaseCFS.REFERENCIA = LinqQuery.REFERENCIA;
                            objPaseCFS.CONTENEDOR = LinqQuery.CONTENEDOR;
                            objPaseCFS.DOCUMENTO = LinqQuery.DOCUMENTO;
                            objPaseCFS.PRIMERA = LinqQuery.PRIMERA;
                            objPaseCFS.MARCA = LinqQuery.MARCA;
                            objPaseCFS.CANTIDAD = LinqQuery.CANTIDAD;
                            objPaseCFS.CIATRANS = string.Empty;
                            objPaseCFS.CHOFER = string.Empty;
                            objPaseCFS.PLACA = string.Empty;
                            objPaseCFS.FECHA_SALIDA = LinqQuery.FECHA_SALIDA;
                            objPaseCFS.CNTR_DD = LinqQuery.CNTR_DD.Value;
                            objPaseCFS.AGENTE_DESC = LinqQuery.AGENTE_DESC;
                            objPaseCFS.FACTURADO_DESC = LinqQuery.FACTURADO_DESC;
                            objPaseCFS.IMPORTADOR = LinqQuery.IMPORTADOR;
                            objPaseCFS.IMPORTADOR_DESC = LinqQuery.IMPORTADOR_DESC;

                            if (LinqQuery.CNTR_DD.Value)
                            {
                                objPaseCFS.FECHA_SALIDA_PASE = LinqQuery.FECHA_SALIDA_PASE;
                                this.TxtDesaduanamiento.Text = "SI";
                                this.TxtFechaHasta.Text = objPaseCFS.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                this.TxtFechaCas.Text = objPaseCFS.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                objPaseCFS.FECHA_SALIDA_PASE = LinqQuery.FECHA_SALIDA_PASE;
                                this.TxtDesaduanamiento.Text = "NO";
                                this.TxtFechaHasta.Text = objPaseCFS.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                this.TxtFechaCas.Text = objPaseCFS.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                            }

                            this.TxtContenedorSeleccionado.Text = objPaseCFS.CANTIDAD.ToString();

                            if (LinqQuery.PAGADO == false)
                            {
                                this.BtnGrabar.Attributes.Add("disabled", "disabled");
                            }
                            else
                            {
                                this.BtnGrabar.Attributes.Remove("disabled");

                            }
                            

                            objPaseCFS.FECHA_AUT_PPWEB = LinqQuery.FECHA_AUT_PPWEB;
                            objPaseCFS.HORA_AUT_PPWEB = LinqQuery.HORA_AUT_PPWEB;

                            objPaseCFS.TIPO_CNTR = LinqQuery.TIPO_CNTR;
                            objPaseCFS.ID_TURNO = LinqQuery.ID_TURNO;
                            objPaseCFS.TURNO = LinqQuery.TURNO;
                            objPaseCFS.D_TURNO = LinqQuery.D_TURNO;
                            objPaseCFS.ID_PASE = double.Parse(LinqQuery.ID_PASE.Value.ToString());
                            objPaseCFS.ESTADO = LinqQuery.ESTADO;
                            objPaseCFS.ENVIADO = LinqQuery.ENVIADO;
                            objPaseCFS.AUTORIZADO = LinqQuery.AUTORIZADO;
                            objPaseCFS.VENTANILLA = LinqQuery.VENTANILLA;
                            objPaseCFS.USUARIO_ING = LinqQuery.USUARIO_ING;
                            objPaseCFS.FECHA_ING = System.DateTime.Now.Date;
                            objPaseCFS.USUARIO_MOD = LinqQuery.USUARIO_MOD;
                            objPaseCFS.ESTADO_PAGO = LinqQuery.ESTADO_PAGO;

                            objPaseCFS.ID_PPWEB = LinqQuery.ID_PPWEB;
                            objPaseCFS.ID_UNIDAD = LinqQuery.ID_UNIDAD;

                            if (LinqQuery.CNTR_DD.Value)
                            {
                                objPaseCFS.TIPO_CNTR = string.Format("{0} - {1}", LinqQuery.TIPO_CNTR, "Desaduanamiento Directo");
                            }

                            //detalle de pases
                            objPaseCFS.Detalle.Clear();

                            //detalle de subitems
                            objPaseCFS.DetalleSubItem.Clear();

                            //consulta detalle de subitems
                            var Tarja = PasePuerta.Pase_WebCFS.ObtenerTarjaCFS(objPaseCFS.MRN, objPaseCFS.MSN, objPaseCFS.HSN, objPaseCFS.GKEY.Value);
                            if (Tarja.Exitoso)
                            {
                                var LinqTarja = (from Tbl in Tarja.Resultado.Where(Tbl => Tbl.CONSECUTIVO != 0)
                                                 select new
                                                 {
                                                     CONSECUTIVO = Tbl.CONSECUTIVO,
                                                     CARGA = objPaseCFS.CARGA,
                                                     CANTIDAD = Tbl.CANTIDAD,
                                                     MRN = LinqQuery.MRN,
                                                     MSN = LinqQuery.MSN,
                                                     HSN = LinqQuery.HSN,
                                                     P2D_ALTO = Tbl.P2D_ALTO == null ? 0 : Tbl.P2D_ALTO.Value,
                                                     P2D_ANCHO = Tbl.P2D_ANCHO == null ? 0 : Tbl.P2D_ANCHO.Value,
                                                     P2D_LARGO = Tbl.P2D_LARGO == null ? 0 : Tbl.P2D_LARGO.Value,
                                                     PESO = Tbl.PESO == null ? 0 : Tbl.PESO.Value,
                                                     P2D_VOLUMEN = Tbl.P2D_VOLUMEN == null ? 0 : Tbl.P2D_VOLUMEN.Value,
                                                     IMO = string.IsNullOrEmpty(Tbl.IMO) ? "NO APLICA" : Tbl.IMO.Trim()
                                                 }).ToList().OrderBy(x => x.CONSECUTIVO);

                                List<Int64> Lista = new List<Int64>();

                                foreach (var Det in LinqTarja)
                                {

                                    objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
                                    objPaseCFSTarja.CARGA = Det.CARGA;
                                    objPaseCFSTarja.MRN = Det.MRN;
                                    objPaseCFSTarja.MSN = Det.MSN;
                                    objPaseCFSTarja.HSN = Det.HSN;
                                    objPaseCFSTarja.CONSECUTIVO = Det.CONSECUTIVO;
                                    objPaseCFSTarja.CANTIDAD = Det.CANTIDAD.Value;
                                    objPaseCFSTarja.CIATRANS = string.Empty;
                                    objPaseCFSTarja.CHOFER = string.Empty;
                                    objPaseCFSTarja.ID_CIATRANS = string.Empty;
                                    objPaseCFSTarja.ID_CHOFER = string.Empty;
                                    objPaseCFSTarja.PLACA = string.Empty;
                                    objPaseCFSTarja.VISTO = false;
                                    objPaseCFSTarja.TRANSPORTISTA_DESC = string.Empty;
                                    objPaseCFSTarja.CHOFER_DESC = string.Empty;
                                    objPaseCFSTarja.ESTADO_PAGO = objPaseCFS.ESTADO_PAGO;
                                    objPaseCFSTarja.MARCADO_SUBITEMS = string.Empty;

                                    objPaseCFSTarja.P2D_ALTO = Det.P2D_ALTO;
                                    objPaseCFSTarja.P2D_ANCHO = Det.P2D_ANCHO;
                                    objPaseCFSTarja.P2D_LARGO = Det.P2D_LARGO;
                                    objPaseCFSTarja.PESO = Det.PESO;
                                    objPaseCFSTarja.P2D_VOLUMEN = Det.P2D_VOLUMEN;
                                    objPaseCFSTarja.IMO = Det.IMO;

                                    Lista.Add(Det.CONSECUTIVO.Value);

                                    objPaseCFS.DetalleSubItem.Add(objPaseCFSTarja);


                                }

                                //valida si tiene ubicacion la carga       
                                var LinqUbicacion = Pase_WebCFS.Verficar_Ubicacion(ClsUsuario.loginname, Lista);
                                if (!LinqUbicacion.Exitoso)
                                {
                                    MensajesErrores = string.Format("Se presentaron los siguientes problemas: La carga no presenta ubicación..{0}", LinqUbicacion.MensajeProblema);

                                    this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Emisión Pase CFS", "La carga no presenta ubicación..", MensajesErrores.Trim(), objPaseCFS.CARGA,
                                        objPaseCFS.FACTURADO_DESC, objPaseCFS.AGENTE_DESC, out MensajeCasos, false);

                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", MensajeCasos));
                                    return;
                                }
                                else
                                {
                                    objPaseCFS.UBICACION =string.IsNullOrEmpty(LinqUbicacion.Resultado) ? "" : LinqUbicacion.Resultado;
                                }

                            }
                            else
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe ubicación para la carga ingresada..{0}..{1}", objPaseCFS.CARGA, Tarja.MensajeProblema));
                                return;
                            }

                            var TotalBultos = objPaseCFS.DetalleSubItem.Sum(x => x.CANTIDAD);
                            this.LabelTotal.InnerText = string.Format("DETALLE DE SUB. ÍTEMS - TOTAL BULTOS: {0}", TotalBultos);

                            tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem;
                            tablePagination_Tarja.DataBind();


                            objPaseCFS.EXPRESS = false;

                            //Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;


                            /*consulto si tiene proforma emitida*/
                            List<Cls_Bil_Configuraciones> ListP2D = Cls_Bil_Configuraciones.Get_Validacion("P2D_PASE", out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en configuraciones.....{0}</b>", cMensajes));
                                return;

                            }

                            bool Valida_P2D = false;

                            if (ListP2D.Count != 0)
                            {
                                Valida_P2D = true;
                            }

                            //si tiene activo para el servicio de PORT TO DOOR
                            if (Valida_P2D )
                            //if (Valida_P2D && (ClsUsuario.loginname.Trim().Equals("TER_INCALINES") || ClsUsuario.loginname.Trim().Equals("TER_COKA") || ClsUsuario.loginname.Trim().Equals("TER_UNI")
                            //    || ClsUsuario.loginname.Trim().Equals("TER_SACO") || ClsUsuario.loginname.ToUpper().Trim().Equals("TERMINAL")))
                            {

                                //valido que tenga una cotizacion activa
                                objValida = new P2D_Valida_Proforma();
                                objValida.MRN = this.TXTMRN.Text.Trim();
                                objValida.MSN = this.TXTMSN.Text.Trim();
                                objValida.HSN = this.TXTHSN.Text.Trim();
                                objValida.RUC = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();
                                if (objValida.PopulateMyData(out cMensajes))
                                {


                                    /*verificar si tiene servicio de carbono neutro*/
                                    XDocument XMLContenedores = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                           new XElement("CONTENEDORES",
                                                                        new XElement("CONTENEDOR",
                                                                new XAttribute("gkey", LinqQuery.ID_UNIDAD == null ? "0" : LinqQuery.ID_UNIDAD.ToString()),
                                                                new XAttribute("contenedor", LinqQuery.CONTENEDOR == null ? "" : LinqQuery.CONTENEDOR.Trim())
                                                                  )));


                                    //valida si tiene ya cargado el servicio de transporte, ya no muestra el mensaje de que tiene cotizaciones.
                                    List<P2D_Valida_Proforma> ListTransp = P2D_Valida_Proforma.Validacion_Servicio_Transporte_Cfs(XMLContenedores.ToString(), out cMensajes);
                                    if (!String.IsNullOrEmpty(cMensajes))
                                    {

                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener datos de servicio de transporte....{0}</b>", cMensajes));
                                        return;
                                    }
                                    /*listado de unidades con o sin el servicio*/
                                    var LinqTransp = (from TblFact in ListTransp.Where(TblFact => TblFact.gkey != 0 && TblFact.servicio == 1)
                                                      select new
                                                      {
                                                          gkey = TblFact.gkey,
                                                          contenedor = TblFact.contenedor,
                                                          servicio = TblFact.servicio
                                                      }).Distinct();

                                    if (LinqTransp.Count() != 0)
                                    {
                                        this.acepta_servicio.Visible = true;
                                        this.texto_servicio.Visible = true;
                                        this.texto_servicio.InnerHtml = TextoLeyenda;
                                        this.ChkTodos.Checked = true;
                                        this.UPTODOS.Update();

                                        if (objValida.ID_CIUDAD.HasValue)
                                        {
                                            this.CboCiudad.SelectedValue = objValida.ID_CIUDAD.ToString();
                                        }

                                        if (objValida.ID_ZONA.HasValue)
                                        {
                                            this.CboZonas.SelectedValue = objValida.ID_ZONA.ToString();
                                        }

                                        if (objValida.LATITUD.HasValue)
                                        {
                                            this.TxtLat.Text = objValida.LATITUD.ToString();
                                        }
                                        if (objValida.LONGITUD.HasValue)
                                        {
                                            this.TxtLon.Text = objValida.LONGITUD.ToString();
                                        }

                                        objPaseCFS.EXPRESS = objValida.EXPRESS;

                                        if (objValida.EXPRESS)
                                        {
                                            this.LblExpress.Visible = true;
                                        }

                                        if (objValida.APILABLE)
                                        {
                                            this.LblApilable.Visible = true;
                                            this.Label3.InnerText = "CARGA APILABLE";
                                        }
                                        else
                                        {
                                            this.LblApilable.Visible = true;
                                            this.Label3.InnerText = "CARGA NO APILABLE";
                                        }

                                        this.Txtdireccion.Text = objValida.DIRECCION;

                                        this.UPACEPTA.Update();

                                        //DATOS DEL TRANSPORTISTA POR DEFECTO
                                        //parametros de LIFTIF
                                        Configuraciones.ModuloAccesorios Cfgs = new Configuraciones.ModuloAccesorios("LIFTIF");
                                        Cfgs.ConfiguracionBase = "DATACON";
                                        string pv = string.Empty;
                                        if (!Cfgs.Inicializar(out pv))
                                        {
                                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener configuraciones LIFTIF....{0}</b>", pv));
                                            return;
                                        }

                                        var ptransporte = Cfgs.ObtenerConfiguracion("TRANSPORTE");

                                        string empresa_transporte = ptransporte == null ? string.Empty : ptransporte.valor;

                                        if (!string.IsNullOrEmpty(empresa_transporte))
                                        {

                                            if (empresa_transporte.Split('-').ToList().Count > 1)
                                            {

                                                string IdEmpresa = empresa_transporte.Split('-').ToList()[0].Trim();

                                                string DesEmpresa = (empresa_transporte.Split('-').ToList().Count >= 3 ? string.Format("{0} - {1}", empresa_transporte.Split('-').ToList()[1].Trim(), empresa_transporte.Split('-').ToList()[2].Trim()) : empresa_transporte.Split('-').ToList()[1].Trim());


                                                this.IdTxtempresa.Value = IdEmpresa;

                                                //marcar cargas y agregar datos de la empresa de transporte.
                                                objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                                                if (objPaseCFS != null)
                                                {
                                                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI"))
                                                                                       select p.CONSECUTIVO).ToList();

                                                    if (LinqValidaSubItemsFaltantes.Count > 0)
                                                    {
                                                        //proceso de marcar subitems
                                                        foreach (var Det in objPaseCFS.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI")))
                                                        {
                                                            ConsecutivoSelec = Det.CONSECUTIVO.Value;
                                                            var Detalle = objPaseCFS.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
                                                            if (Detalle != null)
                                                            {
                                                                Detalle.VISTO = true;
                                                                Detalle.CIATRANS = empresa_transporte;
                                                                Detalle.ID_CIATRANS = IdEmpresa;

                                                            }
                                                        }

                                                        tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem.Where(p => string.IsNullOrEmpty(p.MARCADO_SUBITEMS));
                                                        tablePagination_Tarja.DataBind();

                                                    }
                                                }

                                                Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                                                this.Pintar_Grilla();





                                            }
                                            else
                                            {

                                                this.IdTxtempresa.Value = string.Empty;
                                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información LIFTIF</b>"));
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.texto_servicio.InnerHtml = string.Empty;
                                        this.acepta_servicio.Visible = false;
                                        this.texto_servicio.Visible = false;
                                        this.Txtdireccion.Text = string.Empty;
                                        this.UPACEPTA.Update();
                                    }
                                }
                                else
                                {
                                    //validar si tiene el servicio facturado, para confirmar es tiene el p2d activo
                                    /*verificar si tiene servicio de carbono neutro*/
                                    XDocument XMLContenedores = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                           new XElement("CONTENEDORES",
                                                                        new XElement("CONTENEDOR",
                                                                new XAttribute("gkey", LinqQuery.ID_UNIDAD == null ? "0" : LinqQuery.ID_UNIDAD.ToString()),
                                                                new XAttribute("contenedor", LinqQuery.CONTENEDOR == null ? "" : LinqQuery.CONTENEDOR.Trim())
                                                                  )));


                                    //valida si tiene ya cargado el servicio de transporte, ya no muestra el mensaje de que tiene cotizaciones.
                                    List<P2D_Valida_Proforma> ListTransp = P2D_Valida_Proforma.Validacion_Servicio_Transporte_Cfs(XMLContenedores.ToString(), out cMensajes);
                                    if (!String.IsNullOrEmpty(cMensajes))
                                    {

                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener datos de servicio de transporte....{0}</b>", cMensajes));
                                        return;
                                    }
                                    /*listado de unidades con o sin el servicio*/
                                    var LinqTransp = (from TblFact in ListTransp.Where(TblFact => TblFact.gkey != 0 && TblFact.servicio == 1)
                                                      select new
                                                      {
                                                          gkey = TblFact.gkey,
                                                          contenedor = TblFact.contenedor,
                                                          servicio = TblFact.servicio
                                                      }).Distinct();

                                    if (LinqTransp.Count() != 0)
                                    {
                                        this.acepta_servicio.Visible = true;
                                        this.texto_servicio.Visible = true;
                                        this.texto_servicio.InnerHtml = TextoLeyenda;
                                        this.ChkTodos.Checked = true;
                                        this.UPTODOS.Update();

                                        if (objValida.ID_CIUDAD.HasValue)
                                        {
                                            this.CboCiudad.SelectedValue = objValida.ID_CIUDAD.ToString();
                                        }

                                        if (objValida.ID_ZONA.HasValue)
                                        {
                                            this.CboZonas.SelectedValue = objValida.ID_ZONA.ToString();
                                        }

                                        this.Txtdireccion.Text = objValida.DIRECCION;

                                        this.UPACEPTA.Update();

                                        //DATOS DEL TRANSPORTISTA POR DEFECTO
                                        //parametros de LIFTIF
                                        Configuraciones.ModuloAccesorios Cfgs = new Configuraciones.ModuloAccesorios("LIFTIF");
                                        Cfgs.ConfiguracionBase = "DATACON";
                                        string pv = string.Empty;
                                        if (!Cfgs.Inicializar(out pv))
                                        {
                                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener configuraciones LIFTIF....{0}</b>", pv));
                                            return;
                                        }

                                        var ptransporte = Cfgs.ObtenerConfiguracion("TRANSPORTE");

                                        string empresa_transporte = ptransporte == null ? string.Empty : ptransporte.valor;

                                        if (!string.IsNullOrEmpty(empresa_transporte))
                                        {

                                            if (empresa_transporte.Split('-').ToList().Count > 1)
                                            {

                                                string IdEmpresa = empresa_transporte.Split('-').ToList()[0].Trim();

                                                string DesEmpresa = (empresa_transporte.Split('-').ToList().Count >= 3 ? string.Format("{0} - {1}", empresa_transporte.Split('-').ToList()[1].Trim(), empresa_transporte.Split('-').ToList()[2].Trim()) : empresa_transporte.Split('-').ToList()[1].Trim());


                                                this.IdTxtempresa.Value = IdEmpresa;

                                                //marcar cargas y agregar datos de la empresa de transporte.
                                                objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                                                if (objPaseCFS != null)
                                                {
                                                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI"))
                                                                                       select p.CONSECUTIVO).ToList();

                                                    if (LinqValidaSubItemsFaltantes.Count > 0)
                                                    {
                                                        //proceso de marcar subitems
                                                        foreach (var Det in objPaseCFS.DetalleSubItem.Where(x => string.IsNullOrEmpty(x.MARCADO_SUBITEMS) && x.ESTADO_PAGO.Equals("SI")))
                                                        {
                                                            ConsecutivoSelec = Det.CONSECUTIVO.Value;
                                                            var Detalle = objPaseCFS.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
                                                            if (Detalle != null)
                                                            {
                                                                Detalle.VISTO = true;
                                                                Detalle.CIATRANS = empresa_transporte;
                                                                Detalle.ID_CIATRANS = IdEmpresa;

                                                            }
                                                        }

                                                        tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem.Where(p => string.IsNullOrEmpty(p.MARCADO_SUBITEMS));
                                                        tablePagination_Tarja.DataBind();

                                                    }
                                                }

                                                Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                                                this.Pintar_Grilla();





                                            }
                                            else
                                            {

                                                this.IdTxtempresa.Value = string.Empty;
                                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información LIFTIF</b>"));
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.texto_servicio.InnerHtml = string.Empty;
                                        this.acepta_servicio.Visible = false;
                                        this.texto_servicio.Visible = false;
                                        this.Txtdireccion.Text = string.Empty;
                                        this.UPACEPTA.Update();
                                    }



                                }
                            }


                            Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                        }
                        else
                        {
                            tablePagination_Tarja.DataSource = null;
                            tablePagination_Tarja.DataBind();
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", Carga.MensajeProblema));
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
                    objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar los pase a puerta de carga suelta. </b>"));
                        return;
                    }

                    if (objPaseCFS.DetalleSubItem.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, poder generar los pase a puerta de carga suelta. </b>"));
                        return;
                    }

                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == false || string.IsNullOrEmpty(x.MARCADO_SUBITEMS))
                                                       select p.CONSECUTIVO).ToList();

                    if (LinqValidaSubItemsFaltantes.Count > 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Existen bultos o subitems sin seleccionar, debe completar toda la partida para poder generar los pase a puerta de carga suelta.</b>"));
                        return;
                    }

                    if (objPaseCFS.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen pases de puertas de carga suelta pendientes para generar. </b>"));
                        return;
                    }

                    LoginName = objPaseCFS.IV_USUARIO_CREA.Trim();

                    //pase sin turno.
                    //verificar si es pase sin turno
                    var PaseSinturno = Pase_CFS.EsPaseSinTurno(LoginName, objPaseCFS.MRN, objPaseCFS.MSN, objPaseCFS.HSN);
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
                     
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", objPaseCFS.CARGA, PaseSinturno.MensajeProblema));
                        return;
                    }

                    //pase con turno
                    if (!EsPasesinTurno)
                    {
                        /***********************************************************************************************************************************************
                        *valida que tenga un turno ingresado
                        **********************************************************************************************************************************************/
                        foreach (var Det in objPaseCFS.Detalle)
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
                    }


                    
                    
                    /*contenedores seleccionados para emitir pase*/
                    var LinqQuery = (from Tbl in objPaseCFS.Detalle.Where(Tbl => Tbl.CANTIDAD != 0)
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
                                         ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD,
                                         UBICACION = objPaseCFS.UBICACION,


                                     }).ToList().OrderBy(x => x.CONTENEDOR);


                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/

                    string token = string.Empty;
                    int hub_id = 0;
                    string url = string.Empty;
                    string contact = string.Empty;
                    string name = string.Empty;
                    string address = string.Empty;
                    string phone = string.Empty;
                    string identification_number = string.Empty;
                    string email = string.Empty;
                    string extra_address = string.Empty;
                    string internal_id = string.Empty;
                    string order_number = string.Empty;
                    decimal latitude = 0;
                    decimal longitude = 0;
                    decimal unit_weight = 0;
                    decimal unit_volume = 0;
                    bool apilable = false;
                    bool Express = false;
                    /*verifica si tiene el servicio de P2D*/
                    if (this.acepta_servicio.Visible)
                    {
                        //parametros de LIFTIF
                        Configuraciones.ModuloAccesorios Cfgs = new Configuraciones.ModuloAccesorios("LIFTIF");
                        Cfgs.ConfiguracionBase = "DATACON";
                        string pv = string.Empty;
                        if (!Cfgs.Inicializar(out pv))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener configuraciones LIFTIT....{0}</b>", pv));
                            return;
                        }

                        var ptoken = Cfgs.ObtenerConfiguracion("TOKEN");
                        var phub_id = Cfgs.ObtenerConfiguracion("hub_id");
                        var purl = Cfgs.ObtenerConfiguracion("CREAR");

                        token = ptoken == null ? string.Empty : ptoken.valor;
                        hub_id = phub_id == null ? 2443 : int.Parse(phub_id.valor);
                        url = purl == null ? string.Empty : purl.valor;

                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        //consultar informacion del cliente
                        var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, objPaseCFS.FACTURADO);
                        if (Cliente.Exitoso)
                        {
                            var ListaCliente = Cliente.Resultado;
                            if (ListaCliente != null)
                            {
                                contact = string.Format("{0}", ListaCliente.CLNT_NAME.Trim());
                                name = string.Format("{0}", ListaCliente.CLNT_NAME.Trim());
                                address = string.IsNullOrEmpty(this.Txtdireccion.Text) ? ListaCliente.CLNT_ADRESS : this.Txtdireccion.Text.Trim();
                                phone = "99999999";
                                identification_number = objPaseCFS.FACTURADO;
                                //email = string.IsNullOrEmpty(ListaCliente.CLNT_EMAIL) ? (string.IsNullOrEmpty(ClsUsuario.email) ? "mail@mail.com" : ClsUsuario.email)  : ListaCliente.CLNT_EMAIL;
                                

                                string full_mail = string.IsNullOrEmpty(ListaCliente.CLNT_FAX_INVC) ? "mail@mail.com" : ListaCliente.CLNT_FAX_INVC.Replace(",",";").Trim();
                                if (full_mail.Split(';').ToList().Count > 1)
                                {
                                    email = full_mail.Split(';').ToList()[0].Trim();
                                }
                                else
                                {
                                    email = string.IsNullOrEmpty(ListaCliente.CLNT_FAX_INVC) ? "mail@mail.com" : ListaCliente.CLNT_FAX_INVC.Trim();
                                }

                                extra_address = string.IsNullOrEmpty(this.Txtdireccion.Text) ? ListaCliente.CLNT_ADRESS : this.Txtdireccion.Text.Trim();
                                internal_id = objPaseCFS.FACTURADO;

                            }
                            else
                            {
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente</b>"));
                                return;
                            }
                        }

                        //consulta datos del volumen y peso.
                        //valido que tenga una cotizacion activa
                        objValida = new P2D_Valida_Proforma();
                        objValida.MRN = this.TXTMRN.Text.Trim();
                        objValida.MSN = this.TXTMSN.Text.Trim();
                        objValida.HSN = this.TXTHSN.Text.Trim();
                        objValida.RUC = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();
                        if (objValida.PopulateMyData(out cMensajes))
                        {
                             unit_weight = objValida.TOTAL_TN;
                             unit_volume = objValida.TOTAL_M3;
                             apilable = objValida.APILABLE;
                             Express = objValida.EXPRESS;
                        }

                    }



                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {

                        P2D_Pase_CFS pase = new P2D_Pase_CFS();
                        pase.ID_CARGA = Det.GKEY;
                        pase.ESTADO = "GN";
                        pase.FECHA_EXPIRACION = Det.FECHA_SALIDA_PASE;
                        
                        pase.ID_PLACA = Det.PLACA;
                        pase.ID_CHOFER = Det.ID_CHOFER;
                        pase.ID_EMPRESA = Det.ID_CIATRANS;
                        pase.CANTIDAD_CARGA = Det.CANTIDAD;
                        pase.USUARIO_REGISTRO = Det.USUARIO;
                        pase.TIPO_CARGA = "CFS";
                     
                        pase.CONSIGNATARIO_ID = Det.IMPORTADOR;
                        pase.CONSIGNARIO_NOMBRE = Det.IMPORTADOR_DESC;
                        pase.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                        pase.CHOFER_DESC = Det.CHOFER_DESC;
                        pase.PPW = Det.ID_PPWEB;
                        pase.ID_UNIDAD = Det.ID_UNIDAD;

                        //pase con servicio p2d
                        if (this.acepta_servicio.Visible)
                        {
                            pase.ID_CIUDAD = Int64.Parse(this.CboCiudad.SelectedValue);
                            pase.ID_ZONA = Int64.Parse(this.CboZonas.SelectedValue);
                            pase.DIRECCION = this.Txtdireccion.Text.Trim();
                            pase.P2D = true;
                            pase.ENVIADO_LIFTIF = false;

                            if (!decimal.TryParse(this.TxtLatitud.Text, out latitude))
                            {
                                latitude = 0;
                            }
                            pase.LATITUD = latitude;

                            if (!decimal.TryParse(this.TxtLongitud.Text, out longitude))
                            {
                                longitude = 0;
                            }

                            pase.LONGITUD = longitude;

                            pase.CONTACTO = contact;
                            pase.CLIENTE = name;
                            pase.DIRECCION_CLIENTE = address;
                            pase.TELEFONOS = phone;
                            pase.EMAIL = email;
                            pase.ID_CLIENTE = internal_id;
                            pase.PRODUCTO = string.IsNullOrEmpty(Det.MARCA) ? "CARGA SUELTA CONTECON" : Det.MARCA.Trim(); ;
                            pase.PESO = unit_weight;
                            pase.VOLUMEN = unit_volume;


                        }
                        else
                        {
                            pase.ID_CIUDAD = null;
                            pase.ID_ZONA = null;
                            pase.DIRECCION = null;
                            pase.P2D = false;
                            pase.ENVIADO_LIFTIF = false;
                            pase.LATITUD = null;
                            pase.LONGITUD = null;
                            pase.CONTACTO = null;
                            pase.CLIENTE = null;
                            pase.DIRECCION_CLIENTE = null;
                            pase.TELEFONOS = null;
                            pase.EMAIL = null;
                            pase.ID_CLIENTE = null;
                            pase.PRODUCTO = null;
                            pase.PESO = null;
                            pase.VOLUMEN = null;
                        }

                        var Resultado = pase.Insertar(Det.TURNO.Value, Det.CONTENEDOR, Det.SUB_SECUENCIA,Det.BULTOS_HORARIOS.Value,Det.MRN, Det.MSN, Det.HSN, Det.CNTR_DD);
                        if (Resultado.Exitoso)
                        {
                            nTotal++;
                            if (nTotal == 1)
                            {
                                id_carga = securetext(Det.CARGA);
                            }

                            //si tiene el servicio de p2d
                            //transmitir pase liftif
                            if (this.acepta_servicio.Visible)
                            {
                                order_number = pase.ID_PASE.ToString().Trim();

                                if (!decimal.TryParse(this.TxtLatitud.Text, out latitude))
                                {
                                    latitude = 0;
                                }

                                if (!decimal.TryParse(this.TxtLongitud.Text, out longitude))
                                {
                                    longitude = 0;
                                }

                                var Orden = new order();
                                //cabecera de la carga
                                Orden.hub_id = hub_id;
                                Orden.initial_delivery_time = pase.FECHA_EXPIRACION.Value;
                                Orden.finish_delivery_time = pase.FECHA_EXPIRACION.Value;
                                Orden.start_time = (Det.TURNO_DESDE.HasValue ? Det.TURNO_DESDE.Value.ToString("HH:mm") : System.DateTime.Now.ToString("HH:mm"));
                                Orden.end_time = (Det.TURNO_HASTA.HasValue ? Det.TURNO_HASTA.Value.ToString("HH:mm") : System.DateTime.Now.AddHours(2).ToString("HH:mm")); 
                                Orden.latitude = latitude.ToString("N7");
                                Orden.longitude = longitude.ToString("N7");
                                Orden.neighborhood = "";
                                Orden.city =  this.CboCiudad.SelectedItem.Text.ToString(); 
                                Orden.state =  this.CboCiudad.SelectedItem.Text.ToString(); 
                                Orden.order_number = order_number;
                                Orden.zone = this.CboZonas.SelectedItem.Text.ToString();
                                Orden.observations = string.Format("MRN: {0} , DAI: {1}  ,  DELIVERY: {2}, {3}, TIPO SERVICIO: {4}", Det.CARGA, Det.DOCUMENTO, string.IsNullOrEmpty(pase.NUMERO_PASE_N4) ? "" : pase.NUMERO_PASE_N4, (apilable==true ? "NO APILABLE" : "APILABLE"), (Express == true ? "EXPRESS":"NORMAL" ));

                                //datos del cliente
                                var Customer = new customer();
                                Customer.contact = contact;
                                Customer.name = name;
                                Customer.address = address;
                                Customer.phone = phone;
                                Customer.identification_number = identification_number;
                                Customer.email = email;
                                Customer.extra_address = extra_address;
                                Customer.internal_id = internal_id;
                                Orden.customer = Customer;

                                bool TIENE_IMO = false;
                                string TIPO_IMO = string.Empty;
                                int ii = 1;

                                //detalle de la carga
                                foreach (var Sub in objPaseCFS.DetalleSubItem)
                                {

                                    if (!Sub.IMO.Equals("NO APLICA"))
                                    {
                                        if (ii == 1)
                                        {
                                            TIPO_IMO = Sub.IMO;
                                            TIENE_IMO = true;
                                            ii++;
                                        }
                                        
                                    }

                                    var Items = new items();
                                    Items.description = string.Format("UBICACION: {0} , Medidas: {1}, Descripción: {2}", Det.UBICACION , string.Format("{0} x {1} x {2}", Sub.P2D_LARGO, Sub.P2D_LARGO, Sub.P2D_ANCHO), string.IsNullOrEmpty(Det.MARCA) ? "CARGA SUELTA CONTECON" : Det.MARCA.Trim());
                                    Items.sku = string.Format("{0} x {1} x {2}",Sub.P2D_LARGO, Sub.P2D_LARGO, Sub.P2D_ANCHO);
                                    Items.barcode = string.Format("{0}", (!Sub.IMO.Equals("NO APLICA") ? string.Format("IMO: {0}",Sub.IMO) : ""));
                                    Items.quantity = Sub.CANTIDAD;
                                    Items.unit_weight = Sub.PESO.Value;
                                    Items.unit_volume = Sub.P2D_VOLUMEN.Value;
                                    Items.unit_price = 0;
                                    Items.total_price = 0;
                                    Orden.items.Add(Items);
                                   
                                }
                                /*
                                 var Items = new items();
                                    Items.description = string.IsNullOrEmpty(Det.MARCA) ? "CARGA SUELTA CONTECON" : Det.MARCA.Trim();
                                    Items.sku = string.Format("{0} x {1} x {2}",Sub.P2D_LARGO, Sub.P2D_LARGO, Sub.P2D_ANCHO);
                                    Items.barcode = "";
                                    Items.quantity = Det.BULTOS_HORARIOS.Value;
                                    Items.unit_weight = Math.Round((unit_weight / (Det.BULTOS_HORARIOS.HasValue ? Det.BULTOS_HORARIOS.Value : 1)), 5);
                                    Items.unit_volume = unit_volume;
                                    Items.unit_price = 0;
                                    Items.total_price = 0;
                                    Orden.items.Add(Items);
                                 */


                                //datos del imo
                                if (TIENE_IMO)
                                {
                                    var Meta = new meta();
                                    Meta.imo_clasification = TIPO_IMO;
                                    Orden.meta = Meta;

                                }

                                var Ord = new create_order(token, url);
                                Ord.order = Orden;


                                /*
                                    grabar el json enviando
                                */
                                var json = Ord.json();
                                //graba log del servicio
                                objLogLiftif = new P2D_Traza_Liftif();
                                objLogLiftif.USUARIO = Det.USUARIO;
                                objLogLiftif.TOKEN = token;
                                objLogLiftif.ORIGEN = "emisionpasecfs:BtnGrabar_Click";
                                objLogLiftif.MENSAJE = json;
                                objLogLiftif.ESTADO = "ENVIADO";
                                objLogLiftif.PASE = Int64.Parse(pase.ID_PASE.ToString().Trim());

                                string xerror;
                                var Enviado = objLogLiftif.SaveTransaction(out xerror);
                                /*fin de nuevo proceso de grabado*/
                                if (!Enviado.HasValue || Enviado.Value <= 0)
                                {

                                }


                                /*enviando a liftit*/
                                var Result = Ord.WriteApiOrder();
                                if (Result.code.Equals("ERROR"))
                                {

                                    //graba log del servicio
                                    objLogLiftif = new P2D_Traza_Liftif();
                                    objLogLiftif.USUARIO = Det.USUARIO;
                                    objLogLiftif.TOKEN = token;
                                    objLogLiftif.ORIGEN = "emisionpasecfs:BtnGrabar_Click";
                                    objLogLiftif.MENSAJE = Result.message;
                                    objLogLiftif.ESTADO = Result.code;
                                    objLogLiftif.PASE = Int64.Parse(pase.ID_PASE.ToString().Trim());

                                   
                                    var nProcesoApp = objLogLiftif.SaveTransaction(out xerror);
                                    /*fin de nuevo proceso de grabado*/
                                    if (!nProcesoApp.HasValue || nProcesoApp.Value <= 0)
                                    {

                                    }

                                }
                                else
                                {
                                    //graba log del servicio
                                    objLogLiftif = new P2D_Traza_Liftif();
                                    objLogLiftif.USUARIO = Det.USUARIO;
                                    objLogLiftif.TOKEN = token;
                                    objLogLiftif.ORIGEN = "emisionpasecfs:BtnGrabar_Click";
                                    objLogLiftif.MENSAJE = Result.message;
                                    objLogLiftif.ESTADO = Result.code;
                                    objLogLiftif.PASE = Int64.Parse(pase.ID_PASE.ToString().Trim());

                                    
                                    var nProcesoApp = objLogLiftif.SaveTransaction(out xerror);
                                    /*fin de nuevo proceso de grabado*/
                                    if (!nProcesoApp.HasValue || nProcesoApp.Value <= 0)
                                    {

                                    }

                                    //actualiza informacion del pase de puerta con los resultados de liftif
                                    objActualiza_Pase = new P2D_Actualiza_PasePuerta();
                                    objActualiza_Pase.ID_PASE = Int64.Parse(pase.ID_PASE.ToString().Trim());
                                    objActualiza_Pase.ORDER_ID = Result.id;
                                    objActualiza_Pase.TRACKING_NUMBER = Result.tracking_number;

                                    var nIdRegistro = objActualiza_Pase.SaveTransaction_Update_cfs(out cMensajes);
                                    if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                                    {

                                    }
                                   
                                }
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
                        string link = string.Format("<a href='../pasepuertacfs/imprimirpasecfs.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta</a>", id_carga);

                        //limpiar
                        objPaseCFS.Detalle.Clear();
                        objPaseCFS.DetalleSubItem.Clear();

                        Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                        this.acepta_servicio.Visible = false;
                        this.UPACEPTA.Update();

                        this.tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem;
                        tablePagination_Tarja.DataBind();
                        tablePagination.DataSource = objPaseCFS.Detalle;
                        tablePagination.DataBind();

                        this.Limpia_Campos();


                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar {0} pase de puerta con éxito, para proceder a imprimir los mismo, <br/>por favor dar click en el siguiente link: {1} </b>", nTotal, link));
                        return;
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Grabar Pase CFS", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }
           


        }

        #region "Eventos de la grilla de pases de puerta cfs"

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
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
                        objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;

                        //existe pase a remover
                        var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));
                        if (Detalle != null)
                        {
                            string Llave = Detalle.LLAVE;
                            //remover pase
                            objPaseCFS.Detalle.Remove(objPaseCFS.Detalle.Where(p => p.LLAVE == Llave).FirstOrDefault());

                            //recorrido de subitems
                            foreach (var Det in objPaseCFS.DetalleSubItem.Where(x => x.MARCADO_SUBITEMS.Equals(Llave)))
                            {
                                ConsecutivoSelec = Det.CONSECUTIVO.Value;
                                var DetalleSubItem = objPaseCFS.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
                                if (DetalleSubItem != null)
                                {
                                    DetalleSubItem.CIATRANS = string.Empty;
                                    DetalleSubItem.CHOFER = string.Empty;
                                    DetalleSubItem.PLACA = string.Empty;
                                    DetalleSubItem.ID_CIATRANS = string.Empty;
                                    DetalleSubItem.ID_CHOFER = string.Empty;
                                    DetalleSubItem.TRANSPORTISTA_DESC = string.Empty;
                                    DetalleSubItem.CHOFER_DESC = string.Empty;
                                    DetalleSubItem.MARCADO_SUBITEMS = string.Empty;
                                    DetalleSubItem.VISTO = false;
                                }
                            }

                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información temporal de los subitems: {0} </b>", t.ToString()));
                            return;
                        }

                        tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem.Where(p => p.VISTO == false && string.IsNullOrEmpty(p.MARCADO_SUBITEMS));
                        tablePagination_Tarja.DataBind();

                        tablePagination.DataSource = objPaseCFS.Detalle;
                        tablePagination.DataBind();

                        this.Pintar_Grilla();

                        Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;


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
                objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                var Detalle = objPaseCFS.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
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

                tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem.Where(p => string.IsNullOrEmpty(p.MARCADO_SUBITEMS));
                tablePagination_Tarja.DataBind();

                Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                this.Pintar_Grilla();



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
                Server.HtmlEncode(this.Txtdireccion.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    this.Carga_CboCiudad();
                    this.Carga_CboZonas();

                    TextoLeyenda = leyenda_servicio_p2d();

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}