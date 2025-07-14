using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft;
using System.Web.Script.Services;

namespace CSLSite.services
{
    /// <summary>
    /// Descripción breve de atraque
    /// </summary>
    [WebService(Namespace = "http://www.cgsa.com.ec")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class aisv : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string chofer_compania(entidad objeto)
        {
            var cf = objeto.parametros.Where(p => p.nombre.Equals("placa")).FirstOrDefault(); //la placa del camion
            var dv = objeto.parametros.Where(p => p.nombre.Equals("driver")).FirstOrDefault(); // la licencia del chofer
            var r = new resultado();
            //son nulos no validar
            if (cf == null || dv == null)
            {
                r.accion = "Nothing";
                r.esvalido = "1";
                r.mensaje = string.Empty;
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            var re = jAisvContainer.IsDriverInCompany(cf.valor, dv.valor);
            if (!re)
            {
                r.accion = "ShowAlert";
                r.esvalido = "0";
                r.mensaje = string.Format("Aviso!<br/>El camión {0} no se corresponde con la empresa de transporte del chofer {1} ", cf.valor.ToUpper(), dv.valor.ToUpper());
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(r);
        }
    }
}
