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
  

    public partial class emisionpasesolicitudbrbk : System.Web.UI.Page
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

        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();

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
           
            
            UPBOTONES.Update();
            UPCONTENEDOR.Update();
            UPRETIRADOS.Update();
            UPSALDO.Update();
            UPBODEGA.Update();
            UPPRODUCTO.Update();
         
            UPCAS.Update();
          
            this.UPPAGADO.Update();
           
            this.UPTEXTO.Update();
            this.UPTITULO.Update();

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
          
            this.TxtContenedorSeleccionado.Text = string.Empty;
            this.TxtRetirados.Text = string.Empty;
            this.TxtSaldo.Text = string.Empty;
            this.TxtBodega.Text = string.Empty;
            this.TxtTipoProducto.Text = string.Empty;

         
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
            Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
        }

       

     

       

        private void Cargar_Solicitud_Pendientes()
        {
            try
            {
               
                if (string.IsNullOrEmpty(this.TxtFechaFiltroDesde.Text))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
                    this.TxtFechaFiltroDesde.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.TxtFechaFiltroHasta.Text))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
                    this.TxtFechaFiltroHasta.Focus();
                    return;
                }

                CultureInfo enUS = new CultureInfo("en-US");

                if (!string.IsNullOrEmpty(TxtFechaFiltroDesde.Text))
                {
                    if (!DateTime.TryParseExact(TxtFechaFiltroDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaFiltroDesde.Text));
                        this.TxtFechaFiltroDesde.Focus();
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(TxtFechaFiltroHasta.Text))
                {
                    if (!DateTime.TryParseExact(TxtFechaFiltroHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaFiltroHasta.Text));
                        this.TxtFechaFiltroHasta.Focus();
                        return;

                    }
                }

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Tabla = brbk_solicitud_pendiente.Solicitud_Pendientes_EmitirPase(fechadesde, fechahasta, ClsUsuario.ruc, out cMensajes);
                if (Tabla == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de solicitudes pendientes de generar pase de puerta. {0}", cMensajes));
                    return;
                }
                if (Tabla.Count <= 0)
                {
                    grilla.DataSource = null;
                    grilla.DataBind();

                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de solicitudes pendientes de generar pase de puerta."));
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

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.Cargar_Solicitud_Pendientes();

        }


   
       
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

                  
                   


                    if (e.CommandName == "Rechazar")
                    {
                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objPaseBRBK = Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                        //existe pase a remover
                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));
                        if (Detalle != null)
                        {

                            //ejecuta proceso de autorizacion a la base de datos
                            brbk_solicitud_pendiente Update = new brbk_solicitud_pendiente();
                            Update.ID_SOL = Int64.Parse(Detalle.ID_PASE.ToString());
                            Update.SECUENCIA = Int64.Parse(Detalle.LLAVE);
                            Update.USUARIO_ESTADO = ClsUsuario.loginname;

                            var nProceso = Update.SaveTransaction_Rechazar_User(out cMensajes);
                            if (!nProceso.HasValue || nProceso.Value <= 0)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo rechazar el turno {0} - {1}...{2}</b>", Detalle.FECHA_SALIDA_PASE.Value.ToString("dd/MM/yyyy"), Detalle.D_TURNO, cMensajes));
                                return;
                            }

                            Detalle.ESTADO = "R";
                            Detalle.ESTADO_PASE = "RECHAZADA";

                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar información temporal de los turnos a rechazar: {0} </b>", t.ToString()));
                            return;
                        }

                      
                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                Label Estado = e.Item.FindControl("LblEstado") as Label;
                Label LblPase = e.Item.FindControl("LblPase") as Label;

                Button BtnRechazar = e.Item.FindControl("BtnRechazar") as Button;
                CheckBox Chk = e.Item.FindControl("chkPase") as CheckBox;

                Label LblPaseExpirado = e.Item.FindControl("LblPaseExpirado") as Label;

                if (Estado.Text.Equals("A") && LblPase.Text.Equals("0"))
                {
                    BtnRechazar.Attributes.Remove("disabled");
                    Chk.Attributes.Remove("disabled");

                    
                   if (!string.IsNullOrEmpty(LblPaseExpirado.Text))
                   {
                        Chk.Attributes["disabled"] = "disabled";
                    }
                   else
                   {
                        Chk.Attributes.Remove("disabled");
                    }
                  
                }
                else
                {
                    BtnRechazar.Attributes["disabled"] = "disabled";
                    Chk.Attributes["disabled"] = "disabled";
                }

            }
        }

        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label LblSecuencia = (Label)item.FindControl("LblSecuencia");

                string SECUENCIA = LblSecuencia.Text;
              

                //actualiza datos del contenedor
                objPaseBRBK = Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(SECUENCIA));
                if (Detalle != null)
                {
                    if (Detalle.ESTADO.Equals("R") || !string.IsNullOrEmpty(Detalle.PASE_EXPIRADO))
                    {
                        Detalle.VISTO = false;
                    }
                    else
                    {
                        Detalle.VISTO = chkPase.Checked;
                    }
                   

                }

                tablePagination.DataSource = objPaseBRBK.Detalle;
                tablePagination.DataBind();

                Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                this.Pintar_Grilla();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }

        private void Pintar_Grilla()
        {
            foreach (RepeaterItem xitem in tablePagination.Items)
            {
                CheckBox ChkVisto = xitem.FindControl("chkPase") as CheckBox;

                Label LblFechaSalida = xitem.FindControl("LblFechaSalida") as Label;
                Label LblFechaturno = xitem.FindControl("LblFechaturno") as Label;
                Label LblTurno = xitem.FindControl("LblTurno") as Label;
                Label LblEmpresa = xitem.FindControl("LblEmpresa") as Label;
                Label LblContenedor = xitem.FindControl("LblContenedor") as Label;
                Label LblChofer = xitem.FindControl("LblChofer") as Label;
                Label LblPlaca = xitem.FindControl("LblPlaca") as Label;
                Label LblEstadoPase = xitem.FindControl("LblEstadoPase") as Label;
              
                if (ChkVisto.Checked == true)
                {

                    LblFechaSalida.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblFechaturno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblTurno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEmpresa.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblContenedor.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblChofer.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblPlaca.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEstadoPase.ForeColor = System.Drawing.Color.PaleVioletRed;

                }



            }
        }



        #endregion

        #region "Transportista"
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


                    objPaseBRBK = Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todos los pases a modificar el transportista... </b>"));
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
                        string SECUENCIA = Det.LLAVE;

                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(SECUENCIA));
                        if (Detalle != null)
                        {
                            if (Detalle.ESTADO.Equals("R") || !string.IsNullOrEmpty(Detalle.PASE_EXPIRADO))
                            {

                            }
                            else
                            {
                                Detalle.VISTO = ChkEstado;
                            }

                        }
                    }


                    tablePagination.DataSource = objPaseBRBK.Detalle;
                    tablePagination.DataBind();

                    Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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

                    objPaseBRBK = Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar actualizar la empresa de transporte... </b>"));
                        return;
                    }
                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de turnos, para poder actualizar los datos. </b>"));
                        return;
                    }

                    var LinqValidaPase = (from p in objPaseBRBK.Detalle.Where(x => x.VISTO == true)
                                          select p.ID_PASE).ToList();

                    if (LinqValidaPase.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los turnos, para actualizar con la empresa de transporte: {0} </b>", Txtempresa.Text.Trim()));
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
                        string SECUENCIAL = Det.LLAVE;

                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(SECUENCIAL));
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


                    Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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

        #endregion

        #region "Generar pase de puerta"

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

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //instancia sesion
                    objPaseBRBK = Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar los pase a puerta de carga break bulk. </b>"));
                        return;
                    }



                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen turnos aprobados para generar pases de puertas de break bulk.</b>"));
                        return;
                    }

                    LoginName = ClsUsuario.loginname;

                 

                    var LinqValidaPase = (from p in objPaseBRBK.Detalle.Where(x => x.VISTO == false && x.ESTADO.Equals("A"))
                                          select p.ID_PASE).ToList();

                    if (LinqValidaPase.Count != 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar todos los turnos aprobados, para generar pases de puertas de break bulk.. </b>"));
                        return;
                    }

                    var LinqValidaPase2 = (from p in objPaseBRBK.Detalle.Where(x => x.ESTADO.Equals("A"))
                                          select p.ID_PASE).ToList();

                    if (LinqValidaPase2.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar todos los turnos aprobados, para generar pases de puertas de break bulk.. </b>"));
                        return;
                    }

                    /***********************************************************************************************************************************************
                    *valida que tenga un turno ingresado
                    **********************************************************************************************************************************************/
                    foreach (var Det in objPaseBRBK.Detalle)
                    {

                        if (string.IsNullOrEmpty(Det.D_TURNO))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se ha ingresado el turno para poder generar el pase de puerta de la carga: {0} </b>", Det.CARGA));
                            return;
                        }

                        if (string.IsNullOrEmpty(Det.ID_CIATRANS))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se ha ingresado la empresa de transporte para poder generar el pase de puerta de la carga: {0} </b>", Det.CARGA));
                            return;
                        }

                    }


                    this.BtnGrabar.Attributes.Add("disabled", "disabled");
                    this.Actualiza_Paneles();

                    /*contenedores seleccionados para emitir pase*/
                    var LinqQuery = (from Tbl in objPaseBRBK.Detalle.Where(Tbl => Tbl.CANTIDAD_BULTOS != 0 && Tbl.ESTADO.Equals("A") && Tbl.PASE_DESDE_SOLICITUD.Equals(0))
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
                                         SOLICITUD = Int64.Parse(Tbl.ID_PASE.Value.ToString()),
                                         SECUENCIA = Int64.Parse(Tbl.LLAVE),
                                         CANTIDAD_BULTOS = Tbl.CANTIDAD_BULTOS,
                                         CANTIDAD_VEHICULOS = Tbl.CANTIDAD_VEHICULOS,
                                         LLAVE = Tbl.LLAVE

                                     }).ToList().OrderBy(x => x.CONTENEDOR);


                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/
                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {
                        //recorro cantidad de pases a emitir
                        int i = 1;
                        for (i = 1; i <= Det.CANTIDAD_VEHICULOS; i++)
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
                            pase.CANTIDAD_CARGA = Det.CANTIDAD_BULTOS;
                            pase.USUARIO_REGISTRO = LoginName;
                            pase.TIPO_CARGA = "BRBK";

                            pase.CONSIGNATARIO_ID = Det.IMPORTADOR;
                            pase.CONSIGNARIO_NOMBRE = Det.IMPORTADOR_DESC;
                            pase.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                            pase.CHOFER_DESC = Det.CHOFER_DESC;
                            pase.PPW = Det.ID_PPWEB;
                            pase.ID_UNIDAD = Det.ID_UNIDAD;



                            var Resultado = pase.Insertar_Solicitud(Det.TURNO.Value, Det.CONTENEDOR, Det.CANTIDAD_BULTOS.Value, Det.MRN, Det.MSN, Det.HSN, Det.SOLICITUD, Det.SECUENCIA,Det.CNTR_DD );
                            if (Resultado.Exitoso)
                            {
                                nTotal++;
                                if (nTotal == 1)
                                {
                                    id_carga = securetext(Det.CARGA);
                                }

                                //actualiza detalle con el pase generado
                                var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(Det.LLAVE));
                                if (Detalle != null)
                                {
                                    Detalle.PASE_DESDE_SOLICITUD = Int64.Parse(pase.ID_PASE.ToString());
                                }

                            }
                            else
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar el pase de puerta para la carga: {0}, Total bultos {1} Existen los siguientes problemas: {2}, {3} </b>", Det.CARGA, Det.CANTIDAD_BULTOS, Resultado.MensajeInformacion, Resultado.MensajeProblema));
                                return;
                            }

                        }
                        
                    }




                    //imprimir pase de puerta
                    if (nTotal != 0)
                    {
                        string link = string.Format("<a href='../pasepuertabrbk/imprimirpasebrbk.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta</a>", id_carga);

                      
                        Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        this.OcultarLoading("1");


                        this.Cargar_Solicitud_Pendientes();
                       


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
            
               
               
                Server.HtmlEncode(this.TxtContenedorSeleccionado.Text.Trim());
                Server.HtmlEncode(this.TxtRetirados.Text.Trim());
                Server.HtmlEncode(this.TxtSaldo.Text.Trim());
                Server.HtmlEncode(this.TxtBodega.Text.Trim());

                this.TxtFechaFiltroDesde.Text = Server.HtmlEncode(this.TxtFechaFiltroDesde.Text);
                this.TxtFechaFiltroHasta.Text = Server.HtmlEncode(this.TxtFechaFiltroHasta.Text);

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();
                    
                    string desde = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaFiltroDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaFiltroHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");

                    this.Cargar_Solicitud_Pendientes();

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
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
                        Int64 ID_SOL = Int64.Parse(t.ToString());

                        objCab = new brbk_solicitud_pendiente();
                        objCab.ID_SOL = ID_SOL;
                        if (!objCab.PopulateMyData_Aprobados(out OError))
                        {

                            return;
                        }
                        else
                        {

                            int SALDO_FINAL = 0;


                            this.Titulo.InnerText = string.Format("SOLICITUD PENDIENTE DE GENERAR PASE DE PUERTA. SOLICITUD # {0}", ID_SOL);

                            this.TXTMRN.Text = objCab.MRN;
                            this.TXTMSN.Text = objCab.MSN;
                            this.TXTHSN.Text = objCab.HSN;


                            Int64 ID_TIPO_TURNO = objCab.ID_TIPO_TURNO;
                            string TIPO_TURNO = objCab.TIPO_TURNO;

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
                                            objPaseBRBK = Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
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

                                          
                                            this.TxtFechaCas.Text = objPaseBRBK.FECHA_SALIDA_PASE.Value.ToString("MM/dd/yyyy");


                                            this.TxtContenedorSeleccionado.Text = objPaseBRBK.CANTIDAD.ToString();
                                            this.TxtSaldo.Text = objPaseBRBK.CNTR_CANTIDAD_SALDO.ToString();
                                            this.TxtRetirados.Text = objPaseBRBK.CANTIDAD_EMITIDOS.ToString();
                                            this.TxtBodega.Text = objPaseBRBK.CNTR_UBICACION;
                                            this.TxtTipoProducto.Text = objPaseBRBK.Tipo_Producto;
                                
                                            this.TxtSaldo.Text = SALDO_FINAL.ToString();

                                            if (LinqFinal.PAGADO == false)
                                            {
                                                this.TxtPagado.Text = "NO";
                                               
                                                this.BtnGrabar.Attributes.Add("disabled", "disabled");
                                            }
                                            else
                                            {
                                                
                                               this.BtnGrabar.Attributes.Remove("disabled");
                                                this.TxtPagado.Text = "SI";
                                            }


                                            objPaseBRBK.FECHA_AUT_PPWEB = LinqFinal.FECHA_AUT_PPWEB;
                                            objPaseBRBK.HORA_AUT_PPWEB = LinqFinal.HORA_AUT_PPWEB;

                                            objPaseBRBK.TIPO_CNTR = LinqFinal.TIPO_CNTR;
                                            objPaseBRBK.ID_TURNO = LinqFinal.ID_TURNO;
                                            objPaseBRBK.TURNO = LinqFinal.TURNO;
                                            objPaseBRBK.D_TURNO = LinqFinal.D_TURNO;
                                            //objPaseBRBK.ID_PASE = double.Parse(LinqFinal.ID_PASE.Value.ToString());
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
                                                objDetallePaseBRBK.PASE_DESDE_SOLICITUD = Det.PASE_DESDE_SOLICITUD;
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
                                                objDetallePaseBRBK.USUARIO_MOD = objPaseBRBK.USUARIO_MOD;
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

                                             
                                                objDetallePaseBRBK.ID_TIPO_TURNO = ID_TIPO_TURNO;
                                                objDetallePaseBRBK.TIPO_TURNO = string.Format("Tipo turno: {0}",TIPO_TURNO);

                                                objPaseBRBK.Detalle.Add(objDetallePaseBRBK);
                                            }

                                            tablePagination.DataSource = objPaseBRBK.Detalle;
                                            tablePagination.DataBind();

                                            Session["EmitirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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