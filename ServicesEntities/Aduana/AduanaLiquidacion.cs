using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;



namespace Aduana
{
    [Serializable]
    [XmlRoot(ElementName = "AduanaLiquidacion")]
    public class AduanaLiquidacion
    {
        [XmlAttribute(AttributeName = "codigo_liquidacion")]
        public Int64 codigo_liquidacion { get; set; }

        [XmlAttribute(AttributeName = "numero_factura")]
        public string numero_factura { get; set; }

        [XmlAttribute(AttributeName = "monto")]
        public decimal monto { get; set; }

        [XmlAttribute(AttributeName = "fecha_liquidacion")]
        public DateTime? fecha_liquidacion { get; set; }

    }
}
