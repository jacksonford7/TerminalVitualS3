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
  

    public partial class cancelacionpasebrbk : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaBRBK_Cabecera objPaseBRBK = new Cls_Bil_PasePuertaBRBK_Cabecera();
        private Cls_Bil_PasePuertaBRBK_SubItems objPaseBRBKTarja = new  Cls_Bil_PasePuertaBRBK_SubItems();
        private Cls_Bil_PasePuertaBRBK_Detalle objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();

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
            objPaseBRBK = new Cls_Bil_PasePuertaBRBK_Cabecera();
            Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
        }


        private void Pintar_Grilla()
        {
            //foreach (RepeaterItem xitem in tablePagination.Items)
            //{
            //    CheckBox ChkVisto = xitem.FindControl("chkPase") as CheckBox;

            //    Label LblPase = xitem.FindControl("LblPase") as Label;
            //    Label LblCantidad = xitem.FindControl("LblCantidad") as Label;
            //    Label LblFechaSalida = xitem.FindControl("LblFechaSalida") as Label;
            //    Label LblTurno = xitem.FindControl("LblTurno") as Label;
            //    Label LblEmpresa = xitem.FindControl("LblEmpresa") as Label;
            //    Label LblChofer = xitem.FindControl("LblChofer") as Label;
            //    Label LblPlaca = xitem.FindControl("LblPlaca") as Label;
            //    Label LblFechaturno = xitem.FindControl("LblFechaturno") as Label;
            //    Label LblEstado = xitem.FindControl("LblEstado") as Label;
            //    Label LblMensaje = xitem.FindControl("LblMensaje") as Label;
            //    Label LblIdSolicitud = xitem.FindControl("LblIdSolicitud") as Label;
            //    if (ChkVisto.Checked == true || LblEstado.Text == "EXPIRADO" || !LblIdSolicitud.Text.Equals("0"))
            //    {

            //        LblPase.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblCantidad.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblFechaSalida.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblTurno.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblEmpresa.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblChofer.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblPlaca.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblFechaturno.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblEstado.ForeColor = System.Drawing.Color.PaleVioletRed;
            //        LblMensaje.ForeColor = System.Drawing.Color.PaleVioletRed;
            //    }

                

            //}
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

                    objPaseBRBK = Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todos los pases a cancelar... </b>"));
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

                    Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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
                                             ID_CHOFER = p.Field<string>("ID_CHOFER") == null ? "" : p.Field<string>("ID_CHOFER").Trim(),
                                             ID_EMPRESA = p.Field<string>("ID_EMPRESA") == null ? "" : p.Field<string>("ID_EMPRESA").Trim(),
                                             ID_PLACA = p.Field<string>("ID_PLACA") == null ? "" : p.Field<string>("ID_PLACA").Trim(),
                                             ID_CARGA = p.Field<Int64>("GKEY"),
                                             IN_OUT = (Final == null) ? "" : Final.UBICACION,
                                             PATIO = (Final == null) ? "" : Final.MENSAJE,
                                             ESTADO_TRANCCION = (Final == null) ? true : Final.ESTADO,
                                             ID_UNIDAD = p.Field<Int64?>("ID_UNIDAD"),
                                             ID_SOL = p.Field<Int64?>("ID_SOL"),
                                             SECUENCIA_SOL = p.Field<Int64?>("SECUENCIA_SOL")
                                             //ORDER_ID = p.Field<string>("ORDER_ID") == null ? "" : p.Field<string>("ORDER_ID").Trim()
                                         });


                        if (LinqQuery != null)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseBRBK = Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                            objPaseBRBK.FECHA = DateTime.Now;
                           
                            objPaseBRBK.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseBRBK.SESION = this.hf_BrowserWindowName.Value;

                            objPaseBRBK.Detalle.Clear();

                            Int64 pValor = 0;

                            foreach (var Det in LinqQuery)
                            {

                                objPaseBRBK.MRN = Det.MRN;
                                objPaseBRBK.MSN = Det.MSN;
                                objPaseBRBK.HSN = Det.HSN;

                                List<Cls_Bil_PasePuertaBRBK_InOut> PaseUsado = Cls_Bil_PasePuertaBRBK_InOut.Pase_Utilizado(Int64.Parse(Det.NUMERO_PASE_N4.ToString()), out cMensajes);

                                if (PaseUsado != null)
                                {
                                    pValor = (from Tbl in PaseUsado select new { VALOR = (Tbl.VALOR == 0) ? 0 : Tbl.VALOR }).FirstOrDefault().VALOR;
                                }
                                else { pValor = 0; }


                                objPaseBRBK.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objPaseBRBK.FECHA_SALIDA_PASE = Det.FECHA_PASE;

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

                                objDetallePaseBRBK.FECHA_SALIDA = Det.FECHA_SALIDA;
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
                                //objDetallePaseBRBK.SUB_SECUENCIA = Det.SUB_SECUENCIA;
                                objDetallePaseBRBK.CIATRANS = string.Format("{0} - {1}", Det.ID_EMPRESA, Det.TRANSPORTISTA_DESC);
                                objDetallePaseBRBK.CHOFER = (!string.IsNullOrEmpty(Det.ID_CHOFER) ? string.Format("{0} - {1}", Det.ID_CHOFER, Det.CHOFER_DESC) : string.Empty);
                                objDetallePaseBRBK.PLACA = Det.ID_PLACA;
                                objDetallePaseBRBK.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                                objDetallePaseBRBK.CHOFER_DESC = Det.CHOFER_DESC;
                                objDetallePaseBRBK.LLAVE = Det.ID_PASE.ToString();
                                objDetallePaseBRBK.ID_UNIDAD = (Det.ID_UNIDAD==null ? 0 : Det.ID_UNIDAD);
                                
                                objDetallePaseBRBK.ESTADO = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseBRBK.MOSTRAR_MENSAJE = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseBRBK.ORDENAMIENTO = (Det.ESTADO.Equals("GN") ? "1" : "2");
                                objDetallePaseBRBK.IN_OUT = Det.IN_OUT;
                                objDetallePaseBRBK.ESTADO_TRANSACCION = Det.ESTADO_TRANCCION;

                                objDetallePaseBRBK.ID_SOL = Det.ID_SOL.HasValue ? Det.ID_SOL : 0;
                                objDetallePaseBRBK.SECUENCIA_SOL = Det.SECUENCIA_SOL.HasValue ? Det.SECUENCIA_SOL : 0;

                                //objDetallePaseBRBK.ORDEN = Det.ORDEN;
                                //objDetallePaseBRBK.ORDER_ID = Det.ORDER_ID;
                                //objDetallePaseBRBK.P2D = Det.P2D;
                                //objDetallePaseBRBK.ENVIADO_LIFTIF = Det.ENVIADO_LIFTIF;

                                /*
                                objDetallePaseBRBK.ESTADO =  "EXPIRADO";
                                objDetallePaseBRBK.MOSTRAR_MENSAJE ="EXPIRADO";
                                objDetallePaseBRBK.ORDENAMIENTO = (Det.ESTADO.Equals("GN") ? "1" : "2");
                                objDetallePaseBRBK.IN_OUT = Det.IN_OUT;
                                objDetallePaseBRBK.ESTADO_TRANSACCION = Det.ESTADO_TRANCCION;
                                */
                                if (pValor == 1)
                                {
                                    objDetallePaseBRBK.MOSTRAR_MENSAJE = string.Format("{0} - {1} {2} {3}", objDetallePaseBRBK.ESTADO, "PASE UTILIZADO", (!string.IsNullOrEmpty(Det.PATIO) ? string.Format(" - {0}", Det.PATIO) : string.Empty),
                                         (Det.ID_SOL.HasValue ? (Det.ID_SOL.Value != 0 ? string.Format(" - SOLICITUD # {0}", Det.ID_SOL.Value) : "") : ""));
                                    objDetallePaseBRBK.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseBRBK.ORDENAMIENTO, "1");
                                    objDetallePaseBRBK.ESTADO_TRANSACCION = false;
                                }
                                else
                                {
                                    objDetallePaseBRBK.MOSTRAR_MENSAJE = string.Format("{0} - {1} {2} {3}", objDetallePaseBRBK.ESTADO, "PASE SIN USAR", (!string.IsNullOrEmpty(Det.PATIO) ? string.Format(" - {0}", Det.PATIO) : string.Empty),
                                         (Det.ID_SOL.HasValue ? (Det.ID_SOL.Value != 0 ? string.Format(" - SOLICITUD # {0}", Det.ID_SOL.Value) : "") : ""));
                                    objDetallePaseBRBK.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseBRBK.ORDENAMIENTO, "2");
                                }


                                objDetallePaseBRBK.CAMBIO_TURNO = "NO";
                                objDetallePaseBRBK.SERVICIO = Det.SERVICIO;

                                //si no tiene nada pendiente de facturar
                                if (!objDetallePaseBRBK.SERVICIO.Value)
                                {

                                    if (Det.CNTR_DD.Value)
                                    {
                                        
                                        objDetallePaseBRBK.TIPO_CNTR = string.Format("{0} - {1}", Det.TIPO_CNTR, "Desaduanamiento Directo");
                                    }

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

                                    //objPaseBRBK.Detalle.Add(objDetallePaseBRBK);
                                }


                            }

                            tablePagination.DataSource = objPaseBRBK.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                            tablePagination.DataBind();

                            Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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
                    objPaseBRBK = Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder cancelar los pases de carga suelta. </b>"));
                        return;
                    }

                    if (objPaseBRBK.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen pases de puertas de carga suelta pendientes para cancelar.  </b>"));
                        return;
                    }

                    var LinqValidaFaltantes = (from p in objPaseBRBK.Detalle.Where(x => x.VISTO == true)
                                                       select p.ID_PASE).ToList();

                    if (LinqValidaFaltantes.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los pases de puerta a cancelar</b>"));
                        return;
                    }

                    
                    LoginName = objPaseBRBK.IV_USUARIO_CREA.Trim();

 
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
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se puede cancelar el pase # :{0}, la unidad tiene estado: {1}, desmarque el pase, para continuar con el proceso.. </b>", Det.ID_PASE, Det.MENSAJE));
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
                                         SUB_SECUENCIA = (Tbl.SUB_SECUENCIA == null) ? string.Empty : Tbl.SUB_SECUENCIA,
                                         NUMERO_PASE_N4 = (Tbl.NUMERO_PASE_N4 == null) ? "0" : Tbl.NUMERO_PASE_N4,
                                         ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD
                                     }).ToList().OrderBy(x => x.CONTENEDOR);


                    
                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {

                        Pase_BRBK pase = new Pase_BRBK();
                        pase.ID_PASE = (decimal)Det.ID_PASE;
                        pase.PPW = Det.ID_PPWEB;
                        pase.ESTADO = "CA";
                        pase.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;
                        pase.USUARIO_ESTADO = ClsUsuario.loginname;
                        pase.TIPO_CARGA = "BRBK";
                        pase.MOTIVO_CANCELACION = "CAMBIO DE TURNO";

                        var Resultado = pase.Cancelar(ClsUsuario.loginname);
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
                           /*************************************************************************************************************************************
                           * crear caso salesforce
                           ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas:no se pudo cancelar el pase de puerta para la carga: {0}, Total bultos: {1} Existen los siguientes problemas: {2}, {3}", Det.CARGA, Det.CANTIDAD_CARGA, Resultado.MensajeInformacion, Resultado.MensajeProblema);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación BREAK BULK", "Error al cancelar pase BREAK BULK", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
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
                        objPaseBRBK.Detalle.Clear();
                        objPaseBRBK.DetalleSubItem.Clear();

                        Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                       
                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        this.Limpia_Campos();

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron con la cancelación de {0} pase de puerta con éxito</b>", nTotal));
                       
                        return;
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Cancelar Pase BREAK BULK", false, null, null, ex.StackTrace, ex);
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
                objPaseBRBK = Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.ID_PASE.Equals(ID_PASE));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPase.Checked;

                }

                tablePagination.DataSource = objPaseBRBK.Detalle.OrderBy(p => p.ORDENAMIENTO);
                tablePagination.DataBind();

                Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

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


                    if (e.CommandName == "Eliminar")
                    {
                        objPaseBRBK = Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;

                        //existe pase a remover
                        var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.LLAVE.Equals(t.ToString()));
                        if (Detalle != null)
                        {
                            string Llave = Detalle.LLAVE;
                            //remover pase
                            objPaseBRBK.Detalle.Remove(objPaseBRBK.Detalle.Where(p => p.LLAVE == Llave).FirstOrDefault());

                          
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información del pase: {0} </b>", t.ToString()));
                            return;
                        }

                      
                        tablePagination.DataSource = objPaseBRBK.Detalle;
                        tablePagination.DataBind();

                        this.Pintar_Grilla();

                        Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;


                        this.Actualiza_Paneles();

                    }

                    //nuevos servicios
                    if (e.CommandName == "Facturar")
                    {
                        objPaseBRBK = Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                        
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

                        Session["CanPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;


                        string id_carga = securetext(CARGA.Replace("-", "+"));
                        string link = string.Format("<a href='../cargabrbk/facturacionbrbk.aspx?ID_CARGA={0}' target ='_parent'>Facturar BREAK BULK</a>", id_carga);
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar nuevos eventos para emitir nueva factura adicional con éxito, para proceder a emitir la misma, <br/>por favor ir a la opción de IMPO BRBK para generar la factura. </b>"));
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
                Label LblIdSolicitud = e.Item.FindControl("LblIdSolicitud") as Label;
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
                    if (!LblIdSolicitud.Text.Equals("0"))
                    {
                        Chk.Enabled = false;

                        if (Estado.Text.Equals("EXPIRADO"))
                        {
                            BtnEvento.Visible = true;
                        }
                        else
                        {
                            BtnEvento.Visible = false;
                            Chk.Enabled = true;
                        }
                        //BtnEvento.Visible = true;
                    }
                    else
                    {
                        Chk.Enabled = true;
                        BtnEvento.Visible = false;
                    }
                   
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