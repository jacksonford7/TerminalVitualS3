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
using ClsNotasCreditos;
using System.Data;
using System.Globalization;



using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Collections;

using System.Net;


namespace CSLSite
{
    public partial class frm_nota_credito : System.Web.UI.Page
    {
       
        private credit_head objNotaCredito = new credit_head();
        private credit_detail objNotaCredito_Detail = new credit_detail();
        private credit_level_approval objNotaCredito_Niveles = new credit_level_approval();

        private user objUsuarios = new user();
        usuario user2;
        string sg;

        private GetFile.Service getFile = new GetFile.Service();
       // private String xmlDocumentos;
        private DataTable dtDocumentos = new DataTable();
        private String cRutaArchivo = string.Empty;

        private string xmlDocumentos
        {
            get
            {
                return (string)Session["xmlDocumentos"];
            }
            set
            {
                Session["xmlDocumentos"] = value;
            }

        }

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


        private void Actualiza_Panel()
        {
            this.UpdateCabecera.Update();
            this.UpdateCboConcepto.Update();
            this.UpdateTxtGlosa.Update();
            this.UpdateTxtSubtotal.Update();
            this.UpdateTxtIva.Update();
            this.UpdateTxtTotal.Update();
            this.UpdateRuta.Update();
        }

        private void Limpiar()
        {

            Session["Action"] = "I";//NUEVO INGRESO 
            this.Accion.Value = "I";
            Session["path"] = string.Empty;

            /*variables de la factura*/
            this.id_factura.Value = "0";
            this.fec_factura.Value = null;
            this.id_cliente.Value = "0";     
            this.ruc_cliente.Value = null;
            this.nombre_cliente.Value = null;
            this.email_cliente.Value = null;
            this.total_factura.Value = "0";
            this.iva_factura.Value = "0";
            this.porc_iva.Value = "0";

                  
            this.TxtNumeroFactura.Text = null;
            this.TxtFechaFactura.Text = null;
            this.TxtRucFactura.Text = null;
            this.TxtNombreFactura.Text = null;
            this.TxtemailFactura.Text = null;
            this.TxtSubtotalFactura.Text = null;
            this.TxtIvaFactura.Text = null;
            this.TxtTotalFactura.Text = null;
            this.ChkTodos.Checked = false; 
            this.TxtFechaEmision.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.CboConcepto.SelectedIndex = -1;
            this.TxtGlosa.Text = null;
            this.TxtSubtotal.Text = null;
            this.TxtIva.Text = null;
            this.TxtTotal.Text = null;
            this.LblRuta.Text = null;
            //this.fsuploadarchivo.ClientID.ine="" ;
            this.TxtFechaEmision.Enabled = false;
           this.TxtNumeroFactura.Enabled = true;

            tablePagination.DataSource = null;
            tablePagination.DataBind();

          
            this.TxtNumeroFactura.Focus();

            //cabecera de la transaccion
            objNotaCredito = new credit_head();
            Session["Nota_Credito"] = objNotaCredito;

            xmlDocumentos = null;
            this.fsuploadarchivo = this.AsyncFileUpload1;

        }

        private void Carga_ListadoConceptos()
        {
            try
            {

                List<concepts> ListConceptos = concepts.ListConceptos();
                if (ListConceptos != null)
                {
                    this.CboConcepto.DataSource = ListConceptos;
                    this.CboConcepto.DataBind();
                }
                else
                {
                    this.CboConcepto.DataSource = null;
                    this.CboConcepto.DataBind();
                }



            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "CboConcepto", "Carga_ListadoConceptos", "Hubo un error al cargar conceptos", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void New_ExportFileUpload()
        {

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("Ruta");
            bool agrego = false;
     
            if (fsuploadarchivo.HasFile)
            {
                string rutafile = Server.MapPath(fsuploadarchivo.FileName);
                string finalname;
                 
                var p = CSLSite.app_start.CredencialesHelper.UploadFile_NotaCredito(Server.MapPath(fsuploadarchivo.FileName), fsuploadarchivo.PostedFile.InputStream, out finalname);
                if (!p)
                {
                    this.Alerta(finalname);
                    return;
                }
                else {
                    agrego = true;
                    dtDocumentos.Rows.Add(finalname);
                }
              
            }

            if (agrego)
            {
                dtDocumentos.AcceptChanges();
                dtDocumentos.TableName = "Documentos";
                StringWriter sw = new StringWriter();
                dtDocumentos.WriteXml(sw);
                xmlDocumentos = sw.ToString();

                this.Alerta("Archivo agregado exitosamente.");
            }
            else {
                xmlDocumentos = null;
                this.LblRuta.Text = null;
            }
            
        }


        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }



        #region "Eventos del Form"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            user2 = Page.Tracker();
            if (user2 != null)
            {

                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user2.nombres, user2.apellidos);

            }
            if (user2 != null && !string.IsNullOrEmpty(user2.nombregrupo))
            {

                var sp = string.IsNullOrEmpty(user2.codigoempresa) ? user2.ruc : user2.codigoempresa;
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
                Limpiar();
                this.Carga_ListadoConceptos();
            }
            this.TxtFechaEmision.Text = Server.HtmlEncode(this.TxtFechaEmision.Text);
          
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["path"] = this.ruta.Value;
               
                /*carga listado de usuarios*/
                this.xfinder.Visible = true;

            }

           
        }

        protected void btsubir_Click(object sender, EventArgs e)
        {
            if (!this.fsuploadarchivo.HasFile)
            {
                this.Alerta("Por favor seleccione el archivo  (.pdf, .xls, .doc, .msg)");
                return;
            }

            var nombrefile = fsuploadarchivo.PostedFile.FileName;

            /*if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".PDF" || System.IO.Path.GetExtension(nombrefile).ToUpper() != ".msg" || System.IO.Path.GetExtension(nombrefile).ToUpper() != ".")
            {
                this.Alerta("La extensión del archivo debe ser pdf");
                return;
            }*/

            if (fsuploadarchivo.PostedFile.ContentLength > 58720256)
            {
                this.Alerta("El archivo pdf excede el tamaño límite 7 megabyte");
                return;
            }

            this.LblRuta.Text= fsuploadarchivo.PostedFile.FileName.ToString();

            /*agrega archivo*/
            this.New_ExportFileUpload();

            this.Calcula_Valores();

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {   

               /*valida existencia de nota de credito*/
                var cMensaje = objNotaCredito.Validate_credit_head(this.TxtNumeroFactura.Text);
                if (cMensaje != string.Empty)
                {
                    this.Alerta(cMensaje);
                    this.Actualiza_Panel();
                    return;
                }
                /*consulta cabecera de factura*/
                List < credit_head> Lista = credit_head.Get_Cabecera_Factura(this.TxtNumeroFactura.Text);
                if (Lista != null)
                {
                    var xList = Lista.FirstOrDefault();
                    if (xList != null)
                    {
                        this.TxtFechaFactura.Text = xList.fec_factura.HasValue ? xList.fec_factura.Value.ToString("dd/MM/yyyy") : "";
                        this.TxtRucFactura.Text = xList.ruc_cliente !=null ? xList.ruc_cliente : "";
                        this.TxtNombreFactura.Text = xList.nombre_cliente != null ? xList.nombre_cliente : "";
                        this.TxtemailFactura.Text = xList.email_cliente != null ? xList.email_cliente : "";
                        this.TxtSubtotalFactura.Text = xList.total_factura != 0 ? xList.total_factura.ToString() : "0.00";
                        this.TxtIvaFactura.Text = xList.iva_factura != 0 ? xList.iva_factura.ToString() : "0.00";
                        this.TxtTotalFactura.Text = (xList.total_factura + xList.iva_factura) != 0 ? (xList.total_factura + xList.iva_factura).ToString() : "0.00";

                        this.id_factura.Value = xList.id_factura.ToString();
                        this.fec_factura.Value = xList.fec_factura.HasValue ? xList.fec_factura.Value.ToString("dd/MM/yyyy") : "";
                        this.id_cliente.Value = xList.id_cliente.ToString();
                        this.ruc_cliente.Value = xList.ruc_cliente ;
                        this.nombre_cliente.Value = xList.nombre_cliente;
                        this.email_cliente.Value = xList.email_cliente;
                        this.total_factura.Value = xList.total_factura.ToString();
                        this.iva_factura.Value = xList.iva_factura.ToString();
                        this.porc_iva.Value = xList.porc_iva.ToString();

                        this.TxtFechaEmision.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        /*consulta detalle de factura*/

                        List<credit_detail> ListDetalle = credit_detail.Get_Detalle_Factura(xList.id_factura);
                        if (ListDetalle != null)
                        {
                            tablePagination.DataSource = ListDetalle;
                            tablePagination.DataBind();

                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            this.Alerta("No existe detalle de factura con el número ingresado: " + this.TxtNumeroFactura.Text);
                            this.Actualiza_Panel();
                            return;
                        }

                        usuario sUser = null;
                        sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime dFechaEmision = new DateTime();
                        DateTime dFechaFactura = new DateTime();

                        if (!DateTime.TryParseExact(this.TxtFechaEmision.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out dFechaEmision))
                        {
                            this.Alerta(string.Format("! El formato de fecha de N/C debe ser dia/Mes/Anio {0}", this.TxtFechaEmision.Text));
                            TxtFechaEmision.Focus();
                            TxtFechaEmision.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            return;
                        }

                        if (!DateTime.TryParseExact(this.fec_factura.Value, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dFechaFactura))
                        {
                            this.Alerta(string.Format("! El formato de fecha de factura debe ser dia/Mes/Anio {0}", this.fec_factura.Value));
                            return;
                        }

                        objNotaCredito = Session["Nota_Credito"] as credit_head;

                        objNotaCredito.nc_id = 0;
                        objNotaCredito.nc_date = dFechaEmision;
                        objNotaCredito.nc_concept = this.TxtGlosa.Text.Trim();
                        objNotaCredito.nc_number = string.Empty;
                        objNotaCredito.nc_authorization = string.Empty;
                        objNotaCredito.id_concept = int.Parse(this.CboConcepto.SelectedValue);
                        objNotaCredito.nc_state = true;
                        objNotaCredito.nc_subtotal = 0;
                        objNotaCredito.nc_iva = 0;
                        objNotaCredito.nc_total = 0;
                        objNotaCredito.nc_valor_nivel = 1;
                        objNotaCredito.id_level = 1;
                        objNotaCredito.id_factura = Int64.Parse(this.id_factura.Value);
                        objNotaCredito.num_factura = this.TxtNumeroFactura.Text;
                        objNotaCredito.fec_factura = dFechaFactura;
                        objNotaCredito.id_cliente = Int64.Parse(this.id_cliente.Value);
                        objNotaCredito.ruc_cliente = this.ruc_cliente.Value;
                        objNotaCredito.nombre_cliente = this.nombre_cliente.Value;
                        objNotaCredito.email_cliente = this.email_cliente.Value;
                        objNotaCredito.total_factura = decimal.Parse(this.total_factura.Value);
                        objNotaCredito.iva_factura = decimal.Parse(this.iva_factura.Value); ;
                        objNotaCredito.porc_iva = decimal.Parse(this.porc_iva.Value); ;

                        objNotaCredito.Create_user = sUser.loginname;
                        
                        objNotaCredito.Action = Session["Action"].ToString();
                        objNotaCredito.Mod_user = sUser.loginname;

                    }
                    else
                    {
                        this.TxtFechaFactura.Text = null;
                        this.TxtRucFactura.Text = null;
                        this.TxtNombreFactura.Text = null;
                        this.TxtemailFactura.Text = null;
                        this.TxtSubtotalFactura.Text = null;
                        this.TxtIvaFactura.Text = null;
                        this.TxtTotalFactura.Text = null;

                        this.id_factura.Value = null;
                        this.fec_factura.Value = null;
                        this.id_cliente.Value = null;
                        this.ruc_cliente.Value = null;
                        this.nombre_cliente.Value = null;
                        this.email_cliente.Value = null;
                        this.total_factura.Value = null;
                        this.iva_factura.Value = null;
                        this.porc_iva.Value = null;

                        this.Alerta("No existe información con el  número de factura ingresado: " + this.TxtNumeroFactura.Text);
                        this.Actualiza_Panel();

                        return;
                    }
                }
                else {

                    this.TxtFechaFactura.Text = null;
                    this.TxtRucFactura.Text = null;
                    this.TxtNombreFactura.Text = null;
                    this.TxtemailFactura.Text = null;
                    this.TxtSubtotalFactura.Text = null;
                    this.TxtIvaFactura.Text = null;
                    this.TxtTotalFactura.Text = null;

                    this.id_factura.Value = null;
                    this.fec_factura.Value = null;
                    this.id_cliente.Value = null;
                    this.ruc_cliente.Value = null;
                    this.nombre_cliente.Value = null;
                    this.email_cliente.Value = null;
                    this.total_factura.Value = null;
                    this.iva_factura.Value = null;
                    this.porc_iva.Value = null;

                  
                    this.Alerta("No existe información con el número de factura ingresado: " + this.TxtNumeroFactura.Text);
                    this.Actualiza_Panel();

                    return;
                }

                this.Actualiza_Panel();

            }
            catch (Exception exc)
            {
                this.Alerta(exc.Message);    
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            try
            {
 
                this.Calcula_Valores();

                Decimal nCantidad = 0;

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                objNotaCredito = Session["Nota_Credito"] as credit_head;
                if (objNotaCredito == null)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde, objeto 'Nota_Credito' sin valores ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo Session[Nota_Credito]"), "frm_NOTA_CREDITO", "BtnGrabar_Click", string.Format("Hubo un error al grabar:{0}", this.TxtNumeroFactura.Text), sUser != null ? sUser.loginname : "Nologin"));
                    this.Alerta(sg);
                    return;
                }

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime dFechaEmision = new DateTime();
                

                if (!DateTime.TryParseExact(this.TxtFechaEmision.Text.Trim(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out dFechaEmision))
                {
                    this.Alerta(string.Format("! El formato de fecha de N/C debe ser dia/Mes/Anio {0}", this.TxtFechaEmision.Text));
                    TxtFechaEmision.Focus();
                    TxtFechaEmision.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    return;
                }

               

                objNotaCredito.nc_id = 1;
                objNotaCredito.nc_date = dFechaEmision;
                objNotaCredito.nc_concept = this.TxtGlosa.Text.Trim();
                objNotaCredito.nc_number = string.Empty;
                objNotaCredito.nc_authorization = string.Empty;
                objNotaCredito.id_concept = int.Parse(this.CboConcepto.SelectedValue);
                objNotaCredito.nc_state = true;
                objNotaCredito.nc_subtotal = decimal.Parse(this.TxtSubtotal.Text);
                objNotaCredito.nc_iva = decimal.Parse(this.TxtIva.Text);
                objNotaCredito.nc_total = decimal.Parse(this.TxtTotal.Text);
                objNotaCredito.nc_valor_nivel = 1;
                objNotaCredito.id_level = 1;
             

                objNotaCredito.Create_user = sUser.loginname;

                objNotaCredito.Action = Session["Action"].ToString();
                objNotaCredito.Mod_user = sUser.loginname;

                objNotaCredito.Detalle.Clear();
                
                int nSecuencia = 1;
                foreach (RepeaterItem item in tablePagination.Items)
                {

                    TextBox TxtCantidad = item.FindControl("TxtNewCantidad") as TextBox;
                    Label LblPrecio = item.FindControl("lbl_precio") as Label;
                    Label LblPorcentaje = item.FindControl("lbl_porc_iva") as Label;
                    Label LblSubtotal = item.FindControl("lbl_nc_subtotal") as Label;
                    Label LblIva = item.FindControl("lbl_nc_iva") as Label;
                    Label LblCantAnterior = item.FindControl("lbl_cantidad") as Label;

                    Label LblCodItem = item.FindControl("lbl_codigo_item") as Label;
                    Label LblUnidad_Bl = item.FindControl("lbl_unidad_bl") as Label;
                    Label LblCodServicio = item.FindControl("lbl_codigo_servicio") as Label;
                    Label LblDescServicio = item.FindControl("lbl_desc_servicio") as Label;
                    Label LblSubtotalAnt = item.FindControl("lbl_subtotal") as Label;
                    Label LblNumCarga = item.FindControl("lbl_numero_carga") as Label;
                    Label LblValorIva = item.FindControl("lbl_iva") as Label;

                    if (TxtCantidad.Text != string.Empty)
                    {
                        var x = TxtCantidad.Text;
                        if (!Decimal.TryParse(x, out nCantidad)) { nCantidad = 0; }

                        if (nCantidad != 0)
                        {
                            objNotaCredito_Detail = new credit_detail();
                            objNotaCredito_Detail.nc_id = 0;
                            objNotaCredito_Detail.sequence = nSecuencia;
                            objNotaCredito_Detail.codigo_item = Int64.Parse(LblCodItem.Text);
                            objNotaCredito_Detail.codigo_servicio = LblCodServicio.Text;
                            objNotaCredito_Detail.unidad_bl = LblUnidad_Bl.Text;
                            objNotaCredito_Detail.desc_servicio = LblDescServicio.Text;
                            objNotaCredito_Detail.id_factura = objNotaCredito.id_factura;
                            objNotaCredito_Detail.cantidad = decimal.Parse(LblCantAnterior.Text);
                            objNotaCredito_Detail.precio = decimal.Parse(LblPrecio.Text);
                            objNotaCredito_Detail.subtotal = decimal.Parse(LblSubtotalAnt.Text);
                            objNotaCredito_Detail.iva = decimal.Parse(LblValorIva.Text);
                            objNotaCredito_Detail.iva_porcentaje = decimal.Parse(LblPorcentaje.Text);
                            objNotaCredito_Detail.numero_carga = LblNumCarga.Text;
                            objNotaCredito_Detail.nc_cantidad = nCantidad;//decimal.Parse(TxtCantidad.Text);
                            objNotaCredito_Detail.nc_precio = decimal.Parse(LblPrecio.Text);
                            objNotaCredito_Detail.nc_subtotal = decimal.Parse(LblSubtotal.Text);
                            objNotaCredito_Detail.nc_iva = decimal.Parse(LblIva.Text);
                            objNotaCredito_Detail.Action = Session["Action"].ToString();
                            objNotaCredito_Detail.Create_user = sUser.loginname;
                            objNotaCredito_Detail.Mod_user = sUser.loginname;

                            objNotaCredito.Detalle.Add(objNotaCredito_Detail);

                            nSecuencia = nSecuencia + 1;
                        }
                        
                    }
                      
                }

                objNotaCredito.DetalleNivel.Clear();
                objNotaCredito_Niveles = new credit_level_approval();
                objNotaCredito_Niveles.nc_id = 0;
                objNotaCredito_Niveles.id_level = 1;
                objNotaCredito_Niveles.id_group = 1;
                objNotaCredito_Niveles.IdUsuario = 1;
                objNotaCredito_Niveles.id_concept = int.Parse(this.CboConcepto.SelectedValue);
                objNotaCredito_Niveles.aprobado = true;
                objNotaCredito_Niveles.level = 1;
                objNotaCredito_Niveles.Usuario = sUser.loginname;
                objNotaCredito_Niveles.nc_total = 1;
                objNotaCredito_Niveles.Action = Session["Action"].ToString();
                objNotaCredito.DetalleNivel.Add(objNotaCredito_Niveles);

                if (xmlDocumentos != null)
                {
                    objNotaCredito.xmlDocumentos = xmlDocumentos;
                }
                else {
                    objNotaCredito.xmlDocumentos = string.Empty;
                }
                
                /*fin adjunto archivo pdf*/

                var nIdRegistro = objNotaCredito.SaveTransaction(out sg);
                if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                {
                    this.MessageBox(sg, this);
                    return;
                }

                var s = string.Format("Éxito al registrar nueva nota de crédito, número interno # {0} {1} ",  nIdRegistro.Value.ToString("D10"), sg);
                this.MessageBox(s, this);

                string cId = securetext(nIdRegistro.Value.ToString());
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../nota_credito/nota_credito_preview.aspx?nc_id=" + cId + "');", true);

                //ClientScript.RegisterStartupScript(typeof(Page), "closePage", s, true);

                this.Limpiar();
              
              
            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "frm__nota_credito", "BtnGrabar_Click", "Hubo un error al grabar nota de credito", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;

            }


        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();

            this.Actualiza_Panel();

          

        }

        //private void DescargarDocumento(String ruta)
        //{
        //    try
        //    {
        //        String prueba;
        //        HttpContext.Current.Response.Clear();
        //        HttpContext.Current.Response.ContentType = "pdf";
        //        prueba = Path.GetFileName(ruta).ToString();
        //        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + prueba);
        //        HttpContext.Current.Response.TransmitFile(ruta);
        //        HttpContext.Current.Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var number = log_csl.save_log<Exception>(ex, "frm_nota_credito", "DescargarDocumento()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
        //        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
        //    }
        //}



        protected void TxtNewCantidad_TextChanged(object sender, EventArgs e)
        {
           
            Calcula_Valores();

        }

        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                decimal nSubtotalAC = 0, nIvaAC = 0;
                decimal nSubtotalNC = 0;
                decimal nIvaNC = 0;
                decimal nCantidad = 0, nCantidadAnt = 0;
                decimal nPrecio = 0;
                decimal nPorcentaje = 12;
                decimal nTotalNotaCredito = 0;
                decimal nBaseIva = 0;
                decimal nBaseExento = 0;
                decimal nValorIVa = 0;
                decimal nTotalNotaCredito2 = 0;

                if (this.ChkTodos.Checked)
                {
                   
                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        TextBox TxtCantidad = item.FindControl("TxtNewCantidad") as TextBox;
                        Label LblPrecio = item.FindControl("lbl_precio") as Label;
                        Label LblPorcentaje = item.FindControl("lbl_porc_iva") as Label;
                        Label LblSubtotal = item.FindControl("lbl_nc_subtotal") as Label;
                        Label LblIva = item.FindControl("lbl_nc_iva") as Label;
                        Label LblCantAnterior = item.FindControl("lbl_cantidad") as Label;

                        Label LblCodServicio = item.FindControl("lbl_codigo_servicio") as Label;
                        Label LblDescServicio = item.FindControl("lbl_desc_servicio") as Label;
                        Label LblSubtotalAnt = item.FindControl("lbl_subtotal") as Label;
                        Label LblNumCarga = item.FindControl("lbl_numero_carga") as Label;
                        Label LblValorIva = item.FindControl("lbl_iva") as Label;

                        if (LblCantAnterior.Text != string.Empty)
                        {
                            var x = LblCantAnterior.Text;
                            if (Decimal.TryParse(x, out nCantidadAnt))
                            {
                                //nCantidadAnt = Convert.ToDecimal(LblCantAnterior.Text);/*cantidad anterior*/

                                if (nCantidadAnt > 0)
                                {
                                    TxtCantidad.Text = LblCantAnterior.Text;/*cantidad nueva*/
                                    nCantidad = Convert.ToDecimal(TxtCantidad.Text);/*cantidad nueva*/
                                    nPrecio = Convert.ToDecimal(LblPrecio.Text);/*precio actual*/
                                    nPorcentaje = Convert.ToDecimal(LblPorcentaje.Text);/*porcentaje iva actual*/



                                    nSubtotalNC = Math.Round((nCantidad * nPrecio), 2);/*nuevo subtotal*/
                                    nIvaNC = Math.Round(((nSubtotalNC * nPorcentaje) / 100), 2);/*nuevo valor del iva*/

                                    nSubtotalAC = nSubtotalAC + nSubtotalNC;/*nuevo subtotal*/
                                    nIvaAC = nIvaAC + nIvaNC;
                                    nTotalNotaCredito = nSubtotalAC + nIvaAC;

                                    LblSubtotal.Text = nSubtotalNC.ToString();
                                    LblIva.Text = nIvaNC.ToString();

                                    if (nPorcentaje != 0)
                                    {
                                        nBaseIva = nBaseIva + nSubtotalNC;
                                    }
                                    else
                                    {
                                        nBaseExento = nBaseExento + nSubtotalNC;
                                    }

                                    LblPrecio.ForeColor = System.Drawing.Color.Blue;
                                    LblSubtotal.ForeColor = System.Drawing.Color.Blue;
                                    LblIva.ForeColor = System.Drawing.Color.Blue;
                                    LblCantAnterior.ForeColor = System.Drawing.Color.Blue;
                                    TxtCantidad.ForeColor = System.Drawing.Color.Blue;
                                    LblCodServicio.ForeColor = System.Drawing.Color.Blue;
                                    LblDescServicio.ForeColor = System.Drawing.Color.Blue;
                                    LblSubtotalAnt.ForeColor = System.Drawing.Color.Blue;
                                    LblNumCarga.ForeColor = System.Drawing.Color.Blue;
                                    LblValorIva.ForeColor = System.Drawing.Color.Blue;
                                }
                            }
                            
                            
                        }

                    }
      
                }
                else
                {
                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        TextBox TxtCantidad = item.FindControl("TxtNewCantidad") as TextBox;
                        Label LblPrecio = item.FindControl("lbl_precio") as Label;
                        Label LblPorcentaje = item.FindControl("lbl_porc_iva") as Label;
                        Label LblSubtotal = item.FindControl("lbl_nc_subtotal") as Label;
                        Label LblIva = item.FindControl("lbl_nc_iva") as Label;
                        Label LblCantAnterior = item.FindControl("lbl_cantidad") as Label;

                        Label LblCodServicio = item.FindControl("lbl_codigo_servicio") as Label;
                        Label LblDescServicio = item.FindControl("lbl_desc_servicio") as Label;
                        Label LblSubtotalAnt = item.FindControl("lbl_subtotal") as Label;
                        Label LblNumCarga = item.FindControl("lbl_numero_carga") as Label;
                        Label LblValorIva = item.FindControl("lbl_iva") as Label;

                        if (LblCantAnterior.Text != string.Empty)
                        {
                            TxtCantidad.Text = null;/*cantidad nueva*/
                            LblIva.Text = "0.0000";
                            LblSubtotal.Text = "0.0000";

                            nCantidad =0;/*cantidad nueva*/
                            nPrecio = Convert.ToDecimal(LblPrecio.Text);/*precio actual*/
                            nPorcentaje = Convert.ToDecimal(LblPorcentaje.Text);/*porcentaje iva actual*/
                            nCantidadAnt = Convert.ToDecimal(LblCantAnterior.Text);/*cantidad anterior*/

                            nBaseIva = 0;
                            nBaseExento = 0;

                            LblPrecio.ForeColor = System.Drawing.Color.Black;
                            LblSubtotal.ForeColor = System.Drawing.Color.Black;
                            LblIva.ForeColor = System.Drawing.Color.Black;
                            LblCantAnterior.ForeColor = System.Drawing.Color.Black;
                            TxtCantidad.ForeColor = System.Drawing.Color.Black;
                            LblCodServicio.ForeColor = System.Drawing.Color.Black;
                            LblDescServicio.ForeColor = System.Drawing.Color.Black;
                            LblSubtotalAnt.ForeColor = System.Drawing.Color.Black;
                            LblNumCarga.ForeColor = System.Drawing.Color.Black;
                            LblValorIva.ForeColor = System.Drawing.Color.Black;
                        }

                    }
                }

                nValorIVa = Math.Round(((nBaseIva * nPorcentaje) / 100), 2);
                nTotalNotaCredito2 = nBaseIva + nValorIVa + nBaseExento;
                this.TxtSubtotal.Text = String.Format("{0:0.##}", nBaseIva + nBaseExento);
                this.TxtIva.Text = String.Format("{0:0.##}", nValorIVa);
                this.TxtTotal.Text = String.Format("{0:0.##}", nTotalNotaCredito2);
                this.Actualiza_Panel();



            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "frm_nota_credito", "ChkTodos_CheckedChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void  Calcula_Valores()
        {

            try
            {

                decimal nSubtotalAC = 0, nIvaAC = 0;
                decimal nSubtotalNC = 0;
                decimal nIvaNC = 0;
                decimal nCantidad = 0, nCantidadAnt = 0;
                decimal nPrecio = 0;
                decimal nPorcentaje = 12;
                decimal nTotalNotaCredito = 0;
                decimal nBaseIva = 0;
                decimal nBaseExento = 0;
                decimal nValorIVa = 0;
                decimal nTotalNotaCredito2 = 0;


                foreach (RepeaterItem item in tablePagination.Items)
                {
               
                    TextBox TxtCantidad = item.FindControl("TxtNewCantidad") as TextBox;
                    Label LblPrecio = item.FindControl("lbl_precio") as Label;
                    Label LblPorcentaje = item.FindControl("lbl_porc_iva") as Label;
                    Label LblSubtotal = item.FindControl("lbl_nc_subtotal") as Label;
                    Label LblIva = item.FindControl("lbl_nc_iva") as Label;
                    Label LblCantAnterior = item.FindControl("lbl_cantidad") as Label;
                    Label LblCodServicio = item.FindControl("lbl_codigo_servicio") as Label;
                    Label LblDescServicio = item.FindControl("lbl_desc_servicio") as Label;
                    Label LblSubtotalAnt = item.FindControl("lbl_subtotal") as Label;
                    Label LblNumCarga = item.FindControl("lbl_numero_carga") as Label;
                    Label LblValorIva = item.FindControl("lbl_iva") as Label;

                    if (TxtCantidad.Text != string.Empty)
                    {

                        var x = TxtCantidad.Text;
                        if (!Decimal.TryParse(x, out nCantidad)) { nCantidad = 0; TxtCantidad.Text = null; }

                        var y = LblCantAnterior.Text;
                        if (!Decimal.TryParse(y, out nCantidadAnt)) { nCantidadAnt = 0; }

                        ///nCantidad = Convert.ToDecimal(TxtCantidad.Text);/*cantidad nueva*/
                        nPrecio = Convert.ToDecimal(LblPrecio.Text);/*precio actual*/
                        nPorcentaje = Convert.ToDecimal(LblPorcentaje.Text);/*porcentaje iva actual*/
                        //nCantidadAnt = Convert.ToDecimal(LblCantAnterior.Text);/*cantidad anterior*/

                        if (nCantidad > nCantidadAnt)
                        {

                            this.Alerta(string.Format("La cantidad actual {0}, no puede ser mayor que la original {1}", nCantidad, nCantidadAnt));
                            TxtCantidad.Text = String.Format("{0:0.##}", nCantidadAnt);
                            nCantidad = nCantidadAnt;
                        }


                        nSubtotalNC = Math.Round((nCantidad * nPrecio), 2);/*nuevo subtotal*/
                        nIvaNC = Math.Round(((nSubtotalNC * nPorcentaje) / 100), 2);/*nuevo valor del iva*/

                        nSubtotalAC = nSubtotalAC + nSubtotalNC;/*nuevo subtotal*/
                        nIvaAC = nIvaAC + nIvaNC;
                        nTotalNotaCredito = nSubtotalAC + nIvaAC;

                        LblSubtotal.Text = nSubtotalNC.ToString();
                        LblIva.Text = nIvaNC.ToString();

                        if (nPorcentaje != 0)
                        {
                            nBaseIva = nBaseIva + nSubtotalNC;
                        }
                        else
                        {
                            nBaseExento = nBaseExento + nSubtotalNC;
                        }

                        LblPrecio.ForeColor = System.Drawing.Color.Blue;
                        LblSubtotal.ForeColor = System.Drawing.Color.Blue;
                        LblIva.ForeColor = System.Drawing.Color.Blue;
                        LblCantAnterior.ForeColor = System.Drawing.Color.Blue;
                        TxtCantidad.ForeColor = System.Drawing.Color.Blue;
                        LblCodServicio.ForeColor = System.Drawing.Color.Blue;
                        LblDescServicio.ForeColor = System.Drawing.Color.Blue;
                        LblSubtotalAnt.ForeColor = System.Drawing.Color.Blue;
                        LblNumCarga.ForeColor = System.Drawing.Color.Blue;
                        LblValorIva.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {
                        LblPrecio.ForeColor = System.Drawing.Color.Black;
                        LblSubtotal.ForeColor = System.Drawing.Color.Black;
                        LblIva.ForeColor = System.Drawing.Color.Black;
                        LblCantAnterior.ForeColor = System.Drawing.Color.Black;
                        TxtCantidad.ForeColor = System.Drawing.Color.Black;
                        LblCodServicio.ForeColor = System.Drawing.Color.Black;
                        LblDescServicio.ForeColor = System.Drawing.Color.Black;
                        LblSubtotalAnt.ForeColor = System.Drawing.Color.Black;
                        LblNumCarga.ForeColor = System.Drawing.Color.Black;
                        LblValorIva.ForeColor = System.Drawing.Color.Black;
                    }

                }

                nValorIVa = Math.Round(((nBaseIva * nPorcentaje) / 100), 2);
                nTotalNotaCredito2 = nBaseIva + nValorIVa + nBaseExento;
                this.TxtSubtotal.Text = String.Format("{0:0.##}", nBaseIva + nBaseExento);
                this.TxtIva.Text = String.Format("{0:0.##}", nValorIVa);
                this.TxtTotal.Text = String.Format("{0:0.##}", nTotalNotaCredito2);
                this.Actualiza_Panel();

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "frm_nota_credito", "Calcula_Valores()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }

        }





        #endregion

        //protected void BtnBajar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

                


        //        //// TheDownload("@c:/tmp/MANUALSISTEMAWEBOPC_1010201912035228.pdf");


        //        //// WebClient webClient = new WebClient();
        //        //// // webClient.DownloadFile("http://cgdes19:5052/AISV33/documentos/2019/10/MANUALSISTEMAWEBOPC_1010201912035228.pdf", "archivo.pdf");

        //        //// webClient.DownloadFileAsync(new Uri("http://cgdes19:5052/AISV33/documentos/2019/10/MANUALSISTEMAWEBOPC_1010201912035228.pdf"), "test.jpg");

        //        ////// webClient.DownloadString("http://cgdes19:5052/AISV33/documentos/2019/10/MANUALSISTEMAWEBOPC_1010201912035228.pdf");
        //        ////// webClient.OpenRead("http://cgdes19:5052/AISV33/documentos/2019/10/MANUALSISTEMAWEBOPC_1010201912035228.pdf");

        //        ////WebClient Client = new WebClient();
        //        ////Client.DownloadFile("http://cgdes19:5052/AISV33/documentos/2019/10/MANUALSISTEMAWEBOPC_1010201912035228.pdf", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "File.pdf");

        //        //WebClient User = new WebClient();
        //        //Byte[] FileBuffer = User.DownloadData("http://cgdes19:5052/AISV33/documentos/2019/10/MANUALSISTEMAWEBOPC_1010201912035228.pdf");
   
        //        //if (FileBuffer != null)
        //        //{
        //        //    //Response.ContentType = "application/pdf";
        //        //    //Response.AddHeader("content-length", FileBuffer.Length.ToString());
        //        //    //Response.BinaryWrite(FileBuffer);
        //        //    HttpContext.Current.Response.Clear();
        //        //    //HttpContext.Current.Response.ContentType = "application/pdf";
        //        //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=MANUALSISTEMAWEBOPC_1010201912035228.pdf");

        //        //    HttpContext.Current.Response.AddHeader("content-length", FileBuffer.Length.ToString());
        //        //    HttpContext.Current.Response.ContentType = "application/octet-stream";
        //        //    HttpContext.Current.Response.BinaryWrite(FileBuffer);
        //        //    // HttpContext.Current.Response.End();
        //        //}

        //        TheDownload("~/documentos/2019/10/01.zip");

        //    }
        //    catch (Exception ex)
        //    {
        //        var number = log_csl.save_log<Exception>(ex, "frm_nota_credito", "BtnBajar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
        //        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
        //    }
        //}

       

        //public void TheDownload(string path) {
        //    try
        //    {
        //        System.IO.FileInfo toDownload = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(path));
        //        HttpContext.Current.Response.Clear();
        //        HttpContext.Current.Response.ClearHeaders();
        //        HttpContext.Current.Response.ClearContent();
        //        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
        //        HttpContext.Current.Response.AddHeader("Content-Length", toDownload.Length.ToString());
        //        //HttpContext.Current.Response.ContentType = "application/octet-stream";
        //       // HttpContext.Current.Response.WriteFile(path);
        //        HttpContext.Current.Response.Write(path);
        //        // HttpContext.Current.Response.End();
        //        HttpContext.Current.Response.TransmitFile(ruta);
        //        this.Alerta(toDownload.Name.ToString());




        //    }
        //    catch (Exception ex)
        //    {
        //        var number = log_csl.save_log<Exception>(ex, "frm_nota_credito", "TheDownload()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
        //        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
        //    }
        //}

    }
}