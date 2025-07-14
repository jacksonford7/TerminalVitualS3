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
using System.Data;
using System.Configuration;

namespace CSLSite
{
    public partial class turno_cancela : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "turno_cancela", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turno_cancela", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
              this.agencia.Value = user.ruc;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
            this.nbrboo.Value = Server.HtmlEncode(nbrboo.Value);
            this.tfecha.Text = Server.HtmlEncode(this.tfecha.Text);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;
            }
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                 
                    
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }
                    populate();
                    var sid = QuerySegura.EncryptQueryString(string.Format("{0};{1};{2}", nbrboo.Value, tfecha.Text, this.agencia.Value));
                    this.aprint.HRef = string.Format("reporte.aspx?sid={0}",sid);
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
   
        private void populate()
        {
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            Session["resultado"] = null;
            var table = new System.Data.DataTable();
            var menerr = string.Empty;
            try
            {
                DateTime desde;
                if (!DateTime.TryParseExact(this.tfecha.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde".ToUpper();
                    sinresultado.Visible = true;
                    return;
                }

                TimeSpan ts = desde.Date - DateTime.Now.Date;
                int diferenciaEnDias = ts.Days;
                int diasMaxCacelacion = turno.GetMaxDiasCancelacion();
                if (diferenciaEnDias < diasMaxCacelacion)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerHtml = "Toda cancelaciòn de reserva debe procesarse 48 horas antes de la operaciòn, caso contrario generarà costos adicionales por extra-manipuleos. <br/>" +
                                                  "Favor comunicarse con nuestra Àrea de Logistica y Almacenamiento <br />" + 
                                                  "Pbx: +593 (04) 6006300, 3901700 ext. 4002, 4021, 4005. <br />" +
                                                  "Email: <a href='mailto:CGSA-Consolidaciones@cgsa.com.ec'>CGSA-Consolidaciones@cgsa.com.ec</a>;";

                    sinresultado.Visible = true;
                    return;
                }

                var msg = turno.GetHorariosProgramados(nbrboo.Value, this.agencia.Value, desde, out menerr);
                if (!string.IsNullOrEmpty(menerr))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = menerr;
                    sinresultado.Visible = true;
                    return;
                }

                table = turno.GetHorariosProgramados(nbrboo.Value, this.agencia.Value, desde, out menerr);
                if (table.Rows.Count <= 0)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = "No se encontraron resultados, revise el booking / fecha".ToUpper();
                    sinresultado.Visible = true;
                    return;
                }

                Session["resultado"] = table;
                this.tablePagination.DataSource = table;
                this.tablePagination.DataBind();
                xfinder.Visible = true;
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "turno_cancela", "populate", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;
            }
            finally
            {
                table.Dispose();
            }
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var t = this.getUserBySesion();
            if (t == null)
            {
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Error al ItemCommand"), "turno_cancela", "populate", "Hubo un error al buscar el usuario", "sistema"));
                sinresultado.Visible = true;
            }

            DateTime desde;
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            if (!DateTime.TryParseExact(this.tfecha.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
            {
                xfinder.Visible = false;
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-info";
                this.sinresultado.InnerText = "NO SE ENCONTRARON RESULTADOS REVISE LA FECHA";
                sinresultado.Visible = true;
                return;
            }

            //if (string.IsNullOrEmpty(tmail.Value))
            //{
            //    sb.Append("alert('Escriba el correo electronico para la notificaciòn.');");
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
            //    return;
            //}

            var id = Int64.Parse(e.CommandArgument.ToString());
            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            var user = new ObjectSesion();
            //el mail del usuario logueado
            var user_email = sUser.email;
            string mensaje = string.Empty, mail = string.Empty;
            string sdesde = "", shasta = ""; int sresultado = 0; 
            try
            {
                DataTable tabladet = new DataTable();
                tabladet = (DataTable)Session["resultado"];
                var detalle = from p in tabladet.AsEnumerable()
                              where p.Field<Int64>("ID_HORARIO_DET") == id
                              select new
                              {
                                  DESDE = p.Field<string>("DESDE"),
                                  HASTA = p.Field<string>("HASTA"),
                                  RESERVADO = p.Field<Int32>("RESERVADO")
                              };
                foreach (var det in detalle)
                {
                    sdesde = det.DESDE;
                    shasta = det.HASTA;
                    sresultado = det.RESERVADO;
                }

                var exportador = turno.GetExportador(sUser.id);
                if (exportador == "0" || string.IsNullOrEmpty(exportador))
                {
                    exportador = agencia.Value.ToString();
                }
                var nombre_empresa = turno.GetNombreEmpresa(nbrboo.Value.ToString());
                if (nombre_empresa == "0" || string.IsNullOrEmpty(nombre_empresa))
                {
                    nombre_empresa = agencia.Value.ToString();
                }

                user.clase = "turnos"; user.metodo = "ValidateJSON";
                user.transaccion = "asignar_turno"; user.usuario = sUser.loginname;
                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje del Sistema de Terminal Virtual de Contecon S.A, para comunicarle lo siguiente.<br/>A continuación el detalle de la cancelación:<br/>", sUser.nombres, sUser.apellidos));
                //mail = string.Concat(mail, string.Format("<strong>Fecha: </strong>{0}<br/><strong>Linea: </strong>{1}<br/><strong>Booking: </strong>{2}<br/>", desde.Date, this.agencia.Value.ToString(), nbrboo.Value.ToString()));
                mail = string.Concat(mail, string.Format("<strong>Línea: </strong>{0}<br/><strong>Exportador: </strong>{1}<br/><strong>Booking: </strong>{2}<br/><strong>Fecha Consolidación: </strong>{3}<br/>", exportador, nombre_empresa, nbrboo.Value.ToString(), desde.Date.ToString("dd/MM/yyyy")));
                mail = string.Concat(mail, "<table rules='all' border='10'><tr><th align='center'>Desde</th><th align='center'>Hasta</th><th align='center'>Cancelado</th></tr>");
                //var car = new CSLSite.unitService.mailserviceSoapClient();
                //car.sendmail(mail, string.Format("{0};" + destinatarios, ""), "", "");
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = ex.Message;
                sinresultado.Visible = true;
                return;
            }

            var error = string.Empty;
            if (e.CommandName == "Anula")
            {
                if (!turno.Cancelar(this.nbrboo.Value, desde, t.ruc, t.loginname, id, out error))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = error;
                    sinresultado.Visible = true;
                    return;
                }
                try
                {
                    string serror = string.Empty;
                    string destinatarios = turno.GetMails();
                    destinatarios = user_email + ";" + tmail.Value + ";" + destinatarios + ";CGSA-Consolidaciones@cgsa.com.ec";
                    mail = string.Concat(mail, string.Format("<tr><td align='center'>{0}</td><td align='center'>{1}</td><td align='center'>{2}</td></tr>", sdesde, shasta, sresultado.ToString()));
                    mail = string.Concat(mail, "</table>");
                    var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";



                    turno.addMail(out serror, sUser.email, "Se generó una reserva de turnos para consolidación, *Booking " 
                        + nbrboo.Value.ToString(), mail, correoBackUp, user.usuario, "", "");
                    if (!string.IsNullOrEmpty(serror))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format(serror, csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Error al ItemCommand"), "turno_cancela", "populate", "Hubo un error al enviar mail", "sistema"));
                        sinresultado.Visible = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = ex.Message;
                    sinresultado.Visible = true;
                    return;
                }
            }
            else if (e.CommandName == "Modifica")
            {
                List<string> listvalor = new List<string>();
                int original = 0;
                int nuevo = 0;
                if (!string.IsNullOrEmpty(valornull()))
                {
                    sb.Append("alert('No se puede Modificar valores en blanco o nulos.');");
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
                    populate();
                    return;
                }
                listvalor = valormodifica(out original);
                if (listvalor.Count == 0)
                {
                    sb.Append("alert('No existen nuevos valores para Modificar.');");
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
                    populate();
                    return;

                }
                else if (listvalor.Count >= 2)
                {
                    sb.Append("alert('Modifique un turno a la vez.');");
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
                    populate();
                    return;
                }
                nuevo = Convert.ToInt32(listvalor[0]);
                if (nuevo > original)
                {
                    sb.Append("alert('El valor a modificar no puede ser mayor al valor original.');");
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
                    populate();
                    return;
                }
                if (!turno.Modificar(this.nbrboo.Value, nuevo, desde, t.ruc, t.loginname, id, out error))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = error;
                    sinresultado.Visible = true;
                    return;
                }
                try
                {
                    string serror = string.Empty;
                    string destinatarios = turno.GetMails();
                    destinatarios = user_email + ";" + tmail.Value + ";" + destinatarios;
                    mail = string.Concat(mail, string.Format("<tr><td align='center'>{0}</td><td align='center'>{1}</td><td align='center'>{2}</td></tr>", sdesde, shasta, nuevo.ToString()));
                    mail = string.Concat(mail, "</table>");
                    var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";

                    turno.addMail(out serror, destinatarios, "Se genero una modificación de turno para consolidación, Booking " + nbrboo.Value.ToString(), mail,correoBackUp, user.usuario, "", "");
                    if (!string.IsNullOrEmpty(serror))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format(serror, csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Error al ItemCommand"), "turno_cancela", "populate", "Hubo un error al enviar mail", "sistema"));
                        sinresultado.Visible = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = ex.Message;
                    sinresultado.Visible = true;
                    return;
                }
            }
            sb.Append("alert('Proceso exitoso en unos minutos recibirá el mail.');");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
            populate();
        }

        public List<string> valormodifica(out int original)
        {
            List<string> valor = new List<string>();
            original = 0;
            foreach (RepeaterItem rpItem in this.tablePagination.Items)
            {
                if (rpItem.ItemType == ListItemType.Item || rpItem.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtValorModifica = (TextBox)rpItem.FindControl("caja");
                    if (!string.IsNullOrEmpty(txtValorModifica.Text))
                    {
                        HiddenField hd = (HiddenField)rpItem.FindControl("hdreservado");
                        if (hd.Value.ToString() != txtValorModifica.Text)
                        {
                            original = Convert.ToInt32(hd.Value);
                            valor.Add(txtValorModifica.Text);
                        }
                    }
                }
            }
            return valor;
        }

        public string valornull()
        {
            string valor = string.Empty;
            foreach (RepeaterItem rpItem in this.tablePagination.Items)
            {
                if (rpItem.ItemType == ListItemType.Item || rpItem.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtValorModifica = (TextBox)rpItem.FindControl("caja");
                    if (string.IsNullOrEmpty(txtValorModifica.Text))
                    {
                        valor = "1";
                        return valor;
                    }
                }
            }
            return valor;
        }
    }
}