using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using Aduana.Importacion;
using N4.Entidades;

namespace CSLSite.Controllers
{
    public class ContenedorController : ApiController
    {
        [HttpGet]
        [Route("api/contenedor/buscar")]
        public IHttpActionResult Buscar(string mrn, string msn, string hsn)
        {
            if (HttpContext.Current.Session["control"] == null)
            {
                return Unauthorized();
            }

            var user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            string agenteCodigo = string.Empty;
            var agenteCod = Agente.ObtenerAgentePorRuc(user.loginname, user.ruc);
            if (agenteCod.Exitoso && agenteCod.Resultado != null)
            {
                agenteCodigo = agenteCod.Resultado.codigo;
            }

            var validacion = new ecu_validacion_cntr();
            var resultado = validacion.CargaPorManifiestoImpo(user.loginname, user.ruc, agenteCodigo, mrn, msn, hsn, true);
            if (!resultado.Exitoso)
            {
                return BadRequest(resultado.MensajeProblema);
            }

            var first = resultado.Resultado.Where(r => r.gkey != null).FirstOrDefault();
            string idAgente = first?.agente_id;
            string idCliente = first?.importador_id;
            string descCliente = first?.importador ?? string.Empty;

            string rucAgente = string.Empty;
            string descAgente = string.Empty;
            if (!string.IsNullOrEmpty(idAgente))
            {
                var agenteInfo = Agente.ObtenerAgente(user.loginname, idAgente);
                if (agenteInfo.Exitoso && agenteInfo.Resultado != null)
                {
                    rucAgente = agenteInfo.Resultado.ruc?.Trim();
                    descAgente = agenteInfo.Resultado.nombres?.Trim();
                }
            }

            string emailCliente = string.Empty;
            if (!string.IsNullOrEmpty(idCliente))
            {
                var clienteInfo = Cliente.ObtenerCliente(user.loginname, idCliente);
                if (clienteInfo.Exitoso && clienteInfo.Resultado != null)
                {
                    descCliente = clienteInfo.Resultado.CLNT_NAME?.Trim();
                    emailCliente = clienteInfo.Resultado.CLNT_EMAIL;
                }
            }

            return Ok(new
            {
                Contenedores = resultado.Resultado,
                IdAgente = idAgente,
                IdCliente = idCliente,
                DescCliente = descCliente,
                RucAgente = rucAgente,
                DescAgente = descAgente,
                EmailCliente = emailCliente
            });
        }
    }
}
