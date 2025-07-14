using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Aduanas.Entidades
{
    [Serializable]
    [XmlRoot(ElementName = "AutorizacionSalida")]
    public class AutorizacionSalida
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "estado")]
        public string estado { get; set; }

        [XmlAttribute(AttributeName = "importador")]
        public string importador { get; set; }

        [XmlAttribute(AttributeName = "importador_id")]
        public string importador_id { get; set; }

        [XmlAttribute(AttributeName = "agente")]
        public string agente { get; set; }

        [XmlAttribute(AttributeName = "agente_id")]
        public string agente_id { get; set; }

        [XmlAttribute(AttributeName = "declaracion")]
        public string declaracion { get; set; }

        [XmlAttribute(AttributeName = "fecha")]
        public DateTime fecha { get; set; }

        [XmlAttribute(AttributeName = "procesado")]
        public string procesado { get; set; }




    }

}

