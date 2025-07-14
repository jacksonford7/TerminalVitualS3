using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "detalles")]
	public class Detalles
	{

		[XmlElement(ElementName = "detalle")]
		public List<Detalle> Detalle { get; set; }
	}
}
