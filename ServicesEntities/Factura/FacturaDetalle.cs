using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace Factura
{
    [Serializable]
    [XmlRoot(ElementName = "FacturaDetalle")]
    public class FacturaDetalle
    {
        [XmlAttribute(AttributeName = "id_factura")]
        public Int64 id_factura { get; set; }

        [XmlAttribute(AttributeName = "secuencial")]
        public Int16 secuencial { get; set; }

        [XmlAttribute(AttributeName = "codigo_servicio")]
        public string codigo_servicio { get; set; }

        [XmlAttribute(AttributeName = "cantidad")]
        public decimal cantidad { get; set; }

        [XmlAttribute(AttributeName = "precio")]
        public decimal precio { get; set; }

        [XmlAttribute(AttributeName = "subtotal")]
        public decimal subtotal { get; set; }

        [XmlAttribute(AttributeName = "iva")]
        public decimal iva { get; set; }

        [XmlAttribute(AttributeName = "gkey_contenedor")]
        public Int64 gkey_contenedor { get; set; }


    }
}
