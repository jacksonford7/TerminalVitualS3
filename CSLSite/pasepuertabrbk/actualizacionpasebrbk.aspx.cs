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
  

    public partial class actualizacionpasebrbk : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaBRBK_Cabecera objPaseBRBK = new Cls_Bil_PasePuertaBRBK_Cabecera();
        private Cls_Bil_PasePuertaBRBK_SubItems objPaseBRBKTarja = new Cls_Bil_PasePuertaBRBK_SubItems();
        private Cls_Bil_PasePuertaBRBK_Detalle objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();

        private Cls_Bil_PasePuertaBRBK_Temporal objReserva;
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }

        private Cls_Bil_Stock_Pases_CFS objCtock = new Cls_Bil_Stock_Pases_CFS();

    
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
        private string Fecha_Tope = string.Empty;
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
        private DateTime FechaTopeSalida;
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
                        Mensaje = string.Format("se envió una notificación a nuestro personal de Tesorería  para que realicen las respectivas revisiones del caso generado #  {0} ...Casilla de atención: tesoreia@cgsa.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del caso generado #  {0} ...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                }
                else
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de Tesorería  para que realicen las respectivas revisiones del problema {0} ...Casilla de atención: tesoreia@cgsa.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.", rt.MensajeProblema);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del problema {0} ....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00. ", rt.MensajeProblema);
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
         
            UPBOTONES.Update();
            UPCONTENEDOR.Update();
            UPFECHASALIDA.Update();
            UPTURNO.Update();

            UPCANTIDADRETIRAR.Update();
            UPRETIRADOS.Update();
            UPSALDO.Update();
            UPBODEGA.Update();
            UPPRODUCTO.Update();
            UPAGREGATURNO.Update();
            UPAGREGATURNO.Update();
            UPFACTURAHASTA.Update();
            UPPASE.Update();

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
            this.TxtRetirados.Text = string.Empty;
            this.TxtSaldo.Text = string.Empty;
            this.TxtBodega.Text = string.Empty;
            this.TxtTipoProducto.Text = string.Empty;

            this.TxtCantidadRetirar.Text = string.Empty;
            this.TxtPaseModifica.Text = string.Empty;
            this.TxtFacturadoHasta.Text = string.Empty;

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
            Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
        }

        private void Turno_Default()
        {
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Seleccione *" });
            //this.CboTurnos.Items.Clear();
            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();
        }

        private void Pase_Sin_Turno_Default()
        {
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Pase Sin turno *" });
            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();
        }

        private void Pintar_Grilla()
        {
            foreach (RepeaterItem xitem in tablePagination.Items)
            {
                CheckBox ChkVisto = xitem.FindControl("chkPase") as CheckBox;

                Label LblPase = xitem.FindControl("LblPase") as Label;
                Label LblCantidad = xitem.FindControl("LblCantidad") as Label;
                Label LblFechaSalida = xitem.FindControl("LblFechaSalida") as Label;
                Label LblTurno = xitem.FindControl("LblTurno") as Label;
                Label LblEmpresa = xitem.FindControl("LblEmpresa") as Label;
                Label LblChofer = xitem.FindControl("LblChofer") as Label;
                Label LblPlaca = xitem.FindControl("LblPlaca") as Label;
                Label LblFechaturno = xitem.FindControl("LblFechaturno") as Label;
                Label LblEstado = xitem.FindControl("LblEstado") as Label;
                Label LblMensaje = xitem.FindControl("LblMensaje") as Label;
                Label LblIdSolicitud = xitem.FindControl("LblIdSolicitud") as Label;
                if (ChkVisto.Checked == true || LblEstado.Text == "EXPIRADO" || !LblIdSolicitud.Text.Equals("0"))
                {

                    LblPase.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblCantidad.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblFechaSalida.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblTurno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEmpresa.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblChofer.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblPlaca.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblFechaturno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEstado.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblMensaje.ForeColor = System.Drawing.Color.PaleVioletRed;
                }

                

            }
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
                    

                    objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todos los pases a modificar... </b>"));
                        return;
                    }
                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de pases, para poder seleccionar... </b>"));
                        return;
                    }


                    //proceso de marcar subitems
                    foreach (var Det in objPaseBRBK.Detalle)
                    {
                        double ID_PASE = Det.ID_PASE.Value;

                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
                        if (Detalle != null)
                        {
                            Int64 ID_SOL = (Detalle.ID_SOL.HasValue ? Detalle.ID_SOL.Value : 0);

                            if (Detalle.ESTADO.Equals("EXPIRADO") || Detalle.ESTADO_TRANSACCION==false || ID_SOL != 0)
                            {
                            }
                            else
                            {
                                Detalle.VISTO = ChkEstado;
                            }
 
                        }
                    }


                    tablePagination.DataSource = objPaseBRBK.Detalle.OrderBy(p => p.ORDENAMIENTO);
                    tablePagination.DataBind();

                    Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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

                    string Pase = this.TxtPaseModifica.Text;

                    if (string.IsNullOrEmpty(Pase))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el pase de puerta a modificar el turno</b>"));
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFacturadoHasta.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el pase de puerta a modificar el turno</b>"));
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar la fecha del turno... </b>"));
                        return;
                    }
                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de pases, para poder seleccionar la fecha del turno... </b>"));
                        return;
                    }

                    HoraHasta = "00:00";
                    Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la actualización del pase.. Mes/Día/año</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    Fecha_Tope = string.Format("{0} {1}", this.TxtFacturadoHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha_Tope, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaTopeSalida))
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha del pase seleccionado, no es valida para la actualización del mismo.. Mes/Día/año</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }


                    FechaFacturaHasta = FechaTopeSalida;

                    if (FechaActualSalida.Date < System.DateTime.Now.Date)
                    {
                        this.Turno_Default();
                        //this.TxtFechaHasta.Text = objPaseBRBK.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
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


                    if (FechaActualSalida.Date == System.DateTime.Today.Date)
                    {
                        this.Turno_Default();
                        //this.TxtFechaHasta.Text = System.DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha del turno, no puede ser igual a la fecha actual</b>"));
                        this.TxtFechaHasta.Focus();
                    }

                    //if (FechaActualSalida.Date == FechaFacturaHasta.Date)
                    //{
                    //    this.Turno_Default();
                    //    this.TxtFechaHasta.Text = FechaFacturaHasta.AddDays(-1).ToString("MM/dd/yyyy");
                    //    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, debe ser un día antes que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), FechaFacturaHasta.ToString("MM/dd/yyyy")));
                    //    this.TxtFechaHasta.Focus();
                    //    return;
                    //}



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
                 
                    //Int64 IDDISPONIBLEDET = 0;
                    //string MRN = string.Empty;
                    //string MSN = string.Empty;
                    //string HSN = string.Empty;
                    //string BODEGA = string.Empty;

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

                    objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;

                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder agregar un turno... </b>"));
                        return;
                    }
                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de pases de puerta, para poder cambiar el turno... </b>"));
                        return;
                    }

                    string Pase = this.TxtPaseModifica.Text;

                    if (string.IsNullOrEmpty(Pase))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el pase de puerta a modificar el turno</b>"));
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFacturadoHasta.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el pase de puerta a modificar el turno</b>"));
                        return;
                    }


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

                    
                    HoraHasta = "00:00";
                    Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                    {
                        List_Turnos = new List<Cls_Bil_Turnos>();
                        List_Turnos.Clear();

                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la actualización del pase.. Mes/Día/año</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }


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

                                HoraHasta = "00:00";
                                Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                                if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                                {
                                    List_Turnos = new List<Cls_Bil_Turnos>();
                                    List_Turnos.Clear();
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


                    double PASE = 0;
                    if (!double.TryParse(Pase, out PASE))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "# pase null"));
                        return;
                    }

                    var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.ID_PASE == PASE);
                    if (Detalle != null)
                    {
                        Detalle.D_TURNO = this.CboTurnos.SelectedItem.ToString().Substring(0, 5);
                        Detalle.TURNO = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());
                        Detalle.ID_TURNO = Convert.ToInt16(TurnoSelect.Split('-').ToList()[1].Trim());
                        Detalle.TURNO_DESDE = FechaTurnoInicio;
                        Detalle.TURNO_HASTA = FechaTurnoFinal;
                        

                        HoraHasta = Detalle.D_TURNO;
                        Fecha = string.Format("{0} {1}", FechaActualSalida.Date.ToString("MM/dd/yyyy"), HoraHasta);

                        if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                        {
                            List_Turnos = new List<Cls_Bil_Turnos>();
                            List_Turnos.Clear();

                            this.Turno_Default();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para poder agregar la información del turno, Mes/Día/año </b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        Detalle.CAMBIO_TURNO = "SI";
                        Detalle.FECHA_SALIDA_PASE = FechaActualSalida;

                       
                    }

                       
                    tablePagination.DataSource = objPaseBRBK.Detalle.OrderBy(p => p.ORDENAMIENTO);
                    tablePagination.DataBind();

                    Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();

                    this.TxtCantidadRetirar.Text = string.Empty;
                    this.TxtPaseModifica.Text = string.Empty;
                    this.TxtFacturadoHasta.Text = string.Empty;
                    this.TxtFechaHasta.Text = string.Empty;
                    this.Turno_Default();

                    this.Actualiza_Paneles();


                    objReserva = new Cls_Bil_PasePuertaBRBK_Temporal();
                    objReserva.IDX_ROW = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());
                    objReserva.MRN = objPaseBRBK.MRN;
                    objReserva.MSN = objPaseBRBK.MSN;
                    objReserva.HSN = objPaseBRBK.HSN;
                    objReserva.ID_PASE = Int64.Parse(PASE.ToString());
                    objReserva.USUARIO_CREA = ClsUsuario.loginname;

                    var nProceso = objReserva.SaveTransaction_Update(out cMensajes);
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {

                        this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo grabar datos del turno.{0}</b>", cMensajes));
                        return;
                    }



                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAgregaTruno_Click), "BtnAgregaTruno_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                }
               
            }
        }
        #endregion


        #region "datos del transportista"

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
                   

                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar la Compañía de Transporte para poder agregar la información </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }

                    objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar el o los pases de puerta... </b>"));
                        return;
                    }
                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de pases de puerta, para poder actualizar los datos. </b>"));
                        return;
                    }

                    var LinqValidaPase = (from p in objPaseBRBK.Detalle.Where(x => x.VISTO == true)
                                          select p.ID_PASE).ToList();

                    if (LinqValidaPase.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los pase, para actualizar con la empresa de transporte: {0} </b>", Txtempresa.Text.Trim()));
                        return;
                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //valida que exista Empresa Transporte 
                    if (!string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        EmpresaSelect = this.Txtempresa.Text.Trim();
                        if (EmpresaSelect.Split('-').ToList().Count > 1)
                        {
                            //Int32 p = EmpresaSelect.Split('-').ToList().Count;
                            IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();
                            DesEmpresa = (EmpresaSelect.Split('-').ToList().Count >= 3 ? string.Format("{0} - {1}",EmpresaSelect.Split('-').ToList()[1].Trim(), EmpresaSelect.Split('-').ToList()[2].Trim()) : EmpresaSelect.Split('-').ToList()[1].Trim());
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


                    //actualizado datos de chofer, transportista
                    foreach (var Det in objPaseBRBK.Detalle.Where(x => x.VISTO == true))
                    {
                        double ID_PASE = Det.ID_PASE.Value;

                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
                        if (Detalle != null)
                        {
                            Detalle.CIATRANS = EmpresaSelect;
                            Detalle.CHOFER = ChoferSelect;
                            Detalle.PLACA = PlacaSelect;
                            Detalle.ID_CIATRANS = IdEmpresa;
                            Detalle.ID_CHOFER = IdChofer;
                            Detalle.TRANSPORTISTA_DESC = DesEmpresa;
                            Detalle.CHOFER_DESC = DesChofer;
                        }
                    }

                    tablePagination.DataSource = objPaseBRBK.Detalle;
                    tablePagination.DataBind();


                    Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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

        #endregion

        #region "Cargar informacion al buscar carga"
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                   
                    OcultarLoading("2");
                    CultureInfo enUS = new CultureInfo("en-US");
                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();

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
                    var Carga = PasePuerta.Pase_BRBK.ObtenerListaEditable(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc, ClsUsuario.loginname, null);
                    if (Carga.Exitoso)
                    {


                        /*********************************************NUEVA VALIDACION************************************************/
                        /*estado de la unidad*/
                        List<Int64> Lista = new List<Int64>();
                        List<Cls_Bil_PasePuertaBRBK_Validacion> Validacion = new List<Cls_Bil_PasePuertaBRBK_Validacion>();

                        var LinqPases = (from p in Carga.Resultado.AsEnumerable()
                                         where !string.IsNullOrEmpty(p.Field<string>("NUMERO_PASE_N4"))
                                         select new
                                         {
                                             PASE = p.Field<string>("NUMERO_PASE_N4")
                                         }).Distinct();

                        if (LinqPases != null && LinqPases.Count() > 0)
                        {
                            foreach (var Det in LinqPases)
                            {
                                Lista.Add(Int64.Parse(Det.PASE.ToString()));
                            }
                        }
                        var EstadoPases = N4.Importacion.container_brbk.ValidarEstadoTransaccion(Lista, ClsUsuario.loginname.Trim());

                        if (EstadoPases.Exitoso)
                        {
                            var Estados = from p in EstadoPases.Resultado.AsEnumerable()
                                          where p.Item1 != 0
                                          select new
                                          {
                                              NUMERO_PASE_N4 = p.Item1,
                                              UBICACION = p.Item2,
                                              MENSAJE = p.Item3,
                                              ESTADO = p.Item4
                                          };

                            foreach (var Det in Estados)
                            {
                                Validacion.Add(new Cls_Bil_PasePuertaBRBK_Validacion { NUMERO_PASE_N4 = Det.NUMERO_PASE_N4, UBICACION = Det.UBICACION, MENSAJE = Det.MENSAJE, ESTADO = Det.ESTADO });
                            }

                        }
                      
                        var LinqEstados = from p in Validacion.AsEnumerable()
                                          select new
                                          {
                                              NUMERO_PASE_N4 = p.NUMERO_PASE_N4,
                                              UBICACION = p.UBICACION,
                                              MENSAJE = p.MENSAJE,
                                              ESTADO = p.ESTADO
                                          };


                        /*********************************************FIN NUEVA VALIDACION************************************************/

                        var LinqQuery = (from p in Carga.Resultado.AsEnumerable()
                                         join c in LinqEstados on Int64.Parse(p.Field<string>("NUMERO_PASE_N4").ToString()) equals c.NUMERO_PASE_N4 into TmpFinal
                                         from Final in TmpFinal.DefaultIfEmpty()
                                         where !string.IsNullOrEmpty(p.Field<string>("NUMERO_PASE_N4"))
                                         select new
                                         {
                                             ID_PPWEB = p.Field<Int64?>("ID_PPWEB"),
                                             ID_PASE = p.Field<Decimal>("ID_PASE"),
                                             MRN = p.Field<string>("MRN") == null ? "" : p.Field<string>("MRN").Trim(),
                                             MSN = p.Field<string>("MSN") == null ? "" : p.Field<string>("MSN").Trim(),
                                             HSN = p.Field<string>("HSN") == null ? "" : p.Field<string>("HSN").Trim(),
                                             FACTURA = p.Field<string>("FACTURA") == null ? "" : p.Field<string>("FACTURA").Trim(),
                                             AGENTE = p.Field<string>("AGENTE") == null ? "" : p.Field<string>("AGENTE").Trim(),
                                             FACTURADO = p.Field<string>("FACTURADO") == null ? "" : p.Field<string>("FACTURADO").Trim(),
                                             GKEY = p.Field<Int64>("GKEY"),
                                             CONTENEDOR = p.Field<string>("CONTENEDOR") == null ? "" : p.Field<string>("CONTENEDOR").Trim(),
                                             DOCUMENTO = p.Field<string>("DOCUMENTO") == null ? "" : p.Field<string>("DOCUMENTO").Trim(),
                                             PRIMERA = p.Field<string>("PRIMERA") == null ? "" : p.Field<string>("PRIMERA").Trim(),
                                             MARCA = p.Field<string>("MARCA") == null ? "" : p.Field<string>("MARCA").Trim(),
                                             CANTIDAD = p.Field<int?>("CANTIDAD"),
                                             CANTIDAD_CARGA = p.Field<int?>("CANTIDAD_CARGA"),
                                             CIATRANS = p.Field<string>("CIATRANS") == null ? "" : p.Field<string>("CIATRANS").Trim(),
                                             CHOFER = p.Field<string>("CHOFER") == null ? "" : p.Field<string>("CHOFER").Trim(),
                                             PLACA = p.Field<string>("PLACA") == null ? "" : p.Field<string>("PLACA").Trim(),
                                             FECHA_SALIDA = p.Field<DateTime?>("FECHA_SALIDA"),
                                             FECHA_AUT_PPWEB = p.Field<DateTime?>("FECHA_AUT_PPWEB"),
                                             TIPO_CNTR = p.Field<string>("TIPO_CNTR") == null ? "" : p.Field<string>("TIPO_CNTR").Trim(),
                                             ID_TURNO = p.Field<Int64?>("ID_TURNO"),
                                             D_TURNO = p.Field<string>("D_TURNO") == null ? "" : p.Field<string>("D_TURNO").Trim(),
                                             FECHA_PASE = p.Field<DateTime?>("FECHA_PASE"),
                                             FECHA_ING = p.Field<DateTime?>("FECHA_ING"),
                                             AGENTE_DESC = p.Field<string>("AGENTE_DESC") == null ? "" : p.Field<string>("AGENTE_DESC").Trim(),
                                             FACTURADO_DESC = p.Field<string>("FACTURADO_DESC") == null ? "" : p.Field<string>("FACTURADO_DESC").Trim(),
                                             IMPORTADOR = p.Field<string>("IMPORTADOR") == null ? "" : p.Field<string>("IMPORTADOR").Trim(),
                                             IMPORTADOR_DESC = p.Field<string>("IMPORTADOR_DESC") == null ? "" : p.Field<string>("IMPORTADOR_DESC").Trim(),
                                             CNTR_DD = p.Field<bool?>("CNTR_DD") == null ? false : p.Field<bool?>("CNTR_DD"),
                                             TRANSPORTISTA_DESC = p.Field<string>("TRANSPORTISTA_DESC") == null ? "" : p.Field<string>("TRANSPORTISTA_DESC").Trim(),
                                             CHOFER_DESC = p.Field<string>("CHOFER_DESC") == null ? "" : p.Field<string>("CHOFER_DESC").Trim(),
                                             NUMERO_PASE_N4 = p.Field<string>("NUMERO_PASE_N4") == null ? "" : p.Field<string>("NUMERO_PASE_N4").Trim(),
                                             FECHA_EXPIRACION = p.Field<DateTime?>("FECHA_EXPIRACION"),
                                             SERVICIO = p.Field<bool?>("SERVICIO") == null ? false : p.Field<bool?>("SERVICIO"),
                                             ESTADO = p.Field<string>("ESTADO") == null ? "" : p.Field<string>("ESTADO").Trim(),
                                             ID_CHOFER = p.Field<string>("ID_CHOFER") == null ? "" : p.Field<string>("ID_CHOFER").Trim(),
                                             ID_EMPRESA = p.Field<string>("ID_EMPRESA") == null ? "" : p.Field<string>("ID_EMPRESA").Trim(),
                                             ID_PLACA = p.Field<string>("ID_PLACA") == null ? "" : p.Field<string>("ID_PLACA").Trim(),
                                             ID_CARGA = p.Field<Int64>("GKEY"),
                                             IN_OUT = (Final == null) ? "" : Final.UBICACION,
                                             PATIO = (Final == null) ? "" : Final.MENSAJE,
                                             ESTADO_TRANCCION = (Final == null) ? true : Final.ESTADO,
                                             ID_UNIDAD = p.Field<Int64?>("ID_UNIDAD"),
                                             idProducto = p.Field<int?>("idProducto"),
                                             idItem = p.Field<int?>("idItem"),
                                             Tipo_Producto = p.Field<string>("Tipo_Producto") == null ? "" : p.Field<string>("Tipo_Producto").Trim(),
                                             BODEGA = p.Field<string>("BODEGA") == null ? "" : p.Field<string>("BODEGA").Trim(),
                                             FECHA_ULT_FACTURA = p.Field<DateTime?>("FECHA_ULT_FACTURA"),
                                             ID_SOL = p.Field<Int64?>("ID_SOL"),
                                             SECUENCIA_SOL = p.Field<Int64?>("SECUENCIA_SOL")
                                         });


                        if (LinqQuery != null)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                            objPaseBRBK.FECHA = DateTime.Now;
                           
                            objPaseBRBK.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseBRBK.SESION = this.hf_BrowserWindowName.Value;

                            objPaseBRBK.Detalle.Clear();
                            
                            Int64 pValor = 0;
                            int SALDO_FINAL = 0;
                            int CANTIDAD_TOT_BL = 0;
                            int CANTIDAD_USADA = 0;


                            //para sacar informacion del saldo
                            string _BL = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());

                            var Saldo = PasePuerta.SaldoCargaBRBK.SaldoPendienteBRBK(ClsUsuario.ruc, _BL);
                            if (Saldo.Exitoso)
                            {
                                var LinqSaldo = (from Tbl in Saldo.Resultado.Where(p => !string.IsNullOrEmpty(p.NUMERO_CARGA))
                                                 select new
                                                 {
                                                     SALDO_FINAL = Tbl.SALDO_FINAL,
                                                     CANTIDAD_TOT_BL = Tbl.CANTIDAD_TOT_BL,
                                                     CANTIDAD_USADA = (Tbl.CANTIDAD_TOT_BL - Tbl.SALDO_FINAL)

                                                 }).FirstOrDefault();

                                if (LinqSaldo != null)
                                {
                                    SALDO_FINAL = LinqSaldo.SALDO_FINAL;
                                    CANTIDAD_TOT_BL = LinqSaldo.CANTIDAD_TOT_BL;
                                    CANTIDAD_USADA = LinqSaldo.CANTIDAD_USADA;
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


                            foreach (var Det in LinqQuery)
                            {

                                List<Cls_Bil_PasePuertaBRBK_InOut> PaseUsado = Cls_Bil_PasePuertaBRBK_InOut.Pase_Utilizado(Int64.Parse(Det.NUMERO_PASE_N4.ToString()), out cMensajes);

                                if (PaseUsado != null)
                                {
                                    pValor = (from Tbl in PaseUsado select new { VALOR = (Tbl.VALOR == 0) ? 0 : Tbl.VALOR }).FirstOrDefault().VALOR;
                                }
                                else { pValor = 0; }

                                objPaseBRBK.MRN = Det.MRN;
                                objPaseBRBK.MSN = Det.MSN;
                                objPaseBRBK.HSN = Det.HSN;

                                objPaseBRBK.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objPaseBRBK.FECHA_SALIDA_PASE = Det.FECHA_PASE;

                                objPaseBRBK.idProducto = Det.idProducto;
                                objPaseBRBK.idItem = Det.idItem;
                                objPaseBRBK.Tipo_Producto = Det.Tipo_Producto;

                                objPaseBRBK.CNTR_CANTIDAD_SALDO = SALDO_FINAL;
                                objPaseBRBK.CANTIDAD_EMITIDOS = CANTIDAD_USADA;
                                objPaseBRBK.CNTR_UBICACION = Det.BODEGA;

                                this.TxtSaldo.Text = objPaseBRBK.CNTR_CANTIDAD_SALDO.ToString();
                                this.TxtRetirados.Text = objPaseBRBK.CANTIDAD_EMITIDOS.ToString();
                                this.TxtBodega.Text = objPaseBRBK.CNTR_UBICACION;
                                this.TxtTipoProducto.Text = objPaseBRBK.Tipo_Producto;

                                this.TxtSaldo.Text = SALDO_FINAL.ToString();

                                objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();
                                objDetallePaseBRBK.FECHA = objPaseBRBK.FECHA;
                                objDetallePaseBRBK.MRN = objPaseBRBK.MRN;
                                objDetallePaseBRBK.MSN = objPaseBRBK.MSN;
                                objDetallePaseBRBK.HSN = objPaseBRBK.HSN;
                                objDetallePaseBRBK.IV_USUARIO_CREA = objPaseBRBK.IV_USUARIO_CREA;
                                objDetallePaseBRBK.SESION = objPaseBRBK.SESION;

                                objDetallePaseBRBK.FACTURA = Det.FACTURA;
                                objDetallePaseBRBK.CARGA = string.Format("{0}-{1}-{2}", Det.MRN, Det.MSN, Det.HSN);
                                objDetallePaseBRBK.AGENTE = Det.AGENTE;
                                objDetallePaseBRBK.FACTURADO = Det.FACTURADO;
                                objDetallePaseBRBK.GKEY = (Int64)Det.GKEY;
                                objDetallePaseBRBK.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseBRBK.DOCUMENTO = Det.DOCUMENTO;
                                objDetallePaseBRBK.PRIMERA = Det.PRIMERA;
                                objDetallePaseBRBK.MARCA = Det.MARCA;
                                objDetallePaseBRBK.CANTIDAD = Det.CANTIDAD;
                                objDetallePaseBRBK.CANTIDAD_CARGA = Det.CANTIDAD_CARGA;
                                objDetallePaseBRBK.CIATRANS = Det.ID_EMPRESA;
                                objDetallePaseBRBK.CHOFER = Det.ID_CHOFER;
                                objDetallePaseBRBK.PLACA = Det.ID_PLACA;

                               // objDetallePaseBRBK.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objDetallePaseBRBK.FECHA_SALIDA = Det.FECHA_ULT_FACTURA;
                               
                                objDetallePaseBRBK.CNTR_DD = Det.CNTR_DD.Value;
                                objDetallePaseBRBK.AGENTE_DESC = Det.AGENTE_DESC;
                                objDetallePaseBRBK.FACTURADO_DESC = Det.FACTURADO_DESC;
                                objDetallePaseBRBK.IMPORTADOR = Det.IMPORTADOR;
                                objDetallePaseBRBK.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;
                                objDetallePaseBRBK.FECHA_SALIDA_PASE = Det.FECHA_PASE;
                                objDetallePaseBRBK.FECHA_AUT_PPWEB = Det.FECHA_AUT_PPWEB;
                                objDetallePaseBRBK.TIPO_CNTR = Det.TIPO_CNTR;
                                objDetallePaseBRBK.ID_TURNO = Det.ID_TURNO;
                                objDetallePaseBRBK.TURNO = Det.ID_TURNO;
                                objDetallePaseBRBK.D_TURNO = Det.D_TURNO;
                                objDetallePaseBRBK.ID_PASE = (double)Det.ID_PASE;
                                objDetallePaseBRBK.ESTADO = Det.ESTADO;
                                objDetallePaseBRBK.FECHA_ING = Det.FECHA_ING;
                                objDetallePaseBRBK.ID_PPWEB = Det.ID_PPWEB;
                                objDetallePaseBRBK.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;

                                objDetallePaseBRBK.ID_CIATRANS = Det.ID_EMPRESA;
                                objDetallePaseBRBK.ID_CHOFER = Det.ID_CHOFER;

                                objDetallePaseBRBK.CIATRANS = string.Format("{0} - {1}", Det.ID_EMPRESA, Det.TRANSPORTISTA_DESC);
                                objDetallePaseBRBK.CHOFER = (!string.IsNullOrEmpty(Det.ID_CHOFER) ? string.Format("{0} - {1}", Det.ID_CHOFER, Det.CHOFER_DESC) : string.Empty);
                                objDetallePaseBRBK.PLACA = Det.ID_PLACA;
                                objDetallePaseBRBK.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                                objDetallePaseBRBK.CHOFER_DESC = Det.CHOFER_DESC;
                                objDetallePaseBRBK.LLAVE = Det.ID_PASE.ToString();
                                objDetallePaseBRBK.ID_UNIDAD = (Det.ID_UNIDAD == null ? 0 : Det.ID_UNIDAD);

                                objDetallePaseBRBK.ESTADO = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseBRBK.MOSTRAR_MENSAJE = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseBRBK.ORDENAMIENTO = (Det.ESTADO.Equals("GN") ? "1" : "2");
                                objDetallePaseBRBK.IN_OUT = Det.IN_OUT;
                                objDetallePaseBRBK.ESTADO_TRANSACCION = Det.ESTADO_TRANCCION;

                                objDetallePaseBRBK.FECHA_ULT_FAC = Det.FECHA_ULT_FACTURA;

                                objDetallePaseBRBK.ID_SOL = Det.ID_SOL.HasValue ? Det.ID_SOL : 0;
                                objDetallePaseBRBK.SECUENCIA_SOL = Det.SECUENCIA_SOL.HasValue ? Det.SECUENCIA_SOL : 0;

                                if (pValor == 1)
                                {
                                    objDetallePaseBRBK.MOSTRAR_MENSAJE = string.Format("{0} - {1} {2} {3}", objDetallePaseBRBK.ESTADO, "PASE UTILIZADO", (!string.IsNullOrEmpty(Det.PATIO) ? string.Format(" - {0}", Det.PATIO) : string.Empty),
                                          (Det.ID_SOL.HasValue ? (Det.ID_SOL.Value != 0 ? string.Format(" - SOLICITUD # {0}", Det.ID_SOL.Value) : "") : "")
                                        );
                                    objDetallePaseBRBK.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseBRBK.ORDENAMIENTO, "1");
                                    objDetallePaseBRBK.ESTADO_TRANSACCION = false;
                                }
                                else
                                {
                                    objDetallePaseBRBK.MOSTRAR_MENSAJE = string.Format("{0} - {1} {2} {3}", objDetallePaseBRBK.ESTADO, "PASE SIN USAR", (!string.IsNullOrEmpty(Det.PATIO) ? string.Format(" - {0}", Det.PATIO) : string.Empty),
                                          (Det.ID_SOL.HasValue ? (Det.ID_SOL.Value != 0 ? string.Format(" - SOLICITUD # {0}", Det.ID_SOL.Value) : "") : "")
                                        );
                                    objDetallePaseBRBK.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseBRBK.ORDENAMIENTO, "2");
                                }


                                objDetallePaseBRBK.CAMBIO_TURNO = "NO";
                                objDetallePaseBRBK.SERVICIO = Det.SERVICIO;


                                //si no tiene nada pendiente de facturar
                                if (!objDetallePaseBRBK.SERVICIO.Value)
                                {

                                    if (Det.CNTR_DD.Value)
                                    {
                                     
                                       // this.TxtFechaHasta.Text = Det.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
                                     
                                        objDetallePaseBRBK.TIPO_CNTR = string.Format("{0} - {1}", Det.TIPO_CNTR, "Desaduanamiento Directo");
                                    }
                                    else
                                    {

      
                                       // this.TxtFechaHasta.Text = Det.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
           
                                    }

                                    this.TxtContenedorSeleccionado.Text = CANTIDAD_TOT_BL.ToString();

                                    //valido si el pase fue utilizado y si la entrega esta completa
                                    if (pValor == 1 && !string.IsNullOrEmpty(Det.PATIO))
                                    {
                                        if (!Det.PATIO.Trim().ToUpper().Equals("ENTREGA COMPLETADA"))
                                        {
                                            objPaseBRBK.Detalle.Add(objDetallePaseBRBK);
                                        }
                                    }
                                    else
                                    {
                                        objPaseBRBK.Detalle.Add(objDetallePaseBRBK);
                                    }

                                       
                                }


                            }

                        
                            //datos del turno
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


                            tablePagination.DataSource = objPaseBRBK.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                            tablePagination.DataBind();


                            Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                            if (objPaseBRBK.Detalle.Count == 0)
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", objPaseBRBK.CARGA));
                                return;
                            }

                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
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
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Actualizar Pase BREAK BULK", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));
                }
            }




        }
        #endregion




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
                    objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder actualizar los pase a puerta de carga suelta. </b>"));
                        return;
                    }

                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen pases de puertas de carga suelta pendientes para generar.  </b>"));
                        return;
                    }

                    var LinqValidaFaltantes = (from p in objPaseBRBK.Detalle.Where(x => x.VISTO == true)
                                                       select p.ID_PASE).ToList();

                    if (LinqValidaFaltantes.Count <= 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los pases de puerta a realizar la actualización</b>"));
                        return;
                    }

                    
                    LoginName = objPaseBRBK.IV_USUARIO_CREA.Trim();


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
                    


                    /*VALIDACION NUEVA, VEHICULO ESTA FUERA 20-05-2020*/
                    /*estado de la unidad*/
                    List<Int64> Lista = new List<Int64>();

                    var LinqGkey = (from p in objPaseBRBK.Detalle.Where(x => x.VISTO == true)
                                    select new
                                    {
                                        NUMERO_PASE_N4 = Int64.Parse((string.IsNullOrEmpty(p.NUMERO_PASE_N4) ? "0" : p.NUMERO_PASE_N4)),
                                        ID_PASE = p.ID_PASE
                                    }).ToList();


                    if (LinqGkey.Count() > 0)
                    {
                        foreach (var Det in LinqGkey)
                        {
                            Lista.Add(Det.NUMERO_PASE_N4);
                        }
                    }

                    var EstadoUnidad = N4.Importacion.container_brbk.ValidarEstadoTransaccion(Lista, LoginName);
                    if (EstadoUnidad.Exitoso)
                    {
                        var LinqUnidades = (from p in EstadoUnidad.Resultado.AsEnumerable()
                                            join c in LinqGkey on p.Item1 equals c.NUMERO_PASE_N4 into TmpFinal
                                            from Final in TmpFinal.DefaultIfEmpty()
                                            where p.Item1 != 0
                                            select new
                                            {
                                                NUMERO_PASE_N4 = p.Item1,
                                                UBICACION = p.Item2,
                                                MENSAJE = p.Item3,
                                                ESTADO = p.Item4,
                                                ID_PASE = (Final == null) ? 0 : Final.ID_PASE
                                            });
                        foreach (var Det in LinqUnidades)
                        {
                            if (!Det.ESTADO)
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se puede actualizar el pase # :{0}, la unidad tiene estado: {1}, desmarque el pase, para continuar con el proceso.. </b>", Det.ID_PASE, Det.MENSAJE));
                                return;
                            }
                        }
                        /**********************FIN VALIDACION*************************************/

                    }



                    /*contenedores seleccionados para emitir pase*/
                    var LinqQuery = (from Tbl in objPaseBRBK.Detalle.Where(Tbl => Tbl.VISTO == true)
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
                                         CANTIDAD_CARGA = (Tbl.CANTIDAD_CARGA == null) ? 0 : Tbl.CANTIDAD_CARGA,
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
                                         TURNO = (int)((Tbl.TURNO == null) ? 0 : Tbl.TURNO),
                                         D_TURNO = (Tbl.D_TURNO == null) ? string.Empty : Tbl.D_TURNO,
                                         ID_PASE = (Tbl.ID_PASE == null) ? 0 : Tbl.ID_PASE,                          
                                         USUARIO = Tbl.USUARIO_ING,
                                         TURNO_DESDE = (Tbl.TURNO_DESDE.HasValue ? Tbl.TURNO_DESDE : null),
                                         TURNO_HASTA = (Tbl.TURNO_HASTA.HasValue ? Tbl.TURNO_HASTA : null),
                                         CNTR_DD = Tbl.CNTR_DD,
                                         AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? string.Empty : Tbl.AGENTE_DESC,
                                         FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                         IMPORTADOR = (Tbl.IMPORTADOR == null) ? string.Empty : Tbl.IMPORTADOR,
                                         IMPORTADOR_DESC = (Tbl.IMPORTADOR_DESC == null) ? string.Empty : Tbl.IMPORTADOR_DESC,
                                         TRANSPORTISTA_DESC = (Tbl.TRANSPORTISTA_DESC == null) ? string.Empty : Tbl.TRANSPORTISTA_DESC,
                                         CHOFER_DESC = (Tbl.CHOFER_DESC == null) ? string.Empty : Tbl.CHOFER_DESC,
                                         NUMERO_PASE_N4 = (Tbl.NUMERO_PASE_N4 == null) ? "0" : Tbl.NUMERO_PASE_N4,
                                         ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD,
                                         CAMBIO_TURNO = (string.IsNullOrEmpty(Tbl.CAMBIO_TURNO) ? "NO" : Tbl.CAMBIO_TURNO)
                                     }).ToList().OrderBy(x => x.CONTENEDOR);


                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/

                    string token = string.Empty;
                   
                    string order_id = string.Empty;
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
                  

                   
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    int nTotal = 0;

                    foreach (var Det in LinqQuery)
                    {
                        if (Det.CAMBIO_TURNO.Equals("SI"))
                        {
                            
                        }

                        Pase_BRBK pase = new Pase_BRBK();
                        pase.ID_PASE = (decimal)Det.ID_PASE;
                        pase.PPW = Det.ID_PPWEB;
                        pase.ID_CARGA = Det.GKEY;
                        pase.ESTADO = "GN";
                        pase.FECHA_EXPIRACION = Det.FECHA_SALIDA_PASE;
                        pase.CANTIDAD_CARGA = Det.CANTIDAD_CARGA;
                        pase.ID_PLACA = (string.IsNullOrEmpty(Det.PLACA) ? null : Det.PLACA);
                        pase.ID_CHOFER = (string.IsNullOrEmpty(Det.ID_CHOFER) ? null : Det.ID_CHOFER);
                        pase.ID_EMPRESA = (string.IsNullOrEmpty(Det.ID_CIATRANS) ? null : Det.ID_CIATRANS);
                        pase.CONSIGNATARIO_ID = (string.IsNullOrEmpty(Det.IMPORTADOR) ? null : Det.IMPORTADOR);
                        pase.CONSIGNARIO_NOMBRE = (string.IsNullOrEmpty(Det.IMPORTADOR_DESC) ? null : Det.IMPORTADOR_DESC);
                        pase.TRANSPORTISTA_DESC = (string.IsNullOrEmpty(Det.TRANSPORTISTA_DESC) ? null : Det.TRANSPORTISTA_DESC);
                        pase.CHOFER_DESC = (string.IsNullOrEmpty(Det.CHOFER_DESC) ? null : Det.CHOFER_DESC);
                        pase.ID_UNIDAD = Det.ID_UNIDAD;


                        pase.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;


                        pase.USUARIO_REGISTRO = ClsUsuario.loginname;
                        pase.TIPO_CARGA = "BRBK";
                     
                        
                        pase.PPW = Det.ID_PPWEB;

                       

                        var Resultado = pase.Actualizar(ClsUsuario.loginname,Det.TURNO, Det.CAMBIO_TURNO, Det.MRN, Det.MSN, Det.HSN);
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
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar el pase de puerta para la carga: {0}, Total bultos {1} Existen los siguientes problemas: {2}, {3} </b>", Det.CARGA,Det.CANTIDAD_CARGA , Resultado.MensajeInformacion, Resultado.MensajeProblema));
                            return;
                        }

                    }




                    if (nTotal != 0)
                    {
                        string link = string.Format("<a href='../pasepuertabrbk/imprimirpasebrbk.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta</a>", id_carga);

                        //limpiar
                        objPaseBRBK.Detalle.Clear();
                        objPaseBRBK.DetalleSubItem.Clear();

                        Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                     

                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        this.Limpia_Campos();


                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron con la actualización de {0} pase de puerta con éxito, para proceder a imprimir los mismo, <br/>por favor dar click en el siguiente link: {1} </b>", nTotal, link));
                        return;
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Actualizar Pase CFS", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }
           


        }

        #region "Eventos de la grilla de pases de puerta cfs"

        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label LblPase = (Label)item.FindControl("LblPase");

                double ID_PASE = double.Parse(LblPase.Text);
                //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;

                //actualiza datos del contenedor
                objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPase.Checked;

                }

                tablePagination.DataSource = objPaseBRBK.Detalle.OrderBy(p => p.ORDENAMIENTO);
                tablePagination.DataBind();

                Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                this.Pintar_Grilla();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }

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

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

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

                    double PASE = 0;
                    if (!double.TryParse(t, out PASE))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;
                    }

                    if (e.CommandName == "Actualizar")
                    {
                        this.TxtPaseModifica.Text = t.ToString();

                        

                        objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;

                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.ID_PASE == PASE);
                        if (Detalle != null)
                        {
                            FechaActualSalida = Detalle.FECHA_SALIDA_PASE.Value;
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                            this.TxtFacturadoHasta.Text = Detalle.FECHA_ULT_FAC.Value.ToString("MM/dd/yyyy");
                            this.TxtCantidadRetirar.Text = Detalle.CANTIDAD_CARGA.ToString();

                            if (!string.IsNullOrEmpty(Detalle.CIATRANS))
                            {
                                this.Txtempresa.Text = Detalle.CIATRANS;

                            }
                            if (!string.IsNullOrEmpty(Detalle.CHOFER))
                            {
                                this.TxtChofer.Text = Detalle.CHOFER;

                            }
                            if (!string.IsNullOrEmpty(Detalle.PLACA))
                            {
                                this.TxtPlaca.Text = Detalle.PLACA;

                            }

                            Detalle.VISTO = true;


                            this.Actualiza_Paneles();

                            tablePagination.DataSource = objPaseBRBK.Detalle.OrderBy(p => p.ORDENAMIENTO);
                            tablePagination.DataBind();

                            Session["PaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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
                        else
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = string.Empty;
                            this.TxtFacturadoHasta.Text = string.Empty;
                            this.TxtCantidadRetirar.Text = string.Empty;

                            this.CboTurnos.Focus();

                        }


                        this.Actualiza_Paneles();

                    } 


                    if (e.CommandName == "Eliminar")
                    {
                        objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;


                        //existe pase a remover
                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));
                        if (Detalle != null)
                        {
                            string Llave = Detalle.LLAVE;
                            //remover pase
                            objPaseBRBK.Detalle.Remove(objPaseBRBK.Detalle.Where(p => p.LLAVE == Llave).FirstOrDefault());

                            //recorrido de subitems
                            foreach (var Det in objPaseBRBK.DetalleSubItem.Where(x => x.MARCADO_SUBITEMS.Equals(Llave)))
                            {
                                ConsecutivoSelec = Det.CONSECUTIVO.Value;
                                var DetalleSubItem = objPaseBRBK.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
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

                      
                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        this.Pintar_Grilla();

                        Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;


                        this.Actualiza_Paneles();

                    }

                    //nuevos servicios
                    if (e.CommandName == "Facturar")
                    {
                        objPaseBRBK = Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                        string CARGA = string.Empty;
                        Int64 ID_UNIDAD = 0;
                        bool proceso_ok = false;
                        Dictionary<Int64, string> Lista_Gkeys = new Dictionary<Int64, string>();

                        //marcar a todos
                        foreach (var Det in objPaseBRBK.Detalle.Where(p => p.SERVICIO == false && p.ID_PASE == PASE))
                        {
                            var Existe = objPaseBRBK.Detalle.FirstOrDefault(q => q.ID_PASE.Equals(Det.ID_PASE));
                            if (Existe != null)
                            {
                                Existe.SERVICIO = true;
                                CARGA = Existe.CARGA;
                                ID_UNIDAD = Existe.ID_UNIDAD.Value;
                                Lista_Gkeys.Add(Int64.Parse(Existe.ID_PASE.ToString()), Existe.NUMERO_PASE_N4);
                                proceso_ok = true;
                            }
                        }

                        if (proceso_ok)
                        {
                            var Resultado = Pase_BRBK.Marcar_Servicio(ClsUsuario.loginname, ID_UNIDAD, Lista_Gkeys);
                            if (!Resultado.Exitoso)
                            {
                                /*************************************************************************************************************************************
                               * crear caso salesforce
                               ***********************************************************************************************************************************/
                                MensajesErrores = string.Format("Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: {1}, {2} </b>", CARGA, Resultado.MensajeInformacion, Resultado.MensajeProblema);

                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Break Bulk", "Error al cancelar pase Break Bulk", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                   "", "", out MensajeCasos, false);

                                /*************************************************************************************************************************************
                                * fin caso salesforce
                                **************************************************************************************************************************************/

                                this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: {1} </b>", CARGA, MensajeCasos));

                        

                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: No existen registros de pases </b>", CARGA));
                            return;
                        }


                        tablePagination.DataSource = objPaseBRBK.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                        tablePagination.DataBind();

                        Session["ActuPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;


                        string id_carga = securetext(CARGA.Replace("-", "+"));
                        string link = string.Format("<a href='../cargabrbk/facturacionbrbk.aspx?ID_CARGA={0}' target ='_parent'>Facturar BREAK BULK</a>", id_carga);
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar nuevos eventos para emitir nueva factura adicional con éxito, para proceder a emitir la misma, <br/>por favor ir a la opción de IMPO BRBK para generar la factura. </b>"));
                        return;
                        
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Actualizar Pase BREAK BULK", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }

        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox Chk = e.Item.FindControl("chkPase") as CheckBox;
                Label Estado = e.Item.FindControl("LblEstado") as Label;
                Label Estado2 = e.Item.FindControl("LblIn_Out") as Label;
                Label LblCambioturno = e.Item.FindControl("LblCambioturno") as Label;
                Label LblEstadoTransaccion = e.Item.FindControl("LblEstadoTransaccion") as Label;
                Label LblIdSolicitud = e.Item.FindControl("LblIdSolicitud") as Label;

                bool estado_transaccion = bool.Parse(LblEstadoTransaccion.Text);
                Button Btn = e.Item.FindControl("BtnActualizar") as Button;

                Button BtnEvento = e.Item.FindControl("BtnEvento") as Button;
                if (Estado.Text.Equals("EXPIRADO") || estado_transaccion == false || LblCambioturno.Text.Equals("SI"))
                {
                    Chk.Enabled = false;

                    Btn.Attributes["disabled"] = "disabled";

                    if (Estado.Text.Equals("EXPIRADO"))
                    {
                        BtnEvento.Visible = true;
                    }
                    else {
                        BtnEvento.Visible = false;
                    }

                }
                else {

                    if (!LblIdSolicitud.Text.Equals("0"))
                    {
                        Chk.Enabled = false;

                        Btn.Attributes["disabled"] = "disabled";

                        if (Estado.Text.Equals("EXPIRADO"))
                        {
                            BtnEvento.Visible = true;
                        }
                        else
                        {
                            BtnEvento.Visible = false;
                            //Chk.Enabled = true;
                        }
                        //BtnEvento.Visible = true;
                    }
                    else
                    {
                        Chk.Enabled = true;
                        BtnEvento.Visible = false;
                    }

                    //if (!LblIdSolicitud.Text.Equals("0"))
                    //{
                    //    Chk.Enabled = false;

                    //    if (Estado.Text.Equals("EXPIRADO"))
                    //    {
                    //        BtnEvento.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        BtnEvento.Visible = false;
                    //    }
                    //}
                    //else
                    //{
                    //    BtnEvento.Visible = false;
                    //}

                    // BtnEvento.Visible = false;
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

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                  
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