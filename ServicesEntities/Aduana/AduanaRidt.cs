using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace Aduana
{
    [Serializable]
    [XmlRoot(ElementName = "AduanaRidt")]
    public class AduanaRidt
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "id_cliente")]
        public string contenedor { get; set; }

        [XmlAttribute(AttributeName = "id_agente")]
        public string id_agente { get; set; }


    }
}
