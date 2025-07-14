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

namespace CSLSite
{
    public partial class replica : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            this.IsAllowAccess();
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
                this.booking.Text =Server.HtmlEncode(this.booking.Text);
                this.aisvn.Text =Server.HtmlEncode(this.aisvn.Text);
                this.cntrn.Text =Server.HtmlEncode(this.cntrn.Text);
               
                this.desded.Text = Server.HtmlEncode(this.desded.Text);
                this.desded.Text = Server.HtmlEncode(this.hastad.Text);
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
                        Session.Clear();
                        return;
                    }
                    populate();
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
        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            if (estado.ToString().ToLower() == "r")
            {
                return "<span>Registrado</span>";
            }
            if (estado.ToString().ToLower() == "a")
            {
                return "<span class='red' >Anulado</span>";
            }
            if (estado.ToString().ToLower() == "i")
            {
                return "<span class='azul' >Ingresado</span>";
            }
            if (estado.ToString().ToLower() == "s")
            {
                return "<span class='naranja' >Salida</span>";
            }
            return "<span>sin estado!</span>";
        }
        public static string boton(object estado)
        { 
          return  estado.ToString().ToLower() == "r" ? "ver":"xver";
        }
        public static string tipos(object tipo, object movi)
        {
            if (tipo == null || movi == null)
            {
                return "!error";
            }

            if (tipo.ToString().Trim().Length < 1 || movi.ToString().Trim().Length < 1)
            {
                return "!error";
            }

            if (movi.ToString().Trim() == "E")
            {
                if (tipo.ToString().Trim() == "C")
                {
                    return "Full";
                }
                else
                {
                    return "C. Suelta";
                }
            }
            else
            {
                return "Consolidación";
            }
        }
        public static string securetext(object number)
        {
            if (number == null || number.ToString().Length <= 2)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                //aqui replicar
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }
                    var user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    var xpars = e.CommandArgument.ToString().Split(';');
                    if (xpars.Length <= 0 || xpars.Length < 5)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    if (string.IsNullOrEmpty(xpars[5]) || xpars[5].ToLower() != "r")
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = "No se puede anular este AISV ya que su estado ha cambiado, es posible que la carga ya se encuentre patios.";
                        sinresultado.Visible = true;
                        return;
                    }

                    if (jAisvContainer.UnidadEstado(xpars[2]) > 1)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("No se puede replicar AISV {0} ya que la unidad {1}, se encuentra en patios", xpars[0], xpars[2]);
                        sinresultado.Visible = true;
                        return;
                    }
                  
                    //cancel advice si es unidad
                    if (!string.IsNullOrEmpty(xpars[2]))
                    {
                        if (xpars[2] == "null")
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal y no se encontró el contenedor. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El registro era de contenedor lleno pero no se encontró la unidad"), "consulta", "tablePagination_ItemCommand", "Contenedor fué nulo", user.loginname));
                            sinresultado.Visible = true;
                            return;
                        }

                        //SI ES CONTENEDOR O ES CONSOLIDACION
                        if (xpars[3].ToUpper().Contains("C") || xpars[4].ToUpper().Contains("C"))
                        {
                            //si hay unidad o contenedor.
                            if (!string.IsNullOrEmpty(xpars[2]))
                            {
                                if (jAisvContainer.ConfirmacionPreaviso(xpars[2].Trim()))
                                {
                                    sinresultado.Attributes["class"] = string.Empty;
                                    sinresultado.Attributes["class"] = "msg-critico";
                                    sinresultado.InnerText = string.Format("No se puede replicar el AISV: {0} ya que el Contenedor: {1}, cuenta con un preaviso", xpars[0], xpars[2]);
                                    sinresultado.Visible = true;
                                    return;
                                }
                            }
                        }
                          
                    }

                    //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                    var userk = new ObjectSesion();
                    userk.clase = "replicar"; 
                    userk.metodo = "tablePagination_ItemCommand";
                    userk.usuario = user.loginname;
                    

                    //aqui era el error.
                    userk.token = HttpContext.Current.Request.Cookies["token"].Value;

                    //REPLICAR XQ NO HAY TRABA.
                    CatalogosTableAdapters.replicador_detalleTableAdapter ta;
                    ta = new CatalogosTableAdapters.replicador_detalleTableAdapter();
                    Catalogos.replicador_detalleDataTable tb = new Catalogos.replicador_detalleDataTable();
                    ta.Fill(tb, xpars[0]);
                    if (tb.Rows.Count <= 0)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("No existen comandos N4 para el AISV:{0}, que se puedan replicar, debe proceder manualmente", xpars[0]);
                        sinresultado.Visible = true;
                        return;
                    }
                    var webService = new n4WebService();
                    var strinb = new StringBuilder();
                    string oerror = string.Empty;
                    var ss = new StringBuilder();

                   
                    strinb.Append("<ol style='margin:0;padding:0;' >");
                    bool huboerror = false;
                    foreach (Catalogos.replicador_detalleRow r in tb.Rows)
                    {
                        if (!r.IsxmlN4Null())
                        {
                            userk.transaccion = string.Format("{0}R",r.transaccion);
                            if (webService.InvokeN4Service(userk, r.xmlPortal, ref oerror, r.transaccion) > 1)
                            {
                                strinb.AppendFormat("<li>{0}</li>", oerror);
                                ss.AppendLine(oerror);
                                huboerror = true;
                            }
                        }
                    }
                     strinb.Append("</ol>");
                    if (huboerror)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerHtml = string.Format("<span>{0}</span><br/>{1}", "Los comandos fueron ejecutados, pero hubieron los siguientes problemas:", strinb.ToString());
                        sinresultado.Visible = true;
                        return;
                    }
                    dbconfig.UpdateAISV(xpars[0], user.loginname, huboerror? ss.ToString():"OK" );
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerText = string.Format("La replicación del AISV  No.{0} ha resultado exitosa.", xpars[0]);
                     sinresultado.Visible = true;
                    populate();
                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la replicación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al anular", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }
        protected string jsarguments(object aisv, object referencia, object unidad, object movimiento, object tipo, object estado)
        {
            return string.Format("{0};{1};{2};{3};{4};{5}",
                aisv != null ? aisv.ToString().Trim() : "0", 
                referencia != null ? referencia.ToString().Trim() : "na", 
                unidad != null ? unidad.ToString().Trim() : "null", 
                movimiento != null ? movimiento.ToString().Trim() : "x", 
                tipo != null ? tipo.ToString().Trim() : "x",estado);
            /*
             0 AISV, 1 REFERENCIA, 2 UNIDAD,3 MOVIMIENTO, 4 TIPO_AISV
             */
        }
       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
           var table = new Catalogos.replicadorDataTable();
           var ta = new CatalogosTableAdapters.replicadorTableAdapter();
           try
           {
               DateTime desde;
               DateTime hasta;
               if (!DateTime.TryParseExact(  desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                   sinresultado.Visible = true;
                   return;
               }
               if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                   sinresultado.Visible = true;
                   return;
               }
               if (desde.Year != hasta.Year)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-alerta";
                   this.sinresultado.InnerText = "El rango máximo de consulta es de 1 año, gracias por entender.";
                   sinresultado.Visible = true;
                   return;
               }
               TimeSpan ts = desde - hasta;
               // Difference in days.
               if (ts.Days > 30)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-alerta";
                   this.sinresultado.InnerText = "El rango máximo de consulta es de 1 mes, gracias por entender.";
                   sinresultado.Visible = true;
                   return;
               }
               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
                ta.Fill(table, aisvn.Text, cntrn.Text, booking.Text, desde, hasta);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la unidad, documento o no. aisv";
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
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "populate", "Hubo un error al buscar", t.loginname));
               sinresultado.Visible = true;
           }
           finally
           {
               ta.Dispose();
               table.Dispose();
           }
       }
    }
}