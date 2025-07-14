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
using CSLSite.app_start;

namespace CSLSite
{
    public partial class consultapro : System.Web.UI.Page
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
            var est = estado as bool?;
            if (!est.HasValue)
            {
                return "<span>sin estado!</span>";
            }
            if (est.Value)
            {
                return "<span class='azul' >Activo</span>";
            }
            else
            {
                return "<span class='red'  >Anulado</span>";
            }
        }
        public static string boton(object estado)
        {
            var est = estado as bool?;
            
            return  (est.HasValue && est.Value)  ? "ver":"xver";
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
                try
                {
                    var user = new usuario();

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }
                  user =  Page.getUserBySesion();

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
                    Int64 secuencia = 0;
                    var argumento = e.CommandArgument.ToString();
                    if (string.IsNullOrEmpty(argumento))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    var param = argumento.Split(';');
                    if (param.Length!=4)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    if (!Int64.TryParse(param[1],out secuencia))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

             
                    //boking , secuencia, proforma
                    var aisvs = ProformaHelper.AisvActivos(param[0]);
                    if (aisvs > 0)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerHtml = string.Format("El Booking {1} tiene {0} documentos AISV activos, no es posible anular esta proforma.<br/>Si aún desea continuar con el proceso, por favor ANULE todos los AISV amparados con el booking {1} ", aisvs, param[0]);
                        sinresultado.Visible = true;
                        return;
                    }

                    string vt = string.Empty;
                    //2017-->Anular liquidación senae
                    if (!string.IsNullOrEmpty(param[3]) && param[3].Length > 1)
                    {
                       //Aqui se debe validar que no estae pagada si esta pagada no se anula //
                        if (ProformaHelper.LiquidacionPagada(param[3]) > 0)
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerHtml = string.Format(@"El numero de liquidación {0} 
                            ya se encuentra en estado PAGADO, por este motivo no puede ser eliminada", param[3]);
                            sinresultado.Visible = true;
                            return;
                        }


                        if (!Proforma.BorrarLiquidacion(secuencia, user.loginname, param[3], out vt))
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = vt;
                            sinresultado.Visible = true;
                            return;
                        }
                    }
                   //anular proforma despues de liquidación
                    if (!Proforma.Borrar(secuencia, user.loginname, out vt))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = "No fúe posible anular este documento, es probable que la carga ya este ingresada o que el transporte ya este fuera de la terminal, confirme con planificación"; ;
                        sinresultado.Visible = true;
                        return;
                    }

                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerText = string.Format("La anulación de la proforma  No.{0} ha resultado exitosa.", param[2]);
                    sinresultado.Visible = true;
                    populate();
                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación de este documento, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consultapro", "Item_comand", "Hubo un error al anular", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }
        private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
           var table = new Catalogos.proformaListaDataTable();
           var ta = new CatalogosTableAdapters.proformaListaTableAdapter();
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
               var user = new usuario();
              

               user = Page.getUserBySesion();

               
               ta.ClearBeforeFill = true;
               ta.Fill(table,this.nbrboo.Value,desde,hasta, user.loginname);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise el numero de booking o las fechas";
                   sinresultado.Visible = true;
                   return;
               }

               //para que presente el numero de liquidacion si hay liquidacion
               foreach (var f in table)
               {
                   if (!f.IsliquidacionNull())
                   {
                       f.secuencia = f.liquidacion;
                   }
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
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta-proforma", "populate", "Hubo un error al buscar", t.loginname));
               sinresultado.Visible = true;
           }
           finally
           {
               ta.Dispose();
               table.Dispose();
           }
       }

        public static string setparametros (object boking, object secuencia, object proforma, object liquidacion )
        {
            if (boking == null || secuencia == null || proforma== null)
            {
                return "null";
            }

            return string.Format("{0};{1};{2};{3}",boking,secuencia,proforma,liquidacion);
        }
    }
}