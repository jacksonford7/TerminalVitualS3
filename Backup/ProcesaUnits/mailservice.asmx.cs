using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Threading.Tasks;
using System.Threading;
using ConectorN4;
using System.Text;

namespace ProcesaUnits
{
    /// <summary>
    /// Servicio que toma las unidades las contrasta contra N4, guarda en la base y envía el mail
    /// Va ser interno solo este server puede acceder 
    /// </summary>
    [WebService(Namespace = "http://www.cgsa.com.ec/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class mailservice : System.Web.Services.WebService
    {
        [WebMethod(EnableSession=true,Description="Permite preavisar unidades y enviar mail")]
        public void preavisar(HashSet<unidad> containers, string usuario, string mail,string token, string freigthKind)
        {
            try
            {
                if (containers.Count <= 0){ return;}
                //preparar la tarea para ue se haga en background
                CancellationToken tks = new CancellationToken();
                ParallelOptions parOpts = new ParallelOptions();
                parOpts.CancellationToken = tks;
                parOpts.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;
                parOpts.TaskScheduler = TaskScheduler.Current;

                TaskCreationOptions tco = new TaskCreationOptions();
                tco = TaskCreationOptions.PreferFairness;
                Task task = null;
                task = Task.Factory.StartNew(() =>
                {
                    var servicio = new n4WebService();
                    //por cada registro crear una unit
                    int u =0;
                    var sb = string.Empty;
                   foreach(var item in containers)
                   {
                       try
                       {
                           item.preadvice = false;
                           //Por cada registro comprobar que esté en N4 si/no.
                           item.exist = DataHelper.UnitIsCreated(item.unidadID);
                           var xunit = string.Empty;
                           xunit = "<argo:snx xmlns:argo=\"http://www.navis.com/argo\" >";
                           xunit = string.Concat(xunit, string.Format("<unit id=\"{0}\" category=\"EXPORT\"  transit-state=\"INBOUND\" freight-kind=\"{3}\" line=\"{1}\"><booking id=\"{2}\" />",item.unidadID,item.lineaID,item.bokingID,freigthKind));
                         //  xunit = string.Concat(xunit, string.Format("<handling  remark=\"Unidad preavisada por:{0} (preaviso de vacíos) desde CSL\" />",usuario));
                           xunit = string.Concat(xunit, string.Format("<contents  shipper-id=\"{0}\" />",item.shiper));
                           if (!item.exist)
                           {
                               xunit = string.Concat(xunit, string.Format("<equipment  class=\"CTR\"   role=\"PRIMARY\"  eqid=\"{0}\"  type=\"{1}\" >", item.unidadID,item.uniISO));
                               //NUEVO CREARLE UN REEFER AL EQUIPO
                               if (!string.IsNullOrEmpty(item.reefer) && item.reefer.ToUpper().Contains("Y"))
                               {
                                   xunit = string.Concat(xunit, "<reefer is-controlled-atmosphere=\"N\" is-starvent=\"N\" is-super-freeze=\"N\" is-temperature-controlled=\"Y\" rfr-type=\"INTEG_AIR\"	/>");
                               }
                               xunit = string.Concat(xunit, string.Format( "<ownership owner=\"{0}\" operator=\"{0}\" />", item.lineaID));
                               xunit = string.Concat(xunit, "</equipment>");
                           }
                           if (!string.IsNullOrEmpty(item.imo) && item.imo !="0" && freigthKind!="MTY")
                           {
                               xunit = string.Concat(xunit, string.Format("<hazards><hazard imdg=\"{0}\" ltd-qty-flag=\"N\" marine-pollutants=\"N\"></hazard></hazards>", item.imo));
                           }
                              //LE PONE LOS PUESRTOS
                               var pod = string.IsNullOrEmpty(item.pod) ?  "NA" : item.pod.Trim().ToUpper();
                               var pod1= string.IsNullOrEmpty(item.pod1) ? "NA" : item.pod1.Trim().ToUpper();

                               if (!pod.Contains("NA") && !pod1.Contains("NA"))
                               {
                                   xunit = string.Concat(xunit, string.Format("<routing pol=\"ECGYE\" pod-1=\"{1}\"  pod-2=\"{2}\" ><carrier facility=\"GYE\" mode=\"TRUCK\" direction=\"IB\" qualifier=\"DECLARED\" /> <carrier facility=\"GYE\" mode=\"TRUCK\" direction=\"IB\" qualifier=\"ACTUAL\" /><carrier id=\"{0}\" facility=\"GYE\" mode=\"VESSEL\" direction=\"OB\" qualifier=\"DECLARED\" />   <carrier id=\"{0}\" facility=\"GYE\" mode=\"VESSEL\" direction=\"OB\" qualifier=\"ACTUAL\" /></routing >", item.referencia, pod, pod1));
                               }
                               else 
                               {
                                   if (pod == "NA")
                                   {
                                       pod = pod1;
                                   }
                                   if (pod != "NA")
                                   {
                                       xunit = string.Concat(xunit, string.Format("<routing pol=\"ECGYE\" pod-1=\"{1}\" ><carrier facility=\"GYE\" mode=\"TRUCK\" direction=\"IB\" qualifier=\"DECLARED\" /> <carrier facility=\"GYE\" mode=\"TRUCK\" direction=\"IB\" qualifier=\"ACTUAL\" /><carrier id=\"{0}\" facility=\"GYE\" mode=\"VESSEL\" direction=\"OB\" qualifier=\"DECLARED\" />   <carrier id=\"{0}\" facility=\"GYE\" mode=\"VESSEL\" direction=\"OB\" qualifier=\"ACTUAL\" /></routing >", item.referencia, pod));
                                   }
                                   else
                                   {
                                       xunit = string.Concat(xunit, string.Format("<routing pol=\"ECGYE\" ><carrier facility=\"GYE\" mode=\"TRUCK\" direction=\"IB\" qualifier=\"DECLARED\" /> <carrier facility=\"GYE\" mode=\"TRUCK\" direction=\"IB\" qualifier=\"ACTUAL\" /><carrier id=\"{0}\" facility=\"GYE\" mode=\"VESSEL\" direction=\"OB\" qualifier=\"DECLARED\" />   <carrier id=\"{0}\" facility=\"GYE\" mode=\"VESSEL\" direction=\"OB\" qualifier=\"ACTUAL\" /></routing >", item.referencia));
                                   }   
                               }
                               //NUEVO AISV USUARIO
                               if (!string.IsNullOrEmpty(usuario))
                               {
                                   xunit = string.Concat(xunit, string.Format(" <handling  remark=\"Unidad preavisada desde CSL (Masivo) por el usuario {0}\" />", usuario));
                               }
                               //NUEVO AISV NUMERO
                               xunit = string.Concat(xunit, string.Format("<unit-flex  unit-flex-2=\"AISV_{0}\"  />",freigthKind));
                              //NUEVO CARACTERISTICAS DEL REEFER
                               if (!string.IsNullOrEmpty(item.reefer) && item.reefer.ToUpper().Contains("Y"))
                               {
                                   if (!string.IsNullOrEmpty(item.temp))
                                   {
                                       xunit = string.Concat(xunit, string.Format("<reefer  temp-reqd-c=\"{0}\" temp-min-c=\"{0}\" temp-max-c=\"{0}\" temp-display-unit=\"C\"    extended-time-monitors=\"N\"   is-power=\"N\" wanted-is-power=\"Y\"   /> ", item.temp));         
                                   }
                                   if (!string.IsNullOrEmpty(item.temp)  && !string.IsNullOrEmpty(item.hume))
                                   {
                                       xunit = string.Concat(xunit, string.Format("<reefer  temp-reqd-c=\"{0}\" temp-min-c=\"{0}\" temp-max-c=\"{0}\" temp-display-unit=\"C\" humidity-pct=\"{1}\"   extended-time-monitors=\"N\"   is-power=\"N\" wanted-is-power=\"Y\"   /> ", item.temp,item.hume));
                                   }
                                   if (!string.IsNullOrEmpty(item.temp) && !string.IsNullOrEmpty(item.hume) && !string.IsNullOrEmpty(item.vent))
                                   {
                                       xunit = string.Concat(xunit, string.Format("<reefer  temp-reqd-c=\"{0}\" temp-min-c=\"{0}\" temp-max-c=\"{0}\" temp-display-unit=\"C\" humidity-pct=\"{1}\"  vent-required-value=\"{2}\"  vent-required-unit=\"{3}\" extended-time-monitors=\"N\"   is-power=\"N\" wanted-is-power=\"Y\"   /> ", item.temp, item.hume, item.vent, item.ventU));         
                                   }
                               }
                              xunit = string.Concat(xunit, "</unit></argo:snx>");
                          //avisar la unidad
                           var x = new ObjectSesion();
                           x.clase = "mailservice"; x.metodo = "preavisar(ws)";
                           x.token = token; x.usuario = usuario;
                           x.transaccion = "Preavisar vacíos";
                           if (servicio.InvokeN4Service(x, xunit, ref sb, (++u).ToString()) < 2)
                           {
                               item.preadvice = true;
                           }
                           item.mensaje = sb;
                       }
                       catch(Exception ex)
                       {
                           item.preadvice = false;
                           item.mensaje = ex.Message;
                          // csl_log.log_csl.save_log<Exception>(ex, "mailservice", "preavisar_for", usuario + ':' + token + ':' + mail, usuario+"-"+item.unidadID);
                           continue; 
                       }
                     }
                   //despues de actualizar cada unidad
                   var xmail = new StringBuilder();
                   xmail.Append("<html>");
                   xmail.Append("<style>*{ font-size:1em; vertical-aling:top;}  h3{color:Gray; background-color:yellow;} table > th{ background-color:#CCC; text-aling:center;}  table{width:100%; vertical-aling:top; } table td,table th {border:1px solid #CCC;vertical-aling:top;} div{margin:0; padding:0; overflow:auto;max-height:250px; } br{margin:1px;} p{ margin:1px; padding:0px;}</style>");
                   xmail .Append( string.Format("Estimado/a: {0}<br/>",usuario));
                   xmail  .Append("<h3>Este es un aviso del Centro de servicios en línea de Contecon S.A, para comunicarle lo siguiente</h3>");
                   xmail.Append("<p>Resultados del procesamiento de las unidades vacías para preaviso:</p>");
                   xmail.Append("<table cellspacing=\"2px\" cellpadding=\"1px\">");
                   xmail.Append("<tr><th><strong>Unidades preavisadas</strong></th><th><strong>Unidades con problemas</strong></th></tr>");
                   xmail.Append("<tr>");
                    //celda de oks
                   xmail.Append("<td><div><table><tr>Contenedor<th></th><th>ISO</th></tr>");
                   int number = 0;
                   StringBuilder xmlinsert = new StringBuilder();
                   xmlinsert.Append("<CSL>");
                   var xnumber = 0;
                   foreach (var c in containers)
                   {
                       if (c.preadvice)
                       {
                           xmail.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>",c.unidadID,c.uniISO);
                           xmlinsert.AppendFormat("<CSL_P_Mant_cslContenedorVacio CnvContenedor=\"{0}\" CnvBooking=\"{1}\" CnvUsuario=\"{2}\" flag=\"I\"  />",c.unidadID,c.bokingID,usuario);
                           number++;
                           xnumber++;
                       }
                   }
                   xmlinsert.Append("</CSL>");
                   xmail.AppendFormat("</table></div>Total preavisos:{0}</td>",number);
                    //celda de malos
                   xmail.Append("<td><div><table><tr>Contenedor<th></th><th>ISO(Booking)</th><th>Mensaje</th></tr>");
                   number = 0;
                   foreach (var c in containers)
                   {
                       if (!c.preadvice)
                       {
                           xmail.AppendFormat("<tr><td>{0}</td><td>{2}</td><td>{1}</td></tr>", c.unidadID, c.mensaje,c.uniISO);
                           number++;
                       }
                   }
                   xmail.AppendFormat("</table></div>Total fallidos:{0}</td>", number);
                   xmail.Append("</tr>");
                   xmail.Append("</table>");
                   xmail.Append("</html>");
                   csl_log.log_csl.mailSenderDB(xmail.ToString(), 1, mail, usuario);
                   if (xnumber > 0)
                   {
                       DataHelper.InsertUnits(xmlinsert.ToString());
                   }
                }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);
                task.ContinueWith((a) => { a.Dispose(); });
            }
            catch (Exception e)
            {
                csl_log.log_csl.save_log<Exception>(e, "mailservice", "preavisar", usuario + ':' + token + ':' + mail, usuario);
            }
        }

        [WebMethod(EnableSession = true, Description = "Permite cancelar los preavisos y enviar mail")]
        public void cancelar(HashSet<unidad> containers, string usuario, string mail ,string token)
        {
            try
            {
                if (containers.Count <= 0) { return; }
                //preparar la tarea para ue se haga en background
                CancellationToken tks = new CancellationToken();
                ParallelOptions parOpts = new ParallelOptions();
                parOpts.CancellationToken = tks;
                parOpts.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;
                parOpts.TaskScheduler = TaskScheduler.Current;

                TaskCreationOptions tco = new TaskCreationOptions();
                tco = TaskCreationOptions.PreferFairness;
                Task task = null;
                task = Task.Factory.StartNew(() =>
                {

                    var servicio = new n4WebService();
                    //por cada registro crear una unit
                    int u = 0;
                    var sb = string.Empty;
                    foreach (var item in containers)
                    {
                        try
                        {
                            //Por cada registro comprobar que esté en N4 si/no.
                            item.exist = true;
                            var xunit = "<argo:snx xmlns:argo=\"http://www.navis.com/argo\" ><unit-cancel-preadvise facility=\"GYE\">";
                            xunit = string.Concat(xunit, string.Format("<unit-identity id=\"{0}\" type=\"CONTAINERIZED\"><carrier direction=\"OB\" qualifier=\"DECLARED\" mode=\"VESSEL\" id=\"{1}\"/>", item.unidadID, item.referencia));
                            xunit = string.Concat(xunit, "</unit-identity></unit-cancel-preadvise></argo:snx> ");
                            //cancelar la unidad
                            var x = new ObjectSesion();
                            x.clase = "mailservice"; x.metodo = "cancelar(ws)";
                            x.token = token; x.usuario = usuario;
                            x.transaccion = "Cancelar preavisos";
                            if (servicio.InvokeN4Service(x, xunit, ref sb, (++u).ToString()) <= 2)
                            {
                                item.iscancel  = true;
                            }
                            item.mensaje = sb;
                        }
                        catch (Exception ex)
                        {
                            item.iscancel = false;
                            item.mensaje = ex.Message;
                            continue;
                        }
                    }
                    //despues de actualizar cada unidad
                    var xmail = new StringBuilder();
                    xmail.Append("<html>");
                    xmail.Append("<style>*{ font-size:1em;vertical-aling:top;}  h3{color:white; background-color:orange;} table > th{ background-color:#CCC; text-aling:center;}  table{width:100%; } table td,table th {border:1px solid #CCC;vertical-aling:top;} div{margin:0; padding:0; overflow:auto;max-height:250px; } br{margin:1px;} p{ margin:1px; padding:0px;}</style>");
                    xmail.Append(string.Format("Estimado/a: {0}<br/>", usuario));
                    xmail.Append("<h3>Este es un aviso del Centro de servicios en línea de Contecon S.A, para comunicarle lo siguiente</h3>");
                    xmail.Append("<p>Resultados del procesamiento de las unidades vacías para cancelación de su preaviso:</p>");
                    xmail.Append("<table cellspacing=\"2px\" cellpadding=\"1px\">");
                    xmail.Append("<tr><th><strong>Unidades canceladas</strong></th><th><strong>Unidades con problemas</strong></th></tr>");
                    xmail.Append("<tr>");
                    //celda de oks
                    xmail.Append("<td><div><table>");
                    int number = 0;
                    foreach (var c in containers)
                    {
                        if (c.iscancel)
                        {
                            xmail.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", c.unidadID, c.uniISO);
                            number++;
                        }
                    }
                    xmail.AppendFormat("</table></div>Total de cancelaciones:{0}</td>", number);
                    //celda de malos
                    xmail.Append("<td><div><table>");
                    number = 0;
                    foreach (var c in containers)
                    {
                        if (!c.iscancel)
                        {
                            xmail.AppendFormat("<tr><td>{0}</td><td>{1}</td><tr>", c.unidadID, c.mensaje);
                            number++;
                        }
                    }
                    xmail.AppendFormat("</table></div>Total fallidos:{0}</td>", number);
                    xmail.Append("</tr>");
                    xmail.Append("</table>");
                    xmail.Append("</html>");
                    csl_log.log_csl.mailSenderDB(xmail.ToString(), 1, mail, usuario);
                }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);
                task.ContinueWith((a) => { a.Dispose(); });
            }
            catch (Exception e)
            {
                csl_log.log_csl.save_log<Exception>(e, "mailservice", "cancelar", usuario + ':' + token + ':' + mail, usuario);
            }
        }

        [WebMethod(EnableSession = true, Description = "Permite enviar un mail desde la cuenta de csl")]
        public void sendmail(string hmtlmail, string mail, string usuario = null, string token = null)
        {
            if (string.IsNullOrEmpty(usuario)) { usuario = "nWgenN4"; }
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(hmtlmail))
            {
                return;
            }
            csl_log.log_csl.mailSenderDB(hmtlmail, 0, mail, usuario);
        }

        [WebMethod(EnableSession = true, Description = "Permite enviar mail de alerta a usuario cuando se intenta sobrescribir")]
        public void catchOverwrite( string unidad, string booking, string usuario, string mailintenta, string token = null)
        {
            try
            {
                CancellationToken tks = new CancellationToken();
                ParallelOptions parOpts = new ParallelOptions();
                parOpts.CancellationToken = tks;
                parOpts.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;
                parOpts.TaskScheduler = TaskScheduler.Current;
                TaskCreationOptions tco = new TaskCreationOptions();
                tco = TaskCreationOptions.PreferFairness;
                Task task = null;
                task = Task.Factory.StartNew(() =>
                {
                    var t = DataHelper.GetAISVData(unidad, booking);
                    if (t == null)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La búsqueda del aisv anterior es null"), "mailservice", "catchOverwrite", string.Concat(unidad, ";", booking), usuario);
                        return;
                    }
                    //despues de actualizar cada unidad
                    var xmail = new StringBuilder();
                    xmail.Append("<html>");
                    xmail.Append("<style>*{ font-size:1em;vertical-aling:top;}  h3{color:white; background-color:red;} table > th{ background-color:#CCC; text-aling:center;}  table{width:100%; } table td,table th {border:1px solid #CCC;} div{margin:0; padding:0; overflow:auto;max-height:250px; } br{margin:1px;} p{ margin:1px; padding:0px;}</style>");
                    xmail.Append(string.Format("Estimado/a: {0}<br/>", t.Item3));
                    xmail.Append("<h3>Este es un aviso del Centro de servicios en línea de Contecon Guayquil S.A, para comunicarle lo siguiente:</h3>");
                    xmail.AppendFormat("<p>El usuario {0}, está intentando realizar un nuevo AISV para ingresar el contenedor número [{1}] bajo el booking [{2}], el mismo que se encuentra asociado en el AISV No. [{3}]. <br/>Favor resolver este problema lo antes posible.</p>", usuario, unidad, booking, t.Item1);
                    xmail.AppendFormat("<p>Fecha del ingreso del AISV {0}, {1}</p>", t.Item1, t.Item4);
                    xmail.AppendFormat("<p>Fecha de Notificación {0}.</p>", DateTime.Now);
                    xmail.Append("</html>");
                    if (string.IsNullOrEmpty(usuario))
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("usuario es null"), "mailservice", "catchOverwrite", string.Concat(unidad, ";", booking), usuario);
                        return;
                    }
                    if (string.IsNullOrEmpty(t.Item2) || string.IsNullOrEmpty(xmail.ToString()))
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Mail o Mensaje es null"), "mailservice", "catchOverwrite", string.Concat(unidad, ";", booking), usuario);
                        return;
                    }
                    var mailall =!string.IsNullOrEmpty( mailintenta)?string.Concat(t.Item2,";",mailintenta):t.Item2;
                    csl_log.log_csl.mailSenderDB(xmail.ToString(), 2, mailall, usuario);
                }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);
                task.ContinueWith((a) => { a.Dispose(); });
            }
            catch (Exception e)
            {
                csl_log.log_csl.save_log<Exception>(e, "mailservice", "catchOverwrite",string.Concat(unidad,";",booking), usuario);
            }

        }

        [WebMethod(EnableSession = true, Description = "Permite valida la conexión a cada motor de datos relacionado")]
        public string retornarOk(string server, string user, string pass, string catalogo)
        {
            var tex = string.Empty;
            if (!DataHelper.getOk(server, user, pass, catalogo, ref tex))
            {
                return tex;
            }
            return "Conexión exitosa";
        }
    }
    [Serializable]
    public class unidad
    {
        public string unidadID { get; set; }
        public string bokingID { get; set; }
        public string itemID { get; set; }
        public string uniISO { get; set; }
        public string lineaID { get; set; }
        public string mensaje { get; set; }
        public bool preadvice { get; set; }
        public string referencia { get; set; }
        public bool iscancel { get; set; }
        public bool exist { get; set; }
        public string imo { get; set; }
        public string pod { get; set; }
        public string pod1 { get; set; }
        public string shiper { get; set; }
        //TEMEPRATURA -> VENTILACIÓN Y UNIDAD
        public string temp { get; set; }
        public string reefer { get; set; }
        public string hume { get; set; }
        public string vent { get; set; }
        public string ventU { get; set; }

        public unidad( string unidadid,string bokingid,string itemid,string unitiso, string bimo)
        {
            this.unidadID = unidadid;
            this.bokingID = bokingid;
            this.itemID = itemid;
            this.uniISO = unitiso;
            this.imo = bimo;
        }
        public unidad() { }
    }
}