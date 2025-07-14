using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace N4Billing
{
    [Serializable]
    [XmlRoot(ElementName = "Cliente")]
    public class Cliente
    {
        [XmlAttribute(AttributeName = "id_cliente")]
        public string id_cliente { get; set; }

        [XmlAttribute(AttributeName = "ruc")]
        public string ruc { get; set; }

        [XmlAttribute(AttributeName = "nombre")]
        public string nombre { get; set; }

        [XmlAttribute(AttributeName = "dias_credito")]
        public Int16 dias_credito { get; set; }

        [XmlAttribute(AttributeName = "codigo_sap")]
        public string codigo_sap { get; set; }

        [XmlAttribute(AttributeName = "email")]
        public string email { get; set; }

        [XmlAttribute(AttributeName = "bloqueo")]
        public bool bloqueo { get; set; }

        [XmlAttribute(AttributeName = "liberacion")]
        public bool liberacion { get; set; }

    }
}
