using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace N4
{
    [Serializable]
    [XmlRoot(ElementName = "Contenedor")]
    public class Contenedor
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "tipo")]
        public string tipo { get; set; }//si es rf/rc

        [XmlAttribute(AttributeName = "referencia")]
        public string referencia { get; set; }

        [XmlAttribute(AttributeName = "contenedor")]
        public string contenedor { get; set; }

        [XmlAttribute(AttributeName = "gkey")]
        public Int64 gkey { get; set; }

        [XmlAttribute(AttributeName = "trafico")]
        public string trafico { get; set; }

        [XmlAttribute(AttributeName = "tamano")]
        public string tamano { get; set; }

        [XmlAttribute(AttributeName = "fecha_cas")]
        public DateTime? fecha_cas { get; set; }

        [XmlAttribute(AttributeName = "booking")]
        public string booking { get; set; }

        [XmlAttribute(AttributeName = "imdt")]
        public string imdt { get; set; }

        [XmlAttribute(AttributeName = "bloqueo")]
        public bool bloqueo { get; set; }

        [XmlAttribute(AttributeName = "fecha_ultima_factura")]
        public DateTime? fecha_ultima_factura { get; set; }

        [XmlAttribute(AttributeName = "documento")]
        public string documento { get; set; }

        [XmlAttribute(AttributeName = "in_out")]
        public string in_out { get; set; }


        [XmlAttribute(AttributeName = "aisv_codigo")]
        public string aisv_codigo { get; set; }

        [XmlAttribute(AttributeName = "cliente_facturar")]
        public string cliente_facturar { get; set; }

        [XmlAttribute(AttributeName = "dda")]
        public string dda { get; set; }

       
        [XmlAttribute(AttributeName = "pase_puerta")]
        public Int64 pase_puerta { get; set; }

       
    }
}
