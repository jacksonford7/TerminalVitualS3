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
    [XmlRoot(ElementName = "CompaniaTransporte")]
    public class CompaniaTransporte : ModuloBase
    {
        [XmlAttribute(AttributeName = "id")]
        public Int64 id { get; set; }

        [XmlAttribute(AttributeName = "ruc")]
        public string ruc { get; set; }

        [XmlAttribute(AttributeName = "razon_social")]
        public string razon_social { get; set; }

        [XmlAttribute(AttributeName = "contacto")]
        public string contacto { get; set; }

        [XmlAttribute(AttributeName = "direccion")]
        public string direccion { get; set; }


        [XmlAttribute(AttributeName = "ciudad")]
        public string ciudad { get; set; }

        [XmlAttribute(AttributeName = "telefono")]
        public string telefono { get; set; }

        [XmlAttribute(AttributeName = "mail")]
        public string mail { get; set; }

        public CompaniaTransporte() : base()
        {


        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        //un chofer individual
        public static ResultadoOperacion<CompaniaTransporte> ObtenerCompania(string usuario, string ruc)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<CompaniaTransporte>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            p.Parametros.Add("ruc", ruc);
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, ruc);
#endif
            var rp = BDOpe.ComandoSelectAEntidad<CompaniaTransporte>(bcon, "[Bill].[compania_individual]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<CompaniaTransporte>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<CompaniaTransporte>.CrearFalla(rp.MensajeProblema);
        }

        //Toda la lista para pantalla
        public static ResultadoOperacion<List<CompaniaTransporte>> ObtenerCompanias(string usuario, string pista)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<CompaniaTransporte>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            pista = pista?.Trim();
            p.Parametros.Add("pista", pista);
            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<CompaniaTransporte>(bcon, "[Bill].[compania_lista]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<CompaniaTransporte>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<CompaniaTransporte>>.CrearFalla(rp.MensajeProblema);
        }

    }
}
