using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ServicesEntities;

namespace Aduana
{
    [Serializable]
    [XmlRoot(ElementName = "IngresoMercancia")]
    public class IngresoMercancia
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "estado")]
        public string estado { get; set; }

        [XmlAttribute(AttributeName = "unidad")]
        public string unidad { get; set; }

        [XmlAttribute(AttributeName = "tipo_carga")]
        public string tipo_carga { get; set; }

        [XmlAttribute(AttributeName = "carga_id")]
        public Int64 carga_id { get; set; }

        [XmlAttribute(AttributeName = "fecha")]
        public DateTime fecha { get; set; }
    }
}
