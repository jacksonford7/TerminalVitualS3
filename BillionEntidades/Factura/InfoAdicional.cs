using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "infoAdicional")]
	public class InfoAdicional
	{

		[XmlElement(ElementName = "campoAdicional")]
		public List<CampoAdicional> CampoAdicional { get; set; }
	}
}
