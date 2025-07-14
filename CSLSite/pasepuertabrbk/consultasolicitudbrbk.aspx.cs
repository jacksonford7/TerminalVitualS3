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
  

    public partial class consultasolicitudbrbk : System.Web.UI.Page
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

        private brbk_solicitud_pendiente objCab = new brbk_solicitud_pendiente();
        private brbk_solicitud_pendiente_det objDet = new brbk_solicitud_pendiente_det();

        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();


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
            //UPCANTIDADRETIRAR.Update();
            
            UPBOTONES.Update();
            UPCONTENEDOR.Update();
            UPRETIRADOS.Update();
            UPSALDO.Update();
            UPBODEGA.Update();
            UPPRODUCTO.Update();
            //UPFECHASALIDA.Update();
            //UPTURNO.Update();
            UPCAS.Update();
           // UPAGREGATURNO.Update();
            this.UPPAGADO.Update();
            //UPVEHICULOS.Update();
            //UPCANTIDADRETIRAR.Update();
           // this.UPTOTALBULTOS.Update();
            this.UPTEXTO.Update();
            this.UPTITULO.Update();
           // this.UPTIPOHORARIO.Update();
        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
            this.TXTMSN.Text = string.Empty;
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                this.TXTHSN.Text = string.Format("{0}", "0000");
            }
           // this.Txtempresa.Text = string.Empty;
          
            this.TxtPagado.Text = string.Empty;
          //  this.TxtFechaHasta.Text = string.Empty;
            this.TxtContenedorSeleccionado.Text = string.Empty;
            this.TxtRetirados.Text = string.Empty;
            this.TxtSaldo.Text = string.Empty;
            this.TxtBodega.Text = string.Empty;
            this.TxtTipoProducto.Text = string.Empty;
            //this.TxtCantidadRetirar.Text = string.Empty;
            //this.TxtNumeroVehiculos.Text = string.Empty;

          
            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();

            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });

            //this.CboTurnos.DataSource = List_Turnos;
            //this.CboTurnos.DataTextField = "Turno";
            //this.CboTurnos.DataValueField = "IdPlan";
            //this.CboTurnos.DataBind();

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

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg_lista.Visible = true;
            this.banmsg_lista.InnerHtml = Mensaje;       
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
            Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
        }

        private void Turno_Default()
        {
            //turno por defecto
            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Seleccione *" });
            //this.CboTurnos.DataSource = List_Turnos;
            //this.CboTurnos.DataTextField = "Turno";
            //this.CboTurnos.DataValueField = "IdPlan";
            //this.CboTurnos.DataBind();
        }

        private void Pase_Sin_Turno_Default()
        {
            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Pase Sin turno *" });
            //this.CboTurnos.DataSource = List_Turnos;
            //this.CboTurnos.DataTextField = "Turno";
            //this.CboTurnos.DataValueField = "IdPlan";
            //this.CboTurnos.DataBind();
        }


        //private void Carga_Turnos()
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {

        //            CultureInfo enUS = new CultureInfo("en-US");

        //            this.Ocultar_Mensaje();

        //            List_Turnos = new List<Cls_Bil_Turnos>();
        //            List_Turnos.Clear();

        //            if (string.IsNullOrEmpty(TxtFechaHasta.Text))
        //            {
        //                this.Turno_Default();
        //                this.Actualiza_Paneles();
        //                return;
        //            }


        //            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
        //            objPaseBRBK = Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
        //            if (objPaseBRBK == null)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar la fecha del turno... </b>"));
        //                return;
        //            }
        //            if (objPaseBRBK.CNTR_CANTIDAD_SALDO == 0)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe saldo de bultos disponibles, para poder seleccionar la fecha del turno... </b>"));
        //                return;
        //            }

        //            HoraHasta = "00:00";
        //            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
        //            if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
        //            {
        //                this.Turno_Default();
        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase.. Mes/Día/año</b>"));
        //                this.TxtFechaHasta.Focus();
        //                return;
        //            }

        //            FechaFacturaHasta = objPaseBRBK.FECHA_SALIDA.Value;

        //            if (FechaActualSalida.Date < System.DateTime.Now.Date)
        //            {
        //                this.Turno_Default();
        //                // this.TxtFechaHasta.Text = objPaseBRBK.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), System.DateTime.Now.ToString("MM/dd/yyyy")));
        //                this.TxtFechaHasta.Focus();
        //                return;
        //            }

        //            if (FechaActualSalida.Date > FechaFacturaHasta.Date)
        //            {
        //                this.Turno_Default();
        //                //this.TxtFechaHasta.Text = objPaseBRBK.FECHA_SALIDA.Value.ToString("MM/dd/yyyy");
        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM/dd/yyyy"), FechaFacturaHasta.ToString("MM/dd/yyyy")));
        //                this.TxtFechaHasta.Focus();
        //                return;
        //            }

        //            Int64 ID_TIPO = 0;

        //            if (this.CboTipoTurno.SelectedIndex != -1)
        //            {
        //                ID_TIPO = Int64.Parse(this.CboTipoTurno.SelectedValue.ToString());
        //            }


        //            var Turnos = PasePuerta.TurnoBRBK.ObtenerTurnosSolicitud(ClsUsuario.ruc, FechaActualSalida, ID_TIPO);
        //            if (Turnos.Exitoso)
        //            {
        //                //turno por defecto
        //                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "", "0"), Turno = "* Seleccione *" });
        //                var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
        //                                 select new
        //                                 {
        //                                     IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm"), (Tbl.Bultos == null ? 0 : Tbl.Bultos)),
        //                                     Turno = string.Format("{0}", Tbl.Turno)
        //                                 }).ToList().OrderBy(x => x.Turno);

        //                foreach (var Items in LinqQuery)
        //                {
        //                    List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
        //                }

        //                this.CboTurnos.DataSource = List_Turnos;
        //                this.CboTurnos.DataTextField = "Turno";
        //                this.CboTurnos.DataValueField = "IdPlan";
        //                this.CboTurnos.DataBind();

        //            }
        //            else
        //            {
        //                this.Turno_Default();
        //                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
        //                return;

        //            }



        //            this.Actualiza_Paneles();

        //        }
        //        catch (Exception ex)
        //        {

        //            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Turnos), "Carga_Turnos", false, null, null, ex.StackTrace, ex);
        //            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //            this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
        //            return;

        //        }
        //    }

        //}

        private void Carga_Tipo_Turnos()
        {
            if (Response.IsClientConnected)
            {
                try
                {

                   // List<brbk_tipo_turnos> Listado = brbk_tipo_turnos.CboTipoTurnos(out cMensajes);

                    //this.CboTipoTurno.DataSource = Listado;
                    //this.CboTipoTurno.DataTextField = "TIPO";
                    //this.CboTipoTurno.DataValueField = "ID";
                    //this.CboTipoTurno.DataBind();

                    this.Actualiza_Paneles();

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Tipo_Turnos), "Carga_Tipo_Turnos", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                    return;

                }
            }

        }

        private void Cargar_Solicitud_Pendientes()
        {
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");

                if (!string.IsNullOrEmpty(TxtFechaConDesde.Text))
                {
                    if (!DateTime.TryParseExact(TxtFechaConDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                    {
                       
                    }
                }
                if (!string.IsNullOrEmpty(TxtFechaConHasta.Text))
                {
                    if (!DateTime.TryParseExact(TxtFechaConHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                    {

                    }
                }


                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Tabla = brbk_solicitud_pendiente.Solicitud_Pendientes_Todas(fechadesde, fechahasta, out cMensajes);
                if (Tabla == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de solicitudes. {0}", cMensajes));
                    return;
                }
                if (Tabla.Count <= 0)
                {
                    grilla.DataSource = null;
                    grilla.DataBind();

                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de solicitudes."));
                    return;
                }

                grilla.DataSource = Tabla;
                grilla.DataBind();

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Cargar_Solicitud_Pendientes), "Cargar_Solicitud_Pendientes", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));



            }
        }

      
        #endregion



        #region "Eventos del Formulario"



        #region "Eventos del turno"
        //protected void CboTipoTurno_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {

        //            CultureInfo enUS = new CultureInfo("en-US");

        //            this.Ocultar_Mensaje();

        //            List_Turnos = new List<Cls_Bil_Turnos>();
        //            List_Turnos.Clear();

        //            if (string.IsNullOrEmpty(TxtFechaHasta.Text))
        //            {
        //                this.Turno_Default();
        //                this.Actualiza_Paneles();
        //                return;
        //            }


        //            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
        //            objPaseBRBK = Session["SolicitudBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
        //            if (objPaseBRBK == null)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar el tipo de turno... </b>"));
        //                return;
        //            }


        //            HoraHasta = "00:00";
        //            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
        //            if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
        //            {
        //                this.Turno_Default();
        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase.. Mes/Día/año</b>"));
        //                this.TxtFechaHasta.Focus();
        //                return;
        //            }

        //            FechaFacturaHasta = objPaseBRBK.FECHA_SALIDA.Value;



        //            Int64 ID_TIPO = 0;

        //            if (this.CboTipoTurno.SelectedIndex != -1)
        //            {
        //                ID_TIPO = Int64.Parse(this.CboTipoTurno.SelectedValue.ToString());
        //            }


        //            var Turnos = PasePuerta.TurnoBRBK.ObtenerTurnosSolicitud(ClsUsuario.ruc, FechaActualSalida, ID_TIPO);
        //            if (Turnos.Exitoso)
        //            {
        //                //turno por defecto
        //                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "", "0"), Turno = "* Seleccione *" });
        //                var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
        //                                 select new
        //                                 {
        //                                     IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm"), (Tbl.Bultos == null ? 0 : Tbl.Bultos)),
        //                                     Turno = string.Format("{0}", Tbl.Turno)
        //                                 }).ToList().OrderBy(x => x.Turno);

        //                foreach (var Items in LinqQuery)
        //                {
        //                    List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
        //                }

        //                this.CboTurnos.DataSource = List_Turnos;
        //                this.CboTurnos.DataTextField = "Turno";
        //                this.CboTurnos.DataValueField = "IdPlan";
        //                this.CboTurnos.DataBind();

        //            }
        //            else
        //            {
        //                this.Turno_Default();
        //                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, Turnos.MensajeProblema));
        //                return;

        //            }



        //            this.Actualiza_Paneles();

        //        }
        //        catch (Exception ex)
        //        {

        //            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(CboTipoTurno_SelectedIndexChanged), "CboTipoTurno_SelectedIndexChanged", false, null, null, ex.StackTrace, ex);
        //            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //            this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
        //            return;

        //        }
        //    }


        //}

        //protected void TxtFechaHasta_TextChanged(object sender, EventArgs e)
        //{

        //    this.Carga_Turnos();
        //}

        ////APROBAR TURNO
        //protected void BtnAgregaTruno_Click(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {
        //            string Tarjas = string.Empty;
        //            int TotalBultos = 0;
                    
        //            string MRN = string.Empty;
        //            string MSN = string.Empty;
        //            string HSN = string.Empty;
        //            string BODEGA = string.Empty;

        //            string IdEmpresa = string.Empty;
        //            string DesEmpresa = string.Empty;
        //            string IdChofer = string.Empty;
        //            string DesChofer = string.Empty;

        //            CultureInfo enUS = new CultureInfo("en-US");

        //            if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                Session.Clear();
        //                OcultarLoading("1");
        //                return;
        //            }

        //            this.Ocultar_Mensaje();

        //            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
        //            objPaseBRBK = Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
        //            if (objPaseBRBK == null)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la solicitud, para poder aprobar un turno... </b>"));
        //                return;
        //            }

        //            string SECUENCIA = this.TxtSecuencia.Text.Trim();
        //            if (string.IsNullOrEmpty(SECUENCIA))
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el turno a realizar la aprobación... </b>"));
        //                return;

        //            }

        //            int NPASES = 0;
        //            if (!int.TryParse(this.TxtNumeroVehiculos.Text, out NPASES))
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar los vehículos a utilizar... </b>"));
        //                this.TxtNumeroVehiculos.Focus();
        //                return;
        //            }

                  
        //            if (NPASES == 0)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar los vehículos a utilizar... </b>"));
        //                this.TxtNumeroVehiculos.Focus();
        //                return;
        //            }


        //            int NBULTOS = 0;
        //            if (!int.TryParse(this.TxtCantidadRetirar.Text, out NBULTOS))
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar la cantidad de bultos por vehículo... </b>"));
        //                this.TxtTotalBultosRetirar.Focus();
        //                return;
        //            }


        //            if (NBULTOS == 0)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar la cantidad de bultos por vehículo... </b>"));
        //                this.TxtCantidadRetirar.Focus();
        //                return;
        //            }


        //            int NTOTALBULTOS = 0;
        //            if (!int.TryParse(this.TxtTotalBultosRetirar.Text, out NTOTALBULTOS))
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar la cantidad de bultos por vehículo... </b>"));
        //                this.TxtTotalBultosRetirar.Focus();
        //                return;
        //            }


        //            if (NTOTALBULTOS == 0)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar la cantidad de bultos por vehículo... </b>"));
        //                this.TxtTotalBultosRetirar.Focus();
        //                return;
        //            }

        //            int NSALDO_PENDIENTE = 0;
        //            if (!int.TryParse(this.TxtSaldo.Text, out NSALDO_PENDIENTE))
        //            {
        //                NSALDO_PENDIENTE = 0;
        //            }

        //            if (this.CboTurnos.SelectedIndex == 0)
        //            {
        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un turno para poder agregar la información </b>"));
        //                this.CboTurnos.Focus();
        //                return;
        //            }
        //            else
        //            {
        //                TurnoSelect = this.CboTurnos.SelectedValue;
        //            }
                   
                    

        //            HoraHasta = "00:00";
        //            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
        //            if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
        //            {
        //                this.Turno_Default();
        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la solicitud del turno.. Mes/Día/año</b>"));
        //                this.TxtFechaHasta.Focus();
        //                return;
        //            }

                   
        //            string FechaIni = TurnoSelect.Split('-').ToList()[2].Trim();
        //            if (!DateTime.TryParseExact(FechaIni, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaTurnoInicio))
        //            {

        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de inicio del turno seleccionado no es valida, Mes/Día/año </b>"));
        //                this.CboTurnos.Focus();
        //                return;
        //            }
        //            string FechaFin = TurnoSelect.Split('-').ToList()[3].Trim();
        //            if (!DateTime.TryParseExact(FechaFin, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaTurnoFinal))
        //            {

        //                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha final del turno seleccionado no es valida, Mes/Día/año </b>"));
        //                this.CboTurnos.Focus();
        //                return;
        //            }

        //            //validar stock
        //            var Saldo = PasePuerta.SaldoCargaBRBK.SaldoPendienteBRBK(ClsUsuario.ruc, objPaseBRBK.CARGA);
        //            if (Saldo.Exitoso)
        //            {
        //                var LinqQuery = (from Tbl in Saldo.Resultado.Where(p => !string.IsNullOrEmpty(p.NUMERO_CARGA))
        //                                 select new
        //                                 {
        //                                     SALDO_FINAL = Tbl.SALDO_FINAL

        //                                 }).FirstOrDefault();

        //                if (LinqQuery != null)
        //                {
        //                    if (NTOTALBULTOS > LinqQuery.SALDO_FINAL)
        //                    {
        //                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede retirar la cantidad de: {0}, el saldo pendiente es de: {1}, se excede el stock</b>", NTOTALBULTOS, LinqQuery.SALDO_FINAL));
        //                        //this.TxtCantidadRetirar.Text = LinqQuery.SALDO_FINAL.ToString();
        //                        this.TxtCantidadRetirar.Focus();
        //                        return;
        //                    }

        //                    TotalBultos = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0 && !x.LLAVE.Equals(SECUENCIA) &&  !x.ESTADO.Equals("R") && !x.ESTADO.Equals("A")).Sum(x => x.CANTIDAD.Value) + NTOTALBULTOS;

        //                    int TotalDetalle = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0 && !x.LLAVE.Equals(SECUENCIA) && !x.ESTADO.Equals("R")).Sum(x => x.CANTIDAD.Value);


        //                    if (TotalBultos > LinqQuery.SALDO_FINAL)
        //                    {
        //                        int nSbubtotal = (LinqQuery.SALDO_FINAL - TotalDetalle);
        //                        int nSaldoFin = (nSbubtotal < 0 ? 0 : nSbubtotal);

        //                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede retirar la cantidad de: {0}, el saldo pendiente es de: {1}, se excede el stock</b>", NTOTALBULTOS, nSaldoFin));
        //                        //this.TxtCantidadRetirar.Text = nSaldoFin.ToString();
        //                        this.TxtCantidadRetirar.Focus();
        //                        return;
        //                    }

        //                    int TotalPases = objPaseBRBK.Detalle.Where(x => x.CANTIDAD_VEHICULOS.Value != 0 && !x.LLAVE.Equals(SECUENCIA) && !x.ESTADO.Equals("R")).Sum(x => x.CANTIDAD_VEHICULOS.Value) + NPASES;
        //                    int nPendiente = NSALDO_PENDIENTE - TotalBultos;

        //                    this.text_detalle.InnerHtml = string.Format("TURNOS APROBADOS:  &nbsp;&nbsp;&nbsp;&nbsp;T/PASES: {0}  &nbsp;&nbsp;&nbsp;&nbsp;T/BULTOS: {1} &nbsp;&nbsp;&nbsp;&nbsp;S/PENDIENTE: {2}", TotalPases, TotalBultos, nPendiente);
        //                }
        //                else
        //                {
        //                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No puede validar stock...No existe información del saldo de la carga...</b>"));
        //                    this.TxtFechaHasta.Focus();
        //                    return;
        //                }
        //            }
        //            else
        //            {

        //                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No puede validar stock...No existe información del saldo de la carga...</b>"));
        //                this.TxtFechaHasta.Focus();
        //                return;
        //            }

        //            string CIATRANS = string.Empty;
        //            string ID_CIATRANS = string.Empty;
        //            string TRANSPORTISTA_DESC = string.Empty;

        //            //recorre para actualizar datos
        //            var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(SECUENCIA));
        //            if (Detalle != null)
        //            {

                        
        //                //ejecuta proceso de autorizacion a la base de datos
        //                brbk_solicitud_pendiente Update = new brbk_solicitud_pendiente();
        //                Update.ID_SOL = Int64.Parse(Detalle.ID_PASE.ToString());
        //                Update.SECUENCIA = Int64.Parse(Detalle.LLAVE);
        //                Update.FECHA_EXPIRACION = FechaActualSalida;
        //                Update.CANTIDAD_CARGA = NBULTOS;
        //                Update.CANTIDAD_VEHICULOS = NPASES;
        //                Update.TOTAL_CARGA = NTOTALBULTOS;
        //                Update.ID_HORARIO = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());//ID TURNO
        //                Update.HORARIO = this.CboTurnos.SelectedItem.ToString().Substring(0, 5); 
        //                Update.USUARIO_ESTADO = ClsUsuario.loginname;

        //                var nProceso = Update.SaveTransaction_Aprobar(out cMensajes);
        //                if (!nProceso.HasValue || nProceso.Value <= 0)
        //                {
        //                    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo aprobar el turno {0} - {1}...{2}</b>", FechaActualSalida.ToString("dd/MM/yyyy"), this.CboTurnos.SelectedItem.ToString().Substring(0, 5) , cMensajes));
        //                    return;
        //                }
        //                //fin proceso

        //                Detalle.CANTIDAD = NTOTALBULTOS;

        //                Detalle.D_TURNO = this.CboTurnos.SelectedItem.ToString().Substring(0, 5);
        //                Detalle.TURNO = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());//ID TURNO
        //                Detalle.ID_TURNO = Convert.ToInt16(TurnoSelect.Split('-').ToList()[1].Trim());//secuencia turno
        //                Detalle.TURNO_DESDE = FechaTurnoInicio;
        //                Detalle.TURNO_HASTA = FechaTurnoFinal;
        //                Detalle.BULTOS_HORARIOS = NTOTALBULTOS;

        //                HoraHasta = Detalle.D_TURNO;
        //                Fecha = string.Format("{0} {1}", FechaActualSalida.Date.ToString("MM/dd/yyyy"), HoraHasta);
        //                if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
        //                {
        //                    this.Turno_Default();
        //                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para poder agregar la información del turno, Mes/Día/año </b>"));
        //                    this.TxtFechaHasta.Focus();
        //                    return;
        //                }

        //                Detalle.FECHA_SALIDA_PASE = FechaActualSalida;

        //                Detalle.ESTADO = "A";

        //                Detalle.CANTIDAD_VEHICULOS = NPASES;
        //                Detalle.CANTIDAD_BULTOS = NBULTOS;
        //                Detalle.SUB_SECUENCIA = SECUENCIA;
        //                Detalle.ESTADO_PASE = "APROBADA";


        //                this.TxtSecuencia.Text = string.Empty;
        //                this.TxtFechaHasta.Text = string.Empty;
        //                this.TxtNumeroVehiculos.Text = string.Empty;
        //                this.TxtCantidadRetirar.Text = string.Empty;
        //                this.TxtTotalBultosRetirar.Text = string.Empty;
        //                this.Turno_Default();



        //            } 
                    

        //            tablePagination.DataSource = objPaseBRBK.Detalle;
        //            tablePagination.DataBind();

        //            Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

        //            //actualiar solicitudes
        //            this.Cargar_Solicitud_Pendientes();


        //            this.Actualiza_Paneles();


                  

        //        }
        //        catch (Exception ex)
        //        {
        //            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAgregaTruno_Click), "BtnAgregaTruno_Click", false, null, null, ex.StackTrace, ex);
        //            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //            this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar turno valido.. {0} </b>", OError));
        //        }
               
        //    }
        //}
        #endregion

        //protected void TxtNumeroVehoiculos_TextChanged(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {


        //            if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                Session.Clear();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                return;
        //            }

        //            Int64 NUM_VEHICULOS = 0;

        //            if (!Int64.TryParse(TxtNumeroVehiculos.Text, out NUM_VEHICULOS))
        //            {
        //                NUM_VEHICULOS = 0;
        //            }

        //            Int64 NUM_RETIRAR = 0;

        //            if (!Int64.TryParse(TxtCantidadRetirar.Text, out NUM_RETIRAR))
        //            {
        //                NUM_RETIRAR = 0;
        //            }

        //            this.TxtTotalBultosRetirar.Text = (NUM_VEHICULOS * NUM_RETIRAR).ToString();


        //            this.Actualiza_Paneles();


        //        }
        //        catch (Exception ex)
        //        {
        //            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(TxtNumeroVehoiculos_TextChanged), "TxtNumeroVehoiculos_TextChanged", false, null, null, ex.StackTrace, ex);
        //            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

        //        }
        //    }
        //}

        //protected void TxtCantidadRetirar_TextChanged(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {


        //            if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                Session.Clear();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                return;
        //            }

        //            Int64 NUM_VEHICULOS = 0;

        //            if (!Int64.TryParse(TxtNumeroVehiculos.Text, out NUM_VEHICULOS))
        //            {
        //                NUM_VEHICULOS = 0;
        //            }

        //            Int64 NUM_RETIRAR = 0;

        //            if (!Int64.TryParse(TxtCantidadRetirar.Text, out NUM_RETIRAR))
        //            {
        //                NUM_RETIRAR = 0;
        //            }

        //            this.TxtTotalBultosRetirar.Text = (NUM_VEHICULOS * NUM_RETIRAR).ToString();


        //            this.Actualiza_Paneles();


        //        }
        //        catch (Exception ex)
        //        {
        //            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(TxtCantidadRetirar_TextChanged), "TxtCantidadRetirar_TextChanged", false, null, null, ex.StackTrace, ex);
        //            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

        //        }
        //    }
        //}

   
       
        #region "Eventos de la grilla de solicitud de puerta break bulk"

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

                    if (e.CommandName == "Ver")
                    {
                        objPaseBRBK = Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));

                        if (Detalle != null)
                        {
                            //this.TxtSecuencia.Text = Detalle.LLAVE;
                            //FechaActualSalida = Detalle.FECHA_SALIDA_PASE.Value;
                            //this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");

                            //this.TxtNumeroVehiculos.Text = Detalle.CANTIDAD_VEHICULOS.ToString();
                            //this.TxtCantidadRetirar.Text = Detalle.CANTIDAD_BULTOS.ToString();
                            //this.TxtTotalBultosRetirar.Text = Detalle.CANTIDAD.ToString();

                            //this.Carga_Turnos();

                            //try
                            //{
                            //    string IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", Detalle.TURNO.Value, Detalle.TURNO.Value, Detalle.TURNO_DESDE.Value.ToString("yyyy/MM/dd HH:mm"), Detalle.TURNO_HASTA.Value.ToString("yyyy/MM/dd HH:mm"), 0);
                            //    this.CboTurnos.SelectedValue = IdPlan;
                            //}
                            //catch (Exception)
                            //{

                            //}

                        }

                        this.Actualiza_Paneles();

                    }

                    if (e.CommandName == "Aprobar")
                    {
                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objPaseBRBK = Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));

                        if (Detalle != null)
                        {
                            int NSALDO_PENDIENTE = 0;
                            if (!int.TryParse(this.TxtSaldo.Text, out NSALDO_PENDIENTE))
                            {
                                NSALDO_PENDIENTE = 0;
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
                                    if (Detalle.CANTIDAD > LinqQuery.SALDO_FINAL)
                                    {
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede retirar la cantidad de: {0}, el saldo pendiente es de: {1}, se excede el stock</b>", Detalle.CANTIDAD, LinqQuery.SALDO_FINAL));
                                        return;
                                    }

                                    int TotalBultos = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0 && !x.ESTADO.Equals("R") && !x.ESTADO.Equals("A")).Sum(x => x.CANTIDAD.Value);

                                    int TotalDetalle = objPaseBRBK.Detalle.Where(x => x.CANTIDAD.Value != 0 && !x.ESTADO.Equals("R")).Sum(x => x.CANTIDAD.Value);


                                    if (TotalBultos > LinqQuery.SALDO_FINAL)
                                    {
                                        int nSbubtotal = (LinqQuery.SALDO_FINAL - TotalDetalle);
                                        int nSaldoFin = (nSbubtotal < 0 ? 0 : nSbubtotal);

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No puede retirar la cantidad de: {0}, el saldo pendiente es de: {1}, se excede el stock</b>", Detalle.CANTIDAD, nSaldoFin));
                                        
                                        return;
                                    }

                                    int TotalPases = objPaseBRBK.Detalle.Where(x => x.CANTIDAD_VEHICULOS.Value != 0  && !x.ESTADO.Equals("R")).Sum(x => x.CANTIDAD_VEHICULOS.Value) ;
                                    int nPendiente = NSALDO_PENDIENTE - TotalBultos;

                                    this.text_detalle.InnerHtml = string.Format("TURNOS AGREGADOS:  &nbsp;&nbsp;&nbsp;&nbsp;T/PASES: {0}  &nbsp;&nbsp;&nbsp;&nbsp;T/BULTOS: {1} &nbsp;&nbsp;&nbsp;&nbsp;S/PENDIENTE: {2}", TotalPases, TotalBultos, nPendiente);
                                }
                                else
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No puede validar stock...No existe información del saldo de la carga...</b>"));
                                    return;
                                }
                            }



                            //this.TxtSecuencia.Text = Detalle.LLAVE;
                            //FechaActualSalida = Detalle.FECHA_SALIDA_PASE.Value;
                            //this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");

                            //this.TxtNumeroVehiculos.Text = Detalle.CANTIDAD_VEHICULOS.ToString();
                            //this.TxtCantidadRetirar.Text = Detalle.CANTIDAD_BULTOS.ToString();
                            //this.TxtTotalBultosRetirar.Text = Detalle.CANTIDAD.ToString();

                            ////ejecuta proceso de autorizacion a la base de datos
                            //brbk_solicitud_pendiente Update = new brbk_solicitud_pendiente();
                            //Update.ID_SOL = Int64.Parse(Detalle.ID_PASE.ToString());
                            //Update.SECUENCIA = Int64.Parse(Detalle.LLAVE);
                            //Update.FECHA_EXPIRACION = Detalle.FECHA_SALIDA_PASE;
                            //Update.CANTIDAD_CARGA = Detalle.CANTIDAD_BULTOS.Value;
                            //Update.CANTIDAD_VEHICULOS = Detalle.CANTIDAD_VEHICULOS.Value;
                            //Update.TOTAL_CARGA = Detalle.CANTIDAD.Value;
                            //Update.ID_HORARIO = Detalle.TURNO.Value;
                            //Update.HORARIO = Detalle.D_TURNO;
                            //Update.USUARIO_ESTADO = ClsUsuario.loginname;

                            //var nProceso = Update.SaveTransaction_Aprobar(out cMensajes);
                            //if (!nProceso.HasValue || nProceso.Value <= 0)
                            //{
                            //    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo aprobar el turno {0} - {1}...{2}</b>", Detalle.FECHA_SALIDA_PASE.Value.ToString("dd/MM/yyyy"), Detalle.D_TURNO, cMensajes));
                            //    return;
                            //}

                            //Detalle.ESTADO = "A";
                            //Detalle.ESTADO_PASE = "APROBADA";

                            //this.TxtSecuencia.Text = string.Empty;
                            //this.TxtFechaHasta.Text = string.Empty;
                            //this.TxtNumeroVehiculos.Text = string.Empty;
                            //this.TxtCantidadRetirar.Text = string.Empty;
                            //this.TxtTotalBultosRetirar.Text = string.Empty;
                            //this.Turno_Default();

                        }

                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                        //actualiar solicitudes
                        this.Cargar_Solicitud_Pendientes();

                        this.Actualiza_Paneles();

                    }


                    if (e.CommandName == "Rechazar")
                    {
                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objPaseBRBK = Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                        //existe pase a remover
                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));
                        if (Detalle != null)
                        {

                            //ejecuta proceso de autorizacion a la base de datos
                            brbk_solicitud_pendiente Update = new brbk_solicitud_pendiente();
                            Update.ID_SOL = Int64.Parse(Detalle.ID_PASE.ToString());
                            Update.SECUENCIA = Int64.Parse(Detalle.LLAVE);
                            Update.USUARIO_ESTADO = ClsUsuario.loginname;

                            var nProceso = Update.SaveTransaction_Rechazar(out cMensajes);
                            if (!nProceso.HasValue || nProceso.Value <= 0)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo rechazar el turno {0} - {1}...{2}</b>", Detalle.FECHA_SALIDA_PASE.Value.ToString("dd/MM/yyyy"), Detalle.D_TURNO, cMensajes));
                                return;
                            }

                            Detalle.ESTADO = "R";
                            Detalle.ESTADO_PASE = "RECHAZADA";

                            //this.TxtSecuencia.Text = string.Empty;
                            //this.TxtFechaHasta.Text = string.Empty;
                            //this.TxtNumeroVehiculos.Text = string.Empty;
                            //this.TxtCantidadRetirar.Text = string.Empty;
                            //this.TxtTotalBultosRetirar.Text = string.Empty;
                            this.Turno_Default();


                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar información temporal de los turnos a rechazar: {0} </b>", t.ToString()));
                            return;
                        }

                      
                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                        //actualiar solicitudes
                        this.Cargar_Solicitud_Pendientes();

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
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
                
        //        Label Estado = e.Item.FindControl("LblEstado") as Label;
        //        Label LblPaseExpirado = e.Item.FindControl("LblPaseExpirado") as Label;

        //        Button BtnAprobar = e.Item.FindControl("BtnAprobar") as Button;
        //        Button BtnRechazar = e.Item.FindControl("BtnRechazar") as Button;
        //        Button BtnModificar = e.Item.FindControl("BtnModificar") as Button;


        //        if (!Estado.Text.Equals("P"))
        //        {
        //            BtnAprobar.Attributes["disabled"] = "disabled";
        //            BtnRechazar.Attributes["disabled"] = "disabled";
        //            BtnModificar.Attributes["disabled"] = "disabled";

        //        }
        //        else
        //        {
        //            BtnAprobar.Attributes.Remove("disabled");
        //            BtnRechazar.Attributes.Remove("disabled");
        //            BtnModificar.Attributes.Remove("disabled");
                    
        //        }

        //    }
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
            this.banmsg_lista.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                this.banmsg_det.InnerText = string.Empty;
                this.banmsg_lista.InnerText = string.Empty;
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
               // Server.HtmlEncode(this.Txtempresa.Text.Trim());
               
               // Server.HtmlEncode(this.TxtFechaHasta.Text.Trim());
                Server.HtmlEncode(this.TxtContenedorSeleccionado.Text.Trim());
                Server.HtmlEncode(this.TxtRetirados.Text.Trim());
                Server.HtmlEncode(this.TxtSaldo.Text.Trim());
                Server.HtmlEncode(this.TxtBodega.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                   // this.BtnVisualizar.Attributes["disabled"] = "disabled";

                

                }

                this.TxtFechaConDesde.Text = Server.HtmlEncode(this.TxtFechaConDesde.Text);
                this.TxtFechaConHasta.Text = Server.HtmlEncode(this.TxtFechaConHasta.Text);

                if (!Page.IsPostBack)
                {
                    string desde = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaConDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaConHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");

                    this.Cargar_Solicitud_Pendientes();
                }


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    OcultarLoading("1");
                    OcultarLoading("2");
                    return;
                }

                if (string.IsNullOrEmpty(this.TxtFechaConDesde.Text))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
                    this.TxtFechaConDesde.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.TxtFechaConHasta.Text))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
                    this.TxtFechaConHasta.Focus();
                    return;
                }

                CultureInfo enUS = new CultureInfo("en-US");

                if (!string.IsNullOrEmpty(TxtFechaConDesde.Text))
                {
                    if (!DateTime.TryParseExact(TxtFechaConDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaConDesde.Text));
                        this.TxtFechaConDesde.Focus();
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(TxtFechaConHasta.Text))
                {
                    if (!DateTime.TryParseExact(TxtFechaConHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaConHasta.Text));
                        this.TxtFechaConHasta.Focus();
                        return;

                    }
                }

                TimeSpan tsDias = fechahasta - fechadesde;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} \\nNO deber ser mayor a la\\nFecha final: {1}", TxtFechaConDesde.Text, TxtFechaConHasta.Text));
                    return;
                }
                if (diferenciaEnDias > 62)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las solicitudes de hasta dos meses."));
                    return;
                }

                this.Cargar_Solicitud_Pendientes();

            }
        }
        //listado de solicitudes
        protected void grilla_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Ver")
                    {

                        this.Carga_Tipo_Turnos();

                       // this.CboTipoTurno.Attributes["disabled"] = "disabled";

                        Int64 ID_SOL = Int64.Parse(t.ToString());

                        objCab = new brbk_solicitud_pendiente();
                        objCab.ID_SOL = ID_SOL;
                        if (!objCab.PopulateMyData(out OError))
                        {

                            return;
                        }
                        else
                        {

                            int SALDO_FINAL = 0;


                            this.Titulo.InnerText = string.Format("SOLICITUD DE TURNO BRBK # {0}", ID_SOL);

                            this.TXTMRN.Text = objCab.MRN;
                            this.TXTMSN.Text = objCab.MSN;
                            this.TXTHSN.Text = objCab.HSN;

                            //this.CboTipoTurno.SelectedValue = objCab.ID_TIPO_TURNO.ToString();

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
                                                             Tbl.AGENTE,
                                                             Tbl.FACTURADO,
                                                             Tbl.PAGADO,
                                                             Tbl.GKEY,
                                                             Tbl.REFERENCIA,
                                                             Tbl.CONTENEDOR,
                                                             Tbl.DOCUMENTO,
                                                             Tbl.PRIMERA,
                                                             Tbl.MARCA,
                                                             Tbl.CANTIDAD,
                                                             Tbl.CIATRANS,
                                                             Tbl.CHOFER,
                                                             Tbl.PLACA,
                                                             Tbl.FECHA_SALIDA,
                                                             Tbl.FECHA_SALIDA_PASE,
                                                             Tbl.FECHA_AUT_PPWEB,
                                                             Tbl.HORA_AUT_PPWEB,
                                                             Tbl.CNTR_DD,
                                                             Tbl.AGENTE_DESC,
                                                             Tbl.FACTURADO_DESC,
                                                             Tbl.IMPORTADOR,
                                                             Tbl.IMPORTADOR_DESC,
                                                             Tbl.TIPO_CNTR,
                                                             Tbl.ID_TURNO,
                                                             Tbl.TURNO,
                                                             Tbl.D_TURNO,
                                                             Tbl.ID_PASE,
                                                             Tbl.ESTADO,
                                                             Tbl.ENVIADO,
                                                             Tbl.AUTORIZADO,
                                                             Tbl.VENTANILLA,
                                                             Tbl.USUARIO_ING,
                                                             Tbl.USUARIO_MOD,
                                                             Tbl.ESTADO_PAGO,
                                                             Tbl.ID_UNIDAD,
                                                             CNTR_CANTIDAD_SALDO = (Tbl2.CNTR_CANTIDAD_SALDO == null ? 0 : Tbl2.CNTR_CANTIDAD_SALDO),
                                                             CNTR_PESO = (Tbl2.CNTR_PESO == null ? 0 : Tbl2.CNTR_PESO),
                                                             CNTR_CANTIDAD_TOTAL = (Tbl2.CNTR_CANTIDAD_TOTAL == null ? 0 : Tbl2.CNTR_CANTIDAD_TOTAL),
                                                             CNTR_UBICACION = string.IsNullOrEmpty(Tbl2.CNTR_UBICACION) ? "" : Tbl2.CNTR_UBICACION,
                                                             UNIDAD_GKEY = (Tbl2.UNIDAD_GKEY == null ? 0 : Tbl2.UNIDAD_GKEY),
                                                             CANTIDAD_EMITIDOS = ((Tbl2.CNTR_CANTIDAD_TOTAL == null ? 0 : Tbl2.CNTR_CANTIDAD_TOTAL) - (Tbl2.CNTR_CANTIDAD_SALDO == null ? 0 : Tbl2.CNTR_CANTIDAD_SALDO)),
                                                             CNTR_BODEGA = string.IsNullOrEmpty(Tbl2.CNTR_BODEGA) ? "" : Tbl2.CNTR_BODEGA,
                                                             Tbl.idProducto,
                                                             Tbl.idItem,
                                                             Tbl.Tipo_Producto,
                                                             ID_TIPO_TURNO = objCab.ID_TIPO_TURNO
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
                                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe saldo de la carga: {0} pendiente para aprobar la solicitud de turnos..Saldo actual: {1}</b>", _BL, LinqSaldo.SALDO_FINAL));
                                                        return;
                                                    }

                                                }
                                                else
                                                {
                                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! al validar saldo de la carga...No existe información del saldo de la carga...</b>"));
                                                   // this.TxtFechaHasta.Focus();
                                                    return;
                                                }
                                            }
                                            else
                                            {

                                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! al validar saldo de la carga...No existe información del saldo de la carga...</b>"));
                                                return;
                                            }


                                            //fin de validacion


                                            //agrego todos los contenedores a la clase cabecera
                                            objPaseBRBK = Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                                            objPaseBRBK.ID_PASE = objCab.ID_SOL;
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

         
                                            objPaseBRBK.CNTR_CANTIDAD_SALDO = LinqFinal.CNTR_CANTIDAD_SALDO.Value;
                                            objPaseBRBK.CANTIDAD_EMITIDOS = LinqFinal.CANTIDAD_EMITIDOS.Value;
                                            objPaseBRBK.CNTR_UBICACION = LinqFinal.CNTR_BODEGA;

                                            objPaseBRBK.FECHA_SALIDA_PASE = LinqFinal.FECHA_SALIDA_PASE;

                                            objPaseBRBK.idProducto = LinqFinal.idProducto;
                                            objPaseBRBK.idItem = LinqFinal.idItem;
                                            objPaseBRBK.Tipo_Producto = LinqFinal.Tipo_Producto;


                                            objPaseBRBK.ID_TIPO_TURNO = LinqFinal.ID_TIPO_TURNO;

                                            //this.TxtFechaHasta.Text = string.Empty;
                                            this.TxtFechaCas.Text = objPaseBRBK.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");


                                            this.TxtContenedorSeleccionado.Text = objPaseBRBK.CANTIDAD.ToString();
                                            this.TxtSaldo.Text = objPaseBRBK.CNTR_CANTIDAD_SALDO.ToString();
                                            this.TxtRetirados.Text = objPaseBRBK.CANTIDAD_EMITIDOS.ToString();
                                            this.TxtBodega.Text = objPaseBRBK.CNTR_UBICACION;
                                            this.TxtTipoProducto.Text = objPaseBRBK.Tipo_Producto;
                                           // this.TxtCantidadRetirar.Text = SALDO_FINAL.ToString();
                                            this.TxtSaldo.Text = SALDO_FINAL.ToString();

                                            if (LinqFinal.PAGADO == false)
                                            {
                                                this.TxtPagado.Text = "NO";
                                               
                                                //this.BtnAgregaTruno.Attributes.Add("disabled", "disabled");
                                            }
                                            else
                                            {
                                                
                                               // this.BtnAgregaTruno.Attributes.Remove("disabled");
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



                                            //detalle de pases
                                            objPaseBRBK.Detalle.Clear();

                                            foreach (var Det in objCab.Detalle)
                                            {
                                                objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();
                                                objDetallePaseBRBK.ID_PASE = objCab.ID_SOL;
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
                     
                                                objDetallePaseBRBK.CIATRANS = string.Format("{0} - {1}",Det.ID_EMPRESA, Det.TRANSPORTISTA_DESC);
                                                objDetallePaseBRBK.CHOFER = "";
                                                objDetallePaseBRBK.PLACA = "";
                                                objDetallePaseBRBK.FECHA_SALIDA = objPaseBRBK.FECHA_SALIDA;
                                                objDetallePaseBRBK.CNTR_DD = objPaseBRBK.CNTR_DD;
                                                objDetallePaseBRBK.AGENTE_DESC = objPaseBRBK.AGENTE_DESC;
                                                objDetallePaseBRBK.FACTURADO_DESC = objPaseBRBK.FACTURADO_DESC;
                                                objDetallePaseBRBK.IMPORTADOR = objPaseBRBK.IMPORTADOR;
                                                objDetallePaseBRBK.IMPORTADOR_DESC = objPaseBRBK.IMPORTADOR_DESC;
                                                objDetallePaseBRBK.FECHA_SALIDA_PASE =Det.FECHA_EXPIRACION;
                                                objDetallePaseBRBK.FECHA_AUT_PPWEB = Det.FECHA_EXPIRACION;
                                                objDetallePaseBRBK.HORA_AUT_PPWEB = Det.HORARIO;
                                                objDetallePaseBRBK.TIPO_CNTR = objPaseBRBK.TIPO_CNTR;

                                                objDetallePaseBRBK.D_TURNO = Det.HORARIO;
                                                objDetallePaseBRBK.TURNO = Det.ID_HORARIO;
                                                objDetallePaseBRBK.ID_TURNO = Det.SECUENCIA;//secuencia turno
                                                objDetallePaseBRBK.TURNO_DESDE = Det.TURNO_INICIA.Value;
                                                objDetallePaseBRBK.TURNO_HASTA = Det.TURNO_FIN.Value;
                                                objDetallePaseBRBK.BULTOS_HORARIOS = Det.CANTIDAD_CARGA;

                                                
                                                objDetallePaseBRBK.ESTADO = objPaseBRBK.ESTADO;
                                                objDetallePaseBRBK.ESTADO_PASE = Det.ESTADO_DESCRIPCION;
                                                objDetallePaseBRBK.PASE_EXPIRADO = Det.ESTADO_TURNO;
                                                objDetallePaseBRBK.ENVIADO = objPaseBRBK.ENVIADO;
                                                objDetallePaseBRBK.AUTORIZADO = objPaseBRBK.AUTORIZADO;
                                                objDetallePaseBRBK.VENTANILLA = objPaseBRBK.VENTANILLA;
                                                objDetallePaseBRBK.USUARIO_ING = objPaseBRBK.USUARIO_ING;
                                                objDetallePaseBRBK.FECHA_ING = objPaseBRBK.FECHA_ING;
                                                objDetallePaseBRBK.USUARIO_MOD = Det.USUARIO_ESTADO;
                                                objDetallePaseBRBK.FECHA_MOD = Det.FECHA_ESTADO;
                                                objDetallePaseBRBK.ESTADO_PAGO = objPaseBRBK.ESTADO_PAGO;
                                                objDetallePaseBRBK.ID_PPWEB = objPaseBRBK.ID_PPWEB;

                                                objDetallePaseBRBK.CANTIDAD = Det.TOTAL_CARGA;

                                                objDetallePaseBRBK.CANTIDAD_VEHICULOS = Det.CANTIDAD_VEHICULOS;
                                                objDetallePaseBRBK.CANTIDAD_BULTOS = Det.CANTIDAD_CARGA;

                                                objDetallePaseBRBK.ID_CIATRANS = Det.ID_EMPRESA;
                                                objDetallePaseBRBK.ID_CHOFER = "";
                                                objDetallePaseBRBK.CHOFER = "";
                                                objDetallePaseBRBK.PLACA = "";
                                                objDetallePaseBRBK.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                                                objDetallePaseBRBK.CHOFER_DESC = "";


                                                objDetallePaseBRBK.SUB_SECUENCIA = "";
                                                objDetallePaseBRBK.LLAVE = Det.SECUENCIA.ToString();
                                                objDetallePaseBRBK.ID_UNIDAD = objPaseBRBK.ID_UNIDAD;

                                                objDetallePaseBRBK.ESTADO = Det.ESTADO;
                                                objDetallePaseBRBK.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;

                                                objPaseBRBK.Detalle.Add(objDetallePaseBRBK);
                                            }

                                            tablePagination.DataSource = objPaseBRBK.Detalle;
                                            tablePagination.DataBind();

                                            Session["AprobarBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                                            this.Actualiza_Paneles();
                                        }
                                        else
                                        {


                                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga seleccionada..{0}", Carga.MensajeProblema));
                                            return;
                                        }



                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga seleccionada..{0}", Carga.MensajeProblema));
                                    return;
                                }


                            }
                            else
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga seleccionada..{0}", ListaContenedores.MensajeProblema));
                                return;

                            }

                         
                            this.Actualiza_Paneles();
                        }

                    }
                   

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(grilla_ItemCommand), "grilla_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));



                }
            }

        }


    }

}