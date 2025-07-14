using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "campoAdicional")]
	public class CampoAdicional
	{

		[XmlAttribute(AttributeName = "nombre")]
		public string Nombre { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
