using BreakBulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBSEntidades.Banano;
using VBSEntidades.BananoBodega;

namespace CSLSite
{
    public partial class VBS_BAN_Bodega : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;

        #endregion

        private BAN_Catalogo_Bodega oBodega
        {
            get
            {
                return (BAN_Catalogo_Bodega)Session["SessionBAN_Catalogo_Bodega"];
            }
            set
            {
                Session["SessionBAN_Catalogo_Bodega"] = value;
            }
        }

        private BAN_Catalogo_Bloque oBloque
        {
            get
            {
                return (BAN_Catalogo_Bloque)Session["SessionBAN_Catalogo_Bloque"];
            }
            set
            {
                Session["SessionBAN_Catalogo_Bloque"] = value;
            }
        }

        #region "Variables"
        private static Int64? lm = -3;
        private string OError;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode(txtNombre.Text.Trim());
                Server.HtmlEncode(txtCodigo.Text.Trim());
                if (!Page.IsPostBack)
                {
                    LlenaComboTipoBodega();
                    Listado_Bodegas();
                    LlenaComboBodega();
                    this.Ocultar_Mensaje();
                    this.Ocultar_Mensaje_Det();
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
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
            this.banmsgDet.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                this.banmsgDet.InnerText = string.Empty;
                ClsUsuario = Page.Tracker();
                txtNombre.Text = string.Empty;
                txtCodigo.Text = string.Empty;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Metodos"

      
        private void Listado_Bodegas()
        {
            try
            {
                var Resultado = BAN_Catalogo_Bodega.ConsultarLista(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtNombre.Focus();
                }

                if (Resultado != null)
                {

                    var oListaTipoBodega = BAN_Catalogo_TipoBodega.ConsultarLista(out OError);

                    foreach (var item in Resultado)
                    {
                        item.oTipoBodega = oListaTipoBodega.Where(p => p.id == item.idTipo).FirstOrDefault();
                    }

                    var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.nombre))
                                    select new
                                    {
                                        id = Tbl.id,
                                        nombre = Tbl.nombre.Trim(),
                                        codigo = Tbl.codigo.Trim(),
                                        tipo = Tbl.oTipoBodega.descripcion,
                                        estado = Tbl.estado,
                                        capacidad = Tbl.capacidadBox,
                                    };
                    if (LinqQuery != null && LinqQuery.Count() > 0)
                    {
                        tablePagination.DataSource = LinqQuery;
                        tablePagination.DataBind();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                    }

                    this.Actualiza_Paneles();
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Bodegas), "brbkDevice.ListadoDevice", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        private void Listado_BloqueBodegas(int idBodega, BAN_Catalogo_Bodega oBodegaEsp)
        {
            try
            {
                var Resultado = BAN_Catalogo_Bloque.ConsultarLista(idBodega, out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtNombre.Focus();
                }

                if (Resultado != null)
                {
                    foreach (var item in Resultado)
                    {
                        item.oBodega = oBodegaEsp;
                    }

                    var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.nombre))
                                    select new
                                    {
                                        id = Tbl.id,
                                        nombre = Tbl.nombre.Trim(),
                                        bodega = Tbl.oBodega?.nombre,
                                        estado = Tbl.estado,
                                    };
                    if (LinqQuery != null && LinqQuery.Count() > 0)
                    {
                        tablePginacionDetalle.DataSource = LinqQuery;
                        tablePginacionDetalle.DataBind();
                    }
                    else
                    {
                        tablePginacionDetalle.DataSource = null;
                        tablePginacionDetalle.DataBind();
                    }

                    this.Actualiza_Paneles_Det();
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Bodegas), "brbkDevice.ListadoDevice", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        public void LlenaComboTipoBodega()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_TipoBodega.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbTipobodega.DataSource = oEntidad;
                cmbTipobodega.DataValueField = "id";
                cmbTipobodega.DataTextField = "descripcion";
                cmbTipobodega.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboTipoBodega), "VBS_BAN_Bodega.LlenaComboTipoBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboBodega()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Bodega.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbBloqueBodega.DataSource = oEntidad;
                cmbBloqueBodega.DataValueField = "id";
                cmbBloqueBodega.DataTextField = "nombre";
                cmbBloqueBodega.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");
            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje_Det(string Mensaje)
        {
            this.banmsgDet.Visible = true;
            this.banmsgDet.InnerHtml = Mensaje;
            OcultarLoading("2");
            this.Actualiza_Paneles_Det();
        }


        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        private void Actualiza_Paneles()
        {
            this.UPCARGA.Update();
        }

        private void Actualiza_Paneles_Det()
        {
            this.UPEDIT.Update();
            this.UPEditDet.Update();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");

            
            txtCodigo.ForeColor = System.Drawing.Color.Black;
            txtNombre.ForeColor = System.Drawing.Color.Black;
            cmbTipobodega.ForeColor = System.Drawing.Color.Black;
            txtBox.ForeColor = System.Drawing.Color.Black;
            cmbEstado.ForeColor = System.Drawing.Color.Black;
            
            txtCodigo.BackColor = System.Drawing.Color.Empty;
            txtNombre.BackColor = System.Drawing.Color.Empty;
            cmbTipobodega.BackColor = System.Drawing.Color.Empty;
            txtBox.BackColor = System.Drawing.Color.Empty;
            cmbEstado.BackColor = System.Drawing.Color.Empty;
            cmbEstado.SelectedValue = "True";
        }

        private void Ocultar_Mensaje_Det()
        {
            this.banmsgDet.InnerText = string.Empty;
            this.banmsgDet.Visible = false;
            this.Actualiza_Paneles_Det();
            OcultarLoading("2");

            cmbBloqueBodega.ForeColor = System.Drawing.Color.Black;
            txtBloqueNombre.ForeColor = System.Drawing.Color.Black;
            cmbBloqueEstado.ForeColor = System.Drawing.Color.Black;

            cmbBloqueBodega.BackColor = System.Drawing.Color.Empty;
            txtBloqueNombre.BackColor = System.Drawing.Color.Empty;
            cmbBloqueEstado.BackColor = System.Drawing.Color.Empty;
            cmbBloqueEstado.SelectedValue = "True";
        }
        #endregion

        #region "Eventos"
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
                        var Resultado = BAN_Catalogo_Bodega.GetEntidad(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oBodega = Resultado;
                            txtNombre.Text = Resultado.nombre.Trim();
                            txtCodigo.Text = Resultado.codigo.Trim();
                            cmbTipobodega.SelectedValue = Resultado.idTipo.ToString();
                            txtBox.Text = Resultado.capacidadBox.ToString();
                            cmbEstado.SelectedValue = Resultado.estado.ToString();

                            txtCodigo.ForeColor = System.Drawing.Color.Blue;
                            txtNombre.ForeColor = System.Drawing.Color.Blue;
                            cmbTipobodega.ForeColor = System.Drawing.Color.Blue;
                            txtBox.ForeColor = System.Drawing.Color.Blue;
                            cmbEstado.ForeColor = System.Drawing.Color.Blue;

                            txtCodigo.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtNombre.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbTipobodega.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtBox.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbEstado.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                        }
                        else
                        {
                            oBodega = null;
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }
                        Actualiza_Paneles();
                    }

                    if (e.CommandName == "Bloques")
                    {
                        
                        var Resultado = BAN_Catalogo_Bodega.GetEntidad(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            Listado_BloqueBodegas(int.Parse(id.ToString()), Resultado);
                            cmbBloqueBodega.SelectedValue = Resultado.id.ToString();
                            txtBloqueNombre.Text = string.Empty;
                            cmbBloqueEstado.SelectedValue = Resultado.estado.ToString();
                        }
                        else
                        {
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }
                        Actualiza_Paneles_Det();
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

        protected void tablePginacionDetalle_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        var Resultado = BAN_Catalogo_Bloque.GetEntidad(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oBloque = Resultado;
                            cmbBloqueBodega.SelectedValue = Resultado.idBodega.ToString();
                            txtBloqueNombre.Text = Resultado.nombre.Trim();
                            cmbBloqueEstado.SelectedValue = Resultado.estado.ToString();

                            cmbBloqueBodega.ForeColor = System.Drawing.Color.Blue;
                            txtBloqueNombre.ForeColor = System.Drawing.Color.Blue;
                            cmbBloqueEstado.ForeColor = System.Drawing.Color.Blue;

                            cmbBloqueBodega.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtBloqueNombre.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbBloqueEstado.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                        }
                        else
                        {
                            oBloque = null;
                            this.Mostrar_Mensaje_Det(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }
                        Actualiza_Paneles_Det();
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

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    txtNombre.Text = txtNombre.Text.ToUpper().Trim();
                    txtCodigo.Text = txtCodigo.Text.ToUpper().Trim();
                    OcultarLoading("1");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtNombre.Text))
                    {
                        this.Alerta("Ingrese el nombre de la bodega.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre de la bodega"));
                        this.txtNombre.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtCodigo.Text))
                    {
                        this.Alerta("Ingrese el código de la bodega.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor Ingrese el código de la bodega"));
                        this.txtCodigo.Focus();
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

                    BAN_Catalogo_Bodega oEntidad = new BAN_Catalogo_Bodega();
                    if (oBodega != null)
                    {
                        oEntidad.id = oBodega.id;
                        oEntidad.nombre = txtNombre.Text;
                        oEntidad.codigo = txtCodigo.Text;
                        oEntidad.idTipo = int.Parse(cmbTipobodega.SelectedValue.ToString());
                        oEntidad.capacidadBox = int.Parse(txtBox.Text);
                        oEntidad.estado= bool.Parse(cmbEstado.SelectedValue);
                        oEntidad.usuarioModifica = ClsUsuario.loginname;
                    }
                    else
                    {
                        oEntidad.nombre = txtNombre.Text;
                        oEntidad.codigo = txtCodigo.Text;
                        oEntidad.idTipo = int.Parse(cmbTipobodega.SelectedValue.ToString());
                        oEntidad.capacidadBox = int.Parse(txtBox.Text);
                        oEntidad.estado = bool.Parse(cmbEstado.SelectedValue);
                        oEntidad.usuarioCrea = ClsUsuario.loginname;
                    }

                    oEntidad.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.txtNombre.Focus();
                        return;
                    }
                    else
                    {
                        this.Ocultar_Mensaje();
                        btnLimpiar_Click(null, null);
                        this.Alerta("Registro exitoso");
                        this.txtNombre.Focus();
                    }
                    Listado_Bodegas();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAdd), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
                oBodega = null;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            oBodega = null;
            txtNombre.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtBox.Text = string.Empty;
            OcultarLoading("1");
            Ocultar_Mensaje();
        }

        protected void btnBloqueLimpiar_Click(object sender, EventArgs e)
        {
            oBloque = null;
            txtBloqueNombre.Text = string.Empty;
            OcultarLoading("2");
            Ocultar_Mensaje_Det();
        }

        protected void btnBloqueAdd_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    txtNombre.Text = txtNombre.Text.ToUpper().Trim();
                    txtCodigo.Text = txtCodigo.Text.ToUpper().Trim();
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("2");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtBloqueNombre.Text))
                    {
                        this.Alerta("Ingrese el nombre del Bloque.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre del Bloque"));
                        this.txtNombre.Focus();
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
                    var Resultado = BAN_Catalogo_Bodega.GetEntidad(int.Parse(cmbBloqueBodega.SelectedValue));

                    BAN_Catalogo_Bloque oEntidad = new BAN_Catalogo_Bloque();
                    if (oBloque != null)
                    {
                        oEntidad.id = oBloque.id;
                        oEntidad.idBodega = int.Parse(cmbBloqueBodega.SelectedValue.ToString());
                        oEntidad.nombre = txtBloqueNombre.Text;
                        oEntidad.estado = bool.Parse(cmbBloqueEstado.SelectedValue);
                        oEntidad.usuarioModifica = ClsUsuario.loginname;
                    }
                    else
                    {
                        oEntidad.idBodega = int.Parse(cmbBloqueBodega.SelectedValue.ToString());
                        oEntidad.nombre = txtBloqueNombre.Text;
                        oEntidad.estado = bool.Parse(cmbBloqueEstado.SelectedValue);
                        oEntidad.usuarioCrea = ClsUsuario.loginname;
                    }

                    oEntidad.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.txtBloqueNombre.Focus();
                        return;
                    }
                    else
                    {
                        this.Ocultar_Mensaje();
                        btnBloqueLimpiar_Click(null, null);
                        this.Alerta("Registro exitoso");
                        this.txtBloqueNombre.Focus();
                    }
                    Listado_BloqueBodegas(int.Parse(cmbBloqueBodega.SelectedValue), Resultado);
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAdd), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
                oBloque = null;
            }
        }

       
    }
}