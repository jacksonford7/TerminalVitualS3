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
    [XmlRoot(ElementName = "Camion")]
    public class Camion : ModuloBase
    {
        [XmlAttribute(AttributeName = "id")]
        public Int64 id { get; set; }
        [XmlAttribute(AttributeName = "numero")]
        public string numero { get; set; }

        [XmlAttribute(AttributeName = "expira")]
        public DateTime? expira { get; set; }

        public Camion() : base()
        {


        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        //un chofer individual
        public static ResultadoOperacion<Camion> ObtenerCamion(string usuario, string id)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Camion>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("id", id);
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, id);
#endif
            var rp = BDOpe.ComandoSelectAEntidad<Camion>(bcon, "[Bill].[camion_individual]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<Camion>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Camion>.CrearFalla(rp.MensajeProblema);
        }

        //Toda la lista para pantalla
        public static ResultadoOperacion< List<Camion>> ObtenerCamiones(string usuario, string pista)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion< List<Camion>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            pista = pista?.Trim();
            p.Parametros.Add("pista", pista);
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<Camion>(bcon, "[Bill].[camion_lista]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Camion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Camion>>.CrearFalla(rp.MensajeProblema);
        }


        //Toda la lista para pantalla
        public static ResultadoOperacion<List<Camion>> ObtenerCamiones(string usuario, string pista, string trcko)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Camion>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            pista = pista?.Trim();
            p.Parametros.Add("pista", pista);
            p.Parametros.Add("empresa_id", trcko);
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<Camion>(bcon, "[Bill].[camion_empresa_lista]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Camion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Camion>>.CrearFalla(rp.MensajeProblema);
        }

    }
}
