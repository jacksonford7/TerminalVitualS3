using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "pagos")]
	public class Pagos
	{

		[XmlElement(ElementName = "pago")]
		public Pago Pago { get; set; }
	}
}
