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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "proforma", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
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
            this.bkey.Value =Server.HtmlEncode(this.bkey.Value);
            this.bkqty.Value = Server.HtmlEncode( this.bkqty.Value);
            this.nbqty.InnerText = !string.IsNullOrEmpty(this.bkqty.Value)? Server.HtmlEncode(this.bkqty.Value +" U"):"...";
            this.nbrboo.Value = !string.IsNullOrEmpty(this.nbrboo.Value)?Server.HtmlEncode( this.nbrboo.Value):"...";
            this.numbook.InnerText = Server.HtmlEncode(this.nbrboo.Value);
            this.bkrefe.Value = Server.HtmlEncode(this.bkrefe.Value);
            this.bknna.Value = Server.HtmlEncode(this.bknna.Value);

            var listaGral = CslHelper.getImpuestos();
            List<Tuple<string, string>> rtfte = listaGral.Where(c => c.Item1.Contains("1")).Select(j => Tuple.Create(j.Item2, j.Item3.ToString())).ToList();
            rtfte.Add(Tuple.Create("RET. FUENTE 0%","0"));
            List<Tuple<string, string>> rtiva = listaGral.Where(c => c.Item1.Contains("2")).Select(j => Tuple.Create(j.Item2, j.Item3.ToString())).ToList();
            rtiva.Add(Tuple.Create("RET. IVA 0%", "0"));

            populateDrop(dpfte, rtfte);
            populateDrop(dpiva, rtiva);
            if (!IsPostBack)
            {
                sinresultado.Visible = false;
                val_fte.Value = dpfte.SelectedValue;
                val_iva.Value = dpiva.SelectedValue;
            }
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

            if (val_fte.Value.Contains("Z"))
            {
                this.sinresultado.InnerHtml = "Seleccione el porcentaje retención a la Fuente";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }

            if (val_iva.Value.Contains("Z"))
            {
                this.sinresultado.InnerHtml = "Seleccione el porcentaje retención al IVA";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }

            var cntrqty = 0; //caja cantidad
            Int64 bokingkey; // gkey solo una referencia
            Int32 bokinqty = 0; //cantidad del booking
            var bonbr = this.nbrboo.Value;
            Int32 N4BookingQty = 0;
            Int32 ProTotal = 0;

            //nuevos campos------------------------>
            DateTime fechacliente = DateTime.Now;
            DateTime fechazarpe = DateTime.Now;
            int isrefer = 0;
            int horasFree = 0;
            int diasFree = 0;

        
            //nuevo 2017------>
            var cfgs = Session["parametros"] as List<dbconfig>;
            if (cfgs != null && cfgs.Where(s => s.config_name.Contains("diaslibres")).Count() > 0)
            {
                if (!int.TryParse(cfgs.Where(s => s.config_name.Contains("diaslibres")).FirstOrDefault().config_value, out diasFree))
                {
                    this.sinresultado.InnerText = "Hubo un problema al intentar procesar los datos y configuraciones,  por favor salga e ingrese de nuevo al sistema ";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }
            }
            else
            {
                this.sinresultado.InnerText = "Hubo un problema al intentar leer las configuraciones por favor salga e ingrese de nuevo al sistema";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            //-------------------------------------->

            CultureInfo enUS = new CultureInfo("es-US");
            var en_debug = true;
            var actp = cfgs.Where(f => f.config_name.Contains("activar_proforma")).FirstOrDefault();
            if (actp != null && !string.IsNullOrEmpty(actp.config_value))
            {
                en_debug = actp.config_value.Contains("1") ? false : true;
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
                if (!DateTime.TryParseExact(this.txdate.Text.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechacliente))
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
            double total_horas = 0;
            double total_dias = 0;

            if (!en_debug)
            {
                if (isrefer != 0)
                {
                   //Obtener horas libres desde la linea del booking-->20
                    horasFree = ProformaHelper.HorasFree(exporID.Value);
                }
                int tt =(int) (fechazarpe - fechacliente).TotalHours;

                //si asume 100%--> entonces retorna negativo
                horasFree = horasFree >= 0 ? horasFree : tt;
                //asume el 100% de las horas reefer.---->nuevo para horas de reefer
                total_horas = tt - horasFree; // + 24- horasFree;
                //si la resta de lo que asumen es mayor que el uso tons cero
                total_horas = total_horas < 0 ? 0 : total_horas;
                //parametro de días libres
                total_dias = (fechazarpe.Date - fechacliente.Date).TotalDays - diasFree + 1;
            }
            //comprueba que la cantidad sea un número
            if (!int.TryParse(txtqty.Text.Trim(), out cntrqty))
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
            //se obtien la cantidad actual del booking en N4.
            N4BookingQty = ProformaHelper.BookingQty(bokingkey);



            //la cantidad de la proforma actual No puede sobrepasar esta cantidad
            
            //antes del cambio.
            //ProTotal = ProformaHelper.Totalproformas(bokingkey);
            //despues del cambio -- ok aqui hice un cambio pero si esta
            ProTotal = ProformaHelper.Totalproformas(bonbr);


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
            if (diferencia <= 0)
            {
                this.sinresultado.InnerHtml = string.Format("El booking {0}, no tiene reservas disponibles, para poder generar esta proforma:<br/>* Anule proformas anteriores.<br/>* Amplíe la capactidad del Booking", nbrboo.Value);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(string.Format("El booking {0}, no tiene reservas disponibles\\nReservas Booking:[{1}]\\nEn proformas:[{2}]", nbrboo.Value, bokinqty, ProTotal));
                return;

            }
            //diferencia es lo que tiene restante
            if (cntrqty > diferencia)
            {
                this.sinresultado.InnerHtml = string.Format("El booking {0}, no tiene reservas disponibles, para poder cubrir la cantidad solicitada [{1}].<br/>  Para generar esta proforma intente lo siguiente:<br/>* Anule proformas anteriores.<br/>* Amplíe la capactidad del Booking<br/>* Reduzca la cantidad en esta proforma", nbrboo.Value, cntrqty);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(string.Format("El booking {0}, no tiene reservas disponibles:\\nReservas Booking:[{1}]\\nEn proformas:[{2}]\\nDiponible:[{3}]\\nCantidad solicitada:[{4}]  ", nbrboo.Value, bokinqty, ProTotal, diferencia, cntrqty));
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
            try { ta.Fill(table, usn.loginname); }
            catch (Exception ex){

                this.sinresultado.InnerText = ex.Message;

            }

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
                //--solo para refeers
                if (fila.tipo.Contains("R"))
                {
                    fila.cantidad = (decimal)total_horas * cntrqty;
                    fila.nota = !fila.IsnotaNull() && !string.IsNullOrEmpty( fila.nota) ? fila.nota + string.Format("<br/><strong>Total horas asumidas por la agencia [{0}]: = {1}</strong>", exporID.Value, horasFree):string.Empty;
                }
                //solo para secos
                if (fila.tipo.Contains("C"))
                {
                    //TAMAÑO
                    fila.cantidad = ((int)total_dias);// *(this.bksize.Value.Contains("20") ? 1 : 2);
                    fila.cantidad = fila.cantidad * cntrqty;
                    fila.cantidad = fila.cantidad > 0 ? fila.cantidad : 0;
                    fila.nota = !fila.IsnotaNull() && !string.IsNullOrEmpty( fila.nota) ? fila.nota + string.Format("<br/><strong>Días Libres: {0}</strong>", diasFree):string.Empty;
                }
                //TODOS
                if (fila.tipo.Contains("T"))
                {
                    fila.cantidad = fila.cantidad * cntrqty;
                    fila.cantidad = fila.cantidad > 0 ? fila.cantidad : 0;
                }

                //aqui actualizar con default solo en la primera carga
                //fila.aplica = fila.defecto;

            }

            //guarda la tabla, guarda la cantidad del booking
            ViewState["cantidad"] = cntrqty;
            this.alerta.InnerHtml = string.Format("<strong>Proforma generada el {2}-> Booking:{0}, Reservas:{1}, Ret.Iva:{3}%, Ret.Fte:{4}%</strong>", bonbr, txtqty.Text, DateTime.Now.ToString("dd/MM/yyyy HH:mm"),val_iva.Value,val_fte.Value);

            //seteo los valores con los que se genera esta proforma
            ViewState["val_iva"] = val_iva.Value;
            ViewState["val_fte"] = val_fte.Value;
            //-------------------->



      

            //esto carga la table de nuevo
           int mul=(this.bksize.Value.Contains("20") ? 1 : 2);

            populateRepeater(table, cntrqty,mul);
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
                    //nuevo atributo indica grupos excluyentes.
                    string grupo = chk.Attributes["grupo"];

                    if (!chk.Checked && !string.IsNullOrEmpty(html))
                    {
                        this.notario.InnerHtml = html;
                        mpedit.Show();
                        ViewState["data"] = linea;
                        ViewState["cliente"] = chk.ClientID;
                    }
                    var xtable = (Catalogos.ExportacionServicesDataTable)ViewState["table"];
                    if (xtable != null)
                    {
                        int cantidad = 0;
                        if (!string.IsNullOrEmpty(grupo))
                        {
                            //busque todos los de mi grupo y haga lo 
                            if (chk.Checked)
                            {
                                //xtable.Where(r => r.condicion.Equals(grupo) && !r.codigo.Equals(linea)).ToList().ForEach(f =>
                                //{
                                //    f.aplica = false;
                                //});

                            }

                        }
                        if (!string.IsNullOrEmpty(linea))
                        {
                            var fila = xtable.FindBycodigo(linea);
                            fila.aplica = chk.Checked;
                            ViewState["table"] = null;
                             cantidad= int.Parse(ViewState["cantidad"].ToString());
                            ViewState["table"] = xtable;
                            this.alerta.InnerHtml = this.alerta.InnerHtml;
                           
                        }
            
                        populateRepeater(xtable, cantidad, 1);
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
        public void populateRepeater(Catalogos.ExportacionServicesDataTable table, int cantidad,int multipl)
        {
            try
            {
                //2020-03-24: para condiciones default de items
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

                    f.cantidad = f.tipo.Contains("T") ? cantidad: f.cantidad;
                    if (!f.aplica)
                    {
                        f.cantidad = 0;
                    }
                    //TODO CORREGIR SOLO EN PRIMER CASO
                    if (f.tipo.Contains("C") && multipl > 0)
                    {
                        f.vtotal = ((f.costo * multipl) * f.cantidad);
                        f.costo = (f.costo * multipl);
                    }
                    else
                    {
                        f.vtotal = (f.costo * f.cantidad);
                    }
                }
                decimal pct_iva = 0;
                decimal pct_fte = 0;

                //-->Valor seleccionado en el combo FUENTE
                if (!decimal.TryParse(val_fte.Value, out pct_fte))
                {
                    this.sinresultado.InnerText = "Seleccione el porcentaje de Renteción en la Fuente (%)";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }
                //->vALOR SELECCIONADO EN COMBO iva
                if (!decimal.TryParse(val_iva.Value, out pct_iva))
                {
                    this.sinresultado.InnerText = "Seleccione el porcentaje de Renteción del IVA (%)";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }

                tablaNueva.DataSource = table;
                tablaNueva.DataBind();
                var subt = table.Where(a => a.aplica).ToList().Sum(b => b.vtotal);
                stunit.InnerHtml = subt.ToString("C");

                string iva = "00";
                var cfgs = Session["parametros"] as List<dbconfig>;
                if (cfgs != null && cfgs.Where(s => s.config_name.Contains("IVA")).Count() > 0)
                {
                    iva = cfgs.Where(s => s.config_name.Contains("IVA")).FirstOrDefault().config_value;  
                } 
                else
                {
                    this.sinresultado.InnerText = "Hubo un problema al intentar leer las configuraciones por favor salga e ingrese de nuevo al sistema";
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }

                this.etiIva.InnerHtml = string.Format(" <span style='color:blue;'> IVA {0}% <strong>(+)</strong> </span>", iva);
                this.etisrfte.InnerHtml = string.Format("<span style='color:red;'>Ret.Fte {0}%<strong>(-)</strong> </span>", pct_fte);
                this.stisriva.InnerHtml = string.Format("<span style='color:red;'>Ret.IVA {0}%<strong>(-)</strong> </span>", pct_iva);

                var ivac = decimal.Parse(iva);
                var ivaprint = subt * (ivac / 100);
              
                //-->Nuevo proforma 2017
                decimal Rfte = subt* (pct_fte / 100);
                decimal Riva =ivaprint* (pct_iva / 100);
                siva.InnerHtml = string.Format("{0:c}", ivaprint);// ivaprint.ToString("C");
                srfte.InnerHtml = string.Format("{0:c}",Rfte );
                sriva.InnerText = string.Format("{0:c}", Riva);
                sttal.InnerHtml = string.Format("{0:c}", subt + ivaprint - Rfte - Riva);//().ToString("C");
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

            //intenta obtener desde la busqueda el valor del QTY y ese usa
            var strQty = ViewState["cantidad"] != null ? ViewState["cantidad"].ToString() : string.Empty;

   
            if (string.IsNullOrEmpty(strQty))
            {
                this.sinresultado.InnerText = string.Format("Por favor primero genere la proforma antes de guardar o imprimir QTY:{0}", ViewState["cantidad"]);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }




           //obtiene la cantidad de la caja y la valida
            if (!Int32.TryParse(strQty, out qty) || qty <= 0)
            {
                this.sinresultado.InnerText = "Por favor comuníquese con nosotros, al parecer el [Booking/QTY] presenta problemas.";
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
            var cfgs = Session["parametros"] as List<dbconfig>;
            if (cfgs != null && cfgs.Where(s => s.config_name.Contains("activar_proforma")).Count() > 0)
            {
                if (cfgs.Where(s => s.config_name.Contains("activar_proforma")).FirstOrDefault().config_value.Contains("1"))
                {
                    debug_tipe = false;
                }
            }
            else
            {
                this.sinresultado.InnerText = "Hubo un problema al intentar leer las configuraciones por favor salga e ingrese de nuevo al sistema";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            if (!debug_tipe)
            {
                if (!DateTime.TryParseExact(this.txdate.Text.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechacliente))
                {
                    this.sinresultado.InnerText = "La fecha estimada de ingreso no tiene un formato válido";
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
            proforma.FechaSalida = fechacliente;
            proforma.IdGrupo = usn.grupo.HasValue ? usn.grupo.Value:0; 
            proforma.IdUsuario = usn.id; 
            proforma.UsuarioIngreso = usn.loginname;
           
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

            var OIVA =cfgs.Where(s => s.config_name.Contains("IVA")).FirstOrDefault();
            if (OIVA == null || !decimal.TryParse( OIVA.config_value, out iva))
            {
                this.sinresultado.InnerText = "Hubo un problema al intentar leer las configuraciones por favor salga e ingrese de nuevo al sistema";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = false;
                this.Alerta(this.sinresultado.InnerText);
                return;
            }
            proforma.IVA = totalIva * (iva / 100);
            

            var pct_iva = decimal.Parse(ViewState["val_iva"] as string);
            var pct_fte = decimal.Parse(ViewState["val_fte"] as string);
            
            proforma.ReteFuente = totalIva * (pct_fte/100);
            proforma.ReteIVA = proforma.IVA * (pct_iva/100);

            proforma.PctIVA = pct_iva;
            proforma.PctFuente = pct_fte;
            proforma.IvaMas = iva;

            proforma.SubTotal = totalIva;
            proforma.Total = totalIva + proforma.IVA - proforma.ReteFuente - proforma.ReteIVA;
            var msm = string.Empty;
            string se;
            if (usn.IsCredito)
            {
                se = proforma.Guardar(out msm);
            }
            else
            {
                se = proforma.GuardarContado(usn, out msm);
            }

            //aqui-->Grabar el mensaje de correro
            //nuevo correccion.
            if (usn.bloqueo_cartera)
            {
                var msmout = string.Empty;
                var mailtesorera = "tesoreria@cgsa.com.ec";
                var otes = cfgs.Where(s => s.config_name.Contains("mail_tesoreria")).FirstOrDefault();
                if (otes != null && !string.IsNullOrEmpty(otes.config_value))
                {
                    mailtesorera = otes.config_value;
                }
                var htm = new StringBuilder();
                htm.Append("Estimados:<br/>Este mensaje es para comunicarles que el cliente:<br/>");
                htm.AppendFormat("Nombres: {0}<br/>",usn.nombres);
                htm.AppendFormat("Apellidos: {0}<br/>", usn.apellidos);
                htm.AppendFormat("RUC: {0}<br/>", usn.ruc);
                htm.AppendFormat("Acaba de generar la proforma/liquidación no.{0}, por un total de {1:c} para el booking: {2}<br/>",proforma.Secuencia,proforma.Total,proforma.Bokingnbr);
                htm.Append("Esto con el propósito de informarle ya que el cliente en mención presenta valores vencidos a la fecha, sus contenedores presentarán el bloqueo correspondiente y será impedido de embarcar hasta que se solucione dicha situacion.");
                htm.Append("<br/>Terminal Virtual");
                CLSDataCentroSolicitud.addMail(out msmout, mailtesorera, string.Format("Cartera Vencida->RUC {0}", usn.ruc),htm.ToString(),"",usn.loginname,"proforma-mail",usn.loginname);
            }
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
                   
                    populateRepeater(xtable, cantidad,1);
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
        private void populateDrop(DropDownList dp, List<Tuple<string, string>> origen)
        {

            origen.Add(Tuple.Create("* Seleccione *","Z"));
            dp.DataSource = origen.OrderBy(g=>g.Item2);
            dp.DataValueField = "item2";
            dp.DataTextField = "item1";
            dp.DataBind();

            if (dp.Items.Count > 0)
            {
                if (dp.Items.FindByValue("Z") != null)
                {
                    dp.Items.FindByValue("Z").Selected = true;
                }
            }
        }
    }
}