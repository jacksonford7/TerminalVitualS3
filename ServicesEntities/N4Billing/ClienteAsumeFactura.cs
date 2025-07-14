using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace ServicesEntities.N4Billing
{
    [Serializable]
    [XmlRoot(ElementName = "ClienteAsumeFactura")]
    public class ClienteAsumeFactura
    {

        [XmlAttribute(AttributeName = "id_cliente")]
        public string id_cliente { get; set; }

        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "ruc_asume")]
        public string ruc_asume { get; set; }

        [XmlAttribute(AttributeName = "nombre_asume")]
        public string nombre_asume { get; set; }

        [XmlAttribute(AttributeName = "direccion_asume")]
        public string direccion_asume { get; set; }

        [XmlAttribute(AttributeName = "email_asume")]
        public string email_asume { get; set; }

        [XmlAttribute(AttributeName = "telefono_asume")]
        public string telefono_asume { get; set; }
    }
}
