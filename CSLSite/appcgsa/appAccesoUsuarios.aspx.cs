using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class appAccesoUsuarios : System.Web.UI.Page
    {
        private string IdServicios = string.Empty;
        public static usuario sUser = null;
        public static UsuarioSeguridad us;
        public static UsuarioSeguridad usAutenticado;

        private List<user_options> objCabecera = new List<user_options>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }


        /// <summary>
        /// Código que se ejeutará al iniciar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {

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

            Page.SslOn();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Seguridad s = new Seguridad();
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                usAutenticado = s.consultarUsuarioPorId(sUser.id);

                hdfIdUsuarioLogeado.Value = usAutenticado.idUsuario.ToString();
                hdIdTipoUsuarioLogeado.Value = usAutenticado.IdTipoUsuario.ToString();
                hdTipoUsuariologeado.Value = usAutenticado.tipoUsuario.ToString();
                                               
                if (Session["usuarioId"] != null)
                {
                    int idUsuario = int.Parse(Session["usuarioId"].ToString());
                    us = s.consultarUsuarioPorId(idUsuario);

                    if (us != null)
                    {
                        cargarTipoUsuarios();
                        cargarIdTipoUsuarios();

                        txtUsuaroName.Text = us.usuario + " - " + us.nombres;
                        hdfIdUsuario.Value = us.idUsuario.ToString();
                        cmbTipoUsuario.SelectedValue = us.IdTipoUsuario.ToString();
                        ddlTipoUsuario.SelectedValue = us.tipoUsuario.ToString();
                    }
                }

                cargarFiltros();
            }
        }

        private void cargarTipoUsuarios()
        {
            HashSet<Tuple<string, string>> tipoUsuariosTemp = new HashSet<Tuple<string, string>>();
            tipoUsuariosTemp = Seguridad.getDetalleCatalogosSeguridad(ConfigurationManager.AppSettings["catalogoTipoUsuarios"]);
            HashSet<Tuple<string, string>> tipoUsuarios = new HashSet<Tuple<string, string>>();
            if (us.tipoUsuario != ConfigurationManager.AppSettings["tipoAdministradorInterno"])
            {
                foreach (Tuple<string, string> item in tipoUsuariosTemp)
                {
                    if (item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorInterno"] && item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
                    {
                        tipoUsuarios.Add(item);
                    }
                }
            }
            else
            {
                ddlTipoUsuario.Items.Add(new ListItem("TODOS", "0"));
                tipoUsuarios = tipoUsuariosTemp;
            }

            ddlTipoUsuario.DataSource = tipoUsuarios;
            ddlTipoUsuario.DataValueField = "item1";
            ddlTipoUsuario.DataTextField = "item2";
            ddlTipoUsuario.DataBind();
        }

        private void cargarFiltros()
        {
            HashSet<Tuple<int, string>> FiltrosTemp = new HashSet<Tuple<int, string>>();
            FiltrosTemp = Seguridad.getFiltrosApp(int.Parse(hdfIdUsuarioLogeado.Value));
            HashSet<Tuple<int, string>> Filtros = new HashSet<Tuple<int, string>>();

            cmbFiltro.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
            cmbFiltro.DataSource = FiltrosTemp;
            cmbFiltro.DataValueField = "item1";
            cmbFiltro.DataTextField = "item2";
            cmbFiltro.DataBind();
        }

        private void cargarIdTipoUsuarios()
        {
            HashSet<Tuple<int, string>> TipoUsuariosTemp = new HashSet<Tuple<int, string>>();
            TipoUsuariosTemp = Seguridad.getTipoUsuarios();
            HashSet<Tuple<int, string>> tiposUsuarios = new HashSet<Tuple<int, string>>();

            cmbTipoUsuario.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
            cmbTipoUsuario.DataSource = TipoUsuariosTemp;
            cmbTipoUsuario.DataValueField = "item1";
            cmbTipoUsuario.DataTextField = "item2";
            cmbTipoUsuario.DataBind();
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }
                                                                                                                
        private void Actualiza_Paneles()
        {
            //UPDETALLE.Update();
            UPBOTONES.Update();
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

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        protected void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            objCabecera.Clear();
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPCAB.Update();

            GrillaDetalle.DataSource = null;
            GrillaDetalle.DataBind();
            UPDET.Update();

            //        return;
            //this.BtnFacturar.Attributes.Add("disabled", "disabled");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader('" + 1 + "');", true);
            if (Response.IsClientConnected)
            {
                try
                {
                    string _ERROR = string.Empty;
                    OcultarLoading("2");          

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //codigo de consulta
                
                    if (!string.IsNullOrEmpty(cmbFiltro.Text.Trim()) )//ddlEstado.SelectedValue != "0" || ddlTipoUsuario.SelectedValue != "0")
                    {
                        cargarResultados(int.Parse(hdfIdUsuarioLogeado.Value), int.Parse(cmbFiltro.SelectedValue));
                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "No se cargó los filtros"));
                    }

                    //
                    this.Ocultar_Mensaje();
                    UPCAB.Update();
                    UPDET.Update();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }

        protected string jsarguments(object idOpcion,object idFiltro)
        {
            return string.Format("{0};{1}", idOpcion != null ? idOpcion.ToString().Trim() : "0", idFiltro != null ? idFiltro.ToString().Trim() : "0");
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../appcgsa/appUsuarios.aspx");
        }

        #region "Gridview Cabecera"
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                Button btnRW = (Button)e.Row.FindControl("btnAdd");
              
            }
        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                
                var xpars = e.CommandArgument.ToString().Split(';');

                string v_idOpcion = (string.IsNullOrEmpty(xpars[0]) ? string.Empty : xpars[0]);

                Seguridad oSeguridad = new Seguridad();
                var us = oSeguridad.consultarUsuarioPorId(int.Parse(hdfIdUsuario.Value));
                var usLogeado = oSeguridad.consultarUsuarioPorId(int.Parse(hdfIdUsuarioLogeado.Value));
                oSeguridad.GuardarOpcionesAsignadas(int.Parse(hdfIdUsuario.Value), int.Parse(v_idOpcion),0, 0, usLogeado.idUsuario, usLogeado.nombreUsuario);

                tablePagination.DataSource = null;
                tablePagination.DataBind();

                GrillaDetalle.DataSource = null;
                GrillaDetalle.DataBind();
                cargarResultados(usLogeado.idUsuario, int.Parse(cmbFiltro.SelectedValue));
               
                UPDET.Update();
                UPCAB.Update();
              
            }
        }
        protected void tablePagination_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (tablePagination.Rows.Count > 0)
                {
                    tablePagination.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    tablePagination.HeaderRow.TableSection = TableRowSection.TableHeader;

                   
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }

        }
        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          
        }
        #endregion

        #region "Grilla de Detalle Booking"
        protected void GrillaDetalle_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (GrillaDetalle.Rows.Count > 0)
                {
                    GrillaDetalle.UseAccessibleHeader = true;
                    // Agrega la sección THEAD y TBODY.
                    GrillaDetalle.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void GrillaDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
        }
        protected void GrillaDetalle_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {

                var xpars = e.CommandArgument.ToString().Split(';');

                string v_idOpcion = (string.IsNullOrEmpty(xpars[0]) ? string.Empty : xpars[0]);

                Seguridad oSeguridad = new Seguridad();
                var us = oSeguridad.consultarUsuarioPorId(int.Parse(hdfIdUsuario.Value));
                var usLogeado = oSeguridad.consultarUsuarioPorId(int.Parse(hdfIdUsuarioLogeado.Value));
                oSeguridad.EliminarOpcionesAsignadas(int.Parse(hdfIdUsuario.Value), int.Parse(v_idOpcion), usLogeado.idUsuario, usLogeado.nombreUsuario);

                tablePagination.DataSource = null;
                tablePagination.DataBind();

                GrillaDetalle.DataSource = null;
                GrillaDetalle.DataBind();
                cargarResultados(usLogeado.idUsuario, int.Parse(cmbFiltro.SelectedValue));

                UPDET.Update();
                UPCAB.Update();

            }
        }
        protected void GrillaDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }
        #endregion

        private void cargarResultados( int usuario, int filtro)
        {
            string identificacionEmpresa = string.Empty;
            if (us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
            {
                identificacionEmpresa = us.codigoEmpresa;
            }
            else
            {
                identificacionEmpresa = "0";
            }

            try
            {
                //List<UsuarioSeguridad> usuarios = new List<UsuarioSeguridad>();
                //Seguridad seguridad = new Seguridad();
                var OpcionesDisponibles = Seguridad.getOpcionesDisponibles(usuario, filtro);
                var OpcionesAsignadas = Seguridad.getOpcionesAsignadas(int.Parse(hdfIdUsuario.Value));
                //var OpcionesExistentes = Seguridad.getOpcionesDisponibles(int.Parse(hdfIdUsuario.Value), filtro);


                //var OpcionesDisponiblesFinal = (from A in OpcionesDisponibles
                //                                join B in OpcionesAsignadas on new { A.opcion}  equals new { B.opcion }
                //                                select new
                //                                { }).ToList();

                var OpcionesDisponiblesFinal = (from t in OpcionesDisponibles
                                                 where !OpcionesAsignadas.Any(pv => pv.opcion == t.opcion)
                                                 select t).ToList();


                //usuarios = seguridad. (usuario, nombreUsuario, identificacionUsuario, nombreEmpresa, estado, tipoUsuario, identificacionEmpresa);
                if (OpcionesDisponiblesFinal != null)
                {
                    if (OpcionesDisponiblesFinal.Count > 0)
                    {
                       
                        tablePagination.DataSource = OpcionesDisponiblesFinal.OrderBy(x => x.opcion);
                        tablePagination.DataBind();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                    }
                }

                if (OpcionesAsignadas != null)
                {
                    if (OpcionesAsignadas.Count > 0)
                    {

                        GrillaDetalle.DataSource = OpcionesAsignadas.Where(y=> y.IdFiltro == int.Parse(cmbFiltro.SelectedValue.ToString())) .OrderBy(x => x.opcion);
                        GrillaDetalle.DataBind();
                    }
                    else
                    {
                        GrillaDetalle.DataSource = null;
                        GrillaDetalle.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
              Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }

            UPDET.Update();
            UPCAB.Update();
        }
    }

    public class user_options
    {
        public int? servicio { get; set; }
        public int? opcion { get; set; }
        public string descripcion { get; set; }
        public int? IdFiltro { get; set; }
        public int? IdUsuario { get; set; }
    }
}