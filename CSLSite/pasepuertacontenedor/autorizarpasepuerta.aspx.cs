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

using System.Data;
using System.Web.Script.Services;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using PasePuerta;
using CSLSite;

namespace CSLSite
{
  

    public partial class autorizarpasepuerta : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaContenedor_Cabecera objPaseContenedor = new Cls_Bil_PasePuertaContenedor_Cabecera();
        private Cls_Bil_PasePuertaContenedor_Detalle objDetallePaseContenedor = new Cls_Bil_PasePuertaContenedor_Detalle();
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

        //private Int64 Gkey = 0;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        //private int Fila = 1;
        private string TipoServicio = string.Empty;

       
        private DateTime FechaFacturaHasta;
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
        private string Tipo_Contenedor = string.Empty;
        private DateTime FechaActualSalida;
        private DateTime FechaMenosHoras;
        private TimeSpan HorasDiferencia;
        private int TotalHoras = 0;
        private DateTime FechaInicial;
        private DateTime FechaFinal;
        private string ContenedorSelec = string.Empty;
        private string EmpresaSelect = string.Empty;
        private string ChoferSelect = string.Empty;
        private string PlacaSelect = string.Empty;
        private string TurnoSelect = string.Empty;
        private DateTime FechaTurnoInicio;
        private DateTime FechaTurnoFinal;
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

        private void Actualiza_Paneles()
        {
            UPCARGA.Update();
           
            UPBOTONES.Update();
        

            UpTipoCarga.Update();
            UpTipoAutorizacion.Update();

        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
            this.TXTMSN.Text = string.Empty;
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                this.TXTHSN.Text = string.Format("{0}", "0000");
            }
          
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
            objPaseContenedor = new Cls_Bil_PasePuertaContenedor_Cabecera();
            Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
        }

        private void Turno_Default()
        {
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });
           
        }

        private void Pintar_Grilla()
        {
            foreach (RepeaterItem xitem in tablePagination.Items)
            {
                CheckBox ChkVisto = xitem.FindControl("chkPase") as CheckBox;

                Label LblCarga = xitem.FindControl("LblCarga") as Label;
                Label LblContenedor = xitem.FindControl("LblContenedor") as Label;
                Label LblFechaSalida = xitem.FindControl("LblFechaSalida") as Label;
                Label LblTipo = xitem.FindControl("LblTipo") as Label;
                Label LblTurno = xitem.FindControl("LblTurno") as Label;
                Label LblEmpresa = xitem.FindControl("LblEmpresa") as Label;
                Label LblChofer = xitem.FindControl("LblChofer") as Label;
                Label LblPlaca = xitem.FindControl("LblPlaca") as Label;
                Label LblFechaturno = xitem.FindControl("LblFechaturno") as Label;
                Label LblEstado = xitem.FindControl("LblEstado") as Label;
                if (ChkVisto.Checked == true)
                {

                    LblCarga.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblContenedor.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblFechaSalida.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblTipo.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblTurno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEmpresa.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblChofer.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblPlaca.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblFechaturno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEstado.ForeColor = System.Drawing.Color.PaleVioletRed;
                }

            }
        }
      
        #endregion



        #region "Eventos del Formulario"

      

      

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    this.BtnGrabar.Attributes.Remove("disabled");
                    this.Actualiza_Paneles();

                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (this.CboTipoCarga.SelectedIndex == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor seleccionar el tipo de carga a realizar la autorización"));
                        this.CboTipoCarga.Focus();
                        return;
                    }

                    switch (this.CboTipoAutorizacion.SelectedIndex)
                    {
                        case 0://credito

                            if (string.IsNullOrEmpty(this.TXTMRN.Text) && string.IsNullOrEmpty(this.TXTMSN.Text) && string.IsNullOrEmpty(this.TXTHSN.Text) && string.IsNullOrEmpty(this.TxtFactura.Text))
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de la carga o factura"));
                                this.TXTMRN.Focus();
                                return;
                            }

                            break;

                        case 1://sin factura
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
                            break;

                        case 2://cambio de fecha

                            if (string.IsNullOrEmpty(this.TXTMRN.Text) && string.IsNullOrEmpty(this.TXTMSN.Text) && string.IsNullOrEmpty(this.TXTHSN.Text) && string.IsNullOrEmpty(this.TxtContenedor.Text))
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de la carga o contenedor"));
                                this.TXTMRN.Focus();
                                return;
                            }
                            break;
                    }

                    string cContenedor = string.IsNullOrEmpty(this.TxtContenedor.Text) ? null : this.TxtContenedor.Text.Trim();
                    string cFactura = string.IsNullOrEmpty(this.TxtFactura.Text) ? null : this.TxtFactura.Text.Trim();


                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Contenedores = PasePuerta.Pase_Web.ObtenerCargaPaseAutorizar(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), CboTipoCarga.SelectedValue.ToString(), cContenedor, cFactura);

                    if (Contenedores.Exitoso)
                    {


                        /*query contenedores*/
                        var LinqQuery = (from Tbl in Contenedores.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.CONTENEDOR))
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
                                             CIATRANS = (Tbl.CIATRANS == null) ? string.Empty : Tbl.CIATRANS,
                                             CHOFER = (Tbl.CHOFER == null) ? string.Empty : Tbl.CHOFER,
                                             PLACA = (Tbl.PLACA == null) ? string.Empty : Tbl.PLACA,
                                             CAS = (Tbl.CAS.HasValue ? Tbl.CAS : null),
                                             CNTR_DD = (Tbl.CNTR_DD == null) ? false : Tbl.CNTR_DD,
                                             AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? string.Empty : Tbl.AGENTE_DESC,
                                             FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                             IMPORTADOR = (Tbl.IMPORTADOR == null) ? string.Empty : Tbl.IMPORTADOR,
                                             IMPORTADOR_DESC = (Tbl.IMPORTADOR_DESC == null) ? string.Empty : Tbl.IMPORTADOR_DESC,
                                             FECHA_SALIDA = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                             FECHA_SALIDA_PASE = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
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
                                             USUARIO_ING = ClsUsuario.loginname,
                                             USUARIO_MOD = ClsUsuario.loginname,
                                             ESTADO_PAGO = (Tbl.PAGADO==true ? "SI" : "NO"),
                                             TIPO_CONSULTA = (Tbl.TIPO_CONSULTA == null) ? string.Empty : Tbl.TIPO_CONSULTA
                                         }).ToList().OrderBy(x => x.CONTENEDOR);

                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;

                            objPaseContenedor.FECHA = DateTime.Now;
                            objPaseContenedor.MRN = this.TXTMRN.Text;
                            objPaseContenedor.MSN = this.TXTMSN.Text;
                            objPaseContenedor.HSN = this.TXTHSN.Text;
                            objPaseContenedor.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseContenedor.SESION = this.hf_BrowserWindowName.Value;


                            objPaseContenedor.Detalle.Clear();

                            foreach (var Det in LinqQuery)
                            {

                                objDetallePaseContenedor = new Cls_Bil_PasePuertaContenedor_Detalle();
                                objDetallePaseContenedor.ID_PPWEB = Det.ID_PPWEB;
                                objDetallePaseContenedor.MRN = Det.MRN;
                                objDetallePaseContenedor.MSN = Det.MSN;
                                objDetallePaseContenedor.HSN = Det.HSN;
                                objDetallePaseContenedor.CARGA = Det.CARGA;
                                objDetallePaseContenedor.FACTURA = Det.FACTURA;
                                objDetallePaseContenedor.AGENTE = Det.AGENTE;
                                objDetallePaseContenedor.FACTURADO = Det.FACTURADO;
                                objDetallePaseContenedor.PAGADO = Det.PAGADO;
                                objDetallePaseContenedor.GKEY = Det.GKEY;
                                objDetallePaseContenedor.REFERENCIA = Det.REFERENCIA;
                                objDetallePaseContenedor.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseContenedor.DOCUMENTO = Det.DOCUMENTO;
                                //objDetallePaseContenedor.CIATRANS = Det.CIATRANS;
                                //objDetallePaseContenedor.CHOFER = Det.CHOFER;
                                //objDetallePaseContenedor.PLACA = Det.PLACA;
                                objDetallePaseContenedor.CIATRANS = string.Empty;
                                objDetallePaseContenedor.CHOFER = string.Empty;
                                objDetallePaseContenedor.PLACA = string.Empty;
                                objDetallePaseContenedor.CAS = Det.CAS;
                                objDetallePaseContenedor.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objDetallePaseContenedor.CNTR_DD = Det.CNTR_DD.Value;
                                objDetallePaseContenedor.AGENTE_DESC = Det.AGENTE_DESC;
                                objDetallePaseContenedor.FACTURADO_DESC = Det.FACTURADO_DESC;
                                objDetallePaseContenedor.IMPORTADOR = Det.IMPORTADOR;
                                objDetallePaseContenedor.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;
                                //desaduanamiento directo 
                                if (Det.CNTR_DD.Value)
                                {
                                    //objDetallePaseContenedor.FECHA_SALIDA_PASE = null;
                                    objDetallePaseContenedor.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                                }
                                else
                                {
                                    objDetallePaseContenedor.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                                }
                               
                                objDetallePaseContenedor.FECHA_AUT_PPWEB = Det.FECHA_AUT_PPWEB;
                                objDetallePaseContenedor.HORA_AUT_PPWEB = Det.HORA_AUT_PPWEB;

                                objDetallePaseContenedor.TIPO_CNTR = Det.TIPO_CNTR;
                                objDetallePaseContenedor.ID_TURNO = Det.ID_TURNO;
                                objDetallePaseContenedor.TURNO = Det.TURNO;
                                objDetallePaseContenedor.D_TURNO = Det.D_TURNO;
                                objDetallePaseContenedor.ID_PASE = Det.ID_PASE;
                                objDetallePaseContenedor.ESTADO = Det.ESTADO;
                                objDetallePaseContenedor.ENVIADO = Det.ENVIADO;
                                objDetallePaseContenedor.AUTORIZADO = Det.AUTORIZADO;
                                objDetallePaseContenedor.VENTANILLA = Det.VENTANILLA;
                                objDetallePaseContenedor.USUARIO_ING = Det.USUARIO_ING;
                                objDetallePaseContenedor.FECHA_ING = System.DateTime.Now.Date;
                                objDetallePaseContenedor.USUARIO_MOD = Det.USUARIO_MOD;
                                objDetallePaseContenedor.ESTADO_PAGO = Det.ESTADO_PAGO;

                                objDetallePaseContenedor.TIPO_CONSULTA = Det.TIPO_CONSULTA;

                                if (Det.CNTR_DD.Value)
                                {
                                    objDetallePaseContenedor.TIPO_CNTR = string.Format("{0} - {1}",Det.TIPO_CNTR, "Desaduanamiento Directo");
                                }
                                
                                objPaseContenedor.Detalle.Add(objDetallePaseContenedor);

                            }

                            tablePagination.DataSource = objPaseContenedor.Detalle;
                            tablePagination.DataBind();

                            Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
                            
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                        }


                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", Contenedores.MensajeProblema));
                       
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
                    if (string.IsNullOrEmpty(this.TxtComentario.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b><b>Informativo! Por favor ingresar un comentario</b>"));
                        this.TxtComentario.Focus();
                        return;
                    }


                    //instancia sesion
                    objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                    if (objPaseContenedor == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe generar la consulta, para poder autorizar los pase de puerta </b>"));
                        return;
                    }
                    if (objPaseContenedor.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! No existe detalle de cargas, para poder autorizar los pase de puerta </b>"));
                        return;
                    }

                    var LinqListContenedor = (from p in objPaseContenedor.Detalle.Where(x => x.VISTO == true)
                                              select p.CONTENEDOR).ToList();

                    if (LinqListContenedor.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar los pases a generar la autorización </b>"));
                        return;
                    }


                    LoginName = objPaseContenedor.IV_USUARIO_CREA.Trim();

                    /*contenedores seleccionados para emitir pase*/
                    var LinqQuery = (from Tbl in objPaseContenedor.Detalle.Where(Tbl => !String.IsNullOrEmpty(Tbl.CONTENEDOR) && Tbl.VISTO == true)
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
                                         CIATRANS = (Tbl.CIATRANS == null) ? string.Empty : Tbl.CIATRANS,
                                         CHOFER = (Tbl.CHOFER == null) ? string.Empty : Tbl.CHOFER,
                                         PLACA = (Tbl.PLACA == null) ? string.Empty : Tbl.PLACA,
                                         CAS = (Tbl.CAS.HasValue ? Tbl.CAS : null),
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
                                         ID_CHOFER = (Tbl.ID_CHOFER == null) ? string.Empty : Tbl.ID_CHOFER,
                                         ID_CIATRANS = (Tbl.ID_CIATRANS == null) ? string.Empty : Tbl.ID_CIATRANS,
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
                                         TIPO_CONSULTA = (Tbl.TIPO_CONSULTA == null) ? string.Empty : Tbl.TIPO_CONSULTA
                                     }).ToList().OrderBy(x => x.CONTENEDOR);

                    
                    this.BtnGrabar.Attributes["disabled"] = "disabled";
                    this.UPBOTONES.Update();

                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/
                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {

                        Pase_Container pase = new Pase_Container();
                        pase.ID_CARGA = Det.GKEY;
                        pase.PPW = Det.ID_PPWEB;

                        var Resultado = pase.Autorizar(LoginName, Det.TIPO_CONSULTA, this.TxtComentario.Text.Trim(),  Det.ID_PPWEB, Det.CONTENEDOR);
                      
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
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo liberar el pase de puerta para el contenedor: {0}, Existen los siguientes problemas: {1}, {2} </b>", Det.CONTENEDOR, Resultado.MensajeInformacion, Resultado.MensajeProblema));
                            return;
                        }

                    }

                    if (nTotal != 0)
                    {
                       
                        //limpiar
                        objPaseContenedor.Detalle.Clear();
                        Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
                        tablePagination.DataSource = objPaseContenedor.Detalle;
                        tablePagination.DataBind();

                        this.Limpia_Campos();

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron con la autorización de {0} pase de puerta con éxito</b>", nTotal));
                        return;
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Grabar", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }
           


        }

        #region "Eventos de la grilla"

        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label LblContenedor = (Label)item.FindControl("LblContenedor");

                ContenedorSelec = LblContenedor.Text;
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                //actualiza datos del contenedor
                objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPase.Checked;
                   
                }

                tablePagination.DataSource = objPaseContenedor.Detalle;
                tablePagination.DataBind();

                Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;

                this.Pintar_Grilla();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }


        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
           
        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox Chk = e.Item.FindControl("chkPase") as CheckBox;
                Label Estado = e.Item.FindControl("LblEstado") as Label;
             
                if (Estado.Text.Equals("SI"))
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
               
                Server.HtmlEncode(this.TxtFactura.Text.Trim());
                Server.HtmlEncode(this.TxtContenedor.Text.Trim());
                Server.HtmlEncode(this.TxtComentario.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    CboTipoAutorizacion_SelectedIndexChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


        protected void CboTipoCarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.TxtFactura.Text = string.Empty;
            this.TxtContenedor.Text = string.Empty;
            this.TxtComentario.Text = string.Empty;
            this.TXTMRN.Text = string.Empty;
            this.TXTMSN.Text = string.Empty;
            this.TXTHSN.Text = "0000";

            this.Actualiza_Paneles();

        }
        protected void CboTipoAutorizacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CboTipoAutorizacion.SelectedIndex != -1)
            {

                switch (this.CboTipoAutorizacion.SelectedIndex)
                {
                    case 0:
                        this.TxtContenedor.Text = string.Empty;
                        this.TxtFactura.Text = string.Empty;

                        this.TxtContenedor.Attributes["disabled"] = "disabled";
                        this.TxtFactura.Attributes.Remove("disabled");
                        break;

                    case 1:
                        this.TxtContenedor.Text = string.Empty;
                        this.TxtFactura.Text = string.Empty;

                        this.TxtContenedor.Attributes["disabled"] = "disabled";
                        this.TxtFactura.Attributes["disabled"] = "disabled";
                       
                        break;

                    case 2:
                        this.TxtContenedor.Text = string.Empty;
                        this.TxtFactura.Text = string.Empty;

                        this.TxtContenedor.Attributes.Remove("disabled");
                        this.TxtFactura.Attributes["disabled"] = "disabled";

                        break;

                }
                
            }

            this.Actualiza_Paneles();

        }

    }

}