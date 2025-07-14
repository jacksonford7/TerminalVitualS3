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

using System.Net;
using Microsoft.Reporting.WebForms;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using CSLSite;

namespace CSLSite
{

    public partial class imprimirpasebrbk : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;    
        private Cls_Bil_PasePuertaBRBK_Cabecera objPaseBRBK = new Cls_Bil_PasePuertaBRBK_Cabecera();
        private Cls_Bil_PasePuertaBRBK_SubItems objPaseBRBKTarja = new Cls_Bil_PasePuertaBRBK_SubItems();
        private Cls_Bil_PasePuertaBRBK_Detalle objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();


        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;
 
        private string LoginName = string.Empty;
        private string Tipo_Contenedor = string.Empty;
       
        private string ContenedorSelec = string.Empty;
       
       
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
            UPFECHA.Update();
            UPBUSCARREPORTE.Update();
            UPPASEPUERTA.Update();
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
            this.imagen.Visible = true;
            this.rwReporte.Visible = false;

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
            Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;
        }


        #endregion

        #region "Metodos del Reporte"

        public Boolean inicializaReporte(String Reporte)
        {
           
            String wuser = Page.User.Identity.Name;  
            if (System.IO.File.Exists(Reporte) != true)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Reporte no existe"));
                return false;
            }
     
            rwReporte.ProcessingMode = ProcessingMode.Local;
            rwReporte.LocalReport.ReportPath = Reporte;
            rwReporte.LocalReport.EnableExternalImages = true;
            rwReporte.LocalReport.Refresh();
            rwReporte.Visible = true;

            return true;
        }

        public void AñadeDatasorurce(ReportDataSource wdatasourc)
        {
            rwReporte.LocalReport.DataSources.Clear();
            rwReporte.LocalReport.DataSources.Add(wdatasourc);
           
           
        }
        public Boolean AcceptAllCertifications(Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void GetReporte()
        {
            String Reporte = "";
            ReportDataSource wdatasourc;
            DataSet wdataset = new DataSet();
    
            DataTable dt = new DataTable();
            wdataset.Tables.Add(dt);
            dt.Columns.Add("CONTAINER", typeof(string));
            dt.Columns.Add("MRN", typeof(string));


            Reporte = "rptpasepuertabrbk.rdlc";
            Reporte = this.Server.MapPath(@"..\reportes\" + Reporte);
            if (inicializaReporte(Reporte) != true)
            {
                return;
            }

            wdatasourc = new ReportDataSource("dsPasePuerta", wdataset.Tables[0]);
            AñadeDatasorurce(wdatasourc);
            rwReporte.LocalReport.Refresh();
        }

        #endregion


        #region "Eventos del Formulario"

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
                objPaseBRBK = Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.NUMERO_PASE_N4.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    if (!Detalle.ESTADO.Equals("EXPIRADO"))
                    {
                        Detalle.VISTO = chkPase.Checked;
                    }
                    else { Detalle.VISTO = false; }

                }

                tablePagination.DataSource = objPaseBRBK.Detalle.OrderBy(p => p.ESTADO);
                tablePagination.DataBind();

                Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox Chk = e.Item.FindControl("chkPase") as CheckBox;
                Label Estado = e.Item.FindControl("LblEstado") as Label;
                if (Estado.Text.Equals("EXPIRADO") || Estado.Text.Equals("EXPIRADO - D/D"))
                {
                    Chk.Enabled = false;
                }
            }
        }

        #endregion

        #region "Eventos page"
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

            ClsUsuario = Page.Tracker();
            if (ClsUsuario != null)
            {
                if (!Page.IsPostBack)
                {
                    this.Limpia_Campos();


                    rwReporte.Visible = false;
                    this.imagen.Visible = true;
                    this.Actualiza_Paneles();
                }

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    numero_carga = QuerySegura.DecryptQueryString(Request.QueryString["id_carga"]);
                    if (numero_carga == null || string.IsNullOrEmpty(numero_carga))
                    {
                        this.TXTMRN.Text = string.Empty;
                        this.TXTMSN.Text = string.Empty;
                        if (string.IsNullOrEmpty(this.TXTHSN.Text))
                        { this.TXTHSN.Text = string.Format("{0}", "0000"); }
                    }
                    else
                    {
                        numero_carga = numero_carga.Trim().Replace("\0", string.Empty);
                        if (numero_carga.Split('+').ToList().Count > 0)
                        {
                            this.TXTMRN.Text = numero_carga.Split('-').ToList()[0].Trim();
                            this.TXTMSN.Text = numero_carga.Split('-').ToList()[1].Trim();
                            this.TXTHSN.Text = numero_carga.Split('-').ToList()[2].Trim();

                            this.BtnBuscar_Click(sender, e);

                            this.ChkTodos_CheckedChanged(sender, e);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "Eventos Botones"
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");

                    this.ChkTodos.Checked = false;
                    this.imagen.Visible = true;
                    this.rwReporte.Visible = false;

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
                    var CargaSuelta = PasePuerta.PaseWebBRBK.ObtenerListaPase(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc);

                    if (CargaSuelta.Exitoso)
                    {

                        /*query contenedores*/
                        var LinqQuery = (from Tbl in CargaSuelta.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.NUMERO_PASE_N4))
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
                                             CNTR_DD = (Tbl.CNTR_DD == null) ? false : Tbl.CNTR_DD,
                                             AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? string.Empty : Tbl.AGENTE_DESC,
                                             FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                             IMPORTADOR = (Tbl.IMPORTADOR == null) ? string.Empty : Tbl.IMPORTADOR,
                                             IMPORTADOR_DESC = (Tbl.IMPORTADOR_DESC == null) ? string.Empty : Tbl.IMPORTADOR_DESC,
                                             FECHA_SALIDA = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                             FECHA_SALIDA_PASE = (Tbl.FECHA_AUT_PPWEB.HasValue ? Tbl.FECHA_AUT_PPWEB : null),
                                             TIPO_CNTR = (Tbl.TIPO_CNTR == null) ? string.Empty : Tbl.TIPO_CNTR,
                                             D_TURNO = (Tbl.D_TURNO == null) ? string.Empty : Tbl.D_TURNO,
                                             ID_PASE = (Tbl.ID_PASE == null) ? 0 : Tbl.ID_PASE,
                                             ESTADO = (Tbl.ESTADO == null) ? string.Empty : Tbl.ESTADO,
                                             CANTIDAD_CARGA = Tbl.CANTIDAD,
                                             PRIMERA = (Tbl.PRIMERA == null) ? string.Empty : Tbl.PRIMERA,
                                             MARCA = (Tbl.MARCA == null) ? string.Empty : Tbl.MARCA,
                                             NUMERO_PASE_N4 = (Tbl.NUMERO_PASE_N4 == null) ? string.Empty : Tbl.NUMERO_PASE_N4,
                                             USUARIO_ING = (Tbl.USUARIO_ING == null) ? string.Empty : Tbl.USUARIO_ING,
                                             USUARIO_MOD = (Tbl.USUARIO_ING == null) ? string.Empty : Tbl.USUARIO_ING
                                         }).ToList().OrderBy(x => x.CONTENEDOR);

                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseBRBK = Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;

                            objPaseBRBK.FECHA = DateTime.Now;
                            objPaseBRBK.MRN = this.TXTMRN.Text;
                            objPaseBRBK.MSN = this.TXTMSN.Text;
                            objPaseBRBK.HSN = this.TXTHSN.Text;
                            objPaseBRBK.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseBRBK.SESION = this.hf_BrowserWindowName.Value;


                            objPaseBRBK.Detalle.Clear();

                            foreach (var Det in LinqQuery)
                            {

                                objDetallePaseBRBK = new Cls_Bil_PasePuertaBRBK_Detalle();
                                objDetallePaseBRBK.ID_PPWEB = Det.ID_PPWEB;
                                objDetallePaseBRBK.MRN = Det.MRN;
                                objDetallePaseBRBK.MSN = Det.MSN;
                                objDetallePaseBRBK.HSN = Det.HSN;
                                objDetallePaseBRBK.CARGA = Det.CARGA;
                                objDetallePaseBRBK.FACTURA = Det.FACTURA;
                                objDetallePaseBRBK.AGENTE = Det.AGENTE;
                                objDetallePaseBRBK.FACTURADO = Det.FACTURADO;
                                objDetallePaseBRBK.PAGADO = Det.PAGADO;
                                objDetallePaseBRBK.GKEY = Det.GKEY;
                                objDetallePaseBRBK.REFERENCIA = Det.REFERENCIA;
                                objDetallePaseBRBK.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseBRBK.DOCUMENTO = Det.DOCUMENTO;
                                objDetallePaseBRBK.CIATRANS = Det.CIATRANS;
                                objDetallePaseBRBK.CHOFER = Det.CHOFER;
                                objDetallePaseBRBK.PLACA = Det.PLACA;
                                objDetallePaseBRBK.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objDetallePaseBRBK.CNTR_DD = Det.CNTR_DD.Value;
                                objDetallePaseBRBK.AGENTE_DESC = Det.AGENTE_DESC;
                                objDetallePaseBRBK.FACTURADO_DESC = Det.FACTURADO_DESC;
                                objDetallePaseBRBK.IMPORTADOR = Det.IMPORTADOR;
                                objDetallePaseBRBK.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;
                                objDetallePaseBRBK.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                                objDetallePaseBRBK.PRIMERA = Det.PRIMERA;
                                objDetallePaseBRBK.MARCA = Det.MARCA;

                                objDetallePaseBRBK.TIPO_CNTR = Det.TIPO_CNTR;
                                objDetallePaseBRBK.D_TURNO = string.Format("{0} de {1}", (Det.FECHA_SALIDA_PASE.HasValue ? Det.FECHA_SALIDA_PASE.Value.ToString("yyyy/MM/dd") : string.Empty), Det.D_TURNO);
                                objDetallePaseBRBK.ID_PASE = double.Parse(Det.ID_PASE.Value.ToString());
                                objDetallePaseBRBK.ESTADO = (Det.ESTADO.Equals("EX") ? "EXPIRADO" : "ACTIVO");
                                objDetallePaseBRBK.USUARIO_ING = Det.USUARIO_ING;
                                objDetallePaseBRBK.FECHA_ING = System.DateTime.Now.Date;
                                objDetallePaseBRBK.USUARIO_MOD = Det.USUARIO_MOD;
                                objDetallePaseBRBK.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;
                                objDetallePaseBRBK.CANTIDAD = Det.CANTIDAD_CARGA;
                                objDetallePaseBRBK.VISTO = false;
                                if (Det.CNTR_DD.Value)
                                {
                                    objDetallePaseBRBK.TIPO_CNTR = string.Format("{0} - {1}", (Det.ESTADO.Equals("EX") ? "EXPIRADO" : "ACTIVO"), "D/D");
                                }
                                else
                                {
                                    objDetallePaseBRBK.TIPO_CNTR = (Det.ESTADO.Equals("EX") ? "EXPIRADO" : "ACTIVO");
                                }

                                objPaseBRBK.Detalle.Add(objDetallePaseBRBK);

                            }

                            tablePagination.DataSource = objPaseBRBK.Detalle.OrderByDescending(p=> p.ID_PASE);
                            tablePagination.DataBind();

                            Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada.."));

                        }


                    }
                    else
                    {

                        objPaseBRBK = Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                        if (objPaseBRBK != null)
                        {
                            objPaseBRBK.Detalle.Clear();
                        }
                        Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", CargaSuelta.MensajeProblema));

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

        protected void BtnCargar_Click(object sender, EventArgs e)
        {

            List<string> Lista = new List<string>();

            OcultarLoading("2");
            rwReporte.Visible = false;

            objPaseBRBK = Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
            if (objPaseBRBK == null)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta, para poder imprimir los pase a puerta de carga suelta (CFS)"));
                return;
            }
            if (objPaseBRBK.Detalle.Count == 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>No existe detalle de carga, para poder imprimir los pase a puerta"));
                return;
            }

            var LinqListPases = (from p in objPaseBRBK.Detalle.Where(x => x.VISTO == true)
                                      select p.ID_PASE).ToList();

            if (LinqListPases.Count == 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe seleccionar los pases a imprimir.."));
                return;
            }

            var LinqQuery = (from Tbl in objPaseBRBK.Detalle.Where(Tbl => !String.IsNullOrEmpty(Tbl.NUMERO_PASE_N4) && Tbl.VISTO == true)
                             select new
                             {
                                 ID_PASE = (Tbl.ID_PASE == null ? 0 : Tbl.ID_PASE)
                             }).ToList().OrderBy(x => x.ID_PASE);

            foreach (var Det in LinqQuery)
            {
                Lista.Add(Det.ID_PASE.ToString());
            }

            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            var DsReporte = PasePuerta.Pase_BRBK.ImprimirPasesBreakBulk(Lista, ClsUsuario.ruc);
            if (DsReporte.Exitoso)
            {
                DataSet wdataset = new DataSet();
                String Reporte = "";
                ReportDataSource wdatasourc;

                wdataset = DsReporte.Resultado;

                ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertifications;

                Reporte = "rptpasepuertabrbk.rdlc";
                Reporte = this.Server.MapPath(@"..\reportes\" + Reporte);
                if (inicializaReporte(Reporte) != true)
                {
                    return;
                }

                wdatasourc = new ReportDataSource("dsPasePuerta", wdataset.Tables[0]);
                AñadeDatasorurce(wdatasourc);
                rwReporte.LocalReport.Refresh();
                this.rwReporte.Visible = true;
                rwReporte.DataBind();
                this.imagen.Visible = false;
                this.Ocultar_Mensaje();
                this.Actualiza_Paneles();

            }
            else
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>No se pudo cargar datos del reporte.."));
                return;
            }
        }
        #endregion

        #region "Eventos check"
        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool ChkEstado = this.ChkTodos.Checked;

                objPaseBRBK = Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaBRBK_Cabecera;
                if (objPaseBRBK == null)
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }

                foreach (var Det in objPaseBRBK.Detalle)
                {
                    var Detalle = objPaseBRBK.Detalle.FirstOrDefault(f => f.NUMERO_PASE_N4.Equals(Det.NUMERO_PASE_N4));
                    if (Detalle != null)
                    {
                        //Detalle.VISTO = ChkEstado;
                        if (!Detalle.ESTADO.Equals("EXPIRADO"))
                        {
                            Detalle.VISTO = ChkEstado;
                        }
                        else { Detalle.VISTO = false; }
                    }
                }


                tablePagination.DataSource = objPaseBRBK.Detalle.OrderBy(p => p.ESTADO);
                tablePagination.DataBind();

                Session["ImprimirPaseBRBK" + this.hf_BrowserWindowName.Value] = objPaseBRBK;

                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", ex.Message));

            }
        }

        #endregion






        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

     


   
     
    }

}