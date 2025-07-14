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


namespace CSLSite
{


    public partial class cas_manual_cfs : System.Web.UI.Page
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

                var Resultado = CasBBK.ListarPendientes(Usuario.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());
                if (Resultado != null)
                {
                    if (Resultado.Exitoso)
                    {
                        var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.mrn))
                                        select new
                                        {
                                            id = (Tbl.id_manifiesto_detalle == null ? 0 : Tbl.id_manifiesto_detalle),
                                            carga = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                            total_items_manifiesto = (Tbl.total_items_manifiesto == null ? 0 : Tbl.total_items_manifiesto),
                                            fecha_manifiesto = Tbl.fecha_manifiesto,
                                            consignatario_manifiesto = (Tbl.consignatario_manifiesto == null ? string.Empty : Tbl.consignatario_manifiesto),
                                            contenedor_manifiesto = (Tbl.contenedor_manifiesto == null ? string.Empty : Tbl.contenedor_manifiesto),
                                            descripcion_manifiesto = (Tbl.descripcion_manifiesto == null ? string.Empty : Tbl.descripcion_manifiesto),
                                            bl_manifiesto = (Tbl.bl_manifiesto == null ? string.Empty : Tbl.bl_manifiesto),
                                            consignatario_manifiesto_id = (Tbl.consignatario_manifiesto_id == null ? string.Empty : Tbl.consignatario_manifiesto_id),
                                            desconsolidador_manifiesto = (Tbl.desconsolidador_manifiesto == null ? string.Empty : Tbl.desconsolidador_manifiesto),
                                            id_manifiesto = (Tbl.id_manifiesto == null ? 0 : Tbl.id_manifiesto),
                                            id_manifiesto_detalle = (Tbl.id_manifiesto_detalle == null ? 0 : Tbl.id_manifiesto_detalle),
                                            mrn = (Tbl.mrn == null ? string.Empty : Tbl.mrn),
                                            msn = (Tbl.msn == null ? string.Empty : Tbl.msn),
                                            hsn = (Tbl.hsn == null ? string.Empty : Tbl.hsn),
                                            peso_total = (Tbl.peso_total == null ? 0 : Tbl.peso_total)
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {

                            objCas = Session["CasManual" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                            objCas.Detalle_Cas.Clear();
                            foreach (var Det in LinqQuery)
                            {
                                objDetalleCas = new Cls_Bil_CasManual();
                                objDetalleCas.id_manifiesto_detalle = Det.id;
                                objDetalleCas.total_items_manifiesto = Det.total_items_manifiesto.Value;
                                objDetalleCas.fecha_manifiesto = Det.fecha_manifiesto;
                                objDetalleCas.consignatario_manifiesto = Det.consignatario_manifiesto;
                                objDetalleCas.contenedor_manifiesto = Det.contenedor_manifiesto;
                                objDetalleCas.descripcion_manifiesto = Det.descripcion_manifiesto;
                                objDetalleCas.bl_manifiesto = Det.bl_manifiesto;
                                objDetalleCas.consignatario_manifiesto_id = Det.consignatario_manifiesto_id;
                                objDetalleCas.desconsolidador_manifiesto = Det.desconsolidador_manifiesto;
                                objDetalleCas.id_manifiesto = Det.id_manifiesto;
                                objDetalleCas.id = Det.id;
                                objDetalleCas.carga = Det.carga;
                                objDetalleCas.visto = false;
                                objDetalleCas.llave = string.Format("{0}-{1}-{2}", Det.carga, Det.total_items_manifiesto, Det.id_manifiesto_detalle);
                                objDetalleCas.MRN = Det.mrn;
                                objDetalleCas.MSN= Det.msn;
                                objDetalleCas.HSN = Det.hsn;
                                objDetalleCas.peso_total = Det.peso_total;
                                objCas.Detalle_Cas.Add(objDetalleCas);
                            }
                            tablePagination.DataSource = objCas.Detalle_Cas;
                            tablePagination.DataBind();

                            Session["CasManual" + this.hf_BrowserWindowName.Value] = objCas;
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                        }
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen cargas pendientes que mostrar: {0} , {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                        return;

                    }
                    this.Actualiza_Paneles();

                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen cargas pendientes que mostrar: {0} , {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                    return;
                }

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Cargas), "cas_manual_cfs", false, null, null, ex.StackTrace, ex);
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
            Session["CasManual" + this.hf_BrowserWindowName.Value] = objCas;
            Session["TransaccionContDet"] = objCas.Detalle_Cas;
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

            this.banmsg.Visible = IsPostBack;
         

            if (!Page.IsPostBack)
            {

                this.banmsg.InnerText = string.Empty;
                

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {

                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.idempresa);
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


                    objCas = Session["CasManual" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objCas == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder autorizar las cargas </b>"));
                        return;
                    }

                    if (objCas.Detalle_Cas.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen cargas  pendientes para aprobar.  </b>"));
                        return;
                    }

                    var LinqValidaFaltantes = (from p in objCas.Detalle_Cas.Where(x => x.visto == true)
                                               select p.llave).ToList();

                    if (LinqValidaFaltantes.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas para realizar las autorizaciones</b>"));
                        return;
                    }

                    if (ClsUsuario != null)
                    {
                        var Desconsolidadora = N4.Entidades.Forwarder.ObtenerForwarderPorRUC(ClsUsuario.loginname, ClsUsuario.ruc);
                        if (Desconsolidadora.Exitoso)
                        {
                            var ListaDesconsolidadora = Desconsolidadora.Resultado;
                            if (ListaDesconsolidadora != null) {
                                IdDesConsolidadora = ListaDesconsolidadora.CODIGO_FW;                              
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0} - {1}", ClsUsuario.loginname, Desconsolidadora.MensajeProblema));
                            return;
                        }

                    }
                    //usar para pruebas desarrollo
                    //IdDesConsolidadora = "09908844";

                    foreach (var Det in objCas.Detalle_Cas.Where(p=> p.visto==true))
                    {
                        string ruc_desconsolidadora = string.Empty;
                        string nombre_desconsolidadora = string.Empty;
                        //validaciones
                        if (Det.total_items_manifiesto ==0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! El total de ítems del manifiesto de la carga {0}, no puede ser cero..</b>", Det.carga));
                            this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());
                            return;
                        }

                        var Desconsolidadora = N4.Entidades.Forwarder.ObtenerForwarderPorCodigo(ClsUsuario.loginname, Det.desconsolidador_manifiesto.Trim());
                        if (Desconsolidadora.Exitoso)
                        {
                            var ListaDesconsolidadora = Desconsolidadora.Resultado;
                            if (ListaDesconsolidadora != null)
                            {
                                ruc_desconsolidadora = ListaDesconsolidadora.CLNT_CUSTOMER;
                                nombre_desconsolidadora = ListaDesconsolidadora.CLNT_NAME;

                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada al id: {0} - {1}", Det.desconsolidador_manifiesto.Trim(), Desconsolidadora.MensajeProblema));
                            this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());
                            return;
                        }

                        CasBBK cas = new CasBBK();
                        cas.mrn = Det.MRN;
                        cas.msn = Det.MSN;
                        cas.hsn = Det.HSN;
                        cas.total_items_manifiesto = Det.total_items_manifiesto;
                        cas.fecha_manifiesto = Det.fecha_manifiesto;
                        cas.consignatario_manifiesto = Det.consignatario_manifiesto.Trim();
                        cas.fecha_libera = DateTime.Now;
                        cas.usuario_libera = ClsUsuario.loginname;
                        cas.desconsolidador_asigna_id = ruc_desconsolidadora;
                        cas.desconsolidador_asigna_nombre = nombre_desconsolidadora;
                        cas.consignatario_manifiesto_id = Det.consignatario_manifiesto_id.Trim();
                        cas.contenedor_manifiesto = Det.contenedor_manifiesto.Trim();
                        cas.bl_manifiesto = Det.bl_manifiesto.Trim();
                        cas.descripcion_manifiesto = Det.descripcion_manifiesto.Trim();
                        cas.desconsolidador_manifiesto = Det.desconsolidador_manifiesto.Trim();
                        cas.id_manifiesto = Det.id_manifiesto;
                        cas.id_manifiesto_detalle = Det.id_manifiesto_detalle;

                        var Resultado = cas.NuevaCas();
                        if (Resultado.Exitoso)
                        {
                            nTotal++;

                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo autorizar la carga: {0}, Existen los siguientes problemas: {1}, {2} </b>", Det.carga.Trim(), Resultado.MensajeInformacion, Resultado.MensajeProblema));
                            this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());
                            return;
                        }


                    }

                    OcultarLoading("1");
                    OcultarLoading("2");

                    if (nTotal != 0)
                    {

                        this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedió con la autorización de salida de {0} carga (s). Proceso realizado con éxito. </b>", nTotal));
                        return;
                    }

                }




            }
            catch (Exception ex)
            {
               
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "cas_manual_cfs.aspx", false, null, null, ex.StackTrace, ex);
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

                    objCas = Session["CasManual" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objCas == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder seleccionar todas las carga para autorizar... </b>"));
                        return;
                    }
                    if (objCas.Detalle_Cas.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de cargas, para poder seleccionar... </b>"));
                        return;
                    }

                    foreach (var Det in objCas.Detalle_Cas)
                    {
                        string llave = Det.llave;
                        var Detalle = objCas.Detalle_Cas.FirstOrDefault(f => f.llave.Equals(llave));
                        if (Detalle != null)
                        {
                             Detalle.visto = ChkEstado; 

                        }
                    }

                    tablePagination.DataSource = objCas.Detalle_Cas;
                    tablePagination.DataBind();

                    Session["CasManual" + this.hf_BrowserWindowName.Value] = objCas;

                }
                    
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ChkTodos_CheckedChanged), "cas_manual_cfs", false, null, null, ex.StackTrace, ex);
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
                    objCas = Session["CasManual" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
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
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFiltrar_Click), "cas_manual_cfs", false, null, null, ex.StackTrace, ex);
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
                Label llave = (Label)item.FindControl("llave");

                string pllave  = llave.Text;
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                //actualiza datos del contenedor
                objCas = Session["CasManual" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                var Detalle = objCas.Detalle_Cas.FirstOrDefault(f => f.llave.Equals(pllave));
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

                //tablePagination.DataSource = objCas.Detalle_Cas;
               // tablePagination.DataBind();

                Session["CasManual" + this.hf_BrowserWindowName.Value] = objCas;


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

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0} - {1}", ClsUsuario.loginname,Desconsolidadora.MensajeProblema));
                            return;
                        }

                    }
                    //para pruebas
                    //IdDesConsolidadora = "09908844";

                    this.Listado_Cargas(ClsUsuario.loginname.Trim(), IdDesConsolidadora, this.TXTMRN.Text.Trim());

                    this.Ocultar_Mensaje();
                 

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "cas_manual_cfs",false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));   
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }



            
        }

      
       //protected void BtnNuevo_Click(object sender, EventArgs e)
       //{
       //     try
       //     {

       //         Response.Redirect("~/contenedor/contenedorimportacion.aspx", false);

                

       //     }
       //     catch (Exception ex)
       //     {
       //         this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

       //     }

       // }

        


    }
}