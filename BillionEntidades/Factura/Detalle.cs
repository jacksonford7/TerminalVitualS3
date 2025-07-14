using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{

	[XmlRoot(ElementName = "detalle")]
	public class Detalle
	{

		[XmlElement(ElementName = "codigoPrincipal")]
		public string CodigoPrincipal { get; set; }

		[XmlElement(ElementName = "descripcion")]
		public string Descripcion { get; set; }

		[XmlElement(ElementName = "cantidad")]
		public string Cantidad { get; set; }

		[XmlElement(ElementName = "precioUnitario")]
		public string PrecioUnitario { get; set; }

		[XmlElement(ElementName = "descuento")]
		public string Descuento { get; set; }

		[XmlElement(ElementName = "precioTotalSinImpuesto")]
		public string PrecioTotalSinImpuesto { get; set; }

		[XmlElement(ElementName = "impuestos")]
		public Impuestos Impuestos { get; set; }
	}
}
