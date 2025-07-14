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
    public partial class VBS_BAN_Ubicacion : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;

        #endregion

        private BAN_Catalogo_Ubicacion oUbicacion
        {
            get
            {
                return (BAN_Catalogo_Ubicacion)Session["SessionBAN_Ubicacion"];
            }
            set
            {
                Session["SessionBAN_Ubicacion"] = value;
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
                Server.HtmlEncode(txtBarcode.Text.Trim());
                if (!Page.IsPostBack)
                {                   
                    LlenaComboBodega();
                    cmbBodega_SelectedIndexChanged(null,null);
                    LlenaComboFila();
                    LlenaComboAltura();
                    LlenaComboProfundida();
                    Listado_Ubicacion();
                    this.Ocultar_Mensaje();
                }
            }
            catch// (Exception ex)
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
                txtBarcode.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
                txtMt2.Text = string.Empty;
                txtCapacidadBox.Text = string.Empty;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Metodos"

      
        private void Listado_Ubicacion()
        {
            try
            {
                var Resultado = BAN_Catalogo_Ubicacion.ConsultarLista(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtBarcode.Focus();
                }

                if (Resultado != null)
                {
                    var oBodega = BAN_Catalogo_Bodega.ConsultarLista(out OError);
                    var oBloque = BAN_Catalogo_Bloque.ConsultarLista(null, out OError);
                    var oFila = BAN_Catalogo_Fila.ConsultarLista(out OError);
                    var oAltura = BAN_Catalogo_Altura.ConsultarLista(out OError);
                    var oProfundidad = BAN_Catalogo_Profundidad.ConsultarLista(out OError);

                    foreach (var item in Resultado)
                    {
                        item.oBloque = oBloque.Where(p => p.id == int.Parse(item.idBloque.ToString())).FirstOrDefault();
                        item.oBloque.oBodega = oBodega.Where(p => p.id == int.Parse(item.idBodega.ToString())).FirstOrDefault();
                        item.oFila = oFila.Where(p => p.id == int.Parse(item.idFila.ToString())).FirstOrDefault();
                        item.oAltura = oAltura.Where(P => P.id == int.Parse(item.idAltura.ToString())).FirstOrDefault();
                        item.oProfundidad = oProfundidad.Where(p => p.id == int.Parse(item.idProfundidad.ToString())).FirstOrDefault();
                    }

                    var LinqQuery = from Tbl in Resultado.Where(Tbl => Tbl.idBodega == int.Parse(cmbBodega.SelectedValue) && Tbl.idBloque == int.Parse(cmbBloque.SelectedValue) && Tbl.idFila == int.Parse(cmbFila.SelectedValue))
                                    select new
                                    {
                                        id = Tbl.id,
                                        bodega = Tbl.oBloque?.oBodega?.nombre.Trim(),
                                        bloque = Tbl.oBloque?.nombre.Trim(),
                                        fila = Tbl.oFila?.descripcion.Trim(),
                                        altura = Tbl.oAltura?.descripcion.Trim(),
                                        profundidad = Tbl.oProfundidad.descripcion.Trim(),
                                        barcode = Tbl.barcode.Trim(),
                                        disponible = Tbl.disponible,
                                        estado = Tbl.estado,
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Ubicacion), "VBS_BAN_Ubicacion.Listado_Ubicacion", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        public void LlenaComboBodega()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Bodega.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbBodega.DataSource = oEntidad;
                cmbBodega.DataValueField = "id";
                cmbBodega.DataTextField = "nombre";
                cmbBodega.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboFila()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Fila.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbFila.DataSource = oEntidad;
                cmbFila.DataValueField = "id";
                cmbFila.DataTextField = "descripcion";
                cmbFila.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboAltura()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Altura.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbAltura.DataSource = oEntidad;
                cmbAltura.DataValueField = "id";
                cmbAltura.DataTextField = "descripcion";
                cmbAltura.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboProfundida()
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Profundidad.ConsultarLista(out oError); //ds.Tables[0].DefaultView;
                cmbProfundidad.DataSource = oEntidad;
                cmbProfundidad.DataValueField = "id";
                cmbProfundidad.DataTextField = "descripcion";
                cmbProfundidad.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboBodega), "VBS_BAN_Bodega.LlenaComboBodega", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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

            cmbAltura.ForeColor = System.Drawing.Color.Black;
            cmbBloque.ForeColor = System.Drawing.Color.Black;
            cmbBodega.ForeColor = System.Drawing.Color.Black;
            cmbEstado.ForeColor = System.Drawing.Color.Black;
            cmbDisponible.ForeColor = System.Drawing.Color.Black;
            cmbFila.ForeColor = System.Drawing.Color.Black;
            cmbProfundidad.ForeColor = System.Drawing.Color.Black;
            txtBarcode.ForeColor = System.Drawing.Color.Black;
            txtCapacidadBox.ForeColor = System.Drawing.Color.Black;
            txtDescripcion.ForeColor = System.Drawing.Color.Black;
            txtMt2.ForeColor = System.Drawing.Color.Black;
            
            cmbAltura.BackColor = System.Drawing.Color.Empty;
            cmbBloque.BackColor = System.Drawing.Color.Empty;
            cmbBodega.BackColor = System.Drawing.Color.Empty;
            cmbEstado.BackColor = System.Drawing.Color.Empty;
            cmbDisponible.BackColor = System.Drawing.Color.Empty;
            cmbFila.BackColor = System.Drawing.Color.Empty;
            cmbProfundidad.BackColor = System.Drawing.Color.Empty;
            txtBarcode.BackColor = System.Drawing.Color.Empty;
            txtCapacidadBox.BackColor = System.Drawing.Color.Empty;
            txtDescripcion.BackColor = System.Drawing.Color.Empty;
            txtMt2.BackColor = System.Drawing.Color.Empty;
            cmbEstado.SelectedValue = "True";
            cmbDisponible.SelectedValue = "True";
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
                        var Resultado = BAN_Catalogo_Ubicacion.GetEntidad(int.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oUbicacion = Resultado;
                            cmbBodega.SelectedValue = Resultado.idBodega.ToString();
                            cmbBodega_SelectedIndexChanged(null,null);
                            cmbBloque.SelectedValue = Resultado.idBloque.ToString();
                            cmbFila.SelectedValue = Resultado.idFila.ToString();
                            cmbAltura.SelectedValue = Resultado.idAltura.ToString();
                            cmbProfundidad.SelectedValue = Resultado.idProfundidad.ToString();
                            cmbEstado.SelectedValue = Resultado.estado.ToString();
                            cmbDisponible.SelectedValue = Resultado.disponible.ToString();
                            txtBarcode.Text = Resultado.barcode;
                            txtDescripcion.Text = Resultado.descripcion;
                            txtCapacidadBox.Text = Resultado.capacidadBox.ToString();
                            txtMt2.Text = Resultado.mt2.ToString();

                            cmbAltura.ForeColor = System.Drawing.Color.Blue;
                            cmbBloque.ForeColor = System.Drawing.Color.Blue;
                            cmbBodega.ForeColor = System.Drawing.Color.Blue;
                            cmbEstado.ForeColor = System.Drawing.Color.Blue;
                            cmbDisponible.ForeColor = System.Drawing.Color.Blue;
                            cmbFila.ForeColor = System.Drawing.Color.Blue;
                            cmbProfundidad.ForeColor = System.Drawing.Color.Blue;
                            txtBarcode.ForeColor = System.Drawing.Color.Blue;
                            txtCapacidadBox.ForeColor = System.Drawing.Color.Blue;
                            txtDescripcion.ForeColor = System.Drawing.Color.Blue;
                            txtMt2.ForeColor = System.Drawing.Color.Blue;

                            cmbAltura.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbBloque.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbBodega.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbEstado.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbDisponible.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbFila.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            cmbProfundidad.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtBarcode.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtCapacidadBox.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtDescripcion.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            txtMt2.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                        }
                        else
                        {
                            oUbicacion = null;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }
                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
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
                    txtBarcode.Text = txtBarcode.Text.ToUpper().Trim();
                    txtDescripcion.Text = txtDescripcion.Text.ToUpper().Trim();
                    txtCapacidadBox.Text = "0";
                    OcultarLoading("1");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
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
                    

                    BAN_Catalogo_Ubicacion oEntidad = new BAN_Catalogo_Ubicacion();
                    oEntidad.idBodega = int.Parse(cmbBodega.SelectedValue);
                    oEntidad.idBloque = int.Parse(cmbBloque.SelectedValue);
                    oEntidad.idFila = int.Parse(cmbFila.SelectedValue);
                    oEntidad.idAltura = int.Parse(cmbAltura.SelectedValue);
                    oEntidad.idProfundidad = int.Parse(cmbProfundidad.SelectedValue);
                    oEntidad.barcode = "";//txtBarcode.Text;
                    oEntidad.descripcion = "";// txtDescripcion.Text;
                    oEntidad.capacidadBox = int.Parse(txtCapacidadBox.Text);
                    oEntidad.mt2 = 0;// int.Parse(txtMt2.Text);
                    oEntidad.estado = bool.Parse(cmbEstado.SelectedValue);
                    oEntidad.disponible = bool.Parse(cmbDisponible.SelectedValue);

                    if (oUbicacion != null)
                    {
                        oEntidad.id = oUbicacion.id;
                        oEntidad.usuarioModifica = ClsUsuario.loginname;
                    }
                    else
                    {
                        oEntidad.usuarioCrea= ClsUsuario.loginname;
                    }

                    oEntidad.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.txtBarcode.Focus();
                        return;
                    }
                    else
                    {
                        this.Ocultar_Mensaje();
                        btnLimpiar_Click(null, null);
                        this.Alerta("Registro exitoso");
                        this.txtBarcode.Focus();
                    }
                    Listado_Ubicacion();
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAdd), "BtnAdd_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
                oUbicacion = null;
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            oUbicacion = null;
            txtBarcode.Text = string.Empty;
            txtCapacidadBox.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtMt2.Text = string.Empty;
            Ocultar_Mensaje();
        }

        protected void cmbBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string oError;
                var oEntidad = BAN_Catalogo_Bloque.ConsultarLista(int.Parse(cmbBodega.SelectedValue),out oError); //ds.Tables[0].DefaultView;
                cmbBloque.DataSource = oEntidad;
                cmbBloque.DataValueField = "id";
                cmbBloque.DataTextField = "nombre";
                cmbBloque.DataBind();
                Listado_Ubicacion();
            }
            catch //(Exception ex)
            {
               
            }
        }

        protected void cmbFila_SelectedIndexChanged(object sender, EventArgs e)
        {
            Listado_Ubicacion();
        }

        protected void cmbBloque_SelectedIndexChanged(object sender, EventArgs e)
        {
            Listado_Ubicacion();
        }
    }
}