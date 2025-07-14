using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "factura")]
	public class Factura
	{

		[XmlElement(ElementName = "infoTributaria")]
		public InfoTributaria InfoTributaria { get; set; }

		[XmlElement(ElementName = "infoFactura")]
		public InfoFactura InfoFactura { get; set; }

		[XmlElement(ElementName = "detalles")]
		public Detalles Detalles { get; set; }

		[XmlElement(ElementName = "infoAdicional")]
		public InfoAdicional InfoAdicional { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
