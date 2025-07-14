using BreakBulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBSEntidades.BananoMuelle;

namespace CSLSite
{
    public partial class VBS_BAN_Exportador : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;

        #endregion

        private BAN_Catalogo_Exportador OShipper
        {
            get
            {
                return (BAN_Catalogo_Exportador)Session["SessionBAN_Catalogo_Exportador"];
            }
            set
            {
                Session["SessionBAN_Catalogo_Exportador"] = value;
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
                Server.HtmlEncode(txtRuc.Text.Trim());
                if (!Page.IsPostBack)
                {
                    LlenaComboLinea();
                    ConsultaListado();
                    this.Ocultar_Mensaje();
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

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                ClsUsuario = Page.Tracker();
                txtNombre.Text = string.Empty;
                txtRuc.Text = string.Empty;

                if (ClsUsuario != null)
                {
                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.txtRucCliente.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Metodos"
        public void LlenaComboLinea()
        {
            try
            {
                var oEntidad = BAN_Catalogo_Linea.ConsultarListaLlenaCombo(txtRucCliente.Text); //ds.Tables[0].DefaultView;
                cmbLinea.DataSource = oEntidad;
                cmbLinea.DataValueField = "ID";
                cmbLinea.DataTextField = "nombre";
                cmbLinea.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboLinea), "VBS_BAN_Exportador.LlenaComboLinea", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void ConsultaListado()
        {
            try
            {
                var Resultado = BAN_Catalogo_Exportador.ConsultarListaExportador(txtRucCliente.Text, out OError);
                var oLineas = BAN_Catalogo_Linea.ConsultarListaLineas(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtNombre.Focus();
                }

                if (Resultado != null)
                {
                   foreach ( BAN_Catalogo_Exportador oExportador in Resultado)
                    {
                        oExportador.Linea = oLineas.Where(p => p.id == oExportador.idLinea).FirstOrDefault();
                    }

                    var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.nombre))
                                    select new
                                    {
                                        id = Tbl.id,
                                        linea  = Tbl.Linea.codLine + " - "+ Tbl.Linea.nombre,
                                        ruc = Tbl.ruc,
                                        nombre = Tbl.nombre.Trim(),
                                        estado = Tbl.estado,
                                        usuarioCrea = Tbl.usuarioCrea.Trim(),
                                        fechaCreacion = Tbl.fechaCreacion.Value.ToString("dd/MM/yyyy HH:mm"),
                                        fechaModificacion = Tbl.fechaModifica.Value.ToString("dd/MM/yyyy HH:mm"),
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ConsultaListado), "VBS_BAN_Exportador.ConsultaListado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
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

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        private void Actualiza_Paneles()
        {
            this.UPCARGA.Update();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");

            txtRuc.ForeColor = System.Drawing.Color.Black;
            txtNombre.ForeColor = System.Drawing.Color.Black;
            cmbEstado.ForeColor = System.Drawing.Color.Black;
            txtRuc.BackColor = System.Drawing.Color.Empty;
            txtNombre.BackColor = System.Drawing.Color.Empty;
            cmbEstado.BackColor = System.Drawing.Color.Empty;
            cmbEstado.SelectedValue = "True";
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
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    btnLimpiar_Click(null, null);

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "Modificar")
                    {
                        //PagoAsignado pago = new PagoAsignado(id, ClsUsuario.loginname.Trim());
                        var Resultado = BAN_Catalogo_Exportador.GetExportaador(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            OShipper = Resultado;
                            txtNombre.Text = Resultado.nombre.Trim();
                            txtRuc.Text = Resultado.ruc.Trim();
                            cmbEstado.SelectedValue = Resultado.estado.ToString();

                            txtRuc.ForeColor = System.Drawing.Color.Blue;
                            txtNombre.ForeColor = System.Drawing.Color.Blue; ;
                            cmbEstado.ForeColor = System.Drawing.Color.Blue; ;
                            txtRuc.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtNombre.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbEstado.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                        }
                        else
                        {
                            OShipper = null;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }
                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "VBS_BAN_Exportador.tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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
                    txtRuc.Text = txtRuc.Text.ToUpper().Trim();
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.cmbLinea.Text))
                    {
                        this.Alerta("Seleccione la linea.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccione la linea"));
                        this.txtNombre.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtNombre.Text))
                    {
                        this.Alerta("Ingrese el nombre del exportador.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre del exportador"));
                        this.txtNombre.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtRuc.Text))
                    {
                        this.Alerta("Ingrese el ruc del exportador.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor Ingrese el ruc del exportador"));
                        this.txtRuc.Focus();
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

                    //#####################################################
                    //VERIFICA SI YA EXISTE EL EXPORTADOR PARA LA LINEA 
                    //#####################################################
                    if (OShipper is null)
                    {
                        var Resultado = BAN_Catalogo_Exportador.ConsultarListaExportador(txtRucCliente.Text, out OError);

                        if (Resultado.Where(p => p.idLinea == int.Parse(cmbLinea.SelectedValue) && p.ruc == txtRuc.Text).Count() > 0)
                        {
                            this.Alerta("No se permite registros repetidos [RUC]");
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor Ingrese verificar, No se permite registros repetidos [RUC]"));
                            this.txtRuc.Focus();
                            return;
                        }

                        if (Resultado.Where(p => p.idLinea == int.Parse(cmbLinea.SelectedValue) && p.nombre == txtNombre.Text).Count() > 0)
                        {
                            this.Alerta("No se permite registros repetidos [NOMBRE]");
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor Ingrese verificar, No se permite registros repetidos [NOMBRE]"));
                            this.txtNombre.Focus();
                            return;
                        }
                    }
                    

                    BAN_Catalogo_Exportador oEntidad = new BAN_Catalogo_Exportador();
                    oEntidad.idLinea = int.Parse(cmbLinea.SelectedValue);
                    oEntidad.ruc = txtRuc.Text;
                    oEntidad.nombre = txtNombre.Text;
                    oEntidad.estado = bool.Parse(cmbEstado.SelectedValue);

                    if (OShipper != null)
                    {
                        oEntidad.id = OShipper.id;
                        oEntidad.usuarioModifica = ClsUsuario.loginname;
                    }
                    else
                    {
                        oEntidad.usuarioCrea = ClsUsuario.loginname;
                    }

                    oEntidad.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
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
                    ConsultaListado();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAdd), "BtnAdd_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
                OShipper = null;
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            OShipper = null;
            txtNombre.Text = string.Empty;
            txtRuc.Text = string.Empty;
            Ocultar_Mensaje();
        }

        protected void cmbLinea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}