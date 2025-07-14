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
using System.Configuration;

namespace CSLSite
{
    public partial class reservas_consolidacion : System.Web.UI.Page
    {
        #region "Anterior"
        // //AntiXRCFG
        // protected override void OnInit(EventArgs e)
        // {
        //     //base.OnInit(e);
        //     //ViewStateUserKey = Session.SessionID;
        // }
        // protected void Page_Init(object sender, EventArgs e)
        // {
        //     this.IsAllowAccess();
        //     Page.Tracker();
        //     if (!IsPostBack)
        //     {
        //         this.IsCompatibleBrowser();
        //         Page.SslOn();
        //     }
        //         this.booking.Text =Server.HtmlEncode(this.booking.Text);
        //         this.aisvn.Text =Server.HtmlEncode(this.aisvn.Text);
        //         this.cntrn.Text =Server.HtmlEncode(this.cntrn.Text);
        //         this.docnum.Text = Server.HtmlEncode(this.docnum.Text);
        //         this.desded.Text = Server.HtmlEncode(this.desded.Text);
        //         this.desded.Text = Server.HtmlEncode(this.hastad.Text);
        // }
        // protected void Page_Load(object sender, EventArgs e)
        // {
        //     //if (Response.IsClientConnected)
        //     //{
        //     //    xfinder.Visible = IsPostBack;
        //     //    sinresultado.Visible = false;
        //     //}
        // }
        // protected void btbuscar_Click(object sender, EventArgs e)
        // {
        //     if (Response.IsClientConnected)
        //     {
        //         try
        //         {
        //             //if (HttpContext.Current.Request.Cookies["token"] == null)
        //             //{
        //             //    System.Web.Security.FormsAuthentication.SignOut();
        //             //    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //             //}
        //             //populate();
        //         }
        //         catch (Exception ex)
        //         {
        //             //var t = this.getUserBySesion();
        //             //sinresultado.Attributes["class"] = string.Empty;
        //             //sinresultado.Attributes["class"] = "msg-critico";
        //             //sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
        //             //sinresultado.Visible = true;
        //         }
        //     }
        // }
        // public static string anulado(object estado)
        // {
        //     if (estado == null)
        //     {
        //         return "<span>sin estado!</span>";
        //     }
        //     if (estado.ToString().ToLower() == "r")
        //     {
        //         return "<span>Registrado</span>";
        //     }
        //     if (estado.ToString().ToLower() == "a")
        //     {
        //         return "<span class='red' >Anulado</span>";
        //     }
        //     if (estado.ToString().ToLower() == "i")
        //     {
        //         return "<span class='azul' >Ingresado</span>";
        //     }
        //     if (estado.ToString().ToLower() == "s")
        //     {
        //         return "<span class='naranja' >Salida</span>";
        //     }
        //     return "<span>sin estado!</span>";
        // }
        // public static string boton(object estado)
        // { 
        //   return  estado.ToString().ToLower() == "r" ? "ver":"xver";
        // }
        // public static string tipos(object tipo, object movi)
        // {
        //     if (tipo == null || movi == null)
        //     {
        //         return "!error";
        //     }

        //     if (tipo.ToString().Trim().Length < 1 || movi.ToString().Trim().Length < 1)
        //     {
        //         return "!error";
        //     }

        //     if (movi.ToString().Trim() == "E")
        //     {
        //         if (tipo.ToString().Trim() == "C")
        //         {
        //             return "Full";
        //         }
        //         else
        //         {
        //             return "C. Suelta";
        //         }
        //     }
        //     else
        //     {
        //         return "Consolidación";
        //     }
        // }
        // public static string securetext(object number)
        // {
        //     if (number == null || number.ToString().Length <= 2)
        //     {
        //         return string.Empty;
        //     }
        //     return QuerySegura.EncryptQueryString(number.ToString());
        // }
        // protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        // {
        //     if (Response.IsClientConnected)
        //     {
        //         try
        //         {
        //             if (HttpContext.Current.Request.Cookies["token"] == null)
        //             {
        //                 System.Web.Security.FormsAuthentication.SignOut();
        //                 Session.Clear();
        //                 System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                 return;
        //             }
        //             var user = Page.getUserBySesion();
        //             if (user == null)
        //             {
        //                 sinresultado.Attributes["class"] = string.Empty;
        //                 sinresultado.Attributes["class"] = "msg-critico";
        //                 sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
        //                 sinresultado.Visible = true;
        //                 return;
        //             }
        //             if (e.CommandArgument == null)
        //             {
        //                 sinresultado.Attributes["class"] = string.Empty;
        //                 sinresultado.Attributes["class"] = "msg-critico";
        //                 sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
        //                 sinresultado.Visible = true;
        //                 return;
        //             }

        //             var xpars = e.CommandArgument.ToString().Split(';');
        //             if (xpars.Length <= 0 || xpars.Length < 5)
        //             {
        //                 sinresultado.Attributes["class"] = string.Empty;
        //                 sinresultado.Attributes["class"] = "msg-critico";
        //                 sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
        //                 sinresultado.Visible = true;
        //                 return;
        //             }

        //             if (string.IsNullOrEmpty(xpars[5]) || xpars[5].ToLower() != "r")
        //             {
        //                 sinresultado.Attributes["class"] = string.Empty;
        //                 sinresultado.Attributes["class"] = "msg-critico";
        //                 sinresultado.InnerText = "No se puede anular este AISV ya que su estado ha cambiado, es posible que la carga ya se encuentre patios.";
        //                 sinresultado.Visible = true;
        //                 return;
        //             }

        //             if (!jAisvContainer.UnidadEnDeposito(xpars[2]))
        //             {
        //                 sinresultado.Attributes["class"] = string.Empty;
        //                 sinresultado.Attributes["class"] = "msg-critico";
        //                 sinresultado.InnerText = string.Format("No puede anular el AISV {0} ya que la unidad {1}, se encuentra en patios", xpars[0], xpars[2]);
        //                 sinresultado.Visible = true;
        //                 return;
        //             }

        //             //0.->AISV
        //             //1.->REFERENCIA
        //             //2.->UNIDAD
        //             //3.->MOV
        //             //4.->TIPO
        //             string vt = string.Empty;
        //             //cancel advice si es unidad
        //             if (!string.IsNullOrEmpty(xpars[2]))
        //             {
        //                 if (xpars[2] == "null")
        //                 {
        //                     sinresultado.Attributes["class"] = string.Empty;
        //                     sinresultado.Attributes["class"] = "msg-critico";
        //                     sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal y no se encontró el contenedor. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El registro era de contenedor lleno pero no se encontró la unidad"), "consulta", "tablePagination_ItemCommand", "Contenedor fué nulo", user.loginname));
        //                     sinresultado.Visible = true;
        //                     return;
        //                 }

        //                 //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
        //                 var userk = new ObjectSesion();
        //                 userk.clase = "consulta"; userk.metodo = "tablePagination_ItemCommand";
        //                 userk.transaccion = "AISV Consulta"; userk.usuario = user.loginname;

        //                //aqui era el error.
        //                 userk.token = HttpContext.Current.Request.Cookies["token"].Value;

        //                 //SI ES CONTENEDOR O ES CONSOLIDACION
        //                 if (xpars[3].ToUpper().Contains("C") || xpars[4].ToUpper().Contains("C"))
        //                 {
        //                    //si hay unidad o contenedor.
        //                     if (!string.IsNullOrEmpty(xpars[2]))
        //                     {
        //                         if (jAisvContainer.ConfirmacionPreaviso(xpars[2].Trim()))
        //                         {
        //                             if (!jAisvContainer.cancelAdvice(userk, xpars[2], xpars[1], out vt))
        //                             {
        //                                 sinresultado.Attributes["class"] = string.Empty;
        //                                 sinresultado.Attributes["class"] = "msg-critico";
        //                                 sinresultado.InnerText = string.Format("No se pudo eliminar el registro por la siguiente causa:[{1}], algo salió mal. Código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException(vt), "CancelAdvice", "tablePagination_ItemCommand", vt, user.loginname), vt);
        //                                 sinresultado.Visible = true;
        //                                 return;
        //                             }
        //                         }
        //                     }
        //                 }

        //             }
        //             if (!jAisvContainer.delete(xpars[0], user.loginname, out vt))
        //             {
        //                 sinresultado.Attributes["class"] = string.Empty;
        //                 sinresultado.Attributes["class"] = "msg-critico";
        //                 sinresultado.InnerText = vt;
        //                 sinresultado.Visible = true;
        //                 return;
        //             }

        //             //nuevo bloque si el aisv cambió por concurrencia, entonces no se puede anular
        //             if (!string.IsNullOrEmpty(vt))
        //             {
        //                 var negativo = 2;
        //                 if (!int.TryParse(vt, out negativo) || negativo < 0)
        //                 {
        //                     sinresultado.Attributes["class"] = string.Empty;
        //                     sinresultado.Attributes["class"] = "msg-critico";
        //                     sinresultado.InnerText = "No fúe posible anular este documento, es probable que la carga ya este ingresada o que el transporte ya este fuera de la terminal, confirme con planificación"; ;
        //                     sinresultado.Visible = true;
        //                     populate();
        //                     return;
        //                 }

        //             }
        //             sinresultado.Attributes["class"] = string.Empty;
        //             sinresultado.Attributes["class"] = "msg-info";
        //             sinresultado.InnerText = string.Format("La anulación del AISV  No.{0} ha resultado exitosa.", xpars[0]);
        //             sinresultado.Visible = true;
        //             populate();
        //         }
        //         catch (Exception ex)
        //         {
        //              var t = this.getUserBySesion();
        //              sinresultado.Attributes["class"] = string.Empty;
        //              sinresultado.Attributes["class"] = "msg-critico";
        //              sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al anular", t.loginname));
        //              sinresultado.Visible = true;

        //         }
        //     }
        // }
        // protected string jsarguments(object aisv, object referencia, object unidad, object movimiento, object tipo, object estado)
        // {
        //     return string.Format("{0};{1};{2};{3};{4};{5}", aisv != null ? aisv.ToString().Trim() : "0", referencia != null ? referencia.ToString().Trim() : "na", unidad != null ? unidad.ToString().Trim() : "null", movimiento != null ? movimiento.ToString().Trim() : "x", tipo != null ? tipo.ToString().Trim() : "x",estado);
        // }
        //private void populate()
        //{
        //    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
        //    Session["resultado"] = null;
        //    var table = new Catalogos.Listar_AISVDataTable();
        //    var ta = new CatalogosTableAdapters.Listar_AISVTableAdapter();
        //    try
        //    {
        //        DateTime desde;
        //        DateTime hasta;
        //        if (!DateTime.TryParseExact(  desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
        //        {
        //            xfinder.Visible = false;
        //            sinresultado.Attributes["class"] = string.Empty;
        //            sinresultado.Attributes["class"] = "msg-info";
        //            this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
        //            sinresultado.Visible = true;
        //            return;
        //        }
        //        if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
        //        {
        //            xfinder.Visible = false;
        //            sinresultado.Attributes["class"] = string.Empty;
        //            sinresultado.Attributes["class"] = "msg-info";
        //            this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
        //            sinresultado.Visible = true;
        //            return;
        //        }
        //        if (desde.Year != hasta.Year)
        //        {
        //            xfinder.Visible = false;
        //            sinresultado.Attributes["class"] = string.Empty;
        //            sinresultado.Attributes["class"] = "msg-alerta";
        //            this.sinresultado.InnerText = "El rango máximo de consulta es de 1 año, gracias por entender.";
        //            sinresultado.Visible = true;
        //            return;
        //        }
        //        TimeSpan ts = desde - hasta;
        //        // Difference in days.
        //        if (ts.Days > 30)
        //        {
        //            xfinder.Visible = false;
        //            sinresultado.Attributes["class"] = string.Empty;
        //            sinresultado.Attributes["class"] = "msg-alerta";
        //            this.sinresultado.InnerText = "El rango máximo de consulta es de 1 mes, gracias por entender.";
        //            sinresultado.Visible = true;
        //            return;
        //        }
        //        var user = Page.getUserBySesion();
        //        ta.ClearBeforeFill = true;
        //        //todo el filtro del usuario
        //        ta.Fill(table, user.loginname, this.aisvn.Text.Trim(), this.docnum.Text.Trim(), this.cntrn.Text.Trim(),this.booking.Text.Trim(), desde, hasta);
        //        if (table.Rows.Count <= 0)
        //        {
        //            xfinder.Visible = false;
        //            sinresultado.Attributes["class"] = string.Empty;
        //            sinresultado.Attributes["class"] = "msg-info";
        //            this.sinresultado.InnerText = "No se encontraron resultados, revise la unidad, documento o # aisv";
        //            sinresultado.Visible = true;
        //            return;
        //        }

        //        Session["resultado"] = table;
        //        this.tablePagination.DataSource = table;
        //        this.tablePagination.DataBind();
        //        xfinder.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var t = this.getUserBySesion();
        //        sinresultado.Attributes["class"] = string.Empty;
        //        sinresultado.Attributes["class"] = "msg-critico";
        //        sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "populate", "Hubo un error al buscar", t.loginname));
        //        sinresultado.Visible = true;
        //    }
        //    finally
        //    {
        //        ta.Dispose();
        //        table.Dispose();
        //    }
        //}
#endregion 

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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                this.agencia.Value = user.ruc;
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
                    sinresultado.Visible = false;
                    //alerta.Visible = false;
                    //xfinder.Visible = true;
                    if (string.IsNullOrEmpty(this.tfechaini.Text) &&
                        string.IsNullOrEmpty(this.tfechafin.Text) && 
                        string.IsNullOrEmpty(this.tbooking.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;Ingrese al menos un parametro de busqueda.{0}</strong>", this.tfechaini.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }
                    /*
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechaini;
                    DateTime fechafin;
                    if (!string.IsNullOrEmpty(tbooking.Text))
                    {
                        this.tfechaini.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        this.tfechafin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrEmpty(tfechaini.Text) && !string.IsNullOrEmpty(tfechafin.Text))
                    {
                        
                    }
                    if (!DateTime.TryParseExact(this.tfechaini.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaini))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;El formato de la Fecha Desde, debe ser dia/Mes/Anio {0}</strong>", this.tfechaini.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }
                    if (!DateTime.TryParseExact(this.tfechafin.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechafin))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;El formato de la Fecha Hasta, debe ser dia/Mes/Anio {0}</strong>", this.tfechafin.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }*/
                    //ejecutar ambos query

                    tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los filtros para el reporte.";
                    var tablix = turnoConsolidacion.GetRptReservasFull(this.tfechaini.Text, this.tfechafin.Text, this.tbooking.Text, this.agencia.Value, true, chkDetalle.Checked);
                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();
                    xfinder.Visible = true;
                    Session["DetalleReserva"] = chkDetalle.Checked;
                    if (string.IsNullOrEmpty(tbooking.Text))
                    {
                        tbooking.Text = "0";
                    }
                    var sid = QuerySegura.EncryptQueryString(string.Format("{0};{1};{2};{3}", tbooking.Text, this.agencia.Value, this.tfechaini.Text, this.tfechafin.Text));
                    this.aprint.HRef = string.Format("rptreservas.aspx?sid={0}", sid);
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
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public static string ValidateJSON(turnoConsolidacion objeto)
        {
            try
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "turnos", "ValidateJSON", "No autenticado", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Actualmente no se encuentra autenticado, sera redireccionado a la pagina de login");
                }
                //validar que la sesión este activa y viva
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "turnos", "ValidateJSON", "Sesión no existe", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Su sesión ha expirado, sera redireccionado a la pagina de login");
                }
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                //Validacion 3 -> Si su token existe, es válido, y no ha expirado
                if (token == null || !csl_log.log_csl.validateToken(token.Value))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "turnos", "ValidateJSON", "Sin Valor Token", "No tokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../cuenta/menu.aspx'", "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
                }

                var jmsg = new jMessage();
                var cantmaxbkg = turnoConsolidacion.GetCantMaxBkg(objeto.linea, objeto.booking);
                var sumcantreserva = turnoConsolidacion.GetSumCantReserva(objeto.booking);
                int validacantidad = 0;
                if (objeto.detalles.Count > 0)
                {
                    foreach (var v in objeto.detalles)
                    {
                        if (!string.IsNullOrEmpty(v.reserva))
                        {
                            if (v.reserva != "0")
                            {
                                validacantidad = validacantidad + Convert.ToInt32(v.reserva);
                            }
                        }
                    }
                }
                if (sumcantreserva == cantmaxbkg)
                {
                    jmsg.resultado = false;
                    string msgerror = "Booking no dispone de contenedores.";

                    jmsg.mensaje = msgerror;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                sumcantreserva = sumcantreserva + validacantidad;
                if (sumcantreserva > cantmaxbkg)
                {
                    jmsg.resultado = false;
                    string msgerror = "La cantidad a reservar supera el maximo del Booking." + System.Environment.NewLine +
                                      "*Cantidad total del Booking: " + cantmaxbkg.ToString() + System.Environment.NewLine +
                                      "*Minimo a reservar: " + (cantmaxbkg - turnoConsolidacion.GetSumCantReserva(objeto.booking)).ToString();

                    jmsg.mensaje = msgerror;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                var user = new ObjectSesion();

                var exportador = turnoConsolidacion.GetExportador(sUser.id);
                if (exportador == "0" || string.IsNullOrEmpty(exportador))
                {
                    exportador = objeto.linea;
                }
                var nombre_empresa = turnoConsolidacion.GetNombreEmpresa(objeto.booking);
                if (nombre_empresa == "0" || string.IsNullOrEmpty(nombre_empresa))
                {
                    nombre_empresa = objeto.linea;
                }

                user.clase = "turnos"; user.metodo = "ValidateJSON";
                user.transaccion = "asignar_turno"; user.usuario = sUser.loginname;
                string mensaje = string.Empty;
                jmsg.data = string.Empty;
                jmsg.fluir = false;
                //aqui usuario;
                objeto.usuario = sUser.loginname; ;
                //depurar los valores
                DataTransformHelper.CleanProperties<turnoConsolidacion>(objeto);
                //revalidar la información
                jmsg.resultado = turnoConsolidacion.validar(objeto, out mensaje);
                if (!jmsg.resultado)
                {
                    jmsg.mensaje = mensaje;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //guardar--->
                if (!objeto.add(out mensaje))
                {
                    jmsg.resultado = false;
                    jmsg.mensaje = mensaje;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                string mail = string.Empty;
                string destinatarios = turnoConsolidacion.GetMails();
                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje del Centro de Servicios en Linea de Contecon S.A, para comunicarle lo siguiente.<br/>A continuacion el detalle de su reserva:<br/>", sUser.nombres, sUser.apellidos));
                mail = string.Concat(mail, string.Format("<strong>Ruc: </strong>{0}<br/><strong>Exportador: </strong>{1}<br/><strong>Booking: </strong>{2}<br/><strong>Fecha Consolidación: </strong>{3}<br/>", exportador, nombre_empresa, objeto.booking, objeto.fecha_pro));
                if (objeto.detalles.Count > 0)
                {
                    mail = string.Concat(mail, "<table rules='all' border='10'><tr><th align='center'>Desde</th><th align='center'>Hasta</th><th align='center'>Reservado</th></tr>");
                    foreach (var l in objeto.detalles)
                    {
                        if (!string.IsNullOrEmpty(l.reserva))
                        {
                            if (l.reserva != "0")
                            {
                                validacantidad = validacantidad + Convert.ToInt32(l.reserva);
                                mail = string.Concat(mail, string.Format("<tr><td align='center'>{0}</td><td align='center'>{1}</td><td align='center'>{2}</td></tr>", l.desde, l.hasta, l.reserva));   
                            }
                        }
                    }
                    mail = string.Concat(mail, "</table>");
                }
                else
                {
                    mail = string.Concat(mail, "Hubo un problema y no se encontraron detalles que mostrar, comuniquese con CGSA");
                }
                //var car = new CSLSite.unitService.mailserviceSoapClient();
                //car.sendmail(mail, string.Format("{0};" + destinatarios, objeto.mail), objeto.usuario, token.Value);
                string error = string.Empty;
                //el mail del usuario logueado
                var user_email = sUser.email;
                var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                destinatarios = string.Format("{0};{1};{2};{3};CGSA-Consolidaciones@cgsa.com.ec", user_email, objeto.mail, destinatarios, correoBackUp);

                turnoConsolidacion.addMail(out error, destinatarios, "Se genero una reserva de turnos para consolidación,* Booking " + objeto.booking, mail, objeto.mail, objeto.usuario, objeto.idlinea, objeto.linea);
                if (!string.IsNullOrEmpty(error))
                {
                    jmsg.resultado = false;
                    jmsg.mensaje = error;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                return CslHelper.JsonNewResponse(true, true, "window.location='../cuenta/menu.aspx'", "Proceso exitoso en unos minutos recibirá una notificación via mail.");
            }
            catch (Exception ex)
            {
                var t = string.Format("Estimado usuario\\nOcurrió un problema durante su solicitud\\n{0}\\nPor favor intente lo siguiente salir y volver a entrar al sistema\\nSi esto no funciona envienos un correo con el mensaje de error y lo atenderemos de inmediato.\\nMuchas gracias", ex.Message.Replace(":", "").Replace("'", "").Replace("/", ""));
                return "{ \"resultado\":false, \"fluir\":false, \"mensaje\":\"" + t + "\" }";
            }
        }
    }
}