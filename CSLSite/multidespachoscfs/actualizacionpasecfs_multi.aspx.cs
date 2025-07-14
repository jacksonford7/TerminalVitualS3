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
  

    public partial class actualizacionpasecfs_multi : System.Web.UI.Page
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
           
           
        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
           
            this.Txtempresa.Text = string.Empty;
            this.TxtChofer.Text = string.Empty;
            this.TxtPlaca.Text = string.Empty;
           
         

            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();

            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });

          

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
            Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;
        }

        private void Turno_Default()
        {
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Seleccione *" });
          
        }

        private void Pase_Sin_Turno_Default()
        {
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}-{4}", "0", "0", "", "","0"), Turno = "* Pase Sin turno *" });
          
        }

        private void Pintar_Grilla()
        {
            foreach (RepeaterItem xitem in tablePagination.Items)
            {
                CheckBox ChkVisto = xitem.FindControl("chkPase") as CheckBox;

                Label LblPase = xitem.FindControl("LblPase") as Label;
                Label LblCarga = xitem.FindControl("LblCarga") as Label;
                Label LblCantidad = xitem.FindControl("LblCantidad") as Label;
                Label LblFechaSalida = xitem.FindControl("LblFechaSalida") as Label;
                Label LblTurno = xitem.FindControl("LblTurno") as Label;
                Label LblEmpresa = xitem.FindControl("LblEmpresa") as Label;
                Label LblChofer = xitem.FindControl("LblChofer") as Label;
                Label LblPlaca = xitem.FindControl("LblPlaca") as Label;
                Label LblFechaturno = xitem.FindControl("LblFechaturno") as Label;
                Label LblEstado = xitem.FindControl("LblEstado") as Label;
                Label LblMensaje = xitem.FindControl("LblMensaje") as Label;
                if (ChkVisto.Checked == true || LblEstado.Text == "EXPIRADO")
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
                    LblCarga.ForeColor = System.Drawing.Color.PaleVioletRed;
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
                   

                    objPaseCFS = Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todos los pases a modificar... </b>"));
                        return;
                    }
                    if (objPaseCFS.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de pases, para poder seleccionar... </b>"));
                        return;
                    }


                    //proceso de marcar subitems
                    foreach (var Det in objPaseCFS.Detalle)
                    {
                        double ID_PASE = Det.ID_PASE.Value;
                        var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
                        if (Detalle != null)
                        {
                            if (Detalle.ESTADO.Equals("EXPIRADO") || Detalle.ESTADO_TRANSACCION==false){ }
                            else { Detalle.VISTO = ChkEstado;  }
 
                        }
                    }


                    tablePagination.DataSource = objPaseCFS.Detalle.OrderBy(p => p.ORDENAMIENTO);
                    tablePagination.DataBind();

                    Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

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
                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();


                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar la Compañía de Transporte para poder agregar la información </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }

                    objPaseCFS = Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar el o los pases de puerta... </b>"));
                        return;
                    }
                    if (objPaseCFS.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de pases de puerta, para poder actualizar los datos. </b>"));
                        return;
                    }

                    var LinqValidaPase = (from p in objPaseCFS.Detalle.Where(x => x.VISTO == true)
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
                    foreach (var Det in objPaseCFS.Detalle.Where(x => x.VISTO == true))
                    {
                        double ID_PASE = Det.ID_PASE.Value;

                        var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
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

                    tablePagination.DataSource = objPaseCFS.Detalle;
                    tablePagination.DataBind();


                    Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

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
                    bool Tiene_Servicio_p2d = false;

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
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de despacho"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Carga = PasePuerta.Pase_CFS.ObtenerListaEditable_MultiDespacho(this.TXTMRN.Text.Trim(), ClsUsuario.ruc, ClsUsuario.loginname);
                    if (Carga.Exitoso)
                    {


                        /*********************************************NUEVA VALIDACION************************************************/
                        /*estado de la unidad*/
                        List<Int64> Lista = new List<Int64>();
                        List<Cls_Bil_PasePuertaCFS_Validacion> Validacion = new List<Cls_Bil_PasePuertaCFS_Validacion>();

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
                        var EstadoPases = N4.Importacion.container_cfs.ValidarEstadoTransaccion(Lista, ClsUsuario.loginname.Trim());

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
                                Validacion.Add(new Cls_Bil_PasePuertaCFS_Validacion { NUMERO_PASE_N4 = Det.NUMERO_PASE_N4, UBICACION = Det.UBICACION, MENSAJE = Det.MENSAJE, ESTADO = Det.ESTADO });
                            }

                        }
                        else
                        {
                            /*foreach (var Det in LinqPases)
                            {
                                Validacion.Add(new Cls_Bil_PasePuertaCFS_Validacion { NUMERO_PASE_N4 = Int64.Parse(Det.PASE), UBICACION = string.Empty, MENSAJE = string.Empty, ESTADO = false });

                            }*/
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
                                             SUB_SECUENCIA = p.Field<string>("SUB_SECUENCIA") == null ? "" : p.Field<string>("SUB_SECUENCIA").Trim(),
                                             ID_CHOFER = p.Field<string>("ID_CHOFER") == null ? "" : p.Field<string>("ID_CHOFER").Trim(),
                                             ID_EMPRESA = p.Field<string>("ID_EMPRESA") == null ? "" : p.Field<string>("ID_EMPRESA").Trim(),
                                             ID_PLACA = p.Field<string>("ID_PLACA") == null ? "" : p.Field<string>("ID_PLACA").Trim(),
                                             ID_CARGA = p.Field<Int64>("GKEY"),
                                             IN_OUT = (Final == null) ? "" : Final.UBICACION,
                                             PATIO = (Final == null) ? "" : Final.MENSAJE,
                                             ESTADO_TRANCCION = (Final == null) ? true : Final.ESTADO,
                                             ID_UNIDAD = p.Field<Int64?>("ID_UNIDAD"),
                                             ORDEN = p.Field<string>("ORDEN") == null ? "" : p.Field<string>("ORDEN").Trim(),
                                             ID_CIUDAD = p.Field<Int64?>("ID_CIUDAD"),
                                             ID_ZONA = p.Field<Int64?>("ID_ZONA"),
                                             DIRECCION = p.Field<string>("DIRECCION") == null ? "" : p.Field<string>("DIRECCION").Trim(),
                                             ORDER_ID = p.Field<string>("ORDER_ID") == null ? "" : p.Field<string>("ORDER_ID").Trim(),
                                             TRACKING_NUMBER = p.Field<string>("TRACKING_NUMBER") == null ? "" : p.Field<string>("TRACKING_NUMBER").Trim(),
                                             P2D = p.Field<bool?>("P2D") == null ? false : p.Field<bool?>("P2D"),
                                             ENVIADO_LIFTIF = p.Field<bool?>("ENVIADO_LIFTIF") == null ? false : p.Field<bool?>("ENVIADO_LIFTIF"),
                                             LATITUD = p.Field<decimal?>("LATITUD"),
                                             LONGITUD = p.Field<decimal?>("LONGITUD"),
                                             CONTACTO = p.Field<string>("CONTACTO") == null ? "" : p.Field<string>("CONTACTO").Trim(),
                                             CLIENTE = p.Field<string>("CLIENTE") == null ? "" : p.Field<string>("CLIENTE").Trim(),
                                             TELEFONOS = p.Field<string>("TELEFONOS") == null ? "" : p.Field<string>("TELEFONOS").Trim(),
                                             EMAIL = p.Field<string>("EMAIL") == null ? "" : p.Field<string>("EMAIL").Trim(),
                                             ID_CLIENTE = p.Field<string>("ID_CLIENTE") == null ? "" : p.Field<string>("ID_CLIENTE").Trim(),
                                             PRODUCTO = p.Field<string>("PRODUCTO") == null ? "" : p.Field<string>("PRODUCTO").Trim(),
                                             PESO = p.Field<decimal?>("PESO"),
                                             VOLUMEN = p.Field<decimal?>("VOLUMEN"),
                                             DIRECCION_CLIENTE = p.Field<string>("DIRECCION_CLIENTE") == null ? "" : p.Field<string>("DIRECCION_CLIENTE").Trim()
                                         });


                        if (LinqQuery != null)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseCFS = Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                            objPaseCFS.FECHA = DateTime.Now;
                           
                            objPaseCFS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseCFS.SESION = this.hf_BrowserWindowName.Value;

                            objPaseCFS.Detalle.Clear();
                            objPaseCFS.DetalleSubItem.Clear();

                            Int64 pValor = 0;


                           

                            foreach (var Det in LinqQuery)
                            {

                                List<Cls_Bil_PasePuertaCFS_InOut> PaseUsado = Cls_Bil_PasePuertaCFS_InOut.Pase_Utilizado(Int64.Parse(Det.NUMERO_PASE_N4.ToString()), out cMensajes);

                                if (PaseUsado != null)
                                {
                                    pValor = (from Tbl in PaseUsado select new { VALOR = (Tbl.VALOR == 0) ? 0 : Tbl.VALOR }).FirstOrDefault().VALOR;
                                }
                                else { pValor = 0; }

                                objPaseCFS.MRN = Det.MRN;
                                objPaseCFS.MSN = Det.MSN;
                                objPaseCFS.HSN = Det.HSN;

                                objPaseCFS.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objPaseCFS.FECHA_SALIDA_PASE = Det.FECHA_PASE;

                                objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();
                                objDetallePaseCFS.FECHA = objPaseCFS.FECHA;
                                objDetallePaseCFS.MRN = objPaseCFS.MRN;
                                objDetallePaseCFS.MSN = objPaseCFS.MSN;
                                objDetallePaseCFS.HSN = objPaseCFS.HSN;
                                objDetallePaseCFS.IV_USUARIO_CREA = objPaseCFS.IV_USUARIO_CREA;
                                objDetallePaseCFS.SESION = objPaseCFS.SESION;

                                objDetallePaseCFS.FACTURA = Det.FACTURA;
                                objDetallePaseCFS.CARGA = string.Format("{0}-{1}-{2}", Det.MRN, Det.MSN, Det.HSN);
                                objDetallePaseCFS.AGENTE = Det.AGENTE;
                                objDetallePaseCFS.FACTURADO = Det.FACTURADO;
                                objDetallePaseCFS.GKEY = (Int64)Det.GKEY;
                                objDetallePaseCFS.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseCFS.DOCUMENTO = Det.DOCUMENTO;
                                objDetallePaseCFS.PRIMERA = Det.PRIMERA;
                                objDetallePaseCFS.MARCA = Det.MARCA;
                                objDetallePaseCFS.CANTIDAD = Det.CANTIDAD;
                                objDetallePaseCFS.CANTIDAD_CARGA = Det.CANTIDAD_CARGA;
                                objDetallePaseCFS.CIATRANS = Det.ID_EMPRESA;
                                objDetallePaseCFS.CHOFER = Det.ID_CHOFER;
                                objDetallePaseCFS.PLACA = Det.ID_PLACA;

                                objDetallePaseCFS.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objDetallePaseCFS.CNTR_DD = Det.CNTR_DD.Value;
                                objDetallePaseCFS.AGENTE_DESC = Det.AGENTE_DESC;
                                objDetallePaseCFS.FACTURADO_DESC = Det.FACTURADO_DESC;
                                objDetallePaseCFS.IMPORTADOR = Det.IMPORTADOR;
                                objDetallePaseCFS.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;
                                objDetallePaseCFS.FECHA_SALIDA_PASE = Det.FECHA_PASE;
                                objDetallePaseCFS.FECHA_AUT_PPWEB = Det.FECHA_AUT_PPWEB;
                                objDetallePaseCFS.TIPO_CNTR = Det.TIPO_CNTR;
                                objDetallePaseCFS.ID_TURNO = Det.ID_TURNO;
                                objDetallePaseCFS.TURNO = Det.ID_TURNO;
                                objDetallePaseCFS.D_TURNO = Det.D_TURNO;
                                objDetallePaseCFS.ID_PASE = (double)Det.ID_PASE;
                                objDetallePaseCFS.ESTADO = Det.ESTADO;
                                objDetallePaseCFS.FECHA_ING = Det.FECHA_ING;
                                objDetallePaseCFS.ID_PPWEB = Det.ID_PPWEB;
                                objDetallePaseCFS.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;

                                objDetallePaseCFS.ID_CIATRANS = Det.ID_EMPRESA;
                                objDetallePaseCFS.ID_CHOFER = Det.ID_CHOFER;
                                objDetallePaseCFS.SUB_SECUENCIA = Det.SUB_SECUENCIA;
                                objDetallePaseCFS.CIATRANS = string.Format("{0} - {1}", Det.ID_EMPRESA, Det.TRANSPORTISTA_DESC);
                                objDetallePaseCFS.CHOFER = (!string.IsNullOrEmpty(Det.ID_CHOFER) ? string.Format("{0} - {1}", Det.ID_CHOFER, Det.CHOFER_DESC) : string.Empty);
                                objDetallePaseCFS.PLACA = Det.ID_PLACA;
                                objDetallePaseCFS.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                                objDetallePaseCFS.CHOFER_DESC = Det.CHOFER_DESC;
                                objDetallePaseCFS.LLAVE = Det.SUB_SECUENCIA;
                                objDetallePaseCFS.ID_UNIDAD = (Det.ID_UNIDAD == null ? 0 : Det.ID_UNIDAD);

                                objDetallePaseCFS.ESTADO = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseCFS.MOSTRAR_MENSAJE = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseCFS.ORDENAMIENTO = (Det.ESTADO.Equals("GN") ? "1" : "2");
                                objDetallePaseCFS.IN_OUT = Det.IN_OUT;
                                objDetallePaseCFS.ESTADO_TRANSACCION = Det.ESTADO_TRANCCION;

                                if (pValor == 1)
                                {
                                    objDetallePaseCFS.MOSTRAR_MENSAJE = string.Format("{0} - {1} {2}", objDetallePaseCFS.ESTADO, "PASE UTILIZADO", (!string.IsNullOrEmpty(Det.PATIO) ? string.Format(" - {0}", Det.PATIO) : string.Empty));
                                    objDetallePaseCFS.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseCFS.ORDENAMIENTO, "1");
                                    objDetallePaseCFS.ESTADO_TRANSACCION = false;
                                }
                                else
                                {
                                    objDetallePaseCFS.MOSTRAR_MENSAJE = string.Format("{0} - {1} {2}", objDetallePaseCFS.ESTADO, "PASE SIN USAR", (!string.IsNullOrEmpty(Det.PATIO) ? string.Format(" - {0}", Det.PATIO) : string.Empty));
                                    objDetallePaseCFS.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseCFS.ORDENAMIENTO, "2");
                                }


                                objDetallePaseCFS.CAMBIO_TURNO = "NO";
                                objDetallePaseCFS.SERVICIO = Det.SERVICIO;


                                objDetallePaseCFS.ORDEN = Det.ORDEN;
                                objDetallePaseCFS.ID_CIUDAD = Det.ID_CIUDAD;
                                objDetallePaseCFS.ID_ZONA = Det.ID_ZONA;
                                objDetallePaseCFS.DIRECCION = Det.DIRECCION;
                                objDetallePaseCFS.ORDER_ID = Det.ORDER_ID;
                                objDetallePaseCFS.TRACKING_NUMBER = Det.TRACKING_NUMBER;
                                objDetallePaseCFS.P2D = (!Det.P2D.HasValue ? false : Det.P2D);
                                objDetallePaseCFS.ENVIADO_LIFTIF = (!Det.ENVIADO_LIFTIF.HasValue ? false : Det.ENVIADO_LIFTIF);
                                objDetallePaseCFS.LATITUD = (!Det.LATITUD.HasValue ? 0 : Det.LATITUD);
                                objDetallePaseCFS.LONGITUD = (!Det.LONGITUD.HasValue ? 0 : Det.LONGITUD);
                                objDetallePaseCFS.CONTACTO = Det.CONTACTO;
                                objDetallePaseCFS.CLIENTE = Det.CLIENTE;
                                objDetallePaseCFS.TELEFONOS = Det.TELEFONOS;
                                objDetallePaseCFS.EMAIL = Det.EMAIL;
                                objDetallePaseCFS.ID_CLIENTE = Det.ID_CLIENTE;
                                objDetallePaseCFS.PRODUCTO = Det.PRODUCTO;
                                objDetallePaseCFS.PESO = (!Det.PESO.HasValue ? 0 : Det.PESO);
                                objDetallePaseCFS.VOLUMEN = (!Det.VOLUMEN.HasValue ? 0 : Det.VOLUMEN);
                                objDetallePaseCFS.DIRECCION_CLIENTE = Det.DIRECCION_CLIENTE;

                                Tiene_Servicio_p2d = objDetallePaseCFS.P2D.Value;

                                //si no tiene nada pendiente de facturar
                                if (!objDetallePaseCFS.SERVICIO.Value)
                                {

                                    if (Det.CNTR_DD.Value)
                                    {

                                        objDetallePaseCFS.TIPO_CNTR = string.Format("{0} - {1}", Det.TIPO_CNTR, "Desaduanamiento Directo");
                                    }

                                    if (!objDetallePaseCFS.ESTADO.Equals("EXPIRADO"))
                                    {
                                        objDetallePaseCFS.VISTO = true;
                                    }
                                   
                                    objPaseCFS.Detalle.Add(objDetallePaseCFS);
                                }


                            }

 

                            tablePagination.DataSource = objPaseCFS.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                            tablePagination.DataBind();


                            Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                            if (objPaseCFS.Detalle.Count == 0)
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", objPaseCFS.CARGA));
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
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Actualizar Pase CFS", false, null, null, ex.StackTrace, ex);
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
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Por favor ingresar el número de despacho </b>"));
                        this.TXTMRN.Focus();
                        return;
                    }
                   
                    //instancia sesion
                    objPaseCFS = Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder actualizar los pase a puerta de carga suelta multidespacho </b>"));
                        return;
                    }

                    if (objPaseCFS.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen pases de puertas de carga suelta pendientes para generar.  </b>"));
                        return;
                    }

                    var LinqValidaFaltantes = (from p in objPaseCFS.Detalle.Where(x => x.VISTO == true)
                                                       select p.ID_PASE).ToList();

                    if (LinqValidaFaltantes.Count <= 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los pases de puerta a realizar la actualización</b>"));
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
                            
                            if (string.IsNullOrEmpty(Det.ID_CIATRANS))
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se ha ingresado la empresa de transporte para poder generar el pase de puerta de la carga multidespacho: {0} </b>", Det.CARGA));
                                this.IdTxtempresa.Focus();
                                return;
                            }

                        }
                    }


                    /*VALIDACION NUEVA, VEHICULO ESTA FUERA 20-05-2020*/
                    /*estado de la unidad*/
                    List<Int64> Lista = new List<Int64>();

                    var LinqGkey = (from p in objPaseCFS.Detalle.Where(x => x.VISTO == true)
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

                    var EstadoUnidad = N4.Importacion.container_cfs.ValidarEstadoTransaccion(Lista, LoginName);
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
                    var LinqQuery = (from Tbl in objPaseCFS.Detalle.Where(Tbl => Tbl.VISTO == true)
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
                                         SUB_SECUENCIA = (Tbl.SUB_SECUENCIA == null) ? string.Empty : Tbl.SUB_SECUENCIA,
                                         NUMERO_PASE_N4 = (Tbl.NUMERO_PASE_N4 == null) ? "0" : Tbl.NUMERO_PASE_N4,
                                         ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD,
                                         ORDEN = Tbl.ORDEN,
                                         ID_CIUDAD = Tbl.ID_CIUDAD,
                                         ID_ZONA = Tbl.ID_ZONA,
                                         DIRECCION = Tbl.DIRECCION,
                                         ORDER_ID = Tbl.ORDER_ID,
                                         TRACKING_NUMBER = Tbl.TRACKING_NUMBER,
                                         P2D = Tbl.P2D,
                                         ENVIADO_LIFTIF =Tbl.ENVIADO_LIFTIF,
                                         LATITUD = Tbl.LATITUD,
                                         LONGITUD =Tbl.LONGITUD,
                                         CONTACTO = Tbl.CONTACTO,
                                         CLIENTE = Tbl.CLIENTE,
                                         TELEFONOS = Tbl.TELEFONOS,
                                         EMAIL = Tbl.EMAIL,
                                         ID_CLIENTE = Tbl.ID_CLIENTE,
                                         PRODUCTO = Tbl.PRODUCTO,
                                         PESO = Tbl.PESO,
                                         VOLUMEN = Tbl.VOLUMEN,
                                         DIRECCION_CLIENTE = Tbl.DIRECCION_CLIENTE
                                }).ToList().OrderBy(x => x.CONTENEDOR);


                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/

                                

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {

                        P2D_Pase_CFS pase = new P2D_Pase_CFS();
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
                        pase.TIPO_CARGA = "CFS";
                        pase.PPW = Det.ID_PPWEB;
                        pase.ID_CIUDAD = null;
                        pase.ID_ZONA = null;
                        pase.DIRECCION = null;
                        pase.LATITUD = null;
                        pase.LONGITUD = null;
                        

                        var Resultado = pase.Actualizar_MultiDespacho(ClsUsuario.loginname);
                        if (Resultado.Exitoso)
                        {
                            nTotal++;
                            if (nTotal == 1)
                            {
                                id_carga = securetext(Det.CARGA);

                                id_carga = securetext(this.TXTMRN.Text);
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
                        string link = string.Format("<a href='../multidespachoscfs/imprimirpasecfs_multi.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta MultiDespacho</a>", id_carga);

                        //limpiar
                        objPaseCFS.Detalle.Clear();
                        objPaseCFS.DetalleSubItem.Clear();

                        Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                        

                        tablePagination.DataSource = objPaseCFS.Detalle;
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
                
                //actualiza datos del contenedor
                objPaseCFS = Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPase.Checked;

                }

                tablePagination.DataSource = objPaseCFS.Detalle.OrderBy(p => p.ORDENAMIENTO);
                tablePagination.DataBind();

                Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

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

                   

                    //nuevos servicios
                    if (e.CommandName == "Facturar")
                    {
                        objPaseCFS = Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                        string CARGA = string.Empty;
                        Int64 ID_UNIDAD = 0;
                        bool proceso_ok = false;
                        Dictionary<Int64, string> Lista_Gkeys = new Dictionary<Int64, string>();

                        Int64 ID_PASE = 0;
                        ID_PASE = Int64.Parse(t);

                        string _mrn = string.Empty;
                        string _msn = string.Empty;
                        string _hsn = string.Empty;

                        //marcar a todos
                        foreach (var Det in objPaseCFS.Detalle.Where(p => p.SERVICIO == false && p.ID_PASE == ID_PASE))
                        {
                            var Existe = objPaseCFS.Detalle.FirstOrDefault(q => q.ID_PASE == ID_PASE);
                            if (Existe != null)
                            {
                                Existe.SERVICIO = true;
                                CARGA = Existe.CARGA;
                                ID_UNIDAD = Existe.ID_UNIDAD.Value;
                                Lista_Gkeys.Add(Int64.Parse(Existe.ID_PASE.ToString()), Existe.NUMERO_PASE_N4);
                                proceso_ok = true;
                                _mrn = Existe.MRN;
                                _msn = Existe.MSN;
                                _hsn = Existe.HSN;
                            }
                        }

                        if (proceso_ok)
                        {
                            var Resultado = Pase_CFS.Marcar_Servicio_MultiDespacho(ClsUsuario.loginname, ID_UNIDAD, Lista_Gkeys);
                            if (!Resultado.Exitoso)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: {1}, {2} </b>", CARGA, Resultado.MensajeInformacion, Resultado.MensajeProblema));
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: No existen registros de pases </b>", CARGA));
                            return;
                        }


                        tablePagination.DataSource = objPaseCFS.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                        tablePagination.DataBind();

                        Session["ActuPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;


                        string id_carga = securetext(CARGA.Replace("-", "+"));
                        string link = string.Format("<a href='../cargacfs/facturacioncfsadicional.aspx?ID_CARGA={0}' target ='_parent'>Facturar E-Pass Vencido Multidespacho CFS</a>", id_carga);
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar nuevos eventos para emitir nueva factura adicional con éxito, para proceder a emitir la misma, <br/>por favor dar click en el siguiente link: {0} </b>", link));
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

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox Chk = e.Item.FindControl("chkPase") as CheckBox;
                Label Estado = e.Item.FindControl("LblEstado") as Label;
                Label Estado2 = e.Item.FindControl("LblIn_Out") as Label;
                Label LblCambioturno = e.Item.FindControl("LblCambioturno") as Label;
                Label LblEstadoTransaccion = e.Item.FindControl("LblEstadoTransaccion") as Label;
                bool estado_transaccion = bool.Parse(LblEstadoTransaccion.Text);

               
                Button BtnEvento = e.Item.FindControl("BtnEvento") as Button;
                if (Estado.Text.Equals("EXPIRADO") || estado_transaccion == false || LblCambioturno.Text.Equals("SI"))
                {
                    Chk.Enabled = false;

                    if (Estado.Text.Equals("EXPIRADO"))
                    {
                        BtnEvento.Visible = true;
                    }
                    else
                    {
                        BtnEvento.Visible = false;
                    }

                }
                else {
                    BtnEvento.Visible = false;
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

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
               
                Server.HtmlEncode(this.Txtempresa.Text.Trim());
                Server.HtmlEncode(this.TxtChofer.Text.Trim());
                Server.HtmlEncode(this.TxtPlaca.Text.Trim());
                

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