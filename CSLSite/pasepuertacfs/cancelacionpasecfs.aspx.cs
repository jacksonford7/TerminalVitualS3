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
  

    public partial class cancelacionpasecfs : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objPaseCFS = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_PasePuertaCFS_SubItems objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
        private Cls_Bil_PasePuertaCFS_Detalle objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();

        private P2D_Traza_Liftif objLogLiftif = new P2D_Traza_Liftif();
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }

      

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;
        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
        private string LoginName = string.Empty;
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
        private Int64 ConsecutivoSelec = 0;

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
            UPBOTONES.Update();

        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
            this.TXTMSN.Text = string.Empty;
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                this.TXTHSN.Text = string.Format("{0}", "0000");
            }
            

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
            Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;
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
                }

                

            }
        }

       
        private void MostrarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader2();", true);
            UPBOTONES.Update();
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

                    objPaseCFS = Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todos los pases a cancelar... </b>"));
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

                    Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

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
                    var Carga = PasePuerta.Pase_CFS.ObtenerListaEditable(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc, ClsUsuario.loginname, null);
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
                                             ORDER_ID = p.Field<string>("ORDER_ID") == null ? "" : p.Field<string>("ORDER_ID").Trim(),
                                             P2D = p.Field<bool>("P2D"),
                                             ENVIADO_LIFTIF = p.Field<bool>("ENVIADO_LIFTIF")
                                         });


                        if (LinqQuery != null)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseCFS = Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                            objPaseCFS.FECHA = DateTime.Now;
                           
                            objPaseCFS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseCFS.SESION = this.hf_BrowserWindowName.Value;

                            objPaseCFS.Detalle.Clear();

                            Int64 pValor = 0;

                            foreach (var Det in LinqQuery)
                            {

                                objPaseCFS.MRN = Det.MRN;
                                objPaseCFS.MSN = Det.MSN;
                                objPaseCFS.HSN = Det.HSN;

                                List<Cls_Bil_PasePuertaCFS_InOut> PaseUsado = Cls_Bil_PasePuertaCFS_InOut.Pase_Utilizado(Int64.Parse(Det.NUMERO_PASE_N4.ToString()), out cMensajes);

                                if (PaseUsado != null)
                                {
                                    pValor = (from Tbl in PaseUsado select new { VALOR = (Tbl.VALOR == 0) ? 0 : Tbl.VALOR }).FirstOrDefault().VALOR;
                                }
                                else { pValor = 0; }


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
                                objDetallePaseCFS.ID_UNIDAD = (Det.ID_UNIDAD==null ? 0 : Det.ID_UNIDAD);
                                
                                objDetallePaseCFS.ESTADO = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseCFS.MOSTRAR_MENSAJE = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseCFS.ORDENAMIENTO = (Det.ESTADO.Equals("GN") ? "1" : "2");
                                objDetallePaseCFS.IN_OUT = Det.IN_OUT;
                                objDetallePaseCFS.ESTADO_TRANSACCION = Det.ESTADO_TRANCCION;
                                objDetallePaseCFS.ORDEN = Det.ORDEN;
                                objDetallePaseCFS.ORDER_ID = Det.ORDER_ID;
                                objDetallePaseCFS.P2D = Det.P2D;
                                objDetallePaseCFS.ENVIADO_LIFTIF = Det.ENVIADO_LIFTIF;

                                /*
                                objDetallePaseCFS.ESTADO =  "EXPIRADO";
                                objDetallePaseCFS.MOSTRAR_MENSAJE ="EXPIRADO";
                                objDetallePaseCFS.ORDENAMIENTO = (Det.ESTADO.Equals("GN") ? "1" : "2");
                                objDetallePaseCFS.IN_OUT = Det.IN_OUT;
                                objDetallePaseCFS.ESTADO_TRANSACCION = Det.ESTADO_TRANCCION;
                                */
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

                                //si no tiene nada pendiente de facturar
                                if (!objDetallePaseCFS.SERVICIO.Value)
                                {

                                    if (Det.CNTR_DD.Value)
                                    {
                                        
                                        objDetallePaseCFS.TIPO_CNTR = string.Format("{0} - {1}", Det.TIPO_CNTR, "Desaduanamiento Directo");
                                    }
                                   
                                    objPaseCFS.Detalle.Add(objDetallePaseCFS);
                                }


                            }

                            tablePagination.DataSource = objPaseCFS.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                            tablePagination.DataBind();

                            Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

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
                    //this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "BtnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                    return;
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

                    this.MostrarLoading();

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
                    objPaseCFS = Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objPaseCFS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder cancelar los pases de carga suelta. </b>"));
                        return;
                    }

                    if (objPaseCFS.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen pases de puertas de carga suelta pendientes para cancelar.  </b>"));
                        return;
                    }

                    var LinqValidaFaltantes = (from p in objPaseCFS.Detalle.Where(x => x.VISTO == false)
                                                       select p.ID_PASE).ToList();

                    if (LinqValidaFaltantes.Count != 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar todos los pases de puerta a cancelar</b>"));
                        return;
                    }

                    
                    LoginName = objPaseCFS.IV_USUARIO_CREA.Trim();

 
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
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se puede cancelar el pase # :{0}, la unidad tiene estado: {1}, desmarque el pase, para continuar con el proceso.. </b>", Det.ID_PASE, Det.MENSAJE));
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
                                         ORDEN = (Tbl.ORDEN == null) ? string.Empty : Tbl.ORDEN,
                                         ORDER_ID = (Tbl.ORDER_ID == null) ? string.Empty : Tbl.ORDER_ID,
                                         P2D = (Tbl.P2D == null) ? false : Tbl.P2D,
                                         ENVIADO_LIFTIF = (Tbl.ENVIADO_LIFTIF == null) ? false : Tbl.ENVIADO_LIFTIF
                                     }).ToList().OrderBy(x => x.CONTENEDOR);


                    //parametros de LIFTIT
                    Configuraciones.ModuloAccesorios Cfgs = new Configuraciones.ModuloAccesorios("LIFTIF");
                    Cfgs.ConfiguracionBase = "DATACON";
                    string pv = string.Empty;
                    if (!Cfgs.Inicializar(out pv))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener configuraciones LIFTIT....{0}</b>", pv));
                        return;
                    }

                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {

                        Pase_CFS pase = new Pase_CFS();
                        pase.ID_PASE = (decimal)Det.ID_PASE;
                        pase.PPW = Det.ID_PPWEB;
                        pase.ESTADO = "CA";
                        pase.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;
                        pase.USUARIO_ESTADO = ClsUsuario.loginname;
                        pase.TIPO_CARGA = "CFS";
                        pase.MOTIVO_CANCELACION = "CAMBIO DE TURNO";

                        var Resultado = pase.Cancelar(ClsUsuario.loginname);
                        if (Resultado.Exitoso)
                        {
                            nTotal++;
                            if (nTotal == 1)
                            {
                                id_carga = securetext(Det.CARGA);
                            }

                            //si fue transmitido a liftit, procede a enviar la cancelacion de la orden
                            if (Det.ENVIADO_LIFTIF.Value && Det.P2D.Value)
                            {
                                string token = string.Empty;
                                string url = string.Empty;

                                

                                var ptoken = Cfgs.ObtenerConfiguracion("TOKEN");
                                var purl = Cfgs.ObtenerConfiguracion("CREAR");
                                token = ptoken == null ? string.Empty : ptoken.valor;
                                url = purl == null ? string.Empty : purl.valor;

                                var Ord = new delete_order(token, url, Det.ORDER_ID);
                                var Result = Ord.WriteApiOrder();
                                if (Result.code.Equals("ERROR"))
                                {

                                    //graba log del servicio
                                    objLogLiftif = new P2D_Traza_Liftif();
                                    objLogLiftif.USUARIO = ClsUsuario.loginname;
                                    objLogLiftif.TOKEN = token;
                                    objLogLiftif.ORIGEN = "cancelacionpasecfs:BtnGrabar_Click";
                                    objLogLiftif.MENSAJE = Result.message;
                                    objLogLiftif.ESTADO = Result.code;
                                    objLogLiftif.PASE = Int64.Parse(pase.ID_PASE.ToString().Trim());

                                    string xerror;
                                    var nProcesoApp = objLogLiftif.SaveTransaction(out xerror);
                                    /*fin de nuevo proceso de grabado*/
                                    if (!nProcesoApp.HasValue || nProcesoApp.Value <= 0)
                                    {

                                    }


                                    //graba para reprocesar
                                    objLogLiftif = new P2D_Traza_Liftif();
                                    objLogLiftif.USUARIO = ClsUsuario.loginname;
                                    objLogLiftif.ACCION = "DEL";
                                    objLogLiftif.PASE = Int64.Parse(pase.ID_PASE.ToString().Trim());

                                    var nProcesoApp2 = objLogLiftif.SaveTransaction_Reprocesar(out xerror);
                                    /*fin de nuevo proceso de grabado*/
                                    if (!nProcesoApp2.HasValue || nProcesoApp2.Value <= 0)
                                    {

                                    }

                                }
                                else
                                {
                                    //graba log del servicio
                                    objLogLiftif = new P2D_Traza_Liftif();
                                    objLogLiftif.USUARIO = ClsUsuario.loginname;
                                    objLogLiftif.TOKEN = token;
                                    objLogLiftif.ORIGEN = "cancelacionpasecfs:BtnGrabar_Click";
                                    objLogLiftif.MENSAJE = Result.message;
                                    objLogLiftif.ESTADO = Result.code;
                                    objLogLiftif.PASE = Int64.Parse(pase.ID_PASE.ToString().Trim());

                                    string xerror;
                                    var nProcesoApp = objLogLiftif.SaveTransaction(out xerror);
                                    /*fin de nuevo proceso de grabado*/
                                    if (!nProcesoApp.HasValue || nProcesoApp.Value <= 0)
                                    {

                                    }

                                }
                            }
                        }
                        else
                        {
                           /*************************************************************************************************************************************
                           * crear caso salesforce
                           ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas:no se pudo cancelar el pase de puerta para la carga: {0}, Total bultos: {1} Existen los siguientes problemas: {2}, {3}", Det.CARGA, Det.CANTIDAD_CARGA, Resultado.MensajeInformacion, Resultado.MensajeProblema);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Error al cancelar pase CFS", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                               "", "", out MensajeCasos, false);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            **************************************************************************************************************************************/

                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo cancelar el pase de puerta para la carga: {0}, Total bultos: {1} Existen los siguientes problemas: {2} </b>", Det.CARGA,Det.CANTIDAD_CARGA , MensajeCasos));
                            return;
                        }

                    }

                    if (nTotal != 0)
                    {
                        
                        //limpiar
                        objPaseCFS.Detalle.Clear();
                        objPaseCFS.DetalleSubItem.Clear();

                        Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                       
                        tablePagination.DataSource = objPaseCFS.Detalle;
                        tablePagination.DataBind();

                        this.Limpia_Campos();

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron con la cancelación de {0} pase de puerta con éxito</b>", nTotal));
                       
                        return;
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Cancelar Pase CFS", false, null, null, ex.StackTrace, ex);
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
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                //actualiza datos del contenedor
                objPaseCFS = Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPase.Checked;

                }

                tablePagination.DataSource = objPaseCFS.Detalle.OrderBy(p => p.ORDENAMIENTO);
                tablePagination.DataBind();

                Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

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

                    if (e.CommandName == "Eliminar")
                    {
                        objPaseCFS = Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;

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

                      
                        tablePagination.DataSource = objPaseCFS.Detalle;
                        tablePagination.DataBind();

                        this.Pintar_Grilla();

                        Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;


                        this.Actualiza_Paneles();

                    }

                    //nuevos servicios
                    if (e.CommandName == "Facturar")
                    {
                        objPaseCFS = Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                        
                        string CARGA = string.Empty;
                        Int64 ID_UNIDAD = 0;
                        bool proceso_ok = false;
                        Dictionary<Int64, string> Lista_Gkeys = new Dictionary<Int64, string>();

                        //marcar a todos
                        foreach (var Det in objPaseCFS.Detalle.Where(p => p.SERVICIO == false))
                        {
                            var Existe = objPaseCFS.Detalle.FirstOrDefault(q => q.ID_PASE.Equals(Det.ID_PASE));
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
                            var Resultado = Pase_CFS.Marcar_Servicio(ClsUsuario.loginname, ID_UNIDAD, Lista_Gkeys);
                            if (!Resultado.Exitoso)
                            {

                                /*************************************************************************************************************************************
                                * crear caso salesforce
                                ***********************************************************************************************************************************/
                                MensajesErrores = string.Format("Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: {1}, {2} </b>", CARGA, Resultado.MensajeInformacion, Resultado.MensajeProblema);

                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Error al cancelar pase CFS", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                   "", "", out MensajeCasos, false);

                                /*************************************************************************************************************************************
                                * fin caso salesforce
                                **************************************************************************************************************************************/

                                // this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo cancelar el pase de puerta para la carga: {0}, Total bultos: {1} Existen los siguientes problemas: {2} </b>", Det.CARGA, Det.CANTIDAD_CARGA, MensajeCasos));
                                // return;

                                this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: {1} </b>", CARGA, MensajeCasos));
                                return;
                            }
                            else
                            {
                                //caso de que tenga el servicio de p2d
                                var Existe = objPaseCFS.Detalle.FirstOrDefault(q => q.ID_PASE.Equals(double.Parse(t)) && q.P2D == true && q.ENVIADO_LIFTIF==true);
                                if (Existe != null)
                                {
                                    //parametros de LIFTIT
                                    Configuraciones.ModuloAccesorios Cfgs = new Configuraciones.ModuloAccesorios("LIFTIF");
                                    Cfgs.ConfiguracionBase = "DATACON";
                                    string pv = string.Empty;
                                    if (!Cfgs.Inicializar(out pv))
                                    {
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener configuraciones LIFTIT....{0}</b>", pv));
                                        return;
                                    }

                                    string token = string.Empty;
                                    string url = string.Empty;

                                    var ptoken = Cfgs.ObtenerConfiguracion("TOKEN");
                                    var purl = Cfgs.ObtenerConfiguracion("CREAR");
                                    token = ptoken == null ? string.Empty : ptoken.valor;
                                    url = purl == null ? string.Empty : purl.valor;

                                    var Ord = new delete_order(token, url, Existe.ORDER_ID);
                                    var Result = Ord.WriteApiOrder();
                                    if (Result.code.Equals("ERROR"))
                                    {

                                        //graba log del servicio
                                        objLogLiftif = new P2D_Traza_Liftif();
                                        objLogLiftif.USUARIO = ClsUsuario.loginname;
                                        objLogLiftif.TOKEN = token;
                                        objLogLiftif.ORIGEN = "cancelacionpasecfs:tablePagination_ItemCommand";
                                        objLogLiftif.MENSAJE = Result.message;
                                        objLogLiftif.ESTADO = Result.code;
                                        objLogLiftif.PASE = Int64.Parse(Existe.ID_PASE.ToString().Trim());

                                        string xerror;
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
                                        objLogLiftif.USUARIO = ClsUsuario.loginname;
                                        objLogLiftif.TOKEN = token;
                                        objLogLiftif.ORIGEN = "cancelacionpasecfs:tablePagination_ItemCommand";
                                        objLogLiftif.MENSAJE = Result.message;
                                        objLogLiftif.ESTADO = Result.code;
                                        objLogLiftif.PASE = Int64.Parse(Existe.ID_PASE.ToString().Trim());

                                        string xerror;
                                        var nProcesoApp = objLogLiftif.SaveTransaction(out xerror);
                                        /*fin de nuevo proceso de grabado*/
                                        if (!nProcesoApp.HasValue || nProcesoApp.Value <= 0)
                                        {

                                        }

                                    }

                                }
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: No existen registros de pases </b>", CARGA));
                            return;
                        }

                        tablePagination.DataSource = objPaseCFS.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                        tablePagination.DataBind();

                        Session["CanPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;


                        string id_carga = securetext(CARGA.Replace("-", "+"));
                        string link = string.Format("<a href='../cargacfs/facturacioncfsadicional.aspx?ID_CARGA={0}' target ='_parent'>Facturar E-Pass Vencido CFS</a>", id_carga);
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar nuevos eventos para emitir nueva factura adicional con éxito, para proceder a emitir la misma, <br/>por favor dar click en el siguiente link: {0} </b>", link));
                        return;
                        
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
                    else { BtnEvento.Visible = false; }

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