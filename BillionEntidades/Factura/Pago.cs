using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "pago")]
	public class Pago
	{

		[XmlElement(ElementName = "formaPago")]
		public string FormaPago { get; set; }

		[XmlElement(ElementName = "total")]
		public string Total { get; set; }

		[XmlElement(ElementName = "plazo")]
		public string Plazo { get; set; }

		[XmlElement(ElementName = "unidadTiempo")]
		public string UnidadTiempo { get; set; }
	}
}
