using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace N4
{
    [Serializable]
    [XmlRoot(ElementName = "CargaBreakBulk")]
    public class CargaBreakBulk
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "tipo")]
        public string tipo { get; set; }//si es impo/expo

        [XmlAttribute(AttributeName = "gkey")]
        public Int64 gkey { get; set; }

        [XmlAttribute(AttributeName = "marca")]
        public string marca { get; set; }

        [XmlAttribute(AttributeName = "descripcion")]
        public string descripcion { get; set; }

        [XmlAttribute(AttributeName = "status")]
        public string status { get; set; }

        [XmlAttribute(AttributeName = "segunda")]
        public string segunda { get; set; }

        [XmlAttribute(AttributeName = "primera")]
        public string primera { get; set; }

        [XmlAttribute(AttributeName = "documento")]
        public string documento { get; set; }


        [XmlAttribute(AttributeName = "cantidad")]
        public Decimal cantidad { get; set; }

        [XmlAttribute(AttributeName = "fecha_ultima_factura")]
        public DateTime? fecha_ultima_factura { get; set; }

        [XmlAttribute(AttributeName = "fecha_salida")]
        public DateTime? fecha_salida { get; set; }

        [XmlAttribute(AttributeName = "fecha_factura")]
        public DateTime? fecha_factura { get; set; }

    }
}
