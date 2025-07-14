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
  

    public partial class p2d_emisionpase_freightforwarder : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objPaseCFS = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_PasePuertaCFS_SubItems objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
        private Cls_Bil_PasePuertaCFS_Detalle objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();
        private cfs_detalle_pase_multidespacho objDetallePaseFacturas = new cfs_detalle_pase_multidespacho();

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
           
            UPBOTONES.Update();
            UPCAS.Update();
            UPFECHASALIDA.Update();
            UPTURNO.Update();
        
          
           
        }

        private void Limpia_Campos()
        {
          
            this.Txtempresa.Text = string.Empty;
            this.TxtChofer.Text = string.Empty;
            this.TxtPlaca.Text = string.Empty;
            this.TxtFechaHasta.Text = string.Empty;
          

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

        private void MostrarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader('" + valor + "');", true);
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
            int i = 0;
            foreach (RepeaterItem xitem in grilla.Items)
            {
            

                Label CARGA = xitem.FindControl("CARGA") as Label;
                Label CONTENEDOR = xitem.FindControl("CONTENEDOR") as Label;
                Label FECHA_SALIDA_PASE = xitem.FindControl("FECHA_SALIDA_PASE") as Label;
                Label MARCA = xitem.FindControl("MARCA") as Label;
                Label CANTIDAD = xitem.FindControl("CANTIDAD") as Label;
                Label ESTADO_PAGO = xitem.FindControl("ESTADO_PAGO") as Label;
                Label UBICACION = xitem.FindControl("UBICACION") as Label;

                if (i == 0)
                {
                    CARGA.ForeColor = System.Drawing.Color.PaleVioletRed;
                    CONTENEDOR.ForeColor = System.Drawing.Color.PaleVioletRed;
                    FECHA_SALIDA_PASE.ForeColor = System.Drawing.Color.PaleVioletRed;
                    MARCA.ForeColor = System.Drawing.Color.PaleVioletRed;
                    CANTIDAD.ForeColor = System.Drawing.Color.PaleVioletRed;
                    ESTADO_PAGO.ForeColor = System.Drawing.Color.PaleVioletRed;
                    UBICACION.ForeColor = System.Drawing.Color.PaleVioletRed;
                }

               

                i++;
            }
        }


     
        #endregion

      

        #region "Eventos del Formulario"

      


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
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la factura, para poder seleccionar la fecha del turno... </b>"));
                        return;
                    }
                    if (objPaseCFS.DetallePaseFacturas.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de cargas, para poder seleccionar la fecha del turno... </b>"));
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


                        List<Int64> Lista = new List<Int64>();
                        foreach (var Det in objPaseCFS.DetalleSubItem)
                        {
                            Lista.Add(Det.CONSECUTIVO.Value);
                        }

                        var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista, false, objPaseCFS.EXPRESS);
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

                        return;
                    }



                    //verificar si es pase sin turno
                    string carga_sin_turno = string.Empty;

                    foreach (var Det in objPaseCFS.DetallePaseFacturas)
                    {
                        carga_sin_turno = string.Format("{0}-{1}-{2}", Det.MRN, Det.MSN, Det.HSN);
                        var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, Det.MRN, Det.MSN, Det.HSN);
                        if (PaseSinturno.Exitoso)
                        {
                            if (PaseSinturno.Resultado)
                            {
                                EsPasesinTurno = true;
                                break;
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
                    }
                   

                    //si es pase sin turno
                    if (EsPasesinTurno)
                    {
                        this.Pase_Sin_Turno_Default();
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se puede generar pase de puertas de cargas sin turno, de la carga: {0}. </b>", carga_sin_turno));
                        return;
                    }
                    else
                    {
                        List<Int64> Lista = new List<Int64>();
                        foreach (var Det in objPaseCFS.DetalleSubItem)
                        {
                            Lista.Add(Det.CONSECUTIVO.Value);
                        }

                        var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista, false, objPaseCFS.EXPRESS);
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
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la factura, para poder agregar un turno... </b>"));
                        return;
                    }
                    if (objPaseCFS.DetallePaseFacturas.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de cargas, para poder agregar un turno... </b>"));
                        return;
                    }
                    if (objPaseCFS.DetalleSubItem.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, para poder agregar un turno... </b>"));
                        return;
                    }

                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == false)
                                                       select p.CONSECUTIVO).ToList();

                    if (LinqValidaSubItemsFaltantes.Count > 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Existen bultos o subitems sin seleccionar, debe completar toda la partida para poder agregar el turno. </b>"));
                        return;
                    }


                    //verificar si es pase sin turno
                    string carga_sin_turno = string.Empty;

                    foreach (var Det in objPaseCFS.DetallePaseFacturas)
                    {
                        carga_sin_turno = string.Format("{0}-{1}-{2}", Det.MRN, Det.MSN, Det.HSN);

                        var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, Det.MRN, Det.MSN, Det.HSN);
                        if (PaseSinturno.Exitoso)
                        {
                            if (PaseSinturno.Resultado)
                            {
                                EsPasesinTurno = true;
                                break;
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
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Existen cargas, agregadas como sin turno, no podrá agregar un turnos para este tipo de cargas:{0}  </b>", carga_sin_turno));
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

                                break;


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


                        foreach (var Pas in objPaseCFS.Detalle)
                        {
                            var lst_pase = new StringBuilder();
                            lst_pase.Append("<TARJA>");

                            foreach (var i in objPaseCFS.DetalleSubItem.Where(x => x.CONSECUTIVO != 0))
                            {
                                lst_pase.AppendFormat("<VALOR CONSECUTIVO=\"{0}\"/>", i.CONSECUTIVO);
                            }

                            lst_pase.Append("</TARJA>");

                            objCtock.IDDISPONIBLEDET = IDDISPONIBLEDET;
                            objCtock.FECHA = FechaActualSalida;
                            objCtock.Bultos = TotalBultos;
                            objCtock.MRN = Pas.MRN;
                            objCtock.MSN = Pas.MSN;
                            objCtock.HSN = Pas.HSN;
                            objCtock.BODEGA = BODEGA;
                            objCtock.subitems = lst_pase.ToString();
                            objCtock.USUARIOING = ClsUsuario.loginname;

                            var nProceso = objCtock.SaveTransaction(out cMensajes);
                            if (!nProceso.HasValue || nProceso.Value <= 0)
                            {

                                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo grabar datos del turno.{0}</b>", cMensajes));
                                return;
                            }

                        }
                        
                     
                        List<Int64> Lista = new List<Int64>();
                        foreach (var Det in objPaseCFS.DetalleSubItem)
                        {
                            Lista.Add(Det.CONSECUTIVO.Value);
                        }

                        var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista, false, objPaseCFS.EXPRESS);
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
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                            return;

                        }

                        this.Actualiza_Paneles();
                        
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

        #region "Eventos del combo de las facturas"
        private void CboFacturas()
        {
            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
               // ClsUsuario.ruc = "0992424311001";
                List<P2D_MULTI_Pendiente> Listado = P2D_MULTI_Pendiente.cargar_facturas_emitirpase(ClsUsuario.ruc, out cMensajes);

                this.gvCustomers.DataSource = Listado;
                this.gvCustomers.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "CboFacturas", "Hubo un error al cargar facturas", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.originalBackgroundColor=this.style.backgroundColor;this.style.backgroundColor='#bbbbbb';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor=this.originalBackgroundColor;";
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvCustomers, "Select$" + e.Row.RowIndex);

               
            }
        }

        //cuando selecciona una factura
        protected void gvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (gvCustomers.SelectedRow != null)
                    {
                        grilla.DataSource = null;
                        grilla.DataBind();

                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        tablePagination_Tarja.DataSource = null;
                        tablePagination_Tarja.DataBind();

                        this.Ocultar_Mensaje();

                        

                      

                        hfCustomerId.Value = Server.HtmlDecode(gvCustomers.SelectedRow.Cells[0].Text);
                        txtCustomer.Text = Server.HtmlDecode(gvCustomers.SelectedRow.Cells[1].Text);

                        //busca contenedores por ruc de usuario
                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                       // ClsUsuario.ruc = "0992424311001";
                        var Carga = PasePuerta.Pase_WebCFS.ObtenerCargaPaseCFS_MultiDespachoTransporte(Int64.Parse(hfCustomerId.Value));
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
                                                 DIRECCION = (Tbl.DIRECCION == null) ? string.Empty : Tbl.DIRECCION,
                                                 CIUDAD = (Tbl.CIUDAD == null) ? string.Empty : Tbl.CIUDAD,
                                                 ZONA = (Tbl.ZONA == null) ? string.Empty : Tbl.ZONA,
                                                 ID_CIUDAD = Tbl.ID_CIUDAD,
                                                 ID_ZONA = Tbl.ID_ZONA

                                             }).ToList();

                            objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;

                            //detalle de cargas
                            objPaseCFS.DetallePaseFacturas.Clear();

                            //detalle de pases
                            objPaseCFS.Detalle.Clear();

                            //detalle de subitems
                            objPaseCFS.DetalleSubItem.Clear();

                            int i = 0;

                            Configuraciones.ModuloAccesorios Cfgs = new Configuraciones.ModuloAccesorios("LIFTIF");
                            Cfgs.ConfiguracionBase = "DATACON";
                            string pv = string.Empty;
                            if (!Cfgs.Inicializar(out pv))
                            {
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener configuraciones LIFTIF....{0}</b>", pv));
                                return;
                            }

                            var ptransporte = Cfgs.ObtenerConfiguracion("TRANSPORTE");
                            string IdEmpresa = string.Empty;
                            string DesEmpresa = string.Empty;

                            string empresa_transporte = ptransporte == null ? string.Empty : ptransporte.valor;
                            if (!string.IsNullOrEmpty(empresa_transporte))
                            {
                                if (empresa_transporte.Split('-').ToList().Count > 1)
                                {
                                     IdEmpresa = empresa_transporte.Split('-').ToList()[0].Trim();

                                     DesEmpresa = (empresa_transporte.Split('-').ToList().Count >= 3 ? string.Format("{0} - {1}", empresa_transporte.Split('-').ToList()[1].Trim(), empresa_transporte.Split('-').ToList()[2].Trim()) : empresa_transporte.Split('-').ToList()[1].Trim());

                                    this.IdTxtempresa.Value = IdEmpresa;
                                    this.Txtempresa.Text = empresa_transporte;
                                    this.UPTRANSPORTE.Update();
                                }
                            }

                            foreach (var Det in LinqQuery)
                            {
                                objDetallePaseFacturas = new cfs_detalle_pase_multidespacho();
                                objDetallePaseFacturas.FECHA = DateTime.Now;
                                objDetallePaseFacturas.MRN = Det.MRN;
                                objDetallePaseFacturas.MSN = Det.MSN;
                                objDetallePaseFacturas.HSN = Det.HSN;
                                objDetallePaseFacturas.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objDetallePaseFacturas.SESION = this.hf_BrowserWindowName.Value;

                                objDetallePaseFacturas.FACTURA = Det.FACTURA;
                                objDetallePaseFacturas.CARGA = Det.CARGA;
                                objDetallePaseFacturas.AGENTE = Det.AGENTE;
                                objDetallePaseFacturas.FACTURADO = Det.FACTURADO;
                                objDetallePaseFacturas.PAGADO = Det.PAGADO;
                                objDetallePaseFacturas.GKEY = Det.GKEY;
                                objDetallePaseFacturas.REFERENCIA = Det.REFERENCIA;
                                objDetallePaseFacturas.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseFacturas.DOCUMENTO = Det.DOCUMENTO;
                                objDetallePaseFacturas.PRIMERA = Det.PRIMERA;
                                objDetallePaseFacturas.MARCA = Det.MARCA;
                                objDetallePaseFacturas.CANTIDAD = Det.CANTIDAD;
                                objDetallePaseFacturas.CIATRANS = empresa_transporte;
                                objDetallePaseFacturas.ID_CIATRANS = IdEmpresa;
                                objDetallePaseFacturas.CHOFER = string.Empty;
                                objDetallePaseFacturas.PLACA = string.Empty;
                                objDetallePaseFacturas.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objDetallePaseFacturas.CNTR_DD = Det.CNTR_DD.Value;
                                objDetallePaseFacturas.AGENTE_DESC = Det.AGENTE_DESC;
                                objDetallePaseFacturas.FACTURADO_DESC = Det.FACTURADO_DESC;
                                objDetallePaseFacturas.IMPORTADOR = Det.IMPORTADOR;
                                objDetallePaseFacturas.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;

                                objDetallePaseFacturas.DIRECCION = Det.DIRECCION;
                                objDetallePaseFacturas.CIUDAD = Det.CIUDAD;
                                objDetallePaseFacturas.ZONA = Det.ZONA;
                                objDetallePaseFacturas.ID_CIUDAD = Det.ID_CIUDAD;
                                objDetallePaseFacturas.ID_ZONA = Det.ID_ZONA;

                                if (Det.CNTR_DD.Value)
                                {
                                    objDetallePaseFacturas.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;

                                    if (i == 0)
                                    {
                                        this.TxtFechaHasta.Text = Det.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                        this.TxtFechaCas.Text = Det.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                        objPaseCFS.FECHA_SALIDA = Det.FECHA_SALIDA;
                                        objPaseCFS.EXPRESS = false;
                                    }    
                                }
                                else
                                {
                                    objDetallePaseFacturas.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                                    if (i == 0)
                                    {
                                        this.TxtFechaHasta.Text = Det.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                        this.TxtFechaCas.Text = Det.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");
                                        objPaseCFS.FECHA_SALIDA = Det.FECHA_SALIDA;
                                        objPaseCFS.EXPRESS = false;
                                    }
                                       
                                }

                                if (Det.PAGADO == false)
                                {
                                    this.BtnGrabar.Attributes.Add("disabled", "disabled");
                                }

                                objDetallePaseFacturas.FECHA_AUT_PPWEB = Det.FECHA_AUT_PPWEB;
                                objDetallePaseFacturas.HORA_AUT_PPWEB = Det.HORA_AUT_PPWEB;

                                objDetallePaseFacturas.TIPO_CNTR = Det.TIPO_CNTR;
                                objDetallePaseFacturas.ID_TURNO = Det.ID_TURNO;
                                objDetallePaseFacturas.TURNO = Det.TURNO;
                                objDetallePaseFacturas.D_TURNO = Det.D_TURNO;
                                objDetallePaseFacturas.ID_PASE = double.Parse(Det.ID_PASE.Value.ToString());
                                objDetallePaseFacturas.ESTADO = Det.ESTADO;
                                objDetallePaseFacturas.ENVIADO = Det.ENVIADO;
                                objDetallePaseFacturas.AUTORIZADO = Det.AUTORIZADO;
                                objDetallePaseFacturas.VENTANILLA = Det.VENTANILLA;
                                objDetallePaseFacturas.USUARIO_ING = Det.USUARIO_ING;
                                objDetallePaseFacturas.FECHA_ING = System.DateTime.Now.Date;
                                objDetallePaseFacturas.USUARIO_MOD = Det.USUARIO_MOD;
                                objDetallePaseFacturas.ESTADO_PAGO = Det.ESTADO_PAGO;

                                objDetallePaseFacturas.ID_PPWEB = Det.ID_PPWEB;
                                objDetallePaseFacturas.ID_UNIDAD = Det.ID_UNIDAD;

                                if (Det.CNTR_DD.Value)
                                {
                                    objDetallePaseFacturas.TIPO_CNTR = string.Format("{0} - {1}", Det.TIPO_CNTR, "Desaduanamiento Directo");
                                    this.BtnGrabar.Attributes.Add("disabled", "disabled");
                                }

                                objPaseCFS.DetallePaseFacturas.Add(objDetallePaseFacturas);


                                //detalle de cargas para ingresar subitems
                                //consulta detalle de subitems
                                var Tarja = PasePuerta.Pase_WebCFS.ObtenerTarjaCFS(Det.MRN, Det.MSN, Det.HSN, Det.GKEY.Value);
                                if (Tarja.Exitoso)
                                {
                                    var LinqTarja = (from Tbl in Tarja.Resultado.Where(Tbl => Tbl.CONSECUTIVO != 0)
                                                     select new
                                                     {
                                                         CONSECUTIVO = Tbl.CONSECUTIVO,
                                                         CARGA = Det.CARGA,
                                                         CANTIDAD = Tbl.CANTIDAD,
                                                         MRN = Det.MRN,
                                                         MSN = Det.MSN,
                                                         HSN = Det.HSN,
                                                         P2D_ALTO = Tbl.P2D_ALTO == null ? 0 : Tbl.P2D_ALTO.Value,
                                                         P2D_ANCHO = Tbl.P2D_ANCHO == null ? 0 : Tbl.P2D_ANCHO.Value,
                                                         P2D_LARGO = Tbl.P2D_LARGO == null ? 0 : Tbl.P2D_LARGO.Value,
                                                         PESO = Tbl.PESO == null ? 0 : Tbl.PESO.Value,
                                                         P2D_VOLUMEN = Tbl.P2D_VOLUMEN == null ? 0 : Tbl.P2D_VOLUMEN.Value,
                                                         IMO = string.IsNullOrEmpty(Tbl.IMO) ? "NO APLICA" : Tbl.IMO.Trim()
                                                     }).ToList().OrderBy(x => x.CONSECUTIVO);

                                    List<Int64> Lista = new List<Int64>();

                                    foreach (var Tar in LinqTarja)
                                    {

                                        objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
                                        objPaseCFSTarja.CARGA = Tar.CARGA;
                                        objPaseCFSTarja.MRN = Tar.MRN;
                                        objPaseCFSTarja.MSN = Tar.MSN;
                                        objPaseCFSTarja.HSN = Tar.HSN;
                                        objPaseCFSTarja.CONSECUTIVO = Tar.CONSECUTIVO;
                                        objPaseCFSTarja.CANTIDAD = Tar.CANTIDAD.Value;
                                        objPaseCFSTarja.CIATRANS = empresa_transporte;
                                        objPaseCFSTarja.CHOFER = string.Empty;
                                        objPaseCFSTarja.ID_CIATRANS = IdEmpresa;
                                        objPaseCFSTarja.ID_CHOFER = string.Empty;
                                        objPaseCFSTarja.PLACA = string.Empty;
                                        objPaseCFSTarja.VISTO = true;
                                        objPaseCFSTarja.TRANSPORTISTA_DESC = string.Empty;
                                        objPaseCFSTarja.CHOFER_DESC = string.Empty;
                                        objPaseCFSTarja.ESTADO_PAGO = Det.ESTADO_PAGO;
                                        objPaseCFSTarja.MARCADO_SUBITEMS = string.Empty;

                                        objPaseCFSTarja.P2D_ALTO = Tar.P2D_ALTO;
                                        objPaseCFSTarja.P2D_ANCHO = Tar.P2D_ANCHO;
                                        objPaseCFSTarja.P2D_LARGO = Tar.P2D_LARGO;
                                        objPaseCFSTarja.PESO = Tar.PESO;
                                        objPaseCFSTarja.P2D_VOLUMEN = Tar.P2D_VOLUMEN;
                                        objPaseCFSTarja.IMO = Tar.IMO;

                                        Lista.Add(Tar.CONSECUTIVO.Value);

                                        objPaseCFS.DetalleSubItem.Add(objPaseCFSTarja);


                                    }

                                    //valida si tiene ubicacion la carga       
                                    var LinqUbicacion = Pase_WebCFS.Verficar_Ubicacion(ClsUsuario.loginname, Lista);
                                    if (!LinqUbicacion.Exitoso)
                                    {
                                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: La carga no presenta ubicación..{0}", LinqUbicacion.MensajeProblema);

                                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Emisión Pase CFS", "La carga no presenta ubicación..", MensajesErrores.Trim(), Det.CARGA,
                                            Det.FACTURADO_DESC, Det.AGENTE_DESC, out MensajeCasos, false);

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", MensajeCasos));
                                        return;
                                    }
                                    else
                                    {
                                        objDetallePaseFacturas.UBICACION = string.IsNullOrEmpty(LinqUbicacion.Resultado) ? "" : LinqUbicacion.Resultado;
                                    }


                                }
                                else
                                {
                                    this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe ubicación para la carga ingresada..{0}..{1}", Det.CARGA, Tarja.MensajeProblema));
                                    return;
                                }


                                i++;
                            }

                           

                            var TotalBultos = objPaseCFS.DetalleSubItem.Sum(x => x.CANTIDAD);
                            this.LabelTotal.InnerText = string.Format("DETALLE DE SUB. ÍTEMS - TOTAL BULTOS: {0}", TotalBultos);

                            objPaseCFS.EXPRESS = false;
                            objPaseCFS.IV_USUARIO_CREA = ClsUsuario.loginname;

                            tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem;
                            tablePagination_Tarja.DataBind();


                            grilla.DataSource = objPaseCFS.DetallePaseFacturas;
                            grilla.DataBind();

                            this.UPDETALLEFACTURAS.Update();
                            this.UPCARGA.Update();

                            Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                            this.Pintar_Grilla();

                            BtnAgregar_Click(sender, e);
                        }
                        else
                        {
                            grilla.DataSource = null;
                            grilla.DataBind();

                            this.UPDETALLEFACTURAS.Update();
                            this.UPCARGA.Update();

                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con la factura {0} seleccionada", txtCustomer.Text));
                            return;

                        }
                    }
                    else
                    {
                        txtCustomer.Text = "";
                    }
                }
                
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "gvCustomers_SelectedIndexChanged", "Hubo un error al seleccionar factura", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }

        }

        #endregion



        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        //agregar transportista
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


                    if (string.IsNullOrEmpty(Txtempresa.Text))
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

                    var LinqValidaSubItemsFaltantes = (from p in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true )
                                                       select p.CONSECUTIVO).ToList();

                    if (LinqValidaSubItemsFaltantes.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen subitems pendientes para realizar pases de puerta CFS. </b>"));
                        return;
                    }

                   
                   

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //valida que exista Empresa Transporte 
                    if (!string.IsNullOrEmpty(Txtempresa.Text) )
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
                    string Tarjas = string.Join(",", objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true ).Select(kvp => kvp.CONSECUTIVO));
                 
                    //actualizado datos de chofer y subitems seleccionados, por cada vez que se agrega 
                    foreach (var Det in objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true))
                    {
                        ConsecutivoSelec = Det.CONSECUTIVO.Value;
                        var Detalle = objPaseCFS.DetalleSubItem.FirstOrDefault(f => f.CONSECUTIVO.Equals(ConsecutivoSelec));
                        if (Detalle != null)
                        {
                          
                            Detalle.CIATRANS = EmpresaSelect;
                            Detalle.CHOFER =  ChoferSelect;
                            Detalle.PLACA = PlacaSelect;
                            Detalle.ID_CIATRANS =  IdEmpresa;
                            Detalle.ID_CHOFER =IdChofer;
                            Detalle.TRANSPORTISTA_DESC =  DesEmpresa;
                            Detalle.CHOFER_DESC =DesChofer;
                            Detalle.MARCADO_SUBITEMS = Tarjas;
                        }
                    }

                    tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem;
                    tablePagination_Tarja.DataBind();


                    objPaseCFS.Detalle.Clear();

                    int i = 0;
                    foreach (var Det in objPaseCFS.DetallePaseFacturas)
                    {
                        objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();
                        objDetallePaseCFS.FECHA = Det.FECHA;
                        objDetallePaseCFS.MRN = Det.MRN;
                        objDetallePaseCFS.MSN = Det.MSN;
                        objDetallePaseCFS.HSN = Det.HSN;
                        objDetallePaseCFS.IV_USUARIO_CREA = Det.IV_USUARIO_CREA;
                        objDetallePaseCFS.SESION = Det.SESION;

                        objDetallePaseCFS.FACTURA = Det.FACTURA;
                        objDetallePaseCFS.CARGA = Det.CARGA;
                        objDetallePaseCFS.AGENTE = Det.AGENTE;
                        objDetallePaseCFS.FACTURADO = Det.FACTURADO;
                        objDetallePaseCFS.PAGADO = Det.PAGADO;
                        objDetallePaseCFS.GKEY = Det.GKEY;
                        objDetallePaseCFS.REFERENCIA = Det.REFERENCIA;
                        objDetallePaseCFS.CONTENEDOR = Det.CONTENEDOR;
                        objDetallePaseCFS.DOCUMENTO = Det.DOCUMENTO;
                        objDetallePaseCFS.PRIMERA = Det.PRIMERA;
                        objDetallePaseCFS.MARCA = Det.MARCA;
                        objDetallePaseCFS.CANTIDAD = Det.CANTIDAD;
                        objDetallePaseCFS.CIATRANS = Det.CIATRANS;
                        objDetallePaseCFS.CHOFER = Det.CHOFER;
                        objDetallePaseCFS.PLACA = Det.PLACA;
                        objDetallePaseCFS.FECHA_SALIDA = Det.FECHA_SALIDA;
                        objDetallePaseCFS.CNTR_DD = Det.CNTR_DD;
                        objDetallePaseCFS.AGENTE_DESC = Det.AGENTE_DESC;
                        objDetallePaseCFS.FACTURADO_DESC = Det.FACTURADO_DESC;
                        objDetallePaseCFS.IMPORTADOR = Det.IMPORTADOR;
                        objDetallePaseCFS.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;
                        objDetallePaseCFS.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                        objDetallePaseCFS.FECHA_AUT_PPWEB = Det.FECHA_AUT_PPWEB;
                        objDetallePaseCFS.HORA_AUT_PPWEB = Det.HORA_AUT_PPWEB;
                        objDetallePaseCFS.TIPO_CNTR = Det.TIPO_CNTR;
                        //objDetallePaseCFS.ID_TURNO = Det.ID_TURNO;
                        //objDetallePaseCFS.TURNO = Det.TURNO;
                        //objDetallePaseCFS.D_TURNO = Det.D_TURNO;
                        objDetallePaseCFS.ID_PASE = Det.ID_PASE;
                        objDetallePaseCFS.ESTADO = Det.ESTADO;
                        objDetallePaseCFS.ENVIADO = Det.ENVIADO;
                        objDetallePaseCFS.AUTORIZADO = Det.AUTORIZADO;
                        objDetallePaseCFS.VENTANILLA = Det.VENTANILLA;
                        objDetallePaseCFS.USUARIO_ING = Det.USUARIO_ING;
                        objDetallePaseCFS.FECHA_ING = Det.FECHA_ING;
                        objDetallePaseCFS.USUARIO_MOD = Det.USUARIO_MOD;
                        objDetallePaseCFS.ESTADO_PAGO = Det.ESTADO_PAGO;
                        objDetallePaseCFS.ID_PPWEB = Det.ID_PPWEB;

                        //total de bultos
                        var TotalBultosSelect = objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true  && x.CARGA.Equals(Det.CARGA)).Sum(p => p.CANTIDAD);

                        var xTarjas = string.Join(",", objPaseCFS.DetalleSubItem.Where(x => x.VISTO == true  && x.CARGA.Equals(Det.CARGA)).Select(kvp => kvp.CONSECUTIVO));

                        objDetallePaseCFS.CANTIDAD = TotalBultosSelect;


                        objDetallePaseCFS.ID_CIATRANS = IdEmpresa;
                        objDetallePaseCFS.ID_CHOFER = IdChofer;
                        objDetallePaseCFS.CIATRANS = EmpresaSelect;
                        objDetallePaseCFS.CHOFER = ChoferSelect;
                        objDetallePaseCFS.PLACA = PlacaSelect;
                        objDetallePaseCFS.TRANSPORTISTA_DESC = DesEmpresa;
                        objDetallePaseCFS.CHOFER_DESC = DesChofer;


                        objDetallePaseCFS.SUB_SECUENCIA = xTarjas;
                        objDetallePaseCFS.LLAVE = xTarjas;
                        objDetallePaseCFS.ID_UNIDAD = objPaseCFS.ID_UNIDAD;

                        objDetallePaseCFS.ID_ZONA = Det.ID_ZONA;
                        objDetallePaseCFS.ID_CIUDAD = Det.ID_CIUDAD;
                        objDetallePaseCFS.DIRECCION = Det.DIRECCION;
                    

                    
                        objPaseCFS.Detalle.Add(objDetallePaseCFS);

                        i++;
                    }

                    //si exsiten cantidades pendientes o seleccionadas
                    if (i != 0)
                    {
                        
                    }
                    else
                    {
                        var cargas = string.Join(",", objPaseCFS.DetallePaseFacturas.Select(kvp => kvp.CARGA));

                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: No se pudo presentar por pantalla el detalle de pases a emitir: {0}", cargas);

                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Problemas al presentar detalle de pases a emitir CFS", MensajesErrores.Trim(), cargas,
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


                        foreach (var Det in objPaseCFS.DetallePaseFacturas)
                        {
                            //verificar si es pase sin turno
                            var PaseSinturno = Pase_CFS.EsPaseSinTurno(ClsUsuario.ruc, Det.MRN, Det.MSN, Det.HSN);
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
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se pudo obtener datos de pase sin turno, de la carga: {0}...mensaje problema: {1} </b>", Det.CARGA, PaseSinturno.MensajeProblema));
                                return;
                            }
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

                            var Turnos = PasePuerta.TurnoCFS.ObtenerTurnos(ClsUsuario.ruc, FechaActualSalida, Lista, false, objPaseCFS.EXPRESS);
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

                   
                    this.Actualiza_Paneles();

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAgregar_Click), "emisionpasecfs_multi", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));

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

                    

                    //instancia sesion
                    objPaseCFS = Session["PaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar los pase a puerta de carga suelta multidespacho. </b>"));
                        return;
                    }

                    if (objPaseCFS.DetallePaseFacturas.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la factura a emitir pase de puerta multidespacho. </b>"));
                        return;
                    }

                    if (objPaseCFS.DetalleSubItem.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bultos o subitems, poder generar los pase a puerta de carga suelta multidespacho. </b>"));
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
                    string carga_sin_turno = string.Empty;
                    foreach (var Pase in objPaseCFS.Detalle)
                    {
                        carga_sin_turno = string.Format("{0}-{1}-{2}", Pase.MRN, Pase.MSN, Pase.HSN);

                        var PaseSinturno = Pase_CFS.EsPaseSinTurno(LoginName, Pase.MRN, Pase.MSN, Pase.HSN);
                        if (PaseSinturno.Exitoso)
                        {
                            if (PaseSinturno.Resultado)
                            {
                                EsPasesinTurno = true;
                                break;
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
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se puede generar pases de cargassin turno, de la carga: {0}. </b>", carga_sin_turno));
                        return;
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
                                         ID_CIUDAD = Tbl.ID_CIUDAD,
                                         ID_ZONA = Tbl.ID_ZONA,
                                         DIRECCION = Tbl.DIRECCION,

                                     }).ToList().OrderBy(x => x.CONTENEDOR);


                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/

                    string token = string.Empty;
                   
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
                       
                        pase.ID_CIUDAD = Det.ID_CIUDAD;
                        pase.ID_ZONA = Det.ID_ZONA;
                        pase.DIRECCION = Det.DIRECCION;
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

                        Int64 IV_ID = Int64.Parse(hfCustomerId.Value);

                        var Resultado = pase.Insertar_MultiDespacho_Transporte(IV_ID, Det.TURNO.Value, Det.CONTENEDOR, Det.SUB_SECUENCIA,Det.BULTOS_HORARIOS.Value,Det.MRN, Det.MSN, Det.HSN, Det.CNTR_DD);
                        if (Resultado.Exitoso)
                        {
                            nTotal++;
                            if (nTotal == 1)
                            {
                                id_carga = securetext(string.Format("9025{0}", IV_ID.ToString("D8")));
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
                        string link = string.Format("<a href='../p2d/p2d_imprimirpase_freightforwarder.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta FREIGHT FORWARDER</a>", id_carga);

                        //limpiar
                        objPaseCFS.DetallePaseFacturas.Clear();
                        objPaseCFS.Detalle.Clear();
                        objPaseCFS.DetalleSubItem.Clear();

                        Session["PaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                       

                        this.tablePagination_Tarja.DataSource = objPaseCFS.DetalleSubItem;
                        tablePagination_Tarja.DataBind();

                        tablePagination.DataSource = objPaseCFS.Detalle;
                        tablePagination.DataBind();

                        grilla.DataSource = objPaseCFS.DetallePaseFacturas;
                        grilla.DataBind();

                        this.CboFacturas();

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

#if !DEBUG
                this.IsAllowAccess();
#endif

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

                
                Server.HtmlEncode(this.Txtempresa.Text.Trim());
                Server.HtmlEncode(this.TxtChofer.Text.Trim());
                Server.HtmlEncode(this.TxtPlaca.Text.Trim());
                Server.HtmlEncode(this.TxtFechaHasta.Text.Trim());
               
                
                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                   

                    this.CboFacturas();

                   

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}