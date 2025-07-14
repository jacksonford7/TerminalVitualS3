using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;



namespace Aduanas.Entidades
{
    [Serializable]
    [XmlRoot(ElementName = "AduanaContenedor")]
    public class AduanaContenedor
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "contenedor")]
        public string contenedor { get; set; }

        [XmlAttribute(AttributeName = "gkey")]
        public Int64 gkey { get; set; }

    }
}
