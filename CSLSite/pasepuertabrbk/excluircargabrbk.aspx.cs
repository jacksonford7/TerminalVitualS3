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
  

    public partial class excluircargabrbk : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaBRBK_Cabecera objPaseBRBK = new Cls_Bil_PasePuertaBRBK_Cabecera();
        //private Cls_Bil_PasePuertaBRBK_SubItems objPaseBRBKTarja = new Cls_Bil_PasePuertaBRBK_SubItems();
        //private Cls_Bil_PasePuertaBRBK_Detalle objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();

    
     
        //private Cls_Bil_PasePuertaBRBK_Temporal objReserva ;
        //private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

   

        private brbk_valida_solicitud objValidaSolicitud = new brbk_valida_solicitud();

        private brbk_excluir_numero_carga objTipo = new brbk_excluir_numero_carga();

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
          
            
            UPBOTONES.Update();
            UPCONTENEDOR.Update();
            UPRETIRADOS.Update();
            UPSALDO.Update();
            UPBODEGA.Update();
            UPPRODUCTO.Update();
           
            UPCAS.Update();
          
            this.UPPAGADO.Update();
          

        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
            this.TXTMSN.Text = string.Empty;
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                this.TXTHSN.Text = string.Format("{0}", "0000");
            }
         
          
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
            Session["ExcluirSolicitudBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
        }

      

        #endregion



        #region "Eventos del Formulario"


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
                                                  NUMERO_CARA = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim().ToUpper(), this.TXTMSN.Text.Trim().ToUpper(), this.TXTHSN.Text.Trim().ToUpper())
                                              });

                       
                        //informacion de N4Middleware
                        var Carga = PasePuerta.PaseWebBRBK.ObtenerCargaExcluirBRBK(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                        if (Carga.Exitoso)
                        {
                            /*query contenedores*/
                            var LinqQuery = (from Tbl in Carga.Resultado.Where(Tbl => !string.IsNullOrEmpty(Tbl.MRN))
                                             select new
                                             {
                                                 ID_PPWEB = Tbl.ID_PPWEB,
                                                 CARGA = string.Format("{0}-{1}-{2}", Tbl.MRN.ToUpper(), Tbl.MSN.ToUpper(), Tbl.HSN.ToUpper()),
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
                                                 ESTADO_PAGO = "SI",
                                                 ID_UNIDAD = (Tbl.ID_UNIDAD == null) ? 0 : Tbl.ID_UNIDAD,
                                                 idProducto = (Tbl.idProducto == null) ? 0 : Tbl.idProducto,
                                                 idItem = (Tbl.idItem == null) ? 0 : Tbl.idItem,
                                                 Tipo_Producto = (Tbl.Tipo_Producto == null) ? string.Empty : Tbl.Tipo_Producto,
                                             });

                            if (LinqQuery != null)
                            {

                                /*left join de contenedores*/
                                var LinqFinal = (from Tbl in LinqQuery
                                                 join Tbl2 in LinqPartidadN4 on Tbl.CARGA equals Tbl2.NUMERO_CARA //into TmpFinal
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
                                    //VALIDO SI LA CARGA YA FUE EXCLUIDA
                                    objValidaSolicitud = new brbk_valida_solicitud();
                                    objValidaSolicitud.MRN = this.TXTMRN.Text.Trim();
                                    objValidaSolicitud.MSN = this.TXTMSN.Text.Trim();
                                    objValidaSolicitud.HSN = this.TXTHSN.Text.Trim();
                                    objValidaSolicitud.RUC = ClsUsuario.ruc;
                                    if (objValidaSolicitud.Valida_Carga_Excluida(out OError))
                                    {
                                        this.BtnGrabar.Attributes.Add("disabled", "disabled");
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", objValidaSolicitud.MENSAJE));
                                        return;
                                    }


                                    //fin de validacion

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
                                                
                                            }

                                        }
                                        else
                                        {
                                            
                                        }
                                    }
                                    else
                                    {

                                       
                                    }



                                    //agrego todos los contenedores a la clase cabecera
                                    objPaseBRBK = Session["ExcluirSolicitudBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
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
                                    objPaseBRBK.PAGADO = true;
                                    
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

                                    this.TxtFechaCas.Text = string.Empty;
                                    

                                    this.TxtContenedorSeleccionado.Text = objPaseBRBK.CANTIDAD.ToString();
                                    this.TxtSaldo.Text = objPaseBRBK.CNTR_CANTIDAD_SALDO.ToString();
                                    this.TxtRetirados.Text = objPaseBRBK.CANTIDAD_EMITIDOS.ToString();
                                    this.TxtBodega.Text = objPaseBRBK.CNTR_UBICACION;
                                    this.TxtTipoProducto.Text = objPaseBRBK.Tipo_Producto;
                                  
                                   
                                    this.TxtSaldo.Text = SALDO_FINAL.ToString();

                                    if (LinqFinal.PAGADO == false)
                                    {
                                        this.TxtPagado.Text = "NO";
                                        
                                    }
                                    else
                                    {
                                        this.TxtPagado.Text = "SI";
                                    }

                                    this.BtnGrabar.Attributes.Remove("disabled");


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

                                    Session["ExcluirSolicitudBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
 
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
                    if (string.IsNullOrEmpty(this.TxtObservaciones.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b><b>Informativo! Por favor ingresar la observación </b>"));
                        this.TxtObservaciones.Focus();
                        return;
                    }

                    //instancia sesion
                    objPaseBRBK = Session["ExcluirSolicitudBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                    if (objPaseBRBK == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para excluir la carga a la modalidad de turnos especiales break bulk. </b>"));
                        return;
                    }

 
                    LoginName = objPaseBRBK.IV_USUARIO_CREA.Trim();

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objTipo = new brbk_excluir_numero_carga();
                    objTipo.ID = 0;
                    objTipo.MRN = objPaseBRBK.MRN;
                    objTipo.MSN = objPaseBRBK.MSN;
                    objTipo.HSN = objPaseBRBK.HSN;
                    objTipo.USUARIO = ClsUsuario.loginname;
                    objTipo.RUC = ClsUsuario.ruc;
                    objTipo.BODEGA = objPaseBRBK.CNTR_UBICACION;
                    objTipo.OBSERVACION = this.TxtObservaciones.Text;
                    objTipo.TIPO_PRODUCTO = (objPaseBRBK.idProducto.HasValue ? objPaseBRBK.idProducto : null);

                  
                    objTipo.Save(out cMensajes);

                    if (cMensajes != string.Empty)
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se puede exluir el número de carga: {0}-{1}-{2}, {3}", objPaseBRBK.MRN , objPaseBRBK.MSN, objPaseBRBK.HSN, cMensajes));
                        return;
                    }
                    else
                    {
                        this.BtnGrabar.Attributes["disabled"] = "disabled";
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Número de carga excluido con éxito..{0}", cMensajes));
                     

                    }




                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Excluir # CARGA BREAK BULK", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }
           


        }

    
      

      

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
              
                Server.HtmlEncode(this.TxtContenedorSeleccionado.Text.Trim());
                Server.HtmlEncode(this.TxtRetirados.Text.Trim());
                Server.HtmlEncode(this.TxtSaldo.Text.Trim());
                Server.HtmlEncode(this.TxtBodega.Text.Trim());
                Server.HtmlEncode(this.TxtObservaciones.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    this.BtnGrabar.Attributes["disabled"] = "disabled";

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}