using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{
	[XmlRoot(ElementName = "totalImpuesto")]
	public class TotalImpuesto
	{

		[XmlElement(ElementName = "codigo")]
		public string Codigo { get; set; }

		[XmlElement(ElementName = "codigoPorcentaje")]
		public string CodigoPorcentaje { get; set; }

		[XmlElement(ElementName = "baseImponible")]
		public string BaseImponible { get; set; }

		[XmlElement(ElementName = "tarifa")]
		public string Tarifa { get; set; }

		[XmlElement(ElementName = "valor")]
		public string Valor { get; set; }
	}
}
