using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace Aduana
{
    [Serializable]
    [XmlRoot(ElementName = "AduanaDae")]
    public class AduanaDae
    {
        [XmlAttribute(AttributeName = "codigo_dae")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "id_agente")]
        public string id_agente { get; set; }

        [XmlAttribute(AttributeName = "id_exportador")]
        public string id_exportador { get; set; }

        [XmlAttribute(AttributeName = "estado")]
        public string estado { get; set; }

        [XmlAttribute(AttributeName = "cantidad")]
        public decimal cantidad { get; set; }
    }
}
