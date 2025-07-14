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
    [XmlRoot(ElementName = "Chofer")]
    public class Chofer : ModuloBase
    {
        [XmlAttribute(AttributeName = "id")]
        public Int64 id { get; set; }
        [XmlAttribute(AttributeName = "numero")]
        public string numero { get; set; }

        [XmlAttribute(AttributeName = "nombres")]
        public string nombres { get; set; }

        [XmlAttribute(AttributeName = "batnbr")]
        public string batnbr { get; set; }

        [XmlAttribute(AttributeName = "expira")]
        public DateTime? expira { get; set; }

        public Chofer() : base()
        {


        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        //un chofer individual
        public static ResultadoOperacion<Chofer> ObtenerChofer(string usuario, string id)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Chofer>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("id", id);
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, id);
#endif
            var rp = BDOpe.ComandoSelectAEntidad<Chofer>(bcon, "[Bill].[chofer_individual]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<Chofer>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Chofer>.CrearFalla(rp.MensajeProblema);
        }

        //Toda la lista para pantalla
        public static ResultadoOperacion< List<Chofer>> ObtenerChoferes(string usuario, string pista)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion< List<Chofer>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            pista = pista?.Trim();
            p.Parametros.Add("pista", pista);
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<Chofer>(bcon, "[Bill].[choferes_lista]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Chofer>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Chofer>>.CrearFalla(rp.MensajeProblema);
        }

        public static ResultadoOperacion<List<Chofer>> ObtenerChoferes(string usuario, string pista, string trcko)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Chofer>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            pista = pista?.Trim();
            p.Parametros.Add("pista", pista);
            p.Parametros.Add("empresa_id", trcko);
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<Chofer>(bcon, "[Bill].[choferes_empresa_lista]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Chofer>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Chofer>>.CrearFalla(rp.MensajeProblema);
        }

    }
}
