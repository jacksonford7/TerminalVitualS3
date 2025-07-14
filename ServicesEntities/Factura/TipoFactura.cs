using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace ServicesEntities.Factura
{
    [Serializable]
    [XmlRoot(ElementName = "TipoFactura")]
     public class TipoFactura
    {

        [XmlAttribute(AttributeName = "id_tipo")]
        public string id_tipo { get; set; }

        [XmlAttribute(AttributeName = "nombre_tipo")]
        public string nombre_tipo { get; set; }

        [XmlAttribute(AttributeName = "descripcion_tipo")]
        public string descripcion_tipo { get; set; }

    }
}
