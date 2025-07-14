using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace Factura
{
    [Serializable]
    [XmlRoot(ElementName = "FacturaCabecera")]
    public class FacturaCabecera
    {
        [XmlAttribute(AttributeName = "id_factura")]
        public Int64 id_factura { get; set; }

        [XmlAttribute(AttributeName = "fecha_generacion")]
        public DateTime? fecha_generacion { get; set; }

        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "id_cliente")]
        public string id_cliente { get; set; }

        [XmlAttribute(AttributeName = "id_agente")]
        public string id_agente { get; set; }

        [XmlAttribute(AttributeName = "fecha_maxima")]
        public DateTime? fecha_maxima { get; set; }

        [XmlAttribute(AttributeName = "usuario")]
        public string usuario { get; set; }

        [XmlAttribute(AttributeName = "estado")]
        public string estado { get; set; }

        [XmlAttribute(AttributeName = "tipo_factura")]
        public string tipo_factura { get; set; }

        
        [XmlAttribute(AttributeName = "subtotal")]
        public decimal subtotal { get; set; }

        [XmlAttribute(AttributeName = "iva")]
        public decimal iva { get; set; }

        [XmlAttribute(AttributeName = "total")]
        public decimal total { get; set; }

        [XmlAttribute(AttributeName = "numero_factura")]
        public string numero_factura { get; set; }

        [XmlAttribute(AttributeName = "numero_draf")]
        public string numero_draf { get; set; }

        [XmlAttribute(AttributeName = "booking")]
        public string booking { get; set; }

        [XmlArray("Factura_items")]
        [XmlArrayItem("Factura_item", typeof(FacturaDetalle))]
        public List<FacturaDetalle> Factura_item { get; set; }

        public FacturaCabecera()
        {
            this.Factura_item = new List<FacturaDetalle>();
        }
    }
}
