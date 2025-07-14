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
using Salesforces;
using System.Data;
using System.Net;
using SqlConexion;
using CasManual;

namespace CSLSite
{


    public partial class backoffice_descon_manual : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_CasManual objDetalleCas = new Cls_Bil_CasManual();
        private string cMensajes;

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
       
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

        private string LoginName = string.Empty;
        

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private static Int64? lm = -3;
        private string OError;

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

        private void Listado_Cargas()
        {
            try
            {
               
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                var Resultado = PagoAsignado.ListaAsignacion(ClsUsuario.loginname.Trim(), ClsUsuario.ruc.Trim(), null,null);
           
                if (Resultado != null)
                {

                    if (Resultado.Exitoso)
                    {
                        var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.mrn))
                                        select new
                                        {
                                            id_asignacion = Tbl.id_asignacion,
                                            carga = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                            ruc = Tbl.ruc.Trim(),
                                            nombre = Tbl.nombre.Trim(),
                                            fecha_asignado = Tbl.fecha_asignado.Value.ToString("dd/MM/yyyy HH:mm"),
                                            login_asigna = Tbl.login_asigna
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            //tablePagination.DataSource = LinqQuery;
                            //tablePagination.DataBind();
                        }
                        else
                        {
                            //tablePagination.DataSource = null;
                            //tablePagination.DataBind();
                        }
                    }
                    else
                    {
                        //tablePagination.DataSource = null;
                        //tablePagination.DataBind();
                    }
                    this.Actualiza_Paneles();
                  
                }
                else {
                    //tablePagination.DataSource = null;
                    //tablePagination.DataBind();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                return;
            }

        }

        private void Actualiza_Paneles()
        {
           
            this.UPCARGA.Update();
            this.UPDATOSCLIENTE.Update();
            this.UPAGENTE.Update();

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
           
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
          
        }


        private void Carga_Desconsolidadora()
        {

            try
            {
                if (!string.IsNullOrEmpty(this.TxtIdAgente.Text))
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
                    var Forwarder = N4.Entidades.Forwarder.ObtenerForwarderPorCodigo(ClsUsuario.loginname, this.TxtIdAgente.Text.Trim());

                    if (Forwarder.Exitoso)
                    {
                        var ListaForwarder = Forwarder.Resultado;
                        if (ListaForwarder != null)
                        {
                            this.TXTAGENCIA.Text = string.Format("{0}", ListaForwarder.CLNT_NAME.Trim());
                        }
                        else
                        {
                            this.TXTAGENCIA.Text = string.Empty;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos de la desconsolidadora</b>"));
                            return;
                        }
                    }
                    else
                    {
                        this.TXTAGENCIA.Text = string.Empty;
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos de la desconsolidadora, {0}</b>", Forwarder.MensajeProblema));
                        return;
                    }
                }
                else
                {
                    this.TXTAGENCIA.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Desconsolidadora), "N4.Entidades.Forwarder.ObtenerForwarderPorCodigo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        
        }

        private void HandleCustomPostbackEvent(string ctrlName, string args)
        {
            try
            {
                this.Ocultar_Mensaje();

                if (ctrlName == TxtIdAgente.UniqueID && args == "OnBlur")
                {
                    this.Carga_Desconsolidadora();
                   
                }
               

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                return;

            }
            
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

                var onBlurScript = Page.ClientScript.GetPostBackEventReference(TxtIdAgente, "OnBlur");
                TxtIdAgente.Attributes.Add("onblur", onBlurScript);

            
                this.banmsg.InnerText = string.Empty;

                ClsUsuario = Page.Tracker();
                
                if (ClsUsuario != null)
                {

                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                }

                this.TXTMRN.Text = string.Empty;
                this.TXTMSN.Text = string.Empty;
                if (string.IsNullOrEmpty(this.TXTHSN.Text))
                { this.TXTHSN.Text = string.Format("{0}", "0000"); }

            }

        }

        protected void chkMarcar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkMarcar = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkMarcar.NamingContainer;
                Label llave = (Label)item.FindControl("llave");

                string pllave = llave.Text;
                

                //actualiza datos del contenedor
                objCas = Session["CasManual" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                var Detalle = objCas.Detalle_Cas.FirstOrDefault(f => f.llave.Equals(pllave));
                if (Detalle != null)
                {
                    Detalle.visto = chkMarcar.Checked;

                }


                tablePagination.DataSource = objCas.Detalle_Cas;
                tablePagination.DataBind();

                Session["CasManual" + this.hf_BrowserWindowName.Value] = objCas;


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());

                Server.HtmlEncode(this.TXTAGENCIA.Text.Trim());

                if (!Page.IsPostBack)
                {     
                   this.Crear_Sesion();
                }
                else
                {
                    var ctrlName = Request.Params[Page.postEventSourceID];
                    var args = Request.Params[Page.postEventArgumentID];

                    HandleCustomPostbackEvent(ctrlName, args);

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    if (ClsUsuario != null)
                    {

                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                    }


                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
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
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MSN"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga HSN"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Resultado = CasBBK.ListarParaCorregir(ClsUsuario.loginname.Trim(), this.TXTMRN.Text.Trim(),this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
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
                                    objDetalleCas.MSN = Det.msn;
                                    objDetalleCas.HSN = Det.hsn;
                                    objDetalleCas.peso_total = Det.peso_total;
                                    objCas.Detalle_Cas.Add(objDetalleCas);

                                    this.TxtIdAgente.Text = Det.desconsolidador_manifiesto;
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

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen cargas pendientes que mostrar: {0}  {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                            return;

                        }

                        if (!string.IsNullOrEmpty(this.TxtIdAgente.Text))
                        {
                            this.Carga_Desconsolidadora();
                        }
                        this.Actualiza_Paneles();

                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>No existen cargas pendientes que mostrar: {0}  {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                        return;
                    }

                }
                catch (Exception ex)
                {
                 
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "BtnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }    
        }

        protected void BtnActualizar_Click(object sender, EventArgs e)
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
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga MSN"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de la carga HSN"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtIdAgente.Text) || string.IsNullOrEmpty(this.TXTAGENCIA.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el código de la desconsolidadora"));
                        this.TxtIdAgente.Focus();
                        return;
                    }

                    objCas = Session["CasManual" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaCFS_Cabecera;
                    if (objCas == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder actualizar la desconsolidadora de las cargas </b>"));
                        return;
                    }

                    if (objCas.Detalle_Cas.Count == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen cargas  pendientes para actualizar la desconsolidadora.</b>"));
                        return;
                    }

                    var LinqValidaFaltantes = (from p in objCas.Detalle_Cas.Where(x => x.visto == true)
                                               select p.llave).ToList();

                    if (LinqValidaFaltantes.Count == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas para realizar la actualización de la desconsolidadora..</b>"));
                        return;
                    }

                    if (ClsUsuario != null)
                    {
                        
                        var Forwarder = N4.Entidades.Forwarder.ObtenerForwarderPorCodigo(ClsUsuario.loginname, this.TxtIdAgente.Text.Trim());
                        if (Forwarder.Exitoso)
                        {
                            var ListaForwarder = Forwarder.Resultado;
                            if (ListaForwarder != null)
                            {
                                this.TXTAGENCIA.Text = string.Format("{0}", ListaForwarder.CLNT_NAME.Trim());
                            }
                        }
                        else
                        {
                            this.TXTAGENCIA.Text = string.Empty;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos de la desconsolidadora, {0}</b>", Forwarder.MensajeProblema));
                            return;
                        }
                    }
                    

                    foreach (var Det in objCas.Detalle_Cas.Where(p => p.visto == true))
                    {
                        string ruc_desconsolidadora = string.Empty;
                        string nombre_desconsolidadora = string.Empty;

                        var Grabar = Aduanas.Entidades.Manifiesto.ActualizarManifiestoCFS(ClsUsuario.loginname,Det.id_manifiesto.Value,null, this.TxtIdAgente.Text.Trim());
                        if (Grabar.Exitoso)
                        {
                            nTotal++;
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo actualizar la carga: {0}, Existen los siguientes problemas: {1}, {2} </b>", Det.carga.Trim(), Grabar.MensajeInformacion, Grabar.MensajeProblema));
                            return;  
                        }

                    }

                    OcultarLoading("1");
                    OcultarLoading("2");

                    if (nTotal != 0)
                    {

                        tablePagination.DataSource = objCas.Detalle_Cas;
                        tablePagination.DataBind();
                        this.TxtIdAgente.Text = string.Empty;
                        this.TXTAGENCIA.Text = string.Empty;
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Se procedió con la actualización de desconsolidadora de {0} cargas. Proceso realizado con éxito. </b>", nTotal));
                        this.Actualiza_Paneles();
                        return;
                    }

                }




            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnActualizar_Click), "backoffice_descon_manual.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
   }
}