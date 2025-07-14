using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using CSLSite.N4Object;
using CSLSite.XmlTool;
using ConectorN4;
using Newtonsoft.Json;
using csl_log;
using System.Text;
using ControlOPC.Entidades;
using System.Data;
using System.Globalization;
using ReceptioMtyStock;

namespace CSLSite
{
    public partial class stock_register_respaldo : System.Web.UI.Page
    {
 
        private StockRegister objStockRegister = new StockRegister();
        usuario user;
        string sg;

        #region "Propiedades"

        public static string v_mensaje = string.Empty;

        #endregion

        #region "Metodos"


        private void MessageBox(String msg, Page page)
        {
            StringBuilder s = new StringBuilder();
            msg = msg.Replace("'", " ");
            msg = msg.Replace(System.Environment.NewLine, " ");
            msg = msg.Replace("/", "");
            s.Append(" alert('").Append(msg).Append("');");
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);

        }

        private void Limpiar()
        {

            this.TxtCantidad.Text = null;
            this.TxtFechaTransaccion.Text = null;
            
            CboDeposito.DataSource = null;
            CboDeposito.DataBind();

            CboOperacion.DataSource = null;
            CboOperacion.DataBind();

            TableStock.DataSource = null;
            TableStock.DataBind();

            /*carga todas las bodegas*/
            CargaDepositos();
            /*carga operaciones: ingreso, egreso*/
            this.CargaOperaciones();

            this.TxtFechaTransaccion.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm") ;  

        }

        private void CargaDepositos()
        {
            try
            {
                List<Line_Depot> Lista = Line_Depot.ListLineDepot((int)Session["id_line"]);

                if (Lista != null && Lista.Count > 0)  
                {
                    this.CboDeposito.DataSource = Lista;
                    CboDeposito.DataBind();
                }
                else
                {
                    CboDeposito.DataSource = null;
                    CboDeposito.DataBind();
                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        private void CargaOperaciones()
        {
            try
            {
                List<StockOperation> Lista = StockOperation.ListStockOperation();

                if (Lista != null && Lista.Count > 0)
                {
                    this.CboOperacion.DataSource = Lista;
                    CboOperacion.DataBind();
                }
                else
                {
                    CboOperacion.DataSource = null;
                    CboOperacion.DataBind();
                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        private void CargaLineaNaviera(string codigoempresa)
        {
            try
            {
                List<Line> Lista = Line.ListLine(codigoempresa);
                var xList = Lista.FirstOrDefault();

                if (xList != null)
                {
                    this.LblNombre.Text = xList.name;
                    Session["id_line"] = xList.id_line;
                }
                else
                {
                    Session["id_line"] = "0";
                    this.LblNombre.Text = null;
                }
            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }

        private void CargaStockRegister(Int64 _i_id_depot)
        {
            try
            {

                    List<StockRegister> ListStock = StockRegister.ListStockRegister((int) Session["id_line"], _i_id_depot);
                    if (ListStock != null)
                    {
                    TableStock.DataSource = ListStock;
                    TableStock.DataBind();
                    }
                    else {
                    TableStock.DataSource = null;
                    TableStock.DataBind();
                    }
              

            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "stock_register", "CargaLineaNaviera", "Hubo un error al cargar detalle de stock", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        //protected void RemoverStock_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {

        //            if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                Session.Clear();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                return;
        //            }

        //            var user = Page.getUserBySesion();
        //            if (user == null)
        //            {

        //                var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "RemoverGruas_ItemCommand", "No se pudo obtener usuario", "anónimo"));
        //                this.MessageBox(cMensaje2.ToString(), this);
        //                return;
        //            }

        //            if (e.CommandArgument == null)
        //            {

        //                var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "RemoverBodegas_ItemCommand", "CommandArgument is NULL", user.loginname));
        //                this.MessageBox(cMensaje2.ToString(), this);
        //                return;
        //            }
        //            //
        //            var xpars = e.CommandArgument.ToString();
        //            if (xpars.Length <= 0)
        //            {

        //                var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "RemoverStock_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
        //                this.MessageBox(cMensaje2.ToString(), this);
        //                return;
        //            }

        //            string cMensaje = "";
        //            Int64  _Id = int.Parse(xpars.ToString());
        //            if (_Id != 0)
        //            {

        //                objStockRegister = new StockRegister();
        //                objStockRegister.Id = _Id;
                       
        //                objStockRegister.Delete(out cMensaje);
        //                if (cMensaje != string.Empty)
        //                {
        //                    this.MessageBox(cMensaje.ToString(), this);
        //                }
        //                else
        //                {
        //                    this.MessageBox("Se elimino transacción con éxito", this);
                            
        //                    /*carga movimientos*/
        //                    if (this.CboDeposito.SelectedIndex != -1)
        //                    {
        //                        CargaStockRegister(Convert.ToInt64(CboDeposito.SelectedValue));
        //                    }
        //                }


        //            }
                  
        //        }
        //        catch (Exception ex)
        //        {
        //            var t = this.getUserBySesion();
        //            var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de la transacción, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "stock_register", "RemoverStock_ItemCommand", "Hubo un error al eliminar", t.loginname));
        //            this.MessageBox(cMensaje2.ToString(), this);

        //        }

        //    }
        //}


        protected void Opciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Option rowOption = (Option)e.Item.DataItem;
                Label lbl = (Label)e.Item.FindControl("lblNotes");
                if (lbl.Text == "SALDO FINAL")
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lblFecha")).ForeColor = System.Drawing.Color.Red;
                    //((Label)e.Item.FindControl("lblCid")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lblname")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lblNotes")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lblIngreso")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lblEgreso")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbltotal")).ForeColor = System.Drawing.Color.Red;
                   
                }
                if (lbl.Text == "SALDO MOVIMIENTOS")
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lblFecha")).ForeColor = System.Drawing.Color.Green;
                    //((Label)e.Item.FindControl("lblCid")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("lblname")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("lblNotes")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("lblIngreso")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("lblEgreso")).ForeColor = System.Drawing.Color.Green;
                    ((Label)e.Item.FindControl("lbltotal")).ForeColor = System.Drawing.Color.Green;

                }
            }


        }


        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            //this.IsAllowAccess();

             user = Page.Tracker();
            if (user != null)
            {
               // this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
                /*datos de la linea naviera*/
                this.CargaLineaNaviera(user.codigoempresa);
              

            }
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {

                var sp = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                string t = null;
                if (!string.IsNullOrEmpty(sp))
                {
                    t = CslHelper.getShiperName(sp);
                }
               
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                Limpiar();

                /*carga movimientos*/
                if (this.CboDeposito.SelectedIndex != -1)
                {
                    CargaStockRegister(Convert.ToInt64(CboDeposito.SelectedValue));
                }
            }
        }


        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            try
            {

                string cMensaje = "";
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                DateTime dfecha;

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());


                if (String.IsNullOrEmpty(this.LblNombre.Text) != false)
                {
                    this.MessageBox("No tiene una línea  de naviera asignada", this);
                    return;
                }

                if (this.CboDeposito.SelectedValue == string.Empty)
                {
                    this.MessageBox("Por favor debe seleccionar la bodega", this);
                    return;
                }

                if ((int)Session["id_line"] == 0)
                {
                    this.MessageBox("Error, no existe una línea naviera para el usuario" + sUser.nombres, this);
                    return;
                }

                if (this.CboOperacion.SelectedValue == string.Empty)
                {
                    this.MessageBox("Por favor debe seleccionar el tipo de operación a realiar (+/-)", this);
                    return;
                }

                if (String.IsNullOrEmpty(this.TxtFechaTransaccion.Text) != false)
                {
                    this.TxtFechaTransaccion.Focus();
                    this.MessageBox("Ingrese una Fecha para la transacción", this);
                    return;
                }

                if (!DateTime.TryParseExact(this.TxtFechaTransaccion.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out dfecha))
                {
                    this.TxtFechaTransaccion.Focus();
                    this.MessageBox("Ingrese una Fecha valida para la transacción", this);
                    return;
                }


                objStockRegister = new StockRegister();
                objStockRegister.Id_line = (int)Session["id_line"];
                objStockRegister.Id_depot = Convert.ToInt32(this.CboDeposito.SelectedValue);
                objStockRegister.Id_operation = Convert.ToInt32(this.CboOperacion.SelectedValue);
                objStockRegister.Operation_user = sUser.loginname;
                objStockRegister.Operation_notes = this.CboOperacion.SelectedItem.ToString();
                objStockRegister.Qty = Convert.ToInt32(this.TxtCantidad.Text);
                objStockRegister.Create_Date = dfecha;

                objStockRegister.Save(out cMensaje);
                if (cMensaje != string.Empty)
                {
                    this.MessageBox(cMensaje.ToString(), this);
                }  
                else
                {
                    this.MessageBox("Se agrego transacción con éxito", this);
                    this.TxtCantidad.Text = null;

                    //this.Limpiar();

                    /*carga movimientos*/
                    if (this.CboDeposito.SelectedIndex != -1)
                    {
                        CargaStockRegister(Convert.ToInt64(CboDeposito.SelectedValue));
                    }
                }

          
            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "stock_register", "BtnAgregar_Click", "Hubo un error al agregar transacción", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
              
            }

           
        }

        protected void CboDeposito_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (this.CboDeposito.SelectedIndex != -1)
                {
                    CargaStockRegister(Convert.ToInt64(CboDeposito.SelectedValue));

                  
                }
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "stock_register", "CboDeposito_SelectedIndexChanged", "Hubo un error al cargar stock", user != null ? user.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }
        }
    }
}