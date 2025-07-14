using BreakBulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.brbk
{
    public partial class brbkproducto : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
   
        #endregion

        private productos oProducto
        {
            get
            {
                return (productos)Session["SessionProducto"];
            }
            set
            {
                Session["SessionProducto"] = value;
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

                if (!Page.IsPostBack)
                {
                    LlenaComboManiobras();
                    LlenaComboItems();
                    Listado_Cargas();
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

            this.banmsg.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                ClsUsuario = Page.Tracker();
                txtNombre.Text= string.Empty;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Metodos"

        public void LlenaComboManiobras()
        {
            try
            {
                var dtManiobras = maniobra.consultaManiobras(); //ds.Tables[0].DefaultView;
                cmbManiobra.DataSource = dtManiobras;
                cmbManiobra.DataValueField = "ID";
                cmbManiobra.DataTextField = "nombre";
                cmbManiobra.DataBind();
                cmbManiobra.Enabled = true;

                cmbManiobra2.DataSource = dtManiobras;
                cmbManiobra2.DataValueField = "ID";
                cmbManiobra2.DataTextField = "nombre";
                cmbManiobra2.DataBind();
                cmbManiobra2.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboManiobras), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboItems()
        {
            try
            {
                cmbItem.DataSource = items.consultaItems(); //ds.Tables[0].DefaultView;
                cmbItem.DataValueField = "ID";
                cmbItem.DataTextField = "nombre";
                cmbItem.DataBind();
                cmbItem.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboManiobras), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void Listado_Cargas()
        {
            try
            {
                var Resultado = productos.listadoProductos(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtNombre.Focus();
                }

                if (Resultado != null)
                {
                    foreach (var a in Resultado)
                    {
                        a.Maniobra = maniobra.GetManiobra(a.idManiobra);
                        a.Maniobra2 = maniobra.GetManiobra(a.idManiobra2);
                        a.Items = items.GetItems(a.item);
                    }

                    var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.nombre))
                                    select new
                                    {
                                        id = Tbl.id,
                                        nombre = Tbl.nombre.Trim(),
                                        maniobra = Tbl.Maniobra?.nombre.Trim(),
                                        maniobra2 = Tbl.Maniobra2?.nombre.Trim(),
                                        categoria = Tbl.Items?.nombre.Trim(),
                                        estado = Tbl.estado,
                                        usuarioCrea = Tbl.usuarioCrea.Trim(),
                                        fechaCreacion = Tbl.fechaCreacion.Value.ToString("dd/MM/yyyy HH:mm"),
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Cargas), "ListaAsignacion", false, null, null, ex.StackTrace, ex);
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
                        var Resultado = productos.GetProducto(int.Parse(id.ToString()));
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
                        }
                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "BorrarAsignacion", false, null, null, ex.StackTrace, ex);
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
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbManiobra.SelectedValue)) 
                    {
                        this.Alerta("Seleccione una maniobra.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una maniobra."));
                        cmbManiobra.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbItem.SelectedValue))
                    {
                        this.Alerta("Seleccione un Item.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una maniobra."));
                        cmbManiobra.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtNombre.Text))
                    {
                        this.Alerta("Ingrese el nombre del producto.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre del producto"));
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

                    productos oProd = new productos();
                    if  (oProducto != null)
                    {
                        oProd.id = oProducto.id;
                        oProd.nombre = txtNombre.Text;
                        oProd.idManiobra = int.Parse(cmbManiobra.SelectedValue);
                        oProd.idManiobra2 = int.Parse(cmbManiobra2.SelectedValue);
                        oProd.item = int.Parse(cmbItem.SelectedValue);
                        oProd.estado = bool.Parse(cmbEstado.SelectedValue);
                        oProd.usuarioModifica = ClsUsuario.loginname;
                    }
                    else
                    {
                        oProd.nombre = txtNombre.Text;
                        oProd.idManiobra = int.Parse(cmbManiobra.SelectedValue);
                        oProd.idManiobra2 = int.Parse(cmbManiobra2.SelectedValue);
                        oProd.item = int.Parse(cmbItem.SelectedValue);
                        oProd.estado = bool.Parse(cmbEstado.SelectedValue);
                        oProd.usuarioCrea = ClsUsuario.loginname;
                    }
                    
                    oProd.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}",OError));
                        this.txtNombre.Focus();
                    }
                        
                    Listado_Cargas();
                    this.Ocultar_Mensaje();
                    btnLimpiar_Click(null,null);
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAdd), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
                oProducto = null;
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            oProducto = null;
            txtNombre.Text = string.Empty;
        }
    }
}