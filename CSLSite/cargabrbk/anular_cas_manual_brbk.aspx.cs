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
using ControlPagos.Importacion;
using SqlConexion;
using CSLSite;
using CasManual;
using BreakBulk;

namespace CSLSite
{


    public partial class anular_cas_manual_brbk : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_CasManual objDetalleCas = new Cls_Bil_CasManual();
        #endregion

        #region "Variables"

        private static Int64? lm = -3;
        private string OError;
        private string cMensajes;
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

        private void Listado_Cargas(string Usuario, string IdDesConsolidadora, string MRN)
        {
            try
            {

                var Resultado = Cls_Bil_CasManual.Listado_Autorizaciones_brbk_mrn(MRN, IdDesConsolidadora, out cMensajes);
                if (Resultado != null)
                {
                    
                        var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.MRN))
                                        select new
                                        {
                                            id = (Tbl.id == null ? 0 : Tbl.id),
                                            carga = string.Format("{0}-{1}-{2}", Tbl.MRN, Tbl.MSN, Tbl.HSN),
                                            total_items_manifiesto = (Tbl.total_items_manifiesto),
                                            fecha_manifiesto = Tbl.fecha_manifiesto,
                                            consignatario_manifiesto = (Tbl.consignatario_manifiesto == null ? string.Empty : Tbl.consignatario_manifiesto),
                                            contenedor_manifiesto = (Tbl.contenedor_manifiesto == null ? string.Empty : Tbl.contenedor_manifiesto),
                                            descripcion_manifiesto = (Tbl.descripcion_manifiesto == null ? string.Empty : Tbl.descripcion_manifiesto),
                                            bl_manifiesto = (Tbl.bl_manifiesto == null ? string.Empty : Tbl.bl_manifiesto),
                                            consignatario_manifiesto_id = (Tbl.consignatario_manifiesto_id == null ? string.Empty : Tbl.consignatario_manifiesto_id),
                                            desconsolidador_manifiesto = (Tbl.desconsolidador_manifiesto == null ? string.Empty : Tbl.desconsolidador_manifiesto),
                                            id_manifiesto = (Tbl.id_manifiesto == null ? 0 : Tbl.id_manifiesto),
                                            id_manifiesto_detalle = (Tbl.id_manifiesto_detalle == null ? 0 : Tbl.id_manifiesto_detalle),
                                            mrn = (Tbl.MRN == null ? string.Empty : Tbl.MRN),
                                            msn = (Tbl.MSN == null ? string.Empty : Tbl.MSN),
                                            hsn = (Tbl.HSN == null ? string.Empty : Tbl.HSN),
                                            Tbl.peso_total,
                                            facturas = (Tbl.facturas == null ? string.Empty : Tbl.facturas),
                                            usuario_libera = (Tbl.usuario_libera == null ? string.Empty : Tbl.usuario_libera),
                                            fecha_libera = Tbl.fecha_libera,
                                            desconsolidador_asigna_id = (Tbl.desconsolidador_asigna_id == null ? string.Empty : Tbl.desconsolidador_asigna_id),
                                            desconsolidador_asigna_nombre = (Tbl.desconsolidador_asigna_nombre == null ? string.Empty : Tbl.desconsolidador_asigna_nombre),
                                            desconsolidador_naviera = (Tbl.desconsolidador_naviera == null ? string.Empty : Tbl.desconsolidador_naviera),
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {

                            objCas = Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                            objCas.Detalle_Cas.Clear();
                            foreach (var Det in LinqQuery)
                            {
                                objDetalleCas = new Cls_Bil_CasManual();
                                objDetalleCas.id = Det.id;
                                objDetalleCas.total_items_manifiesto = Det.total_items_manifiesto;
                                objDetalleCas.fecha_manifiesto = Det.fecha_manifiesto;
                                objDetalleCas.consignatario_manifiesto = Det.consignatario_manifiesto;
                                objDetalleCas.contenedor_manifiesto = Det.contenedor_manifiesto;
                                objDetalleCas.descripcion_manifiesto = Det.descripcion_manifiesto;
                                objDetalleCas.bl_manifiesto = Det.bl_manifiesto;
                                objDetalleCas.consignatario_manifiesto_id = Det.consignatario_manifiesto_id;
                                objDetalleCas.desconsolidador_manifiesto = Det.desconsolidador_manifiesto;
                                objDetalleCas.id_manifiesto = Det.id_manifiesto;
                                objDetalleCas.carga = Det.carga;
                                objDetalleCas.visto = false;
                                objDetalleCas.llave = string.Format("{0}-{1}-{2}", Det.carga, Det.total_items_manifiesto, Det.id);
                                objDetalleCas.MRN = Det.mrn;
                                objDetalleCas.MSN= Det.msn;
                                objDetalleCas.HSN = Det.hsn;
                                objDetalleCas.peso_total = Det.peso_total;
                                objDetalleCas.facturas = Det.facturas;
                                objDetalleCas.usuario_libera = Det.usuario_libera;
                                objDetalleCas.fecha_libera = Det.fecha_libera;
                                objDetalleCas.desconsolidador_asigna_id = Det.desconsolidador_asigna_id;
                                objDetalleCas.desconsolidador_asigna_nombre = (!string.IsNullOrEmpty(Det.desconsolidador_asigna_id) ? string.Format("[{0}] - {1}", Det.desconsolidador_asigna_id,Det.desconsolidador_asigna_nombre) : string.Empty);
                                objDetalleCas.desconsolidador_naviera = Det.desconsolidador_naviera;
                                objCas.Detalle_Cas.Add(objDetalleCas);
                            }
                            tablePagination.DataSource = objCas.Detalle_Cas;
                            tablePagination.DataBind();

                            Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] = objCas;
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen cargas pendientes que mostrar"));
                        return;
                    }
                    
                    
                    this.Actualiza_Paneles();

                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen cargas pendientes que mostrar"));
                    return;
                }

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Cargas), "anular_cas_manual_brbk", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }

        }

       

        private void Actualiza_Paneles()
        {
           
            this.UPCARGA.Update();

            
        }


        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
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
            if (Tipo == 1)//cabecera
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
            if (Tipo == 2)//detalle
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("2");
            }

            this.Actualiza_Paneles();

        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
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
            objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
            Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] = objCas;
            //Session["TransaccionContDet"] = objCas.Detalle_Cas;
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
         

            if (!Page.IsPostBack)
            {

                this.banmsg.InnerText = string.Empty;
                

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {

                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                }

                this.TXTMRN.Text = string.Empty;
               
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Response.IsClientConnected)
                {

                    Server.HtmlEncode(this.TXTMRN.Text.Trim());


                    if (!Page.IsPostBack)
                    {
                        this.Crear_Sesion();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}", ex.Message));
            }

        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    int nTotal = 0;

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdDesConsolidadora = string.Empty;

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MRN</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }


                    objCas = Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objCas == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder revertir las autorizaciones </b>"));
                        return;
                    }

                    if (objCas.Detalle_Cas.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen cargas  pendientes para anular.  </b>"));
                        return;
                    }

                    var LinqValidaFaltantes = (from p in objCas.Detalle_Cas.Where(x => x.visto == true)
                                               select p.llave).ToList();

                    if (LinqValidaFaltantes.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas para realizar las anulaciones</b>"));
                        return;
                    }

                    if (ClsUsuario != null)
                    {
                        /*
                        var Desconsolidadora = N4.Entidades.Forwarder.ObtenerForwarderPorRUC(ClsUsuario.loginname, ClsUsuario.ruc);
                        if (Desconsolidadora.Exitoso)
                        {
                            var ListaDesconsolidadora = Desconsolidadora.Resultado;
                            if (ListaDesconsolidadora != null)
                            {
                                IdDesConsolidadora = ListaDesconsolidadora.CODIGO_FW;
                            }
                        }
                        else
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0} - {1}", ClsUsuario.loginname, Desconsolidadora.MensajeProblema));
                            return;
                        }
                        */

                        var Desconsolidadora = carrier.GetCurrier(ClsUsuario.ruc);
                       
                        if (Desconsolidadora != null)
                        {
                            var ListaDesconsolidadora = Desconsolidadora.carrier_id;
                            IdDesConsolidadora = ListaDesconsolidadora;

                        }
                        else
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0}", ClsUsuario.loginname));
                            return;
                        }
                    }


                    foreach (var Det in objCas.Detalle_Cas.Where(p=> p.visto==true))
                    {
                        
                        objDetalleCas = new Cls_Bil_CasManual();
                        objDetalleCas.id = Det.id;
                        objDetalleCas.Anular_Brbk(out cMensajes);
                        if (cMensajes != string.Empty)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo anular la autorización de la carga: {0}, Existen los siguientes problemas: {1} </b>", Det.carga.Trim(), cMensajes));
                            this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());
                            return;

                        }
                        else
                        {
                            nTotal++;
                        }
                      
                    }

                    if (nTotal != 0)
                    {
                        this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedió con la anulación de las autorizaciones de salida de {0} cargas. Procesado realizado con éxito. </b>", nTotal));
                        return;
                    }

                    OcultarLoading("1");
                    OcultarLoading("2");

                    this.Actualiza_Paneles();

                }




            }
            catch (Exception ex)
            {
               
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "anular_cas_manual_brbk.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }

        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    bool ChkEstado = this.ChkTodos.Checked;

                    CultureInfo enUS = new CultureInfo("en-US");
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objCas = Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objCas == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todas las carga que se desea anular... </b>"));
                        return;
                    }
                    if (objCas.Detalle_Cas.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de cargas, para poder seleccionar... </b>"));
                        return;
                    }

                    foreach (var Det in objCas.Detalle_Cas)
                    {
                        Int64 llave = Det.id.Value;
                        var Detalle = objCas.Detalle_Cas.FirstOrDefault(f => f.id.Equals(llave));
                        if (Detalle != null)
                        {
                            if (string.IsNullOrEmpty(Detalle.facturas.Trim()))
                            {
                                Detalle.visto = ChkEstado;
                            }
                           

                        }
                    }

                    tablePagination.DataSource = objCas.Detalle_Cas;
                    tablePagination.DataBind();

                    Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] = objCas;

                }
                    
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ChkTodos_CheckedChanged), "anular_cas_manual_brbk", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

            }
        }

        protected void BtnFiltrar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    objCas = Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objCas == null) { return; }

                    if (TxtFiltro.Text != string.Empty)
                    {


                        var q = (from A in objCas.Detalle_Cas
                                 where A.carga.ToUpper().Contains(TxtFiltro.Text.ToUpper())
                                 select A).ToList();

                        tablePagination.DataSource = q;
                        tablePagination.DataBind();

                    }
                    else
                    {
                        var q = (from A in objCas.Detalle_Cas
                                 where A.carga != string.Empty
                                 select A).ToList();

                        tablePagination.DataSource = q;
                        tablePagination.DataBind();
                    }

                    
                    this.Ocultar_Mensaje();
                    //UPDETALLE.Update();
                   
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFiltrar_Click), "anular_cas_manual_brbk", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }
        }

        protected void chkMarcar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkMarcar = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkMarcar.NamingContainer;
                Label id = (Label)item.FindControl("id");

                Int64 pllave  = Int64.Parse(id.Text);
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                //actualiza datos del contenedor
                objCas = Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                var Detalle = objCas.Detalle_Cas.FirstOrDefault(f => f.id.Equals(pllave));
                if (Detalle != null)
                {
                    Detalle.visto = chkMarcar.Checked;

                }

                if (TxtFiltro.Text != string.Empty)
                {


                    var q = (from A in objCas.Detalle_Cas
                             where A.carga.Contains(TxtFiltro.Text)
                             select A).ToList();

                    tablePagination.DataSource = q;
                    tablePagination.DataBind();

                }
                else
                {
                    var q = (from A in objCas.Detalle_Cas
                             where A.carga != string.Empty
                             select A).ToList();

                    tablePagination.DataSource = q;
                    tablePagination.DataBind();
                }


                Session["CasManualBrbk" + this.hf_BrowserWindowName.Value] = objCas;


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }


        protected void BtnBuscar_Click(object sender, EventArgs e)
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

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MRN"));
                        this.TXTMRN.Focus();
                        
                        return;
                    }

                    string IdDesConsolidadora = string.Empty;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (ClsUsuario != null)
                    {
                        var ruc_contecon = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];
                        if (!ClsUsuario.ruc.Equals(ruc_contecon.ToString().Trim()))
                        {
                            var Desconsolidadora = carrier.GetCurrier(ClsUsuario.ruc);
                            if (Desconsolidadora != null)
                            {
                                var ListaDesconsolidadora = Desconsolidadora.carrier_id;
                                IdDesConsolidadora = ListaDesconsolidadora;

                            }
                            else
                            {

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0}", ClsUsuario.loginname));
                                return;
                            }


                        }
                        else
                        {
                            IdDesConsolidadora = "CGSA";
                        }

                        //var Desconsolidadora = N4.Entidades.Forwarder.ObtenerForwarderPorRUC(ClsUsuario.loginname, ClsUsuario.ruc);
                        //if (Desconsolidadora.Exitoso)
                        //{
                        //    var ListaDesconsolidadora = Desconsolidadora.Resultado;
                        //    if (ListaDesconsolidadora != null)
                        //    {
                        //        IdDesConsolidadora = ListaDesconsolidadora.CODIGO_FW;
                        //    }
                        //}
                        //else
                        //{

                        //    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0} - {1}", ClsUsuario.loginname,Desconsolidadora.MensajeProblema));
                        //    return;
                        //}

                    }
                    //para pruebas
                    //IdDesConsolidadora = "CGSA";

                    this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());

                    //this.Ocultar_Mensaje();
                 

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "anular_cas_manual_brbk", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));   
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }



            
        }


        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox Chk = e.Item.FindControl("chkMarcar") as CheckBox;
                Label facturas = e.Item.FindControl("facturas") as Label;
                
                if (!string.IsNullOrEmpty(facturas.Text))
                {
                    Chk.Enabled = false;
                }
                else
                {
                    Chk.Enabled = true;
                }

            }
        }




    }
}