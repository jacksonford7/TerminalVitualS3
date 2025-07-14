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
using BillionEntidades;
using N4Ws.Entidad;
using PasePuerta;

namespace CSLSite.atraque
{
    public partial class consulta : System.Web.UI.Page
    {
        private static Int64? lm = -3;
        private string OError;
        private Cls_Sol_Estado objEstado = new Cls_Sol_Estado();

        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);

            this.UPBOTONES.Update();
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg_det.InnerHtml = Mensaje;

            OcultarLoading("1");


            this.UPMENSAJE.Update();
        }

        private void Ocultar_Mensaje()
        {

            
            this.banmsg_det.InnerText = string.Empty;
           
            this.banmsg_det.Visible = false;
            this.UPMENSAJE.Update();
            OcultarLoading("1");
     
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
               
                this.desded.Text = Server.HtmlEncode(this.desded.Text.Trim());
                this.hastad.Text = Server.HtmlEncode(this.hastad.Text.Trim());
                this.treferencia.Text = Server.HtmlEncode(this.treferencia.Text.Trim());
                this.timo.Text = Server.HtmlEncode(this.timo.Text.Trim());
                this.tnave.Text = Server.HtmlEncode(this.tnave.Text.Trim());
                this.teta.Text = Server.HtmlEncode(this.teta.Text.Trim());
                this.tetb.Text = Server.HtmlEncode(this.tetb.Text.Trim());

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
        public static string securetext(object number)
        {
            if (number == null || number.ToString().Length <= 2)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
           var table = new Catalogos.listarSolicitudDataTable();
           var ta = new CatalogosTableAdapters.listarSolicitudTableAdapter();
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

               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
               //todo el filtro del usuario
               ta.Fill(table, user.loginname, this.treferencia.Text.Trim(), this.timo.Text.Trim(),this.tmani.Text.Trim(), this.tnave.Text.Trim(), desde, hasta);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise los argumentos de búsqueda";
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

                    if (e.CommandArgument == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos");
                        sinresultado.Visible = true;
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                      
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null");
                        sinresultado.Visible = true;
                        return;

                    }

                    if (e.CommandName == "Ver")
                    {

                        var tabla = new Catalogos.obtenerSolicitudDataTable();
                        var ta = new CatalogosTableAdapters.obtenerSolicitudTableAdapter();
                        
                        ta.Fill(tabla, t.ToString());
                        if (tabla.Rows.Count <= 0)
                        {
                            var ex = new ApplicationException(string.Format("Tabla de consulta vacía"));

                            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);

                            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                            sinresultado.Visible = true;

                            return;

                           
                        }
                        var fila = tabla.FirstOrDefault();

                        this.tvIn.Text = fila.viajeIn;
                        this.tvOu.Text = fila.viajeOut;
                        this.tmrn.Text = fila.imrn;
                        this.tdae.Text = fila.emrn;

                        this.teta_ant.Text = fila.etb;
                        this.tetb_ant.Text = fila.eta;
                        this.thoras.Text = fila.uso;
                        this.Txtetd.Text = fila.ets;

                        this.teta.Text = string.Empty;
                        this.tetb.Text = string.Empty;

                        this.RdbNaveContainera.Enabled = false;
                        this.RdbMixta.Enabled = false;

                        this.fecembarque.Text = string.Empty;
                        this.fecbanano.Text = string.Empty;

                        this.TxtContador.Text = fila.contador.ToString();

                        if (fila.tipooperacion.Equals("Operación mixta"))
                        {
                            this.RdbNaveContainera.Checked = false;
                            this.RdbMixta.Checked = true;

                            this.fecembarque.Text = string.Empty;
                            this.fecbanano.Text = string.Empty;

                            this.fecembarque.Attributes.Remove("disabled");
                            this.fecbanano.Attributes.Remove("disabled");
                        }
                        else
                        {
                            this.RdbNaveContainera.Checked = true;
                            this.RdbMixta.Checked = false;

                            this.fecembarque.Text = string.Empty;
                            this.fecbanano.Text = string.Empty;


                            this.fecbanano.Attributes.Add("disabled", "disabled");
                            this.fecembarque.Attributes.Add("disabled", "disabled");

                           
                        }


                        this.UPCHECK.Update();
                       
                        CultureInfo enUS = new CultureInfo("en-US");

                        DateTime fecembarque = new DateTime();

                        if (!DateTime.TryParseExact(fila.embarqueplanificado, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.None, out fecembarque))
                        {
                            this.Txtfecembarque.Text = string.Empty;
                           
                        }
                        else
                        {
                            this.Txtfecembarque.Text = fecembarque.ToString("yyyy-MM-dd HH:mm");
                          
                        }

                      

                        DateTime fecbanano = new DateTime();

                        if (!DateTime.TryParseExact(fila.cutoffbbk, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.None, out fecbanano))
                        {
                            this.Txtfecbanano.Text = string.Empty;
                        }
                        else
                        {
                            this.Txtfecbanano.Text = fecbanano.ToString("yyyy-MM-dd HH:mm");
                        }

                        UPBANANO.Update();
                        UPFECHAEMBARQUE.Update();

                        DateTime eta = new DateTime();

                      

                        Ocultar_Mensaje();
                        //etb
                        if (!DateTime.TryParseExact(fila.ets, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out eta))
                        {
                            this.BtnGrabar.Attributes.Add("disabled", "disabled");
                            this.tets.Text = string.Empty;
                            this.tetscambio.Text = string.Empty;
                        }
                        else
                        {
                            this.tets.Text = eta.ToString("yyyy-MM-dd HH:mm");
                            this.tetscambio.Text = eta.ToString("yyyy-MM-dd HH:mm");

                            DateTime FecActual = DateTime.Now;
                            TimeSpan Diferencia = eta.Subtract(FecActual);
                            double Horas = Diferencia.TotalHours;

                             DateTime new_etb = eta.AddHours(-24);
                            if (FecActual > new_etb)
                            {
                                this.Txthoras.Text = "36";
                                //this.BtnGrabar.Attributes.Add("disabled", "disabled");
                                this.Mostrar_Mensaje(string.Format("<b>Informativo! Tiene hasta 24 horas, para actualizar la solicitud de atraque, el plazo supero las horas de modificación, no podrá modificar el (ETA, ETB). <br/> {0} </b>",
                                    (fila.contador == 2 ? "Se cumplió con el límite de intentos de actualización del campo 29. Fecha de embarque de contenedores planificado" : "")));
                            }
                            else
                            {
                                this.Txthoras.Text = "0";
                                this.BtnGrabar.Attributes.Remove("disabled");
                            }
                            //if (Horas > 24)
                            //{
                            //    this.Txthoras.Text = "0";
                            //    this.BtnGrabar.Attributes.Remove("disabled");

                            //}
                            //else
                            //{
                            //    this.Txthoras.Text = "36";
                            //    //this.BtnGrabar.Attributes.Add("disabled", "disabled");
                            //    this.Mostrar_Mensaje(string.Format("<b>Informativo! Tiene hasta 24 horas, para actualizar la solicitud de atraque, el plazo supero las horas de modificación, no podrá modificar el (ETA, ETB). <br/> {0} </b>",
                            //        (fila.contador==2 ? "Se cumplió con el límite de intentos de actualización del campo 29. Fecha de embarque de contenedores planificado" : "")));
                            //}

                            //quitar en produccion
                            //this.BtnGrabar.Attributes.Remove("disabled");

                        }


                        DateTime fecetd = new DateTime();

                        if (!DateTime.TryParseExact(fila.ets, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.None, out fecetd))
                        {
                            this.Txtetd.Text = string.Empty;
                        }
                        else
                        {
                            this.Txtetd.Text = fecetd.ToString("yyyy-MM-dd HH:mm");
                        }

                        //valida estado  
                        objEstado = new Cls_Sol_Estado();
                        objEstado.REFERENCIA = fila.referencia;

                        if (!objEstado.PopulateMyData(out OError))
                        {

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                            sinresultado.Visible = true;
                            return;
                        }
                        else
                        {
                            this.estado.Value = objEstado.ESTADO;

                            if (objEstado.ESTADO.Equals("40WORKING") && fila.tipooperacion.Equals("Operación mixta"))
                            {
                                this.estado.Value = objEstado.ESTADO;
                            }
                            else
                            {
                                this.estado.Value = "NOAPLICA";
                            }
                        }




                        this.UPOCULTOS.Update();

                        //habilitar en produccion
                        //ya tiene actualización
                        //if (!string.IsNullOrEmpty(fila.cae_manifiestoImpo_ant))
                        //{
                        //    this.BtnGrabar.Attributes.Add("disabled", "disabled");
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! Ya se ha realizado una actualización sobre la solicitud de atraque, no podrá modificar la misma.</b>"));
                        //}


                        //this.tets_ant.Text = fila.ets;

                        this.Titulo.InnerText = string.Format("ACTUALIZAR SOLICITUD DE ATRAQUE - REFERENCIA: {0}", fila.referencia);
                        this.REFERENCIA.Value = fila.referencia;

                        this.UPVIAJE.Update();
                        this.UPSENAE.Update();
                        this.UPFECHAS.Update();
                        this.UPFECHAS2.Update();
  
                        this.UPHORAS.Update();
                        this.UPTITULO.Update();
                        this.UPBOTONES.Update();
                        this.UPFECHAETS.Update();



                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));


                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                    sinresultado.Visible = true;

                   
                }
            }

        }


        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                try
                {

                    DateTime  fechadesde = new DateTime();
                    DateTime fechahasta = new DateTime();
                    DateTime cutoffbbk = new DateTime();
                    DateTime embarqueplanificado = new DateTime();
                    DateTime cutoffbbk_ant = new DateTime();
                    DateTime embarqueplanificado_ant = new DateTime();
                    DateTime etb = new DateTime();
                    DateTime etd = new DateTime();

                    CultureInfo enUS = new CultureInfo("en-US");

                    if (string.IsNullOrEmpty(this.REFERENCIA.Value))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Debe seleccionar la referencia, para actualizar.</b>"));
                        return;
                    }


                    if (string.IsNullOrEmpty(this.tvIn.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Debe ingresar el campo 11. Viaje entrante.</b>"));
                        this.tvIn.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.tvOu.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Debe ingresar el campo 12. Número saliente.</b>"));    
                        this.tvOu.Focus();
                      
                        return;
                    }

                    if (string.IsNullOrEmpty(this.tmrn.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Debe ingresar el campo 20. Manifiesto de Importación.</b>"));   
                        this.tmrn.Focus();
                       
                        return;
                    }

                    if (string.IsNullOrEmpty(this.tdae.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Debe ingresar el campo 21. Manifiesto de Exportación.</b>"));
                       
                        this.tdae.Focus();

                        return;
                    }

                  
                    if (string.IsNullOrEmpty(this.thoras.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Debe ingresar el campo 26. Número de horas uso de muelle.</b>"));

                        this.thoras.Focus();

                        return;
                    }

                    if (!string.IsNullOrEmpty(teta.Text))
                    {
                        if (!DateTime.TryParseExact(teta.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechadesde))
                        {
        
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", teta.Text));
                            this.teta.Focus();

                            return;

                        }
                        else
                        {

                            
                            //DateTime FecActual = DateTime.Now;
                            //TimeSpan Diferencia = fechadesde.Subtract(FecActual);
                            //double nHoras = Diferencia.TotalHours;

                            //if (nHoras >= 24)
                            //{

                            //}
                            //else
                            //{
                            //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>La [Fecha estimada de Atraque muelle CGSA:{0}] debe ser mayor a 24H desde la fecha actual: {1}", teta.Text, FecActual.ToString("dd/MM/yyyy HH:mm")));

                            //    this.teta.Focus();

                            //    return;
                            //}


                        }


                    }
                    else
                    {
                        if (!DateTime.TryParseExact(this.teta_ant.Text, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", teta.Text));

                            this.teta.Focus();

                            return;

                        }
                    }

                    if (!string.IsNullOrEmpty(tetb.Text))
                    {
                        if (!DateTime.TryParseExact(tetb.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechahasta))
                        {

                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la  [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", teta.Text));

                            this.tetb.Focus();

                            return;

                        }
                    }
                    else
                    {
                        if (!DateTime.TryParseExact(tetb_ant.Text, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out fechahasta))
                        {

                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la  [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", teta.Text));

                            this.tetb.Focus();

                            return;

                        }
                    }


                    int Horas = 0;
                    if (!int.TryParse(this.thoras.Text,out Horas))
                    {

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! Debe ingresar el campo 26. Número de horas uso de muelle.</b>"));

                        this.thoras.Focus();

                        return;

                    }

                    //valido que el ETB no sea menor al ETA
                    if (!string.IsNullOrEmpty(tetb.Text))
                    {
                        if (fechahasta < fechadesde)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el ETB {0}, el mismo no puede ser menor al ETA {1}.", fechahasta.ToString("dd/MM/yyyy HH:mm"),
                                  fechadesde.ToString("dd/MM/yyyy HH:mm")));
                            this.tetb.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(teta.Text))
                    {
                        if (fechahasta < fechadesde)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el ETB {0}, el mismo no puede ser menor al ETA {1}.", fechahasta.ToString("dd/MM/yyyy HH:mm"),
                                  fechadesde.ToString("dd/MM/yyyy HH:mm")));
                            this.tetb.Focus();
                            return;
                        }
                    }



                    //saco el etd anterior.
                    etd = fechahasta.AddHours(Horas);

                    //valido que el cut off de bnn no sea mayor al ETD
                    if (!string.IsNullOrEmpty(fecbanano.Text))
                    {
                        if (!DateTime.TryParseExact(fecbanano.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out cutoffbbk))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de [30. Cutoff  (BBK)] debe ser: dia/Mes/Anio {0}", fecbanano.Text));
                            this.fecbanano.Focus();
                            return;
                        }

                        if (cutoffbbk > etd)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el [30. Cutoff  (BBK)] {0}, el mismo no puede ser mayor al ETD {1}.", cutoffbbk.ToString("dd/MM/yyyy HH:mm"), etd.ToString("dd/MM/yyyy HH:mm")
                                 ));
                            this.fecbanano.Focus();
                            return;
                        }

                    }

                    //valido que la fecha de embaruqe no sea mayor que el ETD
                    if (!string.IsNullOrEmpty(fecembarque.Text))
                    {
                        if (!DateTime.TryParseExact(fecembarque.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out embarqueplanificado))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de [29. Fecha de embarque de contenedores planificado] debe ser: dia/Mes/Anio {0}", fecembarque.Text));
                            this.fecembarque.Focus();
                            return;
                        }

                        if (embarqueplanificado > etd)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el [29. Fecha de embarque de contenedores planificado] {0}, el mismo no puede ser mayor al ETD {1}.", embarqueplanificado.ToString("dd/MM/yyyy HH:mm"),
                                etd.ToString("dd/MM/yyyy HH:mm")
                                 ));
                            this.fecembarque.Focus();
                            return;
                        }

                        //valida fecha de actualizacion no sea mayor a 24 horas antes del ETD
                        //DateTime new_etb = etd.AddHours(-24);

                        //if (embarqueplanificado > new_etb)
                        //{
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el  [29. Fecha de embarque de contenedores planificado] {0} la fecha ingresada debe ser menor a 24 horas antes del ETD {1}, fecha sugerida: {2}", embarqueplanificado.ToString("dd/MM/yyyy HH:mm"), etd.ToString("dd/MM/yyyy HH:mm"), new_etb.ToString("dd/MM/yyyy HH:mm")));
                        //    this.fecembarque.Focus();
                        //    return;
                        //}
                    }




                    //validacion de fechas y horas
                    string cutoffbbk_banano = string.Empty;

                    //nuevas validaciones cutoffbbk (campo 30)
                    if (!string.IsNullOrEmpty(fecbanano.Text))
                    {


                        if (!DateTime.TryParseExact(fecbanano.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out cutoffbbk))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de [30. Cutoff  (BBK)] debe ser: dia/Mes/Anio {0}", fecbanano.Text));
                            this.fecbanano.Focus();
                            return;
                        }
                        //datos cutoffbbk anterior
                        if (!DateTime.TryParseExact(this.Txtfecbanano.Text, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out cutoffbbk_ant))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de [30. Cutoff  (BBK) anterior] debe ser: dia/Mes/Anio {0}", this.Txtfecbanano.Text));
                            return;
                        }
                      
                        //etd = fechahasta.AddHours(Horas);
                        //if (cutoffbbk > etd.AddHours(-12))
                        //{
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el cut off (BBK) {0} Supera las 12 horas antes del ETD: Fecha nueva estimada de zarpe {1}, fecha sugererida {2}", cutoffbbk.ToString("dd/MM/yyyy HH:mm"), 
                        //        etd.ToString("dd/MM/yyyy HH:mm"), etd.AddHours(-12).ToString("dd/MM/yyyy HH:mm")));
                        //    this.fecbanano.Focus();
                        //    return;
                        //}

                        if (!string.IsNullOrEmpty(tetb.Text))
                        {
                            if (!DateTime.TryParseExact(tetb.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out etb))
                            {

                                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la  [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", teta.Text));
                                this.tetb.Focus();
                                return;

                            }
                        }
                        else
                        {
                            if (!DateTime.TryParseExact(tetb_ant.Text, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out etb))
                            {

                                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la  [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", tetb.Text));
                                this.tetb.Focus();
                                return;
                            }

                        }
                        //validacion 24 horas antes
                        // DateTime new_etb = etb.AddHours(-24);
                        //DateTime new_etb = etd.AddHours(-24);
                        //if (cutoffbbk > new_etb)
                        //{
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el cut off (BBK) {0} la fecha ingresada debe ser menor a 24 horas antes del ETD {1}, fecha sugerida: {2}", cutoffbbk.ToString("dd/MM/yyyy HH:mm"), etd.ToString("dd/MM/yyyy HH:mm"), new_etb.ToString("dd/MM/yyyy HH:mm")));
                        //    this.fecbanano.Focus();
                        //    return;
                        //}

                        //if (cutoffbbk > etd)
                        //{
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el cut off (BBK) {0} la fecha ingresada no puede ser mayor al ETD {1}", cutoffbbk.ToString("dd/MM/yyyy HH:mm"), etd.ToString("dd/MM/yyyy HH:mm")));
                        //    this.fecbanano.Focus();
                        //    return;
                        //}

                        cutoffbbk_banano = cutoffbbk.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Txtfecbanano.Text))
                        {
                            cutoffbbk_banano = Txtfecbanano.Text;
                        }
                        else
                        {
                            cutoffbbk_banano = null;
                        }
                     

                    }
                    //nuevas validaciones Fecha de embarque de contenedores planificado (campo 29)
                    string fecembarque_banano = string.Empty;
                    if (!string.IsNullOrEmpty(fecembarque.Text))
                    {

                        if (!DateTime.TryParseExact(fecembarque.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out embarqueplanificado))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de [29. Fecha de embarque de contenedores planificado] debe ser: dia/Mes/Anio {0}", fecembarque.Text));
                            this.fecembarque.Focus();
                            return;
                        }
                        //datos embarque anterior
                        if (!DateTime.TryParseExact(this.Txtfecembarque.Text, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out embarqueplanificado_ant))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de [29. Fecha de embarque de contenedores planificado] debe ser: dia/Mes/Anio {0}", this.Txtfecembarque.Text));
                            return;
                        }
                        

                        //if (embarqueplanificado > etd.AddHours(-12))
                        //{
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el  [29. Fecha de embarque de contenedores planificado] {0} Supera las 12 horas antes del ETD: Fecha nueva estimada de zarpe {1}, fecha sugererida {2}", embarqueplanificado.ToString("dd/MM/yyyy HH:mm"), etd.ToString("dd/MM/yyyy HH:mm"), etd.AddHours(-12).ToString("dd/MM/yyyy HH:mm") ));
                        //    this.fecembarque.Focus();
                        //    return;
                        //}

                        //validacion etb
                        if (!string.IsNullOrEmpty(tetb.Text))
                        {
                            if (!DateTime.TryParseExact(tetb.Text, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out etb))
                            {

                                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la  [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", teta.Text));
                                this.tetb.Focus();
                                return;

                            }
                        }
                        else
                        {
                            if (!DateTime.TryParseExact(tetb_ant.Text, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out etb))
                            {

                                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la  [Fecha estimada de Atraque muelle CGSA] debe ser: dia/Mes/Anio {0}", tetb.Text));
                                this.tetb.Focus();
                                return;
                            }

                        }
                        //validacion 24 horas antes
                        //DateTime new_etb = etb.AddHours(-24);
                        //DateTime new_etb = etd.AddHours(-24);

                        //if (embarqueplanificado > new_etb)
                        //{
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el  [29. Fecha de embarque de contenedores planificado] {0} la fecha ingresada debe ser menor a 24 horas antes del ETD {1}, fecha sugerida: {2}", embarqueplanificado.ToString("dd/MM/yyyy HH:mm"), etd.ToString("dd/MM/yyyy HH:mm"), new_etb.ToString("dd/MM/yyyy HH:mm")));
                        //    this.fecbanano.Focus();
                        //    return;
                        //}

                        //if (embarqueplanificado > etd)
                        //{
                        //    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> No se puede actualizar el [29. Fecha de embarque de contenedores planificado] {0} la fecha ingresada no puede ser mayor al ETD {1}", cutoffbbk.ToString("dd/MM/yyyy HH:mm"), etd.ToString("dd/MM/yyyy HH:mm")));
                        //    this.fecbanano.Focus();
                        //    return;
                        //}

                        fecembarque_banano = embarqueplanificado.ToString("yyyy-MM-dd HH:mm");

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Txtfecembarque.Text))
                        {
                            fecembarque_banano = Txtfecembarque.Text;
                        }
                        else
                        {
                            fecembarque_banano = null;
                        }
                    }



                    string TIPO = (this.RdbNaveContainera.Checked ? "C" : (this.RdbMixta.Checked ? "M" : "C"));

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    bool Actualizar = false;
                    var ResultadoEvt = Vessel_Visit.Actualizar_VesselVisit(this.REFERENCIA.Value, fechahasta.ToString("yyyy-MM-dd HH:mm"), this.thoras.Text.Trim(),
                     fechadesde.ToString("yyyy-MM-dd HH:mm")  , this.tmrn.Text.Trim(), this.tdae.Text.Trim(), this.tvIn.Text.Trim(), this.tvOu.Text.Trim(), ClsUsuario.loginname.Trim(), 
                     TIPO, cutoffbbk_banano, fecembarque_banano);

                    if (ResultadoEvt.Exitoso)
                    {
                        Actualizar = true;
                    }
                    else
                    {
                        Actualizar = false;

                        this.Mostrar_Mensaje(string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo actualizar Vessel visit: {0}, Existen los siguientes problemas: {1} </b>", ResultadoEvt.MensajeInformacion, ResultadoEvt.MensajeProblema));
                        return;

                    }

                    if (Actualizar)
                    {
                        Cls_Sol_Atraque objSol = new Cls_Sol_Atraque();
                        objSol.referencia = this.REFERENCIA.Value;
                        objSol.cae_manifiestoImpo_ant = tmrn.Text.Trim();
                        objSol.cae_manifiestoExpo_ant = tdae.Text.Trim();
                        objSol.nave_codigoViajeIn_ant = tvIn.Text.Trim();
                        objSol.nave_codigoViajeOut_ant = tvOu.Text.Trim();

                        if (string.IsNullOrEmpty(this.teta.Text))
                        {
                            objSol.veo_eta_ant = null;
                        }
                        else
                        {
                            objSol.veo_eta_ant = fechadesde;
                        }

                        if (string.IsNullOrEmpty(this.tetb.Text))
                        {
                            objSol.veo_ets_ant = null;
                        }
                        else
                        {
                            objSol.veo_ets_ant = fechahasta;
                        }

                        objSol.veo_horasUsoMuelle_ant = Horas;

                        if (string.IsNullOrEmpty(this.fecbanano.Text))
                        {
                            objSol.cutoffbbk_ant = null;
                        }
                        else
                        {
                            objSol.cutoffbbk_ant = cutoffbbk.ToString("yyyy/MM/dd HH:mm") ;
                        }

                        if (string.IsNullOrEmpty(this.fecembarque.Text))
                        {
                            objSol.embarqueplanificado_ant = null;
                        }
                        else
                        {
                            objSol.embarqueplanificado_ant = embarqueplanificado.ToString("yyyy/MM/dd HH:mm");
                        }


                        string v_mensaje = string.Empty;

                        objSol.Update(out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo actualizar Vessel visit: {0}, Existen los siguientes problemas: {1} </b>", this.REFERENCIA.Value, v_mensaje));
                            return;

                        }
                        else
                        {
                            this.TxtContador.Text = "0";
                            this.Txthoras.Text = "0";
                            this.UPOCULTOS.Update();

                            populate();

                            this.Mostrar_Mensaje(string.Format("<b>Informativo! Se procedió con la actualización de la visita de la referencia {0}, con éxito. </b>", this.REFERENCIA.Value));

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            this.sinresultado.InnerText = string.Format("<b>Informativo! Se procedió con la actualización de la visita de la referencia {0}, con éxito. </b>", this.REFERENCIA.Value);
                            sinresultado.Visible = true;


                            this.BtnGrabar.Attributes.Add("disabled", "disabled");
                            this.UPBOTONES.Update();

                          
                        }
                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Actualizar Vessel Visit", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));


                    this.Mostrar_Mensaje(OError);
                    

                }


            }
        }
    }
}