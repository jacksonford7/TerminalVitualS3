using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace N4Billing
{
    [Serializable]
    [XmlRoot(ElementName = "Placa")]
    public class Placa
    {
        [XmlAttribute(AttributeName = "id_camion")]
        public string id_camion { get; set; }

        [XmlAttribute(AttributeName = "placa")]
        public string placa { get; set; }
       
        [XmlAttribute(AttributeName = "activo")]
        public bool activo { get; set; }
    }
}
