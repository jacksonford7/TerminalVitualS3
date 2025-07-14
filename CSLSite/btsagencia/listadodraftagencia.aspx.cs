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
using BillionReglasNegocio;
using PasePuerta;

namespace CSLSite
{
    public partial class listadodraftagencia : System.Web.UI.Page
    {
        #region "Clases"
        private BTS_Transmitir_BL Carga = new BTS_Transmitir_BL();

        #endregion

        #region "Variables"

        private string cMensajes;


        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();

        #endregion

    
        #region "Metodos"

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
           
        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
        }

        
        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }


        private void Mostrar_Mensaje(string Mensaje)
        {
           
               
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading();
           

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;   
            this.Actualiza_Paneles();
            OcultarLoading();

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

#if !DEBUG
                this.IsAllowAccess();
#endif

            this.banmsg.Visible = IsPostBack;
          
            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
              
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

                this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
                this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);

                if (!Page.IsPostBack)
                {
                    string desde = DateTime.Today.Month.ToString("D2") + "/01/"  + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

        protected void chkFacturar_CheckedChanged(object sender, EventArgs e)
        {

            

        }

        private void GenerarConsulta()
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
                        OcultarLoading();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFechaDesde.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TxtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    CultureInfo enUS = new CultureInfo("en-US");

                    if (!string.IsNullOrEmpty(TxtFechaDesde.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;

                        }
                    }

                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} \\nNO deber ser mayor a la\\nFecha final: {1}", TxtFechaDesde.Text, TxtFechaDesde.Text));
                        return;
                    }
                    if (diferenciaEnDias > 31)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un mes."));
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    List<BTS_Listado_Facturas> ListFactura = BTS_Listado_Facturas.Datos_Draft(fechadesde, fechahasta, ClsUsuario.loginname, ClsUsuario.ruc, out cMensajes);


                    if (ListFactura == null)
                    {

                        grilla.DataSource = ListFactura;
                        grilla.DataBind();

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                    if (ListFactura.Count <= 0)
                    {

                        grilla.DataSource = ListFactura;
                        grilla.DataBind();

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }

                    grilla.DataSource = ListFactura;
                    grilla.DataBind();


                    this.Actualiza_Paneles();
                    this.Ocultar_Mensaje();


                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                }
            }

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {


            this.GenerarConsulta();

            
        }



        protected void grilla_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
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

                    if (e.CommandName == "Rechazar")
                    {
                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        string Draft = string.Empty;

                        List<BTS_Draf_Impresion> ListFactura = BTS_Draf_Impresion.Datos_Factura(Int64.Parse(t), out cMensajes);
                        if (ListFactura != null)
                        {
                            var x = ListFactura.FirstOrDefault();
                            Draft = x.DRAF;
                        }

                        BTS_Procesa_Draf Update = new BTS_Procesa_Draf();
                        Update.ID = Int64.Parse(t);
                        Update.IV_USUARIO_CREA = ClsUsuario.loginname;

                        var nProceso = Update.SaveTransaction_Anular_Update(out cMensajes);
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo anular el draft # {0} - {1}</b>", Draft, cMensajes));
                            return;
                        }
                        else
                        {
                            this.GenerarConsulta();

                            this.Mostrar_Mensaje(string.Format("<b>Informativo! Draft # {0} anulado con éxito</b>", Draft));
                        }

                    }

                    if (e.CommandName == "Grabar")
                    {
                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        string Draft = string.Empty;
                        string INVOICETYPE = string.Empty;
                        List<String> Lista = new List<String>();

                        List<BTS_Draf_Impresion> ListFactura = BTS_Draf_Impresion.Datos_Factura(Int64.Parse(t), out cMensajes);
                        if (ListFactura != null)
                        {
                            var x = ListFactura.FirstOrDefault();
                            Draft = x.DRAF;
                            INVOICETYPE = x.INVOICETYPE;
                            Lista.Add(Draft.Trim());
                        }

                        /*proceso finalizar draft de factura*/
                        var BillingFin = new N4Ws.Entidad.billing();
                        MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                        Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                        Fin.drftInvoiceNbrs = Lista;
                        Fin.invoiceTypeId = INVOICETYPE;
                        BillingFin.MergeInvoiceRequest = Fin;
                        var Finalizar = Servicios.N4ServicioBasicoMergeAndFinalizeTransaction(BillingFin, ClsUsuario.loginname.Trim());
                        if (Finalizar != null)
                        {
                            var Factura = Finalizar;
                            string NumeroFactura = Factura.response.billInvoice.finalNumber;
                            NumeroFactura = "00" + NumeroFactura;
                            string Establecimiento = NumeroFactura.Substring(0, 3);
                            string PuntoEmision = NumeroFactura.Substring(3, 3);
                            string Original = NumeroFactura.Substring(6, 9);
                            string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);

                            BTS_Procesa_Draf Update = new BTS_Procesa_Draf();
                            Update.ID = Int64.Parse(t);
                            Update.USUARIO_FINALIZA = ClsUsuario.loginname;
                            Update.FACTURA = FacturaFinal;

                            var nProceso = Update.SaveTransaction_Actualizar_Draf(out cMensajes);
                            if (!nProceso.HasValue || nProceso.Value <= 0)
                            {
                                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo actualizar el draft # {0} - asignación de factura... {1}</b>", Draft, cMensajes));
                                return;
                            }
                            else
                            {
                                

                                string mensaje = string.Format("<b>Informativo! Se genero la siguiente factura # {0} con éxito</b>", FacturaFinal);

                                ///*proceso para generar bl*/
                                //List<BTS_Transmitir_BL> BL = BTS_Transmitir_BL.Informacion_BL(Int64.Parse(t), out cMensajes);
                                //if (BL != null)
                                //{
                                //    foreach(var Det in BL)
                                //    {
                                //        string request = string.Empty;
                                //        string response = string.Empty;


                                //        Crear_BL OBJBL = new Crear_BL();
                                //        OBJBL.USUARIO_REGISTRO = Page.User.Identity.Name.ToUpper();

                                //        var Resultado = OBJBL.Generar_BL(Det.NBR, Det.LINE, Det.VISIT, Det.CATEGORY,
                                //            Det.SHIPPER, Det.CONSIGNEE, Det.CARGOFIRST, Det.CARGOSECOND,
                                //            Det.OPERATION, Det.WEIGHT, Det.VOLUME, Det.CANWEIGHT,
                                //            Det.POL, Det.NOTES, Det.ITEMNBR, Det.COMMODITY,
                                //            Det.PRODUCT, Det.PACKAGING, Det.TOTALWEIGHT, Det.QTY,
                                //            Det.MARKS, Det.POSITION);

                                //        if (Resultado.Exitoso)
                                //        {
                                //            Carga = new BTS_Transmitir_BL();
                                //            Carga.NBR = Det.NBR;
                                //            Carga.ID = Det.ID;
                                //            Carga.MENSAJE = "EXITOSO";
                                //            Carga.USER = ClsUsuario.loginname;


                                //            var grabar = Carga.Save(out cMensajes);
                                //            /*fin de nuevo proceso de grabado*/
                                //            if (!grabar.HasValue || grabar.Value <= 0)
                                //            {
                                //                mensaje = mensaje + string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos del BL generado...{0} </b>", cMensajes);
                                //            }
                                //        }
                                //        else
                                //        {
                                //            mensaje = mensaje + string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos del BL generado...{0} </b>", Resultado.MensajeProblema);
                                //        }

                                //        //    Respuesta.ResultadoOperacion<bool> resp;

                                //        // resp = ServicioSCA.CrearBL_BTS(Det.NBR, Det.LINE, Det.VISIT, Det.CATEGORY,
                                //        //    Det.SHIPPER, Det.CONSIGNEE, Det.CARGOFIRST, Det.CARGOSECOND,
                                //        //    Det.OPERATION, Det.WEIGHT, Det.VOLUME, Det.CANWEIGHT,
                                //        //    Det.POL, Det.NOTES, Det.ITEMNBR, Det.COMMODITY,
                                //        //    Det.PRODUCT, Det.PACKAGING, Det.TOTALWEIGHT, Det.QTY,
                                //        //    Det.MARKS, Det.POSITION,
                                //        //    Page.User.Identity.Name.ToUpper(), out request, out response);

                                //        //if (resp.Exitoso)
                                //        //{
                                //        //    Carga = new BTS_Transmitir_BL();
                                //        //    Carga.NBR = Det.NBR;
                                //        //    Carga.ID = Det.ID;
                                //        //    Carga.MENSAJE = resp.MensajeInformacion;
                                //        //    Carga.USER = ClsUsuario.loginname;


                                //        //    var grabar = Carga.Save(out cMensajes);
                                //        //    /*fin de nuevo proceso de grabado*/
                                //        //    if (!grabar.HasValue || grabar.Value <= 0)
                                //        //    {
                                //        //        mensaje = mensaje + string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos del BL generado...{0} </b>", cMensajes);
                                //        //    }
                                //        //}
                                //        //else
                                //        //{

                                //        //}

                                       
                                //    }
                                //}

                                /* fin proceso */
                                this.GenerarConsulta();

                                this.Mostrar_Mensaje(string.Format("{0}", mensaje));
                            }

                        }
                        else
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Problemas al finalizar el DRAFT # {0}", t.ToString()));
                            return;
                        }
                        /*proceso finalizar draft de factura*/
                    }

                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                return;

            }
        }
       
    }
}