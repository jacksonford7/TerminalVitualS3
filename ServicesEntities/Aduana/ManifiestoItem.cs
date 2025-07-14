using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Aduana
{
    [Serializable]
    [XmlRoot(ElementName = "ManifiestoItem")]
    public class ManifiestoItem
    {

        [XmlAttribute(AttributeName = "tipo_carga")]
        public string tipo_carga { get; set; }

        [XmlAttribute(AttributeName = "secuencia")]
        public int secuencia { get; set; }

        [XmlAttribute(AttributeName = "descripcion")]
        public string descripcion { get; set; }

        [XmlAttribute(AttributeName = "equipo_id")]
        public string equipo_id { get; set; }

        [XmlAttribute(AttributeName = "equipo_condicion")]
        public string equipo_condicion { get; set; } //FF,LF,FL,LL

       [XmlAttribute(AttributeName = "equipo_tipo")] //char code
       public string equipo_tipo { get; set; }


        [XmlAttribute(AttributeName = "peso_bruto")] //
        public Decimal peso_bruto { get; set; }

        [XmlAttribute(AttributeName = "cant_paq")] //
        public Decimal cant_paq { get; set; }

        [XmlAttribute(AttributeName = "bl_doc")]
        public string bl_doc { get; set; }

       

    }
}
