using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "impuestos")]
	public class Impuestos
	{

		[XmlElement(ElementName = "impuesto")]
		public Impuesto Impuesto { get; set; }
	}
}
