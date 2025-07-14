using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AccesoDatos;
using Configuraciones;
using Respuesta;

namespace N4.Entidades
{
    [Serializable]
    [XmlRoot(ElementName = "Agente")]
    public class Agente:ModuloBase
    {
        [XmlAttribute(AttributeName = "ruc")]
        public string ruc { get; set; }
        [XmlAttribute(AttributeName = "nombres")]
        public string nombres { get; set; }
        [XmlAttribute(AttributeName = "codigo")]
        public string codigo { get; set; }

        public Agente():base()
        {
           
            
        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }


        public static ResultadoOperacion<Agente> ObtenerAgente(string usuario, string codigo)
        {
            var p = new Agente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
               return ResultadoOperacion<Agente>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
             p.Parametros.Add("id", codigo);
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, codigo);
#endif
            var rp= BDOpe.ComandoSelectAEntidad<Agente>(bcon, "[Bill].[agente_aduana]",  p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<Agente>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Agente>.CrearFalla(rp.MensajeProblema);
        }

        public static ResultadoOperacion<Agente> ObtenerAgentePorRuc(string usuario, string ruc)
        {
            var p = new Agente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Agente>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("id", ruc);
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, ruc);
#endif
            var rp = BDOpe.ComandoSelectAEntidad<Agente>(bcon, "[Bill].[agente_aduana_codigo]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<Agente>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Agente>.CrearFalla(rp.MensajeProblema);
        }



    }
}
