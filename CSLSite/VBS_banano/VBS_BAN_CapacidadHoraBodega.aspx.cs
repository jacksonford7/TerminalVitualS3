using BillionEntidades;
using BreakBulk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBSEntidades.BananoBodega;

namespace CSLSite
{
    public partial class VBS_BAN_CapacidadHoraBodega : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private List<BAN_Capacidad_Hora_Bodega> objCapacidadHoraDet = new List<BAN_Capacidad_Hora_Bodega>();
        #endregion

        #region "Variables"
        private string cMensajes;
        private static Int64? lm = -3;
        private string OError;
        #endregion

        #region "Propiedades"
        private Int64? nSesion { get { return (Int64)Session["nSesion"]; } set { Session["nSesion"] = value; } }
        #endregion

        #region "Metodos"
        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCAB.Update();
            UPBOTONES.Update();
            UPDISPONIBILIDAD.Update();
        }
        
        private void Limpia_Datos_cliente()
        {
            this.TXTMRN.Text = string.Empty;
            this.txtNave.Text = string.Empty;
            this.txtDescripcionNave.Text = string.Empty;
            this.fecETA.Text = string.Empty;

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
        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.msjErrorDisp.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            this.msjErrorDisp.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }
       
        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.msjErrorDisp.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.msjErrorDisp.Visible = false;
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
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCapacidadHoraDet = new List<BAN_Capacidad_Hora_Bodega>();
            Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;
        }
        public void LlenaComboEstado()
        {
            try
            {
                cmbEstado.DataSource = estados.consultaEstados();
                cmbEstado.DataValueField = "ID";
                cmbEstado.DataTextField = "nombre";
                cmbEstado.DataBind();
                cmbEstado.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboBodega()
        {
            try
            {
                cmbBodega.DataSource = BAN_Catalogo_Bodega.ConsultarLista(out OError);
                cmbBodega.DataValueField = "id";
                cmbBodega.DataTextField = "nombre";
                cmbBodega.DataBind();
                cmbBodega.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboBloque()
        {
            try
            {
                cmbBloque.DataSource = BAN_Catalogo_Bloque.ConsultarLista(int.Parse(cmbBodega.SelectedValue) ,out OError);
                cmbBloque.DataValueField = "id";
                cmbBloque.DataTextField = "nombre";
                cmbBloque.DataBind();
                cmbBloque.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void LlenarComboInicial()
        {
            try
            {
                List<BAN_HorarioInicial_Bodega> Listado = BAN_HorarioInicial_Bodega.ConsultarHorariosIniciales(out cMensajes);
                //var idhora = Listado.FirstOrDefault().Id_Hora;

                this.cboHorarioInicial.DataSource = Listado;
                this.cboHorarioInicial.DataTextField = "Desc_Hora";
                this.cboHorarioInicial.DataValueField = "Id_Hora";
                this.cboHorarioInicial.DataBind();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_cboHorarioInicials", "Hubo un error al cargar Tipo de cargas", t.loginname));
                this.Mostrar_Mensaje(Error);
            }
        }
        private void LlenarComboFinal(int idHoraInicial)
        {
            try
            {
                List<BAN_HorarioFinal_Bodega> Listado = BAN_HorarioFinal_Bodega.ConsultarHorarioFinal(out cMensajes);

                var oLista = Listado.Where(p => p.Id_HorarioIni >= idHoraInicial).ToList();
                this.cboHorarioFinal.DataSource = oLista;
                this.cboHorarioFinal.DataTextField = "Desc_Hora";
                this.cboHorarioFinal.DataValueField = "Id_Hora";
                this.cboHorarioFinal.DataBind();
                UPTurnos.Update();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_cboHorarioInicials", "Hubo un error al cargar Tipo de cargas", t.loginname));
                this.Mostrar_Mensaje(Error);
            }
        }

        private void ConsultarDataTarjaN4Middle()
        {
            bool vActivo = false;
            try
            {
                var Resultado = BAN_Capacidad_Hora_Bodega.ConsultarListadoCapacidadPorNave(txtNave.Text, int.Parse(cmbBloque.SelectedValue),  out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                    return;
                }

                if (Resultado != null)
                {
                    var oBodega = BAN_Catalogo_Bodega.ConsultarLista(out OError);
                    
                    foreach (var a in Resultado)
                    {
                        a.oBloque = BAN_Catalogo_Bloque.GetEntidad(int.Parse(a.idBloque.ToString()));
                        a.oBloque.oBodega = oBodega.Where(p=> p.id == a.oBloque.idBodega).FirstOrDefault();
                        a.idBodega = a.oBloque?.oBodega?.id;
                        if (a.box == 0)
                        {
                            try { if (string.IsNullOrEmpty(txtBoxIngresado.Text)) { a.box = 0; } else { a.box = int.Parse(txtBoxIngresado.Text); } } catch { a.box = 0; }
                            //this.btnGrabar.Attributes.Remove("disabled");
                        }
                        else
                        {
                            vActivo = true;
                            //this.btnGrabar.Attributes["disabled"] = "disabled";
                        }
                    }

                    objCapacidadHoraDet = Resultado;
                    Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;


                    var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.idBloque.ToString()))
                                    select new
                                    {
                                        id = Tbl.id,
                                        idNave = txtNave.Text.Trim(),
                                        nave = txtDescripcionNave.Text,
                                        idHoraInicio = Tbl.idHoraInicio,
                                        horaInicio = Tbl.horaInicio,
                                        idHoraFin = Tbl.idHoraFin,
                                        horaFin = Tbl.horaFin,
                                        idBodega = Tbl.oBloque?.oBodega?.id,
                                        Bodega = Tbl.oBloque?.oBodega?.nombre,
                                        idBloque = Tbl.idBodega,
                                        Bloque = Tbl.oBloque?.nombre.Trim(),
                                        box =  Tbl.box,
                                        boxExtra = Tbl.boxExtra,
                                        estado = Tbl.estado
                                        //usuarioCrea = Tbl.usuarioCrea.Trim(),
                                        //fechaCreacion = Tbl.fechaCreacion?.ToString("dd/MM/yyyy HH:mm"),
                                        //fechaModificacion = Tbl.fechaModifica.Value.ToString("dd/MM/yyyy HH:mm"),
                                    };
                    if (LinqQuery != null && LinqQuery.Count() > 0)
                    {
                        tablePagination.DataSource = LinqQuery;
                        tablePagination.DataBind();
                        this.btnGrabar.Attributes.Remove("disabled");
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        //this.btnGrabar.Attributes["disabled"] = "disabled";
                    }

                    //this.btnGrabar.Attributes["disabled"] = "disabled";


                    //if (Resultado.Count() > 0)
                    //{
                    //    tablePagination.DataSource = Resultado;
                    //    tablePagination.DataBind();
                    //    this.btnGrabar.Attributes.Remove("disabled");
                    //}

                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                }

                if (vActivo)
                {
                    this.btnGrabar.Attributes["disabled"] = "disabled";
                }
                else
                {
                    this.btnGrabar.Attributes.Remove("disabled");
                }
                UPBOTONES.Update();

                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDataTarjaN4Middle), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }
        protected string jsarguments(object ID_CAB, object ID_DET)
        {
            return string.Format("{0};{1}", ID_CAB != null ? ID_CAB.ToString().Trim() : "0", ID_DET != null ? ID_DET.ToString().Trim() : "0");
        }
        protected void ValidarDatosNave()
        {
            try
            {
                var oNave = nave.GetNave(txtNave.Text);
                txtDescripcionNave.Text = oNave.name;
                fecETA.Text = oNave.published_eta.ToString("dd/MM/yyyy");
                TXTMRN.Text = oNave.in_customs_voy_nbr;
                UPCAB.Update();
            }
            catch { }

        }
        private void Mostrar_MensajeLiquidacion(string Mensaje)
        {
            this.msjErrorDisp.Visible = true;
            this.msjErrorDisp.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            UPDISPONIBILIDAD.Update();
        }
        private void ConsultarDisponibilidad(string _idNave)
        {
            try
            {
                var Resultado = BAN_Capacidad_Hora_Fecha.ConsultarLista(_idNave, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.TXTMRN.Focus();
                }

                if (Resultado != null)
                {
                    if (Resultado.Count > 0)
                    {

                        var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.usuarioCrea))
                                        select new
                                        {
                                            id = Tbl.id,
                                            nave = Tbl.nave,
                                            fecha = Tbl.fecha.ToString("yyyy/MM/dd"),
                                            horaInicio = Tbl.horaInicio,
                                            horaFin = Tbl.horaFin.Trim(),
                                        };
                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            dgvDisponibilidad.DataSource = LinqQuery;
                            dgvDisponibilidad.DataBind();
                        }
                    }
                    else
                    {
                        dgvDisponibilidad.DataSource = null;
                        dgvDisponibilidad.DataBind();
                    }
                }
                else
                {
                    dgvDisponibilidad.DataSource = null;
                    dgvDisponibilidad.DataBind();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultarDisponibilidad), "ConsultarDisponibilidad", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        #endregion
        #region "Forma"
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
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
                this.msjErrorDisp.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;
                    this.msjErrorDisp.InnerText = string.Empty;

                    ClsUsuario = Page.Tracker();
                    if (ClsUsuario != null)
                    {
                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                    }

                    this.txtNave.Text = string.Empty;
                    this.TXTMRN.Text = string.Empty;
                   
                }

                ClsUsuario = Page.Tracker();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode(this.txtNave.Text.Trim());


                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    //var ClsUsuario = HttpContext.Current.Session["control"] as usuario;
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    this.btnGrabar.Attributes["disabled"] = "disabled";
                    sinresultado.Visible = false;
                    //objCapacidadHoraDet.Detalle = objDetalle;
                    LlenaComboEstado();
                    LlenaComboBodega();
                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    LlenarComboInicial();
                    cboHorarioInicial.Items.Add(item);

                    cmbBodega.Items.Add(item);
                    cmbBodega.SelectedValue = "0";
                    cboHorarioInicial.SelectedValue = "0";
                    cmbEstado.SelectedValue = "NUE";

                    

                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion



        #region "Eventos"
        #region "Gridview Cabecera"
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               // Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                CheckBox Chk = (CheckBox)e.Row.FindControl("CHKPRO");
                TextBox txtBox = (TextBox)e.Row.FindControl("txtBoxes");
                TextBox txtBoxExtras = (TextBox)e.Row.FindControl("txtBoxExtra");

                string v_estado = DataBinder.Eval(e.Row.DataItem, "estado").ToString().Trim();
                string v_box = DataBinder.Eval(e.Row.DataItem, "box").ToString().Trim();
                string v_boxExtra = DataBinder.Eval(e.Row.DataItem, "boxExtra").ToString().Trim();

                long v_id = long.Parse(DataBinder.Eval(e.Row.DataItem, "id").ToString().Trim());
               
                if (v_id <= 0)
                {
                    //btnEditar.Enabled = false;
                    Chk.Checked = true;
                    try { if (string.IsNullOrEmpty(txtBoxIngresado.Text)) { v_box = "0"; v_boxExtra = "0"; } else { v_box = txtBoxIngresado.Text; v_boxExtra = txtBoxExtra.Text; }  } catch { v_box = "0"; v_boxExtra = "0"; }
                    txtBox.Text = v_box;//"0";
                    txtBoxExtras.Text = v_boxExtra;
                //this.btnGrabar.Attributes.Remove("disabled");
                }
                else
                {

                    //this.btnGrabar.Attributes["disabled"] = "disabled";

                    if (v_estado !="True")
                    {
                        Chk.Checked = false;
                        e.Row.ForeColor = System.Drawing.Color.DarkSlateBlue;
                    }
                    else
                    {
                        Chk.Checked = true;
                        txtBox.Text = v_box;
                        txtBoxExtras.Text = v_boxExtra;
                    }
                }


                try
                {
                    var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    ClsUsuario = ClsUsuario_;
                }
                catch
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }


                int wContenedor = int.Parse(tablePagination.DataKeys[e.Row.RowIndex].Value.ToString());
                var currentStatRow = (from objCab in objCapacidadHoraDet.AsEnumerable()
                                      where objCab.id == wContenedor
                                      select objCab).FirstOrDefault();

                currentStatRow.idNave = txtNave.Text;
                currentStatRow.nave = txtDescripcionNave.Text;
                currentStatRow.usuarioCrea = ClsUsuario.loginname;

                /* objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora>;

                 var currentStatRow = (from objCab in objCapacidadHoraDet.AsEnumerable()
                                       where objCab.id == wContenedor
                                       select objCab).FirstOrDefault();*/

                Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;

                this.Actualiza_Paneles();
            }


        }
        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }
                objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;
                if (objCapacidadHoraDet == null) { return; }

                var oDet = objCapacidadHoraDet.Where(a => a.id == v_ID).FirstOrDefault();
             
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

                    // Agrega el elemento TH en la fila de encabezado.               
                    // Agrega la sección TFOOT. 
                    //tablePagination.FooterRow.TableSection = TableRowSection.TableFooter;
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }

        }
        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;

                if (objCapacidadHoraDet == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Debe generar la consulta"));
                    return;
                }
                else
                {
                    tablePagination.PageIndex = e.NewPageIndex;
                    tablePagination.DataSource = objCapacidadHoraDet;
                    tablePagination.DataBind();
                    this.Actualiza_Paneles();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));
            }
        }
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    //var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "Modificar")
                    {
                        //PagoAsignado pago = new PagoAsignado(id, ClsUsuario.loginname.Trim());
                        /*var Resultado = productos.GetProducto(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oProducto = Resultado;
                            txtNombre.Text = Resultado.nombre;
                            cmbEstado.SelectedValue = Resultado.estado.ToString();
                            cmbManiobra.SelectedValue = Resultado.Maniobra.id.ToString();
                            cmbItem.SelectedValue = Resultado.Items.id.ToString();
                        }
                        else
                        {
                            oProducto = null;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }*/
                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "BorrarAsignacion", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }
        #endregion
        #region "Gridview Fechas"
        protected void dgvDisponibilidad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    //var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "Eliminar")
                    {
                        //PagoAsignado pago = new PagoAsignado(id, ClsUsuario.loginname.Trim());
                        /*var Resultado = productos.GetProducto(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oProducto = Resultado;
                            txtNombre.Text = Resultado.nombre;
                            cmbEstado.SelectedValue = Resultado.estado.ToString();
                            cmbManiobra.SelectedValue = Resultado.Maniobra.id.ToString();
                            cmbItem.SelectedValue = Resultado.Items.id.ToString();
                        }
                        else
                        {
                            oProducto = null;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }*/
                        try
                        {
                            var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                            ClsUsuario = ClsUsuario_;
                        }
                        catch
                        {
                            Response.Redirect("../login.aspx", false);
                            return;
                        }
                        BAN_Capacidad_Hora_Fecha oEntidad = new BAN_Capacidad_Hora_Fecha();
                        oEntidad.id = id;
                        oEntidad.fecha = DateTime.Now;
                        oEntidad.estado = false;
                        oEntidad.usuarioModifica = ClsUsuario.loginname;
                        msjErrorDisp.Visible = false;

                        oEntidad.id = oEntidad.Save_Update(out OError);

                        if (OError != string.Empty)
                        {
                            this.Alerta(OError);
                            this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                            this.TxtFechaDesde.Focus();
                            return;
                        }
                        else
                        {
                            ConsultarDisponibilidad(txtNave.Text);
                            this.Alerta("Transacción exitosa");
                        }



                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "BorrarAsignacion", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }
        #endregion
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes["disabled"] = "disabled";
            Limpia_Datos_cliente();
            //objDetalle.Clear();
            objCapacidadHoraDet = new List<BAN_Capacidad_Hora_Bodega>();
            Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.btnGrabar.Attributes["disabled"] = "disabled";
            sinresultado.Visible = false;
            ValidarDatosNave();
            tablePagination.DataSource = null;
            tablePagination.DataBind();
            this.Ocultar_Mensaje();
            UPDETALLE.Update();
            if (Response.IsClientConnected)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.txtNave.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                        this.txtNave.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>MRN no encontrado"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.fecETA.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Valor de ETA no válido"));
                        this.fecETA.Focus();
                        return;
                    }

                    if (this.cmbBodega.SelectedValue == "0")
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Seleccione una bodega y bloque"));
                        this.fecETA.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbBloque.SelectedValue.ToString()))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Seleccione un bloque"));
                        this.fecETA.Focus();
                        return;
                    }

                    ConsultarDataTarjaN4Middle();
                    ConsultarDisponibilidad(txtNave.Text);
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    this.btnGrabar.Attributes["disabled"] = "disabled";
                    UPDETALLE.Update();

                    objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;
                    if (objCapacidadHoraDet != null)
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            Session.Clear();
                            OcultarLoading("1");
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtNave.Text))
                        {
                            this.Alerta("Ingrese el nombre de la nave.");
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre de la nave"));
                            this.txtNave.Focus();
                            return;
                        }


              

                        if (objCapacidadHoraDet != null)
                        {
                            if (objCapacidadHoraDet.Count > 0)
                            {
                                try
                                {
                                    var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                                    ClsUsuario = ClsUsuario_;
                                }
                                catch
                                {
                                    Response.Redirect("../login.aspx", false);
                                    return;
                                }


                                using (var scope = new TransactionScope())
                                {
                                    foreach (var oItem in objCapacidadHoraDet)
                                    {
                                        BAN_Capacidad_Hora_Bodega oBAN_Capacidad_Hora = new BAN_Capacidad_Hora_Bodega();
                                        oBAN_Capacidad_Hora = oItem;
                                        oBAN_Capacidad_Hora.usuarioModifica = ClsUsuario.loginname;
                                        oItem.id = oBAN_Capacidad_Hora.Save_Update(out OError);

                                        if (OError != string.Empty)
                                        {
                                            break;
                                        }
                                    }
                                    if ( string.IsNullOrEmpty(OError))
                                    {
                                        scope.Complete();
                                    }
                                }

                                if (OError != string.Empty)
                                {
                                    this.Alerta(OError);
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                                    this.TXTMRN.Focus();
                                    return;
                                }
                                else
                                {
                                    btnBuscar_Click(null, null);
                                    this.Alerta("Transacción exitosa");
                                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Configuración de la nave {0} almacenada con exito", txtNave.Text.ToString()));
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGrabar_Click), "btnGrabar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                //this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        #endregion

        protected void txtBoxes_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtBox = (TextBox)row.FindControl("txtBoxes");
            int wContenedor = int.Parse(tablePagination.DataKeys[row.RowIndex].Value.ToString());

            objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;

            var currentStatRow = (from objCab in objCapacidadHoraDet.AsEnumerable()
                                  where objCab.id == wContenedor
                                  select objCab).FirstOrDefault();

            string naveSelect = txtDescripcionNave.Text.ToString();
            string IdEmpresa = string.Empty;

            if (string.IsNullOrEmpty(txtBox.Text.ToString()))
            {
                txtBox.Text = "0";
            }

            currentStatRow.box = int.Parse( txtBox.Text.ToString());
            currentStatRow.nave = naveSelect;
            this.btnGrabar.Attributes.Remove("disabled");
            Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;



            //############################
            TextBox currentTextBox = (TextBox)sender;

            // Obtén el índice de fila actual
            GridViewRow currentRow = (GridViewRow)currentTextBox.Parent.Parent;
            int rowIndex = currentRow.RowIndex;

            // Obtén el índice de columna actual
            int colIndex = currentTextBox.TabIndex;

            // Mueve el foco al siguiente control en la misma columna pero en la siguiente fila
            int nextRowIndex = rowIndex + 1;
            if (nextRowIndex < tablePagination.Rows.Count)
            {
                Control nextControl = tablePagination.Rows[nextRowIndex].Cells[colIndex].FindControl("txtBoxes");
                if (nextControl != null && nextControl is TextBox)
                {
                    ((TextBox)nextControl).Focus();
                }
            }


            this.Actualiza_Paneles();
        }
        protected void txtBoxExtra_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
                TextBox txtBox = (TextBox)row.FindControl("txtBoxExtra");
                int wContenedor = int.Parse(tablePagination.DataKeys[row.RowIndex].Value.ToString());

                objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;

                var currentStatRow = (from objCab in objCapacidadHoraDet.AsEnumerable()
                                      where objCab.id == wContenedor
                                      select objCab).FirstOrDefault();

                string naveSelect = txtDescripcionNave.Text.ToString();
                string IdEmpresa = string.Empty;

                if (string.IsNullOrEmpty(txtBox.Text.ToString()))
                {
                    txtBox.Text = "0";
                }

                currentStatRow.boxExtra = int.Parse(txtBox.Text.ToString());
                currentStatRow.nave = naveSelect;
                this.btnGrabar.Attributes.Remove("disabled");
                Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;



                //############################
                TextBox currentTextBox = (TextBox)sender;

                // Obtén el índice de fila actual
                GridViewRow currentRow = (GridViewRow)currentTextBox.Parent.Parent;
                int rowIndex = currentRow.RowIndex;

                // Obtén el índice de columna actual
                int colIndex = currentTextBox.TabIndex;

                // Mueve el foco al siguiente control en la misma columna pero en la siguiente fila
                int nextRowIndex = rowIndex + 1;
                if (nextRowIndex < tablePagination.Rows.Count)
                {
                    Control nextControl = tablePagination.Rows[nextRowIndex].Cells[colIndex].FindControl("txtBoxes");
                    if (nextControl != null && nextControl is TextBox)
                    {
                        ((TextBox)nextControl).Focus();
                    }
                }


                this.Actualiza_Paneles();
            }
            catch
            {

            }
        }

        protected void CHKPRO_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            CheckBox chkEstado = (CheckBox)row.FindControl("CHKPRO");
            int wContenedor = int.Parse(tablePagination.DataKeys[row.RowIndex].Value.ToString());

            objCapacidadHoraDet = Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] as List<BAN_Capacidad_Hora_Bodega>;

            var currentStatRow = (from objCab in objCapacidadHoraDet.AsEnumerable()
                                  where objCab.id == wContenedor
                                  select objCab).FirstOrDefault();

            currentStatRow.estado = chkEstado.Checked;
            currentStatRow.usuarioModifica = "";
            this.btnGrabar.Attributes.Remove("disabled");
            Session["Transaccion_BAN_Capacidad_HoraBodega" + this.hf_BrowserWindowName.Value] = objCapacidadHoraDet;
            this.Actualiza_Paneles();
        }

        protected void cmbBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaComboBloque();
                this.Actualiza_Paneles();
            }
            catch
            {

            }
        }

        protected void btnAgregarFecha_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrorDisp.Visible = false;
                if (Response.IsClientConnected)
                {
                    DateTime fecha = new DateTime();
                    CultureInfo enUS = new CultureInfo("en-US");
                    if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fecha))
                    {
                        this.Alerta(string.Format("EL FORMATO DE FECHA PROCESO DEBE SER Mes/dia/Anio {0}", TxtFechaDesde.Text));
                        TxtFechaDesde.Focus();
                        return;
                    }

                    if (this.cboHorarioInicial.SelectedValue == "0")
                    {
                        this.Mostrar_MensajeLiquidacion(string.Format("<b>Informativo! </b>Por favor seleccione un horario"));
                        this.cboHorarioInicial.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNave.Text))
                    {
                        this.Mostrar_MensajeLiquidacion(string.Format("<b>Informativo! </b>Por favor seleccione una referencia"));
                        this.txtNave.Focus();
                        return;
                    }

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }
                    BAN_Capacidad_Hora_Fecha oEntidad = new BAN_Capacidad_Hora_Fecha();
                    oEntidad.idNave = txtNave.Text;
                    oEntidad.nave = txtDescripcionNave.Text;
                    oEntidad.fecha = fecha;
                    oEntidad.idHoraInicio = int.Parse(cboHorarioInicial.SelectedValue);
                    oEntidad.horaInicio = cboHorarioInicial.SelectedItem.Text;
                    oEntidad.idHoraFin = int.Parse(cboHorarioFinal.SelectedValue);
                    oEntidad.horaFin = cboHorarioFinal.SelectedItem.Text;
                    oEntidad.estado = true;
                    oEntidad.usuarioCrea = ClsUsuario.loginname;
                    msjErrorDisp.Visible = false;

                    oEntidad.id = oEntidad.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_MensajeLiquidacion(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.TxtFechaDesde.Focus();
                        return;
                    }
                    else
                    {
                        ConsultarDisponibilidad(txtNave.Text);
                        this.Alerta("Transacción exitosa");
                    }

                }
            }
            catch (Exception ex)
            {
                //this.btnActualizar.Attributes["disabled"] = "disabled";
                Session["TransaccionStowage_Plan_Det" + this.hf_BrowserWindowName.Value] = null;
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAgregarFecha_Click), "btnAgregarFecha_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
            OcultarLoading("1");
            OcultarLoading("2");
            Actualiza_Paneles();
        }

        protected void cboHorarioInicial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboHorarioInicial.SelectedValue == "0")
                {
                    cboHorarioFinal.Items.Clear();
                    cboHorarioFinal.DataSource = null;
                    this.cboHorarioFinal.DataBind();
                    return;
                }

                LlenarComboFinal(int.Parse(cboHorarioInicial.SelectedValue));
                this.Actualiza_Paneles();
            }
            catch { }
        }

        protected void btnDisponiblidad_Click(object sender, EventArgs e)
        {
            Ocultar_Mensaje();
        }
    }
}