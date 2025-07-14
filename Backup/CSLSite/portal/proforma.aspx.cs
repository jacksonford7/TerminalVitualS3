using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using System.Web.Script.Services;
using CSLSite.app_start;
using csl_log;

namespace CSLSite
{
    public partial class proforma : System.Web.UI.Page
    {
        //AntiXRCFG.
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "proforma", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "proforma", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../csl/login", true);
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
               sinresultado.Visible = false;
                
            }
            this.bkey.Value =Server.HtmlEncode(this.bkey.Value);
            this.bkqty.Value = Server.HtmlEncode( this.bkqty.Value);
            this.nbqty.InnerText = !string.IsNullOrEmpty(this.bkqty.Value)? Server.HtmlEncode(this.bkqty.Value +" U"):"...";
            this.nbrboo.Value = !string.IsNullOrEmpty(this.nbrboo.Value)?Server.HtmlEncode( this.nbrboo.Value):"...";
            this.numbook.InnerText = Server.HtmlEncode(this.nbrboo.Value);
            this.bkrefe.Value = Server.HtmlEncode(this.bkrefe.Value);
            this.bknna.Value = Server.HtmlEncode(this.bknna.Value);
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            var usn = new usuario();

                        usn = this.getUserBySesion();
                        if (usn == null)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "proforma", "btbuscar_Click", "No usuario", Request.UserHostAddress);
                            this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                            return;
                        }
            

            var cntrqty = 0; //caja cantidad
            Int64 bokingkey; // gkey solo una referencia
            Int32 bokinqty=0; //cantidad del booking
            var bonbr = this.nbrboo.Value;
            Int32 N4BookingQty = 0;
            Int32 ProTotal = 0;

            //nuevos campos------------------------>
            DateTime fechacliente = DateTime.Now;
            DateTime fechazarpe = DateTime.Now ;
            int isrefer = 0;
           
            //-------------------------------------->

            CultureInfo enUS = new CultureInfo("es-US");
 
            var en_debug = true;
            if (System.Configuration.ConfigurationManager.AppSettings["activar_proforma"] != null &&
                System.Configuration.ConfigurationManager.AppSettings["activar_proforma"].Contains("1"))
            {
                en_debug = false;
            }

  
            //si esta en debug
            if (!en_debug)
            {

                if (!DateTime.TryParseExact(this.bketd.Value, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechazarpe))
                {
                    this.sinresultado.InnerText = "La fecha de zarpe tiene un formato incorrecto.";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }
              
                
                if (!DateTime.TryParseExact(this.txdate.Text.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacliente))
                {
                    this.sinresultado.InnerText = "La fecha estimada de retiro no tiene un formato válido";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }
                if (!int.TryParse(this.bkrefer.Value, out isrefer))
                {
                    this.sinresultado.InnerText = "No se ha podido determinar si las unidades utilizan conexión";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }
            }


  

            //refer use horas
            //
            double total_horas = 0;
            double total_dias = 0;

            if (!en_debug)
            {
               
                total_horas = (fechazarpe.Date - fechacliente.Date).TotalHours + 24;
                total_dias = (fechazarpe.Date - fechacliente.Date).TotalDays + 1;
            }
            
            //--------------------------------------------->


            //comprueba que la cantidad sea un número
            if(!int.TryParse(txtqty.Text.Trim(),out cntrqty))
            {
                this.sinresultado.InnerText = "La cantidad de la proforma esta incorrecta ";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //Verifica el numero de booking
            if (string.IsNullOrEmpty(nbrboo.Value))
            {
                this.sinresultado.InnerText = "Por favor seleccione el booking";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //Verfificar el key del booking
            if (!Int64.TryParse(bkey.Value, out bokingkey) || bokingkey <=0)
            {
                this.sinresultado.InnerText = "Por favor comuniquese con nosotros, al parecer el Booking presenta problemas.";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //se obtien la cantidad actual del booking en N4.
            N4BookingQty = ProformaHelper.BookingQty(bokingkey);
            //la cantidad de la proforma actual No puede sobrepasar esta cantidad
            ProTotal = ProformaHelper.Totalproformas(bokingkey);
            //resto la cantidad que tiene N4 - las proformas activas
            var diferencia = N4BookingQty - ProTotal;
            //verficar la cantidad
            if (!Int32.TryParse(bkqty.Value, out bokinqty) || bokinqty <= 0)
            {
                this.sinresultado.InnerHtml = string.Format("El booking {0}, no tiene reservas disponibles.", nbrboo.Value);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }

            //No hay cupo, porque el total del booking ha sido superado por proformas
            if (diferencia <=0)
            {
                this.sinresultado.InnerHtml = string.Format("El booking {0}, no tiene reservas disponibles, para poder generar esta proforma:<br/>* Anule proformas anteriores.<br/>* Amplíe la capactidad del Booking", nbrboo.Value);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta( string.Format("El booking {0}, no tiene reservas disponibles\\nReservas Booking:[{1}]\\nEn proformas:[{2}]", nbrboo.Value,bokinqty,ProTotal));
                return;
            
            }
            //diferencia es lo que tiene restante
            if (cntrqty > diferencia)
            {
                this.sinresultado.InnerHtml = string.Format("El booking {0}, no tiene reservas disponibles, para poder cubrir la cantidad solicitada [{1}].<br/>  Para generar esta proforma intente lo siguiente:<br/>* Anule proformas anteriores.<br/>* Amplíe la capactidad del Booking<br/>* Reduzca la cantidad en esta proforma", nbrboo.Value,cntrqty);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(string.Format("El booking {0}, no tiene reservas disponibles:\\nReservas Booking:[{1}]\\nEn proformas:[{2}]\\nDiponible:[{3}]\\nCantidad solicitada:[{4}]  ", nbrboo.Value, bokinqty, ProTotal,diferencia, cntrqty));
                return;
            }
            //este verifica la cantidad que paso contra la que dice N4 y comprueba.
            if (!ProformaHelper.confirmarBoking(bokingkey, bokinqty))
            {
                this.sinresultado.InnerText = "Los datos del booking han sido modificados por la agencia por favor vuelva a seleccionarlo";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta("Los datos del booking han sido modificados por la agencia por favor vuelva a seleccionarlo");
                return;
            }
            var table = new Catalogos.ExportacionServicesDataTable();
            var ta = new CatalogosTableAdapters.ExportacionServicesTableAdapter();
             ta.Fill(table, usn.loginname);


            if (table.Rows.Count <= 0)
            {
                this.sinresultado.InnerText = "Hubo un problema al intentar generar la proforma por favor salga de la aplicación en intente en unos minutos";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                return;
            }

            //no es refer eliminar servicios de refer.

            if (!en_debug)
            {
                if (isrefer == 0)
                {
                    var ser_refer = table.Where(i => i.tipo.Contains("R")).ToList();
                    foreach (var d in ser_refer)
                    {
                        table.RemoveExportacionServicesRow(d);
                    }
                }
            }

            foreach (var fila in table)
            {

                    if (fila.tipo.Contains("R"))
                    {
                        fila.cantidad = (decimal)total_horas * cntrqty;
                    }
                    if (fila.tipo.Contains("C"))
                    {
                        fila.cantidad = ((int)total_dias - 5) * (this.bksize.Value.Contains("20") ? 1 : 2);
                        fila.cantidad = fila.cantidad * cntrqty;
                        fila.cantidad = fila.cantidad > 0 ? fila.cantidad : 0;
                    }

                    if (fila.tipo.Contains("T"))
                    {
                        fila.cantidad = fila.cantidad * cntrqty;
                        fila.cantidad = fila.cantidad > 0 ? fila.cantidad : 0;
                    }
            }
            

            //a todos los refeer cobrar x hora
            // a todos cobrar almacenaje


            //guarda la tabla, guarda la cantidad del booking
          
            ViewState["cantidad"] = cntrqty;
            this.alerta.InnerHtml = string.Format("<strong>Proforma generada para el Booking #:{0}, Reservas #:{1}, Fecha:{2}</strong>",bonbr,txtqty.Text,DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            //esto carga la table de nuevo
            populateRepeater(table, cntrqty);
            ViewState["table"] = table;
            sinresultado.Visible = false;
            btnera.Visible = true;
            alerta.Visible = true;
            xfinder.Visible = true;
        }
        protected void chkCheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                var chk = (CheckBox)sender;
                if (chk != null)
                {
                    var linea = chk.Attributes["argumento"];
                    var html = chk.Attributes["evento"];
                    if (!chk.Checked && !string.IsNullOrEmpty(html))
                    {
                        this.notario.InnerHtml = html;
                        mpedit.Show();
                        ViewState["data"] = linea;
                        ViewState["cliente"] = chk.ClientID;
                    }
                    if (!string.IsNullOrEmpty(linea))
                    {
                        var xtable = (Catalogos.ExportacionServicesDataTable)ViewState["table"];
                        var fila = xtable.FindBycodigo(linea);
                        fila.aplica = chk.Checked;
                        ViewState["table"] = null;
                        int cantidad = int.Parse(ViewState["cantidad"].ToString());
                        populateRepeater(xtable, cantidad);
                        ViewState["table"] = xtable;
                        this.alerta.InnerHtml = this.alerta.InnerHtml;
                    }
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "Proforma", "chkCheckedChanged", "Hubo un error cambiar el check", t != null ? t.loginname : "NoUser"));
                sinresultado.Visible = true;
            }
        }
        public void populateRepeater(Catalogos.ExportacionServicesDataTable table, int cantidad)
        {
            try
            {
                foreach (var f in table)
                {
                    if (!f.icont)
                    {
                        f.mensaje = !f.IsnotaNull() && !string.IsNullOrEmpty(f.nota)? 
                        string.Format("<a class='infotip'><span class='classic'>{0}</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>", !f.IsnotaNull() ? f.nota:""):"";
                      //f.contenido =  !f.IsnotaNull() && !string.IsNullOrEmpty(f.nota)? string.Format("<a class='tooltip'><span class='classic'>{0}</span>{1}</a>", f.nota, f.descripcion):f.descripcion;
                        f.contenido = f.descripcion;
                        f.icont = true;
                    }
                    
                    f.cantidad = f.tipo.Contains("T")? cantidad: f.cantidad;
                    if (!f.aplica)
                    {
                        f.cantidad = 0;
                    }
                    f.vtotal = (f.costo * f.cantidad);
                }
                tablaNueva.DataSource = table;
                tablaNueva.DataBind();
                var subt = table.Where(a => a.aplica).ToList().Sum(b => b.vtotal);
                stunit.InnerHtml = subt.ToString("C");
                string iva = "12";
                if (System.Configuration.ConfigurationManager.AppSettings["iva"] != null)
                {
                    iva = System.Configuration.ConfigurationManager.AppSettings["iva"];
                }
                this.etiIva.InnerText = string.Format("IVA {0}%(+)", iva);
                var ivac = decimal.Parse(iva);
                var ivaprint = subt * (ivac / 100);
                siva.InnerHtml = ivaprint.ToString("C");
                sttal.InnerHtml = (subt + ivaprint).ToString("C");
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "Proforma", "populateRepeater", "Hubo un ERROR en BOTON CANCELAR", t != null ? t.loginname : "NoUser"));
                sinresultado.Visible = true;
            }
        }
        protected void btprint_Click(object sender, EventArgs e)
        {
            var usn = new usuario();
            HttpCookie token = HttpContext.Current.Request.Cookies["token"];
           usn = this.getUserBySesion();
           if (usn == null)
           {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "proforma", "btbuscar_Click", "No usuario", Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
           }
           if (token == null)
           {
                this.Alerta("Estimado Cliente,Su sesión ha expirado, sera redireccionado a la pagina de login", true);
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Clear();
                return;
           }

            Int64 bokingkey = 0; // gkey solo una referencia
            Int32 qty =0;
            decimal totalIva = 0;
            Int32 reserva = 0;

           //obtiene la cantidad de la caja y la valida
            if (!Int32.TryParse(txtqty.Text, out qty) || qty <= 0)
            {
                this.sinresultado.InnerText = "Por favor comuníquese con nosotros, al parecer el Booking presenta problemas.";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //obtiene el gkey del booking y lo valida
            if (!Int64.TryParse(bkey.Value, out bokingkey) || bokingkey <= 0)
            {
                this.sinresultado.InnerText = "Por favor comuniquese con nosotros, al parecer el Booking presenta problemas.";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //valida el numero de booking en texto
            if (string.IsNullOrEmpty(nbrboo.Value))
            {
                this.sinresultado.InnerText = "Por favor seleccione el booking";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //verficar la cantidad de reservas del booking
            if (!Int32.TryParse(bkqty.Value, out reserva) || reserva <= 0)
            {
                this.sinresultado.InnerText = string.Format("El booking {0}, no tiene reservas disponibles", nbrboo.Value);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //nuevos campos------------------------>
            DateTime fechacliente = DateTime.Now;
            DateTime fechazarpe;
            DateTime fecgaCutoff;
            int isrefer = 0;
            //-------------------------------------->

            CultureInfo enUS = new CultureInfo("es-US");
            if (!DateTime.TryParseExact(this.bketd.Value, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechazarpe))
            {
                this.sinresultado.InnerText = "La fecha de zarpe tiene un formato incorrecto.";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }



            if (!DateTime.TryParseExact(this.bkcutof.Value.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecgaCutoff))
            {
                this.sinresultado.InnerText = "La fecha estimada de cutOff no tiene un formato válido";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }



            var debug_tipe = true;
            if (System.Configuration.ConfigurationManager.AppSettings["activar_proforma"] != null && System.Configuration.ConfigurationManager.AppSettings["activar_proforma"].Contains("1"))
            {
                debug_tipe = false;
            }

        
            if (!debug_tipe)
            {
                if (!DateTime.TryParseExact(this.txdate.Text.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacliente))
                {
                    this.sinresultado.InnerText = "La fecha estimada de retiro no tiene un formato válido";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }

                if (!int.TryParse(this.bkrefer.Value, out isrefer))
                {
                    this.sinresultado.InnerText = "No se ha podido determinar si las unidades utilizan conexión";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }
            }








            var proforma = new Proforma();
            proforma.Email = usn.email;
            var xtable = (Catalogos.ExportacionServicesDataTable)ViewState["table"];
            proforma.FechaSalida = DateTime.Now;
            proforma.IdGrupo = usn.grupo.HasValue ? usn.grupo.Value:0; 
            proforma.IdUsuario = usn.id; 
            proforma.UsuarioIngreso = usn.loginname;
            proforma.ReteFuente = 0;
            proforma.ReteIVA = 0;
            proforma.Referencia = bkrefe.Value;
            proforma.Nave = bknna.Value;
            proforma.RUC = usn.ruc;
            proforma.Token = token != null ? token.Value : "DEBUG"; ;
            proforma.Gkey = bokingkey;
            proforma.Estado = true;
            proforma.Bokingnbr = nbrboo.Value;
            proforma.Cantidad = qty;
            proforma.Reserva = reserva;

            //Nuevos campos
            proforma.FechaCliente = fechacliente;
            proforma.Etd = fechazarpe;
            proforma.Reefer = isrefer== 1;
            proforma.Cutoff = fecgaCutoff;
            proforma.Size = this.bksize.Value;
            
            var secu =1;
            foreach (var r in xtable)
            {
                if (r.aplica)
                {
                    var detalle = new ProformaDetalle();
                    detalle.BL = nbrboo.Value;
                    detalle.Cantidad = r.tipo.Contains("T")? qty:r.cantidad ;
                    detalle.CodigoServicio = r.codigo;
                    detalle.DescServicio = r.descripcion;
                    //detalle.Contenedor =""
                    detalle.FechaAlmacenaje = DateTime.Now;
                    detalle.Item = secu;
                    detalle.Referencia = bkrefe.Value;
                    detalle.ValorTotal = r.vtotal;
                    detalle.ValorUnitario = r.costo;
                    proforma.Detalle.Add(detalle);
                    totalIva += r.vtotal;
                    secu++;
                }
            }

            //calculos de iva y totales
            decimal iva = 0;
            if (System.Configuration.ConfigurationManager.AppSettings["iva"] != null)
            {
                if(!decimal.TryParse(System.Configuration.ConfigurationManager.AppSettings["iva"],out iva))
                {
                    iva = 0;
                }
            }
            proforma.IVA = totalIva * (iva/100);
            proforma.Total = totalIva +  proforma.IVA ;
            proforma.SubTotal = totalIva;

            var msm = string.Empty;
            var se=   proforma.Guardar(out msm);
            if (!string.IsNullOrEmpty(msm))
            {
                this.sinresultado.InnerText = msm;
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                return;
            }
            var sid = QuerySegura.EncryptQueryString(se);
            this.Popup(string.Format("../portal/printproforma.aspx?sid={0}", sid));

            //esto es para limpiar
            this.sinresultado.InnerText = string.Format("Proforma {0} generada con éxito.", proforma.Secuencia);
            sinresultado.Visible = true;
            btnera.Visible = false;
            alerta.Visible = false;
            xfinder.Visible = false;
            nbrboo.Value = string.Empty;
            return;

        }
        protected void repeat_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Header)
            {
                if (e.Item.HasControls())
                {
                    foreach (var c in e.Item.Controls)
                    {
                        var control = c as CheckBox;
                        if (control != null)
                        {
                            ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(control);
                        }
                    }
                }
            }

        }
        protected void btcancel_Click(object sender, EventArgs e)
        {
            try
            {
                var linea = ViewState["data"] as string;
                if (!string.IsNullOrEmpty(linea))
                {
                    var xtable = (Catalogos.ExportacionServicesDataTable)ViewState["table"];
                    var fila = xtable.FindBycodigo(linea);
                    fila.aplica = true;
                    ViewState["table"] = null;
                    ViewState["table"] = xtable;
                    int cantidad = int.Parse(ViewState["cantidad"].ToString());
                    populateRepeater(xtable, cantidad);
                    this.alerta.InnerHtml = this.alerta.InnerHtml;
                }
                var cct = ViewState["cliente"] as string;
                var tx = string.Format(" var xxxx1 =  document.getElementById('{0}'); if(xxxx1 != null) xxxx1.checked=true;  ", cct);
                this.Addscript(tx);
                mpedit.Hide();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "Proforma", "btcancel_Click", "Hubo un ERROR en BOTON CANCELAR", t != null ? t.loginname : "NoUser"));
                sinresultado.Visible = true;
            }
        }
    }
}