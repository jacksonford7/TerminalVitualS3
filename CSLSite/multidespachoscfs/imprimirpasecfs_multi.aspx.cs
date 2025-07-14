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

    public partial class imprimirpasecfs_multi : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;    
        private Cls_Bil_PasePuertaCFS_Cabecera objPaseCFS = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_PasePuertaCFS_SubItems objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
        private Cls_Bil_PasePuertaCFS_Detalle objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();


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
            UPRESUMEN.Update();

        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
           
          
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
            objPaseCFS = new Cls_Bil_PasePuertaCFS_Cabecera();
            Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;
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


            Reporte = "rptpasepuertacfs.rdlc";
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
                objPaseCFS = Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.NUMERO_PASE_N4.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    if (!Detalle.ESTADO.Equals("EXPIRADO"))
                    {
                        Detalle.VISTO = chkPase.Checked;
                    }
                    else { Detalle.VISTO = false; }

                }

                tablePagination.DataSource = objPaseCFS.Detalle.OrderBy(p => p.ESTADO);
                tablePagination.DataBind();

                Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

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

           // this.IsAllowAccess();

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
              

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    this.BtnResumen.Attributes["disabled"] = "disabled";

                    numero_carga = QuerySegura.DecryptQueryString(Request.QueryString["id_carga"]);
                    if (numero_carga == null || string.IsNullOrEmpty(numero_carga))
                    {
                        this.TXTMRN.Text = string.Empty;
                        
                       
                    }
                    else
                    {
                        numero_carga = numero_carga.Trim().Replace("\0", string.Empty);
                        if (!string.IsNullOrEmpty(numero_carga))
                        {
                            this.TXTMRN.Text = numero_carga.Trim();
                          

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
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de despacho"));
                        this.TXTMRN.Focus();
                        return;
                    }
                   
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var CargaSuelta = PasePuerta.Pase_WebCFS.ObtenerListaPase_MultiDespacho(this.TXTMRN.Text.Trim(), ClsUsuario.ruc);

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
                            objPaseCFS = Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;

                            objPaseCFS.FECHA = DateTime.Now;
                            objPaseCFS.MRN = this.TXTMRN.Text;
                            objPaseCFS.MSN = "";
                            objPaseCFS.HSN = "";
                            objPaseCFS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseCFS.SESION = this.hf_BrowserWindowName.Value;


                            objPaseCFS.Detalle.Clear();

                            foreach (var Det in LinqQuery)
                            {

                                objDetallePaseCFS = new Cls_Bil_PasePuertaCFS_Detalle();
                                objDetallePaseCFS.ID_PPWEB = Det.ID_PPWEB;
                                objDetallePaseCFS.MRN = Det.MRN;
                                objDetallePaseCFS.MSN = Det.MSN;
                                objDetallePaseCFS.HSN = Det.HSN;
                                objDetallePaseCFS.CARGA = Det.CARGA;
                                objDetallePaseCFS.FACTURA = Det.FACTURA;
                                objDetallePaseCFS.AGENTE = Det.AGENTE;
                                objDetallePaseCFS.FACTURADO = Det.FACTURADO;
                                objDetallePaseCFS.PAGADO = Det.PAGADO;
                                objDetallePaseCFS.GKEY = Det.GKEY;
                                objDetallePaseCFS.REFERENCIA = Det.REFERENCIA;
                                objDetallePaseCFS.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseCFS.DOCUMENTO = Det.DOCUMENTO;
                                objDetallePaseCFS.CIATRANS = Det.CIATRANS;
                                objDetallePaseCFS.CHOFER = Det.CHOFER;
                                objDetallePaseCFS.PLACA = Det.PLACA;
                                objDetallePaseCFS.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objDetallePaseCFS.CNTR_DD = Det.CNTR_DD.Value;
                                objDetallePaseCFS.AGENTE_DESC = Det.AGENTE_DESC;
                                objDetallePaseCFS.FACTURADO_DESC = Det.FACTURADO_DESC;
                                objDetallePaseCFS.IMPORTADOR = Det.IMPORTADOR;
                                objDetallePaseCFS.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;
                                objDetallePaseCFS.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                                objDetallePaseCFS.PRIMERA = Det.PRIMERA;
                                objDetallePaseCFS.MARCA = Det.MARCA;

                                objDetallePaseCFS.TIPO_CNTR = Det.TIPO_CNTR;
                                objDetallePaseCFS.D_TURNO = string.Format("{0} de {1}", (Det.FECHA_SALIDA_PASE.HasValue ? Det.FECHA_SALIDA_PASE.Value.ToString("yyyy/MM/dd") : string.Empty), Det.D_TURNO);
                                objDetallePaseCFS.ID_PASE = double.Parse(Det.ID_PASE.Value.ToString());
                                objDetallePaseCFS.ESTADO = (Det.ESTADO.Equals("EX") ? "EXPIRADO" : "ACTIVO");
                                objDetallePaseCFS.USUARIO_ING = Det.USUARIO_ING;
                                objDetallePaseCFS.FECHA_ING = System.DateTime.Now.Date;
                                objDetallePaseCFS.USUARIO_MOD = Det.USUARIO_MOD;
                                objDetallePaseCFS.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4;
                                objDetallePaseCFS.CANTIDAD = Det.CANTIDAD_CARGA;
                                objDetallePaseCFS.VISTO = false;
                                if (Det.CNTR_DD.Value)
                                {
                                    objDetallePaseCFS.TIPO_CNTR = string.Format("{0} - {1}", (Det.ESTADO.Equals("EX") ? "EXPIRADO" : "ACTIVO"), "D/D");
                                }
                                else
                                {
                                    objDetallePaseCFS.TIPO_CNTR = (Det.ESTADO.Equals("EX") ? "EXPIRADO" : "ACTIVO");
                                }

                                objPaseCFS.Detalle.Add(objDetallePaseCFS);

                            }

                            tablePagination.DataSource = objPaseCFS.Detalle.OrderBy(p=> p.ESTADO) ;
                            tablePagination.DataBind();

                            Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de despacho ingresado.."));

                        }


                    }
                    else
                    {

                        objPaseCFS = Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                        if (objPaseCFS != null)
                        {
                            objPaseCFS.Detalle.Clear();
                        }
                        Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de despacho ingresado..{0}", CargaSuelta.MensajeProblema));

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

            objPaseCFS = Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
            if (objPaseCFS == null)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta, para poder imprimir los pase a puerta de carga suelta - Multidespacho (CFS)"));
                return;
            }
            if (objPaseCFS.Detalle.Count == 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>No existe detalle de carga, para poder imprimir los pase a puerta"));
                return;
            }

            var LinqListPases = (from p in objPaseCFS.Detalle.Where(x => x.VISTO == true)
                                      select p.ID_PASE).ToList();

            if (LinqListPases.Count == 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe seleccionar los pases a imprimir.."));
                return;
            }

            var LinqQuery = (from Tbl in objPaseCFS.Detalle.Where(Tbl => !String.IsNullOrEmpty(Tbl.NUMERO_PASE_N4) && Tbl.VISTO == true)
                             select new
                             {
                                 ID_PASE = (Tbl.ID_PASE == null ? 0 : Tbl.ID_PASE)
                             }).ToList().OrderBy(x => x.ID_PASE);

            foreach (var Det in LinqQuery)
            {
                Lista.Add(Det.ID_PASE.ToString());
            }

            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            var DsReporte = PasePuerta.Pase_CFS.ImprimirPasesCFS_ALT(Lista, ClsUsuario.ruc);
            if (DsReporte.Exitoso)
            {
                DataSet wdataset = new DataSet();
                String Reporte = "";
                ReportDataSource wdatasourc;

                wdataset = DsReporte.Resultado;

                ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertifications;

                Reporte = "rptpasepuertacfsmulti.rdlc";
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
                this.BtnResumen.Attributes.Remove("disabled");

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

                objPaseCFS = Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                if (objPaseCFS == null)
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }

                foreach (var Det in objPaseCFS.Detalle)
                {
                    var Detalle = objPaseCFS.Detalle.FirstOrDefault(f => f.NUMERO_PASE_N4.Equals(Det.NUMERO_PASE_N4));
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


                tablePagination.DataSource = objPaseCFS.Detalle.OrderBy(p => p.ESTADO);
                tablePagination.DataBind();

                Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] = objPaseCFS;

                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", ex.Message));

            }
        }

        #endregion

        #region "Visualizar"
        protected void BtnResumen_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                objPaseCFS = Session["ImprimirPaseCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;

                if (objPaseCFS == null)
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Aún no ha generado la consulta, para poder imprimir el resumen de pase de puertas"));
                    return;
                }

                if (string.IsNullOrEmpty(this.TXTMRN.Text))
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Aún no ha generado la consulta, para poder imprimir el resumen de pase de puertas"));
                    return;
                }

                //IMPRIMIR FACTURA -FORMATO HTML
                string cId = securetext(this.TXTMRN.Text.Trim());
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../multidespachoscfs/pases_multi_preview.aspx?id_comprobante=" + cId + "');", true);

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