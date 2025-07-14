using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ServicesEntities.PasePuerta
{
    [Serializable]
    [XmlRoot(ElementName = "PasePuerta")]
    public class PasePuerta
    {
        [XmlAttribute(AttributeName = "id_pase")]
        public Int64 id_pase { get; set; }

        [XmlAttribute(AttributeName = "gkey")]
        public Int64 gkey { get; set; }

        [XmlAttribute(AttributeName = "estado")]
        public string estado { get; set; }

        [XmlAttribute(AttributeName = "fecha_expiracion")]
        public DateTime? fecha_expiracion { get; set; }

        [XmlAttribute(AttributeName = "tipo_carga")]
        public string tipo_carga { get; set; }

        [XmlAttribute(AttributeName = "numero_pase_n4")]
        public Int64 numero_pase_n4 { get; set; }

        [XmlAttribute(AttributeName = "id_placa")]
        public string id_placa { get; set; }

        [XmlAttribute(AttributeName = "id_chofer")]
        public string id_chofer { get; set; }

        [XmlAttribute(AttributeName = "id_transporte")]
        public string id_transporte { get; set; }

    }
}
