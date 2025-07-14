using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ConectorN4;
using System.Text;
using System.IO;

namespace CSLSite
{
    public static class Utility
    {

        public  const string firma = "<p>Usted recibirá el estado de su solicitud en el transcurso de 24 a 48 horas.</p><p>Es un placer servirle,</p><p>Atentamente,<br/><strong>Terminal Virtual</strong><br/>Contecon Guayaquil S.A. CGSA<br/>An ICTSI Group Company.</p><p><i>El contenido de este mensaje es informativo, por favor no responda este correo.</i></p>";
        public const string firmaNo = "<p>Es un placer servirle,</p><p>Atentamente,<br/><strong>Terminal Virtual</strong><br/>Contecon Guayaquil S.A. CGSA<br/>An ICTSI Group Company.</p><p><i>El contenido de este mensaje es informativo, por favor no responda este correo.</i></p>";
        public static void mostrarMensajeRedireccionando(this System.Web.UI.Page page, string mensaje, string pagina){
            ScriptManager.RegisterStartupScript(page, page.GetType(), "Script", string.Format(@"<script> language='javascript'>alert('{0}'); window.location='{1}';</script>", mensaje, pagina),false);
        }
        public static void mostrarMensaje(this System.Web.UI.Page page, string mensaje)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "Script", string.Format(@"<script> language='javascript'>alert('{0}');</script>", mensaje), false);
        }
        public static void mostrarMensaje(this System.Web.UI.Page page, string mensaje, bool salir = true)
        {
            if (!salir)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Script", string.Format(@"<script> language='javascript'> alert('{0}');</script>", mensaje), false);
                return;
            }
            ScriptManager.RegisterStartupScript(page, page.GetType(), "Script", string.Format(@"<script> language='javascript'> alert('{0}'); window.close();  window.opener.location.href = window.opener.location.href;</script>", mensaje), false);
            return;
        }
        public static string validacionN4(ObjectSesion sesObj, string xml)
        {

            string me = string.Empty;
            string errorValidacion = string.Empty;
            n4WebService n4s = new n4WebService();
            var i = n4s.InvokeN4Service(sesObj, xml, ref errorValidacion, DateTime.Now.ToString("yyyyMMddHHmm") );
            return errorValidacion;
        }
        public static string repesaje_msg_ingreso(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado el repesaje de la(s) unidad(es) detallada(s) a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga.ToUpper());
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            foreach (var t in contenedores)
            {
                xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
            }
            xmensaje.Append("</table>");
        //   xmensaje.Append("<br/>Si su contenedor no aparece en esta lista, favor remitirse a la novedad presentada al momento de la programación del servicio");

            xmensaje.Append(Utility.firma);
            return xmensaje.ToString();
        }
        public static string verificacion_msg_ingreso(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado la verificación de sellos de la(s) unidad(es) detallada(s) a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            foreach (var t in contenedores)
            {
                xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
            }
            xmensaje.Append("</table>");
            xmensaje.Append(Utility.firma);
            return xmensaje.ToString();
        }
        public static string etiqueta_msg_ingreso(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado el etiquetado/desetiquetado de la(s) unidad(es) detallada(s) a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            foreach (var t in contenedores)
            {
                xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
            }
            xmensaje.Append("</table><br/>");

          //  xmensaje.Append("<p>Consideraciones generales:</p>");
            //xmensaje.Append("<p><strong>Nota:</strong>Las unidades que no constan en este listado no fueron programadas, debido a que no se encuentran registradas en nuestro sistema.<p/>");
            xmensaje.Append("<p>Cualquier consulta adicional, envíe un correo a las casilla YardPlanners@cgsa.com.ec</p>");
            xmensaje.Append(Utility.firma);
            return xmensaje.ToString();
        }
        public static string tecnica_msg_ingreso(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado la revisión técnica refrigerada de la(s) unidad(es) detallada(s) a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
            xmensaje.Append("<table cellpadding='0' cellspacing='1' border='0'>");
            foreach (var t in contenedores)
            {
                xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
            }
            xmensaje.Append("</table><br/>");

           xmensaje.Append("<p><strong>Nota:</strong>Sírvase verficar el estado de su solicitud en el plazo de 2 horas.<p/>");
            
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string correccion_iie_msg_ingreso(string contenedor, string aisv, string tipocor, string daeE, string daeB, string cii, string iie)
        {
            StringBuilder xmensaje = new StringBuilder();
            if (!string.IsNullOrEmpty(cii) && !string.IsNullOrEmpty(iie))
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha transmitido un Informe de Ingreso de Exportación bajo la  DAE correcta y una Solicitud de Eliminación en la DAE errada, de la unidad detallada a continuación: </h3>");
            }
            else
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, Le informamos que se ha transmitido el Informe de Ingreso de Exportación de la unidad detallada a continuación, debido al error registrado durante la generación del AISV: </h3>");
            
            }

            //; gates-helpdesk@cgsa.com.ec 
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='1'>");
            xmensaje.AppendFormat("<tr><td><strong> Contenedor:</strong> </td><td>{0}</td></tr>", contenedor);
            xmensaje.AppendFormat("<tr><td><strong> AISV:</strong> </td><td>{0}</td></tr>", aisv);
            xmensaje.AppendFormat("<tr><td><strong> Tipo de corrección:</strong> </td><td>{0}</td></tr>", tipocor);
            xmensaje.AppendFormat("<tr><td><strong> DAE errada:</strong> </td><td>{0}</td></tr>", daeE);
            xmensaje.AppendFormat("<tr><td><strong> DAE correcta:</strong> </td><td>{0}</td></tr>", daeB);
            if (!string.IsNullOrEmpty(cii))
            {
                xmensaje.AppendFormat("<tr><td><strong> No. de entrega de eliminación:</strong> </td><td>{0}</td></tr>", cii);
            }
            xmensaje.AppendFormat("<tr><td><strong> No. de entrega de nuevo ingreso:</strong> </td><td>{0}</td></tr>", iie);
            xmensaje.Append("</table><br/>");
            xmensaje.Append("<p>Sírvase verificar el ingreso de exportación de su contenedor en el sistema Ecuapass, bajo la DAE indicada en su solicitud, en la opción 2.11 Servicios de información de Despacho/Carga de Servicios Informativos, sección: Cargas, opción: Consulta del Informe de Ingreso de Carga de Exportación.</p>");
            xmensaje.Append("<p>Cualquier consulta adicional, envíe un correo a las casillas ec.sac@contecon.com.ec</p>");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string restiba_msg_ingreso(string senaer, string trafico, string contenedorFCL, string contenedorMTY, string producto, string fecha, string comentario, string boking, string nocarga)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha registrado la Solicitud de Reestiba de Mercancías del contenedor detallado a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> No. Solicitud de Reestiba (SENAE):</strong> </td><td>{0}</td></tr>", senaer);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.AppendFormat("<tr><td><strong> Contenedor lleno:</strong> </td><td>{0}</td></tr>", contenedorFCL);
            xmensaje.AppendFormat("<tr><td><strong> Contenedor Vacío:</strong> </td><td>{0}</td></tr>", contenedorMTY);
            xmensaje.AppendFormat("<tr><td><strong> Producto/Embalaje </strong> </td><td>{0}</td></tr>", producto);
            xmensaje.AppendFormat("<tr><td><strong> Fecha/Hora propuesta:</strong> </td><td>{0}</td></tr>", fecha);
            xmensaje.AppendFormat("<tr><td><strong> Comentarios adicionales:</strong> </td><td>{0}</td></tr>", comentario);
            xmensaje.Append("</table><br");
            xmensaje.Append("<p>Se le notificará por este medio, la fecha y hora de la programación de la reestiba. Es responsabilidad del exprotador presentar la comunicación sobre la reestiba solicitada a la Policía Nacional Antinarcóticos (PNA), quienes dispondrán en el documento si la operación será efectuada con un delegado de esta autoridad, el mismo que deberá ser presentado en la fecha y hora planificada para esta operación.</p>");
            xmensaje.Append(Utility.firma);
            return xmensaje.ToString();
        }
        public static string correccion_iie_msg_cobro(string contenedor, string aisv, string tipocor, string daeE, string daeB, string cii, string iie)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, las transmisiones efectuadas de la unidad detallada a continuación, han sido aceptadas por el sistema ECUAPASS:</h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Contenedor:</strong> </td><td>{0}</td></tr>", contenedor);
            xmensaje.AppendFormat("<tr><td><strong> AISV:</strong> </td><td>{0}</td></tr>", aisv);
            xmensaje.AppendFormat("<tr><td><strong> Tipo de corrección:</strong> </td><td>{0}</td></tr>", tipocor);
            xmensaje.AppendFormat("<tr><td><strong> DAE errada:</strong> </td><td>{0}</td></tr>", daeE);
            xmensaje.AppendFormat("<tr><td><strong> DAE correcta:</strong> </td><td>{0}</td></tr>", daeB);

            if (!string.IsNullOrEmpty(cii))
            {
                xmensaje.AppendFormat("<tr><td><strong> No. de entrega de eliminación:</strong> </td><td>{0}</td></tr>", cii);
            }

            xmensaje.AppendFormat("<tr><td><strong> No. de entrega de nuevo ingreso:</strong> </td><td>{0}</td></tr>", iie);
            xmensaje.Append("</table><br/>");
           // xmensaje.Append("<p>Cada una de estas transmisiones tienen un costo, el cual será facturado a su representada, el mismo que deberá ser cancelado antes del embarque de su contenedor.</p>");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string restiba_msg_ingreso(string lleno, string vacio, string trafico, string solsena,  string producto, string fecha, string nota    )
        {

            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha registrado la Solicitud de Reestiba de mercancías, que se encuentra en el Contenedor Lleno detallado a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Solicitud de reestiba SENAE:</strong> </td><td>{0}</td></tr>", solsena);

            var expo = false;
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
                expo = true;
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            xmensaje.AppendFormat("<tr><td><strong>Contenedor Lleno:</strong></td><td>{0}</td></tr>", lleno);
            xmensaje.AppendFormat("<tr><td><strong>Contenedor Vacío:</strong></td><td>{0}</td></tr>", vacio);
            xmensaje.AppendFormat("<tr><td><strong>Producto y embalaje:</strong></td><td>{0}</td></tr>", producto);
            xmensaje.AppendFormat("<tr><td><strong>Fecha y hora propuesta:</strong></td><td>{0}</td></tr>", fecha);
            xmensaje.AppendFormat("<tr><td><strong>Comentarios:</strong></td><td>{0}</td></tr>", nota);
            xmensaje.Append("</table>");

            xmensaje.Append("<p>Se le notificará por este medio, la fecha y hora de la programación de la Reestiba.</p>");
            if (expo)
            {
                xmensaje.Append("<p>Es responsabilidad del exportador presentar la comunicación sobre la reestiba solicitada a la Policía Nacional Antinarcóticos (PNA), quienes dispondrán en el documento si la operación será efectuada con un delegado de esta autoridad, el mismo que deberá ser presentado en la fecha y hora planificada para esta operación.</p>");
                xmensaje.Append("<p>La operación de reestiba no implica el embarque del contenedor en la nave respectiva.</p>");
            }
            else
            {
                 xmensaje.Append("<p>Es responsabilidad del importador gestionar con el Servicio Nacional de Aduana del Ecuador  (SENAE), que el funcionario asignado se encuentre presente en la fecha y hora planificada para esta operación.</p>");
            }
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string restiba_msg_proceso(string lleno, string vacio, string trafico, string solsena, string producto, string fecha, string nota, string observaciones)
        {

            StringBuilder xmensaje = new StringBuilder();

            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha  programado la Operación de Reestiba, de acuerdo a lo detallado a continuación: </h3>");
            if (!string.IsNullOrEmpty(observaciones))
            {
                xmensaje.AppendFormat("<p><strong>Observaciones:</strong></p><p style='border: 1px solid orange; margin:2px; padding:2px;'>{0}</p>", observaciones);
            }

            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Solicitud de reestiba SENAE:</strong> </td><td>{0}</td></tr>", solsena);
            var expo = false;
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
                expo = true;
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            xmensaje.AppendFormat("<tr><td><strong>Contenedor Lleno:</strong></td><td>{0}</td></tr>", lleno);
            xmensaje.AppendFormat("<tr><td><strong>Contenedor Vacío:</strong></td><td>{0}</td></tr>", vacio);
            xmensaje.AppendFormat("<tr><td><strong>Producto y embalaje:</strong></td><td>{0}</td></tr>", producto);
           // xmensaje.AppendFormat("<tr><td><strong>Fecha y hora propuesta:</strong></td><td>{0}</td></tr>", fecha);
         //   xmensaje.AppendFormat("<tr><td><strong>Comentarios:</strong></td><td>{0}</td></tr>", nota);
            xmensaje.Append("</table>");



            if (expo)
            {
                xmensaje.Append("<p>La operación de reestiba no implica el embarque del contenedor en la nave respectiva.</p>");
            }
            //else
            //{
            //    xmensaje.Append("<p>Es responsabilidad del importador gestionar con el Servicio Nacional de Aduana del Ecuador  (SENAE), que el funcionario asignado se encuentre presente en la fecha y hora planificada para esta operación.</p>");
            //}
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string restiba_fin(string lleno, string vacio, string trafico, string solsena, string producto, string fecha, string nota, string observaciones)
        {

            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha finalizado la Solicitud de Reestiba de mercancías, de acuerdo a lo detallado a continuación:</h3>");

            if (!string.IsNullOrEmpty(observaciones))
            {
                xmensaje.AppendFormat("<p><strong>Observaciones:</strong></p><p style='border: 1px solid orange; margin:2px; padding:2px;'>{0}</p>", observaciones);
            }
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Solicitud de reestiba SENAE:</strong> </td><td>{0}</td></tr>", solsena);
            var expo = false;
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
                expo = true;
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            xmensaje.AppendFormat("<tr><td><strong>Contenedor Lleno:</strong></td><td>{0}</td></tr>", lleno);
            xmensaje.AppendFormat("<tr><td><strong>Contenedor Vacío:</strong></td><td>{0}</td></tr>", vacio);
            xmensaje.AppendFormat("<tr><td><strong>Producto y embalaje:</strong></td><td>{0}</td></tr>", producto);
           // xmensaje.AppendFormat("<tr><td><strong>Fecha y hora propuesta:</strong></td><td>{0}</td></tr>", fecha);
          //  xmensaje.AppendFormat("<tr><td><strong>Comentarios:</strong></td><td>{0}</td></tr>", nota);
            xmensaje.Append("</table>");



            if (expo)
            {
                xmensaje.Append("<p>La operación de reestiba no implica el embarque del contenedor en la nave respectiva. Sírvase verificar que la información del nuevo contenedor se encuentra actualizada en la DAE, caso contrario envíe un correo a la casilla CGSA-Consolidaciones@cgsa.com.ec</p>");
            }
            else
            {
                xmensaje.Append("<p>La actualización del número de carga es realizada por el funcionario de aduana que estuvo presente en la operación.</p>");
            }
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string cerrojo_msg(List<string> contenedores,  string estado="I", string observacion=null)
        {

             StringBuilder xmensaje = new StringBuilder();
            if (!string.IsNullOrEmpty(estado) && estado.ToUpper().Contains("I"))
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha registrado el Servicio de Cerrojo Electrónico para la(s) unidad(es) detallada(s) a continuación: </h3>");
            }
            else
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha finalizado el Servicio de Cerrojo Electrónico para la(s) unidad(es) detallada(s) a continuación: </h3>");
            }

            if (!string.IsNullOrEmpty(observacion))
            {
                xmensaje.AppendFormat("<p style='border:1px solid orange;'>{0}</p>", observacion);
            }

            xmensaje.Append("<h3>CONTENEDOR(ES) PROGRAMADO(S):</h3>");
            //aca las unidades   
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            foreach (var t in contenedores)
            {
                xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
            }
            xmensaje.Append("</table><br/>");
          //  xmensaje.Append("Consideraciones generales:");
            //xmensaje.Append("Si su contenedor no aparece en el listado, favor remitirse a la novedad presentada al momento de la programación del servicio.");
            xmensaje.Append("Cualquier consulta adicional envíe un correo a la casilla: YardPlanners@cgsa.com.ec");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string repesaje_msg_proceso(List<string> contenedores, string trafico, string estado, string boking, string nocarga , string observaciones)
        {
            bool isExpo = false;
            StringBuilder xmensaje = new StringBuilder();
            if (estado.ToLower().Contains("finalizada"))
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha finalizado el repesaje de la(s) unidad(es) detallada(s) a continuación, con los siguientes datos: </h3>");
                if (!string.IsNullOrEmpty(observaciones))
                {
                    xmensaje.AppendFormat("<strong>Observaciones:</strong><p style='border:1px solid orange;'>{0}</p>",observaciones);
                }
            }
            else
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado el repesaje de la(s) unidad(es) detallada(s) a continuación: </h3>");
            }
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación"; 
                isExpo = true;
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
           

            var tf = "NULL";
            if (isExpo)
            { 
             tf="EXPRT";
            }
            else
            {
                tf = "IMPRT";
            }

            //obtenego la info de cada contenedor ACTUAL EN N4
            var n4cntr = unidadN4.consultaUnidadesN4(tf, "P", contenedores);
            bool n4_ok = n4cntr != null && n4cntr.Count > 0;
            unitFull cn4 = null;
            if (n4_ok)
            {
                xmensaje.Append("<table cellpadding='1' cellspacing='1' border='1'>");
                xmensaje.Append("<tr><td><strong> NUMERO</strong></td><td><strong>PESO KG.</strong></td></tr>");
            }
            else
            {
                xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            }
            foreach (var t in contenedores)
            {
                if (n4_ok)
                {
                    cn4 = n4cntr.Where(u => u.id.Contains(t)).FirstOrDefault();
                }
                if (cn4 == null)
                {
                    xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
                }
                else
                {
                    xmensaje.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", t, cn4.peso);
                }
            }
            xmensaje.Append("</table>");

            if (isExpo)
            {
                xmensaje.Append("<p>Cualquier consulta adicional envíe un correo a la casilla: yardplanners@cgsa.com.ec.</p>");
            }
            else
            {
                xmensaje.Append("<p>Cualquier consulta adicional envíe un correo a la casilla:ec.sac@contecon.com.ec.</p>");
            }

            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string verificacion_msg_proceso(List<string> contenedores, string trafico, string estado, string boking, string nocarga, string observaciones)
        {
            bool isExpo = false;
            StringBuilder xmensaje = new StringBuilder();
            if (estado.ToLower().Contains("finalizada"))
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha finalizado la verificación de sellos de la(s) unidad(es) detallada(s) a continuación, con los siguientes datos: </h3>");
                if (!string.IsNullOrEmpty(observaciones))
                {
                    xmensaje.AppendFormat("<strong>Observaciones:</strong><p style='border:1px solid orange;'>{0}</p>", observaciones);
                }
            }
            else
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado el repesaje de la(s) unidad(es) detallada(s) a continuación: </h3>");
            }
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación"; isExpo = true;
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   



            var tf = "NULL";
            if (isExpo)
            {
                tf = "EXPRT";
            }
            else
            {
                tf = "IMPRT";
            }

            //obtenego la info de cada contenedor ACTUAL EN N4
            var n4cntr = unidadN4.consultaUnidadesN4(tf, "P", contenedores);
            bool n4_ok = n4cntr != null && n4cntr.Count > 0;
                unitFull cn4 = null;



                if (n4_ok)
                {
                    xmensaje.Append("<table cellpadding='1' cellspacing='1' border='1'>");
                    xmensaje.Append("<tr><td>NUMERO</td><td>SELLO1</td><td>SELLO2</td><td>SELLO3</td><td>SELLO4</td></tr>");
                }
                else
                {
                    xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
                }
            foreach (var t in contenedores)
            {
                if (n4_ok)
                {
                    cn4 = n4cntr.Where(u => u.id.Contains(t)).FirstOrDefault();
                }
                if (cn4 == null)
                {
                    xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
                }
                else
                {
                    xmensaje.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", t, cn4.s1,cn4.s2,cn4.s3,cn4.s4);
                }
            }
            xmensaje.Append("</table><br/>");

            if (isExpo)
            {
                xmensaje.Append("<p>Cualquier consulta adicional envíe un correo a la casilla: auxiliaresplanning@cgsa.com.ec.</p>");
            }
            else
            {
                xmensaje.Append("<p>Cualquier consulta adicional envíe un correo a la casilla: ec.sac@contecon.com.ec.</p>");
            }

            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string correccion_iie_msg_fin(string contenedor, string aisv, string tipocor, string daeE, string daeB, string cii, string iie, string observacion)
        {
            StringBuilder xmensaje = new StringBuilder();
            if (!string.IsNullOrEmpty(cii) && !string.IsNullOrEmpty(iie))
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que la(s) transmisión(es) efectuada(s) de la unidad detallada a continuación, ha(n) sido aceptada(s): </h3>");
            }
            else
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que la(s) transmisión(es) efectuada(s) de la unidad detallada a continuación, ha(n) sido aceptada(s): </h3>");
            }


            if (!string.IsNullOrEmpty(observacion))
            {
                xmensaje.AppendFormat("<strong>Observaciones:</strong><p style='border:1px solid orange;'>{0}</p>", observacion);
            }

            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='1'>");
            xmensaje.AppendFormat("<tr><td><strong>Contenedor:</strong> </td><td>{0}</td></tr>", contenedor);
            if (!string.IsNullOrEmpty(aisv))
            {
                xmensaje.AppendFormat("<tr><td><strong> AISV:</strong> </td><td>{0}</td></tr>", aisv);
            }
            if (!string.IsNullOrEmpty(tipocor))
            {
                xmensaje.AppendFormat("<tr><td><strong> Tipo de corrección:</strong> </td><td>{0}</td></tr>", tipocor);
            }
            if (!string.IsNullOrEmpty(daeE))
            {
                xmensaje.AppendFormat("<tr><td><strong> DAE errada:</strong> </td><td>{0}</td></tr>", daeE);
            }
            if(!string.IsNullOrEmpty(daeB))
            {
               xmensaje.AppendFormat("<tr><td><strong> DAE correcta:</strong> </td><td>{0}</td></tr>", daeB);
            }
            if (!string.IsNullOrEmpty(cii))
            {
                xmensaje.AppendFormat("<tr><td><strong> No. de entrega de eliminación:</strong> </td><td>{0}</td></tr>", cii);
            }
            if(!string.IsNullOrEmpty(iie))
            {
                xmensaje.AppendFormat("<tr><td><strong> No. de entrega de nuevo ingreso:</strong> </td><td>{0}</td></tr>", iie);
            }
            xmensaje.Append("</table><br/>");
            xmensaje.Append("<p>Cualquier consulta adicional, envíe un correo a las casillas ec.sac@contecon.com.ec</p>");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string etiqueta_msg_proceso(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {

            //string asunto;
            StringBuilder xmensaje = new StringBuilder();
            // asunto = string.Format("CSC-{0} Solicitud de Repesaje", numero);
            if (estado.Contains("Finalizad"))
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha finalizado el etiquetado/desetiquetado de la(s) unidad(es) detallada(s) a continuación: </h3>");
            }
            else
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha sido procesada su solicitud el etiquetado/desetiquetado de la(s) unidad(es) detallada(s) a continuación: </h3>");
            }
           
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            bool isExpo = false;
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
                isExpo = true;
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   


            var tf = "NULL";
            if (isExpo)
            {
                tf = "EXPRT";
            }
            else
            {
                tf = "IMPRT";
            }

            //obtenego la info de cada contenedor ACTUAL EN N4
            var n4cntr = unidadN4.consultaUnidadesN4(tf, "P", contenedores);
            bool n4_ok = n4cntr != null && n4cntr.Count > 0;
            unitFull cn4 = null;

            if (n4_ok)
            {
                xmensaje.Append("<table cellpadding='1' cellspacing='1' border='1'>");
                xmensaje.Append("<tr><td><strong>NUMERO</strong></td><td><strong>ES IMO?<strong/></td></tr>");
            }
            else
            {
                xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            }


            foreach (var t in contenedores)
            {
                if (n4_ok)
                {
                    cn4 = n4cntr.Where(u => u.id.Contains(t)).FirstOrDefault();
                }
                if (cn4 == null)
                {
                    xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
                }
                else
                {
                    xmensaje.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", t, cn4.imo?"SI":"NO");
                }
            }


           // xmensaje.Append("<p><strong>Nota:</strong>Las unidades que no constan en este listado no fueron programadas, debido a que no se encuentran registradas en nuestro sistema.<p/>");
            xmensaje.Append("<p>Cualquier consulta adicional, envíe un correo a las casilla YardPlanners@cgsa.com.ec</p>");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string tecnica_msg_proceso(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {

            //string asunto;
            StringBuilder xmensaje = new StringBuilder();
            if (estado.Contains("Finaliza"))
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que ha finalizado la revisión técnica refrigerada de la(s) unidad(es) detallada(s) a continuación: </h3>");
            }
            else
            {
                xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha procesado su solicitud de revisión técnica refrigerada de la(s) unidad(es) detallada(s) a continuación: </h3>");
            }
            bool isExpo = false;
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
                isExpo = true;
            }
            else
            {
                trafico = "Importación";
            }
            xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
         


            var tf = "NULL";
            if (isExpo)
            {
                tf = "EXPRT";
            }
            else
            {
                tf = "IMPRT";
            }

            //obtenego la info de cada contenedor ACTUAL EN N4
            var n4cntr = unidadN4.consultaUnidadesN4(tf, "P", contenedores);
            bool n4_ok = n4cntr != null && n4cntr.Count > 0;
            unitFull cn4 = null;





            if (n4_ok)
            {
                xmensaje.Append("<table cellpadding='1' cellspacing='1' border='1'>");
                xmensaje.Append("<tr><td><strong>NUMERO</strong></td><td><strong>POSICIÓN<strong/></td></tr>");
            }
            else
            {
                xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            }


            foreach (var t in contenedores)
            {
                if (n4_ok)
                {
                    cn4 = n4cntr.Where(u => u.id.Contains(t)).FirstOrDefault();
                }
                if (cn4 == null)
                {
                    xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
                }
                else
                {
                   xmensaje.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", t, cn4.patio);
                }
            }
            xmensaje.Append("</table><br/>");

            xmensaje.Append("<p>En caso de tener alguna consulta adicional, favor escriba a la siguiente casilla: yardplanners@cgsa.com.ec<p/>");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string late_arriva_msg(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {

            //string asunto;
            StringBuilder xmensaje = new StringBuilder();

            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado el servicio de Late Arrival en la(s) unidad(es) detallada(s) a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
            }
            else
            {
                trafico = "Importación";
            }
          //  xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
            xmensaje.Append("<table cellpadding='0' cellspacing='1' border='0'>");
            foreach (var t in contenedores)
            {
                xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
            }
            xmensaje.Append("</table>");

            xmensaje.Append("<p>Consideraciones generales:</p>");
            xmensaje.Append("<p>Todo contenedor debe ser pre-avisado (AISV), para programar una solicitud de Late Arrival.</p>");
            xmensaje.Append("<p>En caso que su contenedor no aparezca en este listado, favor remitirse a la novedad presentada al momento de guardar su solicitud.</p>");
            xmensaje.Append("<p>El plazo máximo para el ingreso a la Terminal de un contenedor con el servicio de Late Arrival es de hasta 4 horas posterior al Cut Off del buque planificado.</p>");
            xmensaje.Append("<p>En caso de tener alguna consulta adicional, favor escriba a la siguiente casilla: vslplanners@cgsa.com.ec<p/>");

            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        }
        public static string late_arriva_proc(List<string> contenedores, string trafico, string estado, string boking, string nocarga)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h3>Estimado Cliente:<br/><br/>De acuerdo a su solicitud, le informamos que se ha programado el servicio de Late Arrival en la(s) unidad(es) detallada(s) a continuación: </h3>");
            xmensaje.Append("<h3>INFORMACIÓN SOBRE SU SOLICITUD</h3>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong> Estado de la solicitud:</strong> </td><td>{0}</td></tr>", estado);
            if (!string.IsNullOrEmpty(trafico) && trafico.ToUpper().Contains("EXPRT"))
            {
                trafico = "Exportación";
            }
            else
            {
                trafico = "Importación";
            }
           // xmensaje.AppendFormat("<tr><td><strong> Tráfico:</strong></td><td>{0}</td></tr>", trafico);
            if (!string.IsNullOrEmpty(boking))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. Booking:</strong></td><td>{0}</td></tr>", boking);
            }
            if (!string.IsNullOrEmpty(nocarga))
            {
                xmensaje.AppendFormat("<tr><td><strong>No. de Carga:</strong></td><td>{0}</td></tr>", nocarga);
            }
            xmensaje.Append("</table>");
            xmensaje.Append("<h3>CONTENEDOR(ES):</h3>");
            //aca las unidades   
            xmensaje.Append("<table cellpadding='0' cellspacing='1' border='0'>");
            foreach (var t in contenedores)
            {
                xmensaje.AppendFormat("<tr><td>{0}</td></tr>", t);
            }
            xmensaje.Append("</table>");
            xmensaje.Append(Utility.firma);
            return xmensaje.ToString();
        }
        public static string nuevo_usuario(string usuario, string clave)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h4>Estimado Cliente:<br/><br/>Este es un mensaje de Contecon Guayaquil S.A., a continuación se detalla la información con la cual podrá acceder a nuestro Sistema de Terminal Virtual, en el portal [ https://www.cgsa.com.ec ].</h4>");
            xmensaje.Append("<table cellpadding='1' cellspacing='1' border='0'>");
            xmensaje.AppendFormat("<tr><td><strong>Usuario</strong> </td><td>{0}</td></tr>", usuario);
            xmensaje.AppendFormat("<tr><td><strong>Contraseña</strong> </td><td>{0}</td></tr>", clave);
            xmensaje.Append("</table>");
            xmensaje.Append("<p><strong>Nota:</strong>El sistema automáticamente le pedirá que cambie su contraseña. De existir alguna consulta adicional, contáctenos a la casilla ec.sac@contecon.com.ec</p>");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();
        
        }
        public static string cambio_clave(string usuario, string fecha)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.AppendFormat("<h4>Estimado Cliente:<br/><br/>Se ha realizado el cambio de contraseña al usuario {0} en la siguiente fecha y hora {1}</h4>",usuario,fecha);
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();

        }
        public static string recupera_clave(string usuario, string clave, string fecha)
        {
            StringBuilder xmensaje = new StringBuilder();
            xmensaje.Append("<h4>Estimado Cliente:</h4>");
            xmensaje.AppendFormat("<p>Se ha recibido su solicitud para realizar la recuperación de la contraseña del usuario {0} en la siguiente fecha y hora {1}.</p>",usuario,fecha);
            xmensaje.AppendFormat("<p>La contraseña que deberá utilizar para acceder a nuestro Sistema de Terminal Virtual es:  {0}</p>", clave);
            xmensaje.Append("<p>Por su seguridad al iniciar sesión, automáticamente se le solicitará el cambio de contraseña.</p>");
            xmensaje.Append(Utility.firmaNo);
            return xmensaje.ToString();

        }
        public static string ComprobarFolderAndFile(string rutafile)
        {
            try
            {
                string ext = Path.GetExtension(rutafile);
                string archivo = Path.GetFileNameWithoutExtension(rutafile);
                string ruta = Path.GetDirectoryName(rutafile);
                try
                {
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                }
                catch
                {
                    return "-1";
                }
                var c = 1;
                while (File.Exists(string.Format("{0}\\{1}{2}", ruta, archivo, ext)))
                {
                    archivo = string.Format("{0}{1}", archivo, c++);
                }
                return string.Format("{0}\\{1}{2}", ruta, archivo, ext);
            }
            catch
            {
                return "-2";
            }
        }
    }
}