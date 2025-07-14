using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace N4Billing
{
    [Serializable]
    [XmlRoot(ElementName = "Chofer")]
    public class Chofer
    {
        [XmlAttribute(AttributeName = "id_chofer")]
        public string id_chofer { get; set; }

        [XmlAttribute(AttributeName = "cedula")]
        public string cedula { get; set; }

        [XmlAttribute(AttributeName = "nombre")]
        public string nombre { get; set; }

        [XmlAttribute(AttributeName = "activo")]
        public bool activo { get; set; }
    }
}
