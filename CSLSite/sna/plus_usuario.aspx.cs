using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Text;
using System.IO;
using CSLSite.unitService;
using System.Text.RegularExpressions;
using csl_log;



namespace CSLSite
{
    public partial class nuevo_sna : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
           
        }
        private string sid = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "nuevo_sna", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
           this.IsTokenAlive();

           Page.Tracker();
           if (!IsPostBack)
           {
               this.IsCompatibleBrowser();
               Page.SslOn();
           }

            try
            {
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                // aqui cargar datos de este registro con DATASET
                if (!string.IsNullOrEmpty(sid))
                {
                    sid = sid.Trim().Replace("\0", string.Empty);
                    var enter = 0;
                    if (int.TryParse(sid, out enter))
                    {
                        populacliente(enter);
                    }
                }
                else
                {
#if !DEBUG
                    this.IsAllowAccess();
#endif
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "nuevo_sna", "Page_Init", sid, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                base.Response.Write(close);
            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // this.sinresultado.Visible = IsPostBack;
            if (!IsPostBack)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
            }
           

        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            var user = this.getUserBySesion();
            if (user.loginname == null || user.loginname.Trim().Length <= 0)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "msg-critico";
                 this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "aisv_sav", "btbuscar_Click","SAV", user.loginname));
                 return;
             }
     
             var token = HttpContext.Current.Request.Cookies["token"];
             //Validacion 3 -> Si su token existe
             if (token == null)
             {
                 var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "aisv_sav", "btbuscar_Click", token.Value, user.loginname);
                 var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                 this.PersonalResponse(personalms, "../cuenta/menu.aspx", true);
                 return;
             }


        

            //1 categoria
            if (string.IsNullOrEmpty(dpcategoria.SelectedValue) || dpcategoria.SelectedValue.Equals("0"))
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = "Por favor seleccione la categoría.";
                return;

            }
            //ruc
            if (string.IsNullOrEmpty(txtruc.Text))
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = "Por favor escriba el numero de RUC";
                return;

            }
            //descripcion
            if (string.IsNullOrEmpty(txtnombre.Text))
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = "Por favor escriba nombre / descripcion";
                return;

            }

            if (string.IsNullOrEmpty(txtmail.Text))
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = "Por favor escriba el correo electrónico";
                return;

            }

            this.bandera.Value = "1";
            //GUARDAR REGISTRO DE SNA

         

            var us = new app_start.SNA_Usuarios();
            us.cliente_ruc = txtruc.Text;
            us.cliente_estado = dpsuscrip.SelectedValue == "1" ? true : false;
            us.cliente_nombres = txtnombre.Text;
            us.cliente_categoria = dpcategoria.SelectedValue;
            us.cliente_email = txtmail.Text;
            us.cliente_telefono = txtfono.Text;

            if (!string.IsNullOrEmpty(sid))
            {
                us.cliente_modificadopor = user.loginname;
                int idd = 0;
                if (int.TryParse(sid, out idd))
                {
                    us.cliente_id = idd;
                }
            }
            else
            {
                us.cliente_creadopor = user.loginname;
            }


            if (string.IsNullOrEmpty(sid))
            {

                var ru = app_start.SNA_Usuarios.UsuarioExiste(us.cliente_ruc);
                if (ru)
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("El usuario con RUC {0}, ya existe intente editar", us.cliente_ruc);
                    return;
                }

            }

            if (!Mail())
            {
                return;
            }

            var rr = us.Guardar();
            if (rr < 0)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = "Ha ocurrido una excepcion al guardar, favor intente mas tarde";
                return;

            }

            //msg-info
            this.sinresultado.Attributes["class"] = string.Empty;
            this.sinresultado.Attributes["class"] = "msg-info";
            this.sinresultado.InnerText = string.Format("Se ha {1} el cliente con RUC {0}",txtruc.Text, string.IsNullOrEmpty(sid)?"creado":"modificado");

       
        
            //Response.Redirect("../cuenta/menu.aspx", false);
            //Context.ApplicationInstance.CompleteRequest();
        }

        private void populacliente(int clienteID) {
            var table = new Catalogos.sna_clienteDataTable();
            var ta = new CatalogosTableAdapters.sna_clienteTableAdapter();
            ta.ClearBeforeFill = true;
            ta.Fill(table, clienteID);
            if (table.Rows.Count > 0) {
                btrevisa.Visible = false;
                var fila = table.FirstOrDefault();
                txtruc.Text = fila.cliente_ruc;
                txtruc.Enabled = false;
                txtfono.Text = fila.Iscliente_telefonoNull() ? string.Empty : fila.cliente_telefono;
                txtmail.Text = fila.Iscliente_emailNull() ? string.Empty : fila.cliente_email;
                txtnombre.Text = fila.cliente_nombres;
                this.etiquetaprint.InnerText = "Editar Cliente";
                //categoria
                if (dpcategoria.Items.Count > 0)
                {
                    if (dpcategoria.Items.FindByValue(fila.cliente_categoria) != null)
                    {
                        dpcategoria.SelectedIndex = -1;
                        dpcategoria.Items.FindByValue(fila.cliente_categoria).Selected = true;
                    }
                }
                //categoria
                if (dpsuscrip.Items.Count > 0)
                {
                    var acti = fila.Iscliente_estadoNull() ? false : fila.cliente_estado;
                    var valorBusca = acti ? "1" : "0";
                    if (dpsuscrip.Items.FindByValue(valorBusca) != null)
                    {
                        dpsuscrip.SelectedIndex = -1;
                        dpsuscrip.Items.FindByValue(valorBusca).Selected = true;
                    }
                }
                if (!fila.Iscliente_modificadoNull())
                {
                    trw.Attributes["class"] = string.Empty;
                   
                    this.modificaciones.InnerText = string.Format("Realizada por {0} el {1}", fila.cliente_modificadopor, fila.Iscliente_modificadoNull() ? "?" : fila.cliente_modificado.ToString("dd/MM/yyyy HH:mm"));
                }
                
               


            }
        }


        private  bool Mail( ) {

            if (string.IsNullOrEmpty(sid))
            {
                var cfgs = new List<dbconfig>();
                cfgs = dbconfig.GetActiveConfig(null, null, null);
                //recuperarmail destno
                var maildestino = cfgs.Where(a => a.config_name.Contains("mailforces")).FirstOrDefault();
                var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUp")).FirstOrDefault();

                string destino = "contecon.it@gmail.com";
                string backup = "contecon.it@gmail.com";
                if (maildestino != null && !string.IsNullOrEmpty(maildestino.config_value))
                {
                    destino = maildestino.config_value;
                }
                if (correoBackUp != null && !string.IsNullOrEmpty(correoBackUp.config_value))
                {
                    backup = correoBackUp.config_value;
                }
                //RUC;email;whatsapp;tipodeservicio
                var asunto = string.Format("{0};{1};{2};Servicio de Trazabilidad de Carga", txtruc.Text, txtmail.Text, txtfono.Text);
                string mail = string.Empty;
                mail = string.Format("Se ha creado el usuario con ruc {0}", txtruc.Text);
                string errorMail = string.Empty;
                CLSDataSeguridad.addMail(out errorMail, destino, asunto, mail, backup, "CGSA", "", "");
                if (!string.IsNullOrEmpty(errorMail))
                {

                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = errorMail;
                    return false;
                }
            }
            return true;
        }

        protected void btrevisa_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                //var table = new Catalogos.sna_cliente_informacionDataTable();
                //var ta = new CatalogosTableAdapters.sna_cliente_informacionTableAdapter();
                var table = new Catalogos.stc_cliente_informacionDataTable();
                var ta = new CatalogosTableAdapters.stc_cliente_informacionTableAdapter();

                ta.ClearBeforeFill = true;
                ta.Fill(table, txtruc.Text);
                if (table.Rows.Count > 0)
                {
                    var fila = table.FirstOrDefault();
                    txtruc.Text = fila.cliente_ruc;
                    txtfono.Text = fila.Iscliente_telefonoNull() || string.IsNullOrEmpty(fila.cliente_telefono)  ? string.Empty : fila.cliente_telefono;
                    txtmail.Text = fila.Iscliente_emailNull() || string.IsNullOrEmpty(fila.cliente_email) ? string.Empty : fila.cliente_email;
                    txtnombre.Text = fila.cliente_nombres;
                }
            }
        }
    }
}