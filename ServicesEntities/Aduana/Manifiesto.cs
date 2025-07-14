using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Aduana
{
    [Serializable]
    [XmlRoot(ElementName = "Manifiesto")]
    public abstract class Manifiesto
    {
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "func_code")]
        public string func_code { get; set; }

        [XmlAttribute(AttributeName = "procesado")]
        public string procesado { get; set; }

        [XmlAttribute(AttributeName = "total_gross")]
        public Decimal total_gross { get; set; }

        [XmlAttribute(AttributeName = "cant_abordo")]
        public int cant_abordo { get; set; }

        [XmlAttribute(AttributeName = "equipos")]
        public int equipos { get; set; }

        [XmlAttribute(AttributeName = "bl_doc")]
        public string bl_doc { get; set; }

        [XmlAttribute(AttributeName = "importador")]
        public string importador { get; set; }

        [XmlAttribute(AttributeName = "importador_id")]
        public string importador_id { get; set; }

        [XmlAttribute(AttributeName = "fecha")]
        public DateTime fecha { get; set; }

        [XmlArray("Manifiesto_items")]
        [XmlArrayItem("manifiesto_item", typeof(ManifiestoItem))]
        public List<ManifiestoItem> manifiesto_item { get; set; }

        public Manifiesto()
        {
            this.manifiesto_item = new List<ManifiestoItem>();
        }
    }
}
